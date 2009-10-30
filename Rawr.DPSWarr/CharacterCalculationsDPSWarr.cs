using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DPSWarr {
    public class CharacterCalculationsDPSWarr : CharacterCalculationsBase {
        #region Variables
        public Stats AverageStats { get; set; }
        public Stats MaximumStats { get; set; }
        public Stats UnbuffedStats { get; set; }
        public Stats BuffedStats { get; set; }
        public CombatFactors combatFactors { get; set; }
        public Rotation Rot { get; set; }
        public List<Buff> ActiveBuffs { get; set; }
        #endregion

        #region Points
        private float _overallPoints = 0f;
        public override float OverallPoints { get { return _overallPoints; } set { _overallPoints = value; } }
        private float[] _subPoints = new float[] { 0f, 0f };
        public override float[] SubPoints { get { return _subPoints; } set { _subPoints = value; } }
        public float TotalDPS { get { return _subPoints[0]; } set { _subPoints[0] = value; } }
        public float TotalHPS;
        public float Survivability { get { return _subPoints[1]; } set { _subPoints[1] = value; } }
        #endregion

        #region Display Values
        public int TargetLevel { get; set; }
        public float Duration { get; set; }
        public string floorstring { get; set; }
#if (!RAWR3 && DEBUG)
        public long calculationTime { get; set; }
#endif
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
        public float MaxArmorPenetration { get; set; }
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
        public Skills.Charge CH { get; set; }
        public Skills.Intercept IN { get; set; }
        public Skills.Intervene IV { get; set; }
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
        public Skills.EnragedRegeneration ER { get; set; }
        // Periodics
        public Skills.ShatteringThrow ST { get; set; }
        public Skills.SweepingStrikes SW { get; set; }
        public Skills.DeathWish Death { get; set; }
        public Skills.Recklessness RK { get; set; }
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
        public Skills.Execute EX { get; set; }
        // Markov Work
        public Skills.FakeWhite FW { get; set; }
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
                                AverageStats.Health, AverageStats.Stamina, BaseHealth, StatConversion.GetHealthFromStamina(AverageStats.Stamina)));
            dictValues.Add("Armor",string.Format("{0}*Increases Attack Power by {1}",Armor,TeethBonus));
            dictValues.Add("Strength",string.Format("{0}*Increases Attack Power by {1}",AverageStats.Strength,AverageStats.Strength*2f));
            dictValues.Add("Attack Power", string.Format("{0}*Increases DPS by {1:0.0}", (int)AverageStats.AttackPower,AverageStats.AttackPower/14f));
            dictValues.Add("Agility",string.Format("{0}*3.192% : Base Crit at lvl 80"+
                                Environment.NewLine + "{1:0.000%} : Crit Increase"+
                                Environment.NewLine + "{2:0.000%} : Total Crit Increase"+
                                Environment.NewLine + "Increases Armor by {3:0}",
                                AverageStats.Agility, AgilityCritBonus, AgilityCritBonus + 0.03192f,
                                StatConversion.GetArmorFromAgility(AverageStats.Agility)));
            dictValues.Add("Crit", string.Format("{0:00.00%} : {1}*" +
                                                      "{2:00.00%} : Rating " +
                                Environment.NewLine + "{3:00.00%} : MH Crit" +
                                Environment.NewLine + "{4:00.00%} : OH Crit" +
                                Environment.NewLine + "Target level affects this" +
                                Environment.NewLine + "LVL 80 will match tooltip in game" +
                                Environment.NewLine + "LVL 83 has a total of ~4.8% drop",
                                CritPercent, AverageStats.CritRating,
                                StatConversion.GetCritFromRating(AverageStats.CritRating),
                                MhCrit, OhCrit));
            dictValues.Add("Haste", string.Format("{0:00.00%} : {1}*" +
                                "The percentage is affected both by Haste Rating and Blood Frenzy talent",
                                HastePercent, (float)Math.Floor(AverageStats.HasteRating)));
            dictValues.Add("Armor Penetration", string.Format("{0:00.00%} : {2}*" +
                                                      "With Procs: {1:00.00%}" +
                                Environment.NewLine + 
                                Environment.NewLine + "Average Breakdown:" +
                                Environment.NewLine + "{3:0.00%} : Rating (including proc averages)" +
                                Environment.NewLine + "{4:0.00%} : Arms Stance" +
                                Environment.NewLine + "{5:0.00%} : Mace Spec",
                                ArmorPenetration,
                                MaxArmorPenetration,
                                AverageStats.ArmorPenetrationRating, ArmorPenetrationRating2Perc,
                                ArmorPenetrationStance,
                                ArmorPenetrationMaceSpec));
            // old
            float HitPercent = StatConversion.GetHitFromRating(HitRating);
            float HitPercBonus = AverageStats.PhysicalHit;
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
                                StatConversion.GetHitFromRating(AverageStats.HitRating),
                                AverageStats.HitRating,
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
                AverageStats.ExpertiseRating,
                WeapMastPerc,
                StatConversion.GetDodgeParryReducFromExpertise(MhExpertise), MhExpertise,
                StatConversion.GetDodgeParryReducFromExpertise(OhExpertise), OhExpertise,
                (sec2lastNumB1 > 0 ? sec2lastNumB1 : sec2lastNumB1 * -1), (lastNumB1 > 0 ? lastNumB1 : lastNumB1 * -1),
                (sec2lastNumB2 > 0 ? sec2lastNumB2 : sec2lastNumB2 * -1), (lastNumB2 > 0 ? lastNumB2 : lastNumB2 * -1)
            ));

            dictValues.Add("Description", string.Format("DPS : PerHit : #ActsD"));
            // DPS Fury
            format = "{0:0000} : {1:0000} : {2:000.00}";
            if (Rot.GetType() == typeof(FuryRotation)) {
                FuryRotation fr = (FuryRotation)Rot;
                dictValues.Add("Bloodsurge",  string.Format(format, fr._BS_DPS, BS.DamageOnUse, fr._BS_GCDs) + BS.GenTooltip(fr._BS_GCDs, fr._BS_DPS / TotalDPS));
                dictValues.Add("Bloodthirst", string.Format(format, fr._BT_DPS, BT.DamageOnUse, fr._BT_GCDs) + BT.GenTooltip(fr._BT_GCDs, fr._BT_DPS / TotalDPS));
                dictValues.Add("Whirlwind",   string.Format(format, fr._WW_DPS, WW.DamageOnUse, fr._WW_GCDs) + WW.GenTooltip(fr._WW_GCDs, fr._WW_DPS / TotalDPS));
            } else {
                dictValues.Add("Bloodsurge",  string.Format(format, 0, 0, 0));
                dictValues.Add("Bloodthirst", string.Format(format, 0, 0, 0));
                dictValues.Add("Whirlwind",   string.Format(format, 0, 0, 0));
            }
            // DPS Arms
            format = "{0:0000} : {1:0000} : {2:" + floorstring + "}";
            if (Rot.GetType() == typeof(ArmsRotation)) {
                ArmsRotation ar = (ArmsRotation)Rot;
                dictValues.Add("Bladestorm",        string.Format(format,ar._BLS_DPS,BLS.DamageOnUse/6f,ar._BLS_GCDs)+BLS.GenTooltip(ar._BLS_GCDs,ar._BLS_DPS/TotalDPS));
                dictValues.Add("Mortal Strike",     string.Format(format,ar._MS_DPS ,MS.DamageOnUse    ,ar._MS_GCDs )+MS.GenTooltip( ar._MS_GCDs, ar._MS_DPS /TotalDPS));
                dictValues.Add("Rend",              string.Format(format,ar._RD_DPS ,RD.TickSize       ,ar._RD_GCDs )+RD.GenTooltip( ar._RD_GCDs, ar._RD_DPS /TotalDPS));
                dictValues.Add("Overpower",         string.Format(format,ar._OP_DPS ,OP.DamageOnUse    ,ar._OP_GCDs )+OP.GenTooltip( ar._OP_GCDs, ar._OP_DPS /TotalDPS));
                dictValues.Add("Taste for Blood",   string.Format(format,ar._TB_DPS ,TB.DamageOnUse    ,ar._TB_GCDs )+TB.GenTooltip( ar._TB_GCDs, ar._TB_DPS /TotalDPS));
                dictValues.Add("Sudden Death",      string.Format(format,ar._SD_DPS ,SD.DamageOnUse    ,ar._SD_GCDs )+SD.GenTooltip( ar._SD_GCDs, ar._SD_DPS /TotalDPS));
                dictValues.Add("Slam",              string.Format(format,ar._SL_DPS ,SL.DamageOnUse    ,Rot._SL_GCDs)+SL.GenTooltip( ar._SL_GCDs, ar._SL_DPS /TotalDPS));
                dictValues.Add("Sword Spec",        string.Format(format,ar._SS_DPS ,SS.DamageOnUse    ,ar._SS_Acts )+SS.GenTooltip( ar._SS_Acts, ar._SS_DPS /TotalDPS));
            } else {
                dictValues.Add("Bladestorm",        string.Format(format, 0, 0, 0));
                dictValues.Add("Mortal Strike",     string.Format(format, 0, 0, 0));
                dictValues.Add("Rend",              string.Format(format, 0, 0, 0));
                dictValues.Add("Overpower",         string.Format(format, 0, 0, 0));
                dictValues.Add("Taste for Blood",   string.Format(format, 0, 0, 0));
                dictValues.Add("Sudden Death",      string.Format(format, 0, 0, 0));
                dictValues.Add("Slam",              string.Format(format, 0, 0, 0));
                dictValues.Add("Sword Spec",        string.Format(format, 0, 0, 0));
            }
            // DPS Maintenance
            format = "{0:0000} : {1:0000} : {2:" + floorstring + "}";
            dictValues.Add("Thunder Clap",          string.Format(format,Rot._TH_DPS ,TH.DamageOnUse ,Rot._Thunder_GCDs)+TH.GenTooltip(Rot._Thunder_GCDs, Rot._TH_DPS /TotalDPS));
            dictValues.Add("Shattering Throw",      string.Format(format,Rot._Shatt_DPS,ST.DamageOnUse,Rot._Shatt_GCDs )+ST.GenTooltip(Rot._Shatt_GCDs, Rot._Shatt_DPS/TotalDPS));
            // DPS General
            dictValues.Add("Deep Wounds",           string.Format("{0:0000}*{1:00.0%} of DPS",Rot._DW_DPS     ,Rot._DW_DPS/TotalDPS));
            dictValues.Add("Heroic Strike",         string.Format(format, HS.DPS, HS.DamageOnUse, HS.Activates, HS.DPS / TotalDPS)+HS.GenTooltip(HS.Activates,HS.DPS/TotalDPS));
            dictValues.Add("Cleave",                string.Format(format, CL.DPS, CL.DamageOnUse, CL.Activates, CL.DPS / TotalDPS)+CL.GenTooltip(CL.Activates,CL.DPS/TotalDPS));
            dictValues.Add("White DPS",             string.Format("{0:0000} : {1:0000}", WhiteDPS, WhiteDmg) + Whites.GenTooltip(WhiteDPSMH, WhiteDPSOH, TotalDPS));
            dictValues.Add("Execute",               string.Format(format,Rot._EX_DPS, EX.DamageOnUse, Rot._EX_GCDs) + EX.GenTooltip(Rot._EX_GCDs, Rot._EX_DPS / TotalDPS));
            //
            dictValues.Add("Total DPS",             string.Format("{0:#,##0} : {1:#,###,##0}*"+Rot.GCDUsage,TotalDPS,TotalDPS*Duration));
            // Rage
            format = "{0:0000}";
            dictValues.Add("Total Generated Rage",      string.Format("{0:00} = {1:0} + {2:0}",WhiteRage+OtherRage,WhiteRage,OtherRage));
            dictValues.Add("Needed Rage for Abilities", string.Format(format,NeedyRage));
            dictValues.Add("Available Free Rage",       string.Format(format,FreeRage ));
#if (!RAWR3 && DEBUG)
            dictValues.Add("Calculation Time", string.Format("{0}", calculationTime));
#endif
            return dictValues;
        }

        public override float GetOptimizableCalculationValue(string calculation) {
            switch (calculation) {
                case "Health": return AverageStats.Health;
                case "Armor": return AverageStats.Armor + AverageStats.BonusArmor;
                case "Strength": return AverageStats.Strength;
                case "Attack Power": return AverageStats.AttackPower;
                case "Agility": return AverageStats.Agility;
                case "Crit %": return combatFactors._c_mhycrit * 100f;
                case "Haste %": return combatFactors.TotalHaste * 100f;
                case "Armor Penetration %": return AverageStats.ArmorPenetration * 100f;
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
