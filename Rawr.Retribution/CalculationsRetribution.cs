using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Retribution
{
	[System.ComponentModel.DisplayName("Retribution|Spell_Holy_AuraOfLight")]
    class CalculationsRetribution : CalculationsBase
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
					"Basic Stats:Attack Power",
					"Basic Stats:Agility",
					"Basic Stats:Strength",
					"Basic Stats:Crit",
					"Basic Stats:Hit",
					"Basic Stats:Expertise Rating",
					"Basic Stats:Haste Rating",
					"Basic Stats:Armor Penetration",
					"Basic Stats:Weapon Damage",
                    "Basic Stats:Spell Damage",
					"DPS Breakdown:Crusader Strike",
                    "DPS Breakdown:Seal",
                    "DPS Breakdown:White",
                    "DPS Breakdown:Judgement",
                    "DPS Breakdown:Consecration",
                    "DPS Breakdown:Exorcism",
                    "DPS Breakdown:Windfury",
                    "DPS Breakdown:Total DPS"

                        
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
            get { return _calculationOptionsPanel ?? (_calculationOptionsPanel = new CalculationOptionsPanelRetribution()); }
        }

        private List<Item.ItemType> _relevantItemTypes = null;
        public override List<Item.ItemType> RelevantItemTypes
        {
            get
            {
                return _relevantItemTypes ?? (_relevantItemTypes = new List<Item.ItemType>(new Item.ItemType[]
					{
						Item.ItemType.None,
						Item.ItemType.Cloth,
                        Item.ItemType.Leather,
                        Item.ItemType.Mail,
                        Item.ItemType.Plate,
                        Item.ItemType.Libram,
                        Item.ItemType.Polearm,
                        Item.ItemType.TwoHandAxe,
                        Item.ItemType.TwoHandMace,
                        Item.ItemType.TwoHandSword
					}));
            }
        }

		public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.Paladin; } }
		public override ComparisonCalculationBase CreateNewComparisonCalculation()
        {
            return  new ComparisonCalculationRetribution();
        }

        public override CharacterCalculationsBase CreateNewCharacterCalculations()
        {
            return new CharacterCalculationsRetribution();
        }


		public override ICalculationOptionBase DeserializeDataObject(string xml)
		{
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsRetribution));
			System.IO.StringReader reader = new System.IO.StringReader(xml);
			CalculationOptionsRetribution calcOpts = serializer.Deserialize(reader) as CalculationOptionsRetribution;
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
			CalculationOptionsRetribution calcOpts = character.CalculationOptions as CalculationOptionsRetribution;
            int sealChoice = calcOpts.Seal;
            character = GetTalents(character);
            Stats stats = GetCharacterStats( character, additionalItem );
            
            CharacterCalculationsRetribution calcs = new CharacterCalculationsRetribution();
            calcs.BasicStats = stats;


            float avgBaseWeaponHit = 0.0f, hasteBonus = 0.0f, hastedSpeed = 1.0f, physicalCritModifier = 0.0f, chanceToBeDodged = 6.5f, chanceToMiss = 9.0f; ;
            float chanceToGlance = 0.25f, glancingAmount = 0.35f;
            float consDPS = 0.0f, exoDPS=0.0f, wfDPS = 0.0f, jobDPS = 0.0f,socDPS=0.0f,jocDPS = 0.0f;
            #region Mitigation
            //Default Boss Armor
            float bossArmor = calcOpts.BossArmor;
            float totalArP = stats.ArmorPenetration;
            float modifiedTargetArmor = bossArmor - totalArP;
            float mitigation = 1 - modifiedTargetArmor / (modifiedTargetArmor + 10557.5f);
            #endregion
            string shattrathFaction = calcOpts.ShattrathFaction;
            if (stats.ShatteredSunMightProc > 0)
            {
                switch (shattrathFaction)
                {
                    case "Aldor":
                        stats.AttackPower += 39.13f;
                        break;
                }
            }


            #region White Damage and Multipliers
            //2 Handed Spec
            float twoHandedSpec = 1.0f + (0.02f * (float)calcOpts.TwoHandedSpec);
            if (character.MainHand != null)
            {
                avgBaseWeaponHit = twoHandedSpec * (character.MainHand.MinDamage + character.MainHand.MaxDamage + stats.WeaponDamage * 2f) / 2.0f;
                hastedSpeed = (stats.HasteRating == 0) ? character.MainHand.Speed : character.MainHand.Speed / (1 + stats.HasteRating / 1576f);
            }       

            //Add Attack Power Bonus
            avgBaseWeaponHit += twoHandedSpec*(stats.AttackPower / 14.0f) * ((character.MainHand == null) ? 1.0f : character.MainHand.Speed);

            
            //Take Non-Stat Buffs into account

            physicalCritModifier = 1.0f + ((stats.CritRating / 22.08f) / 100.0f) * (1f + stats.BonusCritMultiplier * 2f);

            chanceToBeDodged -= stats.ExpertiseRating / 15.76f;
            if (chanceToBeDodged < 0.0f) chanceToBeDodged = 0.0f;

            chanceToMiss -= stats.HitRating / 15.76f;
            if (chanceToMiss < 0.0f) chanceToMiss = 0.0f;

            float avgBaseWeaponHitPost = (avgBaseWeaponHit * physicalCritModifier - avgBaseWeaponHit * (chanceToMiss + chanceToBeDodged) / 100.0f
                - avgBaseWeaponHit * chanceToGlance * glancingAmount);
            //Fight duration
            float fightDuration = calcOpts.FightLength * 60;
            //Improved Sanctity Aura
			float impSancAura = 1f + 0.01f * (float)calcOpts.ImprovedSanctityAura;
            //Crusade
			float crusade = 1f + 0.01f * (float)calcOpts.Crusade;
            //Avenging Wrath -- Calculating uptime
            int remainder = 0, noOfFullAW = 0, noOfFullBL;
            int div = Math.DivRem(Convert.ToInt32(fightDuration), 180,out remainder);
            if (remainder == 0) 
                noOfFullAW = div;
            else
            noOfFullAW = Convert.ToInt32(Math.Ceiling(Convert.ToDouble((fightDuration + 20) / 180)));
            float partialUptime = fightDuration - noOfFullAW * 180;
            if (partialUptime < 0) partialUptime = 0;
            float totalUptime = partialUptime + noOfFullAW * 20f;
            float avWrath = 1f + 0.30f * totalUptime / fightDuration;

            

            #region Bloodlust
            if (stats.Bloodlust > 0)
            {
                //Bloodlust -- Calculating uptime
                //div = Math.DivRem(Convert.ToInt32(fightDuration), 600, out remainder);
                //if (remainder == 0)
                //    noOfFullBL = div;
                //else
                //    noOfFullBL = Convert.ToInt32(Math.Ceiling(Convert.ToDouble((fightDuration + 40) / 600)));

                hastedSpeed = (character.MainHand == null) ? 1.0f : character.MainHand.Speed /
                    (1 + stats.HasteRating / 1576f + 0.003f * calcOpts.BloodlustUptime);
            }
            #endregion

            //#region Drums of Battle
            //if (stats.DrumsOfBattle > 0)
            //{
            //    hastedSpeed = (character.MainHand == null) ? 1.0f : hastedSpeed / (1 + (stats.DrumsOfBattle / 1576f) / 4f);
            //}
            //#endregion
            
            //Vengeance
			float vengeance = 1f + 0.03f * (float)calcOpts.Vengeance;
            //Sanctity Aura
			float sancAura = 1f + 0.1f * (float)calcOpts.SanctityAura;
            //Misery 
            float misery = 1f + stats.BonusSpellPowerMultiplier;
            //SpellCrit Mod
            float judgementCrit = 1.0f + (0.03f * (float)calcOpts.Fanaticism)
                + (stats.CritRating / 22.08f) / 100f;
            //Blood Frenzy : TODO Take from Debuff List
            float bloodFrenzy = 1.0f + stats.BonusPhysicalDamageMultiplier;
            float ssoNeckProcDPS = 0f;
           

            //TODO: Add Mitigation
            avgBaseWeaponHitPost *= impSancAura * crusade * avWrath * vengeance * bloodFrenzy * mitigation;
            float dpsWhite = avgBaseWeaponHitPost / hastedSpeed;
            calcs.WhiteDPSPoints = dpsWhite;
            #endregion

            if (stats.ShatteredSunMightProc > 0)
            {
                switch (shattrathFaction)
                {
                    case "Scryer":
                        ssoNeckProcDPS = 350f * avWrath * misery * impSancAura * physicalCritModifier / 50f;
                        break;
                }
            }

            #region Seal of Blood
            if (calcOpts.Seal == 1)
            {
                float ppmSoB = (60f / hastedSpeed * (1 - (chanceToBeDodged + chanceToMiss) / 100.0f)) * (1 + 0.2f * (1 - (chanceToMiss + chanceToBeDodged) / 100.0f));
                float avgSoBHitPre = 0.35f * avgBaseWeaponHit;
                float avgSoBHitPost = avgSoBHitPre * physicalCritModifier - avgSoBHitPre * (chanceToBeDodged + chanceToMiss) / 100.0f;

                //TODO: Add Partial Resists
                avgSoBHitPost *= impSancAura * crusade * avWrath * vengeance * sancAura * misery * 0.96f;
                float dpsSoB = avgSoBHitPost * ppmSoB / 60f;
                calcs.SealDPSPoints = dpsSoB;
            }
            #endregion

            #region Windfury
            float avgTimeBetnWF = (hastedSpeed / (1.0f - (chanceToBeDodged + chanceToMiss) / 100f)) * 5.0f;
            float wfAPIncrease = stats.WindfuryAPBonus;
            float wfHitPre = avgBaseWeaponHit + (wfAPIncrease / 14f) * ((character.MainHand == null) ? 0 : character.MainHand.Speed);
            float wfHitPost = (wfHitPre * physicalCritModifier) - (wfHitPre * (chanceToMiss + chanceToBeDodged) / 100f) -
                (wfHitPre * glancingAmount * chanceToGlance);
            if (wfAPIncrease > 0)
            {
                wfHitPost *= impSancAura * crusade * vengeance * bloodFrenzy * avWrath * mitigation;
            }
            else
            {
                wfHitPost = 0f;
            }
            wfDPS = wfHitPost / avgTimeBetnWF;
            calcs.WFDPSPoints = wfDPS;
            #endregion 
            
            #region Seal of Command
            if (calcOpts.Seal == 0)
            {
                float socProcChance = 7.0f / (60f / hastedSpeed);
                float whiteHits = (60f / hastedSpeed) * (1f - (chanceToBeDodged + chanceToMiss) / 100f);
                float socHitsOffWhite = whiteHits * socProcChance;
                float wfHits = whiteHits * 0.2f * (1f - (chanceToBeDodged + chanceToMiss) / 100f);
                float socHitsOffWF = wfHits * (1f - socProcChance) * socProcChance;
                float socTotal = socHitsOffWhite + socHitsOffWF;
                float avgSoCPre = (0.7f * avgBaseWeaponHit + 0.2f * stats.SpellDamageRating + 0.29f * 219f);
                float avgSoCPost = (avgSoCPre * physicalCritModifier - avgSoCPre * (chanceToBeDodged + chanceToMiss) / 100f);
                avgSoCPost *= impSancAura * vengeance * crusade * avWrath * sancAura * misery * 0.96f;
                socDPS = avgSoCPost * socTotal / 60f;
                calcs.SealDPSPoints = socDPS;

                if (wfDPS > 0)
                {
                    float wfOffSoCDPS = (socTotal / 60f) * wfHitPost * 0.20f;
                    calcs.WFDPSPoints += wfOffSoCDPS;
                }
            }
            #endregion

            #region Crusader Strike
            float csCooldown = 6.0f;
            float avgCSHitPre = twoHandedSpec*(((character.MainHand != null) ? (character.MainHand.MinDamage + character.MainHand.MaxDamage + stats.WeaponDamage*2f) / 2.0f : 0.0f)+
                3.30f * (stats.AttackPower / 14f)) * 1.10f;
            float avgCSHitPost = avgCSHitPre* physicalCritModifier - avgCSHitPre*(chanceToBeDodged+chanceToMiss)/100f;
            
            //TODO: Add Mitigation
            avgCSHitPost *= impSancAura * crusade * avWrath * vengeance * mitigation*(1f+stats.BonusCrusaderStrikeDamageMultiplier)*bloodFrenzy;
            float dpsCS = avgCSHitPost/ csCooldown;
            calcs.CSDPSPoints = dpsCS;
            #endregion

            #region Consecration
            if (calcOpts.ConsecRank != 0)
            {
                //Rank 1
                float cooldownCons = 8.0f, avgConsPre= 0.0f;
                int consRank = calcOpts.ConsecRank;
                if (consRank == 1)
                {
                    avgConsPre = 64f + 0.46f * stats.SpellDamageRating + 0.97f * 219f;
                }
                else if(consRank ==6)
                {
                    avgConsPre = 512f + 0.95f * stats.SpellDamageRating + 0.97f * 219f;
                }
                else if (consRank == 4)
                {
                    avgConsPre = 280 + 0.95f * stats.SpellDamageRating + 0.97f * 219f;
                }
                float avgConsPost = avgConsPre * (1.0f - 0.14f / 8.0f) * impSancAura * crusade * avWrath * vengeance * sancAura * misery * 0.96f;
                consDPS = avgConsPost / cooldownCons;
                calcs.ConsDPSPoints = consDPS;
            }
              
            #endregion

            #region Exorcism
            if (calcOpts.Exorcism)
            {               
                float cooldownExo = 15.0f, avgExoPre = 0.0f;

                avgExoPre = 665f + 0.43f * (stats.SpellDamageRating + 219f);

                float avgExoPost = avgExoPre * (1.0f - 0.14f) * impSancAura * crusade * avWrath * vengeance * sancAura * misery * 0.96f;
                exoDPS = avgExoPost / cooldownExo;
                calcs.ExoDPSPoints = exoDPS;
            }
            #endregion

            #region Judgement of Blood
            if (calcOpts.Seal == 1)
            {
                float cooldownJoB = 9.0f;
                float avgJoBPre = 310f + 0.43f * (stats.SpellDamageRating + 219f);
                float avgJoBPost = (avgJoBPre * judgementCrit) - (avgJoBPre * 0.14f);
                avgJoBPost *= impSancAura * vengeance * misery * avWrath * crusade * sancAura * 0.96f;
                jobDPS = avgJoBPost / cooldownJoB;
                calcs.JudgementDPSPoints = jobDPS;
            }
            #endregion                       

            #region Judgement of Command
            if (calcOpts.Seal == 0)
            {
                float cooldownJoC = 9.0f;
                float avgJoCPre = 240f + 0.43f * (stats.SpellDamageRating + 219f);
                float avgJoCPost = (avgJoCPre * judgementCrit) - (avgJoCPre * 0.14f);
                avgJoCPost *= impSancAura * vengeance * misery * avWrath * crusade * sancAura * 0.96f;
                jocDPS = avgJoCPost / cooldownJoC;
                calcs.JudgementDPSPoints = jocDPS;
            }
            #endregion

            calcs.DPSPoints = dpsCS + dpsWhite + calcs.SealDPSPoints + consDPS + exoDPS + wfDPS + calcs.JudgementDPSPoints + ssoNeckProcDPS;
            calcs.SubPoints = new float[] { calcs.DPSPoints };
            calcs.OverallPoints = calcs.DPSPoints;
            calcs.BasicStats.WeaponDamage = avgBaseWeaponHit * impSancAura;
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
           
            Stats statsRace;
            switch (character.Race)
            {
                case Character.CharacterRace.BloodElf:
                    statsRace = new Stats()
                    {
                        Health = 4377f,
                        Mana = 2335f,
                        Strength = 123f,
                        Agility = 79f,
                        Stamina = 118f,
                        Intellect = 87f,
                        Spirit = 88f,
                        AttackPower = 436,
                        CritRating = 3.81f * 22.08f
                    };
                    break;
                case Character.CharacterRace.Draenei:
                    statsRace = new Stats()
                    {
                        Health = 4387f,
                        Mana = 2335f,
                        Strength = 127f,
                        Agility = 74f,
                        Stamina = 119f,
                        Intellect = 84f,
                        Spirit = 147,
                        AttackPower = 444,
                        CritRating = 3.61f * 22.08f,
                        HitRating = 1f * 15.76f
                    };
                    break;
                case Character.CharacterRace.Human:
                    statsRace = new Stats()
                    {
                        Health = 4397f,
                        Mana = 2335f,
                        Strength = 126f,
                        Agility = 77f,
                        Stamina = 120f,
                        Intellect = 83f,
                        Spirit = 89,
                        BonusIntellectMultiplier = 0.03f,
                        BonusSpiritMultiplier = 0.1f,
                        AttackPower = 442,
                        CritRating = 3.73f * 22.08f                        
                    };
                    //Expertise for Humans
                    if (character.MainHand != null && (character.MainHand.Type == Item.ItemType.TwoHandMace
                        || character.MainHand.Type == Item.ItemType.TwoHandSword))
                    {
                        statsRace.ExpertiseRating = 5f * 3.9f;
                    }
                    break;
                case Character.CharacterRace.Dwarf:
                    statsRace = new Stats()
                    {
                        Health = 4397f,
                        Mana = 2335f,
                        Strength = 128f,
                        Agility = 73f,
                        Stamina = 120f,
                        Intellect = 83f,
                        Spirit = 89,
                        AttackPower = 446,
                        CritRating = 3.57f * 22.08f
                    };
                    break;
                default:
                    statsRace = new Stats();
                    break;
            }

            

            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            CalculationOptionsRetribution calcOpts = character.CalculationOptions as CalculationOptionsRetribution;

            //Add Expose Weakness since it's not listed as an AP buff
            if(statsBuffs.ExposeWeakness > 0)
            {
                statsBuffs.AttackPower += calcOpts.ExposeWeaknessAPValue;
            }

            //Libram of Divine Judgement
            if (character.Ranged != null && character.Ranged.Id == 33503 && calcOpts.Seal == 0)
            {
                statsBuffs.AttackPower += character.Ranged.Stats.JudgementOfCommandAttackPowerBonus;
            }

            //Mongoose
            if (character.MainHand != null && character.MainHandEnchant != null && character.MainHandEnchant.Id == 2673)
            {
                statsBuffs.Agility += 120f * ((40f * (1f / (60f / character.MainHand.Speed)) / 6f));
                statsBuffs.HasteRating += (15.76f * 2f) * ((40f * (1f / (60f / character.MainHand.Speed)) / 6f));
            }

            //Executioner
            if (character.MainHand != null && character.MainHandEnchant != null && character.MainHandEnchant.Id == 3225)
            {
                statsBuffs.ArmorPenetration += 840f * ((40f * (1f / (60f / character.MainHand.Speed)) / 6f));                
            }

            //base
            
            Stats statsGearEnchantsBuffs = statsBaseGear + statsEnchants + statsBuffs;
            TalentTree tree = character.Talents;
            float agiBase = (float)Math.Floor(statsRace.Agility * (1 + statsRace.BonusAgilityMultiplier));
            float agiBonus = (float)Math.Floor(statsGearEnchantsBuffs.Agility * (1 + statsRace.BonusAgilityMultiplier));
            float strBase = (float)Math.Floor(statsRace.Strength * (1 + statsRace.BonusStrengthMultiplier));
            float strBonus = (float)Math.Floor(statsGearEnchantsBuffs.Strength * (1 + statsRace.BonusStrengthMultiplier));
            float staBase = (float)Math.Floor(statsRace.Stamina * (1 + statsRace.BonusStaminaMultiplier));
            float staBonus = (float)Math.Floor(statsGearEnchantsBuffs.Stamina * (1 + statsRace.BonusStaminaMultiplier));
            
            //drums of war
            statsGearEnchantsBuffs.AttackPower += statsGearEnchantsBuffs.DrumsOfWar * calcOpts.DrumsOfWarUptime / 100f;

            //drums of battle
            statsGearEnchantsBuffs.HasteRating += statsGearEnchantsBuffs.DrumsOfBattle * calcOpts.DrumsOfBattleUptime / 100f;

            //ferocious inspiriation
            if (character.ActiveBuffsContains("Ferocious Inspiration"))
                statsGearEnchantsBuffs.BonusPhysicalDamageMultiplier = ((1f + statsGearEnchantsBuffs.BonusPhysicalDamageMultiplier) *
                    (float)Math.Pow(1.03f, calcOpts.NumberOfFerociousInspirations - 1f)) - 1f;

            Stats statsTotal = new Stats();
            statsTotal.BonusAttackPowerMultiplier = ((1 + statsRace.BonusAttackPowerMultiplier) * (1 + statsGearEnchantsBuffs.BonusAttackPowerMultiplier)) - 1;
            statsTotal.BonusAgilityMultiplier = ((1 + statsRace.BonusAgilityMultiplier) * (1 + statsGearEnchantsBuffs.BonusAgilityMultiplier)) - 1;
            statsTotal.BonusStrengthMultiplier = ((1 + statsRace.BonusStrengthMultiplier) * (1 + statsGearEnchantsBuffs.BonusStrengthMultiplier)) - 1;
            statsTotal.BonusStaminaMultiplier = ((1 + statsRace.BonusStaminaMultiplier) * (1 + statsGearEnchantsBuffs.BonusStaminaMultiplier)) - 1;


            statsTotal.Agility = (agiBase + (float)Math.Floor((agiBase * statsBuffs.BonusAgilityMultiplier) + agiBonus * (1 + statsBuffs.BonusAgilityMultiplier)));
          

            statsTotal.Strength = (strBase + (float)Math.Floor((strBase * statsBuffs.BonusStrengthMultiplier) + strBonus * (1 + statsBuffs.BonusStrengthMultiplier)));

			statsTotal.Strength *= 1f + 0.02f * (float)calcOpts.DivineStrength;
            statsTotal.Stamina = (staBase + (float)Math.Round((staBase * statsBuffs.BonusStaminaMultiplier) + staBonus * (1 + statsBuffs.BonusStaminaMultiplier)));          

            statsTotal.Health = (float)Math.Round(((statsRace.Health + statsGearEnchantsBuffs.Health + ((statsTotal.Stamina-staBase) * 10f))));

            statsTotal.AttackPower = (float)Math.Floor((statsRace.AttackPower + statsGearEnchantsBuffs.AttackPower + (statsTotal.Strength - strBase )* 2.0f) * (1f + statsTotal.BonusAttackPowerMultiplier));


            statsTotal.CritRating = statsRace.CritRating + statsGearEnchantsBuffs.CritRating;
            statsTotal.CritRating += (((statsTotal.Agility - agiBase)/ 25f) * 22.08f);
			statsTotal.CritRating += (22.08f * (float)calcOpts.Conviction);
			statsTotal.CritRating += (22.08f * (float)calcOpts.SanctifiedSeals);
            statsTotal.HitRating = statsRace.HitRating + statsGearEnchantsBuffs.HitRating;
			statsTotal.HitRating += (15.76f * (float)calcOpts.Precision);
			statsTotal.SpellHitRating += (15.76f * (float)calcOpts.Precision);
            statsTotal.SpellHitRating += 15.76f * statsGearEnchantsBuffs.SpellHitRating;
			statsTotal.SpellCritRating += (22.08f * (float)calcOpts.SanctifiedSeals);
            statsTotal.SpellHitRating += 22.08f * statsGearEnchantsBuffs.SpellCritRating;
            statsTotal.ExpertiseRating = statsRace.ExpertiseRating + statsGearEnchantsBuffs.ExpertiseRating;
            

            statsTotal.HasteRating = statsRace.HasteRating + statsGearEnchantsBuffs.HasteRating;

            statsTotal.DodgeRating = statsRace.DodgeRating + statsGearEnchantsBuffs.DodgeRating;
            statsTotal.DodgeRating = ((statsTotal.Agility / 20f) * 18.92f);
            

            statsTotal.ParryRating = statsRace.ParryRating + statsGearEnchantsBuffs.ParryRating;
            

            statsTotal.ArmorPenetration = statsRace.ArmorPenetration + statsGearEnchantsBuffs.ArmorPenetration;
            statsTotal.Bloodlust = statsGearEnchantsBuffs.Bloodlust;
            statsTotal.DrumsOfBattle = statsGearEnchantsBuffs.DrumsOfBattle;
            statsTotal.SpellDamageRating = statsGearEnchantsBuffs.SpellDamageRating;
            statsTotal.SpellDamageRating += statsGearEnchantsBuffs.SpellDamageFromSpiritPercentage * statsGearEnchantsBuffs.Spirit;
            statsTotal.BonusCritMultiplier = statsGearEnchantsBuffs.BonusCritMultiplier;
            statsTotal.BonusPhysicalDamageMultiplier = statsGearEnchantsBuffs.BonusPhysicalDamageMultiplier;
            statsTotal.BonusCrusaderStrikeDamageMultiplier = statsGearEnchantsBuffs.BonusCrusaderStrikeDamageMultiplier;
            statsTotal.WindfuryAPBonus = statsGearEnchantsBuffs.WindfuryAPBonus;
            statsTotal.WeaponDamage = statsGearEnchantsBuffs.WeaponDamage;
            statsTotal.BonusSpellPowerMultiplier = statsGearEnchantsBuffs.BonusSpellPowerMultiplier;
            statsTotal.ShatteredSunMightProc = statsGearEnchantsBuffs.ShatteredSunMightProc;
            return (statsTotal);
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            List<ComparisonCalculationBase> comparisonList = new List<ComparisonCalculationBase>();
            CharacterCalculationsRetribution baseCalc,  calc;
            ComparisonCalculationBase comparison;
            float[] subPoints;

            switch (chartName)
            {
                case "Item Budget":
                    Item[] itemList = new Item[] {
                        new Item() { Stats = new Stats() { Strength = 10 } },
                        new Item() { Stats = new Stats() { Agility = 10 } },
                        new Item() { Stats = new Stats() { SpellDamageRating = 11.7f } },
                        new Item() { Stats = new Stats() { HitRating = 10 } },
                        new Item() { Stats = new Stats() { HasteRating = 10 } },
                        new Item() { Stats = new Stats() { CritRating = 10 } },
                        new Item() { Stats = new Stats() { ArmorPenetration = 66.67f } },
                        new Item() { Stats = new Stats() { AttackPower = 20 } },
                        new Item() { Stats = new Stats() { ExpertiseRating=10 } }
                    };
                    string[] statList = new string[] {
                        "Strength",
                        "Agility",
                        "Spell Damage",
                        "Hit Rating",
                        "Haste Rating",
                        "Crit Rating",
                        "Armor Penetration",
                        "Attack Power",
                        "Expertise Rating"
                    };

                    baseCalc = GetCharacterCalculations(character) as CharacterCalculationsRetribution;


                    for (int index = 0; index < statList.Length; index++)
                    {
                        calc = GetCharacterCalculations(character, itemList[index]) as CharacterCalculationsRetribution;

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

        public override Stats GetRelevantStats(Stats stats)
        {
            return new Stats()
            {
                Health = stats.Health,
                Mana = stats.Mana,
                Stamina = stats.Stamina,
                Intellect = stats.Intellect,
                Spirit = stats.Spirit,
                Mp5 = stats.Mp5,
                Strength = stats.Strength,
                Agility = stats.Agility,
                AttackPower = stats.AttackPower,
				HitRating = stats.HitRating,
				CritRating = stats.CritRating,
				LotPCritRating = stats.LotPCritRating,
                HasteRating = stats.HasteRating,
                ArmorPenetration = stats.ArmorPenetration,
                ExpertiseRating = stats.ExpertiseRating,
                WeaponDamage = stats.WeaponDamage,
                Bloodlust = stats.Bloodlust,
                SpellDamageRating = stats.SpellDamageRating,
                BonusCritMultiplier = stats.BonusCritMultiplier,
                BonusCrusaderStrikeDamageMultiplier = stats.BonusCrusaderStrikeDamageMultiplier,
                BonusPhysicalDamageMultiplier = stats.BonusPhysicalDamageMultiplier,
                DrumsOfBattle = stats.DrumsOfBattle,
                DrumsOfWar = stats.DrumsOfWar,
                WindfuryAPBonus = stats.WindfuryAPBonus
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            return true 
                /*((
                 stats.Health +
                 stats.Mana +
                 stats.Stamina +
                 stats.Intellect +
                 stats.SpellCritRating +
                 stats.SpellDamageRating +
                 stats.Strength +
                 stats.Agility +
                 stats.AttackPower +
                 stats.ArmorPenetration +
                 stats.ExpertiseRating +
                 stats.HasteRating +
                 stats.HitRating +
                 stats.CritRating +
                 stats.LotPCritRating +
                 stats.BonusStrengthMultiplier +
                 stats.BonusAttackPowerMultiplier +
                 stats.BonusPhysicalDamageMultiplier +
                 stats.BonusCritMultiplier +
                 stats.BonusCrusaderStrikeDamageMultiplier +
                 stats.WindfuryAPBonus +
                 stats.Bloodlust +
                 stats.ExposeWeakness +
                 stats.DrumsOfBattle +
                 stats.WeaponDamage +
                 stats.BonusSpellPowerMultiplier +
                 stats.SpellDamageFromSpiritPercentage +
                 stats.SpellHitRating +
                 stats.Spirit) > 0)*/
             ; 
        }

        /// <summary>
        /// Saves the talents for the character
        /// </summary>
        /// <param name="character">The character for whom the talents should be saved</param>
        public Character GetTalents(Character character)
        {
            CalculationOptionsRetribution calcOpts = character.CalculationOptions as CalculationOptionsRetribution;
            if (!calcOpts.TalentsSaved)
            {
                if (character.Talents.Trees.Count > 0)
                {
					if (character.Talents.Trees.ContainsKey("Protection"))
					{
						calcOpts.Precision = character.Talents.Trees["Protection"][2].PointsInvested;
					}
					if (character.Talents.Trees.ContainsKey("Retribution"))
					{
						calcOpts.Crusade = character.Talents.Trees["Retribution"][11].PointsInvested;
						calcOpts.TwoHandedSpec = character.Talents.Trees["Retribution"][12].PointsInvested;
                        calcOpts.SanctityAura = character.Talents.Trees["Retribution"][13].PointsInvested;
						calcOpts.ImprovedSanctityAura = character.Talents.Trees["Retribution"][14].PointsInvested;
						calcOpts.SanctifiedSeals = character.Talents.Trees["Retribution"][17].PointsInvested;
						calcOpts.Fanaticism = character.Talents.Trees["Retribution"][20].PointsInvested;
                        calcOpts.Vengeance = character.Talents.Trees["Retribution"][15].PointsInvested;
						calcOpts.Conviction = character.Talents.Trees["Retribution"][6].PointsInvested;
					}
					if (character.Talents.Trees.ContainsKey("Holy"))
					{
						calcOpts.DivineStrength = character.Talents.Trees["Holy"][0].PointsInvested;
					}
                    calcOpts.TalentsSaved = true;
                }
            }
            return character;
        }
    }
}
