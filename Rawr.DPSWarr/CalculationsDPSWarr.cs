using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.DPSWarr
{
	[Rawr.Calculations.RawrModelInfo("DPSWarr", "Ability_Rogue_Ambush", Character.CharacterClass.Warrior)]
	class CalculationsDPSWarr : CalculationsBase
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
                    "Basic Stats:Hasted Speed",
					"DPS Breakdown:Mortal Strike",
                    "DPS Breakdown:Slam",
                    "DPS Breakdown:White",
                    "DPS Breakdown:Whirlwind",
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
            get { return _calculationOptionsPanel ?? (_calculationOptionsPanel = new CalculationOptionsPanelDPSWarr()); }
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
                        Item.ItemType.Crossbow,
                        Item.ItemType.Bow,
                        Item.ItemType.Gun,
                        Item.ItemType.Thrown,
                        Item.ItemType.Polearm,
                        Item.ItemType.TwoHandAxe,
                        Item.ItemType.TwoHandMace,
                        Item.ItemType.TwoHandSword
					}));
            }
        }

		public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.Warrior; } }
		public override ComparisonCalculationBase CreateNewComparisonCalculation()
        {
            return  new ComparisonCalculationDPSWarr();
        }

        public override CharacterCalculationsBase CreateNewCharacterCalculations()
        {
            return new CharacterCalculationsDPSWarr();
        }

		public override ICalculationOptionBase DeserializeDataObject(string xml)
		{
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsDPSWarr));
			System.IO.StringReader reader = new System.IO.StringReader(xml);
			CalculationOptionsDPSWarr calcOpts = serializer.Deserialize(reader) as CalculationOptionsDPSWarr;
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
            /*
             * To Do:
             * -Deep wounds
             * -Allow more talent choices
             * -Check for certain talents before calculating
             * -Add in rotation choice
             * -Add in GCD check for haste and slam rotations. (2.5 second min swing timer otherwise gcd ruins it)
             * -Add in control for changing rotation during heroism/bloodlust?
             * -Handle bloodlust/heroism same as mage model instead of average over fight.
             * */
            //character = GetTalents(character);
            Stats stats = GetCharacterStats( character, additionalItem );
            
            CharacterCalculationsDPSWarr calcs = new CharacterCalculationsDPSWarr();
            calcs.BasicStats = stats;

			CalculationOptionsDPSWarr calcOpts = character.CalculationOptions as CalculationOptionsDPSWarr;
			
            float avgBaseWeaponHit = 0.0f, hastedSpeed = 2.0f, physicalCritModifier = 0.0f, chanceToBeDodged = 6.5f, chanceToMiss = 9.0f; ;
            float chanceToGlance = 0.25f, glancingAmount = 0.35f;
            float slamDPS = 0.0f, msDPS = 0.0f, wfDPS = 0.0f, wwDPS = 0.0f;
            float rotationTime;
            float FlurryHaste;
            #region Mitigation
            //Default Boss Armor
			float bossArmor = calcOpts.BossArmor;

            float totalArP = stats.ArmorPenetration;
            float modifiedTargetArmor = bossArmor - totalArP;
            float mitigation = 1 - modifiedTargetArmor / (modifiedTargetArmor + 10557.5f);
            #endregion

            //Flurry *Assumes 4 attakcs per flurry to refresh. This is a loose average of the # of attacks
            //for each flurry including instants & slams.
            //Also allows user to check 100% uptime flurry if desired.
            FlurryHaste = (0.05f * calcOpts.Flurry);

            if (calcOpts.FlurryUptime == 0)
            {
                FlurryHaste = 1.0f+(1-(float)Math.Pow((1-(stats.CritRating/22.08f/100.0f)),4.0f))*FlurryHaste;
            }
            else
            {
                FlurryHaste += 1.0f;
            }

            //Variables for Bloodlust/Deathwish uptime
            int remainder = 0, noOfFullDW = 0, noOfFullBL;
            int div;
            float partialUptime, totalUptime;
            
            //Fight duration
            float fightDuration = calcOpts.FightLength * 60;
            
            float bloodlust = 1.0f;
            #region Bloodlust
            if (stats.Bloodlust > 0)
            {
                //Bloodlust -- Calculating uptime *Credit to Ret Model
                //Note, not working correctly in the ret model
                //Haste is multiplicative something the ret model isn't handling.
                div = Math.DivRem(Convert.ToInt32(fightDuration), 600, out remainder);
                if (remainder == 0)
                    noOfFullBL = div;
                else
                    noOfFullBL = Convert.ToInt32(Math.Ceiling(Convert.ToDouble((fightDuration + 40) / 600)));

                partialUptime = fightDuration - noOfFullBL * 600;
                if (partialUptime < 0) partialUptime = 0;
                totalUptime = partialUptime + noOfFullBL * 40f;

                bloodlust = (1.0f + (0.30f * (totalUptime / fightDuration)));
            }
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
            //Check if we have the talent impale.
            float impale = 1.0f + (0.1f * calcOpts.Impale);

            #region White Damage and Multipliers
            
            //2 Handed Spec
            float twoHandedSpec = 1.0f + (0.01f * calcOpts.TwoHandedSpec);
            
            if (character.MainHand != null)
            {
                avgBaseWeaponHit = twoHandedSpec*(character.MainHand.MinDamage + character.MainHand.MaxDamage + stats.WeaponDamage*2f) / 2.0f;
                hastedSpeed = (stats.HasteRating == 0) ? character.MainHand.Speed : character.MainHand.Speed / (1 + (stats.HasteRating + stats.DrumsOfBattle) / 1576f);
                hastedSpeed = hastedSpeed / FlurryHaste;
                hastedSpeed = hastedSpeed / bloodlust;
            }

            rotationTime = (hastedSpeed * 4) + (0.5f * 4);
            rotationTime += ((calcOpts.SlamLatency)*4);

            //Add Attack Power Bonus
            avgBaseWeaponHit += twoHandedSpec*(stats.AttackPower / 14.0f) * ((character.MainHand == null) ? 2.0f : character.MainHand.Speed);

            
            //Take Non-Stat Buffs into account
            physicalCritModifier = 1.0f + ((stats.CritRating / 22.08f) / 100.0f) * (1f + stats.BonusCritMultiplier * 2f);

            chanceToBeDodged -= (float)(Math.Floor(stats.ExpertiseRating / 3.89f)*0.25f);
			chanceToBeDodged -= calcOpts.WeaponMastery;
            if (chanceToBeDodged < 0.0f) chanceToBeDodged = 0.0f;

            chanceToMiss -= stats.HitRating / 15.76f;
            if (chanceToMiss < 0.0f) chanceToMiss = 0.0f;

            float avgBaseWeaponHitPost = (avgBaseWeaponHit * physicalCritModifier - avgBaseWeaponHit * (chanceToMiss + chanceToBeDodged) / 100.0f
                - avgBaseWeaponHit * chanceToGlance * glancingAmount);

            //Death Wish -- Calculating uptime *Credit to Ret model AW up time.
            div = Math.DivRem(Convert.ToInt32(fightDuration), 180,out remainder);
            if (remainder == 0) 
                noOfFullDW = div;
            else
            noOfFullDW = Convert.ToInt32(Math.Ceiling(Convert.ToDouble((fightDuration + 20) / 180)));
            partialUptime = fightDuration - noOfFullDW * 180;
            if (partialUptime < 0) partialUptime = 0;
            totalUptime = partialUptime + noOfFullDW * 20f;
            float deathWish = 1f + 0.20f * totalUptime / fightDuration;

            //Misery 
            float misery = 1f + stats.BonusSpellPowerMultiplier;

            //Blood Frenzy : TODO Take from Debuff List
            float damageMod = 1.0f + stats.BonusDamageMultiplier;
            
            float impSancAura = 1.0f;
            //Added Imp Sanc aura to the buff list, if total damage mod is greater then just
            //blood frenzy assume we have imp sanc... (not the best way...)
            if (damageMod > 1.04f)
            {
                impSancAura = 1.02f;
            }
            float ssoNeckProcDPS = 0f;
           

            //TODO: Add Mitigation
            avgBaseWeaponHitPost *= damageMod * deathWish * mitigation;
            float dpsWhite = (avgBaseWeaponHitPost*4) / rotationTime;
            calcs.WhiteDPSPoints = dpsWhite;
            calcs.HastedSpeed = hastedSpeed;
            #endregion

            if (stats.ShatteredSunMightProc > 0)
            {
                switch (shattrathFaction)
                {
                    case "Scryer":
                        ssoNeckProcDPS = 350f * deathWish * misery * impSancAura * physicalCritModifier / 50f;
                        break;
                }
            }
            #region Slam
            if (character.MainHand != null)
            {
                avgBaseWeaponHit = twoHandedSpec * (character.MainHand.MinDamage + character.MainHand.MaxDamage + stats.WeaponDamage * 2f) / 2.0f;
            }
            avgBaseWeaponHit += twoHandedSpec * (stats.AttackPower / 14.0f) * ((character.MainHand == null) ? 1.0f : character.MainHand.Speed);
            //add in slam damage
            avgBaseWeaponHit += twoHandedSpec * 140;

            physicalCritModifier = 1.0f + ((stats.CritRating / 22.08f) / 100.0f) * ((1f + stats.BonusCritMultiplier * 2f)*impale);

            avgBaseWeaponHitPost = (avgBaseWeaponHit * physicalCritModifier - avgBaseWeaponHit * (chanceToMiss + chanceToBeDodged) / 100.0f);
            avgBaseWeaponHitPost *= damageMod * deathWish * mitigation;
            slamDPS = (avgBaseWeaponHitPost * 4) / rotationTime;
            calcs.SlamDPSPoints = slamDPS;
            #endregion

            //WW and MS are normalized (2h = 3.3 speed)
            float normalizedAP = twoHandedSpec * (stats.AttackPower / 14.0f) * 3.3f;
            #region Mortal Strike
            if (character.MainHand != null)
            {
                avgBaseWeaponHit = twoHandedSpec * (character.MainHand.MinDamage + character.MainHand.MaxDamage + stats.WeaponDamage * 2f) / 2.0f;
            }
            avgBaseWeaponHit += normalizedAP;
            //add in MS damage
            avgBaseWeaponHit += twoHandedSpec * 210;
           
            avgBaseWeaponHitPost = (avgBaseWeaponHit * physicalCritModifier - avgBaseWeaponHit * (chanceToMiss + chanceToBeDodged) / 100.0f);
            avgBaseWeaponHitPost *= damageMod * deathWish * mitigation;
            msDPS = (avgBaseWeaponHitPost * 2) / rotationTime;
            calcs.MSDPSPoints = msDPS;
            #endregion

            #region WhirlWind
            if (character.MainHand != null)
            {
                avgBaseWeaponHit = twoHandedSpec * (character.MainHand.MinDamage + character.MainHand.MaxDamage + stats.WeaponDamage * 2f) / 2.0f;
            }
            avgBaseWeaponHit += normalizedAP;

            avgBaseWeaponHitPost = (avgBaseWeaponHit * physicalCritModifier - avgBaseWeaponHit * (chanceToMiss + chanceToBeDodged) / 100.0f);
            avgBaseWeaponHitPost *= damageMod * deathWish * mitigation;
            
            wwDPS = (avgBaseWeaponHitPost * 1) / rotationTime;
            calcs.WWDPSPoints = wwDPS;
            #endregion

            #region Windfury
            //WF only procs on whites.
            //White time = rotation time/4
            float avgTimeBetnWF = ((rotationTime/4) / (1.0f - (chanceToBeDodged + chanceToMiss) / 100f)) * 5.0f;
            float wfAPIncrease = stats.WindfuryAPBonus;
            float wfHitPre = avgBaseWeaponHit + (wfAPIncrease / 14f) * ((character.MainHand == null) ? 0 : character.MainHand.Speed);
            float wfHitPost = (wfHitPre * physicalCritModifier) - (wfHitPre * (chanceToMiss + chanceToBeDodged) / 100f) -
                (wfHitPre * glancingAmount * chanceToGlance);
            if (wfAPIncrease > 0)
            {
                wfHitPost *= damageMod * deathWish * mitigation;
            }
            else
            {
                wfHitPost = 0f;
            }
            wfDPS = wfHitPost / avgTimeBetnWF;
            calcs.WFDPSPoints = wfDPS;
            #endregion 

            #region SwordSpec
            float swordSpecDps = 0.0f;
            if ((character.MainHand != null) &&
                (character.MainHand.Type == Item.ItemType.TwoHandSword))
            {
                //Assume 11 sword procs every 220 attacks (5%)
                //Each rotation has 11 attacks
                float swordSpecHit = (rotationTime * dpsWhite) / 4;
                swordSpecDps = (swordSpecHit*11) / (rotationTime * 20);
                dpsWhite += swordSpecDps;
                calcs.WhiteDPSPoints += swordSpecDps;
            }
            #endregion

            calcs.DPSPoints = dpsWhite + (character.MainHand == null ? 0 : slamDPS + msDPS + wwDPS) + wfDPS + ssoNeckProcDPS;
            calcs.SubPoints = new float[] { calcs.DPSPoints };
            calcs.OverallPoints = calcs.DPSPoints;
            calcs.BasicStats.WeaponDamage = avgBaseWeaponHit * impSancAura;
            return calcs;
        }

        #region Warrior Race Stats
        private static float[,] BaseWarriorRaceStats = new float[,] 
		{
							//	Strength,	Agility,	Stamina
            /*Human*/		{	145f,	    96f,	    132f,   },
            /*Orc*/			{	148f,		93f,		135f,	},
            /*Dwarf*/		{	146f,	    92f,	    135f,   },
			/*Night Elf*/	{	142f,	    101f,	    132f,   },
	        /*Undead*/		{	144f,	    94f,	    133f,   },
			/*Tauren*/		{	150f,		91f,		135f,	},
	        /*Gnome*/		{	140f,	    99f,	    132f,   },
			/*Troll*/		{	145f,	    98f,	    133f,   },	
			/*BloodElf*/	{	0f,		    0f,		    0f,	    },
			/*Draenei*/		{	146f,		93f,		132f,	},
		};

        private Stats GetRaceStats(Character character)
        {
            Stats statsRace;
            switch (character.Race)
            {
                case Character.CharacterRace.Human:
                    statsRace = new Stats()
                    {
                        Health = 4264f,
                        Strength = (float)BaseWarriorRaceStats[0, 0],
                        Agility = (float)BaseWarriorRaceStats[0, 1],
                        Stamina = (float)BaseWarriorRaceStats[0, 2],
                        PhysicalCrit = 1.18f * 22.08f,
                        AttackPower = 190f,
                        Dodge = 0.75f,
                    };
                    if ((character.MainHand != null) &&
                        ((character.MainHand.Type == Item.ItemType.TwoHandSword) ||
                         (character.MainHand.Type == Item.ItemType.TwoHandMace)))
                    {
                        statsRace.Expertise += 5f;
                    }
                    break;
                case Character.CharacterRace.Orc:
                    statsRace = new Stats()
                    {
                        Health = 4264f,
                        Strength = (float)BaseWarriorRaceStats[1, 0],
                        Agility = (float)BaseWarriorRaceStats[1, 1],
                        Stamina = (float)BaseWarriorRaceStats[1, 2],
                        PhysicalCrit = 1.18f * 22.08f,
                        AttackPower = 190f,
                        Dodge = 0.75f,
                    };

                    if ((character.MainHand != null) &&
                        (character.MainHand.Type == Item.ItemType.TwoHandAxe))
                    {
                        statsRace.Expertise += 5f;
                    }
                    break;
                case Character.CharacterRace.Dwarf:
                    statsRace = new Stats()
                    {
                        Health = 4264f,
                        Strength = (float)BaseWarriorRaceStats[2, 0],
                        Agility = (float)BaseWarriorRaceStats[2, 1],
                        Stamina = (float)BaseWarriorRaceStats[2, 2],
                        PhysicalCrit = 1.18f * 22.08f,
                        AttackPower = 190f,
                        Dodge = 0.75f,
                    };
                    break;
                case Character.CharacterRace.NightElf:
                    statsRace = new Stats()
                    {
                        Health = 4264f,
                        Strength = (float)BaseWarriorRaceStats[3, 0],
                        Agility = (float)BaseWarriorRaceStats[3, 1],
                        Stamina = (float)BaseWarriorRaceStats[3, 2],
                        PhysicalCrit = 1.18f * 22.08f,
                        AttackPower = 190f,
                        Dodge = 1f + 0.75f,
                    };
                    break;
                case Character.CharacterRace.Undead:
                    statsRace = new Stats()
                    {
                        Health = 4264f,
                        Strength = (float)BaseWarriorRaceStats[4, 0],
                        Agility = (float)BaseWarriorRaceStats[4, 1],
                        Stamina = (float)BaseWarriorRaceStats[4, 2],
                        PhysicalCrit = 1.18f * 22.08f,
                        AttackPower = 190f,
                        Dodge = 0.75f,
                    };
                    break;
                case Character.CharacterRace.Tauren:
                    statsRace = new Stats()
                    {
                        Health = 4264f,
                        Strength = (float)BaseWarriorRaceStats[5, 0],
                        Agility = (float)BaseWarriorRaceStats[5, 1],
                        Stamina = (float)BaseWarriorRaceStats[5, 2],
                        PhysicalCrit = 1.18f * 22.08f,
                        AttackPower = 190f,
                        BonusHealthMultiplier = 0.05f,
                        Dodge = 0.75f,
                    };
                    break;
                case Character.CharacterRace.Gnome:
                    statsRace = new Stats()
                    {
                        Health = 4264f,
                        Strength = (float)BaseWarriorRaceStats[6, 0],
                        Agility = (float)BaseWarriorRaceStats[6, 1],
                        Stamina = (float)BaseWarriorRaceStats[6, 2],
                        PhysicalCrit = 1.18f * 22.08f,
                        AttackPower = 190f,
                        Dodge = 0.75f,
                    };
                    break;
                case Character.CharacterRace.Troll:
                    statsRace = new Stats()
                    {
                        Health = 4264f,
                        Strength = (float)BaseWarriorRaceStats[7, 0],
                        Agility = (float)BaseWarriorRaceStats[7, 1],
                        Stamina = (float)BaseWarriorRaceStats[7, 2],
                        PhysicalCrit = 1.18f * 22.08f,
                        AttackPower = 190f,
                        Dodge = 0.75f,
                    };
                    break;
                case Character.CharacterRace.Draenei:
                    statsRace = new Stats()
                    {
                        Health = 4264f,
                        Strength = (float)BaseWarriorRaceStats[9, 0],
                        Agility = (float)BaseWarriorRaceStats[9, 1],
                        Stamina = (float)BaseWarriorRaceStats[9, 2],
                        PhysicalCrit = 1.18f*22.08f,
                        AttackPower = 190f,
                        Hit = 1f,
                        Dodge = 0.75f,
                    };
                    break;
                default:
                    statsRace = new Stats();
                    break;
            }

            return statsRace;
        }
        #endregion

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
            Stats statsRace = GetRaceStats(character); 

            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            //Add Expose Weakness since it's not listed as an AP buff
            if(statsBuffs.ExposeWeakness > 0) statsBuffs.AttackPower += 200f;

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
            //TalentTree tree = character.AllTalents;
            float agiBase = (float)Math.Floor(statsRace.Agility * (1 + statsRace.BonusAgilityMultiplier));
            float agiBonus = (float)Math.Floor(statsGearEnchantsBuffs.Agility * (1 + statsRace.BonusAgilityMultiplier));
            float strBase = (float)Math.Floor(statsRace.Strength * (1 + statsRace.BonusStrengthMultiplier));
            float strBonus = (float)Math.Floor(statsGearEnchantsBuffs.Strength * (1 + statsRace.BonusStrengthMultiplier));
            float staBase = (float)Math.Floor(statsRace.Stamina * (1 + statsRace.BonusStaminaMultiplier));
            float staBonus = (float)Math.Floor(statsGearEnchantsBuffs.Stamina * (1 + statsRace.BonusStaminaMultiplier));

            Stats statsTotal = new Stats();
            statsTotal.BonusAttackPowerMultiplier = ((1 + statsRace.BonusAttackPowerMultiplier) * (1 + statsGearEnchantsBuffs.BonusAttackPowerMultiplier)) - 1;
            statsTotal.BonusAgilityMultiplier = ((1 + statsRace.BonusAgilityMultiplier) * (1 + statsGearEnchantsBuffs.BonusAgilityMultiplier)) - 1;
            statsTotal.BonusStrengthMultiplier = ((1 + statsRace.BonusStrengthMultiplier) * (1 + statsGearEnchantsBuffs.BonusStrengthMultiplier)) - 1;
            statsTotal.BonusStaminaMultiplier = ((1 + statsRace.BonusStaminaMultiplier) * (1 + statsGearEnchantsBuffs.BonusStaminaMultiplier)) - 1;


            statsTotal.Agility = (agiBase + (float)Math.Floor((agiBase * statsBuffs.BonusAgilityMultiplier) + agiBonus * (1 + statsBuffs.BonusAgilityMultiplier)));
          
            statsTotal.Strength = (strBase + (float)Math.Floor((strBase * statsBuffs.BonusStrengthMultiplier) + strBonus * (1 + statsBuffs.BonusStrengthMultiplier)));

            statsTotal.Stamina = (staBase + (float)Math.Round((staBase * statsBuffs.BonusStaminaMultiplier) + staBonus * (1 + statsBuffs.BonusStaminaMultiplier)));          

            statsTotal.Health = (float)Math.Round(((statsRace.Health + statsGearEnchantsBuffs.Health + ((statsTotal.Stamina-staBase) * 10f))));

            statsTotal.AttackPower = (float)Math.Floor((statsRace.AttackPower + statsGearEnchantsBuffs.AttackPower + (statsTotal.Strength * 2)) * (1f + statsTotal.BonusAttackPowerMultiplier));

            statsTotal.CritRating = statsRace.PhysicalCrit + statsGearEnchantsBuffs.CritRating;
            statsTotal.CritRating += ((statsTotal.Agility/ 33.333f) * 22.08f);
            statsTotal.CritRating += statsBuffs.LotPCritRating;
            
            /*Check if axe, if so assume poleaxe spec
              -This allows easier comparison between weapon specs
             */
            if ((character.MainHand != null) &&
                ((character.MainHand.Type == Item.ItemType.TwoHandAxe) 
                || (character.MainHand.Type == Item.ItemType.Polearm)))
            {
                statsTotal.CritRating += (22.08f * 5.0f);
            }


			CalculationOptionsDPSWarr calcOpts = character.CalculationOptions as CalculationOptionsDPSWarr;
			statsTotal.CritRating += (22.08f * calcOpts.Cruelty);
            statsTotal.CritRating += (22.08f * calcOpts.Stance);

            statsTotal.HitRating = statsRace.HitRating + statsGearEnchantsBuffs.HitRating;
			statsTotal.HitRating += (15.76f * calcOpts.Precision);
			
            statsTotal.ExpertiseRating = (statsRace.Expertise*3.9f) + statsGearEnchantsBuffs.ExpertiseRating;
            
            statsTotal.HasteRating = statsRace.HasteRating + statsGearEnchantsBuffs.HasteRating;

            statsTotal.DodgeRating = statsRace.DodgeRating + statsGearEnchantsBuffs.DodgeRating;
            statsTotal.DodgeRating = ((statsTotal.Agility / 20f) * 18.92f);
            
            statsTotal.ParryRating = statsRace.ParryRating + statsGearEnchantsBuffs.ParryRating;
            
            statsTotal.ArmorPenetration = statsRace.ArmorPenetration + statsGearEnchantsBuffs.ArmorPenetration;
            
            statsTotal.Bloodlust = statsGearEnchantsBuffs.Bloodlust;
            statsTotal.DrumsOfBattle = statsGearEnchantsBuffs.DrumsOfBattle;
            statsTotal.BonusCritMultiplier = statsGearEnchantsBuffs.BonusCritMultiplier;
            statsTotal.BonusDamageMultiplier = statsGearEnchantsBuffs.BonusDamageMultiplier;
            statsTotal.WindfuryAPBonus = statsGearEnchantsBuffs.WindfuryAPBonus;
            statsTotal.WeaponDamage = statsGearEnchantsBuffs.WeaponDamage;
            statsTotal.ShatteredSunMightProc = statsGearEnchantsBuffs.ShatteredSunMightProc;
            return (statsTotal);

           
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            List<ComparisonCalculationBase> comparisonList = new List<ComparisonCalculationBase>();
            CharacterCalculationsDPSWarr baseCalc,  calc;
            ComparisonCalculationBase comparison;
            float[] subPoints;

            switch (chartName)
            {
                case "Item Budget":
                    Item[] itemList = new Item[] {
                        new Item() { Stats = new Stats() { Strength = 10 } },
                        new Item() { Stats = new Stats() { Agility = 10 } },
                        new Item() { Stats = new Stats() { HitRating = 10 } },
                        new Item() { Stats = new Stats() { HasteRating = 10 } },
                        new Item() { Stats = new Stats() { CritRating = 10 } },
                        new Item() { Stats = new Stats() { ArmorPenetration = 70.0f } },
                        new Item() { Stats = new Stats() { AttackPower = 20 } },
                        new Item() { Stats = new Stats() { ExpertiseRating=10 } }
                    };
                    string[] statList = new string[] {
                        "Strength",
                        "Agility",
                        "Hit Rating",
                        "Haste Rating",
                        "Crit Rating",
                        "Armor Penetration",
                        "Attack Power",
                        "Expertise Rating"
                    };

                    baseCalc = GetCharacterCalculations(character) as CharacterCalculationsDPSWarr;


                    for (int index = 0; index < statList.Length; index++)
                    {
                        calc = GetCharacterCalculations(character, itemList[index]) as CharacterCalculationsDPSWarr;

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
                Stamina = stats.Stamina,
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
                DrumsOfBattle = stats.DrumsOfBattle,
                BonusCritMultiplier = stats.BonusCritMultiplier,
                BonusCrusaderStrikeDamageMultiplier = stats.BonusCrusaderStrikeDamageMultiplier,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
                WindfuryAPBonus = stats.WindfuryAPBonus
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            return true;/* ((
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

		///// <summary>
		///// Saves the talents for the character
		///// </summary>
		///// <param name="character">The character for whom the talents should be saved</param>
        //public Character GetTalents(Character character)
		//{
		//    if (!character.CalculationOptions.ContainsKey("TalentsSaved") || character.CalculationOptions["TalentsSaved"] == "0")
		//    {
		//        if (character.Talents.Trees.Count > 0)
		//        {
		//            TalentTree tree = character.Talents;
		//            if (character.Talents.Trees.ContainsKey("Arms"))
		//            {
		//                character.CalculationOptions.Add("DeepWounds", tree.GetTalent("Deep Wounds").PointsInvested.ToString());
		//                character.CalculationOptions.Add("Impale", tree.GetTalent("Impale").PointsInvested.ToString());
		//                character.CalculationOptions.Add("TwoHandedSpec", tree.GetTalent("Two-Handed Weapon Specialization").PointsInvested.ToString());
		//                character.CalculationOptions.Add("DeathWish", tree.GetTalent("Death Wish").PointsInvested.ToString());
		//                character.CalculationOptions.Add("MortalStrike", tree.GetTalent("Mortal Strike").PointsInvested.ToString());
		//            }
		//            if (character.Talents.Trees.ContainsKey("Fury"))
		//            {
		//                character.CalculationOptions.Add("Cruelty", tree.GetTalent("Cruelty").PointsInvested.ToString());
		//                character.CalculationOptions.Add("WeaponMastery", tree.GetTalent("Weapon Mastery").PointsInvested.ToString());
		//                character.CalculationOptions.Add("ImpSlam", tree.GetTalent("Improved Slam").PointsInvested.ToString());
		//                character.CalculationOptions.Add("Flurry", tree.GetTalent("Flurry").PointsInvested.ToString());
		//            }
		//            character.CalculationOptions["TalentsSaved"] = "1";
		//        }
		//    }
		//    return character;
		//}
    }
}
