using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Rawr {
	public static class SpecialEffects {
        public static void ProcessMetaGem(string line, Stats stats, bool bisArmory) {
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
                    stats.ManaRestoreOnCast_5_15 = 600; // IED
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
                    stats.BonusBlockValueMultiplier = 0.05f;
                }
                else if (gemBonus == "+2% Intellect")
                {
                    stats.BonusIntellectMultiplier = 0.02f;
                }
                else if (gemBonus == "+2% Mana")
                {
                    stats.BonusManaMultiplier = 0.02f;
				}
                else if (gemBonus.Contains("Reduce Spell Damage Taken by "))
                {
                    int bonus = int.Parse(gemBonus.Substring(gemBonus.Length - 3, 2));
                    stats.SpellDamageTakenMultiplier = (float)bonus / -100f;
                }
                else if (gemBonus == "3% Increased Critical Damage")
				{
					stats.BonusCritMultiplier = 0.03f;
					stats.BonusSpellCritMultiplier = 0.03f;
				}
                else if (gemBonus == "3% Increased Critical Healing Effect")
                {
                    stats.BonusCritHealMultiplier = 0.03f;
                }
                else if (gemBonus == "Minor Run Speed Increase")
                {
                    stats.MovementSpeed = 0.08f;
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
                    stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalCrit, new Stats() { Health = 400f }, 0f, 0f, 0.50f));
                    stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCrit, new Stats() { Health = 400f }, 0f, 0f, 0.50f));
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
                                stats.BonusCritMultiplier = (float)gemBonusValue / 100f;
                                stats.BonusSpellCritMultiplier = (float)gemBonusValue / 100f; // both melee and spell crit use the same text, would have to disambiguate based on id
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
                            case "Defense Rating":
                                stats.DefenseRating = gemBonusValue;
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
                            case "Armor Penetration Rating":
                                stats.ArmorPenetrationRating = gemBonusValue;
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

		public static void ProcessEquipLine(string line, Stats stats, bool isArmory, int ilvl) {
            Match match;
			if (line.StartsWith("Increases initial and per application periodic damage done by Lacerate by "))
			{
				//stats.BonusLacerateDamage = float.Parse(line.Substring("Increases initial and per application periodic damage done by Lacerate by ".Length));
			}
            else if ((match = new Regex(@"Chance on melee or ranged critical strike to increase your armor penetration rating by 678 for 10 sec(s?).").Match(line)).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalCrit, new Stats() { ArmorPenetrationRating = 678f }, 10f, 45, 0.1f));
            }
			else if ((match = new Regex(@"Chance on melee or ranged critical strike to increase your armor penetration rating by (?<amount>\d\d*) for 10 sec(s?).").Match(line)).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalCrit, new Stats() { ArmorPenetrationRating = (float)int.Parse(match.Groups["amount"].Value) }, 10f, 45, 0.15f));
            }
			else if ((match = new Regex(@"Chance on melee and ranged critical strike to increase your armor penetration rating by (?<amount>\d\d*) for 10 sec(s?).").Match(line)).Success)
			{
				stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalCrit, new Stats() { ArmorPenetrationRating = (float)int.Parse(match.Groups["amount"].Value) }, 10f, 45, 0.15f));
			}
			else if (line.StartsWith("Your harmful spells have a chance to increase your haste rating by 522 for 10 sec."))
            {
                // Elemental Focus Stone
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageSpellCast, new Stats() { HasteRating = 522 }, 10f, 45f, 0.1f));
            }
			else if (line.StartsWith("When struck in combat has a chance of"))
			{ // Essence of Gossamer
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageTaken, new Stats() { DamageTakenMultiplier = -0.01f }, 10, 1, 0.05f));
			}
            else if (line.StartsWith("Your melee and ranged attacks have a chance to call on the power"))
			{ //Shattered Sun Pendant of Might
				stats.ShatteredSunMightProc += 1f;
			}
			else if (line.StartsWith("Your spells have a chance to call on the power"))
			{ //Shattered Sun Pendant of Acumen
				stats.ShatteredSunAcumenProc += 1f;
			}
			else if (line.StartsWith("Your heals have a chance to call on the power"))
			{ //Shattered Sun Pendant of Restoration
				stats.ShatteredSunRestoProc += 1f;
			}
			else if (line.StartsWith("Protected from the cold.  Your Frost resistance is increased by 20."))
			{ //Mechanized Snow Goggles of the...
				stats.FrostResistance = 20;
			}
			else if (line.StartsWith("Chance on hit to increase your attack power by 230"))
			{ //Special handling for Shard of Contempt due to higher uptime
				// stats.AttackPower += 90f;
                stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalHit, new Stats() { AttackPower = 230f }, 20f, 45f, .1f));
			}
			else if (line.StartsWith("When you heal or deal damage you have a chance to gain Greatness"))
			{ //Darkmoon Card: Greatness
                stats.AddSpecialEffect(new SpecialEffect(Trigger.HealingSpellHit, new Stats() { HighestStat = 300f }, 15f, 45f, .33f));
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageDone, new Stats() { HighestStat = 300f }, 15f, 45f, .33f));
			}
            else if (line.StartsWith("Each time you deal melee or ranged damage to an opponent, you gain 16 attack power for the next 10 sec., stacking up to 20 times."))
            {
                // Fury of the Five Flights
                stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalHit, new Stats() { AttackPower = 16f }, 10f, 0f, 1f, 20));
            }
            else if ((match = Regex.Match(line, @"Each time you deal melee or ranged damage to an opponent, you gain (?<amount>\d+) attack power for the next 10 sec\., stacking up to 20 times\.")).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalHit, new Stats() { AttackPower = int.Parse(match.Groups["amount"].Value) }, 10f, 0f, 1f, 20));
            }
            else if ((match = Regex.Match(line, @"Each time you cast a damaging or healing spell you gain (?<spellPower>\d+) spell power for the next (?<duration>\d+) sec, stacking up to (?<maxStack>\d+) times.")).Success)
            {
                // Illustration of the Dragon Soul and similar
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { SpellPower = int.Parse(match.Groups["spellPower"].Value) }, int.Parse(match.Groups["duration"].Value), 0f, 1f, int.Parse(match.Groups["maxStack"].Value)));
            }
            else if (line.StartsWith("Each time you cast a damaging or healing spell you gain 25 spell power for the next 10 sec, stacking up to 5 times."))
            {
                // Eye of the Broodmother
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { SpellPower = 25f }, 10f, 0f, 1f, 5));
            }
            else if (line == "Increases attack power by 856 for 20 sec.")
            {   // Wrath Stone
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { AttackPower = 856f }, 20f, 120f));
            }
            else if (line.StartsWith("Mangle has a 40% chance to grant 140 Strength for 8 sec"))
            {
                stats.Strength += 37f; //Ashtongue = 37str
                stats.DruidAshtongueTrinket = 150.0f;
            }
            else if (line.StartsWith("Your spells and attacks in each form have a chance to grant you a blessing for 15 sec."))
                stats.Strength += 32f; //LivingRoot = 32str
            /*            else if (line.StartsWith("Chance on critical hit to increase your attack power by "))
                        {
                            line = line.Substring("Chance on critical hit to increase your attack power by ".Length);
                            if (line.Contains(".")) line = line.Substring(0, line.IndexOf("."));
                            if (line.Contains(" ")) line = line.Substring(0, line.IndexOf(" "));
                            stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalCrit, new Stats() { AttackPower = ((float)int.Parse(line))
                            stats.AttackPower += ((float)int.Parse(line)) / 6f;
                        }
            */
            else if ((match = Regex.Match(line, @"Chance on critical hit to increase your attack power by (?<attackPower>\d+) for (?<duration>\d+) secs.")).Success)
            {
                int ap = int.Parse(match.Groups["attackPower"].Value);
                int duration = int.Parse(match.Groups["duration"].Value);
                stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalCrit, new Stats() { AttackPower = ap }, duration, 45f, .1f));
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCrit, new Stats() { AttackPower = ap }, duration, 45f, .1f));
            }
            else if ((match = new Regex(@"Chance on melee and ranged critical strike to increase your attack power by (?<amount>\d\d*) for 10 sec(s?).").Match(line)).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalCrit, new Stats() { AttackPower = (float)int.Parse(match.Groups["amount"].Value) }, 10f, 50f, .1f));
            }
            else if ((match = new Regex(@"Chance on melee or ranged hit to increase your attack power by (?<amount>\d\d*) for 10 sec(s?).").Match(line)).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalHit, new Stats() { AttackPower = (float)int.Parse(match.Groups["amount"].Value) }, 10f, 45f, .1f));
            }
            else if ((match = new Regex(@"Your melee and ranged attacks have a chance to increase your critical strike rating by (?<amount>\d\d*) for 10 sec(s?).").Match(line)).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalHit, new Stats() { CritRating = (float)int.Parse(match.Groups["amount"].Value) }, 10f, 45f, .15f));
            }
            else if (line.StartsWith("Your spells have a chance to increase your spell power by 850 for 10 sec.") || line.StartsWith("Your spells have a chance to increase your spell power by 751 for 10 sec."))
            {
                // Pandora's Plea
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { SpellPower = 751f }, 10f, 45f, .1f));
            }
            else if (line.StartsWith("Each time you cast a harmful spell, you have a chance to gain 590 spell power for 10 sec."))
            {
                // Abyssal Rune
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageSpellHit, new Stats() { SpellPower = 590f }, 10f, 45f, 0.25f));
            }
            else if ((match = Regex.Match(line, @"Each time you cast a helpful spell, you gain (?<mp5>\d+) mana per 5 sec. for (?<duration>\d+) sec.  Stacks up to (?<stacks>\d+) times.")).Success)
            {   // Solace of the Defeated
                stats.AddSpecialEffect(new SpecialEffect(Trigger.HealingSpellCast, new Stats() { Mp5 = (float)int.Parse(match.Groups["mp5"].Value) }, (float)int.Parse(match.Groups["duration"].Value), 0f, 1.00f, int.Parse(match.Groups["stacks"].Value)));
            }
            else if ((match = Regex.Match(line, @"Each time you cast a helpful spell, you gain (?<mp5>\d+) mana per 5 sec. for (?<duration>\d+) sec. nbsp;Stacks up to (?<stacks>\d+) times.")).Success)
            {   // Solace of the Defeated       // FIXME: WOWHEAD version.

                stats.AddSpecialEffect(new SpecialEffect(Trigger.HealingSpellCast, new Stats() { Mp5 = (float)int.Parse(match.Groups["mp5"].Value) }, (float)int.Parse(match.Groups["duration"].Value), 0f, 1.00f, int.Parse(match.Groups["stacks"].Value)));
            }
            // Idol of the Raven Goddess (already added)
            else if (line.Contains(" critical strike rating to the Leader of the Pack aura"))
            {
                string moonkinline = line;
                string treeline = line;
                // Bear/Cat form
                line = line.Substring(0, line.IndexOf(" critical strike rating to the Leader of the Pack aura"));
                line = line.Substring(line.LastIndexOf(' ') + 1);
                if (line.Contains(".")) line = line.Substring(0, line.IndexOf("."));
                if (line.Contains(" ")) line = line.Substring(0, line.IndexOf(" "));
                stats.CritRating += (float)int.Parse(line);
                // Moonkin Form
                moonkinline = moonkinline.Substring(0, moonkinline.IndexOf(" spell critical strike rating to the Moonkin form aura."));
                moonkinline = moonkinline.Substring(moonkinline.LastIndexOf(' ') + 1);
                if (moonkinline.Contains(".")) moonkinline = moonkinline.Substring(0, moonkinline.IndexOf("."));
                if (moonkinline.Contains(" ")) moonkinline = moonkinline.Substring(0, moonkinline.IndexOf(" "));
                stats.IdolCritRating += (float)int.Parse(moonkinline);
                // Tree of Life form
                treeline = treeline.Substring("Increases the healing spell power granted by the Tree of Life form aura by ".Length);
                if (treeline.Contains(",")) treeline = treeline.Substring(0, treeline.IndexOf(","));
                if (treeline.Contains(".")) treeline = treeline.Substring(0, treeline.IndexOf("."));
                if (treeline.Contains(" ")) treeline = treeline.Substring(0, treeline.IndexOf(" "));
                stats.TreeOfLifeAura += (float)int.Parse(treeline);
            }
            else if (line.StartsWith("Your Mangle ability also increases your attack power by "))
            {
                line = line.Substring("Your Mangle ability also increases your attack power by ".Length);
                if (line.Contains(".")) line = line.Substring(0, line.IndexOf("."));
                if (line.Contains(" ")) line = line.Substring(0, line.IndexOf(" "));
                stats.AttackPower += (float)int.Parse(line);
            }
            else if (line.StartsWith("Increases periodic damage done by Rip by "))
            {
                line = line.Substring("Increases periodic damage done by Rip by ".Length);
                if (line.Contains(".")) line = line.Substring(0, line.IndexOf("."));
                if (line.Contains(" ")) line = line.Substring(0, line.IndexOf(" "));
                stats.BonusRipDamagePerCPPerTick += (float)int.Parse(line);
            }
            else if ((match = new Regex(@"Your melee and ranged attacks have a chance to increase your haste rating by (?<amount>\d\d*) for 10 sec(s?).").Match(line)).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalHit, new Stats() { HasteRating = (float)int.Parse(match.Groups["amount"].Value) }, 10f, 45, .15f));
            }
            else if ((match = new Regex(@"Chance on melee and ranged critical strike to increase your haste rating by (?<amount>\d\d*) for 10 sec(s?).").Match(line)).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalCrit, new Stats() { HasteRating = (float)int.Parse(match.Groups["amount"].Value) }, 10f, 45, .10f));
            }
            else if (line.StartsWith("Your melee and ranged attacks have a chance to increase your armor penetration rating by "))
            {
                line = line.Substring("Your melee and ranged attacks have a chance to increase your armor penetration rating by ".Length);
                if (line.Contains(".")) line = line.Substring(0, line.IndexOf("."));
                if (line.Contains(" ")) line = line.Substring(0, line.IndexOf(" "));
                stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalHit, new Stats() { ArmorPenetrationRating = (float)int.Parse(line) }, 10f, 45, .15f));
            }
            else if (isArmory && line.StartsWith("Increases attack power by ") && !line.Contains("forms only"))
            {
                line = line.Substring("Increases attack power by ".Length);
                if (line.Contains(".")) line = line.Substring(0, line.IndexOf("."));
                if (line.Contains(" ")) line = line.Substring(0, line.IndexOf(" "));
                stats.AttackPower += int.Parse(line);
            }
            else if (isArmory && line.StartsWith("Increases defense rating by "))
            {
                line = line.Substring("Increases defense rating by ".Length);
                if (line.Contains(".")) line = line.Substring(0, line.IndexOf("."));
                if (line.Contains(" ")) line = line.Substring(0, line.IndexOf(" "));
                stats.DefenseRating += int.Parse(line);
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
            else if (isArmory && line.StartsWith("Increases the block value of your shield by "))
            {
                line = line.Substring("Increases the block value of your shield by ".Length);
                if (line.Contains(".")) line = line.Substring(0, line.IndexOf("."));
                if (line.Contains(" ")) line = line.Substring(0, line.IndexOf(" "));
                stats.BlockValue += int.Parse(line);
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
            else if (isArmory && line.StartsWith("Increases your armor penetration rating by "))
            {
                line = line.Substring("Increases your armor penetration rating by ".Length);
                if (line.Contains(".")) line = line.Substring(0, line.IndexOf("."));
                if (line.Contains(" ")) line = line.Substring(0, line.IndexOf(" "));
                stats.ArmorPenetrationRating += int.Parse(line);
            }
            else if (isArmory && line.StartsWith("Increases armor penetration rating by "))
            {
                line = line.Substring("Increases armor penetration rating by ".Length);
                if (line.Contains(".")) line = line.Substring(0, line.IndexOf("."));
                if (line.Contains(" ")) line = line.Substring(0, line.IndexOf(" "));
                stats.ArmorPenetrationRating += int.Parse(line);
            }
            else if (line.StartsWith("Increases the damage dealt by Shred by "))
            {
                line = line.Substring("Increases the damage dealt by Shred by ".Length);
                if (line.Contains(".")) line = line.Substring(0, line.IndexOf("."));
                if (line.Contains(" ")) line = line.Substring(0, line.IndexOf(" "));
                stats.BonusShredDamage += int.Parse(line);
            }
            else if (line.StartsWith("Increases the damage dealt by Mangle (Cat) by "))
            {

                line = line.Substring("Increases the damage dealt by Mangle (Cat) by ".Length);
                if (line.Contains(".")) line = line.Substring(0, line.IndexOf("."));
                if (line.Contains(" ")) line = line.Substring(0, line.IndexOf(" "));
                stats.BonusMangleCatDamage += int.Parse(line);

                // WTB Regex
                stats.BonusMangleBearDamage += 51.75f;

            }
            else if (line.EndsWith(" Weapon Damage."))
            {
                line = line.Trim('+').Substring(0, line.IndexOf(" "));
                stats.WeaponDamage += int.Parse(line);
            }
            else if (line.StartsWith("Your Mangle ability has a chance to grant ") &&
                line.EndsWith("agility for 10 sec.")) //10sec = Idol of Terror
            {
                line = line.Substring("Your Mangle ability has a chance to grant ".Length);
                if (line.Contains(".")) line = line.Substring(0, line.IndexOf("."));
                if (line.Contains(" ")) line = line.Substring(0, line.IndexOf(" "));
                stats.AddSpecialEffect(new SpecialEffect(Trigger.MangleCatHit, new Stats() { Agility = int.Parse(line) }, 10f, 0f, .65f));
                stats.AddSpecialEffect(new SpecialEffect(Trigger.MangleBearHit, new Stats() { Agility = int.Parse(line) }, 10f, 0f, .45f));
            }
            else if (line.StartsWith("Your Mangle ability has a chance to grant ") &&
                line.EndsWith("agility for 12 sec.")) //12sec = Idol of Corruptor
            {
                line = line.Substring("Your Mangle ability has a chance to grant ".Length);
                if (line.Contains(".")) line = line.Substring(0, line.IndexOf("."));
                if (line.Contains(" ")) line = line.Substring(0, line.IndexOf(" "));
                stats.AddSpecialEffect(new SpecialEffect(Trigger.MangleCatHit, new Stats() { Agility = int.Parse(line) }, 12f, 0f, 1f));
                stats.AddSpecialEffect(new SpecialEffect(Trigger.MangleBearHit, new Stats() { Agility = int.Parse(line) }, 12f, 0f, .7f));
            }
            else if (line.StartsWith("While in Bear Form, your Lacerate and Swipe abilities have a chance to grant "))
            {
                //While in Bear Form, your Lacerate and Swipe abilities have a chance to grant 200 dodge rating for 9 sec, and your Cat Form's Mangle and Shred abilities have a chance to grant 200 Agility for 16 sec.

                line = line.Substring("While in Bear Form, your Lacerate and Swipe abilities have a chance to grant ".Length);
                string bearDodge = line.Substring(0, line.IndexOf(' '));
                line = line.Substring("200 dodge rating for 9 sec, and your Cat Form's Mangle and Shred abilities have a chance to grant ".Length);
                if (line.Contains(".")) line = line.Substring(0, line.IndexOf("."));
                if (line.Contains(" ")) line = line.Substring(0, line.IndexOf(" "));
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SwipeBearOrLacerateHit, new Stats() { DodgeRating = int.Parse(bearDodge) }, 9f, 0f, .65f));
                stats.AddSpecialEffect(new SpecialEffect(Trigger.MangleCatOrShredHit, new Stats() { Agility = int.Parse(line) }, 16f, 0f, .85f));
            }
            else if (line.StartsWith("Each time a melee attack strikes you, you have a chance to gain "))
            {
                line = line.Substring("Each time a melee attack strikes you, you have a chance to gain ".Length);
                if (line.Contains(".")) line = line.Substring(0, line.IndexOf("."));
                if (line.Contains(" ")) line = line.Substring(0, line.IndexOf(" "));
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageTaken, new Stats() { BonusArmor = int.Parse(line) }, 10f, 45f, 0.25f));
            }
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
            // Restores 7 mana per 5 sec.
            // Check to see if the desc contains the token 'mana'.  Items like Frostwolf Insignia
            // and Essense Infused Shroom Restore health.
            else if (isArmory && line.StartsWith("Restores ") && line.Contains("mana") && !line.Contains("kill a target"))
            {
                line = line.Substring("Restores ".Length);
                line = line.Substring(0, line.IndexOf(" mana"));
                stats.Mp5 += int.Parse(line);
            }
            else if ((match = Regex.Match(line, @"You gain (?:a|an) (?<buffName>[\w\s]+) each time you cause a (?<trigger>non-periodic|damaging)+ spell critical strike\.(?:\s|nbsp;)+When you reach (?<stackSize>\d+) [\w\s]+, they will release, firing (?<projectile>[\w\s]+) for (?<mindmg>\d+) to (?<maxdmg>\d+) damage\.(?:\s|nbsp;)+[\w\s]+ cannot be gained more often than once every (?<icd>\d+(?:\.\d+)?) sec.")).Success)
            {
                //Capacitor like procs
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
            else if (line.StartsWith("You gain 25% more mana when you use a mana gem.  In addition, using a mana gem grants you 225 spell power for 15 sec."))
            {
                // Serpent-Coil Braid
                stats.BonusManaGem += 0.25f;
                stats.AddSpecialEffect(new SpecialEffect(Trigger.ManaGem, new Stats() { SpellPower = 225 }, 15f, 0f));
            }
            else if (line.StartsWith("Grants 170 increased spell power for 10 sec when one of your spells is resisted."))
            {
                // Eye of Magtheridon
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellMiss, new Stats() { SpellPower = 170 }, 10, 0));
            }
            else if (line.StartsWith("Your spell critical strikes have a 50% chance to grant you 145 spell haste rating for 5 sec."))
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCrit, new Stats() { HasteRating = 145 }, 5, 0, 0.5f));
            }
            else if (line.StartsWith("Your harmful spells have a chance to increase your spell haste rating by 320 for 6 secs."))
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageSpellHit, new Stats() { HasteRating = 320 }, 6, 45, 0.1f));
            }
            else if (line.StartsWith("Your spell casts have a chance to increase your spell power by 590 for 10 sec."))
            {
                // Flow of Knowledge
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { SpellPower = 590 }, 10, 45, 0.1f));
            }
            else if (line.StartsWith("Chance on spell critical hit to increase your spell power by 225 for 10 secs."))
            {
                // Shiffar's Nexus-Horn
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCrit, new Stats() { SpellPower = 225 }, 10, 45, 0.2f));
            }
            else if (line.StartsWith("Increases the effect that healing and mana potions have on the wearer by "))
            {
                line = line.Substring("Increases the effect that healing and mana potions have on the wearer by ".Length);
                line = line.Substring(0, line.IndexOf('%'));
                stats.BonusManaPotion += int.Parse(line) / 100f;
                // TODO health potion effect
            }
            //Your spell critical strikes have a chance to increase your spell power by 190 for 15 sec.
            else if (line.StartsWith("Your spell critical strikes have a chance to increase your spell power by "))
            {
                line = line.Substring("Your spell critical strikes have a chance to increase your spell power by ".Length);
                float value = int.Parse(line.Substring(0, line.IndexOf(" for")));
                line = line.Substring(line.IndexOf(" for") + " for ".Length);
                int duration = int.Parse(line.Substring(0, line.IndexOf(" ")));

                //switch (duration)
                //{
                //    case 15:
                //        if (name == "Sextant of Unstable Currents")
                //        {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCrit, new Stats() { SpellPower = value }, duration, 45, 0.2f));
                //        }
                //        break;
                //}
            }
            // Timbal's Focusing Crystal
            else if (line.StartsWith("Each time one of your spells deals periodic damage, there is a chance 285 to 475 additional damage will be dealt."))
            {
                stats.TimbalsProc = 1.0f;
            }
            // Wrath of Cenarius
            else if (line.StartsWith("Gives a chance when your harmful spells land to increase the damage of your spells and effects by 132 for 10 sec."))
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellHit, new Stats() { SpellPower = 132 }, 10, 0, 0.05f));
            }
            else if (line == "Your melee and ranged attacks have a chance to strike your enemy, dealing 1504 to 2256 arcane damage.")
            {   //Bandit's Insignia
                stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalHit, new Stats() { ArcaneDamage = 1880 }, 0f, 45f, 0.15f));
            }
            else if (line.StartsWith("Gives a chance when your harmful spells land to increase the damage of your spells and effects by up to "))
            {
                // Gives a chance when your harmful spells land to increase the damage of your spells and effects by up to 130 for 10 sec.
                line = line.Substring("Gives a chance when your harmful spells land to increase the damage of your spells and effects by up to ".Length);
                float value = int.Parse(line.Substring(0, line.IndexOf(" for")));
                line = line.Substring(line.IndexOf(" for") + " for ".Length);
                int duration = int.Parse(line.Substring(0, line.IndexOf(" ")));

                //switch (duration)
                //{
                //    case 10:
                //        if (name == "Robe of the Elder Scribes")
                //        {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageSpellHit, new Stats() { SpellPower = value }, duration, 45, 0.1f));
                //        }
                //        break;
                //}
            }
            else if (line.StartsWith("Your offensive spells have a chance on hit to increase your spell power by "))
            {
                // Your offensive spells have a chance on hit to increase your spell power by 95 for 10 secs.
                line = line.Substring("Your offensive spells have a chance on hit to increase your spell power by ".Length);
                float value = int.Parse(line.Substring(0, line.IndexOf(" for")));
                line = line.Substring(line.IndexOf(" for") + " for ".Length);
                int duration = int.Parse(line.Substring(0, line.IndexOf(" ")));

                //switch (duration)
                //{
                //    case 10:
                //        if (name == "Band of the Eternal Sage")
                //        {
                // Fixed in 2.4 to be 10 sec instead of 15
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageSpellHit, new Stats() { SpellPower = value }, duration, 45, 0.1f));
                //        }
                //        break;
                //}
            }
            else if (line.StartsWith("Increases the spell power of your Starfire spell by "))
            {
                line = line.Substring("Increases the spell power of your Starfire spell by ".Length);
                line = line.Replace(".", "");
                stats.StarfireDmg += float.Parse(line, System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (line.StartsWith("Increases the spell power of your Moonfire spell by "))
            {
                line = line.Substring("Increases the spell power of your Moonfire spell by ".Length);
                line = line.Replace(".", "");
                stats.MoonfireDmg += float.Parse(line, System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (line.StartsWith("Increases the damage dealt by Wrath by "))
            {
                line = line.Substring("Increases the damage dealt by Wrath by ".Length);
                line = line.Replace(".", "");
                stats.WrathDmg += float.Parse(line, System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (line.StartsWith("Your Moonfire ability has a chance to grant "))
            {
                line = line.Substring("Your Moonfire ability has a chance to grant ".Length);
                string spellPowerLine = line.Substring(0, line.IndexOf(" spell power for"));
                string durationLine = line.Substring(line.IndexOf("for") + 3, line.IndexOf(" sec.") - line.IndexOf("for") - 3);
                float spellPower = float.Parse(spellPowerLine, System.Globalization.CultureInfo.InvariantCulture);
                float duration = float.Parse(durationLine, System.Globalization.CultureInfo.InvariantCulture);
                stats.AddSpecialEffect(new SpecialEffect() { Chance = 0.5f, Cooldown = 0f, Duration = duration, Trigger = Trigger.MoonfireCast, Stats = new Stats() { SpellPower = spellPower }, MaxStack = 1 });
            }
            else if (line.StartsWith("Increases the spell power of your Insect Swarm by "))
            {
                line = line.Substring("Increases the spell power of your Insect Swarm by ".Length);
                line = line.Replace(".", "");
                stats.InsectSwarmDmg += float.Parse(line, System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (line.StartsWith("Each time your Moonfire spell deals periodic damage, you have a chance to gain "))
            {
                line = line.Substring("Each time your Moonfire spell deals periodic damage, you have a chance to gain ".Length);
                string spellPowerLine = line.Substring(0, line.IndexOf(" critical strike rating for"));
                string durationLine = line.Substring(line.IndexOf("for") + 3, line.IndexOf(" sec.") - line.IndexOf("for") - 3);
                float critRating = float.Parse(spellPowerLine, System.Globalization.CultureInfo.InvariantCulture);
                float duration = float.Parse(durationLine, System.Globalization.CultureInfo.InvariantCulture);
                stats.AddSpecialEffect(new SpecialEffect() { Chance = 0.7f, Cooldown = 0f, Duration = duration, Trigger = Trigger.MoonfireTick, Stats = new Stats() { CritRating = critRating }, MaxStack = 1 });
            }
            else if (line.StartsWith("Increases the final healing value of your Lifebloom by "))
            {
                line = line.Substring("Increases the final healing value of your Lifebloom by ".Length);
                line = line.Replace(".", "");
                stats.LifebloomFinalHealBonus += (float)int.Parse(line);
            }
            else if (line.StartsWith("Reduces the mana cost of Rejuvenation by "))
            {
                line = line.Substring("Reduces the mana cost of Rejuvenation by ".Length);
                line = line.Replace(".", "");
                stats.ReduceRejuvenationCost += (float)int.Parse(line);
            }
            else if (line.StartsWith("Reduces the mana cost of Regrowth by "))
            {
                line = line.Substring("Reduces the mana cost of Regrowth by ".Length);
                line = line.Replace(".", "");
                stats.ReduceRegrowthCost += (float)int.Parse(line);
            }
            else if (line.StartsWith("Gain up to 25 mana each time you cast Healing Touch."))
            {
                stats.ReduceHealingTouchCost += 25;
            }
            else if (line.StartsWith("Increases the periodic healing of Rejuvenation by "))
            {
                line = line.Substring("Increases the periodic healing of Rejuvenation by ".Length);
                line = line.Replace(".", "");
                stats.RejuvenationHealBonus += (float)int.Parse(line);
            }
            else if (line.StartsWith("Increases spell power of Rejuvenation by "))
            {
                line = line.Substring("Increases spell power of Rejuvenation by ".Length);
                line = line.Replace(".", "");
                stats.RejuvenationSpellpower += (float)int.Parse(line);
            }
            else if ((match = Regex.Match(line, @"Each time your Rejuvenation spell deals periodic healing, you have a chance to gain (?<spellPower>\d+) spell power for (?<duration>\d+) sec.")).Success)
            {
                int spellPower = int.Parse(match.Groups["spellPower"].Value);
                // Idol of Flaring Growth
                // Not yet sure about cooldown and proc chance
                stats.AddSpecialEffect(new SpecialEffect(Trigger.RejuvenationTick, new Stats() { SpellPower = spellPower }, int.Parse(match.Groups["duration"].Value), 0, 0.7f));
            }
            else if ((match = Regex.Match(line, @"The periodic healing from your Rejuvenation spell grants (?<spellPower>\d+) spell power for (?<duration>\d+) sec.  Stacks up to (?<stacks>\d+) times.")).Success)
            {
                int spellPower = int.Parse(match.Groups["spellPower"].Value);
                // Idol of the Black Willow
                stats.AddSpecialEffect(new SpecialEffect(Trigger.RejuvenationTick, new Stats() { SpellPower = spellPower }, int.Parse(match.Groups["duration"].Value), 0, 1f, int.Parse(match.Groups["stacks"].Value)));
            }
            else if ((match = Regex.Match(line, @"The periodic healing from your Rejuvenation spell grants (?<spellPower>\d+) spell power for (?<duration>\d+) sec. nbsp;Stacks up to (?<stacks>\d+) times.")).Success)
            {
                int spellPower = int.Parse(match.Groups["spellPower"].Value);
                // Idol of the Black Willow
                stats.AddSpecialEffect(new SpecialEffect(Trigger.RejuvenationTick, new Stats() { SpellPower = spellPower }, int.Parse(match.Groups["duration"].Value), 0, 1f, int.Parse(match.Groups["stacks"].Value)));
            }
            else if (line.StartsWith("Increases the spell power on the periodic portion of your Lifebloom by ")) //if (line.StartsWith("Increases the periodic healing of your Lifebloom by up to "))
            {
                line = line.Substring("Increases the spell power on the periodic portion of your Lifebloom by ".Length);
                line = line.Replace(".", "");
                stats.LifebloomTickHealBonus += (float)int.Parse(line);
            }
            else if (line.StartsWith("Increases the amount healed by Healing Touch by "))
            {
                line = line.Substring("Increases the amount healed by Healing Touch by ".Length);
                line = line.Replace(".", "");
                stats.HealingTouchFinalHealBonus += (float)int.Parse(line);
            }
            else if (line.StartsWith("Increases the spell power of your Nourish by "))
            {
                line = line.Substring("Increases the spell power of your Nourish by ".Length);
                line = line.Replace(".", "");
                stats.NourishSpellpower += (float)int.Parse(line);
            }
            else if (line.StartsWith("Your spell casts have a chance to allow 10% of your mana regeneration to continue while casting for "))
            { //NOTE: What the armory says is "10%" here, but that's for level 80 characters. Still provides 15% at level 70.
                line = line.Substring("Your spell casts have a chance to allow 10% of your mana regeneration to continue while casting for ".Length);
                line = line.Replace(" sec.", "");
                //stats.BangleProc += (float)int.Parse(line);
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { SpellCombatManaRegeneration = 0.1f }, 15.0f, 45.0f));
            }
            else if (line.StartsWith("2% chance on successful spellcast to allow 100% of your Mana regeneration to continue while casting for 15 sec."))
            {
                // Darkmoon Card: Blue Dragon
            }
            else if ((match = new Regex("Reduces the mana cost of Holy Light by (?<amount>\\d\\d*).").Match(line)).Success)
            {
                stats.HolyLightManaCostReduction += (float)int.Parse(match.Groups["amount"].Value);
            }
            else if ((match = new Regex("Increases spell power of Flash of Light by (?<amount>\\d\\d*).").Match(line)).Success)
            {
                stats.FlashOfLightSpellPower += (float)int.Parse(match.Groups["amount"].Value);
            }
            else if ((match = new Regex("Increases spell power of Holy Light by (?<amount>\\d\\d*).").Match(line)).Success)
            {
                stats.HolyLightSpellPower += (float)int.Parse(match.Groups["amount"].Value);
            }
            else if (line.StartsWith("When struck in combat has a chance of increasing your armor by "))
            {
                line = line.Substring("When struck in combat has a chance of increasing your armor by ".Length);
                float value = int.Parse(line.Substring(0, line.IndexOf(" for")));
                line = line.Substring(line.IndexOf(" for") + " for ".Length);
                int duration = int.Parse(line.Substring(0, line.IndexOf(" ")));

                //switch (duration)
                //{
                //    case 10:
                //        if (name == "Band of the Eternal Defender")
                //        {
                //The buff is up about 1/6 the time, so 800/6 = 133 armor
                //Bonus Armor is not affected by armor multipliers.
                stats.BonusArmor += (float)Math.Round(value / 6f);
                //        }
                //        break;
                //}
            }
            else if (line.StartsWith("Increases your pet's critical strike chance by "))
            {
                string critChance = line.Substring("Increases your pet's critical strike chance by ".Length).Trim();
                if (critChance.EndsWith("%."))
                {
                    stats.BonusPetCritChance = float.Parse(critChance.Substring(0, critChance.Length - 2)) / 100f;
                }
            }
            else if (line.StartsWith("Increases damage dealt by your pet by "))
            {
                string critChance = line.Substring("Increases damage dealt by your pet by ".Length).Trim();
                if (critChance.EndsWith("%."))
                {
                    stats.BonusPetDamageMultiplier = float.Parse(critChance.Substring(0, critChance.Length - 2)) / 100f;
                }
            }
            else if (line.StartsWith("Each healing spell you cast has a 2% chance to make your next heal cast within 15 sec cost 450 less mana."))
            {
                stats.ManacostReduceWithin15OnHealingCast += 450;
                stats.AddSpecialEffect(new SpecialEffect(Trigger.HealingSpellCast, new Stats() { HealingOmenProc = 450 }, 0, 0, 0.02f));
            }
            else if (line.StartsWith("Your healing spells have a chance to make your next heal cast within 15 sec cost 800 less mana."))
            {
                // Soul Preserver
                stats.ManacostReduceWithin15OnHealingCast += 800;
                stats.AddSpecialEffect(new SpecialEffect(Trigger.HealingSpellCast, new Stats() { HealingOmenProc = 800 }, 0, 0, 0.02f));
            }
            else if ((match = Regex.Match(line, @"Your damaging and healing spells have a chance to increase your spell power by (?<spellPower>\d+) for (?<duration>\d+) sec.")).Success)
            {
                int spellPower = int.Parse(match.Groups["spellPower"].Value);
                // Forge Ember
                // This is a nasty trick for compatibility = when designing a healer, please use this version:
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { SpellPower = spellPower }, int.Parse(match.Groups["duration"].Value), 45, 0.1f));
            }
            else if ((match = Regex.Match(line, @"Your harmful spells have a chance to increase your spell power by (?<spellPower>\d+) for (?<duration>\d+) sec.")).Success)
            {
                int spellPower = int.Parse(match.Groups["spellPower"].Value);
                // Sundial of the Exiled (NOT FOR HEALERS) / Flare of the Heavens
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageSpellCast, new Stats() { SpellPower = spellPower }, int.Parse(match.Groups["duration"].Value), 45, 0.1f));
            }
            else if (line.StartsWith("Your spells have a chance to increase your spell power by 765 for 10 sec."))
            {
                // Dying Curse
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { SpellPower = 765 }, 10, 45, 0.15f));
            }
            else if ((match = Regex.Match(line, @"Each time you cast a damaging or healing spell, there is chance you will gain up to (?<amount>\d+) mana per 5 for 15 sec.")).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { Mp5 = int.Parse(match.Groups["amount"].Value) }, 15f, 45f, .10f));
            }
            else if (line.StartsWith("Each time you cast a spell, there is chance you will gain 290 spell power."))
            {
                // Tome of Fiery Redemption, T5 Paladin Class Tinket
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { SpellPower = 290 }, 15f, 45f, .15f));
            }
            else if ((match = new Regex("Each time you cast a spell, there is a chance you will gain up to (?<amount>\\d\\d*) mana per 5 for 15 sec.").Match(line)).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { Mp5 = (float)int.Parse(match.Groups["amount"].Value) }, 15f, 45f, .10f));
                stats.ManaRestoreOnCast_10_45 += int.Parse(match.Groups["amount"].Value) * 3f;
            }
            else if (line.StartsWith("Each time you cast a spell you gain 18 Spirit for the next 10 sec., stacking up to 10 times."))
            {
                // Majestic Dragon Figurine
                stats.ExtraSpiritWhileCasting += 180;
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { Spirit = 18.0f }, 10f, 0f, 1.0f, 10));
            }
            else if (line.StartsWith("Your spells have a chance to increase your haste rating by 505 for 10 secs."))
            {
                // Embrace of the Spider
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { HasteRating = 505 }, 10, 45, 0.1f));
            }
            else if ((match = Regex.Match(line, @"Reduces the base mana cost of your spells by (?<amount>\d+).")).Success)
            {
                stats.SpellsManaReduction = int.Parse(match.Groups["amount"].Value);
            }
            else if (line.StartsWith("Your direct healing and heal over time spells have a chance to increase your haste rating by 505 for 10 secs."))
            {
                // The Egg of Mortal Essence
                //stats.SpellHasteFor10SecOnHeal_10_45 += 505;
                stats.AddSpecialEffect(new SpecialEffect(Trigger.HealingSpellCast, new Stats() { HasteRating = 505f }, 10f, 45f, .1f));
            }
            else if (line.StartsWith("Your spell critical strikes have a chance to restore 900 mana."))
            {
                // Soul of the Dead
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCrit, new Stats() { ManaRestore = 900f }, 0f, 45f, .25f));
                stats.ManaRestoreOnCrit_25_45 += 900f;
            }
            else if (line.StartsWith("Your harmful spells have a chance to strike your enemy, dealing 1168 to 1752 shadow damage."))
            {
                // Pendulum of Telluric Currents
                stats.PendulumOfTelluricCurrentsProc += 1;
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellHit, new Stats() { ShadowDamage = 1460 }, 0f, 45f, .1f));
            }
            else if (line.StartsWith("Each time one of your spells deals periodic damage, there is a chance 788 to 1312 additional damage will be dealt."))
            {
                // Extract of Necromantic Power
                stats.ExtractOfNecromanticPowerProc += 1;
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DoTTick, new Stats() { ShadowDamage = 1050f }, 0f, 45f, .1f));
            }
            else if (line.StartsWith("Each time you deal damage, you have a chance to do an additional 1750 to 2250 Shadow damage."))
            {
                // Darkmoon Card: Death
                stats.DarkmoonCardDeathProc += 1;
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageDone, new Stats() { ShadowDamage = 2000f }, 0f, 45f, .35f));
            }
            else if (line.StartsWith("Your direct healing spells have a chance to place a heal over time on your target"))
            {
                Regex r = new Regex("Your direct healing spells have a chance to place a heal over time on your target, healing (?<hot>\\d*) over (?<dur>\\d*) sec\\.");
                Match m = r.Match(line);
                if (m.Success)
                {
                    float hot = int.Parse(m.Groups["hot"].Value);
                    float dur = int.Parse(m.Groups["dur"].Value);
                    // internal cooldown: 45 seconds
                    // 20% chance, so on average procs after 5 casts
                    // lets say 60 seconds
                    stats.AddSpecialEffect(new SpecialEffect(Trigger.HealingSpellCast, new Stats() { Healed = hot }, 0f, 45f, .2f));
                    stats.BonusHoTOnDirectHeals += hot / 60f;
                }
            }
            else if ((match = Regex.Match(line, @"When active, grants the wielder (?<defenseRating>\d+) defense rating and (?<bonusArmor>\d+) armor for 10 sec.")).Success)
            {
                // Quel'Serrar Sanctuary proc (all versions)
                stats.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit, new Stats() { DefenseRating = int.Parse(match.Groups["defenseRating"].Value), BonusArmor = int.Parse(match.Groups["bonusArmor"].Value) }, 10f, 0f, -2f));
            }
            else if (line.StartsWith("Increases spell power of Chain Lightning and Lightning Bolt by "))
            {
                line = line.Replace(".", "");
                line = line.Substring("Increases spell power of Chain Lightning and Lightning Bolt by ".Length);
                stats.LightningSpellPower = float.Parse(line);
            }
            else if (line.StartsWith("Your Lightning Bolt spell has a chance to grant "))
            {
                Regex r = new Regex("Your Lightning Bolt spell has a chance to grant (?<haste>\\d*) haste rating for 10 sec.");
                Match m = r.Match(line);
                if (m.Success)
                {
                    stats.LightningBoltHasteProc_15_45 += (float)int.Parse(m.Groups["haste"].Value);
                    stats.AddSpecialEffect(new SpecialEffect(Trigger.ShamanLightningBolt, new Stats() { HasteRating = int.Parse(m.Groups["haste"].Value) }, 10f, 45f, 0.15f));
                }
            }
            else if (line.StartsWith("Increases the damage dealt by your Lava Burst by "))
            {
                line = line.Replace(".", "");
                line = line.Substring("Increases the damage dealt by your Lava Burst by ".Length);
                stats.LavaBurstBonus = float.Parse(line);
            }
            else if (line.StartsWith("Increases the base damage of your Lava Burst by "))
            {
                line = line.Replace(".", "");
                line = line.Substring("Increases the base damage of your Lava Burst by ".Length);
                stats.LavaBurstBonus = float.Parse(line);
            }
            else if ((match = new Regex("Your Crusader Strike ability also grants you (?<amount>\\d\\d*) attack power for 10 sec.").Match(line)).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.CrusaderStrikeHit, new Stats() { AttackPower = int.Parse(match.Groups["amount"].Value) }, 10f, 0f));
            }
            else if ((match = new Regex("Your Crusader Strike ability grants (?<amount>\\d\\d*) Strength for 15 sec\\. ((nbsp;)| )?Stacks up to 5 times\\.").Match(line)).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.CrusaderStrikeHit, new Stats() { Strength = int.Parse(match.Groups["amount"].Value) }, 15f, 0f, 1f, 5));
            }
            else if ((match = new Regex("Steals (?<amount1>\\d\\d*) to (?<amount2>\\d\\d*) life from target enemy\\.").Match(line)).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit, new Stats() { ShadowDamage = (int.Parse(match.Groups["amount1"].Value) + int.Parse(match.Groups["amount2"].Value)) / 2f }, 0f, 0f, 0.15f));
            }
            else if (line == "Increases the spell power of your Consecration spell by 141.")
            {
                stats.ConsecrationSpellPower = 141f;
            }
            else if (line == "Increases the damage dealt by Crusader Strike by 78.75.")
            {
                stats.CrusaderStrikeDamage = 78.75f;
            }
            else if (line == "Increases the damage dealt by Crusader Strike by 79.")
            {
                stats.CrusaderStrikeDamage = 78.75f;
            }
            else if (line == "Increases the damage done by Divine Storm by 235.")
            {
                stats.DivineStormDamage = 235f;
            }
            else if (line == "Causes your Divine Storm to increase your Critical Strike rating by 73 for 8 sec.")
            {
                stats.DivineStormDamage = 81;
            }
            else if (line == "Causes your Judgements to increase your Critical Strike rating by 53 for 5 sec.")
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.JudgementHit, new Stats() { CritRating = 53 }, 5f, 0f, 1f));
            }
            else if (line == "Causes your Judgements to increase your Critical Strike Rating by 61 for 5 sec.")
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.JudgementHit, new Stats() { CritRating = 61 }, 5f, 0f, 1f));
            }
            else if (line == "Increases the damage dealt by your Crusader Strike ability by 5%.")
            {
                stats.CrusaderStrikeMultiplier = .05f;
            }
            else if (line == "Your healing spells have a chance to cause Blessing of Ancient Kings for 15 sec allowing your heals to shield the target absorbing damage equal to 15% of the amount healed.")
            {
                // Val'anyr, Hammer of Ancient Kings effect
                stats.AddSpecialEffect(new SpecialEffect(Trigger.HealingSpellHit, new Stats() { ShieldFromHealed = .15f }, 15f, 45f, .1f));
            }
            else if (line == "Each time your Seal of Vengeance or Seal of Corruption ability deals periodic damage, you have a chance to gain 200 Strength for 16 sec.")
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SealOfVengeanceTick, new Stats() { Strength = 200 }, 16f, 6f, .7f));
            }
            #region Added by Rawr.ProtPaladin
            else if ((match = new Regex(@"Your Judgement ability also increases your shield block value by (?<amount>\d\d*) for 10 sec(s?).").Match(line)).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.JudgementHit, new Stats() { JudgementBlockValue = (float)int.Parse(match.Groups["amount"].Value) }, 10f, 0f, 1f));
            }
            else if ((match = new Regex(@"Your Judgement ability also grants you (?<amount>\d\d*) resilience rating for 6 sec.").Match(line)).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.JudgementHit, new Stats() { Resilience = (float)int.Parse(match.Groups["amount"].Value) }, 6f, 0f, 1f));
            }
            else if ((match = new Regex(@"Your Holy Shield ability also grants you (?<amount>\d\d*) resilience rating for 6 sec.").Match(line)).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.HolyShield, new Stats() { Resilience = (float)int.Parse(match.Groups["amount"].Value) }, 6f, 0f, 1f));
            }
            else if ((match = new Regex(@"Increases your block value by (?<blockValue>\d\d*) for (?<duration>\d\d*) sec each time you use Holy Shield.").Match(line)).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.HolyShield, new Stats() { HolyShieldBlockValue = (float)int.Parse(match.Groups["blockValue"].Value) }, (float)int.Parse(match.Groups["duration"].Value), 0f, 1f));
            }
            else if (line == "Your Shield of Righteousness deals an additional 96 damage.")
            {
                stats.BonusShieldOfRighteousnessDamage = 96;
            }
            else if ((match = new Regex(@"Each time you use your Hammer of The Righteous ability, you have a chance to gain (?<dodgeRating>\d\d*) dodge rating for (?<duration>\d\d*) sec.").Match(line)).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.HammeroftheRighteous, new Stats() { DodgeRating = (float)int.Parse(match.Groups["dodgeRating"].Value) }, (float)int.Parse(match.Groups["duration"].Value), 10f, 0.8f));
            }
            #endregion
            #region Shaman Totem Relic Slot
            else if (line == "Your Shock spells have a chance to grant 110 attack power for 10 sec.")
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.ShamanShock, new Stats() { AttackPower = 110 }, 10f, 45f));
            }
            else if (line == "Your Storm Strike ability also grants you 60 haste rating for 6 sec.")
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.ShamanStormStrike, new Stats() { HasteRating = 60 }, 6f, 0f));
            }
            else if (line == "Increases the attack power bonus on Windfury Weapon attacks by ")
            {
                line = line.Replace(".", "");
                line = line.Substring("Increases the attack power bonus on Windfury Weapon attacks by ".Length);
                stats.BonusWFAttackPower = float.Parse(line); // Totem of Astral Winds & Totem of Splintering
            }
            else if (line.StartsWith("Your Lava Lash ability also grants you "))
            {
                Regex r = new Regex("Your Lava Lash ability also grants you (?<attackpower>\\d*) attack power for 6 sec.");
                Match m = r.Match(line);
                if (m.Success) // XXX Gladiators Totem of Indomitability
                {
                    float attackPower = (float)int.Parse(m.Groups["attackpower"].Value);
                    stats.AddSpecialEffect(new SpecialEffect(Trigger.ShamanLavaLash, new Stats() { AttackPower = attackPower }, 6f, 0f));
                }
            }
            else if (line.StartsWith("Each time you use your Lava Lash ability,"))
            {
                Regex r = new Regex("Each time you use your Lava Lash ability, you have a chance to gain (?<attackpower>\\d*) attack power for 18 sec.");
                Match m = r.Match(line);
                if (m.Success) // Totem of Quaking Earth
                {
                    float attackPower = (float)int.Parse(m.Groups["attackpower"].Value);
                    stats.AddSpecialEffect(new SpecialEffect(Trigger.ShamanLavaLash, new Stats() { AttackPower = attackPower }, 18f, 9f, 0.8f));
                }
            }
            else if (line.StartsWith("Your Stormstrike ability grants"))
            {
                Regex r = new Regex("Your Stormstrike ability grants (?<attackpower>\\d*) attack power for 15 sec.  Stacks up to 3 times.");
                Match m = r.Match(line.Replace("nbsp;", " "));
                if (m.Success) // Totem of the Avalanche
                {
                    float attackPower = (float)int.Parse(m.Groups["attackpower"].Value);
                    stats.AddSpecialEffect(new SpecialEffect(Trigger.ShamanStormStrike, new Stats() { AttackPower = attackPower }, 15f, 0f, 1f, 3));
                }
            }
            else if (line.StartsWith("Each time you cast Lightning Bolt,"))
            {
                Regex r = new Regex("Each time you cast Lightning Bolt, you have a chance to gain (?<hasterating>\\d*) haste rating for 12 sec.");
                Match m = r.Match(line);
                if (m.Success) // Totem of Electrifying Wind
                {
                    float hasterating = (float)int.Parse(m.Groups["hasterating"].Value);
                    stats.AddSpecialEffect(new SpecialEffect(Trigger.ShamanLightningBolt, new Stats() { HasteRating = hasterating }, 12f, 0f, 0.7f));
                }
            }
            else if (line.StartsWith("The periodic damage from your Flame Shock spell grants"))
            {
                Regex r = new Regex("The periodic damage from your Flame Shock spell grants (?<hasterating>\\d*) haste rating for 30 sec.  Stacks up to 5 times.");
                Match m = r.Match(line.Replace("nbsp;", " "));
                if (m.Success) // 	Bizuri's Totem of Shattered Ice
                {
                    float hasterating = (float)int.Parse(m.Groups["hasterating"].Value);
                    stats.AddSpecialEffect(new SpecialEffect(Trigger.ShamanFlameShockDoTTick, new Stats() { HasteRating = hasterating }, 30f, 0f, 1f, 5));
                }
            }
            else if (line.StartsWith("Your Shock spells grant "))
            {
                Regex r = new Regex("Your Shock spells grant (?<spellpower>\\d*) spell power for 6 sec.");
                Match m = r.Match(line);
                if (m.Success) // XXX Gladiators Totem of Survival
                {
                    float bonusSpellpower = (float)int.Parse(m.Groups["spellpower"].Value);
                    stats.AddSpecialEffect(new SpecialEffect(Trigger.ShamanShock, new Stats() { SpellPower = bonusSpellpower }, 6f, 0f));
                }
            }
            else if (line.StartsWith("Increases weapon damage when you use Stormstrike by "))
            {
                Regex r = new Regex("Increases weapon damage when you use Stormstrike by (?<damage>\\d*).");
                Match m = r.Match(line);
                if (m.Success) // Totem of Dancing Flame
                {
                    stats.BonusSSDamage += (float)int.Parse(m.Groups["damage"].Value);
                }
            }
            #region Rawr.RestoSham
            else if (line.StartsWith("Increases the base amount healed by your chain heal by "))
            {
                line = line.Replace(".", "");
                line = line.Substring("Increases the base amount healed by your chain heal by ".Length);
                stats.TotemCHBaseHeal = float.Parse(line); // Steamcaller's Totem & Totem of the Bay
            }
            else if (line.StartsWith("Reduces the mana cost of Healing Wave by "))
            {
                line = line.Replace(".", "");
                line = line.Substring("Reduces the mana cost of Healing Wave by ".Length);
                stats.TotemHWBaseCost = float.Parse(line); // Totem of Misery
            }
            else if (line.StartsWith("Reduces the base mana cost of Chain Heal by "))
            {
                line = line.Replace(".", "");
                line = line.Substring("Reduces the base mana cost of Chain Heal by ".Length);
                stats.TotemCHBaseCost = float.Parse(line); // Totem of Forest Growth (old, Totem of Healing Rains)
            }
            else if (line.StartsWith("Increases spell power of Healing Wave by "))
            {
                line = line.Replace(".", "");
                line = line.Substring("Increases spell power of Healing Wave by ".Length);
                stats.TotemHWSpellpower = float.Parse(line); // Totem of Spontaneous Regrowth
            }
            else if (line.StartsWith("Increases spell power of Lesser Healing Wave by "))
            {
                line = line.Replace(".", "");
                line = line.Substring("Increases spell power of Lesser Healing Wave by ".Length);
                stats.TotemLHWSpellpower = float.Parse(line); // Totem of the Plains, Possible Future totems
            }
            else if (line.StartsWith("Your Water Shield ability grants an additional "))
            {
                line = line.Replace(".", "");
                line = line.Substring("Your Water Shield ability grants an additional ".Length);
                stats.TotemThunderhead = 1f; // Totem of the Thunderhead, Possible Future totems
            }
            else if (line.StartsWith("Each time you cast Chain Heal, you have a chance to gain "))
            {
                line = line.Replace(".", "");
                line = line.Substring("Each time you cast Chain Heal, you have a chance to gain ".Length);
                stats.RestoShamRelicT9 = 234f; // T9 Resto Relic
            }
            else if (line.StartsWith("Your Riptide spell grants 85 spell power for 15 sec.  Stacks up to 3 times."))
            {
                line = line.Replace(".", "");
                line = line.Substring("Your Riptide spell grants 85 spell power for 15 sec.  Stacks up to 3 times.".Length);
                stats.RestoShamRelicT10 = 85f * 3f; // T9 Resto Relic
            }
            #endregion
            #endregion
            #region 3.2 Trinkets
            else if ((match = Regex.Match(line, @"Each time you hit with a melee or ranged attack, you have a chance to gain (?<amount>\d+) attack power for 10 sec.")).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalHit, new Stats() { AttackPower = int.Parse(match.Groups["amount"].Value) }, 10f, 45f, 0.2f));
            }
            else if ((match = Regex.Match(line.Replace("nbsp;", " "), @"When you deal damage you have a chance to gain Paragon, increasing your Strength or Agility by (?<amount>\d+) for 15 sec.  Your highest stat is always chosen.")).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageDone, new Stats() { Paragon = int.Parse(match.Groups["amount"].Value) }, 15f, 45f, 0.35f));
            }
            else if ((match = Regex.Match(line, @"Each time you cast a helpful spell, you have a chance to gain (?<amount>\d+) mana.")).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { ManaRestore = int.Parse(match.Groups["amount"].Value) }, 0f, 45f, 0.25f));
            }
            #endregion
            #region Icecrown Weapon Procs
            else if (line == "Your weapon swings have a chance to grant you Necrotic Touch for 10 sec, causing all your melee attacks to deal an additional 9% damage as shadow damage.")
            {
                // Black Bruise
                stats.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit, new Stats() { BonusPhysicalDamageMultiplier = .09f }, 10f, 0f, 0.01f));
            }
            #endregion
            #region 3.3 Trinkets
            else if (line == "Each time you deal melee or ranged damage to an opponent, you gain 17 attack power for the next 10 sec, stacking up to 20 times.")
            {
                // Herkuml War Token
                stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalHit, new Stats() { AttackPower = 17f }, 10f, 0f, 1f, 20));
            }
            else if ((match = Regex.Match(line, @"Each time you are struck by a melee attack, you have a 60% chance to gain (?<stamina>\d+) stamina for the next 10 sec, stacking up to 10 times\.")).Success)
            {
                // Unidentifiable Organ
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageTaken, new Stats() { Stamina = int.Parse(match.Groups["stamina"].Value) }, 10.0f, 0.0f, 0.6f, 10));
            }
            else if (line == "Your attacks have a chance to awaken the powers of the races of Northrend, temporarily transforming you and increasing your combat capabilities for 30 sec.")
            {
                // Deathbringer's Will
                stats.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit, new Stats() { DeathbringerProc = ilvl == 277 ? 700 : 600 }, 30f, 90f, 0.15f));
            }
            else if ((match = Regex.Match(line, @"When you deal damage you have a chance to gain (?<amount>\d+) attack power for 15 sec\.")).Success)
            {
                // Whispering Fanged Skull
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageDone, new Stats() { AttackPower = int.Parse(match.Groups["amount"].Value) }, 15f, 45f, 0.35f));
            }
            else if ((match = Regex.Match(line, @"Your melee attacks have a chance to grant you a Mote of Anger\. (nbsp;| )When you reach (?<amount>\d+) Motes of Anger, they will release, causing you to instantly attack for 50% weapon damage with one of your melee weapons\.")).Success)
            {
                // Tiny Abomination Jar
                stats.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit, new Stats() { MoteOfAnger = 0.5f }, 0f, 0f, 0.35f, int.Parse(match.Groups["amount"].Value)));
            }
            else if ((match = Regex.Match(line, @"You gain (?<mana>\d+) mana each time you heal a target with one of your spells.")).Success)
            {
                // Epheremal Snowflake - assume iCD of 0.25 sec.
                stats.AddSpecialEffect(new SpecialEffect(Trigger.HealingSpellHit, new Stats() { ManaRestore = int.Parse(match.Groups["mana"].Value) }, 0f, 0.25f, 1f));
            }
            else if ((match = new Regex(@"Your spell casts have a chance to grant (?<amount>\d\d*) mana per 5 sec for 15 sec.").Match(line)).Success)
            {
                // Purified Lunar Dust
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { Mp5 = (float)int.Parse(match.Groups["amount"].Value) }, 15f, 45f, 0.1f));
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
            */
                // Single Ability.
                Regex regex = new Regex(@"Your (?<ability>\w+\s*\w*) (ability )?(also )?grants (you )?(?<amount>\d*) (?<stat>\w+\s*\w*) for (?<duration>\d*) sec.");
                match = regex.Match(line);
                if (match.Success)
                {
                    string statName = match.Groups["stat"].Value;
                    float amount = int.Parse(match.Groups["amount"].Value);
                    float duration = int.Parse(match.Groups["duration"].Value);
                    string ability = match.Groups["ability"].Value;

                    stats.AddSpecialEffect(EvalRegex(statName, amount, duration, ability, 0f));
                }
                // ok... at this point with the way the code is written, this is really just for Sigil of the Unfaltering Knight, which grants a 
                // 30 sec buff of 53 Def. Since it's a triggered event, and all, it's better to go off the trigger rather than a straight 
                // 53 def all the time.  So adjusting this to be a special effect and assuming 30 secs.
                regex = new Regex(@"Your (?<ability>\w+\s*\w*) (ability )?(will )?(also )?increase (your )?(?<stat>\w+\s*\w*) by (?<amount>\d*).");
                match = regex.Match(line);
                if (match.Success)
                {
                    string statName = match.Groups["stat"].Value;
                    float amount = int.Parse(match.Groups["amount"].Value);
                    string ability = match.Groups["ability"].Value;

                    stats.AddSpecialEffect(EvalRegex(statName, amount, 30, ability, 0f));
                }

                regex = new Regex(@"Your (?<ability>\w+\s*\w*) and (?<ability2>\w+\s*\w*) (abilities )?have a chance to grant (?<amount>\d*) (?<stat>\w+[\s\w]*) for (?<duration>\d*) sec.");
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
                 * Equip: Your Obliterate, Scourge Strike, and Death Strike abilities grants 73 Strength for 15 sec.  Stacks up to 3 times.
                 * */
                regex = new Regex(@"Your (?<ability>\w+\s*\w*), (?<ability2>\w+\s*\w*), and (?<ability3>\w+\s*\w*) (abilities )?grants (?<amount>\d*) (?<stat>\w+[\s\w]*) for (?<duration>\d*) sec. nbsp;Stacks up to (?<stacks>\d*) times.");
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

                regex = new Regex(@"Your (?<ability>\w+\s*\w*), (?<ability2>\w+\s*\w*), and (?<ability3>\w+\s*\w*) (abilities )?grants (?<amount>\d*) (?<stat>\w+[\s\w]*) for (?<duration>\d*) sec. Stacks up to (?<stacks>\d*) times.");
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

                regex = new Regex(@"Each time you use your (?<ability3>\w+\s*\w*), (?<ability>\w+\s*\w*), or (?<ability2>\w+\s*\w*) ability, you have a chance to gain (?<amount>\d*) (?<stat>\w+[\s\w]*) for (?<duration>\d*) sec.");
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

                // Each time you use your [ability] ability, you have a chance to gain [amount] [stat] for [duration] sec.
                regex = new Regex(@"Each time you use your (?<ability>\w+\s*\w*) ability, you have a chance to gain (?<amount>\d*) (?<stat>\w+[\s\w]*) for (?<duration>\d*) sec.");
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
                regex = new Regex(@"Increases the damage dealt by your (?<ability>\w+\s*\w*) (ability )?by (?<amount>\d+[.]*\d*).");
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
                regex = new Regex(@"Increases the damage (dealt |done )?by your (?<ability>\w+\s*\w*) and (?<ability2>\w+\s*\w*) (abilities )?by (?<amount>\d+).");
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
                regex = new Regex(@"Increases the (base )?damage (dealt |done )?by your (?<ability>\w+\s*\w*) ability by (?<amount>\d+) and by your (?<ability2>\w+\s*\w*) ability by (?<amount2>\d+).");
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
                regex = new Regex(@"Increases the (base )?damage (dealt |done )?by your (?<ability>\w+\s*\w*) by (?<amount>\d+), your (?<ability2>\w+\s*\w*) by (?<amount2>\d+), and your (?<ability3>\w+\s*\w*) by (?<amount3>\d+).");
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

		public static void ProcessUseLine(string line, Stats stats, bool isArmory, int id) {
            Regex regex = new Regex(@"Increases (your )?(?<stat>\w\w*( \w\w*)*) by (?<amount>\+?\d\d*)(nbsp;\<small\>.*\<\/small\>)? for (?<duration>\d\d*) sec\. \((?<cooldown>\d\d*) Min Cooldown\)");
            Match match = regex.Match(line);
            if (match.Success)
            {
                Stats s = new Stats();
                string statName = match.Groups["stat"].Value;
                float amount = int.Parse(match.Groups["amount"].Value);
                float duration = int.Parse(match.Groups["duration"].Value);
                float cooldown = int.Parse(match.Groups["cooldown"].Value) * 60;

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
                else if (statName.Equals("armor penetration rating", StringComparison.InvariantCultureIgnoreCase)) { s.ArmorPenetrationRating = amount; }
                else if (statName.Equals("block value", StringComparison.InvariantCultureIgnoreCase)) { s.BlockValue = amount; }
                else if (statName.Equals("maximum health", StringComparison.InvariantCultureIgnoreCase)) { s.Health = amount; }
				else if (statName.Equals("armor", StringComparison.InvariantCultureIgnoreCase)) { s.BonusArmor = amount; }
				else if (statName.Equals("the block value of your shield", StringComparison.InvariantCultureIgnoreCase)) { s.BlockValue = amount; }

                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, s, duration, cooldown));
            }
            // Victor's Call and Vengeance of the Forsaken (232)
            else if (line.Contains("Each time you strike an enemy, you gain 215 attack power.  Stacks up to 5 times.  Entire effect lasts 20 sec."))
            {
                SpecialEffect primary = new SpecialEffect(Trigger.Use, new Stats(), 20f, 2f * 60f);
                SpecialEffect secondary = new SpecialEffect(Trigger.MeleeHit, new Stats() { AttackPower = 215f }, 20f, 0f, 1f, 5);
                primary.Stats.AddSpecialEffect(secondary);
                stats.AddSpecialEffect(primary);
            }
            // Victor's Call and Vengeance of the Forsaken (245)
            else if (line.Contains("Each time you strike an enemy, you gain 250 attack power.  Stacks up to 5 times.  Entire effect lasts 20 sec."))
            {
                SpecialEffect primary = new SpecialEffect(Trigger.Use, new Stats(), 20f, 2f * 60f);
                SpecialEffect secondary = new SpecialEffect(Trigger.MeleeHit, new Stats() { AttackPower = 250f }, 20f, 0f, 1f, 5);
                primary.Stats.AddSpecialEffect(secondary);
                stats.AddSpecialEffect(primary);
            }
            // Wrath: Eye of the Night
            else if (Regex.IsMatch(line, "Increases spell power by (\\d{2}) for all nearby party members."))
            {
                string[] inputs = Regex.Split(line, "Increases spell power by (\\d{2}) for all nearby party members.");
                // the full text reads: "Use: Increases spell power by 34 for all nearby party members.  Lasts 30 min. (1 Hour Cooldown)"
                // So in general you get the 34 spell power for 100% of a given boss fight.  Like most trinkets it is up to the player to
                // use it correctly, so we are going to give them the full power instead of half.
                float spell_power = float.Parse(inputs[1]);
                stats.SpellPower += spell_power;
            }
            // Increases spell power by 183 for 20 sec.
			else if (Regex.IsMatch(line, "Increases spell power by (\\d{3}) for (\\d{2}) sec."))
            {
				string[] inputs = Regex.Split(line, "Increases spell power by (\\d{3}) for (\\d{2}) sec.");
                float spell_power = float.Parse(inputs[1]);
                float uptime = float.Parse(inputs[2]);
                // unfortunately for us the cooldown is on the next line
                float cooldown_sec = 2.0f * 60.0f;
                // Vengeance of the Illidari (tooltip lies!)
                if (id == 28040) { cooldown_sec = 90.0f; }
                // we don't have a stat for this case so do the average
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { SpellPower = spell_power }, uptime, cooldown_sec));
			}
			// Increases spell power by 183 for 20 sec.
			else if (Regex.IsMatch(line, "Increases your spell power by (\\d{3}) for (\\d{2}) sec."))
			{
				string[] inputs = Regex.Split(line, "Increases your spell power by (\\d{3}) for (\\d{2}) sec.");
				float spell_power = float.Parse(inputs[1]);
				float uptime = float.Parse(inputs[2]);
				// unfortunately for us the cooldown is on the next line
				float cooldown_sec = 2.0f * 60.0f;
				// Twilight Serpent
				if (id == 42395) { cooldown_sec = 5.0f * 60.0f; }
				// Vengeance of the Illidari (tooltip lies!)
				if (id == 28040) { cooldown_sec = 90.0f; }
				// we don't have a stat for this case so do the average
				stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { SpellPower = spell_power }, uptime, cooldown_sec));
			}
			// Increases attack power by 183 for 20 sec.
			else if (Regex.IsMatch(line, "Increases armor penetration rating by (\\d{3}) for (\\d{2}) sec."))
			{
				string[] inputs = Regex.Split(line, "Increases armor penetration rating by (\\d{3}) for (\\d{2}) sec.");
				float armor_penetration = float.Parse(inputs[1]);
				float uptime = float.Parse(inputs[2]);
				// unfortunately for us the cooldown isn't listed, so guess 2min.
				stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { ArmorPenetrationRating = armor_penetration }, uptime, 120f));
			}
			// Increases attack power by 183 for 20 sec.
			else if (Regex.IsMatch(line, "Increases your armor penetration rating by (\\d{3}) for (\\d{2}) sec."))
			{
				string[] inputs = Regex.Split(line, "Increases your armor penetration rating by (\\d{3}) for (\\d{2}) sec.");
				float armor_penetration = float.Parse(inputs[1]);
				float uptime = float.Parse(inputs[2]);
				// unfortunately for us the cooldown isn't listed, so guess 2min.
				stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { ArmorPenetrationRating = armor_penetration }, uptime, 120f));
			}
			// Increases attack power by 183 for 20 sec.
			else if (Regex.IsMatch(line, "Increases attack power by (\\d{3}) for (\\d{2}) sec."))
			{
				string[] inputs = Regex.Split(line, "Increases attack power by (\\d{3}) for (\\d{2}) sec.");
				float attack_power = float.Parse(inputs[1]);
				float uptime = float.Parse(inputs[2]);
				// unfortunately for us the cooldown isn't listed, so guess 2min.
				stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { AttackPower = attack_power }, uptime, 120f));
			}
			// Increases attack power by 183 for 20 sec.
			else if (Regex.IsMatch(line, "Increases your attack power by (\\d{3}) for (\\d{2}) sec."))
			{
				string[] inputs = Regex.Split(line, "Increases your attack power by (\\d{3}) for (\\d{2}) sec.");
				float attack_power = float.Parse(inputs[1]);
				float uptime = float.Parse(inputs[2]);
				// unfortunately for us the cooldown isn't listed, so guess 2min.
				stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { AttackPower = attack_power }, uptime, 120f));
			}
			// Increases attack power by 183 for 20 sec.
			else if (Regex.IsMatch(line, "Increases attack power by (\\d{4}) for (\\d{2}) sec."))
			{
				string[] inputs = Regex.Split(line, "Increases attack power by (\\d{4}) for (\\d{2}) sec.");
				float attack_power = float.Parse(inputs[1]);
				float uptime = float.Parse(inputs[2]);
				// unfortunately for us the cooldown isn't listed, so guess 2min.
				stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { AttackPower = attack_power }, uptime, 120f));
			}
			// Increases attack power by 183 for 20 sec.
			else if (Regex.IsMatch(line, "Increases your attack power by (\\d{4}) for (\\d{2}) sec."))
			{
				string[] inputs = Regex.Split(line, "Increases your attack power by (\\d{4}) for (\\d{2}) sec.");
				float attack_power = float.Parse(inputs[1]);
				float uptime = float.Parse(inputs[2]);
				// unfortunately for us the cooldown isn't listed, so guess 2min.
				stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { AttackPower = attack_power }, uptime, 120f));
			}
			// Increases armor by 183 for 20 sec.
			else if (Regex.IsMatch(line, "Increases armor by (\\d{3}) for (\\d{2}) sec."))
			{
				string[] inputs = Regex.Split(line, "Increases armor by (\\d{3}) for (\\d{2}) sec.");
				float armor = float.Parse(inputs[1]);
				float uptime = float.Parse(inputs[2]);
				// unfortunately for us the cooldown isn't listed, so guess 2min.
				stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { BonusArmor = armor }, uptime, 120f));
			}
			// Increases armor by 183 for 20 sec.
			else if (Regex.IsMatch(line, "Increases your armor by (\\d{3}) for (\\d{2}) sec."))
			{
				string[] inputs = Regex.Split(line, "Increases your armor by (\\d{3}) for (\\d{2}) sec.");
				float armor = float.Parse(inputs[1]);
				float uptime = float.Parse(inputs[2]);
				// unfortunately for us the cooldown isn't listed, so guess 2min.
				stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { BonusArmor = armor }, uptime, 120f));
			}
			// Increases armor by 183 for 20 sec.
			else if (Regex.IsMatch(line, "Increases armor by (\\d{4}) for (\\d{2}) sec."))
			{
				string[] inputs = Regex.Split(line, "Increases armor by (\\d{4}) for (\\d{2}) sec.");
				float armor = float.Parse(inputs[1]);
				float uptime = float.Parse(inputs[2]);
				// unfortunately for us the cooldown isn't listed, so guess 2min.
				stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { BonusArmor = armor }, uptime, 120f));
			}
			// Increases armor by 183 for 20 sec.
			else if (Regex.IsMatch(line, "Increases your armor by (\\d{4}) for (\\d{2}) sec."))
			{
				string[] inputs = Regex.Split(line, "Increases your armor by (\\d{4}) for (\\d{2}) sec.");
				float armor = float.Parse(inputs[1]);
				float uptime = float.Parse(inputs[2]);
				// unfortunately for us the cooldown isn't listed, so guess 2min.
				stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { BonusArmor = armor }, uptime, 120f));
			}
			// Increases maximum health by 183 for 20 sec.
			else if (Regex.IsMatch(line, "Increases maximum health by (\\d{3}) for (\\d{2}) sec."))
			{
				string[] inputs = Regex.Split(line, "Increases maximum health by (\\d{3}) for (\\d{2}) sec.");
				float health = float.Parse(inputs[1]);
				float uptime = float.Parse(inputs[2]);
				// unfortunately for us the cooldown isn't listed, so guess 3min.
				stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { Health = health }, uptime, 180f));
			}
			// Increases maximum health by 183 for 20 sec.
			else if (Regex.IsMatch(line, "Increases your maximum health by (\\d{3}) for (\\d{2}) sec."))
			{
				string[] inputs = Regex.Split(line, "Increases your maximum health by (\\d{3}) for (\\d{2}) sec.");
				float health = float.Parse(inputs[1]);
				float uptime = float.Parse(inputs[2]);
				// unfortunately for us the cooldown isn't listed, so guess 3min.
				stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { Health = health }, uptime, 180f));
			}
			// Increases maximum health by 183 for 20 sec.
			else if (Regex.IsMatch(line, "Increases maximum health by (\\d{4}) for (\\d{2}) sec."))
			{
				string[] inputs = Regex.Split(line, "Increases maximum health by (\\d{4}) for (\\d{2}) sec.");
				float health = float.Parse(inputs[1]);
				float uptime = float.Parse(inputs[2]);
				// unfortunately for us the cooldown isn't listed, so guess 3min.
				stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { Health = health }, uptime, 180f));
			}
			// Increases maximum health by 183 for 20 sec.
			else if (Regex.IsMatch(line, "Increases your maximum health by (\\d{4}) for (\\d{2}) sec."))
			{
				string[] inputs = Regex.Split(line, "Increases your maximum health by (\\d{4}) for (\\d{2}) sec.");
				float health = float.Parse(inputs[1]);
				float uptime = float.Parse(inputs[2]);
				// unfortunately for us the cooldown isn't listed, so guess 3min.
				stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { Health = health }, uptime, 180f));
			}
			// Increases dodge rating by 183 for 20 sec.
			else if (Regex.IsMatch(line, "Increases dodge by (\\d{3}) for (\\d{2}) sec."))
			{
				string[] inputs = Regex.Split(line, "Increases dodge by (\\d{3}) for (\\d{2}) sec.");
				float dodge_rating = float.Parse(inputs[1]);
				float uptime = float.Parse(inputs[2]);
				float cooldown_sec = 120f;
				if (id == 44063) { cooldown_sec = 60.0f; } //Figurine - Monarch Crab
				// unfortunately for us the cooldown isn't listed, so guess 2min.
				stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { DodgeRating = dodge_rating }, uptime, cooldown_sec));
			}
			// Increases dodge rating by 183 for 20 sec.
			else if (Regex.IsMatch(line, "Increases dodge rating by (\\d{3}) for (\\d{2}) sec."))
			{
				string[] inputs = Regex.Split(line, "Increases dodge rating by (\\d{3}) for (\\d{2}) sec.");
				float dodge_rating = float.Parse(inputs[1]);
				float uptime = float.Parse(inputs[2]);
				float cooldown_sec = 120f;
				if (id == 44063) { cooldown_sec = 60.0f; } //Figurine - Monarch Crab
				// unfortunately for us the cooldown isn't listed, so guess 2min.
				stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { DodgeRating = dodge_rating }, uptime, cooldown_sec));
			}
			// Increases dodge rating by 183 for 20 sec.
			else if (Regex.IsMatch(line, "Increases your dodge rating by (\\d{3}) for (\\d{2}) sec."))
			{
				string[] inputs = Regex.Split(line, "Increases your dodge rating by (\\d{3}) for (\\d{2}) sec.");
				float dodge_rating = float.Parse(inputs[1]);
				float uptime = float.Parse(inputs[2]);
				float cooldown_sec = 120f;
				if (id == 44063) { cooldown_sec = 60.0f; } //Figurine - Monarch Crab
				// unfortunately for us the cooldown isn't listed, so guess 2min.
				stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { DodgeRating = dodge_rating }, uptime, cooldown_sec));
			}
			// Increases parry rating by 183 for 20 sec.
			else if (Regex.IsMatch(line, "Increases parry rating by (\\d{3}) for (\\d{2}) sec."))
			{
				string[] inputs = Regex.Split(line, "Increases parry rating by (\\d{3}) for (\\d{2}) sec.");
				float parry_rating = float.Parse(inputs[1]);
				float uptime = float.Parse(inputs[2]);
				// unfortunately for us the cooldown isn't listed, so guess 2min.
				stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { ParryRating = parry_rating }, uptime, 120f));
			}
			// Increases parry rating by 183 for 20 sec.
			else if (Regex.IsMatch(line, "Increases your parry rating by (\\d{3}) for (\\d{2}) sec."))
			{
				string[] inputs = Regex.Split(line, "Increases your parry rating by (\\d{3}) for (\\d{2}) sec.");
				float parry_rating = float.Parse(inputs[1]);
				float uptime = float.Parse(inputs[2]);
				// unfortunately for us the cooldown isn't listed, so guess 2min.
				stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { ParryRating = parry_rating }, uptime, 120f));
			}
			// Increases haste rating by 183 for 20 sec.
			else if (Regex.IsMatch(line, "Increases haste rating by (\\d{3}) for (\\d{2}) sec."))
			{
				string[] inputs = Regex.Split(line, "Increases haste rating by (\\d{3}) for (\\d{2}) sec.");
				float haste_rating = float.Parse(inputs[1]);
				float uptime = float.Parse(inputs[2]);
				// unfortunately for us the cooldown isn't listed, so guess 2min.
				stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { HasteRating = haste_rating }, uptime, 120f));
			}
			// Increases haste rating by 183 for 20 sec.
			else if (Regex.IsMatch(line, "Increases your haste rating by (\\d{3}) for (\\d{2}) sec."))
			{
				string[] inputs = Regex.Split(line, "Increases your haste rating by (\\d{3}) for (\\d{2}) sec.");
				float haste_rating = float.Parse(inputs[1]);
				float uptime = float.Parse(inputs[2]);
				// unfortunately for us the cooldown isn't listed, so guess 2min.
				stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { HasteRating = haste_rating }, uptime, 120f));
			}
			// Increases haste rating by 183 for 20 sec.
			else if (Regex.IsMatch(line, "Increases the block value of your shield by (\\d{3}) for (\\d{2}) sec."))
			{
				string[] inputs = Regex.Split(line, "Increases the block value of your shield by (\\d{3}) for (\\d{2}) sec.");
				float block = float.Parse(inputs[1]);
				float uptime = float.Parse(inputs[2]);
				// unfortunately for us the cooldown isn't listed, so guess 2min.
				stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { BlockValue = block }, uptime, 120f));
			}
			// Increases haste rating by 183 for 20 sec.
			else if (Regex.IsMatch(line, "Instantly heal your current friendly target for (\\d{4})."))
			{
				string[] inputs = Regex.Split(line, "Instantly heal your current friendly target for (\\d{4}).");
				float heal = float.Parse(inputs[1]);
				// unfortunately for us the cooldown isn't listed, so guess 2min.
				stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { Healed = heal }, 0, 60f));
			}
            else if (line.StartsWith("Increases your Spirit by "))
            {
                // Bangle of Endless Blessings, Earring of Soulful Meditation
                Regex r = new Regex("Increases your Spirit by \\+?(?<spi>\\d*) for (?<dur>\\d*) sec\\."); // \\(2 Min Cooldown\\)");
                Match m = r.Match(line);
                if (m.Success)
                {
                    int spi = int.Parse(m.Groups["spi"].Value);
                    int dur = int.Parse(m.Groups["dur"].Value);
                    stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { Spirit = spi }, dur, 120f));
				}
			}
            else if (line.StartsWith("Increases speed by "))
            {
                // Figurine - Ruby Hare : "Increases speed by 30% for 6 sec. (3 Min Cooldown)"
                Regex r = new Regex("Increases speed by (?<speed>\\d*)% for (?<dur>\\d*) sec\\."); // \\((?<cd>\\d*) Min Cooldown\\)");
                Match m = r.Match(line);
                if (m.Success)
                {
                    int speed = int.Parse(m.Groups["speed"].Value);
                    int dur = int.Parse(m.Groups["dur"].Value);
                    // Test again with the cd... Available only on wowhead.
                    float cd;
                    Regex rCD = new Regex("Increases speed by (?<speed>\\d*)% for (?<dur>\\d*) sec\\. \\((?<cd>\\d*) Min Cooldown\\)");
                    Match mCD = rCD.Match(line);
                    if (mCD.Success)
                    {
                        cd = int.Parse(mCD.Groups["cd"].Value) * 60.0f;
                    }
                    else
                    {
                        cd = 3.0f * 60.0f;
                    }
                    stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { MovementSpeed = speed/100f }, dur, cd));
                }
            }
            else if (line.StartsWith("Increases maximum health by "))
            {
                // Increases maximum health by 3385 for 15 sec. Shares cooldown with other Battlemaster's trinkets. (3 Min Cooldown)
				Regex r = new Regex("Increases maximum health by (?<health>\\d*) for (?<dur>\\d*) sec\\. Shares cooldown with (other )?Battlemaster\\'s trinkets\\."); // \\((?<cd>\\d*) Min Cooldown\\)");
                Match m = r.Match(line);
				if (m.Success)
                {
                    int health = int.Parse(m.Groups["health"].Value);
                    int dur = int.Parse(m.Groups["dur"].Value);
                    // Test again with the cd... Available only on wowhead.
                    float cd;
                    Regex rCD = new Regex("Increases maximum health by (?<health>\\d*) for (?<dur>\\d*) sec\\. Shares cooldown with other Battlemaster\\'s trinkets\\. \\((?<cd>\\d*) Min Cooldown\\)");
                    Match mCD = rCD.Match(line);
                    if (mCD.Success)
                    {
                        cd = int.Parse(mCD.Groups["cd"].Value) * 60.0f;
                    }
                    else
                    {
                        cd = 3.0f * 60.0f;
                    }
                    stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { Health = health }, dur, cd));
                }
            }
            else if (Regex.IsMatch(line, "When the shield is removed by any means, you regain (\\d{4}) mana."))
            {
                string[] inputs = Regex.Split(line, "When the shield is removed by any means, you regain (\\d{4}) mana.");
                float restore = float.Parse(inputs[1]);
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { ManaRestore = restore }, 0f, 5 * 60f));
            }
            else if (line.StartsWith("For the next 20 sec, your direct heals increase healing received by their target by up to 58."))
            {
                // Talisman of Troll Divinity
                // For 20 seconds, direct healing adds a stack of 58 +healing for 10 seconds
                // Stacks 5 times, 2 minute cd
                // Direct heals: Nourish (1.5) HT (3) Regrowth (2)
                // Assumption: every 2 seconds, a direct healing spell is cast, so after 5 casts, full effect
                // That would mean: 10 seconds ramping up, then 20 seconds having the effect (assuming the stack is refreshed)
                // Average stack of 4 (24/30 * 5)
                // But remember that the spellpower will increase for others in the raid too!
                // stats.BonusHealingReceived = 58 * 4;
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { BonusHealingReceived = 58 * 4f }, 30, 120));
            }
            else if (line.StartsWith("Your heals each cost "))
            {
                // Lower City Prayerbook
                Regex r = new Regex("Your heals each cost (?<mana>\\d*) less mana for the next 15 sec.");
                Match m = r.Match(line);
                if (m.Success)
                {
                    stats.ManacostReduceWithin15OnUse1Min += (float)int.Parse(m.Groups["mana"].Value);
                }
            }
            else if (line.StartsWith("Gain 250 mana each sec. for "))
            {
                //stats.ManaregenFor8SecOnUse5Min += 250;
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { Mp5 = 250 * 5 }, 8.0f, 300.0f));
            }
            else if (line.StartsWith("Conjures a Power Circle lasting for 15 sec.  While standing in this circle, the caster gains 320 spell power."))
            {
                // Shifting Naaru Sliver
				stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { SpellPower = 320 }, 15f, 90.0f));
            }
            else if ((match = Regex.Match(line, @"Each spell cast within 20 seconds will grant a stacking bonus of (?<mp5>\d+) mana regen per 5 sec. Expires after (?<duration>\d+) seconds.")).Success)
            {
                int mp5 = int.Parse(match.Groups["mp5"].Value);
                // Meteorite Crystal and Pendant of the Violet Eye
                // Estimate cast as every 2.0f seconds, average stack height is 0.5f of final value
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { Mp5 = mp5 * int.Parse(match.Groups["duration"].Value) * 0.5f / 2.0f }, int.Parse(match.Groups["duration"].Value), 120));
            }
            else if ((match = Regex.Match(line, @"Each time you cast a helpful spell, you gain (?<amount>\d+) spell power.  Stacks up to (?<stacks>\d+) times.  Entire effect lasts (?<duration>\d+) sec.")).Success)
            {   // Binding Light / Stone
                SpecialEffect primary = new SpecialEffect(Trigger.Use, new Stats(), (float)int.Parse(match.Groups["duration"].Value), 2f * 60f);
                SpecialEffect secondary = new SpecialEffect(Trigger.HealingSpellCast, new Stats() { SpellPower = (float)int.Parse(match.Groups["amount"].Value) }, (float)int.Parse(match.Groups["duration"].Value), 0f, 1f, int.Parse(match.Groups["stacks"].Value));
                primary.Stats.AddSpecialEffect(secondary);
                stats.AddSpecialEffect(primary);
            }
            else if ((match = Regex.Match(line, @"Each time you cast a helpful spell, you gain (?<amount>\d+) spell power. nbsp;Stacks up to (?<stacks>\d+) times. nbsp;Entire effect lasts (?<duration>\d+) sec.")).Success)
            {   // Binding Light / Stone
                SpecialEffect primary = new SpecialEffect(Trigger.Use, new Stats(), (float)int.Parse(match.Groups["duration"].Value), 2f * 60f);
                SpecialEffect secondary = new SpecialEffect(Trigger.HealingSpellCast, new Stats() { SpellPower = (float)int.Parse(match.Groups["amount"].Value) }, (float)int.Parse(match.Groups["duration"].Value), 0f, 1f, int.Parse(match.Groups["stacks"].Value));
                primary.Stats.AddSpecialEffect(secondary);
                stats.AddSpecialEffect(primary);
            }
            else if ((match = Regex.Match(line, @"Each time you cast a harmful spell, you gain (?<hasteRating>\d+) haste rating\. .*Stacks up to (?<stacks>\d+) times\. .*Entire effect lasts (?<duration>\d+) sec\.( \((?<cooldown>\d+) Min Cooldown\))?")).Success)
            {
                // Fetish of Volatile Power
                // armory doesn't give cooldown info
                int cooldown = 120;
                if (match.Groups["cooldown"].Success)
                {
                    cooldown = int.Parse(match.Groups["cooldown"].Value) * 60;
                }
                Stats childEffect = new Stats();
                childEffect.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { HasteRating = int.Parse(match.Groups["hasteRating"].Value) }, float.PositiveInfinity, 0f, 1f, int.Parse(match.Groups["stacks"].Value)));
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, childEffect, int.Parse(match.Groups["duration"].Value), cooldown, 1f));
            }
            else if ((match = Regex.Match(line, @"Each time you are struck by an attack, you gain (?<bonusArmor>\d+) armor\. .*Stacks up to (?<stacks>\d+) times\. .*Entire effect lasts (?<duration>\d+) sec\.( \((?<cooldown>\d+) Min Cooldown\))?")).Success)
            {
                // Fervor of the Frostborn / Eitrigg's Oath
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
            // Figurine - Talasite Owl, 5 min cooldown
            else if (line.StartsWith("Restores 900 mana over 12 sec."))
            {
				//if (stats.Mp5 == 18) // Figurine - Seaspray Albatross, 3 min cooldown
				//    stats.ManaregenOver12SecOnUse3Min += 900;
				//else if (stats.Mp5 == 14) // Figurine - Talasite Owl, 5 min cooldown
				//    stats.ManaregenOver12SecOnUse5Min += 900;
            }
            else if ((match = new Regex(@"Increases the block value of your shield by (?<amount>\d\d*) for 20 sec.").Match(line)).Success)
            {
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { BlockValue = (float)int.Parse(match.Groups["amount"].Value) }, 20.0f, 120.0f));
            }
			else if (line.StartsWith("Your heals each cost "))
			{
				// Lower City Prayerbook
				Regex r = new Regex("Your heals each cost (?<mana>\\d*) less mana for the next 15 sec.");
				Match m = r.Match(line);
				if (m.Success)
				{
					stats.ManacostReduceWithin15OnUse1Min += (float)int.Parse(m.Groups["mana"].Value);
				}
			}
			else if (line.StartsWith("Tap into the power of the skull, increasing haste rating by 175 for 20 sec."))
			{
				// The Skull of Gul'dan
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { HasteRating = 175 }, 20.0f, 120.0f));
			}
			else if (line.StartsWith("Each spell cast within 20 seconds will grant a stacking bonus of 21 mana regen per 5 sec. Expires after 20 seconds.  Abilities with no mana cost will not trigger this trinket."))
			{
				//stats.Mp5OnCastFor20SecOnUse2Min += 21;
			}
			// Mind Quickening Gem
			else if (line.StartsWith("Quickens the mind, increasing the Mage's haste rating by 330 for 20 sec."))
			{
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { HasteRating = 330 }, 20, 300));
			}
//			else if (line.StartsWith("Increases the block value of your shield by 200 for 20 sec."))
//			{
//				stats.BlockValue += (float)Math.Round(200f * (20f / 120f));
//			}
			else if (line.StartsWith("Removes all movement impairing effects and all effects which cause loss of control of your character."))
			{
				stats.PVPTrinket += 1f;
			}
            else if (line.StartsWith("Increases the damage dealt by your Scourge Strike and Obliterate abilities by 420."))
            {
                stats.BonusObliterateDamage += 420;
                stats.BonusScourgeStrikeDamage += 420;
            }
            else if (line.StartsWith("Restores 2340 mana over 12 sec."))
            {
                // Figurine - Sapphire Owl
                //stats.ManaRestore5min = 2340;
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { ManaRestore = 2340 }, 0f, 300f));
            }
            else if (line == "Instantly heal your current friendly target for 2710. (1 Min Cooldown)")
            {
                // Living Ice Crystals
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { Healed = 2710 }, 0f, 60f));
            }
            else if ((match = new Regex(@"Grants (?<amount>\d\d*) haste rating for 20 sec.").Match(line)).Success) {
                // Ephemeral Snowflake
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { HasteRating = (float)int.Parse(match.Groups["amount"].Value) }, 20f, 120f));
            }
            else if (line.StartsWith("Restores 1625 mana"))
            {
                // Sliver of Pure Ice
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { ManaRestore = 1625 }, 0f, 120f));
            }
            else if ((match = Regex.Match(line, @"Every time one of your non-periodic spells deals a critical strike, the bonus is reduced by 184 critical strike rating.")).Success)
            {
                // Nevermelting Ice Crystal - Yes when armory is fixed this needs update
                Stats childEffect = new Stats();
                childEffect.CritRating = 920f;
                childEffect.AddSpecialEffect(new SpecialEffect(Trigger.SpellCrit, new Stats() { CritRating =-184f }, float.PositiveInfinity, 0f, 1f, 5));
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, childEffect, 20f, 180f, 1f));
            }
        }

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
            else if (statName.Equals("armor penetration rating", StringComparison.InvariantCultureIgnoreCase)) { s.ArmorPenetrationRating = amount; }
            else if (statName.Equals("block value", StringComparison.InvariantCultureIgnoreCase)) { s.BlockValue = amount; }
            else if (statName.Equals("critical strike rating", StringComparison.InvariantCultureIgnoreCase)) { s.CritRating = amount; }
            else if (statName.Equals("defense rating", StringComparison.InvariantCultureIgnoreCase)) { s.DefenseRating = amount; }
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
            else if (statName.Equals("armor penetration rating", StringComparison.InvariantCultureIgnoreCase)) { s.ArmorPenetrationRating = amount; }
            else if (statName.Equals("block value", StringComparison.InvariantCultureIgnoreCase)) { s.BlockValue = amount; }
            else if (statName.Equals("critical strike rating", StringComparison.InvariantCultureIgnoreCase)) { s.CritRating = amount; }
            else if (statName.Equals("defense rating", StringComparison.InvariantCultureIgnoreCase)) { s.DefenseRating = amount; }
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
            else if (statName.Equals("armor penetration rating", StringComparison.InvariantCultureIgnoreCase)) { s.ArmorPenetrationRating = amount; }
            else if (statName.Equals("block value", StringComparison.InvariantCultureIgnoreCase)) { s.BlockValue = amount; }
            else if (statName.Equals("critical strike rating", StringComparison.InvariantCultureIgnoreCase)) { s.CritRating = amount; }
            else if (statName.Equals("defense rating", StringComparison.InvariantCultureIgnoreCase)) { s.DefenseRating = amount; }
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
        /// <param name="amount">The amount by which the bonSus should be applied.</param>
        public static void EvalAbility(string ability, Stats s, float amount) {
            switch (ability) {
                case "Blood Strike"  : { s.BonusBloodStrikeDamage   += amount; break; }
                case "Heart Strike"  : { s.BonusHeartStrikeDamage   += amount; break; }
                case "Death Coil"    : { s.BonusDeathCoilDamage     += amount; break; }
                case "Frost Strike"  : { s.BonusFrostStrikeDamage   += amount; break; }
                case "Obliterate"    : { s.BonusObliterateDamage    += amount; break; }
                case "Scourge Strike": { s.BonusScourgeStrikeDamage += amount; break; }
                case "Death Strike"  : { s.BonusDeathStrikeDamage   += amount; break; }
                case "Icy Touch"     : { s.BonusIcyTouchDamage      += amount; break; }
                default: { break; } // Error.
            }
        }
    }
}
