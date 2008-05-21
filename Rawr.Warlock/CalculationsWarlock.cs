using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Warlock
{
	[System.ComponentModel.DisplayName("Warlock|Spell_Nature_FaerieFire")]
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
                if (_characterDisplayCalculationLabels == null)
                {
                    List<string> labels = new List<string>(new string[]
                    {
                        "Basic Stats:Health",
                        "Basic Stats:Mana",
                        "Basic Stats:Stamina",
                        "Basic Stats:Intellect",
                        "Spell Stats:Total Crit %",
                        "Spell Stats:Hit %",
                        "Spell Stats:Haste %",
                        //"Spell Stats:Casting Speed",
                        "Spell Stats:Shadow Damage",
                        "Spell Stats:Fire Damage",
                        "Overall Stats:Total Damage", 
                        "Overall Stats:DPS",
                        //"Shadowbolt Stats:SB Min Hit",
                        //"Shadowbolt Stats:SB Max Hit",
                        //"Shadowbolt Stats:SB Min Crit",
                        //"Shadowbolt Stats:SB Max Crit",
                        //"Shadowbolt Stats:SB Average Hit",
                        //"Shadowbolt Stats:SB Crit Rate",
                        //"Shadowbolt Stats:ISB Uptime",
                        //"Shadowbolt Stats:#SB Casts",
                        //"Incinerate Stats:Incinerate Min Hit",
                        //"Incinerate Stats:Incinerate Max Hit",
                        //"Incinerate Stats:Incinerate Min Crit",
                        //"Incinerate Stats:Incinerate Max Crit",
                        //"Incinerate Stats:Incinerate Average Hit",
                        //"Incinerate Stats:Incinerate Crit Rate",
                        //"Incinerate Stats:#Incinerate Casts",
                        //"Immolate Stats:ImmolateMin Hit",
                        //"Immolate Stats:ImmolateMax Hit",
                        //"Immolate Stats:ImmolateMin Crit",
                        //"Immolate Stats:ImmolateMax Crit",
                        //"Immolate Stats:ImmolateAverage Hit",
                        //"Immolate Stats:ImmolateCrit Rate",
                        //"Immolate Stats:#Immolate Casts",
                        //"Curse of Agony Stats:CoA Tick",
                        //"Curse of Agony Stats:CoA Total Damage",
                        //"Curse of Agony Stats:#CoA Casts",
                        //"Curse of Doom Stats:CoD Total Damage",
                        //"Curse of Doom Stats:#CoD Casts",
                        //"Corruption Stats:Corr Tick",
                        //"Corruption Stats:Corr Total Damage",
                        //"Corruption Stats:#Corr Casts",
                        //"Unstable Affliction Stats:UA Tick",
                        //"Unstable Affliction Stats:UA Total Damage",
                        //"Unstable Affliction Stats:#UA Casts",
                        //"SiphonLife Stats:SL Tick",
                        //"SiphonLife Stats:SL Total Damage",
                        //"SiphonLife Stats:#SL Casts",
                        //"Lifetap Stats:#Lifetaps",
                        //"Lifetap Stats:Mana Per LT"

                    });
                    _characterDisplayCalculationLabels = labels.ToArray();   
                }
                return _characterDisplayCalculationLabels;
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
                if (_relevantItemTypes == null)
                {
                    _relevantItemTypes = new List<Item.ItemType>(new Item.ItemType[]
					{
						Item.ItemType.None,
						Item.ItemType.Cloth,
						Item.ItemType.Dagger,
						Item.ItemType.OneHandSword,
						Item.ItemType.Staff,
						Item.ItemType.Wand,
					});
                }
                return _relevantItemTypes;
            }
        }

		public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.Warlock; } }
		public override ComparisonCalculationBase CreateNewComparisonCalculation()
        {
            return new ComparisonCalculationWarlock();
        }

        public override CharacterCalculationsBase CreateNewCharacterCalculations()
        {
            return new CharacterCalculationsWarlock();
        }


		public override ICalculationOptionBase DeserializeDataObject(string xml)
		{
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsWarlock));
			System.IO.StringReader reader = new System.IO.StringReader(xml);
			CalculationOptionsWarlock calcOpts = serializer.Deserialize(reader) as CalculationOptionsWarlock;
			return calcOpts;
		}

        public Character GetTalents(Character character)
        {
            CalculationOptionsWarlock options = character.CalculationOptions as CalculationOptionsWarlock;

            if (!options.TalentsSaved)
            {
                TalentTree tree = character.Talents;
                options.Suppression = tree.GetTalent("Suppression").PointsInvested;
                options.ImprovedCorruption = tree.GetTalent("ImprovedCorruption").PointsInvested;
                options.ImprovedDrainSoul = tree.GetTalent("ImprovedDrainSoul").PointsInvested;
                options.ImprovedLifeTap = tree.GetTalent("ImprovedLifeTap").PointsInvested;
                options.SoulSiphon = tree.GetTalent("SoulSiphon").PointsInvested;
                options.ImprovedCurseOfAgony = tree.GetTalent("ImprovedCurseOfAgony").PointsInvested;
                options.AmplifyCurse = tree.GetTalent("AmplifyCurse").PointsInvested;
                options.Nightfall = tree.GetTalent("Nightfall").PointsInvested;
                options.EmpoweredCorruption = tree.GetTalent("EmpoweredCorruption").PointsInvested;
                options.ShadowMastery = tree.GetTalent("ShadowMastery").PointsInvested;
                options.Contagion = tree.GetTalent("Contagion").PointsInvested;
                options.ImprovedImp = tree.GetTalent("ImprovedImp").PointsInvested;
                options.DemonicEmbrace = tree.GetTalent("DemonicEmbrace").PointsInvested;
                options.FelIntellect = tree.GetTalent("FelIntellect").PointsInvested;
                options.FelStamina = tree.GetTalent("FelStamina").PointsInvested;
                options.DemonicAegis = tree.GetTalent("DemonicAegis").PointsInvested;
                options.UnholyPower = tree.GetTalent("UnholyPower").PointsInvested;
                options.DemonicSacrifice = tree.GetTalent("DemonicSacrifice").PointsInvested;
                options.ManaFeed = tree.GetTalent("ManaFeed").PointsInvested;
                options.MasterDemonologist = tree.GetTalent("MasterDemonologist").PointsInvested;
                options.SoulLink = tree.GetTalent("SoulLink").PointsInvested;
                options.DemonicKnowledge = tree.GetTalent("DemonicKnowledge").PointsInvested;
                options.DemonicTactics = tree.GetTalent("DemonicTactics").PointsInvested;
                options.ImprovedShadowbolt = tree.GetTalent("ImprovedShadowBolt").PointsInvested;
                options.Cataclysm = tree.GetTalent("Cataclysm").PointsInvested;
                options.Bane = tree.GetTalent("Bane").PointsInvested;
                options.ImprovedFirebolt = tree.GetTalent("ImprovedFirebolt").PointsInvested;
                options.ImprovedLashOfPain = tree.GetTalent("ImprovedLashOfPain").PointsInvested;
                options.Devastation = tree.GetTalent("Devastation").PointsInvested;
                options.ImprovedSearingPain = tree.GetTalent("ImprovedSearingPain").PointsInvested;
                options.DestructiveReach = tree.GetTalent("DestructiveReach").PointsInvested;
                options.ImprovedImmolate = tree.GetTalent("ImprovedImmolate").PointsInvested;
                options.Ruin = tree.GetTalent("Ruin").PointsInvested;
                options.Emberstorm = tree.GetTalent("Emberstorm").PointsInvested;
                options.Backlash = tree.GetTalent("Backlash").PointsInvested;
                options.SoulLeech = tree.GetTalent("SoulLeech").PointsInvested;
                options.ShadowAndFlame = tree.GetTalent("ShadowAndFlame").PointsInvested;
            }

            return character;
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
            CharacterCalculationsWarlock calculations = new CharacterCalculationsWarlock();
            character = GetTalents(character);
            calculations.BasicStats = GetCharacterStats(character, additionalItem);
			calculations.CalculationOptions = character.CalculationOptions as CalculationOptionsWarlock;

            calculations.HitPercent = calculations.BasicStats.SpellHitRating / 12.62f;
            calculations.CritPercent = calculations.BasicStats.SpellCritRating / 22.08f;
            calculations.HastePercent = calculations.BasicStats.SpellHasteRating / 15.77f;
            calculations.GlobalCooldown = 1.5f / (1 + 0.01f * calculations.HastePercent);
            if (calculations.GlobalCooldown < 1)
                calculations.GlobalCooldown = 1;
            calculations.ShadowDamage = calculations.BasicStats.SpellDamageRating + calculations.BasicStats.SpellShadowDamageRating;
            calculations.FireDamage = calculations.BasicStats.SpellDamageRating + calculations.BasicStats.SpellFireDamageRating;

            WarlockSpellRotation wsr = new WarlockSpellRotation(calculations);
            wsr.Calculate(false);
            //wsr.CalculateAdvancedInfo();

            Stats totalStats = calculations.BasicStats;
            //T4 2 piece bonus
            totalStats.SpellShadowDamageRating += totalStats.BonusWarlockSchoolDamageOnCast * (1 - (float)Math.Pow(0.95, 15 * wsr.ShadowSpellsPerSecond));
            totalStats.SpellFireDamageRating += totalStats.BonusWarlockSchoolDamageOnCast * (1 - (float)Math.Pow(0.95, 15 * wsr.FireSpellsPerSecond));

            //Spellstrike 2 piece bonus
            totalStats.SpellDamageRating += totalStats.SpellDamageFor10SecOnHit_5 * (1 - (float)Math.Pow(0.95, 10 * wsr.SpellsPerSecond));
            
            //Quagmirran's Eye
            totalStats.HasteRating += totalStats.SpellHasteFor6SecOnHit_10_45 * 6 / (45 + 9 / wsr.SpellsPerSecond);

            //Band of the Eternal Sage
            totalStats.SpellDamageRating += totalStats.SpellDamageFor10SecOnHit_10_45 * 10 / (45 + 9 / wsr.SpellsPerSecond);

            calculations.HastePercent = totalStats.SpellHasteRating / 15.77f;
            calculations.GlobalCooldown = 1.5f / (1 + 0.01f * calculations.HastePercent);
            if (calculations.GlobalCooldown < 1)
                calculations.GlobalCooldown = 1;
            calculations.ShadowDamage = totalStats.SpellDamageRating + totalStats.SpellShadowDamageRating;
            calculations.FireDamage = totalStats.SpellDamageRating + totalStats.SpellFireDamageRating;

            wsr.Calculate(true);
            //wsr.CalculateDps();

            return calculations;
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
                case Character.CharacterRace.Orc:
                    statsRace = new Stats()
                    {
                        Health = 3310f,
                        Mana = 2335f,
                        Strength = 28f,
                        Agility = 42f,
                        Stamina = 100f,
                        Intellect = 100f,
                        Spirit = 144,
                    };
                    break;
                case Character.CharacterRace.BloodElf:
                    statsRace = new Stats()
                    {
                        Health = 3310f,
                        Mana = 2335f,
                        Strength = 28f,
                        Agility = 42f,
                        Stamina = 100f,
                        Intellect = 100f,
                        Spirit = 144,
                    };
                    break;
                case Character.CharacterRace.Gnome:
                    statsRace = new Stats()
                    {
                        Health = 3310f,
                        Mana = 2335f,
                        Strength = 28f,
                        Agility = 42f,
                        Stamina = 75f,
                        Intellect = 135f,
                        Spirit = 145,
                        ArcaneResistance = 10,
                        BonusIntellectMultiplier = .05f 
                    };
                    break;
                case Character.CharacterRace.Human:
                    statsRace = new Stats()
                    {
                        Health = 3310f,
                        Mana = 2335f,
                        Strength = 28f,
                        Agility = 42f,
                        Stamina = 76f,
                        Intellect = 129f,
                        Spirit = 145,
                        BonusSpiritMultiplier = 0.1f
                    };
                    break;
                case Character.CharacterRace.Troll:
                    statsRace = new Stats()
                    {
                        Health = 3310f,
                        Mana = 2335f,
                        Strength = 28f,
                        Agility = 42f,
                        Stamina = 52f,
                        Intellect = 147f,
                        Spirit = 146
                    };
                    break;
                case Character.CharacterRace.Undead:
                    statsRace = new Stats()
                    {
                        Health = 3310f,
                        Mana = 2335f,
                        Strength = 28f,
                        Agility = 42f,
                        Stamina = 77f,
                        Intellect = 131f,
                        Spirit = 150
                    };
                    break;
                default:
                    statsRace = new Stats();
                    break;
            }

            CalculationOptionsWarlock options = character.CalculationOptions as CalculationOptionsWarlock;
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
            Stats statsGearEnchantsBuffs = statsBaseGear + statsEnchants + statsBuffs;

            Stats statsTotal = statsGearEnchantsBuffs + statsRace;

            statsTotal.BonusSpellPowerMultiplier += 1;
            statsTotal.BonusShadowSpellPowerMultiplier += 1;
            statsTotal.BonusFireSpellPowerMultiplier += 1;

            //strength
            statsTotal.Strength = (float)Math.Floor((Math.Floor(statsRace.Strength * (1 + statsRace.BonusStrengthMultiplier)) + statsGearEnchantsBuffs.Strength * (1 + statsRace.BonusStrengthMultiplier)) * (1 + statsGearEnchantsBuffs.BonusStrengthMultiplier));

            //agility
            statsTotal.Agility = (float)Math.Floor((Math.Floor(statsRace.Agility * (1 + statsRace.BonusAgilityMultiplier)) + statsGearEnchantsBuffs.Agility * (1 + statsRace.BonusAgilityMultiplier)) * (1 + statsGearEnchantsBuffs.BonusAgilityMultiplier));
            
            //intellect
            statsTotal.Intellect = (float)Math.Floor((Math.Floor(statsRace.Intellect * (1 + statsRace.BonusIntellectMultiplier)) + statsGearEnchantsBuffs.Intellect * (1 + statsRace.BonusIntellectMultiplier)) * (1 + statsGearEnchantsBuffs.BonusIntellectMultiplier));
            
            //stamina
            statsTotal.Stamina = (float)Math.Floor((Math.Floor(statsRace.Stamina * (1 + statsRace.BonusStaminaMultiplier)) + statsGearEnchantsBuffs.Stamina * (1 + statsRace.BonusStaminaMultiplier)) * (1 + statsGearEnchantsBuffs.BonusStaminaMultiplier));
            statsTotal.Stamina = (float)Math.Floor(statsTotal.Stamina * (1 + 0.03f * options.DemonicEmbrace));

            //spirit
            statsTotal.Spirit = (float)Math.Floor((Math.Floor(statsRace.Spirit * (1 + statsRace.BonusSpiritMultiplier)) + statsGearEnchantsBuffs.Spirit * (1 + statsRace.BonusSpiritMultiplier)) * (1 + statsGearEnchantsBuffs.BonusSpiritMultiplier));
            statsTotal.Spirit = (float)Math.Floor(statsTotal.Spirit * (1 - 0.01f * options.DemonicEmbrace));

            //spell damage
            statsTotal.SpellDamageRating += statsTotal.SpellDamageFor20SecOnUse2Min / 6;
            statsTotal.SpellDamageRating += statsTotal.SpellDamageFor15SecOnUse90Sec * 15 / 90;
            statsTotal.SpellDamageRating += statsTotal.SpellDamageFor15SecOnUse2Min * 15 / 120;
            statsTotal.SpellDamageRating += 100 + options.DemonicAegis * 10; //assume Fel Armor

            //spell crit rating
            statsTotal.SpellCritRating += (1.701f + statsTotal.Intellect / 82 + options.DemonicTactics + options.Backlash + options.Devastation) * 22.08f;

            //spell haste rating
            statsTotal.SpellHasteRating += statsTotal.DrumsOfBattle * 30 / 120;
            statsTotal.SpellHasteRating += statsTotal.SpellHasteFor20SecOnUse2Min / 6;
            if (statsTotal.Bloodlust > 0)
                statsTotal.SpellHasteRating += 30 * 15.77f * 40 / 600;

            //mp5
            statsTotal.Mp5 += 100; //Assume Super Mana Potions (TODO: Add some kind of potion selector)

            //health
            statsTotal.Health = (float)Math.Round(((statsRace.Health + statsGearEnchantsBuffs.Health + (statsTotal.Stamina * 10f)) * (character.Race == Character.CharacterRace.Tauren ? 1.05f : 1f)));
            statsTotal.Health = (float)Math.Round(statsTotal.Health * (1 + 0.01f * options.FelStamina));

            //mana
            statsTotal.Mana = (float)Math.Round(statsRace.Mana + 15f * statsTotal.Intellect + statsGearEnchantsBuffs.Mana);
            statsTotal.Mana = (float)Math.Round(statsTotal.Mana * (1 + 0.01f * options.FelIntellect));

            //armor
            statsTotal.Armor = (float)Math.Round(statsGearEnchantsBuffs.Armor + statsTotal.Agility * 2);           

            if (!options.PetSacrificed)
            {
                switch (options.Pet)
                {
                    case Pet.Imp:
                        statsTotal.ThreatReductionMultiplier *= 1 + (0.04f * options.MasterDemonologist);
                        break;
                    case Pet.Succubus:
                        statsTotal.BonusSpellPowerMultiplier *= 1 + (0.02f * options.MasterDemonologist);
                        break;
                    case Pet.Felhunter:
                        statsTotal.AllResist += (0.2f * options.MasterDemonologist * 70);
                        break;
                    case Pet.Felguard:
                        statsTotal.BonusSpellPowerMultiplier *= 1 + (0.01f * options.MasterDemonologist);
                        statsTotal.AllResist += (0.1f * options.MasterDemonologist * 70);
                        break;
                }
            }
            else if (options.DemonicSacrifice == 1)
            {
                switch (options.Pet)
                {
                    case Pet.Imp:
                        statsTotal.BonusFireSpellPowerMultiplier *= 1 + 0.15f;
                        break;
                    case Pet.Succubus:
                        statsTotal.BonusShadowSpellPowerMultiplier *= 1 + 0.15f;
                        break;
                    case Pet.Felhunter:
                        statsTotal.Mp5 += statsTotal.Mana * 0.0375f;
                        break;
                    case Pet.Felguard:
                        statsTotal.BonusShadowSpellPowerMultiplier *= 1 + 0.1f;
                        statsTotal.Mp5 += statsTotal.Mana * 0.025f;
                        break;
                    case Pet.Voidwalker:
                        statsTotal.Hp5 += statsTotal.Health * 0.025f;
                        break;
                }
            }

            //Emberstorm
            //have to find some way to represent the reduction in cast time for Incinerate
            statsTotal.BonusFireSpellPowerMultiplier *= 1 + (character.Talents.GetTalent("Emberstorm").PointsInvested * 0.02f);

            //Shadow Mastery
            statsTotal.BonusShadowSpellPowerMultiplier *= 1 + (character.Talents.GetTalent("ShadowMastery").PointsInvested * 0.02f);

            //Soul Link
            statsTotal.BonusSpellPowerMultiplier *= 1 + options.SoulLink * 0.05f;
            
            //Demonic Knowledge
            //TODO: Add pet stats and make Demonic Knowledge model correctly
            //int demonicKnowledge = tree.GetTalent("DemonicKnowledge").PointsInvested;

            //statsTotal.SpellDamageRating *= 1f + 0.05f * pointsInDemonicKnowledge;
            //statsTotal.SpellShadowDamageRating *= 1f + 0.05f * pointsInDemonicKnowledge;
            //statsTotal.SpellFireDamageRating *= 1f + 0.05f * pointsInDemonicKnowledge;

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
                AllResist = stats.AllResist,
                ArcaneResistance = stats.ArcaneResistance,
                FireResistance = stats.FireResistance,
                FrostResistance = stats.FrostResistance,
                NatureResistance = stats.NatureResistance,
                ShadowResistance = stats.ShadowResistance,
                Stamina = stats.Stamina,
                Intellect = stats.Intellect,
                Spirit = stats.Spirit,
                DodgeRating = stats.DodgeRating,
                Health = stats.Health,
                Mp5 = stats.Mp5,
                Resilience = stats.Resilience,
                SpellCritRating = stats.SpellCritRating,
                SpellDamageRating = stats.SpellDamageRating,
                SpellFireDamageRating = stats.SpellFireDamageRating,
                SpellShadowDamageRating = stats.SpellShadowDamageRating,
                SpellArcaneDamageRating = stats.SpellArcaneDamageRating,
                SpellFrostDamageRating = stats.SpellFrostDamageRating,
                SpellPenetration = stats.SpellPenetration,
                SpellHasteRating = stats.SpellHasteRating,
                SpellHitRating = stats.SpellHitRating,
                BonusIntellectMultiplier = stats.BonusIntellectMultiplier,
                BonusSpellCritMultiplier = stats.BonusSpellCritMultiplier,
                BonusSpellPowerMultiplier = stats.BonusSpellPowerMultiplier,
                BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                BonusSpiritMultiplier = stats.BonusSpiritMultiplier,
                Mana = stats.Mana,
                Armor = stats.Armor,
                Hp5 = stats.Hp5,
                SpellCombatManaRegeneration = stats.SpellCombatManaRegeneration,
                BonusShadowSpellPowerMultiplier = stats.BonusAgilityMultiplier, 
                BonusArcaneSpellPowerMultiplier = stats.BonusArcaneSpellPowerMultiplier,
                BonusFireSpellPowerMultiplier = stats.BonusFireSpellPowerMultiplier,
                BonusFrostSpellPowerMultiplier = stats.BonusFrostSpellPowerMultiplier,
                BonusWarlockNukeMultiplier = stats.BonusWarlockNukeMultiplier, 
                LightningCapacitorProc = stats.LightningCapacitorProc,
                TimbalsProc = stats.TimbalsProc, 
                SpellDamageFor20SecOnUse2Min = stats.SpellDamageFor20SecOnUse2Min,
                SpellHasteFor20SecOnUse2Min = stats.SpellHasteFor20SecOnUse2Min,
                Mp5OnCastFor20SecOnUse2Min = stats.Mp5OnCastFor20SecOnUse2Min,
                ManaRestorePerCast = stats.ManaRestorePerCast,
                ManaRestorePerHit = stats.ManaRestorePerHit,
                SpellDamageFor10SecOnHit_10_45 = stats.SpellDamageFor10SecOnHit_10_45,
                SpellDamageFromIntellectPercentage = stats.SpellDamageFromIntellectPercentage,
                SpellDamageFromSpiritPercentage = stats.SpellDamageFromSpiritPercentage,
                SpellDamageFor10SecOnResist = stats.SpellDamageFor10SecOnResist,
                SpellDamageFor15SecOnCrit_20_45 = stats.SpellDamageFor15SecOnCrit_20_45,
                SpellDamageFor15SecOnUse90Sec = stats.SpellDamageFor15SecOnUse90Sec,
                SpellDamageFor15SecOnUse2Min = stats.SpellDamageFor15SecOnUse2Min, 
                SpellHasteFor6SecOnCast_15_45 = stats.SpellHasteFor6SecOnCast_15_45,
                SpellDamageFor10SecOnHit_5 = stats.SpellDamageFor10SecOnHit_5,
                SpellHasteFor6SecOnHit_10_45 = stats.SpellHasteFor6SecOnHit_10_45,
                SpellDamageFor10SecOnCrit_20_45 = stats.SpellDamageFor10SecOnCrit_20_45,
                BonusManaPotion = stats.BonusManaPotion,
                ThreatIncreaseMultiplier = stats.ThreatIncreaseMultiplier,
                ThreatReductionMultiplier = stats.ThreatReductionMultiplier,
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            float warlockStats = stats.Stamina
                + stats.Intellect
                + stats.Spirit
                + stats.Mp5
                + stats.SpellCritRating
                + stats.SpellHasteRating
                + stats.SpellHitRating
                + stats.SpellDamageRating
                + stats.SpellFireDamageRating
                + stats.SpellShadowDamageRating
                + stats.BonusStaminaMultiplier
                + stats.BonusIntellectMultiplier
                + stats.BonusSpellCritMultiplier
                + stats.BonusSpellPowerMultiplier
                + stats.BonusFireSpellPowerMultiplier
                + stats.BonusShadowSpellPowerMultiplier
                + stats.BonusWarlockNukeMultiplier
                + stats.BonusSpiritMultiplier
                + stats.SpellPenetration
                + stats.Mana
                + stats.Health
                + stats.LightningCapacitorProc
                + stats.SpellDamageFor20SecOnUse2Min
                + stats.SpellHasteFor20SecOnUse2Min
                + stats.Mp5OnCastFor20SecOnUse2Min
                + stats.ManaRestorePerHit
                + stats.SpellDamageFor10SecOnHit_10_45
                + stats.SpellDamageFromSpiritPercentage
                + stats.SpellDamageFor10SecOnResist
                + stats.SpellDamageFor15SecOnCrit_20_45
                + stats.SpellDamageFor15SecOnUse90Sec
                + stats.SpellHasteFor6SecOnCast_15_45
                + stats.SpellDamageFor10SecOnHit_5
                + stats.SpellHasteFor6SecOnHit_10_45
                + stats.SpellDamageFor10SecOnCrit_20_45
                + stats.SpellDamageFor15SecOnUse2Min
                + stats.TimbalsProc
                + stats.BonusManaPotion
                + stats.ThreatReductionMultiplier
                + stats.AllResist
                + stats.ArcaneResistance
                + stats.FireResistance
                + stats.FrostResistance
                + stats.NatureResistance
                + stats.ShadowResistance
                + stats.Bloodlust
                + stats.DrumsOfBattle
                + stats.DrumsOfWar; 
            
            return warlockStats > 0;
        }
    }
}