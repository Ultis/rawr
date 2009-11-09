using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
#if RAWR3
using System.Windows.Media;
#else
using System.Drawing;
#endif


namespace Rawr.DPSDK
{
    //[Rawr.Calculations.RawrModelInfo("DPSDK", "spell_deathknight_classicon", CharacterClass.Paladin)]  wont work until wotlk goes live on wowhead
    [Rawr.Calculations.RawrModelInfo("DPSDK", "spell_shadow_deathcoil", CharacterClass.DeathKnight)]
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
					new GemmingTemplate() { Model = "DPSDK", Group = "Rare", //Max Strength
						RedId = bold[1], YellowId = bold[1], BlueId = bold[1], PrismaticId = bold[1], MetaId = chaotic },
					new GemmingTemplate() { Model = "DPSDK", Group = "Rare", //Strength
						RedId = bold[1], YellowId = inscribed[1], BlueId = sovereign[1], PrismaticId = bold[1], MetaId = chaotic },
						
					new GemmingTemplate() { Model = "DPSDK", Group = "Epic", Enabled = true, //Max Strength
						RedId = bold[2], YellowId = bold[2], BlueId = bold[2], PrismaticId = bold[2], MetaId = chaotic },
					new GemmingTemplate() { Model = "DPSDK", Group = "Epic", Enabled = true, //Strength
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
#if RAWR3
                    _subPointNameColors = new Dictionary<string, System.Windows.Media.Color>();
                    _subPointNameColors.Add("DPS", System.Windows.Media.Color.FromArgb(255,0,0,255));
#else
                    _subPointNameColors = new Dictionary<string, System.Drawing.Color>();
                    _subPointNameColors.Add("DPS", System.Drawing.Color.FromArgb(255, 0, 0, 255));
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
        public override string[] CharacterDisplayCalculationLabels {
            get {
                if (_characterDisplayCalculationLabels == null) {
                    List<string> labels = new List<string>(new string[] {
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
        public override string[] CustomChartNames {
            get {
                if (_customChartNames == null) { _customChartNames = new string[] { "Item Budget"/*, "MH Weapon Speed", "OH Weapon Speed"*/ }; }
                return _customChartNames;
            }
        }

#if RAWR3
        private ICalculationOptionsPanel _calculationOptionsPanel = null;
        public override ICalculationOptionsPanel CalculationOptionsPanel
#else
        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
        public override CalculationOptionsPanelBase CalculationOptionsPanel
#endif
        {
            get { return _calculationOptionsPanel ?? (_calculationOptionsPanel = new CalculationOptionsPanelDPSDK()); }
        }

        private List<ItemType> _relevantItemTypes = null;
        public override List<ItemType> RelevantItemTypes {
            get {
                return _relevantItemTypes ?? (_relevantItemTypes = new List<ItemType>(new ItemType[] {
						ItemType.None,
                        ItemType.Leather,
                        ItemType.Mail,
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

        public override CharacterClass TargetClass { get { return CharacterClass.DeathKnight; } }
        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationDPSDK(); }

        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsDPSDK(); }

        public override ICalculationOptionBase DeserializeDataObject(string xml) {
            XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsDPSDK));
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
        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations) {
            CalculationOptionsDPSDK calcOpts = character.CalculationOptions as CalculationOptionsDPSDK;
            int targetLevel = calcOpts.TargetLevel;
            GetTalents(character);

#if RAWR3
   /*         if (character != null && character.MainHand != null && character.MainHand.Item == null)
            {
                character.MainHand.Item = new Item("Test Weapon", ItemQuality.Artifact, ItemType.TwoHandAxe, 12345, "",
                   ItemSlot.TwoHand, "", false, new Stats() { Strength = 100f }, new Stats() { }, ItemSlot.None, ItemSlot.None,
                   ItemSlot.None, 500, 1500, ItemDamageType.Physical, 3.6f, "");
            }*/
#endif

            Stats stats = GetCharacterStats(character, additionalItem);

            CharacterCalculationsDPSDK calcs = new CharacterCalculationsDPSDK();
            calcs.BasicStats = stats;
            calcs.ActiveBuffs = new List<Buff>(character.ActiveBuffs);
            calcs.Talents = calcOpts.talents;

            CombatTable combatTable = new CombatTable(character, calcs, stats, calcOpts/*, additionalItem*/);
            
            //DPS Subgroups
            float dpsWhite = 0f;
            float dpsBCB = 0f;
            float dpsDeathCoil = 0f;
            float dpsIcyTouch = 0f;
            float dpsPlagueStrike = 0f;
            float dpsFrostFever = 0f;
            float dpsBloodPlague = 0f;
            float dpsScourgeStrike = 0f;
            float dpsScourgeStrikeShadow = 0f;
            float dpsScourgeStrikePhysical = 0f;
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
            float dpsOtherFire = 0f;
            float dpsBloodworms = 0f;

            //shared variables
            DeathKnightTalents talents = calcOpts.talents;
            bool DW = combatTable.DW;
            float missedSpecial = 0f;

            float dpsWhiteBeforeArmor = 0f;
            float dpsWhiteMinusGlancing = 0f;
            float fightDuration = calcOpts.FightLength * 60;
            float mitigation;
            float KMProcsPerRotation = 0f;
            float CinderglacierMultiplier = 1f;

            float MHExpertise = stats.Expertise;
            float OHExpertise = stats.Expertise;

            //damage multipliers
            float spellPowerMult = stats.BonusSpellPowerMultiplier;
            float frostSpellPowerMult = stats.BonusSpellPowerMultiplier + Math.Max((stats.BonusFrostDamageMultiplier - stats.BonusShadowDamageMultiplier), 0f);

            float physPowerMult = stats.BonusPhysicalDamageMultiplier;
            // Covers all % physical damage increases.  Blood Frenzy, FI.
            float partialResist = 0.94f; // Average of 6% damage lost to partial resists on spells
            physPowerMult *= 1f + stats.BonusDamageMultiplier;
            spellPowerMult *= 1f + stats.BonusDamageMultiplier;
            frostSpellPowerMult *= 1f + stats.BonusDamageMultiplier;

            //spell AP multipliers, for diseases its per tick
            float HowlingBlastAPMult = 0.2f;
            float IcyTouchAPMult = 0.1f;
            float FrostFeverAPMult = 0.055f;
            float BloodPlagueAPMult = 0.055f;
            float DeathCoilAPMult = 0.15f;
            float UnholyBlightAPMult = 0.013f;
            float GargoyleAPMult = 0.4f;   // pre 3.0.8 == 0.42f...now probably ~0.3f
                                            // TESTED on june 27th 2009 and found to be 0.40f
            float BloodwormsAPMult = 0.006f;

            //for estimating rotation pushback

            calcOpts.rotation.AvgDiseaseMult = calcOpts.rotation.NumDisease * (calcOpts.rotation.DiseaseUptime / 100);
            float commandMult = 0f;

            {

                float OHMult = 0.5f * (1f + (float)talents.NervesOfColdSteel * 0.05f);	//an OH multiplier that is useful sometimes
                Boolean PTR = false; // enable and disable PTR things here

                Rotation temp = new Rotation();
                temp.ManagedRP = calcOpts.rotation.ManagedRP;
                temp.AvgDiseaseMult = calcOpts.rotation.AvgDiseaseMult;
                temp.BloodPlague = calcOpts.rotation.BloodPlague;
                temp.BloodStrike = calcOpts.rotation.BloodStrike;
                temp.CurRotationDuration = calcOpts.rotation.CurRotationDuration;
                temp.curRotationType = calcOpts.rotation.curRotationType;
                temp.DancingRuneWeapon = calcOpts.rotation.DancingRuneWeapon;
                temp.DeathCoil = calcOpts.rotation.DeathCoil;
                temp.DeathStrike = calcOpts.rotation.DeathStrike;
                temp.DiseaseUptime = calcOpts.rotation.DiseaseUptime;
                temp.FrostFever = calcOpts.rotation.FrostFever;
                temp.FrostStrike = calcOpts.rotation.FrostStrike;
                temp.GargoyleDuration = calcOpts.rotation.GargoyleDuration;
                temp.GCDTime = calcOpts.rotation.GCDTime;
                temp.GhoulFrenzy = calcOpts.rotation.GhoulFrenzy;
                temp.HeartStrike = calcOpts.rotation.HeartStrike;
                temp.Horn = calcOpts.rotation.Horn;
                temp.HowlingBlast = calcOpts.rotation.HowlingBlast;
                temp.IcyTouch = calcOpts.rotation.IcyTouch;
                temp.NumDisease = calcOpts.rotation.NumDisease;
                temp.Obliterate = calcOpts.rotation.Obliterate;
                temp.Pestilence = calcOpts.rotation.Pestilence;
                temp.PlagueStrike = calcOpts.rotation.PlagueStrike;
                temp.presence = calcOpts.rotation.presence;
                temp.PTRCalcs = calcOpts.rotation.PTRCalcs || PTR;
                temp.RP = calcOpts.rotation.RP;
                temp.ScourgeStrike = calcOpts.rotation.ScourgeStrike;
                temp.HowlingBlast += talents.Rime * calcOpts.rotation.Obliterate * 0.05f;

                if (temp.ManagedRP)
                {
                    temp.getRP(talents, character);
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
                    if (character.Race == CharacterRace.Orc) { commandMult += .05f; }
                }
                #endregion

                #region Killing Machine
                {
                    float KMPpM = (1f * talents.KillingMachine) * (1f + (StatConversion.GetHasteFromRating(stats.HasteRating, CharacterClass.DeathKnight))) * (1f + stats.PhysicalHaste); // KM Procs per Minute (Defined "1 per point" by Blizzard) influenced by Phys. Haste
                    KMPpM *= calcOpts.KMProcUsage;
                    KMPpM *= 1f - combatTable.totalMHMiss;
                    KMPpM += talents.Deathchill / 2f;

                    float KMPpR = KMPpM / (60f / temp.CurRotationDuration);
                    float totalAbilities = temp.FrostStrike + temp.IcyTouch + temp.HowlingBlast;
                    KMProcsPerRotation = KMPpR;
                }
                #endregion

                #region Cinderglacier
                {
                    float shadowFrostAbilitiesPerSecond = ((temp.DeathCoil + temp.FrostStrike +
                            temp.ScourgeStrike + temp.IcyTouch + temp.HowlingBlast) /
                            combatTable.realDuration);

                    CinderglacierMultiplier *= 1f + (0.2f / (shadowFrostAbilitiesPerSecond / stats.CinderglacierProc));

                }
                #endregion

                #region Mitigation
                {
                    float targetArmor = calcOpts.BossArmor, totalArP = stats.ArmorPenetration;

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
                        float dpsMHglancing = (StatConversion.WHITE_GLANCE_CHANCE_CAP[targetLevel - 80] * combatTable.MH.DPS) * 0.7f;
                        float dpsMHBeforeArmor = ((combatTable.MH.DPS * (1f - calcs.AvoidedAttacks - StatConversion.WHITE_GLANCE_CHANCE_CAP[targetLevel - 80])) * (1f + combatTable.physCrits)) + dpsMHglancing;
                        dpsWhiteMinusGlancing = dpsMHBeforeArmor - dpsMHglancing;
                        dpsWhiteBeforeArmor = dpsMHBeforeArmor;
                        MHDPS = dpsMHBeforeArmor * mitigation;
                    }
                    #endregion

                    #region Off Hand
                    if (DW || (character.MainHand == null && character.OffHand != null))
                    {
                        float dpsOHglancing = (StatConversion.WHITE_GLANCE_CHANCE_CAP[targetLevel - 80] * combatTable.OH.DPS) * 0.7f;
                        float dpsOHBeforeArmor = ((combatTable.OH.DPS * (1f - calcs.AvoidedAttacks - StatConversion.WHITE_GLANCE_CHANCE_CAP[targetLevel - 80])) * (1f + combatTable.physCrits)) + dpsOHglancing;
                        dpsOHBeforeArmor *= OHMult;
                        dpsWhiteMinusGlancing += dpsOHBeforeArmor - dpsOHglancing;
                        dpsWhiteBeforeArmor += dpsOHBeforeArmor;
                        OHDPS = dpsOHBeforeArmor * mitigation;

                    }
                    #endregion

                    dpsWhite = MHDPS + OHDPS;
                }
                #endregion

                #region Blood Caked Blade
                {
                    float dpsMHBCB = 0f;
                    float dpsOHBCB = 0f;
                    if ((combatTable.OH.damage != 0) && (DW || combatTable.MH.damage == 0))
                    {
                        float OHBCBDmg = combatTable.OH.damage * (.25f + .125f * temp.AvgDiseaseMult);
                        dpsOHBCB = OHBCBDmg / combatTable.OH.hastedSpeed;
                        dpsOHBCB *= OHMult;
                    }
                    if (combatTable.MH.damage != 0)
                    {
                        float MHBCBDmg = combatTable.MH.damage * (.25f + .125f * temp.AvgDiseaseMult);
                        dpsMHBCB = MHBCBDmg / combatTable.MH.hastedSpeed;
                    }
                    dpsBCB = dpsMHBCB + dpsOHBCB;
                    dpsBCB *= .1f * (float)talents.BloodCakedBlade;
                }
                #endregion

                #region Death Coil
                {
                    if (temp.DeathCoil > 0f)
                    {
                        float DCCD = combatTable.realDuration / (temp.DeathCoil + (0.05f * (float)talents.SuddenDoom * temp.HeartStrike));
                        float DCDmg = 443f + (DeathCoilAPMult * stats.AttackPower) + stats.BonusDeathCoilDamage;
                        dpsDeathCoil = DCDmg / DCCD;
                        float DCCritDmgMult = .5f * (2f + stats.BonusCritMultiplier);
                        float DCCrit = 1f + ((combatTable.spellCrits + stats.BonusDeathCoilCrit) * DCCritDmgMult);
                        dpsDeathCoil *= DCCrit;

                        //sudden doom stuff
                        // this section is no longer relevant after 3.1.x changes
                        /* float affectedDCMult = temp.BloodStrike + temp.HeartStrike;
                         affectedDCMult *= .04f * (float)talents.SuddenDoom;
                         affectedDCMult /= temp.DeathCoil;
                         dpsDeathCoil += dpsDeathCoil * affectedDCMult;*/

                        dpsDeathCoil *= 1f + (.05f * (float)talents.Morbidity);
                        dpsDeathCoil *= 1f + (talents.GlyphofDarkDeath ? .15f : 0f);
                    }
                }
                #endregion

                #region Icy Touch
                {
                    if (temp.IcyTouch > 0f)
                    {
                        float addedCritFromKM = 0;
                        float ITCD = combatTable.realDuration / temp.IcyTouch;
                        float ITDmg = 236f + (IcyTouchAPMult * stats.AttackPower) + stats.BonusIcyTouchDamage;
                        ITDmg *= 1f + .05f * (float)talents.ImprovedIcyTouch;
                        dpsIcyTouch = ITDmg / ITCD;
                        float ITCritDmgMult = .5f * (2f + stats.CritBonusDamage + stats.BonusCritMultiplier);
                        float ITCrit = 1f + (Math.Min((combatTable.spellCrits + addedCritFromKM + (.05f * (float)talents.Rime)), 1f) * ITCritDmgMult);
                        dpsIcyTouch *= ITCrit;
                    }
                }
                #endregion

                #region Plague Strike
                {
                    if (temp.PlagueStrike > 0f)
                    {
                        float PSCD = combatTable.realDuration / temp.PlagueStrike;
                        float PSDmg = (combatTable.MH.baseDamage + ((stats.AttackPower / 14f) *
                                        combatTable.normalizationFactor)) * 0.5f + 189f;
                        float PSDmgOH = 0f;
                        if (DW) PSDmgOH = (combatTable.OH.baseDamage + ((stats.AttackPower / 14f) *
                                        combatTable.normalizationFactor)) * 0.5f + 189f;
                        PSDmgOH *= (talents.ThreatOfThassarian * 0.333333333f);
                        PSDmgOH *= OHMult;
                        dpsPlagueStrike = (PSDmg + PSDmgOH) / PSCD;
                        float PSCritDmgMult = 1f + (.15f * (float)talents.ViciousStrikes) + stats.BonusCritMultiplier;
                        float PSCrit = 1f + ((combatTable.physCrits + (.03f * (float)talents.ViciousStrikes) +
                                            stats.BonusPlagueStrikeCrit) * PSCritDmgMult);

                        dpsPlagueStrike *= PSCrit;
                        dpsPlagueStrike *= (talents.GlyphofPlagueStrike ? 1.2f : 1f);
                        dpsPlagueStrike *= 1f + (.1f * (float)talents.Outbreak);
                    }

                }
                #endregion

                #region Frost Fever
                {
                    if (temp.IcyTouch > 0f ||
                    (talents.GlyphofHowlingBlast && temp.HowlingBlast > 0f) ||
                    (talents.GlyphofDisease && temp.Pestilence > 0f))
                    {
                        // Frost Fever is renewed with every Icy Touch and starts a new cd
                        float ITCD = (temp.IcyTouch + (talents.GlyphofHowlingBlast ? temp.HowlingBlast : 0f));
                        float FFDmg = 0f;
                        String effectStats;
                        float ticksPerRotation = 5f + talents.Epidemic + (talents.GlyphofScourgeStrike ? Math.Min(temp.ScourgeStrike, 3f) : 0f);
                        if (ITCD > 1f)
                        {
                            ticksPerRotation += (ITCD - 1f) / 2f;
                        }
                        if (ticksPerRotation * 3f > combatTable.realDuration)
                        {
                            float lostTicks = ticksPerRotation - (float)((int)(combatTable.realDuration / 3f));
                            ticksPerRotation -= lostTicks;
                        }
                        float PestRefresh = (15f + talents.Epidemic * 3f +
                            (talents.GlyphofScourgeStrike ? Math.Min(3f * temp.ScourgeStrike, 9f) : 0f));
                        if (PestRefresh * temp.Pestilence - combatTable.realDuration > 0f)
                        {
                            ticksPerRotation = combatTable.realDuration / 3f;
                            Stats maxStats = GetCharacterStatsMaximum(character, additionalItem, calcOpts.FightLength * 60f);
                            Stats tempStats;
                            foreach (SpecialEffect effect in maxStats.SpecialEffects())
                            {
                                if (effect.Trigger != Trigger.Use)
                                {
                                    tempStats = new Stats();
                                    tempStats += maxStats;
                                    tempStats += effect.Stats;
                                    tempStats.Strength += tempStats.HighestStat + tempStats.Paragon;
                                    tempStats.Agility = (float)Math.Floor(tempStats.Agility * (1 + tempStats.BonusAgilityMultiplier));
                                    tempStats.Strength = (float)Math.Floor(tempStats.Strength * (1 + tempStats.BonusStrengthMultiplier));
                                    tempStats.AttackPower = (float)Math.Floor(tempStats.AttackPower + tempStats.Strength * 2);
                                    // Copy from TankDK.
                                    // tempStats.Armor = (float)Math.Floor((tempStats.Armor + tempStats.BonusArmor + 2f * tempStats.Agility) * 1f);
                                    tempStats.Armor = (float)Math.Floor(StatConversion.GetArmorFromAgility(tempStats.Agility) +
                                                        StatConversion.ApplyMultiplier(tempStats.Armor, tempStats.BaseArmorMultiplier) +
                                                        StatConversion.ApplyMultiplier(tempStats.BonusArmor, tempStats.BonusArmorMultiplier));
                                    tempStats.AttackPower += (tempStats.Armor / 180f) * (float)talents.BladedArmor;
                                    tempStats.BonusSpellPowerMultiplier = tempStats.BonusShadowDamageMultiplier;
                                    tempStats.AttackPower *= 1f + tempStats.BonusAttackPowerMultiplier;
                                    if (calcOpts.rotation.presence == CalculationOptionsDPSDK.Presence.Blood)  // a final, multiplicative component
                                    {
                                        tempStats.BonusPhysicalDamageMultiplier *= 1.15f;
                                        tempStats.BonusSpellPowerMultiplier *= 1.15f;
                                    }
                                    else if (calcOpts.rotation.presence == CalculationOptionsDPSDK.Presence.Unholy)  // a final, multiplicative component
                                    {
                                        tempStats.PhysicalHaste += 0.15f;
                                        tempStats.SpellHaste += 0.15f;
                                    }
                                    float tempFFDmg = BloodPlagueAPMult * tempStats.AttackPower + 31.1f;
                                    if (tempFFDmg > FFDmg)
                                    {
                                        FFDmg = tempFFDmg;
                                        effectStats = effect.ToString();
                                    }
                                }
                            }
                        }
                        else
                        {
                            FFDmg = FrostFeverAPMult * stats.AttackPower;
                        }
                        dpsFrostFever = FFDmg * ticksPerRotation / combatTable.realDuration;
                        dpsFrostFever *= 1.15f;	// Patch 3.2: Diseases hit 15% harder.
                        if (PTR && talents.GlyphofIcyTouch)
                        {
                            dpsFrostFever *= 1.2f;
                        }
                        dpsWPFromFF = dpsFrostFever * combatTable.physCrits;
                    }
                }
                #endregion

                #region Blood Plague
                {
                    if (temp.PlagueStrike > 0f || talents.GlyphofDisease)
                    {
                        // Blood Plague is renewed with every Plague Strike and starts a new cd
                        float PSCD = temp.PlagueStrike;
                        float PestRefresh = (15f + talents.Epidemic * 3f +
                            (talents.GlyphofScourgeStrike ? Math.Min(3f * temp.ScourgeStrike, 9f) : 0f));
                        float BPDmg = 0f;
                        String tempEffect = "";
                        float ticksPerRotation = 5f + talents.Epidemic + (talents.GlyphofScourgeStrike ? Math.Min(temp.ScourgeStrike, 3f) : 0f);
                        if (PSCD > 1f)
                        {
                            ticksPerRotation += (PSCD - 1f) / 2f;
                        }
                        if (ticksPerRotation * 3f > combatTable.realDuration)
                        {
                            float lostTicks = ticksPerRotation - (float)((int)(combatTable.realDuration / 3f));
                            ticksPerRotation -= lostTicks;
                        }
                        if (PestRefresh * temp.Pestilence - combatTable.realDuration > 0f)
                        {
                            ticksPerRotation = combatTable.realDuration / 3f;
                            Stats maxStats = GetCharacterStatsMaximum(character, additionalItem, calcOpts.FightLength * 60f);
                            Stats tempStats;
                            foreach (SpecialEffect effect in maxStats.SpecialEffects())
                            {
                                if (effect.Trigger != Trigger.Use)
                                {
                                    tempStats = new Stats();
                                    tempStats += maxStats;
                                    tempStats += effect.Stats;
                                    tempStats.Strength += tempStats.HighestStat + tempStats.Paragon;
                                    tempStats.Agility = (float)Math.Floor(tempStats.Agility * (1 + tempStats.BonusAgilityMultiplier));
                                    tempStats.Strength = (float)Math.Floor(tempStats.Strength * (1 + tempStats.BonusStrengthMultiplier));
                                    tempStats.AttackPower = (float)Math.Floor(tempStats.AttackPower + tempStats.Strength * 2);
                                    // Copy from TankDK.
                                    // tempStats.Armor = (float)Math.Floor((tempStats.Armor + tempStats.BonusArmor + 2f * tempStats.Agility) * 1f);
                                    tempStats.Armor = (float)Math.Floor(StatConversion.GetArmorFromAgility(tempStats.Agility) +
                                                        StatConversion.ApplyMultiplier(tempStats.Armor, tempStats.BaseArmorMultiplier) +
                                                        StatConversion.ApplyMultiplier(tempStats.BonusArmor, tempStats.BonusArmorMultiplier));
                                    tempStats.AttackPower += (tempStats.Armor / 180f) * (float)talents.BladedArmor;
                                    tempStats.BonusSpellPowerMultiplier = tempStats.BonusShadowDamageMultiplier;
                                    tempStats.AttackPower *= 1f + tempStats.BonusAttackPowerMultiplier;
                                    if (calcOpts.rotation.presence == CalculationOptionsDPSDK.Presence.Blood)  // a final, multiplicative component
                                    {
                                        tempStats.BonusPhysicalDamageMultiplier *= 1.15f;
                                        tempStats.BonusSpellPowerMultiplier *= 1.15f;
                                    }
                                    else if (calcOpts.rotation.presence == CalculationOptionsDPSDK.Presence.Unholy)  // a final, multiplicative component
                                    {
                                        tempStats.PhysicalHaste += 0.15f;
                                        tempStats.SpellHaste += 0.15f;
                                    }
                                    float tempBPDmg = BloodPlagueAPMult * tempStats.AttackPower;
                                    if (tempBPDmg > BPDmg)
                                    {
                                        BPDmg = tempBPDmg;
                                        tempEffect = effect.ToString();
                                    }
                                }
                            }
                        }
                        else
                        {
                            BPDmg = BloodPlagueAPMult * stats.AttackPower + 31.1f;
                        }
                        dpsBloodPlague = BPDmg * ticksPerRotation / combatTable.realDuration;
                        dpsBloodPlague *= 1.15f; // Patch 3.2: Diseases hit 15% harder.
                        dpsWPFromBP = dpsBloodPlague * combatTable.physCrits;
                    }
                }
                #endregion

                #region 4T9
                {
                    if (stats.DiseasesCanCrit > 0f)
                    {
                        float DiseaseCritDmgMult = 0.5f * (2f + stats.BonusCritMultiplier);
                        float DiseaseCrit = 1f + combatTable.spellCrits;
                        if (!PTR)
                        {
                            dpsFrostFever *= DiseaseCrit;
                        }
                        dpsBloodPlague *= DiseaseCrit;
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
                    if (talents.ScourgeStrike > 0 && temp.ScourgeStrike > 0f)
                    {
                        if (!PTR)
                        {
                            float SSCD = combatTable.realDuration / temp.ScourgeStrike;
                            float SSDmg = ((combatTable.MH.baseDamage + ((stats.AttackPower / 14f) * combatTable.normalizationFactor)) * 0.40f) + 317.5f +
                                stats.BonusScourgeStrikeDamage;
                            SSDmg *= 1f + 0.10f * temp.AvgDiseaseMult * (1f + stats.BonusPerDiseaseScourgeStrikeDamage);
                            dpsScourgeStrike = SSDmg / SSCD;
                            float SSCritDmgMult = 1f + (.15f * (float)talents.ViciousStrikes) + stats.BonusCritMultiplier;
                            float SSCrit = 1f + ((combatTable.physCrits + (.03f * (float)talents.ViciousStrikes) + (.03f * (float)talents.Subversion)
                                                + stats.BonusScourgeStrikeCrit) * SSCritDmgMult);
                            dpsScourgeStrike = dpsScourgeStrike * SSCrit;
                            dpsScourgeStrike *= 1f + (0.2f / 3f * (float)talents.Outbreak);
                        }
                        else
                        {
                            float SSCD = combatTable.realDuration / temp.ScourgeStrike;
                            float SSDmg = ((combatTable.MH.baseDamage + ((stats.AttackPower / 14f) * combatTable.normalizationFactor)) * 0.5f) + 317.5f +
                                stats.BonusScourgeStrikeDamage;
                            dpsScourgeStrikePhysical = SSDmg / SSCD;
                            float SSCRitDmgMult = 1f + (.15f * (float)talents.ViciousStrikes) + stats.BonusCritMultiplier;
                            float SSCrit = 1f + ((combatTable.physCrits + (0.3f * (float)talents.ViciousStrikes) + (.03f * (float)talents.Subversion)
                                                + stats.BonusScourgeStrikeCrit) * SSCRitDmgMult);
                            dpsScourgeStrikePhysical *= SSCrit;
                            dpsScourgeStrikePhysical *= 1f + (0.2f / 3f * (float)talents.Outbreak);
                        }
                    }
                }
                #endregion

                #region Unholy Blight
                {
                    // now does 20% damage of each death coil over 10 seconds, with glyph increasing
                    // damage by 40%. It rolls like deep wounds, but luckily, we don't care.
                    if (talents.UnholyBlight > 0f && temp.DeathCoil > 0f && !PTR)
                        dpsUnholyBlight = dpsDeathCoil * (0.2f * (1f + (talents.GlyphofUnholyBlight ? 0.4f : 0f)));
                    else if (talents.UnholyBlight > 0f && temp.DeathCoil > 0f && PTR)
                        dpsUnholyBlight = dpsDeathCoil * (0.1f * (1f + (talents.GlyphofUnholyBlight ? 0.4f : 0f)));
                    else
                        dpsUnholyBlight = 0f;
                }
                #endregion

                #region Howling Blast
                {
                    if (talents.HowlingBlast > 0 && (temp.HowlingBlast) > 0f)
                    {
                        float HBDmg = 540 + HowlingBlastAPMult * stats.AttackPower;

                        float guaranteedCrits = 0f;
                        if (KMProcsPerRotation > temp.HowlingBlast)
                        {
                            guaranteedCrits = temp.HowlingBlast;
                            KMProcsPerRotation -= temp.HowlingBlast;
                        }
                        else
                        {
                            guaranteedCrits = KMProcsPerRotation;
                            KMProcsPerRotation = 0f;
                        }
                        float HBCritDmgMult = 1f + (.5f * (2f + (.15f * (float)talents.GuileOfGorefiend) + stats.BonusCritMultiplier));

                        float guaranteedCritDmg = HBCritDmgMult;
                        guaranteedCritDmg *= guaranteedCrits * HBDmg;
                        temp.HowlingBlast -= guaranteedCrits;

                        float HBCD = combatTable.realDuration / (temp.HowlingBlast);
                        dpsHowlingBlast = HBDmg / HBCD;
                        float HBCrit = 1f + (Math.Min((combatTable.spellCrits), 1f) * HBCritDmgMult);
                        dpsHowlingBlast *= HBCrit;
                        dpsHowlingBlast += guaranteedCritDmg / combatTable.realDuration;
                    }
                }
                #endregion

                #region Frost Strike
                {
                    if (talents.FrostStrike > 0 && temp.FrostStrike > 0f)
                    {
                        float addedCritFromKM = KMProcsPerRotation;
                        float FSDmg = (combatTable.MH.baseDamage + ((stats.AttackPower / 14f) *
                                combatTable.normalizationFactor)) * .55f +
                                110.55f + stats.BonusFrostStrikeDamage;


                        float FSDmgOH = 0f;
                        if (DW) FSDmgOH = (((combatTable.OH.baseDamage + ((stats.AttackPower / 14f) *
                                combatTable.normalizationFactor)) * 0.55f + 110.55f) * 0.5f) + stats.BonusFrostStrikeDamage;
                        FSDmgOH *= 1f + 0.05f * (float)talents.NervesOfColdSteel;
                        FSDmgOH *= (talents.ThreatOfThassarian * 0.33333333f);

                        float FSCritDmgMult = 1f + (.15f * (float)talents.GuileOfGorefiend) + stats.BonusCritMultiplier;
                        float guaranteedCrits = 0f;
                        if (KMProcsPerRotation > temp.FrostStrike)
                        {
                            guaranteedCrits = temp.FrostStrike;
                            KMProcsPerRotation -= guaranteedCrits;
                        }
                        else
                        {
                            guaranteedCrits = KMProcsPerRotation;
                            KMProcsPerRotation = 0f;
                        }
                        float guaranteedCritDmg = 1f + FSCritDmgMult;
                        guaranteedCritDmg *= guaranteedCrits;
                        guaranteedCritDmg *= FSDmg + FSDmgOH;
                        temp.FrostStrike -= guaranteedCrits;
                        float FSCD = combatTable.realDuration / temp.FrostStrike;

                        float FSCrit = 1f + (Math.Min((combatTable.physCrits + stats.BonusFrostStrikeCrit), 1f) * FSCritDmgMult);
                        dpsFrostStrike = FSDmg / FSCD;
                        dpsFrostStrike *= FSCrit;
                        float dpsFrostStrikeOH = FSDmgOH / FSCD;
                        dpsFrostStrikeOH *= FSCrit;
                        dpsFrostStrike += dpsFrostStrikeOH;
                        dpsFrostStrike += guaranteedCritDmg / combatTable.realDuration;
                        dpsFrostStrike *= 1f + (1f / 3f * 0.01f /*0.0333333333333333f*/) * talents.BloodOfTheNorth;
                    }
                }
                #endregion

                #region Bloodworms
                {
                    if (talents.Bloodworms > 0)
                    {
                        float BloodwormSwing = 50f + BloodwormsAPMult * stats.AttackPower;
                        float BloodwormSwingDPS = BloodwormSwing / 2.0f;    // any haste benefits?
                        float TotalBloodworms = ((fightDuration / combatTable.MH.hastedSpeed) + temp.getMeleeSpecialsPerSecond() * fightDuration)
                            * (0.03f * talents.Bloodworms)
                            * 3f /*average of 3 bloodworms per proc*/;
                        dpsBloodworms = ((TotalBloodworms * BloodwormSwingDPS * 20f) / fightDuration);
                    }
                }
                #endregion

                #region Trinket direct-damage procs, razorice damage, etc
                {
                    dpsOtherArcane = stats.ArcaneDamage;
                    dpsOtherShadow = stats.ShadowDamage;
                    dpsOtherFire = stats.FireDamage;

                    // TODO: Differentiate between MH razorice and OH razorice
                    if (combatTable.MH != null)
                    {
                        float dpsMHglancing = (StatConversion.WHITE_GLANCE_CHANCE_CAP[targetLevel - 80] * combatTable.MH.DPS) * 0.7f;
                        float dpsMHBeforeArmor = ((combatTable.MH.DPS * (1f - calcs.AvoidedAttacks - StatConversion.WHITE_GLANCE_CHANCE_CAP[targetLevel - 80])) * (1f + combatTable.physCrits)) + dpsMHglancing;
                        dpsOtherFrost += (dpsMHBeforeArmor - dpsMHglancing) * stats.BonusFrostWeaponDamage;   // presumably doesn't proc off of glancings, like necrosis
                    }

                    if (combatTable.OH != null)
                    {
                        float dpsOHglancing = (StatConversion.WHITE_GLANCE_CHANCE_CAP[targetLevel - 80] * combatTable.OH.DPS) * 0.7f;
                        float dpsOHBeforeArmor = ((combatTable.OH.DPS * (1f - calcs.AvoidedAttacks - StatConversion.WHITE_GLANCE_CHANCE_CAP[targetLevel - 80])) * (1f + combatTable.physCrits)) + dpsOHglancing;
                        dpsOHBeforeArmor *= OHMult;
                        dpsOtherFrost += (dpsOHBeforeArmor - dpsOHglancing) * stats.BonusFrostWeaponDamage;
                    }
                    if (DW) dpsOtherFrost /= 2f; //razorice only actually effects the weapon its on, not both. this is closer than it would be otherwise.

                    float OtherCritDmgMult = .5f * (1f + stats.BonusCritMultiplier);
                    float OtherCrit = 1f + ((combatTable.spellCrits) * OtherCritDmgMult);
                    dpsOtherArcane *= OtherCrit;
                    dpsOtherShadow *= OtherCrit;
                    dpsOtherFire *= OtherCrit;
                }
                #endregion



                #region Obliterate
                {
                    if (temp.Obliterate > 0f)
                    {
                        float OblitCD = combatTable.realDuration / temp.Obliterate;
                        float OblitDmg = ((combatTable.MH.baseDamage + ((stats.AttackPower / 14f) * combatTable.normalizationFactor)) *
                            0.8f) + stats.BonusObliterateDamage + 467.2f;

                        float OblitDmgOH = 0f;
                        if (DW) OblitDmgOH = ((((combatTable.OH.baseDamage + ((stats.AttackPower / 14f) * combatTable.normalizationFactor)) *
                            0.8f) + 467.2f) * 0.5f) + stats.BonusObliterateDamage;
                        OblitDmgOH *= 1f + 0.05f * (float)talents.NervesOfColdSteel;
                        OblitDmgOH *= (talents.ThreatOfThassarian * (1f / 3f/*0.33333333333f*/));

                        dpsObliterate = (OblitDmg + OblitDmgOH) / OblitCD;
                        dpsObliterate *= 1f + 0.125f * (float)temp.AvgDiseaseMult * (1f + stats.BonusPerDiseaseObliterateDamage);
                        float OblitCritDmgMult = 1f + (.15f * (float)talents.GuileOfGorefiend) + stats.BonusCritMultiplier;
                        float OblitCrit = 1f + ((combatTable.physCrits +
                            (0.03f * (float)talents.Subversion) +
                            (0.05f * (float)talents.Rime) +
                            stats.BonusObliterateCrit) * OblitCritDmgMult);
                        dpsObliterate *= OblitCrit;
                        dpsObliterate *= (talents.GlyphofObliterate ? 1.2f : 1f);
                    }
                }
                #endregion

                #region Death Strike
                {
                    if (temp.DeathStrike > 0f)
                    {
                        float DSCD = combatTable.realDuration / temp.DeathStrike;
                        // TODO: This should be changed to make use of the new glyph stats:
                        float DSDmg = ((combatTable.MH.baseDamage + ((stats.AttackPower / 14f) * combatTable.normalizationFactor)) * 0.75f) + 222.75f + stats.BonusDeathStrikeDamage;
                        if (DW)
                        {
                            float DSDmgOH = ((combatTable.OH.baseDamage + ((stats.AttackPower / 14f) *
                            combatTable.normalizationFactor)) * 0.75f) + 222.75f;
                            DSDmgOH *= 0.5f;
                            DSDmgOH += stats.BonusDeathStrikeDamage;
                            DSDmgOH *= 1f + 0.05f * talents.NervesOfColdSteel;
                            DSDmgOH *= (talents.ThreatOfThassarian * (1f / 3f/*0.33333333333333f*/));
                            DSDmg += DSDmgOH;
                        }
                        DSDmg *= 1f + 0.15f * (float)talents.ImprovedDeathStrike;
                        DSDmg *= (talents.GlyphofDeathStrike ? 1.25f : 1f);
                        dpsDeathStrike = DSDmg / DSCD;
                        float DSCritDmgMult = 1f + (.15f * (float)talents.MightOfMograine) + stats.BonusCritMultiplier;
                        float DSCrit = 1f + ((combatTable.physCrits +
                            (.03f * (float)talents.ImprovedDeathStrike) +
                            stats.BonusDeathStrikeCrit) * DSCritDmgMult);
                        dpsDeathStrike *= DSCrit;
                    }
                }
                #endregion

                #region Blood Strike
                {
                    if (temp.BloodStrike > 0f)
                    {
                        float BSCD = combatTable.realDuration / temp.BloodStrike;
                        float BSDmg = ((combatTable.MH.baseDamage + ((stats.AttackPower / 14f) * combatTable.normalizationFactor)) *
                            0.4f) + 305.6f + stats.BonusBloodStrikeDamage;
                        if (DW)
                        {
                            float BSDmgOH = ((combatTable.OH.baseDamage + ((stats.AttackPower / 14f) *
                            combatTable.normalizationFactor)) * 0.4f) + 305.6f;
                            BSDmgOH *= 0.5f;
                            BSDmgOH += stats.BonusBloodStrikeDamage;
                            BSDmgOH *= 1f + 0.05f * (float)talents.NervesOfColdSteel;
                            BSDmgOH *= (talents.ThreatOfThassarian * (1f / 3f/*0.3333333333333333f*/));
                            BSDmg += BSDmgOH;
                        }
                        BSDmg *= 1f + 0.12f * (float)temp.AvgDiseaseMult * (1f + stats.BonusPerDiseaseBloodStrikeDamage);
                        dpsBloodStrike = BSDmg / BSCD;
                        float BSCritDmgMult = 1f + (.15f * (float)talents.MightOfMograine);
                        BSCritDmgMult += (.15f * (float)talents.GuileOfGorefiend) + stats.BonusCritMultiplier;
                        float BSCrit = 1f + ((combatTable.physCrits + (.03f * (float)talents.Subversion)) * BSCritDmgMult);
                        dpsBloodStrike = (dpsBloodStrike) * BSCrit;
                        dpsBloodStrike *= 1f + ((1f / 3f/*0.3333333333333333f*/) * (float)talents.BloodOfTheNorth);
                        dpsBloodStrike *= 1f + (.05f * (float)talents.BloodyStrikes);
                    }
                }
                #endregion

                #region Heart Strike
                {
                    if (talents.HeartStrike > 0 && temp.HeartStrike > 0f)
                    {
                        float HSCD = combatTable.realDuration / temp.HeartStrike;
                        float HSDmg = ((combatTable.MH.baseDamage + ((stats.AttackPower / 14f) * combatTable.normalizationFactor)) *
                            0.5f) + 368f + stats.BonusHeartStrikeDamage;
                        HSDmg *= 1f + 0.1f * (float)temp.AvgDiseaseMult * (1f + stats.BonusPerDiseaseHeartStrikeDamage);
                        dpsHeartStrike = HSDmg / HSCD;
                        //float HSCrit = 1f + combatTable.physCrits + ( .03f * (float)talents.Subversion );
                        float HSCritDmgMult = 1f + (.15f * (float)talents.MightOfMograine) + stats.BonusCritMultiplier;
                        float HSCrit = 1f + ((combatTable.physCrits + (.03f * (float)talents.Subversion)) * HSCritDmgMult);
                        dpsHeartStrike = (dpsHeartStrike) * HSCrit;
                        dpsHeartStrike *= 1f + (.15f * (float)talents.BloodyStrikes);
                    }
                }
                #endregion

                #region Gargoyle
                {
                    if (temp.GargoyleDuration > 0f && talents.SummonGargoyle > 0f)
                    {
                        float GargoyleCastTime = 2.0f;  //2.0 second base cast time
                        GargoyleCastTime *= combatTable.MH.hastedSpeed / combatTable.MH.baseSpeed;
                        // benefits from all haste effects

                        float GargoyleStrike = 120f + GargoyleAPMult * stats.AttackPower;
                        float GargoyleStrikeCount = 30f / GargoyleCastTime;
                        float GargoyleDmg = GargoyleStrike * GargoyleStrikeCount;
                        GargoyleDmg *= 1f + (.5f * .05f);  // crit rate is uninfluenced by stats, but roughly crap
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
                            float numCDs = timeleft / ((4f - .75f * talents.NightOfTheDead) * 60f);
                            float duration = numCDs * 60f;

                            uptime = duration / timeleft;
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
                            GhoulhastedSpeed = GhoulBaseSpeed;

                        if (talents.GhoulFrenzy > 0f)
                        {
                            float GhoulFrenzyHaste = 0.25f * (temp.GhoulFrenzy / combatTable.realDuration) * 30f;
                            GhoulhastedSpeed /= 1f + GhoulFrenzyHaste;
                        }
                        GhoulhastedSpeed /= 1f + statsBuffs.PhysicalHaste;

                        float dmgSwing = GhoulWeaponBaseDamage + (GhoulAP / GhoulAPdivisor) * GhoulBaseSpeed;
                        float dpsSwing = dmgSwing / GhoulhastedSpeed;
                        float dpsGhoulGlancing = (dpsSwing * 0.24f) * 0.75f;

                        float dpsClaw = (1.5f * dmgSwing) / ClawCD;

                        dpsSwing *= (1f - missedSpecial) - 0.24f;   // the Ghoul only has one weapon || Glancings added further down
                        dpsClaw *= 1f - missedSpecial;

                        dpsSwing *= 1f + .054f;// + stats.LotPCritRating / 4591f; // needs other crit modifiers, but doesn't inherit crit from master
                        dpsSwing += dpsGhoulGlancing;

                        dpsClaw *= 1f + .054f;// + stats.LotPCritRating / 4591f; // needs other crit modifiers, but doesn't inherit crit from master

                        dpsGhoul = dpsSwing + dpsClaw;

                        //dpsGhoul *= 1f + statsBuffs.BonusPhysicalDamageMultiplier; 
                        // commented out because ghoul doesn't benefit from most bonus physical damage multipliers (ie blood presence, bloody vengeance, etc)
                        int targetArmor = calcOpts.BossArmor;
                        float modArmor = 1f - StatConversion.GetArmorDamageReduction(character.Level, targetArmor,
                            stats.ArmorPenetration, 0f, 0f * stats.ArmorPenetrationRating);

                        dpsGhoul *= modArmor;
                        dpsGhoul *= 1f - (.0065f * missedSpecial);	// pet expertise now scales with player hit rating. Hopefully goes off of melee rather than spell hit.
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
                float ObliterateMult = 1f;
                float DeathStrikeMult = 1f;
                float PlagueStrikeMult = 1f;
                float ScourgeStrikeMult = 1f;
                float ScourgeStrikePhsyicalMult = 1f;
                float ScourgeStrikeShadowMult = 1f;
                float UnholyBlightMult = 1f;
                float WhiteMult = 1f;
                float WanderingPlagueMult = 1f;
                float otherShadowMult = 1f;
                float otherArcaneMult = 1f;
                float otherFrostMult = 1f;

                #region Apply Physical Mitigation
                {
                    float physMit = mitigation;
                    physMit *= 1f + (!DW ? .02f * talents.TwoHandedWeaponSpecialization : 0f);

                    dpsBCB *= physMit;
                    dpsBloodStrike *= physMit;
                    dpsHeartStrike *= physMit;
                    dpsObliterate *= physMit;
                    dpsDeathStrike *= physMit;
                    dpsPlagueStrike *= physMit;
                    dpsScourgeStrikePhysical *= physMit;
                    dpsBloodworms *= 1f - StatConversion.GetArmorDamageReduction(character.Level, calcOpts.BossArmor, stats.ArmorPenetration, 0f, 0f);

                    WhiteMult *= physPowerMult;
                    BCBMult *= physPowerMult;
                    ScourgeStrikePhsyicalMult *= physPowerMult;
                    BloodStrikeMult *= physPowerMult;
                    HeartStrikeMult *= physPowerMult;
                    ObliterateMult *= physPowerMult;
                    DeathStrikeMult *= physPowerMult;
                    PlagueStrikeMult *= physPowerMult;
                }
                #endregion

                #region Apply Elemental Strike Mitigation
                {
                    float strikeMit = /*missedSpecial **/ partialResist;
                    strikeMit *= (!DW ? 1f + .02f * talents.TwoHandedWeaponSpecialization : 1f);

                    ScourgeStrikeShadowMult *= strikeMit;
                    dpsFrostStrike *= strikeMit * (1f - missedSpecial) * (1f - combatTable.dodgedSpecial);

                    ScourgeStrikeShadowMult *= spellPowerMult;
                    FrostStrikeMult *= frostSpellPowerMult;
                }
                #endregion

                #region Apply Magical Mitigation
                {
                    // some of this applies to necrosis, I wonder if it is ever accounted for
                    float magicMit = partialResist /** combatTable.spellResist*/;
                    // magicMit = 1f - magicMit;

                    dpsBloodPlague *= magicMit;
                    dpsDeathCoil *= magicMit * (1f - combatTable.spellResist);
                    dpsFrostFever *= magicMit;
                    dpsHowlingBlast *= magicMit * (1f - combatTable.spellResist);
                    dpsIcyTouch *= magicMit;
                    dpsUnholyBlight *= magicMit * (1f - combatTable.spellResist);

                    BloodPlagueMult *= spellPowerMult;
                    DeathCoilMult *= spellPowerMult;
                    FrostFeverMult *= frostSpellPowerMult;
                    HowlingBlastMult *= frostSpellPowerMult;
                    IcyTouchMult *= frostSpellPowerMult;
                    UnholyBlightMult *= spellPowerMult;
                    otherShadowMult *= spellPowerMult;
                    otherArcaneMult *= spellPowerMult;
                    otherFrostMult *= frostSpellPowerMult;
                }
                #endregion

                #region Cinderglacier multipliers
                {
                    DeathCoilMult *= CinderglacierMultiplier;
                    HowlingBlastMult *= CinderglacierMultiplier;
                    IcyTouchMult *= CinderglacierMultiplier;
                    ScourgeStrikeShadowMult *= CinderglacierMultiplier;
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

                    float HysteriaCoeff = .2f / 6f; // current uptime is 16.666...%
                    float HysteriaMult = HysteriaCoeff * (float)talents.Hysteria;
                    BCBMult *= 1 + HysteriaMult;
                    BloodStrikeMult *= 1 + HysteriaMult;
                    HeartStrikeMult *= 1 + HysteriaMult;
                    ObliterateMult *= 1 + HysteriaMult;
                    DeathStrikeMult *= 1 + HysteriaMult;
                    PlagueStrikeMult *= 1 + HysteriaMult;
                    WhiteMult *= 1 + HysteriaMult;
                    ScourgeStrikePhsyicalMult *= 1 + HysteriaMult;

                    float BlackIceMult = .02f * (float)talents.BlackIce;
                    FrostFeverMult *= 1 + BlackIceMult;
                    HowlingBlastMult *= 1 + BlackIceMult;
                    IcyTouchMult *= 1 + BlackIceMult;
                    FrostStrikeMult *= 1 + BlackIceMult;
                    DeathCoilMult *= 1 + BlackIceMult;
                    ScourgeStrikeShadowMult *= 1 + BlackIceMult;
                    BloodPlagueMult *= 1 + BlackIceMult;
                    otherShadowMult *= 1 + BlackIceMult;
                    otherFrostMult *= 1 + BlackIceMult;

                    float MercilessCombatMult = .315f * 0.06f * (float)talents.MercilessCombat;   // The last 35% of a Boss don't take 35% of the fight-time...say .315 (10% faster)
                    ObliterateMult *= 1 + MercilessCombatMult;
                    HowlingBlastMult *= 1 + MercilessCombatMult;
                    IcyTouchMult *= 1 + MercilessCombatMult;
                    FrostStrikeMult *= 1 + MercilessCombatMult;

                    float GlacierRot = (.2f / 3f/*0.66666666666666666*/) * (float)talents.GlacierRot;
                    HowlingBlastMult *= 1 + GlacierRot;
                    IcyTouchMult *= 1 + GlacierRot;
                    FrostStrikeMult *= 1 + GlacierRot;


                    float CryptFeverMult = .1f * (float)talents.CryptFever;
                    float CryptFeverBuff = stats.BonusDiseaseDamageMultiplier;
                    CryptFeverMult = Math.Max(CryptFeverMult, CryptFeverBuff);
                    FrostFeverMult *= 1 + CryptFeverMult;
                    BloodPlagueMult *= 1 + CryptFeverMult;
                    // UnholyBlightMult *= 1 + CryptFeverMult;

                    float DesecrationMult = .01f * (float)talents.Desolation;  	// the new desecration is basically a flat 1% per point
                    BCBMult *= 1 + DesecrationMult;								// modelling this any more accurately would require a total revamp
                    BloodPlagueMult *= 1 + DesecrationMult;						// of the rotation system
                    BloodStrikeMult *= 1 + DesecrationMult;
                    DeathCoilMult *= 1 + DesecrationMult;
                    FrostFeverMult *= 1 + DesecrationMult;
                    FrostStrikeMult *= 1 + DesecrationMult;
                    IcyTouchMult *= 1 + DesecrationMult;
                    ObliterateMult *= 1 + DesecrationMult;
                    DeathStrikeMult *= 1 + DesecrationMult;
                    PlagueStrikeMult *= 1 + DesecrationMult;
                    ScourgeStrikePhsyicalMult *= 1 + DesecrationMult;
                    ScourgeStrikeShadowMult *= 1 + DesecrationMult;
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
                        FrostFeverMult *= 1 + BoneMult;
                        FrostStrikeMult *= 1 + BoneMult;
                        IcyTouchMult *= 1 + BoneMult;
                        ObliterateMult *= 1 + BoneMult;
                        DeathStrikeMult *= 1 + BoneMult;
                        PlagueStrikeMult *= 1 + BoneMult;
                        ScourgeStrikePhsyicalMult *= 1 + BoneMult;
                        ScourgeStrikeShadowMult *= 1 + BoneMult;
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

                #region 2T10
                {
                    ScourgeStrikeShadowMult *= 1 + stats.BonusScourgeStrikeMultiplier;
                    ScourgeStrikePhsyicalMult *= 1 + stats.BonusScourgeStrikeMultiplier;
                    ObliterateMult *= 1 + stats.BonusObliterateMultiplier;
                    HeartStrikeMult *= 1 + stats.BonusHeartStrikeMultiplier;
                }
                #endregion

                #region Scourge Strike Is Annoying
                if (PTR)
                {
                    dpsScourgeStrikeShadow = dpsScourgeStrikePhysical * (0.25f * temp.AvgDiseaseMult);
                    dpsScourgeStrike = dpsScourgeStrikePhysical * ScourgeStrikePhsyicalMult + dpsScourgeStrikeShadow * ScourgeStrikeShadowMult;
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
                calcs.NecrosisDPS = dpsWhite * (.04f * talents.Necrosis);
                calcs.ObliterateDPS = dpsObliterate * ObliterateMult;
                calcs.DeathStrikeDPS = dpsDeathStrike * DeathStrikeMult;
                calcs.PlagueStrikeDPS = dpsPlagueStrike * PlagueStrikeMult;
                calcs.ScourgeStrikeDPS = dpsScourgeStrike * (!PTR ? ScourgeStrikeMult : 1f);
                calcs.UnholyBlightDPS = dpsUnholyBlight * DeathCoilMult;
                calcs.WhiteDPS = dpsWhite * WhiteMult;
                calcs.WanderingPlagueDPS = dpsWanderingPlague * WanderingPlagueMult;
                calcs.OtherDPS = dpsOtherShadow * otherShadowMult +
                    dpsOtherArcane * otherArcaneMult +
                    dpsOtherFire * otherArcaneMult +
                    dpsOtherFrost * otherFrostMult;


                calcs.DPSPoints = calcs.BCBDPS + calcs.BloodPlagueDPS + calcs.BloodStrikeDPS + calcs.DeathCoilDPS + calcs.FrostFeverDPS + calcs.FrostStrikeDPS +
                                  calcs.GargoyleDPS + calcs.GhoulDPS + calcs.WanderingPlagueDPS + calcs.HeartStrikeDPS + calcs.HowlingBlastDPS + calcs.IcyTouchDPS +
                                  calcs.NecrosisDPS + calcs.ObliterateDPS + calcs.DeathStrikeDPS + calcs.PlagueStrikeDPS + calcs.ScourgeStrikeDPS + calcs.UnholyBlightDPS +
                                  calcs.WhiteDPS + calcs.OtherDPS + calcs.BloodwormsDPS;

                #region Dancing Rune Weapon
                {
                    if (talents.DancingRuneWeapon > 0)
                    {
                        float dpsDRWMaximum = 0f;
                        String effectsStats = "";
                        Stats maxStats = GetCharacterStatsMaximum(character, additionalItem, 90f);
                        DRW drw;
                        Stats tempStats;

                        foreach (SpecialEffect effect in maxStats.SpecialEffects())
                        {
                            if (effect.Trigger != Trigger.Use)
                            {
                                tempStats = new Stats();
                                tempStats += maxStats;
                                tempStats += effect.Stats;



                                tempStats.Strength += tempStats.HighestStat + tempStats.Paragon;

                                tempStats.Agility = (float)Math.Floor(tempStats.Agility * (1 + tempStats.BonusAgilityMultiplier));
                                tempStats.Strength = (float)Math.Floor(tempStats.Strength * (1 + tempStats.BonusStrengthMultiplier));
                                tempStats.Stamina = (float)Math.Floor(tempStats.Stamina * (1 + tempStats.BonusStaminaMultiplier));
                                tempStats.Intellect = (float)Math.Floor(tempStats.Intellect * (1 + tempStats.BonusIntellectMultiplier));
                                tempStats.Spirit = (float)Math.Floor(tempStats.Spirit * (1 + tempStats.BonusSpiritMultiplier));
                                tempStats.Health = (float)Math.Floor(tempStats.Health + (tempStats.Stamina * 10f));
                                tempStats.Mana = (float)Math.Floor(tempStats.Mana + (tempStats.Intellect * 15f));
                                tempStats.AttackPower = (float)Math.Floor(tempStats.AttackPower + tempStats.Strength * 2);
                                // Copy from TankDK.
                                // tempStats.Armor = (float)Math.Floor((tempStats.Armor + tempStats.BonusArmor + 2f * tempStats.Agility) * 1f);
                                tempStats.Armor = (float)Math.Floor(StatConversion.GetArmorFromAgility(tempStats.Agility) +
                                                    StatConversion.ApplyMultiplier(tempStats.Armor, tempStats.BaseArmorMultiplier) +
                                                    StatConversion.ApplyMultiplier(tempStats.BonusArmor, tempStats.BonusArmorMultiplier));

                                tempStats.AttackPower += (tempStats.Armor / 180f) * (float)talents.BladedArmor;

                                tempStats.BonusSpellPowerMultiplier = tempStats.BonusShadowDamageMultiplier;

                                tempStats.AttackPower *= 1f + tempStats.BonusAttackPowerMultiplier;

                                if (calcOpts.rotation.presence == CalculationOptionsDPSDK.Presence.Blood)  // a final, multiplicative component
                                {
                                    tempStats.BonusPhysicalDamageMultiplier *= 1.15f;
                                    tempStats.BonusSpellPowerMultiplier *= 1.15f;
                                }
                                else if (calcOpts.rotation.presence == CalculationOptionsDPSDK.Presence.Unholy)  // a final, multiplicative component
                                {
                                    tempStats.PhysicalHaste += 0.15f;
                                    tempStats.SpellHaste += 0.15f;
                                }


                                drw = new DRW(combatTable, calcs, calcOpts, tempStats, character, talents);
                                if (drw.dpsDancingRuneWeapon > dpsDRWMaximum)
                                {
                                    dpsDRWMaximum = drw.dpsDancingRuneWeapon;
                                    effectsStats = effect.ToString();
                                }
                            }

                        }
                        dpsDancingRuneWeapon = dpsDRWMaximum;
                        float DRWUptime = (12f + (talents.GlyphofDancingRuneWeapon ? 5f : 0f)) / 90f;
                        dpsDancingRuneWeapon /= 90f;
                        calcs.DPSPoints += dpsDancingRuneWeapon;
                        calcs.DRWDPS = dpsDancingRuneWeapon;
                        calcs.DRWStats = effectsStats;
                    }
                }
                #endregion

                calcs.OverallPoints = calcs.DPSPoints;
            }

            return calcs;
        }

        private Stats GetRaceStats(Character character) {
            return BaseStats.GetBaseStats(character.Level, CharacterClass.DeathKnight, character.Race);
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
        public override Stats GetCharacterStats(Character character, Item additionalItem) {
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
                statsTalents.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats(){ BonusStrengthMultiplier = 0.1f}, 20f, 60f));
            }
            Stats statsTotal = new Stats();
            Stats statsGearEnchantsBuffs = new Stats();

            statsGearEnchantsBuffs = statsBaseGear + statsBuffs + statsRace + statsTalents;

            statsTotal = GetRelevantStats(statsGearEnchantsBuffs);
            statsTotal.Expertise += (float)StatConversion.GetExpertiseFromRating(statsBaseGear.ExpertiseRating);

            StatsSpecialEffects se = new StatsSpecialEffects(character, statsTotal, new CombatTable(character, statsTotal, calcOpts));

            foreach (SpecialEffect effect in statsTotal.SpecialEffects())
            {
                if (HasRelevantStats(effect.Stats))
                {
                    if (effect.Stats.ArmorPenetrationRating > 0f && effect.Stats.ArmorPenetrationRating + statsTotal.ArmorPenetrationRating +
                        (StatConversion.RATING_PER_ARMORPENETRATION / 100) * 2f * talents.BloodGorged > StatConversion.RATING_PER_ARMORPENETRATION)
                    {
                        Stats tempStats = new Stats();
                        tempStats += effect.Stats;
                        SpecialEffect tempEffect = new SpecialEffect(effect.Trigger, tempStats, effect.Duration, effect.Cooldown, effect.Chance, effect.MaxStack);
                        tempEffect.Stats.ArmorPenetrationRating = (StatConversion.RATING_PER_ARMORPENETRATION - statsTotal.ArmorPenetrationRating - (StatConversion.RATING_PER_ARMORPENETRATION / 100f) * 2f * talents.BloodGorged > 0f ? StatConversion.RATING_PER_ARMORPENETRATION - statsTotal.ArmorPenetrationRating - (StatConversion.RATING_PER_ARMORPENETRATION/100f) * 2f * talents.BloodGorged : 0f);
                        se = new StatsSpecialEffects(character, statsTotal, new CombatTable(character, statsTotal, calcOpts));
                        statsTotal += se.getSpecialEffects(calcOpts, tempEffect);
                    }
                    else
                    {
                        se = new StatsSpecialEffects(character, statsTotal, new CombatTable(character, statsTotal, calcOpts));
                        statsTotal += se.getSpecialEffects(calcOpts, effect);
                    }
                }
            }

            statsTotal.Strength += statsTotal.HighestStat + statsTotal.Paragon;

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

            statsTotal.BonusSpellPowerMultiplier++;
            statsTotal.BonusPhysicalDamageMultiplier++;

            if (calcOpts.rotation.presence == CalculationOptionsDPSDK.Presence.Blood)  // a final, multiplicative component
            {
                statsTotal.BonusPhysicalDamageMultiplier *= 1.15f;
                statsTotal.BonusSpellPowerMultiplier *= 1.15f;
            }
            else if (calcOpts.rotation.presence == CalculationOptionsDPSDK.Presence.Unholy)  // a final, multiplicative component
            {
                statsTotal.PhysicalHaste += 0.15f;
                statsTotal.SpellHaste += 0.15f;
            }

            return (statsTotal);
        }

        public Stats GetCharacterStatsMaximum(Character character, Item additionalItem, float abilityCooldown)
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
                statsTalents.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { BonusStrengthMultiplier = 0.1f }, 20f, 60f));
            }
            Stats statsTotal = new Stats();
            Stats statsGearEnchantsBuffs = new Stats();

            statsGearEnchantsBuffs = statsBaseGear + statsBuffs + statsRace + statsTalents;

            statsTotal = GetRelevantStats(statsGearEnchantsBuffs);
            statsTotal.Expertise += (float)StatConversion.GetExpertiseFromRating(statsBaseGear.ExpertiseRating);

            StatsSpecialEffects se = new StatsSpecialEffects(character, statsTotal, new CombatTable(character, statsTotal, calcOpts));
            int temp = 0;
            Stats statsTemp = new Stats();
            foreach (SpecialEffect effect in statsTotal.SpecialEffects())
            {
                statsTemp.AddSpecialEffect(effect);
            }
            foreach (SpecialEffect effect in statsTemp.SpecialEffects())
            {
                if (HasRelevantStats(effect.Stats))
                {
                    if (effect.Trigger == Trigger.Use)
                    {
                        float uptimeMult = 0f;
                        if (effect.Cooldown > abilityCooldown)
                        {
                            for (int i = 0; i * effect.Cooldown < calcOpts.FightLength * 60f; i++)
                            {
                                if (i * effect.Cooldown % abilityCooldown == 0)
                                {
                                    uptimeMult++;
                                }
                            }
                            uptimeMult /= calcOpts.FightLength * 60f / abilityCooldown;
                        }
                        if (effect.Stats.ArmorPenetrationRating > 0f && effect.Stats.ArmorPenetrationRating + statsTotal.ArmorPenetrationRating + 12.31623993f * 2f * talents.BloodGorged > 1232f)
                        {
                            Stats tempStats = new Stats();
                            tempStats += effect.Stats;
                            tempStats.ArmorPenetrationRating = (1232f - statsTotal.ArmorPenetrationRating - 12.31623993f * 2f * talents.BloodGorged > 0f ? 1232f - statsTotal.ArmorPenetrationRating - 12.31623993f * 2f * talents.BloodGorged : 0f);
                            statsTotal += tempStats * uptimeMult;
                        }
                        else
                        {
                            statsTotal += effect.Stats * uptimeMult;
                        }
                    }
                    else
                    {
                        statsTotal.AddSpecialEffect(effect);
                        temp++;
                    }
                }
            }
            return (statsTotal);
        }

        #region custom charts
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

                #region weapon speed charts
                /*               case "MH Weapon Speed":
                    string[] speedList = new String[] {"1.4", "1.6", "1.8", "2.0", "2.2", "2.4", "2.6", "2.8"};
                    Item MH = character.MainHand.Item;
                    Item MH14 = new Item("", MH.Quality, MH.Type, MH.Id, MH.IconPath, MH.Slot, MH.SetName, MH.Unique, new Stats(), MH.SocketBonus, MH.SocketColor1,
                        MH.SocketColor2, MH.SocketColor3, (int)((MH.MinDamage / MH.Speed) * 1.4f), (int)((MH.MaxDamage / MH.Speed) * 1.4f), MH.DamageType, 1.4f, MH.RequiredClasses);
                    Item MH16 = new Item("", MH.Quality, MH.Type, MH.Id, MH.IconPath, MH.Slot, MH.SetName, MH.Unique, new Stats(), MH.SocketBonus, MH.SocketColor1,
                        MH.SocketColor2, MH.SocketColor3, (int)((MH.MinDamage / MH.Speed) * 1.6f), (int)((MH.MaxDamage / MH.Speed) * 1.6f), MH.DamageType, 1.6f, MH.RequiredClasses);
                    Item MH18 = new Item("", MH.Quality, MH.Type, MH.Id, MH.IconPath, MH.Slot, MH.SetName, MH.Unique, new Stats(), MH.SocketBonus, MH.SocketColor1,
                        MH.SocketColor2, MH.SocketColor3, (int)((MH.MinDamage / MH.Speed) * 1.8f), (int)((MH.MaxDamage / MH.Speed) * 1.8f), MH.DamageType, 1.8f, MH.RequiredClasses);
                    Item MH20 = new Item("", MH.Quality, MH.Type, MH.Id, MH.IconPath, MH.Slot, MH.SetName, MH.Unique, new Stats(), MH.SocketBonus, MH.SocketColor1,
                        MH.SocketColor2, MH.SocketColor3, (int)((MH.MinDamage / MH.Speed) * 2f), (int)((MH.MaxDamage / MH.Speed) * 2f), MH.DamageType, 2f, MH.RequiredClasses);
                    Item MH22 = new Item("", MH.Quality, MH.Type, MH.Id, MH.IconPath, MH.Slot, MH.SetName, MH.Unique, new Stats(), MH.SocketBonus, MH.SocketColor1,
                        MH.SocketColor2, MH.SocketColor3, (int)((MH.MinDamage / MH.Speed) * 2.2f), (int)((MH.MaxDamage / MH.Speed) * 2.2f), MH.DamageType, 2.2f, MH.RequiredClasses);
                    Item MH24 = new Item("", MH.Quality, MH.Type, MH.Id, MH.IconPath, MH.Slot, MH.SetName, MH.Unique, new Stats(), MH.SocketBonus, MH.SocketColor1,
                        MH.SocketColor2, MH.SocketColor3, (int)((MH.MinDamage / MH.Speed) * 2.4f), (int)((MH.MaxDamage / MH.Speed) * 2.4f), MH.DamageType, 2.4f, MH.RequiredClasses);
                    Item MH26 = new Item("", MH.Quality, MH.Type, MH.Id, MH.IconPath, MH.Slot, MH.SetName, MH.Unique, new Stats(), MH.SocketBonus, MH.SocketColor1,
                        MH.SocketColor2, MH.SocketColor3, (int)((MH.MinDamage / MH.Speed) * 2.6f), (int)((MH.MaxDamage / MH.Speed) * 2.6f), MH.DamageType, 2.6f, MH.RequiredClasses);
                    Item MH28 = new Item("", MH.Quality, MH.Type, MH.Id, MH.IconPath, MH.Slot, MH.SetName, MH.Unique, new Stats(), MH.SocketBonus, MH.SocketColor1,
                        MH.SocketColor2, MH.SocketColor3, (int)((MH.MinDamage / MH.Speed) * 2.8f), (int)((MH.MaxDamage / MH.Speed) * 2.8f), MH.DamageType, 2.8f, MH.RequiredClasses);
                    Item[] MHitemList = new Item[] {MH14, MH16, MH18, MH20, MH22, MH24, MH26, MH28};

                    baseCalc = GetCharacterCalculations(character) as CharacterCalculationsDPSDK;

                    for (int index = 0; index < MHitemList.Length; index++)
                    {
                        calc = GetCharacterCalculations(character, MHitemList[index]) as CharacterCalculationsDPSDK;

                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = speedList[index];
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
                case "OH Weapon Speed":
                    string[] OHspeedList = new String[] { "1.4", "1.6", "1.8", "2.0", "2.2", "2.4", "2.6", "2.8" };
                    Item OH = character.OffHand.Item;
                    Item OH14 = new Item("", OH.Quality, OH.Type, OH.Id, OH.IconPath, ItemSlot.OffHand, OH.SetName, OH.Unique, new Stats(), OH.SocketBonus, OH.SocketColor1,
                        OH.SocketColor2, OH.SocketColor3, (int)((OH.MinDamage / OH.Speed) * 1.4f), (int)((OH.MaxDamage / OH.Speed) * 1.4f), OH.DamageType, 1.4f, OH.RequiredClasses);
                    Item OH16 = new Item("", OH.Quality, OH.Type, OH.Id, OH.IconPath, ItemSlot.OffHand, OH.SetName, OH.Unique, new Stats(), OH.SocketBonus, OH.SocketColor1,
                        OH.SocketColor2, OH.SocketColor3, (int)((OH.MinDamage / OH.Speed) * 1.6f), (int)((OH.MaxDamage / OH.Speed) * 1.6f), OH.DamageType, 1.6f, OH.RequiredClasses);
                    Item OH18 = new Item("", OH.Quality, OH.Type, OH.Id, OH.IconPath, ItemSlot.OffHand, OH.SetName, OH.Unique, new Stats(), OH.SocketBonus, OH.SocketColor1,
                        OH.SocketColor2, OH.SocketColor3, (int)((OH.MinDamage / OH.Speed) * 1.8f), (int)((OH.MaxDamage / OH.Speed) * 1.8f), OH.DamageType, 1.8f, OH.RequiredClasses);
                    Item OH20 = new Item("", OH.Quality, OH.Type, OH.Id, OH.IconPath, ItemSlot.OffHand, OH.SetName, OH.Unique, new Stats(), OH.SocketBonus, OH.SocketColor1,
                        OH.SocketColor2, OH.SocketColor3, (int)((OH.MinDamage / OH.Speed) * 2f), (int)((OH.MaxDamage / OH.Speed) * 2f), OH.DamageType, 2f, OH.RequiredClasses);
                    Item OH22 = new Item("", OH.Quality, OH.Type, OH.Id, OH.IconPath, ItemSlot.OffHand, OH.SetName, OH.Unique, new Stats(), OH.SocketBonus, OH.SocketColor1,
                        OH.SocketColor2, OH.SocketColor3, (int)((OH.MinDamage / OH.Speed) * 2.2f), (int)((OH.MaxDamage / OH.Speed) * 2.2f), OH.DamageType, 2.2f, OH.RequiredClasses);
                    Item OH24 = new Item("", OH.Quality, OH.Type, OH.Id, OH.IconPath, ItemSlot.OffHand, OH.SetName, OH.Unique, new Stats(), OH.SocketBonus, OH.SocketColor1,
                        OH.SocketColor2, OH.SocketColor3, (int)((OH.MinDamage / OH.Speed) * 2.4f), (int)((OH.MaxDamage / OH.Speed) * 2.4f), OH.DamageType, 2.4f, OH.RequiredClasses);
                    Item OH26 = new Item("", OH.Quality, OH.Type, OH.Id, OH.IconPath, ItemSlot.OffHand, OH.SetName, OH.Unique, new Stats(), OH.SocketBonus, OH.SocketColor1,
                        OH.SocketColor2, OH.SocketColor3, (int)((OH.MinDamage / OH.Speed) * 2.6f), (int)((OH.MaxDamage / OH.Speed) * 2.6f), OH.DamageType, 2.6f, OH.RequiredClasses);
                    Item OH28 = new Item("", OH.Quality, OH.Type, OH.Id, OH.IconPath, ItemSlot.OffHand, OH.SetName, OH.Unique, new Stats(), OH.SocketBonus, OH.SocketColor1,
                        OH.SocketColor2, OH.SocketColor3, (int)((OH.MinDamage / OH.Speed) * 2.8f), (int)((OH.MaxDamage / OH.Speed) * 2.8f), OH.DamageType, 2.8f, OH.RequiredClasses);
                    Item[] OHitemList = new Item[] { OH14, OH16, OH18, OH20, OH22, OH24, OH26, OH28 };

                    baseCalc = GetCharacterCalculations(character) as CharacterCalculationsDPSDK;

                    for (int index = 0; index < OHitemList.Length; index++)
                    {
                        calc = GetCharacterCalculations(character, OHitemList[index]) as CharacterCalculationsDPSDK;

                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = OHspeedList[index];
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
                    return comparisonList.ToArray();*/
                #endregion
                default:
                    return new ComparisonCalculationBase[0];
            }
        }
        #endregion
        
        public override bool IsItemRelevant(Item item)
        {
            if (item.Slot == ItemSlot.OffHand /*  ||
                (item.Slot == ItemSlot.Ranged && item.Type != ItemType.Sigil) */)
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
                BonusArmor = stats.BonusArmor,
                HighestStat = stats.HighestStat,
                Paragon = stats.Paragon,

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
                DiseasesCanCrit = stats.DiseasesCanCrit,

                BonusDeathCoilCrit = stats.BonusDeathCoilCrit,
                BonusDeathStrikeCrit = stats.BonusDeathStrikeCrit,
                BonusFrostStrikeCrit = stats.BonusFrostStrikeCrit,
                BonusObliterateCrit = stats.BonusObliterateCrit,
                BonusHeartStrikeMultiplier = stats.BonusHeartStrikeMultiplier,
                BonusScourgeStrikeMultiplier = stats.BonusScourgeStrikeMultiplier,
                BonusObliterateMultiplier = stats.BonusObliterateMultiplier,
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
                        effect.Trigger == Trigger.ScourgeStrikeHit ||
                        effect.Trigger == Trigger.ObliterateHit ||
                        effect.Trigger == Trigger.DeathStrikeHit ||
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
                        effect.Trigger == Trigger.ScourgeStrikeHit ||
                        effect.Trigger == Trigger.ObliterateHit ||
                        effect.Trigger == Trigger.DeathStrikeHit ||
                        effect.Trigger == Trigger.IcyTouchHit ||
                        effect.Trigger == Trigger.PlagueStrikeHit ||
                        effect.Trigger == Trigger.RuneStrikeHit ||
                        effect.Trigger == Trigger.Use)
                    {
                        foreach (SpecialEffect e in effect.Stats.SpecialEffects())
                        {
                            HasRelevantStats(e.Stats);
                        }
                        return relevantStats(effect.Stats);
                        
                    }
                }
            }
            return relevantStats(stats);
        }
        private bool relevantStats(Stats stats)
        {
            return (stats.Health + stats.Strength + stats.Agility + stats.Stamina + stats.AttackPower + stats.Bloodlust +
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
                + stats.ShadowDamage + stats.ArcaneDamage + stats.CinderglacierProc + stats.BonusFrostWeaponDamage + stats.DiseasesCanCrit + 
                stats.HighestStat + stats.BonusCritMultiplier + stats.Paragon + stats.FireDamage + stats.Armor + stats.BonusArmor
                + stats.BonusDamageMultiplier + stats.BonusPhysicalDamageMultiplier + stats.BonusObliterateMultiplier +
                stats.BonusHeartStrikeMultiplier + stats.BonusScourgeStrikeMultiplier) != 0;
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
