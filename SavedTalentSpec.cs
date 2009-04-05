using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr
{
    public class SavedTalentSpec
    {

        public string Name { get; set; }
        public Character.CharacterClass Class { get; set; }
        public string Spec { get; set; }

        public int Tree1 { get; set; }
        public int Tree2 { get; set; }
        public int Tree3 { get; set; }

        public SavedTalentSpec() : this("", null, 0, 0, 0) { ; }

        public SavedTalentSpec(String name, TalentsBase talentSpec, int tree1, int tree2, int tree3)
        {
            Name = name;
            Tree1 = tree1;
            Tree2 = tree2;
            Tree3 = tree3;
            if (talentSpec != null)
            {
                Spec = talentSpec.ToString();
                if (talentSpec.GetType() == typeof(DeathKnightTalents)) Class = Character.CharacterClass.DeathKnight;
                if (talentSpec.GetType() == typeof(WarriorTalents)) Class = Character.CharacterClass.Warrior;
                if (talentSpec.GetType() == typeof(PaladinTalents)) Class = Character.CharacterClass.Paladin;
                if (talentSpec.GetType() == typeof(ShamanTalents)) Class = Character.CharacterClass.Shaman;
                if (talentSpec.GetType() == typeof(HunterTalents)) Class = Character.CharacterClass.Hunter;
                if (talentSpec.GetType() == typeof(RogueTalents)) Class = Character.CharacterClass.Rogue;
                if (talentSpec.GetType() == typeof(DruidTalents)) Class = Character.CharacterClass.Druid;
                if (talentSpec.GetType() == typeof(WarlockTalents)) Class = Character.CharacterClass.Warlock;
                if (talentSpec.GetType() == typeof(PriestTalents)) Class = Character.CharacterClass.Priest;
                if (talentSpec.GetType() == typeof(MageTalents)) Class = Character.CharacterClass.Mage;
            }
        }

        public TalentsBase TalentSpec()
        {
            if (Spec == null) return null;
            TalentsBase spec;
            if (Class == Character.CharacterClass.DeathKnight) spec = new DeathKnightTalents(Spec);
            else if (Class == Character.CharacterClass.Warrior) spec = new WarriorTalents(Spec);
            else if (Class == Character.CharacterClass.Paladin) spec = new PaladinTalents(Spec);
            else if (Class == Character.CharacterClass.Shaman) spec = new ShamanTalents(Spec);
            else if (Class == Character.CharacterClass.Hunter) spec = new HunterTalents(Spec);
            else if (Class == Character.CharacterClass.Rogue) spec = new RogueTalents(Spec);
            else if (Class == Character.CharacterClass.Druid) spec = new DruidTalents(Spec);
            else if (Class == Character.CharacterClass.Warlock) spec = new WarlockTalents(Spec);
            else if (Class == Character.CharacterClass.Priest) spec = new PriestTalents(Spec);
            else spec = new MageTalents(Spec);
            return spec;
        }

        public override string ToString()
        {
            return string.Format("{0} ({1}/{2}/{3})", Name, Tree1, Tree2, Tree3);
        }

        public bool Equals(TalentsBase talents)
        {
            if (talents == null || Spec == null) return false;
            return talents.ToString().Equals(Spec);
        }
    }
}
