using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Rogue.BasicStats
{
    public class BaseRogueStats : Stats
    {
        public BaseRogueStats(Character.CharacterRace race)
        {
            if (race == Character.CharacterRace.Tauren || race == Character.CharacterRace.Draenei)
                return;


            Health = 7924f;
            Agility = _raceStats[race][0];
            Strength = _raceStats[race][1];
            Stamina = _raceStats[race][2];
            AttackPower = 140;
            PhysicalCrit = 3.73f;
            DodgeRating = (float) (-0.59*18.92f);
            

            Health += Stamina * 10f;

        }

        private static readonly Dictionary<Character.CharacterRace, float[]> _raceStats = new Dictionary<Character.CharacterRace, float[]>
                                                                  {
                                                                    // Agility,Strength,Stamina
                                                                    { Character.CharacterRace.Human, new [] {158f, 95f, 89f}},
                                                                    { Character.CharacterRace.Orc, new [] {155f, 98f, 91f}},
                                                                    { Character.CharacterRace.Dwarf, new [] {154f, 97f, 92f,}},
                                                                    { Character.CharacterRace.NightElf, new [] {194f, 110f, 104f}},
                                                                    { Character.CharacterRace.Undead, new [] {156f, 94f, 90f}},
                                                                    { Character.CharacterRace.Tauren, new [] {0f, 0f, 0f}},
                                                                    { Character.CharacterRace.Gnome, new [] {161f, 90f, 88f}},
                                                                    { Character.CharacterRace.Troll, new [] {160f, 96f, 90f}},
                                                                    { Character.CharacterRace.BloodElf, new [] {160f, 92f, 87f}},
                                                                    { Character.CharacterRace.Draenei, new [] {0f, 0f, 0f}}
                                                                  };
    }
}
