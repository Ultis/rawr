using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Retribution
{
	[Rawr.Calculations.RawrModelInfo("Retribution", "Spell_Holy_AuraOfLight", Character.CharacterClass.Paladin)]
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
					    "Basic Stats:Strength",
					    "Basic Stats:Agility",
					    "Basic Stats:Attack Power",
					    "Basic Stats:Crit Rating",
					    "Basic Stats:Hit Rating",
					    "Basic Stats:Expertise",
					    "Basic Stats:Haste Rating",
					    "Basic Stats:Armor Penetration",
					    "Advanced Stats:Weapon Damage*Damage before misses and mitigation",
					    "Advanced Stats:Attack Speed",
					    "Advanced Stats:Crit Chance",
					    "Advanced Stats:Avoided Attacks",
					    "Advanced Stats:Enemy Mitigation",
                        "DPS Breakdown:White",
                        "DPS Breakdown:Seal",
                        "DPS Breakdown:Windfury",
					    "DPS Breakdown:Crusader Strike",
                        "DPS Breakdown:Judgement",
                        "DPS Breakdown:Consecration",
                        "DPS Breakdown:Exorcism",
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
            return new ComparisonCalculationRetribution();
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
            GetTalents(character);
            Stats stats = GetCharacterStats(character, additionalItem);

            CharacterCalculationsRetribution calcs = new CharacterCalculationsRetribution();
            calcs.BasicStats = stats;
            calcs.ActiveBuffs = new List<Buff>(character.ActiveBuffs);

            #region Damage Multipliers
            float twoHandedSpec = 1f + (0.02f * (float)calcOpts.TwoHandedSpec);
            float impSancAura = 1f + 0.01f * (float)calcOpts.ImprovedSanctityAura;
            float crusade = 1f + 0.01f * (float)calcOpts.Crusade;
            float vengeance = 1f + 0.03f * (float)calcOpts.Vengeance;
            float sancAura = 1f + 0.1f * (float)calcOpts.SanctityAura;
            float spellPower = 1f + stats.BonusSpellPowerMultiplier; // Covers all % spell damage increases.  Misery, FI.
            float physPower = 1f + stats.BonusPhysicalDamageMultiplier; // Covers all % physical damage increases.  Blood Frenzy, FI.
            float partialResist = 0.953f; // Average of 4.7% damage lost to partial resists on spells

            float physCritMult = 1f + stats.BonusCritMultiplier * 2f;
            float spellCritMult = 1f + stats.BonusCritMultiplier * 3f;
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
            #endregion

            float dpsWhite = 0f, dpsSeal = 0f, dpsWindfury = 0f, dpsCrusader = 0f, dpsJudgement = 0f, dpsConsecration = 0f, dpsExorcism = 0f;
            float baseSpeed = 1f, hastedSpeed = 1f, baseDamage = 0f, mitigation = 1f;
            float whiteHit = 0f, physCrits = 0f, totalMiss = 0f, spellCrits = 0f, spellResist = 0f;
            float chanceToGlance = 0.25f, glancingAmount = 0.35f;

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
                mitigation = 1f - (targetArmor / (targetArmor + 10557.5f));

                // Executioner enchant.  ASSUMPTION: Executioner has a 40% uptime.
                if (stats.ExecutionerProc > 0)
                {
                    float exeArmor = targetArmor, exeMitigation = 1.0f, exeUptime = 0.4f, exeArmorPen = 840f;

                    // Find mitigation while Executioner is up
                    exeArmor = targetArmor - exeArmorPen;
                    if (exeArmor < 0) exeArmor = 0f;
                    exeMitigation = 1f - (exeArmor / (exeArmor + 10557.5f));

                    // Weighted average of mitigation with and without Executioner, based on Executioner uptime
                    mitigation = (exeMitigation * exeUptime) + (mitigation * (1 - exeUptime));
                }
            }
            #endregion

            #region Crits, Misses, Resists
            {
                // Crit: Base .65%
                physCrits = .0065f;
                physCrits += stats.CritRating / 2208f;
                physCrits += stats.Agility / 2500f;
                physCrits += stats.Crit;

                // Dodge: Base 6.5%, Minimum 0%
                float chanceDodged = .065f;
                //chanceDodged -= stats.ExpertiseRating / 1576f;
                chanceDodged -= stats.Expertise * .0025f;
                if (chanceDodged < 0f) chanceDodged = 0f;
                calcs.DodgedAttacks = chanceDodged;

                // Miss: Base 9%, Minimum 0%
                float chanceMiss = .09f;
                chanceMiss -= stats.HitRating / 1576f;
                chanceMiss -= stats.Hit;
                if (chanceMiss < 0f) chanceMiss = 0f;
                calcs.MissedAttacks = chanceMiss;

                // Spell Crit: Base 3.26%  **TODO: include intellect
                spellCrits = .0326f;
                spellCrits += stats.SpellCritRating / 2208f;
                spellCrits += stats.SpellCrit;

                // Resists: Base 17%, Minimum 1%
                spellResist = .17f;
                spellResist -= stats.SpellHitRating / 1262f;
                spellResist -= stats.Hit;
                if (spellResist < .01f) spellResist = .01f;

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
                            dpsSSO *= 1f + physCrits * physCritMult - totalMiss;
                            dpsSSO /= 50;  // 50 seconds between procs
                            break;
                    }
                }
                #endregion

                // White damage per hit.  Basic white hits are use elsewhere.
                whiteHit = baseDamage + (stats.AttackPower / 14.0f) * baseSpeed;
                whiteAvgDam = whiteHit * physDamMult;

                // Average white damage per swing
                whiteAvgDam *= (1f + physCrits * physCritMult - totalMiss - chanceToGlance * glancingAmount) * mitigation;

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
                sealAvgDam *= (1f + physCrits * physCritMult - totalMiss) * partialResist;

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
                windAvgDam *= (1f + physCrits * physCritMult - totalMiss) * mitigation;

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
                crusAvgDam *= (1f + physCrits * physCritMult - totalMiss) * mitigation;

                // Total Crusader Strike DPS
                dpsCrusader = crusAvgDam / crusCD;
            }
            #endregion

            #region Judgement
            {
                float judgeCD = 9.0f, judgeCoeff = 0.429f, judgeAvgDam = 0.0f;
                float judgeCrit = physCrits + (0.03f * (float)calcOpts.Fanaticism);

                if (calcOpts.Seal == 0) judgeAvgDam = 240f;
                else judgeAvgDam = 310f;
                judgeAvgDam = (judgeAvgDam + judgeCoeff * stats.SpellDamageRating) * holyDamMult + judgeCoeff * jotc;

                // Judgement average damage per cast.  JoBlood does not get full resisted.
                if (calcOpts.Seal == 0) judgeAvgDam *= (1f + judgeCrit * physCritMult - spellResist);
                else judgeAvgDam *= 1f + judgeCrit * physCritMult;
                judgeAvgDam *= partialResist;

                // Total Judgement DPS
                dpsJudgement = judgeAvgDam / judgeCD;
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

                // Exorcism average damage per cast
                exorAvgDmg *= (1f + (spellCrits / 2f) * spellCritMult - spellResist) * partialResist;

                // Total Exorcism DPS
                dpsExorcism = exorAvgDmg / exorCD;
            }
            #endregion


            calcs.WeaponDamage = whiteHit * physDamMult;
            calcs.AttackSpeed = hastedSpeed;
            calcs.CritChance = physCrits;
            calcs.AvoidedAttacks = totalMiss;
            calcs.EnemyMitigation = 1 - mitigation;

            calcs.WhiteDPS = dpsWhite;
            calcs.SealDPS = dpsSeal;
            calcs.WindfuryDPS = dpsWindfury;
            calcs.CrusaderDPS = dpsCrusader;
            calcs.JudgementDPS = dpsJudgement;
            calcs.ConsecrationDPS = dpsConsecration;
            calcs.ExorcismDPS = dpsExorcism;

            calcs.DPSPoints = dpsWhite + dpsSeal + dpsWindfury + dpsCrusader + dpsJudgement + dpsConsecration + dpsExorcism;
            calcs.OverallPoints = calcs.DPSPoints;
            return calcs;
        }


        private Stats GetRaceStats(Character character)
        {
            Stats statsRace;
            switch (character.Race)
            {
                case Character.CharacterRace.BloodElf:
                    statsRace = new Stats()
                    { Strength = 123f, Agility = 79f, Stamina = 118f, Intellect = 87f, Spirit = 88f };
                    break;
                case Character.CharacterRace.Draenei: // Relevant racials: +1% hit
                    statsRace = new Stats()
                    { Strength = 127f, Agility = 74f, Stamina = 119f, Intellect = 84f, Spirit = 89f, Hit = .01f };
                    break;
                case Character.CharacterRace.Human: // Relevant racials: +10% spirit, +5 expertise when wielding mace or sword
                    statsRace = new Stats()
                    { Strength = 126f, Agility = 77f, Stamina = 120f, Intellect = 83f, Spirit = 89f, BonusSpiritMultiplier = 0.1f, };
                    //Expertise for Humans
                    if (character.MainHand != null && (character.MainHand.Type == Item.ItemType.TwoHandMace || character.MainHand.Type == Item.ItemType.TwoHandSword))
                        statsRace.Expertise = 5f;
                    break;
                case Character.CharacterRace.Dwarf:
                    statsRace = new Stats()
                    { Strength = 128f, Agility = 73f, Stamina = 120f, Intellect = 83f, Spirit = 89f, };
                    break;
                default:
                    statsRace = new Stats();
                    break;
            }
            // Derived stats base amount, common to all races
            statsRace.AttackPower = 190f;
            statsRace.Health = 3197f;
            statsRace.Mana = 2673f;

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
            CalculationOptionsRetribution calcOpts = character.CalculationOptions as CalculationOptionsRetribution;
           
            Stats statsRace = GetRaceStats(character);
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
            Stats statsTalents = new Stats()
            {
                Crit = .01f * ((float)calcOpts.Conviction + (float)calcOpts.SanctifiedSeals),
                Hit = .01f * (float)calcOpts.Precision,
                SpellCrit = .01f * (float)calcOpts.SanctifiedSeals,
                BonusStrengthMultiplier = .02f * (float)calcOpts.DivineStrength
            };
            Stats statsTotal = new Stats();
            Stats statsGearEnchantsBuffs;

            // Libram of Divine Judgement
            if (character.Ranged != null && character.Ranged.Id == 33503 && calcOpts.Seal == 0)
                statsBaseGear.AttackPower += character.Ranged.Stats.JudgementOfCommandAttackPowerBonus;

            // Mongoose  **ASSUMPTION: Mongoose has a 40% uptime
            if (statsEnchants.MongooseProc > 0)
                statsEnchants.Agility += 120f * 0.4f;

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

            statsTotal.Health = (float)Math.Floor(statsGearEnchantsBuffs.Health + (statsTotal.Stamina * 10f));
            statsTotal.Mana = (float)Math.Floor(statsGearEnchantsBuffs.Mana + (statsTotal.Intellect * 15f));
            statsTotal.AttackPower = (float)Math.Floor((statsGearEnchantsBuffs.AttackPower + statsTotal.Strength * 2) * (1f + statsTotal.BonusAttackPowerMultiplier));

            statsTotal.Crit = statsGearEnchantsBuffs.Crit;
            statsTotal.CritRating = statsGearEnchantsBuffs.CritRating;
            statsTotal.CritRating += statsGearEnchantsBuffs.CritMeleeRating + statsGearEnchantsBuffs.LotPCritRating;
            statsTotal.Hit = statsGearEnchantsBuffs.Hit;
            statsTotal.HitRating = statsGearEnchantsBuffs.HitRating;
            statsTotal.ArmorPenetration = statsGearEnchantsBuffs.ArmorPenetration;
            statsTotal.Expertise = statsGearEnchantsBuffs.Expertise;
            statsTotal.Expertise += (float)Math.Floor(statsGearEnchantsBuffs.ExpertiseRating / 3.94);
            statsTotal.HasteRating = statsGearEnchantsBuffs.HasteRating;
            statsTotal.WeaponDamage = statsGearEnchantsBuffs.WeaponDamage;

            statsTotal.SpellCrit = statsGearEnchantsBuffs.SpellCrit;
            statsTotal.SpellCritRating = statsGearEnchantsBuffs.SpellCritRating;
            statsTotal.SpellHitRating = statsGearEnchantsBuffs.SpellHitRating;
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
            CharacterCalculationsRetribution baseCalc, calc;
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
                        new Item() { Stats = new Stats() { SpellDamageRating = 11.7f } },
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

                    baseCalc = GetCharacterCalculations(character) as CharacterCalculationsRetribution;

                    for (int index = 0; index < itemList.Length; index++)
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
                Intellect = stats.Intellect,
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
        public void GetTalents(Character character)
        {
            CalculationOptionsRetribution calcOpts = character.CalculationOptions as CalculationOptionsRetribution;
            if (!calcOpts.TalentsSaved)
            {
				//calcOpts.Precision = character.PaladinTalents.Precision; //TODO: Talent Removed in 3.0
				calcOpts.Crusade = character.PaladinTalents.Crusade;
				calcOpts.TwoHandedSpec = character.PaladinTalents.TwoHandedWeaponSpecialization;
				//calcOpts.SanctityAura = character.PaladinTalents.SanctityAura; //TODO: Talent Removed in 3.0
				//calcOpts.ImprovedSanctityAura = character.PaladinTalents.ImprovedSanctityAura; //TODO: Talent Removed in 3.0
				calcOpts.SanctifiedSeals = character.PaladinTalents.SanctifiedSeals;
				calcOpts.Fanaticism = character.PaladinTalents.Fanaticism;
				calcOpts.Vengeance = character.PaladinTalents.Vengeance;
				calcOpts.Conviction = character.PaladinTalents.Conviction;
				calcOpts.DivineStrength = character.PaladinTalents.DivineStrength;
				calcOpts.TalentsSaved = true;
            }
        }
    }
}
