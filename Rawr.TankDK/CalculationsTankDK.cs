using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
#if RAWR3
using System.Windows.Media;
#else
using System.Drawing;
#endif

namespace Rawr.TankDK {
    [Rawr.Calculations.RawrModelInfo("TankDK", "spell_shadow_deathanddecay", CharacterClass.DeathKnight)]
    public class CalculationsTankDK : CalculationsBase {
        enum Quality {
            Uncommon,
            Rare, 
            Epic,
            Jewelcraft,

            NUM_Quality
        }

        public bool m_bT9_4PC = false;

        public override List<GemmingTemplate> DefaultGemmingTemplates {
            get {
				////Relevant Gem IDs for TankDKs
				//Red
                //                    UC     Rare   Epic   JC
				int[] subtle =      { 39907, 40000, 40115, 42151 }; // +Dodge
                //int[] bold =        { 39900, 39996, 40111, 42142 }; // +Str
                //int[] bright =      { 39906, 39999, 40114, 36766 }; // +AP
                //int[] delicate =    { 39905, 39997, 40112, 42143 }; // +Agi
                //int[] flashing =    { 39908, 40001, 40116, 42152 }; // +Parry

				//Purple
				int[] regal =       { 39938, 40031, 40138, }; // +dodge, Stam
                //int[] balanced =    { 39937, 40029, 40136, }; // +AP +Stam
                //int[] defenders =   { 39939, 40032, 40139, }; // +Parry +Stam

				//Blue
				int[] solid =       { 39919, 40008, 40119, 36767 }; // +Stam

				//Green
				int[] enduring =    { 39976, 40089, 40167,  }; // +Def +Stam

				//Yellow
				int[] thick =       { 39916, 40015, 40126, 42157 }; // +def
                //int[] gleaming =    { }; // +Crit.

				//Orange
				int[] stalwart =    { 39964, 40056, 40160 };
                //int[] accurate =    { }; // +Hit +Expertise
                //int[] deadly =      { }; // +agi +crit
                //int[] deft =        { }; // +Agi +Haste
                //int[] etched =      { }; // +hit +Str
                //int[] glimmering =  { }; // +parry +def
                //int[] glinting =    { }; // +Agi +Hit

				//Meta
				int austere = 41380;

                // TODO: Update template to handle new gems.
				return new List<GemmingTemplate>() {
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
						
				    new GemmingTemplate() { Model = "TankDK", Group = "Rare", //Max Defense
				        RedId = thick[1], YellowId = thick[1], BlueId = thick[1], PrismaticId = thick[1], MetaId = austere },
				    new GemmingTemplate() { Model = "TankDK", Group = "Rare", //Defense 
				        RedId = stalwart[1], YellowId = thick[1], BlueId = enduring[1], PrismaticId = thick[1], MetaId = austere },
				    new GemmingTemplate() { Model = "TankDK", Group = "Rare", //Max Dodge
				        RedId = subtle[1], YellowId = subtle[1], BlueId = subtle[1], PrismaticId = subtle[1], MetaId = austere },
				    new GemmingTemplate() { Model = "TankDK", Group = "Rare", //Dodge
				        RedId = subtle[1], YellowId = stalwart[1], BlueId = regal[1], PrismaticId = subtle[1], MetaId = austere },
				    new GemmingTemplate() { Model = "TankDK", Group = "Rare", //Max Stamina
				        RedId = solid[1], YellowId = solid[1], BlueId = solid[1], PrismaticId = solid[1], MetaId = austere },
				    new GemmingTemplate() { Model = "TankDK", Group = "Rare", //Stamina
				        RedId = regal[1], YellowId = enduring[1], BlueId = solid[1], PrismaticId = solid[1], MetaId = austere },

				    new GemmingTemplate() { Model = "TankDK", Group = "Epic", Enabled = true, //Max Defense
				        RedId = thick[2], YellowId = thick[2], BlueId = thick[2], PrismaticId = thick[2], MetaId = austere },
				    new GemmingTemplate() { Model = "TankDK", Group = "Epic", Enabled = true, //Defense 
				        RedId = stalwart[2], YellowId = thick[2], BlueId = enduring[2], PrismaticId = thick[2], MetaId = austere },
				    new GemmingTemplate() { Model = "TankDK", Group = "Epic", Enabled = true, //Max Dodge
				        RedId = subtle[2], YellowId = subtle[2], BlueId = subtle[2], PrismaticId = subtle[2], MetaId = austere },
				    new GemmingTemplate() { Model = "TankDK", Group = "Epic", Enabled = true, //Dodge
				        RedId = subtle[2], YellowId = stalwart[2], BlueId = regal[2], PrismaticId = subtle[2], MetaId = austere },
				    new GemmingTemplate() { Model = "TankDK", Group = "Epic", Enabled = true, //Max Stamina
				        RedId = solid[2], YellowId = solid[2], BlueId = solid[2], PrismaticId = solid[2], MetaId = austere },
				    new GemmingTemplate() { Model = "TankDK", Group = "Epic",Enabled = true,  //Stamina
				        RedId = regal[2], YellowId = enduring[2], BlueId = solid[2], PrismaticId = solid[2], MetaId = austere },

				    new GemmingTemplate() { Model = "TankDK", Group = "Jeweler", //Max Defense
				        RedId = thick[3], YellowId = thick[3], BlueId = thick[3], PrismaticId = thick[3], MetaId = austere },
				    new GemmingTemplate() { Model = "TankDK", Group = "Jeweler", //Max Dodge
				        RedId = subtle[3], YellowId = subtle[3], BlueId = subtle[3], PrismaticId = subtle[3], MetaId = austere },
				    new GemmingTemplate() { Model = "TankDK", Group = "Jeweler", //Max Stamina
				        RedId = solid[3], YellowId = solid[3], BlueId = solid[3], PrismaticId = solid[3], MetaId = austere },
				};
            }
        }

        private Dictionary<string, Color> _subPointNameColors = null;
        private Dictionary<string, Color> _subPointNameColors_SMT = new Dictionary<string, Color>();
        private Dictionary<string, Color> _subPointNameColors_Burst = new Dictionary<string, Color>();

        public override Dictionary<string, Color> SubPointNameColors
		{
			get
			{
				if (_subPointNameColors == null)
				{
                    return _subPointNameColors_SMT;
				}
				return _subPointNameColors;
			}
		}

        public CalculationsTankDK()
        {
            _subPointNameColors_SMT.Add("Survival", Color.FromArgb(255, 0, 0, 255));
            _subPointNameColors_SMT.Add("Mitigation", Color.FromArgb(255, 255, 0, 0));
            _subPointNameColors_SMT.Add("Threat", Color.FromArgb(255, 0, 255, 0));

            _subPointNameColors_Burst.Add("BurstTime", Color.FromArgb(255, 0, 0, 255));
            _subPointNameColors_Burst.Add("ReactionTime", Color.FromArgb(255, 255, 0, 0));

            _subPointNameColors = _subPointNameColors_SMT;
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
        public override string[] CharacterDisplayCalculationLabels {
            get {
                if (_characterDisplayCalculationLabels == null) {
                    List<string> labels = new List<string>(new string[] {
					    @"Summary:Survival Points*Survival Points represents the total raw damage 
(pre-Mitigation) you can take before dying. Unlike 
Mitigation Points, you should not attempt to maximize this, 
but rather get 'enough' of it, and then focus on Mitigation. 
'Enough' can vary greatly by fight and by your healers, but 
keeping it roughly even with Mitigation Points is a good 
way to maintain 'enough' as you progress. If you find that 
you are being killed by burst damage, focus on Survival Points.",
					    @"Summary:Mitigation Points*Mitigation Points represent the amount of damage you avoid, 
on average, through avoidance stats (Miss, Dodge, Parry) along 
with ways to improve survivablity, +heal or self healing, ability 
cooldowns.  It is directly relational to your Damage Taken. 
Ideally, you want to maximize Mitigation Points, while maintaining 
'enough' Survival Points (see Survival Points). If you find 
yourself dying due to healers running OOM, or being too busy 
healing you and letting other raid members die, then focus on 
Mitigation Points.",
					    @"Summary:Threat Points*Threat Points represent how much threat is capable for the current 
gear/talent setup.  Threat points are represented in Threat per second.",
					    @"Summary:Overall Points*Overall Points are a sum of Mitigation, Survival and Threat Points. 
Overall is typically, but not always, the best way to rate gear. 
For specific encounters, closer attention to Mitigation and Survival 
Points individually may be important.",

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
                        "Basic Stats:Health*Including Frost Presence",
                        "Basic Stats:Armor*Including Frost Presence",

                        @"Defense:Crit*Enemy's crit chance on you. When using the optimizer, set a secondary 
criteria to this <= 0 to ensure that you stay defense-soft capped.",
                        "Defense:Defense Rating",
                        "Defense:Defense",
                        "Defense:Resilience",
                        "Defense:Defense Rating needed*Including Resilience to ensure being uncrittable.",

                        "Advanced Stats:Miss*After Diminishing Returns",
                        "Advanced Stats:Dodge*After Diminishing Returns",
                        "Advanced Stats:Parry*After Diminishing Returns. Includes Str bonus from Unbreakable Armor's average uptime.",
                        "Advanced Stats:Total Avoidance*Miss + Dodge + Parry",
                        "Advanced Stats:Armor Damage Reduction",
                        "Advanced Stats:Reaction Time",
                        "Advanced Stats:Burst Time",

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
        public override string[] CustomChartNames {
            get {
                if (_customChartNames == null) {
                    _customChartNames = new string[] { };
                }
                return _customChartNames;
            }
        }
		

#if RAWR3
        private ICalculationOptionsPanel _calculationOptionsPanel = null;
		public override ICalculationOptionsPanel CalculationOptionsPanel
		{
			get { return _calculationOptionsPanel ?? (_calculationOptionsPanel = new CalculationOptionsPanelTankDK()); }
		}
#else
        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
        /// <summary>
        /// A custom panel inheriting from CalculationOptionsPanelBase which contains controls for
        /// setting CalculationOptions for the model. CalculationOptions are stored in the Character,
        /// and can be used by multiple models. See comments on CalculationOptionsPanelBase for more details.
        /// </summary>
        public override CalculationOptionsPanelBase CalculationOptionsPanel {
            get { return _calculationOptionsPanel ?? (_calculationOptionsPanel = new CalculationOptionsPanelTankDK()); }
        }
#endif

        private List<ItemType> _relevantItemTypes = null;
        /// <summary>
        /// List<ItemType> containing all of the ItemTypes relevant to this model. Typically this
        /// means all types of armor/weapons that the intended class is able to use, but may also
        /// be trimmed down further if some aren't typically used. ItemType.None should almost
        /// always be included, because that type includes items with no proficiancy requirement, such
        /// as rings, necklaces, cloaks, held in off hand items, etc.

        /// </summary>
        public override List<ItemType> RelevantItemTypes {
            get {
                return _relevantItemTypes ?? (_relevantItemTypes = new List<ItemType>(new ItemType[]
					{
						ItemType.None,
                        // Pulling out Leather & Mail.
                        // While it may be fine for a DPS class to under gear for an instance, the armor value of 
                        // Plate really makes a difference for Tanks.
                        ItemType.Plate,
                        ItemType.Sigil,
                        ItemType.Polearm,
                        ItemType.TwoHandAxe,
                        ItemType.TwoHandMace,
                        ItemType.TwoHandSword,
                        ItemType.OneHandAxe,
                        ItemType.OneHandMace,
                        ItemType.OneHandSword
					}));
            }
        }
        /// <summary>
        /// Character class that this model is for.
        /// </summary>
        public override CharacterClass TargetClass { get { return CharacterClass.DeathKnight; } }
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
        public override string[] OptimizableCalculationLabels { 
            get {
                return new string[] {
                    "Chance to be Crit",
                    "Avoidance %",
                    "Damage Reduction %",
                    "% Chance to Hit",
                    "Target Parry %",
                    "Target Dodge %",
                    "Armor",
                    "Health",
                    "Reaction Time",
                    "Burst Time"
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
            // Since calcs is what we return at the end.  And the caller can't handle null value returns - 
            // Lets only return null if calcs is null, otherwise, let's return an empty calcs on other fails.
            CharacterCalculationsTankDK calcs = new CharacterCalculationsTankDK();
            if (null == calcs) { return null; }

            // Ok, this is the initial gathering of our information... we haven't processed the multipliers or anything.
            Stats stats = GetCharacterStats(character, additionalItem);
            // validate that we get a stats object;
            if (null == stats) { return calcs; }

            // Apply the Multipliers
            ProcessStatModifiers(stats, character.DeathKnightTalents.BladedArmor);

            // Import the option values from the options tab on the UI.
            CalculationOptionsTankDK opts = character.CalculationOptions as CalculationOptionsTankDK;
            // Validate opts 
            if (null == opts) { return calcs; }
            // Get the shotrotation/combat model here.
            if (opts.m_Rotation == null) { return calcs; }

            calcs.cType = opts.cType;
            // Level differences.
            int iTargetLevel = opts.TargetLevel;
            int iLevelDiff = iTargetLevel - character.Level;
            float fLevelDiffModifier = iLevelDiff * 0.2f;

            // Apply the ratings to actual stats.
            ProcessRatings(stats);

            // Need to save off the base avoidance stats before having their ratings applied to them.
            float fBaseDodge = stats.Dodge;
            float fBaseParry = stats.Parry;
            float fBaseDef = stats.Defense;
            float fBaseMiss = stats.Miss;

            ProcessAvoidance(stats, iTargetLevel);

            #endregion

            float fChanceToGetHit = 1f - (stats.Miss + stats.Dodge + stats.Parry);

            #region TargetDodge/Parry/Miss & Expertise - finish populating totalstats.
            bool bDualWielding = false;
            float f2hWeaponDamageMultiplier = 0f;
            float hitChance = 0;
            float chanceTargetParry = StatConversion.WHITE_PARRY_CHANCE_CAP[iLevelDiff];
            float chanceTargetDodge = StatConversion.WHITE_DODGE_CHANCE_CAP[iLevelDiff];
            float chanceTargetMiss = StatConversion.WHITE_MISS_CHANCE_CAP[iLevelDiff];
            if (character.MainHand != null) 
            {
                // 2-hander weapon specialization.
                if (character.MainHand.Slot == ItemSlot.TwoHand)
                {
                    f2hWeaponDamageMultiplier = (0.02f * character.DeathKnightTalents.TwoHandedWeaponSpecialization);
                }
                else
                {
                    // Toon is not using a 2h, meaning that he's DW if he's got something in his off hand.
                    bDualWielding = (character.OffHand != null && character.MainHand != null);
                }
                // 8% default miss rate vs lvl 83
                chanceTargetMiss = Math.Max(0f, StatConversion.WHITE_MISS_CHANCE_CAP[iLevelDiff] - stats.PhysicalHit);
                if (bDualWielding) 
                {
                    // Talent: Nerves of Cold Steel
                    // +hit changes only.  See damage buff change further down.
                    chanceTargetMiss = (StatConversion.WHITE_MISS_CHANCE_CAP_DW[iLevelDiff]
                                     - (0.01f * character.DeathKnightTalents.NervesOfColdSteel) - stats.PhysicalHit);
                }
                if (character.Race == CharacterRace.Dwarf &&
                    (character.MainHand.Type == ItemType.TwoHandMace || character.MainHand.Type == ItemType.OneHandMace)) 
                {
                    stats.Expertise += 5;
                }
                if (character.Race == CharacterRace.Human &&
                    (character.MainHand.Type == ItemType.TwoHandMace || character.MainHand.Type == ItemType.OneHandMace ||
                    character.MainHand.Type == ItemType.TwoHandSword || character.MainHand.Type == ItemType.OneHandSword)) 
                {
                    stats.Expertise += 3;
                }
                if (character.Race == CharacterRace.Orc &&
                    (character.MainHand.Type == ItemType.TwoHandAxe || character.MainHand.Type == ItemType.OneHandAxe)) 
                {
                    stats.Expertise += 5;
                }
            }

            chanceTargetParry = Math.Max(0.0f, StatConversion.WHITE_PARRY_CHANCE_CAP[iLevelDiff] - StatConversion.GetDodgeParryReducFromExpertise(stats.Expertise));
            chanceTargetDodge = Math.Max(0.0f, StatConversion.WHITE_DODGE_CHANCE_CAP[iLevelDiff] - StatConversion.GetDodgeParryReducFromExpertise(stats.Expertise));
            hitChance = 1.0f - (chanceTargetMiss + chanceTargetDodge + chanceTargetParry);
            // Can't have more than 100% hit chance.
            hitChance = Math.Min(1f, hitChance);
            
            #endregion

            // need to calculate the rotation after we have the DR values for Dodge/Parry/Miss/haste.
            opts.m_Rotation.m_fDodge = stats.Dodge;
            opts.m_Rotation.m_fParry = stats.Parry;
            opts.m_Rotation.m_fPhysicalHaste = stats.PhysicalHaste;

            if (character.DeathKnightTalents.SpellDeflection > 0)
            {
                Stats newStats = new Stats();
                newStats.SpellDamageTakenMultiplier -= 0.15f * character.DeathKnightTalents.SpellDeflection;
                SpecialEffect se = new SpecialEffect(Trigger.DamageSpellHit, newStats, 0f, 0f, stats.Parry);
                stats.AddSpecialEffect(se);
            }

            // This is the point that SHOULD have the right values according to the paper-doll.
            Stats sPaperDoll = stats.Clone();

            CombatTable ct = new CombatTable(character, calcs, stats, opts);

            // Now that we have the combat table, we should be able to integrate the Special effects.
            // However, the special effects will modify the incoming stats for all aspects, so we have 
            // ensure that as we iterate, we don't count whole sets of stats twice.

            #region Special Effects
            // For now we just factor them in once.
            StatsSpecialEffects sse = new StatsSpecialEffects(character, stats, ct);
            Stats statSE = new Stats();
            foreach (SpecialEffect e in stats.SpecialEffects())
            {
                statSE.Accumulate(sse.getSpecialEffects(opts, e));
            }
            // Any Modifiers from stats need to be applied to statSE
            statSE.Strength = StatConversion.ApplyMultiplier(statSE.Strength, stats.BonusStrengthMultiplier);
            statSE.Agility = StatConversion.ApplyMultiplier(statSE.Agility, stats.BonusAgilityMultiplier);
            statSE.Stamina = StatConversion.ApplyMultiplier(statSE.Stamina, stats.BonusStaminaMultiplier);
//            statSE.Stamina = (float)Math.Floor(statSE.Stamina);
            statSE.Armor = StatConversion.ApplyMultiplier(statSE.Armor, stats.BaseArmorMultiplier);
            statSE.AttackPower = StatConversion.ApplyMultiplier(statSE.AttackPower, stats.BonusAttackPowerMultiplier);
            statSE.BonusArmor = StatConversion.ApplyMultiplier(statSE.BonusArmor, stats.BonusArmorMultiplier);

            float AgiArmor = StatConversion.GetArmorFromAgility(statSE.Agility); // Don't multiply the armor from agility.
            statSE.Armor += statSE.BonusArmor + AgiArmor;
            statSE.Health += StatConversion.GetHealthFromStamina(statSE.Stamina);
            StatConversion.ApplyMultiplier(statSE.Health, stats.BonusHealthMultiplier);
            if (character.DeathKnightTalents.BladedArmor > 0)
            {
                statSE.AttackPower += (statSE.Armor / 180f) * (float)character.DeathKnightTalents.BladedArmor;
            }
            statSE.AttackPower += StatConversion.ApplyMultiplier((statSE.Strength * 2), stats.BonusAttackPowerMultiplier);
            statSE.ParryRating += statSE.Strength * 0.25f;
            statSE.CritRating += statSE.CritMeleeRating;

            // Any Modifiers from statSE need to be applied to stats
            stats.Strength = StatConversion.ApplyMultiplier(stats.Strength, statSE.BonusStrengthMultiplier);
            stats.Agility = StatConversion.ApplyMultiplier(stats.Agility, statSE.BonusAgilityMultiplier);
            stats.Stamina = StatConversion.ApplyMultiplier(stats.Stamina, statSE.BonusStaminaMultiplier);
//            stats.Stamina = (float)Math.Floor(stats.Stamina);
            stats.Armor = StatConversion.ApplyMultiplier(stats.Armor, statSE.BaseArmorMultiplier);
            stats.AttackPower = StatConversion.ApplyMultiplier(stats.AttackPower, statSE.BonusAttackPowerMultiplier);
            stats.BonusArmor = StatConversion.ApplyMultiplier(stats.BonusArmor, statSE.BonusArmorMultiplier);

            // Refresh the base avoidance values
            stats.Dodge = fBaseDodge;
            stats.Parry = fBaseParry;
            stats.Defense = fBaseDef;
            stats.Miss = fBaseMiss;

            stats.Accumulate(statSE);

            #endregion // Special effects 

            // refresh avoidance w/ the new stats.
            float[] fAvoidance = new float[(uint)HitResult.NUM_HitResult];
            for (uint i = 0; i < (uint)HitResult.NUM_HitResult; i++)
            {
                // GetDRAvoidanceChance returns a dec. percentage.
                // Since CurrentAvoidance is a percent, need to multiply by 100.
                fAvoidance[i] = (StatConversion.GetDRAvoidanceChance(character, stats, (HitResult)i, iTargetLevel));
            }

            // So let's populate the miss, dodge and parry values pulling them out of the avoidance number.
            fChanceToGetHit = 1f;
            stats.Miss = Math.Min((StatConversion.CAP_MISSED[(int)CharacterClass.DeathKnight]/100), Math.Max(0, fAvoidance[(int)HitResult.Miss]));
            fChanceToGetHit -= stats.Miss;
            // Dodge needs to be factored in here.
            stats.Dodge = Math.Min((StatConversion.CAP_DODGE[(int)CharacterClass.DeathKnight]/100), Math.Max(0, fAvoidance[(int)HitResult.Dodge]));
            fChanceToGetHit -= stats.Dodge;
            // Pary factors
            stats.Parry = Math.Min((StatConversion.CAP_PARRY[(int)CharacterClass.DeathKnight]/100), Math.Max(0, fAvoidance[(int)HitResult.Parry]));
            float fChanceToGetCrit = fAvoidance[(int)HitResult.Crit];
            // The next call expect Defense rating to NOT be factored into the defense stat
            calcs.DefenseRatingNeeded = StatConversion.GetDefenseRatingNeeded(character, stats, iTargetLevel);

            stats.Defense += StatConversion.GetDefenseFromRating(stats.DefenseRating, character.Class);

            // 5% + Level difference crit chance.
            // Level difference is already factored in above.
            fChanceToGetCrit = Math.Max(0.0f, (.05f - fChanceToGetCrit));

            // refresh Combat table w/ the new stats.
            ct = new CombatTable(character, calcs, stats, opts);

            #region Talents with general reach that aren't already in stats.
            // Talent: Bone Shield 
            // TODO: Change this to a Special Effect.
            float bsDR = 0.0f;
            float bsUptime = 0f;
            if (character.DeathKnightTalents.BoneShield > 0) {
                uint BSStacks = 3;  // The number of bones by default.
                if (character.DeathKnightTalents.GlyphofBoneShield == true) { BSStacks += 2; }

                float fBSCD = 60f;
                if (m_bT9_4PC) fBSCD -= 10f;

                bsUptime = Math.Min(1f,                         // Can't be up for longer than 100% of the time. 
                            (BSStacks * 2f)                   // 2 sec internal cooldown on loosing bones so the DK can't get spammed to death. 
                            / (1 - fChanceToGetHit)   // Loose a bone every time we get hit.
                            / fBSCD);                          // 60 sec cooldown.
                // 20% damage reduction while active.
                bsDR = 0.2f * bsUptime;
            }
            stats.DamageTakenMultiplier -= bsDR;
            #endregion

            // Assuming the Boss has no ArPen
            // From http://www.skeletonjack.com/2009/05/14/dk-tanking-armor-cap/#comments
            // 75% armor cap.  Not sure if this is for DK or for all Tanks.  So I'm just going to handle it here.
            // I'll do more research and see if it needs to go into the general function.
            float ArmorDamageReduction = (float)Math.Min(0.75f, StatConversion.GetArmorDamageReduction(iTargetLevel, stats.Armor, 0f, 0f, 0f));

            #region ***** Survival Rating *****
            // For right now Survival Rating == Effective Health will be HP + Armor/Resistance mitigation values.
            // Everything else is really mitigating damage based on RNG.

            // The health bonus from Frost presence is now include in the character by default.
            float hp = stats.Health;
            float fPhysicalSurvival = hp;
            float fMagicalSurvival = hp;

            // Physical damage:
            // So need the percent that is NOT from magic.
            fPhysicalSurvival = hp / (1f - ArmorDamageReduction);

            // Magical damage:
            // if there is a max resistance, then it's likely they are stacking for that resistance.  So factor in that Max resistance.
            float fMaxResist = Math.Max(stats.ArcaneResistance, stats.FireResistance);
            fMaxResist = Math.Max(fMaxResist, stats.FrostResistance);
            fMaxResist = Math.Max(fMaxResist, stats.NatureResistance);
            fMaxResist = Math.Max(fMaxResist, stats.ShadowResistance);

            fMagicalSurvival = hp / (1f - StatConversion.GetAverageResistance(iTargetLevel, character.Level, fMaxResist, 0f));

            float fEffectiveHealth = (fPhysicalSurvival * (1f - opts.PercentIncomingFromMagic)) + (fMagicalSurvival * opts.PercentIncomingFromMagic);
            // EffHealth is used further down for Burst/Reaction Times.
            calcs.Survival = fEffectiveHealth;
            calcs.SurvivalWeight = opts.SurvivalWeight;
            #endregion

            #region ***** THREAT *****
            float fRotDuration = ct.calcOpts.m_Rotation.getRotationDuration();
            float fThreatTotal = 0f;
            float fThreatPS = 0f;

            fThreatTotal = ct.GetTotalThreat();
            fThreatPS = fThreatTotal / fRotDuration;

            calcs.Threat = fThreatPS;
            // Improved Blood Presence
            if (character.DeathKnightTalents.ImprovedBloodPresence > 0)
            {
                float fDamageDone = fThreatPS / 2.035f; // reducing the TPS by the multiplier for Frost presence for basic DPS number - not the most accurate, but it gets us closer.
                // FullCharacterStats.HealingReceivedMultiplier += 0.5f * character.DeathKnightTalents.ImprovedBloodPresence;
                stats.Healed += (fDamageDone * 0.02f * character.DeathKnightTalents.ImprovedBloodPresence);
            }


            // Threat buffs.
            calcs.Threat *= 1f + (stats.ThreatIncreaseMultiplier - stats.ThreatReductionMultiplier);
            calcs.ThreatWeight = opts.ThreatWeight;
            #endregion

            #region ***** Mitigation Rating *****
            float fFightDuration = opts.FightLength;
            if (fFightDuration == 0f) 
            { 
                opts.FightLength = fFightDuration = 10f;
            }
            float fNumRotations = 0f;
            float fIncMagicalDamage = (opts.IncomingDamage * opts.PercentIncomingFromMagic);
            float fIncPhysicalDamage = (opts.IncomingDamage - fIncMagicalDamage);
            // How much damage per shot normal shot?
            float fPerShotPhysical = fIncPhysicalDamage;
            // How many shots over the length of the fight?
            float fBossAverageAttackSpeed = opts.BossAttackSpeed * (1f + .02f * character.DeathKnightTalents.ImprovedIcyTouch);
            // Factor in attack speed negative haste caused by talents.

            float fTotalBossAttacksPerFight = (fFightDuration * 60f) / fBossAverageAttackSpeed;
            // Integrate Expertise values to prevent additional physical damage coming in:
            // Each parry reducing swing timer by up to 40% so we'll average that damage increase out.
            // Each parry is factored by weapon speed - the faster the weapons, the more likely the boss can parry.
            // Figure out how many shots there are.  Right now, just calculating white damage.
            // How fast is a hasted shot? up to 40% faster.
            // average based on parry haste being equal to Math.Min(Math.Max(timeRemaining-0.4,0.2),timeRemaining)
            float fBossParryHastedSpeed = fBossAverageAttackSpeed * (1f - 0.24f);
            float fShotsParried = 0f;
            float fBossShotCountPerRot = 0f;
            if (fRotDuration > 0) {
                fNumRotations = (fFightDuration * 60f) / fRotDuration;
                // How many shots does the boss take over a given rotation period.
                fBossShotCountPerRot = fRotDuration / fBossAverageAttackSpeed;
                float fCharacterShotCount = 0f;
                if (character.MainHand != null && ct.MH.hastedSpeed > 0f) 
                { 
                    fCharacterShotCount += (fRotDuration / ct.MH.hastedSpeed); 
                }
                if (character.OffHand  != null && ct.OH.hastedSpeed > 0f) 
                { 
                    fCharacterShotCount += (fRotDuration / ct.OH.hastedSpeed); 
                }
                fCharacterShotCount += ct.totalParryableAbilities;
                // The number of shots taken * the chance to be parried.
                // Ensure that this value doesn't go over 100%
                fShotsParried = Math.Min(1f, chanceTargetParry) * fCharacterShotCount;
                // How many shots parried * how fast that is.  is what % of the total GCD we're talking about.
                float fTimeHasted = fShotsParried * fBossParryHastedSpeed;
                float fTimeNormal = fRotDuration - fTimeHasted;
                // Update the shot count w/ the new # of normal shots + the number of hasted shots.
                fBossShotCountPerRot = (fTimeNormal / fBossAverageAttackSpeed) + fShotsParried;
                // Update the total number of attacks if we have rotation data to factor in expertise parry-hasting.
                fTotalBossAttacksPerFight = fBossShotCountPerRot * fNumRotations;
                fBossAverageAttackSpeed = fRotDuration / fBossShotCountPerRot;
            }

            // Mark of blood
            // Cast on the enemy
            // buff that lasts 20 secs or 20 hits
            // heals the target for 4% of max health for each damage dealing hit from that enemy to the target of that enemy.
            // 3 Min CD.
            if (character.DeathKnightTalents.MarkOfBlood > 0)
            {
                // Now that we have the Avg. Boss Attack speed, let's figure how many attacks in 20 secs.
                float AttacksFor20 = Math.Min(20f, 20f / fBossAverageAttackSpeed);
                float MOBhealing = stats.Health * .04f * (AttacksFor20 * fChanceToGetHit); // how many attacks get through avoidance.
                stats.Healed += MOBhealing; // Fire it off every time we can and at least once per fight.
            }

            #region Anti-Magic Shell
            // Anti-Magic Shell. ////////////////////////////////////////////////////////
            // Talent: MagicSuppression increases AMS by 8/16/25% per point.
            // Glyph: GlyphofAntiMagicShell increases AMS by 2 sec.
            // AMS has a 45 sec CD.
            float amsUptime = (5f + (character.DeathKnightTalents.GlyphofAntiMagicShell == true ? 2f : 0f)) / 45f;
            // AMS reduces damage taken by 75% up to a max of 50% health.
            float amsReduction = 0.75f * (1f + character.DeathKnightTalents.MagicSuppression * 0.08f + (character.DeathKnightTalents.MagicSuppression == 3 ? 0.01f : 0f));
            float amsReductionMax = stats.Health * 0.5f;
            // up to 50% of health means that the amdDRvalue equates to the raw damage points removed.  
            // This means that toon health and INC damage values from the options pane are going to affect this quite a bit.
            float amsDRvalue = (Math.Min(amsReductionMax, fIncMagicalDamage * amsReduction) * amsUptime);
            #endregion 

            // For any physical only damage reductions. 
            // Adjust the damage by chance of crit getting through
            fPerShotPhysical += (fPerShotPhysical * fChanceToGetCrit) * 2f;
            // Factor in armor Damage Reduction
            fPerShotPhysical *= (1f - ArmorDamageReduction);

            // Four T8 : AMS grants 10% damage reductions.
            stats.DamageTakenMultiplier -= (stats.BonusAntiMagicShellDamageReduction * amsUptime); 

            // Factor in the AMS damage reduction value 
            fIncMagicalDamage -= amsDRvalue;
            fIncMagicalDamage = StatConversion.ApplyMultiplier(fIncMagicalDamage, stats.SpellDamageTakenMultiplier);

            fIncMagicalDamage = StatConversion.ApplyMultiplier(fIncMagicalDamage, stats.DamageTakenMultiplier);
            fPerShotPhysical = StatConversion.ApplyMultiplier(fPerShotPhysical, stats.DamageTakenMultiplier);

            // Since IncMagical was MagicalDPS - now distribute the damage over the whole fight.
            fIncMagicalDamage = ((fIncMagicalDamage * fChanceToGetCrit) * 2f) * (fFightDuration * 60f);

            // There's still a matter of a muting of the importance of getting Crit Immune give the above functions.
            // We have a problem where the tank could get gibbed if the mitigation fails, and the boss crits.
            if (fChanceToGetCrit > 0)
            {
                // This means that the likelihood of getting gibbed is assured.
                if ((fTotalBossAttacksPerFight * fChanceToGetCrit) > 1)
                {
//                    fIncPhysicalDamage *= 2f;
                }
            }

            // Let's make sure we don't go into negative damage here.
            fIncMagicalDamage = Math.Max(0f, fIncMagicalDamage);
            fPerShotPhysical = Math.Max(0f, fPerShotPhysical);


            // Get the raw per-swing Reaction & Burst Time
            float fAvoidanceTotal = stats.Dodge + stats.Miss;
            // If the character has no weapon, his Parry chance == 0
            if (character.MainHand != null || character.OffHand != null) { fAvoidanceTotal += stats.Parry; }

            // The next 2 returns are in swing count.
            float fReactionSwingCount = GetReactionTime(fAvoidanceTotal);
            float fBurstSwingCount = GetBurstTime(fAvoidanceTotal, fEffectiveHealth, fIncPhysicalDamage);

            // Get how long that actually will be on Average.
            calcs.ReactionTime = fReactionSwingCount * fBossAverageAttackSpeed;
            calcs.BurstTime = fBurstSwingCount * fBossAverageAttackSpeed;

            //calcs.Mitigation = (opts.IncomingDamage * 60f * fFightDuration) - (fIncMagicalDamage + fIncPhysicalDamage);
            //calcs.Mitigation = calcs.Mitigation / (60f * fFightDuration);

            // Total damage avoided between bursts.
            float fBurstDamage = fBurstSwingCount * fPerShotPhysical;
            float fBurstDPS = fBurstDamage / fBossAverageAttackSpeed;
            float fReactionDamage = fReactionSwingCount * fPerShotPhysical;

            // Mitigation is the difference between what damage would have been before and what it is once you factor in mitigation effects.
            calcs.Mitigation = fReactionSwingCount * fBossAverageAttackSpeed * (fIncPhysicalDamage - fPerShotPhysical);
            calcs.Mitigation += StatConversion.ApplyMultiplier(stats.Healed, stats.HealingReceivedMultiplier);
            #endregion

            #region Key Data Validation
            if (float.IsNaN(calcs.Threat) ||
                float.IsNaN(calcs.Survival) ||
                float.IsNaN(calcs.Mitigation) ||
                float.IsNaN(calcs.BurstTime) ||
                float.IsNaN(calcs.ReactionTime) ||
                float.IsNaN(calcs.OverallPoints) )
            {
#if DEBUG
                throw new Exception("One of the Subpoints are Invalid.");
#endif
            }
            #endregion

            #region Display only work
//            if (needsDisplayCalculations)
//            {
                // TODO: not sure if I want this here...
                calcs.BasicStats = sPaperDoll;
                // The full character data.
                calcs.TargetLevel = iTargetLevel;

                calcs.Miss = stats.Miss * 100f;
                calcs.Dodge = stats.Dodge * 100f;
                calcs.Parry = stats.Parry * 100f;
                calcs.Crit = fChanceToGetCrit * 100f;

                calcs.DefenseRating = stats.DefenseRating;
                calcs.Defense = stats.Defense;
                calcs.Resilience = stats.Resilience;

                calcs.TargetDodge = chanceTargetDodge;
                calcs.TargetMiss = chanceTargetMiss;
                calcs.TargetParry = chanceTargetParry;
                calcs.Expertise = stats.Expertise;
//                calcs.BasicStats.ArmorPenetration = StatConversion.GetArmorPenetrationFromRating(sPaperDoll.ArmorPenetrationRating) * 100f;

                calcs.ArmorDamageReduction = ArmorDamageReduction;
//            }
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
        /// <param name="addition
        /// alItem">An additional item to treat the character as wearing.
        /// This is used for gems, which don't have a slot on the character to fit in, so are just
        /// added onto the character, in order to get gem calculations.</param>
        /// <returns>A Stats object containing the final totaled values of all character stats.</returns>
        public override Stats GetCharacterStats(Character character, Item additionalItem) {

            Stats statsTotal = new Stats();

            // Validate that character.CalculationOptions != NULL
            if (null == character.CalculationOptions) {
                // Possibly put some error text here.
                return statsTotal;
            }
            CalculationOptionsTankDK calcOpts = character.CalculationOptions as CalculationOptionsTankDK;
            
            // Start populating data w/ Basic racial & class baseline.
            statsTotal = BaseStats.GetBaseStats(character);
            statsTotal.BaseAgility = BaseStats.GetBaseStats(character).Agility;

            if (statsTotal.Defense < 400f)
                // Adding in the base 400 Defense skill all tanks are expected to have.  
                // There are too many places where this just kinda stuck in.  It should be attached to the toon.
                statsTotal.Defense = 400f;
            AccumulateItemStats(statsTotal, character, additionalItem);
            AccumulateBuffsStats(statsTotal, character.ActiveBuffs); // includes set bonuses.
            // Except the 4 piece T9 - improves CD of VB, UA, and BS by 10 sec.  That has to get handled elsewhere.
            if (character.ActiveBuffsContains("Thassarian's Plate 4 Piece Bonus") ||
                character.ActiveBuffsContains("Koltira's Plate 4 Piece Bonus"))
            {
                // Set the character as having the T9_4pc bonus
                m_bT9_4PC = true;
            }
            AccumulateFrostPresence(statsTotal);
            
            // Stack only the info we care about.
            statsTotal = GetRelevantStats(statsTotal);
            AccumulateTalents(statsTotal, character);

            /* At this point, we're combined all the data from gear and talents and all that happy jazz.
             * However, we haven't applied any special effects nor have we applied any multipliers.
             * Also many special effects are now getting dependant upon combat info (rotations).
             */ 

            return (statsTotal);
        }

        private void ProcessStatModifiers( Stats statsTotal, int iBladedArmor )
        {
            statsTotal.Strength = StatConversion.ApplyMultiplier(statsTotal.Strength, statsTotal.BonusStrengthMultiplier);
            statsTotal.Agility = StatConversion.ApplyMultiplier(statsTotal.Agility, statsTotal.BonusAgilityMultiplier);
            // The stamina value is floor in game for the calculation
            statsTotal.Stamina = StatConversion.ApplyMultiplier(statsTotal.Stamina, statsTotal.BonusStaminaMultiplier);
            statsTotal.Stamina = (float)Math.Floor(statsTotal.Stamina);
            statsTotal.Armor = StatConversion.ApplyMultiplier(statsTotal.Armor, statsTotal.BaseArmorMultiplier);
            statsTotal.AttackPower = StatConversion.ApplyMultiplier(statsTotal.AttackPower, statsTotal.BonusAttackPowerMultiplier);
            statsTotal.BonusArmor = StatConversion.ApplyMultiplier(statsTotal.BonusArmor, statsTotal.BonusArmorMultiplier);

            float AgiArmor = StatConversion.GetArmorFromAgility(statsTotal.Agility); // Don't multiply the armor from agility.
            statsTotal.Armor += statsTotal.BonusArmor + AgiArmor;
            statsTotal.Health += StatConversion.GetHealthFromStamina(statsTotal.Stamina);

            StatConversion.ApplyMultiplier(statsTotal.Health, statsTotal.BonusHealthMultiplier);

            // Talent: BladedArmor //////////////////////////////////////////////////////////////
            if (iBladedArmor > 0)
            {
                statsTotal.AttackPower += (statsTotal.Armor / 180f) * (float)iBladedArmor;
            }
            // AP, crit, etc.  already being factored in w/ multiplier.
            statsTotal.AttackPower += StatConversion.ApplyMultiplier((statsTotal.Strength * 2), statsTotal.BonusAttackPowerMultiplier);
            statsTotal.CritRating += statsTotal.CritMeleeRating;

            // Parry from str. is only available to DKs.
            statsTotal.ParryRating += statsTotal.Strength * 0.25f;

        }

        /// <summary>
        /// Process All the ratings score to their base values.
        /// </summary>
        /// <param name="s"></param>
        private void ProcessRatings(Stats statsTotal)
        {
            statsTotal.PhysicalCrit = StatConversion.ApplyMultiplier(statsTotal.PhysicalCrit
                                        + StatConversion.GetCritFromAgility(statsTotal.Agility, CharacterClass.DeathKnight)
                                        + StatConversion.GetCritFromRating(statsTotal.CritRating), statsTotal.BonusCritMultiplier);
            statsTotal.SpellCrit = StatConversion.ApplyMultiplier(statsTotal.SpellCrit
                                        + StatConversion.GetCritFromRating(statsTotal.CritRating), statsTotal.BonusSpellCritMultiplier);

            statsTotal.PhysicalHit += StatConversion.GetHitFromRating(statsTotal.HitRating, CharacterClass.DeathKnight);
            statsTotal.SpellHit += StatConversion.GetSpellHitFromRating(statsTotal.HitRating);

            // Expertise Rating -> Expertise:
            statsTotal.Expertise += StatConversion.GetExpertiseFromRating(statsTotal.ExpertiseRating);

            statsTotal.ArmorPenetration += StatConversion.GetArmorPenetrationFromRating(statsTotal.ArmorPenetrationRating);

        }

        private void ProcessAvoidance(Stats statsTotal, int iTargetLevel)
        {
            // Get all the character avoidance numbers including deminishing returns.
            // Iterate through each hit type. and use fAvoidance array w/ the hitresult enum.
            float[] fAvoidance = new float[(uint)HitResult.NUM_HitResult];
            Character c = new Character();
            c.Class = CharacterClass.DeathKnight;
            for (uint i = 0; i < (uint)HitResult.NUM_HitResult; i++)
            {
                // GetDRAvoidanceChance returns a dec. percentage.
                // Since CurrentAvoidance is a percent, need to multiply by 100.
                fAvoidance[i] = (StatConversion.GetDRAvoidanceChance(c, statsTotal, (HitResult)i, iTargetLevel));
            }

            // So let's populate the miss, dodge and parry values for the UI display as well as pulling them out of the avoidance number.
            statsTotal.Miss = Math.Min((StatConversion.CAP_MISSED[(int)CharacterClass.DeathKnight]/100), Math.Max(0, fAvoidance[(int)HitResult.Miss]));
            // Dodge needs to be factored in here.
            statsTotal.Dodge = Math.Min((StatConversion.CAP_DODGE[(int)CharacterClass.DeathKnight]/100), Math.Max(0, fAvoidance[(int)HitResult.Dodge]));
            // Pary factors
            statsTotal.Parry = Math.Min((StatConversion.CAP_PARRY[(int)CharacterClass.DeathKnight]/100), Math.Max(0, fAvoidance[(int)HitResult.Parry]));
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
  
        /// <summary>
        /// Filters a Stats object to just the stats relevant to the model.
        /// </summary>
        /// <param name="stats">A complete Stats object containing all stats.</param>
        /// <returns>A filtered Stats object containing only the stats relevant to the model.</returns>
        public override Stats GetRelevantStats(Stats stats) {
            Stats s = new Stats() {
                Strength = stats.Strength,
                Agility = stats.Agility,
                BaseAgility = stats.BaseAgility,
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
                ArmorPenetrationRating = stats.ArmorPenetrationRating,
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
                BonusSpellCritMultiplier = stats.BonusSpellCritMultiplier,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
                BonusPhysicalDamageMultiplier = stats.BonusPhysicalDamageMultiplier,
                BaseArmorMultiplier = stats.BaseArmorMultiplier,
                BonusArmorMultiplier = stats.BonusArmorMultiplier,
                DamageTakenMultiplier = stats.DamageTakenMultiplier,
 
                // Defect 13301: Integrate 2% Threat increase for Armsmen enchant.
                ThreatIncreaseMultiplier = stats.ThreatIncreaseMultiplier,
                ThreatReductionMultiplier = stats.ThreatReductionMultiplier,

                CritMeleeRating = stats.CritMeleeRating,
                Bloodlust = stats.Bloodlust,

                // Bringing in some of the relavent stats from DPSDK.
                // General Damage Mods.
                BonusShadowDamageMultiplier = stats.BonusShadowDamageMultiplier,
                BonusFrostDamageMultiplier = stats.BonusFrostDamageMultiplier,
                BonusDiseaseDamageMultiplier = stats.BonusDiseaseDamageMultiplier,
    			BonusRuneStrikeMultiplier = stats.BonusRuneStrikeMultiplier,

                // Ability mods.
                BonusBloodStrikeDamage = stats.BonusBloodStrikeDamage,
                BonusDeathCoilDamage = stats.BonusDeathCoilDamage,
                BonusDeathStrikeDamage = stats.BonusDeathStrikeDamage,
                BonusFrostStrikeDamage = stats.BonusFrostStrikeDamage,
                BonusHeartStrikeDamage = stats.BonusHeartStrikeDamage,
                BonusIcyTouchDamage = stats.BonusIcyTouchDamage,
                BonusObliterateDamage = stats.BonusObliterateDamage,
                BonusScourgeStrikeDamage = stats.BonusScourgeStrikeDamage,
			    BonusHowlingBlastDamage = stats.BonusHowlingBlastDamage,
			    BonusFrostWeaponDamage = stats.BonusFrostWeaponDamage,

                BonusPerDiseaseBloodStrikeDamage = stats.BonusPerDiseaseBloodStrikeDamage,
                BonusPerDiseaseHeartStrikeDamage = stats.BonusPerDiseaseHeartStrikeDamage,
                BonusPerDiseaseObliterateDamage = stats.BonusPerDiseaseObliterateDamage,
                BonusPerDiseaseScourgeStrikeDamage = stats.BonusPerDiseaseScourgeStrikeDamage,
			
                BonusDeathCoilCrit = stats.BonusDeathCoilCrit,
                BonusDeathStrikeCrit = stats.BonusDeathStrikeCrit,
                BonusFrostStrikeCrit = stats.BonusFrostStrikeCrit,
                BonusObliterateCrit = stats.BonusObliterateCrit,
                BonusPlagueStrikeCrit = stats.BonusPlagueStrikeCrit,
    			BonusScourgeStrikeCrit = stats.BonusScourgeStrikeCrit,

                BonusIceboundFortitudeDuration = stats.BonusIceboundFortitudeDuration,
                BonusAntiMagicShellDamageReduction = stats.BonusAntiMagicShellDamageReduction,

			    BonusHealingReceived = stats.BonusHealingReceived,
                RPp5 = stats.RPp5,
                BonusMaxRunicPower = stats.BonusMaxRunicPower,

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
            foreach (SpecialEffect effect in stats.SpecialEffects()) {
                if (HasRelevantStats(effect.Stats)) {
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
        public override bool HasRelevantStats(Stats stats) {
            foreach (SpecialEffect effect in stats.SpecialEffects()) {
                if (relevantStats(effect.Stats)) {
                    if (effect.Trigger == Trigger.DamageDone ||
                        effect.Trigger == Trigger.DamageTaken ||
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
                        effect.Trigger == Trigger.Use)
                    {
                        return relevantStats(effect.Stats);
                    }
                    // if it has a DK specific trigger, then just return true.
                    else if (
                        effect.Trigger == Trigger.BloodStrikeHit ||
                        effect.Trigger == Trigger.HeartStrikeHit ||
                        effect.Trigger == Trigger.BloodStrikeOrHeartStrikeHit ||
                        effect.Trigger == Trigger.IcyTouchHit ||
                        effect.Trigger == Trigger.PlagueStrikeHit ||
                        effect.Trigger == Trigger.RuneStrikeHit ||
                        effect.Trigger == Trigger.ObliterateHit ||
                        effect.Trigger == Trigger.ScourgeStrikeHit)
                    {
                        return true;
                    }

                }
            }
            return relevantStats(stats);
        }

        /// <summary>
        /// Helper function for HasRelevantStats() function of the base class.
        /// </summary>
        /// <param name="stats"></param>
        /// <returns>true == the stats object has interesting things for this model.</returns>
        private bool relevantStats(Stats stats) {
            bool bResults = false;
            // Core stats
            bResults |= (stats.Strength != 0);
            bResults |= (stats.Agility != 0);
            bResults |= (stats.Stamina != 0);
            bResults |= (stats.Armor != 0);
            bResults |= (stats.BonusArmor != 0);
            bResults |= (stats.Health != 0);

            // Defense stats
            bResults |= (stats.DodgeRating != 0);
            bResults |= (stats.DefenseRating != 0);
            bResults |= (stats.ParryRating != 0);

            bResults |= (stats.Dodge != 0);
            bResults |= (stats.Parry != 0);
            bResults |= (stats.Miss != 0);
            bResults |= (stats.Defense != 0);

            bResults |= (stats.Resilience != 0);

            // Offense stats
            bResults |= (stats.AttackPower != 0);
            bResults |= (stats.HitRating != 0);
            bResults |= (stats.CritRating != 0);
            bResults |= (stats.ArmorPenetration != 0);
            bResults |= (stats.ArmorPenetrationRating != 0);
            bResults |= (stats.ExpertiseRating != 0);
            bResults |= (stats.Expertise != 0);
            bResults |= (stats.HasteRating != 0);
            bResults |= (stats.WeaponDamage != 0);
            bResults |= (stats.PhysicalCrit != 0);
            bResults |= (stats.PhysicalHaste != 0);
            bResults |= (stats.PhysicalHit != 0);
            bResults |= (stats.SpellHitRating != 0);
            bResults |= (stats.SpellHit != 0);

            bResults |= (stats.Healed != 0);

            // Bonus to stats
            bResults |= (stats.BonusArmorMultiplier != 0);
            bResults |= (stats.BaseArmorMultiplier != 0);
            bResults |= (stats.BonusHealthMultiplier != 0);
            bResults |= (stats.BonusStrengthMultiplier != 0);
            bResults |= (stats.BonusStaminaMultiplier != 0);
            bResults |= (stats.BonusAgilityMultiplier != 0);
            bResults |= (stats.BonusCritMultiplier != 0);
            bResults |= (stats.BonusSpellCritMultiplier != 0);
            bResults |= (stats.BonusAttackPowerMultiplier != 0);
            bResults |= (stats.BonusPhysicalDamageMultiplier != 0);
            bResults |= (stats.BonusDamageMultiplier != 0);
            bResults |= (stats.DamageTakenMultiplier != 0);
            bResults |= (stats.ThreatIncreaseMultiplier != 0);
            bResults |= (stats.ThreatReductionMultiplier != 0);

            bResults |= (stats.CritMeleeRating != 0);
            bResults |= (stats.Bloodlust != 0);

            // Bringing in the damage stuff from DPSDK for better threat data
            // Damage Multipliers:
            bResults |= (stats.BonusShadowDamageMultiplier != 0);
            bResults |= (stats.BonusFrostDamageMultiplier != 0);
            bResults |= (stats.BonusDiseaseDamageMultiplier != 0);
            bResults |= (stats.BonusRuneStrikeMultiplier != 0);

            // Bulk Damage:
            bResults |= (stats.BonusBloodStrikeDamage != 0);
            bResults |= (stats.BonusDeathCoilDamage != 0);
            bResults |= (stats.BonusDeathStrikeDamage != 0);
            bResults |= (stats.BonusFrostStrikeDamage != 0);
            bResults |= (stats.BonusHeartStrikeDamage != 0);
            bResults |= (stats.BonusIcyTouchDamage != 0);
            bResults |= (stats.BonusObliterateDamage != 0);
            bResults |= (stats.BonusScourgeStrikeDamage != 0);
            bResults |= (stats.BonusHowlingBlastDamage != 0);
            bResults |= (stats.BonusFrostWeaponDamage != 0);

            bResults |= (stats.BonusPerDiseaseBloodStrikeDamage != 0);
            bResults |= (stats.BonusPerDiseaseHeartStrikeDamage != 0);
            bResults |= (stats.BonusPerDiseaseObliterateDamage != 0);
            bResults |= (stats.BonusPerDiseaseScourgeStrikeDamage != 0);

            // Others
            bResults |= (stats.BonusDeathCoilCrit != 0);
            bResults |= (stats.BonusDeathStrikeCrit != 0);
            bResults |= (stats.BonusFrostStrikeCrit != 0);
            bResults |= (stats.BonusObliterateCrit != 0);
            bResults |= (stats.BonusPlagueStrikeCrit != 0);
            bResults |= (stats.BonusScourgeStrikeCrit != 0);
            bResults |= (stats.BonusIceboundFortitudeDuration != 0);
            bResults |= (stats.BonusAntiMagicShellDamageReduction != 0);
            bResults |= (stats.BonusHealingReceived != 0);
            bResults |= (stats.RPp5 != 0);
            bResults |= (stats.BonusMaxRunicPower != 0);

            // Resistances
            bResults |= (stats.ArcaneResistance != 0);
            bResults |= (stats.FireResistance != 0);
            bResults |= (stats.FrostResistance != 0);
            bResults |= (stats.NatureResistance != 0);
            bResults |= (stats.ShadowResistance != 0);

            return bResults;
        }

        public override bool IsItemRelevant(Item item) {
            if (item.Slot == ItemSlot.Ranged && item.Type != ItemType.Sigil) { return false; }
            return base.IsItemRelevant(item);
        }

        /// <summary>
        /// Pass in the total stats object we're working with and add specific stat modifiers.
        /// </summary>
        /// <param name="s"></param>
        private void AccumulateFrostPresence(Stats s)
        {
            s.BaseArmorMultiplier += .6f; // Bonus armor for Frost Presence down from 80% to 60% as of 3.1.3
            // Patch 3.2: Replace 10% Health w/ 6% Stamina
            s.BonusStaminaMultiplier += .06f; // Bonus 6% Stamina
//            FrostyStats.BonusHealthMultiplier += .1f; // Bonus 10% health for Frost Presence.
            s.DamageTakenMultiplier -= .08f;// Bonus of 8% damage reduced for frost presence. up from 5% for 3.2.2
            s.ThreatIncreaseMultiplier += .45f; // Bonus 45% threat for frost Presence.
        }

        /// <summary>Build the talent special effects.</summary>
        private void AccumulateTalents(Stats FullCharacterStats, Character character) 
        {
            Stats newStats = new Stats();
            float fDamageDone = 0f;
            // ok... I don't like that we have to do some evaluation at this point.
            // However, this health value is not passed forward, just gives an initial pass 
            // to evaluate the value of talents.
            float fHealth = StatConversion.ApplyMultiplier((FullCharacterStats.Health + StatConversion.GetHealthFromStamina(FullCharacterStats.Stamina)), FullCharacterStats.BonusHealthMultiplier);

            #region Blood Talents
            // Butchery
            // 1RPp5 per Point
            // TODO: Implement Runic Regen info.
            if (character.DeathKnightTalents.Butchery > 0) {
                FullCharacterStats.RPp5 += 1 * character.DeathKnightTalents.Butchery;
            }

            // Subversion
            // Increase crit 3% per point of BS, HS, Oblit
            // 3.2.2: also SS
            if (character.DeathKnightTalents.Subversion > 0) 
            { 
                // implmented in CombatTable.cs
            }

            // Blade Barrier
            // Reduce damage by 1% per point for 10 sec.
            if (character.DeathKnightTalents.BladeBarrier > 0) {
                // for now having the functionality that currently exists is better than nothing.
                // TODO: Implement SpecialEffect for Rune CD.
                FullCharacterStats.DamageTakenMultiplier -= (.01f * character.DeathKnightTalents.BladeBarrier);
            }

            // Bladed Armor
            // 1 AP per point per 180 Armor
            // Implmented after Frost Presence above.

            // Scent of Blood
            // 15% after Dodge, Parry or damage received causing 1 melee hit per point to generate 5 runic power.
            // TODO: setup RP gains.

            // 2H weapon spec.
            // 2% per point increased damage
            // Implmented in weapon section above.

            // Rune Tap
            // Convert 1 BR to 10% health.
            if (character.DeathKnightTalents.RuneTap > 0) 
            {
                newStats = new Stats();
                float fCD = 60f;
                newStats.Healed = (fHealth * .1f);
                if (character.DeathKnightTalents.ImprovedRuneTap > 0) 
                {
                    // Improved Rune Tap.
                    // increases the health provided by RT by 33% per point. and lowers the CD by 10 sec per point
                    fCD -= (10f * character.DeathKnightTalents.ImprovedRuneTap);
                    newStats.Healed += (newStats.Healed * (0.33f * character.DeathKnightTalents.ImprovedRuneTap));
                    FullCharacterStats.AddSpecialEffect(new SpecialEffect(Trigger.Use, newStats, 0, fCD));
                }
                FullCharacterStats.AddSpecialEffect(new SpecialEffect(Trigger.Use, newStats, 0, fCD));
            }

            // Dark Conviction 
            // Increase Crit w/ weapons, spells, and abilities by 1% per point.
            if (character.DeathKnightTalents.DarkConviction > 0) {
                FullCharacterStats.PhysicalCrit += (0.01f * character.DeathKnightTalents.DarkConviction);
                FullCharacterStats.SpellCrit += (0.01f * character.DeathKnightTalents.DarkConviction);
            }

            // Death Rune Mastery
            // Create death runes out of Frost & Unholy for each oblit.
            // TODO: Implement Death Runes in CombatTable/Ability/Rotation

            // Spell Deflection
            // Parry chance of taking 15% less damage per point from direct damage spell
            // Implmented after Parry calc above.

            // Vendetta
            // Heals you for up to 2% per point on killing blow
            // TODO: Not important for MT but may be useful in OTing?

            // Bloody Strikes
            // increases damage of BS and HS by 15% per point
            // increases damage of BB by 10% per point
            // Implemented in Combattable.cs

            // Veteran of the 3rd War
            // Patch 3.2 from 2% to 1% per point.
            // increases Str and Stam by 1% per point
            // increases expertise by 2 per point.
            if (character.DeathKnightTalents.VeteranOfTheThirdWar > 0)
            {
                FullCharacterStats.BonusStrengthMultiplier += (.02f * character.DeathKnightTalents.VeteranOfTheThirdWar);
                FullCharacterStats.BonusStaminaMultiplier += (.01f * character.DeathKnightTalents.VeteranOfTheThirdWar);
                FullCharacterStats.Expertise += (2f * character.DeathKnightTalents.VeteranOfTheThirdWar);
            }

            // Mark of blood
            // Cast on the enemy
            // buff that lasts 20 secs or 20 hits
            // heals the target for 4% of max health for each damage dealing hit from that enemy to the target of that enemy.
            // Implemented in Mitigation section above.

            // Bloody Vengence
            // 1% per point bonus to physical damage for 30 secs after a crit w/ up to 3 stacks.
            if (character.DeathKnightTalents.BloodyVengeance > 0) {
                newStats = new Stats();
                newStats.BonusPhysicalDamageMultiplier = .01f * character.DeathKnightTalents.BloodyVengeance;
                FullCharacterStats.AddSpecialEffect(new SpecialEffect(Trigger.DamageSpellCrit, newStats, 30, 0, 1, 3));
                FullCharacterStats.AddSpecialEffect(new SpecialEffect(Trigger.MeleeCrit, newStats, 30, 0, 1, 3));
            }

            // Abominations Might
            // BS & HS have 25% chance per point
            // DS and Oblit have 50% chance per point
            // increase AP by 10% of raid for 10 sec.
            // 1% per point increase to str.
            if (character.DeathKnightTalents.AbominationsMight > 0) {
                FullCharacterStats.BonusStrengthMultiplier += (0.01f * character.DeathKnightTalents.AbominationsMight);
                newStats = new Stats();
                newStats.BonusAttackPowerMultiplier = .1f;
                FullCharacterStats.AddSpecialEffect(new SpecialEffect(Trigger.BloodStrikeHit, newStats, 10, 0, .25f * character.DeathKnightTalents.AbominationsMight));
                FullCharacterStats.AddSpecialEffect(new SpecialEffect(Trigger.HeartStrikeHit, newStats, 10, 0, .25f * character.DeathKnightTalents.AbominationsMight));
                FullCharacterStats.AddSpecialEffect(new SpecialEffect(Trigger.ObliterateHit, newStats, 10, 0, .5f * character.DeathKnightTalents.AbominationsMight));
                FullCharacterStats.AddSpecialEffect(new SpecialEffect(Trigger.DeathStrikeHit, newStats, 10, 0, .5f * character.DeathKnightTalents.AbominationsMight));
            }

            // Bloodworms
            // 3% chance per point per hit to cause the target to spawn 2-4 blood worms
            // Healing you 150% of the damage they do for 20 sec.
            if (character.DeathKnightTalents.Bloodworms > 0) {
                newStats = new Stats();
                // TODO: figure out how much damage the worms do.
                fDamageDone = 100f;
                float fBWAttackSpeed = 2f;
                float fBWDuration = 20f;
                newStats.Healed = ((fDamageDone * fBWDuration / fBWAttackSpeed) * 1.5f);
                FullCharacterStats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalHit, newStats, fBWDuration, 0, .03f * character.DeathKnightTalents.Bloodworms));
            }

            // Hysteria
            // Killy frenzy for 30 sec.
            // Increase physical damage by 20%
            // take damage 1% of max every sec.
            if (character.DeathKnightTalents.Hysteria > 0) 
            {
                float fDur = 30f;
                newStats = new Stats();
                newStats.BonusPhysicalDamageMultiplier += 0.2f;
                newStats.Healed -= (fHealth * 0.01f * fDur);
                FullCharacterStats.AddSpecialEffect(new SpecialEffect(Trigger.Use, newStats, fDur, 3f * 60f));
            }

            // Improved Blood Presence
            // while in frost or unholy, you retain the 2% per point healing from blood presence
            // Healing done to you is increased by 5% per point
            // Implemented above.

            // Improved Death Strike
            // increase damage of DS by 15% per point 
            // increase crit chance of DS by 3% per point
            // Implemented in CombatTable.cs

            // Sudden Doom
            // BS & HS have a 5% per point chance to launch a DC at target
            if (character.DeathKnightTalents.SuddenDoom > 0) 
            { 
                // Implmented in CombatTable.cs
            }

            // Vampiric Blood
            // temp 15% of max health and
            // increases health generated by 35% for 10 sec.
            // 1 min CD. as of 3.2.2
            if (character.DeathKnightTalents.VampiricBlood > 0) {
                newStats = new Stats();
                // TODO: need to figure out how to factor this back in.
                newStats.Health = (fHealth * 0.15f);
                newStats.HealingReceivedMultiplier += 0.35f;

                float fVBCD = 60f;
                if (m_bT9_4PC) fVBCD -= 10f;
                float fVBDur = 10f;
                if (character.DeathKnightTalents.GlyphofVampiricBlood == true)
                {
                    fVBDur += 5f;
                }
                FullCharacterStats.AddSpecialEffect(new SpecialEffect(Trigger.Use, newStats, fVBDur, fVBCD));
            }

            // Will of the Necropolis
            // Damage that takes you below 35% health or while at less than 35% is reduced by 5% per point.  
            // CD 15 sec.
            // Incoming damage must be >= than 5% of total health.
            if (character.DeathKnightTalents.WillOfTheNecropolis > 0) {
                newStats = new Stats();
                newStats.DamageTakenMultiplier -= (0.05f * character.DeathKnightTalents.WillOfTheNecropolis);
                // Need to factor in the damage taken aspect of the trigger.
                // Using the assumption that the tank will be at < 35% health about that % of the time.
                FullCharacterStats.AddSpecialEffect(new SpecialEffect(Trigger.SpellHit, newStats, 0, 15f, 0.35f));
                FullCharacterStats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalHit, newStats, 0, 15f, 0.35f));
            }

            // Heart Strike
            // 3.2.2: Secondary targets of HS take 1/2 as much damage
            // Implemented in CombatTable.cs

            // Might of Mograine
            // increase crit damage of BB, BS, DS, and HS by 15% per point
            // Implemented in CombatTable.cs

            // Blood Gorged
            // when above 75% health, you deal 2% more damage per point
            // when above 75% health, you receive 2% Armor Pen
            if (character.DeathKnightTalents.BloodGorged > 0) {
                // Damage done increase has to be in shot rotation.
                // Assuming a 50% up time 
                FullCharacterStats.ArmorPenetration += (0.02f * character.DeathKnightTalents.BloodGorged * 0.5f);
            }

            // Dancing Rune Weapon
            // not impl
            #endregion

            #region Frost Talents
            // Improved Icy Touch
            // 5% per point additional IT damage
            // 2% per point target haste reduction 
            if (character.DeathKnightTalents.ImprovedIcyTouch > 0) {
                FullCharacterStats.BonusIcyTouchDamage += (0.05f * character.DeathKnightTalents.ImprovedIcyTouch);
                // TODO: Need to factor in the correct haste adjustment for target.
                // For now assuming a straight 2% damage reduction per point.

//                sReturn.DamageTakenMultiplier -= 0.02f * character.DeathKnightTalents.ImprovedIcyTouch;
            }

            // Runic Power Mastery
            // Increases Max RP by 15 per point
            if (character.DeathKnightTalents.RunicPowerMastery > 0) {
                FullCharacterStats.BonusMaxRunicPower += 5 * character.DeathKnightTalents.RunicPowerMastery;
            }

            // Toughness
            // Patch 3.2: Increases Armor Value from items by 2% per point.
            // Reducing duration of all slowing effects by 6% per point.  
            if (character.DeathKnightTalents.Toughness > 0)
            {
                FullCharacterStats.BaseArmorMultiplier += (.02f * character.DeathKnightTalents.Toughness); // Patch 3.2

            }

            // Icy Reach
            // Increases range of IT & CoI and HB by 5 yards per point.

            // Black Ice
            // Increase Frost & shadow damage by 2% per point
            if (character.DeathKnightTalents.BlackIce > 0) {
                FullCharacterStats.BonusFrostDamageMultiplier += 0.02f * character.DeathKnightTalents.BlackIce;
                FullCharacterStats.BonusShadowDamageMultiplier += 0.02f * character.DeathKnightTalents.BlackIce;
            }

            // Nerves of Cold Steel
            // Increase hit w/ 1H weapons by 1% per point
            // Increase damage done by off hand weapons by 5% per point
            // Implement in combat shot roation

            // Icy Talons
            // Increase melee attack speed by 4% per point for the next 20 sec.
            if (character.DeathKnightTalents.IcyTalons > 0) {
                newStats = new Stats();
                newStats.PhysicalHaste += (0.04f * character.DeathKnightTalents.IcyTalons);
                FullCharacterStats.AddSpecialEffect(new SpecialEffect(Trigger.IcyTouchHit, newStats, 20f, 0f));
            }

            // Lichborne
            // for 10 sec, immune to charm, fear, sleep
            // CD 2 Mins

            // Threat of Thassarian: 
            // New 3-point talent. When dual-wielding, your Death Strikes, Obliterates, Plague Strikes, 
            // Blood Strikes and Frost Strikes and Rune Strike (as of 3.2.2) have a 30/60/100% chance 
            // to also deal damage with your  off-hand weapon. Off-hand strikes are roughly one half the effect of the original strike. 

            // Annihilation
            // +1 % per point melee Crit chance 
            // 33% per point that oblit will not consume diseases
            if (character.DeathKnightTalents.Annihilation > 0) {
                FullCharacterStats.PhysicalCrit += (0.01f * character.DeathKnightTalents.Annihilation);
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
            if (character.DeathKnightTalents.FrigidDreadplate > 0) {
                FullCharacterStats.Miss += 0.01f * character.DeathKnightTalents.FrigidDreadplate;
            }

            // Glacier Rot
            // Diseased enemies take 7%, 13% , 20% more damage from IT, HB, FS.
            if (character.DeathKnightTalents.GlacierRot > 0) {
                float fBonus = 0f;
                switch (character.DeathKnightTalents.GlacierRot) {
                    case 1: fBonus = 0.07f; break;
                    case 2: fBonus = 0.13f; break;
                    case 3: fBonus = 0.20f; break;
                }
                FullCharacterStats.BonusIcyTouchDamage += fBonus;
                FullCharacterStats.BonusFrostStrikeDamage += fBonus;
                FullCharacterStats.BonusHowlingBlastDamage += fBonus;
            }

            // Deathchill
            // when active IT, HB, FS, Oblit will crit.

            // Improved Icy Talons
            // increases the melee hast of the group/raid by 20%
            // increases your haste by 5% all the time.
            if (character.DeathKnightTalents.ImprovedIcyTalons > 0) {
                FullCharacterStats.PhysicalHaste += 0.05f;
                // TODO: Factor in raid utility by improving raid haste by 20%
                newStats = new Stats();
                newStats.PhysicalHaste += (0.2f * character.DeathKnightTalents.IcyTalons);
                FullCharacterStats.AddSpecialEffect(new SpecialEffect(Trigger.IcyTouchHit, newStats, 20f, 0f));
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
            if (character.DeathKnightTalents.ImprovedFrostPresence > 0) {
                FullCharacterStats.DamageTakenMultiplier -= (0.01f * character.DeathKnightTalents.ImprovedFrostPresence);
            }

            // Blood of the North
            // Patch 3.2: BS & FS damage +3% per point
            // Patch 3.2: BS & Pest create DeathRune from Blood 33% per point.
            if (character.DeathKnightTalents.BloodOfTheNorth > 0)
            {
                float fBonus = 0f;
                switch (character.DeathKnightTalents.BloodOfTheNorth)
                {
                    case 1:
                        fBonus = 0.03f;
                        break;
                    case 2:
                        fBonus = 0.06f;
                        break;
                    case 3:
                        fBonus = 0.1f;
                        break;
                }
                FullCharacterStats.BonusFrostStrikeDamage += fBonus;
                FullCharacterStats.BonusBloodStrikeDamage += fBonus;
            }

            // Unbreakable Armor
            // Reinforces your armor with a thick coat of ice, Increasing Armor by 25% and increasing your Strength by 10% for 20 sec.
            if (character.DeathKnightTalents.UnbreakableArmor > 0) {
                newStats = new Stats();
                newStats.BonusStrengthMultiplier += 0.10f;
                newStats.BaseArmorMultiplier += .25f;
                newStats.BonusArmorMultiplier += .25f;
                if (character.DeathKnightTalents.GlyphofUnbreakableArmor)
                {
                    // As per wowhead:
                    // Effect: Apply Aura: Add % Modifier (3)
                    // Value: 20
                    newStats.BaseArmorMultiplier += .2f;
                    newStats.BonusArmorMultiplier += .2f;
                }
                float fUACD = 60f;
                if (m_bT9_4PC) fUACD -= 10f;
                FullCharacterStats.AddSpecialEffect(new SpecialEffect(Trigger.Use, newStats, 20f, fUACD));
            }

            // Acclimation
            // When hit by a spell, 10% chance per point to boost resistance to that type of magic for 18 sec.  
            // up to 3 stacks.
            if (character.DeathKnightTalents.Acclimation > 0) {
                newStats = new Stats();
                float chance = (.1f * character.DeathKnightTalents.Acclimation);
                newStats.FireResistance   += 50f;
                newStats.FrostResistance  += 50f;
                newStats.ArcaneResistance += 50f;
                newStats.ShadowResistance += 50f;
                newStats.NatureResistance += 50f;
                FullCharacterStats.AddSpecialEffect(new SpecialEffect(Trigger.SpellHit, newStats, 18f, 0f, chance, 3));
            }

            // Frost Strike

            // Guile of Gorefiend
            // Increases CritStrike Damage of BS, FS, HB, Oblit by 15% per point.
            // Increases Duration of IBF by 2 sec per point.
            // HACK: Implenting IceBound Fortitude. ////////////////////////////////////////////////////////
            // Implmenting IBF here because it's affected by GoGF
            // Four T7 increases IBF by 3 sec.
            // Patch 3.2: IBF has a 120 sec CD. 
            float fIBFDur = (12.0f + character.DeathKnightTalents.GuileOfGorefiend * 2.0f + FullCharacterStats.BonusIceboundFortitudeDuration);
            // IBF reduces damage taken by 20% + 3% for each 28 defense over 400.
            float ibfDefense = StatConversion.GetDefenseFromRating(FullCharacterStats.DefenseRating, character.Class);
            float ibfReduction = 0.2f + (ibfDefense * 0.03f / 28.0f);
            if (character.DeathKnightTalents.GlyphofIceboundFortitude) {
                // Glyphed to 30% + def value.
                ibfReduction += 0.1f;
                // Since 3.1 the glyph is capped to 30%.
            }
            // There has always been a cap on the IBF to 30% the glyph was nerfed, not IBF.
            // So it's not worth it for those who already have alot of DEF. (EG. most tanks)
            ibfReduction = Math.Min(0.3f, ibfReduction);
            newStats = new Stats();
            newStats.DamageTakenMultiplier -= ibfReduction;
            FullCharacterStats.AddSpecialEffect(new SpecialEffect(Trigger.Use, newStats, fIBFDur, 120)); // Patch 3.2

            // Tundra Stalker
            // Your spells & abilities deal 3% per point more damage to targets w/ FF
            // Increases Expertise by 1 per point
            if (character.DeathKnightTalents.TundraStalker > 0) {
                // Assuming FF is always up.
                FullCharacterStats.BonusDamageMultiplier += .03f * character.DeathKnightTalents.TundraStalker;
                FullCharacterStats.Expertise += 1f * character.DeathKnightTalents.TundraStalker;
            }

            // Howling Blast.

            #endregion

            #region UnHoly Talents
            // Vicious Strikes
            // Increases Crit chance by 3% per point of PS and SS
            // Increases Crit Strike Damage by 15% per point of PS and SS

            // Virulence
            // Increases Spell hit +1% per point
            if (character.DeathKnightTalents.Virulence > 0) {
                FullCharacterStats.SpellHit += 0.01f * character.DeathKnightTalents.Virulence;
            }

            // Anticipation
            // Increases dodge by 1% per point
            if (character.DeathKnightTalents.Anticipation > 0) {
                FullCharacterStats.Dodge += 0.01f * character.DeathKnightTalents.Anticipation;
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
            if (character.DeathKnightTalents.RavenousDead > 0) {
                FullCharacterStats.BonusStrengthMultiplier += (0.01f * character.DeathKnightTalents.RavenousDead);
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
            // DS, PS and SS generate 2.5 more runic power per point.

            // Magic Suppression
            // 2% per point less damage from all magic.
            // AMS absorbs additional 8, 16, 25% of spell damage.
            if (character.DeathKnightTalents.MagicSuppression > 0) {
                FullCharacterStats.SpellDamageTakenMultiplier -= 0.02f * character.DeathKnightTalents.MagicSuppression;
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
            // Patch 3.2: You don't cause 1% more damage 
            // Lasts 12 sec.
            /* Patch 3.2
            if (character.DeathKnightTalents.Desecration > 0)
            {
                newStats = new Stats();
                newStats.BonusDamageMultiplier += 0.01f * character.DeathKnightTalents.Desecration;
                // Gonna use an average CD of a rune at 10sec per rune divided by 2 runes == 5 sec.
                sReturn.AddSpecialEffect(new SpecialEffect(Trigger.SpellHit, newStats, 12f, 5f));
            }
            */

            // AntiMagic Zone
            // Creates a zone where party/raid members take 75% less spell damage
            // Lasts 10 secs or X damage.  
            if (character.DeathKnightTalents.AntiMagicZone > 0) {
                newStats = new Stats();
                newStats.SpellDamageTakenMultiplier -= .75f * character.DeathKnightTalents.AntiMagicZone;
                FullCharacterStats.AddSpecialEffect(new SpecialEffect(Trigger.Use, newStats, 10f, 2f * 60f));
            }

            // Improved Unholy Presence
            // in Blood or Frost, retain movement speed (8%, 15%).
            // Runes finish CD 5% per point faster.

            // Ghoul Frenzy
            // Grants pet 25% haste for 30 sec and heals it for 60% health.

            // Crypt Fever
            // CF increases disease damage taken by target by 10% per point

            // Bone Shield
            // 3 Bones as of 3.2.2
            // Takes 20% less dmage from all sources
            // Does 2% more damage to target
            // Each damaging attack consumes a bone.
            // Lasts 5 mins

            // Ebon Plaguebringer
            // CF becomes EP - increases magic damage taken by targets 4, 9, 13% in addition to disease damage
            // Increases crit strike chance by 1% per point
            if (character.DeathKnightTalents.EbonPlaguebringer > 0) {
                FullCharacterStats.PhysicalCrit += 0.01f * character.DeathKnightTalents.EbonPlaguebringer;
                FullCharacterStats.SpellCrit += 0.01f * character.DeathKnightTalents.EbonPlaguebringer;
                float fBonus = 0f;
                switch (character.DeathKnightTalents.EbonPlaguebringer)
                {
                    case 1:
                        fBonus = .04f;
                        break;
                    case 2:
                        fBonus = .09f;
                        break;
                    case 3:
                        fBonus = .13f;
                        break;
                    default:
                        break;
                }
                FullCharacterStats.BonusArcaneDamageMultiplier += fBonus;
                FullCharacterStats.BonusFireDamageMultiplier += fBonus;
                FullCharacterStats.BonusFrostDamageMultiplier += fBonus;
                FullCharacterStats.BonusHolyDamageMultiplier += fBonus;
                FullCharacterStats.BonusNatureDamageMultiplier += fBonus;
                FullCharacterStats.BonusShadowDamageMultiplier += fBonus;
            }

            // Sourge Strike

            // Rage of Rivendare
            // 2% per point more damage to targets w/ BP
            // Expertise +1 per point
            if (character.DeathKnightTalents.RageOfRivendare > 0) {
                FullCharacterStats.Expertise += character.DeathKnightTalents.RageOfRivendare;
                // Assuming BP is always on.
                FullCharacterStats.BonusDamageMultiplier += 0.02f * character.DeathKnightTalents.RageOfRivendare;
            }

            // Summon Gargoyle

            #endregion

//            return sReturn;
        }

        #region Evalutaions And Ratings
        /// <summary>Evaluate how many swings until the tank is next hit.</summary>
        /// <param name="PercAvoidance">a float that is a 0-1 value for % of total avoidance (Dodge + Parry + Miss)</param>
        /// <returns>Float of how many swings until the next hit. Should be > 1</returns>
        private float GetReactionTime(float PercAvoidance) {
            float fReactionTime = 0f;
            // check args.
            if (PercAvoidance < 0f || PercAvoidance > 1f) { return 0f; }// error
            fReactionTime = 1f / (1f - PercAvoidance);
            return fReactionTime;
        }

        /// <summary>
        /// Evaluate how many swings until the tank dies.
        /// </summary>
        /// <param name="PercAvoidance">a float values of Total avoidance 0-1 as a decimal percentage.</param>
        /// <param name="EffectiveHealth">Survival score</param>
        /// <param name="RawPerHit">What's the raw unmitigated damage coming in.</param>
        /// <returns>the number of hits until death.</returns>
        private float GetBurstTime(float PercAvoidance, float EffectiveHealth, float RawPerHit) {
            float fBurstTime = 0f;
            // check args.
            if (PercAvoidance < 0 || PercAvoidance > 1) { return 0f; } // error

            float fHvH = (EffectiveHealth / RawPerHit);

            fBurstTime = (1f / PercAvoidance) * ((1f / (float)Math.Pow((1f - PercAvoidance), fHvH)) - 1f);

            return fBurstTime;
        }

        /// <summary>
        /// Get the Survival rating of the current character.
        /// </summary>
        /// <returns>The value of the survival SubPoint.</returns>
        private float GetSurvivalRating()
        {
            return 0f;
        }

        /// <summary>
        /// Get the MitigationRating of the current setup.
        /// </summary>
        /// <returns>the value of the mitigation subpoint</returns>
        private float GetMitigationRating()
        {
            return 0f;
        }

        /// <summary>
        /// Get the Threat Rating of the current setup.
        /// </summary>
        /// <returns>the value of Threat per second.</returns>
        private float GetThreatRating()
        {
            return 0f;
        }
        #endregion 

        /// <summary>Deserializes the model's CalculationOptions data object from xml</summary>
        /// <param name="xml">The serialized xml representing the model's CalculationOptions data object.</param>
        /// <returns>The model's CalculationOptions data object.</returns>
        public override ICalculationOptionBase DeserializeDataObject(string xml) {
            XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsTankDK));
            StringReader reader = new StringReader(xml);
            CalculationOptionsTankDK calcOpts = serializer.Deserialize(reader) as CalculationOptionsTankDK;
            return calcOpts;
        }
    }
}
