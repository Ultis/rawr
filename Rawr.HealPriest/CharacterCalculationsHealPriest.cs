using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.HealPriest {
    public class CharacterCalculationsHealPriest : CharacterCalculationsBase
    {
        public StatsPriest BasicStats;
        public Character Character;

        #region Points
        private float _overallPoints = 0f;
        public override float OverallPoints { get { return _overallPoints; } set { _overallPoints = value; } }
        private float[] _subPoints = new float[] { 0f, 0f, 0f };
        public override float[] SubPoints { get { return _subPoints; } set { _subPoints = value; } }
        public float BurstPoints { get { return _subPoints[0]; } set { _subPoints[0] = value; } }
        public float SustainPoints { get { return _subPoints[1]; } set { _subPoints[1] = value; } }
        public float ManaPoints { get { return _subPoints[2]; } set { _subPoints[2] = value; } }
        public float BurstGoal { get; set; }
        public float SustainGoal { get; set; }
        public float ManaGoal { get; set; }
        #endregion

        protected Buff GetActiveBuffsByGroup(string group)
        {
            for (int x = 0; x < Character.ActiveBuffs.Count; x++)
                if (Character.ActiveBuffs[x].Group == group)
                    return Character.ActiveBuffs[x];
            return null;
        }

        private string makeActiveBuffTextPercent(Buff activeBuff, float stat)
        {
            return String.Format("\r\n{0}% from {1} given by {2}", (stat * 100f).ToString("0"), activeBuff.Name, activeBuff.Source);
        }

        private string makeActiveBuffText(Buff activeBuff, float stat)
        {
            return String.Format("\r\n{0} from {1} given by {2}", stat.ToString("0"), activeBuff.Name, activeBuff.Source);
        }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();
            Buff activeBuff;
            string s;
            Stats baseStats = BaseStats.GetBaseStats(Character);

            #region General
            dictValues["Health"] = BasicStats.Health.ToString("0");
            dictValues["Mana"] = BasicStats.Mana.ToString("0");
            dictValues["Item Level"] = String.Format("{0}*Lowest: {1}\nHighest: {2}", Character.AvgWornItemLevel.ToString("0"), Character.MinWornItemLevel.ToString("0"), Character.MaxWornItemLevel.ToString("0"));
            dictValues["Speed"] = String.Format("{0}%*{0}% Run speed",
                ((1f + BasicStats.MovementSpeed) * 100f).ToString("0"));
            #endregion
            #region Attributes
            dictValues["Strength"] = BasicStats.Strength.ToString();
            dictValues["Agility"] = BasicStats.Agility.ToString();
            dictValues["Stamina"] = BasicStats.Stamina.ToString();
            dictValues["Intellect"] = BasicStats.Intellect.ToString();
            dictValues["Spirit"] = BasicStats.Spirit.ToString();
            #endregion
            #region Spell
            #region Spell Power
            s = String.Empty;
            float intPower = BasicStats.Intellect - 10;
            if (BasicStats.InnerFire)
                s += String.Format("\n{0} from Inner Fire", PriestInformation.GetInnerFireSpellPowerBonus(Character));
            activeBuff = GetActiveBuffsByGroup("Spell Power");
            if (activeBuff != null)
                s += makeActiveBuffTextPercent(activeBuff, activeBuff.Stats.BonusSpellPowerMultiplier);
            dictValues["Spell Power"] = String.Format("{0}*{1} from {2} Intellect{3}",
                BasicStats.SpellPower.ToString("0"),
                intPower.ToString("0"),
                BasicStats.Intellect.ToString("0"),
                s
                );
            #endregion
            #region Haste
            s = String.Empty;
            if (Character.PriestTalents.Darkness > 0)
                s += String.Format("\n{0}% from {1} points in Darkness", Character.PriestTalents.Darkness, Character.PriestTalents.Darkness);
            activeBuff = GetActiveBuffsByGroup("Spell Haste");
            if (activeBuff != null)
                s += makeActiveBuffTextPercent(activeBuff, activeBuff.Stats.SpellHaste);
            activeBuff = GetActiveBuffsByGroup("Dark Intent");
            if (activeBuff != null)
                s += makeActiveBuffTextPercent(activeBuff, activeBuff.Stats.SpellHaste);
            dictValues["Haste"] = String.Format("{0}%*{1}% from {2} Haste Rating{3}",
                (BasicStats.SpellHaste * 100f).ToString("0.00"),
                (StatConversion.GetSpellHasteFromRating(BasicStats.HasteRating) * 100f).ToString("0.00"), BasicStats.HasteRating.ToString("0"),
                s
                );
            #endregion
            dictValues["Hit"] = (BasicStats.SpellHit * 100f).ToString("0.00");
            dictValues["Penetration"] = BasicStats.SpellPenetration.ToString("0");
            #region Mana Regen
            float manaRegen = StatConversion.GetSpiritRegenSec(BasicStats.Spirit, BasicStats.Intellect) * 5f;
            s = String.Format("\n{0} Mana per 5 sec from Base Mana Regeneration", (baseStats.Mana * 0.05f).ToString("0"));
            activeBuff = GetActiveBuffsByGroup("Mana Regeneration");
            if (activeBuff != null)
                s += makeActiveBuffText(activeBuff, activeBuff.Stats.Mp5);
            dictValues["Mana Regen"] = String.Format("{0}*{1} from Spirit based regen{2}",
                (manaRegen + BasicStats.Mp5).ToString("0"),
                manaRegen.ToString("0"),
                s
                );
            dictValues["Combat Regen"] = String.Format("{0}*{1} from Spirit based regen{2}",
                (manaRegen * BasicStats.SpellCombatManaRegeneration + BasicStats.Mp5).ToString("0"),
                (manaRegen * BasicStats.SpellCombatManaRegeneration).ToString("0"),
                s
                );
            #endregion
            #region Crit
            s = String.Empty;
            activeBuff = GetActiveBuffsByGroup("Critical Strike Chance");
            if (activeBuff != null)
                s += makeActiveBuffTextPercent(activeBuff, activeBuff.Stats.SpellCrit);
            activeBuff = GetActiveBuffsByGroup("Focus Magic, Spell Critical Strike Chance");
            if (activeBuff != null)
                s += makeActiveBuffTextPercent(activeBuff, activeBuff.Stats.SpellCrit);
            dictValues["Crit Chance"] = String.Format("{0}%*{1}% from {2} Crit Rating\n{3}% from {4} Intellect\n{5}% from Priest base{6}",
                (BasicStats.SpellCrit * 100f).ToString("0.00"),
                (StatConversion.GetSpellCritFromRating(BasicStats.CritRating) * 100f).ToString("0.00"), BasicStats.CritRating.ToString("0"),
                (StatConversion.GetSpellCritFromIntellect(BasicStats.Intellect) * 100f).ToString("0.00"), BasicStats.Intellect.ToString("0"),
                (baseStats.SpellCrit * 100f).ToString("0.00"),
                s
                );
            #endregion
            #region Mastery
            s = string.Empty;
            float masteryBase = 0, masteryBonus = 0;
            if (BasicStats.PriestSpec == ePriestSpec.Spec_Disc)
            {
                masteryBase = PriestInformation.DisciplineMasteryBase;
                masteryBonus = PriestInformation.DisciplineMasteryEffect * 100f;
                s += String.Format("\n\nEach point of mastery increases the potency of Absorbs by an additional {0}%.", masteryBonus.ToString("0.00"));
            }
            else if (BasicStats.PriestSpec == ePriestSpec.Spec_Holy)
            {
                masteryBase = PriestInformation.HolyMasteryBase;
                masteryBonus = PriestInformation.HolyMasteryEffect * 100f;
                s += String.Format("\n\nEach point of mastery provides an additional {0}% healing over 6 sec.", masteryBonus.ToString("0.00"));
            }
            dictValues["Mastery"] = String.Format("{0}%*{1}% from {2} Mastery Rating\n{3}% from {4} Base Mastery{5}",
                ((StatConversion.GetMasteryFromRating(BasicStats.MasteryRating) + masteryBase) * masteryBonus).ToString("0.00"),
                (StatConversion.GetMasteryFromRating(BasicStats.MasteryRating) * masteryBonus).ToString("0.00"), BasicStats.MasteryRating.ToString("0"),
                (masteryBase * masteryBonus).ToString("0.00"), masteryBase.ToString("0"),
                s
                );
            #endregion
            #endregion
            #region Defense
            dictValues["Armor"] = String.Format("{0}*{1}% physical damage reduction from same level target",
                BasicStats.Armor.ToString("0"),
                (StatConversion.GetDamageReductionFromArmor(Character.Level, BasicStats.Armor) * 100f).ToString("0.00"));
            dictValues["Dodge"] = String.Format("{0}%", (BasicStats.Dodge * 100f).ToString("0.00"));
            dictValues["Resilience"] = String.Format("{0}*{1}% damage reduction on attacks from other players\n{2}% damage reduction from spells",
                BasicStats.Resilience.ToString("0"),
                (StatConversion.GetDamageReductionFromResilience(BasicStats.Resilience) * 100f).ToString("0.00"),
                (Character.PriestTalents.InnerSanctum * 2f).ToString("0"));
            #endregion
            #region Resistance
            string resistTxt = "{0}*PvP\n{1}\n\nBoss\n{2}";
            string[] resistList = { "Arcane", "Fire", "Frost", "Nature", "Shadow" };
            float[] resistances = { BasicStats.ArcaneResistance + BasicStats.ArcaneResistanceBuff,
                                    BasicStats.FireResistance + BasicStats.FireResistanceBuff,
                                    BasicStats.FrostResistance + BasicStats.FrostResistanceBuff,
                                    BasicStats.NatureResistance + BasicStats.NatureResistanceBuff,
                                    BasicStats.ShadowResistance + BasicStats.ShadowResistanceBuff };

            for (int x = 0; x < resistList.Length; x++)
            {
                dictValues[resistList[x]] = String.Format(resistTxt,
                    resistances[x].ToString("0"),
                    StatConversion.GetResistanceTableString(Character.Level, Character.Level, resistances[x], 0f),
                    StatConversion.GetResistanceTableString(Character.Level + 3, Character.Level, resistances[x], 0f)
                    );
            }           
            #endregion
            #region Model
            CalculationOptionsHealPriest calcOpts = Character.CalculationOptions as CalculationOptionsHealPriest;
            if (calcOpts != null)
            {
                PriestSolver solver = PriestModels.GetModel(this, calcOpts, true);
                solver.Solve();
                List<string> reqs = solver.MeetsRequirements();
                string disp;
                if (reqs.Count > 0)
                    disp = String.Format("{0}\n\n{1}", solver.Name, String.Join("\n", reqs));
                else
                    disp = solver.Name;
                dictValues["Role"] = disp;
                dictValues["Burst Goal"] = this.BurstGoal.ToString("0");
                dictValues["Sust. Goal"] = this.SustainGoal.ToString("0");
                dictValues["Mana Goal"] = this.ManaGoal.ToString("0");
                dictValues["Burst"] = this.BurstPoints.ToString("0");
                dictValues["Sustained"] = this.SustainPoints.ToString("0");
                dictValues["Mana "] = this.ManaPoints.ToString("0");
            }
            #endregion
            #region Holy Spells
            SpellHeal spellHeal = new SpellHeal(Character, BasicStats);
            dictValues["Heal"] = String.Format("{0}*{1}", spellHeal.HPS().ToString("0"), spellHeal.ToString());
            SpellGreaterHeal spellGreaterHeal = new SpellGreaterHeal(Character, BasicStats);
            dictValues["Greater Heal"] = String.Format("{0}*{1}", spellGreaterHeal.HPS().ToString("0"), spellGreaterHeal.ToString());
            SpellFlashHeal spellFlashHeal = new SpellFlashHeal(Character, BasicStats);
            dictValues["Flash Heal"] = String.Format("{0}*{1}", spellFlashHeal.HPS().ToString("0"), spellFlashHeal.ToString());
            SpellBindingHeal spellBindingHeal = new SpellBindingHeal(Character, BasicStats);
            dictValues["Binding Heal"] = String.Format("{0}*{1}", spellBindingHeal.HPS().ToString("0"), spellBindingHeal.ToString());
            SpellRenew spellRenew = new SpellRenew(Character, BasicStats);
            dictValues["Renew"] = String.Format("{0}*{1}", spellRenew.HPS().ToString("0"), spellRenew.ToString());
            if (Character.PriestTalents.Lightwell > 0)
            {
                SpellLightwell spellLW = new SpellLightwell(Character, BasicStats);
                dictValues["Lightwell"] = String.Format("{0}*{1}", spellLW.HPS().ToString("0"), spellLW.ToString());
            }
            else
                dictValues["Lightwell"] = "N/A*You do not have the talent required.";
            SpellPrayerOfHealing spellProH = new SpellPrayerOfHealing(Character, BasicStats);
            dictValues["ProH"] = String.Format("{0}*{1}", spellProH.HPS().ToString("0"), spellProH.ToString());
            SpellHolyNova spellHolyNova = new SpellHolyNova(Character, BasicStats);
            dictValues["Holy Nova"] = String.Format("NYI*{1}", spellHolyNova.HPS().ToString("0"), spellHolyNova.ToString());
            if (Character.PriestTalents.CircleOfHealing > 0)
            {
                SpellCircleOfHealing spellCoH = new SpellCircleOfHealing(Character, BasicStats);
                dictValues["CoH"] = String.Format("{0}*{1}", spellCoH.HPS().ToString("0"), spellCoH.ToString());
            }
            else
            {
                dictValues["CoH"] = "N/A*You do not have the talent required.";
            }
            if (BasicStats.PriestSpec == ePriestSpec.Spec_Disc)
            {
                SpellPenance spellPenance = new SpellPenance(Character, BasicStats);
                dictValues["Penance"] = String.Format("{0}*{1}", spellPenance.HPS().ToString("0"), spellPenance.ToString());
            }
            else
            {
                dictValues["Penance"] =  "N/A*You do not have the correct Talent specialization.";
            }
            if (Character.PriestTalents.Revelations > 0)
            {
                SpellSerenity spellSerenity = new SpellSerenity(Character, BasicStats);
                dictValues["HW Serenity"] = String.Format("{0}*{1}", spellSerenity.HPS().ToString("0"), spellSerenity.ToString());
                SpellSanctuary spellSanctuary = new SpellSanctuary(Character, BasicStats);
                dictValues["HW Sanctuary"] = String.Format("{0}*{1}", spellSanctuary.HPS().ToString("0"), spellSanctuary.ToString());
            }
            else
            {
                dictValues["HW Serenity"] = "N/A*You do not have the talent required.";
                dictValues["HW Sanctuary"] = "N/A*You do not have the talent required.";
            }
            SpellPrayerOfMending spellProM = new SpellPrayerOfMending(Character, BasicStats, 1);
            dictValues["ProM"] = String.Format("{0}*{1}", spellProM.HPS().ToString("0"), spellProM.ToString());
            spellProM = new SpellPrayerOfMending(Character, BasicStats);
            dictValues["ProM 5 Hits"] = String.Format("{0}*{1}", spellProM.HPS().ToString("0"), spellProM.ToString());
            SpellPowerWordShield spellPWS = new SpellPowerWordShield(Character, BasicStats);
            dictValues["PWS"] = String.Format("{0}*{1}", spellPWS.HPS().ToString("0"), spellPWS.ToString());
            SpellDivineHymn spellDivineHymn = new SpellDivineHymn(Character, BasicStats);
            dictValues["Divine Hymn"] = String.Format("{0}*{1}", spellDivineHymn.HPS().ToString("0"), spellDivineHymn.ToString());
            if (Character.Race == CharacterRace.Draenei)
            {
                SpellGiftOfTheNaaru spellGoat = new SpellGiftOfTheNaaru(Character, BasicStats);
                dictValues["Gift of the Naaru"] = String.Format("{0}*{1}", spellGoat.HPS().ToString("0"), spellGoat.ToString());
            }
            else
            {
                dictValues["Gift of the Naaru"] = "N/A*You are not a spacegoat!";
            }
            SpellResurrection spellResurrection = new SpellResurrection(Character, BasicStats);
            dictValues["Resurrection"] = String.Format("{0}*{1}", spellResurrection.CastTime.ToString("0.00"), spellResurrection.ToString());
            #endregion 
            #region Shadow Spells
            #endregion
            return dictValues;
        }

        public override float GetOptimizableCalculationValue(string calculation)
		{
			switch (calculation)
			{
                case "Health": return BasicStats.Health;
                case "Resilience": return BasicStats.Resilience;
                case "Mana": return BasicStats.Mana;
                case "Mana Regen": return BasicStats.Mp5 + StatConversion.GetSpiritRegenSec(BasicStats.Spirit, BasicStats.Intellect) * 5f;
                case "Combat Regen": return BasicStats.Mp5 + StatConversion.GetSpiritRegenSec(BasicStats.Spirit, BasicStats.Intellect) * 5f * BasicStats.SpellCombatManaRegeneration;
                case "Haste Rating": return BasicStats.HasteRating;
                case "Mastery Rating": return BasicStats.MasteryRating;
                case "Haste %": return BasicStats.SpellHaste * 100f;
                case "Renew Ticks": return new SpellRenew(Character, BasicStats).OverTimeTicks;
                case "Crit Rating": return BasicStats.CritRating;
             //   case "Healing Crit %": return (basicStats.SpellCrit * 100f) + character.PriestTalents.HolySpecialization * 1f + character.PriestTalents.RenewedHope * 2f;
             //   case "PW:Shield": return new PowerWordShield(basicStats, character).AvgHeal;
             //   case "GHeal Avg": return new Heal(basicStats, character).AvgHeal;
             //   case "FHeal Avg": return new FlashHeal(basicStats, character).AvgHeal;
             //   case "CoH Avg": return new CircleOfHealing(basicStats, character).AvgHeal;
                case "Armor": return BasicStats.Armor + BasicStats.BonusArmor;
			    case "Arcane Resistance": return BasicStats.ArcaneResistance + BasicStats.ArcaneResistanceBuff;
                case "Fire Resistance": return BasicStats.FireResistance + BasicStats.FireResistanceBuff;
                case "Frost Resistance": return BasicStats.FrostResistance + BasicStats.FrostResistance;
                case "Nature Resistance": return BasicStats.NatureResistance + BasicStats.NatureResistanceBuff;
                case "Shadow Resistance": return BasicStats.ShadowResistance + BasicStats.ShadowResistanceBuff;
            }
			return 0f;
		}
    }
}
