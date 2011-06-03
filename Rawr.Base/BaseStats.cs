using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr
{
    public static class BaseStats
    {   // Only returns level 85 characters.
        public enum DruidForm { Caster, Bear, Cat, Moonkin }

        #region Cache Variables
        // Caching to reduce load.
        private static Stats _lastStats;
        private static int _lastLevel;
        private static CharacterClass _lastClass;
        private static CharacterRace _lastRace;
        private static DruidForm _lastForm;
        private static readonly object syncLock = new object();
        #endregion

        public static Stats GetBaseStats(Character character) { return GetBaseStats(character.Level, character.Class, character.Race, DruidForm.Caster); }
        public static Stats GetBaseStats(int level, CharacterClass characterClass, CharacterRace characterRace) { return GetBaseStats(level, characterClass, characterRace, DruidForm.Caster); }
        public static Stats GetBaseStats(int level, CharacterClass characterClass, CharacterRace characterRace, DruidForm characterForm)
        {   // Health, Mana and some other things are same for every race.
            lock (syncLock)
            {
                #region Cache
                if (level == _lastLevel
                    && characterClass == _lastClass
                    && characterRace == _lastRace
                    && characterForm == _lastForm)
                    return _lastStats.Clone();
                _lastLevel = level;
                _lastClass = characterClass;
                _lastRace = characterRace;
                _lastForm = characterForm;
                #endregion

                Stats S = new Stats();
                #region Race, not class benefit
                // Most Level 85 Race and Class Stats come from:
                // http://code.google.com/p/simulationcraft/source/browse/branches/cataclysm/engine/sc_rating.cpp?r=6207
                // When they were still at 80 as of Jan 01st, 2011

                // From SimCraft
                Stats race = new Stats();
                switch (characterRace)
                {
                    // Alliance
                    case CharacterRace.Human:    race.Strength = 20; race.Agility = 20; race.Stamina = 20; race.Intellect = 20; race.Spirit = 20; break;
                    case CharacterRace.Dwarf:    race.Strength = 25; race.Agility = 16; race.Stamina = 21; race.Intellect = 19; race.Spirit = 19; break;
                    case CharacterRace.NightElf: race.Strength = 16; race.Agility = 24; race.Stamina = 20; race.Intellect = 20; race.Spirit = 20; break;
                    case CharacterRace.Gnome:    race.Strength = 15; race.Agility = 22; race.Stamina = 20; race.Intellect = 24; race.Spirit = 20; break;
                    case CharacterRace.Draenei:  race.Strength = 21; race.Agility = 17; race.Stamina = 20; race.Intellect = 20; race.Spirit = 22; break;
                    case CharacterRace.Worgen:   race.Strength = 23; race.Agility = 22; race.Stamina = 20; race.Intellect = 16; race.Spirit = 19; break;
                    // Horde
                    case CharacterRace.Orc:      race.Strength = 23; race.Agility = 17; race.Stamina = 21; race.Intellect = 17; race.Spirit = 22; break;
                    case CharacterRace.Undead:   race.Strength = 19; race.Agility = 18; race.Stamina = 20; race.Intellect = 18; race.Spirit = 25; break;
                    case CharacterRace.Tauren:   race.Strength = 25; race.Agility = 16; race.Stamina = 21; race.Intellect = 16; race.Spirit = 22; break;
                    case CharacterRace.Troll:    race.Strength = 21; race.Agility = 22; race.Stamina = 20; race.Intellect = 16; race.Spirit = 21; break;
                    case CharacterRace.BloodElf: race.Strength = 17; race.Agility = 22; race.Stamina = 20; race.Intellect = 23; race.Spirit = 18; break;
                    case CharacterRace.Goblin:   race.Strength = 17; race.Agility = 22; race.Stamina = 20; race.Intellect = 23; race.Spirit = 20; break;
                    default: { break; }
                };
                // From Chardev (85)
                //Class           Str Agi Sta Int Spi
                //Druid            76  69  86 136 153
                //Shaman          111  60 128 119 136
                //Death Knight    171 101 154  16  44
                //Hunter           60 178 119  77  88
                //Mage             17  26  43 187 175
                //Paladin         144  77 136  86  97
                //Priest           26  34  51 188 183
                //Rogue           102 186  94  26  53
                //Warlock          43  51  76 161 166
                //Warrior         169 103 153  17  44
                #endregion

                #region Base Stats
                #region All Classes
                S.Miss  = 0.05f;
                S.Block = 0.00f;
                S.Parry = 0.00f;
                #endregion
                switch (characterClass)
                {
                    #region Death Knight
                    case CharacterClass.DeathKnight:
                        Stats dk = new Stats() {
                            Strength = 171, Agility = 101, Stamina = 274, Intellect = 16, Spirit = 44,
                            Health = 43025f,
                            Dodge = 0.0394f, Parry = 0.05f, Block = 0.00f,
                            PhysicalCrit = 0.0049f, AttackPower = 595f,
                        };
                        S.Accumulate(race);
                        S.Accumulate(dk);
                        break;
                    #endregion
                    #region Druid
                    case CharacterClass.Druid:
                        Stats druid = new Stats() {
                            Strength = 76, Agility = 69, Stamina = 86, Intellect = 136, Spirit = 153,
                            Health = 39533f, Mana = 18635f,
                            Dodge = 0.03758f, Parry = 0.05f, Block = 0.05f,
                            PhysicalCrit = 0.03192f, AttackPower = 613f,
                            SpellCrit = 0.0185f, Mp5 = 931f,
                        };
                        S.Accumulate(race);
                        S.Accumulate(druid);
                        switch (characterForm)
                        {
                            case DruidForm.Moonkin:
                            case DruidForm.Caster:
                                S.AttackPower = -10;
                                S.PhysicalCrit = 0.0743f;
                                S.Dodge = 0.0556970f; //??
                                break;
                            case DruidForm.Bear:
                                S.AttackPower = 255;
                                S.PhysicalCrit = 0.074755f;
                                S.Dodge = 0.0556970f;
                                S.BonusStaminaMultiplier = 0.1f;
                                break;
                            case DruidForm.Cat:
                                S.AttackPower = 235;
                                S.PhysicalCrit = 0.074755f;
                                S.Dodge = 0.0556970f;
                                break;
                            default:
                                break;
                        }
                        break;
                    #endregion
                    #region Hunter
                    case CharacterClass.Hunter:
                        Stats hun = new Stats() {
                            Strength = 60, Agility = 178, Stamina = 119, Intellect = 77, Spirit = 88,
                            Health = 39037,
                            Dodge = 0.03758f, Parry = 0.05f,
                            PhysicalCrit = 0.03192f, AttackPower = 0f, RangedAttackPower = 0f/*546*/,
                        };
                        S.Accumulate(race);
                        S.Accumulate(hun);
                        break;
                    #endregion
                    #region Mage
                    case CharacterClass.Mage:
                        Stats mag = new Stats() {
                            Strength = 17, Agility = 26, Stamina = 43, Intellect = 187, Spirit = 175,
                            Health = 36853f, Mana = 17138f,
                            Dodge = 0.03758f, Parry = 0.05f,
                        };
                        S.Accumulate(race);
                        S.Accumulate(mag);
                        break;
                    #endregion
                    #region Paladin
                    case CharacterClass.Paladin:
                        Stats pal = new Stats() {
                            Strength = 144, Agility = 77, Stamina = 136, Intellect = 86, Spirit = 97,
                            Health = 43285f, Mana = 23422,
                            Dodge = 0.0365145297f, Parry = 0.05f, Block = 0.05f,
                            PhysicalCrit = 0.00652f, AttackPower = 235f,
                            SpellCrit = 0.033355f,
                        };
                        S.Accumulate(race);
                        S.Accumulate(pal);
                        break;
                    #endregion
                    #region Priest
                    case CharacterClass.Priest:
                        // added adjustments to the base race here because the math using the previous stats
                        // just don't work for the in game calculations on priest tests. 
                        // also unsure how these changes would effect other modules if moved up.
                        // adding or subracting from the priest stats don't work and throws all other class
                        // calculations off. 
                        switch (characterRace)
                        {
                            case CharacterRace.Human: race.Spirit = 19; break;
                            case CharacterRace.Gnome: race.Intellect = 23; break;
                            case CharacterRace.Goblin: race.Spirit = 18; break;
                        }
                        Stats pri = new Stats() {
                            Strength = 26, Agility = 34, Stamina = 51, Intellect = 169, Spirit = 178,
                            Health = 43285f, Mana = 20590f,
                            Dodge = 0.0337780f,
                            Parry = 0.05f,
                            PhysicalCrit = 0.027f, SpellCrit = 0.012375f,
                        };
                        pri.Mp5 = pri.Mana * 0.05f;     // Always 5% of base mana in regen.
                        S.Accumulate(race);
                        S.Accumulate(pri);
                        break;
                    #endregion
                    #region Rogue
                    case CharacterClass.Rogue:
                        Stats rog = new Stats() {
                            Strength = 102, Agility = 186, Stamina = 94, Intellect = 26, Spirit = 53,
                            Health = 40529f,
                            Dodge = 0.03758f, Parry = 0.05f,
                            PhysicalCrit = 0.03192f, AttackPower = 613f,
                        };
                        S.Accumulate(race);
                        S.Accumulate(rog);
                        break;
                    #endregion
                    #region Shaman
                    case CharacterClass.Shaman:
                        Stats sha = new Stats() {
                            Strength = 111, Agility = 60, Stamina = 128, Intellect = 119, Spirit = 136,
                            Health = 37037f, Mana = 23430f,
                            Dodge = 0.0193f, Parry = 0.05f, Block = 0.05f,
                            PhysicalCrit = 0.02910375f, AttackPower = 140f,
                            SpellCrit = 0.022057f, SpellPower = -10,
                        };
                        S.Accumulate(race);
                        S.Accumulate(sha);
                        break;
                    #endregion
                    #region Warlock
                    case CharacterClass.Warlock:
                        Stats warlock = new Stats() { Strength = 43, Agility = 51, Stamina = 76, Intellect = 153, Spirit = 161,
                            Health = 38184f, Mana = 20553f,
                            Dodge = 0.0238110f, Parry = 0.05f, Block = 0.05f,
                            PhysicalCrit = 0.026219999417663f, SpellCrit = 0.017000000923872f,
                        };
                        S.Accumulate(race);
                        S.Accumulate(warlock);
                        break;
                    #endregion
                    #region Warrior
                    case CharacterClass.Warrior:
                        Stats war = new Stats() {
                            Strength = 169, Agility = 103, Stamina = 153, Intellect = 17, Spirit = 44,
                            Health = 43285f,
                            Dodge = 0.03758f, Parry = 0.05f, Block = 0.05f,
                            PhysicalCrit = 0.03192f, AttackPower = 613f,
                        };
                        S.Accumulate(race);
                        S.Accumulate(war);
                        break;
                    #endregion
                    #region No Class
                    default:
                        break;
                    #endregion
                }
                #endregion

                #region Racials
                // Resistance do not stack with other buffs. Until then I'll commenting them out
                if (characterRace == CharacterRace.Gnome)  //CATA: changed from 5% int to 5% mana
                {
                    // S.ArcaneResistance += 85f;
                    S.BonusManaMultiplier = 0.05f;
                    //S.BonusIntellectMultiplier = 0.05f;
                }
                else if (characterRace == CharacterRace.Human)
                {
                    S.BonusSpiritMultiplier = 0.03f;
                    // Patch 4.0.6+ changed from a 3 minute cooldown to 2 minute cooldown
                    S.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { PVPTrinket = 1 }, 0f, 120f));
                }
                else if (characterRace == CharacterRace.NightElf)
                {
                    // S.NatureResistance += 85f;
                    S.Miss += 0.02f;
                }
                else if (characterRace == CharacterRace.Dwarf)
                {
                    // S.FrostResistance += 85f;
                    // Armor +10% for 8 Sec.
                    S.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { BaseArmorMultiplier = .1f }, 8, 120));
                    // TODO: Add debuff removal.  Doesn't work on all bosses so not sure if we want to.

                }
                else if (characterRace == CharacterRace.Draenei)
                {
                    // S.ArcaneResistance += 85f;
                    S.SpellHit += 0.01f;
                    S.PhysicalHit += 0.01f;
                    // Patch 4.0.6+ changed from a scaling Health restore to a flat 20% of max health
                    S.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { HealthRestoreFromMaxHealth = 0.2f / 15f }, 15f, 180f));
                }
                else if (characterRace == CharacterRace.Worgen)
                {
                    // S.NatureResistance = 64f;
                    // S.ShadowResistance = 64f;
                    // Patch 4.0.6+ Darkflight changed from a 3 minute CD to a 2 minute CD
                    S.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { MovementSpeed = 0.40f }, 10f, 120f));
                    S.PhysicalCrit += 0.01f;
                    S.SpellCrit += 0.01f;
                }
                else if (characterRace == CharacterRace.Tauren)
                {
                    // S.NatureResistance = 85f;
                    S.Health = (float)Math.Floor(S.Health * 1.05f);
                }
                else if (characterRace == CharacterRace.Troll)
                {
                    S.SnareRootDurReduc = .15f;
                    if (characterClass == CharacterClass.DeathKnight || characterClass == CharacterClass.Warrior || characterClass == CharacterClass.Rogue)
                        S.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { PhysicalHaste = 0.2f }, 10f, 180f));
                    else
                        S.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { SpellHaste = 0.2f, PhysicalHaste = 0.2f }, 10f, 180f));
                }
                else if (characterRace == CharacterRace.Undead)
                {
                    S.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { FearDurReduc = 1f }, .1f, 120f));
                }
                else if (characterRace == CharacterRace.Orc)
                {
                    S.StunDurReduc = 0.15f;
                    if (characterClass == CharacterClass.Shaman)
                        S.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { AttackPower = 65 + (level * 13), SpellPower = 75 + (level * 6) }, 15f, 120f));
                    else if (characterClass == CharacterClass.Warlock)
                        S.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { SpellPower = 75 + (level * 6) }, 15f, 120f));
                    else
                        S.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { AttackPower = 65 + (level * 13) }, 15f, 120f));
                }
                else if (characterRace == CharacterRace.BloodElf)
                {
                    // S.ArcaneResistance += 85f;
                    if (characterClass == CharacterClass.DeathKnight || characterClass == CharacterClass.Rogue || characterClass == CharacterClass.Hunter)
                        S.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { ManaorEquivRestore = .15f }, 0f, 120f));
                    else if (characterClass == CharacterClass.Warrior)
                        S.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { BonusRageGen = 15f }, 0f, 120f));
                    else
                        S.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { ManaorEquivRestore = .06f }, 0f, 120f));
                }
                else if (characterRace == CharacterRace.Goblin)
                {
                    S.PhysicalHaste += 0.01f;
                    S.SpellHaste += 0.01f;
                    // TODO: The damage of the rocket belt proc is dependent on the character's current AP and SP
                    S.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { FireDamage = 1f + (level * 2) }, 0f, 120f));
                }
                #endregion

                _lastStats = S.Clone();
                return S;
            }
        }

        public static float GetRacialExpertise(Character character, ItemSlot weaponSlot)
        {
            ItemType weaponType;
            CharacterRace characterRace = character.Race;

            if (weaponSlot == ItemSlot.MainHand && character.MainHand != null)
                weaponType = character.MainHand.Item.Type;
            else if (weaponSlot == ItemSlot.OffHand && character.OffHand != null)
                weaponType = character.OffHand.Item.Type;
            else
                return 0.0f;

            switch (characterRace)
            {
                case CharacterRace.Human:
                    if (weaponType == ItemType.OneHandSword || weaponType == ItemType.TwoHandSword
                        || weaponType == ItemType.OneHandMace || weaponType == ItemType.TwoHandMace)
                    {
                        return 3.0f;
                    }
                    break;
                case CharacterRace.Dwarf:
                    if (weaponType == ItemType.OneHandMace || weaponType == ItemType.TwoHandMace)
                    {
                        return 3.0f;
                    }
                    break;
                case CharacterRace.Gnome:
                    if (weaponType == ItemType.OneHandSword || weaponType == ItemType.Dagger)
                    {
                        return 3.0f;
                    }
                    break;
                case CharacterRace.Orc:
                    if (weaponType == ItemType.OneHandAxe || weaponType == ItemType.TwoHandAxe
                        || weaponType == ItemType.FistWeapon)
                    {
                        return 3.0f;
                    }
                    break;
            }

            return 0.0f;
        }
    }
}
