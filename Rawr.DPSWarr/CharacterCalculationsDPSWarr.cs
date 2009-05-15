using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DPSWarr
{
    public class CharacterCalculationsDPSWarr : CharacterCalculationsBase
    {
        public Stats BasicStats { get; set; }
        public Skills SkillAttacks { get; set; }
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

        public float BaseHealth { get; set; }
        // Attack Table
        public float Miss { get; set; }
        public float HitRating { get; set; }
        public float HitPercent { get; set; }
        public float ExpertiseRating { get; set; }
        public float Expertise { get; set; }
        public float MhExpertise { get; set; }
        public float OhExpertise { get; set; }
        public float AgilityCritBonus { get; set; }
        public float CritRating { get; set; }
        public float CritPercent { get; set; }
        public float MhCrit { get; set; }
        public float OhCrit { get; set; }
        // Offensive
        public float ArmorPenetrationMaceSpec { get; set; }
        public float ArmorPenetrationStance { get; set; }
        public float ArmorPenetrationRating { get; set; }
        public float ArmorPenetrationRating2Perc { get; set; }
        public float ArmorPenetration { get; set; }
        public float buffedArmorPenetration { get; set; }
        public float HasteRating { get; set; }
        public float HastePercent { get; set; }
        public float WeaponSpeed { get; set; }
        public float TeethBonus { get; set; }
        // DPS
        public float WhiteDPS { get; set; }
        public float WhiteDPSMH { get; set; }
        public float WhiteDPSOH { get; set; }
        public Skills.Ability DW { get; set; }
        public Skills.Ability HS { get; set; }
        public Skills.Ability SL { get; set; }
        public Skills.Ability RND { get; set; }
        public Skills.Ability MS { get; set; }
        public Skills.Ability OP { get; set; }
        public Skills.Ability SS { get; set; }
        public Skills.Ability BS { get; set; }
        public Skills.Ability SW { get; set; }
        public Skills.Ability BLS { get; set; }
        public Skills.Ability SD { get; set; }
        public Skills.Ability BT { get; set; }
        public Skills.Ability WW { get; set; }
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

            WhiteAttacks whiteAttacks = new WhiteAttacks(talents, BasicStats, combatFactors);
            if (SkillAttacks == null){SkillAttacks = new Skills(character,talents, BasicStats, combatFactors, whiteAttacks);}

            dictValues.Add("Health",string.Format("{0}*Base {1} + Stam Bonus {2}"
                ,BasicStats.Health, BaseHealth, StatConversion.GetHealthFromStamina(BasicStats.Stamina)));
            dictValues.Add("Stamina",string.Format("{0}*Increases Health by {1}"
                ,BasicStats.Stamina,StatConversion.GetHealthFromStamina(BasicStats.Stamina)));
            dictValues.Add("Armor",string.Format("{0}*Increases Attack Power by {1}",Armor,TeethBonus));
            dictValues.Add("Strength",string.Format("{0}*Increases Attack Power by {1}",BasicStats.Strength,BasicStats.Strength*2));
            dictValues.Add("Attack Power", string.Format("{0}*Increases DPS by {1:0.0}", (int)BasicStats.AttackPower,BasicStats.AttackPower/14));
            dictValues.Add("Agility",string.Format("{0}*Base Crit at lvl 80 3.192%"+
                Environment.NewLine+"Increases Crit by {1:0.00%}"+
                Environment.NewLine+"Total Crit increase of {2:0.00%}"+
                Environment.NewLine+"Increases Armor by {3:0}",
                BasicStats.Agility, AgilityCritBonus, AgilityCritBonus + .03192f, StatConversion.GetArmorFromAgility(BasicStats.Agility)));
            dictValues.Add("Haste",string.Format("{0:0.00%}*Haste Rating {1}", HastePercent, BasicStats.HasteRating));
            dictValues.Add("Crit", string.Format("{0:0.00%}*Crit Rating {1} (+{2:0.00%})" +
                Environment.NewLine + "MH Crit {3:0.00%}" +
                Environment.NewLine + "OH Crit {4:0.00%}",
                CritPercent, BasicStats.CritRating, StatConversion.GetCritFromRating(BasicStats.CritRating), MhCrit, OhCrit));
            dictValues.Add("Armor Penetration",  string.Format("{0:0.00%}*Armor Penetration Rating {1}- {2:0.00%}" +
                Environment.NewLine + "Arms Stance- +{3:0.00%}" +
                Environment.NewLine + "Mace Spec- +{4:0.00%}",
                                ArmorPenetration,
                                BasicStats.ArmorPenetrationRating, ArmorPenetrationRating2Perc,
                                ArmorPenetrationStance,
                                ArmorPenetrationMaceSpec));
            dictValues.Add("Damage Reduction",string.Format("{0:0.00%}",damageReduc));
            dictValues.Add("Hit Rating",
                string.Format("{0}*{1:0.00%} Increased Chance to hit" + Environment.NewLine + "Note: This does not include Precision"
                ,BasicStats.HitRating,StatConversion.GetHitFromRating(BasicStats.HitRating)));
            dictValues.Add("Expertise", 
                string.Format("{0:0.00}*Expertise Rating {1}" +
                                Environment.NewLine + "Num Displayed is Rating Converted + Strength of Arms" +
                                Environment.NewLine + "Reduces chance to be dodged or parried by {2:0.00%}." +
                                Environment.NewLine + "Main Hand Exp- {3:0.00} / {4:0.00%} [Includes Racial]" +
                                Environment.NewLine + "Off Hand Exp- {5:0.00} / {6:0.00%} [Includes Racial]" +
                                Environment.NewLine + Environment.NewLine + "You can free up {7:0} Expertise ({8:0} Rating)",
                                Expertise + BasicStats.Expertise,
                                BasicStats.ExpertiseRating, StatConversion.GetDodgeParryReducFromExpertise(Expertise),
                                MhExpertise, StatConversion.GetDodgeParryReducFromExpertise(MhExpertise),
                                OhExpertise, StatConversion.GetDodgeParryReducFromExpertise(OhExpertise),
                                (StatConversion.GetExpertiseFromDodgeParryReduc(0.065f)-Math.Min(MhExpertise,(OhExpertise!=0?OhExpertise:MhExpertise)))*-1,
                                StatConversion.GetRatingFromExpertise((StatConversion.GetExpertiseFromDodgeParryReduc(0.065f) - Math.Min(MhExpertise, (OhExpertise != 0 ? OhExpertise : MhExpertise))) * -1)));
            // DPS ind
            dictValues.Add("Bloodsurge",    string.Format("{0:0.00} : {1:0.00}",BS.GetDPS(),BS.GetDamageOnUse()));
            dictValues.Add("Bloodthirst",   string.Format("{0:0.00} : {1:0.00}",BT.GetDPS(),BT.GetDamageOnUse()));
            dictValues.Add("Whirlwind",     string.Format("{0:0.00} : {1:0.00}",WW.GetDPS(),WW.GetDamageOnUse()));
            dictValues.Add("Mortal Strike", string.Format("{0:0.00} : {1:0.00}",MS.GetDPS(),MS.GetDamageOnUse()));
            dictValues.Add("Slam",          string.Format("{0:0.00} : {1:0.00}",SL.GetDPS(),SL.GetDamageOnUse()));
            dictValues.Add("Rend",          string.Format("{0:0.00} : {1:0.00}",RND.GetDPS(),RND.GetDamageOnUse()));
            dictValues.Add("Sudden Death",  string.Format("{0:0.00} : {1:0.00}",SD.GetDPS(),SD.GetDamageOnUse()));
            dictValues.Add("Overpower",     string.Format("{0:0.00} : {1:0.00}",OP.GetDPS(),OP.GetDamageOnUse()));
            dictValues.Add("Bladestorm",    string.Format("{0:0.00} : {1:0.00}",BLS.GetDPS(),BLS.GetDamageOnUse()));
            dictValues.Add("Sword Spec",    string.Format("{0:0.00} : {1:0.00}",SS.GetDPS(),SS.GetDamageOnUse()));
            // DPS
            dictValues.Add("Heroic Strike", string.Format("{0:0.00} : {1:0.00}",HS.GetDPS(),HS.GetDamageOnUse()));
            dictValues.Add("Deep Wounds",   string.Format("{0:0.00} : {1:0.00}",DW.GetDPS(),DW.GetDamageOnUse()));
            dictValues.Add("White DPS",     string.Format("{0:0.00}*Main Hand-{1:0.00}"+Environment.NewLine+"Off Hand- {2:0.00}",WhiteDPS, WhiteDPSMH, WhiteDPSOH));
            dictValues.Add("Total DPS",     string.Format("{0:0.00}", TotalDPS));
            // Rage
            dictValues.Add("Generated White DPS Rage", WhiteRage.ToString());
            dictValues.Add("Generated Other Rage", SkillAttacks.OtherRage().ToString());
            dictValues.Add("Ability's Rage Used (BT)", BT.GetAvgRageCost().ToString());
            dictValues.Add("Ability's Rage Used (WW)", WW.GetAvgRageCost().ToString());
            dictValues.Add("Ability's Rage Used (MS)", MS.GetAvgRageCost().ToString());
            dictValues.Add("Ability's Rage Used (OP)", OP.GetAvgRageCost().ToString());
            dictValues.Add("Ability's Rage Used (SD)", SD.GetAvgRageCost().ToString());
            dictValues.Add("Ability's Rage Used (SL)", SL.GetAvgRageCost().ToString());
            dictValues.Add("Ability's Rage Used (BS)", BS.GetAvgRageCost().ToString());
            dictValues.Add("Ability's Rage Used (BLS)", BLS.GetAvgRageCost().ToString());
            dictValues.Add("Ability's Rage Used (SW)", SW.GetAvgRageCost().ToString());
            dictValues.Add("Ability's Rage Used (RND)", RND.GetAvgRageCost().ToString());
            dictValues.Add("Available Free Rage", SkillAttacks.freeRage().ToString());
            
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
                case "Armor Penetration": return BasicStats.ArmorPenetration;
                case "Armor Penetration Rating": return BasicStats.ArmorPenetrationRating;
            }
            return 0.0f;
        }
    }
}
