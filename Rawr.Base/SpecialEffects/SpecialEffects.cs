using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Rawr {
    public static class SpecialEffects {
        public static void ProcessMetaGem(string line, Stats stats, bool bisArmory)
        {
            Match match;
            List<string> gemBonuses = new List<string>();
            string[] gemBonusStrings = line.Split(new string[] { " and ", " & ", ", " }, StringSplitOptions.None);
            foreach (string gemBonusString in gemBonusStrings) {
                if (gemBonusString.IndexOf('+') != gemBonusString.LastIndexOf('+')) {
                    gemBonuses.Add(gemBonusString.Substring(0, gemBonusString.IndexOf(" +")));
                    gemBonuses.Add(gemBonusString.Substring(gemBonusString.IndexOf(" +") + 1));
                } else{ gemBonuses.Add(gemBonusString); }
            }
            foreach (string gemBonus in gemBonuses) {
                if (gemBonus == "Spell Damage +6")
                {
                    stats.SpellPower = 6.0f;
                }
                else if (gemBonus == "Stamina +6")
                {
                    stats.Stamina = 6.0f;
                }
                else if (gemBonus == "Chance to restore mana on spellcast")
                {
                    stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { ManaRestore = 600 }, 0f, 15f, .05f));
                }
                else if (gemBonus == "2% Increased Armor Value from Items")
                {
                    stats.BaseArmorMultiplier = 0.02f; // IED
                }
                else if (gemBonus == "Chance on spellcast - next spell cast in half time" || gemBonus == "Chance to Increase Spell Cast Speed")
                {
                    stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { HasteRating = 320 }, 6, 45, 0.15f));
                }
                else if (gemBonus == "+5% Shield Block Value")
                {
                    stats.BonusBlockValueMultiplier = 0.01f; // Even though this says 5%, is it only worth 1% in-game in reality!
                }
                else if (gemBonus == "+2% Intellect")
                {
                    stats.BonusIntellectMultiplier = 0.02f;
                }
                else if (gemBonus == "+2% Mana")
                {
                    stats.BonusManaMultiplier = 0.02f;
                }
                else if (gemBonus == "+2% Maximum Mana")
                {
                    stats.BonusManaMultiplier = 0.02f;
                }
                else if ((match = new Regex(@"Reduce Spell Damage Taken by\s+(?<amount>\d+).*").Match(gemBonus)).Success)
                {
                    
                    //int bonus = int.Parse(gemBonus.Substring(gemBonus.Length - 3, 2));
                    stats.SpellDamageTakenReductionMultiplier = (float)int.Parse(match.Groups["amount"].Value) / 100f;
                }
                else if (gemBonus == "3% Increased Critical Damage")
                {
                    stats.BonusCritDamageMultiplier = 0.03f;
                    stats.BonusSpellCritDamageMultiplier = 0.03f;
                }
                else if (gemBonus == "3% Increased Critical Healing Effect")
                {
                    stats.BonusCritHealMultiplier = 0.03f;
                }
                else if (gemBonus == "Minor Run Speed Increase")
                {
                    stats.MovementSpeed = 0.08f;
                }
                else if (gemBonus.Contains("Silence Duration Reduced by "))
                {
                    int bonus = int.Parse(gemBonus.Substring(gemBonus.Length - 3, 2));
                    stats.SilenceDurReduc = (float)bonus / 100f;
                }
                else if (gemBonus.Contains("Stun Duration Reduced by "))
                {
                    int bonus = int.Parse(gemBonus.Substring(gemBonus.Length - 3, 2));
                    stats.StunDurReduc = (float)bonus / 100f;
                }
                else if (gemBonus.Contains("Reduces Snare/Root Duration by "))
                {
                    int bonus = int.Parse(gemBonus.Substring(gemBonus.Length - 3, 2));
                    stats.SnareRootDurReduc = (float)bonus / 100f;
                }
                else if (gemBonus.Contains("Fear Duration Reduced by "))
                {
                    int bonus = int.Parse(gemBonus.Substring(gemBonus.Length - 3, 2));
                    stats.FearDurReduc = (float)bonus / 100f;
                }
                else if (gemBonus.Contains("Chance to Increase Melee/Ranged Attack Speed"))
                {
                    // 480 Haste Rating on 100% Chance to proc every 60 seconds for 6 seconds
                    stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageDone, new Stats() { HasteRating = 480f, }, 6f, 60f));
                }
                else if (gemBonus.Contains("% Spell Reflect"))
                {
                    int bonus = int.Parse(gemBonus.Substring(0, 2).Trim('%'));
                    stats.SpellReflectChance = (float)bonus / 100f;
                }
                else if (gemBonus == "Sometimes Heal on Your Crits")
                {
                    // this is supposed to be 2% of your total health healed when it procs, 50% chance to proc on crit
                    stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalCrit, new Stats() { HealthRestoreFromMaxHealth = 0.02f }, 0f, 0f, 0.50f));
                    stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCrit   , new Stats() { HealthRestoreFromMaxHealth = 0.02f }, 0f, 0f, 0.50f));
                }
                else if (gemBonus == "2% Reduced Threat")
                {
                    stats.ThreatReductionMultiplier = 0.02f;
                }
                else
                {
                    try
                    {
                        int gemBonusValue = int.Parse(gemBonus.Substring(0, gemBonus.IndexOf(' ')).Trim('+').Trim('%'));
                        switch (gemBonus.Substring(gemBonus.IndexOf(' ') + 1).Trim())
                        {
                            case "Resist All":
                                stats.ArcaneResistance = gemBonusValue;
                                stats.FireResistance = gemBonusValue;
                                stats.FrostResistance = gemBonusValue;
                                stats.NatureResistance = gemBonusValue;
                                stats.ShadowResistance = gemBonusValue;
                                break;
                            case "Increased Critical Damage":
                                stats.BonusCritDamageMultiplier = (float)gemBonusValue / 100f;
                                stats.BonusSpellCritDamageMultiplier = (float)gemBonusValue / 100f; // both melee and spell crit use the same text, would have to disambiguate based on id
                                break;
                            case "Agility":
                                stats.Agility = gemBonusValue;
                                break;
                            case "Stamina":
                                stats.Stamina = gemBonusValue;
                                break;
                            case "Dodge Rating":
                                stats.DodgeRating = gemBonusValue;
                                break;
                            case "Parry Rating":
                                stats.ParryRating = gemBonusValue;
                                break;
                            case "Block Rating":
                                stats.BlockRating = gemBonusValue;
                                break;
                            case "Hit Rating":
                                stats.HitRating = gemBonusValue;
                                break;
                            case "Haste Rating":
                                stats.HasteRating = gemBonusValue;
                                break;
                            case "Expertise Rating":
                                stats.ExpertiseRating = gemBonusValue;
                                break;
                            case "Strength":
                                stats.Strength = gemBonusValue;
                                break;
                            case "Crit Rating":
                            case "Crit Strike Rating":
                            case "Critical Rating":
                            case "Critical Strike Rating":
                                stats.CritRating = gemBonusValue;
                                break;
                            case "Attack Power":
                                stats.AttackPower = gemBonusValue;
                                break;
                            case "Weapon Damage":
                                stats.WeaponDamage = gemBonusValue;
                                break;
                            case "Resilience":
                            case "Resilience Rating":
                                stats.Resilience = gemBonusValue;
                                break;
                            case "Spell Hit Rating":
                                stats.HitRating = gemBonusValue;
                                break;
                            case "Spell Haste Rating":
                                stats.HasteRating = gemBonusValue;
                                break;
                            case "Spell Damage":
                                // Ignore spell damage from gem if Healing has already been applied, as it might be a "9 Healing 3 Spell" gem. 
                                if (stats.SpellPower == 0)
                                    stats.SpellPower = gemBonusValue;
                                break;
                            case "Spell Damage and Healing":
                                stats.SpellPower = gemBonusValue;
                                break;
                            case "Healing":
                                stats.SpellPower = (float)Math.Round(gemBonusValue / 1.88f);
                                break;
                            case "Spell Power":
                                stats.SpellPower = gemBonusValue;
                                break;
                            case "Spell Crit":
                            case "Spell Crit Rating":
                            case "Spell Critical":
                            case "Spell Critical Rating":
                                stats.CritRating = gemBonusValue;
                                break;
                            case "Mana every 5 seconds":
                            case "Mana ever 5 Sec":
                            case "mana per 5 sec":
                            case "mana per 5 sec.":
                            case "Mana per 5 Seconds":
                                stats.Mp5 = gemBonusValue;
                                break;
                            case "Intellect":
                                stats.Intellect = gemBonusValue;
                                break;
                            case "Spirit":
                                stats.Spirit = gemBonusValue;
                                break;
                        }
                    }
                    catch { }
                }
            }
        }

        public static void ProcessEquipLine(string line, Stats stats, bool isArmory, int ilvl, int id) {
            Match match;
            #region Prep the line, if it needs it
            while (line.Contains("secs")) { line = line.Replace("secs", "sec"); }
            while (line.Contains("sec.")) { line = line.Replace("sec.", "sec"); }
            while (line.Contains("  ")) { line = line.Replace("  ", " "); }
            while (line.EndsWith(".")) { line = line.Substring(0, line.Length-1); }
            #endregion
            if (false) { /*Never run, this is just to make all the stuff below uniform with an 'else if' line start*/ }
            #region Class Specific
            #region Added by Druid: Moonkin
            else if ((match = new Regex(@"Your Moonfire spell grants (?<amount>\d+) spell power for 10 sec").Match(line)).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.MoonfireCast,
                    new Stats() { SpellPower = (float)int.Parse(match.Groups["amount"].Value), }, 10f, 0f));
            }
            else if (line.StartsWith("Your Moonfire ability has a chance to grant "))
            {
                line = line.Substring("Your Moonfire ability has a chance to grant ".Length);
                string spellPowerLine = line.Substring(0, line.IndexOf(" spell power for"));
                string durationLine = line.Substring(line.IndexOf("for") + 3, line.IndexOf(" sec") - line.IndexOf("for") - 3);
                float spellPower = float.Parse(spellPowerLine, System.Globalization.CultureInfo.InvariantCulture);
                float duration = float.Parse(durationLine, System.Globalization.CultureInfo.InvariantCulture);
                stats.AddSpecialEffect(new SpecialEffect(Trigger.MoonfireCast, new Stats() { SpellPower = spellPower, }, duration, 0f, 0.5f));
            }
            #endregion
            #region Added by Shaman: Enhance/Elemental/Resto
            else if ((match = new Regex(@"Your Lava Lash ability also grants you (?<amount>\d+) attack power for (?<dur>\d+) sec").Match(line)).Success)
            {   // Gladiator's Totem of Indomitability
                stats.AddSpecialEffect(new SpecialEffect(Trigger.ShamanLavaLash,
                    new Stats() { AttackPower = (float)int.Parse(match.Groups["amount"].Value) },
                    (float)int.Parse(match.Groups["dur"].Value), 0f));
            }
            else if ((match = new Regex(@"Your Shock spells grant (?<amount>\d+) spellpower for (?<dur>\d+) sec").Match(line)).Success)
            {   // Gladiator's Totem of Survival
                stats.AddSpecialEffect(new SpecialEffect(Trigger.ShamanShock,
                    new Stats() { SpellPower = (float)int.Parse(match.Groups["amount"].Value) },
                    (float)int.Parse(match.Groups["dur"].Value), 0f));
            }
            else if ((match = new Regex(@"The periodic damage from your Flame Shock spell grants (?<amount>\d+) haste rating for (?<dur>\d+) sec Stacks up to (?<stacks>\d+) times").Match(line)).Success)
            {   // Bizuri's Totem of Shattered Ice
                stats.AddSpecialEffect(new SpecialEffect(Trigger.ShamanFlameShockDoTTick,
                    new Stats() { HasteRating = (float)int.Parse(match.Groups["amount"].Value) },
                    (float)int.Parse(match.Groups["dur"].Value), 0f, 1f, int.Parse(match.Groups["stacks"].Value)));
            }
            else if ((match = new Regex(@"Your Storm Strike ability also grants you (?<amount>\d+) haste rating for (?<dur>\d+) sec").Match(line)).Success)
            {   // Totem of Dueling
                stats.AddSpecialEffect(new SpecialEffect(Trigger.ShamanStormStrike,
                    new Stats() { HasteRating = (float)int.Parse(match.Groups["amount"].Value) },
                    (float)int.Parse(match.Groups["dur"].Value), 0f));
            }
            else if ((match = new Regex(@"Each time you cast Lightning Bolt, you have a chance to gain (?<amount>\d+) haste rating for (?<dur>\d+) sec").Match(line)).Success)
            {   // Totem of Electrifying Wind
                stats.AddSpecialEffect(new SpecialEffect(Trigger.ShamanLightningBolt,
                    new Stats() { HasteRating = (float)int.Parse(match.Groups["amount"].Value) },
                    (float)int.Parse(match.Groups["dur"].Value), 0f, 0.70f));
            }
            else if ((match = new Regex(@"Each time you use your Lava Lash ability, you have a chance to gain (?<amount>\d+) attack power for (?<dur>\d+) sec").Match(line)).Success)
            {   // Totem of Quaking Earth
                stats.AddSpecialEffect(new SpecialEffect(Trigger.ShamanLavaLash,
                    new Stats() { AttackPower = (float)int.Parse(match.Groups["amount"].Value) },
                    (float)int.Parse(match.Groups["dur"].Value), 9f, 0.80f));
            }
            else if ((match = new Regex(@"Your Stormstrike ability grants (?<amount>\d+) attack power for (?<dur>\d+) sec Stacks up to (?<stacks>\d+) times").Match(line)).Success)
            {   // Totem of the Avalanche
                stats.AddSpecialEffect(new SpecialEffect(Trigger.ShamanStormStrike,
                    new Stats() { AttackPower = (float)int.Parse(match.Groups["amount"].Value) },
                    (float)int.Parse(match.Groups["dur"].Value), 0f, 1f, int.Parse(match.Groups["stacks"].Value)));
            }
            else if ((match = new Regex(@"Your Lightning Bolt spell has a chance to grant (?<amount>\d+) haste rating for (?<dur>\d+) sec").Match(line)).Success)
            {   // Totem of the Elemental Plane
                //stats.LightningBoltHasteProc_15_45 += (float)int.Parse(match.Groups["amount"].Value);
                stats.AddSpecialEffect(new SpecialEffect(Trigger.ShamanLightningBolt,
                    new Stats() { HasteRating = int.Parse(match.Groups["amount"].Value) },
                    (float)int.Parse(match.Groups["dur"].Value), 45f, 0.15f));
            }
            /*else if ((match = new Regex(@"Your Riptide spell grants (?<amount>\d+) spell power for (?<dur>\d+) sec Stacks up to (?<stacks>\d+) times").Match(line)).Success)
            {   // Totem of the Surging Sea
                // This needs to be remodeled as a SpecialEffect
                stats.RestoShamRelicT10 = int.Parse(match.Groups["amount"].Value) * int.Parse(match.Groups["stacks"].Value);
            }*/
            // Other
            else if (line == "Your Shock spells have a chance to grant 110 attack power for 10 sec")
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.ShamanShock, new Stats() { AttackPower = 110 }, 10f, 45f));
            }
            #endregion
            #region Added by Priest: HealPriest
            #endregion
            #region Added by Priest: Shadow
            #endregion
            #region Added by Warrior: ProtWarr
            #endregion
            #region Added by Warrior: DPSWarr
            #endregion
            #region Added by Paladin: Retadin
            else if ((match = new Regex(@"Your Crusader Strike ability also grants you (?<amount>\d+) attack power for (?<dur>\d+) sec").Match(line)).Success)
            {   // Gladiator's Libram of Fortitude
                stats.AddSpecialEffect(new SpecialEffect(Trigger.CrusaderStrikeHit,
                    new Stats() { AttackPower = int.Parse(match.Groups["amount"].Value) },
                    int.Parse(match.Groups["dur"].Value), 0f));
            }
            else if ((match = new Regex(@"Your Crusader Strike ability grants (?<amount>\d+) Strength for (?<dur>\d+) sec Stacks up to (?<stacks>\d+) times").Match(line)).Success)
            {   // Libram of Three Truths
                stats.AddSpecialEffect(new SpecialEffect(Trigger.CrusaderStrikeHit,
                    new Stats() { Strength = (float)int.Parse(match.Groups["amount"].Value) },
                    (float)int.Parse(match.Groups["dur"].Value), 0f, 1f, int.Parse(match.Groups["stacks"].Value)));
            }
            #endregion
            #region Added by Hunter
            #endregion
            #region Added by Warlock
            #endregion
            #region Added by Mage
            #endregion
            #region Added by Rogue
            #endregion
            #endregion
            #region Professions
            else if (line.StartsWith("Increases the effect that healing and mana potions have on the wearer by "))
            {
                // Alchemist's Stone
                line = line.Substring("Increases the effect that healing and mana potions have on the wearer by ".Length);
                line = line.Substring(0, line.IndexOf('%'));
                stats.BonusManaPotionEffectMultiplier += int.Parse(line) / 100f;
                stats.HealthRestore += int.Parse(line) / 100f;
            }
            #endregion
            #region General
            #region Agility
            else if ((match = new Regex(@"When you deal damage you have a chance to gain (?<amount>\d+) Agility for (?<dur>\d+) sec").Match(line)).Success)
            { // Gladiator's Insignia of Conquest
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageDone,
                    new Stats() { Agility = (float)int.Parse(match.Groups["amount"].Value) },
                    int.Parse(match.Groups["dur"].Value), 100f, 0.15f));
            }
            else if ((match = new Regex(@"Your melee and ranged attacks grant (?<amount>\d+) Agility.*\ Lasts (?<dur>\d+) sec, stacking up to (?<stacks>\d+) times").Match(line)).Success)
            {   // Fluid Death, Tia's Grace
                stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalAttack,
                    new Stats() { Agility = (float)int.Parse(match.Groups["amount"].Value) },
                    (float)int.Parse(match.Groups["dur"].Value), 0f, 1f, int.Parse(match.Groups["stacks"].Value)));
            }
            else if ((match = new Regex(@"Your melee and ranged attacks have a chance to grant (?<amount>\d+) Agility for (?<dur>\d+) sec").Match(line)).Success)
            {   // Key to the Endless Chamber
                stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalAttack,
                    new Stats() { Agility = (float)int.Parse(match.Groups["amount"].Value) },
                    (float)int.Parse(match.Groups["dur"].Value), 75f, 0.1f));
            }
            else if ((match = new Regex(@"Your melee and ranged critical strikes have a chance to grant (?<amount>\d+) Agility for (?<dur>\d+) sec").Match(line)).Success)
            {
                // Patch 4.0.6+ Chance to proc went from 30% to 50% on Physical Crit.
                // Left Eye of Rajh
                stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalCrit,
                    new Stats() { Agility = (float)int.Parse(match.Groups["amount"].Value) },
                    (float)int.Parse(match.Groups["dur"].Value), 50f, 0.5f));
            }
            #endregion
            #region Armor
            else if ((match = new Regex(@"Each time a melee attack strikes you, you have a chance to gain (?<amount>\d+) armor for (?<dur>\d+) sec").Match(line)).Success)
            {   // The Black Heart
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageTakenPhysical,
                    new Stats() { BonusArmor = int.Parse(match.Groups["amount"].Value) },
                    int.Parse(match.Groups["dur"].Value), 45f, 0.25f));
            }
            #endregion
            #region Armor Penetration Rating // Armor Penetration is removed as a stat from WoW
            /*            else if ((match = new Regex(@"Chance on melee (and|or) ranged critical strike to increase your armor penetration rating by (?<amount>\d+) for (?<dur>\d+) sec").Match(line)).Success)
            {   // Needle-Encrusted Scorpion
                stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalCrit,
                    new Stats() { ArmorPenetrationRating = (float)int.Parse(match.Groups["amount"].Value) },
                    (float)int.Parse(match.Groups["dur"].Value), 45,  0.10f));
            }
            else if ((match = new Regex(@"Your melee (and|or) ranged attacks have a chance to increase your armor penetration rating by (?<amount>\d+) for (?<dur>\d+) sec").Match(line)).Success)
            {   // Mjolnir Runestone | Grim Toll
                stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalHit,
                    new Stats() { ArmorPenetrationRating = (float)int.Parse(match.Groups["amount"].Value) },
                    (float)int.Parse(match.Groups["dur"].Value), 45, 0.15f));
            }
 */
            #endregion
            #region Attack Power
            else if ((match = new Regex(@"Each time you deal melee or ranged damage to an opponent, you gain (?<amount>\d+) attack power for the next (?<dur>\d+) sec, stacking up to (?<stacks>\d+) times").Match(line)).Success)
            {   // Fury of the Five Flights
                stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalHit,
                    new Stats() { AttackPower = (float)int.Parse(match.Groups["amount"].Value) },
                    (float)int.Parse(match.Groups["dur"].Value), 0f, 1f, int.Parse(match.Groups["stacks"].Value)));
            }
            else if ((match = new Regex(@"Chance on critical hit to increase your attack power by (?<amount>\d+) for (?<dur>\d+) sec").Match(line)).Success)
            {
                int ap = int.Parse(match.Groups["amount"].Value);
                int duration = int.Parse(match.Groups["dur"].Value);
                stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalCrit, new Stats() { AttackPower = ap }, duration, 45f, 0.10f));
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCrit, new Stats() { AttackPower = ap }, duration, 45f, 0.10f));
            }
            else if ((match = new Regex(@"Chance on melee and ranged critical strike to increase your attack power by (?<amount>\d+) for (?<dur>\d+) sec").Match(line)).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalCrit,
                    new Stats() { AttackPower = (float)int.Parse(match.Groups["amount"].Value) },
                    int.Parse(match.Groups["dur"].Value), 50f, 0.10f));
            }
            else if ((match = new Regex(@"Chance on melee or ranged hit to increase your attack power by (?<amount>\d+) for (?<dur>\d+) sec").Match(line)).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalHit,
                    new Stats() { AttackPower = (float)int.Parse(match.Groups["amount"].Value) },
                    (float)int.Parse(match.Groups["dur"].Value), 45f, 0.10f));
            }
            else if ((match = new Regex(@"Each time you hit with a melee or ranged attack, you have a chance to gain (?<amount>\d+) attack power for (?<dur>\d+) sec").Match(line)).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalHit,
                    new Stats() { AttackPower = int.Parse(match.Groups["amount"].Value) },
                    (float)int.Parse(match.Groups["dur"].Value), 45f, 0.20f));
            }
            else if ((match = new Regex(@"Each time you deal melee or ranged damage to an opponent, you gain (?<amount>\d+) attack power for the next (?<dur>\d+) sec(|,) stacking up to (?<stacks>\d+) times").Match(line)).Success)
            {   // Herkuml War Token
                stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalHit,
                    new Stats() { AttackPower = (float)int.Parse(match.Groups["amount"].Value) },
                    (float)int.Parse(match.Groups["dur"].Value), 0f, 1f, int.Parse(match.Groups["stacks"].Value)));
            }
            else if ((match = new Regex(@"When you deal damage you have a chance to gain (?<amount>\d+) attack power for (?<dur>\d+) sec").Match(line)).Success)
            {   // Whispering Fanged Skull & Sharpened Twilight Scale
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageDone,
                    new Stats() { AttackPower = int.Parse(match.Groups["amount"].Value) },
                    int.Parse(match.Groups["dur"].Value), 45f, 0.35f));
            }
            else if ((match = new Regex(@"Chance on hit to increase your attack power by (?<amount>\d+) for (?<dur>\d+) sec").Match(line)).Success)
            {   // Ashen Band of Vengeance - seems to be 60 sec iCD (wowhead)
                stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalHit,
                    new Stats() { AttackPower = (float)int.Parse(match.Groups["amount"].Value) },
                    (float)int.Parse(match.Groups["dur"].Value), 60f, 0.10f));
            }
            #endregion
            #region Crit Rating
            else if ((match = new Regex(@"Grants (?<amount>\d+) critical strike rating for (?<dur>\d+) sec each time you deal a melee critical strike, stacking up to (?<stack>\d+) times").Match(line)).Success)
            {   // Vessel of Acceleration
                stats.AddSpecialEffect(new SpecialEffect(Trigger.MeleeCrit,
                    new Stats() { CritRating = (float)int.Parse(match.Groups["amount"].Value) },
                    (float)int.Parse(match.Groups["dur"].Value), 0, 1f, 5));
            }
            else if ((match = new Regex(@"Chance on hit to increase your critical strike rating by (?<amount>\d+) for (?<dur>\d+) sec").Match(line)).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalHit,
                    new Stats() { CritRating = (float)int.Parse(match.Groups["amount"].Value) },
                    (float)int.Parse(match.Groups["dur"].Value), 45, 0.10f));
            }
            else if ((match = new Regex(@"Your melee and ranged attacks have a chance to increase your critical strike rating by (?<amount>\d+) for (?<dur>\d+) sec").Match(line)).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalAttack,
                    new Stats() { CritRating = (float)int.Parse(match.Groups["amount"].Value) },
                    (float)int.Parse(match.Groups["dur"].Value), 45f, 0.15f));
            }
            else if ((match = new Regex(@"Your melee and ranged attacks have a chance to grant (?<amount>\d+) critical strike rating for (?<dur>\d+) sec").Match(line)).Success)
            {   // Essence of the Cyclone, Grace of the Herald
                // Grace has a 100 sec ICD, while Essence has a 50 sec ICD
                float internalCooldown = 50f;
                if (id == 56295 || id == 55266) { internalCooldown = 100f; }
                stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalAttack,
                    new Stats() { CritRating = (float)int.Parse(match.Groups["amount"].Value) },
                    (float)int.Parse(match.Groups["dur"].Value), internalCooldown, 0.10f));
            }
            #endregion
            #region Damaging Absorbs
            #endregion
            #region Damaging Procs
            else if ((match = new Regex(@"Sends a shadowy bolt at the enemy causing (?<min>\d+) to (?<max>\d+) Shadow damage.*").Match(line)).Success)
            {   // Raging Deathbringer / Empowered Deathbringer
                int min = int.Parse(match.Groups["min"].Value);
                int max = int.Parse(match.Groups["max"].Value);
                stats.AddSpecialEffect(new SpecialEffect(Trigger.CurrentHandHit,
                    new Stats() { ShadowDamage = (float)(min + max) / 2f, },
                    0f, 0f, 0.05f));  // proc chance is ~5% according to wowhead.  
            }

            else if ((match = new Regex(@"Your harmful spells have a chance to strike your enemy, dealing (?<min>\d+) to (?<max>\d+) shadow damage.*").Match(line)).Success)
            {   // Pendulum of Telluric Currents
                int min = int.Parse(match.Groups["min"].Value);
                int max = int.Parse(match.Groups["max"].Value);
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageSpellHit,
                    new Stats() { ShadowDamage = (float)(min + max) / 2f, },
                    0f, 45f, 0.10f));
            }
            else if ((match = new Regex(@"Each time one of your spells deals periodic damage, there is a chance (?<min>\d+) to (?<max>\d+) additional damage will be dealt.*").Match(line)).Success)
            {   // Extract of Necromantic Power
                stats.ExtractOfNecromanticPowerProc += 1;
                int min = int.Parse(match.Groups["min"].Value);
                int max = int.Parse(match.Groups["max"].Value);
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DoTTick,
                    new Stats() { ShadowDamage = (float)(min + max) / 2f, },
                    0f, 45f, 0.10f));
            }
            else if ((match = new Regex(@"Each time you deal damage, you have a chance to do an additional (?<min>\d+) to (?<max>\d+) Shadow damage.*").Match(line)).Success)
            {   // Darkmoon Card: Death
                stats.DarkmoonCardDeathProc += 1;
                int min = int.Parse(match.Groups["min"].Value);
                int max = int.Parse(match.Groups["max"].Value);
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageDone,
                    new Stats() { ShadowDamage = (float)(min + max) / 2f, },
                    0f, 45f, 0.35f));
            }

            else if ((match = new Regex(@"Your melee and ranged attacks have a chance to strike your enemy, dealing (?<min>\d+) to (?<max>\d+) arcane damage.*").Match(line)).Success)
            {   // Bandit's Insignia
                int min = int.Parse(match.Groups["min"].Value);
                int max = int.Parse(match.Groups["max"].Value);
                stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalAttack,
                    new Stats() { ArcaneDamage = (float)(min + max) / 2f, },
                    0f, 45f, 0.15f));
            }
            else if ((match = new Regex(@"Delivers a fatal wound for (?<min>\d+) damage").Match(line)).Success)
            {   // Tempered Vis'kag the bloodletter
                int min = int.Parse(match.Groups["min"].Value);
                stats.AddSpecialEffect(new SpecialEffect(Trigger.CurrentHandHit,
                    new Stats() { PhysicalDamage = (float)min, },
                    0f, 0f, 0.05f));
            }
            else if ((match = new Regex(@"Your harmful spells have a chance to afflict your victim with Vengeful Wisp, which deals (?<amount>\d+) damage every (?<dur>\d+) sec").Match(line)).Success)
            {   // Harmlight Token
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageSpellHit,
                    new Stats() { NatureDamage = int.Parse(match.Groups["amount"].Value) / (float)int.Parse(match.Groups["dur"].Value) },
                    (float)int.Parse(match.Groups["dur"].Value) * 5f, 75f, 0.10f));
            }
            else if ((match = new Regex(@"Chance to strike your enemies for (?<min>\d+) to (?<max>\d+) Nature damage when dealing damage with melee or ranged attacks").Match(line)).Success)
            {   // Darkmoon Card: Hurricane
                // Need to finalize the percent or see if it is a Proc-Per-Minute
                stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalHit,
                     new Stats() { NatureDamage = (int.Parse(match.Groups["min"].Value) + int.Parse(match.Groups["max"].Value)) / 2f },
                     0f, 0f, 0.05f));
            }
            else if ((match = new Regex(@"When dealing damage with spells, you have a chance to deal (?<min>\d+) to (?<max>\d+) additional Fire damage to the target and gain (?<amount>\d+) Intellect for (?<dur>\d+) sec").Match(line)).Success)
            {   // Darkmoon Card: Volcano
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageDone,
                     new Stats() { FireDamage = (int.Parse(match.Groups["min"].Value) + int.Parse(match.Groups["max"].Value)) / 2f },
                     0f, 45f, .30f));
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageDone,
                     new Stats() { Intellect = int.Parse(match.Groups["amount"].Value) },
                     int.Parse(match.Groups["dur"].Value), 45f, .30f));
            }
            #endregion
            #region Dodge Rating
            else if ((match = new Regex(@"When you parry an attack, you gain (?<amount>\d+) dodge rating for (?<dur>\d+) sec.*\ Cannot occur more often than once every (?<cd1>\d+) sec").Match(line)).Success)
            {   // Throngus's Finger
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageParried,
                    new Stats() { DodgeRating = (float)int.Parse(match.Groups["amount"].Value) },
                    (float)int.Parse(match.Groups["dur"].Value), (float)int.Parse(match.Groups["cd1"].Value), 1f));
            }
            #endregion
            #region Haste Rating
            else if ((match = new Regex(@"Your harmful spells have a chance to increase your haste rating by (?<amount>\d+) for (?<dur>\d+) sec").Match(line)).Success)
            {   // Elemental Focus Stone
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageSpellCast,
                    new Stats() { HasteRating = (float)int.Parse(match.Groups["amount"].Value) },
                    (float)int.Parse(match.Groups["dur"].Value), 45f, 0.10f));
            }
            else if ((match = new Regex(@"Your melee and ranged attacks have a chance to increase your haste rating by (?<amount>\d+) for (?<dur>\d+) sec").Match(line)).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalAttack,
                    new Stats() { HasteRating = (float)int.Parse(match.Groups["amount"].Value) },
                    (float)int.Parse(match.Groups["dur"].Value), 45, 0.15f));
            }
            else if ((match = new Regex(@"Chance on melee and ranged critical strike to increase your haste rating by (?<amount>\d+) for (?<dur>\d+) sec").Match(line)).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalCrit,
                    new Stats() { HasteRating = (float)int.Parse(match.Groups["amount"].Value) },
                    (float)int.Parse(match.Groups["dur"].Value), 45f, 0.10f));
            }
            else if ((match = new Regex(@"Your spell critical strikes have a 50% chance to grant you (?<amount>\d+) spell haste rating for (?<dur>\d+) sec").Match(line)).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCrit,
                    new Stats() { HasteRating = (float)int.Parse(match.Groups["amount"].Value) },
                    (float)int.Parse(match.Groups["dur"].Value), 0f, 0.50f));
            }
            else if ((match = new Regex(@"Your spells have a chance to increase your haste rating by (?<amount>\d+) for (?<dur>\d+) sec").Match(line)).Success)
            {   // Embrace of the Spider
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast,
                    new Stats() { HasteRating = (float)int.Parse(match.Groups["amount"].Value) },
                    (float)int.Parse(match.Groups["dur"].Value), 45f, 0.10f));
            }
            else if ((match = new Regex(@"Your direct healing and heal over time spells have a chance to increase your haste rating by (?<amount>\d+) for (?<dur>\d+) sec").Match(line)).Success)
            {   // The Egg of Mortal Essence
                stats.AddSpecialEffect(new SpecialEffect(Trigger.HealingSpellCast,
                    new Stats() { HasteRating = (float)int.Parse(match.Groups["amount"].Value) },
                    (float)int.Parse(match.Groups["dur"].Value), 45f, 0.10f));
            }
            else if ((match = new Regex(@"Your melee attacks have a chance to grant (?<amount>\d+) haste rating for (?<dur>\d+) sec").Match(line)).Success)
            {   // Crushing Weight
                stats.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit,
                    new Stats() { HasteRating = (float)int.Parse(match.Groups["amount"].Value) },
                    (float)int.Parse(match.Groups["dur"].Value), (float)int.Parse(match.Groups["dur"].Value) * 5f, 0.10f));
            }
            else if ((match = new Regex(@"Your melee and ranged attacks have a chance to grant (?<amount>\d+) haste rating for (?<dur>\d+) sec").Match(line)).Success)
            {   // Prestor's Talisman of Machination
                stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalAttack,
                    new Stats() { HasteRating = (float)int.Parse(match.Groups["amount"].Value) },
                    (float)int.Parse(match.Groups["dur"].Value), (float)int.Parse(match.Groups["dur"].Value) * 5f, 0.10f));
            }
            else if ((match = new Regex(@"Your healing spells have a chance to grant (?<amount>\d+) haste rating for (?<dur>\d+) sec").Match(line)).Success)
            {   // Rainsong
                stats.AddSpecialEffect(new SpecialEffect(Trigger.HealingSpellCast,
                    new Stats() { HasteRating = (float)int.Parse(match.Groups["amount"].Value) },
                    (float)int.Parse(match.Groups["dur"].Value), (float)int.Parse(match.Groups["dur"].Value) * 5f, 0.10f));
            }
            else if ((match = new Regex(@"Your spells have a chance to grant (?<amount>\d+) haste rating for (?<dur>\d+) sec").Match(line)).Success)
            {   // Witching Hourglass
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellHit,
                    new Stats() { HasteRating = (float)int.Parse(match.Groups["amount"].Value) },
                    (float)int.Parse(match.Groups["dur"].Value), (float)int.Parse(match.Groups["dur"].Value) * 5f, 0.10f));
            }
            else if ((match = new Regex(@"Your melee critical strikes have a chance to grant (?<amount>\d+) haste rating for (?<dur>\d+) sec").Match(line)).Success)
            {   // Shrine-Cleansing Purifier, Tank-Commander Insignia
                stats.AddSpecialEffect(new SpecialEffect(Trigger.MeleeCrit,
                    new Stats() { HasteRating = (float)int.Parse(match.Groups["amount"].Value) },
                    (float)int.Parse(match.Groups["dur"].Value), (float)int.Parse(match.Groups["dur"].Value) * 5f, 0.30f));
            }
            else if ((match = new Regex(@"Your melee and ranged critical strikes have a chance to grant (?<amount>\d+) haste rating for (?<dur>\d+) sec").Match(line)).Success)
            {   // Gear Detector
                stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalCrit,
                    new Stats() { HasteRating = (float)int.Parse(match.Groups["amount"].Value) },
                    (float)int.Parse(match.Groups["dur"].Value), (float)int.Parse(match.Groups["dur"].Value) * 5f, 0.30f));
            }
            #endregion
            #region Healed
            else if ((match = new Regex(@"Your healing spells have a chance to instantly heal the most injured nearby party member for (?<min>\d+) to (?<max>\d+)").Match(line)).Success)
            {   // Fall of Mortality, Eye of Awareness
                // TODO: Get ICD
                stats.AddSpecialEffect(new SpecialEffect(Trigger.HealingSpellCast,
                    new Stats() { Healed = (int.Parse(match.Groups["min"].Value) + int.Parse(match.Groups["min"].Value)) / 2f, },
                    0f, 75f, 0.10f));
            }
            #endregion
            #region Intellect
            else if ((match = new Regex(@"Your healing spells have a chance to grant (?<amount>\d+) Intellect for (?<dur>\d+) sec").Match(line)).Success)
            {   // Mandala of Stirring Patterns after 4.0.6
                stats.AddSpecialEffect(new SpecialEffect(Trigger.HealingSpellCast,
                    new Stats() { Intellect = (float)int.Parse(match.Groups["amount"].Value) },
                    (float)int.Parse(match.Groups["dur"].Value), (float)int.Parse(match.Groups["dur"].Value) * 5f, 0.10f));
            }
            #endregion
            #region Mastery Rating
            else if ((match = new Regex(@"Grants (?<amount>\d+) mastery rating for (?<dur>\d+) sec each time you deal periodic spell damage, stacking up to (?<stack>\d+) times").Match(line)).Success)
            {   // Necromantic Focus
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DoTTick,
                    new Stats() { MasteryRating = int.Parse(match.Groups["amount"].Value) },
                    int.Parse(match.Groups["dur"].Value), 0f, 1f, int.Parse(match.Groups["stack"].Value)));
            }
            else if ((match = new Regex(@"Your harmful spells have a chance to grant (?<amount>\d+) mastery rating for (?<dur>\d+) sec").Match(line)).Success)
            {   // Theralion's Mirror
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageSpellHit,
                    new Stats() { MasteryRating = int.Parse(match.Groups["amount"].Value) },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 5f, 0.10f));
            }
            else if ((match = new Regex(@"Your healing spells have a chance to grant (?<amount>\d+) mastery rating for (?<dur>\d+) sec").Match(line)).Success)
            {   // Mandala of Stirring Patterns
                stats.AddSpecialEffect(new SpecialEffect(Trigger.HealingSpellCast,
                    new Stats() { MasteryRating = int.Parse(match.Groups["amount"].Value) },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 5f, 0.10f));
            }
            else if ((match = new Regex(@"Your melee attacks have a chance to grant (?<amount>\d+) mastery rating for (?<dur>\d+) sec").Match(line)).Success)
            {   // Porcelain Crab, Harrison's Insignia of Panache, Schnotzz's Medallion of Command
                stats.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit,
                     new Stats() { MasteryRating = int.Parse(match.Groups["amount"].Value) },
                     int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 5f, .10f));
            }
            else if ((match = new Regex(@"Your spells have a chance to grant (?<amount>\d+) mastery rating for (?<dur>\d+) sec").Match(line)).Success)
            {   // Talisman of Sinister Order
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellHit,
                    new Stats() { MasteryRating = int.Parse(match.Groups["amount"].Value) },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 5f, 0.10f));
            }
            #endregion
            #region Mp5
            else if ((match = new Regex(@"Each time you cast a spell, you gain (?<amount>\d+) mana per 5 sec for (?<dur>\d+) sec Stacks up to (?<stacks>\d+) times").Match(line)).Success)
            {   // Solace of the Defeated
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast,
                    new Stats() { Mp5 = (float)int.Parse(match.Groups["amount"].Value) },
                    (float)int.Parse(match.Groups["dur"].Value), 0f, 1f, int.Parse(match.Groups["stacks"].Value)));
            }
            else if ((match = new Regex(@"Each time you cast a (damaging or healing |)spell, there is a chance you will gain up to (?<amount>\d+) mana per 5 for (?<dur>\d+) sec").Match(line)).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast,
                    new Stats() { Mp5 = (float)int.Parse(match.Groups["amount"].Value) },
                    (float)int.Parse(match.Groups["dur"].Value), 45f, 0.10f));
            }
            else if ((match = new Regex(@"Your spell casts have a chance to grant (?<amount>\d+) mana per 5 sec for (?<dur>\d+) sec").Match(line)).Success)
            {   // Purified Lunar Dust
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast,
                    new Stats() { Mp5 = (float)int.Parse(match.Groups["amount"].Value) },
                    (float)int.Parse(match.Groups["dur"].Value), 45f, 0.10f));
            }
            #endregion
            #region Spell Power
            #region Patch 3.0
            else if ((match = new Regex(@"Each time you cast a damaging or healing spell you gain (?<amount>\d+) spell power for the next (?<dur>\d+) sec, stacking up to (?<stacks>\d+) times").Match(line)).Success)
            {   // Illustration of the Dragon Soul | Eye of the Broodmother
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellHit,
                    new Stats() { SpellPower = int.Parse(match.Groups["amount"].Value) },
                    int.Parse(match.Groups["dur"].Value), 0f, 1f, int.Parse(match.Groups["stacks"].Value)));
            }
            else if ((match = new Regex(@"Your harmful spells have a chance to increase your spell power by (?<amount>\d+) for (?<dur>\d+) sec").Match(line)).Success)
            {   // Sundial of the Exiled (NOT FOR HEALERS) | Flare of the Heavens
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageSpellCast,
                    new Stats() { SpellPower = int.Parse(match.Groups["amount"].Value) },
                    int.Parse(match.Groups["dur"].Value), 45, 0.10f));
            }
            else if ((match = new Regex(@"Your damaging and healing spells have a chance to increase your spell power by (?<amount>\d+) for (?<dur>\d+) sec").Match(line)).Success)
            {   // Forge Ember
                // This is a nasty trick for compatibility = when designing a healer, please use this version:
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellHit,
                    new Stats() { SpellPower = int.Parse(match.Groups["amount"].Value) },
                    int.Parse(match.Groups["dur"].Value), 45, 0.10f));
            }
            else if ((match = new Regex(@"Your spell casts have a chance to increase your spell power by (?<amount>\d+) for (?<dur>\d+) sec").Match(line)).Success)
            {   // Flow of Knowledge
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellHit,
                    new Stats() { SpellPower = int.Parse(match.Groups["amount"].Value) },
                    int.Parse(match.Groups["dur"].Value), 45, 0.10f));
            }
            else if ((match = new Regex(@"Each time you cast a harmful spell, you have a chance to gain (?<amount>\d+) spell power for (?<dur>\d+) sec").Match(line)).Success)
            {   // Abyssal Rune
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageSpellCast,
                    new Stats() { SpellPower = int.Parse(match.Groups["amount"].Value) },
                    int.Parse(match.Groups["dur"].Value), 45f, 0.25f));
            }
            #endregion
            #region Patch 3.1
            else if ((match = new Regex(@"Your spells have a chance to grant (?<amount>\d+) spell power for (?<dur>\d+) sec").Match(line)).Success)
            {   // Pandora's Plea, Anhuur's Hymnal, Tendrils of Burrowing Dark
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellHit,
                    new Stats() { SpellPower = int.Parse(match.Groups["amount"].Value) },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 5f, 0.10f));
            }
            #endregion
            #region Patch 3.3
            else if ((match = new Regex(@"Each time one of your spells deals periodic damage, you have a chance to gain (?<amount>\d+) spell power for (?<dur>\d+) sec").Match(line)).Success)
            {   // Phylactery of the Nameless Lich
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DoTTick,
                    new Stats() { SpellPower = int.Parse(match.Groups["amount"].Value) },
                    int.Parse(match.Groups["dur"].Value), 100f, 0.30f));
            }
            else if ((match = new Regex(@"Each time you deal spell damage to an opponent, you gain (?<amount>\d+) spell power for the next (?<dur>\d+) sec, stacking up to (?<stacks>\d+) times").Match(line)).Success)
            {   // Muradin's Spyglass
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageSpellHit,
                    new Stats() { SpellPower = int.Parse(match.Groups["amount"].Value) },
                    int.Parse(match.Groups["dur"].Value), 0f, 1f, int.Parse(match.Groups["stacks"].Value)));
            }
            else if ((match = new Regex(@"Your offensive spells have a chance on hit to increase your spell power by (?<amount>\d+) for (?<dur>\d+) sec").Match(line)).Success)
            {   // Ashen Band of Destruction - seems to be 60 sec iCD (wowhead)
                float internalCooldown = 45.0f;
                if (id == 50397 || id == 50398) { internalCooldown = 60.0f; }
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageSpellHit,
                    new Stats() { SpellPower = (float)int.Parse(match.Groups["amount"].Value) },
                    (float)int.Parse(match.Groups["dur"].Value), internalCooldown, 0.10f));
            }
           #endregion
            #region Patch 4.0
            // This Regex will probably not be needed after 4.0.6 given that they are changing the Bell of Enraging Resonance
            // equip effect to any damaging spell instead of any damaging spell crit.
            else if ((match = new Regex(@"Your harmful spell critical strikes have a chance to grant (?<amount>\d+) spell power for (?<dur>\d+) sec").Match(line)).Success)
            {   // Bell of Enraging Resonance
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageSpellCrit,
                    new Stats() { SpellPower = int.Parse(match.Groups["amount"].Value) },
                    int.Parse(match.Groups["dur"].Value), 100f, 0.30f));
            }
            else if ((match = new Regex(@"Your damage spells grant Heart's Revelation, increasing spell power by (?<amount>\d+) for (?<dur>\d+) sec and stacking up to (?<stacks>\d+) times").Match(line)).Success)
            {   // Heart of Ignacious
                // The first part is the overall use and cooldown.
                SpecialEffect primary = new SpecialEffect(Trigger.DamageSpellHit, new Stats(), 120f, 120f);
                // The secondary is the Heart's Revelation stacking buff.
                SpecialEffect secondary = new SpecialEffect(Trigger.DamageSpellHit,
                    new Stats() { SpellPower = int.Parse(match.Groups["amount"].Value) }, int.Parse(match.Groups["dur"].Value), 0f, 1f, int.Parse(match.Groups["stacks"].Value));
                primary.Stats.AddSpecialEffect(secondary);
                stats.AddSpecialEffect(primary);
            }
            else if ((match = new Regex(@"When you deal damage you have a chance to gain (?<amount>\d+) spell power for (?<dur>\d+) sec").Match(line)).Success)
            {   // Gladiator's Insignia of Dominance
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageSpellHit,
                    new Stats() { SpellPower = int.Parse(match.Groups["amount"].Value) },
                    int.Parse(match.Groups["dur"].Value), 100f, 0.25f));
            }
            else if ((match = new Regex(@"Your harmful spells have a chance to grant (?<amount>\d+) spell power for (?<dur>\d+) sec").Match(line)).Success)
            {   // Stump of Time
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageSpellHit,
                    new Stats() { SpellPower = int.Parse(match.Groups["amount"].Value) },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 5f, 0.10f));
            }
            else if ((match = new Regex(@"Your healing and damage periodic spells grant (?<amount>\d+) spell power each time they heal or deal damage. Lasts (?<dur>\d+) sec, stacking up to (?<stacks>\d+) times").Match(line)).Success)
            {   // Gale of Shadows
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DoTTick,
                    new Stats() { SpellPower = int.Parse(match.Groups["amount"].Value) },
                    int.Parse(match.Groups["dur"].Value), 0f, 1f, int.Parse(match.Groups["stacks"].Value)));
            }
            else if ((match = new Regex(@"Your spells that damage a target below 35% health grant (?<amount>\d+) spell power for (?<dur>\d+) sec.*\sCannot activate again for (?<cd>\d+) sec after bonus expires.*").Match(line)).Success)
            {   // Sorrowsong
                // Cooldown is using a 20 second cd (10 seconds after the duration ends)
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageSpellHit,
                    new Stats() { SpellPower = int.Parse(match.Groups["amount"].Value) },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["cd"].Value) + int.Parse(match.Groups["dur"].Value), true));
            }
            #endregion
            else if ((match = new Regex(@"Your spells have a chance to increase your spell power by (?<amount>\d+) for (?<dur>\d+) sec").Match(line)).Success)
            {   
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellHit,
                    new Stats() { SpellPower = int.Parse(match.Groups["amount"].Value) },
                    int.Parse(match.Groups["dur"].Value), 45f, 0.10f));
            }
            else if ((match = new Regex(@"Your spell critical strikes have a chance to increase your spell power by (?<amount>\d+) for (?<dur>\d+) sec").Match(line)).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellHit,
                    new Stats() { SpellPower = int.Parse(match.Groups["amount"].Value) },
                    int.Parse(match.Groups["dur"].Value), 45, 0.20f));
            }
            else if ((match = new Regex(@"Gives a chance when your harmful spells land to increase the damage of your spells and effects by up to (?<amount>\d+) for (?<dur>\d+) sec").Match(line)).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageSpellHit,
                    new Stats() { SpellPower = int.Parse(match.Groups["amount"].Value) },
                    int.Parse(match.Groups["dur"].Value), 45, 0.10f));
            }
            #endregion
            #region Spirit
            else if ((match = new Regex(@"Each time you cast a spell you gain (?<amount>\d+) Spirit for the next (?<dur>\d+) sec, stacking up to (?<stacks>\d+) times").Match(line)).Success)
            {   // Majestic Dragon Figurine
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellHit,
                    new Stats() { Spirit = int.Parse(match.Groups["amount"].Value) },
                    int.Parse(match.Groups["dur"].Value), 0f, 1f, int.Parse(match.Groups["stacks"].Value)));
            }
            else if ((match = new Regex(@"Your healing spells have a chance to grant (?<amount>\d+) Spirit for (?<dur>\d+) sec").Match(line)).Success)
            {   // Majestic Dragon Figurine, Blood of Isiset
                stats.AddSpecialEffect(new SpecialEffect(Trigger.HealingSpellCast,
                    new Stats() { Spirit = int.Parse(match.Groups["amount"].Value) },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 5f, .10f));
            }
            else if ((match = new Regex(@"Your healing spells grant Inner Eye, increasing spirit by (?<amount>\d+) for (?<dur>\d+) sec and stacking up to (?<stacks>\d+) times").Match(line)).Success)
            {   // Jar of Ancient Remedies
                // This first Special Effects causes the entire effect to last 90 seconds with a two minutes cooldown.
                SpecialEffect primary = new SpecialEffect(Trigger.HealingSpellCast, new Stats(), 90f, 120f);
                // This is the Inner Eye stacking buff.
                SpecialEffect secondary = new SpecialEffect(Trigger.HealingSpellCast,
                    new Stats() { Spirit = int.Parse(match.Groups["amount"].Value) },
                    int.Parse(match.Groups["dur"].Value), 0f, 1f, int.Parse(match.Groups["stacks"].Value));
                primary.Stats.AddSpecialEffect(secondary);
                stats.AddSpecialEffect(primary);
            }
            // TODO finalize chance rating.
            else if ((match = new Regex(@"Your healing spells have a chance to increase your Spirit by (?<amount>\d+) for (?<dur>\d+) sec.*\ This effect can stack up to (?<stacks>\d+) times").Match(line)).Success)
            {   // Darkmoon Card: Tsunami
                stats.AddSpecialEffect(new SpecialEffect(Trigger.HealingSpellCast,
                    new Stats() { Spirit = int.Parse(match.Groups["amount"].Value) },
                    int.Parse(match.Groups["dur"].Value), 0f, .50f, int.Parse(match.Groups["stacks"].Value)));
            }
            else if ((match = new Regex(@"Your healing spell critical strikes have a chance to grant (?<amount>\d+) spirit for (?<dur>\d+) sec").Match(line)).Success)
            {   // Tear of Blood
                stats.AddSpecialEffect(new SpecialEffect(Trigger.HealingSpellCrit,
                    new Stats() { Spirit = int.Parse(match.Groups["amount"].Value) },
                    int.Parse(match.Groups["dur"].Value), 75f, .30f));
            }
            #endregion
            #region Specialty Stat Procs
            else if ((match = new Regex(@"Your melee and ranged attacks have a chance to grant (?<amount>\d+) Critical Strike Rating, Haste Rating, or Mastery Rating, whichever is currently highest").Match(line)).Success)
            {   // Matrix Restabilizer
                // TO DO: Get length of Proc and Proc chance
                float duration = 15f;
                stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalAttack, new Stats() { HighestSecondaryStat = int.Parse(match.Groups["amount"].Value) },
                    duration, duration * 5f, 0.10f));
            }
            else if ((match = new Regex(@"When you heal or deal damage you have a chance to gain Greatness").Match(line)).Success)
            {   // Darkmoon Card: Greatness
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageOrHealingDone, new Stats() { HighestStat = 300f }, 15f, 45f, .33f));
            }
            else if ((match = new Regex(@"Your harmful spells have a chance to cause you to summon a Val'kyr to fight by your side for 30 sec").Match(line)).Success)
            {   // Nibelung
                // source http://elitistjerks.com/f75/t37825-rawr_mage/p42/#post1517923
                // 5% crit rate, 1.5 crit multiplier, not affected by talents, affected only by target debuffs
                float damage = (id == 50648) ? 30000 : 27008; // enter value for heroic when it becomes known
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageSpellCast, new Stats() { ValkyrDamage = damage }, 0f, 0f, 0.02f));
            }
            else if ((match = new Regex(@"Your melee attacks have a chance to cause a (?<amount>\d+)% increase to the damage done by your melee autoattacks for (?<dur>\d+) sec").Match(line)).Success)
            {   // Unheeded Warning
                stats.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit,
                    new Stats() { BonusWhiteDamageMultiplier = .01f * int.Parse(match.Groups["amount"].Value) },
                    int.Parse(match.Groups["dur"].Value), 50f, 0.30f));
            }
            #endregion
            #region Strength
            else if ((match = new Regex(@"Your melee attacks have a chance to grant (?<amount>\d+) Strength for (?<dur>\d+) sec").Match(line)).Success)
            {   // Heart of Rage, Heart of Solace
                stats.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit,
                     new Stats() { Strength = int.Parse(match.Groups["amount"].Value) },
                     int.Parse(match.Groups["dur"].Value), 100f, .10f));
            }
            else if ((match = new Regex(@"When you deal damage you have a chance to gain (?<amount>\d+) Strength for (?<dur>\d+) sec").Match(line)).Success)
            {   // Gladiator's Insignia of Victory
                stats.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit,
                    new Stats() { Strength = int.Parse(match.Groups["amount"].Value) },
                    int.Parse(match.Groups["dur"].Value), 100f, 0.15f));
            }
            else if ((match = new Regex(@"Your melee attacks grant (?<amount>\d+) Strength. Lasts (?<dur>\d+) sec, stacking up to (?<stacks>\d+) times").Match(line)).Success)
            {   // License to Slay
                stats.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit,
                    new Stats() { Strength = int.Parse(match.Groups["amount"].Value) },
                    int.Parse(match.Groups["dur"].Value), 0f, 1f, int.Parse(match.Groups["stacks"].Value)));
            }
            else if ((match = new Regex(@"Your melee critical strikes have a chance to grant (?<amount>\d+) Strength for (?<dur>\d+) sec").Match(line)).Success)
            {
                // Patch 4.0.6+ Chance to proc went from 30% to 50% on Melee Crit.
                // Right Eye of Rajh
                stats.AddSpecialEffect(new SpecialEffect(Trigger.MeleeCrit,
                        new Stats() { Strength = int.Parse(match.Groups["amount"].Value) },
                        int.Parse(match.Groups["dur"].Value), 50f, .50f));
            }
            #endregion
            #region Weapon Damage
            else if ((match = new Regex(@"Your melee attacks have a chance to increase your weapon damage by (?<amount>\d+) for (?<dur>\d+) sec").Match(line)).Success)
            {   // Patch 4.0.6 Unheeded Warning
                stats.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit,
                     new Stats() { WeaponDamage = int.Parse(match.Groups["amount"].Value) },
                     int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 5f, .10f));
            }
            #endregion
            #region Misc
            else if (line.StartsWith("When struck in combat has a chance of shielding you in a protective barrier which will reduce damage from each attack by 140"))
            {   // Essence of Gossamer 
                // 140 damage reduced from EACH attack.
                // Lasts 10 secs.
                // So this isn't a normal X amount absorbed period over 10 sec. like a Corroded Skeleton Key or priest's bubble.
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageTaken,
                                    new Stats() { DamageAbsorbed = 140f },
                                    10, 1, 0.05f));
            }
            else if (line.StartsWith("Protected from the cold.  Your Frost resistance is increased by 20"))
            {   // Mechanized Snow Goggles of the...
                stats.FrostResistance = 20;
            }
            else if ((match = new Regex(@"You gain (?:a|an) (?<buffName>[\w\s]+) each time you cause a (?<trigger>non-periodic|damaging)+ spell critical strike, granting a chance to fire a (?<projectile>[\w\s]+) for (?<mindmg>\d+) to (?<maxdmg>\d+) damage per +[\w\s]+ accumulated\.(?:\s|nbsp;)+You cannot have more than (?<stackSize>\d+) +[\w\s]+ and cannot gain one more often than once every (?<icd>\d+(?:\.\d+)?) sec").Match(line)).Success)
            {   // Variable Pulse Lightning Capacitor
                string buffName = match.Groups["buffName"].Value.TrimStart(' ');
                int stackSize = int.Parse(match.Groups["stackSize"].Value);
                string projectile = match.Groups["projectile"].Value.TrimStart(' ');
                int mindmg = int.Parse(match.Groups["mindmg"].Value);
                int maxdmg = int.Parse(match.Groups["maxdmg"].Value);
                float avgdmgperstack = (mindmg + maxdmg) / 2f / stackSize;
                float icd = float.Parse(match.Groups["icd"].Value, System.Globalization.CultureInfo.InvariantCulture);
                Trigger trigger = Trigger.SpellCrit;
                if (match.Groups["trigger"].Value.StartsWith("damaging"))
                    trigger = Trigger.DamageSpellCrit;
                Stats projectileStats = new Stats();

                if (projectile.StartsWith("Electrical Charge"))
                    projectileStats.NatureDamage = avgdmgperstack;
                stats.AddSpecialEffect(new SpecialEffect(trigger, projectileStats, 0f, icd));
            }
            else if ((match = Regex.Match(line, @"You gain (?:a|an) (?<buffName>[\w\s]+) each time you cause a (?<trigger>non-periodic|damaging)+ spell critical strike\.(?:\s|nbsp;)+When you reach (?<stackSize>\d+) [\w\s]+, they will release, firing (?<projectile>[\w\s]+) for (?<mindmg>\d+) to (?<maxdmg>\d+) damage\.+[\w\s]+ cannot be gained more often than once every (?<icd>\d+(?:\.\d+)?) sec")).Success)
            {
                // Capacitor like procs
                string buffName = match.Groups["buffName"].Value.TrimStart(' ');
                int stackSize = int.Parse(match.Groups["stackSize"].Value);
                string projectile = match.Groups["projectile"].Value.TrimStart(' ');
                int mindmg = int.Parse(match.Groups["mindmg"].Value);
                int maxdmg = int.Parse(match.Groups["maxdmg"].Value);
                float avgdmgperstack = (mindmg + maxdmg) / 2f / stackSize;
                float icd = float.Parse(match.Groups["icd"].Value, System.Globalization.CultureInfo.InvariantCulture);
                Trigger trigger = Trigger.SpellCrit;
                if (match.Groups["trigger"].Value.StartsWith("damaging"))
                    trigger = Trigger.DamageSpellCrit;
                Stats projectileStats = new Stats();

                if (projectile.StartsWith("a Lightning Bolt"))
                    projectileStats.NatureDamage = avgdmgperstack;
                else if (projectile.StartsWith("a Pillar of Flame"))
                    projectileStats.FireDamage = avgdmgperstack;
                stats.AddSpecialEffect(new SpecialEffect(trigger, projectileStats, 0f, icd));
            }
            // Shadowmourne
            // Your melee attacks have a chance to drain a Soul Fragment granting you 30 Strength.  When you have acquired 10 Soul Fragments you will unleash Chaos Bane, dealing 1900 to 2100 Shadow damage split between all enemies within 15 yards and granting you 270 Strength for 10 sec.
            else if ((match = Regex.Match(line, @"Your melee attacks have a chance to drain a Soul Fragment granting you 30 Strength")).Success)
            //            else if ((match = Regex.Match(line, @"Your melee attacks have a chance to drain a Soul Fragment granting you (?<StrBuff>\d+) Strength.  When you have acquired (?<stackSize>\d+) Soul Fragments you will unleash Chaos Bane, dealing (?<mindmg>\d+) to (?<maxdmg>\d+) Shadow damage split between all enemies within 15 yards and granting you (?<strBuff2>\d+) Strength for (?<icd>) sec")).Success)
            {
                // Capacitor like procs
                Stats BuffStats = new Stats();
                Stats BuffStats2 = new Stats();
                // How much strength per stack.
                BuffStats.Strength = 30;
                // Stack size
                int stackSize = 10;
                // Damage of the final proc.
                int mindmg = 1900;
                int maxdmg = 2100;
                float avgdmg = (mindmg + maxdmg) / 2f;
                // 2ndary Strength Buff
                BuffStats2.Strength = 270;
                BuffStats2.ShadowDamage = avgdmg;
                float icd = 10;
                Trigger trigger = Trigger.MeleeHit;
                // At 10 stacks, the stacks proc the 2nd effect, so you would never have 10 stacks.
                stats.AddSpecialEffect(new SpecialEffect(trigger, BuffStats, 60f, 0f, .2f, (stackSize - 1)));
                stats.AddSpecialEffect(new SpecialEffect(trigger, BuffStats2, icd, 0f, .02f));
            }
            #endregion
            #endregion

            else if (line.StartsWith("Increases your pet's critical strike chance by "))
            {
                string critChance = line.Substring("Increases your pet's critical strike chance by ".Length).Trim();
                if (critChance.EndsWith("%"))
                {
                    stats.BonusPetCritChance = float.Parse(critChance.Substring(0, critChance.Length - 2)) / 100f;
                }
            }
            else if ((match = Regex.Match(line, @"Reduces the base mana cost of your spells by (?<amount>\d+)")).Success)
            {
                stats.SpellsManaCostReduction = int.Parse(match.Groups["amount"].Value);
            }
            else if (line.StartsWith("Your spell critical strikes have a chance to restore 900 mana"))
            {
                // Soul of the Dead
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCrit, new Stats() { ManaRestore = 900f }, 0f, 45f, .25f));
            }
            else if (line.StartsWith("Your direct healing spells have a chance to place a heal over time on your target"))
            {
                Regex r = new Regex("Your direct healing spells have a chance to place a heal over time on your target, healing (?<hot>\\d*) over (?<dur>\\d*) sec");
                Match m = r.Match(line);
                if (m.Success)
                {
                    float hot = int.Parse(m.Groups["hot"].Value);
                    float dur = int.Parse(m.Groups["dur"].Value);
                    // internal cooldown: 45 seconds
                    // 20% chance, so on average procs after 5 casts
                    stats.AddSpecialEffect(new SpecialEffect(Trigger.HealingSpellCast, new Stats() { Healed = hot }, 0f, 45f, .2f));
                }
            }
            else if ((match = new Regex(@"Steals (?<amount1>\d+) to (?<amount2>\d+) life from target enemy.*").Match(line)).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit, new Stats()
                {
                    ShadowDamage = (int.Parse(match.Groups["amount1"].Value) + int.Parse(match.Groups["amount2"].Value)) / 2f,
                    //ProcdShadowDamageMin = int.Parse(match.Groups["amount1"].Value),
                    //ProcdShadowDamageMax = int.Parse(match.Groups["amount2"].Value),
                    // Stealing health means that some is restored to the user.
                    HealthRestore = (int.Parse(match.Groups["amount1"].Value) + int.Parse(match.Groups["amount2"].Value)) / 2f,

                }, 0f, 0f, -2f));
            }
            else if (line == "Your healing spells have a chance to cause Blessing of Ancient Kings for 15 sec allowing your heals to shield the target absorbing damage equal to 15% of the amount healed")
            {
                // Val'anyr, Hammer of Ancient Kings effect
                stats.AddSpecialEffect(new SpecialEffect(Trigger.HealingSpellHit, new Stats() { ShieldFromHealedProc = .15f }, 15f, 45f, .1f));
            }
            else if (line.EndsWith("Weapon Damage"))
            {
                line = line.Trim('+').Substring(0, line.IndexOf(" "));
                stats.WeaponDamage += int.Parse(line);
            }
            #region Special Armory Tags
            else if (isArmory && line.StartsWith("Increases spell power by"))
            {
                line = line.Substring("Increases spell power by".Length);
                line = line.Replace(".", "").Replace(" ", "");
                stats.SpellPower += int.Parse(line);
            }
            else if (isArmory && line.StartsWith("Increases damage done by Shadow spells and effects by up to"))
            {
                line = line.Substring("Increases damage done by Shadow spells and effects by up to".Length);
                line = line.Replace(".", "").Replace(" ", "");
                stats.SpellShadowDamageRating += int.Parse(line);
            }
            else if (isArmory && line.StartsWith("Increases damage done by Fire spells and effects by up to"))
            {
                line = line.Substring("Increases damage done by Fire spells and effects by up to".Length);
                line = line.Replace(".", "").Replace(" ", "");
                stats.SpellFireDamageRating += int.Parse(line);
            }
            else if (isArmory && line.StartsWith("Increases damage done by Frost spells and effects by up to"))
            {
                line = line.Substring("Increases damage done by Frost spells and effects by up to".Length);
                line = line.Replace(".", "").Replace(" ", "");
                stats.SpellFrostDamageRating += int.Parse(line);
            }
            else if (isArmory && line.StartsWith("Increases damage done by Arcane spells and effects by up to"))
            {
                line = line.Substring("Increases damage done by Arcane spells and effects by up to".Length);
                line = line.Replace(".", "").Replace(" ", "");
                stats.SpellArcaneDamageRating += int.Parse(line);
            }
            else if (isArmory && line.StartsWith("Increases damage done by Nature spells and effects by up to"))
            {
                line = line.Substring("Increases damage done by Nature spells and effects by up to".Length);
                line = line.Replace(".", "").Replace(" ", "");
                stats.SpellNatureDamageRating += int.Parse(line);
            }
            else if (isArmory && line.StartsWith("Improves haste rating by"))
            {
                line = line.Substring("Improves haste rating by".Length);
                line = line.Replace(".", "").Replace(" ", "");
                stats.HasteRating += int.Parse(line);
            }
            else if (isArmory && line.StartsWith("Improves critical strike rating by"))
            {
                line = line.Substring("Improves critical strike rating by".Length);
                line = line.Replace(".", "").Replace(" ", "");
                stats.CritRating += int.Parse(line);
            }
            else if (isArmory && line.StartsWith("Increases your spell penetration by"))
            {
                line = line.Substring("Increases your spell penetration by".Length);
                line = line.Replace(".", "").Replace(" ", "");
                stats.SpellPenetration += int.Parse(line);
            }
            else if (isArmory && line.StartsWith("Increases hit rating by "))
            {
                line = line.Substring("Increases hit rating by ".Length);
                line = line.Replace(".", "").Replace(" ", "");
                stats.HitRating += int.Parse(line);
            }
            // Restores 7 mana per 5 sec
            // Check to see if the desc contains the token 'mana'.  Items like Frostwolf Insignia
            // and Essense Infused Shroom Restore health.
            else if (isArmory && line.StartsWith("Restores ") && line.Contains("mana") && !line.Contains("kill a target"))
            {
                line = line.Substring("Restores ".Length);
                line = line.Substring(0, line.IndexOf(" mana"));
                stats.Mp5 += int.Parse(line);
            }
            else if (isArmory && line.StartsWith("Increases attack power by ") && !line.Contains("forms only"))
            {
                line = line.Substring("Increases attack power by ".Length);
                if (line.Contains(".")) line = line.Substring(0, line.IndexOf("."));
                if (line.Contains(" ")) line = line.Substring(0, line.IndexOf(" "));
                stats.AttackPower += int.Parse(line);
            }
            else if (isArmory && line.StartsWith("Increases your dodge rating by "))
            {
                line = line.Substring("Increases your dodge rating by ".Length);
                if (line.Contains(".")) line = line.Substring(0, line.IndexOf("."));
                if (line.Contains(" ")) line = line.Substring(0, line.IndexOf(" "));
                stats.DodgeRating += int.Parse(line);
            }
            else if (isArmory && line.StartsWith("Increases your parry rating by "))
            {
                line = line.Substring("Increases your parry rating by ".Length);
                if (line.Contains(".")) line = line.Substring(0, line.IndexOf("."));
                if (line.Contains(" ")) line = line.Substring(0, line.IndexOf(" "));
                stats.ParryRating += int.Parse(line);
            }
            else if (isArmory && line.StartsWith("Increases your shield block rating by "))
            {
                line = line.Substring("Increases your shield block rating by ".Length);
                if (line.Contains(".")) line = line.Substring(0, line.IndexOf("."));
                if (line.Contains(" ")) line = line.Substring(0, line.IndexOf(" "));
                stats.BlockRating += int.Parse(line);
            }
            else if (isArmory && line.StartsWith("Increases your block rating by "))
            {
                line = line.Substring("Increases your block rating by ".Length);
                if (line.Contains(".")) line = line.Substring(0, line.IndexOf("."));
                if (line.Contains(" ")) line = line.Substring(0, line.IndexOf(" "));
                stats.BlockRating += int.Parse(line);
            }
            else if (isArmory && line.StartsWith("Increases your hit rating by "))
            {
                line = line.Substring("Increases your hit rating by ".Length);
                if (line.Contains(".")) line = line.Substring(0, line.IndexOf("."));
                if (line.Contains(" ")) line = line.Substring(0, line.IndexOf(" "));
                stats.HitRating += int.Parse(line);
            }
            #endregion
            #region 3.0.1 Trinkets
            else if ((match = Regex.Match(line, @"Chance on melee or ranged attack to enter Wracking Pains, during which your attacks will each grant 15 crit rating, stacking up to 10 times. Expires after 20 sec")).Success)
            {
                // Death Knight's Anguish
                SpecialEffect primary = new SpecialEffect(Trigger.PhysicalAttack, new Stats() { }, 20f, 45f, 0.10f);
                SpecialEffect secondary = new SpecialEffect(Trigger.PhysicalAttack, new Stats() { CritRating = 15, }, 20f, 0f, 1f, 10);
                primary.Stats.AddSpecialEffect(secondary);
                stats.AddSpecialEffect(primary);
            }
            #endregion
            #region 3.2 Trinkets
            else if ((match = Regex.Match(line.Replace("nbsp;", " "), @"When you deal damage you have a chance to gain Paragon, increasing your Strength or Agility by (?<amount>\d+) for 15 sec Your highest stat is always chosen")).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageDone, new Stats() { Paragon = int.Parse(match.Groups["amount"].Value) }, 15f, 45f, 0.35f));
            }
            else if ((match = Regex.Match(line, @"Each time you cast a helpful spell, you have a chance to gain (?<amount>\d+) mana")).Success)
            {
                // Ephemeral Snowflake
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { ManaRestore = int.Parse(match.Groups["amount"].Value) }, 0f, 45f, 0.4f));
            }
            #endregion
            #region Icecrown Weapon Procs
            else if ((match = Regex.Match(line, @"Your melee attacks have a chance to grant you Necrotic Touch for 10 sec, causing all your melee attacks to deal an additional (?<amount>\d+)% damage as shadow damage")).Success)
            {
                // Black Bruise
                stats.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit, new Stats() { BonusPhysicalDamageMultiplier = int.Parse(match.Groups["amount"].Value) / 100f }, 10f, 0f, 0.033f));
            }
            else if ((match = Regex.Match(line, @"Each time your spells heal a target you have a chance to cause the target of your heal to heal themselves and friends within 10 yards for (?<amount>\d+) each sec for 6 sec")).Success)
            {
                // Trauma; Procs on Healing Hit and HoTs; Hits up to 7 people
                stats.AddSpecialEffect(new SpecialEffect(Trigger.HealingSpellHit, new Stats() { Healed = int.Parse(match.Groups["amount"].Value) * 7f }, 6f, 0f, 0.01f, 0));
            }
            else if ((match = Regex.Match(line, @"Your melee attacks have a chance to grant you Blessing of Light, increasing your strength by (?<stramount>\d+) and your healing received by up to (?<healamount>\d+) for 10 sec")).Success)
            {
                // Last Word
                stats.AddSpecialEffect(new SpecialEffect(Trigger.HealingSpellHit, new Stats() { Strength = int.Parse(match.Groups["stramount"].Value), BonusHealingReceived = int.Parse(match.Groups["healamount"].Value) }, 10f, 0f, 0.37f, 1));
            }
            #endregion
            #region 3.3 Trinkets
            else if ((match = Regex.Match(line, @"Each time you are struck by a melee attack, you have a 60% chance to gain (?<stamina>\d+) stamina for the next 10 sec, stacking up to 10 times")).Success)
            {
                // Unidentifiable Organ
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageTakenPhysical, new Stats() { Stamina = int.Parse(match.Groups["stamina"].Value) }, 10.0f, 0.0f, 0.6f, 10));
            }
            else if ((match = Regex.Match(line, @"Your melee attacks have a chance to grant you a Mote of Anger\. (nbsp;| )?When you reach (?<amount>\d+) Motes of Anger, they will release, causing you to instantly attack for 50% weapon damage with one of your melee weapons")).Success)
            {
                // Tiny Abomination Jar
                stats.AddSpecialEffect(new SpecialEffect(Trigger.MeleeAttack, new Stats() { MoteOfAnger = 0.5f }, 0f, 0f, 0.5f, int.Parse(match.Groups["amount"].Value)));
            }
            else if ((match = Regex.Match(line, @"You gain (?<mana>\d+) mana each time you heal a target with one of your spells")).Success)
            {
                // Epheremal Snowflake - assume iCD of 0.33 sec (wowhead)
                stats.AddSpecialEffect(new SpecialEffect(Trigger.HealingSpellHit, new Stats() { ManaRestore = int.Parse(match.Groups["mana"].Value) }, 0f, 0.33f, 1f));
            }
            else if ((match = new Regex(@"Each time your spells heal a target you have a chance to cause another nearby friendly target to be instantly healed for (?<min>\d+) to (?<max>\d+)").Match(line)).Success)
            {
                // Althor's Abacus
                float min = (float)int.Parse(match.Groups["min"].Value);
                float max = (float)int.Parse(match.Groups["max"].Value);
                stats.AddSpecialEffect(new SpecialEffect(Trigger.HealingSpellHit, new Stats() { Healed = min + (max - min) / 2f }, 0f, 45f, 0.3f));
            }
            else if ((match = new Regex(@"Your harmful spells have a chance to increase your spell power by (?<initialSP>\d+) and an additional (?<additionalSP>\d+) every (?<frequency>\d+) sec for (?<time>\d+) sec").Match(line)).Success)
            {
                // Dislodged Foreign Object - seems to be 45 sec iCD (wowhead)
                float initialSP = (float)int.Parse(match.Groups["initialSP"].Value);
                float additionalSP = (float)int.Parse(match.Groups["additionalSP"].Value);
                float frequency = (float)int.Parse(match.Groups["frequency"].Value);
                float time = (float)int.Parse(match.Groups["time"].Value);
                float min = initialSP;
                float max = initialSP + additionalSP * (time / frequency - 1f);
                float averageSP = (min + max) / 2f;
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageSpellHit, new Stats() { SpellPower = averageSP }, time, 45f, 0.1f));
            }
            #endregion
            #region 3.3 rings
            else if ((match = new Regex(@"Your helpful spells have a chance to increase your spell power by (?<power>\d+) for (?<time>\d+) sec").Match(line)).Success)
            {
                // Ashen Band of Wisdom - seems to be 60 sec iCD (wowhead)
                float spellpower = (float)int.Parse(match.Groups["power"].Value);
                float time = (float)int.Parse(match.Groups["time"].Value);
                stats.AddSpecialEffect(new SpecialEffect(Trigger.HealingSpellCast, new Stats() { SpellPower = spellpower }, time, 60f, 0.1f));
            }
            else if ((match = new Regex(@"When struck in combat has a chance of increasing your armor by (?<armor>\d+) for (?<time>\d+) sec").Match(line)).Success)
            {
                // Ashen Band of Courage - seems to be 60 sec iCD (wowhead)
                // Apparently confirmed as 3% chance to proc -> http://maintankadin.failsafedesign.com/forum/index.php?f=21&t=27115&rb_v=viewtopic
                float armor = (float)int.Parse(match.Groups["armor"].Value);
                float time = (float)int.Parse(match.Groups["time"].Value);
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageTaken, new Stats() { BonusArmor = armor }, time, 60f, 0.03f));
            }


            #endregion
            #region 3.3.5 Trinkets
            // Note that Sharpened Twilight Scale already is modeled via Whispering Fanged Skull
            else if ((match = new Regex(@"Your damaging spells have a chance to grant (?<spellPower>\d+) spell power for 15 sec").Match(line)).Success)
            {
                // Charred Twilight Scale
                float spellPower = int.Parse(match.Groups["spellPower"].Value);
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageSpellHit, new Stats() { SpellPower = spellPower }, 15.0f, 45.0f, 0.1f));
            }
            else if ((match = new Regex(@"Melee attacks which reduce you below 35% health cause you to gain (a protective shield which absorbs\s+)?(?<amount>\d+) (?<stat>dodge|mastery|(bonus\s+)?armor|damage) (rating\s+)?for (?<dur>\d+) sec.?"
                                      + @"\s+Cannot occur more than once every (?<cooldown>\d+)\s+(sec|min).?").Match(line)).Success)
            {
                /* ===== This one Regex should handle the following items (and anything similar) =====
                 * Corpse Tongue Coin [50349] [50352] (Bonus Armor)
                 * Symbiotic Worm [59332] [65048] (Mastery Rating)
                 * Bedrock Talisman [58182] (Dodge Rating)
                 * Gift of the Greatfather [69138] (Damage Absorb) (This is PTR for next wow patch)
                 * Leaden Despair [55816] [56347] (Bonus Armor)
                */
                // Duration/Cooldown
                float dur = int.Parse(match.Groups["dur"].Value);
                float icd = int.Parse(match.Groups["cooldown"].Value);
                // Determine which value to boost
                float mastery = 0f, dodge = 0f, bonusArmor = 0f, damageAbsorb = 0f;
                if (match.Groups["stat"].Value == "dodge") {
                    dodge = int.Parse(match.Groups["amount"].Value);
                } else if (match.Groups["stat"].Value == "mastery") {
                    mastery = int.Parse(match.Groups["amount"].Value);
                } else if (match.Groups["stat"].Value.Contains("armor")) {
                    bonusArmor = int.Parse(match.Groups["amount"].Value);
                } else if (match.Groups["stat"].Value == "damage") {
                    damageAbsorb = int.Parse(match.Groups["amount"].Value);
                    icd *= 60; // this one is in minutes
                }
                //
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageTakenPutsMeBelow35PercHealth,
                    new Stats() { DodgeRating = dodge, MasteryRating = mastery, BonusArmor = bonusArmor, DamageAbsorbed = damageAbsorb, },
                    dur, icd));
            }
            else if ((match = new Regex(@"Melee attacks which reduce you below 35% health cause you to gain (?<amount>\d+) bonus armor for (?<dur>\d+) sec.*\ Cannot occur more than once every (?<cd1>\d+) sec").Match(line)).Success)
            {   // Leaden Despair
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageTakenPhysical,
                    new Stats() { BonusArmor = int.Parse(match.Groups["amount"].Value) },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["cd1"].Value), 0.175f));
            }
            #endregion
            else
            {
                #region Prototype Sigil Code:
                /* Prototype Sigil Code:

             Sigil of the Unfaltering Knight:
               Your Icy Touch will also increase your defense rating by 53.
             * Your [ability] will also increase your [stat] by [amount].
             Furious Gladiator's Sigil of Strife
               Your Plague Strike ability also grants you 144 attack power for 10 sec.
             * Your [ability] ability also grants you [amount] [stat] for [duration] sec.
             Sigil of Deflection
               Your Rune Strike ability grants 136 dodge rating for 5 sec.
             * Your [ability] ability also grants you [amount] [stat] for [duration] sec.
             Sigil of Insolence
               Each time you use your Rune Strike ability, you have a chance to gain 200 dodge rating for 20 sec.
             * Each time you use your [ability] ability, you have a chance to gain [amount] [stat] for [duration] sec.
             Sigil of the Vengeful Heart
               Increases the damage done by your Death Coil and Frost Strike abilities by 380.
             * Increases the damage done by your [ability] and [ability] abilities by [amount].
             Deadly Gladiator's Sigil of Strife
               Your Plague Strike ability also grants you 120 attack power for 10 sec.
             * Your [ability] ability also grants you [amount] [stat] for [duration] sec.
            Sigil of Awareness
                Increases the base damage dealt by your Scourge Strike by 189, your Obliterate by 336, and your Death Strike by 315.
             * Increases the base damage dealt by your [ability1] by [amount1], your [ability2] by [amount2], and your [ability3] by [amount3].
            Hateful Gladiator's Sigil of Strife
                Your Plague Strike ability also grants you 106 attack power for 6 sec.
             * Your [ability] ability also grants you [amount] [stat] for [duration] sec.
            Sigil of Haunted Dreams
                Your Blood Strike and Heart Strikes have a chance to grant 173 critical strike rating for 10 sec.
             * Your [ability] and [ability] have a chance to grant [amount] [stat] for [duration] sec.
             * Key point (chance) means proc rate factoring
            Savage Gladiator's Sigil of Strife
                Your Plague Strike ability also grants you 94 attack power for 6 sec.
             * Your [ability] ability also grants you [amount] [stat] for [duration] sec.
            Sigil of Arthritic Binding
                Increases the damage dealt by your Scourge Strike ability by 91.35.
             * Increases the damage dealt by your [ability] ability by [amount].
            Sigil of the Frozen Conscience
                Increases the damage dealt by your Icy Touch ability by 111.
             * Increases the damage dealt by your [ability] ability by [amount].
            Sigil of the Wild Buck
                Increases the damage dealt by your Death Coil ability by 80.
             * Increases the damage dealt by your [ability] ability by [amount].
            Sigil of the Dark Rider
                Increases the damage dealt by your Blood Strike and Heart Strike by 90.
             * Increases the damage dealt by your [ability] and [ability] by [amount].
            Sigil of the Bone Gryphon
                 * Your Rune Strike ability grants 44 dodge rating for 15 sec.  Stacks up to 5 times.
                 * Your Rune Strike ability grants [Amount] [stat] for [duration] sec.  Stacks up to [stack] times.
            */
                // Single Ability.
                Regex regex = new Regex(@"Your (?<ability>\w+\s*\w*) (ability )?(also )?grants (you )?(?<amount>\d*) (?<stat>\w+\s*\w*) for (?<duration>\d*) sec(\s*Stacks up to (?<stack>\d*) times)?");
                match = regex.Match(line);
                if (match.Success)
                {
                    string statName = match.Groups["stat"].Value;
                    float amount = int.Parse(match.Groups["amount"].Value);
                    float duration = int.Parse(match.Groups["duration"].Value);
                    string ability = match.Groups["ability"].Value;
                    int stack = 0;
                    if (match.Groups["stack"].Value != "")
                    {
                        stack = int.Parse(match.Groups["stack"].Value);
                    }

                    stats.AddSpecialEffect(EvalRegex(statName, amount, duration, ability, 0f, 1f, stack));
                }
                // ok... at this point with the way the code is written, this is really just for Sigil of the Unfaltering Knight, which grants a 
                // 30 sec buff of 53 Def. Since it's a triggered event, and all, it's better to go off the trigger rather than a straight 
                // 53 def all the time.  So adjusting this to be a special effect and assuming 30 secs.
                regex = new Regex(@"Your (?<ability>\w+\s*\w*) (ability )?(will )?(also )?increase (your )?(?<stat>\w+\s*\w*) by (?<amount>\d*)");
                match = regex.Match(line);
                if (match.Success)
                {
                    string statName = match.Groups["stat"].Value;
                    float amount = int.Parse(match.Groups["amount"].Value);
                    string ability = match.Groups["ability"].Value;

                    stats.AddSpecialEffect(EvalRegex(statName, amount, 30, ability, 0f));
                }

                regex = new Regex(@"Your (?<ability>\w+\s*\w*) and (?<ability2>\w+\s*\w*) (abilities )?have a chance to grant (?<amount>\d*) (?<stat>\w+[\s\w]*) for (?<duration>\d*) sec");
                match = regex.Match(line);
                if (match.Success)
                {
                    string statName = match.Groups["stat"].Value;
                    float amount = int.Parse(match.Groups["amount"].Value);
                    float duration = int.Parse(match.Groups["duration"].Value);
                    string ability = match.Groups["ability"].Value;
                    string ability2 = match.Groups["ability2"].Value;

                    SpecialEffect SE1 = EvalRegex(statName, amount, duration, ability, 0f, 0.15f);
                    stats.AddSpecialEffect(SE1);
                    SpecialEffect SE2 = EvalRegex(statName, amount, duration, ability2, 0f, 0.15f);
                    if (SE1.ToString() != SE2.ToString())
                    {
                        stats.AddSpecialEffect(SE2);
                    }
                }
                /*
                 * Sigil of the Hanged Man
                 * Equip: Your Obliterate, Scourge Strike, and Death Strike abilities grants 73 Strength for 15 sec.  Stacks up to 3 times.
                 * 		line	"Your Obliterate, Scourge Strike, and Death Strike abilities grants 73 Strength for 15 sec.  Stacks up to 3 times"	string
                 * */
                regex = new Regex(@"Your (?<ability>\w+\s*\w*), (?<ability2>\w+\s*\w*), and (?<ability3>\w+\s*\w*) (abilities )?grants (?<amount>\d*) (?<stat>\w+[\s\w]*) for (?<duration>\d*) sec Stacks up to (?<stacks>\d*) times");
                match = regex.Match(line);
                if (match.Success)
                {
                    string statName = match.Groups["stat"].Value;
                    float amount = int.Parse(match.Groups["amount"].Value);
                    float duration = int.Parse(match.Groups["duration"].Value);
                    int stacks = int.Parse(match.Groups["stacks"].Value);
                    string ability = match.Groups["ability"].Value;
                    string ability2 = match.Groups["ability2"].Value;
                    string ability3 = match.Groups["ability3"].Value;

                    SpecialEffect SE1 = EvalRegex(statName, amount, duration, ability, 0f, 1f, stacks);
                    stats.AddSpecialEffect(SE1);
                    SpecialEffect SE2 = EvalRegex(statName, amount, duration, ability2, 0f, 1f, stacks);
                    SpecialEffect SE3 = EvalRegex(statName, amount, duration, ability3, 0f, 1f, stacks);
                    if (SE1.ToString() != SE2.ToString())
                    {
                        stats.AddSpecialEffect(SE2);
                    }
                    if (SE3.ToString() != SE2.ToString() && SE3.ToString() != SE1.ToString())
                    {
                        stats.AddSpecialEffect(SE3);
                    }
                }

                regex = new Regex(@"Each time you use your (?<ability3>\w+\s*\w*), (?<ability>\w+\s*\w*), or (?<ability2>\w+\s*\w*) ability, you have a chance to gain (?<amount>\d*) (?<stat>\w+[\s\w]*) for (?<duration>\d*) sec");
                match = regex.Match(line);
                if (match.Success)
                {
                    string statName = match.Groups["stat"].Value;
                    float amount = int.Parse(match.Groups["amount"].Value);
                    float duration = int.Parse(match.Groups["duration"].Value);
                    string ability = match.Groups["ability"].Value;
                    string ability2 = match.Groups["ability2"].Value;
                    string ability3 = match.Groups["ability3"].Value;

                    SpecialEffect SE1 = EvalRegex(statName, amount, duration, ability, 10f, 0.8f);
                    stats.AddSpecialEffect(SE1);
                    SpecialEffect SE2 = EvalRegex(statName, amount, duration, ability2, 10f, 0.8f);
                    SpecialEffect SE3 = EvalRegex(statName, amount, duration, ability3, 10f, 0.8f);
                    if (SE1.ToString() != SE2.ToString())
                    {
                        stats.AddSpecialEffect(SE2);
                    }
                    if (SE3.ToString() != SE2.ToString())
                    {
                        stats.AddSpecialEffect(SE3);
                    }
                }

                // Each time you use your [ability] ability, you have a chance to gain [amount] [stat] for [duration] sec
                regex = new Regex(@"Each time you use your (?<ability>\w+\s*\w*) ability, you have a chance to gain (?<amount>\d*) (?<stat>\w+[\s\w]*) for (?<duration>\d*) sec");
                match = regex.Match(line);
                if (match.Success)
                {
                    string statName = match.Groups["stat"].Value;
                    float amount = int.Parse(match.Groups["amount"].Value);
                    float duration = int.Parse(match.Groups["duration"].Value);
                    string ability = match.Groups["ability"].Value;

                    SpecialEffect SE1 = EvalRegex(statName, amount, duration, ability, 0f, 0.8f);
                    stats.AddSpecialEffect(SE1);
                }

                // Single Ability damage increase.
                regex = new Regex(@"Increases the damage dealt by your (?<ability>\w+\s*\w*) (ability )?by (?<amount>\d+[.]*\d*)");
                match = regex.Match(line);
                if (match.Success)
                {
                    string statName = match.Groups["stat"].Value;
                    float amount = float.Parse(match.Groups["amount"].Value, System.Globalization.CultureInfo.InvariantCulture);
                    string ability = match.Groups["ability"].Value;
                    //string ability2 = match.Groups["ability2"].Value;

                    EvalAbility(ability, stats, amount);
                }
                // 2 Abilities damage increase.
                regex = new Regex(@"Increases the damage (dealt |done )?by your (?<ability>\w+\s*\w*) and (?<ability2>\w+\s*\w*) (abilities )?by (?<amount>\d+)");
                match = regex.Match(line);
                if (match.Success)
                {
                    string statName = match.Groups["stat"].Value;
                    float amount = int.Parse(match.Groups["amount"].Value);
                    string ability = match.Groups["ability"].Value;
                    string ability2 = match.Groups["ability2"].Value;

                    EvalAbility(ability, stats, amount);
                    EvalAbility(ability2, stats, amount);
                }
                // 2 Abilities with separate damage increases.
                regex = new Regex(@"Increases the (base )?damage (dealt |done )?by your (?<ability>\w+\s*\w*) ability by (?<amount>\d+) and by your (?<ability2>\w+\s*\w*) ability by (?<amount2>\d+)");
                match = regex.Match(line);
                if (match.Success)
                {
                    string statName = match.Groups["stat"].Value;
                    float amount = int.Parse(match.Groups["amount"].Value);
                    float amount2 = int.Parse(match.Groups["amount2"].Value);
                    string ability = match.Groups["ability"].Value;
                    string ability2 = match.Groups["ability2"].Value;

                    EvalAbility(ability, stats, amount);
                    EvalAbility(ability2, stats, amount2);
                }
                // 3 Abilities damage increase.
                regex = new Regex(@"Increases the (base )?damage (dealt |done )?by your (?<ability>\w+\s*\w*) by (?<amount>\d+), your (?<ability2>\w+\s*\w*) by (?<amount2>\d+), and your (?<ability3>\w+\s*\w*) by (?<amount3>\d+)");
                match = regex.Match(line);
                if (match.Success)
                {
                    string statName = match.Groups["stat"].Value;
                    float amount = int.Parse(match.Groups["amount"].Value);
                    float amount2 = int.Parse(match.Groups["amount2"].Value);
                    float amount3 = int.Parse(match.Groups["amount3"].Value);
                    string ability = match.Groups["ability"].Value;
                    string ability2 = match.Groups["ability2"].Value;
                    string ability3 = match.Groups["ability3"].Value;

                    EvalAbility(ability, stats, amount);
                    EvalAbility(ability2, stats, amount2);
                    EvalAbility(ability3, stats, amount3);
                }
                #endregion Prototype Sigil Code
            }
        }

        public static void ProcessUseLine(string line, Stats stats, bool isArmory, int ilvl, int id) {
            Regex regex = new Regex(@"Increases (your )?(?<stat>\w\w*( \w\w*)*) by (?<amount>\+?\d+)(nbsp;\<small\>.*\<\/small\>)?(<a.*q2.*>) for (?<duration>\d+) sec \((?<cooldown>\d+) Min (?<cooldown2>\d+)?.*Cooldown\)");
            /*#region Prep the line, if it needs it
            while (line.Contains("secs")) { line = line.Replace("secs", "sec"); }
            while (line.Contains("sec.")) { line = line.Replace("sec.", "sec"); }
            while (line.Contains("  ")) { line = line.Replace("  ", " "); }
            while (line.EndsWith(".")) { line = line.Substring(0, line.Length-1); }
            #endregion*/
            Match match = regex.Match(line);
            if (match.Success)
            {
                Stats s = new Stats();
                string statName = match.Groups["stat"].Value;
                float amount = int.Parse(match.Groups["amount"].Value);
                float duration = int.Parse(match.Groups["duration"].Value);
                float cooldown = int.Parse(match.Groups["cooldown"].Value) * 60;
                cooldown += int.Parse(match.Groups["cooldown2"].Value);

                if (statName.Equals("attack power", StringComparison.InvariantCultureIgnoreCase)) { s.AttackPower = amount; }
                else if (statName.Equals("melee and ranged attack power", StringComparison.InvariantCultureIgnoreCase)) { s.AttackPower = amount; }
                else if (statName.Equals("haste rating", StringComparison.InvariantCultureIgnoreCase)) { s.HasteRating = amount; }
                else if (statName.Equals("spell power", StringComparison.InvariantCultureIgnoreCase)) { s.SpellPower = amount; }
                else if (statName.Equals("agility", StringComparison.InvariantCultureIgnoreCase)) { s.Agility = amount; }
                else if (statName.Equals("critical strike rating", StringComparison.InvariantCultureIgnoreCase)) { s.CritRating = amount; }
                else if (statName.Equals("dodge", StringComparison.InvariantCultureIgnoreCase)) { s.DodgeRating = amount; }
                else if (statName.Equals("dodge rating", StringComparison.InvariantCultureIgnoreCase)) { s.DodgeRating = amount; }
                else if (statName.Equals("parry", StringComparison.InvariantCultureIgnoreCase)) { s.ParryRating = amount; }
                else if (statName.Equals("parry rating", StringComparison.InvariantCultureIgnoreCase)) { s.ParryRating = amount; }
                else if (statName.Equals("spirit", StringComparison.InvariantCultureIgnoreCase)) { s.Spirit = amount; }
                else if (statName.Equals("maximum health", StringComparison.InvariantCultureIgnoreCase)) { s.Health = amount; }
                else if (statName.Equals("armor", StringComparison.InvariantCultureIgnoreCase)) { s.BonusArmor = amount; }
                else if (statName.Equals("mastery rating", StringComparison.InvariantCultureIgnoreCase)) { s.MasteryRating = amount; }
                
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, s, duration, cooldown));
            }
            // As of Patch 4.0, most basic On Use effects follow a the following rule for their cooldown: Duration of the proc * 6 [ie: 15 second duration has a 90 second Internal CD]
            #region Agility
            else if ((match = new Regex(@"Increases your Agility by (?<amount>\d+) for (?<dur>\d+) sec.*\ ((?<cd1>\d+) Min Cooldown)").Match(line.Replace("  ", " "))).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { Agility = int.Parse(match.Groups["amount"].Value), },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["cd1"].Value) * 60f));
            }
            // Figurine - Demon Panther
            else if ((match = new Regex(@"Increases your Agility by (?<amount>\d+) for (?<dur>\d+) sec").Match(line.Replace("  ", " "))).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { Agility = int.Parse(match.Groups["amount"].Value), },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 6f));
            }
            else if ((match = new Regex(@"Increases Agility by (?<amount>\d+) for (?<dur>\d+) sec").Match(line.Replace("  ", " "))).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { Agility = int.Parse(match.Groups["amount"].Value), },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 6f));
            }
            #endregion
            #region Armor
            // Figurine - Demon Panther
            else if ((match = new Regex(@"Increases your armor by (?<amount>\d+) for (?<dur>\d+) sec").Match(line.Replace("  ", " "))).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { BonusArmor = int.Parse(match.Groups["amount"].Value), },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 6f));
            }
            else if ((match = new Regex(@"Increases armor by (?<amount>\d+) for (?<dur>\d+) sec").Match(line.Replace("  ", " "))).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { BonusArmor = int.Parse(match.Groups["amount"].Value), },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 6f));
            }
            else if ((match = new Regex(@"Increases your bonus armor by (?<amount>\d+) for (?<dur>\d+) sec").Match(line.Replace("  ", " "))).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { BonusArmor = int.Parse(match.Groups["amount"].Value), },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 6f));
            }
            else if ((match = new Regex(@"Increases bonus armor by (?<amount>\d+) for (?<dur>\d+) sec").Match(line.Replace("  ", " "))).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { BonusArmor = int.Parse(match.Groups["amount"].Value), },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 6f));
            }
            // Fervor of the Frostborn / Eitrigg's Oath
            else if ((match = new Regex(@"Each time you are struck by an attack, you gain (?<bonusArmor>\d+) armor\.\s+Stacks up to (?<stacks>\d+) times\.\s+Entire effect lasts (?<duration>\d+) sec(\.\s+\((?<cooldown>\d+) Min Cooldown\))?").Match(line.Replace("  ", " "))).Success)
            {
                // Wowhead: "Each time you are struck by an attack, you gain 1265 armor. Stacks up to 5 times. Entire effect lasts 20 sec (2 Min Cooldown)"
                // Armory:  "Each time you are struck by an attack, you gain 1265 armor. Stacks up to 5 times. Entire effect lasts 20 sec."
                // armory doesn't give cooldown info
                int cooldown = 120;
                if (match.Groups["cooldown"].Success)
                {
                    cooldown = int.Parse(match.Groups["cooldown"].Value) * 60;
                }
                Stats childEffect = new Stats();
                childEffect.AddSpecialEffect(new SpecialEffect(Trigger.DamageTaken, new Stats() { BonusArmor = int.Parse(match.Groups["bonusArmor"].Value) }, float.PositiveInfinity, 0f, 1f, int.Parse(match.Groups["stacks"].Value)));
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, childEffect, int.Parse(match.Groups["duration"].Value), cooldown, 1f));
            }
            #endregion
            #region Attack Power
            else if ((match = new Regex(@"Increases your attack power by (?<amount>\d+) for (?<dur>\d+) sec").Match(line.Replace("  ", " "))).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { AttackPower = int.Parse(match.Groups["amount"].Value), },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 6f));
            }
            else if ((match = new Regex(@"Increases attack power by (?<amount>\d+) for (?<dur>\d+) sec").Match(line.Replace("  ", " "))).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { AttackPower = int.Parse(match.Groups["amount"].Value), },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 6f));
            }
            // Victor's Call and Vengeance of the Forsaken (232 & 245)
            else if ((match = new Regex(@"Each time you strike an enemy.*, you gain (?<amount>\d+) attack power. Stacks up to 5 times. Entire effect lasts 20 sec.*").Match(line.Replace("  ", " "))).Success)
            {
                SpecialEffect primary = new SpecialEffect(Trigger.Use, new Stats(), 20f, 2f * 60f);
                SpecialEffect secondary = new SpecialEffect(Trigger.MeleeHit, new Stats() { AttackPower = (float)int.Parse(match.Groups["amount"].Value) }, 20f, 0f, 1f, 5);
                primary.Stats.AddSpecialEffect(secondary);
                stats.AddSpecialEffect(primary);
            }
            #endregion
            #region Battlemaster Health
            // Battlemaster's Trinkets
            else if (line.Contains("Battlemaster\'s trinkets."))
            {
                Regex r = new Regex("Increases maximum health by (?<health>\\d*) for (?<duration>\\d*) sec");
                Match m = r.Match(line);
                if (m.Success)
                {
                    float health = float.Parse(m.Groups["health"].Value);
                    float duration = float.Parse(m.Groups["duration"].Value);
                    float cooldown = 3.0f * 60.0f; // Default to 3 minute cooldown, if not otherwise known

                    if (!isArmory)
                    {
                        Regex rCD = new Regex("\\((?<cooldown>\\d*) Min Cooldown\\)");
                        Match mCD = rCD.Match(line);
                        if (mCD.Success)
                            cooldown = int.Parse(mCD.Groups["cooldown"].Value) * 60.0f;
                    }

                    stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { BattlemasterHealthProc = health }, duration, cooldown));
                }
            }
            #endregion
            #region Bonus Healing Received
            // Talisman of Troll Divinity
            else if ((match = new Regex(@"For the next (?<maxduration>\d+) sec, your direct heals increase healing received by their target by up to (?<amount>\d+).  This effect lasts (?<stackduration>\d+) sec and stacks up to (?<stacks>\d+) times. ((?<cd1>\d+) Min Cooldown)").Match(line.Replace("  ", " "))).Success)
            {
                // For 20 seconds, direct healing adds a stack of 58 +healing for 10 seconds
                // Stacks 5 times, 2 minute cd
                // Direct heals: Nourish (1.5) HT (3) Regrowth (2)
                // Assumption: every 2 seconds, a direct healing spell is cast, so after 5 casts, full effect
                // That would mean: 10 seconds ramping up, then 20 seconds having the effect (assuming the stack is refreshed)
                // Average stack of 4 (24/30 * 5)
                // But remember that the spellpower will increase for others in the raid too!
                // stats.BonusHealingReceived = 58 * 4;
                Stats childEffect = new Stats();
                childEffect.AddSpecialEffect(new SpecialEffect(Trigger.HealingSpellHit, new Stats() { BonusHealingReceived = int.Parse(match.Groups["amount"].Value) }, int.Parse(match.Groups["stackduration"].Value), 0f, 1f, int.Parse(match.Groups["stacks"].Value)));
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, childEffect, int.Parse(match.Groups["maxduration"].Value), int.Parse(match.Groups["cd1"].Value) * 60f, 1f));
            }
            #endregion
            #region Critical Strike Rating
            else if ((match = new Regex(@"Increases your critical strike rating by (?<amount>\d+) for (?<dur>\d+) sec").Match(line.Replace("  ", " "))).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { CritRating = int.Parse(match.Groups["amount"].Value), },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 6f));
            }
            else if ((match = new Regex(@"Increases critical strike rating by (?<amount>\d+) for (?<dur>\d+) sec").Match(line.Replace("  ", " "))).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { CritRating = int.Parse(match.Groups["amount"].Value), },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 6f));
            }
            else if ((match = new Regex(@"Increases your crit rating by (?<amount>\d+) for (?<dur>\d+) sec").Match(line.Replace("  ", " "))).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { CritRating = int.Parse(match.Groups["amount"].Value), },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 6f));
            }
            else if ((match = new Regex(@"Increases crit rating by (?<amount>\d+) for (?<dur>\d+) sec").Match(line.Replace("  ", " "))).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { CritRating = int.Parse(match.Groups["amount"].Value), },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 6f));
            }
            // Nevermelting Ice Crystal - Yes when armory is fixed this needs update
            else if ((match = new Regex(@"Every time one of your non-periodic spells deals a critical strike, the bonus is reduced by 184 critical strike rating.").Match(line.Replace("  ", " "))).Success)
            {
                Stats childEffect = new Stats();
                childEffect.CritRating = 920f;
                childEffect.AddSpecialEffect(new SpecialEffect(Trigger.SpellCrit, new Stats() { CritRating = -184f }, float.PositiveInfinity, 0f, 1f, 5));
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, childEffect, 20f, 180f, 1f));
            }
            #endregion
            #region Damage Absorbed
            else if ((match = new Regex(@"Absorbs up to (?<amount>\d+) damage for (?<duration>\d+) sec, but once the absorb expires, you take (?<damageamount>\d+)% of the damage absorbed every (?<damagetick>\d+) sec for (?<damagelength>\d+) sec").Match(line)).Success)
            {
                // Stay Of Execution
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { DamageAbsorbed = int.Parse(match.Groups["amount"].Value) }, int.Parse(match.Groups["duration"].Value), 120f));
            }
            // Corroded Skeleton Key
            else if ((match = new Regex(@"Absorbs (?<amount>\d+) damage. Lasts (?<duration>\d+) sec.").Match(line)).Success)
            {
                // Absorbs 6400 damage.  Lasts 10 sec. (2 Min Cooldown)
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { DamageAbsorbed = int.Parse(match.Groups["amount"].Value) }, int.Parse(match.Groups["duration"].Value), 120f));
            }
            #endregion
            #region Dodge Rating
            // Figurine - Demon Panther
            else if ((match = new Regex(@"Increases your dodge rating by (?<amount>\d+) for (?<dur>\d+) sec").Match(line.Replace("  ", " "))).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { DodgeRating = int.Parse(match.Groups["amount"].Value), },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 6f));
            }
            else if ((match = new Regex(@"Increases dodge rating by (?<amount>\d+) for (?<dur>\d+) sec").Match(line.Replace("  ", " "))).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { DodgeRating = int.Parse(match.Groups["amount"].Value), },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 6f));
            }
            #endregion
            #region Elemental Damage
            // Gnomish Lightning Generator
            else if ((match = new Regex(@"Generates a bolt of lightning to strike an enemy for (?<min>\d+) to (?<max>\d+) Nature damage*").Match(line)).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { NatureDamage = (float)(int.Parse(match.Groups["min"].Value) + int.Parse(match.Groups["max"].Value)) / 2f, },
                    0f, 60f, 1f));
            }
            #endregion
            #region Expertise Rating
            else if ((match = new Regex(@"Increases your expertise rating by (?<amount>\d+) for (?<dur>\d+) sec").Match(line.Replace("  ", " "))).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { ExpertiseRating = int.Parse(match.Groups["amount"].Value), },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 6f));
            }
            else if ((match = new Regex(@"Increases expertise rating by (?<amount>\d+) for (?<dur>\d+) sec").Match(line.Replace("  ", " "))).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { ExpertiseRating = int.Parse(match.Groups["amount"].Value), },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 6f));
            }
            #endregion
            #region Healed
            // Living Ice Crystals
            else if ((match = new Regex(@"Instantly heal your current friendly target for (?<amount>\d+). \(?(?<cd1>\d+) Min Cooldown\)?").Match(line)).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { Healed = int.Parse(match.Groups["amount"].Value) }, 0f, int.Parse(match.Groups["cd1"].Value) * 60f));
            }
            // Glowing Twilight Scale proc (Eyes of Twilight); Procs on Healing Hit, NOT HoTs; Hits average of 5 people;
            else if ((match = new Regex(@"For the next (?<duration>\d+) sec, each time your direct healing spells heal a target you cause the target of your heal to heal themselves and friends within 10 yards for (?<amount>\d+) each sec for (?<seconds>\d+) sec").Match(line)).Success)
            {
                SpecialEffect primary = new SpecialEffect(Trigger.Use, new Stats(), int.Parse(match.Groups["duration"].Value), 2f * 60f);
                SpecialEffect secondary = new SpecialEffect(Trigger.HealingSpellCast, new Stats() { Healed = int.Parse(match.Groups["amount"].Value) * 5 }, int.Parse(match.Groups["seconds"].Value), 0f);
                primary.Stats.AddSpecialEffect(secondary);
                stats.AddSpecialEffect(primary);
            }
            #endregion
            #region Health Restore
            else if ((match = new Regex(@"Instantly relase all health stored on the Scales of Life as a self-heal").Match(line)).Success)
            {
                // Scales of Life
                float amount = 0;
                if (ilvl == 378) { amount = 6200; } else { amount = 7000; };
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { HealthRestore = amount, }, 0f, 2 * 60));
            }
            
            // Medallion of Heroism
            else if ((match = new Regex(@"Heal self for (?<amount1>\d\d+) to (?<amount2>\d\d+) damage.*").Match(line)).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { HealthRestore = (float)((int.Parse(match.Groups["amount1"].Value) + int.Parse(match.Groups["amount2"].Value)) / 2f) }, 0f, 2 * 60));
            }
            #endregion
            #region Haste Rating
            else if ((match = new Regex(@"Increases your haste rating by (?<amount>\d+) for (?<dur>\d+) sec").Match(line.Replace("  ", " "))).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { HasteRating = int.Parse(match.Groups["amount"].Value), },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 6f));
            }
            else if ((match = new Regex(@"Increases haste rating by (?<amount>\d+) for (?<dur>\d+) sec").Match(line.Replace("  ", " "))).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { HasteRating = int.Parse(match.Groups["amount"].Value), },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 6f));
            }
            // Fetish of Volatile Power
            else if ((match = new Regex(@"Each time you cast a harmful spell, you gain (?<amount>\d+) haste rating.? .*Stacks up to (?<stacks>\d+) times.? .*Entire effect lasts (?<duration>\d+) sec.?( \((?<cooldown>\d+) Min Cooldown\))?").Match(line.Replace("  ", " "))).Success)
            {
                // armory doesn't give cooldown info
                int cooldown = 120;
                if (match.Groups["cooldown"].Success)
                {
                    cooldown = int.Parse(match.Groups["cooldown"].Value) * 60;
                }
                Stats childEffect = new Stats();
                childEffect.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { HasteRating = int.Parse(match.Groups["amount"].Value) }, float.PositiveInfinity, 0f, 1f, int.Parse(match.Groups["stacks"].Value)));
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, childEffect, int.Parse(match.Groups["duration"].Value), cooldown, 1f));
            }
            // The Skull of Gul'dan
            else if ((match = new Regex(@"Tap into the power of the skull, increasing haste rating by (?<amount>\d+) for (?<dur>\d+) sec").Match(line.Replace("  ", " "))).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { HasteRating = int.Parse(match.Groups["amount"].Value) }, int.Parse(match.Groups["dur"].Value), 120.0f));
            }
            // Mind Quickening Gem
            else if ((match = new Regex(@"Quickens the mind, increasing the Mage's haste rating by (?<amount>\d+) for (?<dur>\d+) sec").Match(line.Replace("  ", " "))).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { HasteRating = int.Parse(match.Groups["amount"].Value) }, int.Parse(match.Groups["dur"].Value), 300));
            }
            // Ephemeral Snowflake
            else if ((match = new Regex(@"Grants (?<amount>\d+) haste rating for 20 sec").Match(line)).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { HasteRating = (float)int.Parse(match.Groups["amount"].Value) }, 20f, 120f));
            }
            // Heart of Ignacious
            else if ((match = new Regex(@"Consumes all applications of Heart's Revelation, increasing your haste rating by (?<amount>\d+) per application consumed. Lasts (?<dur>\d+) sec").Match(line.Replace("  ", " "))).Success)
            {
                /*int spamount = 77;
                if (id == 65110) { spamount = 87; }
                SpecialEffect primary = new SpecialEffect(Trigger.DamageSpellHit, new Stats(), 105f, 120f);
                SpecialEffect secondary = new SpecialEffect(Trigger.DamageSpellHit,
                    new Stats() { SpellPower = spamount }, 15f, 0f, 1f, 5);
                primary.Stats.AddSpecialEffect(secondary);
                stats.AddSpecialEffect(primary);
                */// Assume the user uses this with 5 stacks of Heart's Revelation
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { HasteRating = int.Parse(match.Groups["amount"].Value) * 5f, },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 6f));
            }
            
            #endregion
            #region Hit Rating
            else if ((match = new Regex(@"Increases your hit rating by (?<amount>\d+) for (?<dur>\d+) sec").Match(line.Replace("  ", " "))).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { HitRating = int.Parse(match.Groups["amount"].Value), },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 6f));
            }
            else if ((match = new Regex(@"Increases hit rating by (?<amount>\d+) for (?<dur>\d+) sec").Match(line.Replace("  ", " "))).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { HitRating = int.Parse(match.Groups["amount"].Value), },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 6f));
            }
            #endregion
            #region Intellect
            else if ((match = new Regex(@"Increases your Intellect by (?<amount>\d+) for (?<dur>\d+) sec.*\ ((?<cd1>\d+) Min (?<cd2>\d+) Sec Cooldown)").Match(line.Replace("  ", " "))).Success)
            {   // Mark of the Firelord
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { Intellect = int.Parse(match.Groups["amount"].Value), },
                    int.Parse(match.Groups["dur"].Value), (int.Parse(match.Groups["cd1"].Value) * 60f) + int.Parse(match.Groups["cd2"].Value)));
            }
            else if ((match = new Regex(@"Increases your Intellect by (?<amount>\d+) for (?<dur>\d+) sec.*\ ((?<cd1>\d+) Min Cooldown)").Match(line.Replace("  ", " "))).Success)
            {   // Mark of the Firelord
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { Intellect = int.Parse(match.Groups["amount"].Value), },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["cd1"].Value) * 60f));
            }
            else if ((match = new Regex(@"Increases your Intellect by (?<amount>\d+) for (?<dur>\d+) sec").Match(line.Replace("  ", " "))).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { Intellect = int.Parse(match.Groups["amount"].Value), },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 6f));
            }
            else if ((match = new Regex(@"Increases Intellect by (?<amount>\d+) for (?<dur>\d+) sec").Match(line.Replace("  ", " "))).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { Intellect = int.Parse(match.Groups["amount"].Value), },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 6f));
            }
            #endregion
            #region Mana Restore
            else if ((match = new Regex(@"When the shield is removed by any means, you regain (?<amount>\d+) mana").Match(line)).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { ManaRestore = int.Parse(match.Groups["amount"].Value) }, 0f, 5 * 60f));
            }
            // Figurine - Sapphire Owl
            else if ((match = new Regex(@"Restores (?<amount>\d+) mana over (?<duration>\d+) sec").Match(line)).Success)
            {
                //stats.ManaRestore5min = 2340;
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { ManaRestore = int.Parse(match.Groups["amount"].Value) / int.Parse(match.Groups["duration"].Value) }, int.Parse(match.Groups["duration"].Value), 300f));
            }
            // Sliver of Pure Ice
            else if ((match = new Regex(@"Restores (?<amount>\d+) mana.").Match(line)).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { ManaRestore = int.Parse(match.Groups["amount"].Value) }, 0f, 120f));
            }
            // Jar of Ancient Remedies
            else if ((match = new Regex(@"Grants (?<amount>\d+) mana, but consumes all applications of Inner Eye and prevents Inner Eye from being triggered for 30 sec").Match(line)).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { ManaRestore = (float) int.Parse(match.Groups["amount"].Value) }, 0f, 120f));
            }
            // Tyrande's Favorite Doll
            else if ((match = new Regex(@"Releases all mana stored within the doll, causing you to gain that much mana, and all enemies within 15 yards take 1 point of Arcane damage for each point of mana released").Match(line)).Success)
            {
                // Stores up to 4200 Mana
                // Assume at this point only one mob is hit by the arcane explosion
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { ManaRestore = 4200f, ArcaneDamage = 4200f  }, 0f, 60f));
            }
            #endregion
            #region Mastery Rating
            else if ((match = new Regex(@"Increases your mastery rating by (?<amount>\d+) for (?<dur>\d+) sec").Match(line.Replace("  ", " "))).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { MasteryRating = int.Parse(match.Groups["amount"].Value), },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 6f));
            }
            else if ((match = new Regex(@"Increases mastery rating by (?<amount>\d+) for (?<dur>\d+) sec").Match(line.Replace("  ", " "))).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { MasteryRating = int.Parse(match.Groups["amount"].Value), },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 6f));
            }
            #endregion
            #region Movement Speed
            // Figurine - Ruby Hare : "Increases speed by 30% for 6 sec. (3 Min Cooldown)"
            else if (line.StartsWith("Increases speed by "))
            {
                Regex r = new Regex("Increases speed by (?<speed>\\d*)% for (?<dur>\\d*) sec"); // \\((?<cd>\\d*) Min Cooldown\\)");
                Match m = r.Match(line);
                if (m.Success)
                {
                    int speed = int.Parse(m.Groups["speed"].Value);
                    int dur = int.Parse(m.Groups["dur"].Value);
                    // Test again with the cd... Available only on wowhead.
                    float cd;
                    Regex rCD = new Regex("Increases speed by (?<speed>\\d*)% for (?<dur>\\d*) sec \\((?<cd>\\d*) Min Cooldown\\)");
                    Match mCD = rCD.Match(line);
                    if (mCD.Success)
                    {
                        cd = int.Parse(mCD.Groups["cd"].Value) * 60.0f;
                    }
                    else
                    {
                        cd = 3.0f * 60.0f;
                    }
                    stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { MovementSpeed = speed / 100f }, dur, cd));
                }
            }
            #endregion
            #region Mp5
            else if ((match = new Regex(@"Gain (?<amount>\d+) mana each sec for ").Match(line)).Success)
            {
                //stats.ManaregenFor8SecOnUse5Min += 250;
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { Mp5 = int.Parse(match.Groups["amount"].Value) * 5 }, 8.0f, 300.0f));
            }
            // Meteorite Crystal and Pendant of the Violet Eye
            else if ((match = new Regex(@"Each spell cast within 20 seconds will grant a stacking bonus of (?<mp5>\d+) mana regen per 5 sec.? Expires after (?<duration>\d+) seconds.*").Match(line)).Success)
            {
                // Estimate cast as every 2.0f seconds, average stack height is 0.5f of final value
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { Mp5 = int.Parse(match.Groups["mp5"].Value) * int.Parse(match.Groups["duration"].Value) * 0.5f / 2.0f }, int.Parse(match.Groups["duration"].Value), 120));
            }
            #endregion
            #region Parry Rating
            else if ((match = new Regex(@"Increases your parry rating by (?<amount>\d+) for (?<dur>\d+) sec").Match(line.Replace("  ", " "))).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { ParryRating = int.Parse(match.Groups["amount"].Value), },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 6f));
            }
            else if ((match = new Regex(@"Increases parry rating by (?<amount>\d+) for (?<dur>\d+) sec").Match(line.Replace("  ", " "))).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { ParryRating = int.Parse(match.Groups["amount"].Value), },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 6f));
            }
            #endregion
            #region PvP Trinket
            else if ((match = new Regex(@"Removes all movement impairing effects and all effects which cause loss of control of your character.").Match(line)).Success)
            {
                stats.PVPTrinket += 1f;
            }
            #endregion
            #region Resilience Rating
            else if ((match = new Regex(@"Increases your resilience by (?<amount>\d+) for (?<dur>\d+) sec.*\((?<cd1>\d+) Min Cooldown\)?").Match(line.Replace("  ", " "))).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { Resilience = int.Parse(match.Groups["amount"].Value), },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 6f));
            }
            else if ((match = new Regex(@"Increases resilience by (?<amount>\d+) for (?<dur>\d+) sec.*\((?<cd1>\d+) Min Cooldown\)?").Match(line.Replace("  ", " "))).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { Resilience = int.Parse(match.Groups["amount"].Value), },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 6f));
            }
            #endregion
            #region Resistance
            // Sindragosa's Flawless Fang
            else if ((match = new Regex(@"Increases resistance to Arcane, Fire, Frost, Nature, and Shadow spells by (?<amount>\d+) for (?<duration>\d+) sec.").Match(line)).Success)
            {
                // Increases resistance to Arcane, Fire, Frost, Nature, and Shadow spells by 239 for 10 sec. (1 Min Cooldown)
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats()
                    {
                        ArcaneResistance = int.Parse(match.Groups["amount"].Value),
                        FireResistance = int.Parse(match.Groups["amount"].Value),
                        FrostResistance = int.Parse(match.Groups["amount"].Value),
                        NatureResistance = int.Parse(match.Groups["amount"].Value),
                        ShadowResistance = int.Parse(match.Groups["amount"].Value),
                    },
                    int.Parse(match.Groups["duration"].Value), 60f));
            }
            // Mirror of Broken Images
            else if ((match = new Regex(@"Increases Arcane, Fire, Frost, Nature, and Shadow resistances by (?<amount>\d+) for (?<duration>\d+) sec.").Match(line)).Success)
            {
                // Increases Arcane, Fire, Frost, Nature, and Shadow resistances by 400 for 10 sec. (1 Min Cooldown)
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats()
                    {
                        ArcaneResistance = int.Parse(match.Groups["amount"].Value),
                        FireResistance = int.Parse(match.Groups["amount"].Value),
                        FrostResistance = int.Parse(match.Groups["amount"].Value),
                        NatureResistance = int.Parse(match.Groups["amount"].Value),
                        ShadowResistance = int.Parse(match.Groups["amount"].Value),
                    },
                    int.Parse(match.Groups["duration"].Value), 60f));
            }
            #endregion
            #region Spirit
            else if ((match = new Regex(@"Increases your Spirit by (?<amount>\d+) for (?<dur>\d+) sec").Match(line.Replace("  ", " "))).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { Spirit = int.Parse(match.Groups["amount"].Value), },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 6f));
            }
            else if ((match = new Regex(@"Increases Spirit by (?<amount>\d+) for (?<dur>\d+) sec").Match(line.Replace("  ", " "))).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { Spirit = int.Parse(match.Groups["amount"].Value), },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 6f));
            }
            #endregion
            #region Strength
            else if ((match = new Regex(@"Increases your Strength by (?<amount>\d+) for (?<dur>\d+) sec.*\ ((?<cd1>\d+) Min Cooldown)").Match(line.Replace("  ", " "))).Success)
            {   // Essence of the Eternal Flame
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { Strength = int.Parse(match.Groups["amount"].Value), },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["cd1"].Value) * 60f));
            }
            // Figurine - King of Boars; Magnetite Mirror
            else if ((match = new Regex(@"Increases your Strength by (?<amount>\d+) for (?<dur>\d+) sec").Match(line.Replace("  ", " "))).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { Strength = int.Parse(match.Groups["amount"].Value), },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 6f));
            }
            else if ((match = new Regex(@"Increases Strength by (?<amount>\d+) for (?<dur>\d+) sec").Match(line.Replace("  ", " "))).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { Strength = int.Parse(match.Groups["amount"].Value), },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 6f));
            }
            // Fury of Angerforge
            else if ((match = new Regex(@"Consume 5 stacks of Raw Fury to forge into the form of a Blackwing Dragonkin, granting (?<amount>\d+) Strength for (?<dur>\d+) sec").Match(line.Replace("  ", " "))).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { Strength = int.Parse(match.Groups["amount"].Value), },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 6f));
            }
            #endregion
            #region Spell Mana Reduction
            else if ((match = new Regex(@"Your next (?<casts>\d+) spells cast within (?<dur>\d+) sec will reduce the cost of your holy and nature spells by (?<amount>\d+), stacking up to (?<stacks>\d+) times").Match(line)).Success)
            {   // Jaws of Defeat
                SpecialEffect primary = new SpecialEffect(Trigger.Use, new Stats(), int.Parse(match.Groups["dur"].Value), 120f);
                SpecialEffect secondary = new SpecialEffect(Trigger.SpellCast,
                    new Stats() { NatureSpellsManaCostReduction = int.Parse(match.Groups["amount"].Value), HolySpellsManaCostReduction = int.Parse(match.Groups["amount"].Value), },
                    int.Parse(match.Groups["dur"].Value), 0f, 1f, int.Parse(match.Groups["stacks"].Value));
                primary.Stats.AddSpecialEffect(secondary);
                stats.AddSpecialEffect(primary);
            }
            // Lower City Prayerbook
            else if ((match = new Regex(@"Your heals each cost (?<amount>\d+) less mana for the next (?<duration>\d+) sec. ((?<cd1>\d+) Min Cooldown)").Match(line)).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { SpellsManaCostReduction = int.Parse(match.Groups["amount"].Value) }, int.Parse(match.Groups["duration"].Value), int.Parse(match.Groups["cd1"].Value) * 60f));
            }
            #endregion
            #region Spell Power
            else if ((match = new Regex(@"Increases your spell power by (?<amount>\d+) for (?<dur>\d+) sec").Match(line.Replace("  ", " "))).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { SpellPower = int.Parse(match.Groups["amount"].Value), },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 6f));
            }
            else if ((match = new Regex(@"Increases spell power by (?<amount>\d+) for (?<dur>\d+) sec").Match(line.Replace("  ", " "))).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { SpellPower = int.Parse(match.Groups["amount"].Value), },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 6f));
            }
            // BC: Eye of the Night
            else if ((match = new Regex(@"Increases spell power by (?<dur>\d+) for all nearby party members.").Match(line.Replace("  ", " "))).Success)
            {
                // the full text reads: "Use: Increases spell power by 34 for all nearby party members.  Lasts 30 min. (1 Hour Cooldown)"
                // So in general you get the 34 spell power for 100% of a given boss fight.  Like most trinkets it is up to the player to
                // use it correctly, so we are going to give them the full power instead of half.
                stats.SpellPower += int.Parse(match.Groups["dur"].Value);
            }
            // Shifting Naaru Sliver
            else if ((match = new Regex(@"Conjures a Power Circle lasting for (?<dur>\d+) sec.  While standing in this circle, the caster gains (?<amount>\d+) spell power.").Match(line.Replace("  ", " "))).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { SpellPower = int.Parse(match.Groups["amount"].Value) }, int.Parse(match.Groups["dur"].Value), 90.0f));
            }
            // Binding Light / Stone
            else if ((match = new Regex(@"Each time you cast a helpful spell, you gain (?<amount>\d+) spell power. *Stacks up to (?<stacks>\d+) times. *Entire effect lasts (?<duration>\d+) sec.*").Match(line.Replace("  ", " "))).Success)
            {
                SpecialEffect primary = new SpecialEffect(Trigger.Use, new Stats(),
                    (float)int.Parse(match.Groups["duration"].Value), 2f * 60f);
                SpecialEffect secondary = new SpecialEffect(Trigger.HealingSpellCast,
                    new Stats() { SpellPower = (float)int.Parse(match.Groups["amount"].Value) },
                    (float)int.Parse(match.Groups["duration"].Value),
                    0f, 1f, int.Parse(match.Groups["stacks"].Value));
                primary.Stats.AddSpecialEffect(secondary);
                stats.AddSpecialEffect(primary);
            }
            // World-Queller Focus
            else if ((match = new Regex(@"Your next 5 spells cast within 20 sec will grant a bonus of (?<amount>\d+) spell power, stacking up to (?<stacks>\d+) times.*\ Lasts (?<duration>\d+) sec").Match(line.Replace("  ", " "))).Success)
            {
                SpecialEffect primary = new SpecialEffect(Trigger.Use, new Stats(),
                    (float)int.Parse(match.Groups["duration"].Value), 2f * 60f);
                SpecialEffect secondary = new SpecialEffect(Trigger.SpellCast,
                    new Stats() { SpellPower = (float)int.Parse(match.Groups["amount"].Value) },
                    (float)int.Parse(match.Groups["duration"].Value),
                    0f, 1f, int.Parse(match.Groups["stacks"].Value));
                primary.Stats.AddSpecialEffect(secondary);
                stats.AddSpecialEffect(primary);
            }
            
            #endregion
            #region Spell Penetration
            else if ((match = new Regex(@"Increases your spell penetration by (?<amount>\d+) for (?<dur>\d+) sec").Match(line.Replace("  ", " "))).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { SpellPenetration = int.Parse(match.Groups["amount"].Value), },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 6f));
            }
            else if ((match = new Regex(@"Increases spell penetration by (?<amount>\d+) for (?<dur>\d+) sec").Match(line.Replace("  ", " "))).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { SpellPenetration = int.Parse(match.Groups["amount"].Value), },
                    int.Parse(match.Groups["dur"].Value), int.Parse(match.Groups["dur"].Value) * 6f));
            }
            #endregion
            else if ((match = new Regex(@"Consume all Titanic Power to increase your critical strike rating, haste rating, or mastery rating by (?<amount>\d+) per Titanic Power accumulated").Match(line.Replace("  ", " "))).Success)
            {   // Apparatus of Khaz'goroth
                // TODO: Get duration length - assume 20 seconds
                // Stacks up to 5 times
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { HighestSecondaryStat = int.Parse(match.Groups["amount"].Value) * 5f, },
                    20f, 20f * 6f));
            }

            else if ((match = new Regex(@"Increases the damage dealt by your Scourge Strike and Obliterate abilities by 420").Match(line)).Success)
            {
                stats.BonusDamageObliterate += 420;
                stats.BonusDamageScourgeStrike += 420;
            }
            // Corrupted Egg Shell
            else if ((match = new Regex(@"Places Egg Shell on your current target, absorbing (?<absorb>\d+) damage.*\ While Egg Shell persists, you will gain (?<mp5>\d+) mana every 5 sec.*\ When the effect is cancelled, you gain (?<manarestore>\d+) mana.*\ Lasts (?<duration>\d+) sec").Match(line)).Success)
            {
                // Assume that the shield will be placed on a player who will not be taking any damage for the duration of the proc.
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { DamageAbsorbed = int.Parse(match.Groups["absorb"].Value) },
                    0f, 120));
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { Mp5 = int.Parse(match.Groups["mp5"].Value) },
                    int.Parse(match.Groups["duration"].Value), 120));
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                    new Stats() { ManaRestore = int.Parse(match.Groups["manarestore"].Value) },
                    0f, 120));
            }
        }

        #region Specialized Items that need Extra Handling
        #region DeathBringer's Will (264 & 277)
        /// <summary>
        /// Special Handler for Deathbringer's Will to be call from INSIDE the model.
        /// </summary>
        /// <param name="Class">The class of the character</param>
        /// <param name="value">The current stats.DeathBringerProc value</param>
        /// <returns>List of Special Effects relevant to your class. Will be a list of 3 items or 0 if passing an invalid class.</returns>
        /*public static List<SpecialEffect> GetDeathBringerEffects(CharacterClass Class, float value) {
            List<SpecialEffect> retVal = new List<SpecialEffect>();
            float dur = 30f, cd = 105 * 3, ch = 0.15f;

            SpecialEffect procSTR   = new SpecialEffect(Trigger.PhysicalHit, new Stats { Strength               = value }, dur, cd, ch);
            SpecialEffect procCrit  = new SpecialEffect(Trigger.PhysicalHit, new Stats { CritRating             = value }, dur, cd, ch);
            SpecialEffect procHaste = new SpecialEffect(Trigger.PhysicalHit, new Stats { HasteRating            = value }, dur, cd, ch);
            SpecialEffect procAGI   = new SpecialEffect(Trigger.PhysicalHit, new Stats { Agility                = value }, dur, cd, ch);
            SpecialEffect procAP    = new SpecialEffect(Trigger.PhysicalHit, new Stats { AttackPower            = value * 2 }, dur, cd, ch);

            switch (Class) {
                case CharacterClass.Warrior:     retVal.Add(procSTR);  retVal.Add(procCrit);  break;
                case CharacterClass.Rogue:       retVal.Add(procAGI);   retVal.Add(procAP);    break;
                case CharacterClass.Paladin:     retVal.Add(procSTR); retVal.Add(procHaste); retVal.Add(procCrit);  break;
                case CharacterClass.Hunter:      retVal.Add(procAP);  retVal.Add(procAGI);   retVal.Add(procCrit);  break;
                case CharacterClass.DeathKnight: retVal.Add(procSTR); retVal.Add(procCrit);  retVal.Add(procHaste); break;
                case CharacterClass.Druid:       retVal.Add(procSTR);   retVal.Add(procAGI);   break;
                case CharacterClass.Shaman:      retVal.Add(procAGI); retVal.Add(procAP);    break;
                default: break; // None
            }

            return retVal;
        }*/
        #endregion
        #endregion

        #region EvalRegex
        public static SpecialEffect EvalRegex(string statName, float amount, float duration, string ability, float cooldown) { return EvalRegex(statName, amount, duration, ability, cooldown, 1f); }

        /// <summary>
        /// For those objects that have a special effect trigger (As opposed to just straight stat upgrades)
        /// This allows you to pass in a statName, amount, duration, trigger type, and cooldown and get a SpecialEffect back.
        /// </summary>
        /// <param name="statName">What stat does the special effect target</param>
        /// <param name="amount">By what amount?</param>
        /// <param name="duration">How long in seconds?</param>
        /// <param name="ability">What is the spell triggering? For right now, this only effects the Sigils.  So we're going to use the ability to setup the trigger.</param>
        /// <param name="cooldown">How long in seconds?</param>
        /// <param name="chance">What is the percent chance? (0-1)</param>
        /// <returns>A new SpecialEffect instance that can be used in Stats.AddSpecialEffect()</returns>
        public static SpecialEffect EvalRegex(string statName, float amount, float duration, string ability, float cooldown, float chance)
        {
            Stats s = new Stats();

            if (statName.Equals("attack power", StringComparison.InvariantCultureIgnoreCase)) { s.AttackPower = amount; }
            else if (statName.Equals("agility", StringComparison.InvariantCultureIgnoreCase)) { s.Agility = amount; }
            else if (statName.Equals("armor", StringComparison.InvariantCultureIgnoreCase)) { s.BonusArmor = amount; }
            else if (statName.Equals("critical strike rating", StringComparison.InvariantCultureIgnoreCase)) { s.CritRating = amount; }
            else if (statName.Equals("dodge", StringComparison.InvariantCultureIgnoreCase)) { s.DodgeRating = amount; }
            else if (statName.Equals("dodge rating", StringComparison.InvariantCultureIgnoreCase)) { s.DodgeRating = amount; }
            else if (statName.Equals("melee and ranged attack power", StringComparison.InvariantCultureIgnoreCase)) { s.AttackPower = amount; }
            else if (statName.Equals("haste rating", StringComparison.InvariantCultureIgnoreCase)) { s.HasteRating = amount; }
            else if (statName.Equals("maximum health", StringComparison.InvariantCultureIgnoreCase)) { s.Health = amount; }
            else if (statName.Equals("parry", StringComparison.InvariantCultureIgnoreCase)) { s.ParryRating = amount; }
            else if (statName.Equals("parry rating", StringComparison.InvariantCultureIgnoreCase)) { s.ParryRating = amount; }
            else if (statName.Equals("spell power", StringComparison.InvariantCultureIgnoreCase)) { s.SpellPower = amount; }
            else if (statName.Equals("spirit", StringComparison.InvariantCultureIgnoreCase)) { s.Spirit = amount; }
            else if (statName.Equals("strength", StringComparison.InvariantCultureIgnoreCase)) { s.Strength = amount; }
            Trigger trigger = new Trigger();

            switch (ability)
            {
                case "Icy Touch":
                    trigger = Trigger.IcyTouchHit;
                    break;
                case "Plague Strike":
                    trigger = Trigger.PlagueStrikeHit;
                    break;
                case "Rune Strike":
                    trigger = Trigger.RuneStrikeHit;
                    break;
                case "Blood Strike":
                    trigger = Trigger.BloodStrikeHit;
                    break;
                case "Heart Strike":
                case "Heart Strikes":
                    trigger = Trigger.HeartStrikeHit;
                    break;
                case "Obliterate":
                    trigger = Trigger.ObliterateHit;
                    break;
                case "Scourge Strike":
                    trigger = Trigger.ScourgeStrikeHit;
                    break;
                case "Death Strike":
                    trigger = Trigger.DeathStrikeHit;
                    break;
                default:
                    trigger = Trigger.SpellHit;
                    break;
            }

            return new SpecialEffect(trigger, s, duration, cooldown, chance);

        }

        /// <summary>
        /// For those objects that have a special effect trigger (As opposed to just straight stat upgrades)
        /// This allows you to pass in a statName, amount, duration, trigger type, and cooldown and get a SpecialEffect back.
        /// </summary>
        /// <param name="statName">What stat does the special effect target</param>
        /// <param name="amount">By what amount?</param>
        /// <param name="duration">How long in seconds?</param>
        /// <param name="ability">What is the spell triggering? For right now, this only effects the Sigils.  So we're going to use the ability to setup the trigger.</param>
        /// <param name="cooldown">How long in seconds?</param>
        /// <param name="chance">What is the percent chance? (0-1)</param>
        /// <param name="stacks"> How many stacks?</param>
        /// <returns>A new SpecialEffect instance that can be used in Stats.AddSpecialEffect()</returns>
        public static SpecialEffect EvalRegex(string statName, float amount, float duration, string ability, float cooldown, float chance, int stacks)
        {
            Stats s = new Stats();

            if (statName.Equals("attack power", StringComparison.InvariantCultureIgnoreCase)) { s.AttackPower = amount; }
            else if (statName.Equals("agility", StringComparison.InvariantCultureIgnoreCase)) { s.Agility = amount; }
            else if (statName.Equals("armor", StringComparison.InvariantCultureIgnoreCase)) { s.BonusArmor = amount; }
            else if (statName.Equals("critical strike rating", StringComparison.InvariantCultureIgnoreCase)) { s.CritRating = amount; }
            else if (statName.Equals("dodge", StringComparison.InvariantCultureIgnoreCase)) { s.DodgeRating = amount; }
            else if (statName.Equals("dodge rating", StringComparison.InvariantCultureIgnoreCase)) { s.DodgeRating = amount; }
            else if (statName.Equals("melee and ranged attack power", StringComparison.InvariantCultureIgnoreCase)) { s.AttackPower = amount; }
            else if (statName.Equals("haste rating", StringComparison.InvariantCultureIgnoreCase)) { s.HasteRating = amount; }
            else if (statName.Equals("maximum health", StringComparison.InvariantCultureIgnoreCase)) { s.Health = amount; }
            else if (statName.Equals("parry", StringComparison.InvariantCultureIgnoreCase)) { s.ParryRating = amount; }
            else if (statName.Equals("parry rating", StringComparison.InvariantCultureIgnoreCase)) { s.ParryRating = amount; }
            else if (statName.Equals("spell power", StringComparison.InvariantCultureIgnoreCase)) { s.SpellPower = amount; }
            else if (statName.Equals("spirit", StringComparison.InvariantCultureIgnoreCase)) { s.Spirit = amount; }
            else if (statName.Equals("strength", StringComparison.InvariantCultureIgnoreCase)) { s.Strength = amount; }
            Trigger trigger = new Trigger();

            switch (ability)
            {
                case "Icy Touch":
                    trigger = Trigger.IcyTouchHit;
                    break;
                case "Plague Strike":
                    trigger = Trigger.PlagueStrikeHit;
                    break;
                case "Rune Strike":
                    trigger = Trigger.RuneStrikeHit;
                    break;
                case "Blood Strike":
                    trigger = Trigger.BloodStrikeHit;
                    break;
                case "Heart Strike":
                case "Heart Strikes":
                    trigger = Trigger.HeartStrikeHit;
                    break;
                case "Obliterate":
                    trigger = Trigger.ObliterateHit;
                    break;
                case "Scourge Strike":
                    trigger = Trigger.ScourgeStrikeHit;
                    break;
                case "Death Strike":
                    trigger = Trigger.DeathStrikeHit;
                    break;
                default:
                    trigger = Trigger.SpellHit;
                    break;
            }

            return new SpecialEffect(trigger, s, duration, cooldown, chance, stacks);

        }

        /// <summary>
        /// Using the Regex functions above, update the passed in stat value of a given statName by the amount appropriate.
        /// </summary>
        /// <param name="s">the Stats instance to be updated.</param>
        /// <param name="statName">The stat to update.</param>
        /// <param name="amount">The amount to update the stat by.</param>
        public static void EvalRegex(Stats s, string statName, float amount) {
            if (statName.Equals("attack power", StringComparison.InvariantCultureIgnoreCase)) { s.AttackPower = amount; }
            else if (statName.Equals("agility", StringComparison.InvariantCultureIgnoreCase)) { s.Agility = amount; }
            else if (statName.Equals("armor", StringComparison.InvariantCultureIgnoreCase)) { s.BonusArmor = amount; }
            else if (statName.Equals("critical strike rating", StringComparison.InvariantCultureIgnoreCase)) { s.CritRating = amount; }
            else if (statName.Equals("dodge", StringComparison.InvariantCultureIgnoreCase)) { s.DodgeRating = amount; }
            else if (statName.Equals("dodge rating", StringComparison.InvariantCultureIgnoreCase)) { s.DodgeRating = amount; }
            else if (statName.Equals("melee and ranged attack power", StringComparison.InvariantCultureIgnoreCase)) { s.AttackPower = amount; }
            else if (statName.Equals("haste rating", StringComparison.InvariantCultureIgnoreCase)) { s.HasteRating = amount; }
            else if (statName.Equals("maximum health", StringComparison.InvariantCultureIgnoreCase)) { s.Health = amount; }
            else if (statName.Equals("parry", StringComparison.InvariantCultureIgnoreCase)) { s.ParryRating = amount; }
            else if (statName.Equals("parry rating", StringComparison.InvariantCultureIgnoreCase)) { s.ParryRating = amount; }
            else if (statName.Equals("spell power", StringComparison.InvariantCultureIgnoreCase)) { s.SpellPower = amount; }
            else if (statName.Equals("spirit", StringComparison.InvariantCultureIgnoreCase)) { s.Spirit = amount; }
        }

        /// <summary>
        /// Eval the Bonus damage values of given sigils and assign the bonus damage.
        /// This is specifically for DKs, but could be expanded as necessary.
        /// Hell, it probably could use a re-write once the whole Stats class is restructured.
        /// </summary>
        /// <param name="ability">What ability is having it's damage boosted?</param>
        /// <param name="s">The Stats instance to be updated.</param>
        /// <param name="amount">The amount by which the bonus should be applied.</param>
        public static void EvalAbility(string ability, Stats s, float amount) {
            switch (ability) {
                case "Blood Strike"  : { s.BonusDamageBloodStrike   += amount; break; }
                case "Heart Strike"  : { s.BonusDamageHeartStrike   += amount; break; }
                case "Death Coil"    : { s.BonusDamageDeathCoil     += amount; break; }
                case "Frost Strike"  : { s.BonusDamageFrostStrike   += amount; break; }
                case "Obliterate"    : { s.BonusDamageObliterate    += amount; break; }
                case "Scourge Strike": { s.BonusDamageScourgeStrike += amount; break; }
                case "Death Strike"  : { s.BonusDamageDeathStrike   += amount; break; }
                case "Icy Touch"     : { s.BonusDamageIcyTouch      += amount; break; }
                default: { break; } // Error.
            }
        }
        #endregion
    }
}
