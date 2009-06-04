using System;
using System.Collections.Generic;
using System.Text;


namespace Rawr.TankDK
{
    [Rawr.Calculations.RawrModelInfo("TankDK", "spell_shadow_deathanddecay", Character.CharacterClass.DeathKnight)]
    class CalculationsTankDK : CalculationsBase
    {
        /// <summary>
        /// Setup constants that need to be answered.
        /// </summary>
        #region Constants
        private static readonly float critImpact = 1f;  // How severe is getting crit? 1 = 100%
        private static readonly float BaseThreatValue = 1000f; // Base value of threat modified by Threat weight.
        private static readonly float BasePhysicalCrit = 0f; 

        #endregion // Constants


        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
				////Relevant Gem IDs for TankDKs
				//Red
				int[] subtle = { 39907, 40000, 40115, 42151 }; //
                int[] bold = { }; // +Str
                int[] bright = { }; // +AP
                int[] delicate = { }; // +Agi
                int[] flashing = { }; // +Parry

				//Purple
				int[] regal = { 39938, 40031, 40138 };
                int[] balanced = { }; // +AP +Stam
                int[] defenders = { }; // +Parry +Stam

				//Blue
				int[] solid = { 39919, 40008, 40119, 36767 }; // +Stam

				//Green
				int[] enduring = { 39976, 40089, 40167 }; // +Def +Stam

				//Yellow
				int[] thick = { 39916, 40015, 40126, 42157 }; // +def
                int[] gleaming = { }; // +Crit.

				//Orange
				int[] stalwart = { 39964, 40056, 40160 };
                int[] accurate = { }; // +Hit +Expertise
                int[] deadly = { }; // +agi +crit
                int[] deft = { }; // +Agi +Haste
                int[] etched = { }; // +hit +Str
                int[] glimmering = { }; // +parry +def
                int[] glinting = { }; // +Agi +Hit



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

                        "Basic Stats:Strength*Unbreakable Armor not factored in.",
					    "Basic Stats:Agility",
                        "Basic Stats:Stamina",
					    "Basic Stats:Attack Power",
					    "Basic Stats:Crit Rating",
					    "Basic Stats:Hit Rating",
					    "Basic Stats:Expertise",
					    "Basic Stats:Haste Rating",
					    "Basic Stats:Armor Penetration",
					    "Basic Stats:Armor Penetration Rating",
                        "Basic Stats:Health",
                        "Basic Stats:Armor*Including Frost Presence",

                        "Defense:Crit*Enemy's crit chance on you",
                        "Defense:Defense Rating",
                        "Defense:Defense",
                        "Defense:Resilience",
                        "Defense:Defense Rating needed",

                        "Advanced Stats:Miss",
                        "Advanced Stats:Dodge",
                        "Advanced Stats:Parry*With Blade Barrier and Str bonus from Unbreakable Armor's average uptime.",
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
        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationTankDK(); }

        /// <summary>
        /// Method to get a new instance of the model's custom CharacterCalculations class.
        /// </summary>
        /// <returns>A new instance of the model's custom CharacterCalculations class, 
        /// which inherits from CharacterCalculationsBase</returns>
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsTankDK(); }

        /// <summary>
        /// An array of strings which define what calculations (in addition to the subpoint ratings)
        /// will be available to the optimizer
        /// </summary>
        public override string[] OptimizableCalculationLabels { get { return new string[] {
            "Chance to be Crit",
            "Avoidance %",
            "Damage Reduction %",
            "Target Miss %",
            "Target Parry %",
            "Target Dodge %",
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
            // Import the option values from the options tab on the UI.
            CalculationOptionsTankDK opts = character.CalculationOptions as CalculationOptionsTankDK;
            // Validate opts 
            if (null == opts) return null;
            CharacterCalculationsTankDK calcs = new CharacterCalculationsTankDK();
            //ShotRotation sr = new ShotRotation();

            // Level differences.
            int targetLevel = opts.TargetLevel;
            int characterLevel = character.Level;
            float levelDifference = (targetLevel - characterLevel) * 0.2f;
            // The full character data.
            Stats stats = GetCharacterStats(character, additionalItem);

            calcs.BasicStats = stats;
            calcs.TargetLevel = targetLevel;

            // Add in factors for Unbreakable Armor when passing in the values for calculating Parry values.
            // Need to ensure that for DKs we include Parry from str effected by DR.
            // Talent: Unbreakable Armor specific numbers:
            // UA runs for 20 secs w/ a 2 min cooldown.
            float uaUptime = character.DeathKnightTalents.UnbreakableArmor > 0 ? 20.0f / 120.0f : 0.0f;
            // Increases str by 25% when active.
            stats.Strength += (stats.Strength * 0.25f * uaUptime);

            // Get all the character avoidance numbers including deminishing returns.
            // Iterate through each hit type. and use fAvoidance array w/ the hitresult enum.
            float[] fAvoidance = new float[(uint)HitResult.NUM_HitResult];
            for (uint i = 0; i < (uint)HitResult.NUM_HitResult; i++)
            {
                // GetDRAvoidanceChance returns a dec. percentage.
                // Since CurrentAvoidance is a percent, need to multiply by 100.
                fAvoidance[i] = (StatConversion.GetDRAvoidanceChance(character, stats, (HitResult)i, targetLevel) * 100f);
            }

            float fChanceToGetHit = 100.0f;

            // So let's populate the miss, dodge and parry values for the UI display as well as pulling them out of the avoidance number.
            calcs.Miss = fAvoidance[(int)HitResult.Miss];
            fChanceToGetHit -= calcs.Miss;
            // Dodge needs to be factored in here.
            calcs.Dodge = Math.Min(fChanceToGetHit, fAvoidance[(int)HitResult.Dodge]);
            fChanceToGetHit -= calcs.Dodge;
            // Pary factors
            calcs.Parry = Math.Min(fChanceToGetHit, fAvoidance[(int)HitResult.Parry]);
            fChanceToGetHit -= calcs.Parry;

            // 5% + Level difference crit chance.  
            float attackerCrit = Math.Max(0.0f, ((5.0f) - fAvoidance[(int)HitResult.Crit]));
            calcs.Crit = attackerCrit;
            calcs.DefenseRating = stats.DefenseRating;
            calcs.Defense = (StatConversion.GetDefenseFromRating(stats.DefenseRating, character.Class) + stats.Defense);
            calcs.Resilience = stats.Resilience;
            calcs.DefenseRatingNeeded = StatConversion.GetDefenseRatingNeeded(character, stats, targetLevel);
            // Test of the Defense vs. DefenseRatingNeeded functions
            // Assuming that 540 defense is the max to be uncritable, 
            // if Def is > 540, then DefenseRatingNeeded should be 0 or less.
            

            // The values below represent that values of the talents in mitigating damage.  
            // BB == 1% reduction per point.
            // UA == 5% reduction avaeraged by it's uptime.  
            // ImpFrostPre == 1% reduction per point.
            float talent_dr = (1.0f - character.DeathKnightTalents.BladeBarrier * 0.01f) *
                              //(1.0f - character.DeathKnightTalents.UnbreakableArmor * 0.05f * uaUptime) * // Pulling out UA from Talent_Dr since I would argue it is a pure mitigation tactic.
                              (1.0f - character.DeathKnightTalents.ImprovedFrostPresence * 0.01f);

            // Talent: Mark of Blood //////////////////////////////////////////////////////////
            // Target is healed for 4% of max health per hit for up to 20 hits or 20 secs. 
            // 3 Min cooldown.
            float fMoBHPModifier = 0f;

            // Talent: Bone Shield ////////////////////////////////////////////////////////
            float bsDR = 0.0f;
            float bsUptime = 0f;
            if (character.DeathKnightTalents.BoneShield > 0)
            {
                uint BSStacks = 4;  // The number of bones by default.
                if (character.DeathKnightTalents.GlyphofBoneShield == true)
                {
                    BSStacks += 2;
                }

                bsUptime = Math.Min(1f,                         // Can't be up for longer than 100% of the time. 
                            (BSStacks * 2f) /                   // 2 sec internal cooldown on loosing bones so the DK can't get spammed to death. 
                            ((100f - fChanceToGetHit) / 100)   // Loose a bone every time we get hit.
                            / 120.0f);                          // 120 sec cooldown.
                // 20% damage reduction while active.
                bsDR = 0.2f * bsUptime;
            }

            #region ***** Mitigation Rating *****

            // TODO: Integrate the Proc effect of the Sigil of the Unfaltering Knight.
            // It hasn't been working since it was moved to a SpecialEffect.
            // TODO: Integrate Magic Suppression.
            // TODO: Integrate OnUse def trinkets like for Dodge and such.

            // IceBound Fortitude. ////////////////////////////////////////////////////////
            // Talent: GuileofGorefiend increases IBF by 2 sec per point.
            // IBF has a 60 sec CD.
            float ibfUptime = (12.0f + character.DeathKnightTalents.GuileOfGorefiend * 2.0f) / 60.0f;
            // IBF reduces damage taken by 20% + some amount(?) based on defense.
            float ibfReduction = 0.2f + calcs.Defense * 0.0014f;
            if (character.DeathKnightTalents.GlyphofIceboundFortitude == true)
            {
                // Glyphed to 30% + some amount(?)
                ibfReduction += 0.1f;
            }
            float ibfDR = (ibfReduction * ibfUptime);


            // Assuming Frost Fever is always active.
            // Talent: Improved Icy Touch ////////////////////////////////////////////////////////
            // Reduces target Attack speed by 2% per point.  3pts max.
            float IITDR = 0f;
            IITDR = (float)Math.Min(0.06f, character.DeathKnightTalents.ImprovedIcyTouch * 0.02f);

            // Why are some talents factored in both Survival and Mitigation?  A the very least they should only be in Mitigation.
            float complete_dr = (1.0f - calcs.ArmorDamageReduction)
                                * (1.0f - ibfDR)
                                * (1.0f - bsDR)
                                * (1f - IITDR)
                                * (1.0f - character.DeathKnightTalents.UnbreakableArmor * 0.05f * uaUptime)  // Adding UA to just mitigation complete DR.
                                * talent_dr
                                * (1f - .05f * character.DeathKnightTalents.ImprovedBloodPresence); // addition 5% benefit from Heals landing on the tank per point.

            float critHitAvoidance = fChanceToGetHit + attackerCrit * critImpact;

            calcs.Mitigation = opts.IncomingDamage / (complete_dr * (critHitAvoidance / 100.0f));

            #endregion

            #region ***** THREAT *****

            float fDamageTotal = 0f; 
            if (character.MainHand != null)
            {
                // TODO: Implment DW Tanking - as of 3.1 it's still unrealistic, but hey.  Gotta be complete.
                // TODO: Shot rotation
                // TODO: Single vs. Multi Target tanking.
                float hitBonus = StatConversion.GetHitFromRating(stats.HitRating, character.Class) + stats.PhysicalHit;
                // 8% default miss rate vs lvl 83
                float chanceMiss = Math.Max(0f, 0.08f - hitBonus);
                if ((opts.TargetLevel - 80f) < 3)
                {
                    chanceMiss = Math.Max(0f, 0.05f + 0.005f * (opts.TargetLevel - 80f) - hitBonus);
                }

                calcs.Expertise = stats.Expertise + StatConversion.GetExpertiseFromRating(stats.ExpertiseRating);
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

                // More Unreferenced Constants.
                float chanceParry = Math.Max(0.0f, 0.15f - (calcs.Expertise * 0.0025f));
                float chanceDodge = Math.Max(0.0f, 0.065f - (calcs.Expertise * 0.0025f));
                float hitChance = 1.0f - (chanceMiss + chanceDodge + chanceParry);

                calcs.TargetDodge = chanceDodge;
                calcs.TargetMiss = chanceMiss;
                calcs.TargetParry = chanceParry;

                // Base crit chance of 0% 
                float physCrits = BasePhysicalCrit;
                physCrits += stats.PhysicalCrit;
                calcs.BasicStats.PhysicalCrit = physCrits;

                float totalStaticHaste = 1.0f + (StatConversion.GetHasteFromRating(calcs.BasicStats.HasteRating, character.Class));
                // 4% haste from ITalons
                // 5% haste from IITalons
                totalStaticHaste *= 1.0f + (character.DeathKnightTalents.IcyTalons * 0.04f + character.DeathKnightTalents.ImprovedIcyTalons * 0.05f);

                // Currently only taking into account 2h tanking.
                // Every 14 AP == +1DPS
                float fDamageWhite = (character.MainHand.Item.DPS + stats.AttackPower / 14.0f) * character.MainHand.Speed;

                // White Damage modifiers:
                fDamageWhite *= (1f + 0.04f * character.DeathKnightTalents.Necrosis); // 4% shadow damage added to auto attacks per point.
                fDamageWhite *= (1f + 0.01f * character.DeathKnightTalents.Desecration); // 1% additional damage done while on unholy ground.

                // TODO: Entry point for shot rotation function.
                float fDamageSpecial = 0f;
                 // 2-hander weapon specialization.
                float f2hWeaponDamageMultiplier = 0f;
                if (character.MainHand.Type == Item.ItemType.TwoHandAxe
                    || character.MainHand.Type == Item.ItemType.TwoHandMace
                    || character.MainHand.Type == Item.ItemType.TwoHandSword)
                {
                    f2hWeaponDamageMultiplier = (1f + .02f * character.DeathKnightTalents.TwoHandedWeaponSpecialization);
                }

                // Threat here is damage output by the main weapon as affected by haste.
                fDamageWhite = fDamageWhite / (character.MainHand.Speed / totalStaticHaste);

                fDamageTotal = fDamageWhite + fDamageSpecial;

                // Remove hitchance problems, add Crit chance..
                fDamageTotal *= hitChance;
                // Increase the total damage done by the chance of crits hitting and each crit does 150% damage.
                fDamageTotal += fDamageTotal * physCrits * 1.5f;

                // For right now all damage is only looking at melee damage.  
                // so Annihillation is going in full.  Once we get shot rotation, I'll break it out.
                fDamageTotal = (fDamageWhite + fDamageSpecial)
                                * (1f + .02f * bsUptime) // Bone Shield 2% damage increase when active.
                                * (f2hWeaponDamageMultiplier) // 2h Weapon spec.
                                ;

                calcs.Threat = fDamageTotal;

                // Threat buffs.
                calcs.Threat *= 1.0f + (stats.ThreatIncreaseMultiplier - stats.ThreatReductionMultiplier);

                calcs.ThreatWeight = BaseThreatValue * opts.ThreatWeight;


            }

            #endregion

            #region ***** Survival Rating *****

            // TODO: Integrate Buffs & Debuffs into this function.  Right now there are none.
            // Assuming the Boss has no ArPen
            // From http://www.skeletonjack.com/2009/05/14/dk-tanking-armor-cap/#comments
            // 75% armor cap.  Not sure if this is for DK or for all Tanks.  So I'm just going to handle it here.
            // I'll do more research and see if it needs to go into the general function.
            calcs.ArmorDamageReduction = (float)Math.Min(0.75f, StatConversion.GetArmorDamageReduction(targetLevel, stats.Armor, 0f, 0f, 0f));

            // Adding in the extra 10% health here for frost presence so it's not default in the UI anymore.
            float hp = (calcs.BasicStats.Health * 1.1f);

            // Talent: Improved Blood Presence ///////////////////////////////////////////////////
            if (character.DeathKnightTalents.ImprovedBloodPresence > 0)
            {
                // HACK: Since damage is so low, I'm bringing forward the ThreatWeight modifier as well.
                // Increase equiv. HP by incoming healing that happens due to damage done.
                // 2% of damage done healed per point of IBP 
                hp += ((fDamageTotal * calcs.ThreatWeight) * character.DeathKnightTalents.ImprovedBloodPresence * 0.02f);
            }

            calcs.Survival = hp / (talent_dr * (1.0f - calcs.ArmorDamageReduction) * (1.0f + attackerCrit / 100.0f));
            calcs.SurvivalWeight = opts.SurvivalWeight;

            #endregion



            return calcs;
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
            // Validate that character.CalculationOptions != NULL
            if (null == character.CalculationOptions)
            {
                // Possibly put some error text here.
                return null;
            }
            CalculationOptionsTankDK calcOpts = character.CalculationOptions as CalculationOptionsTankDK;
            DeathKnightTalents talents = character.DeathKnightTalents;

            // Basic racial & class baseline.
            Stats statsRace = GetRaceStats(character);
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
            Stats statsTalents = new Stats()
            {
                BonusStrengthMultiplier = .01f * (float)(talents.AbominationsMight + talents.RavenousDead) + .02f * (float)(talents.VeteranOfTheThirdWar),
                BaseArmorMultiplier = .03f * (float)(talents.Toughness),
                BonusStaminaMultiplier = .02f * (float)(talents.VeteranOfTheThirdWar),
                Expertise = (float)(talents.TundraStalker + talents.RageOfRivendare + talents.VeteranOfTheThirdWar),
                BonusPhysicalDamageMultiplier = .02f * (float)(talents.BloodGorged + talents.RageOfRivendare) + 0.03f * talents.TundraStalker,
                BonusSpellPowerMultiplier = .02f * (float)(talents.BloodGorged + talents.RageOfRivendare) + 0.03f * talents.TundraStalker,
                // Currently SpellHit doesn't have an effect.
                SpellHit = 0.01f * (float)(talents.Virulence),
                Dodge = (0.01f * talents.Anticipation),
                Miss = (0.01f * talents.FrigidDreadplate),
                Defense = 400, // Adding in the base 400 Defense skill all tanks are expected to have.  There are too many places where this just kinda stuck in.  It should be attached to the toon.
            };
            Stats statsFrost = GetFrostPresence();
            // The crit work from talents was getting to complicated to include in the construction.  
            // Talent: VisciousStrikes improve Crit by 3% so converting that to rating. 
            // TODO: Talent BloodyVengence +1% crit for 30 secs after a crit per point & stacks 3x.  
            statsTalents.PhysicalCrit += (0.03f * (float)(talents.ViciousStrikes));
            // Dark Conviction (5pts) & EbonPlagueBringer (3pts) each increase Crit chance by 1% per point.
            // All threat right now is looking at melee strikes.  So adding in Annihilation stright away.
            statsTalents.PhysicalCrit += (.01f * (float)(talents.DarkConviction + talents.EbonPlaguebringer + talents.Annihilation));

            Stats statsTotal = new Stats();
            Stats statsGearEnchantsBuffs = new Stats();

            // We gather up everything here:
            statsGearEnchantsBuffs = statsBaseGear + statsBuffs + statsRace + statsTalents + statsFrost;

            // Stack only the info we care about.
            statsTotal = GetRelevantStats(statsGearEnchantsBuffs);

            // Apply Stat modifiers
            statsTotal.Strength = (float)Math.Floor(ApplyMultiplier(statsGearEnchantsBuffs.Strength, statsGearEnchantsBuffs.BonusStrengthMultiplier));
            statsTotal.Agility = (float)Math.Floor(ApplyMultiplier(statsGearEnchantsBuffs.Agility, statsGearEnchantsBuffs.BonusAgilityMultiplier));
            statsTotal.Stamina = (float)Math.Floor(ApplyMultiplier(statsGearEnchantsBuffs.Stamina, statsGearEnchantsBuffs.BonusStaminaMultiplier));
            statsTotal.Armor = (float)Math.Floor(ApplyMultiplier((statsGearEnchantsBuffs.Armor + StatConversion.GetArmorFromAgility(statsTotal.Agility)), statsGearEnchantsBuffs.BaseArmorMultiplier) + 
                                ApplyMultiplier(statsGearEnchantsBuffs.BonusArmor, statsGearEnchantsBuffs.BonusArmorMultiplier));

            statsTotal.Health = (float)Math.Floor(ApplyMultiplier((statsTotal.Health + StatConversion.GetHealthFromStamina(statsTotal.Stamina)), statsGearEnchantsBuffs.BonusHealthMultiplier));

            if (talents.BladedArmor > 0)
            {
                statsTotal.AttackPower += (statsTotal.Armor / 180f) * (float)talents.BladedArmor;
            }
            statsTotal.AttackPower = (float)Math.Floor(ApplyMultiplier((statsTotal.AttackPower + (statsTotal.Strength * 2)), statsGearEnchantsBuffs.BonusAttackPowerMultiplier));
            statsTotal.CritRating = (float)Math.Floor(ApplyMultiplier((statsGearEnchantsBuffs.CritRating + statsGearEnchantsBuffs.CritMeleeRating), statsGearEnchantsBuffs.BonusCritMultiplier));
            
            statsTotal.CritRating += statsGearEnchantsBuffs.LotPCritRating + StatConversion.GetCritFromAgility(statsTotal.Agility, character.Class);

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
            Stats Base = new Stats();
            Base = BaseStats.GetBaseStats(character);
            return Base;
        }

        /// <summary>
        /// Filters a Stats object to just the stats relevant to the model.
        /// </summary>
        /// <param name="stats">A complete Stats object containing all stats.</param>
        /// <returns>A filtered Stats object containing only the stats relevant to the model.</returns>
        public override Stats GetRelevantStats(Stats stats)
        {
            Stats s = new Stats()
            {
                Strength = stats.Strength,
                Agility = stats.Agility,
                Stamina = stats.Stamina,
                Armor = stats.Armor,
                BonusArmor = stats.BonusArmor,
                Health = stats.Health,

                DefenseRating = stats.DefenseRating,
                ParryRating = stats.ParryRating,
                DodgeRating = stats.DodgeRating,

                Defense = stats.Defense,
                Dodge = stats.Dodge,
                Parry = stats.Parry,
                Miss = stats.Miss,

                Resilience = stats.Resilience,

                AttackPower = stats.AttackPower,
                HitRating = stats.HitRating,
                CritRating = stats.CritRating,
                ArmorPenetration = stats.ArmorPenetration,
                ExpertiseRating = stats.ExpertiseRating,
                Expertise = stats.Expertise,
                HasteRating = stats.HasteRating,
                WeaponDamage = stats.WeaponDamage,
                PhysicalCrit = stats.PhysicalCrit,
                PhysicalHaste = stats.PhysicalHaste,
                PhysicalHit = stats.PhysicalHit,

                BonusHealthMultiplier = stats.BonusHealthMultiplier,
                BonusStrengthMultiplier = stats.BonusStrengthMultiplier,
                BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
                BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,
                BonusCritMultiplier = stats.BonusCritMultiplier,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
                BonusSpellPowerMultiplier = stats.BonusSpellPowerMultiplier,
                BaseArmorMultiplier = stats.BaseArmorMultiplier,
                BonusArmorMultiplier = stats.BonusArmorMultiplier,
 
           // Defect 13301: Integrate 2% Threat increase for Armsmen enchant.
                ThreatIncreaseMultiplier = stats.ThreatIncreaseMultiplier,
                ThreatReductionMultiplier = stats.ThreatReductionMultiplier,

                LotPCritRating = stats.LotPCritRating,
                CritMeleeRating = stats.CritMeleeRating,
                Bloodlust = stats.Bloodlust,

                // Bringing in some of the relavent stats from DPSDK.
                // General Damage Mods.
                BonusShadowDamageMultiplier = stats.BonusShadowDamageMultiplier,
                BonusFrostDamageMultiplier = stats.BonusFrostDamageMultiplier,
                BonusDiseaseDamageMultiplier = stats.BonusDiseaseDamageMultiplier,

                // Ability mods.
                BonusBloodStrikeDamage = stats.BonusBloodStrikeDamage,
                BonusDeathCoilDamage = stats.BonusDeathCoilDamage,
                BonusDeathStrikeDamage = stats.BonusDeathStrikeDamage,
                BonusFrostStrikeDamage = stats.BonusFrostStrikeDamage,
                BonusHeartStrikeDamage = stats.BonusHeartStrikeDamage,
                BonusIcyTouchDamage = stats.BonusIcyTouchDamage,
                BonusObliterateDamage = stats.BonusObliterateDamage,
                BonusScourgeStrikeDamage = stats.BonusScourgeStrikeDamage,

            };

            // Also bringing in the trigger events from DPSDK - 
            // Since I'm going to move the +Def bonus for the Sigil of the Unfaltering Knight
            // To a special effect.  Also there are alot of OnUse and OnEquip special effects
            // That probably aren't being taken into effect.
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (HasRelevantStats(effect.Stats))
                {
                    s.AddSpecialEffect(effect);
                }
            }
            return s;
        }


        /// <summary>
        /// Tests whether there are positive relevant stats in the Stats object.
        /// </summary>
        /// <param name="stats">The complete Stats object containing all stats.</param>
        /// <returns>True if any of the non-Zero stats in the Stats are relevant.  
        /// I realize that there aren't many stats that have negative values, but for completeness.</returns>
        public override bool HasRelevantStats(Stats stats)
        {
            bool bResults = false;
            bResults |= (stats.Health != 0);
            bResults |= (stats.Strength != 0);
            bResults |= (stats.Agility != 0);
            bResults |= (stats.Stamina != 0);
            bResults |= (stats.AttackPower != 0);
            bResults |= (stats.HitRating != 0);
            bResults |= (stats.CritRating != 0);
            bResults |= (stats.PhysicalCrit != 0);
            bResults |= (stats.ArmorPenetration != 0);
            bResults |= (stats.ExpertiseRating != 0);
            bResults |= (stats.Expertise != 0);
            bResults |= (stats.HasteRating != 0);
            bResults |= (stats.WeaponDamage != 0);
            bResults |= (stats.BonusArmor != 0);
            bResults |= (stats.DodgeRating != 0);
            bResults |= (stats.DefenseRating != 0);
            bResults |= (stats.ParryRating != 0);
            bResults |= (stats.Resilience != 0);
            bResults |= (stats.Dodge != 0);
            bResults |= (stats.Parry != 0);
            bResults |= (stats.Miss != 0);
            bResults |= (stats.Defense != 0);
            bResults |= (stats.BonusArmorMultiplier != 0);
            bResults |= (stats.BaseArmorMultiplier != 0);
            bResults |= (stats.BonusHealthMultiplier != 0);
            bResults |= (stats.BonusStrengthMultiplier != 0);
            bResults |= (stats.BonusStaminaMultiplier != 0);
            bResults |= (stats.BonusAgilityMultiplier != 0);
            bResults |= (stats.BonusCritMultiplier != 0);
            bResults |= (stats.BonusAttackPowerMultiplier != 0);
            bResults |= (stats.BonusPhysicalDamageMultiplier != 0);
            bResults |= (stats.BonusSpellPowerMultiplier != 0);
            bResults |= (stats.ThreatIncreaseMultiplier != 0);
            bResults |= (stats.ThreatReductionMultiplier != 0);
            bResults |= (stats.CritMeleeRating != 0);
            bResults |= (stats.LotPCritRating != 0);
            bResults |= (stats.Bloodlust != 0);

            // Bringing in the damage stuff from DPSDK for better threat data
            bResults |= (stats.BonusShadowDamageMultiplier != 0);
            bResults |= (stats.BonusFrostDamageMultiplier != 0);
            bResults |= (stats.BonusDiseaseDamageMultiplier != 0);

            bResults |= (stats.BonusBloodStrikeDamage != 0);
            bResults |= (stats.BonusDeathCoilDamage != 0);
            bResults |= (stats.BonusDeathStrikeDamage != 0);
            bResults |= (stats.BonusFrostStrikeDamage != 0);
            bResults |= (stats.BonusHeartStrikeDamage != 0);
            bResults |= (stats.BonusIcyTouchDamage != 0);
            bResults |= (stats.BonusObliterateDamage != 0);
            bResults |= (stats.BonusScourgeStrikeDamage != 0);

            if (bResults)
            {
                foreach (SpecialEffect effect in stats.SpecialEffects())
                {
                    bResults |= (effect.Trigger == Trigger.DamageDone ||
                                effect.Trigger == Trigger.DamageSpellCast ||
                                effect.Trigger == Trigger.DamageSpellCrit ||
                                effect.Trigger == Trigger.DamageSpellHit ||
                                effect.Trigger == Trigger.SpellCast ||
                                effect.Trigger == Trigger.SpellCrit ||
                                effect.Trigger == Trigger.SpellHit ||
                                effect.Trigger == Trigger.DoTTick ||
                                effect.Trigger == Trigger.MeleeCrit ||
                                effect.Trigger == Trigger.MeleeHit ||
                                effect.Trigger == Trigger.PhysicalCrit ||
                                effect.Trigger == Trigger.PhysicalHit ||
                                effect.Trigger == Trigger.BloodStrikeOrHeartStrikeHit ||
                                effect.Trigger == Trigger.IcyTouchHit ||
                                effect.Trigger == Trigger.PlagueStrikeHit ||
                                effect.Trigger == Trigger.RuneStrikeHit ||
                                effect.Trigger == Trigger.Use);
                }
            }
            return bResults;
        }

        private float ApplyMultiplier(float baseValue, float multiplier)
        {
            return (baseValue * (1f + multiplier));
        }
        private Stats GetFrostPresence()
        {
            Stats FrostyStats = new Stats();
            FrostyStats.BaseArmorMultiplier += .6f; // Bonus armor for Frost Presence down from 80% to 60% as of 3.1.3
            FrostyStats.BonusArmorMultiplier += .6f; // Bonus armor for Frost Presence down from 80% to 60% as of 3.1.3
            FrostyStats.BonusHealthMultiplier += .1f; // Bonus 10% health for Frost Presence.
            FrostyStats.DamageTakenMultiplier -= .05f;// Bonus of 5% damage reduced for frost presence.
            FrostyStats.ThreatIncreaseMultiplier += .45f; // Bonus 45% threat for frost Presence.
            return FrostyStats;
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
