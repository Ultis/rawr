using System;
using System.Collections.Generic;

using Rawr;

namespace Rawr.ShadowPriest
{
    [System.ComponentModel.DisplayName("ShadowPriest|Spell_Shadow_ShadowForm")]
    public class CalculationsShadowPriest : CalculationsBase 
    {
        public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.Priest; } }

        private Dictionary<string, System.Drawing.Color> _subPointNameColors = null;
        public override Dictionary<string, System.Drawing.Color> SubPointNameColors
        {
            get
            {
                if (_subPointNameColors == null)
                {
                    _subPointNameColors = new Dictionary<string, System.Drawing.Color>();
                    _subPointNameColors.Add("Dps", System.Drawing.Color.FromArgb(0, 128, 255));
                    _subPointNameColors.Add("Survivability", System.Drawing.Color.FromArgb(64, 128, 32));
                }
                return _subPointNameColors;
            }
        }

        private string[] _characterDisplayCalculationLabels = null;
        public override string[] CharacterDisplayCalculationLabels
        {
            get
            {
                if (_characterDisplayCalculationLabels == null)
                    _characterDisplayCalculationLabels = new string[] {
					"Basic Stats:Health",
					"Basic Stats:Mana",
					"Basic Stats:Stamina",
					"Basic Stats:Intellect",
					"Basic Stats:Spirit",
					"Basic Stats:Shadow Damage",
					"Basic Stats:Regen",
					"Basic Stats:Spell Crit",
					"Basic Stats:Spell Hit",
					"Basic Stats:Spell Haste",
                    "Basic Stats:Global Cooldown",
                    "Spells:Vampiric Touch",
                    "Spells:Shadow Word Pain",
				    "Spells:Shadow Word Death",
                    "Spells:Mind Blast",
                    "Spells:Mind Flay",
                    "Spells:Vampiric Embrace",
                    "Spells:Power Word Shield",
                    "Simulation:Damage done",
                    "Simulation:Dps"
				};
                return _characterDisplayCalculationLabels;
            }
        }

        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
        public override CalculationOptionsPanelBase CalculationOptionsPanel
        {
            get {
                if (_calculationOptionsPanel == null)
                {
                    _calculationOptionsPanel = new CalculationOptionsPanelShadowPriest();
                }
                return _calculationOptionsPanel;
            }
        }

        private string[] _customChartNames = null;
        public override string[] CustomChartNames
        {
            get
            {
                if (_customChartNames == null)
                    _customChartNames = new string[] {};
                return _customChartNames;
            }
        }

        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationShadowPriest(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsShadowPriest(); }

        private List<Item.ItemType> _relevantItemTypes = null;
        public override List<Item.ItemType> RelevantItemTypes
        {
            get {
                if (_relevantItemTypes == null) {
                    _relevantItemTypes = new List<Item.ItemType>(new Item.ItemType[]{
                        Item.ItemType.None,
                        Item.ItemType.Cloth,
                        Item.ItemType.Dagger,
                        Item.ItemType.Wand,
                        Item.ItemType.OneHandMace,
                        Item.ItemType.Staff
                    });
                }
                return _relevantItemTypes;
            }
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            throw new NotImplementedException();
        }

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
        {
            Stats stats = GetCharacterStats(character, additionalItem);
            Stats statsRace = GetRaceStats(character);
            CharacterCalculationsShadowPriest calculatedStats = new CharacterCalculationsShadowPriest();
            CalculationOptionsShadowPriest calculationOptions = character.CalculationOptions as CalculationOptionsShadowPriest;
            
            calculatedStats.BasicStats = stats;
            calculatedStats.Talents = character.Talents;
            calculatedStats.CalculationOptions = calculationOptions;

            calculatedStats.BasicStats.Spirit = statsRace.Spirit + (calculatedStats.BasicStats.Spirit - statsRace.Spirit) * (1 + character.Talents.GetTalent("Spirit of Redemption").PointsInvested * 0.05f);

            calculatedStats.SpiritRegen = (float)Math.Floor(5 * 0.0093271 * calculatedStats.BasicStats.Spirit * Math.Sqrt(calculatedStats.BasicStats.Intellect));
            calculatedStats.RegenInFSR = (float)Math.Floor((calculatedStats.BasicStats.Mp5 + character.Talents.GetTalent("Meditation").PointsInvested * 0.1f * calculatedStats.SpiritRegen * (1 + calculatedStats.BasicStats.SpellCombatManaRegeneration)));
            calculatedStats.RegenOutFSR = calculatedStats.BasicStats.Mp5 + calculatedStats.SpiritRegen;
           
            calculatedStats.BasicStats.SpellCrit = (float)Math.Round((calculatedStats.BasicStats.Intellect / 80) +
                (calculatedStats.BasicStats.SpellCritRating / 22.08) + 1.85, 2);

            calculatedStats.BasicStats.SpellDamageRating += calculatedStats.BasicStats.Spirit * character.Talents.GetTalent("Spiritual Guidance").PointsInvested * 0.05f;

            Solver solver = new Solver(stats, character.Talents, calculationOptions);
            solver.Calculate();

            calculatedStats.DpsPoints = solver.OverallDps;
            //int hitcap = GetSpellHitCap(character.Talents);
            //calculatedStats.DpsPoints = calculatedStats.BasicStats.SpellDamageRating + calculatedStats.BasicStats.SpellShadowDamageRating
            //    + (calculatedStats.BasicStats.SpellHasteRating)
            //    + (calculatedStats.BasicStats.SpellCritRating / 5.57f)
            //    + (calculatedStats.BasicStats.Spirit * 0.11f)
            //    + (calculatedStats.BasicStats.Intellect * 0.055f)
            //    - (calculatedStats.BasicStats.SpellHitRating < hitcap ? (hitcap - calculatedStats.BasicStats.SpellHitRating) * 1.364f: 0);

            calculatedStats.SurvivalPoints = calculatedStats.BasicStats.Stamina / 10;
            calculatedStats.OverallPoints = calculatedStats.DpsPoints + calculatedStats.SurvivalPoints;

            return calculatedStats;
        }

        public static int GetSpellHitCap(TalentTree talents)
        {
            return (int)Math.Round(202 - talents.GetTalent("Shadow Focus").PointsInvested * 12.6f * 2);
        }

        public Stats GetRaceStats(Character character)
        {
            switch (character.Race)
            {
                case Character.CharacterRace.NightElf:
                    return new Stats()
                    {
                        Health = 3434f,
                        Mana = 2470f,
                        Stamina = 57f,
                        Agility = 50f,
                        Intellect = 147f,
                        Spirit = 151f
                    };
                case Character.CharacterRace.Dwarf:
                    return new Stats()
                    {
                        Health = 3434f,
                        Mana = 2470f,
                        Stamina = 61f,
                        Agility = 41f,
                        Intellect = 144f,
                        Spirit = 150f
                    };
                case Character.CharacterRace.Draenei:
                    return new Stats()
                    {
                        Health = 3434f,
                        Mana = 2470f,
                        Stamina = 57f,
                        Agility = 42f,
                        Intellect = 146f,
                        Spirit = 160f
                    };
                case Character.CharacterRace.Human:
                    return new Stats()
                    {
                        Health = 3434f,
                        Mana = 2470f,
                        Stamina = 58f,
                        Agility = 45f,
                        Intellect = 145f,
                        Spirit = 174f
                    };
                case Character.CharacterRace.BloodElf:
                    return new Stats()
                    {
                        Health = 3211f,
                        Mana = 2620f,
                        Stamina = 56f,
                        Agility = 47f,
                        Intellect = 149f,
                        Spirit = 157f
                    };
                case Character.CharacterRace.Troll:
                    return new Stats()
                    {
                        Health = 3211f,
                        Mana = 2620f,
                        Stamina = 59f,
                        Agility = 59f,
                        Intellect = 141f,
                        Spirit = 159f
                    };
                case Character.CharacterRace.Undead:
                    return new Stats()
                    {
                        Health = 3181,
                        Mana = 2530f,
                        Stamina = 59f,
                        Agility = 43f,
                        Intellect = 145f,
                        Spirit = 156f,
                    };
            }
            return null;
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            Stats statsRace = GetRaceStats(character);
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            Stats statsTotal = statsBaseGear + statsEnchants + statsBuffs + statsRace;

            statsTotal.Stamina = (float)Math.Round((statsTotal.Stamina) * (1 + statsTotal.BonusStaminaMultiplier));
            statsTotal.Intellect = (float)Math.Round(statsTotal.Intellect * (1 + statsTotal.BonusIntellectMultiplier));
            statsTotal.Spirit = (float)Math.Round((statsTotal.Spirit) * (1 + statsTotal.BonusSpiritMultiplier));
            statsTotal.Healing = (float)Math.Round(statsTotal.Healing + (statsTotal.SpellDamageFromSpiritPercentage * statsTotal.Spirit));
            statsTotal.Mana = statsTotal.Mana + ((statsTotal.Intellect - 20f) * 15f + 20f);
            statsTotal.Health = statsTotal.Health + (statsTotal.Stamina * 10f);

            return statsTotal;
        }
       
        public override Stats GetRelevantStats(Stats stats)
        {
            return new Stats()
            {
                Stamina = stats.Stamina,
                Intellect = stats.Intellect,
                Mp5 = stats.Mp5,
                SpellDamageRating = stats.SpellDamageRating,
                SpellShadowDamageRating = stats.SpellShadowDamageRating,
                SpellHitRating = stats.SpellHitRating,
                SpellCritRating = stats.SpellCritRating,
                SpellHasteRating = stats.SpellHasteRating,
                Health = stats.Health,
                Mana = stats.Mana,
                Spirit = stats.Spirit,
                BonusManaPotion = stats.BonusManaPotion,
                ThreatReductionMultiplier = stats.ThreatReductionMultiplier,
                SpellDamageFor15SecOnUse2Min = stats.SpellDamageFor15SecOnUse2Min,
                SpellDamageFor15SecOnUse90Sec = stats.SpellDamageFor15SecOnUse90Sec,
                SpellDamageFor20SecOnUse2Min = stats.SpellDamageFor20SecOnUse2Min,
                SpellHasteFor20SecOnUse2Min = stats.SpellHasteFor20SecOnUse2Min,
                SpellHasteFor20SecOnUse5Min = stats.SpellHasteFor20SecOnUse5Min
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            return (stats.Stamina + stats.Intellect + stats.Spirit + stats.Mp5 + stats.SpellDamageRating + stats.SpellShadowDamageRating+ stats.SpellCritRating
                + stats.SpellHasteRating + stats.BonusSpiritMultiplier + stats.SpellDamageFromSpiritPercentage + stats.BonusIntellectMultiplier
                + stats.BonusManaPotion + stats.SpellHitRating + stats.ThreatReductionMultiplier
                + stats.SpellDamageFor15SecOnUse2Min + stats.SpellDamageFor15SecOnUse90Sec
                + stats.SpellDamageFor20SecOnUse2Min + stats.SpellHasteFor20SecOnUse2Min
                + stats.SpellHasteFor20SecOnUse5Min) > 0;
        }

        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsShadowPriest));
            System.IO.StringReader reader = new System.IO.StringReader(xml);
            CalculationOptionsShadowPriest calcOpts = serializer.Deserialize(reader) as CalculationOptionsShadowPriest;
            return calcOpts;
        }
    }
}
