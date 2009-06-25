using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
#if SILVERLIGHT
using System.Windows.Media;
#else
using System.Drawing;
#endif

namespace Rawr.DPSDK
{
    //[Rawr.Calculations.RawrModelInfo("DPSDK", "spell_deathknight_classicon", Character.CharacterClass.Paladin)]  wont work until wotlk goes live on wowhead
    [Rawr.Calculations.RawrModelInfo("DPSDK", "spell_shadow_deathcoil", Character.CharacterClass.DeathKnight)]
    public class CalculationsDPSDK : CalculationsBase
    {
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                ////Relevant Gem IDs for DPSDKs

                //Red
                int[] bold = { 39900, 39996, 40111, 42142 };

                //Purple
                int[] sovereign = { 39934, 40022, 40129 };

                //Orange
                int[] inscribed = { 39947, 40037, 40142 };

                //Meta
                int chaotic = 41285;

                return new List<GemmingTemplate>()
				{
					new GemmingTemplate() { Model = "DPSDK", Group = "Uncommon", //Max Strength
						RedId = bold[0], YellowId = bold[0], BlueId = bold[0], PrismaticId = bold[0], MetaId = chaotic },
					new GemmingTemplate() { Model = "DPSDK", Group = "Uncommon", //Strength
						RedId = bold[0], YellowId = inscribed[0], BlueId = sovereign[0], PrismaticId = bold[0], MetaId = chaotic },
						
					new GemmingTemplate() { Model = "DPSDK", Group = "Rare", Enabled = true, //Max Strength
						RedId = bold[1], YellowId = bold[1], BlueId = bold[1], PrismaticId = bold[1], MetaId = chaotic },
					new GemmingTemplate() { Model = "DPSDK", Group = "Rare", Enabled = true, //Strength
						RedId = bold[1], YellowId = inscribed[1], BlueId = sovereign[1], PrismaticId = bold[1], MetaId = chaotic },
						
					new GemmingTemplate() { Model = "DPSDK", Group = "Epic", //Max Strength
						RedId = bold[2], YellowId = bold[2], BlueId = bold[2], PrismaticId = bold[2], MetaId = chaotic },
					new GemmingTemplate() { Model = "DPSDK", Group = "Epic", //Strength
						RedId = bold[2], YellowId = inscribed[2], BlueId = sovereign[2], PrismaticId = bold[2], MetaId = chaotic },
						
					new GemmingTemplate() { Model = "DPSDK", Group = "Jeweler", //Max Strength
						RedId = bold[3], YellowId = bold[3], BlueId = bold[3], PrismaticId = bold[3], MetaId = chaotic },
					new GemmingTemplate() { Model = "DPSDK", Group = "Jeweler", //Strength
						RedId = bold[2], YellowId = bold[3], BlueId = bold[3], PrismaticId = bold[2], MetaId = chaotic },
				};
            }
        }

        private Dictionary<string, Color> _subPointNameColors = null;
        /// <summary>

        /// Dictionary<string, Color> that includes the names of each rating which your model will use,

        /// and a color for each. These colors will be used in the charts.

        /// 

        /// EXAMPLE: 

        /// subPointNameColors = new Dictionary<string, System.Drawing.Color>();

        /// subPointNameColors.Add("Mitigation", System.Drawing.Colors.Red);

        /// subPointNameColors.Add("Survival", System.Drawing.Colors.Blue);

        /// </summary>

        public override Dictionary<string, Color> SubPointNameColors
        {
            get
            {
                if (_subPointNameColors == null)
                {
#if SILVERLIGHT
                    _subPointNameColors = new Dictionary<string, System.Windows.Media.Color>();
                    _subPointNameColors.Add("DPS", System.Windows.Media.Color.FromArgb(255,0,0,255));
#else
                    _subPointNameColors = new Dictionary<string, System.Drawing.Color>();
                    _subPointNameColors.Add("DPS", System.Drawing.Color.Blue);
#endif
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
					    "Basic Stats:Strength",
					    "Basic Stats:Agility",
					    "Basic Stats:Attack Power",
					    "Basic Stats:Crit Rating",
					    "Basic Stats:Hit Rating",
					    "Basic Stats:Expertise",
					    "Basic Stats:Haste Rating",
					    "Basic Stats:Armor Penetration Rating",
                        "Basic Stats:Armor",
					    "Advanced Stats:Weapon Damage*Damage before misses and mitigation",
					    "Advanced Stats:Attack Speed",
					    "Advanced Stats:Crit Chance",
					    "Advanced Stats:Avoided Attacks",
					    "Advanced Stats:Enemy Mitigation",
                        "DPS Breakdown:White",
                        "DPS Breakdown:BCB*Blood Caked Blade",
                        "DPS Breakdown:Necrosis",
					 //   "DPS Breakdown:Windfury*Contribution from White, BCB, and Necrosis; NOT added to total",
                        "DPS Breakdown:Death Coil",
                        "DPS Breakdown:Icy Touch",
                        "DPS Breakdown:Plague Strike",
                        "DPS Breakdown:Frost Fever",
                        "DPS Breakdown:Blood Plague",
                        "DPS Breakdown:Scourge Strike",
                        "DPS Breakdown:Unholy Blight",
                        "DPS Breakdown:Frost Strike",
                        "DPS Breakdown:Howling Blast",
                        "DPS Breakdown:Obliterate",
                        "DPS Breakdown:Death Strike",
                        "DPS Breakdown:Blood Strike",
                        "DPS Breakdown:Heart Strike",
                        "DPS Breakdown:DRW*Dancing Rune Weapon",
                        "DPS Breakdown:Gargoyle",
                        "DPS Breakdown:Wandering Plague",
                        "DPS Breakdown:Ghoul",
                        "DPS Breakdown:Bloodworms",
                        "DPS Breakdown:Other",
                        "DPS Breakdown:Total DPS",
                    });
                    _characterDisplayCalculationLabels = labels.ToArray();
                }
                return _characterDisplayCalculationLabels;
            }
        }

        private string[] _customChartNames = null;
        public override string[] CustomChartNames
        {
            get
            {
                if (_customChartNames == null)
                {
                    _customChartNames = new string[] { "Item Budget" };
                }
                return _customChartNames;
            }
        }

#if SILVERLIGHT
        private ICalculationOptionsPanel _calculationOptionsPanel = null;
        public override ICalculationOptionsPanel CalculationOptionsPanel
#else
        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
        public override CalculationOptionsPanelBase CalculationOptionsPanel
#endif
        {
            get { return _calculationOptionsPanel ?? (_calculationOptionsPanel = new CalculationOptionsPanelDPSDK()); }
        }

        private List<Item.ItemType> _relevantItemTypes = null;
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

        public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.DeathKnight; } }
        public override ComparisonCalculationBase CreateNewComparisonCalculation()
        {
            return new ComparisonCalculationDPSDK();
        }

        public override CharacterCalculationsBase CreateNewCharacterCalculations()
        {
            return new CharacterCalculationsDPSDK();
        }


        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            XmlSerializer serializer =
                new XmlSerializer(typeof(CalculationOptionsDPSDK));
            System.IO.StringReader reader = new System.IO.StringReader(xml);
            CalculationOptionsDPSDK calcOpts = serializer.Deserialize(reader) as CalculationOptionsDPSDK;
            return calcOpts;
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

            CalculationOptionsDPSDK calcOpts = character.CalculationOptions as CalculationOptionsDPSDK;
            GetTalents(character);

#if SILVERLIGHT
            if (character != null && character.MainHand != null && character.MainHand.Item == null)
            {
                character.MainHand.Item = new Item("Test Weapon", Item.ItemQuality.Artifact, Item.ItemType.TwoHandAxe, 12345, "",
                   Item.ItemSlot.TwoHand, "", false, new Stats() { Strength = 100f }, new Stats() { }, Item.ItemSlot.None, Item.ItemSlot.None,
                   Item.ItemSlot.None, 500, 1500, Item.ItemDamageType.Physical, 3.6f, "");
            }
#endif

            Stats stats = GetCharacterStats(character, additionalItem);

            CharacterCalculationsDPSDK calcs = new CharacterCalculationsDPSDK();
            calcs.BasicStats = stats;
            calcs.ActiveBuffs = new List<Buff>(character.ActiveBuffs);
            calcs.Talents = calcOpts.talents;

            CombatTable combatTable = new CombatTable(character, calcs, stats, calcOpts);
            
            //DPS Subgroups
            float dpsWhite = 0f;
            float dpsBCB = 0f;
            float dpsNecrosis = 0f;
            float dpsDeathCoil = 0f;
            float dpsIcyTouch = 0f;
            float dpsPlagueStrike = 0f;
            float dpsFrostFever = 0f;
            float dpsBloodPlague = 0f;
            float dpsScourgeStrike = 0f;
            float dpsUnholyBlight = 0f;
            float dpsFrostStrike = 0f;
            float dpsHowlingBlast = 0f;
            float dpsObliterate = 0f;
            float dpsDeathStrike = 0f;
            float dpsBloodStrike = 0f;
            float dpsHeartStrike = 0f;
            float dpsDancingRuneWeapon = 0f;
            float dpsGargoyle = 0f;
            float dpsWanderingPlague = 0f;
            float dpsWPFromFF = 0f;
            float dpsWPFromBP = 0f;
            float dpsGhoul = 0f;
            float dpsOtherShadow = 0f;
            float dpsOtherArcane = 0f;
            float dpsOtherFrost = 0f;
            float dpsBloodworms = 0f;

            //shared variables
            DeathKnightTalents talents = calcOpts.talents;
            bool DW = combatTable.DW;
            float missedSpecial = 0f;

            float dpsWhiteBeforeArmor = 0f;
            float dpsWhiteMinusGlancing = 0f;
            float fightDuration = calcOpts.FightLength * 60;
            float mitigation;
            float KMRatio = 0f;
            float CinderglacierMultiplier = 1f;

            float MHExpertise = stats.Expertise;
            float OHExpertise = stats.Expertise;

            //damage multipliers
            float spellPowerMult = 1f + stats.BonusSpellPowerMultiplier;
            float frostSpellPowerMult = 1f + stats.BonusSpellPowerMultiplier + Math.Max((stats.BonusFrostDamageMultiplier - stats.BonusShadowDamageMultiplier), 0f);

            float physPowerMult = 1f + stats.BonusPhysicalDamageMultiplier;
            // Covers all % physical damage increases.  Blood Frenzy, FI.
            float partialResist = 0.94f; // Average of 6% damage lost to partial resists on spells

            //spell AP multipliers, for diseases its per tick
            float HowlingBlastAPMult = 0.1f;
            float IcyTouchAPMult = 0.1f;
            float FrostFeverAPMult = 0.055f;
            float BloodPlagueAPMult = 0.055f;
            float DeathCoilAPMult = 0.15f;
            float UnholyBlightAPMult = 0.013f;
            float GargoyleAPMult = 0.37f;   // pre 3.0.8 == 0.42f...now probably ~0.3f
            float BloodwormsAPMult = 0.006f;

            //for estimating rotation pushback

            calcOpts.rotation.avgDiseaseMult = calcOpts.rotation.numDisease * (calcOpts.rotation.diseaseUptime / 100);
            float commandMult = 0f;

            #region non-ptr
            if (!calcOpts.rotation.PTRCalcs)
            {
                calcOpts.presence = calcOpts.rotation.presence;

                if (calcOpts.rotation.managedRP)
                {
                    calcOpts.rotation.getRP(talents, character);
                }

                #region Impurity Application
                {
                    float impurityMult = 1f + (.04f * (float)talents.Impurity);

                    HowlingBlastAPMult *= impurityMult;
                    IcyTouchAPMult *= impurityMult;
                    FrostFeverAPMult *= impurityMult;
                    BloodPlagueAPMult *= impurityMult;
                    DeathCoilAPMult *= impurityMult;
                    UnholyBlightAPMult *= impurityMult;
                    GargoyleAPMult *= impurityMult;
                }
                #endregion

                #region racials
                {
                    if (character.Race == Character.CharacterRace.Orc)
                    {
                        commandMult += .05f;
                    }
                }
                #endregion

                #region Killing Machine
                {
                    float KMPpM = (1f * talents.KillingMachine) * (1f + (StatConversion.GetHasteFromRating(stats.HasteRating, Character.CharacterClass.DeathKnight))); // KM Procs per Minute (Defined "1 per point" by Blizzard) influenced by Phys. Haste
                    float addHastePercent = 1f;


                    /*   if (calcOpts.Bloodlust)
                       {
                           float numLust = fightDuration % 600f;  // bloodlust changed in 3.0, can only have one every 10 minutes.
                           float fullLustDur = (numLust - 1) * 600f + 40f;
                           if (fightDuration < fullLustDur) // if the last lust doesn't go its full duration
                           {
                               float lastLustFraction = (fullLustDur - fightDuration) / 40f;
                               numLust -= 1f;
                               numLust += lastLustFraction;
                           }

                           float bloodlustUptime = (numLust * 40f) / fightDuration;

                           addHastePercent += 0.3f * bloodlustUptime;
                       }

                       if (calcOpts.talents.ImprovedIcyTalons != 0)
                       {
                           addHastePercent += 0.05f;
                       }*/

                    addHastePercent += stats.PhysicalHaste;

                    KMPpM *= addHastePercent;

                    float KMPpR = KMPpM / (60 / calcOpts.rotation.curRotationDuration);
                    float totalAbilities = calcOpts.rotation.FrostStrike + calcOpts.rotation.IcyTouch + calcOpts.rotation.HowlingBlast;
                    KMRatio = KMPpR / totalAbilities;
                }
                #endregion

                #region Cinderglacier
                {
                    /*    if (character.GetEnchantBySlot(Character.CharacterSlot.MainHand) != null && character.GetEnchantBySlot(Character.CharacterSlot.MainHand).Id == 3369)
                        {
                            float shadowFrostAbilitiesPerSecond = (calcOpts.rotation.DeathCoil + calcOpts.rotation.FrostStrike +
                                calcOpts.rotation.ScourgeStrike + calcOpts.rotation.IcyTouch + calcOpts.rotation.HowlingBlast) / combatTable.realDuration;
                            SpecialEffect temp = new SpecialEffect(Trigger.Use, new Stats(), 0, -1f);
                            float avgPPS = temp.GetAverageUptime(combatTable.MH.hastedSpeed, 1f - calcs.AvoidedAttacks, combatTable.MH.baseSpeed, calcOpts.FightLength * 60f)/60;
                            CinderglacierMultiplier *= 1f + (avgPPS * 2) / shadowFrostAbilitiesPerSecond;
                        }
                        if (character.GetEnchantBySlot(Character.CharacterSlot.OffHand) != null && character.GetEnchantBySlot(Character.CharacterSlot.OffHand).Id == 3369)
                        {
                            float shadowFrostAbilitiesPerSecond = (calcOpts.rotation.DeathCoil + calcOpts.rotation.FrostStrike +
                                calcOpts.rotation.ScourgeStrike + calcOpts.rotation.IcyTouch + calcOpts.rotation.HowlingBlast) / combatTable.realDuration;
                            SpecialEffect temp = new SpecialEffect(Trigger.Use, new Stats(), 0, -1f);
                            float avgPPS = temp.GetAverageUptime(combatTable.MH.hastedSpeed, 1f - calcs.AvoidedAttacks, combatTable.MH.baseSpeed, calcOpts.FightLength * 60f) / 60;
                            CinderglacierMultiplier *= 1f + (avgPPS * 2) / shadowFrostAbilitiesPerSecond;
                        }*/
                    float shadowFrostAbilitiesPerSecond = ((calcOpts.rotation.DeathCoil + calcOpts.rotation.FrostStrike +
                            calcOpts.rotation.ScourgeStrike + calcOpts.rotation.IcyTouch + calcOpts.rotation.HowlingBlast) /
                            combatTable.realDuration);

                    CinderglacierMultiplier *= 1f + (0.2f / (shadowFrostAbilitiesPerSecond / stats.CinderglacierProc));

                }
                #endregion

                #region Mitigation
                {
                    float targetArmor = calcOpts.BossArmor, totalArP = stats.ArmorPenetration;

                    //// Effective armor after ArP

                    //targetArmor -= totalArP;
                    //float ratingCoeff = stats.ArmorPenetrationRating / 1539f;
                    //targetArmor *= (1 - ratingCoeff);
                    //if (targetArmor < 0) targetArmor = 0f;

                    //// Convert armor to mitigation

                    ////mitigation = 1f - (targetArmor/(targetArmor + 10557.5f));

                    ////mitigation = 1f - targetArmor / (targetArmor + 400f + 85f * (5.5f * (float)calcOpts.TargetLevel - 265.5f));

                    //mitigation = 1f - (targetArmor / ((467.5f * (float)calcOpts.TargetLevel) + targetArmor - 22167.5f));

                    mitigation = 1f - StatConversion.GetArmorDamageReduction(character.Level, targetArmor,
                    stats.ArmorPenetration, 0f, stats.ArmorPenetrationRating);

                    calcs.EnemyMitigation = 1f - mitigation;
                    calcs.EffectiveArmor = mitigation;
                }
                #endregion

                #region White Dmg
                {
                    float MHDPS = 0f, OHDPS = 0f;
                    #region Main Hand
                    {
                        float dpsMHglancing = (0.24f * combatTable.MH.DPS) * 0.75f;
                        float dpsMHBeforeArmor = ((combatTable.MH.DPS * (1f - calcs.AvoidedAttacks - 0.24f)) * (1f + combatTable.physCrits)) + dpsMHglancing;
                        dpsWhiteMinusGlancing = dpsMHBeforeArmor - dpsMHglancing;
                        dpsWhiteBeforeArmor = dpsMHBeforeArmor;
                        MHDPS = dpsMHBeforeArmor * mitigation;
                    }
                    #endregion

                    #region Off Hand
                    if (DW || (character.MainHand == null && character.OffHand != null))
                    {
                        float dpsOHglancing = (0.24f * combatTable.OH.DPS) * 0.75f;
                        float dpsOHBeforeArmor = ((combatTable.OH.DPS * (1f - calcs.AvoidedAttacks - 0.24f)) * (1f + combatTable.physCrits)) + dpsOHglancing;
                        dpsWhiteMinusGlancing += dpsOHBeforeArmor - dpsOHglancing;
                        dpsWhiteBeforeArmor += dpsOHBeforeArmor;
                        OHDPS = dpsOHBeforeArmor * mitigation;
                    }
                    #endregion

                    dpsWhite = MHDPS + OHDPS;
                }
                #endregion

                #region Necrosis
                {
                    dpsNecrosis = dpsWhiteMinusGlancing * (.04f * (float)talents.Necrosis); // doesn't proc off Glancings
                }
                #endregion

                #region Blood Caked Blade
                {
                    float dpsMHBCB = 0f;
                    float dpsOHBCB = 0f;
                    if ((combatTable.OH.damage != 0) && (DW || combatTable.MH.damage == 0))
                    {
                        float OHBCBDmg = combatTable.OH.damage * (.25f + .125f * calcOpts.rotation.avgDiseaseMult);
                        dpsOHBCB = OHBCBDmg / combatTable.OH.hastedSpeed;
                    }
                    if (combatTable.MH.damage != 0)
                    {
                        float MHBCBDmg = combatTable.MH.damage * (.25f + .125f * calcOpts.rotation.avgDiseaseMult);
                        dpsMHBCB = MHBCBDmg / combatTable.MH.hastedSpeed;
                    }
                    dpsBCB = dpsMHBCB + dpsOHBCB;
                    dpsBCB *= .1f * (float)talents.BloodCakedBlade;
                }
                #endregion

                #region Death Coil
                {
                    if (calcOpts.rotation.DeathCoil > 0f)
                    {
                        float DCCD = combatTable.realDuration / (calcOpts.rotation.DeathCoil + (0.05f * (float)talents.SuddenDoom * calcOpts.rotation.HeartStrike));
                        float DCDmg = 443f + (DeathCoilAPMult * stats.AttackPower) + stats.BonusDeathCoilDamage;
                        dpsDeathCoil = DCDmg / DCCD;
                        float DCCritDmgMult = .5f * (2f + stats.CritBonusDamage);
                        float DCCrit = 1f + ((combatTable.spellCrits + stats.BonusDeathCoilCrit) * DCCritDmgMult);
                        dpsDeathCoil *= DCCrit;

                        //sudden doom stuff
                        // this section is no longer relevant after 3.1.x changes
                        /* float affectedDCMult = calcOpts.rotation.BloodStrike + calcOpts.rotation.HeartStrike;
                         affectedDCMult *= .04f * (float)talents.SuddenDoom;
                         affectedDCMult /= calcOpts.rotation.DeathCoil;
                         dpsDeathCoil += dpsDeathCoil * affectedDCMult;*/

                        dpsDeathCoil *= 1f + (.05f * (float)talents.Morbidity) + (talents.GlyphofDarkDeath ? .15f : 0f);
                    }
                }
                #endregion

                #region Icy Touch
                // this seems to handle crit strangely.
                // additionally, looks like it's missing some multipliers? maybe they're applied later
                {
                    if (calcOpts.rotation.IcyTouch > 0f)
                    {
                        float addedCritFromKM = KMRatio;
                        float ITCD = combatTable.realDuration / calcOpts.rotation.IcyTouch;
                        float ITDmg = 236f + (IcyTouchAPMult * stats.AttackPower) + stats.BonusIcyTouchDamage;
                        ITDmg *= 1f + .1f * (float)talents.ImprovedIcyTouch;
                        dpsIcyTouch = ITDmg / ITCD;
                        float ITCritDmgMult = .5f * (2f + stats.CritBonusDamage);
                        float ITCrit = 1f + ((combatTable.spellCrits + addedCritFromKM + (.05f * (float)talents.Rime)) * ITCritDmgMult);
                        dpsIcyTouch *= ITCrit;
                    }
                }
                #endregion

                #region Plague Strike
                {
                    if (calcOpts.rotation.PlagueStrike > 0f)
                    {
                        float PSCD = combatTable.realDuration / calcOpts.rotation.PlagueStrike;
                        float PSDmg = (combatTable.MH.baseDamage + ((stats.AttackPower / 14f) * combatTable.normalizationFactor)) * .5f + 189f;
                        dpsPlagueStrike = PSDmg / PSCD;
                        float PSCritDmgMult = 1f + (.15f * (float)talents.ViciousStrikes);
                        float PSCrit = 1f + ((combatTable.physCrits + (.03f * (float)talents.ViciousStrikes) + stats.BonusPlagueStrikeCrit) * PSCritDmgMult);
                        dpsPlagueStrike *= PSCrit;

                        dpsPlagueStrike *= (talents.GlyphofPlagueStrike ? 1.2f : 1f);

                        dpsPlagueStrike *= 1f + (.1f * (float)talents.Outbreak);
                    }
                }
                #endregion

                #region Frost Fever
                {
                    if (calcOpts.rotation.IcyTouch > 0f || (talents.GlyphofHowlingBlast && calcOpts.rotation.HowlingBlast > 0f) || (talents.GlyphofDisease))
                    {
                        // Frost Fever is renewed with every Icy Touch and starts a new cd
                        float ITCD = calcOpts.rotation.curRotationDuration / (calcOpts.rotation.IcyTouch + (talents.GlyphofHowlingBlast ? calcOpts.rotation.HowlingBlast : 0f));
                        float FFCD = 3f / (calcOpts.rotation.diseaseUptime / 100);
                        int tempF = (int)Math.Floor(ITCD / FFCD);
                        FFCD = ((ITCD - ((float)tempF * FFCD)) / ((float)tempF + 1f)) + FFCD;
                        float FFDmg = FrostFeverAPMult * stats.AttackPower + 25.6f;
                        dpsFrostFever = FFDmg / FFCD;
                        //dpsFrostFever *= 1f + combatTable.spellCrits;                            // Frost Fever can't crit

                        //dpsWPFromFF = FFDmg / (FFCD / combatTable.physCrits);
                        dpsWPFromFF = dpsFrostFever * combatTable.physCrits;
                    }
                }
                #endregion

                #region Blood Plague
                {
                    if (calcOpts.rotation.PlagueStrike > 0f || talents.GlyphofPestilence)
                    {
                        // Blood Plague is renewed with every Plague Strike and starts a new cd
                        float PSCD = calcOpts.rotation.curRotationDuration / calcOpts.rotation.PlagueStrike;
                        float BPCD = 3f / (calcOpts.rotation.diseaseUptime / 100);
                        int tempF = (int)Math.Floor(PSCD / BPCD);
                        BPCD = ((PSCD - ((float)tempF * BPCD)) / ((float)tempF + 1f)) + BPCD;
                        float BPDmg = BloodPlagueAPMult * stats.AttackPower + 31.1f;
                        dpsBloodPlague = BPDmg / BPCD;
                        //dpsBloodPlague *= 1f + combatTable.spellCrits;                           // Blood Plague can't crit

                        //dpsWPFromBP = BPDmg / (BPCD / combatTable.physCrits);
                        dpsWPFromBP = dpsBloodPlague * combatTable.physCrits;
                    }
                }
                #endregion

                #region Wandering Plague
                {
                    dpsWanderingPlague = dpsWPFromBP + dpsWPFromFF;
                    dpsWanderingPlague *= (1f / 3f) * (float)talents.WanderingPlague;
                }
                #endregion

                #region Scourge Strike
                {
                    if (talents.ScourgeStrike > 0 && calcOpts.rotation.ScourgeStrike > 0f)
                    {
                        float SSCD = combatTable.realDuration / calcOpts.rotation.ScourgeStrike;
                        float SSDmg = ((combatTable.MH.baseDamage + ((stats.AttackPower / 14f) * combatTable.normalizationFactor)) * .45f) + 357.188f +
                            stats.BonusScourgeStrikeDamage;
                        SSDmg *= 1f + 0.11f * calcOpts.rotation.avgDiseaseMult * (1f + stats.BonusPerDiseaseScourgeStrikeDamage);
                        dpsScourgeStrike = SSDmg / SSCD;
                        float SSCritDmgMult = 1f + (.15f * (float)talents.ViciousStrikes) + stats.CritBonusDamage;
                        float SSCrit = 1f + ((combatTable.physCrits + (.03f * (float)talents.ViciousStrikes) + stats.BonusScourgeStrikeCrit) * SSCritDmgMult);
                        dpsScourgeStrike = dpsScourgeStrike * SSCrit;
                        dpsScourgeStrike *= 1f + (.0666666666666666666f * (float)talents.Outbreak);
                    }
                }
                #endregion

                #region Unholy Blight
                {
                    //The cooldown on this 1 second and I assume 100% uptime
                    float UBDmg = UnholyBlightAPMult * stats.AttackPower + 37;
                    dpsUnholyBlight = UBDmg * (1f + combatTable.spellCrits);
                    dpsUnholyBlight *= (float)talents.UnholyBlight;
                }
                #endregion

                #region Frost Strike
                {
                    if (talents.FrostStrike > 0 && calcOpts.rotation.FrostStrike > 0f)
                    {
                        float addedCritFromKM = KMRatio;
                        float FSCD = combatTable.realDuration / calcOpts.rotation.FrostStrike;
                        float FSDmg = (combatTable.MH.baseDamage + ((stats.AttackPower / 14f) * combatTable.normalizationFactor)) * .6f +
                            150f + stats.BonusFrostStrikeDamage;
                        dpsFrostStrike = FSDmg / FSCD;
                        float FSCritDmgMult = 1f + (.15f * (float)talents.GuileOfGorefiend) + stats.CritBonusDamage;
                        float FSCrit = 1f + ((combatTable.physCrits + addedCritFromKM + stats.BonusFrostStrikeCrit) * FSCritDmgMult);
                        dpsFrostStrike *= FSCrit;
                        dpsFrostStrike *= 1f + .03f * talents.BloodOfTheNorth;
                    }
                }
                #endregion

                #region Bloodworms
                {
                    if (talents.Bloodworms > 0)
                    {
                        float BloodwormSwing = 50f + BloodwormsAPMult * stats.AttackPower;
                        float BloodwormSwingDPS = BloodwormSwing / 2.0f;    // any haste benefits?
                        float TotalBloodworms = ((fightDuration / combatTable.MH.hastedSpeed) + calcOpts.rotation.getMeleeSpecialsPerSecond() * fightDuration)
                            * (0.03f * talents.Bloodworms)
                            * 3f /*average of 3 bloodworms per proc*/;
                        dpsBloodworms = ((TotalBloodworms * BloodwormSwingDPS * 20) / fightDuration);
                    }
                }
                #endregion

                #region Trinket direct-damage procs, razorice damage, etc
                {
                    dpsOtherArcane = stats.ArcaneDamage;
                    dpsOtherShadow = stats.ShadowDamage;

                    if (combatTable.MH != null)
                    {
                        float dpsMHglancing = (0.24f * combatTable.MH.DPS) * 0.75f;
                        float dpsMHBeforeArmor = ((combatTable.MH.DPS * (1f - calcs.AvoidedAttacks - 0.24f)) * (1f + combatTable.physCrits)) + dpsMHglancing;
                        dpsOtherFrost += (dpsMHBeforeArmor - dpsMHglancing) * stats.BonusFrostWeaponDamage;   // presumably doesn't proc off of glancings, like necrosis
                    }

                    if (combatTable.OH != null)
                    {
                        float dpsOHglancing = (0.24f * combatTable.OH.DPS) * 0.75f;
                        float dpsOHBeforeArmor = ((combatTable.OH.DPS * (1f - calcs.AvoidedAttacks - 0.24f)) * (1f + combatTable.physCrits)) + dpsOHglancing;
                        dpsOtherFrost += (dpsOHBeforeArmor - dpsOHglancing) * stats.BonusFrostWeaponDamage;
                    }

                    float OtherCritDmgMult = .5f * (1f + stats.CritBonusDamage);
                    float OtherCrit = 1f + ((combatTable.spellCrits) * OtherCritDmgMult);
                    dpsOtherArcane *= OtherCrit;
                    dpsOtherShadow *= OtherCrit;
                }
                #endregion

                #region Howling Blast
                {
                    if (talents.HowlingBlast > 0 && calcOpts.rotation.HowlingBlast > 0f)
                    {
                        float addedCritFromKM = KMRatio;
                        float HBCD = combatTable.realDuration / calcOpts.rotation.HowlingBlast;
                        float HBDmg = 540 + HowlingBlastAPMult * stats.AttackPower;
                        dpsHowlingBlast = HBDmg / HBCD;
                        float HBCritDmgMult = .5f * (2f + (.15f * (float)talents.GuileOfGorefiend) + stats.CritBonusDamage);
                        float HBCrit = 1f + ((combatTable.spellCrits + addedCritFromKM) * HBCritDmgMult);
                        dpsHowlingBlast *= HBCrit;
                    }
                }
                #endregion

                #region Obliterate
                {
                    if (calcOpts.rotation.Obliterate > 0f)
                    {
                        // this is missing +crit chance from rime
                        float OblitCD = combatTable.realDuration / calcOpts.rotation.Obliterate;
                        float OblitDmg = ((combatTable.MH.baseDamage + ((stats.AttackPower / 14f) * combatTable.normalizationFactor)) *
                            0.8f) + stats.BonusObliterateDamage;
                        OblitDmg *= 1f + 0.125f * (float)calcOpts.rotation.avgDiseaseMult * (1f + stats.BonusPerDiseaseObliterateDamage);
                        dpsObliterate = OblitDmg / OblitCD;
                        //float OblitCrit = 1f + combatTable.physCrits + ( .03f * (float)talents.Subversion );
                        //OblitCrit += .05f * (float)talents.Rime;
                        float OblitCritDmgMult = 1f + (.15f * (float)talents.GuileOfGorefiend) + stats.CritBonusDamage;
                        float OblitCrit = 1f + ((combatTable.physCrits +
                            (.03f * (float)talents.Subversion) +
                            (0.05f * (float)talents.Rime) +
                            stats.BonusObliterateCrit) * OblitCritDmgMult);
                        dpsObliterate *= OblitCrit;
                        dpsObliterate *= (talents.GlyphofObliterate ? 1.2f : 1f);
                    }
                }
                #endregion

                #region Death Strike
                {
                    if (calcOpts.rotation.DeathStrike > 0f)
                    {
                        // TODO: this is missing +crit chance from rime
                        float DSCD = combatTable.realDuration / calcOpts.rotation.DeathStrike;
                        // TODO: This should be changed to make use of the new glyph stats:
                        float DSDmg = ((combatTable.MH.baseDamage + ((stats.AttackPower / 14f) * combatTable.normalizationFactor)) * 0.75f) + 222.75f + stats.BonusDeathStrikeDamage;
                        DSDmg *= 1f + 0.15f * (float)talents.ImprovedDeathStrike;
                        DSDmg *= (talents.GlyphofDeathStrike ? 1.25f : 1f);
                        dpsDeathStrike = DSDmg / DSCD;
                        float DSCritDmgMult = 1f + (.15f * (float)talents.MightOfMograine) + stats.CritBonusDamage;
                        float DSCrit = 1f + ((combatTable.physCrits +
                            (.03f * (float)talents.ImprovedDeathStrike) +
                            stats.BonusDeathStrikeCrit) * DSCritDmgMult);
                        dpsDeathStrike *= DSCrit;
                    }
                }
                #endregion

                #region Blood Strike
                {
                    if (calcOpts.rotation.BloodStrike > 0f)
                    {
                        float BSCD = combatTable.realDuration / calcOpts.rotation.BloodStrike;
                        float BSDmg = ((combatTable.MH.baseDamage + ((stats.AttackPower / 14f) * combatTable.normalizationFactor)) *
                            0.4f) + 305.6f + stats.BonusBloodStrikeDamage;
                        BSDmg *= 1f + 0.125f * (float)calcOpts.rotation.avgDiseaseMult * (1f + stats.BonusPerDiseaseBloodStrikeDamage);
                        dpsBloodStrike = BSDmg / BSCD;
                        float BSCritDmgMult = 1f + (.15f * (float)talents.MightOfMograine);
                        BSCritDmgMult += (.15f * (float)talents.GuileOfGorefiend) + stats.CritBonusDamage;
                        float BSCrit = 1f + ((combatTable.physCrits + (.03f * (float)talents.Subversion)) * BSCritDmgMult);
                        dpsBloodStrike = (dpsBloodStrike) * BSCrit;
                        dpsBloodStrike *= 1f + (.03f * (float)talents.BloodOfTheNorth);
                        dpsBloodStrike *= 1f + (.15f * (float)talents.BloodyStrikes);
                    }
                }
                #endregion

                #region Heart Strike
                {
                    if (talents.HeartStrike > 0 && calcOpts.rotation.HeartStrike > 0f)
                    {
                        float HSCD = combatTable.realDuration / calcOpts.rotation.HeartStrike;
                        float HSDmg = ((combatTable.MH.baseDamage + ((stats.AttackPower / 14f) * combatTable.normalizationFactor)) *
                            0.5f) + 368f + stats.BonusHeartStrikeDamage;
                        HSDmg *= 1f + 0.1f * (float)calcOpts.rotation.avgDiseaseMult * (1f + stats.BonusPerDiseaseHeartStrikeDamage);
                        dpsHeartStrike = HSDmg / HSCD;
                        //float HSCrit = 1f + combatTable.physCrits + ( .03f * (float)talents.Subversion );
                        float HSCritDmgMult = 1f + (.15f * (float)talents.MightOfMograine) + stats.CritBonusDamage;
                        float HSCrit = 1f + ((combatTable.physCrits + (.03f * (float)talents.Subversion)) * HSCritDmgMult);
                        dpsHeartStrike = (dpsHeartStrike) * HSCrit;
                        dpsHeartStrike *= 1f + (.15f * (float)talents.BloodyStrikes);
                    }
                }
                #endregion

                #region Gargoyle
                {
                    if (calcOpts.rotation.GargoyleDuration > 0f)
                    {
                        float GargoyleCastTime = 2.4f;
                        /* // Gargoyle usually doesn't get Bloodlust because he is triggered at the beginning of the fight...might take a BL over time ratio...maybe
                        if (stats.Bloodlust > 0)
                        {
                            GargoyleCastTime *= .7f;
                        }
                        */
                        float GargoyleStrike = GargoyleAPMult * stats.AttackPower;
                        float GargoyleStrikeCount = calcOpts.rotation.GargoyleDuration / GargoyleCastTime;
                        float GargoyleDmg = GargoyleStrike * GargoyleStrikeCount;
                        GargoyleDmg *= 1f + (.5f * combatTable.spellCrits);  // Gargoyle does 150% crits apparently
                        dpsGargoyle = GargoyleDmg / 180f;
                    }
                }
                #endregion

                #region Ghoul
                {
                    if (calcOpts.Ghoul)
                    {
                        float uptime = 1f;
                        if (talents.MasterOfGhouls == 0)
                        {
                            float timeleft = calcOpts.FightLength * 60;
                            float numCDs = timeleft / (5 * 60);
                            float duration = numCDs * 120f;

                            uptime = duration / timeleft;
                            /* // not quite sure what this was supposed to do...
                            timeleft -= 300f;
                            if (timeleft > 120f)
                            {
                                uptime = 240f / calcOpts.FightLength;
                            }
                            else
                            {
                                uptime = (timeleft + 120f) / calcOpts.FightLength;
                            }
                            */
                        }

                        float GhoulBaseStrength = 331f;
                        float GhoulBaseStrengthMult = 0.7f;
                        float GhoulBaseAP = 836f;
                        float GhoulBaseSpeed = 2f;
                        float GhoulAPMult = 1f;                 // 1 Str = x AP     
                        float ClawCD = 4f;
                        float GhoulWeaponBaseDamage = 101.3f;   // I found these values to be fairly correct after a number of tests (v3.0.8)
                        float GhoulAPdivisor = 16.7f;           // 

                        float GlyphofGhoulValue = 0f;
                        if (talents.GlyphoftheGhoul) GlyphofGhoulValue = 0.4f;

                        float GhoulStrengthMult = GhoulBaseStrengthMult + (GhoulBaseStrengthMult * (0.2f * (float)talents.RavenousDead)) + GlyphofGhoulValue;
                        float GhoulStrength = GhoulBaseStrength + stats.Strength * GhoulStrengthMult;

                        Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

                        GhoulStrength *= 1f + statsBuffs.BonusStrengthMultiplier;
                        GhoulStrength += statsBuffs.Strength;

                        if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Flask of Chromatic Wonder"))) GhoulStrength -= 18f;
                        if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Flask of Chromatic Wonder (Mixology)"))) GhoulStrength -= 4f;
                        if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Elixir of Mighty Strength"))) GhoulStrength -= 50f;
                        if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Elixir of Mighty Strength (Mixology)"))) GhoulStrength -= 16f;
                        if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Guru's Elixir"))) GhoulStrength -= 20f;
                        if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Guru's Elixir (Mixology)"))) GhoulStrength -= 6f;
                        if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Scroll of Strength VIII"))) GhoulStrength -= 30f;

                        float GhoulAP = GhoulBaseAP + GhoulStrength * GhoulAPMult;

                        GhoulAP *= 1f + statsBuffs.BonusAttackPowerMultiplier;
                        GhoulAP += statsBuffs.AttackPower;

                        if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Flask of Endless Rage"))) GhoulAP -= 180f;
                        if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Flask of Endless Rage (Mixology)"))) GhoulAP -= 64f;
                        if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Wrath Elixir"))) GhoulAP -= 90f;
                        if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Wrath Elixir (Mixology)"))) GhoulAP -= 32f;
                        if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Attack Power Food"))) GhoulAP -= 80f;
                        if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Fish Feast"))) GhoulAP -= 80f;

                        float GhoulhastedSpeed = 0f;
                        if (combatTable.MH != null)
                            GhoulhastedSpeed = ((combatTable.MH.hastedSpeed == 0 ? 1.0f : combatTable.MH.hastedSpeed) / (combatTable.MH.baseSpeed == 0 ? 1.0f : combatTable.MH.baseSpeed)) * GhoulBaseSpeed;
                        else
                            GhoulhastedSpeed = (2.0f) * GhoulBaseSpeed;
                        GhoulhastedSpeed /= 1f + statsBuffs.PhysicalHaste;

                        float dmgSwing = GhoulWeaponBaseDamage + (GhoulAP / GhoulAPdivisor) * GhoulBaseSpeed;
                        float dpsSwing = dmgSwing / GhoulhastedSpeed;
                        float dpsGhoulGlancing = (dpsSwing * 0.24f) * 0.75f;

                        float dpsClaw = (1.5f * dmgSwing) / ClawCD;

                        dpsSwing *= (1f - missedSpecial) - 0.24f;   // the Ghoul only has one weapon || Glancings added further down
                        dpsClaw *= 1f - missedSpecial;

                        dpsSwing *= 1f + .054f + stats.LotPCritRating / 4591f; // needs other crit modifiers, but doesn't inherit crit from master
                        dpsSwing += dpsGhoulGlancing;

                        dpsClaw *= 1f + .054f + stats.LotPCritRating / 4591f; // needs other crit modifiers, but doesn't inherit crit from master

                        dpsGhoul = dpsSwing + dpsClaw;

                        //dpsGhoul *= 1f + statsBuffs.BonusPhysicalDamageMultiplier; 
                        // commented out because ghoul doesn't benefit from most bonus physical damage multipliers (ie blood presence, bloody vengeance, etc)
                        int targetArmor = calcOpts.BossArmor;
                        float modArmor = 1f - StatConversion.GetArmorDamageReduction(character.Level, targetArmor,
                            stats.ArmorPenetration, 0f, stats.ArmorPenetrationRating);

                        dpsGhoul *= modArmor;
                        dpsGhoul *= 1f - .0065f;

                        // dpsGhoul *= mitigation;
                        // commented out because ghoul doesn't benefit from master's ArPr
                        dpsGhoul *= uptime;
                    }
                    else
                        dpsGhoul = 0;
                }
                #endregion

                float BCBMult = 1f;
                float BloodPlagueMult = 1f;
                float BloodStrikeMult = 1f;
                float DeathCoilMult = 1f;
                float DancingRuneWeaponMult = 1f;
                float FrostFeverMult = 1f;
                float FrostStrikeMult = 1f;
                float GargoyleMult = 1f + commandMult;
                float GhoulMult = 1f + commandMult;
                float BloodwormsMult = 1f + commandMult;
                float HeartStrikeMult = 1f;
                float HowlingBlastMult = 1f;
                float IcyTouchMult = 1f;
                float NecrosisMult = 1f;
                float ObliterateMult = 1f;
                float DeathStrikeMult = 1f;
                float PlagueStrikeMult = 1f;
                float ScourgeStrikeMult = 1f;
                float UnholyBlightMult = 1f;
                float WhiteMult = 1f;
                float WanderingPlagueMult = 1f;
                float otherShadowMult = 1f;
                float otherArcaneMult = 1f;
                float otherFrostMult = 1f;

                #region Apply Physical Mitigation
                {
                    float physMit = mitigation;
                    //  physMit *= physPowerMult;
                    physMit *= 1f + (!DW ? .02f * talents.TwoHandedWeaponSpecialization : 0f);

                    dpsBCB *= physMit;
                    dpsBloodStrike *= physMit;
                    dpsHeartStrike *= physMit;
                    dpsObliterate *= physMit;
                    dpsDeathStrike *= physMit;
                    dpsPlagueStrike *= physMit;
                    dpsBloodworms *= 1f - StatConversion.GetArmorDamageReduction(character.Level, calcOpts.BossArmor, stats.ArmorPenetration, 0f, 0f);

                    WhiteMult += physPowerMult - 1f;
                    BCBMult += physPowerMult - 1f;
                    BloodStrikeMult += physPowerMult - 1f;
                    HeartStrikeMult += physPowerMult - 1f;
                    ObliterateMult += physPowerMult - 1f;
                    DeathStrikeMult += physPowerMult - 1f;
                    PlagueStrikeMult += physPowerMult - 1f;
                }
                #endregion

                #region Apply Elemental Strike Mitigation
                {
                    float strikeMit = /*missedSpecial **/ partialResist;
                    strikeMit *= (!DW ? 1f + .02f * talents.TwoHandedWeaponSpecialization : 1f);

                    dpsScourgeStrike *= strikeMit;
                    // dpsScourgeStrike *= 1f - combatTable.dodgedSpecial;
                    dpsFrostStrike *= strikeMit * (1f - missedSpecial);

                    ScourgeStrikeMult += spellPowerMult - 1f;
                    FrostStrikeMult += frostSpellPowerMult - 1f;
                }
                #endregion

                #region Apply Magical Mitigation
                {
                    // some of this applies to necrosis, I wonder if it is ever accounted for
                    float magicMit = partialResist /** combatTable.spellResist*/;
                    // magicMit = 1f - magicMit;

                    dpsNecrosis *= magicMit;
                    dpsBloodPlague *= magicMit;
                    dpsDeathCoil *= magicMit * (1 - combatTable.spellResist);
                    dpsFrostFever *= magicMit;
                    dpsHowlingBlast *= magicMit * (1 - combatTable.spellResist);
                    dpsIcyTouch *= magicMit;
                    dpsUnholyBlight *= magicMit * (1 - combatTable.spellResist);


                    NecrosisMult += spellPowerMult - 1f;
                    BloodPlagueMult += spellPowerMult - 1f;
                    DeathCoilMult += spellPowerMult - 1f;
                    FrostFeverMult += frostSpellPowerMult - 1f;
                    HowlingBlastMult += frostSpellPowerMult - 1f;
                    IcyTouchMult += frostSpellPowerMult - 1f;
                    UnholyBlightMult += spellPowerMult - 1f;
                    otherShadowMult += spellPowerMult - 1f;
                    otherArcaneMult += spellPowerMult - 1f;
                    otherFrostMult += frostSpellPowerMult - 1f;
                }
                #endregion

                #region Cinderglacier multipliers
                {
                    DeathCoilMult *= CinderglacierMultiplier;
                    HowlingBlastMult *= CinderglacierMultiplier;
                    IcyTouchMult *= CinderglacierMultiplier;
                    ScourgeStrikeMult *= CinderglacierMultiplier;
                    FrostStrikeMult *= CinderglacierMultiplier;
                }
                #endregion

                #region Apply Multi-Ability Talent Multipliers
                {
                    float BloodyVengeanceMult = .03f * (float)talents.BloodyVengeance;
                    BCBMult *= 1 + BloodyVengeanceMult;
                    BloodStrikeMult *= 1 + BloodyVengeanceMult;
                    HeartStrikeMult *= 1 + BloodyVengeanceMult;
                    ObliterateMult *= 1 + BloodyVengeanceMult;
                    DeathStrikeMult *= 1 + BloodyVengeanceMult;
                    PlagueStrikeMult *= 1 + BloodyVengeanceMult;
                    WhiteMult *= 1 + BloodyVengeanceMult;

                    float HysteriaCoeff = .3f / 6f; // current uptime is 16.666...%
                    float HysteriaMult = HysteriaCoeff * (float)talents.Hysteria;
                    BCBMult *= 1 + HysteriaMult;
                    BloodStrikeMult *= 1 + HysteriaMult;
                    HeartStrikeMult *= 1 + HysteriaMult;
                    ObliterateMult *= 1 + HysteriaMult;
                    DeathStrikeMult *= 1 + HysteriaMult;
                    PlagueStrikeMult *= 1 + HysteriaMult;
                    WhiteMult *= 1 + HysteriaMult;

                    float BlackIceMult = .02f * (float)talents.BlackIce;
                    FrostFeverMult *= 1 + BlackIceMult;
                    HowlingBlastMult *= 1 + BlackIceMult;
                    IcyTouchMult *= 1 + BlackIceMult;
                    FrostStrikeMult *= 1 + BlackIceMult;
                    DeathCoilMult *= 1 + BlackIceMult;
                    ScourgeStrikeMult *= 1 + BlackIceMult;
                    BloodPlagueMult *= 1 + BlackIceMult;
                    otherShadowMult *= 1 + BlackIceMult;
                    otherFrostMult *= 1 + BlackIceMult;

                    float MercilessCombatMult = .315f * 0.06f * (float)talents.MercilessCombat;   // The last 35% of a Boss don't take 35% of the fight-time...say .315 (10% faster)
                    ObliterateMult *= 1 + MercilessCombatMult;
                    HowlingBlastMult *= 1 + MercilessCombatMult;
                    IcyTouchMult *= 1 + MercilessCombatMult;
                    FrostStrikeMult *= 1 + MercilessCombatMult;

                    float GlacierRot = .0666666666666f * (float)talents.GlacierRot;
                    HowlingBlastMult *= 1 + GlacierRot;
                    IcyTouchMult *= 1 + GlacierRot;
                    FrostStrikeMult *= 1 + GlacierRot;


                    float CryptFeverMult = .1f * (float)talents.CryptFever;
                    float CryptFeverBuff = stats.BonusDiseaseDamageMultiplier;
                    CryptFeverMult = Math.Max(CryptFeverMult, CryptFeverBuff);
                    FrostFeverMult *= 1 + CryptFeverMult;
                    BloodPlagueMult *= 1 + CryptFeverMult;
                    UnholyBlightMult *= 1 + CryptFeverMult;

                    /* float DesecrationChance = (calcOpts.rotation.PlagueStrike * 12f) / calcOpts.rotation.curRotationDuration +
                                                (calcOpts.rotation.ScourgeStrike * 12f) / calcOpts.rotation.curRotationDuration;
                     if (DesecrationChance > 1f) DesecrationChance = 1f;*/
                    float DesecrationMult = .01f * (float)talents.Desecration;  //the new desecration is basically a flat 1% per point
                    BCBMult *= 1 + DesecrationMult;
                    BloodPlagueMult *= 1 + DesecrationMult;
                    BloodStrikeMult *= 1 + DesecrationMult;
                    DeathCoilMult *= 1 + DesecrationMult;
                    DancingRuneWeaponMult *= 1 + DesecrationMult;
                    FrostFeverMult *= 1 + DesecrationMult;
                    FrostStrikeMult *= 1 + DesecrationMult;
                    //GargoyleMult *= 1 + DesecrationMult;
                    HeartStrikeMult *= 1 + DesecrationMult;
                    HowlingBlastMult *= 1 + DesecrationMult;
                    IcyTouchMult *= 1 + DesecrationMult;
                    NecrosisMult *= 1 + DesecrationMult;
                    ObliterateMult *= 1 + DesecrationMult;
                    DeathStrikeMult *= 1 + DesecrationMult;
                    PlagueStrikeMult *= 1 + DesecrationMult;
                    ScourgeStrikeMult *= 1 + DesecrationMult;
                    UnholyBlightMult *= 1 + DesecrationMult;
                    WhiteMult *= 1 + DesecrationMult;
                    otherShadowMult *= 1 + DesecrationMult;
                    otherArcaneMult *= 1 + DesecrationMult;
                    otherFrostMult *= 1 + DesecrationMult;

                    if ((float)talents.BoneShield >= 1f)
                    {
                        float BoneMult = .02f;
                        BCBMult *= 1 + BoneMult;
                        BloodPlagueMult *= 1 + BoneMult;
                        BloodStrikeMult *= 1 + BoneMult;
                        DeathCoilMult *= 1 + BoneMult;
                        DancingRuneWeaponMult *= 1 + BoneMult;
                        FrostFeverMult *= 1 + BoneMult;
                        //GargoyleMult *= 1 + BoneMult;
                        FrostStrikeMult *= 1 + BoneMult;
                        HeartStrikeMult *= 1 + BoneMult;
                        HowlingBlastMult *= 1 + BoneMult;
                        IcyTouchMult *= 1 + BoneMult;
                        NecrosisMult *= 1 + BoneMult;
                        ObliterateMult *= 1 + BoneMult;
                        DeathStrikeMult *= 1 + BoneMult;
                        PlagueStrikeMult *= 1 + BoneMult;
                        ScourgeStrikeMult *= 1 + BoneMult;
                        UnholyBlightMult *= 1 + BoneMult;
                        WhiteMult *= 1 + BoneMult;
                        otherShadowMult *= 1 + BoneMult;
                        otherArcaneMult *= 1 + BoneMult;
                        otherFrostMult *= 1 + BoneMult;
                    }
                }
                #endregion

                #region Pet uptime modifiers
                {
                    BloodwormsMult *= calcOpts.BloodwormsUptime;
                    GhoulMult *= calcOpts.GhoulUptime;
                }
                #endregion

                //feed variables for output
                calcs.BCBDPS = dpsBCB * BCBMult;
                calcs.BloodPlagueDPS = dpsBloodPlague * BloodPlagueMult;
                calcs.BloodStrikeDPS = dpsBloodStrike * BloodStrikeMult;
                calcs.DeathCoilDPS = dpsDeathCoil * DeathCoilMult;
                calcs.DRWDPS = dpsDancingRuneWeapon * DancingRuneWeaponMult;
                calcs.FrostFeverDPS = dpsFrostFever * FrostFeverMult;
                calcs.FrostStrikeDPS = dpsFrostStrike * FrostStrikeMult;
                calcs.GargoyleDPS = dpsGargoyle * GargoyleMult;
                calcs.GhoulDPS = dpsGhoul * GhoulMult;
                calcs.BloodwormsDPS = dpsBloodworms * BloodwormsMult;
                calcs.HeartStrikeDPS = dpsHeartStrike * HeartStrikeMult;
                calcs.HowlingBlastDPS = dpsHowlingBlast * HowlingBlastMult;
                calcs.IcyTouchDPS = dpsIcyTouch * IcyTouchMult;
                calcs.NecrosisDPS = dpsNecrosis * NecrosisMult;
                calcs.ObliterateDPS = dpsObliterate * ObliterateMult;
                calcs.DeathStrikeDPS = dpsDeathStrike * DeathStrikeMult;
                calcs.PlagueStrikeDPS = dpsPlagueStrike * PlagueStrikeMult;
                calcs.ScourgeStrikeDPS = dpsScourgeStrike * ScourgeStrikeMult;
                calcs.UnholyBlightDPS = dpsUnholyBlight * UnholyBlightMult;
                calcs.WhiteDPS = dpsWhite * WhiteMult;
                calcs.WanderingPlagueDPS = dpsWanderingPlague * WanderingPlagueMult;
                calcs.OtherDPS = dpsOtherShadow * otherShadowMult +
                    dpsOtherArcane * otherArcaneMult +
                    dpsOtherFrost * otherFrostMult;


                calcs.DPSPoints = calcs.BCBDPS + calcs.BloodPlagueDPS + calcs.BloodStrikeDPS + calcs.DeathCoilDPS + calcs.FrostFeverDPS + calcs.FrostStrikeDPS +
                                  calcs.GargoyleDPS + calcs.GhoulDPS + calcs.WanderingPlagueDPS + calcs.HeartStrikeDPS + calcs.HowlingBlastDPS + calcs.IcyTouchDPS +
                                  calcs.NecrosisDPS + calcs.ObliterateDPS + calcs.DeathStrikeDPS + calcs.PlagueStrikeDPS + calcs.ScourgeStrikeDPS + calcs.UnholyBlightDPS +
                                  calcs.WhiteDPS + calcs.OtherDPS + calcs.BloodwormsDPS;

                #region Dancing Rune Weapon
                {
                    if (talents.DancingRuneWeapon > 0)
                    {
                        float DRWUptime = (5f + (1.5f * talents.RunicPowerMastery) + (talents.GlyphofDancingRuneWeapon ? 5f : 0)) / 90f;
                        dpsDancingRuneWeapon = (calcs.DPSPoints - calcs.GhoulDPS - calcs.BloodwormsDPS) * DRWUptime;
                        dpsDancingRuneWeapon *= 0.5f; // "doing the same attacks as the Death Knight but for 50% reduced damage."
                        calcs.DPSPoints += dpsDancingRuneWeapon;
                        calcs.DRWDPS = dpsDancingRuneWeapon;
                    }
                }
                #endregion

                calcs.OverallPoints = calcs.DPSPoints;
            }
            #endregion

            #region 3.2 PTR
            else
            {
                calcOpts.presence = calcOpts.rotation.presence;

                if (calcOpts.rotation.managedRP)
                {
                    calcOpts.rotation.getRP(talents, character);
                }

                #region Impurity Application
                {
                    float impurityMult = 1f + (.04f * (float)talents.Impurity);

                    HowlingBlastAPMult *= impurityMult;
                    IcyTouchAPMult *= impurityMult;
                    FrostFeverAPMult *= impurityMult;
                    BloodPlagueAPMult *= impurityMult;
                    DeathCoilAPMult *= impurityMult;
                    UnholyBlightAPMult *= impurityMult;
                    GargoyleAPMult *= impurityMult;
                }
                #endregion

                #region racials
                {
                    if (character.Race == Character.CharacterRace.Orc)
                    {
                        commandMult += .05f;
                    }
                }
                #endregion

                #region Killing Machine
                {
                    float KMPpM = (1f * talents.KillingMachine) * (1f + (StatConversion.GetHasteFromRating(stats.HasteRating, Character.CharacterClass.DeathKnight))); // KM Procs per Minute (Defined "1 per point" by Blizzard) influenced by Phys. Haste
                    float addHastePercent = 1f;


                    /*   if (calcOpts.Bloodlust)
                       {
                           float numLust = fightDuration % 600f;  // bloodlust changed in 3.0, can only have one every 10 minutes.
                           float fullLustDur = (numLust - 1) * 600f + 40f;
                           if (fightDuration < fullLustDur) // if the last lust doesn't go its full duration
                           {
                               float lastLustFraction = (fullLustDur - fightDuration) / 40f;
                               numLust -= 1f;
                               numLust += lastLustFraction;
                           }

                           float bloodlustUptime = (numLust * 40f) / fightDuration;

                           addHastePercent += 0.3f * bloodlustUptime;
                       }

                       if (calcOpts.talents.ImprovedIcyTalons != 0)
                       {
                           addHastePercent += 0.05f;
                       }*/

                    addHastePercent += stats.PhysicalHaste;

                    KMPpM *= addHastePercent;

                    float KMPpR = KMPpM / (60 / calcOpts.rotation.curRotationDuration);
                    float totalAbilities = calcOpts.rotation.FrostStrike + calcOpts.rotation.IcyTouch + calcOpts.rotation.HowlingBlast;
                    KMRatio = KMPpR / totalAbilities;
                }
                #endregion

                #region Cinderglacier
                {
                    /*    if (character.GetEnchantBySlot(Character.CharacterSlot.MainHand) != null && character.GetEnchantBySlot(Character.CharacterSlot.MainHand).Id == 3369)
                        {
                            float shadowFrostAbilitiesPerSecond = (calcOpts.rotation.DeathCoil + calcOpts.rotation.FrostStrike +
                                calcOpts.rotation.ScourgeStrike + calcOpts.rotation.IcyTouch + calcOpts.rotation.HowlingBlast) / combatTable.realDuration;
                            SpecialEffect temp = new SpecialEffect(Trigger.Use, new Stats(), 0, -1f);
                            float avgPPS = temp.GetAverageUptime(combatTable.MH.hastedSpeed, 1f - calcs.AvoidedAttacks, combatTable.MH.baseSpeed, calcOpts.FightLength * 60f)/60;
                            CinderglacierMultiplier *= 1f + (avgPPS * 2) / shadowFrostAbilitiesPerSecond;
                        }
                        if (character.GetEnchantBySlot(Character.CharacterSlot.OffHand) != null && character.GetEnchantBySlot(Character.CharacterSlot.OffHand).Id == 3369)
                        {
                            float shadowFrostAbilitiesPerSecond = (calcOpts.rotation.DeathCoil + calcOpts.rotation.FrostStrike +
                                calcOpts.rotation.ScourgeStrike + calcOpts.rotation.IcyTouch + calcOpts.rotation.HowlingBlast) / combatTable.realDuration;
                            SpecialEffect temp = new SpecialEffect(Trigger.Use, new Stats(), 0, -1f);
                            float avgPPS = temp.GetAverageUptime(combatTable.MH.hastedSpeed, 1f - calcs.AvoidedAttacks, combatTable.MH.baseSpeed, calcOpts.FightLength * 60f) / 60;
                            CinderglacierMultiplier *= 1f + (avgPPS * 2) / shadowFrostAbilitiesPerSecond;
                        }*/
                    float shadowFrostAbilitiesPerSecond = ((calcOpts.rotation.DeathCoil + calcOpts.rotation.FrostStrike +
                            calcOpts.rotation.ScourgeStrike + calcOpts.rotation.IcyTouch + calcOpts.rotation.HowlingBlast) /
                            combatTable.realDuration);

                    CinderglacierMultiplier *= 1f + (0.2f / (shadowFrostAbilitiesPerSecond / stats.CinderglacierProc));

                }
                #endregion

                #region Mitigation
                {
                    float targetArmor = calcOpts.BossArmor, totalArP = stats.ArmorPenetration;

                    //// Effective armor after ArP

                    //targetArmor -= totalArP;
                    //float ratingCoeff = stats.ArmorPenetrationRating / 1539f;
                    //targetArmor *= (1 - ratingCoeff);
                    //if (targetArmor < 0) targetArmor = 0f;

                    //// Convert armor to mitigation

                    ////mitigation = 1f - (targetArmor/(targetArmor + 10557.5f));

                    ////mitigation = 1f - targetArmor / (targetArmor + 400f + 85f * (5.5f * (float)calcOpts.TargetLevel - 265.5f));

                    //mitigation = 1f - (targetArmor / ((467.5f * (float)calcOpts.TargetLevel) + targetArmor - 22167.5f));

                    mitigation = 1f - StatConversion.GetArmorDamageReduction(character.Level, targetArmor,
                    stats.ArmorPenetration, 0f, stats.ArmorPenetrationRating);

                    calcs.EnemyMitigation = 1f - mitigation;
                    calcs.EffectiveArmor = mitigation;
                }
                #endregion

                #region White Dmg
                {
                    float MHDPS = 0f, OHDPS = 0f;
                    #region Main Hand
                    {
                        float dpsMHglancing = (0.24f * combatTable.MH.DPS) * 0.75f;
                        float dpsMHBeforeArmor = ((combatTable.MH.DPS * (1f - calcs.AvoidedAttacks - 0.24f)) * (1f + combatTable.physCrits)) + dpsMHglancing;
                        dpsWhiteMinusGlancing = dpsMHBeforeArmor - dpsMHglancing;
                        dpsWhiteBeforeArmor = dpsMHBeforeArmor;
                        MHDPS = dpsMHBeforeArmor * mitigation;
                    }
                    #endregion

                    #region Off Hand
                    if (DW || (character.MainHand == null && character.OffHand != null))
                    {
                        float dpsOHglancing = (0.24f * combatTable.OH.DPS) * 0.75f;
                        float dpsOHBeforeArmor = ((combatTable.OH.DPS * (1f - calcs.AvoidedAttacks - 0.24f)) * (1f + combatTable.physCrits)) + dpsOHglancing;
                        dpsWhiteMinusGlancing += dpsOHBeforeArmor - dpsOHglancing;
                        dpsWhiteBeforeArmor += dpsOHBeforeArmor;
                        OHDPS = dpsOHBeforeArmor * mitigation;
                    }
                    #endregion

                    dpsWhite = MHDPS + OHDPS;
                }
                #endregion

                #region Necrosis
                {
                    dpsNecrosis = dpsWhiteMinusGlancing * (.04f * (float)talents.Necrosis); // doesn't proc off Glancings
                }
                #endregion

                #region Blood Caked Blade
                {
                    float dpsMHBCB = 0f;
                    float dpsOHBCB = 0f;
                    if ((combatTable.OH.damage != 0) && (DW || combatTable.MH.damage == 0))
                    {
                        float OHBCBDmg = combatTable.OH.damage * (.25f + .125f * calcOpts.rotation.avgDiseaseMult);
                        dpsOHBCB = OHBCBDmg / combatTable.OH.hastedSpeed;
                    }
                    if (combatTable.MH.damage != 0)
                    {
                        float MHBCBDmg = combatTable.MH.damage * (.25f + .125f * calcOpts.rotation.avgDiseaseMult);
                        dpsMHBCB = MHBCBDmg / combatTable.MH.hastedSpeed;
                    }
                    dpsBCB = dpsMHBCB + dpsOHBCB;
                    dpsBCB *= .1f * (float)talents.BloodCakedBlade;
                }
                #endregion

                #region Death Coil
                {
                    if (calcOpts.rotation.DeathCoil > 0f)
                    {
                        float DCCD = combatTable.realDuration / (calcOpts.rotation.DeathCoil + (0.05f * (float)talents.SuddenDoom * calcOpts.rotation.HeartStrike));
                        float DCDmg = 443f + (DeathCoilAPMult * stats.AttackPower) + stats.BonusDeathCoilDamage;
                        dpsDeathCoil = DCDmg / DCCD;
                        float DCCritDmgMult = .5f * (2f + stats.CritBonusDamage);
                        float DCCrit = 1f + ((combatTable.spellCrits + stats.BonusDeathCoilCrit) * DCCritDmgMult);
                        dpsDeathCoil *= DCCrit;

                        //sudden doom stuff
                        // this section is no longer relevant after 3.1.x changes
                        /* float affectedDCMult = calcOpts.rotation.BloodStrike + calcOpts.rotation.HeartStrike;
                         affectedDCMult *= .04f * (float)talents.SuddenDoom;
                         affectedDCMult /= calcOpts.rotation.DeathCoil;
                         dpsDeathCoil += dpsDeathCoil * affectedDCMult;*/

                        dpsDeathCoil *= 1f + (.05f * (float)talents.Morbidity) + (talents.GlyphofDarkDeath ? .15f : 0f);
                    }
                }
                #endregion

                #region Icy Touch
                // this seems to handle crit strangely.
                // additionally, looks like it's missing some multipliers? maybe they're applied later
                {
                    if (calcOpts.rotation.IcyTouch > 0f)
                    {
                        float addedCritFromKM = KMRatio;
                        float ITCD = combatTable.realDuration / calcOpts.rotation.IcyTouch;
                        float ITDmg = 236f + (IcyTouchAPMult * stats.AttackPower) + stats.BonusIcyTouchDamage;
                        ITDmg *= 1f + .1f * (float)talents.ImprovedIcyTouch;
                        dpsIcyTouch = ITDmg / ITCD;
                        float ITCritDmgMult = .5f * (2f + stats.CritBonusDamage);
                        float ITCrit = 1f + ((combatTable.spellCrits + addedCritFromKM + (.05f * (float)talents.Rime)) * ITCritDmgMult);
                        dpsIcyTouch *= ITCrit;
                    }
                }
                #endregion

                #region Plague Strike
                {
                    if (calcOpts.rotation.PlagueStrike > 0f)
                    {
                        float PSCD = combatTable.realDuration / calcOpts.rotation.PlagueStrike;
                        float PSDmg = (combatTable.MH.baseDamage + ((stats.AttackPower / 14f) * combatTable.normalizationFactor)) * .5f + 189f;
                        dpsPlagueStrike = PSDmg / PSCD;
                        float PSCritDmgMult = 1f + (.15f * (float)talents.ViciousStrikes);
                        float PSCrit = 1f + ((combatTable.physCrits + (.03f * (float)talents.ViciousStrikes) + stats.BonusPlagueStrikeCrit) * PSCritDmgMult);
                        dpsPlagueStrike *= PSCrit;

                        dpsPlagueStrike *= (talents.GlyphofPlagueStrike ? 1.2f : 1f);

                        dpsPlagueStrike *= 1f + (.1f * (float)talents.Outbreak);
                    }
                }
                #endregion

                #region Frost Fever
                {
                    if (calcOpts.rotation.IcyTouch > 0f || (talents.GlyphofHowlingBlast && calcOpts.rotation.HowlingBlast > 0f) || (talents.GlyphofDisease))
                    {
                        // Frost Fever is renewed with every Icy Touch and starts a new cd
                        float ITCD = calcOpts.rotation.curRotationDuration / (calcOpts.rotation.IcyTouch + (talents.GlyphofHowlingBlast ? calcOpts.rotation.HowlingBlast : 0f));
                        float FFCD = 3f / (calcOpts.rotation.diseaseUptime / 100);
                        int tempF = (int)Math.Floor(ITCD / FFCD);
                        FFCD = ((ITCD - ((float)tempF * FFCD)) / ((float)tempF + 1f)) + FFCD;
                        float FFDmg = FrostFeverAPMult * stats.AttackPower + 25.6f;
                        dpsFrostFever = FFDmg / FFCD;
                        dpsFrostFever *= 1.15f;
                        //dpsFrostFever *= 1f + combatTable.spellCrits;                            // Frost Fever can't crit

                        //dpsWPFromFF = FFDmg / (FFCD / combatTable.physCrits);
                        dpsWPFromFF = dpsFrostFever * combatTable.physCrits;
                    }
                }
                #endregion

                #region Blood Plague
                {
                    if (calcOpts.rotation.PlagueStrike > 0f || talents.GlyphofPestilence)
                    {
                        // Blood Plague is renewed with every Plague Strike and starts a new cd
                        float PSCD = calcOpts.rotation.curRotationDuration / calcOpts.rotation.PlagueStrike;
                        float BPCD = 3f / (calcOpts.rotation.diseaseUptime / 100);
                        int tempF = (int)Math.Floor(PSCD / BPCD);
                        BPCD = ((PSCD - ((float)tempF * BPCD)) / ((float)tempF + 1f)) + BPCD;
                        float BPDmg = BloodPlagueAPMult * stats.AttackPower + 31.1f;
                        dpsBloodPlague = BPDmg / BPCD;
                        dpsBloodPlague *= 1.15f;
                        //dpsBloodPlague *= 1f + combatTable.spellCrits;                           // Blood Plague can't crit

                        //dpsWPFromBP = BPDmg / (BPCD / combatTable.physCrits);
                        dpsWPFromBP = dpsBloodPlague * combatTable.physCrits;
                    }
                }
                #endregion

                #region Wandering Plague
                {
                    dpsWanderingPlague = dpsWPFromBP + dpsWPFromFF;
                    dpsWanderingPlague *= (1f / 3f) * (float)talents.WanderingPlague;
                }
                #endregion

                #region Scourge Strike
                {
                    if (talents.ScourgeStrike > 0 && calcOpts.rotation.ScourgeStrike > 0f)
                    {
                        float SSCD = combatTable.realDuration / calcOpts.rotation.ScourgeStrike;
                        float SSDmg = ((combatTable.MH.baseDamage + ((stats.AttackPower / 14f) * combatTable.normalizationFactor)) * .45f) + 357.188f +
                            stats.BonusScourgeStrikeDamage;
                        SSDmg *= 1f + 0.11f * calcOpts.rotation.avgDiseaseMult * (1f + stats.BonusPerDiseaseScourgeStrikeDamage);
                        dpsScourgeStrike = SSDmg / SSCD;
                        float SSCritDmgMult = 1f + (.15f * (float)talents.ViciousStrikes) + stats.CritBonusDamage;
                        float SSCrit = 1f + ((combatTable.physCrits + (.03f * (float)talents.ViciousStrikes) + stats.BonusScourgeStrikeCrit) * SSCritDmgMult);
                        dpsScourgeStrike = dpsScourgeStrike * SSCrit;
                        dpsScourgeStrike *= 1f + (.0666666666666666666f * (float)talents.Outbreak);
                    }
                }
                #endregion

                #region Unholy Blight
                {
                    //The cooldown on this 1 second and I assume 100% uptime
                    dpsUnholyBlight = dpsDeathCoil * (0.3f * (1f + (talents.GlyphofUnholyBlight ? 0.4f : 0f)));
                }
                #endregion

                #region Frost Strike
                {
                    if (talents.FrostStrike > 0 && calcOpts.rotation.FrostStrike > 0f)
                    {
                        float addedCritFromKM = KMRatio;
                        float FSCD = combatTable.realDuration / calcOpts.rotation.FrostStrike;
                        float FSDmg = (combatTable.MH.baseDamage + ((stats.AttackPower / 14f) * combatTable.normalizationFactor)) * .55f +
                            150f + stats.BonusFrostStrikeDamage;
                        dpsFrostStrike = FSDmg / FSCD;
                        float FSCritDmgMult = 1f + (.15f * (float)talents.GuileOfGorefiend) + stats.CritBonusDamage;
                        float FSCrit = 1f + ((combatTable.physCrits + addedCritFromKM + stats.BonusFrostStrikeCrit) * FSCritDmgMult);
                        dpsFrostStrike *= FSCrit;
                        dpsFrostStrike *= 1f + .03f * talents.BloodOfTheNorth;
                    }
                }
                #endregion

                #region Bloodworms
                {
                    if (talents.Bloodworms > 0)
                    {
                        float BloodwormSwing = 50f + BloodwormsAPMult * stats.AttackPower;
                        float BloodwormSwingDPS = BloodwormSwing / 2.0f;    // any haste benefits?
                        float TotalBloodworms = ((fightDuration / combatTable.MH.hastedSpeed) + calcOpts.rotation.getMeleeSpecialsPerSecond() * fightDuration)
                            * (0.03f * talents.Bloodworms)
                            * 3f /*average of 3 bloodworms per proc*/;
                        dpsBloodworms = ((TotalBloodworms * BloodwormSwingDPS * 20) / fightDuration);
                    }
                }
                #endregion

                #region Trinket direct-damage procs, razorice damage, etc
                {
                    dpsOtherArcane = stats.ArcaneDamage;
                    dpsOtherShadow = stats.ShadowDamage;

                    if (combatTable.MH != null)
                    {
                        float dpsMHglancing = (0.24f * combatTable.MH.DPS) * 0.75f;
                        float dpsMHBeforeArmor = ((combatTable.MH.DPS * (1f - calcs.AvoidedAttacks - 0.24f)) * (1f + combatTable.physCrits)) + dpsMHglancing;
                        dpsOtherFrost += (dpsMHBeforeArmor - dpsMHglancing) * stats.BonusFrostWeaponDamage;   // presumably doesn't proc off of glancings, like necrosis
                    }

                    if (combatTable.OH != null)
                    {
                        float dpsOHglancing = (0.24f * combatTable.OH.DPS) * 0.75f;
                        float dpsOHBeforeArmor = ((combatTable.OH.DPS * (1f - calcs.AvoidedAttacks - 0.24f)) * (1f + combatTable.physCrits)) + dpsOHglancing;
                        dpsOtherFrost += (dpsOHBeforeArmor - dpsOHglancing) * stats.BonusFrostWeaponDamage;
                    }

                    float OtherCritDmgMult = .5f * (1f + stats.CritBonusDamage);
                    float OtherCrit = 1f + ((combatTable.spellCrits) * OtherCritDmgMult);
                    dpsOtherArcane *= OtherCrit;
                    dpsOtherShadow *= OtherCrit;
                }
                #endregion

                #region Howling Blast
                {
                    if (talents.HowlingBlast > 0 && calcOpts.rotation.HowlingBlast > 0f)
                    {
                        float addedCritFromKM = KMRatio;
                        float HBCD = combatTable.realDuration / calcOpts.rotation.HowlingBlast;
                        float HBDmg = 540 + HowlingBlastAPMult * stats.AttackPower;
                        dpsHowlingBlast = HBDmg / HBCD;
                        float HBCritDmgMult = .5f * (2f + (.15f * (float)talents.GuileOfGorefiend) + stats.CritBonusDamage);
                        float HBCrit = 1f + ((combatTable.spellCrits + addedCritFromKM) * HBCritDmgMult);
                        dpsHowlingBlast *= HBCrit;
                    }
                }
                #endregion

                #region Obliterate
                {
                    if (calcOpts.rotation.Obliterate > 0f)
                    {
                        // this is missing +crit chance from rime
                        float OblitCD = combatTable.realDuration / calcOpts.rotation.Obliterate;
                        float OblitDmg = ((combatTable.MH.baseDamage + ((stats.AttackPower / 14f) * combatTable.normalizationFactor)) *
                            0.8f) + stats.BonusObliterateDamage;
                        OblitDmg *= 1f + 0.125f * (float)calcOpts.rotation.avgDiseaseMult * (1f + stats.BonusPerDiseaseObliterateDamage);
                        dpsObliterate = OblitDmg / OblitCD;
                        //float OblitCrit = 1f + combatTable.physCrits + ( .03f * (float)talents.Subversion );
                        //OblitCrit += .05f * (float)talents.Rime;
                        float OblitCritDmgMult = 1f + (.15f * (float)talents.GuileOfGorefiend) + stats.CritBonusDamage;
                        float OblitCrit = 1f + ((combatTable.physCrits +
                            (.03f * (float)talents.Subversion) +
                            (0.05f * (float)talents.Rime) +
                            stats.BonusObliterateCrit) * OblitCritDmgMult);
                        dpsObliterate *= OblitCrit;
                        dpsObliterate *= (talents.GlyphofObliterate ? 1.2f : 1f);
                    }
                }
                #endregion

                #region Death Strike
                {
                    if (calcOpts.rotation.DeathStrike > 0f)
                    {
                        // TODO: this is missing +crit chance from rime
                        float DSCD = combatTable.realDuration / calcOpts.rotation.DeathStrike;
                        // TODO: This should be changed to make use of the new glyph stats:
                        float DSDmg = ((combatTable.MH.baseDamage + ((stats.AttackPower / 14f) * combatTable.normalizationFactor)) * 0.75f) + 222.75f + stats.BonusDeathStrikeDamage;
                        DSDmg *= 1f + 0.15f * (float)talents.ImprovedDeathStrike;
                        DSDmg *= (talents.GlyphofDeathStrike ? 1.25f : 1f);
                        dpsDeathStrike = DSDmg / DSCD;
                        float DSCritDmgMult = 1f + (.15f * (float)talents.MightOfMograine) + stats.CritBonusDamage;
                        float DSCrit = 1f + ((combatTable.physCrits +
                            (.03f * (float)talents.ImprovedDeathStrike) +
                            stats.BonusDeathStrikeCrit) * DSCritDmgMult);
                        dpsDeathStrike *= DSCrit;
                    }
                }
                #endregion

                #region Blood Strike
                {
                    if (calcOpts.rotation.BloodStrike > 0f)
                    {
                        float BSCD = combatTable.realDuration / calcOpts.rotation.BloodStrike;
                        float BSDmg = ((combatTable.MH.baseDamage + ((stats.AttackPower / 14f) * combatTable.normalizationFactor)) *
                            0.4f) + 305.6f + stats.BonusBloodStrikeDamage;
                        BSDmg *= 1f + 0.25f * (float)calcOpts.rotation.avgDiseaseMult * (1f + stats.BonusPerDiseaseBloodStrikeDamage);
                        dpsBloodStrike = BSDmg / BSCD;
                        float BSCritDmgMult = 1f + (.15f * (float)talents.MightOfMograine);
                        BSCritDmgMult += (.15f * (float)talents.GuileOfGorefiend) + stats.CritBonusDamage;
                        float BSCrit = 1f + ((combatTable.physCrits + (.03f * (float)talents.Subversion)) * BSCritDmgMult);
                        dpsBloodStrike = (dpsBloodStrike) * BSCrit;
                        dpsBloodStrike *= 1f + (.03f * (float)talents.BloodOfTheNorth);
                        dpsBloodStrike *= 1f + (.15f * (float)talents.BloodyStrikes);
                    }
                }
                #endregion

                #region Heart Strike
                {
                    if (talents.HeartStrike > 0 && calcOpts.rotation.HeartStrike > 0f)
                    {
                        float HSCD = combatTable.realDuration / calcOpts.rotation.HeartStrike;
                        float HSDmg = ((combatTable.MH.baseDamage + ((stats.AttackPower / 14f) * combatTable.normalizationFactor)) *
                            0.5f) + 368f + stats.BonusHeartStrikeDamage;
                        HSDmg *= 1f + 0.1f * (float)calcOpts.rotation.avgDiseaseMult * (1f + stats.BonusPerDiseaseHeartStrikeDamage);
                        dpsHeartStrike = HSDmg / HSCD;
                        //float HSCrit = 1f + combatTable.physCrits + ( .03f * (float)talents.Subversion );
                        float HSCritDmgMult = 1f + (.15f * (float)talents.MightOfMograine) + stats.CritBonusDamage;
                        float HSCrit = 1f + ((combatTable.physCrits + (.03f * (float)talents.Subversion)) * HSCritDmgMult);
                        dpsHeartStrike = (dpsHeartStrike) * HSCrit;
                        dpsHeartStrike *= 1f + (.15f * (float)talents.BloodyStrikes);
                    }
                }
                #endregion

                #region Gargoyle
                {
                    if (calcOpts.rotation.GargoyleDuration > 0f && talents.SummonGargoyle > 0f)
                    {
                        float GargoyleCastTime = 2.4f;
                        if (stats.Bloodlust > 0)
                        {
                            GargoyleCastTime *= .7f;
                        }

                        float GargoyleStrike = GargoyleAPMult * stats.AttackPower;
                        float GargoyleStrikeCount = calcOpts.rotation.GargoyleDuration / GargoyleCastTime;
                        float GargoyleDmg = GargoyleStrike * GargoyleStrikeCount;
                        GargoyleDmg *= 1f + (.5f * combatTable.spellCrits);  // Gargoyle does 150% crits apparently
                        dpsGargoyle = GargoyleDmg / 180f;
                    }
                }
                #endregion

                #region Ghoul
                {
                    if (calcOpts.Ghoul)
                    {
                        float uptime = 1f;
                        if (talents.MasterOfGhouls == 0)
                        {
                            float timeleft = calcOpts.FightLength * 60;
                            float numCDs = timeleft / (5 * 60);
                            float duration = numCDs * 120f;

                            uptime = duration / timeleft;
                            /* // not quite sure what this was supposed to do...
                            timeleft -= 300f;
                            if (timeleft > 120f)
                            {
                                uptime = 240f / calcOpts.FightLength;
                            }
                            else
                            {
                                uptime = (timeleft + 120f) / calcOpts.FightLength;
                            }
                            */
                        }

                        float GhoulBaseStrength = 331f;
                        float GhoulBaseStrengthMult = 0.7f;
                        float GhoulBaseAP = 836f;
                        float GhoulBaseSpeed = 2f;
                        float GhoulAPMult = 1f;                 // 1 Str = x AP     
                        float ClawCD = 4f;
                        float GhoulWeaponBaseDamage = 101.3f;   // I found these values to be fairly correct after a number of tests (v3.0.8)
                        float GhoulAPdivisor = 16.7f;           // 

                        float GlyphofGhoulValue = 0f;
                        if (talents.GlyphoftheGhoul) GlyphofGhoulValue = 0.4f;

                        float GhoulStrengthMult = GhoulBaseStrengthMult + (GhoulBaseStrengthMult * (0.2f * (float)talents.RavenousDead)) + GlyphofGhoulValue;
                        float GhoulStrength = GhoulBaseStrength + stats.Strength * GhoulStrengthMult;

                        Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

                        GhoulStrength *= 1f + statsBuffs.BonusStrengthMultiplier;
                        GhoulStrength += statsBuffs.Strength;

                        if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Flask of Chromatic Wonder"))) GhoulStrength -= 18f;
                        if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Flask of Chromatic Wonder (Mixology)"))) GhoulStrength -= 4f;
                        if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Elixir of Mighty Strength"))) GhoulStrength -= 50f;
                        if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Elixir of Mighty Strength (Mixology)"))) GhoulStrength -= 16f;
                        if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Guru's Elixir"))) GhoulStrength -= 20f;
                        if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Guru's Elixir (Mixology)"))) GhoulStrength -= 6f;
                        if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Scroll of Strength VIII"))) GhoulStrength -= 30f;

                        float GhoulAP = GhoulBaseAP + GhoulStrength * GhoulAPMult;

                        GhoulAP *= 1f + statsBuffs.BonusAttackPowerMultiplier;
                        GhoulAP += statsBuffs.AttackPower;

                        if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Flask of Endless Rage"))) GhoulAP -= 180f;
                        if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Flask of Endless Rage (Mixology)"))) GhoulAP -= 64f;
                        if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Wrath Elixir"))) GhoulAP -= 90f;
                        if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Wrath Elixir (Mixology)"))) GhoulAP -= 32f;
                        if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Attack Power Food"))) GhoulAP -= 80f;
                        if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Fish Feast"))) GhoulAP -= 80f;

                        float GhoulhastedSpeed = 0f;
                        if (combatTable.MH != null)
                            GhoulhastedSpeed = ((combatTable.MH.hastedSpeed == 0 ? 1.0f : combatTable.MH.hastedSpeed) / (combatTable.MH.baseSpeed == 0 ? 1.0f : combatTable.MH.baseSpeed)) * GhoulBaseSpeed;
                        else
                            GhoulhastedSpeed = (2.0f) * GhoulBaseSpeed;
                        GhoulhastedSpeed /= 1f + statsBuffs.PhysicalHaste;

                        float dmgSwing = GhoulWeaponBaseDamage + (GhoulAP / GhoulAPdivisor) * GhoulBaseSpeed;
                        float dpsSwing = dmgSwing / GhoulhastedSpeed;
                        float dpsGhoulGlancing = (dpsSwing * 0.24f) * 0.75f;

                        float dpsClaw = (1.5f * dmgSwing) / ClawCD;

                        dpsSwing *= (1f - missedSpecial) - 0.24f;   // the Ghoul only has one weapon || Glancings added further down
                        dpsClaw *= 1f - missedSpecial;

                        dpsSwing *= 1f + .054f + stats.LotPCritRating / 4591f; // needs other crit modifiers, but doesn't inherit crit from master
                        dpsSwing += dpsGhoulGlancing;

                        dpsClaw *= 1f + .054f + stats.LotPCritRating / 4591f; // needs other crit modifiers, but doesn't inherit crit from master

                        dpsGhoul = dpsSwing + dpsClaw;

                        //dpsGhoul *= 1f + statsBuffs.BonusPhysicalDamageMultiplier; 
                        // commented out because ghoul doesn't benefit from most bonus physical damage multipliers (ie blood presence, bloody vengeance, etc)
                        int targetArmor = calcOpts.BossArmor;
                        float modArmor = 1f - StatConversion.GetArmorDamageReduction(character.Level, targetArmor,
                            stats.ArmorPenetration, 0f, stats.ArmorPenetrationRating);

                        dpsGhoul *= modArmor;
                        dpsGhoul *= 1f - .0065f;

                        // dpsGhoul *= mitigation;
                        // commented out because ghoul doesn't benefit from master's ArPr
                        dpsGhoul *= uptime;
                    }
                    else
                        dpsGhoul = 0;
                }
                #endregion

                float BCBMult = 1f;
                float BloodPlagueMult = 1f;
                float BloodStrikeMult = 1f;
                float DeathCoilMult = 1f;
                float DancingRuneWeaponMult = 1f;
                float FrostFeverMult = 1f;
                float FrostStrikeMult = 1f;
                float GargoyleMult = 1f + commandMult;
                float GhoulMult = 1f + commandMult;
                float BloodwormsMult = 1f + commandMult;
                float HeartStrikeMult = 1f;
                float HowlingBlastMult = 1f;
                float IcyTouchMult = 1f;
                float NecrosisMult = 1f;
                float ObliterateMult = 1f;
                float DeathStrikeMult = 1f;
                float PlagueStrikeMult = 1f;
                float ScourgeStrikeMult = 1f;
                float UnholyBlightMult = 1f;
                float WhiteMult = 1f;
                float WanderingPlagueMult = 1f;
                float otherShadowMult = 1f;
                float otherArcaneMult = 1f;
                float otherFrostMult = 1f;

                #region Apply Physical Mitigation
                {
                    float physMit = mitigation;
                    //  physMit *= physPowerMult;
                    physMit *= 1f + (!DW ? .02f * talents.TwoHandedWeaponSpecialization : 0f);

                    dpsBCB *= physMit;
                    dpsBloodStrike *= physMit;
                    dpsHeartStrike *= physMit;
                    dpsObliterate *= physMit;
                    dpsDeathStrike *= physMit;
                    dpsPlagueStrike *= physMit;
                    dpsBloodworms *= 1f - StatConversion.GetArmorDamageReduction(character.Level, calcOpts.BossArmor, stats.ArmorPenetration, 0f, 0f);

                    WhiteMult += physPowerMult - 1f;
                    BCBMult += physPowerMult - 1f;
                    BloodStrikeMult += physPowerMult - 1f;
                    HeartStrikeMult += physPowerMult - 1f;
                    ObliterateMult += physPowerMult - 1f;
                    DeathStrikeMult += physPowerMult - 1f;
                    PlagueStrikeMult += physPowerMult - 1f;
                }
                #endregion

                #region Apply Elemental Strike Mitigation
                {
                    float strikeMit = /*missedSpecial **/ partialResist;
                    strikeMit *= (!DW ? 1f + .02f * talents.TwoHandedWeaponSpecialization : 1f);

                    dpsScourgeStrike *= strikeMit;
                    // dpsScourgeStrike *= 1f - combatTable.dodgedSpecial;
                    dpsFrostStrike *= strikeMit * (1f - missedSpecial) * (1f - combatTable.dodgedSpecial);

                    ScourgeStrikeMult += spellPowerMult - 1f;
                    FrostStrikeMult += frostSpellPowerMult - 1f;
                }
                #endregion

                #region Apply Magical Mitigation
                {
                    // some of this applies to necrosis, I wonder if it is ever accounted for
                    float magicMit = partialResist /** combatTable.spellResist*/;
                    // magicMit = 1f - magicMit;

                    dpsNecrosis *= magicMit;
                    dpsBloodPlague *= magicMit;
                    dpsDeathCoil *= magicMit * (1 - combatTable.spellResist);
                    dpsFrostFever *= magicMit;
                    dpsHowlingBlast *= magicMit * (1 - combatTable.spellResist);
                    dpsIcyTouch *= magicMit;
                    dpsUnholyBlight *= magicMit * (1 - combatTable.spellResist);


                    NecrosisMult += spellPowerMult - 1f;
                    BloodPlagueMult += spellPowerMult - 1f;
                    DeathCoilMult += spellPowerMult - 1f;
                    FrostFeverMult += frostSpellPowerMult - 1f;
                    HowlingBlastMult += frostSpellPowerMult - 1f;
                    IcyTouchMult += frostSpellPowerMult - 1f;
                    UnholyBlightMult += spellPowerMult - 1f;
                    otherShadowMult += spellPowerMult - 1f;
                    otherArcaneMult += spellPowerMult - 1f;
                    otherFrostMult += frostSpellPowerMult - 1f;
                }
                #endregion

                #region Cinderglacier multipliers
                {
                    DeathCoilMult *= CinderglacierMultiplier;
                    HowlingBlastMult *= CinderglacierMultiplier;
                    IcyTouchMult *= CinderglacierMultiplier;
                    ScourgeStrikeMult *= CinderglacierMultiplier;
                    FrostStrikeMult *= CinderglacierMultiplier;
                }
                #endregion

                #region Apply Multi-Ability Talent Multipliers
                {
                    float BloodyVengeanceMult = .03f * (float)talents.BloodyVengeance;
                    BCBMult *= 1 + BloodyVengeanceMult;
                    BloodStrikeMult *= 1 + BloodyVengeanceMult;
                    HeartStrikeMult *= 1 + BloodyVengeanceMult;
                    ObliterateMult *= 1 + BloodyVengeanceMult;
                    DeathStrikeMult *= 1 + BloodyVengeanceMult;
                    PlagueStrikeMult *= 1 + BloodyVengeanceMult;
                    WhiteMult *= 1 + BloodyVengeanceMult;

                    float HysteriaCoeff = .3f / 6f; // current uptime is 16.666...%
                    float HysteriaMult = HysteriaCoeff * (float)talents.Hysteria;
                    BCBMult *= 1 + HysteriaMult;
                    BloodStrikeMult *= 1 + HysteriaMult;
                    HeartStrikeMult *= 1 + HysteriaMult;
                    ObliterateMult *= 1 + HysteriaMult;
                    DeathStrikeMult *= 1 + HysteriaMult;
                    PlagueStrikeMult *= 1 + HysteriaMult;
                    WhiteMult *= 1 + HysteriaMult;

                    float BlackIceMult = .02f * (float)talents.BlackIce;
                    FrostFeverMult *= 1 + BlackIceMult;
                    HowlingBlastMult *= 1 + BlackIceMult;
                    IcyTouchMult *= 1 + BlackIceMult;
                    FrostStrikeMult *= 1 + BlackIceMult;
                    DeathCoilMult *= 1 + BlackIceMult;
                    ScourgeStrikeMult *= 1 + BlackIceMult;
                    BloodPlagueMult *= 1 + BlackIceMult;
                    otherShadowMult *= 1 + BlackIceMult;
                    otherFrostMult *= 1 + BlackIceMult;

                    float MercilessCombatMult = .315f * 0.06f * (float)talents.MercilessCombat;   // The last 35% of a Boss don't take 35% of the fight-time...say .315 (10% faster)
                    ObliterateMult *= 1 + MercilessCombatMult;
                    HowlingBlastMult *= 1 + MercilessCombatMult;
                    IcyTouchMult *= 1 + MercilessCombatMult;
                    FrostStrikeMult *= 1 + MercilessCombatMult;

                    float GlacierRot = .0666666666666f * (float)talents.GlacierRot;
                    HowlingBlastMult *= 1 + GlacierRot;
                    IcyTouchMult *= 1 + GlacierRot;
                    FrostStrikeMult *= 1 + GlacierRot;


                    float CryptFeverMult = .1f * (float)talents.CryptFever;
                    float CryptFeverBuff = stats.BonusDiseaseDamageMultiplier;
                    CryptFeverMult = Math.Max(CryptFeverMult, CryptFeverBuff);
                    FrostFeverMult *= 1 + CryptFeverMult;
                    BloodPlagueMult *= 1 + CryptFeverMult;
                    UnholyBlightMult *= 1 + CryptFeverMult;

                    /* float DesecrationChance = (calcOpts.rotation.PlagueStrike * 12f) / calcOpts.rotation.curRotationDuration +
                                                (calcOpts.rotation.ScourgeStrike * 12f) / calcOpts.rotation.curRotationDuration;
                     if (DesecrationChance > 1f) DesecrationChance = 1f;*/
                    float DesecrationMult = .01f * (float)talents.Desecration;  //the new desecration is basically a flat 1% per point
                    BCBMult *= 1 + DesecrationMult;
                    BloodPlagueMult *= 1 + DesecrationMult;
                    BloodStrikeMult *= 1 + DesecrationMult;
                    DeathCoilMult *= 1 + DesecrationMult;
                    DancingRuneWeaponMult *= 1 + DesecrationMult;
                    FrostFeverMult *= 1 + DesecrationMult;
                    FrostStrikeMult *= 1 + DesecrationMult;
                    //GargoyleMult *= 1 + DesecrationMult;
                    HeartStrikeMult *= 1 + DesecrationMult;
                    HowlingBlastMult *= 1 + DesecrationMult;
                    IcyTouchMult *= 1 + DesecrationMult;
                    NecrosisMult *= 1 + DesecrationMult;
                    ObliterateMult *= 1 + DesecrationMult;
                    DeathStrikeMult *= 1 + DesecrationMult;
                    PlagueStrikeMult *= 1 + DesecrationMult;
                    ScourgeStrikeMult *= 1 + DesecrationMult;
                    UnholyBlightMult *= 1 + DesecrationMult;
                    WhiteMult *= 1 + DesecrationMult;
                    otherShadowMult *= 1 + DesecrationMult;
                    otherArcaneMult *= 1 + DesecrationMult;
                    otherFrostMult *= 1 + DesecrationMult;

                    if ((float)talents.BoneShield >= 1f)
                    {
                        float BoneMult = .02f;
                        BCBMult *= 1 + BoneMult;
                        BloodPlagueMult *= 1 + BoneMult;
                        BloodStrikeMult *= 1 + BoneMult;
                        DeathCoilMult *= 1 + BoneMult;
                        DancingRuneWeaponMult *= 1 + BoneMult;
                        FrostFeverMult *= 1 + BoneMult;
                        //GargoyleMult *= 1 + BoneMult;
                        FrostStrikeMult *= 1 + BoneMult;
                        HeartStrikeMult *= 1 + BoneMult;
                        HowlingBlastMult *= 1 + BoneMult;
                        IcyTouchMult *= 1 + BoneMult;
                        NecrosisMult *= 1 + BoneMult;
                        ObliterateMult *= 1 + BoneMult;
                        DeathStrikeMult *= 1 + BoneMult;
                        PlagueStrikeMult *= 1 + BoneMult;
                        ScourgeStrikeMult *= 1 + BoneMult;
                        UnholyBlightMult *= 1 + BoneMult;
                        WhiteMult *= 1 + BoneMult;
                        otherShadowMult *= 1 + BoneMult;
                        otherArcaneMult *= 1 + BoneMult;
                        otherFrostMult *= 1 + BoneMult;
                    }
                }
                #endregion

                #region Pet uptime modifiers
                {
                    BloodwormsMult *= calcOpts.BloodwormsUptime;
                    GhoulMult *= calcOpts.GhoulUptime;
                }
                #endregion

                //feed variables for output
                calcs.BCBDPS = dpsBCB * BCBMult;
                calcs.BloodPlagueDPS = dpsBloodPlague * BloodPlagueMult;
                calcs.BloodStrikeDPS = dpsBloodStrike * BloodStrikeMult;
                calcs.DeathCoilDPS = dpsDeathCoil * DeathCoilMult;
                calcs.DRWDPS = dpsDancingRuneWeapon * DancingRuneWeaponMult;
                calcs.FrostFeverDPS = dpsFrostFever * FrostFeverMult;
                calcs.FrostStrikeDPS = dpsFrostStrike * FrostStrikeMult;
                calcs.GargoyleDPS = dpsGargoyle * GargoyleMult;
                calcs.GhoulDPS = dpsGhoul * GhoulMult;
                calcs.BloodwormsDPS = dpsBloodworms * BloodwormsMult;
                calcs.HeartStrikeDPS = dpsHeartStrike * HeartStrikeMult;
                calcs.HowlingBlastDPS = dpsHowlingBlast * HowlingBlastMult;
                calcs.IcyTouchDPS = dpsIcyTouch * IcyTouchMult;
                calcs.NecrosisDPS = dpsNecrosis * NecrosisMult;
                calcs.ObliterateDPS = dpsObliterate * ObliterateMult;
                calcs.DeathStrikeDPS = dpsDeathStrike * DeathStrikeMult;
                calcs.PlagueStrikeDPS = dpsPlagueStrike * PlagueStrikeMult;
                calcs.ScourgeStrikeDPS = dpsScourgeStrike * ScourgeStrikeMult;
                calcs.UnholyBlightDPS = dpsUnholyBlight * UnholyBlightMult;
                calcs.WhiteDPS = dpsWhite * WhiteMult;
                calcs.WanderingPlagueDPS = dpsWanderingPlague * WanderingPlagueMult;
                calcs.OtherDPS = dpsOtherShadow * otherShadowMult +
                    dpsOtherArcane * otherArcaneMult +
                    dpsOtherFrost * otherFrostMult;


                calcs.DPSPoints = calcs.BCBDPS + calcs.BloodPlagueDPS + calcs.BloodStrikeDPS + calcs.DeathCoilDPS + calcs.FrostFeverDPS + calcs.FrostStrikeDPS +
                                  calcs.GargoyleDPS + calcs.GhoulDPS + calcs.WanderingPlagueDPS + calcs.HeartStrikeDPS + calcs.HowlingBlastDPS + calcs.IcyTouchDPS +
                                  calcs.NecrosisDPS + calcs.ObliterateDPS + calcs.DeathStrikeDPS + calcs.PlagueStrikeDPS + calcs.ScourgeStrikeDPS + calcs.UnholyBlightDPS +
                                  calcs.WhiteDPS + calcs.OtherDPS + calcs.BloodwormsDPS;

                #region Dancing Rune Weapon
                {
                    if (talents.DancingRuneWeapon > 0)
                    {
                        float DRWUptime = (5f + (1.5f * talents.RunicPowerMastery) + (talents.GlyphofDancingRuneWeapon ? 5f : 0)) / 90f;
                        dpsDancingRuneWeapon = (calcs.DPSPoints - calcs.GhoulDPS - calcs.BloodwormsDPS) * DRWUptime;
                        dpsDancingRuneWeapon *= 0.5f; // "doing the same attacks as the Death Knight but for 50% reduced damage."
                        calcs.DPSPoints += dpsDancingRuneWeapon;
                        calcs.DRWDPS = dpsDancingRuneWeapon;
                    }
                }
                #endregion

                calcs.OverallPoints = calcs.DPSPoints;
            }
            #endregion

            return calcs;
        }

        private Stats GetRaceStats(Character character)
        {
            Stats statsRace;
            switch (character.Race)
            {
                case Character.CharacterRace.Human:
                    statsRace = new Stats() { Strength = 108f, Agility = 73f, Stamina = 99f, Intellect = 29f, Spirit = 46f, Armor = 146f, Health = 2169f };
                    break;
                case Character.CharacterRace.Dwarf:
                    statsRace = new Stats() { Strength = 110f, Agility = 69f, Stamina = 102f, Intellect = 28f, Spirit = 41f, Armor = 138f, Health = 2199f };
                    break;
                case Character.CharacterRace.NightElf:
                    statsRace = new Stats() { Strength = 105f, Agility = 78f, Stamina = 98f, Intellect = 29f, Spirit = 42f, Armor = 156f, Health = 2159f };
                    break;
                case Character.CharacterRace.Gnome:
                    statsRace = new Stats() { Strength = 103f, Agility = 76f, Stamina = 98f, Intellect = 33f, Spirit = 42f, Armor = 152f, Health = 2159f };
                    break;
                case Character.CharacterRace.Draenei:
                    statsRace = new Stats() { Strength = 109f, Agility = 70f, Stamina = 98f, Intellect = 30f, Spirit = 44f, Armor = 140f, Health = 2159f };
                    break;
                case Character.CharacterRace.Orc:
                    statsRace = new Stats() { Strength = 111f, Agility = 70f, Stamina = 101f, Intellect = 26f, Spirit = 45f, Armor = 140f, Health = 2189f };
                    break;
                case Character.CharacterRace.Troll:
                    statsRace = new Stats() { Strength = 109f, Agility = 75f, Stamina = 100f, Intellect = 25f, Spirit = 43f, Armor = 150f, Health = 2179f };
                    break;
                case Character.CharacterRace.Undead:
                    statsRace = new Stats() { Strength = 107f, Agility = 71f, Stamina = 100f, Intellect = 27f, Spirit = 47f, Armor = 142f, Health = 2179f };
                    break;
                case Character.CharacterRace.BloodElf:
                    statsRace = new Stats() { Strength = 105f, Agility = 75f, Stamina = 97f, Intellect = 33f, Spirit = 41f, Armor = 150f, Health = 2149f };
                    break;
                case Character.CharacterRace.Tauren:
                    statsRace = new Stats() { Strength = 113f, Agility = 68f, Stamina = 101f, Intellect = 24f, Spirit = 34f, Armor = 136f, Health = 2298f };
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
            // armor doesn't matter, its always 2x agility + items, then modifiers
            statsRace.Health += 8328f;
            statsRace.AttackPower = 202f + (67f * 2);

            return statsRace;
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
            CalculationOptionsDPSDK calcOpts = character.CalculationOptions as CalculationOptionsDPSDK;
            DeathKnightTalents talents = calcOpts.talents;

            Stats statsRace = GetRaceStats(character);
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            //Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
            Stats statsTalents = new Stats()
            {
                BonusStrengthMultiplier = .01f * (float)(talents.AbominationsMight + talents.RavenousDead) + .02f * (float)(/*talents.ShadowOfDeath + */talents.VeteranOfTheThirdWar),
                BaseArmorMultiplier = .03f * (float)(talents.Toughness),
                BonusStaminaMultiplier = .02f * (float)(/*talents.ShadowOfDeath + */talents.VeteranOfTheThirdWar),
                Expertise = (float)(talents.TundraStalker + talents.RageOfRivendare) + 2f * (float)(talents.VeteranOfTheThirdWar),
                BonusPhysicalDamageMultiplier = .02f * (float)(talents.BloodGorged + talents.RageOfRivendare) + 0.03f * talents.TundraStalker,
                BonusSpellPowerMultiplier = .02f * (float)(talents.BloodGorged + talents.RageOfRivendare) + 0.03f * talents.TundraStalker,
            };
            if (talents.UnbreakableArmor > 0)
            {
                statsTalents.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats(){BonusStrengthMultiplier = 0.25f}, 20f, 120f));
            }
            Stats statsTotal = new Stats();
            Stats statsGearEnchantsBuffs = new Stats();

            if (character.ActiveBuffsContains("Ferocious Inspiration"))
            {
                statsBuffs.BonusPhysicalDamageMultiplier = ((1f + statsBuffs.BonusPhysicalDamageMultiplier) * (1f + (.01f * calcOpts.FerociousInspiration)));
                statsBuffs.BonusSpellPowerMultiplier = ((1f + statsBuffs.BonusSpellPowerMultiplier) * (1f + (.01f * calcOpts.FerociousInspiration)));
            }

            statsGearEnchantsBuffs = statsBaseGear + statsBuffs + statsRace + statsTalents;

            statsTotal = GetRelevantStats(statsGearEnchantsBuffs);
            statsTotal.Expertise += (float)StatConversion.GetExpertiseFromRating(statsBaseGear.ExpertiseRating);

            StatsSpecialEffects se = new StatsSpecialEffects(character, statsTotal, new CombatTable(character, statsTotal, calcOpts));

            foreach (SpecialEffect effect in statsTotal.SpecialEffects())
            {
                if (HasRelevantStats(effect.Stats))
                {
                    se = new StatsSpecialEffects(character, statsTotal, new CombatTable(character, statsTotal, calcOpts));
                    statsTotal += se.getSpecialEffects(calcOpts, effect);
                }
            }

            statsTotal.Strength += statsTotal.HighestStat;

            statsTotal.Agility = (float)Math.Floor(statsTotal.Agility * (1 + statsTotal.BonusAgilityMultiplier));
            statsTotal.Strength = (float)Math.Floor(statsTotal.Strength * (1 + statsTotal.BonusStrengthMultiplier));
            statsTotal.Stamina = (float)Math.Floor(statsTotal.Stamina * (1 + statsTotal.BonusStaminaMultiplier));
            statsTotal.Intellect = (float)Math.Floor(statsTotal.Intellect * (1 + statsTotal.BonusIntellectMultiplier));
            statsTotal.Spirit = (float)Math.Floor(statsTotal.Spirit * (1 + statsTotal.BonusSpiritMultiplier));
            statsTotal.Health = (float)Math.Floor(statsTotal.Health + (statsTotal.Stamina * 10f));
            statsTotal.Mana = (float)Math.Floor(statsTotal.Mana + (statsTotal.Intellect * 15f));
            statsTotal.AttackPower = (float)Math.Floor(statsTotal.AttackPower + statsTotal.Strength * 2);
            // Copy from TankDK.
            // statsTotal.Armor = (float)Math.Floor((statsTotal.Armor + statsTotal.BonusArmor + 2f * statsTotal.Agility) * 1f);
            statsTotal.Armor = (float)Math.Floor(StatConversion.GetArmorFromAgility(statsTotal.Agility) +
                                StatConversion.ApplyMultiplier(statsTotal.Armor, statsTotal.BaseArmorMultiplier) +
                                StatConversion.ApplyMultiplier(statsTotal.BonusArmor, statsTotal.BonusArmorMultiplier));

            statsTotal.AttackPower += (statsTotal.Armor / 180f) * (float)talents.BladedArmor;

            statsTotal.BonusSpellPowerMultiplier = statsTotal.BonusShadowDamageMultiplier;

            statsTotal.AttackPower *= 1f + statsTotal.BonusAttackPowerMultiplier;

            if (calcOpts.presence == CalculationOptionsDPSDK.Presence.Blood)  // a final, multiplicative component
            {
                statsTotal.BonusPhysicalDamageMultiplier *= 1.15f;
                statsTotal.BonusSpellPowerMultiplier *= 1.15f;
            }
            else if (calcOpts.presence == CalculationOptionsDPSDK.Presence.Unholy)  // a final, multiplicative component
            {
                statsTotal.PhysicalHaste += 0.15f;
                statsTotal.SpellHaste += 0.15f;
            }

            return (statsTotal);
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            List<ComparisonCalculationBase> comparisonList = new List<ComparisonCalculationBase>();
            CharacterCalculationsDPSDK baseCalc, calc;
            ComparisonCalculationBase comparison;
            float[] subPoints;

            switch (chartName)
            {
                case "Item Budget":
                    Item[] itemList = new Item[] {
                        new Item() { Stats = new Stats() { Strength = 10 } },
                        new Item() { Stats = new Stats() { Agility = 10 } },
                        new Item() { Stats = new Stats() { AttackPower = 20 } },
                        new Item() { Stats = new Stats() { CritRating = 10 } },
                        new Item() { Stats = new Stats() { HitRating = 10 } },
                        new Item() { Stats = new Stats() { ExpertiseRating = 10 } },
                        new Item() { Stats = new Stats() { HasteRating = 10 } },
                        new Item() { Stats = new Stats() { ArmorPenetrationRating = 10 } },
                    };
                    string[] statList = new string[] {
                        "Strength",
                        "Agility",
                        "Attack Power",
                        "Crit Rating",
                        "Hit Rating",
                        "Expertise Rating",
                        "Haste Rating",
                        "Armor Penetration Rating",
                    };

                    baseCalc = GetCharacterCalculations(character) as CharacterCalculationsDPSDK;

                    for (int index = 0; index < itemList.Length; index++)
                    {
                        calc = GetCharacterCalculations(character, itemList[index]) as CharacterCalculationsDPSDK;

                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = statList[index];
                        comparison.Equipped = false;
                        comparison.OverallPoints = calc.OverallPoints - baseCalc.OverallPoints;
                        subPoints = new float[calc.SubPoints.Length];
                        for (int i = 0; i < calc.SubPoints.Length; i++)
                        {
                            subPoints[i] = calc.SubPoints[i] - baseCalc.SubPoints[i];
                        }
                        comparison.SubPoints = subPoints;

                        comparisonList.Add(comparison);
                    }
                    return comparisonList.ToArray();
                default:
                    return new ComparisonCalculationBase[0];
            }
        }

        public override bool IsItemRelevant(Item item)
        {
            if (item.Slot == Item.ItemSlot.OffHand /*  ||
                (item.Slot == Item.ItemSlot.Ranged && item.Type != Item.ItemType.Sigil) */
                                                                                          )
                return false;
            return base.IsItemRelevant(item);
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            Stats s = new Stats()
            {
                Health = stats.Health,
                Strength = stats.Strength,
                Agility = stats.Agility,
                Stamina = stats.Stamina,
                Armor = stats.Armor,
                HighestStat = stats.HighestStat,

                AttackPower = stats.AttackPower,
                HitRating = stats.HitRating,
                CritRating = stats.CritRating,
                ArmorPenetrationRating = stats.ArmorPenetrationRating,
                ArmorPenetration = stats.ArmorPenetration,
                Expertise = stats.Expertise,
                ExpertiseRating = stats.ExpertiseRating,
                HasteRating = stats.HasteRating,
                WeaponDamage = stats.WeaponDamage,
                PhysicalCrit = stats.PhysicalCrit,
                PhysicalHaste = stats.PhysicalHaste,
                PhysicalHit = stats.PhysicalHit,

                SpellHit = stats.SpellHit,
                SpellCrit = stats.SpellCrit,
                SpellHaste = stats.SpellHaste,

                BonusStrengthMultiplier = stats.BonusStrengthMultiplier,
                BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
                BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,
                BonusCritMultiplier = stats.BonusCritMultiplier,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
                BonusPhysicalDamageMultiplier = stats.BonusPhysicalDamageMultiplier,

                LotPCritRating = stats.LotPCritRating,
                CritMeleeRating = stats.CritMeleeRating,
                Bloodlust = stats.Bloodlust,

                BonusShadowDamageMultiplier = stats.BonusShadowDamageMultiplier,
                BonusFrostDamageMultiplier = stats.BonusFrostDamageMultiplier,
                BonusDiseaseDamageMultiplier = stats.BonusDiseaseDamageMultiplier,

                BonusBloodStrikeDamage = stats.BonusBloodStrikeDamage,
                BonusDeathCoilDamage = stats.BonusDeathCoilDamage,
                BonusDeathStrikeDamage = stats.BonusDeathStrikeDamage,
                BonusFrostStrikeDamage = stats.BonusFrostStrikeDamage,
                BonusHeartStrikeDamage = stats.BonusHeartStrikeDamage,
                BonusIcyTouchDamage = stats.BonusIcyTouchDamage,
                BonusObliterateDamage = stats.BonusObliterateDamage,
                BonusScourgeStrikeDamage = stats.BonusScourgeStrikeDamage,

                BonusDeathCoilCrit = stats.BonusDeathCoilCrit,
                BonusDeathStrikeCrit = stats.BonusDeathStrikeCrit,
                BonusFrostStrikeCrit = stats.BonusFrostStrikeCrit,
                BonusObliterateCrit = stats.BonusObliterateCrit,
                BonusPerDiseaseBloodStrikeDamage = stats.BonusPerDiseaseBloodStrikeDamage,
                BonusPerDiseaseHeartStrikeDamage = stats.BonusPerDiseaseHeartStrikeDamage,
                BonusPerDiseaseObliterateDamage = stats.BonusPerDiseaseObliterateDamage,
                BonusPerDiseaseScourgeStrikeDamage = stats.BonusPerDiseaseScourgeStrikeDamage,
                BonusPlagueStrikeCrit = stats.BonusPlagueStrikeCrit,
                BonusRPFromDeathStrike = stats.BonusRPFromDeathStrike,
                BonusRPFromObliterate = stats.BonusRPFromObliterate,
                BonusRPFromScourgeStrike = stats.BonusRPFromScourgeStrike,
                BonusRuneStrikeMultiplier = stats.BonusRuneStrikeMultiplier,
                BonusScourgeStrikeCrit = stats.BonusScourgeStrikeCrit,

                BonusFrostWeaponDamage = stats.BonusFrostWeaponDamage,
                CinderglacierProc = stats.CinderglacierProc
            };

            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (HasRelevantStats(effect.Stats))
                {
                    if (effect.Trigger == Trigger.DamageDone ||
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
                        effect.Trigger == Trigger.BloodStrikeOrHeartStrikeHit ||
                        effect.Trigger == Trigger.IcyTouchHit ||
                        effect.Trigger == Trigger.PlagueStrikeHit ||
                        effect.Trigger == Trigger.RuneStrikeHit ||
                        effect.Trigger == Trigger.Use)
                    {
                        s.AddSpecialEffect(effect);
                    }
                }
            }

            return s;
        }

        public override bool HasRelevantStats(Stats stats)
        {
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (relevantStats(effect.Stats))
                {
                    if (effect.Trigger == Trigger.DamageDone ||
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
                        effect.Trigger == Trigger.BloodStrikeOrHeartStrikeHit ||
                        effect.Trigger == Trigger.IcyTouchHit ||
                        effect.Trigger == Trigger.PlagueStrikeHit ||
                        effect.Trigger == Trigger.RuneStrikeHit ||
                        effect.Trigger == Trigger.Use)
                    {
                        return true;
                    }
                }
            }
            return relevantStats(stats);
        }
        private bool relevantStats(Stats stats)
        {
            return (stats.Health + stats.Strength + stats.Agility + stats.Stamina + stats.AttackPower +
                stats.HitRating + stats.CritRating + stats.ArmorPenetrationRating + stats.ArmorPenetration +
                stats.ExpertiseRating + stats.HasteRating + stats.WeaponDamage +
                stats.BonusStrengthMultiplier + stats.BonusStaminaMultiplier + stats.BonusAgilityMultiplier + stats.BonusCritMultiplier +
                stats.BonusAttackPowerMultiplier + stats.BonusPhysicalDamageMultiplier + stats.ShadowDamage +
                stats.CritMeleeRating + stats.BonusShadowDamageMultiplier + stats.SpellHaste
                + stats.BonusFrostDamageMultiplier + stats.BonusScourgeStrikeDamage + stats.PhysicalCrit + stats.PhysicalHaste
                + stats.PhysicalHit + stats.SpellCrit + stats.SpellHit + stats.SpellHaste + stats.BonusDiseaseDamageMultiplier
                + stats.BonusBloodStrikeDamage + stats.BonusDeathCoilDamage + stats.BonusDeathStrikeDamage + stats.BonusFrostStrikeDamage
                + stats.BonusHeartStrikeDamage + stats.BonusIcyTouchDamage + stats.BonusObliterateDamage + stats.BonusScourgeStrikeDamage
                + stats.BonusDeathCoilCrit + stats.BonusDeathStrikeCrit + stats.BonusFrostStrikeCrit + stats.BonusObliterateCrit
                + stats.BonusPerDiseaseBloodStrikeDamage + stats.BonusPerDiseaseHeartStrikeDamage + stats.BonusPerDiseaseObliterateDamage
                + stats.BonusPerDiseaseScourgeStrikeDamage + stats.BonusPlagueStrikeCrit + stats.BonusRPFromDeathStrike
                + stats.BonusRPFromObliterate + stats.BonusRPFromScourgeStrike + stats.BonusRuneStrikeMultiplier + stats.BonusScourgeStrikeCrit
                + stats.ShadowDamage + stats.ArcaneDamage + stats.CinderglacierProc + stats.BonusFrostWeaponDamage + stats.HighestStat) != 0;
        }


        /// <summary>

        /// Saves the talents for the character

        /// </summary>

        /// <param name="character">The character for whom the talents should be saved</param>

        public void GetTalents(Character character)
        {
            CalculationOptionsDPSDK calcOpts = character.CalculationOptions as CalculationOptionsDPSDK;
            // When switching from TankDK to DPSDK, I see calcOpts as null.  Check first before using.
            if (null != calcOpts)
            {
                calcOpts.talents = character.DeathKnightTalents;
            }
        }

        private string[] _optimizableCalculationLabels = null;
        public override string[] OptimizableCalculationLabels
        {
            get
            {
                if (_optimizableCalculationLabels == null)
                    _optimizableCalculationLabels = new string[] {
                        "Health",
                        "Nature Resistance",
                        "Fire Resistance",
                        "Frost Resistance",
                        "Shadow Resistance",
                        "Arcane Resistance",
                        "Crit Rating",
                        "Expertise Rating",
                        "Hit Rating",
                        "Haste Rating",
                        "Target Miss %",
                        "Target Dodge %"
                    };

                return _optimizableCalculationLabels;
            }
        }
    }
}
