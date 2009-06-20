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
        private static readonly float BaseThreatValue = 1f; // Base value of threat modified by Threat weight.
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
                    _subPointNameColors.Add("Survival", System.Drawing.Color.Blue);
                    _subPointNameColors.Add("Mitigation", System.Drawing.Color.Red);
                    _subPointNameColors.Add("Threat", System.Drawing.Color.Green);
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

					    @"Summary:Survival Points*Survival Points represents the total raw damage 
(pre-Mitigation) you can take before dying. Unlike 
Mitigation Points, you should not attempt to maximize this, 
but rather get 'enough' of it, and then focus on Mitigation. 
'Enough' can vary greatly by fight and by your healers, but 
keeping it roughly even with Mitigation Points is a good 
way to maintain 'enough' as you progress. If you find that 
you are being killed by burst damage, focus on Survival Points.",
					    @"Summary:Mitigation Points*Mitigation Points represent the amount of damage you avoid, 
on average, through avoidance stats (Miss, Dodge, Parry) along with ways to improve survivablity, +heal or self 
healing, ability cooldowns.  It is directly relational to your Damage Taken. Ideally, you want to maximize 
Mitigation Points, while maintaining 'enough' Survival Points 
(see Survival Points). If you find yourself dying due to healers 
running OOM, or being too busy healing you and letting other 
raid members die, then focus on Mitigation Points.",
					    @"Summary:Overall Points*Overall Points are a sum of Mitigation and Survival Points. 
Overall is typically, but not always, the best way to rate gear. 
For specific encounters, closer attention to Mitigation and 
Survival Points individually may be important.",

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
        public override string[] OptimizableCalculationLabels 
        { 
            get 
            { return new string[] 
                {
                    "Chance to be Crit",
                    "Avoidance %",
                    "Damage Reduction %",
                    "Target Miss %",
                    "Target Parry %",
                    "Target Dodge %",
                    "Armor"
                }; 
            } 
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
            #region Setup what we need and validate.
            // Import the option values from the options tab on the UI.
            CalculationOptionsTankDK opts = character.CalculationOptions as CalculationOptionsTankDK;
            // Validate opts 
            if (null == opts) return null;

            CharacterCalculationsTankDK calcs = new CharacterCalculationsTankDK();
            if (null == calcs) return null;

            // Level differences.
            int iTargetLevel = opts.TargetLevel;
            // The full character data.
            Stats stats = GetCharacterStats(character, additionalItem);
            // validate that we get a stats object;
            if (null == stats) return null;

            // Get the shotrotation/combat model here.
            Rotation rot = opts.rotation;
            if (null == rot)
            {
                rot = new Rotation();
                opts.rotation = rot;
            }
            rot.m_FullStats = stats;

            CombatTable ct = new CombatTable(character, stats, opts);

            #endregion

            #region Store off values for display.
            calcs.BasicStrength = stats.Strength;
            calcs.BasicStats = stats;
            calcs.TargetLevel = iTargetLevel;

            #endregion

            float fLevelDiffModifier = (iTargetLevel - character.Level) * 0.2f;
            float fChanceToGetHit = 100.0f;

            // Get all the character avoidance numbers including deminishing returns.
            // Iterate through each hit type. and use fAvoidance array w/ the hitresult enum.
            float[] fAvoidance = new float[(uint)HitResult.NUM_HitResult];
            for (uint i = 0; i < (uint)HitResult.NUM_HitResult; i++)
            {
                // GetDRAvoidanceChance returns a dec. percentage.
                // Since CurrentAvoidance is a percent, need to multiply by 100.
                fAvoidance[i] = (StatConversion.GetDRAvoidanceChance(character, stats, (HitResult)i, iTargetLevel));
            }

            // So let's populate the miss, dodge and parry values for the UI display as well as pulling them out of the avoidance number.
            stats.Miss = fAvoidance[(int)HitResult.Miss];
            calcs.Miss = stats.Miss * 100f;
            fChanceToGetHit -= calcs.Miss;
            // Dodge needs to be factored in here.
            stats.Dodge = Math.Min(fChanceToGetHit, fAvoidance[(int)HitResult.Dodge]);
            calcs.Dodge = stats.Dodge * 100f;
            fChanceToGetHit -= calcs.Dodge;
            // Pary factors
            stats.Parry = Math.Min(fChanceToGetHit, fAvoidance[(int)HitResult.Parry]);
            calcs.Parry = stats.Parry * 100f;
            fChanceToGetHit -= calcs.Parry;

            // 5% + Level difference crit chance.  
            float attackerCrit = Math.Max(0.0f, ((.05f) - fAvoidance[(int)HitResult.Crit]));
            calcs.Crit = attackerCrit * 100f;
            calcs.DefenseRating = stats.DefenseRating;
            calcs.Defense = (StatConversion.GetDefenseFromRating(stats.DefenseRating, character.Class) + stats.Defense);
            calcs.Resilience = stats.Resilience;
            calcs.DefenseRatingNeeded = StatConversion.GetDefenseRatingNeeded(character, stats, iTargetLevel);

            #region Talents with general reach that aren't already in stats.

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
            stats.DamageTakenMultiplier -= bsDR;

            #endregion

            // Assuming the Boss has no ArPen
            // From http://www.skeletonjack.com/2009/05/14/dk-tanking-armor-cap/#comments
            // 75% armor cap.  Not sure if this is for DK or for all Tanks.  So I'm just going to handle it here.
            // I'll do more research and see if it needs to go into the general function.
            calcs.ArmorDamageReduction = (float)Math.Min(0.75f, StatConversion.GetArmorDamageReduction(iTargetLevel, stats.Armor, 0f, 0f, 0f));

            #region TargetDodge/Parry/Miss & Expertise
            bool bDualWielding = false;
            float f2hWeaponDamageMultiplier = 0f;
            float hitChance = 0;
            if (character.MainHand != null)
            {
                // 2-hander weapon specialization.
                if (character.MainHand.Slot == Item.ItemSlot.TwoHand)
                {
                    f2hWeaponDamageMultiplier = (.02f * character.DeathKnightTalents.TwoHandedWeaponSpecialization);
                }
                else
                {
                    // Toon is not using a 2h, meaning that he's DW if he's got something in his off hand.
                    bDualWielding = (character.OffHand != null);
                }

                // TODO: Shot rotation
                // TODO: Single vs. Multi Target tanking.
                float hitBonus = StatConversion.GetHitFromRating(stats.HitRating, character.Class) + stats.PhysicalHit;
                // 8% default miss rate vs lvl 83
                float chanceMiss = Math.Max(0f, 0.08f - hitBonus);
                if ((opts.TargetLevel - 80f) < 3)
                {
                    chanceMiss = Math.Max(0f, 0.05f + 0.005f * (opts.TargetLevel - 80f) - hitBonus);
                }
                if (bDualWielding)
                {
                    // Talent: Nerves of Cold Steel. .////////////////////////////////////////////
                    // +hit changes only.  See damage buff change further down.
                    chanceMiss += (0.19f - (.01f * character.DeathKnightTalents.NervesOfColdSteel));
                }



                // 6.5 % for a boss mob to dodge.
                // 15% for a boss mob to parry.
                float chanceParry = Math.Max(0.0f, 0.15f - StatConversion.GetDodgeParryReducFromExpertise(stats.Expertise));
                float chanceDodge = Math.Max(0.0f, 0.065f - StatConversion.GetDodgeParryReducFromExpertise(stats.Expertise));
                hitChance = 1.0f - (chanceMiss + chanceDodge + chanceParry);
                // Can't have more than 100% hit chance.
                hitChance = Math.Min(1f, hitChance);

                calcs.TargetDodge = chanceDodge;
                calcs.TargetMiss = chanceMiss;
                calcs.TargetParry = chanceParry;

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
            }
            #endregion

            #region ***** Survival Rating *****

            // The extra 10% health for Frost presence is now include in the character by default.
            float hp = calcs.BasicStats.Health;

            // For right now Effective Health will be HP + Armor/Resistance mitigation values.
            // Everything else is really mitigating damage based on RNG.

            // Physical damage:
            // So need the percent that is NOT from magic.
            calcs.Survival = hp / (1f - calcs.ArmorDamageReduction * (1f - opts.PercentIncomingFromMagic));

            // Magical damage:
            // if there is a max resistance, then it's likely they are stacking for that resistance.  So factor in that Max resistance.
            float fMaxResist = Math.Max(stats.ArcaneResistance, stats.FireResistance);
            fMaxResist = Math.Max(fMaxResist, stats.FrostResistance);
            fMaxResist = Math.Max(fMaxResist, stats.NatureResistance);
            fMaxResist = Math.Max(fMaxResist, stats.ShadowResistance);

            calcs.Survival += hp / (1f - StatConversion.GetAverageResistance(iTargetLevel, character.Level, fMaxResist, 0f) * opts.PercentIncomingFromMagic);
            calcs.SurvivalWeight = opts.SurvivalWeight;

            #endregion

            #region ***** Mitigation Rating *****

            float fIncMagicalDamage = (opts.IncomingDamage * opts.PercentIncomingFromMagic);
            float fIncPhysicalDamage = (opts.IncomingDamage - fIncMagicalDamage);
            // Integrate Expertise values to prevent additional physical damage coming in:
            // Each parry reducing swing timer by up to 40% so we'll average that damage increase out.
            // Each parry is factored by weapon speed - the faster the weapons, the more likely the boss can parry.
            // Figure out how many shots there are.  Right now, just calculating white damage.
            // TODO: once rotation is worked out, use that to get shotCount per rotation.
            float fShotCount = 0f;
            /*
            if (character.MainHand != null)
                fShotCount += (fRotationDuration / character.MainHand.Speed);
            if (character.OffHand != null)
                fShotCount += (fRotationDuration / character.OffHand.Speed);
            */
            // So now add in the 40% damage increase per shot, * the likelihood of being parried.
            fIncPhysicalDamage += (fIncPhysicalDamage * .4f * fShotCount) * calcs.TargetParry;

            if (character.DeathKnightTalents.SpellDeflection > 0)
            {
                Stats newStats = new Stats();
                newStats.SpellDamageTakenMultiplier -= .15f * character.DeathKnightTalents.SpellDeflection;
                stats += new SpecialEffect(Trigger.DamageSpellHit, newStats, 0, 0).GetAverageStats(3.0f, stats.Parry);
            }

            // Anti-Magic Shell. ////////////////////////////////////////////////////////
            // Talent: MagicSuppression increases AMS by 8/16/25% per point.
            // Glyph: GlyphofAntiMagicShell increases AMS by 2 sec.
            // AMS has a 45 sec CD.
            float amsUptime = (5.0f + (character.DeathKnightTalents.GlyphofAntiMagicShell == true ? 2.0f : 0.0f)) / 45.0f;
            // AMS reduces damage taken by 75% up to a max of 50% health.
            float amsReduction = 0.75f * (1.0f + character.DeathKnightTalents.MagicSuppression * 0.08f + (character.DeathKnightTalents.MagicSuppression == 3 ? 0.01f : 0f));
            float amsReductionMax = stats.Health * 0.5f;
            // up to 50% of health means that the amdDRvalue equates to the raw damage points removed.  
            // This means that toon health and INC damage values from the options pane are going to affect this quite a bit.
            float amsDRvalue = (Math.Min(amsReductionMax, fIncMagicalDamage * amsReduction) * amsUptime);

            // For any physical only damage reductions. 
            // if only 60% of the hits are landing, then tank is only taking 60% of the inc damage.
            fIncPhysicalDamage *= (fChanceToGetHit / 100f);
            // Adjust the damage by chance of crit getting through
            fIncPhysicalDamage += (fIncPhysicalDamage * attackerCrit) * 2f;
            // Factor in armor Damage Reduction
            fIncPhysicalDamage *= (1f - calcs.ArmorDamageReduction);
            // Talent: Unbreakable Armor
            float uaDR = (stats.Armor * 5f * .01f);
            // Glyph increases DR by 20%
            if (character.DeathKnightTalents.GlyphofUnbreakableArmor)
                uaDR *= 1.2f;
            // Mitigate the UA Damage reduction by its uptime.
            uaDR *= 20f / 120f;
            fIncPhysicalDamage -= uaDR;

            // Four T8 : AMS grants 10% damage reductions.
            Boolean fourT8 = character.ActiveBuffsContains("Darkruned Plate 4 Piece Bonus");
            if (fourT8)
            {
                stats.DamageTakenMultiplier -= (0.1f * amsUptime); 
            }

            // Factor in the AMS damage reduction value 
            fIncMagicalDamage -= amsDRvalue;
            fIncMagicalDamage = StatConversion.ApplyMultiplier(fIncMagicalDamage, stats.SpellDamageTakenMultiplier);

            fIncMagicalDamage = StatConversion.ApplyMultiplier(fIncMagicalDamage, stats.DamageTakenMultiplier);
            fIncPhysicalDamage = StatConversion.ApplyMultiplier(fIncPhysicalDamage, stats.DamageTakenMultiplier);

            // Let's make sure we don't go into negative damage here.
            fIncMagicalDamage = Math.Max(0f, fIncMagicalDamage);
            fIncPhysicalDamage = Math.Max(0f, fIncPhysicalDamage);

            calcs.Mitigation = opts.IncomingDamage - (fIncMagicalDamage + fIncPhysicalDamage);
            // HACK: Throwing my weight around.
            float fMitigationWeight = 10f;
            calcs.Mitigation *= fMitigationWeight;

            #endregion

            #region ***** THREAT *****

            float fGCD = rot.getGCDTime();
            float fDamageTotal = ct.GetTotalThreat();
            if (fDamageTotal > 0)
            {
                calcs.Threat = fDamageTotal;
            }
            else
            if (character.MainHand != null)
            {

                // Base chance of spell hit is 83%
                float SpellHitChance = .83f;
                SpellHitChance += stats.SpellHit;
                // Can't have more than 100% hit chance.
                SpellHitChance = Math.Min(1f, SpellHitChance);

                float totalStaticHaste = stats.PhysicalHaste;
                totalStaticHaste += StatConversion.GetHasteFromRating(stats.HasteRating, character.Class);

                // Every 14 AP == +1DPS
                float fDPSfromAP = stats.AttackPower / 14.0f;
                float fDamageWhite = (character.MainHand.Item.DPS + fDPSfromAP) * (character.MainHand.Speed);
                // Talent: BloodCaked Blade .////////////////////////////////////////////
                // 10% chance per point per auto attack
                // to cause 25% (+12.5% per disease) weapon damage (assuming 1 disease)
                fDamageWhite += (fDamageWhite * .375f) * (.1f * character.DeathKnightTalents.BloodCakedBlade);

                // Talent: Nerves of Cold Steel. .////////////////////////////////////////////
                // +5% damage to off-hand damage.
                if (bDualWielding && character.OffHand != null)
                {
                    fDamageWhite += (character.OffHand.Item.DPS * (1f + (.05f * character.DeathKnightTalents.NervesOfColdSteel))
                                    + (stats.AttackPower / 14.0f)) * (character.OffHand.Speed);
                }

                // TODO: Entry point for shot rotation function.
                // Hack!!!!!!!!!!!!!!!!!!!!!!!!!!
                // provide a % based way of scaling the damage by magic.
                float fDamageSpecial = fDamageWhite * opts.PercThreatFromSpells;
                fDamageWhite = fDamageWhite - fDamageSpecial;

                // Apply Haste.
                // Haste really only affects white damage w/ some slight GCD changes - but those are negligible.
                fDamageWhite *= 1f + totalStaticHaste;
                // White Damage modifiers:
                fDamageWhite *= (1f + 0.04f * character.DeathKnightTalents.Necrosis); // 4% shadow damage added to auto attacks per point.

                // Talent: Black Ice //////////////////////////////////////////////////////
                // Factoring it to all special effects since most shadow and frost damage is being done by special effects.
                fDamageSpecial *= (1f + 0.01f * character.DeathKnightTalents.BlackIce);
                // Talent: Rage of Rivendare //////////////////////////////////////////////////////
                // +2% damage by non-white damage per point.
                fDamageSpecial *= (1f + 0.02f * character.DeathKnightTalents.RageOfRivendare);

                // Remove hitchance problems, add Crit chance..
                fDamageWhite *= hitChance;
                fDamageSpecial *= SpellHitChance;


                fDamageTotal = fDamageWhite + fDamageSpecial;
                fDamageTotal *= (1f + 0.01f * character.DeathKnightTalents.Desecration) // 1% additional damage done while on unholy ground.
                    * (1f + 0.02f * character.DeathKnightTalents.BloodGorged * .5f); // 2% additional Damage done per point while > 75% health.  Assuming that's going to be 1/2 the time.

                // For right now all damage is only looking at melee damage.  
                // so Annihillation is going in full.  Once we get shot rotation, I'll break it out.
                fDamageTotal = (fDamageWhite + fDamageSpecial)
                                * (1f + .02f * bsUptime) // Bone Shield 2% damage increase when active.
                                * (1f + f2hWeaponDamageMultiplier) // 2h Weapon spec.
                                ;

                // Increase the total damage done by the chance of crits hitting and each crit does 100% extra damage.
                fDamageTotal += (fDamageTotal * calcs.BasicStats.PhysicalCrit);

                calcs.Threat = fDamageTotal;

                // Threat buffs.
                calcs.Threat *= 1.0f + (stats.ThreatIncreaseMultiplier - stats.ThreatReductionMultiplier);
            }

            calcs.ThreatWeight = BaseThreatValue * opts.ThreatWeight;

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

            // Basic racial & class baseline.
            Stats statsRace = GetRaceStats(character);
            statsRace.Defense = 400; // Adding in the base 400 Defense skill all tanks are expected to have.  There are too many places where this just kinda stuck in.  It should be attached to the toon.
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            Stats statsFrost = GetFrostPresence();
            Stats statsTotal = new Stats();
            Stats statsGearEnchantsBuffs = new Stats();
            // We gather up everything here:
            statsGearEnchantsBuffs = statsBaseGear + statsBuffs + statsRace;

            // Stack only the info we care about.
            statsTotal = GetRelevantStats(statsGearEnchantsBuffs);
            statsTotal += statsFrost;
            Stats statsTalents = GetTalents(statsTotal, character);
            statsTotal += statsTalents;
            // Adding in Special Effects. 
            foreach (SpecialEffect effect in statsTotal.SpecialEffects())
            {
                statsTotal += effect.GetAverageStats();
            }

            // Apply Stat modifiers
            statsTotal.Strength = StatConversion.ApplyMultiplier(statsTotal.Strength, statsTotal.BonusStrengthMultiplier);
            statsTotal.Agility = StatConversion.ApplyMultiplier(statsTotal.Agility, statsTotal.BonusAgilityMultiplier);
            statsTotal.Stamina = StatConversion.ApplyMultiplier(statsTotal.Stamina, statsTotal.BonusStaminaMultiplier);
            statsTotal.Armor += StatConversion.GetArmorFromAgility(statsTotal.Agility);
            statsTotal.Armor = StatConversion.ApplyMultiplier(statsTotal.Armor, statsTotal.BaseArmorMultiplier);
            statsTotal.Armor += StatConversion.ApplyMultiplier(statsTotal.BonusArmor, statsTotal.BonusArmorMultiplier);

            statsTotal.Health = StatConversion.ApplyMultiplier((statsTotal.Health + StatConversion.GetHealthFromStamina(statsTotal.Stamina)), statsTotal.BonusHealthMultiplier);

            // Talent: BladedArmor //////////////////////////////////////////////////////////////
            if (character.DeathKnightTalents.BladedArmor > 0)
            {
                statsTotal.AttackPower += (statsTotal.Armor / 180f) * (float)character.DeathKnightTalents.BladedArmor;
            }
            // AP, crit, etc.  already being factored in w/ multiplier.
            statsTotal.AttackPower = StatConversion.ApplyMultiplier((statsTotal.AttackPower + (statsTotal.Strength * 2)), statsTotal.BonusAttackPowerMultiplier);
            statsTotal.CritRating = StatConversion.ApplyMultiplier((statsTotal.CritRating + statsTotal.CritMeleeRating), statsTotal.BonusCritMultiplier);
            statsTotal.PhysicalCrit = StatConversion.ApplyMultiplier(statsTotal.PhysicalCrit 
                                        + StatConversion.GetCritFromAgility(statsTotal.Agility, character.Class)
                                        + StatConversion.GetCritFromRating(statsTotal.CritRating), statsTotal.BonusCritMultiplier);
            statsTotal.SpellCrit = StatConversion.ApplyMultiplier(statsTotal.SpellCrit
                                        + StatConversion.GetCritFromRating(statsTotal.CritRating), statsTotal.BonusCritMultiplier);

            statsTotal.PhysicalCrit += statsTotal.LotPCritRating;
            statsTotal.SpellCrit += statsTotal.LotPCritRating;

            statsTotal.SpellHit += StatConversion.GetSpellHitFromRating(statsTotal.HitRating);

            // Parry from str. is only available to DKs.
            statsTotal.ParryRating += statsTotal.Strength * 0.25f;

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
                SpellHitRating = stats.SpellHitRating,
                SpellHit = stats.SpellHit,

                Healed = stats.Healed,

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
 
                // Frost presence.
                DamageTakenMultiplier = stats.DamageTakenMultiplier,
 
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

                BonusPlagueStrikeCrit = stats.BonusPlagueStrikeCrit,
                BonusIceboundFortitudeDuration = stats.BonusIceboundFortitudeDuration,
                BonusRuneStrikeMultiplier = stats.BonusRuneStrikeMultiplier,
                BonusAntiMagicShellDamageReduction = stats.BonusAntiMagicShellDamageReduction,

                // Resistances
                ArcaneResistance = stats.ArcaneResistance,
                FireResistance = stats.FireResistance,
                FrostResistance = stats.FrostResistance,
                NatureResistance = stats.NatureResistance,
                ShadowResistance = stats.ShadowResistance,
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
            bResults |= (stats.SpellHitRating != 0);
            bResults |= (stats.SpellHit != 0);
            bResults |= (stats.CritRating != 0);
            bResults |= (stats.PhysicalCrit != 0);
            bResults |= (stats.PhysicalHaste != 0);
            bResults |= (stats.PhysicalHit != 0);
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
            bResults |= (stats.Healed != 0);
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
            bResults |= (stats.DamageTakenMultiplier != 0);
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

            bResults |= (stats.BonusPlagueStrikeCrit != 0);
            bResults |= (stats.BonusIceboundFortitudeDuration != 0);
            bResults |= (stats.BonusRuneStrikeMultiplier != 0);
            bResults |= (stats.BonusAntiMagicShellDamageReduction != 0);

            bResults |= (stats.ArcaneResistance != 0);
            bResults |= (stats.FireResistance != 0);
            bResults |= (stats.FrostResistance != 0);
            bResults |= (stats.NatureResistance != 0);
            bResults |= (stats.ShadowResistance != 0);


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
                                effect.Trigger == Trigger.BloodStrikeHit ||
                                effect.Trigger == Trigger.HeartStrikeHit ||
                                effect.Trigger == Trigger.IcyTouchHit ||
                                effect.Trigger == Trigger.PlagueStrikeHit ||
                                effect.Trigger == Trigger.RuneStrikeHit ||
                                effect.Trigger == Trigger.Use);
                }
            }
            return bResults;
        }

        public override bool IsItemRelevant(Item item)
        {
            if (item.Slot == Item.ItemSlot.Ranged && item.Type != Item.ItemType.Sigil)
                return false;
            return base.IsItemRelevant(item);
        }

        private Stats GetFrostPresence()
        {
            Stats FrostyStats = new Stats();
            FrostyStats.BaseArmorMultiplier += .6f; // Bonus armor for Frost Presence down from 80% to 60% as of 3.1.3
            FrostyStats.BonusHealthMultiplier += .1f; // Bonus 10% health for Frost Presence.
            FrostyStats.DamageTakenMultiplier -= .05f;// Bonus of 5% damage reduced for frost presence.
            FrostyStats.ThreatIncreaseMultiplier += .45f; // Bonus 45% threat for frost Presence.
            return FrostyStats;
        }

        /// <summary>
        /// Build the talent special effects.
        /// </summary>
        private Stats GetTalents(Stats FullCharacterStats, Character character)
        {
            Stats sReturn = new Stats();
            Stats newStats = new Stats();
            float fDamageDone = 0f;

            #region Blood Talents.
            // Butchery
            // 1RPp5 per Point
            // TODO: Implement Runic Regen info.
            if (character.DeathKnightTalents.Butchery > 0)
            {
                sReturn.RPp5 += 1 * character.DeathKnightTalents.Butchery;
            }

            // Subversion
            // Increase crit 3% per point of BS, HS, Oblit
            if (character.DeathKnightTalents.Subversion > 0)
            {
            }

            // Blade Barrier
            // Reduce damage by 1% per point for 10 sec.
            if (character.DeathKnightTalents.BladeBarrier > 0)
            {
                // for now having the functionality that currently exists is better than nothing.
                // TODO: Implement SpecialEffect for Rune CD.
                sReturn.DamageTakenMultiplier -= (.01f * character.DeathKnightTalents.BladeBarrier);
            }

            // Bladed Armor
            // 1 AP per point per 180 Armor
            // Implmented after Frost Presence above.

            // Scent of Blood
            // 15% after Dodge, Parry or damage received causing 1 melee hit per point to generate 5 runic power.

            // 2H weapon spec.
            // 2% per point increased damage

            // Rune Tap
            // Convert 1 BR to 10% health.
            if (character.DeathKnightTalents.RuneTap > 0)
            {
                newStats = new Stats();
                float fCD = 60f;
                newStats.Healed = (FullCharacterStats.Health * .1f);
                if (character.DeathKnightTalents.ImprovedRuneTap == 0)
                {
                    sReturn.AddSpecialEffect(new SpecialEffect(Trigger.Use, newStats, 0, fCD));
                }
                else
                {
                    // Improved Rune Tap.
                    // increases the health provided by RT by 33% per point. and lowers the CD by 10 sec per point
                    fCD -= (10f * character.DeathKnightTalents.ImprovedRuneTap);
                    newStats.Healed *= 1f + (.33f * character.DeathKnightTalents.ImprovedRuneTap);
                    sReturn.AddSpecialEffect(new SpecialEffect(Trigger.Use, newStats, 0, fCD));
                }
            }

            // Dark Conviction 
            // Increase Crit w/ weapons, spells, and abilities by 1% per point.
            if (character.DeathKnightTalents.DarkConviction > 0)
            {
                sReturn.PhysicalCrit += (.01f * character.DeathKnightTalents.DarkConviction);
                sReturn.SpellCrit += (.01f * character.DeathKnightTalents.DarkConviction);
            }

            // Death Rune Mastery
            // Create death runes out of Frost & Unholy for each oblit.

            // Spell Deflection
            // Parry chance of taking 15% less damage per point from direct damage spell

            // Vendetta
            // Heals you for up to 2% per point on killing blow

            // Bloody Strikes
            // increases damage of BS and HS by 15% per point
            // increases damage of BB by 10% per point

            // Veteran of the 3rd War
            // increases Str and Stam by 2% per point
            // increases expertise by 2 per point.
            if (character.DeathKnightTalents.VeteranOfTheThirdWar > 0)
            {
                sReturn.BonusStrengthMultiplier += (.02f * character.DeathKnightTalents.VeteranOfTheThirdWar);
                sReturn.BonusStaminaMultiplier += (.02f * character.DeathKnightTalents.VeteranOfTheThirdWar);
                sReturn.Expertise += (2f * character.DeathKnightTalents.VeteranOfTheThirdWar);
            }

            // Mark of blood
            // buff that lasts 20 secs or 20 hits
            // heals the target for 4% of max health for each hit.
            if (character.DeathKnightTalents.MarkOfBlood > 0)
            {
                // TODO: Need to know how many hits are incoming.
                // for now assuming 10 hits.
                newStats = new Stats();
                newStats.Healed = (FullCharacterStats.Health * .04f * 10f);
                sReturn.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, newStats, 20, 3f * 60f));
            }

            // Bloody Vengence
            // 1% per point bonus to physical damage for 30 secs after a crit w/ up to 3 stacks.
            if (character.DeathKnightTalents.BloodyVengeance > 0)
            {
                newStats = new Stats();
                newStats.BonusPhysicalDamageMultiplier = .01f * character.DeathKnightTalents.BloodyVengeance;
                sReturn.AddSpecialEffect(new SpecialEffect(Trigger.DamageSpellCrit, newStats, 30, 0, 1, 3));
                sReturn.AddSpecialEffect(new SpecialEffect(Trigger.MeleeCrit, newStats, 30, 0, 1, 3));
            }

            // Abominations Might
            // BS & HS have 25% chance per point
            // DS and Oblit have 50% chance per point
            // increase AP by 10% of raid for 10 sec.
            // 1% per point increase to str.
            if (character.DeathKnightTalents.AbominationsMight > 0)
            {
                sReturn.BonusStrengthMultiplier += (.01f * character.DeathKnightTalents.AbominationsMight);
                newStats = new Stats();
                newStats.BonusAttackPowerMultiplier = .1f;
                sReturn.AddSpecialEffect(new SpecialEffect(Trigger.BloodStrikeHit, newStats, 10, 0));
                sReturn.AddSpecialEffect(new SpecialEffect(Trigger.HeartStrikeHit, newStats, 10, 0));
                // TODO: Add DS & Oblit hit.
                //                sReturn.AddSpecialEffect(new SpecialEffect(Trigger.DeathStrikeHit, newStats, 10, 0);
                //                sReturn.AddSpecialEffect(new SpecialEffect(Trigger.ObliterateHit, newStats, 10, 0);
            }

            // Bloodworms
            // 3% chance per point per hit to cause the target to spawn 2-4 blood worms
            // Healing you 150% of the damage they do for 20 sec.
            if (character.DeathKnightTalents.Bloodworms > 0)
            {
                newStats = new Stats();
                // TODO: figure out how much damage the worms do.
                fDamageDone = 30f;
                newStats.Healed = (fDamageDone * 1.5f);
                sReturn.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalHit, newStats, 20, 0, .03f * character.DeathKnightTalents.Bloodworms));
            }

            // Hysteria
            // Killy frenzy for 30 sec.
            // Increase physical damage by 20%
            // take damage 1% of max every sec.
            if (character.DeathKnightTalents.Hysteria > 0)
            {
                float fDur = 30f;
                newStats = new Stats();
                newStats.BonusPhysicalDamageMultiplier += .2f;
                newStats.Healed -= (FullCharacterStats.Health * .01f * fDur);
                sReturn.AddSpecialEffect(new SpecialEffect(Trigger.Use, newStats, fDur, 3f * 60f));
            }

            // Improved Blood Presence
            // while in frost or unholy, you retain the 2% per point healing from blood presence
            // Healing done to you is increased by 5% per point
            if (character.DeathKnightTalents.ImprovedBloodPresence > 0)
            {
                fDamageDone = 100f; // This needs to be factored in from threat - so may have to pull it out of here.
                sReturn.HealingReceivedMultiplier += .5f * character.DeathKnightTalents.ImprovedBloodPresence;
                sReturn.Healed += (fDamageDone * .02f * character.DeathKnightTalents.ImprovedBloodPresence);
            }

            // Improved Death Strike
            // increase damage of DS by 15% per point 
            // increase crit chance of DS by 3% per point

            // Sudden Doom
            // BS & HS have a 5% per point chance to launch a DC at target
            if (character.DeathKnightTalents.SuddenDoom > 0)
            {
            }

            // Vampiric Blood
            // temp 15% of max health and
            // increases health generated by 35% for 20 sec.
            // 2 min CD.
            if (character.DeathKnightTalents.VampiricBlood > 0)
            {
                newStats = new Stats();
                newStats.Health += (FullCharacterStats.Health * .15f);
                newStats.HealingReceivedMultiplier += .35f;
                sReturn.AddSpecialEffect(new SpecialEffect(Trigger.Use, newStats, 20, 2f * 60f));
            }

            // Will of the Necropolis
            // Damage that takes you below 35% health or while at less than 35% is reduced by 5% per point.  
            // CD 15 sec.
            // Incoming damage must be >= than 5% of total health.
            if (character.DeathKnightTalents.WillOfTheNecropolis > 0)
            {
                newStats = new Stats();
                newStats.DamageTakenMultiplier -= (.05f * character.DeathKnightTalents.WillOfTheNecropolis);
                // Need to factor in the damage taken aspect of the trigger.
                // Using the assumption that the tank will be at < 35% health about that % of the time.
                sReturn.AddSpecialEffect(new SpecialEffect(Trigger.SpellHit, newStats, 0, 15f, .35f));
                sReturn.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalHit, newStats, 0, 15f, .35f));
            }

            // Heart Strike

            // Might of Mograine
            // increase crit damage of BB, BS, DS, and HS by 15% per point

            // Blood Gorged
            // when above 75% health, you deal 2% more damage per point
            // when above 75% health, you receive 2% Armor Pen
            if (character.DeathKnightTalents.BloodGorged > 0)
            {
                // Damage done increase has to be in shot rotation.
                // Assuming a 50% up time 
                sReturn.ArmorPenetration += (.02f * character.DeathKnightTalents.BloodGorged * .5f);
            }

            // Dancing Rune Weapon
            #endregion

            #region Frost Talents
            // Improved Icy Touch
            // 5% per point additional IT damage
            // 2% per point target haste reduction 
            if (character.DeathKnightTalents.ImprovedIcyTouch > 0)
            {
                sReturn.BonusIcyTouchDamage += (.05f * character.DeathKnightTalents.ImprovedIcyTouch);
                // TODO: Need to factor in the correct haste adjustment for target.
                // For now assuming a straight 2% damage reduction per point.
                sReturn.DamageTakenMultiplier -= .02f * character.DeathKnightTalents.ImprovedIcyTouch;
            }

            // Runic Power Mastery
            // Increases Max RP by 15 per point
            if (character.DeathKnightTalents.RunicPowerMastery > 0)
            {
                sReturn.BonusMaxRunicPower += 5 * character.DeathKnightTalents.RunicPowerMastery;
            }

            // Toughness
            // Increases Armor Value from items by 3% per point.
            // Reducing duration of all slowing effects by 6% per point.  
            if (character.DeathKnightTalents.Toughness > 0)
            {
                sReturn.BaseArmorMultiplier += (.03f * character.DeathKnightTalents.Toughness);
            }

            // Icy Reach
            // Increases range of IT & CoI and HB by 5 yards per point.

            // Black Ice
            // Increase Frost & shadow damage by 2% per point
            if (character.DeathKnightTalents.BlackIce > 0)
            {
                sReturn.BonusFrostDamageMultiplier += .02f * character.DeathKnightTalents.BlackIce;
                sReturn.BonusShadowDamageMultiplier += .02f * character.DeathKnightTalents.BlackIce;
            }

            // Nerves of Cold Steel
            // Increase hit w/ 1H weapons by 1% per point
            // Increase damage done by off hand weapons by 5% per point
            // Implement in combat shot roation

            // Icy Talons
            // Increase melee attack speed by 4% per point for the next 20 sec.
            if (character.DeathKnightTalents.IcyTalons > 0)
            {
                newStats = new Stats();
                newStats.PhysicalHaste += (.04f * character.DeathKnightTalents.IcyTalons);
                sReturn.AddSpecialEffect(new SpecialEffect(Trigger.SpellHit, newStats, 20, 0));
            }

            // Lichborne
            // for 15 sec, immune to charm, fear, sleep

            // Annihilation
            // +1 % per point melee Crit chance 
            // 33% per point that oblit will not consume diseases
            if (character.DeathKnightTalents.Annihilation > 0)
            {
                sReturn.PhysicalCrit += (.01f * character.DeathKnightTalents.Annihilation);
            }

            // Killing Machine
            // Melee attacks have a chance to make IT, HB, or FS a crit.
            // increased proc per point.

            // Chill of the Grave
            // CoI, HB, IT and Oblit generate 2.5 RP per point.

            // Endless Winter
            // CoI has 50% per point to cause FF
            // Mind Freeze RP cost is reduced by 50% per point.

            // Frigid Dreadplate
            // Melee attacks against you will miss by +1% per point
            if (character.DeathKnightTalents.FrigidDreadplate > 0)
            {
                sReturn.Miss += .01f * character.DeathKnightTalents.FrigidDreadplate;
            }

            // Glacier Rot
            // Diseased enemies take 7%, 13% , 20% more damage from IT, HB, FS.
            if (character.DeathKnightTalents.GlacierRot > 0)
            {
                float fBonus = 0f;
                switch (character.DeathKnightTalents.GlacierRot)
                {
                    case 1:
                        fBonus = 0.07f;
                        break;
                    case 2:
                        fBonus = 0.13f;
                        break;
                    case 3:
                        fBonus = 0.2f;
                        break;
                }
                sReturn.BonusIcyTouchDamage += fBonus;
                sReturn.BonusFrostStrikeDamage += fBonus;
                // TODO:
                //sReturn.BonusHowlingBlastDamage += fBonus;
            }

            // Deathchill
            // when active IT, HB, FS, Oblit will crit.

            // Improved Icy Talons
            // increases the melee hast of the group/raid by 20%
            // increases your haste by 5% all the time.
            if (character.DeathKnightTalents.ImprovedIcyTalons > 0)
            {
                sReturn.PhysicalHaste += .05f;
            }

            // Merciless Combat
            // addtional 6% per point damage for IT, HB, Oblit, and FS
            // on targets of less than 35% health.

            // Rime
            // increases crit chance of IT and Oblit by 5% per point
            // Oblit has a 5% per point to reset CD of HB and HB consumes no runes
            
            // Chilblains
            // FF victimes are movement reduced 15, 30, 50%

            // Hungering Cold
            // Spell that freezes all enemies w/ 10 yards.

            // Improved Frost Presence
            // retain the health bonus 5% per point when in non-Frost presence
            // Decrease damage done to you by 1% per point.
            if (character.DeathKnightTalents.ImprovedFrostPresence > 0)
            {
                sReturn.DamageTakenMultiplier -= (.01f * character.DeathKnightTalents.ImprovedFrostPresence);
            }

            // Blood of the North
            // BS & FS damage +5% per point
            // BS & Pest create DeathRune from Blood 20% per point.
            if (character.DeathKnightTalents.BloodOfTheNorth > 0)
            {
                sReturn.BonusFrostStrikeDamage += (.05f * character.DeathKnightTalents.BloodOfTheNorth);
                sReturn.BonusBloodStrikeDamage += (.05f * character.DeathKnightTalents.BloodOfTheNorth);
            }

            // Unbreakable Armor
            // Reinforces your armor with a thick coat of ice, reducing damage from all attacks by [5 * AR * 0.01] and increasing your Strength by 25% for 20 sec.  The amount of damage reduced increases as your armor increases.
            if (character.DeathKnightTalents.UnbreakableArmor > 0)
            {
                newStats = new Stats();
                newStats.BonusStrengthMultiplier += .25f;
                sReturn.AddSpecialEffect(new SpecialEffect(Trigger.Use, newStats, 20, 2 * 60f));
            }

            // Acclimation
            // When hit by a spell, 10% chance per point to boost resistance to that type of magic for 18 sec.  
            // up to 3 stacks.
            if (character.DeathKnightTalents.Acclimation > 0)
            {
                newStats = new Stats();
                float chance = (.1f * character.DeathKnightTalents.Acclimation);
                newStats.FireResistance += 50f;
                newStats.FrostResistance += 50f;
                newStats.ArcaneResistance += 50f;
                newStats.ShadowResistance += 50f;
                newStats.NatureResistance += 50f;
                sReturn.AddSpecialEffect(new SpecialEffect(Trigger.SpellHit, newStats, 18, 0, chance, 3));
            }

            // Frost Strike

            // Guile of Gorefiend
            // Increases CritStrike Damage of BS, FS, HB, Oblit by 15% per point.
            // Increases Duration of IBF by 2 sec per point.
            // HACK: Implenting IceBound Fortitude. ////////////////////////////////////////////////////////
            // Implmenting IBF here because it's affected by GoGF
            // Four T7 increases IBF by 3 sec.
            // IBF has a 60 sec CD.
            Boolean fourT7 = character.ActiveBuffsContains("Scourgeborne Plate 4 Piece Bonus");
            float fIBFDur = (12.0f + character.DeathKnightTalents.GuileOfGorefiend * 2.0f + (fourT7 ? 3.0f : 0.0f));
            // IBF reduces damage taken by 20% + 3% for each 28 defense over 400.
            float ibfReduction = 0.2f + ((FullCharacterStats.Defense - 400) * 0.03f / 28.0f);
            if (character.DeathKnightTalents.GlyphofIceboundFortitude)
            {
                // Glyphed to 30% + def value.
                ibfReduction += 0.1f;
                // Since 3.1 the glyph is capped to 30%.
            }
            // There has always been a cap on the IBF to 30% the glyph was nerfed, not IBF.
            // So it's not worth it for those who already have alot of DEF. (EG. most tanks)
            ibfReduction = Math.Min(0.3f, ibfReduction);
            newStats = new Stats();
            newStats.DamageTakenMultiplier -= ibfReduction;
            sReturn.AddSpecialEffect(new SpecialEffect(Trigger.Use, newStats, fIBFDur, 60));
            if (character.DeathKnightTalents.GuileOfGorefiend > 0)
            {
                // TODO: Crit Damage multiplier
            }

            // Tundra Stalker
            // Your spells & abilities deal 3% per point more damage to targets w/ FF
            // Increases Expertise by 1 per point
            if (character.DeathKnightTalents.TundraStalker > 0)
            {
                // Assuming FF is always up.
                sReturn.BonusDamageMultiplier += .03f * character.DeathKnightTalents.TundraStalker;
                sReturn.Expertise += 1f * character.DeathKnightTalents.TundraStalker;
            }

            // Howling Blast.

            #endregion

            #region UnHoly Talents
            // Vicious Strikes 
            // Increases Crit chance by 3% per point of PS and SS
            // Increases Crit Strike Damage by 15% per point of PS and SS

            // Virulence
            // Increases Spell hit +1% per point
            if (character.DeathKnightTalents.Virulence > 0)
            {
                sReturn.SpellHit += .01f * character.DeathKnightTalents.Virulence;
            }

            // Anticipation
            // Increases dodge by 1% per point
            if (character.DeathKnightTalents.Anticipation > 0)
            {
                sReturn.Dodge += .01f * character.DeathKnightTalents.Anticipation;
            }

            // Epidemic
            // Increases Duration of BP and FF by 3 sec per point

            // Morbidity
            // increases dam & healing of DC by 5% per point
            // Decreases CD of DnD by 5 sec per point

            // Unholy Command
            // reduces CD of DG by 5 sec per point

            // Ravenous Dead
            // Increases Str +1% per point.
            // Increases contribution of your str & stam to ghoul by 20% per point
            if (character.DeathKnightTalents.RavenousDead > 0)
            {
                sReturn.BonusStrengthMultiplier += (.01f * character.DeathKnightTalents.RavenousDead);
                // Ghouls don't help tank here.
            }

            // Outbreak
            // increases dam of PS by 10% per point
            // increases dam of SS by 7% per point

            // Necrosis
            // Autoattacks deal additional 4% shadow

            // Corpse Explosion
            // Does damage by blowing up a corpse to all targets in 10 yards

            // On a Pale Horse
            // Reduce dur of stun and fear by 10% per point
            // increase mount speed byh 10% per point

            // Blood-Caked Blade
            // 10% chance per point to cause Blood-Caked strike
            
            // Night of the Dead
            // Reduces CD of Raise Dead by 45 sec per point
            // Reduces CD of Army of the dead by 5 min per point
            
            // Unholy Blight
            // Shadow Damage done to all targets in 10 yards for 20 sec.

            // Impurity
            // Attack Power bonus to spells increased by 4% per point.
            
            // Dirge
            // DS, Oblit, PS and SS generate 2.5 more runic power per point.

            // Magic Suppression
            // 2% per point less damage from all magic.
            // AMS absorbs additional 8, 16, 25% of spell damage.
            if (character.DeathKnightTalents.MagicSuppression > 0)
            {
                sReturn.SpellDamageTakenMultiplier -= .02f * character.DeathKnightTalents.MagicSuppression;
                // TODO: factor in AMS
            }

            // Reaping 
            // BS or Pest convert to DR.

            // Master of Ghouls
            // Reduces CD on Raise Dead by 60 sec.
            // Ghoul summoned is perm (pet). 

            // Desecration
            // PS and SS cause Desecrated Ground effect.
            // Targets are slowed by 10% per point
            // You cause 1% more damage 
            // Lasts 12 sec.
            if (character.DeathKnightTalents.Desecration > 0)
            {
                newStats = new Stats();
                newStats.BonusDamageMultiplier += .01f * character.DeathKnightTalents.Desecration;
                // Gonna use an average CD of a rune at 10sec per rune divided by 2 runes == 5 sec.
                sReturn.AddSpecialEffect(new SpecialEffect(Trigger.SpellHit, newStats, 12, 5));
            }

            // AntiMagic Zone
            // Creates a zone where party/raid members take 75% less spell damage
            // Lasts 10 secs or X damage.  
            if (character.DeathKnightTalents.AntiMagicZone > 0)
            {
                newStats = new Stats();
                newStats.SpellDamageTakenMultiplier -= .75f * character.DeathKnightTalents.AntiMagicZone;
                sReturn.AddSpecialEffect(new SpecialEffect(Trigger.Use, newStats, 10, 2 * 60));
            }

            // Improved Unholy Presence
            // in Blood or Frost, retain movement speed (8%, 15%).
            // Runes finish CD 5% per point faster.

            // Ghoul Frenzy
            // Grants pet 25% haste for 30 sec and heals it for 60% health.

            // Crypt Fever
            // CF increases disease damage taken by target by 10% per point

            // Bone Shield
            // 4 Bones 
            // Takes 20% less dmage from all sources
            // Does 2% more damage to target
            // Each damaging attack consumes a bone.
            // Lasts 5 mins

            // Ebon Plaguebringer
            // CF becomes EP - increases magic damage taken by targets 4, 9, 13% in addition to disease damage
            // Increases crit strike chance by 1% per point
            if (character.DeathKnightTalents.EbonPlaguebringer > 0)
            {
                // TODO: implment magic damage bonus.
                sReturn.BonusCritChance += .01f * character.DeathKnightTalents.EbonPlaguebringer;
            }

            // Sourge Strike

            // Rage of Rivendare
            // 2% per point more damage to targets w/ BP
            // Expertise +1 per point
            if (character.DeathKnightTalents.RageOfRivendare > 0)
            {
                sReturn.Expertise += character.DeathKnightTalents.RageOfRivendare;
                // Assuming BP is always on.
                sReturn.BonusDamageMultiplier += .02f * character.DeathKnightTalents.RageOfRivendare;
            }

            // Summon Gargoyle

            #endregion

            return sReturn;
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
