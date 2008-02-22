using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Warlock
{
    [System.ComponentModel.DisplayName("Warlock")]
    class CalculationsWarlock : CalculationsBase
    {


        private Dictionary<string, System.Drawing.Color> _subPointNameColors = null;
        /// <summary>
        /// Dictionary<string, Color> that includes the names of each rating which your model will use,
        /// and a color for each. These colors will be used in the charts.
        /// 
        /// EXAMPLE: 
        /// subPointNameColors = new Dictionary<string, System.Drawing.Color>();
        /// subPointNameColors.Add("Mitigation", System.Drawing.Colors.Red);
        /// subPointNameColors.Add("Survival", System.Drawing.Colors.Blue);
        /// </summary>
        public override Dictionary<string, System.Drawing.Color> SubPointNameColors
        {
            get {
                
                if (_subPointNameColors == null)
                {
                    _subPointNameColors = new Dictionary<string, System.Drawing.Color>();
                    _subPointNameColors.Add("DPS", System.Drawing.Color.Blue);
                }
                return _subPointNameColors;
                }
        }


        private string[] _characterDisplayCalculationLabels = null;
        /// <summary>
        /// An array of strings which will be used to build the calculation display.
        /// Each string must be in the format of "Heading:Label". Heading will be used as the
        /// text of the group box containing all labels that have the same Heading.
        /// Label will be the label of that calculation, and may be appended with '*' followed by
        /// a description of that calculation which will be displayed in a tooltip for that label.
        /// Label (without the tooltip string) must be unique.
        /// 
        /// EXAMPLE:
        /// characterDisplayCalculationLabels = new string[]
        /// {
        ///		"Basic Stats:Health",
        ///		"Basic Stats:Armor",
        ///		"Advanced Stats:Dodge",
        ///		"Advanced Stats:Miss*Chance to be missed"
        /// };
        /// </summary>
        public override string[] CharacterDisplayCalculationLabels
        {
            get 
            {
                return _characterDisplayCalculationLabels ?? (_characterDisplayCalculationLabels = new string[]
                    {
                        "Basic Stats:Health",
                        "Basic Stats:Mana",
                        "Basic Stats:Stamina",
                        "Basic Stats:Intellect",
                        "Spell Stats:Spell Crit Rate",
                        "Spell Stats:Spell Hit Rate",
                        "Spell Stats:Casting Speed",
                        "Spell Stats:Shadow Damage",
                        "Spell Stats:Fire Damage",
                        "Overall Stats:DPS",
                    });
            }
        }

        private string[] _customChartNames = null;
        public override string[] CustomChartNames
        {
            get { return _customChartNames ?? ( _customChartNames = new string[] { }); }
        }


        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
        public override CalculationOptionsPanelBase CalculationOptionsPanel
        {
            get { return _calculationOptionsPanel ?? (_calculationOptionsPanel = new CalculationOptionsPanelWarlock()); }
        }

        private List<Item.ItemType> _relevantItemTypes = null;
        public override List<Item.ItemType> RelevantItemTypes
        {
            get
            {
                return _relevantItemTypes ?? (_relevantItemTypes = new List<Item.ItemType>(new Item.ItemType[]
					{
						Item.ItemType.None,
						Item.ItemType.Cloth,
						Item.ItemType.Dagger,
						Item.ItemType.OneHandSword,
						Item.ItemType.Staff,
						Item.ItemType.Wand,
					}));
            }
        }

        public override ComparisonCalculationBase CreateNewComparisonCalculation()
        {
            return new ComparisonCalculationWarlock();
        }

        public override CharacterCalculationsBase CreateNewCharacterCalculations()
        {
            return new CharacterCalculationsWarlock();
        }

        /// <summary>
        /// GetCharacterCalculations is the primary method of each model, where a majority of the calculations
        /// and formulae will be used. GetCharacterCalculations should call GetCharacterStats(), and based on
        /// those total stats for the character, and any calculationoptions on the character, perform all the 
        /// calculations required to come up with the final calculations defined in 
        /// CharacterDisplayCalculationLabels, including an Overall rating, and all Sub ratings defined in 
        /// SubPointNameColors.
        /// </summary>
        /// <param name="character">The character to perform calculations for.</param>
        /// <param name="additionalItem">An additional item to treat the character as wearing.
        /// This is used for gems, which don't have a slot on the character to fit in, so are just
        /// added onto the character, in order to get gem calculations.</param>
        /// <returns>A custom CharacterCalculations object which inherits from CharacterCalculationsBase,
        /// containing all of the final calculations defined in CharacterDisplayCalculationLabels. See
        /// CharacterCalculationsBase comments for more details.</returns>
        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
        {
            Stats charStats = GetCharacterStats(character, additionalItem);
            int duration = Int32.Parse(character.CalculationOptions["Duration"]);
            float latency = float.Parse(character.CalculationOptions["Latency"]);
            float gcd = 1.5f;

            Spell sbolt = new ShadowBolt(character, charStats);
            Spell coa = new CurseOfAgony(character, charStats);
            WarlockSpellRotation wsr = new WarlockSpellRotation(charStats, character, duration);
            wsr.AddSpell(sbolt, 2);
            wsr.AddSpell(coa, 1);

            float[] dps = wsr.GetDPS;
            CharacterCalculationsWarlock ccw = new CharacterCalculationsWarlock();
            ccw.SubPoints = dps;
            ccw.OverallPoints = dps[0];
            ccw.TotalStats = charStats;
            return ccw;

        }


        /// <summary>
        /// GetCharacterStats is the 2nd-most calculation intensive method in a model. Here the model will
        /// combine all of the information about the character, including race, gear, enchants, buffs,
        /// calculationoptions, etc., to form a single combined Stats object. Three of the methods below
        /// can be called from this method to help total up stats: GetItemStats(character, additionalItem),
        /// GetEnchantsStats(character), and GetBuffsStats(character.ActiveBuffs).
        /// </summary>
        /// <param name="character">The character whose stats should be totaled.</param>
        /// <param name="additionalItem">An additional item to treat the character as wearing.
        /// This is used for gems, which don't have a slot on the character to fit in, so are just
        /// added onto the character, in order to get gem calculations.</param>
        /// <returns>A Stats object containing the final totaled values of all character stats.</returns>
        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            Stats statsRace;
            switch (character.Race)
            {
                case Character.CharacterRace.BloodElf:
                    statsRace = new Stats()
                    {
                        Health = 3213f,
                        Mana = 1961f,
                        Strength = 28f,
                        Agility = 42f,
                        Stamina = 49f,
                        Intellect = 149f,
                        Spirit = 144,
                    };
                    break;
                case Character.CharacterRace.Draenei:
                    statsRace = new Stats()
                    {
                        Health = 3213f,
                        Mana = 1961f,
                        Strength = 28f,
                        Agility = 42f,
                        Stamina = 50f,
                        Intellect = 152f,
                        Spirit = 147,
                    };
                    break;
                case Character.CharacterRace.Gnome:
                    statsRace = new Stats()
                    {
                        Health = 3213f,
                        Mana = 1961f,
                        Strength = 28f,
                        Agility = 42f,
                        Stamina = 50f,
                        Intellect = 154f,
                        Spirit = 145,
                        ArcaneResistance = 10,
                        BonusIntellectMultiplier = .05f 
                    };
                    break;
                case Character.CharacterRace.Human:
                    statsRace = new Stats()
                    {
                        Health = 3213f,
                        Mana = 1961f,
                        Strength = 28f,
                        Agility = 42f,
                        Stamina = 51f,
                        Intellect = 151f,
                        Spirit = 145,
                        BonusIntellectMultiplier = 0.03f,
                        BonusSpiritMultiplier = 0.1f
                    };
                    break;
                case Character.CharacterRace.Troll:
                    statsRace = new Stats()
                    {
                        Health = 3213f,
                        Mana = 1961f,
                        Strength = 28f,
                        Agility = 42f,
                        Stamina = 52f,
                        Intellect = 147f,
                        Spirit = 146,
                        BonusIntellectMultiplier = 0.03f
                    };
                    break;
                case Character.CharacterRace.Undead:
                    statsRace = new Stats()
                    {
                        Health = 3213f,
                        Mana = 1961f,
                        Strength = 28f,
                        Agility = 42f,
                        Stamina = 52f,
                        Intellect = 149f,
                        Spirit = 150,
                        BonusIntellectMultiplier = 0.03f
                    };
                    break;
                default:
                    statsRace = new Stats();
                    break;
            }

            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);


            //base
            Stats statsMinusBuffs = statsRace + statsBaseGear + statsEnchants;

            statsMinusBuffs.Intellect = statsMinusBuffs.Intellect * (1 + statsMinusBuffs.BonusIntellectMultiplier);
            statsMinusBuffs.Stamina = statsMinusBuffs.Stamina * (1 + statsMinusBuffs.BonusStaminaMultiplier);

            //parse talents
            TalentTree tree = character.Talents;
            //Backlash
            statsMinusBuffs.SpellCritRating += tree.GetTalent("Backlash").PointsInvested * 22.08f;
            //Ruin
            statsMinusBuffs.BonusSpellCritMultiplier += tree.GetTalent("Ruin").PointsInvested == 1 ? 0.5f : 0.0f;
            //Demonic Embrace
            statsMinusBuffs.Stamina *= (1f + 0.03f * tree.GetTalent("DemonicEmbrace").PointsInvested);
            //Fel Intellect
            statsMinusBuffs.Intellect *= (1f + 0.01f * tree.GetTalent("FelIntellect").PointsInvested);
            //Fel Stamina
            statsMinusBuffs.Stamina *= (1f + 0.01f * tree.GetTalent("FelStamina").PointsInvested);
            //Fel Armor
            statsMinusBuffs.SpellDamageRating += 100f + (tree.GetTalent("DemonicAegis").PointsInvested * 10f);
            //buffs
            Stats statsTotal = statsMinusBuffs + statsBuffs;

            statsTotal.Intellect = statsTotal.Intellect * (1 + statsBuffs.BonusIntellectMultiplier);
            statsTotal.Stamina = statsTotal.Stamina * (1 + statsBuffs.BonusStaminaMultiplier);

            
            //Calc final derived stats
            statsTotal.SpellCritRating += 22.08f * statsTotal.Intellect / 81.92f;
            statsTotal.Health = (float)Math.Round(((statsTotal.Health + (statsTotal.Stamina * 10f)) * (character.Race == Character.CharacterRace.Tauren ? 1.05f : 1f)));
            statsTotal.Mana = (float)Math.Round(statsTotal.Mana + 15f * statsTotal.Intellect);

            return statsTotal;
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            throw new NotImplementedException();
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            return new Stats()
            {
                Health = stats.Health,
                Mana = stats.Mana,
                Stamina = stats.Stamina,
                Intellect = stats.Intellect,
                Spirit = stats.Spirit,
                Mp5 = stats.Mp5,
                SpellCritRating = stats.SpellCritRating,
                SpellDamageRating = stats.SpellDamageRating,
                SpellFireDamageRating = stats.SpellFireDamageRating,
                SpellShadowDamageRating = stats.SpellShadowDamageRating,
                SpellHasteRating = stats.SpellHasteRating,
                SpellHitRating = stats.SpellHitRating,
                BonusIntellectMultiplier = stats.BonusIntellectMultiplier,
                BonusSpellCritMultiplier = stats.BonusSpellCritMultiplier,
                BonusSpellPowerMultiplier = stats.BonusSpellPowerMultiplier,
                BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                BonusSpiritMultiplier = stats.BonusSpiritMultiplier,
                BonusFireSpellPowerMultiplier = stats.BonusFireSpellPowerMultiplier,
                BonusShadowSpellPowerMultiplier = stats.BonusShadowSpellPowerMultiplier,
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            return (
                stats.Health +
                stats.Mana +
                stats.Stamina +
                stats.Intellect +
                stats.Spirit +
                stats.Mp5 +
                stats.SpellCritRating +
                stats.SpellDamageRating +
                stats.SpellFireDamageRating +
                stats.SpellShadowDamageRating +
                stats.SpellHasteRating +
                stats.SpellHitRating +
                stats.BonusIntellectMultiplier +
                stats.BonusSpellCritMultiplier +
                stats.BonusSpellPowerMultiplier +
                stats.BonusStaminaMultiplier +
                stats.BonusSpiritMultiplier +
                stats.SpellFireDamageRating +
                stats.SpellShadowDamageRating +
                stats.BonusFireSpellPowerMultiplier +
                stats.BonusShadowSpellPowerMultiplier) > 0;
                  }
    }
}