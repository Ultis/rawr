using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DPSWarr {
    public class CharacterCalculationsDPSWarr : CharacterCalculationsBase {
        #region Variables
        public Stats BasicStats { get; set; }
        public CombatFactors combatFactors { get; set; }
        public Rotation Rot { get; set; }
        public List<Buff> ActiveBuffs { get; set; }
        #endregion

        #region Points
        private float _overallPoints = 0f;
        public override float OverallPoints { get { return _overallPoints; } set { _overallPoints = value; } }
        private float[] _subPoints = new float[] { 0f };
        public override float[] SubPoints { get { return _subPoints; } set { _subPoints = value; } }
        public float TotalDPS { get { return _subPoints[0]; } set { _subPoints[0] = value; } }
        public float TotalDPS2 { get; set; }
        #endregion

        #region display values
        public int TargetLevel { get; set; }
        public float Duration { get; set; }
        public string floorstring { get; set; }
        #region Attack Table
        public float Miss { get; set; }
        public float HitRating { get; set; }
        //public float HitPercent { get; set; }
        //public float HitPercBonus { get; set; }
        //public float HitPercentTtl { get; set; }
        //public float HitCanFree { get; set; }
        public float ExpertiseRating { get; set; }
        public float Expertise { get; set; }
        public float MhExpertise { get; set; }
        public float OhExpertise { get; set; }
        public float WeapMastPerc { get; set; }
        public float AgilityCritBonus { get; set; }
        public float CritRating { get; set; }
        public float CritPercent { get; set; }
        public float MhCrit { get; set; }
        public float OhCrit { get; set; }
        #endregion
        #region Offensive
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
        #endregion
        #region DPS
        // White
        public Skills.WhiteAttacks Whites { get; set; }
        public float WhiteDPS { get; set; }
        public float WhiteDmg { get; set; }
        public float WhiteDPSMH { get; set; }
        public float WhiteDPSOH { get; set; }
        public float TotalDamagePerSecond { get; set; }
        #endregion
        #region Abilities
        // Anti-Debuff
        public Skills.HeroicFury HF { get; set; }
        public Skills.EveryManForHimself EM { get; set; }
        // Rage Generators
        public Skills.SecondWind SndW { get; set; }
        public Skills.BerserkerRage BZ { get; set; }
        public Skills.Bloodrage BR { get; set; }
        // Maintenance
        public Skills.BattleShout BTS { get; set; }
        public Skills.CommandingShout CS { get; set; }
        public Skills.DemoralizingShout DS { get; set; }
        public Skills.SunderArmor SN { get; set; }
        public Skills.ThunderClap TH { get; set; }
        public Skills.Hamstring HMS { get; set; }
        // Periodics
        public Skills.ShatteringThrow ST { get; set; }
        public Skills.SweepingStrikes SW { get; set; }
        public Skills.DeathWish Death { get; set; }
        public Skills.Recklessness RK { get; set; }
        public Skills.Trinket1 Trinket1 { get; set; }
        public Skills.Trinket2 Trinket2 { get; set; }
        // Fury
        public Skills.WhirlWind WW { get; set; }
        public Skills.BloodThirst BT { get; set; }
        public Skills.BloodSurge BS { get; set; }
        // Arms
        public Skills.Bladestorm BLS { get; set; }
        public Skills.MortalStrike MS { get; set; }
        public Skills.Rend RD { get; set; }
        public Skills.OverPower OP { get; set; }
        public Skills.TasteForBlood TB { get; set; }
        public Skills.Suddendeath SD { get; set; }
        public Skills.Slam SL { get; set; }
        public Skills.Swordspec SS { get; set; }
        // Generic
        public Skills.Cleave CL { get; set; }
        public Skills.DeepWounds DW { get; set; }
        public Skills.HeroicStrike HS { get; set; }
        public Skills.OnAttack Which { get; set; }
        #endregion
        #region Neutral
        public float BaseHealth { get; set; }
        public float WhiteRage { get; set; }
        public float OtherRage { get; set; }
        public float NeedyRage { get; set; }
        public float FreeRage { get; set; }
        public float Stamina { get; set; }
        public float Health { get; set; }
        #endregion
        #region Defensive
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
        #endregion
        #endregion

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues() {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();
            string format = "";

            // Base Stats
            dictValues.Add("Health and Stamina", string.Format("{0:##,##0} : {1:##,##0}*{2:00,000} : Base Health" +
                                Environment.NewLine + "{3:00,000} : Stam Bonus",
                                BasicStats.Health, BasicStats.Stamina, BaseHealth, StatConversion.GetHealthFromStamina(BasicStats.Stamina)));
            dictValues.Add("Armor",string.Format("{0}*Increases Attack Power by {1}",Armor,TeethBonus));
            dictValues.Add("Strength",string.Format("{0}*Increases Attack Power by {1}",BasicStats.Strength,BasicStats.Strength*2f));
            dictValues.Add("Attack Power", string.Format("{0}*Increases DPS by {1:0.0}", (int)BasicStats.AttackPower,BasicStats.AttackPower/14f));
            dictValues.Add("Agility",string.Format("{0}*3.192% : Base Crit at lvl 80"+
                                Environment.NewLine + "{1:0.000%} : Crit Increase"+
                                Environment.NewLine + "{2:0.000%} : Total Crit Increase"+
                                Environment.NewLine + "Increases Armor by {3:0}",
                                BasicStats.Agility, AgilityCritBonus, AgilityCritBonus + 0.03192f,
                                StatConversion.GetArmorFromAgility(BasicStats.Agility)));
            dictValues.Add("Crit", string.Format("{0:00.00%} : {1}*" +
                                                      "{2:00.00%} : Rating " +
                                Environment.NewLine + "{3:00.00%} : MH Crit" +
                                Environment.NewLine + "{4:00.00%} : OH Crit" +
                                Environment.NewLine + "Target level affects this" +
                                Environment.NewLine + "LVL 80 will match tooltip in game" +
                                Environment.NewLine + "LVL 83 has a total of ~4.8% drop",
                                CritPercent, BasicStats.CritRating,
                                StatConversion.GetCritFromRating(BasicStats.CritRating),
                                MhCrit, OhCrit));
            dictValues.Add("Haste", string.Format("{0:00.00%} : {1}*" +
                                "The percentage is affected both by Haste Rating and Blood Frenzy talent",
                                HastePercent, BasicStats.HasteRating));
            dictValues.Add("Armor Penetration", string.Format("{0:00.00%} : {1}*" +
                                                      "{2:0.00%} : Rating" +
                                Environment.NewLine + "{3:0.00%} : Arms Stance" +
                                Environment.NewLine + "{4:0.00%} : Mace Spec",
                                ArmorPenetration,
                                BasicStats.ArmorPenetrationRating, ArmorPenetrationRating2Perc,
                                ArmorPenetrationStance,
                                ArmorPenetrationMaceSpec));
            // old
            float HitPercent = StatConversion.GetHitFromRating(HitRating);
            float HitPercBonus = BasicStats.PhysicalHit;
            //HitPercentTtl = HitPercent + HitPercBonus;
            /*HitCanFree =
                StatConversion.GetRatingFromHit(
                    combatFactors.WhMissCap //StatConversion.WHITE_MISS_CHANCE_CAP
                    - HitPercBonus
                    - StatConversion.GetHitFromRating(HitRating)
                )
                * -1f;*/
            // Hit Soft Cap ratings check, how far from it
            float capA1         = StatConversion.WHITE_MISS_CHANCE_CAP[combatFactors.CalcOpts.TargetLevel - combatFactors.Char.Level];
            float convcapA1     = (float)Math.Ceiling(StatConversion.GetRatingFromHit(capA1));
            float sec2lastNumA1 = (convcapA1 - StatConversion.GetRatingFromHit(HitPercent) - StatConversion.GetRatingFromHit(HitPercBonus)) * -1;
            //float lastNumA1    = StatConversion.GetRatingFromExpertise((convcapA1 - Math.Min(MhExpertise, (OhExpertise != 0 ? OhExpertise : MhExpertise))) * -1);
            // Hit Hard Cap ratings check, how far from it
            float capA2         = StatConversion.WHITE_MISS_CHANCE_CAP_DW[combatFactors.CalcOpts.TargetLevel - combatFactors.Char.Level];
            float convcapA2     = (float)Math.Ceiling(StatConversion.GetRatingFromHit(capA2));
            float sec2lastNumA2 = (convcapA2 - StatConversion.GetRatingFromHit(HitPercent) - StatConversion.GetRatingFromHit(HitPercBonus)) * -1;
            //float lastNumA2   = StatConversion.GetRatingFromExpertise((sec2lastNumA2 - Math.Min(MhExpertise, (OhExpertise != 0 ? OhExpertise : MhExpertise))) * -1);
            dictValues.Add("Hit",
                string.Format("{0:00.00%} : {1}*" + "{2:0.00%} : From Other Bonuses" +
                                Environment.NewLine + "{3:0.00%} : Total Hit % Bonus" +
                                Environment.NewLine + Environment.NewLine + "White Two-Hander Cap: " +
                                (sec2lastNumA1 > 0 ? "You can free {4:0} Rating"
                                                   : "You need {4:0} more Rating") +
                                Environment.NewLine + "White Dual Wield Cap: " +
                                (sec2lastNumA2 > 0 ? "You can free {5:0} Rating"
                                                   : "You need {5:0} more Rating"),
                                StatConversion.GetHitFromRating(BasicStats.HitRating),
                                BasicStats.HitRating,
                                HitPercBonus,
                                HitPercent + HitPercBonus,
                                (sec2lastNumA1 > 0 ? sec2lastNumA1 : sec2lastNumA1 * -1),
                                (sec2lastNumA2 > 0 ? sec2lastNumA2 : sec2lastNumA2 * -1)
                            ));
            // Dodge Cap ratings check, how far from it, uses lesser of MH and OH
            // Also factors in Weapon Mastery
            float capB1         = StatConversion.YELLOW_DODGE_CHANCE_CAP[combatFactors.CalcOpts.TargetLevel - combatFactors.Char.Level] - WeapMastPerc;
            float convcapB1     = (float)Math.Ceiling(StatConversion.GetExpertiseFromDodgeParryReduc(capB1));
            float sec2lastNumB1 = (convcapB1 - Math.Min(MhExpertise, (OhExpertise != 0 ? OhExpertise : MhExpertise))) * -1;
            float lastNumB1     = StatConversion.GetRatingFromExpertise((convcapB1 - WeapMastPerc - Math.Min(MhExpertise, (OhExpertise != 0 ? OhExpertise : MhExpertise))) * -1);
            // Parry Cap ratings check, how far from it, uses lesser of MH and OH
            float capB2         = StatConversion.YELLOW_PARRY_CHANCE_CAP[combatFactors.CalcOpts.TargetLevel - combatFactors.Char.Level];
            float convcapB2     = (float)Math.Ceiling(StatConversion.GetExpertiseFromDodgeParryReduc(capB2));
            float sec2lastNumB2 = (convcapB2 - Math.Min(MhExpertise, (OhExpertise != 0 ? OhExpertise : MhExpertise))) * -1;
            float lastNumB2     = StatConversion.GetRatingFromExpertise((convcapB2 - Math.Min(MhExpertise, (OhExpertise != 0 ? OhExpertise : MhExpertise))) * -1);
            dictValues.Add("Expertise",
                string.Format("{0:00.00%} : {1:00.00} : {2}*" +
                                                      "Following includes Racial bonus and Strength of Arms" +
                                Environment.NewLine + "{3:00.00%} Weapon Mastery (Dodge Only)" +
                                Environment.NewLine + "{4:00.00%} : {5:00.00} : MH" +
                                Environment.NewLine + "{6:00.00%} : {7:00.00} : OH" +
                                Environment.NewLine + Environment.NewLine + "Dodge Cap: " +
                                (lastNumB1 > 0 ? "You can free {8:0} Expertise ({9:0} Rating)"
                                             : "You need {8:0} more Expertise ({9:0} Rating)") +
                                Environment.NewLine + "Parry Cap: " +
                                (lastNumB2 > 0 ? "You can free {10:0} Expertise ({11:0} Rating)"
                                             : "You need {10:0} more Expertise ({11:0} Rating)"),
                                StatConversion.GetDodgeParryReducFromExpertise(Expertise),
                                Expertise,
                                BasicStats.ExpertiseRating,
                                WeapMastPerc,
                                StatConversion.GetDodgeParryReducFromExpertise(MhExpertise), MhExpertise,
                                StatConversion.GetDodgeParryReducFromExpertise(OhExpertise), OhExpertise,
                                (sec2lastNumB1 > 0 ? sec2lastNumB1 : sec2lastNumB1 * -1), (lastNumB1 > 0 ? lastNumB1 : lastNumB1 * -1),
                                (sec2lastNumB2 > 0 ? sec2lastNumB2 : sec2lastNumB2 * -1), (lastNumB2 > 0 ? lastNumB2 : lastNumB2 * -1)
                            ));

            // DPS Fury
            format = "{0:0000} : {1:0000} : {2:000.00}";
            dictValues.Add("Description",       string.Format("DPS : PerHit : #ActsD"));
            dictValues.Add("Bloodsurge",        string.Format(format,Rot._BS_DPS ,BS.DamageOnUse ,Rot._BS_GCDs    )+BS.GenTooltip( Rot._BS_GCDs, Rot._BS_DPS /TotalDPS));
            dictValues.Add("Bloodthirst",       string.Format(format,Rot._BT_DPS ,BT.DamageOnUse ,Rot._BT_GCDs    )+BT.GenTooltip( Rot._BT_GCDs, Rot._BT_DPS /TotalDPS));
            dictValues.Add("Whirlwind",         string.Format(format,Rot._WW_DPS ,WW.DamageOnUse ,Rot._WW_GCDs    )+WW.GenTooltip( Rot._WW_GCDs, Rot._WW_DPS /TotalDPS));
            // DPS Arms
            format = "{0:0000} : {1:0000} : {2:" + floorstring + "}";
            dictValues.Add("Bladestorm",        string.Format(format,Rot._BLS_DPS,BLS.DamageOnUse/6f,Rot._BLS_GCDs)+BLS.GenTooltip(Rot._BLS_GCDs,Rot._BLS_DPS/TotalDPS));
            dictValues.Add("Mortal Strike",     string.Format(format,Rot._MS_DPS ,MS.DamageOnUse ,Rot._MS_GCDs    )+MS.GenTooltip( Rot._MS_GCDs, Rot._MS_DPS /TotalDPS));
            dictValues.Add("Rend",              string.Format(format,Rot._RD_DPS ,RD.TickSize    ,Rot._RD_GCDs    )+RD.GenTooltip( Rot._RD_GCDs, Rot._RD_DPS /TotalDPS));
            dictValues.Add("Overpower",         string.Format(format,Rot._OP_DPS ,OP.DamageOnUse ,Rot._OP_GCDs    )+OP.GenTooltip( Rot._OP_GCDs, Rot._OP_DPS /TotalDPS));
            dictValues.Add("Taste for Blood",   string.Format(format,Rot._TB_DPS ,TB.DamageOnUse ,Rot._TB_GCDs    )+TB.GenTooltip( Rot._TB_GCDs, Rot._TB_DPS /TotalDPS));
            dictValues.Add("Sudden Death",      string.Format(format,Rot._SD_DPS ,SD.DamageOnUse ,Rot._SD_GCDs    )+SD.GenTooltip( Rot._SD_GCDs, Rot._SD_DPS /TotalDPS));
            dictValues.Add("Slam",              string.Format(format,Rot._SL_DPS ,SL.DamageOnUse ,Rot._SL_GCDs    )+SL.GenTooltip( Rot._SL_GCDs, Rot._SL_DPS /TotalDPS));
            dictValues.Add("Sword Spec",        string.Format(format,Rot._SS_DPS ,SS.DamageOnUse ,Rot._SS_Acts    )+SS.GenTooltip( Rot._SS_Acts, Rot._SS_DPS /TotalDPS));
            // DPS Maintenance
            format = "{0:0000} : {1:0000} : {2:" + floorstring + "}";
            dictValues.Add("Thunder Clap",      string.Format(format,Rot._TH_DPS ,TH.DamageOnUse ,Rot._Thunder_GCDs)+TH.GenTooltip(Rot._Thunder_GCDs, Rot._TH_DPS /TotalDPS));
            dictValues.Add("Shattering Throw",  string.Format(format,Rot._Shatt_DPS,ST.DamageOnUse,Rot._Shatt_GCDs )+ST.GenTooltip(Rot._Shatt_GCDs, Rot._Shatt_DPS/TotalDPS));
            // DPS General
            dictValues.Add("Deep Wounds",       string.Format("{0:0000}*{1:00.0%} of DPS",Rot._DW_DPS     ,Rot._DW_DPS/TotalDPS));
            dictValues.Add("Heroic Strike",     string.Format(format, HS.DPS, HS.DamageOnUse, HS.Activates, HS.DPS / TotalDPS));
            dictValues.Add("Cleave",            string.Format(format, CL.DPS, CL.DamageOnUse, CL.Activates, CL.DPS / TotalDPS));
            dictValues.Add("White DPS",         string.Format("{0:0000} : {1:0000}", WhiteDPS, WhiteDmg)+Whites.GenTooltip(WhiteDPSMH / TotalDPS, WhiteDPSOH / TotalDPS));
            dictValues.Add("Total DPS",         string.Format("{0:#,##0} : {1:#,###,##0}*"+Rot.GCDUsage,TotalDPS,TotalDPS*Duration));
            // Rage
            format = "{0:00.000}";
            dictValues.Add("Total Generated Rage",      string.Format("{0:00.000} = {1:0.000} + {2:0.000}",WhiteRage+OtherRage,WhiteRage,OtherRage));
            dictValues.Add("Needed Rage for Abilities", string.Format(format,NeedyRage));
            dictValues.Add("Available Free Rage",       string.Format(format,FreeRage ));
            
            return dictValues;
        }

        public override float GetOptimizableCalculationValue(string calculation) {
            switch (calculation) {
                case "Health": return BasicStats.Health;
                case "Armor": return BasicStats.Armor + BasicStats.BonusArmor;
                case "Strength": return BasicStats.Strength;
                case "Attack Power": return BasicStats.AttackPower;
                case "Agility": return BasicStats.Agility;
                case "Crit %": return combatFactors._c_mhycrit * 100f;
                case "Haste %": return combatFactors.TotalHaste * 100f;
                case "Armor Penetration %": return BasicStats.ArmorPenetration * 100f;
                case "% Chance to Miss (White)": return combatFactors._c_wmiss * 100f;
                case "% Chance to Miss (Yellow)": return combatFactors._c_ymiss * 100f;
                case "% Chance to be Dodged": return combatFactors._c_mhdodge * 100f;
                case "% Chance to be Parried": return combatFactors._c_mhparry * 100f;
                case "% Chance to be Avoided (Yellow/Dodge)": return combatFactors._c_ymiss * 100f + combatFactors._c_mhdodge * 100f;
            }
            return 0.0f;
        }
    }
}
