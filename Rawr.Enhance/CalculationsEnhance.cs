using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Globalization;
using Rawr.Enhance;

namespace Rawr
{
    [Rawr.Calculations.RawrModelInfo("Enhance", "inv_jewelry_talisman_04", Character.CharacterClass.Shaman)]
	public class CalculationsEnhance : CalculationsBase
    {
        #region Gemming Template
        private List<GemmingTemplate> _defaultGemmingTemplates = null;
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                //Meta
                int chaotic = 41285;
                int relentless = 41398;

                if (_defaultGemmingTemplates == null)
                {
                    Gemming gemming = new Gemming();
                    _defaultGemmingTemplates = new List<GemmingTemplate>();
                    _defaultGemmingTemplates.AddRange(gemming.addTemplates("Uncommon", 0, relentless, false));
                    _defaultGemmingTemplates.AddRange(gemming.addTemplates("Uncommon", 0, chaotic, false));
                    _defaultGemmingTemplates.AddRange(gemming.addTemplates("Rare", 1, relentless, true)); // Enable Rare gems by default
                    _defaultGemmingTemplates.AddRange(gemming.addTemplates("Rare", 1, chaotic, true));    // Enable Rare gems by default
                    _defaultGemmingTemplates.AddRange(gemming.addTemplates("Epic", 2, relentless, false));
                    _defaultGemmingTemplates.AddRange(gemming.addTemplates("Epic", 2, chaotic, false));
                    _defaultGemmingTemplates.AddRange(gemming.addJewelerTemplates(relentless, false));
                    _defaultGemmingTemplates.AddRange(gemming.addJewelerTemplates(chaotic, false));
                }
                return _defaultGemmingTemplates;
            }
        }
        #endregion

        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
		public override CalculationOptionsPanelBase CalculationOptionsPanel
		{
			get
			{
				if (_calculationOptionsPanel == null)
				{
					_calculationOptionsPanel = new CalculationOptionsPanelEnhance();
				}
				return _calculationOptionsPanel;
			}
        }

        #region Display labels
        private string[] _characterDisplayCalculationLabels = null;
		public override string[] CharacterDisplayCalculationLabels
		{
			get
			{
				if (_characterDisplayCalculationLabels == null)
					_characterDisplayCalculationLabels = new string[] {
                    "Summary:DPS Points*Your total expected DPS with this kit and selected glyphs and buffs",
                    "Summary:Survivability Points*Assumes basic 2% of total health as Survivability",
                    "Summary:Overall Points*This is the sum of Total DPS and Survivability. If you want sort items by DPS only select DPS from the sort dropdown top right",
					"Basic Stats:Health",
                    "Basic Stats:Mana",
					"Basic Stats:Attack Power",
					"Basic Stats:Agility",
					"Basic Stats:Intellect",
                    "Basic Stats:Strength",
                    "Basic Stats:Yellow Hit",
                    "Basic Stats:Spell Hit",
					"Basic Stats:White Hit",
                    "Basic Stats:Melee Crit",
                    "Basic Stats:Spell Crit",
                    "Basic Stats:Spellpower",
					"Basic Stats:Total Expertise",
   				    "Basic Stats:Hit Rating",
					"Basic Stats:Haste Rating",
					"Basic Stats:Armour Pen Rating",
					"Complex Stats:Avoided Attacks*The percentage of your attacks that fail to land.",
					"Complex Stats:Avg MH Speed",
                    "Complex Stats:Avg OH Speed",
					"Complex Stats:Armor Mitigation",
                    "Complex Stats:UR Uptime*Unleashed Rage Uptime percentage.",
                    "Complex Stats:Flurry Uptime",
                    "Complex Stats:ED Uptime*Elemental Devastation Uptime percentage",
                    "Complex Stats:Avg Time to 5 Stack*Average time it takes to get 5 stacks of Maelstrom Weapon.",
                    "Attacks:White Damage",
                    "Attacks:Windfury Attack",
                    "Attacks:Flametongue Attack",
                    "Attacks:Stormstrike",
                    "Attacks:Lava Lash",
                    "Attacks:Searing/Magma Totem",
                    "Attacks:Earth Shock",
                    "Attacks:Lightning Bolt",
                    "Attacks:Lightning Shield",
                    "Attacks:Spirit Wolf",
                    "Attacks:Total DPS",
                    "Module Version:Enhance Version"
				};
				return _characterDisplayCalculationLabels;
			}
		}

		private string[] _optimizableCalculationLabels = null;
		public override string[] OptimizableCalculationLabels
		{
			get
			{
				if (_optimizableCalculationLabels == null)
					_optimizableCalculationLabels = new string[] {
					"Health"
					};
				return _optimizableCalculationLabels;
			}
		}

		private string[] _customChartNames = null;
		public override string[] CustomChartNames
		{
			get
			{
				if (_customChartNames == null)
					_customChartNames = new string[] {
					"Combat Table (White)",
					"Combat Table (Yellow)",
					"Relative Stat Values"
					};
				return _customChartNames;
			}
        }
        #endregion 

        private Dictionary<string, System.Drawing.Color> _subPointNameColors = null;
		public override Dictionary<string, System.Drawing.Color> SubPointNameColors
		{
			get
			{
				if (_subPointNameColors == null)
				{
					_subPointNameColors = new Dictionary<string, System.Drawing.Color>();
					_subPointNameColors.Add("DPS", System.Drawing.Color.FromArgb(160, 0, 224));
					_subPointNameColors.Add("Survivability", System.Drawing.Color.FromArgb(64, 128, 32));
				}
				return _subPointNameColors;
			}
		}

		private List<Item.ItemType> _relevantItemTypes = null;
		public override List<Item.ItemType> RelevantItemTypes
		{
			get
			{
				if (_relevantItemTypes == null)
				{
					_relevantItemTypes = new List<Item.ItemType>(new Item.ItemType[]
					{
						Item.ItemType.None,
                        Item.ItemType.Cloth,
						Item.ItemType.Leather,
                        Item.ItemType.Mail,
						Item.ItemType.Totem,
					//	Item.ItemType.Staff,
					//	Item.ItemType.TwoHandMace, // Removed two handed options so as not to screw up recommendations
                    //  Item.ItemType.TwoHandAxe,  // Two handers are simply NOT viable for Enhancement Shamans
                        Item.ItemType.Dagger,
                        Item.ItemType.OneHandAxe,
                        Item.ItemType.OneHandMace,
                        Item.ItemType.FistWeapon
					});
				}
				return _relevantItemTypes;
			}
		}

		public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.Shaman; } }
		public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationEnhance(); }
		public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsEnhance(); }

		public override ICalculationOptionBase DeserializeDataObject(string xml)
		{
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsEnhance));
			System.IO.StringReader reader = new System.IO.StringReader(xml);
			CalculationOptionsEnhance calcOpts = serializer.Deserialize(reader) as CalculationOptionsEnhance;
			return calcOpts;
        }

        #region Main Calculations
        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange)
        {
            #region Applied Stats
            //_cachedCharacter = character;
			CalculationOptionsEnhance calcOpts = character.CalculationOptions as CalculationOptionsEnhance;
            int targetLevel = calcOpts.TargetLevel;
            float targetArmor = calcOpts.TargetArmor;
            float bloodlustUptime = calcOpts.BloodlustUptime;
            Stats stats = GetCharacterStats(character, additionalItem);
            Stats statsRace = GetRaceStats(character);
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            CharacterCalculationsEnhance calculatedStats = new CharacterCalculationsEnhance();
            calculatedStats.BasicStats = stats;
            calculatedStats.BaseStats = ApplyTalents(character, statsRace + statsBaseGear);
            calculatedStats.BuffStats = GetBuffsStats(character.ActiveBuffs);
            calculatedStats.TargetLevel = targetLevel;
            calculatedStats.ActiveBuffs = new List<Buff>(character.ActiveBuffs);
            
            //Set up some talent variables
            float initialAP = stats.AttackPower;
            int TS = character.ShamanTalents.ThunderingStrikes;
            int DWS = character.ShamanTalents.DualWieldSpecialization;
            float shockSpeed = 6f - (.2f * character.ShamanTalents.Reverberation);
            float concussionMultiplier = 1f + .01f * character.ShamanTalents.Concussion;
            float staticShockChance = .02f * character.ShamanTalents.StaticShock;
            float shieldBonus = 1f + .05f * character.ShamanTalents.ImprovedShields;
            float callofFlameBonus = 1f + .05f * character.ShamanTalents.CallOfFlame;
            float windfuryWeaponBonus = 1250f + stats.TotemWFAttackPower;
            float callOfThunder = .05f * character.ShamanTalents.CallOfThunder;
            switch (character.ShamanTalents.ElementalWeapons){
                case 1:
                    windfuryWeaponBonus += windfuryWeaponBonus * .13f;
                    break;
                case 2:
                    windfuryWeaponBonus += windfuryWeaponBonus * .27f;
                    break;
                case 3:
                    windfuryWeaponBonus += windfuryWeaponBonus * .4f;
                    break;
            }
            float flurryHasteBonus = .05f * character.ShamanTalents.Flurry + stats.BonusFlurryHaste;
            float edCritBonus = .03f * character.ShamanTalents.ElementalDevastation;
            float critMultiplierMelee = 2f + stats.BonusCritMultiplier;
            float critMultiplierSpell = 1.5f + .1f * character.ShamanTalents.ElementalFury + stats.BonusSpellCritMultiplier;
            float mwPPM = 2 * character.ShamanTalents.MaelstromWeapon * (1 + stats.BonusMWFreq);
            int stormstrikeSpeed = 8;
            float weaponMastery = 1f;
            switch (character.ShamanTalents.WeaponMastery){
                case 1:
                    weaponMastery = 1.04f;
                    break;
                case 2:
                    weaponMastery = 1.07f;
                    break;
                case 3:
                    weaponMastery = 1.1f;
                    break;
            }
            float unleashedRage = 0f;
            switch (character.ShamanTalents.UnleashedRage){
                case 1:
                    unleashedRage = .04f;
                    break;
                case 2:
                    unleashedRage = .07f;
                    break;
                case 3:
                    unleashedRage = .1f;
                    break;
            }
            //gear stuff
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

            float spellCritModifier = stats.SpellCrit;
            if (calcOpts.MainhandImbue == "Flametongue")
            {
                spellCritModifier += character.ShamanTalents.GlyphofFlametongueWeapon ? .02f : 0f;
                stats.SpellPower += (float)Math.Floor(211f * (1 + character.ShamanTalents.ElementalWeapons * .1f));
            }
            if (calcOpts.OffhandImbue == "Flametongue" && character.ShamanTalents.DualWield == 1)
            {
                spellCritModifier += character.ShamanTalents.GlyphofFlametongueWeapon ? .02f : 0f;
                stats.SpellPower += (float)Math.Floor(211f * (1 + character.ShamanTalents.ElementalWeapons * .1f));
            }
            
            //totem procs
            stats.HasteRating += stats.LightningBoltHasteProc_15_45 * 10f / 55f; // exact copy of Elemental usage for totem (relic)
            stats.HasteRating += stats.TotemSSHaste * 6f / stormstrikeSpeed;
            stats.SpellPower += stats.TotemShockSpellPower;
            stats.AttackPower += stats.TotemLLAttackPower + stats.TotemShockAttackPower;

            //trinket procs
            if (stats.GreatnessProc > 0)
            {
                float expectedAgi = (float)Math.Floor(stats.Agility * (1 + stats.BonusAgilityMultiplier));
                float expectedStr = (float)Math.Floor(stats.Strength * (1 + stats.BonusStrengthMultiplier));
                float expectedInt = (float)Math.Floor(stats.Intellect * (1 + stats.BonusIntellectMultiplier));
                // Highest stat
                if (expectedAgi > expectedStr)
                {
                    if (expectedAgi > expectedInt)
                    {
                        stats.Agility += stats.GreatnessProc * 15f / 47f;  // proc calc lifted from Rawr.Cat odd that its 47sec CD??
                        stats.AttackPower += stats.GreatnessProc * 15f / 47f;
                    }
                    else
                    {
                        stats.Intellect += stats.GreatnessProc * 15f / 47f;
                        stats.AttackPower += AddAPFromStrAgiInt(character, 0, 0, stats.GreatnessProc * 15f / 47f);
                    }
                }
                else
                {
                    if (expectedAgi > expectedInt)
                    {
                        stats.Strength += stats.GreatnessProc * 15f / 47f;
                        stats.AttackPower += stats.GreatnessProc * 15f / 47f;
                    }
                    else
                    {
                        stats.Intellect += stats.GreatnessProc * 15f / 47f;
                        stats.AttackPower += AddAPFromStrAgiInt(character, 0, 0, stats.GreatnessProc * 15f / 47f);
                    }
                }
            }
            stats.HasteRating += stats.HasteRatingOnPhysicalAttack * 10 / 45; // Haste trinket (Meteorite Whetstone/Dragonspine Trophy)
            stats.HasteRating += stats.HasteRatingFor20SecOnUse2Min * 20f / 120f;
            stats.HasteRating += stats.SpellHasteFor10SecOnCast_10_45 * 10f / 45f;
            stats.SpellPower += stats.SpellPowerFor10SecOnCast_15_45 * 10f / 45f;
            stats.SpellPower += stats.SpellPowerFor10SecOnHit_10_45 * 10f / 45f;
            // Finally make sure to add in the spellpower from MQ gained from all the bonus AP added in this section
            stats.SpellPower += character.ShamanTalents.MentalQuickness * .1f * (stats.AttackPower - initialAP);
            #endregion

            ////////////////////////////
            // Main calculation Block //
            ////////////////////////////

			#region Damage Model
            float damageReduction = ArmorCalculations.GetDamageReduction(character.Level, targetArmor, stats.ArmorPenetration, stats.ArmorPenetrationRating);
            float attackPower = stats.AttackPower;
            float hitBonus = stats.PhysicalHit + StatConversion.GetHitFromRating(stats.HitRating);
            float expertiseBonus = 0.0025f * (stats.Expertise + StatConversion.GetExpertiseFromRating(stats.ExpertiseRating));
            float glancingRate = 0.24f;

            float meleeCritModifier = stats.PhysicalCrit;
            float baseMeleeCrit = StatConversion.GetCritFromRating(stats.CritMeleeRating + stats.CritRating) + StatConversion.GetCritFromAgility(stats.Agility, character.Class) + .01f * TS;
            float chanceCrit = Math.Min(0.75f, (1 + stats.BonusCritChance) * (baseMeleeCrit + meleeCritModifier) + .00005f); //fudge factor for rounding
            float chanceDodge = Math.Max(0f, 0.065f - expertiseBonus);
            float chanceWhiteMiss = Math.Max(0f, 0.28f - hitBonus - .02f * DWS) + chanceDodge;
            float chanceYellowMiss = Math.Max(0f, 0.08f - hitBonus - .02f * DWS) + chanceDodge; // base miss 8% now

            float hitBonusSpell = stats.SpellHit + StatConversion.GetSpellHitFromRating(stats.HitRating);
            float chanceSpellMiss = Math.Max(0f, .17f - hitBonusSpell);
            float baseSpellCrit = StatConversion.GetSpellCritFromRating(stats.SpellCritRating + stats.CritRating) + StatConversion.GetSpellCritFromIntellect(stats.Intellect) + .01f * TS;
            float chanceSpellCrit = Math.Min(0.75f, (1 + stats.BonusCritChance) * (baseSpellCrit + spellCritModifier) + .00005f); //fudge factor for rounding
            float spellDamage = stats.SpellPower * (1 + stats.BonusSpellPowerMultiplier);
            float bonusSpellDamage = stats.BonusDamageMultiplier;
            float bonusPhysicalDamage = (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusPhysicalDamageMultiplier) - 1f;
            float bonusFireDamage = (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusFireDamageMultiplier) - 1f;
            float bonusNatureDamage = (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusNatureDamageMultiplier) - 1f;
            float bonusLSDamage = stats.BonusLSDamage; // 2 piece T7 set bonus
            float chanceWhiteCrit = Math.Min(chanceCrit, 1f - glancingRate - chanceWhiteMiss);
            float chanceYellowCrit = Math.Min(chanceCrit, 1f - chanceYellowMiss);

            float hasteBonus = StatConversion.GetHasteFromRating(stats.HasteRating, character.Class);
            float unhastedMHSpeed = character.MainHand == null ? 3.0f : character.MainHand.Item.Speed;
            float wdpsMH = character.MainHand == null ? 46.3f : character.MainHand.Item.DPS;
            float unhastedOHSpeed = character.OffHand == null ? 3.0f : character.OffHand.Item.Speed;
            float wdpsOH = character.OffHand == null ? 46.3f : character.OffHand.Item.DPS;

            float baseHastedMHSpeed = unhastedMHSpeed / (1f + hasteBonus) / (1f + stats.PhysicalHaste);
            float baseHastedOHSpeed = unhastedOHSpeed / (1f + hasteBonus) / (1f + stats.PhysicalHaste);

            //XXX: Only MH WF for now
            float chanceToProcWFPerHit = .2f + (character.ShamanTalents.GlyphofWindfuryWeapon ? .02f : 0f);
            float avgHitsToProcWF = 1 / chanceToProcWFPerHit;

            //The Swing Loop
            //This is where we figure out feedback systems -- WF, MW, ED, Flurry, etc.
            //It's also where we'll figure out GCD interference when we model that.
            //--------------
            float flurryUptime = 1f;
            float edUptime = 0f;
            float urUptime = 0f;
            float bloodlustHaste = 1 + (bloodlustUptime * stats.Bloodlust);
            float hastedMHSpeed = baseHastedMHSpeed / bloodlustHaste;
            float hastedOHSpeed = baseHastedOHSpeed / bloodlustHaste;
            float hitsPerSMHSS = (1f - chanceYellowMiss) / stormstrikeSpeed;
            float hitsPerSOHSS = character.ShamanTalents.DualWield == 1 ? ((1f - 2 * chanceYellowMiss) / stormstrikeSpeed) : 0f; //OH only swings if MH connects
            float hitsPerSLL = (1f - chanceYellowMiss) / 6f;
            float swingsPerSMHMelee = 0f;
            float swingsPerSOHMelee = 0f;
            float wfProcsPerSecond = 0f;
            float mwProcsPerSecond = 0f;
            float secondsToFiveStack = 10f;
            float averageMeleeCritChance = chanceYellowCrit;
            float earthShocksPerS = (1f - chanceSpellMiss) / shockSpeed;
            float couldCritSwingsPerSecond = 0f;
            float hitsPerSOH = 0f;
            float hitsPerSMH = 0f;
            float hitsPerSWF = 0f;
            for (int i = 0; i < 5; i++)
            {
                float bonusHaste = (1f + (flurryUptime * flurryHasteBonus)) * bloodlustHaste;
                hastedMHSpeed = baseHastedMHSpeed / bonusHaste;
                hastedOHSpeed = baseHastedOHSpeed / bonusHaste;
                swingsPerSMHMelee = 1f / hastedMHSpeed;
                swingsPerSOHMelee = 1f / hastedOHSpeed;
                //Flat Windfury Society
                float hitsThatProcWFPerS = (1f - chanceWhiteMiss) * swingsPerSMHMelee + hitsPerSMHSS;
                float windfuryTimeToFirstHit = hastedMHSpeed - (3 % hastedMHSpeed);
                //later -- //windfuryTimeToFirstHit = hasted
                wfProcsPerSecond = 1f / (3f + windfuryTimeToFirstHit + ((avgHitsToProcWF - 1) * hitsThatProcWFPerS));
                hitsPerSWF = 2f * wfProcsPerSecond * (1f - chanceYellowMiss);

                //Due to attack table, a white swing has the same chance to crit as a yellow hit
                couldCritSwingsPerSecond = swingsPerSMHMelee + swingsPerSOHMelee + hitsPerSMHSS + hitsPerSOHSS + hitsPerSLL + hitsPerSWF;
                float swingsThatConsumeFlurryPerSecond = swingsPerSMHMelee + swingsPerSOHMelee;
                flurryUptime = 1f - (float)Math.Pow(1 - averageMeleeCritChance, (3 / swingsThatConsumeFlurryPerSecond) * couldCritSwingsPerSecond);

                hitsPerSMH = swingsPerSMHMelee * (1f - chanceWhiteMiss - chanceDodge) + hitsPerSWF + hitsPerSMHSS;
                if (character.ShamanTalents.DualWield == 1)
                    hitsPerSOH = swingsPerSOHMelee * (1f - chanceWhiteMiss - chanceDodge) + hitsPerSOHSS + hitsPerSLL;
                mwProcsPerSecond = (mwPPM / (60f / unhastedMHSpeed)) * hitsPerSMH + (mwPPM / (60f / unhastedOHSpeed)) * hitsPerSOH;
                secondsToFiveStack /* oh but i want it now! */ = 5 / mwProcsPerSecond;

                float couldCritSpellsPerS = (earthShocksPerS + 1 / secondsToFiveStack) * (1f - chanceSpellMiss);
                edUptime = 1f - (float)Math.Pow(1-chanceSpellCrit, 10 * couldCritSpellsPerS);
                
                averageMeleeCritChance = chanceYellowCrit + edUptime * edCritBonus;
            }
            urUptime = 1f - (float)Math.Pow(1 - averageMeleeCritChance, 10 * couldCritSwingsPerSecond);
            attackPower += attackPower * unleashedRage * urUptime;
            float yellowAttacksPerSecond = hitsPerSWF + hitsPerSMHSS + (character.ShamanTalents.DualWield == 1 ? hitsPerSOHSS : 0f);
                    
            if (stats.MongooseProc > 0 | stats.BerserkingProc > 0)
            {
                if (character.MainHandEnchant != null)
                {
                    float whiteAttacksPerSecond = swingsPerSMHMelee * (1f - chanceWhiteMiss - chanceDodge);
                    if (character.MainHandEnchant.Id == 2673) // Mongoose Enchant
                    {
                        float timeBetweenMongooseProcs = 60f / (whiteAttacksPerSecond + yellowAttacksPerSecond);
                        float mongooseUptime = 15f / timeBetweenMongooseProcs;
                        float mongooseAgility = 120f * mongooseUptime * (1 + stats.BonusAgilityMultiplier);
                        chanceCrit = Math.Min(0.75f, chanceCrit + StatConversion.GetCritFromAgility(mongooseAgility, character.Class));
                        attackPower += mongooseAgility * (1 + stats.BonusAttackPowerMultiplier);
                        baseHastedMHSpeed /= 1f + (0.02f * mongooseUptime);
                    }
                    if (character.MainHandEnchant.Id == 3789) // Berserker Enchant
                    {
                        float timeBetweenBerserkingProcs = 45f / (whiteAttacksPerSecond + yellowAttacksPerSecond);
                        float berserkingUptime = 15f / timeBetweenBerserkingProcs;
                        attackPower += 400f * berserkingUptime * (1 + stats.BonusAttackPowerMultiplier);
                    }
                }
                if (character.OffHandEnchant != null && character.ShamanTalents.DualWield == 1)
                {
                    float whiteAttacksPerSecond = swingsPerSOHMelee * (1f - chanceWhiteMiss - chanceDodge);
                    if (character.OffHandEnchant.Id == 2673)  // Mongoose Enchant
                    {
                        float timeBetweenMongooseProcs = 60f / (whiteAttacksPerSecond + yellowAttacksPerSecond);
                        float mongooseUptime = 15f / timeBetweenMongooseProcs;
                        float mongooseAgility = 120f * mongooseUptime * (1 + stats.BonusAgilityMultiplier);
                        chanceCrit = Math.Min(0.75f, chanceCrit + StatConversion.GetCritFromAgility(mongooseAgility, character.Class));
                        attackPower += mongooseAgility * (1 + stats.BonusAttackPowerMultiplier);
                        baseHastedOHSpeed /= 1f + (0.02f * mongooseUptime);
                    }
                    if (character.OffHandEnchant.Id == 3789) // Berserker Enchant
                    {
                        float timeBetweenBerserkingProcs = 45f / (whiteAttacksPerSecond + yellowAttacksPerSecond);
                        float berserkingUptime = 15f / timeBetweenBerserkingProcs;
                        attackPower += 400f * berserkingUptime * (1 + stats.BonusAttackPowerMultiplier);
                    }
                }
            }
           
            #endregion

            #region Individual DPS
            //1: Melee DPS
            float APDPS = attackPower / 14f;
            float adjustedMHDPS = (wdpsMH + APDPS);
            float adjustedOHDPS = (wdpsOH + APDPS) * .5f;

            float dpsMHMeleeNormal = adjustedMHDPS * (1 - chanceWhiteCrit - glancingRate);
            float dpsMHMeleeCrits = adjustedMHDPS * chanceWhiteCrit * critMultiplierMelee;
            float dpsMHMeleeGlances = adjustedMHDPS * glancingRate * .35f;

            float dpsOHMeleeNormal = adjustedOHDPS * (1 - chanceWhiteCrit - glancingRate);
            float dpsOHMeleeCrits = adjustedOHDPS * chanceWhiteCrit * critMultiplierMelee;
            float dpsOHMeleeGlances = adjustedMHDPS * glancingRate * .35f;

            float meleeMultipliers = weaponMastery * (1 - damageReduction) * (1 - chanceWhiteMiss) * (1 + bonusPhysicalDamage);

            float dpsMHMeleeTotal = ((dpsMHMeleeNormal + dpsMHMeleeCrits + dpsMHMeleeGlances) * unhastedMHSpeed / hastedMHSpeed) * meleeMultipliers;
            float dpsOHMeleeTotal = ((dpsOHMeleeNormal + dpsOHMeleeCrits + dpsOHMeleeGlances) * unhastedOHSpeed / hastedOHSpeed) * meleeMultipliers;
            float dpsMelee = dpsMHMeleeTotal + character.ShamanTalents.DualWield == 1 ? dpsOHMeleeTotal : 0f;
                              

            //2: Stormstrike DPS
            float damageMHSwing = adjustedMHDPS * unhastedMHSpeed;
            float damageOHSwing = adjustedOHDPS * unhastedOHSpeed;
            float dpsSS = 0f;
            if (character.ShamanTalents.Stormstrike == 1)
            {
                float dpsMHSS = (1 + chanceYellowCrit * (critMultiplierMelee - 1)) * damageMHSwing * hitsPerSMHSS;
                float dpsOHSS = (1 + chanceYellowCrit * (critMultiplierMelee - 1)) * damageOHSwing * hitsPerSOHSS;
                if (character.ShamanTalents.DualWield == 0)
                    dpsOHSS = 0f;
                dpsSS = (dpsMHSS + dpsOHSS) * weaponMastery * (1 - damageReduction) * (1 + bonusNatureDamage) * (1 + stats.BonusLLSSDamage);
            }

            //3: Lavalash DPS
            float dpsLL = 0f;
            if (character.ShamanTalents.LavaLash == 1)
            {
                dpsLL = (1 + chanceYellowCrit * (critMultiplierMelee - 1)) * damageOHSwing * hitsPerSLL
                      * (1 + bonusFireDamage) * (1 + stats.BonusLLSSDamage) * weaponMastery; //and no armor reduction yeya!
                if (calcOpts.OffhandImbue == "Flametongue" && character.ShamanTalents.DualWield == 1)
                {  // 25% bonus dmg if FT imbue in OH
                    if (character.ShamanTalents.GlyphofLavaLash)
                        dpsLL *= 1.25f * 1.1f; // +10% bonus dmg if Lava Lash Glyph
                    else
                        dpsLL *= 1.25f;
                }
            }

            //4: Earth Shock DPS
            float ssGlyphBonus = character.ShamanTalents.GlyphofStormstrike ? .08f : 0f;
            float stormstrikeMultiplier = 1.2f + ssGlyphBonus;
            float damageESBase = 872f;
            float coefES = .3858f;
            float damageES = stormstrikeMultiplier * concussionMultiplier * (damageESBase + coefES * spellDamage);
            float hitRollMultiplier = (1 - chanceSpellMiss) + chanceSpellCrit * (critMultiplierSpell - 1);
            float dpsES = (hitRollMultiplier * damageES / shockSpeed) * (1 + bonusNatureDamage);

            //5: Lightning Bolt DPS
            float damageLBBase = 765f;
            float coefLB = .7143f;
            // LightningSpellPower is for totem of hex/the void/ancestral guidance
            float damageLB = stormstrikeMultiplier * concussionMultiplier * (damageLBBase + coefLB * (spellDamage + stats.LightningSpellPower));
            float lbhitRollMultiplier = (1 - chanceSpellMiss) + (chanceSpellCrit + callOfThunder) * (critMultiplierSpell - 1);
            float dpsLB = (lbhitRollMultiplier * damageLB / secondsToFiveStack) * (1 + bonusNatureDamage);
            if (character.ShamanTalents.GlyphofLightningBolt)
                dpsLB *= 1.04f; // 4% bonus dmg if Lightning Bolt Glyph
            if (stats.PendulumOfTelluricCurrentsProc > 0)
                dpsLB += (1168f + 1752f) / 2f / 45f; // need to put the bonus dmg somewhere this seems as good a place as any, not great place though :(
            
            //6: Windfury DPS
            float dpsWF = 0f;
            if (calcOpts.MainhandImbue == "Windfury")
            {
                float damageWFHit = damageMHSwing + (windfuryWeaponBonus / 14 * unhastedMHSpeed);
                dpsWF = (1 + chanceYellowCrit * (critMultiplierMelee - 1)) * damageWFHit * weaponMastery * hitsPerSWF
                        * (1 - damageReduction) * (1 - chanceYellowMiss) * (1 + bonusPhysicalDamage);
            }

            //7: Lightning Shield DPS
            float staticShockProcsPerS = (hitsPerSMH + hitsPerSOH) * staticShockChance;
            float damageLSBase = 380;
            float damageLSCoef = 0.33f; // co-efficient from www.wowwiki.com/Spell_power_coefficient
            float damageLS = stormstrikeMultiplier * shieldBonus * (damageLSBase + damageLSCoef * spellDamage);
            float dpsLS = (1 - chanceSpellMiss) * staticShockProcsPerS * damageLS * (1 + bonusNatureDamage) * (1 + bonusLSDamage);
            if (character.ShamanTalents.GlyphofLightningShield)
                dpsLS *= 1.2f; // 20% bonus dmg if Lightning Shield Glyph

            //8: Searing Totem DPS
            float damageSTBase = calcOpts.Magma ? 371f : 105f;
            float damageSTCoef = calcOpts.Magma ? .1f : .1667f;
            float damageST = (damageSTBase + damageSTCoef * spellDamage) * callofFlameBonus;
            float dpsST = (hitRollMultiplier * damageST / 2) * (1 + bonusFireDamage);

            //9: Flametongue Weapon DPS
            float dpsFT = 0f;
            if (calcOpts.MainhandImbue == "Flametongue")
            {
                float damageFTBase = 274 * unhastedMHSpeed / 4.0f;
                float damageFTCoef = 0.03811f * unhastedMHSpeed;
                float damageFT = damageFTBase + damageFTCoef * spellDamage;
                dpsFT += hitRollMultiplier * damageFT * hitsPerSMH * (1 + bonusFireDamage);
            }
            if (calcOpts.OffhandImbue == "Flametongue" && character.ShamanTalents.DualWield == 1)
            {
                float damageFTBase = 274 * unhastedOHSpeed / 4.0f;
                float damageFTCoef = 0.03811f * unhastedOHSpeed;
                float damageFT = damageFTBase + damageFTCoef * spellDamage;
                dpsFT += hitRollMultiplier * damageFT * hitsPerSOH * (1 + bonusFireDamage);
            } 

            //10: Doggies!  TTT article suggests 300-450 dps while the dogs are up plus 30% of AP
            float dpsDogs = 0f;
            if (character.ShamanTalents.FeralSpirit == 1)
            {
                float FSglyphdps = character.ShamanTalents.GlyphofFeralSpirit ? (attackPower * .3f) / 14f : 0f;
                dpsDogs = 2 * ((375f + .3f * APDPS + FSglyphdps) * (45f / 180f)) * (1 + bonusPhysicalDamage);
            }
            #endregion

            calculatedStats.DPSPoints = dpsMelee + dpsSS + dpsLL + dpsES + dpsLB + dpsWF + dpsLS + dpsST + dpsFT + dpsDogs;
			calculatedStats.SurvivabilityPoints = stats.Health * 0.02f;
            calculatedStats.OverallPoints = calculatedStats.DPSPoints + calculatedStats.SurvivabilityPoints;
			calculatedStats.AvoidedAttacks = chanceWhiteMiss * 100f;
			calculatedStats.DodgedAttacks = chanceDodge * 100f;
			calculatedStats.MissedAttacks = calculatedStats.AvoidedAttacks - calculatedStats.DodgedAttacks;
            calculatedStats.YellowHit = (float)Math.Floor((1 - chanceYellowMiss) * 10000f) / 100f;
            calculatedStats.SpellHit = (float)Math.Floor((1 - chanceSpellMiss) * 10000f) / 100f;
            calculatedStats.WhiteHit = (float)Math.Floor((1 - chanceWhiteMiss) * 10000f) / 100f; 
            calculatedStats.MeleeCrit = (float)Math.Floor(chanceWhiteCrit * 10000f) / 100f;
            calculatedStats.YellowCrit = (float)Math.Floor(chanceYellowCrit * 10000f) / 100f;
            calculatedStats.SpellCrit = (float)Math.Floor(chanceSpellCrit * 10000f) / 100f;
            calculatedStats.AttackSpeed = unhastedOHSpeed / (1f + .3f) / (1f + .2f) / (1f + hasteBonus);
			calculatedStats.ArmorMitigation = damageReduction * 100f;
            calculatedStats.AvMHSpeed = hastedMHSpeed;
            calculatedStats.AvOHSpeed = hastedOHSpeed;
            calculatedStats.EDUptime = edUptime * 100f;
            calculatedStats.URUptime = urUptime  * 100f;
            calculatedStats.FlurryUptime = flurryUptime * 100f;
            calculatedStats.SecondsTo5Stack = secondsToFiveStack;
            calculatedStats.TotalExpertise = (float) Math.Floor(expertiseBonus * 400f + 0.0001);
            
            calculatedStats.SwingDamage = dpsMelee;
            calculatedStats.Stormstrike = dpsSS;
            calculatedStats.LavaLash = dpsLL;
            calculatedStats.EarthShock = dpsES;
            calculatedStats.LightningBolt = dpsLB;
            calculatedStats.WindfuryAttack = dpsWF;
            calculatedStats.LightningShield = dpsLS;
            calculatedStats.SearingMagma = dpsST;
            calculatedStats.FlameTongueAttack = dpsFT;
            calculatedStats.SpiritWolf = dpsDogs;

			return calculatedStats;
        }
        #endregion 

        #region Get Race Stats
        private Stats GetRaceStats(Character character)
        {
            Stats statsRace = new Stats()
            {
                Mana = 4116f,
                AttackPower = 140f,
                SpellCrit = 0.0220f, 
                PhysicalCrit = 0.0292f
            };

            switch (character.Race)
            {
                case Character.CharacterRace.Draenei:
                    statsRace.Health = 6305f;
                    statsRace.Strength = 121f;
                    statsRace.Agility = 71f;
                    statsRace.Stamina = 135f;
                    statsRace.Intellect = 129f;
                    statsRace.Spirit = 145f;
                    break;

                case Character.CharacterRace.Tauren:
                    statsRace.Health = 6313f;
                    statsRace.BonusStaminaMultiplier = .05f;
                    statsRace.Strength = 125f;
                    statsRace.Agility = 69f;
                    statsRace.Stamina = 138f;
                    statsRace.Intellect = 123f;
                    statsRace.Spirit = 145f;
                    break;

                case Character.CharacterRace.Orc:
                    statsRace.Health = 6305f;
                    statsRace.Strength = 123f;
                    statsRace.Agility = 71f;
                    statsRace.Stamina = 138f;
                    statsRace.Intellect = 125f;
                    statsRace.Spirit = 146f;
                    break;

                case Character.CharacterRace.Troll:
                    statsRace.Health = 6305f;
                    statsRace.Strength = 121f;
                    statsRace.Agility = 76f;
                    statsRace.Stamina = 137f;
                    statsRace.Intellect = 124f;
                    statsRace.Spirit = 144f;
                    break;
            }
            return statsRace;
        }
        #endregion

        #region Get Character Stats
        public override Stats GetCharacterStats(Character character, Item additionalItem)
		{
            Stats statsRace = GetRaceStats(character);
            Stats statsBaseGear = GetItemStats(character, additionalItem);
			// Stats statsEnchants = GetEnchantsStats(character);
			Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            Stats statsGearEnchantsBuffs = statsBaseGear + statsBuffs; // +statsEnchants;
            statsGearEnchantsBuffs.Agility += statsGearEnchantsBuffs.AverageAgility;

			CalculationOptionsEnhance calcOpts = character.CalculationOptions as CalculationOptionsEnhance;
			if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Ferocious Inspiration")))
				statsGearEnchantsBuffs.BonusDamageMultiplier = ((1f + statsGearEnchantsBuffs.BonusDamageMultiplier) * 
					(float)Math.Pow(1.03f, calcOpts.NumberOfFerociousInspirations - 1f)) - 1f;

            int AK = character.ShamanTalents.AncestralKnowledge;
            float agiBase = (float)Math.Floor(statsRace.Agility * (1 + statsRace.BonusAgilityMultiplier));
			float agiBonus = (float)Math.Floor(statsGearEnchantsBuffs.Agility * (1 + statsRace.BonusAgilityMultiplier));
			float strBase = (float)Math.Floor(statsRace.Strength * (1 + statsRace.BonusStrengthMultiplier));
			float strBonus = (float)Math.Floor(statsGearEnchantsBuffs.Strength * (1 + statsRace.BonusStrengthMultiplier));
            float intBase = (float)Math.Floor(statsRace.Intellect * (1 + statsRace.BonusIntellectMultiplier) * (1 + .02f * AK)+ .0001f); // added fudge factor because apparently Visual Studio can't multiply 125 * 1.04 to get 130.
            float intBonus = (float)Math.Floor(statsGearEnchantsBuffs.Intellect * (1 + statsRace.BonusIntellectMultiplier) * (1 + .02f * AK));
            float staBase = (float)Math.Floor(statsRace.Stamina);  
			float staBonus = (float)Math.Floor(statsGearEnchantsBuffs.Stamina);
            float spiBase = (float)Math.Floor(statsRace.Spirit);  
			float spiBonus = (float)Math.Floor(statsGearEnchantsBuffs.Spirit);
						
			Stats statsTotal = GetRelevantStats(statsRace + statsGearEnchantsBuffs);
            statsTotal.BonusAttackPowerMultiplier = ((1 + statsRace.BonusAttackPowerMultiplier) * (1 + statsGearEnchantsBuffs.BonusAttackPowerMultiplier)) - 1;
            statsTotal.BonusIntellectMultiplier = ((1 + statsRace.BonusIntellectMultiplier) * (1 + statsGearEnchantsBuffs.BonusIntellectMultiplier)) - 1;
            statsTotal.BonusSpiritMultiplier = ((1 + statsRace.BonusSpiritMultiplier) * (1 + statsGearEnchantsBuffs.BonusSpiritMultiplier)) - 1;
            statsTotal.BonusAgilityMultiplier = ((1 + statsRace.BonusAgilityMultiplier) * (1 + statsGearEnchantsBuffs.BonusAgilityMultiplier)) - 1;
			statsTotal.BonusStrengthMultiplier = ((1 + statsRace.BonusStrengthMultiplier) * (1 + statsGearEnchantsBuffs.BonusStrengthMultiplier)) - 1;
			statsTotal.BonusStaminaMultiplier = ((1 + statsRace.BonusStaminaMultiplier) * (1 + statsGearEnchantsBuffs.BonusStaminaMultiplier)) - 1;
            statsTotal.BonusSpellPowerMultiplier = ((1 + statsRace.BonusSpellPowerMultiplier) * (1 + statsGearEnchantsBuffs.BonusSpellPowerMultiplier)) - 1;
            statsTotal.BonusCritMultiplier = ((1 + statsRace.BonusCritMultiplier) * (1 + statsGearEnchantsBuffs.BonusCritMultiplier)) - 1;
            statsTotal.Agility = (float)Math.Floor((agiBase + agiBonus) * (1 + statsBuffs.BonusAgilityMultiplier));
			statsTotal.Strength = (float)Math.Floor((strBase + strBonus) * (1 + statsBuffs.BonusStrengthMultiplier));
			statsTotal.Stamina = (float)Math.Round((staBase + staBonus) * (1 + statsBuffs.BonusStaminaMultiplier));
			statsTotal.Health = (float)Math.Round((statsRace.Health + statsGearEnchantsBuffs.Health) * (1 + statsRace.BonusStaminaMultiplier) );
            statsTotal.Intellect = (float)Math.Floor((intBase + intBonus) * (1 + statsBuffs.BonusIntellectMultiplier));
            statsTotal.Spirit = (float)Math.Floor((spiBase + spiBonus) * (1 + statsBuffs.BonusSpiritMultiplier)); 
            statsTotal.Mana = statsRace.Mana + statsGearEnchantsBuffs.Mana;
            statsTotal.AttackPower = (float)Math.Floor((statsRace.AttackPower + statsGearEnchantsBuffs.AttackPower) * (1f + statsTotal.BonusAttackPowerMultiplier));
			statsTotal.SpellPower = (statsRace.SpellPower + statsGearEnchantsBuffs.SpellPower) * (1f + statsTotal.BonusSpellPowerMultiplier);
            statsTotal = ApplyTalents(character, statsTotal);
            return statsTotal;
		}

        private float AddAPFromStrAgiInt(Character character, float strength, float agility, float intellect) 
        {
            float intBonusToAP = 0.0f;
            switch (character.ShamanTalents.MentalDexterity)
            {
                case 1:
                    intBonusToAP = .33f * intellect;
                    break;
                case 2:
                    intBonusToAP = .66f * intellect;
                    break;
                case 3:
                    intBonusToAP = 1f * intellect;
                    break;
            }
            return (float)Math.Floor(strength + agility + intBonusToAP);
        }

        private Stats ApplyTalents(Character character, Stats stats) // also includes basic class benefits
        {
            stats.Mana += 15f * stats.Intellect;
            stats.Health += 10f * stats.Stamina;
            stats.Expertise += 3 * character.ShamanTalents.UnleashedRage;
            
            int MQ = character.ShamanTalents.MentalQuickness;
            stats.AttackPower += AddAPFromStrAgiInt(character, stats.Strength, stats.Agility, stats.Intellect); 
            stats.AttackPower = (float)Math.Floor(stats.AttackPower * (1f + stats.BonusAttackPowerMultiplier));
            stats.SpellPower = (float)Math.Floor(stats.SpellPower + (stats.AttackPower * .1f * MQ * (1f + stats.BonusSpellPowerMultiplier)));
            return stats;
        }
        #endregion

        public override void SetDefaults(Character character)
        {
            character.ActiveBuffs.Add(Buff.GetBuffByName("Strength of Earth Totem"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Unleashed Rage"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Flametongue Totem"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Bloodlust"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Windfury Totem"));

            if (character.ShamanTalents.ImprovedWindfuryTotem == 2)
                character.ActiveBuffs.Add(Buff.GetBuffByName("Improved Windfury Totem"));
            if (character.ShamanTalents.EnhancingTotems == 3)
            {
                character.ActiveBuffs.Add(Buff.GetBuffByName("Enhancing Totems (Agility/Strength)")); // add both the Agi Str one 
                character.ActiveBuffs.Add(Buff.GetBuffByName("Enhancing Totems (Spell Power)")); // and the spellpower one
            }

            character.ShamanTalents.GlyphofStormstrike = true;
            character.ShamanTalents.GlyphofFlametongueWeapon = true;
            character.ShamanTalents.GlyphofWindfuryWeapon = true;

        }

        #region RelevantGlyphs
        private static List<string> _relevantGlyphs;
        public override List<string> GetRelevantGlyphs()
        {
            if (_relevantGlyphs == null)
            {
                _relevantGlyphs = new List<string>();
                _relevantGlyphs.Add("Glyph of Feral Spirit");
                _relevantGlyphs.Add("Glyph of Flametongue Weapon");
                _relevantGlyphs.Add("Glyph of Lava Lash");
                _relevantGlyphs.Add("Glyph of Lightning Shield");
                _relevantGlyphs.Add("Glyph of Lightning Bolt");
                _relevantGlyphs.Add("Glyph of Shocking");
                _relevantGlyphs.Add("Glyph of Stormstrike");
                _relevantGlyphs.Add("Glyph of Windfury Weapon");
            }
            return _relevantGlyphs;
        }
        #endregion

        #region Custom Chart Data
        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
		{
			switch (chartName)
			{
				case "Combat Table (White)":
					CharacterCalculationsEnhance currentCalculationsEnhanceWhite = GetCharacterCalculations(character) as CharacterCalculationsEnhance;
					ComparisonCalculationEnhance calcMissWhite = new ComparisonCalculationEnhance()		{ Name = "    Miss    " };
					ComparisonCalculationEnhance calcDodgeWhite = new ComparisonCalculationEnhance()	{ Name = "   Dodge   " };
					ComparisonCalculationEnhance calcCritWhite = new ComparisonCalculationEnhance()		{ Name = "  Crit  " };
					ComparisonCalculationEnhance calcGlanceWhite = new ComparisonCalculationEnhance()	{ Name = " Glance " };
					ComparisonCalculationEnhance calcHitWhite = new ComparisonCalculationEnhance()		{ Name = "Hit" };
					if (currentCalculationsEnhanceWhite != null)
					{
						calcMissWhite.OverallPoints = calcMissWhite.DPSPoints = currentCalculationsEnhanceWhite.MissedAttacks;
						calcDodgeWhite.OverallPoints = calcDodgeWhite.DPSPoints = currentCalculationsEnhanceWhite.DodgedAttacks;
						calcCritWhite.OverallPoints = calcCritWhite.DPSPoints = currentCalculationsEnhanceWhite.MeleeCrit;
						calcGlanceWhite.OverallPoints = calcGlanceWhite.DPSPoints = 25f;
						calcHitWhite.OverallPoints = calcHitWhite.DPSPoints = (100f - calcMissWhite.OverallPoints - 
						                             calcDodgeWhite.OverallPoints - calcCritWhite.OverallPoints - calcGlanceWhite.OverallPoints);
					}
					return new ComparisonCalculationBase[] { calcMissWhite, calcDodgeWhite, calcCritWhite, calcGlanceWhite, calcHitWhite };

				case "Combat Table (Yellow)":
					CharacterCalculationsEnhance currentCalculationsEnhanceYellow = GetCharacterCalculations(character) as CharacterCalculationsEnhance;
					ComparisonCalculationEnhance calcMissYellow = new ComparisonCalculationEnhance()	{ Name = "    Miss    " };
					ComparisonCalculationEnhance calcDodgeYellow = new ComparisonCalculationEnhance()	{ Name = "   Dodge   " };
					ComparisonCalculationEnhance calcCritYellow = new ComparisonCalculationEnhance()	{ Name = "  Crit  " };
					ComparisonCalculationEnhance calcGlanceYellow = new ComparisonCalculationEnhance()	{ Name = " Glance " };
					ComparisonCalculationEnhance calcHitYellow = new ComparisonCalculationEnhance()		{ Name = "Hit" };
					if (currentCalculationsEnhanceYellow != null)
					{
						calcMissYellow.OverallPoints = calcMissYellow.DPSPoints = currentCalculationsEnhanceYellow.MissedAttacks;
						calcDodgeYellow.OverallPoints = calcDodgeYellow.DPSPoints = currentCalculationsEnhanceYellow.DodgedAttacks;
						calcCritYellow.OverallPoints = calcCritYellow.DPSPoints = currentCalculationsEnhanceYellow.YellowCrit;
						calcGlanceYellow.OverallPoints = calcGlanceYellow.DPSPoints = 0f;
						calcHitYellow.OverallPoints = calcHitYellow.DPSPoints = (100f - calcMissYellow.OverallPoints -
						calcDodgeYellow.OverallPoints - calcCritYellow.OverallPoints - calcGlanceYellow.OverallPoints);
					}
					return new ComparisonCalculationBase[] { calcMissYellow, calcDodgeYellow, calcCritYellow, calcGlanceYellow, calcHitYellow };

				case "Relative Stat Values":
					float dpsBase =		GetCharacterCalculations(character).OverallPoints;
					float dpsStr =		(GetCharacterCalculations(character, new Item() { Stats = new Stats() { Strength = 16 } }).OverallPoints - dpsBase);
					float dpsAgi =		(GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = 16 } }).OverallPoints - dpsBase);
				    float dpsAP  =		(GetCharacterCalculations(character, new Item() { Stats = new Stats() { AttackPower = 32 } }).OverallPoints - dpsBase);
                    float dpsInt =      (GetCharacterCalculations(character, new Item() { Stats = new Stats() { Intellect = 16 } }).OverallPoints - dpsBase);
                    float dpsCrit =		(GetCharacterCalculations(character, new Item() { Stats = new Stats() { CritRating = 16} }).OverallPoints - dpsBase);
					float dpsExp =		(GetCharacterCalculations(character, new Item() { Stats = new Stats() { ExpertiseRating = 16 } }).OverallPoints - dpsBase);
					float dpsHaste =	(GetCharacterCalculations(character, new Item() { Stats = new Stats() { HasteRating = 16 } }).OverallPoints - dpsBase);
					float dpsHit =      (GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 16 } }).OverallPoints - dpsBase);
					float dpsDmg =      (GetCharacterCalculations(character, new Item() { Stats = new Stats() { WeaponDamage = 1 } }).OverallPoints - dpsBase);
					float dpsPen =      (GetCharacterCalculations(character, new Item() { Stats = new Stats() { ArmorPenetrationRating = 16 } }).OverallPoints - dpsBase);
                    float dpsSpd =      (GetCharacterCalculations(character, new Item() { Stats = new Stats() { SpellPower = 19 } }).OverallPoints - dpsBase);
                    float dpsSta =      (GetCharacterCalculations(character, new Item() { Stats = new Stats() { Stamina = 24 } }).OverallPoints - dpsBase);

					return new ComparisonCalculationBase[] { 
						new ComparisonCalculationEnhance() { Name = "24 Stamina", OverallPoints = dpsAgi, DPSPoints = dpsAgi },
						new ComparisonCalculationEnhance() { Name = "16 Agility", OverallPoints = dpsAgi, DPSPoints = dpsAgi },
						new ComparisonCalculationEnhance() { Name = "16 Strength", OverallPoints = dpsStr, DPSPoints = dpsStr },
						new ComparisonCalculationEnhance() { Name = "32 Attack Power", OverallPoints = dpsAP, DPSPoints = dpsAP },
						new ComparisonCalculationEnhance() { Name = "16 Intellect", OverallPoints = dpsInt, DPSPoints = dpsInt },
						new ComparisonCalculationEnhance() { Name = "16 Crit Rating", OverallPoints = dpsCrit, DPSPoints = dpsCrit },
						new ComparisonCalculationEnhance() { Name = "16 Expertise Rating", OverallPoints = dpsExp, DPSPoints = dpsExp },
						new ComparisonCalculationEnhance() { Name = "16 Haste Rating", OverallPoints = dpsHaste, DPSPoints = dpsHaste },
						new ComparisonCalculationEnhance() { Name = "16 Hit Rating", OverallPoints = dpsHit, DPSPoints = dpsHit },
//						new ComparisonCalculationEnhance() { Name = "Weapon Damage", OverallPoints = dpsDmg, DPSPoints = dpsDmg },
						new ComparisonCalculationEnhance() { Name = "16 Armor Penetration", OverallPoints = dpsPen, DPSPoints = dpsPen },
                        new ComparisonCalculationEnhance() { Name = "19 Spellpower", OverallPoints = dpsSpd, DPSPoints = dpsSpd }
					};

				default:
					return new ComparisonCalculationBase[0];
			}
        }
        #endregion

        #region Relevant Stats
        public override bool IsItemRelevant(Item item)
		{
			if ((item.Slot == Item.ItemSlot.Ranged && item.Type != Item.ItemType.Totem)) 
				return false;
			return base.IsItemRelevant(item);
		}

		public override Stats GetRelevantStats(Stats stats)
		{
			return new Stats()
				{
					Agility = stats.Agility,
					Strength = stats.Strength,
                    Intellect = stats.Intellect,
					Spirit = stats.Spirit,
                    AttackPower = stats.AttackPower,
					CritRating = stats.CritRating,
					HitRating = stats.HitRating,
					Stamina = stats.Stamina,
					HasteRating = stats.HasteRating,
                    Expertise = stats.Expertise,
					ExpertiseRating = stats.ExpertiseRating,
                    ArmorPenetration = stats.ArmorPenetration,
                    ArmorPenetrationRating = stats.ArmorPenetrationRating,
					MongooseProc = stats.MongooseProc,
                    BerserkingProc = stats.BerserkingProc,
					WeaponDamage = stats.WeaponDamage,
					BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
					BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,
					BonusCritMultiplier = stats.BonusCritMultiplier,
					BonusDamageMultiplier = stats.BonusDamageMultiplier,
					BonusStrengthMultiplier = stats.BonusStrengthMultiplier,
                    BonusIntellectMultiplier = stats.BonusIntellectMultiplier,
                    BonusSpiritMultiplier = stats.BonusSpiritMultiplier,
                    BonusSpellPowerMultiplier = stats.BonusSpellPowerMultiplier,
                    BonusPhysicalDamageMultiplier = stats.BonusPhysicalDamageMultiplier,
                    BonusNatureDamageMultiplier = stats.BonusNatureDamageMultiplier,
                    BonusFireDamageMultiplier = stats.BonusFireDamageMultiplier,
                    Health = stats.Health,
                    Mana = stats.Mana,
					ExposeWeakness = stats.ExposeWeakness,
					Bloodlust = stats.Bloodlust,
					ShatteredSunMightProc = stats.ShatteredSunMightProc,
					SpellPower = stats.SpellPower,
                    SpellCritRating = stats.SpellCritRating,
                    CritMeleeRating = stats.CritMeleeRating,
					LightningBoltHasteProc_15_45 = stats.LightningBoltHasteProc_15_45,
                    LightningSpellPower = stats.LightningSpellPower,
                    TotemLLAttackPower = stats.TotemLLAttackPower,
                    TotemShockAttackPower = stats.TotemShockAttackPower,
                    TotemShockSpellPower = stats.TotemShockSpellPower,
                    TotemSSHaste = stats.TotemSSHaste,
                    TotemWFAttackPower = stats.TotemWFAttackPower,
                    GreatnessProc = stats.GreatnessProc,
                    BonusLSDamage = stats.BonusLSDamage,
                    BonusFlurryHaste = stats.BonusFlurryHaste,
                    BonusMWFreq = stats.BonusMWFreq,
                    BonusLLSSDamage = stats.BonusLLSSDamage,
                    HasteRatingOnPhysicalAttack = stats.HasteRatingOnPhysicalAttack,
                    HasteRatingFor20SecOnUse2Min = stats.HasteRatingFor20SecOnUse2Min,
                    SpellHasteFor10SecOnCast_10_45 = stats.SpellHasteFor10SecOnCast_10_45,
                    SpellPowerFor10SecOnCast_15_45 = stats.SpellPowerFor10SecOnCast_15_45,
                    SpellPowerFor10SecOnHit_10_45 = stats.SpellPowerFor10SecOnHit_10_45,
                    PendulumOfTelluricCurrentsProc = stats.PendulumOfTelluricCurrentsProc,
                    PhysicalHit = stats.PhysicalHit,
                    PhysicalHaste = stats.PhysicalHaste,
                    PhysicalCrit = stats.PhysicalCrit,
                    SpellHit = stats.SpellHit,
                    SpellHaste = stats.SpellHaste,
                    SpellCrit = stats.SpellCrit
				};
		}

		public override bool HasRelevantStats(Stats stats)
		{
			return (stats.Agility + stats.ArmorPenetration + stats.AttackPower + stats.Intellect + stats.Expertise +
				stats.BonusAgilityMultiplier + stats.BonusAttackPowerMultiplier + stats.BonusCritMultiplier +
                stats.BonusStrengthMultiplier + stats.CritRating + stats.ExpertiseRating + stats.HasteRating +
                stats.HitRating + stats.Stamina + stats.Mana + stats.ArmorPenetrationRating + stats.Strength + 
				stats.WeaponDamage + stats.ExposeWeakness + stats.Bloodlust + stats.CritMeleeRating + stats.Spirit +
				stats.ShatteredSunMightProc + stats.SpellPower + stats.BonusIntellectMultiplier + stats.MongooseProc +
                stats.BerserkingProc + stats.BonusSpellPowerMultiplier + stats.HasteRatingOnPhysicalAttack +
                stats.BonusDamageMultiplier + stats.SpellCritRating + stats.LightningSpellPower + stats.BonusMWFreq +
                stats.LightningBoltHasteProc_15_45 + stats.TotemWFAttackPower + stats.TotemSSHaste +
                stats.TotemShockSpellPower + stats.TotemShockAttackPower + stats.TotemLLAttackPower + 
                stats.GreatnessProc + stats.HasteRatingFor20SecOnUse2Min + stats.BonusSpiritMultiplier + 
                stats.SpellHasteFor10SecOnCast_10_45 + stats.SpellPowerFor10SecOnCast_15_45 + stats.BonusFlurryHaste +
                stats.BonusLSDamage + stats.PhysicalCrit + stats.SpellPowerFor10SecOnHit_10_45 +
                stats.PendulumOfTelluricCurrentsProc + stats.PhysicalHaste + stats.PhysicalHit + stats.SpellCrit +
                stats.SpellHit + stats.SpellHaste + stats.BonusPhysicalDamageMultiplier + stats.BonusLLSSDamage +
                stats.BonusNatureDamageMultiplier + stats.BonusFireDamageMultiplier) > 0;
        }
        #endregion
    }
        

    #region Char Calcs Get/Set
    public class CharacterCalculationsEnhance : CharacterCalculationsBase
    {
		private float _overallPoints = 0f;
		public override float OverallPoints
		{
			get { return _overallPoints; }
			set { _overallPoints = value; }
		}

		private float[] _subPoints = new float[] { 0f, 0f };
		public override float[] SubPoints
		{
			get { return _subPoints; }
			set { _subPoints = value; }
		}

		public float DPSPoints
		{
			get { return _subPoints[0]; }
			set { _subPoints[0] = value; }
		}

		public float SurvivabilityPoints
		{
			get { return _subPoints[1]; }
			set { _subPoints[1] = value; }
		}

		private Stats _basicStats;
		public Stats BasicStats
		{
			get { return _basicStats; }
			set { _basicStats = value; }
		}

        private Stats _baseStats;
        public Stats BaseStats
        {
            get { return _baseStats; }
            set { _baseStats = value; }
        }

        private Stats _buffStats;
        public Stats BuffStats
        {
            get { return _buffStats; }
            set { _buffStats = value; }
        }

        private int _targetLevel;
		public int TargetLevel
		{
			get { return _targetLevel; }
			set { _targetLevel = value; }
		}

        private float _totalExpertise;
        public float TotalExpertise
        {
            get { return _totalExpertise; }
            set { _totalExpertise = value; }
        }
        
        private float _avoidedAttacks;
		public float AvoidedAttacks
		{
			get { return _avoidedAttacks; }
			set { _avoidedAttacks = value; }
		}

		private float _dodgedAttacks;
		public float DodgedAttacks
		{
			get { return _dodgedAttacks; }
			set { _dodgedAttacks = value; }
		}

		private float _missedAttacks;
		public float MissedAttacks
		{
			get { return _missedAttacks; }
			set { _missedAttacks = value; }
		}

		private float _whiteCrit;
		public float MeleeCrit
		{
			get { return _whiteCrit; }
			set { _whiteCrit = value; }
		}

		private float _yellowCrit;
		public float YellowCrit
		{
			get { return _yellowCrit; }
			set { _yellowCrit = value; }
		}

        private float _spellCrit;
        public float SpellCrit
        {
            get { return _spellCrit; }
            set { _spellCrit = value; }
        }

        private float _whiteHit;
        public float WhiteHit
        {
            get { return _whiteHit; }
            set { _whiteHit = value; }
        }

        private float _yellowHit;
        public float YellowHit
        {
            get { return _yellowHit; }
            set { _yellowHit = value; }
        }

        private float _spellHit;
        public float SpellHit
        {
            get { return _spellHit; }
            set { _spellHit = value; }
        }

		private float _attackSpeed;
		public float AttackSpeed
		{
			get { return _attackSpeed; }
			set { _attackSpeed = value; }
		}

		private float _armorMitigation;
		public float ArmorMitigation
		{
			get { return _armorMitigation; }
			set { _armorMitigation = value; }
		}

        private float _urUptime;
        public float URUptime
        {
            get { return _urUptime; }
            set { _urUptime = value; }
        }

        private float _edUptime;
        public float EDUptime
        {
            get { return _edUptime; }
            set { _edUptime = value; }
        }

        private float _flurryUptime;
        public float FlurryUptime
        {
            get { return _flurryUptime; }
            set { _flurryUptime = value; }
        }

        private float _secondsTo5Stack;
        public float SecondsTo5Stack
        {
            get { return _secondsTo5Stack; }
            set { _secondsTo5Stack = value; }
        }

        private float _avMHSpeed;
        public float AvMHSpeed
        {
            get { return _avMHSpeed; }
            set { _avMHSpeed = value; }
        }

        private float _avOHSpeed;
        public float AvOHSpeed
        {
            get { return _avOHSpeed; }
            set { _avOHSpeed = value; }
        }

        private float _meleeDamage;
		public float MeleeDamage
		{
			get { return _meleeDamage; }
			set { _meleeDamage = value; }
		}

        private float _swingDamage;
        public float SwingDamage
        {
            get { return _swingDamage; }
            set { _swingDamage = value; }
        }

        private float _windfuryAttack;
        public float WindfuryAttack
        {
            get { return _windfuryAttack; }
            set { _windfuryAttack = value; }
        }

        private float _flametongueAttack;
        public float FlameTongueAttack
        {
            get { return _flametongueAttack; }
            set { _flametongueAttack = value; }
        }

        private float _lightningBolt;
        public float LightningBolt
        {
            get { return _lightningBolt; }
            set { _lightningBolt = value; }
        }

        private float _earthShock;
        public float EarthShock
        {
            get { return _earthShock; }
            set { _earthShock = value; }
        }

        private float _searingMagma;
        public float SearingMagma
        {
            get { return _searingMagma; }
            set { _searingMagma = value; }
        }

        private float _stormstrike;
        public float Stormstrike
        {
            get { return _stormstrike; }
            set { _stormstrike = value; }
        }

        private float _spiritWolf;
        public float SpiritWolf
        {
            get { return _spiritWolf; }
            set { _spiritWolf = value; }
        }

        private float _lightningShield;
        public float LightningShield
        {
            get { return _lightningShield; }
            set { _lightningShield = value; }
        }

        private float _lavaLash;
        public float LavaLash
        {
            get { return _lavaLash; }
            set { _lavaLash = value; }
        }

        public List<Buff> ActiveBuffs { get; set; }
        #endregion

		public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
		{
			Dictionary<string, string> dictValues = new Dictionary<string, string>();
            dictValues.Add("Health", BasicStats.Health.ToString("F0", CultureInfo.InvariantCulture));
            dictValues.Add("Mana", BasicStats.Mana.ToString("F0", CultureInfo.InvariantCulture));
            dictValues.Add("Attack Power", BasicStats.AttackPower.ToString("F0", CultureInfo.InvariantCulture));
            dictValues.Add("Agility", BasicStats.Agility.ToString("F0", CultureInfo.InvariantCulture));
            dictValues.Add("Strength", BasicStats.Strength.ToString("F0", CultureInfo.InvariantCulture));
            dictValues.Add("Intellect", BasicStats.Intellect.ToString("F0", CultureInfo.InvariantCulture));

            dictValues.Add("White Hit", WhiteHit.ToString("F2", CultureInfo.InvariantCulture) + "%");
            dictValues.Add("Yellow Hit", YellowHit.ToString("F2", CultureInfo.InvariantCulture) + "%");
            dictValues.Add("Spell Hit", SpellHit.ToString("F2", CultureInfo.InvariantCulture) + "%");
            dictValues.Add("Melee Crit", String.Format("{0}*Crit Rating {1} (+{2}% crit chance)",
                MeleeCrit.ToString("F2", CultureInfo.InvariantCulture) + "%",
                (BasicStats.CritMeleeRating + BasicStats.CritRating).ToString("F0", CultureInfo.InvariantCulture),
                (StatConversion.GetCritFromRating(BasicStats.CritMeleeRating + BasicStats.CritRating) * 100f).ToString("F2", CultureInfo.InvariantCulture)));
            dictValues.Add("Spell Crit", String.Format("{0}*Crit Rating {1} (+{2}% crit chance)",
                SpellCrit.ToString("F2", CultureInfo.InvariantCulture) + "%",
                (BasicStats.SpellCritRating + BasicStats.CritRating).ToString("F0", CultureInfo.InvariantCulture),
                (StatConversion.GetSpellCritFromRating(BasicStats.SpellCritRating + BasicStats.CritRating) * 100f).ToString("F2", CultureInfo.InvariantCulture)));

            dictValues.Add("Spellpower", BasicStats.SpellPower.ToString("F0", CultureInfo.InvariantCulture));
            dictValues.Add("Total Expertise",
                String.Format((TotalExpertise > 26 ? "{0} (Cap Exceeded)*{1} Expertise\r\n{2} Expertise Rating\r\n{3}% Dodged" :
                                                     "{0}*{1} Expertise\r\n{2} Expertise Rating\r\n{3}% Dodged"),
                TotalExpertise.ToString("F0", CultureInfo.InvariantCulture),
                BasicStats.Expertise.ToString("F0", CultureInfo.InvariantCulture),
                BasicStats.ExpertiseRating.ToString("F0", CultureInfo.InvariantCulture), 
                DodgedAttacks.ToString("F2", CultureInfo.InvariantCulture)));
            dictValues.Add("Haste Rating", String.Format("{0}*{1}% Melee Haste\r\n{2}% Spell Haste", 
                BasicStats.HasteRating.ToString("F0", CultureInfo.InvariantCulture),
                (StatConversion.GetHasteFromRating(BasicStats.HasteRating, Character.CharacterClass.Shaman) * 100f).ToString("F2", CultureInfo.InvariantCulture),
                (StatConversion.GetSpellHasteFromRating(BasicStats.HasteRating, Character.CharacterClass.Shaman) * 100f).ToString("F2", CultureInfo.InvariantCulture)));
            dictValues.Add("Hit Rating", String.Format("{0}*{1}% Melee Hit\r\n{2}% Spell Hit",
                BasicStats.HitRating.ToString("F0", CultureInfo.InvariantCulture),
                (StatConversion.GetHitFromRating(BasicStats.HitRating) * 100f).ToString("F2", CultureInfo.InvariantCulture),
                (StatConversion.GetSpellHitFromRating(BasicStats.HitRating) * 100f).ToString("F2", CultureInfo.InvariantCulture)));
            dictValues.Add("Armour Pen Rating", String.Format("{0}*{1}% Armour Penetration",
                BasicStats.ArmorPenetrationRating.ToString("F0", CultureInfo.InvariantCulture),
                (StatConversion.GetArmorPenetrationFromRating(BasicStats.ArmorPenetrationRating) * 100f).ToString("F2", CultureInfo.InvariantCulture)));
            float spellMiss = 100 - SpellHit;
            dictValues.Add("Avoided Attacks", String.Format("{0}%*{1}% Boss Dodged\r\n{2}% Spell Misses\r\n{3}% White Misses",
                        AvoidedAttacks.ToString("F2", CultureInfo.InvariantCulture), 
                        DodgedAttacks.ToString("F2", CultureInfo.InvariantCulture),
                        spellMiss.ToString("F2", CultureInfo.InvariantCulture), 
                        MissedAttacks.ToString("F2", CultureInfo.InvariantCulture)));
            dictValues.Add("Avg MH Speed", AvMHSpeed.ToString("F2", CultureInfo.InvariantCulture));
            dictValues.Add("Avg OH Speed", AvOHSpeed.ToString("F2", CultureInfo.InvariantCulture));
            dictValues.Add("Armor Mitigation", ArmorMitigation.ToString("F2", CultureInfo.InvariantCulture) + "%");
            					
            dictValues.Add("UR Uptime", URUptime.ToString("F2", CultureInfo.InvariantCulture) + "%");
            dictValues.Add("ED Uptime", EDUptime.ToString("F2", CultureInfo.InvariantCulture) + "%");
            dictValues.Add("Flurry Uptime", FlurryUptime.ToString("F2", CultureInfo.InvariantCulture) + "%");
            dictValues.Add("Avg Time to 5 Stack", SecondsTo5Stack.ToString("F2", CultureInfo.InvariantCulture) + " sec");

            dictValues.Add("DPS Points", DPSPoints.ToString("F2", CultureInfo.InvariantCulture));
            dictValues.Add("Survivability Points", SurvivabilityPoints.ToString("F2", CultureInfo.InvariantCulture));
            dictValues.Add("Overall Points", OverallPoints.ToString("F2", CultureInfo.InvariantCulture));

            dictValues.Add("White Damage", dpsOutputFormat(SwingDamage,DPSPoints));
            dictValues.Add("Windfury Attack", dpsOutputFormat(WindfuryAttack,DPSPoints));
            dictValues.Add("Flametongue Attack", dpsOutputFormat(FlameTongueAttack,DPSPoints));
            dictValues.Add("Lightning Bolt", dpsOutputFormat(LightningBolt,DPSPoints));
            dictValues.Add("Earth Shock", dpsOutputFormat(EarthShock,DPSPoints));
            dictValues.Add("Searing/Magma Totem", dpsOutputFormat(SearingMagma,DPSPoints));
            dictValues.Add("Stormstrike", dpsOutputFormat(Stormstrike,DPSPoints));
            dictValues.Add("Spirit Wolf", dpsOutputFormat(SpiritWolf,DPSPoints));
            dictValues.Add("Lightning Shield", dpsOutputFormat(LightningShield,DPSPoints));
            dictValues.Add("Lava Lash", dpsOutputFormat(LavaLash, DPSPoints));
            dictValues.Add("Total DPS", DPSPoints.ToString("F2", CultureInfo.InvariantCulture));
            
            dictValues.Add("Enhance Version", typeof(CalculationsEnhance).Assembly.GetName().Version.ToString());
			
			return dictValues;
		}

        private String dpsOutputFormat(float dps, float totaldps)
        {
            float percent = dps / totaldps * 100f;
            return string.Format("{0}*{1}% of total dps", 
                dps.ToString("F2", CultureInfo.InvariantCulture),
                percent.ToString("F2", CultureInfo.InvariantCulture));
        }

		public override float GetOptimizableCalculationValue(string calculation)
		{
			switch (calculation)
			{
				case "Health": return BasicStats.Health;
                case "DPS Points": return DPSPoints;
				case "Nature Resist": return BasicStats.NatureResistance;
				case "Fire Resist": return BasicStats.FireResistance;
				case "Frost Resist": return BasicStats.FrostResistance;
				case "Shadow Resist": return BasicStats.ShadowResistance;
				case "Arcane Resist": return BasicStats.ArcaneResistance;
			}
			return 0f;
		}
    }

    #region Comparison Calcs
    public class ComparisonCalculationEnhance : ComparisonCalculationBase
	{
		private string _name = string.Empty;
		public override string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		private float _overallPoints = 0f;
		public override float OverallPoints
		{
			get { return _overallPoints; }
			set { _overallPoints = value; }
		}

		private float[] _subPoints = new float[] { 0f };
		public override float[] SubPoints
		{
			get { return _subPoints; }
			set { _subPoints = value; }
		}

		public float DPSPoints
		{
			get { return _subPoints[0]; }
			set { _subPoints[0] = value; }
		}

		private Item _item = null;
		public override Item Item
		{
			get { return _item; }
			set { _item = value; }
		}

        private ItemInstance _itemInstance = null;
        public override ItemInstance ItemInstance
        {
            get { return _itemInstance; }
            set { _itemInstance = value; }
        }

		private bool _equipped = false;
		public override bool Equipped
		{
			get { return _equipped; }
			set { _equipped = value; }
		}

        public override String BaseStat
        {
            get { return " Attack Power"; }
        }

        public override bool getBaseStatOption(Character character) 
        {
            CalculationOptionsEnhance calcOpts = character.CalculationOptions as CalculationOptionsEnhance;
            return calcOpts.BaseStatOption;
        }

       	public override string ToString()
		{
			return string.Format("{0}: ({1}O {2}DPS)", Name, Math.Round(OverallPoints), Math.Round(DPSPoints));
		}
    }
    #endregion
}
