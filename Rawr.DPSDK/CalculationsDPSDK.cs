using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DPSDK
{
    //[Rawr.Calculations.RawrModelInfo("DPSDK", "spell_deathknight_classicon", Character.CharacterClass.Paladin)]  wont work until wotlk goes live on wowhead
    [Rawr.Calculations.RawrModelInfo("DPSDK", "spell_shadow_deathcoil", Character.CharacterClass.DeathKnight)]
	class CalculationsDPSDK : CalculationsBase
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
            get
            {
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
					    "Basic Stats:Strength",
					    "Basic Stats:Agility",
					    "Basic Stats:Attack Power",
					    "Basic Stats:Crit Rating",
					    "Basic Stats:Hit Rating",
					    "Basic Stats:Expertise",
					    "Basic Stats:Haste Rating",
					//    "Basic Stats:Armor Penetration",
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
                        "DPS Breakdown:Blood Strike",
                        "DPS Breakdown:Heart Strike",
                        "DPS Breakdown:DRW*Dancing Rune Weapon",
                        "DPS Breakdown:Gargoyle",
                        "DPS Breakdown:Wandering Plague",
                        "DPS Breakdown:Ghoul",
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


        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
        public override CalculationOptionsPanelBase CalculationOptionsPanel
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
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsDPSDK));
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
        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
        {
            CalculationOptionsDPSDK calcOpts = character.CalculationOptions as CalculationOptionsDPSDK;
            GetTalents(character);
            Stats stats = GetCharacterStats(character, additionalItem);

            CharacterCalculationsDPSDK calcs = new CharacterCalculationsDPSDK();
            calcs.BasicStats = stats;
            calcs.ActiveBuffs = new List<Buff>(character.ActiveBuffs);
            calcs.Talents = calcOpts.talents;

            //DPS Subgroups
            float dpsWhite = 0f;
            float dpsBCB = 0f;
            float dpsNecrosis = 0f;
            float dpsWindfury = 0f;
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
            float dpsBloodStrike = 0f;
            float dpsHeartStrike = 0f;
            float dpsDancingRuneWeapon = 0f;
            float dpsGargoyle = 0f;
            float dpsWanderingPlague = 0f;
            float dpsWPFromFF = 0f;
            float dpsWPFromBP = 0f;
            float dpsGhoul = 0f;

            //shared variables
            DeathKnightTalents talents = calcOpts.talents;
            bool DW = character.MainHand != null && character.OffHand != null;
            float missedSpecial = 0f, dodgedSpecial = 0f;
            float hitBonus = .01f * (float)talents.NervesOfColdSteel;
            float dpsWhiteBeforeArmor = 0f;
            float dpsWhiteMinusGlancing = 0f;
            float combinedSwingTime = 2f;
            float fightDuration = calcOpts.FightLength * 60;
            float mitigation, physCrits, spellCrits, spellResist, totalMHMiss, totalOHMiss, realDuration;
            float KMRatio = 0f;

            float MHExpertise = stats.Expertise;
            float OHExpertise = stats.Expertise;

            //damage multipliers
            float spellPowerMult = 1f + stats.BonusSpellPowerMultiplier;
            float frostSpellPowerMult = spellPowerMult; //implement razorice here later

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
            float GargoyleAPMult = 0.3f;   // pre 3.0.8 == 0.42f...now probably ~0.3f                   

            //for estimating rotation pushback
            float totalMeleeAbilities = 0f;
            float totalSpellAbilities = 0f;

            totalMeleeAbilities = calcOpts.rotation.PlagueStrike + calcOpts.rotation.ScourgeStrike + calcOpts.rotation.Obliterate + calcOpts.rotation.BloodStrike + calcOpts.rotation.HeartStrike + calcOpts.rotation.FrostStrike;
            totalSpellAbilities = calcOpts.rotation.DeathCoil + calcOpts.rotation.IcyTouch + calcOpts.rotation.HowlingBlast;

            calcOpts.rotation.avgDiseaseMult = calcOpts.rotation.numDisease * (calcOpts.rotation.diseaseUptime / 100);
            float commandMult = 0f;

            int numt7 = 0;

            #region T7setbonus
            {
                // Tier 7 set bonus hack
                if (character.Chest != null)
                {
                    if (character.Chest.SetName != null && character.Chest.SetName.Contains("Battlegear"))
                    {
                        numt7++;
                    }
                }
                if (character.Head != null)
                {
                    if (character.Head.SetName != null && character.Head.SetName.Contains("Battlegear"))
                    {
                        numt7++;
                    }
                }
                if (character.Hands != null)
                {
                    if (character.Hands.SetName != null && character.Hands.SetName.Contains("Battlegear"))
                    {
                        numt7++;
                    }
                }
                if (character.Legs != null)
                {
                    if (character.Legs.SetName != null && character.Legs.SetName.Contains("Battlegear"))
                    {
                        numt7++;
                    }
                }
                if (character.Shoulders != null)
                {
                    if (character.Shoulders.SetName != null && character.Shoulders.SetName.Contains("Battlegear"))
                    {
                        numt7++;
                    }
                }
            }
            #endregion

            #region Impurity Application
            {
                float impurityMult = 1f + (.05f * (float)talents.Impurity);

                HowlingBlastAPMult *= impurityMult;
                IcyTouchAPMult *= impurityMult;
                FrostFeverAPMult *= impurityMult;
                BloodPlagueAPMult *= impurityMult;
                DeathCoilAPMult *= impurityMult;
                UnholyBlightAPMult *= impurityMult;
                GargoyleAPMult *= impurityMult;
            }
            #endregion

            if (character.Race == Character.CharacterRace.Dwarf)
            {
                if (character.MainHand != null &&
                    (character.MainHand.Type == Item.ItemType.OneHandMace ||
                     character.MainHand.Type == Item.ItemType.TwoHandMace))
                {
                    MHExpertise += 5f;
                }

                if (character.OffHand != null && character.OffHand.Type == Item.ItemType.OneHandMace)
                {
                    OHExpertise += 5f;
                }
            }
            else if (character.Race == Character.CharacterRace.Orc)
            {
                if (character.MainHand != null &&
                    (character.MainHand.Type == Item.ItemType.OneHandAxe ||
                     character.MainHand.Type == Item.ItemType.TwoHandAxe))
                {
                    MHExpertise += 5f;
                }

                if (character.OffHand != null && character.OffHand.Type == Item.ItemType.OneHandAxe)
                {
                    OHExpertise += 5f;
                }
                commandMult += .05f;
            }
            if (character.Race == Character.CharacterRace.Human)
            {
                if (character.MainHand != null &&
                    (character.MainHand.Type == Item.ItemType.OneHandSword ||
                     character.MainHand.Type == Item.ItemType.TwoHandSword ||
                     character.MainHand.Type == Item.ItemType.OneHandMace ||
                     character.MainHand.Type == Item.ItemType.TwoHandMace))
                {
                    MHExpertise += 3f;
                }

                if (character.OffHand != null &&
                    (character.OffHand.Type == Item.ItemType.OneHandSword ||
                    character.OffHand.Type == Item.ItemType.OneHandMace))
                {
                    OHExpertise += 3f;
                }
            }

            Weapon MH = new Weapon(null, null, null, 0f), OH = new Weapon(null, null, null, 0f);

            if (character.MainHand != null)
            {
                MH = new Weapon(character.MainHand, stats, calcOpts, MHExpertise);
                calcs.MHAttackSpeed = MH.hastedSpeed;
                calcs.MHWeaponDamage = MH.damage;
                calcs.MHExpertise = MH.effectiveExpertise;
            }

            if (character.OffHand != null)
            {
                OH = new Weapon(character.OffHand, stats, calcOpts, OHExpertise);

                float OHMult = .05f * (float)talents.NervesOfColdSteel;
                OH.damage *= .5f + OHMult;

                //need this for weapon swing procs
                //combinedSwingTime = 1f / MH.hastedSpeed + 1f / OH.hastedSpeed;
                //combinedSwingTime = 1f / combinedSwingTime;
                combinedSwingTime = (MH.hastedSpeed + OH.hastedSpeed) / 4;
                calcs.OHAttackSpeed = OH.hastedSpeed;
                calcs.OHWeaponDamage = OH.damage;
                calcs.OHExpertise = OH.effectiveExpertise;
            }
            else
            {
                MH.damage *= 1f + (.02f * talents.TwoHandedWeaponSpecialization);
                combinedSwingTime = MH.hastedSpeed;
                calcs.OHAttackSpeed = 0f;
                calcs.OHWeaponDamage = 0f;
                calcs.OHExpertise = 0f;
            }

            if (character.MainHand == null && character.OffHand == null)
            {
                combinedSwingTime = 2f;
                MH = new Weapon(null, stats, calcOpts, 0f);
            }

            #region Mitigation
            {
                float targetArmor = calcOpts.BossArmor, totalArP = stats.ArmorPenetration;

                // Effective armor after ArP
                targetArmor -= totalArP;
                float ratingCoeff = stats.ArmorPenetrationRating / 1539f;
                targetArmor *= (1 - ratingCoeff);
                if (targetArmor < 0) targetArmor = 0f;

                // Convert armor to mitigation
                //mitigation = 1f - (targetArmor/(targetArmor + 10557.5f));
                //mitigation = 1f - targetArmor / (targetArmor + 400f + 85f * (5.5f * (float)calcOpts.TargetLevel - 265.5f));
                mitigation = 1f - (targetArmor / ((467.5f * (float)calcOpts.TargetLevel) + targetArmor - 22167.5f));
                calcs.EnemyMitigation = 1f - mitigation;
                calcs.EffectiveArmor = targetArmor;
            }
            #endregion

            #region Crits, Resists
            {
                // Attack Rolltable (DW):
                // 27.0% miss     (9.0% with 2H)
                //  6.5% dodge
                // 24.0% glancing (75% hit-dmg)
                // xx.x% crit
                // remaining = hit

                // Crit: Base .65%
                physCrits = .0065f;
                physCrits += stats.CritRating / 4591f;
                physCrits += stats.Agility / 6250f;
                physCrits += .01f * (float)(talents.DarkConviction + talents.EbonPlaguebringer + talents.Annihilation);
                physCrits += stats.PhysicalCrit;
                calcs.CritChance = physCrits;

                float chanceAvoided = 0.335f;

                float chanceDodged = 0.065f;

                calcs.DodgedMHAttacks = MH.chanceDodged;
                calcs.DodgedOHAttacks = OH.chanceDodged;

                if (character.MainHand != null)
                {
                    chanceDodged = MH.chanceDodged;
                }

                if (character.OffHand != null)
                {
                    if (character.MainHand != null)
                    {
                        chanceDodged += OH.chanceDodged;
                        chanceDodged /= 2;
                    }
                    else
                    {
                        chanceDodged = OH.chanceDodged;
                    }
                }

                calcs.DodgedAttacks = chanceDodged;

                float chanceMiss = 0f;
                if (character.OffHand == null) chanceMiss = .08f;
                else chanceMiss = .27f;
                chanceMiss -= stats.HitRating / 3279f;
                chanceMiss -= hitBonus;
                chanceMiss -= stats.PhysicalHit;
                if (chanceMiss < 0f) chanceMiss = 0f;
                calcs.MissedAttacks = chanceMiss;

                chanceAvoided = chanceDodged + chanceMiss;
                calcs.AvoidedAttacks = chanceDodged + chanceMiss;

                chanceMiss = .08f;
                chanceMiss -= stats.HitRating / 3279f;
                chanceMiss -= hitBonus;
                chanceMiss -= stats.PhysicalHit;
                if (chanceMiss < 0f) chanceMiss = 0f;
                chanceDodged = MH.chanceDodged;
                missedSpecial = chanceMiss;
                dodgedSpecial = chanceDodged;
                // calcs.MissedAttacks = chanceMiss           

                spellCrits = 0f;
                spellCrits += stats.CritRating / 4591;
                spellCrits += stats.SpellCrit;
                spellCrits += .01f * (float)(talents.DarkConviction + talents.EbonPlaguebringer);
                calcs.SpellCritChance = spellCrits;

                // Resists: Base 17%
                spellResist = .17f;
                spellResist -= stats.HitRating / 2624f;
                spellResist -= hitBonus + (.01f * talents.Virulence);
                spellResist -= stats.SpellHit;
                if (spellResist < 0f) spellResist = 0f;

                // Total physical misses
                totalMHMiss = calcs.DodgedMHAttacks + chanceMiss;
                totalOHMiss = calcs.DodgedOHAttacks + chanceMiss;
                realDuration = calcOpts.rotation.curRotationDuration;
                realDuration += ((totalMeleeAbilities - calcOpts.rotation.FrostStrike) * chanceDodged * 1.5f) +
                    ((totalMeleeAbilities - calcOpts.rotation.FrostStrike) * chanceMiss * 1.5f) +
                    ((calcOpts.rotation.IcyTouch * spellResist * 1.5f));
            }
            #endregion

            #region Killing Machine
            {
                float KMPpM = (1f * talents.KillingMachine) * (1f + stats.HasteRating / 3278f); // KM Procs per Minute (Defined "1 per point" by Blizzard) influenced by Phys. Haste
                float addHastePercent = 1f;

                if (calcOpts.Bloodlust)
                {
                    float numLust = fightDuration % 300f;  // bloodlust changed in 3.0, can only have one every 5 minutes.
                    float fullLustDur = (numLust - 1) * 300f + 40f;
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
                }

                if (calcOpts.Windfury)
                {
                    addHastePercent += 0.2f;
                }

                KMPpM *= addHastePercent;

                float KMPpR = KMPpM / (60 / calcOpts.rotation.curRotationDuration);
                float totalAbilities = calcOpts.rotation.FrostStrike + calcOpts.rotation.IcyTouch + calcOpts.rotation.HowlingBlast;
                KMRatio = KMPpR / totalAbilities;
            }
            #endregion

            //sadly in this model it is very difficult to track the individual benefit of windfury
            #region Windfury
            {
                if (calcOpts.Windfury)
                {
                    float WFMult = 1.2f;
                    MH.hastedSpeed /= WFMult;
                    OH.hastedSpeed /= WFMult;
                }
            }
            #endregion

            #region White Dmg
            {
                float MHDPS = 0f, OHDPS = 0f;
                #region Main Hand
                {
                    float dpsMHglancing = (0.24f * MH.DPS) * 0.75f;
                    float dpsMHBeforeArmor = ((MH.DPS * (1f - calcs.AvoidedAttacks - 0.24f)) * (1f + physCrits)) + dpsMHglancing;
                    dpsWhiteMinusGlancing = dpsMHBeforeArmor - dpsMHglancing;
                    dpsWhiteBeforeArmor = dpsMHBeforeArmor;
                    MHDPS = dpsMHBeforeArmor * mitigation;
                }
                #endregion

                #region Off Hand
                {
                    float dpsOHglancing = (0.24f * OH.DPS) * 0.75f;
                    float dpsOHBeforeArmor = ((OH.DPS * (1f - calcs.AvoidedAttacks - 0.24f)) * (1f + physCrits)) + dpsOHglancing;
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
                if (OH.damage != 0)
                {
                    float MHBCBDmg = MH.damage * (.25f + .125f * calcOpts.rotation.avgDiseaseMult);
                    float OHBCBDmg = OH.damage * (.25f + .125f * calcOpts.rotation.avgDiseaseMult);
                    float dpsMHBCB = MHBCBDmg / MH.hastedSpeed;
                    float dpsOHBCB = OHBCBDmg / OH.hastedSpeed;
                    dpsBCB = dpsMHBCB + dpsOHBCB;
                    dpsBCB *= .1f * (float)talents.BloodCakedBlade;
                }
                else if (MH.damage != 0)
                {
                    float BCBDmg = MH.damage * (.25f + .125f * calcOpts.rotation.avgDiseaseMult);
                    dpsBCB = BCBDmg / MH.hastedSpeed;
                    dpsBCB *= .1f * (float)talents.BloodCakedBlade;
                }
            }
            #endregion

            #region Windfury Contribution
            {
                if (calcOpts.Windfury)
                {
                    dpsWindfury = (dpsWhite + dpsNecrosis + dpsBCB) * (1f / 6f);
                    // you're at 120% now, so find what the original 20% was
                }
            }
            #endregion

            #region Death Coil
            {
                if (calcOpts.rotation.DeathCoil > 0f)
                {
                    float DCCD = realDuration / calcOpts.rotation.DeathCoil;
                    float DCDmg = 443f + (DeathCoilAPMult * stats.AttackPower) + 
                        (character.Ranged != null && character.Ranged.Id == 40867 ? 80 : 0);     // Sigil of the Wild Buck
                    dpsDeathCoil = DCDmg / DCCD;
                    dpsDeathCoil *= 1f + spellCrits;

                    //sudden doom stuff
                    // this doesn't appear to account for the 100% crit on SD proc DCs?
                    float affectedDCMult = calcOpts.rotation.BloodStrike + calcOpts.rotation.HeartStrike;
                    affectedDCMult *= .04f * (float)talents.SuddenDoom;
                    affectedDCMult /= calcOpts.rotation.DeathCoil;
                    dpsDeathCoil += dpsDeathCoil * affectedDCMult;

                    dpsDeathCoil *= 1f + (.05f * (float)talents.Morbidity);
                }
            }
            #endregion

            #region Icy Touch
            // this seems to handle crit strangely.
            // additionally, looks like it's missing some multipliers? maybe they're applied later
            // uh oh, this assumes that we ALWAYS have at least one IT per rotation!
            {
                if (calcOpts.rotation.IcyTouch > 0f)
                {
                    float addedCritFromKM = KMRatio;
                    float ITCD = realDuration / calcOpts.rotation.IcyTouch;
                    float ITDmg = 236f + (IcyTouchAPMult * stats.AttackPower) + 
                        (character.Ranged != null && character.Ranged.Id == 40822 ? 203 : 0);     // Sigil of Frozen Conscience
                    ITDmg *= 1f + .1f * (float)talents.ImprovedIcyTouch;
                    dpsIcyTouch = ITDmg / ITCD;
                    dpsIcyTouch *= 1f + spellCrits + addedCritFromKM + (.05f * (float)talents.Rime);
                }
            }
            #endregion

            #region Plague Strike
            {
                if (calcOpts.rotation.PlagueStrike > 0f)
                {
                    float PSCD = realDuration / calcOpts.rotation.PlagueStrike;
                    float PSDmg = (MH.baseDamage + ((stats.AttackPower / 14f) * (DW ? 2.4f : 3.3f))) * .3f + 113.4f;
                    dpsPlagueStrike = PSDmg / PSCD;
                    float PSCritDmgMult = 1f + (.15f * (float)talents.ViciousStrikes);
                    float PSCrit = 1f + ((physCrits + (.03f * (float)talents.ViciousStrikes)) * PSCritDmgMult);
                    dpsPlagueStrike *= PSCrit;

                    float GlyphofPSValue = 0f;
                    if (calcOpts.GlyphofPS) GlyphofPSValue = .2f * (calcOpts.rotation.diseaseUptime / 100);

                    dpsPlagueStrike *= 1f + GlyphofPSValue;

                    dpsPlagueStrike *= 1f + (.1f * (float)talents.Outbreak);
                }
            }
            #endregion

            #region Frost Fever
            {
                if (calcOpts.rotation.IcyTouch > 0f)
                {
                    // Frost Fever is renewed with every Icy Touch and starts a new cd
                    float ITCD = calcOpts.rotation.curRotationDuration / calcOpts.rotation.IcyTouch;
                    float FFCD = 3f / (calcOpts.rotation.diseaseUptime / 100);
                    int tempF = (int)Math.Floor(ITCD / FFCD);
                    FFCD = ((ITCD - ((float)tempF * FFCD)) / ((float)tempF + 1f)) + FFCD;
                    float FFDmg = FrostFeverAPMult * stats.AttackPower + 25.6f;
                    dpsFrostFever = FFDmg / FFCD;
                    //dpsFrostFever *= 1f + spellCrits;                            // Frost Fever can't crit

                    //dpsWPFromFF = FFDmg / (FFCD / physCrits);
                    dpsWPFromFF = dpsFrostFever * physCrits;
                }
            }
            #endregion

            #region Blood Plague
            {
                if (calcOpts.rotation.PlagueStrike > 0f)
                {
                    // Blood Plague is renewed with every Plague Strike and starts a new cd
                    float PSCD = calcOpts.rotation.curRotationDuration / calcOpts.rotation.PlagueStrike;
                    float BPCD = 3f / (calcOpts.rotation.diseaseUptime / 100);
                    int tempF = (int)Math.Floor(PSCD / BPCD);
                    BPCD = ((PSCD - ((float)tempF * BPCD)) / ((float)tempF + 1f)) + BPCD;
                    float BPDmg = BloodPlagueAPMult * stats.AttackPower + 31.1f;
                    dpsBloodPlague = BPDmg / BPCD;
                    //dpsBloodPlague *= 1f + spellCrits;                           // Blood Plague can't crit

                    //dpsWPFromBP = BPDmg / (BPCD / physCrits);
                    dpsWPFromBP = dpsBloodPlague * physCrits;
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
                    float SSCD = realDuration / calcOpts.rotation.ScourgeStrike;
                    float SSDmg = (MH.baseDamage + ((stats.AttackPower / 14f) * (DW ? 2.4f : 3.3f))) * .6f + 190.5f +
                        (character.Ranged != null && character.Ranged.Id == 40207 ? 420 : 0) +     // Sigil of Awareness
                        (character.Ranged != null && character.Ranged.Id == 40875 ? 203 : 0) +     // Sigil of Arthritic Binding
                        (92.25f * calcOpts.rotation.avgDiseaseMult);
                    dpsScourgeStrike = SSDmg / SSCD;
                    float SSCritDmgMult = 1f + (.15f * (float)talents.ViciousStrikes);
                    float SSCrit = 1f + ((physCrits + (.03f * (float)talents.ViciousStrikes) + (numt7 > 1 ? .05f : 0)) * SSCritDmgMult);
                    dpsScourgeStrike = dpsScourgeStrike * SSCrit;
                }
            }
            #endregion

            #region Unholy Blight
            {
                //The cooldown on this 1 second and I assume 100% uptime
                float UBDmg = UnholyBlightAPMult * stats.AttackPower + 37;
                dpsUnholyBlight = UBDmg * (1f + spellCrits);
                dpsUnholyBlight *= (float)talents.UnholyBlight;
            }
            #endregion

            #region Frost Strike
            {
                if (talents.FrostStrike > 0 && calcOpts.rotation.FrostStrike > 0f)
                {
                    float addedCritFromKM = KMRatio;
                    float FSCD = realDuration / calcOpts.rotation.FrostStrike;
                    float FSDmg = (MH.baseDamage + ((stats.AttackPower / 14f) * (DW ? 2.4f : 3.3f))) * .6f + 150f;
                    dpsFrostStrike = FSDmg / FSCD;
                    float FSCritDmgMult = (.15f * (float)talents.GuileOfGorefiend);
                    float FSCrit = 1f + physCrits + addedCritFromKM;
                    dpsFrostStrike += dpsFrostStrike * FSCrit * FSCritDmgMult;
                }
            }
            #endregion

            #region Howling Blast
            {
                if (talents.HowlingBlast > 0 && calcOpts.rotation.HowlingBlast > 0f)
                {
                    float addedCritFromKM = KMRatio;
                    float HBCritDmgMult = 1f + (.15f * (float)talents.GuileOfGorefiend);
                    float HBCD = realDuration / calcOpts.rotation.HowlingBlast;
                    float HBDmg = 270 + HowlingBlastAPMult * stats.AttackPower;
                    HBDmg *= 2 * (calcOpts.rotation.diseaseUptime / 100);
                    dpsHowlingBlast = HBDmg / HBCD;
                    float HBCrit = spellCrits + addedCritFromKM;
                    dpsHowlingBlast = (dpsHowlingBlast * HBCritDmgMult) * HBCrit;
                }
            }
            #endregion

            #region Obliterate
            {
                if (calcOpts.rotation.Obliterate > 0f)
                {
                    // this is missing +crit chance from rime
                    float OblitCD = realDuration / calcOpts.rotation.Obliterate;
                    float OblitDmg = (MH.baseDamage + ((stats.AttackPower / 14f) * (DW ? 2.4f : 3.3f))) + 292f + 
                        (character.Ranged != null && character.Ranged.Id == 40207 ? 420 : 0) + (146f * calcOpts.rotation.avgDiseaseMult);   // Sigil of Awareness
                    dpsObliterate = OblitDmg / OblitCD;
                    //float OblitCrit = 1f + physCrits + ( .03f * (float)talents.Subversion );
                    //OblitCrit += .05f * (float)talents.Rime;
                    float OblitCritDmgMult = 1f + (.15f * (float)talents.MightOfMograine);
                    OblitCritDmgMult += (.15f * (float)talents.GuileOfGorefiend);
                    float OblitCrit = 1f + ((physCrits +
                        (.03f * (float)talents.Subversion) +
                        (0.05f * (float)talents.Rime) +
                        (numt7 > 1 ? .05f : 0f)) * OblitCritDmgMult);
                    dpsObliterate = dpsObliterate * OblitCrit;
                    dpsObliterate *= (calcOpts.GlyphofOblit ? 1.2f : 1f);
                }
            }
            #endregion

            #region Blood Strike
            {
                if (calcOpts.rotation.BloodStrike > 0f)
                {
                    float BSCD = realDuration / calcOpts.rotation.BloodStrike;
                    float BSDiseaseDmg = 95.5f * (1f + 0.2f * (float)talents.BloodyStrikes);
                    float BSDmg = (MH.baseDamage + ((stats.AttackPower / 14f) * (DW ? 2.4f : 3.3f))) * 
                        (.5f * (1f + (.1f * (float)talents.BloodyStrikes))) + 
                        191f + (character.Ranged != null && character.Ranged.Id == 39208 ? 90 : 0) + (BSDiseaseDmg * calcOpts.rotation.avgDiseaseMult);        // Sigil of the Dark Rider
                    dpsBloodStrike = BSDmg / BSCD;
                    float BSCritDmgMult = 1f + (.15f * (float)talents.MightOfMograine);
                    BSCritDmgMult += (.15f * (float)talents.GuileOfGorefiend);
                    float BSCrit = 1f + ((physCrits + (.03f * (float)talents.Subversion)) * BSCritDmgMult);
                    dpsBloodStrike = (dpsBloodStrike) * BSCrit;

                    dpsBloodStrike *= 1f + (.03f * (float)talents.BloodOfTheNorth);
                }
            }
            #endregion

            #region Heart Strike
            {
                if (talents.HeartStrike > 0 && calcOpts.rotation.HeartStrike > 0f)
                {
                    float HSCD = realDuration / calcOpts.rotation.HeartStrike;
                    float HSDiseaseDmg = 110.4f * (1f + 0.2f * (float)talents.BloodyStrikes);
                    float HSDmg = ((MH.baseDamage + ((stats.AttackPower / 14f) * (DW ? 2.4f : 3.3f))) * 0.6f + 220.8f + 
                        (character.Ranged != null && character.Ranged.Id == 39208 ? 90 : 0)) *         // Sigil of the Dark Rider
                        (1f + 0.1f * (float)talents.BloodyStrikes) +
                        (HSDiseaseDmg * calcOpts.rotation.avgDiseaseMult);
                    dpsHeartStrike = HSDmg / HSCD;
                    //float HSCrit = 1f + physCrits + ( .03f * (float)talents.Subversion );
                    float HSCritDmgMult = 1f + (.15f * (float)talents.MightOfMograine);
                    float HSCrit = 1f + ((physCrits + (.03f * (float)talents.Subversion)) * HSCritDmgMult);
                    dpsHeartStrike = (dpsHeartStrike) * HSCrit;
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
                    GargoyleDmg *= 1f + (.5f * spellCrits);  // Gargoyle does 150% crits apparently
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
                    if (calcOpts.GlyphofGhoul) GlyphofGhoulValue = 0.4f;

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

                    float GhoulhastedSpeed = (MH.hastedSpeed / MH.baseSpeed) * GhoulBaseSpeed;
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
                    dpsGhoul *= 1f - (calcOpts.BossArmor - stats.ArmorPenetration) / ((calcOpts.BossArmor - stats.ArmorPenetration) + 400f + 85f * (5.5f * (float)calcOpts.TargetLevel - 265.5f));
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
            float HeartStrikeMult = 1f;
            float HowlingBlastMult = 1f;
            float IcyTouchMult = 1f;
            float NecrosisMult = 1f;
            float ObliterateMult = 1f;
            float PlagueStrikeMult = 1f;
            float ScourgeStrikeMult = 1f;
            float UnholyBlightMult = 1f;
            float WhiteMult = 1f;
            float WindfuryMult = 1f;
            float WanderingPlagueMult = 1f;

            #region Apply Physical Mitigation
            {
                float physMit = mitigation;
              //  physMit *= physPowerMult;
                physMit *= 1f + (!DW ? .02f * talents.TwoHandedWeaponSpecialization : 0f);

                dpsBCB *= physMit;
                dpsBloodStrike *= physMit;
                dpsHeartStrike *= physMit;
                dpsObliterate *= physMit;
                dpsPlagueStrike *= physMit;               

                WhiteMult += physPowerMult - 1f;
                BCBMult += physPowerMult - 1f;
                BloodStrikeMult += physPowerMult - 1f;
                HeartStrikeMult += physPowerMult - 1f;
                ObliterateMult += physPowerMult - 1f;
                PlagueStrikeMult += physPowerMult - 1f;
            }
            #endregion

            #region Apply Elemental Strike Mitigation
            {
                float strikeMit = /*missedSpecial **/ partialResist;
                strikeMit *= (!DW ? 1f + .02f * talents.TwoHandedWeaponSpecialization : 0f);

                dpsScourgeStrike *= strikeMit;
                // dpsScourgeStrike *= 1f - dodgedSpecial;
                dpsFrostStrike *= strikeMit * (1f - missedSpecial);

                ScourgeStrikeMult += spellPowerMult - 1f;
                FrostStrikeMult += frostSpellPowerMult - 1f;
            }
            #endregion

            #region Apply Magical Mitigation
            {
                // some of this applies to necrosis, I wonder if it is ever accounted for
                float magicMit = partialResist /** spellResist*/;
                // magicMit = 1f - magicMit;

                dpsNecrosis *= magicMit;
                dpsBloodPlague *= magicMit;
                dpsDeathCoil *= magicMit * (1 - spellResist);
                dpsFrostFever *= magicMit;
                dpsHowlingBlast *= magicMit * (1 - spellResist);
                dpsIcyTouch *= magicMit;
                dpsUnholyBlight *= magicMit * (1 - spellResist);

                NecrosisMult += spellPowerMult - 1f;
                BloodPlagueMult += spellPowerMult - 1f;
                DeathCoilMult += spellPowerMult - 1f;
                FrostFeverMult += frostSpellPowerMult - 1f;
                HowlingBlastMult += frostSpellPowerMult - 1f;
                IcyTouchMult += frostSpellPowerMult - 1f;
                UnholyBlightMult += spellPowerMult - 1f;
            }
            #endregion

            #region Apply Multi-Ability Talent Multipliers
            {
                float BloodyVengeanceMult = .03f * (float)talents.BloodyVengeance;
                BCBMult += BloodyVengeanceMult;
                BloodStrikeMult += BloodyVengeanceMult;
                HeartStrikeMult += BloodyVengeanceMult;
                ObliterateMult += BloodyVengeanceMult;
                PlagueStrikeMult += BloodyVengeanceMult;
                WhiteMult += BloodyVengeanceMult;

                float HysteriaCoeff = .3f / 6f; // current uptime is 16.666...%
                float HysteriaMult = HysteriaCoeff * (float)talents.Hysteria;
                BCBMult += HysteriaMult;
                BloodStrikeMult += HysteriaMult;
                HeartStrikeMult += HysteriaMult;
                ObliterateMult += HysteriaMult;
                PlagueStrikeMult += HysteriaMult;
                WhiteMult += HysteriaMult;

                float BlackIceMult = .06f * (float)talents.BlackIce;
                FrostFeverMult += BlackIceMult;
                HowlingBlastMult += BlackIceMult;
                IcyTouchMult += BlackIceMult;
                FrostStrikeMult += BlackIceMult;

                float MercilessCombatMult = .315f * 0.06f * (float)talents.MercilessCombat;   // The last 35% of a Boss don't take 35% of the fight-time...say .315 (10% faster)
                ObliterateMult += MercilessCombatMult;
                HowlingBlastMult += MercilessCombatMult;
                IcyTouchMult += MercilessCombatMult;
                FrostStrikeMult += MercilessCombatMult;

                float GlacierRot = .05f * (float)talents.GlacierRot;
                HowlingBlastMult += GlacierRot;
                IcyTouchMult += GlacierRot;
                FrostStrikeMult += GlacierRot;

                if (calcOpts.CryptFever)
                {
                    float CryptFeverMult = .1f * (float)talents.CryptFever;
                    FrostFeverMult += CryptFeverMult;
                    BloodPlagueMult += CryptFeverMult;
                    UnholyBlightMult += CryptFeverMult;
                }

                float DesecrationChance = (calcOpts.rotation.PlagueStrike * 12f) / calcOpts.rotation.curRotationDuration;
                if (DesecrationChance > 1f) DesecrationChance = 1f;
                float DesecrationMult = .05f * (DesecrationChance * (.2f * (float)talents.Desecration));
                BCBMult += DesecrationMult;
                BloodPlagueMult += DesecrationMult;
                BloodStrikeMult += DesecrationMult;
                DeathCoilMult += DesecrationMult;
                DancingRuneWeaponMult += DesecrationMult;
                FrostFeverMult += DesecrationMult;
                FrostStrikeMult += DesecrationMult;
                //GargoyleMult += DesecrationMult;
                HeartStrikeMult += DesecrationMult;
                HowlingBlastMult += DesecrationMult;
                IcyTouchMult += DesecrationMult;
                NecrosisMult += DesecrationMult;
                ObliterateMult += DesecrationMult;
                PlagueStrikeMult += DesecrationMult;
                ScourgeStrikeMult += DesecrationMult;
                UnholyBlightMult += DesecrationMult;
                WhiteMult += DesecrationMult;
                WindfuryMult += DesecrationMult;

                if ((float)talents.BoneShield >= 1f)
                {
                    float BoneMult = .02f;
                    BCBMult += BoneMult;
                    BloodPlagueMult += BoneMult;
                    BloodStrikeMult += BoneMult;
                    DeathCoilMult += BoneMult;
                    DancingRuneWeaponMult += BoneMult;
                    FrostFeverMult += BoneMult;
                    //GargoyleMult += BoneMult;
                    FrostStrikeMult += BoneMult;
                    HeartStrikeMult += BoneMult;
                    HowlingBlastMult += BoneMult;
                    IcyTouchMult += BoneMult;
                    NecrosisMult += BoneMult;
                    ObliterateMult += BoneMult;
                    PlagueStrikeMult += BoneMult;
                    ScourgeStrikeMult += BoneMult;
                    UnholyBlightMult += BoneMult;
                    WhiteMult += BoneMult;
                    WindfuryMult += BoneMult;
                }
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
            calcs.HeartStrikeDPS = dpsHeartStrike * HeartStrikeMult;
            calcs.HowlingBlastDPS = dpsHowlingBlast * HowlingBlastMult;
            calcs.IcyTouchDPS = dpsIcyTouch * IcyTouchMult;
            calcs.NecrosisDPS = dpsNecrosis * NecrosisMult;
            calcs.ObliterateDPS = dpsObliterate * ObliterateMult;
            calcs.PlagueStrikeDPS = dpsPlagueStrike * PlagueStrikeMult;
            calcs.ScourgeStrikeDPS = dpsScourgeStrike * ScourgeStrikeMult;
            calcs.UnholyBlightDPS = dpsUnholyBlight * UnholyBlightMult;
            calcs.WhiteDPS = dpsWhite * WhiteMult;
            calcs.WindfuryDPS = dpsWindfury * WindfuryMult;
            calcs.WanderingPlagueDPS = dpsWanderingPlague * WanderingPlagueMult;


            calcs.DPSPoints = calcs.BCBDPS + calcs.BloodPlagueDPS + calcs.BloodStrikeDPS + calcs.DeathCoilDPS + calcs.FrostFeverDPS + calcs.FrostStrikeDPS +
                              calcs.GargoyleDPS + calcs.GhoulDPS + calcs.WanderingPlagueDPS + calcs.HeartStrikeDPS + calcs.HowlingBlastDPS + calcs.IcyTouchDPS +
                              calcs.NecrosisDPS + calcs.ObliterateDPS + calcs.PlagueStrikeDPS + calcs.ScourgeStrikeDPS + calcs.UnholyBlightDPS + calcs.WhiteDPS;
            //windfury and DRW are handled elsewhere

            #region Dancing Rune Weapon
            {
                if (talents.DancingRuneWeapon > 0)
                {
                    float DRWUptime = 1f / 9f;
                    dpsDancingRuneWeapon = (calcs.DPSPoints - calcs.GhoulDPS) * DRWUptime;
                    calcs.DPSPoints += dpsDancingRuneWeapon;
                    calcs.DRWDPS = dpsDancingRuneWeapon;
                }
            }
            #endregion

            calcs.OverallPoints = calcs.DPSPoints;
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
            Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
            Stats statsTalents = new Stats()
            {
                BonusStrengthMultiplier = .01f * (float)(talents.AbominationsMight + talents.RavenousDead) + .02f * (float)(talents.ShadowOfDeath + talents.VeteranOfTheThirdWar),
                BonusArmorMultiplier = .03f * (float)(talents.Toughness),
                BonusStaminaMultiplier = .02f * (float)(talents.ShadowOfDeath + talents.VeteranOfTheThirdWar),
                Expertise = (float)(talents.TundraStalker + talents.BloodGorged + talents.RageOfRivendare) + 2f * (float)(talents.VeteranOfTheThirdWar),
                BonusPhysicalDamageMultiplier = .02f * (float)(talents.BloodGorged + talents.RageOfRivendare + talents.TundraStalker),
                BonusSpellPowerMultiplier = .02f * (float)(talents.BloodGorged + talents.RageOfRivendare + talents.TundraStalker)
            };
            Stats statsTotal = new Stats();
            Stats statsGearEnchantsBuffs = new Stats();

            // Mongoose  **ASSUMPTION: Mongoose has a 40% uptime
            if (statsEnchants.MongooseProc > 0)
                statsEnchants.Agility += 120f * 0.4f;

            //calculate drums uptime...if it lands on an even minute mark ignore it, as it will have a duration of 0
            //removed due to no drums in WotLK
            /*float drumsEffectiveFightDuration = (float)calcOpts.FightLength - 1f;
            float numDrums = drumsEffectiveFightDuration % 2;
            float drumsUptime = (numDrums * .5f) / (float)calcOpts.FightLength;

            // Drums of War - 60 AP/30 SD
            if (calcOpts.DrumsOfWar)
            {
                statsBuffs.AttackPower += drumsUptime * 60f;
                statsBuffs.SpellPower += drumsUptime * 30f;
            }

            // Drums of Battle - 80 Haste
            if (calcOpts.DrumsOfBattle)
            {
                statsBuffs.HasteRating += drumsUptime * 80f;
            }*/

            // Ferocious Inspiriation  **Temp fix - FI increases all damage, not just physical damage
            if (character.ActiveBuffsContains("Ferocious Inspiration"))
            {
                statsBuffs.BonusPhysicalDamageMultiplier = ((1f + statsBuffs.BonusPhysicalDamageMultiplier) * (1f + (.01f * calcOpts.FerociousInspiration)));
                statsBuffs.BonusSpellPowerMultiplier     = ((1f + statsBuffs.BonusSpellPowerMultiplier)     * (1f + (.01f * calcOpts.FerociousInspiration)));
            }

            statsGearEnchantsBuffs = statsBaseGear + statsEnchants + statsBuffs + statsRace + statsTalents;

            statsTotal.BonusAttackPowerMultiplier = statsGearEnchantsBuffs.BonusAttackPowerMultiplier;
            statsTotal.BonusAgilityMultiplier = statsGearEnchantsBuffs.BonusAgilityMultiplier;
            statsTotal.BonusStrengthMultiplier = statsGearEnchantsBuffs.BonusStrengthMultiplier;
            statsTotal.BonusStaminaMultiplier = statsGearEnchantsBuffs.BonusStaminaMultiplier;

            statsTotal.Agility = (float)Math.Floor(statsGearEnchantsBuffs.Agility * (1 + statsGearEnchantsBuffs.BonusAgilityMultiplier));
            statsTotal.Strength = (float)Math.Floor(statsGearEnchantsBuffs.Strength * (1 + statsGearEnchantsBuffs.BonusStrengthMultiplier));
            statsTotal.Stamina = (float)Math.Floor(statsGearEnchantsBuffs.Stamina * (1 + statsGearEnchantsBuffs.BonusStaminaMultiplier));
            statsTotal.Intellect = (float)Math.Floor(statsGearEnchantsBuffs.Intellect * (1 + statsGearEnchantsBuffs.BonusIntellectMultiplier));
            statsTotal.Spirit = (float)Math.Floor(statsGearEnchantsBuffs.Spirit * (1 + statsGearEnchantsBuffs.BonusSpiritMultiplier));

            statsTotal.Armor = (float) Math.Floor((statsGearEnchantsBuffs.Armor + 2f * statsTotal.Agility) * 1f);
            statsTotal.Health = (float)Math.Floor(statsGearEnchantsBuffs.Health + (statsTotal.Stamina * 10f));
            statsTotal.Mana = (float)Math.Floor(statsGearEnchantsBuffs.Mana + (statsTotal.Intellect * 15f));
            statsTotal.AttackPower = (float)Math.Floor(statsGearEnchantsBuffs.AttackPower + statsTotal.Strength * 2);

            if (talents.BladedArmor > 0)
            {
                statsTotal.AttackPower += (statsGearEnchantsBuffs.Armor / 180f) * (float)talents.BladedArmor;
            }

            statsTotal.AttackPower *= 1f + statsTotal.BonusAttackPowerMultiplier;

            //double check to make sure they dont have it selected in the buffs tab
            if (calcOpts.UnleashedRage)
            {
                statsTotal.AttackPower *= 1.1f;
            }
            else
            {
                statsTotal.AttackPower *= 1f + ( .1f * (float)talents.AbominationsMight );
            }

            statsTotal.CritRating = statsGearEnchantsBuffs.CritRating;
            statsTotal.CritRating += statsGearEnchantsBuffs.CritMeleeRating + statsGearEnchantsBuffs.LotPCritRating;
            statsTotal.HitRating = statsGearEnchantsBuffs.HitRating;
            statsTotal.ArmorPenetration = statsBuffs.ArmorPenetration; // Just statsBuffs, because ArPen doesnt exist anywhere else
            statsTotal.Expertise = statsGearEnchantsBuffs.Expertise;
            statsTotal.Expertise += (float)Math.Floor(statsGearEnchantsBuffs.ExpertiseRating / 8);
            statsTotal.HasteRating = statsGearEnchantsBuffs.HasteRating;
            // Haste trinket (Meteorite Whetstone)
            statsTotal.HasteRating += statsGearEnchantsBuffs.HasteRatingOnPhysicalAttack * 10 / 45;
            statsTotal.WeaponDamage = statsGearEnchantsBuffs.WeaponDamage;
            statsTotal.ArmorPenetrationRating = statsGearEnchantsBuffs.ArmorPenetrationRating;

            statsTotal.SpellCrit = statsGearEnchantsBuffs.SpellCrit;
            statsTotal.CritRating = statsGearEnchantsBuffs.CritRating;
            statsTotal.HitRating = statsGearEnchantsBuffs.HitRating;
            statsTotal.SpellPower = statsGearEnchantsBuffs.SpellPower;
            statsTotal.SpellPower += statsGearEnchantsBuffs.SpellDamageFromSpiritPercentage * statsGearEnchantsBuffs.Spirit;

            statsTotal.SpellHit = statsGearEnchantsBuffs.SpellHit;
            statsTotal.PhysicalHit = statsGearEnchantsBuffs.PhysicalHit;
            statsTotal.PhysicalCrit = statsGearEnchantsBuffs.PhysicalCrit;
            statsTotal.PhysicalHaste = statsGearEnchantsBuffs.PhysicalHaste;

            statsTotal.BonusCritMultiplier = statsGearEnchantsBuffs.BonusCritMultiplier;

            statsTotal.BonusPhysicalDamageMultiplier = statsGearEnchantsBuffs.BonusPhysicalDamageMultiplier;
            statsTotal.BonusSpellPowerMultiplier = statsGearEnchantsBuffs.BonusShadowDamageMultiplier;

       /*     if (calcOpts.MagicVuln)
            {
                statsTotal.BonusSpellPowerMultiplier += .13f;
            }*/

            if (calcOpts.presence == CalculationOptionsDPSDK.Presence.Blood)  // a final, multiplicative component
            {
                statsTotal.BonusPhysicalDamageMultiplier += .15f;
                statsTotal.BonusSpellPowerMultiplier += .15f;
            }
            else if (calcOpts.presence == CalculationOptionsDPSDK.Presence.Unholy)  // a final, multiplicative component
            {
                statsTotal.PhysicalHaste *= 1.15f;
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
                (item.Slot == Item.ItemSlot.Ranged && item.Type != Item.ItemType.Sigil) */)
                return false;
            return base.IsItemRelevant(item);
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            return new Stats()
            {
                Health = stats.Health,
                Strength = stats.Strength,
                Agility = stats.Agility,
                Stamina = stats.Stamina,
                Armor = stats.Armor,

                AttackPower = stats.AttackPower,
                HitRating = stats.HitRating,
                CritRating = stats.CritRating,
                ArmorPenetrationRating = stats.ArmorPenetrationRating,
                ExpertiseRating = stats.ExpertiseRating,
                HasteRating = stats.HasteRating,
                WeaponDamage = stats.WeaponDamage,
                PhysicalCrit = stats.PhysicalCrit,
                PhysicalHaste = stats.PhysicalHaste,
                PhysicalHit = stats.PhysicalHit,
                
                //HitRating = stats.HitRating,
                SpellHit = stats.SpellHit,
                SpellCrit = stats.SpellCrit,

                BonusStrengthMultiplier = stats.BonusStrengthMultiplier,
                BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
                BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,
                BonusCritMultiplier = stats.BonusCritMultiplier,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,

                LotPCritRating = stats.LotPCritRating,
                CritMeleeRating = stats.CritMeleeRating,
                WindfuryAPBonus = stats.WindfuryAPBonus,
                Bloodlust = stats.Bloodlust,

                BonusShadowDamageMultiplier = stats.BonusShadowDamageMultiplier,
                BonusFrostDamageMultiplier = stats.BonusFrostDamageMultiplier

                
                
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            return (stats.Health + stats.Strength + stats.Agility + stats.Stamina + stats.AttackPower +
                stats.HitRating + stats.CritRating + stats.ArmorPenetrationRating + stats.ArmorPenetration + stats.ExpertiseRating + stats.HasteRating + stats.WeaponDamage + 
                stats.BonusStrengthMultiplier + stats.BonusStaminaMultiplier + stats.BonusAgilityMultiplier + stats.BonusCritMultiplier +
                stats.BonusAttackPowerMultiplier + stats.BonusPhysicalDamageMultiplier + 
                stats.CritMeleeRating + stats.LotPCritRating + stats.WindfuryAPBonus + stats.Bloodlust + stats.BonusShadowDamageMultiplier
                + stats.BonusFrostDamageMultiplier + stats.BonusScourgeStrikeDamage + stats.PhysicalCrit + stats.PhysicalHaste
                + stats.PhysicalHit + stats.SpellCrit + stats.SpellHit + stats.SpellHaste) != 0;
        }


        /// <summary>
        /// Saves the talents for the character
        /// </summary>
        /// <param name="character">The character for whom the talents should be saved</param>
        public void GetTalents(Character character)
        {
            CalculationOptionsDPSDK calcOpts = character.CalculationOptions as CalculationOptionsDPSDK;
            calcOpts.talents = character.DeathKnightTalents;
        }
    }
}
