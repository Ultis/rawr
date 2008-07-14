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
            Stats stats = GetCharacterStats(character, additionalItem);

            CharacterCalculationsRetribution calcs = new CharacterCalculationsRetribution();
            calcs.BasicStats = stats;

            float dpsWhite = 0f, dpsSeal = 0f, dpsCrusader = 0f, dpsConsecration = 0f, dpsExorcism = 0f, dpsJudgement = 0f, dpsWindfury = 0.0f;
            float baseSpeed = 1f, hastedSpeed = 1f, baseDamage = 0f, mitigation = 1f;
            float whiteHit = 0f, physHitsCrits = 1f, totalMiss = 0f, spellHitsCrits = 1f, spellResist = 0f;
            float chanceToGlance = 0.25f, glancingAmount = 0.35f;

            // Damage Multipliers
            float twoHandedSpec = 1f + (0.02f * (float)calcOpts.TwoHandedSpec);
            float impSancAura = 1f + 0.01f * (float)calcOpts.ImprovedSanctityAura;
            float crusade = 1f + 0.01f * (float)calcOpts.Crusade;
            float vengeance = 1f + 0.03f * (float)calcOpts.Vengeance;
            float sancAura = 1f + 0.1f * (float)calcOpts.SanctityAura;
            float spellPower = 1f + stats.BonusSpellPowerMultiplier; // Covers all % spell damage increases.  Misery, FI.
            float physPower = 1f + stats.BonusPhysicalDamageMultiplier; // Covers all % physical damage increases.  Blood Frenzy, FI.
            float partialResist = 0.953f; // Average of 4.7% damage lost to partial resists on spells
            float jotc = 219f;

            // Avenging Wrath -- Calculating uptime
            float fightDuration = calcOpts.FightLength * 60;
            int remainder = 0, noOfFullAW = 0;
            int div = Math.DivRem(Convert.ToInt32(fightDuration), 180, out remainder);
            if (remainder == 0)
                noOfFullAW = div;
            else
                noOfFullAW = Convert.ToInt32(Math.Ceiling(Convert.ToDouble((fightDuration + 20) / 180)));
            float partialUptime = fightDuration - noOfFullAW * 180;
            if (partialUptime < 0) partialUptime = 0;
            float totalUptime = partialUptime + noOfFullAW * 20f;
            float avWrath = 1f + 0.30f * totalUptime / fightDuration;

            // Combined damage multipliers
            float allDamMult = avWrath * crusade * impSancAura;
            float holyDamMult = allDamMult * spellPower * sancAura * vengeance;
            float physDamMult = allDamMult * physPower * twoHandedSpec * vengeance;


            if (character.MainHand != null)
            {
                baseSpeed = character.MainHand.Speed;
                baseDamage = (character.MainHand.MinDamage + character.MainHand.MaxDamage) / 2f + stats.WeaponDamage;
            }


            #region Attack Speed
            {
                hastedSpeed = baseSpeed / (1f + (stats.HasteRating / 1576f));

                // Mongoose Enchant grants 2% haste
                if (stats.MongooseProc > 0)
                    hastedSpeed /= 1f + (0.02f * 0.4f);  // ASSUMPTION: Mongoose has a 40% uptime
                if (stats.Bloodlust > 0)
                {
                    float bloodlustUptime = (calcOpts.Bloodlust * 40f);

                    if (bloodlustUptime > fightDuration) bloodlustUptime = 1f;
                    else bloodlustUptime /= fightDuration;

                    hastedSpeed /= 1f + (0.3f * bloodlustUptime);
                }
            }
            #endregion


            #region Mitigation
            {
                float targetArmor = calcOpts.BossArmor, totalArP = stats.ArmorPenetration;

                // Effective armor after ArP
                targetArmor -= totalArP;
                if (targetArmor < 0) targetArmor = 0f;

                // Convert armor to mitigation
                mitigation = 1.0f - (targetArmor / (targetArmor + 10557.5f));

                // Executioner enchant.  ASSUMPTION: Executioner has a 40% uptime.
                if (stats.ExecutionerProc > 0)
                {
                    float exeArmor = targetArmor, exeMitigation = 1.0f, exeUptime = 0.4f, exeArmorPen = 840f;

                    // Find mitigation while Executioner is up
                    exeArmor = targetArmor - exeArmorPen;
                    if (exeArmor < 0) exeArmor = 0f;
                    exeMitigation = 1.0f - (exeArmor / (exeArmor + 10557.5f));

                    // Weighted average of mitigation with and without Executioner, based on Executioner uptime
                    mitigation = (exeMitigation * exeUptime) + (mitigation * (1 - exeUptime));
                }
            }
            #endregion


            #region Crits, Misses, Resists
            {
                float chanceDodged = .065f, chanceMiss = .090f;
                spellResist = .17f;

                // Crits include hits before dodge/miss/resist are subtracted out
                physHitsCrits = 1f + (stats.CritRating / 2208f) * (1f + stats.BonusCritMultiplier * 2f);
                spellHitsCrits = 1f + (stats.SpellCritRating / 2208f) * (1f + stats.BonusCritMultiplier * 1.5f);

                // Calculate from Rating
                spellResist -= stats.SpellHitRating / 1262f;
                chanceDodged -= stats.ExpertiseRating / 1576f;
                chanceMiss -= stats.HitRating / 1576f;

                // Cap at minimum
                if (spellResist < .01f) spellResist = .01f;
                if (chanceDodged < 0f) chanceDodged = 0f;
                if (chanceMiss < 0f) chanceMiss = 0f;

                // Total physical misses
                totalMiss = chanceDodged + chanceMiss;
            }
            #endregion


            #region White Damage
            {
                float whiteAvgDam = 0f, dpsSSO = 0.0f;

                #region SSO Neck Procs
                if (stats.ShatteredSunMightProc > 0)
                {
                    string shattrathFaction = calcOpts.ShattrathFaction;
                    switch (shattrathFaction)
                    {
                        case "Aldor":
                            stats.AttackPower += 39.13f;
                            break;
                        case "Scryer":
                            dpsSSO = 350f * allDamMult * spellPower;
                            dpsSSO *= (physHitsCrits - totalMiss);
                            dpsSSO /= 50;  // 50 seconds between procs
                            break;
                    }
                }
                #endregion

                // White damage per hit.  Basic white hits are use elsewhere.
                whiteHit = baseDamage + (stats.AttackPower / 14.0f) * baseSpeed;
                whiteAvgDam = whiteHit * physDamMult;

                // Average white damage per swing
                whiteAvgDam *= (physHitsCrits - totalMiss - chanceToGlance * glancingAmount) * mitigation;

                // Total white DPS.  Scryer SSO neck added as "white"
                dpsWhite = whiteAvgDam / hastedSpeed;
                dpsWhite += dpsSSO;
            }
            #endregion


            #region Seal
            {
                float sealActualPPM = 0f, sealAvgDam = 0f, windProcRate = .2f;
                if (calcOpts.Seal == 0) // Seal of Command
                {
                    float socPPM = 7f, socCoeff = 0.2f, socHolyCoeff = 0.29f;

                    // Find real PPM.  Procs 7 times per minute before misses
                    sealActualPPM = socPPM * (1f - totalMiss);
                    // Chain Procs: Windfury procs Seal of Command
                    if (stats.WindfuryAPBonus > 0)
                    {
                        // Chance SoC has proc'd on a swing
                        float sealProcChance = socPPM / (60f / hastedSpeed);

                        // Proc chain fails if Windfury misses
                        windProcRate *= (1 - totalMiss);

                        // SoC procs off of Windfury only if SoC has not already proc'd
                        sealActualPPM *= 1 + windProcRate * (1 - sealProcChance);
                    }

                    // Seal Damage per hit
                    sealAvgDam = 0.7f * whiteHit * twoHandedSpec + socCoeff * stats.SpellDamageRating;
                    sealAvgDam = sealAvgDam * holyDamMult + socHolyCoeff * jotc;
                }
                else // Seal of Blood
                {
                    // Find real PPM.  Procs on every hit.
                    sealActualPPM = (60f / hastedSpeed) * (1 - totalMiss);
                    // Chain Procs: Windfury procs Seal of Blood
                    if (stats.WindfuryAPBonus > 0) sealActualPPM *= 1 + windProcRate * (1 - totalMiss);

                    // Seal Damage per hit
                    sealAvgDam = 0.35f * whiteHit * holyDamMult * twoHandedSpec;
                }

                // Seal average damage per proc
                sealAvgDam *= (physHitsCrits - totalMiss) * partialResist;

                // Total Seal DPS
                dpsSeal = sealAvgDam * sealActualPPM / 60f;
            }
            #endregion


            #region Windfury
            if (stats.WindfuryAPBonus > 0)
            {
                float windProcRate = .2f, windPerMin = 0f, windAvgDam = 0f, windAPBonus = stats.WindfuryAPBonus;

                // Find real PPM.  Chance to proc on every hit. and damage per hit
                windPerMin = (60f / hastedSpeed) * (1 - totalMiss) * windProcRate;
                // Chain Procs: Seal of Command can procs Windfury
                if (calcOpts.Seal == 0)
                {
                    float socPPM = 7f;

                    // Proc chain fails if either swing or SoC misses
                    socPPM *= (1 - totalMiss) * (1 - totalMiss);

                    // Windfury procs off of SoC only if Windfury has not already proc'd
                    windPerMin += socPPM * windProcRate * (1 - windProcRate);
                }

                // Windfury damage per hit
                windAvgDam = whiteHit + (windAPBonus / 14) * baseSpeed;
                windAvgDam *= physDamMult;

                // Windfury average damage per proc
                windAvgDam *= (physHitsCrits - totalMiss) * mitigation;

                // Total Windfury DPS
                dpsWindfury = windAvgDam * windPerMin / 60f;
            }
            #endregion


            #region Crusader Strike
            {
                float crusCD = 6f, crusCoeff = 1.1f, crusAvgDam = 0f;

                // Crusader Strike damage per hit
                crusAvgDam = baseDamage + 3.3f * (stats.AttackPower / 14f);
                crusAvgDam *= crusCoeff * physDamMult * (1f + stats.BonusCrusaderStrikeDamageMultiplier);

                // Crusader Strike average damage per swing
                crusAvgDam *= (physHitsCrits - totalMiss) * mitigation;

                // Total Crusader Strike DPS
                dpsCrusader = crusAvgDam / crusCD;
            }
            #endregion


            #region Consecration
            if (calcOpts.ConsecRank != 0)
            {
                float consCD = 9f, consCoeff = 0.952f, consAvgDam = 0f;
                int consRank = calcOpts.ConsecRank;

                // Consecration damage pre-resists
                switch (consRank) // Rank damage + coeff * level reduction * spelldamage
                {
                    case 1: consAvgDam = 64f + consCoeff * (35f / 70f) * stats.SpellDamageRating; break;
                    case 2: consAvgDam = 120f + consCoeff * (45f / 70f) * stats.SpellDamageRating; break;
                    case 3: consAvgDam = 184f + consCoeff * (55f / 70f) * stats.SpellDamageRating; break;
                    case 4: consAvgDam = 280f + consCoeff * (65f / 70f) * stats.SpellDamageRating; break;
                    case 5: consAvgDam = 384f + consCoeff * stats.SpellDamageRating; break;
                    case 6: consAvgDam = 512f + consCoeff * stats.SpellDamageRating; break;
                }
                consAvgDam = consAvgDam * holyDamMult + consCoeff * jotc;

                // Consecration average damage post-resists.
                consAvgDam *= partialResist;

                // Total Consecration DPS
                dpsConsecration = consAvgDam / consCD;
            }

            #endregion


            #region Exorcism
            if (calcOpts.Exorcism)
            {
                float exorCD = 18f, exorCoeff = 0.429f, exorAvgDmg = 0f;

                // Exorcism damage per spell hit
                exorAvgDmg = 665f + exorCoeff * stats.SpellDamageRating;
                exorAvgDmg = exorAvgDmg * holyDamMult + exorCoeff * jotc;

                // Exorcism average damage per cast  **ADD spell crit
                exorAvgDmg *= (spellHitsCrits - spellResist) * partialResist;

                // Total Exorcism DPS
                dpsExorcism = exorAvgDmg / exorCD;
            }
            #endregion


            #region Judgement
            {
                float judgeCD = 9.0f, judgeCoeff = 0.429f, judgeAvgDam = 0.0f;
                float judgeCrit = 1f + (0.03f * (float)calcOpts.Fanaticism) + (stats.CritRating / 22.08f) / 100f;

                if (calcOpts.Seal == 0) judgeAvgDam = 240f;
                else judgeAvgDam = 310f;
                judgeAvgDam = (judgeAvgDam + judgeCoeff * stats.SpellDamageRating) * holyDamMult + judgeCoeff * jotc;

                // Judgement average damage per cast.  JoBlood does not get full resisted.
                if (calcOpts.Seal == 0) judgeAvgDam *= (judgeCrit - spellResist);
                else judgeAvgDam *= judgeCrit;
                judgeAvgDam *= partialResist;

                // Total Judgement DPS
                dpsJudgement = judgeAvgDam / judgeCD;
            }
            #endregion


            calcs.WhiteDPSPoints = dpsWhite;
            calcs.SealDPSPoints = dpsSeal;
            calcs.WFDPSPoints = dpsWindfury;
            calcs.CSDPSPoints = dpsCrusader;
            calcs.ConsDPSPoints = dpsConsecration;
            calcs.ExoDPSPoints = dpsExorcism;
            calcs.JudgementDPSPoints = dpsJudgement;

            calcs.DPSPoints = dpsWhite + dpsSeal + dpsWindfury + dpsCrusader + dpsConsecration + dpsExorcism + dpsJudgement;
            calcs.SubPoints = new float[] { calcs.DPSPoints };
            calcs.OverallPoints = calcs.DPSPoints;
            calcs.BasicStats.WeaponDamage = whiteHit * physDamMult;
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
                        AttackPower = 436f,
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
                        Spirit = 147f,
                        AttackPower = 444f,
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
                        Spirit = 89f,
                        BonusSpiritMultiplier = 0.1f,
                        AttackPower = 442f,
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
                        Spirit = 89f,
                        AttackPower = 446f,
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

            // Libram of Divine Judgement
            if (character.Ranged != null && character.Ranged.Id == 33503 && calcOpts.Seal == 0)
                statsBaseGear.AttackPower += character.Ranged.Stats.JudgementOfCommandAttackPowerBonus;

            // Mongoose Haste% and Executioner moved to GetCharacterCalculations
            // Mongoose  **ASSUMPTION: Mongoose has a 40% uptime
            if (statsEnchants.MongooseProc > 0)
                statsEnchants.Agility += 120f * 0.4f;
                //statsBuffs.HasteRating += (15.76f * 2f) * 0.4f;

            // Executioner
            //if (character.MainHand != null && character.MainHandEnchant != null && character.MainHandEnchant.Id == 3225)
            //{
            //    statsBuffs.ArmorPenetration += 840f * ((40f * (1f / (60f / character.MainHand.Speed)) / 6f));                
            //}

            // Expose Weakness
            if (statsBuffs.ExposeWeakness > 0)
                statsBuffs.AttackPower += calcOpts.ExposeWeaknessAPValue;

            // Drums of War - 60 AP/30 SD with a 25% uptime per drum
            if (statsBuffs.DrumsOfWar > 0)
            {
                statsBuffs.AttackPower += calcOpts.DrumsOfWar * 15f;
                statsBuffs.SpellDamageRating += calcOpts.DrumsOfWar * 7.5f;
            }

            // Drums of Battle - 80 Haste with a 25% uptime per drum
            if (statsBuffs.DrumsOfBattle > 0)
                statsBuffs.HasteRating += calcOpts.DrumsOfBattle * 20f;

            // Ferocious Inspiriation  **Temp fix - FI increases all damage, not just physical damage
            if (character.ActiveBuffsContains("Ferocious Inspiration"))
            {
                statsBuffs.BonusPhysicalDamageMultiplier = ((1f + statsBuffs.BonusPhysicalDamageMultiplier) *
                    (float)Math.Pow(1.03f, calcOpts.FerociousInspiration - 1f)) - 1f;
                statsBuffs.BonusSpellPowerMultiplier = ((1f + statsBuffs.BonusSpellPowerMultiplier) *
                    (float)Math.Pow(1.03f, calcOpts.FerociousInspiration)) - 1f;
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
            statsTotal.AttackPower = (float)Math.Floor((statsRace.AttackPower + statsGearEnchantsBuffs.AttackPower + (statsTotal.Strength - strBase) * 2.0f) * 
                (1f + statsTotal.BonusAttackPowerMultiplier));

            statsTotal.CritRating = statsRace.CritRating + statsGearEnchantsBuffs.CritRating;
            statsTotal.CritRating += statsGearEnchantsBuffs.CritMeleeRating + statsGearEnchantsBuffs.LotPCritRating;
            statsTotal.CritRating += 22.08f * ((statsTotal.Agility - agiBase) / 25f);
			statsTotal.CritRating += 22.08f * ((float)calcOpts.Conviction + (float)calcOpts.SanctifiedSeals);
            statsTotal.HitRating = statsRace.HitRating + statsGearEnchantsBuffs.HitRating;
			statsTotal.HitRating += 15.76f * (float)calcOpts.Precision;
            statsTotal.ArmorPenetration = statsRace.ArmorPenetration + statsGearEnchantsBuffs.ArmorPenetration;
            statsTotal.ExpertiseRating = statsRace.ExpertiseRating + statsGearEnchantsBuffs.ExpertiseRating;
            statsTotal.HasteRating = statsRace.HasteRating + statsGearEnchantsBuffs.HasteRating;
            statsTotal.WeaponDamage = statsGearEnchantsBuffs.WeaponDamage;

            statsTotal.SpellCritRating = statsGearEnchantsBuffs.SpellCritRating;
            statsTotal.SpellCritRating += (22.08f * (float)calcOpts.SanctifiedSeals);
            statsTotal.SpellHitRating = statsGearEnchantsBuffs.SpellHitRating;
            statsTotal.SpellHitRating += (12.62f * (float)calcOpts.Precision);
            statsTotal.SpellDamageRating = statsGearEnchantsBuffs.SpellDamageRating;
            statsTotal.SpellDamageRating += statsGearEnchantsBuffs.SpellDamageFromSpiritPercentage * statsGearEnchantsBuffs.Spirit;

            statsTotal.BonusCritMultiplier = statsGearEnchantsBuffs.BonusCritMultiplier;
            statsTotal.BonusPhysicalDamageMultiplier = statsGearEnchantsBuffs.BonusPhysicalDamageMultiplier;
            statsTotal.BonusSpellPowerMultiplier = statsGearEnchantsBuffs.BonusSpellPowerMultiplier;

            statsTotal.WindfuryAPBonus = statsGearEnchantsBuffs.WindfuryAPBonus;
            statsTotal.Bloodlust = statsGearEnchantsBuffs.Bloodlust;

            statsTotal.ShatteredSunMightProc = statsGearEnchantsBuffs.ShatteredSunMightProc;
            statsTotal.ExecutionerProc = statsGearEnchantsBuffs.ExecutionerProc;
            statsTotal.MongooseProc = statsGearEnchantsBuffs.MongooseProc;
            statsTotal.BonusCrusaderStrikeDamageMultiplier = statsGearEnchantsBuffs.BonusCrusaderStrikeDamageMultiplier;
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
                        new Item() { Stats = new Stats() { ExpertiseRating = 10 } }
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
                Spirit = stats.Spirit,

                AttackPower = stats.AttackPower,
                HitRating = stats.HitRating,
                CritRating = stats.CritRating,
                ArmorPenetration = stats.ArmorPenetration,
                ExpertiseRating = stats.ExpertiseRating,
                HasteRating = stats.HasteRating,
                WeaponDamage = stats.WeaponDamage,

                SpellCritRating = stats.SpellCritRating,
                SpellHitRating = stats.SpellHitRating,
                SpellDamageRating = stats.SpellDamageRating,
                SpellDamageFromSpiritPercentage = stats.SpellDamageFromSpiritPercentage,

                BonusStrengthMultiplier = stats.BonusStrengthMultiplier,
                BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
                BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,
                BonusCritMultiplier = stats.BonusCritMultiplier,
                BonusPhysicalDamageMultiplier = stats.BonusPhysicalDamageMultiplier,
                BonusSpellPowerMultiplier = stats.BonusSpellPowerMultiplier,

                LotPCritRating = stats.LotPCritRating,
                CritMeleeRating = stats.CritMeleeRating,
                WindfuryAPBonus = stats.WindfuryAPBonus,
                Bloodlust = stats.Bloodlust,
                ExposeWeakness = stats.ExposeWeakness,
                DrumsOfBattle = stats.DrumsOfBattle,
                DrumsOfWar = stats.DrumsOfWar,
                ShatteredSunMightProc = stats.ShatteredSunMightProc,
                ExecutionerProc = stats.ExecutionerProc,
                MongooseProc = stats.MongooseProc,

                BonusCrusaderStrikeDamageMultiplier = stats.BonusCrusaderStrikeDamageMultiplier
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            return (stats.Health + stats.Strength + stats.Agility + stats.Stamina + stats.Spirit + stats.AttackPower +
                stats.HitRating + stats.CritRating + stats.ArmorPenetration + stats.ExpertiseRating + stats.HasteRating + stats.WeaponDamage + 
                stats.SpellCritRating + stats.SpellHitRating + stats.SpellDamageRating + stats.SpellDamageFromSpiritPercentage +
                stats.BonusStrengthMultiplier + stats.BonusStaminaMultiplier + stats.BonusAgilityMultiplier + stats.BonusCritMultiplier +
                stats.BonusAttackPowerMultiplier + stats.BonusPhysicalDamageMultiplier + stats.BonusSpellPowerMultiplier +
                stats.CritMeleeRating + stats.LotPCritRating + stats.WindfuryAPBonus + stats.Bloodlust + stats.ExposeWeakness +
                stats.DrumsOfBattle + stats.DrumsOfWar + stats.ShatteredSunMightProc + stats.ExecutionerProc + stats.MongooseProc +
                stats.BonusCrusaderStrikeDamageMultiplier) != 0;
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
