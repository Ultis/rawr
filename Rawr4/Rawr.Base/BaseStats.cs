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
                #endregion

                #region Base Stats
                #region All Classes
                S.Miss = 0.05f;
                S.Block = 0.0f;
                S.Parry = 0.0f;
                #endregion
                switch (characterClass)
                {
                    #region Death Knight
                    case CharacterClass.DeathKnight:
                        // Blood Elf, Draenei, Dwarf, Gnome, Human, Night Elf, Orc, Tauren, Troll, Undead
                        S.Mana = 0;
                        S.Health = 43025;
                        S.Armor = 0;
                        S.AttackPower = 615;
                        S.PhysicalCrit = 0.0319f;
                        S.Dodge = 0.0362500f;
                        S.Parry = 0.05f;
                        S.SpellPower = 0;
                        S.SpellCrit = 0.0f;
                        switch (characterRace)
                        {
                            case CharacterRace.BloodElf:
                                S.Strength = 172 + 16;
                                S.Agility = 114 + 9;
                                S.Stamina = 158 + 185;
                                S.Intellect = 39 + 1;
                                S.Spirit = 58 + 4;
                                break;
                            case CharacterRace.Draenei:
                                S.Strength = 176 + 16;
                                S.Agility = 109 + 9;
                                S.Stamina = 162 + 185;
                                S.Intellect = 36 + 1;
                                S.Spirit = 61 + 4;
                                break;
                            case CharacterRace.Dwarf:
                                S.Strength = 177 + 16;
                                S.Agility = 108 + 9;
                                S.Stamina = 163 + 185;
                                S.Intellect = 34 + 1;
                                S.Spirit = 58 + 4;
                                break;
                            case CharacterRace.Gnome:
                                S.Strength = 170 + 16;
                                S.Agility = 115 + 9;
                                S.Stamina = 159 + 185;
                                S.Intellect = 37 + 1;   // 39/1.05 = 37.14 ~37
                                S.Spirit = 59 + 4;
                                break;
                            case CharacterRace.Human:
                                S.Strength = 175 + 16;
                                S.Agility = 112 + 9;
                                S.Stamina = 160 + 185;
                                S.Intellect = 35 + 1;
                                S.Spirit = 58 + 4;  // 60/1.03 = 58.25 ~58.
                                break;
                            case CharacterRace.NightElf:
                                S.Strength = 172 + 16;
                                S.Agility = 117 + 9;
                                S.Stamina = 159 + 185;
                                S.Intellect = 35 + 1;
                                S.Spirit = 59 + 4;
                                break;
                            case CharacterRace.Orc:
                                S.Strength = 178 + 16;
                                S.Agility = 109 + 9;
                                S.Stamina = 162 + 185;
                                S.Intellect = 32 + 1;
                                S.Spirit = 62 + 4;
                                break;
                            case CharacterRace.Tauren:
                                S.Strength = 180 + 16;
                                S.Agility = 107 + 9;
                                S.Stamina = 162 + 185;
                                S.Intellect = 30 + 1;
                                S.Spirit = 61 + 4;
                                break;
                            case CharacterRace.Troll:
                                S.Strength = 176 + 16;
                                S.Agility = 114 + 9;
                                S.Stamina = 165 + 185;
                                S.Intellect = 31 + 1;
                                S.Spirit = 60 + 4;
                                break;
                            case CharacterRace.Undead:
                                S.Strength = 174 + 16;
                                S.Agility = 110 + 9;
                                S.Stamina = 161 + 185;
                                S.Intellect = 33 + 1;
                                S.Spirit = 64 + 4;
                                break;
                            default:
                                break;
                        }
                        break;
                    #endregion
                    #region Druid
                    case CharacterClass.Druid:
                        // NightElf, Tauren, Worgen, Troll
                        S.Mana = 18635;
                        S.Health = 39533;
                        S.Armor = 0;
                        S.SpellPower = 0;
                        S.SpellCrit = 0.0185f;
                        S.Mp5 = 931f;
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
                                S.AttackPower = 170;
                                S.PhysicalCrit = 0.074755f;
                                S.Dodge = 0.0556970f;
                                break;
                            default:
                                break;
                        }
                        if (characterRace == CharacterRace.NightElf)
                        {
                            S.Strength = 92;
                            S.Agility = 93;
                            S.Stamina = 106;
                            S.Intellect = 156;
                            S.Spirit = 173;
                        }
                        else if (characterRace == CharacterRace.Worgen)
                        {
                            S.Strength = 99;
                            S.Agility = 91;
                            S.Stamina = 106;
                            S.Intellect = 152;
                            S.Spirit = 172;
                        }
                        else if (characterRace == CharacterRace.Tauren)
                        {
                            S.Strength = 101;
                            S.Agility = 85;
                            S.Stamina = 107;
                            S.Intellect = 152;
                            S.Spirit = 175;
                        }
                        else if (characterRace == CharacterRace.Troll)
                        {
                            S.Strength = 97;
                            S.Agility = 91;
                            S.Stamina = 106;
                            S.Intellect = 152;
                            S.Spirit = 174;
                        }
                        break;
                    #endregion
                    #region Hunter
                    case CharacterClass.Hunter:
                        // Blood Elf, Draenei, Dwarf, Night Elf, Orc, Tauren, Troll

                        S.Mana = 5046;
                        S.Armor = 0;
                        S.AttackPower = 140;
                        S.RangedAttackPower = 150;
                        S.PhysicalCrit = -0.0153f;
                        S.Dodge = -0.0412730f;
                        S.Parry = 0.05f;
                        S.SpellPower = 0;
                        S.SpellCrit = 0.0360f;
                        switch (characterRace)
                        {
                            case CharacterRace.BloodElf:
                                S.Strength = 71;
                                S.Agility = 183;
                                S.Stamina = 126;
                                S.Intellect = 94;
                                S.Spirit = 96;
                                S.Health = 7324;
                                break;
                            case CharacterRace.Draenei:
                                S.Strength = 75;
                                S.Agility = 178;
                                S.Stamina = 129;
                                S.Intellect = 91;
                                S.Spirit = 99;
                                S.Health = 7400;
                                break;
                            case CharacterRace.Dwarf:
                                S.Strength = 76;
                                S.Agility = 177;
                                S.Stamina = 131;
                                S.Intellect = 89;
                                S.Spirit = 96;
                                S.Health = 7412;
                                break;
                            case CharacterRace.NightElf:
                                S.Strength = 71;
                                S.Agility = 184;
                                S.Stamina = 127;
                                S.Intellect = 90;
                                S.Spirit = 97;
                                S.Health = 7324;
                                break;
                            case CharacterRace.Orc:
                                S.Strength = 77;
                                S.Agility = 178;
                                S.Stamina = 130;
                                S.Intellect = 87;
                                S.Spirit = 100;
                                S.Health = 7324;
                                break;
                            case CharacterRace.Tauren:
                                S.Strength = 79;
                                S.Agility = 176;
                                S.Stamina = 130;
                                S.Intellect = 85;
                                S.Spirit = 99;
                                S.Health = 7400;
                                break;
                            case CharacterRace.Troll:
                                S.Strength = 75;
                                S.Agility = 183;
                                S.Stamina = 129;
                                S.Intellect = 86;
                                S.Spirit = 98;
                                S.Health = 7324;
                                break;
                            default:
                                break;
                        }
                        break;
                    #endregion
                    #region Mage
                    case CharacterClass.Mage:
                        // Blood Elf, Draenei, Gnome, Human, Troll, Undead
                        S.Mana = 3268;
                        S.Health = 6963;
                        S.Armor = 0;
                        S.AttackPower = -10;
                        S.PhysicalCrit = 0.03f;
                        S.Dodge = 0.0361870f;
                        S.SpellPower = 0;
                        S.SpellCrit = 0.0091f;
                        switch (characterRace)
                        {
                            case CharacterRace.BloodElf:
                                S.Strength = 33;
                                S.Agility = 45;
                                S.Stamina = 57;
                                S.Intellect = 185;
                                S.Spirit = 173;
                                break;
                            case CharacterRace.Draenei:
                                S.Strength = 37;
                                S.Agility = 40;
                                S.Stamina = 58;
                                S.Intellect = 182;
                                S.Spirit = 176;
                                break;
                            case CharacterRace.Gnome:
                                S.Strength = 31;
                                S.Agility = 46;
                                S.Stamina = 58;
                                S.Intellect = 184; // 193/1.05 = 183.8 ~184.
                                S.Spirit = 174;
                                break;
                            case CharacterRace.Human:
                                S.Strength = 36;
                                S.Agility = 43;
                                S.Stamina = 59;
                                S.Intellect = 181;
                                S.Spirit = 174; // 179/1.03 = 173.8 ~174
                                break;
                            case CharacterRace.Troll:
                                S.Strength = 37;
                                S.Agility = 45;
                                S.Stamina = 60;
                                S.Intellect = 177;
                                S.Spirit = 175;
                                break;
                            case CharacterRace.Undead:
                                S.Strength = 35;
                                S.Agility = 41;
                                S.Stamina = 60;
                                S.Intellect = 179;
                                S.Spirit = 179;
                                break;
                            default:
                                break;
                        }
                        break;
                    #endregion
                    #region Paladin
                    case CharacterClass.Paladin:
                        // Blood Elf, Draenei, Dwarf, Human
                        S.Mana = 4394;
                        S.Health = 6934;
                        S.Armor = 0;
                        S.AttackPower = 240;
                        S.PhysicalCrit = 0.0327f;
                        S.SpellPower = 0;
                        S.SpellCrit = 0.03336f;
                        S.Dodge = 0.0349430f;
                        S.Parry = 0.05f;
                        S.Block = 0.05f;
                        //S.Defense = 400.0f;
                        switch (characterRace)
                        {
                            case CharacterRace.BloodElf:
                                S.Strength = 161;
                                S.Agility = 99;
                                S.Stamina = 276;
                                S.Intellect = 109;
                                S.Spirit = 112;
                                break;
                            case CharacterRace.Draenei:
                                S.Strength = 165;
                                S.Agility = 94;
                                S.Stamina = 143; //note: not updated
                                S.Intellect = 106;
                                S.Spirit = 107; //note: not updated
                                break;
                            case CharacterRace.Dwarf:
                                S.Strength = 169;
                                S.Agility = 93;
                                S.Stamina = 146;//note: not updated
                                S.Intellect = 105;
                                S.Spirit = 104;//note: not updated
                                break;
                            case CharacterRace.Human:
                                S.Strength = 164;
                                S.Agility = 97;
                                S.Stamina = 156;
                                S.Intellect = 106;
                                S.Spirit = 117;
                                break;
                            case CharacterRace.Tauren:
                                S.Strength = 169;
                                S.Agility = 93;
                                S.Stamina = 198;
                                S.Intellect = 102;
                                S.Spirit = 116;
                                break;
                            default:
                                break;
                        }
                        break;
                    #endregion
                    #region Priest
                    case CharacterClass.Priest:
                        // Blood Elf, Draenei, Dwarf, Gnome, Goblin, Human, Night Elf, Troll, Undead, Worgen
                        S.Mana = 3863 + 16727;
                        S.Health = 6960 + 36325;
                        S.Armor = 0;
                        S.AttackPower = -10;
                        S.PhysicalCrit = 0.027f;
                        S.Dodge = 0.0337780f;
                        S.SpellPower = 0;
                        S.SpellCrit = 0.0124f;
                        switch (characterRace)
                        {
                            case CharacterRace.BloodElf:
                                S.Strength = 40 + 3;
                                S.Agility = 53 + 3;
                                S.Stamina = 65 + 6;
                                S.Intellect = 178 + 24;
                                S.Spirit = 180 + 16;
                                break;
                            case CharacterRace.Draenei:
                                S.Strength = 44 + 3;
                                S.Agility = 48 + 3;
                                S.Stamina = 66 + 6;
                                S.Intellect = 175 + 24;
                                S.Spirit = 183 + 16;
                                break;
                            case CharacterRace.Dwarf:
                                S.Strength = 45 + 3;
                                S.Agility = 47 + 3;
                                S.Stamina = 70 + 6;
                                S.Intellect = 176 + 24;
                                S.Spirit = 180 + 16;
                                break;
                            case CharacterRace.Gnome:
                                S.Strength = 38 + 3;
                                S.Agility = 54 + 3;
                                S.Stamina = 66 + 6;
                                S.Intellect = 178 + 24; //To-Do: Check Racial +5%
                                S.Spirit = 181 + 16;
                                break;
                            case CharacterRace.Goblin:
                                S.Strength = 40 + 3;
                                S.Agility = 53 + 3;
                                S.Stamina = 67 + 6;
                                S.Intellect = 177 + 24;
                                S.Spirit = 179 + 16;
                                break;
                            case CharacterRace.Human:
                                S.Strength = 43 + 3;
                                S.Agility = 51 + 3;
                                S.Stamina = 67 + 6;
                                S.Intellect = 174 + 24;
                                S.Spirit = 181 + 16;     // 186/1.03 = 180.5->181
                                break;
                            case CharacterRace.NightElf:
                                S.Strength = 40 + 3;
                                S.Agility = 56 + 3;
                                S.Stamina = 66 + 6;
                                S.Intellect = 174 + 24;
                                S.Spirit = 181 + 16;
                                break;
                            case CharacterRace.Troll:
                                S.Strength = 44 + 3;
                                S.Agility = 53 + 3;
                                S.Stamina = 68 + 6;
                                S.Intellect = 170 + 24;
                                S.Spirit = 182 + 16;
                                break;
                            case CharacterRace.Undead:
                                S.Strength = 42 + 3;
                                S.Agility = 49 + 3;
                                S.Stamina = 68 + 6;
                                S.Intellect = 172 + 24;
                                S.Spirit = 186 + 16;
                                break;
                            case CharacterRace.Worgen:
                                S.Strength = 46 + 3;
                                S.Agility = 53 + 3;
                                S.Stamina = 67 + 6;
                                S.Intellect = 170 + 24;
                                S.Spirit = 180 + 16;
                                break;
                            default:
                                break;
                        }
                        break;
                    #endregion
                    #region Rogue
                    case CharacterClass.Rogue:
                        // Blood Elf, Dwarf, Gnome, Human, Night Elf, Orc, Troll, Undead
                        S.Mana = 0;
                        S.Health = 40529;
                        S.Armor = 0;
                        S.AttackPower = 170;
                        S.PhysicalCrit = -0.00295f; //???
                        S.Dodge = 0.0205570f; //???
                        S.Parry = 0.05f; //???
                        S.SpellPower = 0;
                        S.SpellCrit = 0f;
                        switch (characterRace)
                        {
                            case CharacterRace.BloodElf:
                                S.Strength = 119;
                                S.Agility = 218;
                                S.Stamina = 234;
                                S.Intellect = 49;
                                S.Spirit = 69;
                                break;
                            case CharacterRace.Dwarf: //???
                                S.Strength = 115;
                                S.Agility = 185;
                                S.Stamina = 108;
                                S.Intellect = 42;
                                S.Spirit = 66;
                                break;
                            case CharacterRace.Gnome: //???
                                S.Strength = 108;
                                S.Agility = 192;
                                S.Stamina = 104;
                                S.Intellect = 46;   // 48/1.05 = 45.71 ~46
                                S.Spirit = 67;
                                break;
                            case CharacterRace.Human: //???
                                S.Strength = 113;
                                S.Agility = 189;
                                S.Stamina = 105;
                                S.Intellect = 43;
                                S.Spirit = 67;  // 69/1.03 = 66.99 ~67
                                break;
                            case CharacterRace.NightElf: //???
                                S.Strength = 110;
                                S.Agility = 194;
                                S.Stamina = 104;
                                S.Intellect = 43;
                                S.Spirit = 67;
                                break;
                            case CharacterRace.Orc: //???
                                S.Strength = 116;
                                S.Agility = 186;
                                S.Stamina = 107;
                                S.Intellect = 40;
                                S.Spirit = 70;
                                break;
                            case CharacterRace.Troll: //???
                                S.Strength = 114;
                                S.Agility = 191;
                                S.Stamina = 108;
                                S.Intellect = 39;
                                S.Spirit = 68;
                                break;
                            case CharacterRace.Undead: //???
                                S.Strength = 112;
                                S.Agility = 187;
                                S.Stamina = 105;
                                S.Intellect = 41;
                                S.Spirit = 72;
                                break;
                            default:
                                break;
                        }
                        break;
                    #endregion
                    #region Shaman
                    case CharacterClass.Shaman:
                        // Draenei, Dwarf, Goblin, Orc, Tauren, Troll
                        //Item marked with "WotLK" are unchecked lvl 80 values that need to be updated
                        S.Mana = 23430;
                        S.Health = 37037;
                        S.Armor = 0;
                        S.AttackPower = 140;
                        S.PhysicalCrit = 0.02910375f;
                        S.Dodge = 0.0193f;
                        S.Block = 0.05f;
                        S.SpellPower = -10;
                        S.SpellCrit = 0.022057f;
                        switch (characterRace)
                        {
                            case CharacterRace.Draenei:
                                S.Strength = 132;
                                S.Agility = 77;
                                S.Stamina = 148;
                                S.Intellect = 139;
                                S.Spirit = 158;
                                break;
                            case CharacterRace.Dwarf:
                                S.Strength = 136;
                                S.Agility = 76;
                                S.Stamina = 149;
                                S.Intellect = 138;
                                S.Spirit = 155;
                                break;
                            case CharacterRace.Goblin:
                                S.Strength = 128;
                                S.Agility = 82;
                                S.Stamina = 148;
                                S.Intellect = 142;
                                S.Spirit = 154;
                                break;
                            case CharacterRace.Orc:
                                S.Strength = 134;
                                S.Agility = 77;
                                S.Stamina = 209;  //Check this one, seems a bit high
                                S.Intellect = 136;
                                S.Spirit = 158;
                                break;
                            case CharacterRace.Tauren:
                                S.Strength = 125;  //WotLK
                                S.Agility = 70;  //WotLK
                                S.Stamina = 137;  //WotLK
                                S.Intellect = 124;  //WotLK
                                S.Spirit = 145;  //WotLK
                                break;
                            case CharacterRace.Troll:
                                S.Strength = 121;  //WotLK
                                S.Agility = 76;  //WotLK
                                S.Stamina = 137;  //WotLK
                                S.Intellect = 124;  //WotLK
                                S.Spirit = 144;  //WotLK
                                break;
                            default:
                                break;
                        }
                        break;
                    #endregion
                    #region Warlock
                    case CharacterClass.Warlock:
                        // Blood Elf, Gnome, Human, Orc, Undead
                        S.Mana = 3856;
                        S.Health = 7164;
                        S.Armor = 0;
                        S.AttackPower = -10;
                        S.PhysicalCrit = 0.028f;
                        S.Dodge = 0.0238110f;
                        S.SpellPower = 0;
                        S.SpellCrit = 0.01701f;
                        switch (characterRace)
                        {
                            case CharacterRace.BloodElf:
                                S.Strength = 56;
                                S.Agility = 69;
                                S.Stamina = 87;
                                S.Intellect = 163;
                                S.Spirit = 165;
                                break;
                            case CharacterRace.Gnome:
                                S.Strength = 54;
                                S.Agility = 70;
                                S.Stamina = 88;
                                S.Intellect = 162;  // 170/1.05 = 161.9 ~162
                                S.Spirit = 166;
                                break;
                            case CharacterRace.Human:
                                S.Strength = 59;
                                S.Agility = 67;
                                S.Stamina = 89;
                                S.Intellect = 159;
                                S.Spirit = 165; // 170/1.03 = 165.05 ~165
                                break;
                            case CharacterRace.Orc:
                                S.Strength = 62;
                                S.Agility = 64;
                                S.Stamina = 91;
                                S.Intellect = 156;
                                S.Spirit = 169;
                                break;
                            case CharacterRace.Undead:
                                S.Strength = 58;
                                S.Agility = 65;
                                S.Stamina = 90;
                                S.Intellect = 157;
                                S.Spirit = 171;
                                break;
                            default:
                                break;
                        }
                        break;
                    #endregion
                    #region Warrior
                    case CharacterClass.Warrior:
                        Stats war = new Stats() { Strength = 169, Agility = 103, Stamina = 153, Intellect = 17, Spirit = 44,
                            Health = 43285f, Dodge = 0.03758f, Parry = 0.05f, Block = 0.05f, PhysicalCrit = 0.03192f, AttackPower = 220f, };
                        S.Accumulate(race);
                        S.Accumulate(war);
#if FALSE
                        static const _stat_list_t warrior_stats[] = {
                        //         Str   Agi   Sta   Int   Spi  Health    Mana  Crit/Agi Crit/Int Ddg/Agi   MeleCrit  SpellCrit
                          {	80, {  154,   93,  139,   16,   39,   8121,    100,  0.0161,  0.0000,  0.0121,  0.0317876,  0.0000 } },
                          {	85, {  169,  103,  153,   17,   44,  43285,    100,  0.0161,  0.0000,  0.0121,  0.0317876,  0.0000 } },
                          { 0, { 0 } }
                        };
#endif
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
                    if (Rawr.Properties.GeneralSettings.Default.PTRMode)
                    {
                        // Patch 4.0.6+ changed from a 3 minute cooldown to 2 minute cooldown
                        S.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { PVPTrinket = 1 }, 0f, 120f));
                    }
                    else
                    {
                        S.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { PVPTrinket = 1 }, 0f, 180f));
                    }
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
                    if (Rawr.Properties.GeneralSettings.Default.PTRMode)
                    {
                        // Patch 4.0.6+ changed from a scaling Health restore to a flat 20% of max health
                        S.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { HealthRestoreFromMaxHealth = 0.2f / 15f }, 15f, 180f));
                    }
                }
                else if (characterRace == CharacterRace.Worgen)
                {
                    // S.NatureResistance = 64f;
                    // S.ShadowResistance = 64f;
                    if (Rawr.Properties.GeneralSettings.Default.PTRMode)
                    {
                        // Patch 4.0.6+ Darkflight changed from a 3 minute CD to a 2 minute CD
                        S.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { MovementSpeed = 0.40f }, 10f, 120f));
                    }
                    else
                    {
                        S.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { MovementSpeed = 0.40f }, 10f, 180f));
                    }
                    S.PhysicalCrit += 0.01f;
                    S.SpellCrit += 0.01f;
                }
                else if (characterRace == CharacterRace.Tauren)
                {
                    // S.NatureResistance = 85f;
                    S.Health = S.Health * 1.05f;
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
