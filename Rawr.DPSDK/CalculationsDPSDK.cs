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
					    "Basic Stats:Armor Penetration",
                        "Basic Stats:Armor",
					    "Advanced Stats:Weapon Damage*Damage before misses and mitigation",
					    "Advanced Stats:Attack Speed",
					    "Advanced Stats:Crit Chance",
					    "Advanced Stats:Avoided Attacks",
					    "Advanced Stats:Enemy Mitigation",
                        "DPS Breakdown:White",
                        "DPS Breakdown:BCB*Blood Caked Blade",
                        "DPS Breakdown:Necrosis",
					    "DPS Breakdown:Windfury*Contribution from White, BCB, and Necrosis; NOT added to total",
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
            float missedSpecial = 0f;
            float hitBonus = .01f * (float)talents.NervesOfColdSteel;
            float dpsWhiteBeforeArmor = 0f;
            float combinedSwingTime = 2f;
            float fightDuration = calcOpts.FightLength*60;
            float mitigation, physCrits, spellCrits, spellResist, totalMHMiss, totalOHMiss;

            float MHExpertise = stats.Expertise;
            float OHExpertise = stats.Expertise;

            //damage multipliers
            float spellPowerMult = 1f + stats.BonusSpellPowerMultiplier;
            // Covers all % spell damage increases.  Misery, FI.
            float physPowerMult = 1f + stats.BonusPhysicalDamageMultiplier;
            // Covers all % physical damage increases.  Blood Frenzy, FI.
            float partialResist = 0.94f; // Average of 6% damage lost to partial resists on spells

            //spell AP multipliers, for diseases its per tick
            float HowlingBlastAPMult = 0.2f;
            float IcyTouchAPMult = 0.1f;
            float FrostFeverAPMult = 0.055f;
            float BloodPlagueAPMult = 0.055f;
            float DeathCoilAPMult = 0.15f;
            float UnholyBlightAPMult = 0.013f;
            float GargoyleAPMult = 0.4f;

            #region Impurity Application
            {
                float impurityMult = 1f + (.05f*(float) talents.Impurity);

                HowlingBlastAPMult *= impurityMult;
                IcyTouchAPMult *= impurityMult;
                FrostFeverAPMult *= impurityMult;
                BloodPlagueAPMult *= impurityMult;
                DeathCoilAPMult *= impurityMult;
                UnholyBlightAPMult *= impurityMult;
                GargoyleAPMult *= impurityMult;
            }
            #endregion

            if (character.Race == Character.CharacterRace.Dwarf || character.Race == Character.CharacterRace.Human)
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
            }
            if (character.Race == Character.CharacterRace.Human)
            {
                if (character.MainHand != null &&
                    (character.MainHand.Type == Item.ItemType.OneHandSword ||
                     character.MainHand.Type == Item.ItemType.TwoHandSword))
                {
                    MHExpertise += 5f;
                }

                if (character.OffHand != null && character.OffHand.Type == Item.ItemType.OneHandSword)
                {
                    OHExpertise += 5f;
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
                OH.damage *= 1f + OHMult;

                //need this for weapon swing procs
                combinedSwingTime = 1f / MH.hastedSpeed + 1f / OH.hastedSpeed;
                combinedSwingTime = 1f / combinedSwingTime;
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
                float ratingCoeff = stats.ArmorPenetrationRating / 15.4f;
                targetArmor *= (1 - ratingCoeff);
                if (targetArmor < 0) targetArmor = 0f;

                // Convert armor to mitigation
                //mitigation = 1f - (targetArmor/(targetArmor + 10557.5f));
                mitigation = 1f - targetArmor / ( targetArmor + 400f + 85f * ( 5.5f * (float)calcOpts.TargetLevel - 265.5f ) );

                calcs.EnemyMitigation = 1f - mitigation;
            }
            #endregion

            #region Crits, Resists
            {
                // Crit: Base .65%
                physCrits = .0065f;
                physCrits += stats.CritRating/4591f;
                physCrits += stats.Agility/6250f;
                physCrits += .01f * (float)(talents.DarkConviction + talents.EbonPlaguebringer + talents.Annihilation);
                calcs.CritChance = physCrits;

                calcs.DodgedMHAttacks = MH.chanceDodged;
                calcs.DodgedOHAttacks = OH.chanceDodged;
           
                float chanceMiss = 0f;
                if (character.OffHand == null)  chanceMiss = .09f;
                else                            chanceMiss = .28f;
                chanceMiss -= stats.HitRating / 3279f;
                chanceMiss -= hitBonus;
                if (chanceMiss < 0f) chanceMiss = 0f;
                calcs.MissedAttacks = chanceMiss;

                chanceMiss = .09f;
                chanceMiss -= stats.HitRating / 3279f;
                chanceMiss -= hitBonus;
                if (chanceMiss < 0f) chanceMiss = 0f;
                missedSpecial = chanceMiss;
                calcs.MissedAttacks = chanceMiss;

                spellCrits = 0f;
                spellCrits += stats.CritRating/4591;
                spellCrits += stats.SpellCrit;
                spellCrits += .01f * (float)( talents.DarkConviction + talents.EbonPlaguebringer );
                calcs.SpellCritChance = spellCrits;

                // Resists: Base 17%, Minimum 1%
                spellResist = .17f;
                spellResist -= stats.HitRating / 3279f;
                spellResist -= hitBonus;
                if (spellResist < 0f) spellResist = 0f;

                // Total physical misses
                totalMHMiss = calcs.DodgedMHAttacks + chanceMiss;
                totalOHMiss = calcs.DodgedOHAttacks + chanceMiss;
            }
            #endregion

            //killing machine specific stuff
            float totalAbilities = calcOpts.rotation.FrostStrike + calcOpts.rotation.IcyTouch + calcOpts.rotation.HowlingBlast;
            float FSKMRatio = calcOpts.rotation.FrostStrike / totalAbilities;
            float ITKMRatio = calcOpts.rotation.IcyTouch / totalAbilities;
            float HBKMRatio = calcOpts.rotation.HowlingBlast / totalAbilities;
            float numProcs = calcOpts.rotation.curRotationDuration / combinedSwingTime;
            numProcs *= physCrits;

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
                    float dpsMHBeforeArmor = MH.DPS * ( 1f - calcs.MissedAttacks );
                    dpsWhiteBeforeArmor += dpsMHBeforeArmor;
                    MHDPS = dpsMHBeforeArmor * mitigation;
                }
                #endregion

                #region Off Hand
                {
                    float dpsOHBeforeArmor = OH.DPS * ( 1f - calcs.MissedAttacks );
                    dpsWhiteBeforeArmor += dpsOHBeforeArmor;
                    OHDPS = dpsOHBeforeArmor * mitigation;
                }
                #endregion

                dpsWhite = MHDPS + OHDPS;
                dpsWhite *= 1f + physCrits;
            }
            #endregion

            #region Necrosis
            {
                dpsNecrosis = dpsWhite * (.02f * (float)talents.Necrosis);
            }
            #endregion

            #region Blood Caked Blade
            {
                if (MH.damage != 0)
                {
                    float BCBDmg = MH.damage*(.25f + .125f*calcOpts.rotation.avgDiseaseMult);
                    dpsBCB = BCBDmg/combinedSwingTime;
                    dpsBCB *= .3f;
                    dpsBCB *= 1f + physCrits;
                    dpsBCB *= .1f*(float) talents.BloodCakedBlade;
                }
            }
            #endregion

            #region Windfury Contribution
            {
                if (calcOpts.Windfury)
                {
                    dpsWindfury = (dpsWhite + dpsNecrosis + dpsBCB)*(1f/6f);
                        // you're at 120% now, so find what the original 20% was
                }
            }
            #endregion

            #region Death Coil
            {
                if (calcOpts.rotation.DeathCoil > 0f)
                {
                    float DCCD = calcOpts.rotation.curRotationDuration/calcOpts.rotation.DeathCoil;
                    float DCDmg = 443f + (DeathCoilAPMult*stats.AttackPower);
                    dpsDeathCoil = DCDmg/DCCD;
                    dpsDeathCoil *= 1f + spellCrits;

                    //sudden doom stuff
                    float affectedDCMult = calcOpts.rotation.BloodStrike + calcOpts.rotation.HeartStrike;
                    affectedDCMult *= .04f*(float) talents.SuddenDoom;
                    affectedDCMult /= calcOpts.rotation.DeathCoil;
                    dpsDeathCoil += dpsDeathCoil*affectedDCMult;

                    dpsDeathCoil *= 1f + (.05f*(float) talents.Morbidity);
                }
            }
            #endregion

            #region Icy Touch
            {
                float addedCritFromKM = numProcs * ITKMRatio / totalAbilities;
                float ITCD = calcOpts.rotation.curRotationDuration / calcOpts.rotation.IcyTouch;
                float ITDmg = 236f + (IcyTouchAPMult * stats.AttackPower);
                ITDmg *= 1f + .1f * (float) talents.ImprovedIcyTouch;
                dpsIcyTouch = ITDmg / ITCD;
                dpsIcyTouch *= 1f + spellCrits + addedCritFromKM + (.05f * (float)talents.Rime); 
            }
            #endregion

            #region Plague Strike
            {
                float PSCD = calcOpts.rotation.curRotationDuration / calcOpts.rotation.PlagueStrike;
                float PSDmg = MH.damage * .3f + 113.4f;
                dpsPlagueStrike = PSDmg / PSCD;
                float PSCritDmgMult = 1f + ( .15f * (float)talents.ViciousStrikes );
                float PSCrit = 1f + ( ( physCrits + ( .03f * (float)talents.ViciousStrikes ) ) * PSCritDmgMult );
                dpsPlagueStrike += dpsPlagueStrike * PSCrit;

                dpsPlagueStrike *= 1f + (.1f * (float)talents.Outbreak);
            }
            #endregion

            #region Frost Fever
            {
                float FFCD = 3f / calcOpts.rotation.diseaseUptime;
                float FFDmg = FrostFeverAPMult * stats.AttackPower + 25.6f;
                dpsFrostFever = FFDmg / FFCD;
                dpsFrostFever *= 1f + spellCrits;

                dpsWPFromFF = FFDmg / (FFCD / physCrits);
            }
            #endregion

            #region Blood Plague
            {
                float BPCD = 3f / calcOpts.rotation.diseaseUptime;
                float BPDmg = BloodPlagueAPMult * stats.AttackPower + 31.1f;
                dpsBloodPlague = BPDmg / BPCD;
                dpsBloodPlague *= 1f + spellCrits;

                dpsWPFromBP = BPDmg / ( BPCD / physCrits );
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
                    float SSCD = calcOpts.rotation.curRotationDuration/calcOpts.rotation.ScourgeStrike;
                    float SSDmg = MH.damage * .6f + 190.5f + (92.25f * calcOpts.rotation.avgDiseaseMult);
                    dpsScourgeStrike = SSDmg/SSCD;
                    float SSCritDmgMult = 1f + (.15f*(float) talents.ViciousStrikes);
                    float SSCrit = 1f + ((physCrits + (.03f*(float) talents.ViciousStrikes))*SSCritDmgMult);
                    dpsScourgeStrike += dpsScourgeStrike*SSCrit;
                }
            }
            #endregion

            #region Unholy Blight
            {
                //The cooldown on this 1 second and I assume 100% uptime
                float UBDmg = UnholyBlightAPMult * stats.AttackPower + 37;
                dpsUnholyBlight = UBDmg * (1f + spellCrits);
                dpsUnholyBlight *= (float) talents.UnholyBlight;
            }
            #endregion

            #region Frost Strike
            {
                if (talents.FrostStrike > 0 && calcOpts.rotation.FrostStrike > 0f)
                {
                    float addedCritFromKM = numProcs * FSKMRatio / totalAbilities;
                    float FSCD = calcOpts.rotation.curRotationDuration / calcOpts.rotation.FrostStrike;
                    float FSDmg = MH.damage * .6f + 150f;
                    dpsFrostStrike = FSDmg / FSCD;
                    float FSCritDmgMult = ( .15f * (float)talents.GuileOfGorefiend );
                    float FSCrit = 1f + physCrits + addedCritFromKM;
                    dpsFrostStrike += dpsFrostStrike * FSCrit;
                }
            }
            #endregion

            #region Howling Blast
            {
                if (talents.HowlingBlast > 0 && calcOpts.rotation.HowlingBlast > 0f)
                {
                    float addedCritFromKM = numProcs * HBKMRatio / totalAbilities;
                    float HBCritDmgMult = ( .15f * (float)talents.GuileOfGorefiend );
                    float HBCD = calcOpts.rotation.curRotationDuration / calcOpts.rotation.HowlingBlast;
                    float HBDmg = 270 + HowlingBlastAPMult * stats.AttackPower;
                    HBDmg *= 2 * calcOpts.rotation.diseaseUptime;
                    dpsHowlingBlast = HBDmg / HBCD;
                    float HBCrit = 1f + spellCrits + addedCritFromKM;
                    dpsHowlingBlast += (dpsHowlingBlast*HBCritDmgMult) * HBCrit;
                }
            }
            #endregion

            #region Obliterate
            {
                if (calcOpts.rotation.Obliterate > 0f)
                {
                    float OblitCD = calcOpts.rotation.curRotationDuration / calcOpts.rotation.Obliterate;
                    float OblitDmg = MH.damage + 292f + (146f * calcOpts.rotation.avgDiseaseMult);
                    dpsObliterate = OblitDmg / OblitCD;
                    //float OblitCrit = 1f + physCrits + ( .03f * (float)talents.Subversion );
                    //OblitCrit += .05f * (float)talents.Rime;
                    float OblitCritDmgMult = 1f + ( .15f * (float)talents.MightOfMograine );
                    OblitCritDmgMult += ( .15f * (float)talents.GuileOfGorefiend );
                    float OblitCrit = 1f + ((physCrits + (.03f*(float) talents.Subversion)) * OblitCritDmgMult);
                    dpsObliterate += dpsObliterate * OblitCrit;
                }
            }
            #endregion

            #region Blood Strike
            {
                if (calcOpts.rotation.BloodStrike > 0f)
                {
                    float BSCD = calcOpts.rotation.curRotationDuration / calcOpts.rotation.BloodStrike;
                    float BSDiseaseDmg = 95.5f * (1f + 0.2f * (float)talents.BloodyStrikes);
                    float BSDmg = MH.damage * .5f + 191f + ( BSDiseaseDmg * calcOpts.rotation.avgDiseaseMult );
                    dpsBloodStrike = BSDmg / BSCD;
                    //float BSCrit = 1f + physCrits + ( .03f * (float)talents.Subversion );
                    float BSCritDmgMult = 1f + ( .15f * (float)talents.MightOfMograine );
                    BSCritDmgMult += ( .15f * (float)talents.GuileOfGorefiend );
                    float BSCrit = 1f + ((physCrits + (.03f*(float) talents.BloodyStrikes))*BSCritDmgMult);
                    dpsBloodStrike += (dpsBloodStrike * BSCritDmgMult) * BSCrit;

                    dpsBloodStrike *= 1f + (.03f * (float)talents.BloodOfTheNorth);
                }
            }
            #endregion

            #region Heart Strike
            {
                if (talents.HeartStrike > 0 && calcOpts.rotation.HeartStrike > 0f)
                {
                    float HSCD = calcOpts.rotation.curRotationDuration / calcOpts.rotation.HeartStrike;
                    float HSDiseaseDmg = 110.4f * ( 1f + 0.2f * (float)talents.BloodyStrikes );
                    float HSDmg = MH.damage + 220.8f + ( HSDiseaseDmg * calcOpts.rotation.avgDiseaseMult );
                    dpsHeartStrike = HSDmg / HSCD;
                    //float HSCrit = 1f + physCrits + ( .03f * (float)talents.Subversion );
                    float HSCritDmgMult = 1f + ( .15f * (float)talents.MightOfMograine );
                    float HSCrit = 1f + ((physCrits + (.03f*(float) talents.Subversion))*HSCritDmgMult);
                    dpsHeartStrike += (dpsHeartStrike * HSCritDmgMult) * HSCrit;
                }
            }
            #endregion

            #region Gargoyle
            {
                if (calcOpts.rotation.GargoyleDuration > 0f)
                {
                    float GargoyleCastTime = 2.5f;

                    if (stats.Bloodlust > 0)
                    {
                        GargoyleCastTime *= .7f;
                    }

                    float GargoyleStrike = GargoyleAPMult * stats.AttackPower;
                    float GargoyleStrikeCount = calcOpts.rotation.GargoyleDuration / GargoyleCastTime;
                    float GargoyleDmg = GargoyleStrike*GargoyleStrikeCount;
                    GargoyleDmg *= 1f + spellCrits;
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
                        float timeleft = calcOpts.FightLength;
                        float numCDs = timeleft / (5*60);
                        float duration = numCDs * 120f;
                        timeleft -= 300f;
                        if (timeleft > 120f)
                        {
                            uptime = 240f/calcOpts.FightLength;
                        }
                        else
                        {
                            uptime = (timeleft + 120f)/calcOpts.FightLength;
                        }
                    }
                }
            }
            #endregion

            #region Apply Physical Mitigation
            {
                float physMit = mitigation * missedSpecial * ( physPowerMult );
                physMit = 1f - physMit;

                dpsBCB *= physMit;
                dpsBloodStrike *= physMit;
                dpsHeartStrike *= physMit;
                dpsObliterate *= physMit;
                dpsPlagueStrike *= physMit;
            }
            #endregion

            #region Apply Elemental Strike Mitigation
            {
                float strikeMit = missedSpecial * partialResist;
                strikeMit = 1f - strikeMit;
                strikeMit *= spellPowerMult;

                dpsScourgeStrike *= strikeMit;
                dpsFrostStrike *= strikeMit;
            }
            #endregion

            #region Apply Magical Mitigation
            {
                float magicMit = partialResist * spellResist;
                magicMit = 1f - magicMit;
                magicMit *= spellPowerMult;

                dpsBloodPlague *= magicMit;
                dpsDeathCoil *= magicMit;
                dpsFrostFever *= magicMit;
                dpsHowlingBlast *= magicMit;
                dpsIcyTouch *= magicMit;
                dpsUnholyBlight *= magicMit;
            }
            #endregion

            #region Apply Multi-Ability Talent Multipliers
            {
                float BloodyVengeanceMult = 1f + (.03f * (float)talents.BloodyVengeance);
                dpsBCB *= BloodyVengeanceMult;
                dpsBloodStrike *= BloodyVengeanceMult;
                dpsHeartStrike *= BloodyVengeanceMult;
                dpsObliterate *= BloodyVengeanceMult;
                dpsPlagueStrike *= BloodyVengeanceMult;
                dpsWhite *= BloodyVengeanceMult;

                float HysteriaCoeff = .3f / 6f; // current uptime is 16.666...%
                float HysteriaMult = 1f + (HysteriaCoeff * (float)talents.Hysteria); 
                dpsBCB *= HysteriaMult;
                dpsBloodStrike *= HysteriaMult;
                dpsHeartStrike *= HysteriaMult;
                dpsObliterate *= HysteriaMult;
                dpsPlagueStrike *= HysteriaMult;
                dpsWhite *= HysteriaMult;

                float BlackIceMult = 1f + (.06f * (float)talents.BlackIce);
                dpsFrostFever *= BlackIceMult;
                dpsHowlingBlast *= BlackIceMult;
                dpsIcyTouch *= BlackIceMult;
                dpsFrostStrike *= BlackIceMult;

                float MercilessCombatMult = 1f + (.35f * 0.6f * (float)talents.MercilessCombat);
                dpsObliterate *= MercilessCombatMult;
                dpsHowlingBlast *= MercilessCombatMult;
                dpsIcyTouch *= MercilessCombatMult;
                dpsFrostStrike *= MercilessCombatMult;

                if ( calcOpts.CryptFever )
                {
                    float CryptFeverMult = 1.3f;
                    dpsFrostFever *= CryptFeverMult;
                    dpsBloodPlague *= CryptFeverMult;
                    dpsUnholyBlight *= CryptFeverMult;
                }

                float DesicrationMult = 1f + (.05f * .15f * (float)talents.Desecration);  //uptime at 5/5 is about 75%
                dpsBCB *= DesicrationMult;
                dpsBloodPlague *= DesicrationMult;
                dpsBloodStrike *= DesicrationMult;
                dpsDeathCoil *= DesicrationMult;
                dpsDancingRuneWeapon *= DesicrationMult;
                dpsFrostFever *= DesicrationMult;
                dpsFrostStrike *= DesicrationMult;
                dpsGargoyle *= DesicrationMult;
                dpsHeartStrike *= DesicrationMult;
                dpsHowlingBlast *= DesicrationMult;
                dpsIcyTouch *= DesicrationMult;
                dpsNecrosis *= DesicrationMult;
                dpsObliterate *= DesicrationMult;
                dpsPlagueStrike *= DesicrationMult;
                dpsScourgeStrike *= DesicrationMult;
                dpsUnholyBlight *= DesicrationMult;
                dpsWhite *= DesicrationMult;
                dpsWindfury *= DesicrationMult;
            }
            #endregion

            //feed variables for output
            calcs.BCBDPS = dpsBCB;
            calcs.BloodPlagueDPS = dpsBloodPlague;
            calcs.BloodStrikeDPS = dpsBloodStrike;
            calcs.DeathCoilDPS = dpsDeathCoil;
            calcs.DRWDPS = dpsDancingRuneWeapon;
            calcs.FrostFeverDPS = dpsFrostFever;
            calcs.FrostStrikeDPS = dpsFrostStrike;
            calcs.GargoyleDPS = dpsGargoyle;
            calcs.HeartStrikeDPS = dpsHeartStrike;
            calcs.HowlingBlastDPS = dpsHowlingBlast;
            calcs.IcyTouchDPS = dpsIcyTouch;
            calcs.NecrosisDPS = dpsNecrosis;
            calcs.ObliterateDPS = dpsObliterate;
            calcs.PlagueStrikeDPS = dpsPlagueStrike;
            calcs.ScourgeStrikeDPS = dpsScourgeStrike;
            calcs.UnholyBlightDPS = dpsUnholyBlight;
            calcs.WhiteDPS = dpsWhite;
            calcs.WindfuryDPS = dpsWindfury;
            calcs.WanderingPlagueDPS = dpsWanderingPlague;
            

            calcs.DPSPoints = dpsBCB + dpsBloodPlague + dpsBloodStrike + dpsDeathCoil + dpsFrostFever + dpsFrostStrike + dpsGargoyle + dpsWanderingPlague +
                              dpsHeartStrike + dpsHowlingBlast + dpsIcyTouch + dpsNecrosis + dpsObliterate + dpsPlagueStrike + dpsScourgeStrike + dpsUnholyBlight +
                              dpsWhite; //windfury and DRW are handled elsewhere

            #region Dancing Rune Weapon
            {
                if (talents.DancingRuneWeapon > 0)
                {
                    float DRWUptime = 1f/9f;
                    dpsDancingRuneWeapon = calcs.DPSPoints*DRWUptime;
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
                BonusStrengthMultiplier = .01f * (float)(talents.AbominationsMight + talents.RavenousDead) + .02f * (float)(talents.ShadowOfDeath),
                BonusArmorMultiplier = .03f * (float)(talents.Toughness),
                BonusStaminaMultiplier = .02f * (float)(talents.ShadowOfDeath),
                Expertise = (float)(talents.TundraStalker + talents.BloodGorged + talents.RageOfRivendare),
                BonusPhysicalDamageMultiplier = .02f * (float)(talents.BloodGorged + talents.RageOfRivendare + talents.TundraStalker),
                BonusSpellPowerMultiplier = .02f * (float)(talents.BloodGorged + talents.RageOfRivendare + talents.TundraStalker)
            };
            Stats statsTotal = new Stats();
            Stats statsGearEnchantsBuffs = new Stats();

            // Mongoose  **ASSUMPTION: Mongoose has a 40% uptime
            if (statsEnchants.MongooseProc > 0)
                statsEnchants.Agility += 120f * 0.4f;

            //calculate drums uptime...if it lands on an even minute mark ignore it, as it will have a duration of 0
            float drumsEffectiveFightDuration = (float)calcOpts.FightLength - 1f;
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
            }

            // Ferocious Inspiriation  **Temp fix - FI increases all damage, not just physical damage
            if (character.ActiveBuffsContains("Ferocious Inspiration"))
            {
                statsBuffs.BonusPhysicalDamageMultiplier = ((1f + statsBuffs.BonusPhysicalDamageMultiplier) *
                    (float)Math.Pow(1.03f, calcOpts.FerociousInspiration - 1f)) - 1f;
                statsBuffs.BonusSpellPowerMultiplier = ((1f + statsBuffs.BonusSpellPowerMultiplier) *
                    (float)Math.Pow(1.03f, calcOpts.FerociousInspiration)) - 1f;
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
            statsTotal.ArmorPenetration = statsGearEnchantsBuffs.ArmorPenetration;
            statsTotal.Expertise = statsGearEnchantsBuffs.Expertise;
            statsTotal.Expertise += (float)Math.Floor(statsGearEnchantsBuffs.ExpertiseRating / 8);
            statsTotal.HasteRating = statsGearEnchantsBuffs.HasteRating;
            statsTotal.WeaponDamage = statsGearEnchantsBuffs.WeaponDamage;

            statsTotal.SpellCrit = statsGearEnchantsBuffs.SpellCrit;
            statsTotal.CritRating = statsGearEnchantsBuffs.CritRating;
            statsTotal.HitRating = statsGearEnchantsBuffs.HitRating;
            statsTotal.SpellPower = statsGearEnchantsBuffs.SpellPower;
            statsTotal.SpellPower += statsGearEnchantsBuffs.SpellDamageFromSpiritPercentage * statsGearEnchantsBuffs.Spirit;

            statsTotal.BonusCritMultiplier = statsGearEnchantsBuffs.BonusCritMultiplier;

            statsTotal.BonusPhysicalDamageMultiplier = statsGearEnchantsBuffs.BonusPhysicalDamageMultiplier;
            statsTotal.BonusSpellPowerMultiplier = statsGearEnchantsBuffs.BonusSpellPowerMultiplier;

            if (calcOpts.MagicVuln)
            {
                statsTotal.BonusSpellPowerMultiplier += .13f;
            }

            if (calcOpts.presence == CalculationOptionsDPSDK.Presence.Blood)  // a final, multiplicative component
            {
                statsTotal.BonusPhysicalDamageMultiplier *= 1.15f;
                statsTotal.BonusSpellPowerMultiplier *= 1.15f;
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
                        new Item() { Stats = new Stats() { ArmorPenetration = 66.67f } },
                        new Item() { Stats = new Stats() { SpellPower = 11.7f } },
                    };
                    string[] statList = new string[] {
                        "Strength",
                        "Agility",
                        "Attack Power",
                        "Crit Rating",
                        "Hit Rating",
                        "Expertise Rating",
                        "Haste Rating",
                        "Armor Penetration",
                        "Spell Damage",
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
            if (item.Slot == Item.ItemSlot.OffHand ||
                (item.Slot == Item.ItemSlot.Ranged && item.Type != Item.ItemType.Libram))
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
                Intellect = stats.Intellect,
                Spirit = stats.Spirit,
                Armor = stats.Armor,

                AttackPower = stats.AttackPower,
                HitRating = stats.HitRating,
                CritRating = stats.CritRating,
                ArmorPenetration = stats.ArmorPenetration,
                ExpertiseRating = stats.ExpertiseRating,
                HasteRating = stats.HasteRating,
                WeaponDamage = stats.WeaponDamage,
                PhysicalCrit = stats.PhysicalCrit,
                PhysicalHaste = stats.PhysicalHaste,
                PhysicalHit = stats.PhysicalHit,
                
                //HitRating = stats.HitRating,
                SpellPower = stats.SpellPower,
                SpellDamageFromSpiritPercentage = stats.SpellDamageFromSpiritPercentage,
                SpellHit = stats.SpellHit,

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

        public override bool HasRelevantStats(Stats stats)
        {
            return (stats.Health + stats.Strength + stats.Agility + stats.Stamina + stats.Spirit + stats.AttackPower +
                stats.HitRating + stats.CritRating + stats.ArmorPenetration + stats.ExpertiseRating + stats.HasteRating + stats.WeaponDamage + 
                stats.CritRating + stats.HitRating + stats.SpellPower + stats.SpellDamageFromSpiritPercentage +
                stats.BonusStrengthMultiplier + stats.BonusStaminaMultiplier + stats.BonusAgilityMultiplier + stats.BonusCritMultiplier +
                stats.BonusAttackPowerMultiplier + stats.BonusPhysicalDamageMultiplier + stats.BonusSpellPowerMultiplier +
                stats.CritMeleeRating + stats.LotPCritRating + stats.WindfuryAPBonus + stats.Bloodlust) != 0;
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
