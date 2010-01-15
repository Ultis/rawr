using System;
using System.Collections.Generic;
using System.Text;
#if RAWR3
using System.Windows.Media;
#else
using System.Drawing;
#endif
using System.Xml.Serialization;

namespace Rawr.DPSDK
{
    //[Rawr.Calculations.RawrModelInfo("DPSDK", "spell_deathknight_classicon", CharacterClass.Paladin)]  wont work until wotlk goes live on wowhead
    [Rawr.Calculations.RawrModelInfo("DPSDK", "spell_shadow_deathcoil", CharacterClass.DeathKnight)]
    public class CalculationsDPSDK : CalculationsBase
    {
        public static double hawut = new Random().NextDouble() * DateTime.Now.ToOADate();
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
            cacheChar = character;
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
            float dpsDancingRuneWeapon = 0f;
            float dpsGargoyle = 0f;
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

    /*        if (additionalItem == null || additionalItem.Name != "bananas")
            {
                Rotation r = GenerateRotation(character, stats, talents, combatTable, calcOpts);
            }*/

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
            Rotation r = new Rotation();
            r.copyRotation(calcOpts.rotation);


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
                temp.HowlingBlast += talents.Rime * calcOpts.rotation.Obliterate * 0.05f + 
                    (combatTable.DW ? (talents.ThreatOfThassarian / 3) * talents.Rime * calcOpts.rotation.Obliterate * .05f * (1 - talents.Rime * .05f) : 0);
                    //OH Oblit hits can proc rime as well
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

                    double KMPpR = KMPpM / (60f / temp.CurRotationDuration);
                    float totalAbilities = (float)(temp.FrostStrike + temp.IcyTouch + temp.HowlingBlast);
                    KMProcsPerRotation = (float)KMPpR;
                }
                #endregion

                

                #region Mitigation
                {
                    float targetArmor = calcOpts.BossArmor;

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

                AbilityHandler abilities = new AbilityHandler(stats, combatTable, character, calcOpts);

                #region Blood Caked Blade
                {
                    float dpsMHBCB = 0f;
                    float dpsOHBCB = 0f;
                    if ((combatTable.OH.damage != 0) && (DW || combatTable.MH.damage == 0))
                    {
                        float OHBCBDmg = (float)(combatTable.OH.damage * (.25f + .125f * temp.AvgDiseaseMult));
                        dpsOHBCB = OHBCBDmg / combatTable.OH.hastedSpeed;
                        dpsOHBCB *= OHMult;
                    }
                    if (combatTable.MH.damage != 0)
                    {
                        float MHBCBDmg = (float)(combatTable.MH.damage * (.25f + .125f * temp.AvgDiseaseMult));
                        dpsMHBCB = MHBCBDmg / combatTable.MH.hastedSpeed;
                    }
                    dpsBCB = dpsMHBCB + dpsOHBCB;
                    dpsBCB *= .1f * (float)talents.BloodCakedBlade;
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
                            * 3f; // 3 bloodworms per proc
                        dpsBloodworms = ((TotalBloodworms * BloodwormSwingDPS * 20f) / fightDuration);
                    }
                }
                #endregion

                #region Trinket direct-damage procs, razorice damage, etc
                {
                    dpsOtherArcane = stats.ArcaneDamage;
                    dpsOtherShadow = stats.ShadowDamage;
                    dpsOtherFire = stats.FireDamage;
                    dpsOtherFrost = stats.FrostDamage;

                    // TODO: Differentiate between MH razorice and OH razorice. Hardly matters though, since razorice hits only based off of base damage
                    if (combatTable.MH.baseDamage != 0 && combatTable.MH.hastedSpeed != 0)
                    {
                        dpsOtherFrost += stats.BonusFrostWeaponDamage * combatTable.MH.baseDamage / combatTable.MH.hastedSpeed;
                    }
                    if (combatTable.OH.baseDamage != 0 && combatTable.OH.hastedSpeed != 0)
                    {
                        dpsOtherFrost += stats.BonusFrostWeaponDamage * combatTable.OH.baseDamage / combatTable.OH.hastedSpeed;
                    }
                    if (DW) dpsOtherFrost /= 2f; //razorice only actually effects the weapon its on, not both. this is closer than it would be otherwise.

                    float OtherCritDmgMult = .5f * (1f + stats.BonusCritMultiplier);
                    float OtherCrit = 1f + ((combatTable.spellCrits) * OtherCritDmgMult);
                    dpsOtherArcane *= OtherCrit;
                    //dpsOtherShadow *= OtherCrit;
                    dpsOtherFire *= OtherCrit;
                    //dpsOtherFrost *= OtherCrit;
                }
                #endregion
                if (talents.HeartStrike > 0)
                {
                    BloodCycle cycle = new BloodCycle(character, combatTable, stats, calcOpts, abilities);
                    Rotation rot = cycle.GetDamage((int)(calcOpts.FightLength * 60 * 1000));
                    rot.AvgDiseaseMult = 2;
                    rot.NumDisease = 2;
                    rot.CurRotationDuration = calcOpts.FightLength * 60;
                    calcOpts.rotation.copyRotation(rot);
                    r.copyRotation(rot);
                }
                else if (talents.ScourgeStrike > 0)
                {
                    UnholyCycle cycle = new UnholyCycle(character, combatTable, stats, calcOpts, abilities);
                    Rotation rot = cycle.GetDamage((int)(calcOpts.FightLength * 60 * 1000));
                    rot.AvgDiseaseMult = 3;
                    rot.NumDisease = 3;
                    rot.CurRotationDuration = calcOpts.FightLength * 60;
                    calcOpts.rotation.copyRotation(rot);
                    r.copyRotation(rot);
                }
                else if (talents.FrostStrike > 0)
                {
                    FrostCycle primaryCycle = new FrostCycle(character, combatTable, stats, calcOpts, abilities);
                    Rotation rot = primaryCycle.GetDamage((int)(calcOpts.FightLength * 60 * 1000));
                    rot.AvgDiseaseMult = 2;
                    rot.NumDisease = 2;
                    rot.CurRotationDuration = calcOpts.FightLength * 60;
                    calcOpts.rotation.copyRotation(rot);
                    r.copyRotation(rot);
                }
                else
                {
                    // add something to handle stupid rotations here, that or tell people to go fist themselves.
                    r.copyRotation(calcOpts.rotation);
                }
                    
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
                            float GhoulAPdivisor = 14f;           // 

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
                                float GhoulFrenzyHaste = (float)(0.25f * (temp.GhoulFrenzy / combatTable.realDuration) * 30f);
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
                    float DancingRuneWeaponMult = 1f;
                    float GargoyleMult = 1f + commandMult;
                    float GhoulMult = 1f + commandMult;
                    float BloodwormsMult = 1f + commandMult;
                    float WhiteMult = 1f;
                    float otherShadowMult = 1f;
                    float otherArcaneMult = 1f;
                    float otherFrostMult = 1f;

                    #region Apply Physical Mitigation
                    {
                        float physMit = mitigation;
                        physMit *= 1f + (!DW ? .02f * talents.TwoHandedWeaponSpecialization : 0f);

                        dpsBCB *= physMit;
                        dpsBloodworms *= 1f - StatConversion.GetArmorDamageReduction(character.Level, calcOpts.BossArmor, stats.ArmorPenetration, 0f, 0f);

                        WhiteMult *= physPowerMult * (1f + (!DW ? .02f * talents.TwoHandedWeaponSpecialization : 0f));
                        BCBMult *= physPowerMult * (1f + (!DW ? .02f * talents.TwoHandedWeaponSpecialization : 0f));
                    }
                    #endregion

                    #region Apply Magical Mitigation
                    {
                        // some of this applies to necrosis, I wonder if it is ever accounted for
                        float magicMit = partialResist /** combatTable.spellResist*/;
                        // magicMit = 1f - magicMit;

                        otherShadowMult *= spellPowerMult;
                        otherArcaneMult *= spellPowerMult;
                        otherFrostMult *= frostSpellPowerMult;
                    }
                    #endregion

                    #region Cinderglacier multipliers
                    {
                        #region Cinderglacier
                        {
                            if (stats.CinderglacierProc > 0f && combatTable.MH != null)
                            {
                                float shadowFrostAbilitiesPerSecond = (float)((r.DeathCoil + r.FrostStrike +
                                    r.ScourgeStrike + r.IcyTouch + r.HowlingBlast) /
                                    combatTable.realDuration);
                                float CGPercentChance = 1.5f / (60f / combatTable.MH.baseSpeed);
                                float CGPPM = ((60f / combatTable.MH.hastedSpeed) * CGPercentChance) * 1f - combatTable.totalMHMiss; // KM Procs per Minute (Defined "1 per point" by Blizzard) influenced by Phys. Haste
                                CGPPM += (1.5f / (60f / combatTable.MH.baseSpeed)) * ((combatTable.totalMeleeAbilities * (1f - combatTable.missedSpecial) + combatTable.totalSpellAbilities * (1f - combatTable.spellResist)) * 60f / combatTable.realDuration);
                                float ProcsPerAbility = CGPPM / (shadowFrostAbilitiesPerSecond * 60f);
                                CinderglacierMultiplier = 1f + 2f * 0.2f * (ProcsPerAbility);

                            }
                        }
                        #endregion
                        abilities.DC.DamageMod *= CinderglacierMultiplier;
                        abilities.HB.DamageMod *= CinderglacierMultiplier;
                        abilities.IT.DamageMod *= CinderglacierMultiplier;
                        abilities.FS.DamageMod *= CinderglacierMultiplier;
                    }
                    #endregion

                    #region Apply Multi-Ability Talent Multipliers
                    {
                        float BloodyVengeanceMult = .03f * (float)talents.BloodyVengeance;
                        BCBMult *= 1 + BloodyVengeanceMult;
                        WhiteMult *= 1 + BloodyVengeanceMult;

                        float HysteriaCoeff = .2f / 6f; // current uptime is 16.666...%
                        float HysteriaMult = HysteriaCoeff * (float)talents.Hysteria;
                        BCBMult *= 1 + HysteriaMult;
                        WhiteMult *= 1 + HysteriaMult;

                        float BlackIceMult = .02f * (float)talents.BlackIce;
                        otherShadowMult *= 1 + BlackIceMult;
                        otherFrostMult *= 1 + BlackIceMult;

                        float DesecrationMult = .01f * (float)talents.Desolation;  	// the new desecration is basically a flat 1% per point
                        BCBMult *= 1 + DesecrationMult;
                        WhiteMult *= 1 + DesecrationMult;
                        otherShadowMult *= 1 + DesecrationMult;
                        otherArcaneMult *= 1 + DesecrationMult;
                        otherFrostMult *= 1 + DesecrationMult;

                        if ((float)talents.BoneShield >= 1f)
                        {
                            float BoneMult = .02f;
                            BCBMult *= 1 + BoneMult;
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
                calcs.DRWDPS = dpsDancingRuneWeapon * DancingRuneWeaponMult;
                calcs.GargoyleDPS = dpsGargoyle * GargoyleMult;
                calcs.GhoulDPS = dpsGhoul * GhoulMult;
                calcs.BloodwormsDPS = dpsBloodworms * BloodwormsMult;
                calcs.NecrosisDPS = dpsWhite * (.04f * talents.Necrosis);
                calcs.WhiteDPS = dpsWhite * WhiteMult;
                calcs.OtherDPS = dpsOtherShadow * otherShadowMult +
                    dpsOtherArcane * otherArcaneMult +
                    dpsOtherFire * otherArcaneMult +
                    dpsOtherFrost * otherFrostMult;

                if (talents.FrostStrike == 1)
                {
                    calcs.BloodPlagueDPS = (float)((abilities.BP.Damage * r.BPTick) / (calcOpts.FightLength * 60));
                    calcs.BloodStrikeDPS = (float)((abilities.BS.Damage * r.BloodStrike) / (calcOpts.FightLength * 60));
                    calcs.FrostFeverDPS = (float)((abilities.FF.Damage * r.FFTick) / (calcOpts.FightLength * 60));
                    calcs.FrostStrikeDPS = (float)((abilities.FS.Damage * r.FrostStrike + abilities.FS.SecondaryDamage * r.KMFS) / (calcOpts.FightLength * 60));
                    calcs.HowlingBlastDPS = (float)((abilities.HB.Damage * r.HowlingBlast + abilities.HB.SecondaryDamage * r.KMRime) / (calcOpts.FightLength * 60));
                    calcs.IcyTouchDPS = (float)((abilities.IT.Damage * r.IcyTouch) / (calcOpts.FightLength * 60));
                    calcs.ObliterateDPS = (float)((abilities.OB.Damage * r.Obliterate) / (calcOpts.FightLength * 60));
                    calcs.PlagueStrikeDPS = (float)((abilities.PS.Damage * r.PlagueStrike) / (calcOpts.FightLength * 60));
                }

                if (talents.HeartStrike == 1)
                {
                    calcs.HeartStrikeDPS = (float)((abilities.HS.Damage * r.HeartStrike) / (calcOpts.FightLength * 60));
                    calcs.DeathStrikeDPS = (float)((abilities.DS.Damage * r.DeathStrike) / (calcOpts.FightLength * 60));
                    calcs.DeathCoilDPS = (float)((abilities.DC.Damage * r.DeathCoil) / (calcOpts.FightLength * 60));
                    calcs.IcyTouchDPS = (float)((abilities.IT.Damage * r.IcyTouch) / (calcOpts.FightLength * 60));
                    calcs.PlagueStrikeDPS = (float)((abilities.PS.Damage * r.PlagueStrike) / (calcOpts.FightLength * 60));
                    calcs.BloodPlagueDPS = (float)((abilities.BP.Damage * r.BPTick) / (calcOpts.FightLength * 60));
                    calcs.FrostFeverDPS = (float)((abilities.FF.Damage * r.FFTick) / (calcOpts.FightLength * 60));
                }

                if (talents.ScourgeStrike == 1)
                {
                    calcs.ScourgeStrikeDPS = (float)(((abilities.SS.Damage + abilities.SS.SecondaryDamage) * r.ScourgeStrike) / (calcOpts.FightLength * 60));
                    calcs.BloodStrikeDPS = (float)((abilities.BS.Damage * r.BloodStrike) / (calcOpts.FightLength * 60));
                    calcs.DeathCoilDPS = (float)((abilities.DC.Damage * r.DeathCoil) / (calcOpts.FightLength * 60));
                    calcs.IcyTouchDPS = (float)((abilities.IT.Damage * r.IcyTouch) / (calcOpts.FightLength * 60));
                    calcs.PlagueStrikeDPS = (float)((abilities.PS.Damage * r.PlagueStrike) / (calcOpts.FightLength * 60));
                    calcs.BloodPlagueDPS = (float)((abilities.BP.Damage * r.BPTick) / (calcOpts.FightLength * 60));
                    calcs.FrostFeverDPS = (float)((abilities.FF.Damage * r.FFTick) / (calcOpts.FightLength * 60));

                    if (talents.UnholyBlight > 0)
                    {
                        float modifier = (0.1f * (talents.GlyphofUnholyBlight ? 1.4f : 1));
                        calcs.UnholyBlightDPS = calcs.DeathCoilDPS * modifier;
                    }
                }

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
                                    tempStats.BonusShadowDamageMultiplier *= 1.15f;
                                    tempStats.BonusFrostDamageMultiplier *= 1.15f;
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
            cacheChar = character;
            CalculationOptionsDPSDK calcOpts = character.CalculationOptions as CalculationOptionsDPSDK;
            DeathKnightTalents talents = calcOpts.talents;

            Stats statsRace = GetRaceStats(character);
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            //Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character, calcOpts);
            Stats statsTalents = new Stats()
            {
                BonusStrengthMultiplier = .01f * (float)(talents.AbominationsMight + talents.RavenousDead) + .02f * (float)(/*talents.ShadowOfDeath + */talents.VeteranOfTheThirdWar),
                BaseArmorMultiplier = .03f * (float)(talents.Toughness),
                BonusStaminaMultiplier = .02f * (float)(/*talents.ShadowOfDeath + */talents.VeteranOfTheThirdWar),
                Expertise = (float)(talents.TundraStalker + talents.RageOfRivendare) + 2f * (float)(talents.VeteranOfTheThirdWar),
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

            statsTotal.Strength += statsTotal.HighestStat + statsTotal.Paragon + statsTotal.DeathbringerProc/3;
            statsTotal.HasteRating += statsTotal.DeathbringerProc/3;
            statsTotal.CritRating += statsTotal.DeathbringerProc/3;

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

            statsTotal.BonusSpellPowerMultiplier++;
            statsTotal.BonusFrostDamageMultiplier++;
            statsTotal.BonusShadowDamageMultiplier++;

            statsTotal.AttackPower *= 1f + statsTotal.BonusAttackPowerMultiplier;

            statsTotal.BonusPhysicalDamageMultiplier++;

            if (calcOpts.rotation.presence == CalculationOptionsDPSDK.Presence.Blood)  // a final, multiplicative component
            {
                statsTotal.BonusPhysicalDamageMultiplier *= 1.15f;
                statsTotal.BonusSpellPowerMultiplier *= 1.15f;
                statsTotal.BonusShadowDamageMultiplier *= 1.15f;
                statsTotal.BonusFrostDamageMultiplier *= 1.15f;
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
                SpellCritOnTarget = stats.SpellCritOnTarget,
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
                BonusSpellPowerMultiplier = stats.BonusSpellPowerMultiplier,

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
                DeathbringerProc = stats.DeathbringerProc,

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
                + stats.PhysicalHit + stats.SpellCrit + stats.SpellCritOnTarget + stats.SpellHit + stats.SpellHaste + stats.BonusDiseaseDamageMultiplier
                + stats.BonusBloodStrikeDamage + stats.BonusDeathCoilDamage + stats.BonusDeathStrikeDamage + stats.BonusFrostStrikeDamage
                + stats.BonusHeartStrikeDamage + stats.BonusIcyTouchDamage + stats.BonusObliterateDamage + stats.BonusScourgeStrikeDamage
                + stats.BonusDeathCoilCrit + stats.BonusDeathStrikeCrit + stats.BonusFrostStrikeCrit + stats.BonusObliterateCrit
                + stats.BonusPerDiseaseBloodStrikeDamage + stats.BonusPerDiseaseHeartStrikeDamage + stats.BonusPerDiseaseObliterateDamage
                + stats.BonusPerDiseaseScourgeStrikeDamage + stats.BonusPlagueStrikeCrit + stats.BonusRPFromDeathStrike
                + stats.BonusRPFromObliterate + stats.BonusRPFromScourgeStrike + stats.BonusRuneStrikeMultiplier + stats.BonusScourgeStrikeCrit
                + stats.ShadowDamage + stats.ArcaneDamage + stats.CinderglacierProc + stats.BonusFrostWeaponDamage + stats.DiseasesCanCrit + 
                stats.HighestStat + stats.BonusCritMultiplier + stats.Paragon + stats.FireDamage + stats.Armor + stats.BonusArmor
                + stats.BonusDamageMultiplier + stats.BonusPhysicalDamageMultiplier + stats.BonusObliterateMultiplier +
                stats.BonusHeartStrikeMultiplier + stats.BonusScourgeStrikeMultiplier + stats.DeathbringerProc + stats.FrostDamage) != 0;
        }

        public Stats GetBuffsStats(Character character, CalculationOptionsDPSDK calcOpts) {
            List<Buff> removedBuffs = new List<Buff>();
            List<Buff> addedBuffs = new List<Buff>();

            //float hasRelevantBuff;

            #region Racials to Force Enable
            // Draenei should always have this buff activated
            // NOTE: for other races we don't wanna take it off if the user has it active, so not adding code for that
            if (character.Race == CharacterRace.Draenei
                && !character.ActiveBuffs.Contains(Buff.GetBuffByName("Heroic Presence")))
            {
                character.ActiveBuffsAdd(("Heroic Presence"));
            }
            #endregion

            #region Passive Ability Auto-Fixing
            // Removes the Trueshot Aura Buff and it's equivalents Unleashed Rage and Abomination's Might if you are
            // maintaining it yourself. We are now calculating this internally for better accuracy and to provide
            // value to relevant talents
            /*{
                hasRelevantBuff = character.HunterTalents.TrueshotAura;
                Buff a = Buff.GetBuffByName("Trueshot Aura");
                Buff b = Buff.GetBuffByName("Unleashed Rage");
                Buff c = Buff.GetBuffByName("Abomination's Might");
                if (hasRelevantBuff > 0)
                {
                    if (character.ActiveBuffs.Contains(a)) { character.ActiveBuffs.Remove(a); removedBuffs.Add(a); }
                    if (character.ActiveBuffs.Contains(b)) { character.ActiveBuffs.Remove(b); removedBuffs.Add(b); }
                    if (character.ActiveBuffs.Contains(c)) { character.ActiveBuffs.Remove(c); removedBuffs.Add(c); }
                }
            }*/
            #endregion

            #region Special Pot Handling
            /*foreach (Buff potionBuff in character.ActiveBuffs.FindAll(b => b.Name.Contains("Potion")))
            {
                if (potionBuff.Stats._rawSpecialEffectData != null
                    && potionBuff.Stats._rawSpecialEffectData[0] != null)
                {
                    Stats newStats = new Stats();
                    newStats.AddSpecialEffect(new SpecialEffect(potionBuff.Stats._rawSpecialEffectData[0].Trigger,
                                                                potionBuff.Stats._rawSpecialEffectData[0].Stats,
                                                                potionBuff.Stats._rawSpecialEffectData[0].Duration,
                                                                calcOpts.Duration,
                                                                potionBuff.Stats._rawSpecialEffectData[0].Chance,
                                                                potionBuff.Stats._rawSpecialEffectData[0].MaxStack));

                    Buff newBuff = new Buff() { Stats = newStats };
                    character.ActiveBuffs.Remove(potionBuff);
                    character.ActiveBuffsAdd(newBuff);
                    removedBuffs.Add(potionBuff);
                    addedBuffs.Add(newBuff);
                }
            }*/
            #endregion

            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            foreach (Buff b in removedBuffs) {
                character.ActiveBuffsAdd(b);
            }
            foreach (Buff b in addedBuffs) {
                character.ActiveBuffs.Remove(b);
            }

            return statsBuffs;
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
