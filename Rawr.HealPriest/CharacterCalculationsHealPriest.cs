using System;
using System.Collections.Generic;

namespace Rawr.HealPriest
{

    public class CharacterCalculationsHealPriest : CharacterCalculationsBase
    {
        private Stats basicStats;
        private Character character;

        public float SpiritRegen { get; set; }
        public CharacterRace Race { get; set; }

        public Character Character
        {
            get { return character; }
            set { character = value; }
        }
      
        public Stats BasicStats
        {
            get { return basicStats; }
            set { basicStats = value; }
        }

        public override float OverallPoints
        {
            get
            {
                float f = 0f;
                foreach (float f2 in subPoints)
                    f += f2;
                return f;
            }
            set { }
        }

        private float[] subPoints = new float[] { 0f, 0f, 0f };
        public override float[] SubPoints
        {
            get { return subPoints; }
            set { subPoints = value; }
        }

        public float HPSBurstPoints
        {
            get { return subPoints[0]; }
            set { subPoints[0] = value; }
        }

        public float HPSSustainPoints
        {
            get { return subPoints[1]; }
            set { subPoints[1] = value; }
        }

        public float SurvivabilityPoints
        {
            get { return subPoints[2]; }
            set { subPoints[2] = value; }
        }


        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();
            Stats baseStats = BaseStats.GetBaseStats(character);
            #region General
            dictValues.Add("Health", BasicStats.Health.ToString("0"));
            dictValues.Add("Mana", BasicStats.Mana.ToString("0"));

            int iLvl = 0, iCnt = 0, iLvlMax = 0, iLvlMin = int.MaxValue;
            foreach (ItemInstance i in character.GetItems())
            {
                if (i != null &&
                    i.Item._itemLevel > 0 &&
                    !i.Item.FitsInSlot(CharacterSlot.Shirt) &&
                    !i.Item.FitsInSlot(CharacterSlot.Tabard) &&
                    !i.Item.FitsInSlot(CharacterSlot.OffHand))
                {
                    iLvl += i.Item._itemLevel;
                    if (i.Item._itemLevel > iLvlMax)
                        iLvlMax = i.Item._itemLevel;
                    if (i.Item._itemLevel < iLvlMin)
                        iLvlMin = i.Item._itemLevel;
                    iCnt++;
                }
            }
            if (iCnt == 0)
            {   // SIGH HAX FOR EMPTY CHARS LOL.
                iCnt = 1;
                iLvl = 0;
                iLvlMin = 0;
                iLvlMax = 0;
            }
            dictValues.Add("Item Level", String.Format("{0}*Lowest: {1}\r\nHighest: {2}", ((iCnt == 0) ? 0 : iLvl / iCnt).ToString("0"), iLvlMin, iLvlMax));
            dictValues.Add("Speed", String.Format("{0}%*{0}% Run speed",
                ((1f + BasicStats.MovementSpeed) * 100f).ToString("0")));
            #endregion
            #region Attributes
            dictValues.Add("Strength", BasicStats.Strength.ToString());
            dictValues.Add("Agility", BasicStats.Agility.ToString());
            dictValues.Add("Stamina", BasicStats.Stamina.ToString());
            dictValues.Add("Intellect", BasicStats.Intellect.ToString());
            dictValues.Add("Spirit", BasicStats.Spirit.ToString());
            #endregion
            #region Spell
            dictValues.Add("Spell Power", BasicStats.SpellPower.ToString("0"));
            dictValues.Add("Haste", String.Format("{0}%*{1}% from {2} Haste Rating\r\n{3}% from Darkness",
                (BasicStats.SpellHaste * 100f).ToString("0.00"),
                (StatConversion.GetSpellHasteFromRating(BasicStats.HasteRating) * 100f).ToString("0.00"), BasicStats.HasteRating.ToString("0"),
                character.PriestTalents.Darkness.ToString("0")));
            dictValues.Add("Hit", (BasicStats.SpellHit * 100f).ToString("0.00"));
            dictValues.Add("Penetration", BasicStats.SpellPenetration.ToString("0"));
            float manaRegen = StatConversion.GetSpiritRegenSec(BasicStats.Spirit, BasicStats.Intellect) * 5f;
            dictValues.Add("Mana Regen", (manaRegen + BasicStats.Mp5).ToString("0"));
            dictValues.Add("Combat Regen", (manaRegen * BasicStats.SpellCombatManaRegeneration + BasicStats.Mp5).ToString("0"));
            dictValues.Add("Crit Chance", String.Format("{0}%*{1}% from {2} Crit Rating\r\n{3}% from {4} Intellect\r\n{5}% from Priest base",
                (BasicStats.SpellCrit * 100f).ToString("0.00"),
                (StatConversion.GetSpellCritFromRating(BasicStats.CritRating) * 100f).ToString("0.00"), BasicStats.CritRating.ToString("0"),
                (StatConversion.GetSpellCritFromIntellect(BasicStats.Intellect) * 100f).ToString("0.00"), BasicStats.Intellect.ToString("0"),
                (baseStats.SpellCrit * 100f).ToString("0.00")));
            dictValues.Add("Mastery", BasicStats.MasteryRating.ToString("0"));
            #endregion
            #region Defense
            dictValues.Add("Armor", String.Format("{0}*{1}% physical damage reduction from same level target",
                BasicStats.Armor.ToString("0"),
                (StatConversion.GetDamageReductionFromArmor(character.Level, BasicStats.Armor) * 100f).ToString("0.00")));
            dictValues.Add("Dodge", String.Format("{0}%", (BasicStats.Dodge * 100f).ToString("0.00")));
            dictValues.Add("Resilience", String.Format("{0}*{1}% damage reduction on attacks from other players\r\n{2}% damage reduction from spells",
                BasicStats.Resilience.ToString("0"),
                (StatConversion.GetDamageReductionFromResilience(BasicStats.Resilience) * 100f).ToString("0.00"),
                (character.PriestTalents.InnerSanctum * 2f).ToString("0")));
            #endregion
            #region Resistance
            float thisResist;
            thisResist = BasicStats.ArcaneResistance + BasicStats.ArcaneResistanceBuff;
            dictValues.Add("Arcane", String.Format("{0}*{1}% average reduction from Arcane damage",
                thisResist.ToString("0"),
                (StatConversion.GetAverageResistance(character.Level, character.Level, thisResist, 0f) * 100f).ToString("0.00")));
            thisResist = BasicStats.FireResistance + BasicStats.FireResistanceBuff;
            dictValues.Add("Fire", String.Format("{0}*{1}% average reduction from Fire damage",
                thisResist.ToString("0"),
                (StatConversion.GetAverageResistance(character.Level, character.Level, thisResist, 0f) * 100f).ToString("0.00")));
            thisResist = BasicStats.NatureResistance + BasicStats.NatureResistanceBuff;
            dictValues.Add("Nature", String.Format("{0}*{1}% average reduction from Nature damage",
                thisResist.ToString("0"),
                (StatConversion.GetAverageResistance(character.Level, character.Level, thisResist, 0f) * 100f).ToString("0.00")));
            thisResist = BasicStats.FrostResistance + BasicStats.FrostResistanceBuff;
            dictValues.Add("Frost", String.Format("{0}*{1}% average reduction from Frost damage",
                thisResist.ToString("0"),
                (StatConversion.GetAverageResistance(character.Level, character.Level, thisResist, 0f) * 100f).ToString("0.00")));
            thisResist = BasicStats.ShadowResistance + BasicStats.ShadowResistanceBuff;
            dictValues.Add("Shadow", String.Format("{0}*{1}% average reduction from Shadow damage",
                thisResist.ToString("0"),
                (StatConversion.GetAverageResistance(character.Level, character.Level, thisResist, 0f) * 100f).ToString("0.00")));
            #endregion
            #region Model
            #endregion
            #region Holy Spells
            #endregion
            #region Shadow Spells
            #endregion
            #region OldStuffwhichisancient
            /*
            float ResilienceCap = 0.15f, ResilienceFromRating = StatConversion.GetCritReductionFromResilience(1);
            float Resilience = StatConversion.GetCritReductionFromResilience(BasicStats.Resilience);
            dictValues.Add("Resilience", string.Format("{0}*-{1}% Damage from DoT and Mana Drains\n\r-{1}% Chance to be crit\r\n-{2}% Damage from Crits.\r\n{3}", 
                BasicStats.Resilience.ToString(), 
                (Resilience * 100f).ToString("0.00"), 
                (Resilience * 100f * 2.2f).ToString("0.00"),
                (Resilience>ResilienceCap)?(string.Format("{0} rating above cap", ((float)Math.Floor((Resilience - ResilienceCap) / ResilienceFromRating)).ToString("0"))):(string.Format("{0} rating below cap", ((float)Math.Ceiling((ResilienceCap - Resilience) / ResilienceFromRating)).ToString("0")))));
            dictValues.Add("Mana", BasicStats.Mana.ToString());
            dictValues.Add("Intellect", BasicStats.Intellect.ToString());
            dictValues.Add("Spirit", Math.Floor(BasicStats.Spirit).ToString("0"));
            dictValues.Add("Spell Power", string.Format("{0}*{1} from Inner Fire",
                Math.Floor(BasicStats.SpellPower).ToString("0"),
                BasicStats.PriestInnerFire * CalculationsHealPriest.GetInnerFireSpellPowerBonus(character)));
            //dictValues.Add("Healing", Math.Floor(BasicStats.SpellPower * 1.88f).ToString("0"));
            dictValues.Add("In FSR MP5", string.Format("{0}*{1} from MP5\r\n{2} from Meditation\r\n{3} Outside FSR\r\n{4} OFSR w/MP5",
                (BasicStats.Mp5 + RegenInFSR).ToString("0"),
                BasicStats.Mp5.ToString("0"),
                RegenInFSR.ToString("0"),
                RegenOutFSR.ToString("0"),
                (BasicStats.Mp5 + RegenOutFSR).ToString("0")));
            dictValues.Add("Spell Crit", string.Format("{0}%*{1}% from Intellect\r\n{2}% from {6} Crit rating\r\n{3}% from Focused Will\r\n{4}% Class Base\r\n{5}% from Buffs",
                (BasicStats.SpellCrit * 100f).ToString("0.00"), (StatConversion.GetSpellCritFromIntellect(BasicStats.Intellect) * 100f).ToString("0.00"), (StatConversion.GetSpellCritFromRating(BasicStats.CritRating) * 100f).ToString("0.00"), character.PriestTalents.FocusedWill.ToString("0"), (baseStats.SpellCrit * 100f).ToString("0.00"), (BasicStats.SpellCrit * 100f - baseStats.SpellCrit * 100f - StatConversion.GetSpellCritFromRating(BasicStats.CritRating) * 100f - StatConversion.GetSpellCritFromIntellect(BasicStats.Intellect) * 100f - character.PriestTalents.FocusedWill * 1f).ToString("0.00"), BasicStats.CritRating));
            dictValues.Add("Healing Crit", string.Format("{0}%*{1} ({1}%) points in Holy Specialization\r\n{2} ({3}%) points in Renewed Hope",
                ((BasicStats.SpellCrit * 100f) + character.PriestTalents.HolySpecialization * 1f + character.PriestTalents.RenewedHope * 2f).ToString("0.00"),
                character.PriestTalents.HolySpecialization, character.PriestTalents.RenewedHope, character.PriestTalents.RenewedHope * 2));
            dictValues.Add("Spell Haste", string.Format("{0}%*{1}% from {2} Haste rating\r\n{3}% ({6}) points in Enlightenment\r\n{4}% from Buffs\r\n{5}s Global Cooldown", 
                (BasicStats.SpellHaste * 100f).ToString("0.00"), (StatConversion.GetSpellHasteFromRating(BasicStats.HasteRating) * 100f).ToString("0.00"), BasicStats.HasteRating.ToString(), (character.PriestTalents.Enlightenment * 2).ToString("0"), (((1 + BasicStats.SpellHaste) / (1 + StatConversion.GetSpellHasteFromRating(BasicStats.HasteRating)) / (1 + character.PriestTalents.Enlightenment * 0.02f) - 1) * 100f).ToString("0.00"), Math.Max(1.0f, 1.5f / (1 + BasicStats.SpellHaste)).ToString("0.00"), character.PriestTalents.Enlightenment));
            dictValues.Add("Armor", string.Format("{0}*{1}% Damage Reduction.",
                (BasicStats.Armor + BasicStats.BonusArmor).ToString("0"),
                (StatConversion.GetArmorDamageReduction(80, (BasicStats.Armor + BasicStats.BonusArmor), 0f, 0f, 0f) * 100f).ToString("0.00")));


            float[] Resistances = {
                0,
                BasicStats.ArcaneResistance + BasicStats.ArcaneResistanceBuff,
                BasicStats.FireResistance + BasicStats.FireResistanceBuff,
                BasicStats.FrostResistance + BasicStats.FrostResistanceBuff,
                BasicStats.NatureResistance + BasicStats.NatureResistanceBuff,
                BasicStats.ShadowResistance + BasicStats.ShadowResistanceBuff,
            };

            string[] ResistanceNames = {
                "None",
                "Arcane",
                "Fire",
                "Frost",
                "Nature",
                "Shadow",
            };

            string ResistanceString = "*Resistances:";

            float MaxResist = Resistances[0];
            int MaxResistIndex = 0;
            float AvgResist = 0f;
            for (int x = 1; x < Resistances.Length; x++)
            {
                AvgResist += Resistances[x];
                if (Resistances[x] > MaxResist)
                {
                    MaxResist =  Resistances[x];
                    MaxResistIndex = x;
                }
                ResistanceString += string.Format("\r\n{0} : {1}", ResistanceNames[x], Resistances[x]);
            }
            AvgResist /= (Resistances.Length - 1);

            if (AvgResist == 0)
                ResistanceString = "None" + ResistanceString;
            else
            {
                string ResistanceName = (MaxResist == AvgResist) ? "All" : ResistanceNames[MaxResistIndex];
                ResistanceString = string.Format("{0} : {1}", ResistanceName, MaxResist.ToString("0")) + ResistanceString;
                ResistanceString += string.Format("\r\n\r\nResist ({0}):", ResistanceName);
                ResistanceString += string.Format("\r\n{0}", StatConversion.GetResistanceTableString(character.Level + 3, character.Level, MaxResist, 0));
            }

            dictValues.Add("Resistance", ResistanceString);

            BaseSolver solver;
            if ((character.CalculationOptions as CalculationOptionsHealPriest).Role == eRole.CUSTOM)
                solver = new AdvancedSolver(BasicStats, character);
            else
                solver = new Solver(BasicStats, character);
            solver.Calculate(this);

            dictValues.Add("Role", string.Format("{0}*{1}", solver.Role, solver.ActionList));
            dictValues.Add("Burst", string.Format("{0}", HPSBurstPoints.ToString("0")));
            dictValues.Add("Sustained", string.Format("{0}", HPSSustainPoints.ToString("0")));

            dictValues.Add("Renew", new Renew(BasicStats, character).ToString());
            dictValues.Add("Flash Heal", new FlashHeal(BasicStats, character).ToString());
            dictValues.Add("Greater Heal", new Heal(BasicStats, character).ToString());
            dictValues.Add("PoH", new PrayerOfHealing(BasicStats, character).ToString());
            dictValues.Add("Binding Heal", new BindingHeal(BasicStats, character).ToString());
            dictValues.Add("Prayer of Mending", new PrayerOfMending(BasicStats, character).ToString());
            dictValues.Add("Power Word Shield", new PowerWordShield(BasicStats, character).ToString());
            dictValues.Add("Holy Nova", new HolyNova(BasicStats, character).ToString());

            if (character.PriestTalents.CircleOfHealing > 0)
                dictValues.Add("CoH", new CircleOfHealing(BasicStats, character).ToString());
            else
                dictValues.Add("CoH", "- *No required talents");

            if (character.PriestTalents.Lightwell > 0)
                dictValues.Add("Lightwell", new Lightwell(BasicStats, character).ToString());
            else
                dictValues.Add("Lightwell", "- *No required talents");

            if (character.PriestTalents.Penance > 0)
                dictValues.Add("Penance", new Penance(BasicStats, character).ToString());
            else
                dictValues.Add("Penance", "- *No required talents");
            
            if(Race == CharacterRace.Draenei)
                dictValues.Add("Gift of the Naaru", new GiftOfTheNaaru(BasicStats, character).ToString());
            else
                dictValues.Add("Gift of the Naaru", "-");
            dictValues.Add("Divine Hymn", new DivineHymn(BasicStats, character).ToString());
            dictValues.Add("Resurrection", new Resurrection(BasicStats, character).ToString());
            */
            #endregion
            return dictValues;
        }

        public override float GetOptimizableCalculationValue(string calculation)
		{
			switch (calculation)
			{
                case "Health": return basicStats.Health;
                case "Resilience": return basicStats.Resilience;
                case "Mana": return basicStats.Mana;
                case "Mana Regen": return basicStats.Mp5 + StatConversion.GetSpiritRegenSec(basicStats.Spirit, basicStats.Intellect) * 5f;
                case "Combat Regen": return basicStats.Mp5 + StatConversion.GetSpiritRegenSec(basicStats.Spirit, basicStats.Intellect) * 5f * basicStats.SpellCombatManaRegeneration;
                case "Haste Rating": return basicStats.HasteRating;
                case "Mastery Rating": return basicStats.MasteryRating;
                case "Haste %": return basicStats.SpellHaste * 100f;
                case "Crit Rating": return basicStats.CritRating;
             //   case "Healing Crit %": return (basicStats.SpellCrit * 100f) + character.PriestTalents.HolySpecialization * 1f + character.PriestTalents.RenewedHope * 2f;
             //   case "PW:Shield": return new PowerWordShield(basicStats, character).AvgHeal;
             //   case "GHeal Avg": return new Heal(basicStats, character).AvgHeal;
             //   case "FHeal Avg": return new FlashHeal(basicStats, character).AvgHeal;
             //   case "CoH Avg": return new CircleOfHealing(basicStats, character).AvgHeal;
                case "Armor": return basicStats.Armor + basicStats.BonusArmor;
			    case "Arcane Resistance": return basicStats.ArcaneResistance + basicStats.ArcaneResistanceBuff;
                case "Fire Resistance": return basicStats.FireResistance + basicStats.FireResistanceBuff;
                case "Frost Resistance": return basicStats.FrostResistance + basicStats.FrostResistance;
                case "Nature Resistance": return basicStats.NatureResistance + basicStats.NatureResistanceBuff;
                case "Shadow Resistance": return basicStats.ShadowResistance + basicStats.ShadowResistanceBuff;
            }
			return 0f;
		}
    }
}
