using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtWarr {
    public class CharacterCalculationsProtWarr : CharacterCalculationsBase {
        #region Variables
        public Stats BasicStats { get; set; }
        public List<Buff> ActiveBuffs { get; set; }
        public AbilityModelList Abilities { get; set; }
        public CombatFactors combatFactors { get; set; }
        #endregion

        #region Points
        private float _overallPoints = 0f;
        public override float OverallPoints {
            get { return _overallPoints; }
            set { _overallPoints = value; }
        }

        private float[] _subPoints = new float[] { 0f, 0f, 0f };
        public override float[] SubPoints {
            get { return _subPoints; }
            set { _subPoints = value; }
        }

        public float SurvivalPoints {
            get { return _subPoints[0]; }
            set { _subPoints[0] = value; }
        }

        public float MitigationPoints {
            get { return _subPoints[1]; }
            set { _subPoints[1] = value; }
        }

        public float ThreatPoints {
            get { return _subPoints[2]; }
            set { _subPoints[2] = value; }
        }
        #endregion

        #region Display Values
        public float AgilityCritBonus { get; set; }

        public int TargetLevel { get; set; }
        public int RankingMode { get; set; }
        public float ThreatScale { get; set; }

        public float Defense { get; set; } 
        public float Dodge { get; set; }
        public float Parry { get; set; }
        public float Block { get; set; }
        public float BlockValue { get; set; }
        public float Miss { get; set; }
        public float CritReduction { get; set; }
        public float CritVulnerability { get; set; }
        public float ArmorReduction { get; set; }
        public float GuaranteedReduction { get; set; }
        public float DodgePlusMissPlusParry { get; set; }
        public float DodgePlusMissPlusParryPlusBlock { get; set; }
        public float TotalMitigation { get; set; }
        public float BaseAttackerSpeed { get; set; }
        public float AttackerSpeed { get; set; }
        public float DamageTaken { get; set; }
        public float DamageTakenPerHit { get; set; }
        public float DamageTakenPerBlock { get; set; }
        public float DamageTakenPerCrit { get; set; }

        public float TankPoints { get; set; }
        public float BurstTime { get; set; }

        public float NatureReduction { get; set; }
        public float FrostReduction { get; set; }
        public float FireReduction { get; set; }
        public float ShadowReduction { get; set; }
        public float ArcaneReduction { get; set; }
        public float NatureSurvivalPoints { get; set; }
        public float FrostSurvivalPoints { get; set; }
        public float FireSurvivalPoints { get; set; }
        public float ShadowSurvivalPoints { get; set; }
        public float ArcaneSurvivalPoints { get; set; }

        public float MissedAttacks { get; set; }
        public float AvoidedAttacks { get; set; }
        public float DodgedAttacks { get; set; }
        public float ParriedAttacks { get; set; }
        public float BlockedAttacks { get; set; }
        public float HitRating { get; set; }
        public float HitPercent { get; set; }
        public float HitPercBonus { get; set; }
        public float HitPercentTtl { get; set; }
        public float HitCanFree { get; set; }
        public float ExpertiseRating { get; set; }
        public float Expertise { get; set; }
        public float MhExpertise { get; set; }
        public float OhExpertise { get; set; }
        public float WeapMastPerc { get; set; }
        public float Crit { get; set; }
        public float CritPercent { get; set; }
        public float MhCrit { get; set; }
        public float OhCrit { get; set; }
        public float HasteRating { get; set; }
        public float HastePercent { get; set; }
        public float ArmorPenetration { get; set; }
        public float WeaponSpeed { get; set; }
        public float TotalDamagePerSecond { get; set; }
        public float TeethBonus { get; set; }
        
        public float LimitedThreat { get; set; }
        public float UnlimitedThreat { get; set; }
        public string ThreatModel { get; set; }

        public float BaseHealth { get; set; }
        #endregion

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues() {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();
            //string format = "";

            // Base Stats
            dictValues.Add("Health and Stamina", string.Format("{0:##,##0} : {1:##,##0}*{2:00,000} : Base Health" +
                                Environment.NewLine + "{3:00,000} : Stam Bonus",
                                BasicStats.Health, BasicStats.Stamina, BaseHealth, StatConversion.GetHealthFromStamina(BasicStats.Stamina)));
            dictValues.Add("Armor", string.Format("{0}*Reduces physical damage taken by {1:0.00%}" +
                                Environment.NewLine + "Increases Attack Power by {2}",
                                BasicStats.Armor, ArmorReduction, TeethBonus));
            dictValues.Add("Strength", string.Format("{0}*Increases Attack Power by {1}", BasicStats.Strength, BasicStats.Strength * 2));
            dictValues.Add("Attack Power", string.Format("{0}*Increases DPS by {1:0.0}", (int)BasicStats.AttackPower, BasicStats.AttackPower / 14f));
            dictValues.Add("Agility", string.Format("{0}*3.192% : Base Crit at lvl 80" +
                                Environment.NewLine + "{1:0.000%} : Crit Increase" +
                                Environment.NewLine + "{2:0.000%} : Total Crit Increase" +
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
            dictValues.Add("Armor Penetration", string.Format("{0:00.00%} : {1}",ArmorPenetration,BasicStats.ArmorPenetrationRating));
            dictValues.Add("Hit",
                string.Format("{0:00.00%} : {1}*" + "{2:0.00%} : From Other Bonuses" +
                                Environment.NewLine + "{3:0.00%} : Total Hit % Bonus" +
                                Environment.NewLine + Environment.NewLine +
                                (HitCanFree > 0 ? "You can free {4:0} Rating (from yellow cap)"
                                                : "You need {4:0} more Rating (to yellow cap)"),
                                StatConversion.GetHitFromRating(BasicStats.HitRating),
                                BasicStats.HitRating,
                                HitPercBonus,
                                HitPercentTtl,
                                (HitCanFree > 0 ? HitCanFree : HitCanFree * -1)));
            float sec2lastNum = (StatConversion.GetExpertiseFromDodgeParryReduc(StatConversion.WHITE_DODGE_CHANCE_CAP[TargetLevel-80]) - Math.Min(MhExpertise, (OhExpertise != 0 ? OhExpertise : MhExpertise))) * -1;
            float lastNum = StatConversion.GetRatingFromExpertise((StatConversion.GetExpertiseFromDodgeParryReduc(StatConversion.WHITE_DODGE_CHANCE_CAP[TargetLevel-80]) - Math.Min(MhExpertise, (OhExpertise != 0 ? OhExpertise : MhExpertise))) * -1);
            dictValues.Add("Expertise",
                string.Format("{0:00.00%} : {1:00.00} : {2}*" +
                                                      "Following includes Racial bonus and Strength of Arms" +
                                Environment.NewLine + "{3:00.00%} : {4:00.00} : MH" +
                                Environment.NewLine + "{5:00.00%} : {6:00.00} : OH" +
                                Environment.NewLine + "{7:00.00%} Weapon Mastery (Dodge Only)" +
                                Environment.NewLine + Environment.NewLine +
                                (lastNum > 0 ? "You can free {8:0} Expertise ({9:0} Rating)"
                                             : "You need {8:0} more Expertise ({9:0} Rating)"),
                                StatConversion.GetDodgeParryReducFromExpertise(Expertise),
                                Expertise,
                                BasicStats.ExpertiseRating,
                                StatConversion.GetDodgeParryReducFromExpertise(MhExpertise), MhExpertise,
                                StatConversion.GetDodgeParryReducFromExpertise(OhExpertise), OhExpertise,
                                WeapMastPerc,
                                (sec2lastNum > 0 ? sec2lastNum : sec2lastNum * -1),
                                (lastNum > 0 ? lastNum : lastNum * -1)));
            dictValues.Add("Defense", string.Format("{0:0} : {1}", Defense, BasicStats.DefenseRating));
            dictValues.Add("Resilience",
                string.Format(@"{0}*Reduces periodic damage and chance to be critically hit by {1}%." + Environment.NewLine +
                                "Reduces the effect of mana-drains and the damage of critical strikes by {2}%.",
                                BasicStats.Resilience,
                                StatConversion.GetCritReducFromResilience(BasicStats.Resilience),
                                StatConversion.GetCritReducFromResilience(BasicStats.Resilience) * 2f));
            dictValues.Add("Block Value", string.Format("{0}", BlockValue));
            // Defensive Table
            dictValues.Add("Chance Attacker Misses", string.Format("{0:00.00%}", Miss));
            dictValues.Add("Chance to Dodge", string.Format("{0:00.00%} : {1}", Dodge, BasicStats.DodgeRating));
            dictValues.Add("Chance to Parry", string.Format("{0:00.00%} : {1}", Parry, BasicStats.ParryRating));
            dictValues.Add("Chance to Block", string.Format("{0:00.00%} : {1}", Block, BasicStats.BlockRating));
            if (CritVulnerability > 0.0001f) {
                float defenseNeeded = (float)Math.Ceiling((CritVulnerability / (StatConversion.DEFENSE_RATING_AVOIDANCE_MULTIPLIER / 100.0f)) / StatConversion.RATING_PER_DEFENSE);
                float resilienceNeeded = (float)Math.Ceiling(CritVulnerability / (StatConversion.CRITREDUC_PER_RESILIENCE / 100.0f));
                dictValues.Add("Chance Attacker Crits",
                    string.Format("{0:00.00%}*CRITTABLE! Short by {1:0} defense or {2:0} resilience to be uncrittable.",
                                    CritVulnerability, defenseNeeded, resilienceNeeded));
            }else{dictValues.Add("Chance Attacker Crits", string.Format("{0:00.00%}*Chance to crit be reduced by {1:0.00%}", CritVulnerability, CritReduction));}
            dictValues.Add("Chance Attacker Hits", string.Format("{0:00.00%}", 1f - Miss - Dodge - Parry - Block - CritVulnerability));
            dictValues.Add("Total Avoidance", string.Format("{0:00.00%} (+Block {1:00.00%})", Miss + Dodge + Parry,  Miss + Dodge + Parry + Block));
            // Defensive Stats
            dictValues.Add("Damage Taken", string.Format("{0:0.0} DPS*{1:0} damage per normal attack" +
                                               Environment.NewLine + "{2:0} damage per blocked attack" +
                                               Environment.NewLine + "{3:0} damage per critical attack",
                                               DamageTaken, DamageTakenPerHit, DamageTakenPerBlock, DamageTakenPerCrit));
            dictValues.Add("Guaranteed Reduction", string.Format("{0:0.00%}", GuaranteedReduction));
            dictValues.Add("Total Mitigation", string.Format("{0:0.00%}", TotalMitigation));
            // Offensive Stats
            dictValues.Add("Chance to Not Land", string.Format("{0:00.00%}*{1:00.00%} : Missed" +
                                                    Environment.NewLine + "{2:00.00%} : Dodged" +
                                                    Environment.NewLine + "{3:00.00%} : Parried" +
                                                    Environment.NewLine + "{4:00.00%} : Blocked",
                                                    (combatFactors.YwMissCap - HitPercentTtl) + DodgedAttacks + ParriedAttacks + 0.065f,
                                                     combatFactors.YwMissCap - HitPercentTtl, DodgedAttacks, ParriedAttacks, 0.065f));
            dictValues.Add("Total DPS", string.Format("{0:0.0}", TotalDamagePerSecond) + "*" + ThreatModel);
            dictValues.Add("Limited Threat/sec", string.Format("{0:0.0}", LimitedThreat) + "*" + ThreatModel);
            dictValues.Add("Unlimited Threat/sec", string.Format("{0:0.0}", UnlimitedThreat) + "*" + ThreatModel);
            // Resistances
            dictValues.Add("Nature Resist", string.Format("{0:0}*{1:0.00%} Total Reduction in Defensive Stance", BasicStats.NatureResistance + BasicStats.AllResist, NatureReduction));
            dictValues.Add("Fire Resist", string.Format("{0:0}*{1:0.00%} Total Reduction in Defensive Stance", BasicStats.FireResistance + BasicStats.AllResist, FireReduction));
            dictValues.Add("Frost Resist", string.Format("{0:0}*{1:0.00%} Total Reduction in Defensive Stance", BasicStats.FrostResistance + BasicStats.AllResist, FrostReduction));
            dictValues.Add("Shadow Resist", string.Format("{0:0}*{1:0.00%} Total Reduction in Defensive Stance", BasicStats.ShadowResistance + BasicStats.AllResist, ShadowReduction));
            dictValues.Add("Arcane Resist", string.Format("{0:0}*{1:0.00%} Total Reduction in Defensive Stance", BasicStats.ArcaneResistance + BasicStats.AllResist, ArcaneReduction));
            // Complex Stats
            switch (RankingMode) {
                case 2: 
                    dictValues.Add("Ranking Mode", "TankPoints*The average amount of unmitigated damage which can be taken before dying");
                    dictValues.Add("Survival Points", string.Format("{0:0}*Effective Health", SurvivalPoints));
                    break;
                case 3: 
                    dictValues.Add("Ranking Mode", "Burst Time*The average amount of time between events which have a chance to result in a burst death");
                    dictValues.Add("Survival Points", string.Format("{0:0}*{1:0.00} seconds between events", SurvivalPoints, SurvivalPoints / 100.0f));
                    break;
                case 4:
                    dictValues.Add("Ranking Mode", "Damage Output*The average amount of DPS which can be produced");
                    dictValues.Add("Survival Points", string.Format("{0:0}*Survival is not weighted in this mode", SurvivalPoints, SurvivalPoints / 100.0f));
                    break;
                default: 
                    dictValues.Add("Ranking Mode", "Mitigation Scale*Customizable scale which allows you to weight mitigation vs. effective health.");
                    dictValues.Add("Survival Points", string.Format("{0:0}*Effective Health", SurvivalPoints));
                    break;
            }
            dictValues.Add("Overall Points", string.Format("{0:0}", OverallPoints));
            dictValues.Add("Mitigation Points", string.Format("{0:0}", MitigationPoints));
            dictValues.Add("Threat Points", string.Format("{0:0}", ThreatPoints));
            dictValues["Nature Survival"] = NatureSurvivalPoints.ToString();
            dictValues["Fire Survival"] = FireSurvivalPoints.ToString();
            dictValues["Frost Survival"] = FrostSurvivalPoints.ToString();
            dictValues["Shadow Survival"] = ShadowSurvivalPoints.ToString();
            dictValues["Arcane Survival"] = ArcaneSurvivalPoints.ToString();

            return dictValues;
        }

        public override float GetOptimizableCalculationValue(string calculation) {
            float missw = combatFactors._c_wmiss;  // StatConversion.WHITE_MISS_CHANCE_CAP[83] - HitPercent * 100f;
            float missy = combatFactors._c_ymiss;  // StatConversion.YELLOW_MISS_CHANCE_CAP[83] - HitPercent * 100f;
            float dodge = combatFactors._c_mhdodge;//(StatConversion.YELLOW_DODGE_CHANCE_CAP[83] - StatConversion.GetDodgeParryReducFromExpertise(MhExpertise)) * 100f;
            float parry = combatFactors._c_mhparry;//(StatConversion.YELLOW_PARRY_CHANCE_CAP[83] - StatConversion.GetDodgeParryReducFromExpertise(MhExpertise)) * 100f;

            switch (calculation) {
                case "Health": return BasicStats.Health;
                case "Resilience": return BasicStats.Resilience;
                case "Unlimited TPS": return UnlimitedThreat;
                case "Limited TPS": return LimitedThreat;

                case "% Total Mitigation": return TotalMitigation * 100f;
                case "% Guaranteed Reduction": return GuaranteedReduction * 100f;
                case "% Total Avoidance": return DodgePlusMissPlusParry * 100f;
                case "% Total Avoidance+Block": return DodgePlusMissPlusParryPlusBlock * 100f;
                case "% Chance to be Crit": return ((float)Math.Round(CritVulnerability * 100f, 2));

                case "% Chance to Miss (White)": return missw;
                case "% Chance to Miss (Yellow)": return missy;
                case "% Chance to be Dodged": return dodge;
                case "% Chance to be Parried": return parry;
                case "% Chance to be Avoided (Yellow/Dodge)": return missy + dodge;

                case "Burst Time": return BurstTime;
                case "TankPoints": return TankPoints;
                case "Nature Survival": return NatureSurvivalPoints;
                case "Fire Survival": return FireSurvivalPoints;
                case "Frost Survival": return FrostSurvivalPoints;
                case "Shadow Survival": return ShadowSurvivalPoints;
                case "Arcane Survival": return ArcaneSurvivalPoints;
            }
            return 0f;
        }
    }
}