using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DPSWarr
{
    public class CharacterCalculationsDPSWarr : CharacterCalculationsBase
    {
        public Stats BasicStats { get; set; }
        public List<Buff> ActiveBuffs { get; set; }
        //public AbilityModelList Abilities { get; set; }

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
        
        public float TotalDPS
        {
            get { return _subPoints[0];}
            set { _subPoints[0] = value; }
        }

        public int TargetLevel { get; set; }

        // Attack Table
        public float Miss { get; set; }
        public float HitRating { get; set; }
        public float HitPercent { get; set; }
        public float ExpertiseRating { get; set; }
        public float Expertise { get; set; }
        //public float MhExpertise { get; set; }
        //public float OhExpertise { get; set; }
        public float AgilityCritBonus { get; set; }
        public float CritRating { get; set; }
        public float CritPercent { get; set; }
        public float MhCrit { get; set; }
        public float OhCrit { get; set; }
        // Offensive
        public float ArmorPenetration { get; set; }
        public float buffedArmorPenetration { get; set; }
        public float HasteRating { get; set; }
        public float HastePercent { get; set; }
        public float WeaponSpeed { get; set; }
        public float TeethBonus { get; set; }
        // DPS
        public float WhiteDPS { get; set; }
        public float DeepWoundsDPS { get; set; }
        public float HeroicStrikeDPS { get; set; }
        public float SlamDPS { get; set; }
        public float RendDPS { get; set; }
        public float MortalStrikeDPS { get; set; }
        public float OverpowerDPS { get; set; }
        public float SwordSpecDPS { get; set; }
        public float BladestormDPS { get; set; }
        public float BloodsurgeDPS { get; set; }
        public float SuddenDeathDPS { get; set; }
        public float BloodthirstDPS { get; set; }
        public float WhirlwindDPS { get; set; }
        public float TotalDamagePerSecond { get; set; }
        // Neutral
        public float WhiteRage { get; set; }
        public float OtherRage { get; set; }
        public float FreeRage { get; set; }
        public float Stamina { get; set; }
        public float Health { get; set; }
        // Defensive
        public float Armor { get; set; }
        public float CritReduction { get; set; }
        public float ArmorReduction { get; set; }
        public float GuaranteedReduction { get; set; }
        public float damageReduc { get; set; }
        public float MissedAttacks { get; set; }
        public float AvoidedAttacks { get; set; }
        public float DodgedAttacks { get; set; }
        public float ParriedAttacks { get; set; }
        public float BlockedAttacks { get; set; }
        // PvP

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues() {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();
            WarriorTalents talents = new WarriorTalents();
            Character character = new Character();
            CombatFactors combatFactors = new CombatFactors(character, BasicStats);
            
            var whiteAttacks = new WhiteAttacks(talents, BasicStats, combatFactors);
            var skillAttacks = new Skills(talents, BasicStats, combatFactors, whiteAttacks);

            dictValues.Add("Health", BasicStats.Health.ToString());
            dictValues.Add("Stamina", BasicStats.Stamina.ToString());
            dictValues.Add("Armor",string.Format("{0}*Increases Attack Power by {1}",Armor,TeethBonus));
            dictValues.Add("Strength",string.Format("{0}*Increases Attack Power by {1}",BasicStats.Strength,BasicStats.Strength*DPSWarr.StrengthToAP));
            dictValues.Add("Attack Power", string.Format("{0}*Increases DPS by {1:0.0}", (int)BasicStats.AttackPower,BasicStats.AttackPower/14));
            dictValues.Add("Agility",string.Format("{0}*Increases Crit by {1:0.00%}"+Environment.NewLine+"Increases Armor by {2:0}",
                BasicStats.Agility, AgilityCritBonus, BasicStats.Agility*DPSWarr.AgilityToArmor));
            dictValues.Add("Haste",string.Format("{0:0.00%}*Haste Rating {1}", HastePercent, BasicStats.HasteRating));
            dictValues.Add("Crit", string.Format("{0:0.00%}*Crit Rating {1}" +
                Environment.NewLine + "MH Crit {2:0.00%}" +
                Environment.NewLine + "OH Crit {3:0.00%}",
                CritPercent, BasicStats.CritRating,MhCrit,OhCrit));
            dictValues.Add("Armor Penetration", 
                string.Format("{0:0.00%}*Armor Penetration Rating {1}" + Environment.NewLine + "Armor Reduction {2}",
                                BasicStats.ArmorPenetrationRating * DPSWarr.ArPToArmorPenetration,
                                BasicStats.ArmorPenetrationRating,
                                BasicStats.ArmorPenetration * DPSWarr.ArPToArmorPenetration));
            dictValues.Add("Hit Rating",
                string.Format("{0}*% Chance to hit {1}" + Environment.NewLine + "This does not include Precision", BasicStats.HitRating, BasicStats.HitRating*DPSWarr.HitRatingToHit));
            dictValues.Add("Expertise", 
                string.Format("{0:0.00}*Expertise Rating {1}" + Environment.NewLine + "Reduces chance to be dodged or parried by {2:0.00%}." +
                                Environment.NewLine + "Main Hand Exp- {3:0.00}" + Environment.NewLine + "Off Hand Exp- {4:0.00}" +
                                Environment.NewLine + Environment.NewLine + "Weapon types dont seem to affect this like it should. calc error", 
                                BasicStats.ExpertiseRating * DPSWarr.ExpertiseRatingToExpertise + BasicStats.Expertise,
                                BasicStats.ExpertiseRating, Expertise,combatFactors.MhExpertise,combatFactors.OhExpertise));
            // DPS ind
            dictValues.Add("Bloodsurge", skillAttacks.Bloodsurge().ToString());
            dictValues.Add("Bloodthirst",skillAttacks.Bloodthirst().ToString());
            dictValues.Add("Whirlwind",skillAttacks.Whirlwind().ToString());
            dictValues.Add("Mortal Strike", skillAttacks.MortalStrike().ToString());
            dictValues.Add("Slam",skillAttacks.Slam().ToString());
            dictValues.Add("Rend",skillAttacks.Rend().ToString());
            dictValues.Add("Sudden Death",skillAttacks.SuddenDeath().ToString());
            dictValues.Add("Overpower",skillAttacks.Overpower().ToString());
            dictValues.Add("Bladestorm",skillAttacks.BladeStorm().ToString());
            dictValues.Add("Sword Spec",skillAttacks.SwordSpec().ToString());
            // DPS
            dictValues.Add("Heroic Strike", skillAttacks.HeroicStrike().ToString());
            dictValues.Add("Deep Wounds", skillAttacks.Deepwounds().ToString());
            dictValues.Add("White DPS",(whiteAttacks.CalcMhWhiteDPS() + whiteAttacks.CalcOhWhiteDPS()).ToString());
            dictValues.Add("Total DPS",(whiteAttacks.CalcMhWhiteDPS() + whiteAttacks.CalcOhWhiteDPS() + skillAttacks.Bloodthirst() + skillAttacks.Whirlwind() +
                                       skillAttacks.HeroicStrike() + skillAttacks.Bloodsurge() + skillAttacks.Deepwounds() +
                                       skillAttacks.MortalStrike() + skillAttacks.SuddenDeath() + skillAttacks.Slam() + skillAttacks.Overpower() + skillAttacks.Rend() + skillAttacks.SwordSpec() + skillAttacks.BladeStorm()).ToString());
            // Rage
            dictValues.Add("Free Rage", skillAttacks.freeRage().ToString());
            dictValues.Add("White DPS Rage",whiteAttacks.whiteRageGen().ToString());
            dictValues.Add("Other Rage", skillAttacks.OtherRage().ToString());
            
            return dictValues;
        }
        public override float GetOptimizableCalculationValue(string calculation) {
            switch (calculation) {
                case "Health": return BasicStats.Health;
                case "Haste Rating": return BasicStats.HasteRating;
                case "Expertise Rating": return BasicStats.ExpertiseRating;
                case "Hit Rating": return BasicStats.HitRating;
                case "Crit Rating": return BasicStats.CritRating;
                case "Agility": return BasicStats.Agility;
                case "Attack Power": return BasicStats.AttackPower;
            }
            return 0.0f;
        }
    }
}
