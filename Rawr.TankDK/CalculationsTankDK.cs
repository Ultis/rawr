using System;
using System.Collections.Generic;
using System.Text;


namespace Rawr.TankDK
{
    [Rawr.Calculations.RawrModelInfo("TankDK", "spell_shadow_deathanddecay", Character.CharacterClass.DeathKnight)]
    class CalculationsTankDK : CalculationsBase
    {
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
				////Relevant Gem IDs for TankDKs
				//Red
				int[] subtle = { 39907, 40000, 40115, 42151 };

				//Purple
				int[] regal = { 39938, 40031, 40138 };

				//Blue
				int[] solid = { 39919, 40008, 40119, 36767 };

				//Green
				int[] enduring = { 39976, 40089, 40167 };

				//Yellow
				int[] thick = { 39916, 40015, 40126, 42157 };

				//Orange
				int[] stalwart = { 39964, 40056, 40160 };

				//Meta
				int austere = 41380;

				return new List<GemmingTemplate>()
				{
				    new GemmingTemplate() { Model = "TankDK", Group = "Uncommon", //Max Defense
				        RedId = thick[0], YellowId = thick[0], BlueId = thick[0], PrismaticId = thick[0], MetaId = austere },
				    new GemmingTemplate() { Model = "TankDK", Group = "Uncommon", //Defense 
				        RedId = stalwart[0], YellowId = thick[0], BlueId = enduring[0], PrismaticId = thick[0], MetaId = austere },
				    new GemmingTemplate() { Model = "TankDK", Group = "Uncommon", //Max Dodge
				        RedId = subtle[0], YellowId = subtle[0], BlueId = subtle[0], PrismaticId = subtle[0], MetaId = austere },
				    new GemmingTemplate() { Model = "TankDK", Group = "Uncommon", //Dodge
				        RedId = subtle[0], YellowId = stalwart[0], BlueId = regal[0], PrismaticId = subtle[0], MetaId = austere },
				    new GemmingTemplate() { Model = "TankDK", Group = "Uncommon", //Max Stamina
				        RedId = solid[0], YellowId = solid[0], BlueId = solid[0], PrismaticId = solid[0], MetaId = austere },
				    new GemmingTemplate() { Model = "TankDK", Group = "Uncommon", //Stamina
				        RedId = regal[0], YellowId = enduring[0], BlueId = solid[0], PrismaticId = solid[0], MetaId = austere },
						
				    new GemmingTemplate() { Model = "TankDK", Group = "Rare", Enabled = true, //Max Defense
				        RedId = thick[1], YellowId = thick[1], BlueId = thick[1], PrismaticId = thick[1], MetaId = austere },
				    new GemmingTemplate() { Model = "TankDK", Group = "Rare", Enabled = true, //Defense 
				        RedId = stalwart[1], YellowId = thick[1], BlueId = enduring[1], PrismaticId = thick[1], MetaId = austere },
				    new GemmingTemplate() { Model = "TankDK", Group = "Rare", Enabled = true, //Max Dodge
				        RedId = subtle[1], YellowId = subtle[1], BlueId = subtle[1], PrismaticId = subtle[1], MetaId = austere },
				    new GemmingTemplate() { Model = "TankDK", Group = "Rare", Enabled = true, //Dodge
				        RedId = subtle[1], YellowId = stalwart[1], BlueId = regal[1], PrismaticId = subtle[1], MetaId = austere },
				    new GemmingTemplate() { Model = "TankDK", Group = "Rare", Enabled = true, //Max Stamina
				        RedId = solid[1], YellowId = solid[1], BlueId = solid[1], PrismaticId = solid[1], MetaId = austere },
				    new GemmingTemplate() { Model = "TankDK", Group = "Rare", Enabled = true, //Stamina
				        RedId = regal[1], YellowId = enduring[1], BlueId = solid[1], PrismaticId = solid[1], MetaId = austere },

				    new GemmingTemplate() { Model = "TankDK", Group = "Epic", //Max Defense
				        RedId = thick[2], YellowId = thick[2], BlueId = thick[2], PrismaticId = thick[2], MetaId = austere },
				    new GemmingTemplate() { Model = "TankDK", Group = "Epic", //Defense 
				        RedId = stalwart[2], YellowId = thick[2], BlueId = enduring[2], PrismaticId = thick[2], MetaId = austere },
				    new GemmingTemplate() { Model = "TankDK", Group = "Epic", //Max Dodge
				        RedId = subtle[2], YellowId = subtle[2], BlueId = subtle[2], PrismaticId = subtle[2], MetaId = austere },
				    new GemmingTemplate() { Model = "TankDK", Group = "Epic", //Dodge
				        RedId = subtle[2], YellowId = stalwart[2], BlueId = regal[2], PrismaticId = subtle[2], MetaId = austere },
				    new GemmingTemplate() { Model = "TankDK", Group = "Epic", //Max Stamina
				        RedId = solid[2], YellowId = solid[2], BlueId = solid[2], PrismaticId = solid[2], MetaId = austere },
				    new GemmingTemplate() { Model = "TankDK", Group = "Epic", //Stamina
				        RedId = regal[2], YellowId = enduring[2], BlueId = solid[2], PrismaticId = solid[2], MetaId = austere },

				    new GemmingTemplate() { Model = "TankDK", Group = "Jeweler", //Max Defense
				        RedId = thick[3], YellowId = thick[3], BlueId = thick[3], PrismaticId = thick[3], MetaId = austere },
				    new GemmingTemplate() { Model = "TankDK", Group = "Jeweler", //Defense 
				        RedId = thick[3], YellowId = thick[2], BlueId = thick[3], PrismaticId = thick[2], MetaId = austere },
				    new GemmingTemplate() { Model = "TankDK", Group = "Jeweler", //Max Dodge
				        RedId = subtle[3], YellowId = subtle[3], BlueId = subtle[3], PrismaticId = subtle[3], MetaId = austere },
				    new GemmingTemplate() { Model = "TankDK", Group = "Jeweler", //Dodge
				        RedId = subtle[2], YellowId = subtle[3], BlueId = subtle[3], PrismaticId = subtle[2], MetaId = austere },
				    new GemmingTemplate() { Model = "TankDK", Group = "Jeweler", //Max Stamina
				        RedId = solid[3], YellowId = solid[3], BlueId = solid[3], PrismaticId = solid[3], MetaId = austere },
				    new GemmingTemplate() { Model = "TankDK", Group = "Jeweler", //Stamina
				        RedId = solid[3], YellowId = solid[3], BlueId = solid[2], PrismaticId = solid[2], MetaId = austere },
				};
            }
        }

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
            get
            {
                if (_subPointNameColors == null)
                {
                    _subPointNameColors = new Dictionary<string, System.Drawing.Color>();
                    _subPointNameColors.Add("Mitigation", System.Drawing.Color.Blue);
                    _subPointNameColors.Add("Survival", System.Drawing.Color.Yellow);
                    _subPointNameColors.Add("Threat", System.Drawing.Color.Red);
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

                        "Summary:Survival Points",
                        "Summary:Mitigation Points",
                        "Summary:Overall Points",

                        "Basic Stats:Strength",
					    "Basic Stats:Agility",
                        "Basic Stats:Stamina",
					    "Basic Stats:Attack Power",
					    "Basic Stats:Crit Rating",
					    "Basic Stats:Hit Rating",
					    "Basic Stats:Expertise",
					    "Basic Stats:Haste Rating",
/*					    "Basic Stats:Armor Penetration",*/
                        "Basic Stats:Health",
                        "Basic Stats:Armor",

                        "Diminishing Returns:DR Dodge",
                        "Diminishing Returns:DR Parry",
                        "Diminishing Returns:DR Defense",

                        "Defense:Crit*Enemy's crit chance on you",
                        "Defense:Defense Rating",
                        "Defense:Defense",
                        "Defense:Defense Rating needed",

                        "Advanced Stats:Miss",
                        "Advanced Stats:Dodge",
                        "Advanced Stats:Parry*With Blade Barrier",
                        "Advanced Stats:Total Avoidance",
                        "Advanced Stats:Armor Damage Reduction",

                        "Threat Stats:Target Miss*Chance to miss the target",
                        "Threat Stats:Target Dodge*Chance the target dodges",
                        "Threat Stats:Target Parry*Chance the target parries",
                        "Threat Stats:Threat",

                        "Overall Stats:Overall",
                        "Overall Stats:Modified Survival",
                        "Overall Stats:Modified Mitigation",
                        "Overall Stats:Modified Threat",
                    });
                    _characterDisplayCalculationLabels = labels.ToArray();
                }
                return _characterDisplayCalculationLabels;
            }
        }


        private string[] _customChartNames = null;
        /// <summary>
        /// The names of all custom charts provided by the model, if any.
        /// </summary>
        public override string[] CustomChartNames
        {
            get
            {
                if (_customChartNames == null)
                {
                    _customChartNames = new string[] { };
                }
                return _customChartNames;
            }
        }


        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
        /// <summary>
        /// A custom panel inheriting from CalculationOptionsPanelBase which contains controls for
        /// setting CalculationOptions for the model. CalculationOptions are stored in the Character,
        /// and can be used by multiple models. See comments on CalculationOptionsPanelBase for more details.
        /// </summary>
        public override CalculationOptionsPanelBase CalculationOptionsPanel
        {
            get { return _calculationOptionsPanel ?? (_calculationOptionsPanel = new CalculationOptionsPanelTankDK()); }
        }


        private List<Item.ItemType> _relevantItemTypes = null;
        /// <summary>
        /// List<Item.ItemType> containing all of the ItemTypes relevant to this model. Typically this
        /// means all types of armor/weapons that the intended class is able to use, but may also
        /// be trimmed down further if some aren't typically used. Item.ItemType.None should almost
        /// always be included, because that type includes items with no proficiancy requirement, such
        /// as rings, necklaces, cloaks, held in off hand items, etc.
        /// 
        /// EXAMPLE:
        /// relevantItemTypes = new List<Item.ItemType>(new Item.ItemType[]
        /// {
        ///     Item.ItemType.None,
        ///     Item.ItemType.Leather,
        ///     Item.ItemType.Idol,
        ///     Item.ItemType.Staff,
        ///     Item.ItemType.TwoHandMace
        /// });
        /// </summary>
        public override List<Item.ItemType> RelevantItemTypes
        {
            get
            {
                return _relevantItemTypes ?? (_relevantItemTypes = new List<Item.ItemType>(new Item.ItemType[]
					{
						Item.ItemType.None,
                        Item.ItemType.Leather,
                        Item.ItemType.Mail,
                        Item.ItemType.Plate,
                        Item.ItemType.Sigil,
                        Item.ItemType.Polearm,
                        Item.ItemType.TwoHandAxe,
                        Item.ItemType.TwoHandMace,
                        Item.ItemType.TwoHandSword,
                        Item.ItemType.OneHandAxe,
                        Item.ItemType.OneHandMace,
                        Item.ItemType.OneHandSword
					}));
            }
        }


        /// <summary>
        /// Character class that this model is for.
        /// </summary>
        public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.DeathKnight; } }

        /// <summary>
        /// Method to get a new instance of the model's custom ComparisonCalculation class.
        /// </summary>
        /// <returns>A new instance of the model's custom ComparisonCalculation class, 
        /// which inherits from ComparisonCalculationBase</returns>
        public override ComparisonCalculationBase CreateNewComparisonCalculation()
        {
            return new ComparisonCalculationTankDK();
        }

        /// <summary>
        /// Method to get a new instance of the model's custom CharacterCalculations class.
        /// </summary>
        /// <returns>A new instance of the model's custom CharacterCalculations class, 
        /// which inherits from CharacterCalculationsBase</returns>
        public override CharacterCalculationsBase CreateNewCharacterCalculations()
        {
            return new CharacterCalculationsTankDK();
        }

        /// <summary>
        /// An array of strings which define what calculations (in addition to the subpoint ratings)
        /// will be available to the optimizer
        /// </summary>
        public override string[] OptimizableCalculationLabels { get { return new string[] {
            "Crit Reduction",
            "Avoidance",
            "Damage Reduction",
            "Target Miss",
            "Target Parry",
            "Target Dodge",
            "Armor",

            }; } 
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
        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
        {
            CalculationOptionsTankDK opts = character.CalculationOptions as CalculationOptionsTankDK;

            int targetLevel = opts.TargetLevel;
            int characterLevel = character.Level;
            Stats stats = GetCharacterStats(character, additionalItem);
            Stats raceStats = GetRaceStats(character);

            float levelDifference = (targetLevel - characterLevel) * 0.2f;

            float uaUptime = character.DeathKnightTalents.UnbreakableArmor > 0 ? 20.0f / 120.0f : 0.0f;

            CharacterCalculationsTankDK calcs = new CharacterCalculationsTankDK();
            calcs.BasicStats = stats;
            calcs.TargetLevel = targetLevel;

            float baseAgi = raceStats.Agility;
            float defSkill = (float)Math.Floor(stats.DefenseRating / 4.918498039f);
            //float defSkill = stats.DefenseRating / 4.918498039f;

            float dodgeNonDR = stats.Dodge * 100f - levelDifference + baseAgi * (1.0f / 73.52941176f) + stats.Defense * 0.04f;
            float missNonDR = stats.Miss * 100f - levelDifference + stats.Defense * 0.04f;
            float parryNonDR = stats.Parry * 100f - levelDifference + stats.Defense * 0.04f;

            float dodgePreDR = (stats.Agility - baseAgi) * (1.0f/73.52941176f) + (stats.DodgeRating / 39.34798813f) + (defSkill * 0.04f); 
            float missPreDR = (defSkill * 0.04f);
            float parryRatingFromStr = stats.Strength * 0.25f * (1.0f + uaUptime * 0.25f);
            float parryPreDR = (defSkill * 0.04f) + (stats.ParryRating + parryRatingFromStr) / 49.18498611f;

            float dodgePostDR = 1f / (1f / 88.129021f + 0.9560f / dodgePreDR);
            float missPostDR = 1f / (1f / 16.0f + 0.9560f / missPreDR); //TODO: Search for correct value
            float parryPostDR = 1f / (1f / 47.003525f + 0.9560f / parryPreDR);

            float dodgeTotal = dodgeNonDR + dodgePostDR;
            float missTotal = missNonDR + missPostDR;
            float parryTotal = parryNonDR + parryPostDR;


            calculateDRValues(calcs, dodgePreDR, missPreDR, parryPreDR, dodgePostDR, missPostDR, parryPostDR);

            float currentAvoidance = 100.0f;

            calcs.Miss = missTotal; currentAvoidance -= missTotal;
            calcs.Dodge = Math.Min(currentAvoidance, dodgeTotal); currentAvoidance -= Math.Min(currentAvoidance, dodgeTotal);
            calcs.Parry = Math.Min(currentAvoidance, parryTotal); currentAvoidance -= Math.Min(currentAvoidance, parryTotal);

            float critReduction = (stats.Defense + defSkill) * 0.04f + stats.Resilience / 81.97497559f;

            float attackerCrit = Math.Max(0.0f, 5.0f + levelDifference - critReduction);
            calcs.Crit = 5.0f + levelDifference - critReduction;
            calcs.Defense = defSkill + stats.Defense;
            calcs.DefenseRating = stats.DefenseRating;
            calcs.DefenseRatingNeeded = (calcs.Crit / 0.04f) * 4.918498039f;


            float talent_dr = (1.0f - character.DeathKnightTalents.BladeBarrier * 0.01f)/* * (1.0f - character.DeathKnightTalents.FrostAura * 0.01f)*/ *
                                (1.0f - character.DeathKnightTalents.UnbreakableArmor * 0.05f * uaUptime);

//***** Survival Rating *****
            float armor = stats.Armor;
            // Armor Damage Reduction is capped at 75%
            float armor_dr = Math.Min(0.75f, armor / (armor + 400.0f + 85.0f * (targetLevel + 4.5f * (targetLevel - 59.0f))));

            calcs.ArmorDamageReduction = armor_dr;

            float hp = calcs.BasicStats.Health;

            calcs.Survival = hp / (talent_dr * (1.0f - armor_dr) * (1.0f + attackerCrit / 100.0f));
            calcs.SurvivalWeight = opts.SurvivalWeight;


//***** Mitigation Rating *****

            float ibfUptime = (12.0f + character.DeathKnightTalents.GuileOfGorefiend * 2.0f) / 60.0f;
            float ibfReduction = 0.2f + (stats.Defense + defSkill) * 0.0014f;
            if (character.DeathKnightTalents.GlyphofIceboundFortitude == true)
            {
                ibfReduction = 0.3f + ((stats.Defense + defSkill) * 0.0014f);
            }
            float ibfDR = (ibfReduction * ibfUptime);

            float bsDR = 0.0f;
            if (character.DeathKnightTalents.BoneShield > 0)
            {
                float bsUptime = ((8.0f * (100.0f - currentAvoidance) + 60.0f * currentAvoidance)) / (120.0f * 100.0f);
                if (character.DeathKnightTalents.GlyphofBoneShield == true)
                {
                    bsUptime = bsUptime + (bsUptime * 0.5f);
                }
                bsDR = 0.2f * bsUptime;
            }

            float complete_dr = (1.0f - armor_dr) * (1.0f - ibfDR) * (1.0f - bsDR) * talent_dr;


            float critImpact = 1.0f;
            float critHitAvoidance = currentAvoidance + attackerCrit * critImpact;

            calcs.Mitigation = 10000.0f / (complete_dr * (critHitAvoidance / 100.0f));
            


            // ***** THREAT *****

            if (character.MainHand != null)
            {
                float hitBonus = (float)(stats.HitRating / (32.78998947f * 100.0f) + stats.PhysicalHit);
                float chanceMiss = Math.Max(0f, 0.08f - hitBonus);
                if ((opts.TargetLevel - 80f) < 3)
                {
                    chanceMiss = Math.Max(0f, 0.05f + 0.005f * (opts.TargetLevel - 80f) - hitBonus);
                }

                calcs.Expertise = stats.Expertise + (float)Math.Floor((stats.ExpertiseRating / 32.78998947f) / 0.25f);

                if (character.Race == Character.CharacterRace.Dwarf && 
                    (character.MainHand.Type == Item.ItemType.TwoHandMace || character.MainHand.Type == Item.ItemType.OneHandMace))
                {
                    calcs.Expertise += 5;
                }
                if (character.Race == Character.CharacterRace.Human &&
                    (character.MainHand.Type == Item.ItemType.TwoHandMace || character.MainHand.Type == Item.ItemType.OneHandMace ||
                    character.MainHand.Type == Item.ItemType.TwoHandSword || character.MainHand.Type == Item.ItemType.OneHandSword))
                {
                    calcs.Expertise += 3;
                }
                if (character.Race == Character.CharacterRace.Orc &&
                    (character.MainHand.Type == Item.ItemType.TwoHandAxe || character.MainHand.Type == Item.ItemType.OneHandAxe))
                {
                    calcs.Expertise += 5;
                }


                float chanceParry = Math.Max(0.0f, 0.15f - (calcs.Expertise * 0.0025f));
                float chanceDodge = Math.Max(0.0f, 0.065f - (calcs.Expertise * 0.0025f));
                float hitChance = 1.0f - (chanceMiss + chanceDodge + chanceParry);

                calcs.TargetDodge = chanceDodge;
                calcs.TargetMiss = chanceMiss;
                calcs.TargetParry = chanceParry;

                float physCrits = .0065f;
                physCrits += stats.CritRating / 4591f;
                physCrits += stats.Agility / 6250f;
                physCrits += .01f * (float)(character.DeathKnightTalents.DarkConviction
                    + character.DeathKnightTalents.EbonPlaguebringer);

                float weaponDmg = (character.MainHand.Item.DPS + stats.AttackPower / 14.0f) * character.MainHand.Speed;

                // Haste trinket (Meteorite Whetstone)
                calcs.BasicStats.HasteRating += calcs.BasicStats.HasteRatingOnPhysicalAttack * 10 / 45;
                float totalStaticHaste = 1.0f + (calcs.BasicStats.HasteRating / 32.78998947f / 100.0f);
                totalStaticHaste *= 1.0f + (character.DeathKnightTalents.IcyTalons * 0.04f + character.DeathKnightTalents.ImprovedIcyTalons * 0.05f);

                calcs.Threat = weaponDmg / (character.MainHand.Speed / totalStaticHaste);

                calcs.Threat *= hitChance * (physCrits + 1.0f);

                calcs.Threat *= 1.0f + stats.ThreatIncreaseMultiplier;

                calcs.ThreatWeight = 1000.0f * opts.ThreatWeight;

                
            }

            

            return calcs;
        }

        /// <summary>
        /// calculates the effectivenes of avoidance ratings after diminishing returns
        /// </summary>
        /// <param name="calcs"></param>
        /// <param name="dodgePreDR"></param>
        /// <param name="missPreDR"></param>
        /// <param name="parryPreDR"></param>
        /// <param name="dodgePostDR"></param>
        /// <param name="missPostDR"></param>
        /// <param name="parryPostDR"></param>
        private static void calculateDRValues(CharacterCalculationsTankDK calcs, float dodgePreDR, float missPreDR, float parryPreDR, float dodgePostDR, float missPostDR, float parryPostDR)
        {
            float dodgePreEffect = 1.0f / 39.34798813f;
            float parryPreEffect = 1.0f / 49.18498611f;

            float defPreEffect = 0.04f / 4.918498039f;

            float dodgePostEffect = 1f / (1f / 88.129021f + 0.9560f / (dodgePreDR + dodgePreEffect)) - dodgePostDR;
            float parryPostEffect = 1f / (1f / 47.003525f + 0.9560f / (parryPreDR + parryPreEffect)) - parryPostDR;

            calcs.DRDodge = dodgePostEffect / dodgePreEffect * 100.0f;
            calcs.DRParry = parryPostEffect / parryPreEffect * 100.0f;

            float defPostEffect = 1f / (1f / 88.129021f + 0.9560f / (dodgePreDR + defPreEffect)) - dodgePostDR;
            defPostEffect += 1f / (1f / 47.003525f + 0.9560f / (parryPreDR + defPreEffect)) - parryPostDR;
            defPostEffect += 1f / (1f / 16.0f + 0.9560f / (missPreDR + defPreEffect)) - missPostDR;

            calcs.DRDefense = defPostEffect / (3.0f * defPreEffect) * 100.0f;
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
            CalculationOptionsTankDK calcOpts = character.CalculationOptions as CalculationOptionsTankDK;
            DeathKnightTalents talents = character.DeathKnightTalents;

            Stats statsRace = GetRaceStats(character);
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            //Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
            Stats statsTalents = new Stats()
            {
                BonusStrengthMultiplier = .01f * (float)(talents.AbominationsMight + talents.RavenousDead) + .02f * (float)(/*talents.ShadowOfDeath + */talents.VeteranOfTheThirdWar),
                BaseArmorMultiplier = .03f * (float)(talents.Toughness),
                BonusStaminaMultiplier = .02f * (float)(talents.VeteranOfTheThirdWar),
                Expertise = (float)(talents.TundraStalker + talents.RageOfRivendare) + talents.VeteranOfTheThirdWar*0.02f,
                BonusPhysicalDamageMultiplier = .02f * (float)(talents.BloodGorged + talents.RageOfRivendare) + 0.03f * talents.TundraStalker,
                BonusSpellPowerMultiplier = .02f * (float)(talents.BloodGorged + talents.RageOfRivendare) + 0.03f * talents.TundraStalker,
                Dodge = 0.01f * talents.Anticipation,
                Miss = 0.01f * talents.FrigidDreadplate,

            };
            Stats statsTotal = new Stats();
            Stats statsGearEnchantsBuffs = new Stats();

            /*
            //calculate drums uptime...if it lands on an even minute mark ignore it, as it will have a duration of 0
            float drumsEffectiveFightDuration = (float)calcOpts.FightLength - 1f;
            float numDrums = drumsEffectiveFightDuration % 2;
            float drumsUptime = (numDrums * .5f) / (float)calcOpts.FightLength;

            // Drums of War - 60 AP/30 SD
            if (calcOpts.DrumsOfWar)
            {
                statsBuffs.AttackPower += drumsUptime * 60f;
            }

            // Drums of Battle - 80 Haste
            if (calcOpts.DrumsOfBattle)
            {
                statsBuffs.HasteRating += drumsUptime * 80f;
            }
            */

            // Ferocious Inspiriation  **Temp fix - FI increases all damage, not just physical damage
            /*
            if (character.ActiveBuffsContains("Ferocious Inspiration"))
            {
                statsBuffs.BonusPhysicalDamageMultiplier = ((1f + statsBuffs.BonusPhysicalDamageMultiplier) *
                    (float)Math.Pow(1.03f, calcOpts.FerociousInspiration - 1f)) - 1f;
            }
            */

            statsGearEnchantsBuffs = statsBaseGear + statsBuffs + statsRace + statsTalents;

            statsTotal.BonusAttackPowerMultiplier = statsGearEnchantsBuffs.BonusAttackPowerMultiplier;
            statsTotal.BonusAgilityMultiplier = statsGearEnchantsBuffs.BonusAgilityMultiplier;
            statsTotal.BonusStrengthMultiplier = statsGearEnchantsBuffs.BonusStrengthMultiplier;
            statsTotal.BonusStaminaMultiplier = statsGearEnchantsBuffs.BonusStaminaMultiplier;
            statsTotal.BaseArmorMultiplier = statsGearEnchantsBuffs.BaseArmorMultiplier;

            statsTotal.Agility = (float)Math.Floor(statsGearEnchantsBuffs.Agility * (1 + statsGearEnchantsBuffs.BonusAgilityMultiplier));
            statsTotal.Strength = (float)Math.Floor(statsGearEnchantsBuffs.Strength * (1 + statsGearEnchantsBuffs.BonusStrengthMultiplier));
            statsTotal.Stamina = (float)Math.Floor(statsGearEnchantsBuffs.Stamina * (1 + statsGearEnchantsBuffs.BonusStaminaMultiplier));
            statsTotal.Intellect = (float)Math.Floor(statsGearEnchantsBuffs.Intellect * (1 + statsGearEnchantsBuffs.BonusIntellectMultiplier));
            statsTotal.Spirit = (float)Math.Floor(statsGearEnchantsBuffs.Spirit * (1 + statsGearEnchantsBuffs.BonusSpiritMultiplier));

            statsTotal.Armor = (float)Math.Floor(statsGearEnchantsBuffs.Armor * (1.0f + statsGearEnchantsBuffs.BaseArmorMultiplier) * 1.80f + 2f * statsTotal.Agility);
            statsTotal.Armor *= 1.0f + statsGearEnchantsBuffs.BonusArmorMultiplier;

            statsTotal.BonusArmor = statsGearEnchantsBuffs.BonusArmor;

            statsTotal.Health = (float)Math.Floor((statsGearEnchantsBuffs.Health + (statsTotal.Stamina * 10f)) * 1.10f);
            statsTotal.Health *= (1f + statsGearEnchantsBuffs.BonusHealthMultiplier);

            statsTotal.Mana = (float)Math.Floor(statsGearEnchantsBuffs.Mana + (statsTotal.Intellect * 15f));
            statsTotal.AttackPower = (float)Math.Floor(statsGearEnchantsBuffs.AttackPower + statsTotal.Strength * 2);

            statsTotal.Armor += statsTotal.BonusArmor;

            if (talents.BladedArmor > 0)
            {
                statsTotal.AttackPower += (statsGearEnchantsBuffs.Armor / 180f) * (float)talents.BladedArmor;
            }

            statsTotal.AttackPower *= 1f + statsTotal.BonusAttackPowerMultiplier;

            statsTotal.DefenseRating = statsGearEnchantsBuffs.DefenseRating;
            statsTotal.DodgeRating = statsGearEnchantsBuffs.DodgeRating;
            statsTotal.ParryRating = statsGearEnchantsBuffs.ParryRating;

            statsTotal.Defense = statsGearEnchantsBuffs.Defense;
            statsTotal.Dodge = 0.03463600f + statsGearEnchantsBuffs.Dodge;
            statsTotal.Parry = 0.050f + statsGearEnchantsBuffs.Parry;
            statsTotal.Miss = 0.050f + statsGearEnchantsBuffs.Miss;

            statsTotal.Resilience = statsGearEnchantsBuffs.Resilience;
            

            statsTotal.CritRating = statsGearEnchantsBuffs.CritRating;
            statsTotal.CritRating += statsGearEnchantsBuffs.CritMeleeRating + statsGearEnchantsBuffs.LotPCritRating;
            statsTotal.SpellHit = statsGearEnchantsBuffs.SpellHit;
            statsTotal.PhysicalHit = statsGearEnchantsBuffs.PhysicalHit;
            statsTotal.HitRating = statsGearEnchantsBuffs.HitRating;
            statsTotal.ArmorPenetration = statsGearEnchantsBuffs.ArmorPenetration;
            statsTotal.Expertise = statsGearEnchantsBuffs.Expertise;
            statsTotal.ExpertiseRating = statsGearEnchantsBuffs.ExpertiseRating;
            //statsTotal.Expertise += (float)Math.Floor(statsGearEnchantsBuffs.ExpertiseRating / 8);
            statsTotal.HasteRating = statsGearEnchantsBuffs.HasteRating;
            statsTotal.WeaponDamage = statsGearEnchantsBuffs.WeaponDamage;

            //statsTotal.SpellCrit = statsGearEnchantsBuffs.SpellCrit;
            //statsTotal.CritRating = statsGearEnchantsBuffs.CritRating;
            //statsTotal.HitRating = statsGearEnchantsBuffs.HitRating;

            statsTotal.BonusCritMultiplier = statsGearEnchantsBuffs.BonusCritMultiplier;

            statsTotal.BonusPhysicalDamageMultiplier = statsGearEnchantsBuffs.BonusPhysicalDamageMultiplier;

            return (statsTotal);
        }

        /// <summary>
        /// Gets data to fill a custom chart, based on the chart name, as defined in CustomChartNames.
        /// </summary>
        /// <param name="character">The character to build the chart for.</param>
        /// <param name="chartName">The name of the custom chart to get data for.</param>
        /// <returns>The data for the custom chart.</returns>
        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            return new ComparisonCalculationBase[0];
        }


        private Stats GetRaceStats(Character character)
        {
            Stats statsRace;
            switch (character.Race)
            {
                case Character.CharacterRace.Human:
                    statsRace = new Stats() { Strength = 108f, Agility = 73f, Stamina = 99f, Intellect = 29f, Spirit = 46f, Armor = 0f, Health = 2169f };
                    break;
                case Character.CharacterRace.Dwarf:
                    statsRace = new Stats() { Strength = 110f, Agility = 69f, Stamina = 102f, Intellect = 28f, Spirit = 41f, Armor = 0f, Health = 2199f };
                    break;
                case Character.CharacterRace.NightElf:
                    statsRace = new Stats() { Strength = 105f, Agility = 78f, Stamina = 98f, Intellect = 29f, Spirit = 42f, Armor = 0f, Health = 2159f,
                                                Miss = 0.02f};
                    break;
                case Character.CharacterRace.Gnome:
                    statsRace = new Stats() { Strength = 103f, Agility = 76f, Stamina = 98f, Intellect = 33f, Spirit = 42f, Armor = 0f, Health = 2159f };
                    break;
                case Character.CharacterRace.Draenei:
                    statsRace = new Stats() { Strength = 109f, Agility = 70f, Stamina = 98f, Intellect = 30f, Spirit = 44f, Armor = 0f, Health = 2159f,
                                                PhysicalHit = 0.01f, SpellHit = 0.01f};
                    break;
                case Character.CharacterRace.Orc:
                    statsRace = new Stats() { Strength = 111f, Agility = 70f, Stamina = 101f, Intellect = 26f, Spirit = 45f, Armor = 0f, Health = 2189f };
                    break;
                case Character.CharacterRace.Troll:
                    statsRace = new Stats() { Strength = 109f, Agility = 75f, Stamina = 100f, Intellect = 25f, Spirit = 43f, Armor = 0f, Health = 2179f };
                    break;
                case Character.CharacterRace.Undead:
                    statsRace = new Stats() { Strength = 107f, Agility = 71f, Stamina = 100f, Intellect = 27f, Spirit = 47f, Armor = 0f, Health = 2179f };
                    break;
                case Character.CharacterRace.BloodElf:
                    statsRace = new Stats() { Strength = 105f, Agility = 75f, Stamina = 97f, Intellect = 33f, Spirit = 41f, Armor = 0f, Health = 2149f };
                    break;
                case Character.CharacterRace.Tauren:
                    statsRace = new Stats() { Strength = 113f, Agility = 68f, Stamina = 101f, Intellect = 24f, Spirit = 34f, Armor = 0f, Health = 2298f };
                    break;

                default:
                    statsRace = new Stats();
                    break;
            }
            // Derived stats base amount, common to all races
            statsRace.Strength += 67f;
            statsRace.Agility += 39f;
            statsRace.Stamina += 61f;
            statsRace.Intellect += 6f;
            statsRace.Spirit += 17f;

            statsRace.Health += 5792;
            statsRace.AttackPower = 202f + (67f * 2);

            return statsRace;
        }


        /// <summary>
        /// Filters a Stats object to just the stats relevant to the model.
        /// </summary>
        /// <param name="stats">A complete Stats object containing all stats.</param>
        /// <returns>A filtered Stats object containing only the stats relevant to the model.</returns>
        public override Stats GetRelevantStats(Stats stats)
        {
            return new Stats()
            {
                Health = stats.Health,
                Strength = stats.Strength,
                Agility = stats.Agility,
                Stamina = stats.Stamina,
                Intellect = stats.Intellect,
                Spirit = stats.Spirit,
                Armor = stats.Armor,
                BonusArmor = stats.BonusArmor,

                DefenseRating = stats.DefenseRating,
                ParryRating = stats.ParryRating,
                DodgeRating = stats.DodgeRating,

                Defense = stats.Defense,
                Dodge = stats.Dodge,
                Parry = stats.Parry,

                Resilience = stats.Resilience,

                //AttackPower = stats.AttackPower,
                HitRating = stats.HitRating,
                //CritRating = stats.CritRating,
                //ArmorPenetration = stats.ArmorPenetration,
                ExpertiseRating = stats.ExpertiseRating,
                //HasteRating = stats.HasteRating,
                //WeaponDamage = stats.WeaponDamage,
                //PhysicalCrit = stats.PhysicalCrit,
                //PhysicalHaste = stats.PhysicalHaste,
                PhysicalHit = stats.PhysicalHit,

                BonusHealthMultiplier = stats.BonusHealthMultiplier,
                BonusStrengthMultiplier = stats.BonusStrengthMultiplier,
                BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
                BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,
                BonusCritMultiplier = stats.BonusCritMultiplier,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
                BonusSpellPowerMultiplier = stats.BonusSpellPowerMultiplier,

                LotPCritRating = stats.LotPCritRating,
                CritMeleeRating = stats.CritMeleeRating,
                WindfuryAPBonus = stats.WindfuryAPBonus,
                Bloodlust = stats.Bloodlust
            };
        }


        /// <summary>
        /// Tests whether there are positive relevant stats in the Stats object.
        /// </summary>
        /// <param name="stats">The complete Stats object containing all stats.</param>
        /// <returns>True if any of the positive stats in the Stats are relevant.</returns>
        public override bool HasRelevantStats(Stats stats)
        {
            return (stats.Health + stats.Strength + stats.Agility + stats.Stamina + stats.Spirit + stats.AttackPower +
                stats.HitRating + stats.CritRating + stats.ArmorPenetration + stats.ExpertiseRating + stats.HasteRating + stats.WeaponDamage +
                stats.CritRating + stats.HitRating + stats.BonusArmor +
                stats.DodgeRating + stats.DefenseRating + stats.ParryRating + stats.Resilience +
                stats.Dodge + stats.Parry + stats.Defense + stats.BonusArmorMultiplier +
                stats.BonusHealthMultiplier + stats.BonusStrengthMultiplier + stats.BonusStaminaMultiplier + stats.BonusAgilityMultiplier + stats.BonusCritMultiplier +
                stats.BonusAttackPowerMultiplier + stats.BonusPhysicalDamageMultiplier + stats.BonusSpellPowerMultiplier +
                stats.CritMeleeRating + stats.LotPCritRating + stats.WindfuryAPBonus + stats.Bloodlust) != 0;
        }


        /// <summary>
        /// Deserializes the model's CalculationOptions data object from xml
        /// </summary>
        /// <param name="xml">The serialized xml representing the model's CalculationOptions data object.</param>
        /// <returns>The model's CalculationOptions data object.</returns>
        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsTankDK));
            System.IO.StringReader reader = new System.IO.StringReader(xml);
            CalculationOptionsTankDK calcOpts = serializer.Deserialize(reader) as CalculationOptionsTankDK;
            return calcOpts;
        }

    }
}
