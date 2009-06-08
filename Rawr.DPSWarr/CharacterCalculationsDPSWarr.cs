using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DPSWarr
{
    public class CharacterCalculationsDPSWarr : CharacterCalculationsBase
    {
        public Stats BasicStats { get; set; }
        public Skills SkillAttacks { get; set; }
        public Rotation Rot { get; set; }
        public List<Buff> ActiveBuffs { get; set; }
        //public AbilityModelList Abilities { get; set; }

        private float _overallPoints = 0f;
        public override float OverallPoints {
            get { return _overallPoints; }
            set { _overallPoints = value; }
        }

        private float[] _subPoints = new float[] { 0f };
        public override float[] SubPoints {
            get { return _subPoints; }
            set { _subPoints = value; }
        }
        
        public float TotalDPS {
            get { return _subPoints[0];}
            set { _subPoints[0] = value; }
        }

        public float TotalDPS2 { get; set; }

        public int TargetLevel { get; set; }

        public float Duration { get; set; }
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
        public float WhiteDmg { get; set; }
        public float WhiteDPSMH { get; set; }
        public float WhiteDPSOH { get; set; }
        public Skills.Ability DW { get; set; }
        public Skills.Ability HS { get; set; }
        public Skills.Ability SL { get; set; }
        public Skills.DoT RD { get; set; }
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
            string format = "";
            Skills.WhiteAttacks whiteAttacks = new Skills.WhiteAttacks(talents, BasicStats, combatFactors, character);
            if (SkillAttacks == null){SkillAttacks = new Skills(character,talents, BasicStats, combatFactors, whiteAttacks);}

            // Base Stats
            dictValues.Add("Health",string.Format("{0}*Base {1} + Stam Bonus {2}",BasicStats.Health, BaseHealth, StatConversion.GetHealthFromStamina(BasicStats.Stamina)));
            dictValues.Add("Stamina",string.Format("{0}*Increases Health by {1}" ,BasicStats.Stamina,StatConversion.GetHealthFromStamina(BasicStats.Stamina)));
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
                Environment.NewLine + "OH Crit {4:0.00%}" +
                Environment.NewLine + "Boss level affects this" +
                Environment.NewLine + "LVL 80 will match tootlip in game" +
                Environment.NewLine + "83 has a total of ~4.8% drop",
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
                string.Format("{0}*{1:0.00%} Increased Chance to hit" +
                                Environment.NewLine + "Note: This does not include Precision" +
                                Environment.NewLine + Environment.NewLine + "You can free up {2:0} Rating",
                                BasicStats.HitRating,StatConversion.GetHitFromRating(BasicStats.HitRating),
                                StatConversion.GetRatingFromHit(.08f - StatConversion.GetHitFromRating(BasicStats.HitRating))*-1f));
            dictValues.Add("Expertise",
                string.Format("{0:0.00}*Expertise Rating {1}" +
                                Environment.NewLine + "Num Displayed is Rating Converted + Strength of Arms" +
                                Environment.NewLine + "Reduces chance to be dodged or parried by {2:0.00%}." +
                                Environment.NewLine + "Main Hand Exp - {3:00.00} / {4:0.00%} [Includes Racial]" +
                                Environment.NewLine + "Off Hand Exp    - {5:00.00} / {6:0.00%} [Includes Racial]" +
                                Environment.NewLine + Environment.NewLine + "You can free up {7:0} Expertise ({8:0} Rating)",
                                Expertise + BasicStats.Expertise,
                                BasicStats.ExpertiseRating, StatConversion.GetDodgeParryReducFromExpertise(Expertise),
                                MhExpertise, StatConversion.GetDodgeParryReducFromExpertise(MhExpertise),
                                OhExpertise, StatConversion.GetDodgeParryReducFromExpertise(OhExpertise),
                                (StatConversion.GetExpertiseFromDodgeParryReduc(0.065f)-Math.Min(MhExpertise,(OhExpertise!=0?OhExpertise:MhExpertise)))*-1,
                                StatConversion.GetRatingFromExpertise((StatConversion.GetExpertiseFromDodgeParryReduc(0.065f) - Math.Min(MhExpertise, (OhExpertise != 0 ? OhExpertise : MhExpertise))) * -1)));
            // DPS Fury
            format = "{0:0000} : {1:0000} : {2:00.00} : {3:00.0}*{4:00.0%} of DPS";
            dictValues.Add("Description",       string.Format("DPS  : PerHit : RPS : #ActsD"));
            dictValues.Add("Bloodsurge",        string.Format(format,BS.GetDPS(), BS.GetDamageOnUse(), BS.GetRageUsePerSecond() ,BS.GetActivates(),BS.GetDPS()/TotalDPS));
            dictValues.Add("Bloodthirst",       string.Format(format,BT.GetDPS(), BT.GetDamageOnUse(), BT.GetRageUsePerSecond() ,BT.GetActivates(),BT.GetDPS()/TotalDPS));
            dictValues.Add("Whirlwind",         string.Format(format,WW.GetDPS(), WW.GetDamageOnUse(), WW.GetRageUsePerSecond() ,WW.GetActivates(),WW.GetDPS()/TotalDPS));
            // DPS Arms
            format = "{0:0000} : {1:0000} : {2:00.00} : {3:000.00}*{4:00.0%} of DPS";
            dictValues.Add("Mortal Strike",     string.Format(format,Rot._MS_DPS ,MS.GetDamageOnUse() ,Rot._MS_GCDs ,Rot._MS_GCDsD ,Rot._MS_DPS /TotalDPS));
            dictValues.Add("Rend",              string.Format(format,Rot._RD_DPS ,RD.GetTickSize()    ,Rot._RD_GCDs ,Rot._RD_GCDsD ,Rot._RD_DPS /TotalDPS));
            dictValues.Add("Overpower",         string.Format(format,Rot._OP_DPS ,OP.GetDamageOnUse() ,Rot._OP_GCDs ,Rot._OP_GCDsD ,Rot._OP_DPS /TotalDPS));
            dictValues.Add("Sudden Death",      string.Format(format,Rot._SD_DPS ,SD.GetDamageOnUse() ,Rot._SD_GCDs ,Rot._SD_GCDsD ,Rot._SD_DPS /TotalDPS));
            dictValues.Add("Slam",              string.Format(format,Rot._SL_DPS ,SL.GetDamageOnUse() ,Rot._SL_GCDs ,Rot._SL_GCDsD ,Rot._SL_DPS /TotalDPS));
            dictValues.Add("Bladestorm",        string.Format(format,Rot._BLS_DPS,BLS.GetDamageOnUse(),Rot._BLS_GCDs,Rot._BLS_GCDsD,Rot._BLS_DPS/TotalDPS));
            dictValues.Add("Sword Spec",        string.Format(format,SS.GetDPS()          ,SS.GetDamageOnUse() ,SS.GetActivates()     ,SS.GetActivates()      ,SS.GetDPS()          /TotalDPS));
            dictValues.Add("Sweeping Strikes",  string.Format(format,SW.GetDPS()          ,SW.GetDamageOnUse() ,SW.GetActivates()     ,SW.GetActivates()      ,SW.GetDPS()          /TotalDPS));
            // DPS General
            dictValues.Add("Deep Wounds",       string.Format("{0:0000} : {1:0000}*{2:00.0%} of DPS",Rot._DW_DPS, Rot._DW_PerHit,Rot._DW_DPS/TotalDPS));
            dictValues.Add("Heroic Strike",     string.Format(format,HS.GetDPS()          ,HS.GetDamageOnUse() ,HS.heroicStrikeRageCost(),HS.GetActivates(),HS.GetDPS()/TotalDPS));
            dictValues.Add("White DPS",         string.Format("{0:0000} : {1:0000} : {2:00.00} : {3:00.0}*Main Hand-{4:0.00}" + 
                                Environment.NewLine + "Off Hand- {5:0.00}" + 
                                Environment.NewLine + "{6:00.0%} of DPS",
                                WhiteDPS,WhiteDmg,WhiteRage,0f,WhiteDPSMH,WhiteDPSOH,WhiteDPS/TotalDPS));
            dictValues.Add("Total DPS",         string.Format("{0:#,##0} : {1:#,###,##0} : {2:#,###,##0}", TotalDPS,TotalDPS*BT.GetRotation(),TotalDPS*Duration));
            // Rage
            dictValues.Add("Generated White DPS Rage",  string.Format("{0:00.000}",WhiteRage));
            dictValues.Add("Generated Other Rage",      string.Format("{0:00.000}",OtherRage));
            dictValues.Add("Available Free Rage",       string.Format("{0:00.000}",FreeRage));
            
            return dictValues;
        }
        public override float GetOptimizableCalculationValue(string calculation) {
            switch (calculation) {
                case "Health": return BasicStats.Health;
                case "Haste Rating": return BasicStats.HasteRating;
                case "Expertise Rating": return BasicStats.ExpertiseRating;
                case "Hit Rating": return BasicStats.HitRating;
                case "Enemy Avoidance %": return BasicStats.Miss + BasicStats.Dodge;
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
