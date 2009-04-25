using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Rawr
{
	public static class SpecialEffects
	{
        public static void ProcessMetaGem(string line, Stats stats, bool bisArmory)
        {
            List<string> gemBonuses = new List<string>();
			string[] gemBonusStrings = line.Split(new string[] { " and ", " & ", ", " }, StringSplitOptions.None);
			foreach (string gemBonusString in gemBonusStrings)
			{
                if (gemBonusString.IndexOf('+') != gemBonusString.LastIndexOf('+'))
                {
                    gemBonuses.Add(gemBonusString.Substring(0, gemBonusString.IndexOf(" +")));
                    gemBonuses.Add(gemBonusString.Substring(gemBonusString.IndexOf(" +") + 1));
                }
                else
                    gemBonuses.Add(gemBonusString);
			}
            foreach (string gemBonus in gemBonuses)
            {
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
                    stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { ManaRestore = 600 }, 1f, 15f, .05f));
                    stats.ManaRestoreOnCast_5_15 = 600; // IED
				}
				else if (gemBonus == "2% Increased Armor Value from Items")
				{
					stats.BaseArmorMultiplier = 0.02f; // IED
				}
                else if (gemBonus == "Chance on spellcast - next spell cast in half time" || gemBonus == "Chance to Increase Spell Cast Speed")
                {
                    stats.SpellHasteFor6SecOnCast_15_45 = 320; // MSD changed in 2.4
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
				else if (gemBonus == "3% Increased Critical Damage")
				{
					stats.BonusCritMultiplier = 0.03f;
					stats.BonusSpellCritMultiplier = 0.03f;
				}
                else if (gemBonus == "3% Increased Critical Healing Effect")
                {
                    stats.BonusCritHealMultiplier = 0.03f;
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

		public static void ProcessEquipLine(string line, Stats stats, bool isArmory)
		{
            Match match;
			if (line.StartsWith("Increases initial and per application periodic damage done by Lacerate by "))
			{
				//stats.BonusLacerateDamage = float.Parse(line.Substring("Increases initial and per application periodic damage done by Lacerate by ".Length));
			}
            else if (line.StartsWith("Your harmful spells have a chance to increase your spell power by 522 for 10 sec."))
            {
                // special case because of wrong description
                // Elemental Focus Stone
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageSpellCast, new Stats() { HasteRating = 522 }, 10f, 45f, 0.1f));
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
			else if (line.StartsWith("Chance on hit to increase your attack power by 230"))
			{ //Special handling for Shard of Contempt due to higher uptime
				// stats.AttackPower += 90f;
                stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalHit, new Stats() { AttackPower = 230f }, 20f, 45f, .1f));
			}
			else if (line.StartsWith("When you heal or deal damage you have a chance to gain Greatness"))
			{ //Darkmoon Card: Greatness
                stats.GreatnessProc = 300;
                stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalHit, new Stats() { HighestStat = 300f }, 15f, 45f, .33f));
                stats.AddSpecialEffect(new SpecialEffect(Trigger.HealingSpellHit, new Stats() { HighestStat = 300f }, 15f, 45f, .33f));
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageSpellHit, new Stats() { HighestStat = 300f }, 15f, 45f, .33f));
			}
            else if (line.StartsWith("Each time you deal melee or ranged damage to an opponent, you gain 16 attack power for the next 10 sec., stacking up to 20 times."))
            {
                // Fury of the Five Flights
                stats.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit, new Stats() { AttackPower = 16f }, 10f, 0f, 1f, 20));
                //stats.AttackPower += 320; //Fury of the Five Flights = 320ap
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
            else if (line.StartsWith("Your spells have a chance to increase your spell power by 850 for 10 sec."))
            {
                // Pandora's Plea
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { SpellPower = 850f }, 10f, 45f, .1f));
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
                stats.HasteRatingOnPhysicalAttack += int.Parse(match.Groups["amount"].Value) * 10f / 45f;
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
            else if (line.StartsWith("Your Mangle ability has a chance to grant "))
            {
                line = line.Substring("Your Mangle ability has a chance to grant ".Length);
                if (line.Contains(".")) line = line.Substring(0, line.IndexOf("."));
                if (line.Contains(" ")) line = line.Substring(0, line.IndexOf(" "));
                stats.TerrorProc += int.Parse(line);
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
            else if (line.StartsWith("You gain an Electrical Charge each time you cause a damaging spell critical strike."))
            {
                stats.LightningCapacitorProc = 1;
            }
            else if (line.StartsWith("You gain a Thunder Charge each time you cause a damaging spell critical strike."))
            {
                // Thunder Capacitor
                stats.ThunderCapacitorProc = 1;
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
                stats.SpellPowerFor10SecOnResist += 170;
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellMiss, new Stats() { SpellPower = 170 }, 10, 0));
            }
            else if (line.StartsWith("Your spell critical strikes have a 50% chance to grant you 145 spell haste rating for 5 sec."))
            {
                stats.SpellHasteFor5SecOnCrit_50 += 145;
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCrit, new Stats() { HasteRating = 145 }, 5, 0, 0.5f));
            }
            else if (line.StartsWith("Your harmful spells have a chance to increase your spell haste rating by 320 for 6 secs."))
            {
                stats.SpellHasteFor6SecOnHit_10_45 += 320;
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageSpellHit, new Stats() { HasteRating = 320 }, 6, 45, 0.1f));
            }
            else if (line.StartsWith("Your spell casts have a chance to increase your spell power by 590 for 10 sec."))
            {
                // Flow of Knowledge
                stats.SpellPowerFor10SecOnCast_10_45 += 590;
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { SpellPower = 590 }, 10, 45, 0.1f));
            }
            else if (line.StartsWith("Chance on spell critical hit to increase your spell power by 225 for 10 secs."))
            {
                // Shiffar's Nexus-Horn
                stats.SpellPowerFor10SecOnCrit_20_45 += 225;
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
                stats.SpellPowerFor15SecOnCrit_20_45 += value;
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
                stats.SpellDamageFor10SecOnHit_5 += 132;
            }
            else if (line == "Your melee and ranged attacks have a chance to strike your enemy, dealing 1504 to 2256 arcane damage.")
            {   //Bandit's Insignia
                stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalHit, new Stats() { ArcaneDamage = 1880 }, 1f, 45f, 0.15f));
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
                stats.SpellPowerFor10SecOnHit_10_45 += value;
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
                stats.SpellPowerFor10SecOnHit_10_45 += value;
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
                line = line.Substring(0, line.IndexOf(" spell power for 10 sec."));
                stats.UnseenMoonDamageBonus += float.Parse(line, System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (line.StartsWith("Increases the spell power of your Insect Swarm by "))
            {
                line = line.Substring("Increases the spell power of your Insect Swarm by ".Length);
                line = line.Replace(".", "");
                stats.InsectSwarmDmg += float.Parse(line, System.Globalization.CultureInfo.InvariantCulture);
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
                stats.BangleProc += (float)int.Parse(line);
            }
            else if (line.StartsWith("2% chance on successful spellcast to allow 100% of your Mana regeneration to continue while casting for 15 sec."))
            {
                // Darkmoon Card: Blue Dragon
                stats.FullManaRegenFor15SecOnSpellcast += 2f;
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
                //TODO: Don't count this before talents since it's a buff.
                stats.AverageArmor += (float)Math.Round(value / 6f);
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
            }
            else if (line.StartsWith("Your healing spells have a chance to make your next heal cast within 15 sec cost 800 less mana."))
            {
                // Soul Preserver
                stats.ManacostReduceWithin15OnHealingCast += 800;
            }
            else if (line.StartsWith("Your damaging and healing spells have a chance to increase your spell power by 512 for 10 sec."))
            {
                // Forge Ember
                stats.SpellPowerFor10SecOnHit_10_45 += 512;
                // This is a nasty trick for compatibility = when designing a healer, please use this version:
                stats.SpellPowerFor10SecOnHeal_10_45 += 512;
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellHit, new Stats() { SpellPower = 512 }, 10, 45, 0.1f));
            }
            else if (line.StartsWith("Your harmful spells have a chance to increase your spell power by 590 for 10 sec."))
            {
                // Sundial of the Exiled (NOT FOR HEALERS)
                stats.SpellPowerFor10SecOnHit_10_45 += 590;
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageSpellHit, new Stats() { SpellPower = 590 }, 10, 45, 0.1f));
            }
            else if (line.StartsWith("Your spells have a chance to increase your spell power by 765 for 10 sec."))
            {
                // Dying Curse
                stats.SpellPowerFor10SecOnCast_15_45 += 765;
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { SpellPower = 765 }, 10, 45, 0.15f));
            }
            else if (line.StartsWith("Each time you cast a damaging or healing spell, there is chance you will gain up to 176 mana per 5 for 15 sec."))
            {
                // Spark of Life
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { Mp5 = 176f }, 15f, 45f, .10f));
                stats.ManaRestoreOnCast_10_45 += 176 * 3;
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
            }
            else if (line.StartsWith("Your spells have a chance to increase your haste rating by 505 for 10 secs."))
            {
                // Embrace of the Spider
                stats.SpellHasteFor10SecOnCast_10_45 += 505;
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { HasteRating = 505 }, 10, 45, 0.1f));
            }
            else if (line.StartsWith("Your direct healing and heal over time spells have a chance to increase your haste rating by 505 for 10 secs."))
            {
                // The Egg of Mortal Essence
                stats.SpellHasteFor10SecOnHeal_10_45 += 505;
                stats.AddSpecialEffect(new SpecialEffect(Trigger.HealingSpellCast, new Stats() { HasteRating = 505f }, 10f, 45f, .1f));
            }
            else if (line.StartsWith("Your spell critical strikes have a chance to restore 900 mana."))
            {
                // Soul of the Dead
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCrit, new Stats() { ManaRestore = 900f }, 1f, 45f, .25f));
                stats.ManaRestoreOnCrit_25_45 += 900f;
            }
            else if (line.StartsWith("Your harmful spells have a chance to strike your enemy, dealing 1168 to 1752 shadow damage."))
            {
                // Pendulum of Telluric Currents
                stats.PendulumOfTelluricCurrentsProc += 1;
                stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellHit, new Stats() { ShadowDamage = 1460 }, 1f, 45f, .1f));
            }
            else if (line.StartsWith("Each time one of your spells deals periodic damage, there is a chance 788 to 1312 additional damage will be dealt."))
            {
                // Extract of Necromantic Power
                stats.ExtractOfNecromanticPowerProc += 1;
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DoTTick, new Stats() { ShadowDamage = 1050f }, 1f, 45f, .1f));
            }
            else if (line.StartsWith("Each time you deal damage, you have a chance to do an additional 1750 to 2250 Shadow damage."))
            {
                // Darkmoon Card: Death
                stats.DarkmoonCardDeathProc += 1;
                stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageDone, new Stats() { ShadowDamage = 2000f }, 1f, 45f, .35f));
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
                    stats.AddSpecialEffect(new SpecialEffect(Trigger.HealingSpellCast, new Stats() { Healed = hot }, 1f, 45f, .2f));
                    stats.BonusHoTOnDirectHeals += hot / 60f;
                }
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
                }
            }
            else if (line.StartsWith("Increases the damage dealt by your Lava Burst by "))
            {
                line = line.Replace(".", "");
                line = line.Substring("Increases the damage dealt by your Lava Burst by ".Length);
                stats.LavaBurstBonus = float.Parse(line);
            }
            else if ((match = new Regex("Your Crusader Strike ability also grants you (?<amount>\\d\\d*) attack power for 10 sec.").Match(line)).Success)
            {
                stats.APCrusaderStrike_10 += (float)int.Parse(match.Groups["amount"].Value);
            }
            else if (line == "Increases the spell power of your Consecration spell by 141.")
            {
                stats.ConsecrationSpellPower = 141f;
            }
            else if (line == "Increases the damage dealt by Crusader Strike by 115.5.")
            {
                stats.CrusaderStrikeDamage = 115.5f;
            }
            else if (line == "Increases the damage dealt by Crusader Strike by 116.")
            {
                stats.CrusaderStrikeDamage = 115.5f;
            }
            else if (line == "Increases the damage done by Divine Storm by 235.")
            {
                stats.DivineStormDamage = 235f;
            }
            else if (line == "Causes your Divine Storm to increase your Critical Strike rating by 73 for 8 sec.")
            {
                stats.DivineStormDamage = 81;
            }
            else if (line == "Causes your Judgements to increase your Critical Strike Rating by 61 for 5 sec.")
            {
                stats.CritJudgement_5 = 61f;
            }
            else if (line == "Increases the damage dealt by your Crusader Strike ability by 5%.")
            {
                stats.CrusaderStrikeMultiplier = .05f;
            }
            #region Added by Rawr.Enhance
            else if (line == "Your Shock spells have a chance to grant 110 attack power for 10 sec.")
            {
                stats.TotemShockAttackPower += 110f * 10f / 45f; // Stonebreaker's Totem
            }
            else if (line == "Your Storm Strike ability also grants you 60 haste rating for 6 sec.")
            {
                stats.TotemSSHaste += 60f; // Totem of Dueling
            }
            else if (line == "Increases the attack power bonus on Windfury Weapon attacks by ")
            {
                line = line.Replace(".", "");
                line = line.Substring("Increases the attack power bonus on Windfury Weapon attacks by ".Length);
                stats.TotemWFAttackPower = float.Parse(line); // Totem of Astral Winds & Totem of Splintering
            }
            else if (line.StartsWith("Your Lava Lash ability also grants you "))
            {
                Regex r = new Regex("Your Lava Lash ability also grants you (?<attackpower>\\d*) attack power for 6 sec.");
                Match m = r.Match(line);
                if (m.Success) // XXX Gladiators Totem of Indomitability
                {
                    stats.TotemLLAttackPower += (float)int.Parse(m.Groups["attackpower"].Value);
                }
            }
            else if (line.StartsWith("Your Shock spells grant "))
            {
                Regex r = new Regex("Your Shock spells grant (?<spellpower>\\d*) spell power for 6 sec.");
                Match m = r.Match(line);
                if (m.Success) // XXX Gladiators Totem of Survival
                {
                    stats.TotemShockSpellPower += (float)int.Parse(m.Groups["spellpower"].Value);
                }
            }
            else if (line.StartsWith("Increases weapon damage when you use Stormstrike by "))
            {
                Regex r = new Regex("Increases weapon damage when you use Stormstrike by (?<damage>\\d*).");
                Match m = r.Match(line);
                if (m.Success) // Totem of Dancing Flame
                {
                    stats.TotemSSDamage += (float)int.Parse(m.Groups["damage"].Value);
                }
            }
            #endregion
            else if (line.StartsWith("Reduces the mana cost of your spells by "))
            {
                line = line.Replace(".", "");
                line = line.Substring("Reduces the mana cost of your spells by ".Length);
                stats.SpellsManaReduction = float.Parse(line);
            }
        }

		public static void ProcessUseLine(string line, Stats stats, bool isArmory, int id)
		{
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

                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, s, duration, cooldown));
            }
            else if (line.StartsWith("Increases armor penetration rating by "))
            {
                line = line.Substring("Increases armor penetration rating by ".Length);
                if (line.Contains(".")) line = line.Substring(0, line.IndexOf("."));
                if (line.Contains(" ")) line = line.Substring(0, line.IndexOf(" "));
                stats.ArmorPenetrationRating += ((float)int.Parse(line));
            }
            else if (line.StartsWith("Increases agility by "))
            { //Special case: So that we don't increase bear stats by the average value, translate the agi to crit and ap
                line = line.Substring("Increases agility by ".Length);
                if (line.Contains(".")) line = line.Substring(0, line.IndexOf("."));
                if (line.Contains(" ")) line = line.Substring(0, line.IndexOf(" "));
                stats.CritRating += ((((float)int.Parse(line)) / 6f) / 25f) * 45.90598679f;
                stats.AttackPower += (((float)int.Parse(line)) / 6f) * 1.03f;
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
                int integral_uptime = int.Parse(inputs[2]);
                // unfortunately for us the cooldown is on the next line
                int cooldown_min = 2;
                float cooldown_sec = 2.0f * 60.0f;
                // Twilight Serpent
                if (id == 42395) { cooldown_min = 5; cooldown_sec = 5.0f * 60.0f; }
                // Vengeance of the Illidari (tooltip lies!)
                if (id == 28040) { cooldown_min = 1; cooldown_sec = 90.0f; }
                // try to add it to the normal rawr stats
                switch (integral_uptime)
                {
                    case 20:
                        switch (cooldown_min)
                        {
                            case 5:
                                stats.SpellPowerFor20SecOnUse5Min += spell_power;
                                break;
                            case 2:
                                stats.SpellPowerFor20SecOnUse2Min += spell_power;
                                break;
                        }
                        break;
                    case 15:
                        // Vengeance of the Illidari
                        if (id == 28040)
                        {
                            stats.SpellPowerFor15SecOnUse90Sec += spell_power;
                        }
                        break;
                }
                // we don't have a stat for this case so do the average
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { SpellPower = spell_power }, uptime, cooldown_sec));
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
                    if (dur == 20)
                    {
                        stats.SpiritFor20SecOnUse2Min += spi;
                    }
                }
            }
            else if (line.StartsWith("For the next 20 sec, your direct heals increase healing received by their target by up to 58."))
            {
                // Talisman of Troll Divinity
                // For 20 seconds, direct healing adds a stack of 58 +healing for 10 seconds
                // Stacks 5 times, 2 minute cd
                // Direct heals: Nourish (1.5) HT (3) Regrowth (2)
                // Assumption: every 2 seconds, a direct healing spell is cast
                // (1+2+3+4)+11*5 = 70 stacks over the total duration of 30 seconds
                //  70 stacks/ 15 casts = 4.67 stacks
                // But remember that the spellpower will increase for others in the raid too!
                stats.BonusHealingReceived = 58 * 5;
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { BonusHealingReceived = 58 * 4.67f }, 30, 120));
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
                stats.ManaregenFor8SecOnUse5Min += 250;
            }
            else if (line.StartsWith("Conjures a Power Circle lasting for 15 sec.  While standing in this circle, the caster gains 320 spell power."))
            {
                // Shifting Naaru Sliver
                stats.SpellPowerFor15SecOnUse90Sec += 320;
            }
            else if (line.StartsWith("Each spell cast within 20 seconds will grant a stacking bonus of 21 mana regen per 5 sec. Expires after 20 seconds.  Abilities with no mana cost will not trigger this trinket."))
            {
                stats.Mp5OnCastFor20SecOnUse2Min += 21;
            }
            // Figurine - Talasite Owl, 5 min cooldown
            else if (line.StartsWith("Restores 900 mana over 12 sec."))
            {
                if (stats.Mp5 == 18) // Figurine - Seaspray Albatross, 3 min cooldown
                    stats.ManaregenOver12SecOnUse3Min += 900;
                else if (stats.Mp5 == 14) // Figurine - Talasite Owl, 5 min cooldown
                    stats.ManaregenOver12SecOnUse5Min += 900;
                // stats.Mp5 += 5f * 900f / 300f;
            }
            else if (line.StartsWith("Increases the block value of your shield by 200 for 20 sec."))
            {
                stats.BlockValue += (float)Math.Round(200f * (20f / 120f));
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
				stats.ManaregenFor8SecOnUse5Min += 250;
			}
			else if (line.StartsWith("Conjures a Power Circle lasting for 15 sec.  While standing in this circle, the caster gains 320 spell power."))
			{
				// Shifting Naaru Sliver
				stats.SpellPowerFor15SecOnUse90Sec += 320;
			}
			else if (line.StartsWith("Tap into the power of the skull, increasing haste rating by 175 for 20 sec."))
			{
				// The Skull of Gul'dan
				stats.HasteRatingFor20SecOnUse2Min += 175;
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { HasteRating = 175 }, 20.0f, 120.0f));
			}
			else if (line.StartsWith("Each spell cast within 20 seconds will grant a stacking bonus of 21 mana regen per 5 sec. Expires after 20 seconds.  Abilities with no mana cost will not trigger this trinket."))
			{
				stats.Mp5OnCastFor20SecOnUse2Min += 21;
			}
			// Figurine - Talasite Owl, 5 min cooldown
			else if (line.StartsWith("Restores 900 mana over 12 sec."))
			{
				if (stats.Mp5 == 18) // Figurine - Seaspray Albatross, 3 min cooldown
					stats.ManaregenOver12SecOnUse3Min += 900;
				else if (stats.Mp5 == 14) // Figurine - Talasite Owl, 5 min cooldown
					stats.ManaregenOver12SecOnUse5Min += 900;
				// stats.Mp5 += 5f * 900f / 300f;
			}
			// Mind Quickening Gem
			else if (line.StartsWith("Quickens the mind, increasing the Mage's haste rating by 330 for 20 sec."))
			{
				stats.HasteRatingFor20SecOnUse5Min += 330;
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { HasteRating = 330 }, 20, 300));
			}
			else if (line.StartsWith("Increases the block value of your shield by 200 for 20 sec."))
			{
				stats.BlockValue += (float)Math.Round(200f * (20f / 120f));
			}
			else if (line.StartsWith("Removes all movement impairing effects and all effects which cause loss of control of your character."))
			{
				stats.PVPTrinket += 1f;
			}
            else if (line.StartsWith("Increases the damage dealt by your Scourge Strike and Obliterate abilities by 420."))
            {
                stats.BonusObliterateDamage += 420;
                stats.BonusScourgeStrikeDamage += 420;
            }
            else if (line == "Restores 2340 mana over 12 sec. (5 Min Cooldown)")
            {
                // Figurine - Sapphire Owl
                stats.ManaRestore5min = 2340;
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { ManaRestore = 2340 }, 1f, 300f));
            }
            else if (line == "Instantly heal your current friendly target for 2710. (1 Min Cooldown)")
            {
                // Living Ice Crystals
                stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { Healed = 2710 }, 1f, 60f));
                stats.Heal1Min = 2710;
            }
        }
	}
}
