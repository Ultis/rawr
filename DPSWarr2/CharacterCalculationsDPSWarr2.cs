using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DPSWarr2
{
    public class CharacterCalculationsDPSWarr2 : CharacterCalculationsBase
    {
        public Stats BasicStats { get; set; }

        // Required overloads
        private float _overallPoints = 0f;
        public override float OverallPoints
        {
            get { return _overallPoints; }
            set { _overallPoints = value; }
        }

        private float[] _subPoints = new float[] { 0f };
        public override float[] SubPoints
        {
            get { return _subPoints; }
            set { _subPoints = value; }
        }

        // only store things that can be affected by talents for now
        public float MhArP { get; set; }  // Mace Spec
        public float OhArP { get; set; }
        public bool isBattleStance { get; set; }

        public float MhCrit { get; set; } // Axe Spec
        public float OhCrit { get; set; }
        public float MhExp { get; set; }  // Racial Spec
        public float OhExp { get; set; }
        
        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();
            WarriorTalents talents = new WarriorTalents();
            Character character = new Character();
            //CombatFactors combatFactors = new CombatFactors(character, BasicStats);

            //WhiteAttacks whiteAttacks = new WhiteAttacks(talents, BasicStats, combatFactors);
            //if (SkillAttacks == null){SkillAttacks = new Skills(character,talents, BasicStats, combatFactors, whiteAttacks);}

            dictValues.Add("Health",string.Format("{0}", BasicStats.Health));
            dictValues.Add("Stamina",string.Format("{0}*Increases Health by {1}"
                ,BasicStats.Stamina,StatConversion.GetHealthFromStamina(BasicStats.Stamina)));
            dictValues.Add("Armor",string.Format("{0}*Increases Attack Power by {1}",
                BasicStats.Armor, BasicStats.Armor / 180 * talents.ArmoredToTheTeeth));
            dictValues.Add("Strength",string.Format("{0}*Increases Attack Power by {1}", BasicStats.Strength, BasicStats.Strength*2));
            dictValues.Add("Attack Power", string.Format("{0}*Increases DPS by {1:0.0}", (int)BasicStats.AttackPower, BasicStats.AttackPower/14));
            dictValues.Add("Agility",string.Format("{0}*Base Crit at lvl 80 {1:0.00%}"+
                Environment.NewLine+"Increases Crit by {2:0.00%}"+
                Environment.NewLine+"Increases Armor by {3:0}",
                BasicStats.Agility, 
                BaseStats.GetBaseStats(character.Level, character.Class, character.Race).PhysicalCrit,
                StatConversion.GetCritFromAgility(BasicStats.Agility, character.Class),
                StatConversion.GetArmorFromAgility(BasicStats.Agility)));
            dictValues.Add("Haste",string.Format("{0:0.00%}*Haste Rating {1}", 
                StatConversion.GetHasteFromRating(BasicStats.HasteRating, character.Class), BasicStats.HasteRating));
            dictValues.Add("Crit", string.Format("{0:0.00%}{1}*Crit Rating {2} (+{3:0.00%})",
                MhCrit, (OhCrit != MhCrit)?string.Format("/{0:0.00%}",OhCrit):"",
                BasicStats.CritRating, 
                StatConversion.GetCritFromRating(BasicStats.CritRating)));
            dictValues.Add("Armor Penetration",  string.Format("{0:0.00%}{1}*Armor Penetration Rating {2}- {3:0.00%}" +
                Environment.NewLine + "Arms Stance- +{4:0.00%}",
                                MhArP, (OhArP != MhArP)?string.Format("/{0:0.00%}",OhArP):"",
                                BasicStats.ArmorPenetrationRating, StatConversion.GetArmorPenetrationFromRating(BasicStats.ArmorPenetration),
                                isBattleStance?0.10f:0f));
            dictValues.Add("Hit Rating",
                string.Format("{0}*{1:0.00%} Increased Chance to hit",
                BasicStats.HitRating,StatConversion.GetHitFromRating(BasicStats.HitRating)));
            dictValues.Add("Expertise", 
                string.Format("{0:0.00}{1}*Expertise Rating {2}" +
                                Environment.NewLine + Environment.NewLine + "You can free up {3:0} Expertise Rating",
                                MhExp, (OhExp != MhExp)?string.Format("/{0:0.00}",OhExp):"",
                                BasicStats.ExpertiseRating,
                                Math.Ceiling(StatConversion.GetRatingFromExpertise(Math.Min(MhExp,OhExp) - (26f - talents.WeaponMastery*4f),character.Class))));
                                
            // DPS ind
            /* TODO (ebs)
            dictValues.Add("Bloodsurge",    string.Format("{0:0.00}",SkillAttacks.Bloodsurge()));
            dictValues.Add("Bloodthirst",   string.Format("{0:0.00}",SkillAttacks.Bloodthirst()));
            dictValues.Add("Whirlwind",     string.Format("{0:0.00}",SkillAttacks.Whirlwind()));
            dictValues.Add("Mortal Strike", string.Format("{0:0.00}",SkillAttacks.MortalStrike()));
            dictValues.Add("Slam",          string.Format("{0:0.00}",SkillAttacks.Slam()));
            dictValues.Add("Rend",          string.Format("{0:0.00}",SkillAttacks.Rend()));
            dictValues.Add("Sudden Death",  string.Format("{0:0.00}",SkillAttacks.SuddenDeath()));
            dictValues.Add("Overpower",     string.Format("{0:0.00}",SkillAttacks.Overpower()));
            dictValues.Add("Bladestorm",    string.Format("{0:0.00}",SkillAttacks.BladeStorm()));
            dictValues.Add("Sword Spec",    string.Format("{0:0.00}",SkillAttacks.SwordSpec()));
            // DPS
            dictValues.Add("Heroic Strike", string.Format("{0:0.00}",SkillAttacks.HeroicStrike()));
            dictValues.Add("Deep Wounds",   string.Format("{0:0.00}",SkillAttacks.Deepwounds()));
            dictValues.Add("White DPS",     string.Format("{0:0.00}*Main Hand-{1:0.00}"+Environment.NewLine+"Off Hand- {2:0.00}",
                WhiteDPS, WhiteDPSMH, WhiteDPSOH));
            dictValues.Add("Total DPS",string.Format("{0:0.00}",WhiteDPSMH + WhiteDPSOH + SkillAttacks.Bloodthirst() + SkillAttacks.Whirlwind() +
                                       SkillAttacks.HeroicStrike() + SkillAttacks.Bloodsurge() + SkillAttacks.Deepwounds() +
                                       SkillAttacks.MortalStrike() + SkillAttacks.SuddenDeath() + SkillAttacks.Slam() + SkillAttacks.Overpower() +
                                       SkillAttacks.Rend() + SkillAttacks.SwordSpec() + SkillAttacks.BladeStorm()));
            // Rage
            dictValues.Add("Free Rage", SkillAttacks.freeRage().ToString());
            dictValues.Add("White DPS Rage", WhiteRage.ToString());
            dictValues.Add("Other Rage", SkillAttacks.OtherRage().ToString());
            */            
            return dictValues;
        }
    }
}
