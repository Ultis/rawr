using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr
{
    [Rawr.Calculations.RawrModelInfo("Enhance", "inv_jewelry_talisman_04", Character.CharacterClass.Shaman)]
	public class CalculationsEnhance : CalculationsBase
	{

        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
		public override CalculationOptionsPanelBase CalculationOptionsPanel
		{
			get
			{
				if (_calculationOptionsPanel == null)
				{
					_calculationOptionsPanel = new CalculationOptionsPanelCat();
				}
				return _calculationOptionsPanel;
			}
		}

		private string[] _characterDisplayCalculationLabels = null;
		public override string[] CharacterDisplayCalculationLabels
		{
			get
			{
				if (_characterDisplayCalculationLabels == null)
					_characterDisplayCalculationLabels = new string[] {
					"Basic Stats:Health",
                    "Basic Stats:Mana",
					"Basic Stats:Attack Power",
					"Basic Stats:Agility",
					"Basic Stats:Intellect",
                    "Basic Stats:Strength",
                    "Basic Stats:Yellow Hit",
					"Basic Stats:White Hit",
                    "Basic Stats:Spell Hit",
                    "Basic Stats:Melee Crit",
                    "Basic Stats:Spell Crit",
                    "Basic Stats:Spellpower",
					"Basic Stats:Expertise Rating",
					"Basic Stats:Haste Rating",
					"Basic Stats:Penetration Rating",
					"Complex Stats:Avoided Attacks",
					"Complex Stats:Avg MH Speed",
                    "Complex Stats:Avg OH Speed",
					"Complex Stats:Armor Mitigation",
                    "Complex Stats:UR Uptime",
                    "Complex Stats:Flurry Uptime",
                    "Complex Stats:Avg Time to 5 Stack",
                    "Complex Stats:Avg Time to Windfury",
					"Complex Stats:DPS Points*DPS Points is your theoretical DPS.",
					"Complex Stats:Overall Points*Rawr is designed to support an Overall point value, comprised of one or more sub point values. Enhancement shamans only care about DPS, so Overall Points will always be identical to DPS Points.",
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
						Item.ItemType.Staff,
						Item.ItemType.TwoHandMace,
                        Item.ItemType.TwoHandAxe,
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

		public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
		{
			//_cachedCharacter = character;
			CalculationOptionsEnhance calcOpts = character.CalculationOptions as CalculationOptionsEnhance;
            int targetLevel = calcOpts.TargetLevel;
            float targetArmor = calcOpts.TargetArmor;
            float exposeWeaknessAPValue = calcOpts.ExposeWeaknessAPValue;
            float bloodlustUptime = calcOpts.BloodlustUptime;
            Stats stats = GetCharacterStats(character, additionalItem);
            CharacterCalculationsEnhance calculatedStats = new CharacterCalculationsEnhance();
            calculatedStats.BasicStats = stats;
            calculatedStats.TargetLevel = targetLevel;
            calculatedStats.ActiveBuffs = new List<Buff>(character.ActiveBuffs);
            
            //Set up some talent variables
            int ET = character.ShamanTalents.EnhancingTotems;
            int TS = character.ShamanTalents.ThunderingStrikes;
            int DWS = character.ShamanTalents.DualWieldSpecialization;
            float shockSpeed = 6f - (.2f * character.ShamanTalents.Reverberation);
            float spellMultiplier = 1f + .01f * character.ShamanTalents.Concussion;
            float staticShockChance = .02f * character.ShamanTalents.StaticShock;
            float shieldBonus = 1f + .05f * character.ShamanTalents.ImprovedShields;
            float totemBonus = 1f + .05f * character.ShamanTalents.CallOfFlame;
            float windfuryTotemHaste = .16f + (.02f * character.ShamanTalents.ImprovedWindfuryTotem);
            float windfuryWeaponBonus = 1250;
            switch (character.ShamanTalents.ElementalWeapons){
                case 1:
                    windfuryWeaponBonus = windfuryWeaponBonus * .13f;
                    break;
                case 2:
                    windfuryWeaponBonus = windfuryWeaponBonus * .27f;
                    break;
                case 3:
                    windfuryWeaponBonus = windfuryWeaponBonus * .4f;
                    break;
            }
            float flurryHasteBonus = .05f * character.ShamanTalents.Flurry + .05f * Math.Min(1,character.ShamanTalents.Flurry);
            float edCritBonus = .03f * character.ShamanTalents.ElementalDevastation;
            float critMultiplierMelee = 2;
            float critMultiplierSpell = 1.5f + .1f * character.ShamanTalents.ElementalFury;
            float mwPPM = 2 * character.ShamanTalents.MaelstromWeapon;
            int stormstrikeSpeed = 10 - (1 * character.ShamanTalents.ImprovedStormstrike);
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

            //work it girl
            float baseArmor = Math.Max(0f, targetArmor - stats.ArmorPenetration);
            float modPercentDecrease = stats.ArmorPenetrationRating / 1539.529991f;
            baseArmor = baseArmor * (1 - modPercentDecrease);
            float modArmor = (baseArmor / (baseArmor + 10557.5f)); // TODO - old value is 10557.5 this comes from???

            float attackPower = stats.AttackPower + (stats.ExposeWeakness * exposeWeaknessAPValue * (1 + stats.BonusAttackPowerMultiplier));

            float hitBonus = stats.HitRating / 3278.998947f;
            float expertiseBonus = Math.Abs(stats.ExpertiseRating / 3278.998947f) * 0.0025f; 

            float glancingRate = 0.25f;

            float chanceCrit = Math.Min(0.75f, (stats.CritMeleeRating + stats.CritRating) / 4590.598679f + stats.Agility / 8333.333333f + .01f * TS);
            float chanceDodge = Math.Max(0f, 0.065f - expertiseBonus);
            float chanceWhiteMiss = Math.Max(0f, 0.28f - hitBonus - .02f * DWS) + chanceDodge;
            float chanceYellowMiss = Math.Max(0f, 0.08f - hitBonus - .02f * DWS) + chanceDodge; // base miss 8% now

            float hitBonusSpell = stats.HitRating / 2623.199272f;
            float chanceSpellMiss = Math.Max(0f, .17f - hitBonusSpell);
            float chanceSpellCrit = Math.Min(0.75f, (stats.SpellCritRating + stats.CritRating) / 4590.598679f + stats.Intellect / 16666.66709f + .01f * TS);
            float spellDamage = stats.SpellPower;

            float chanceWhiteCrit = Math.Min(chanceCrit, 1f - glancingRate - chanceWhiteMiss);
            float chanceYellowCrit = Math.Min(chanceCrit, 1f - chanceYellowMiss);

            float hasteBonus = stats.HasteRating / 3278.998947f;
            float unhastedMHSpeed = 0.0f;
            float wdpsMH = 0.0f;
            if (character != null && character.MainHand != null)
            {
                unhastedMHSpeed = character.MainHand.Speed;
                wdpsMH = character.MainHand.DPS;
            }

            float unhastedOHSpeed = 0.0f;
            float wdpsOH = 0.0f;
            if (character != null && character.OffHand != null)
            {
                unhastedOHSpeed = character.OffHand.Speed;
                wdpsOH = character.OffHand.DPS;
            }
            float baseHastedMHSpeed = unhastedMHSpeed / (1f + hasteBonus) / (1f + windfuryTotemHaste);
            float baseHastedOHSpeed = unhastedOHSpeed / (1f + hasteBonus) / (1f + windfuryTotemHaste);

            //XXX: Only MH WF for now
            float chanceToProcWFPerHit = .2f;
            float avgHitsToProcWF = 1 / chanceToProcWFPerHit;

            //The Swing Loop
            //This is where we figure out feedback systems -- WF, MW, ED, Flurry, etc.
            //It's also where we'll figure out GCD interference when we model that.
            //--------------
            float flurryUptime = 1f;
            float edUptime = 0;
            float hastedMHSpeed = baseHastedMHSpeed;
            float hastedOHSpeed = baseHastedOHSpeed;
            float hitsPerSMHSS = (1f - chanceYellowMiss) /stormstrikeSpeed;
            float hitsPerSOHSS = (hitsPerSMHSS - chanceYellowMiss) / stormstrikeSpeed; //OH only swings if MH connects
            float hitsPerSLL = (1f - chanceYellowMiss) / 6f;
            float swingsPerSMHMelee, swingsPerSOHMelee;
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
                hastedMHSpeed = baseHastedMHSpeed / (1f + (flurryUptime * flurryHasteBonus));
                hastedOHSpeed = baseHastedOHSpeed / (1f + (flurryUptime * flurryHasteBonus));
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

                hitsPerSMH = swingsPerSMHMelee * (1f - chanceWhiteMiss) + hitsPerSWF + hitsPerSMHSS;
                hitsPerSOH = swingsPerSOHMelee * (1f - chanceWhiteMiss) + hitsPerSOHSS + hitsPerSLL;
                mwProcsPerSecond = (mwPPM / (60f / unhastedMHSpeed)) * hitsPerSMH + (mwPPM / (60f / unhastedOHSpeed))  * hitsPerSOH;
                secondsToFiveStack /* oh but i want it now! */ = 5 / mwProcsPerSecond;

                float couldCritSpellsPerS = (earthShocksPerS + 1 / secondsToFiveStack) * (1f - chanceSpellMiss);
                edUptime = 1f - (float)Math.Pow(1-chanceSpellCrit, 10 * couldCritSpellsPerS);
                
                averageMeleeCritChance = chanceYellowCrit + edUptime * edCritBonus;
            }
            float URUptime = 1f - (float)Math.Pow(1 - averageMeleeCritChance, 10 * couldCritSwingsPerSecond);

            //1: Melee DPS
            float APDPS = attackPower / 14;
            float adjustedMHDPS = wdpsMH + APDPS;
            float adjustedOHDPS = (wdpsOH + APDPS) * .5f;
            float damageMHSwing = adjustedMHDPS * unhastedMHSpeed;
            float damageOHSwing = adjustedOHDPS * unhastedOHSpeed;
        
            float dpsMHMeleeHits = adjustedMHDPS;
            float dpsMHMeleeCrits = adjustedMHDPS * chanceWhiteCrit * critMultiplierMelee;
            float dpsMHMeleeGlances = adjustedMHDPS * glancingRate * .35f;

            float dpsOHMeleeHits = adjustedOHDPS * weaponMastery;
            float dpsOHMeleeCrits = adjustedOHDPS * chanceWhiteCrit * critMultiplierMelee;
            float dpsOHMeleeGlances = adjustedMHDPS * glancingRate * .35f;

            float dpsMelee = (dpsMHMeleeHits + dpsMHMeleeCrits + dpsMHMeleeGlances + dpsOHMeleeHits + dpsOHMeleeCrits + dpsOHMeleeGlances) * weaponMastery * (1 - modArmor);

            //2: Stormstrike DPS
            float dpsMHSS = (1 + chanceYellowCrit * (critMultiplierMelee - 1)) * damageMHSwing * hitsPerSMHSS;
            float dpsOHSS = (1 + chanceYellowCrit * (critMultiplierMelee - 1)) * damageOHSwing * hitsPerSOHSS;

            float dpsSS = (dpsMHSS + dpsOHSS) * weaponMastery * (1 - modArmor);

            //3: Lavalash DPS
            float dpsLL = (1 + chanceYellowCrit * (critMultiplierMelee - 1)) * damageOHSwing * hitsPerSLL * 1.25f; //and no armor reduction yeya!

            //4: Earth Shock DPS
            float stormstrikeMultiplier = 1.2f; // TODO glyph adds 8% if equipped
            float damageESBase = 872f;
            float coefES = .3858f;
            float damageES = stormstrikeMultiplier * spellMultiplier * (damageESBase + coefES * stats.SpellPower);
            float hitRollMultiplier = (1 - chanceSpellMiss) + chanceSpellCrit * (critMultiplierSpell - 1);
            float dpsES =  hitRollMultiplier * damageES / shockSpeed;

            //5: Lightning Bolt DPS
            float damageLBBase = 765f;
            float coefLB = .7143f;
            float damageLB = stormstrikeMultiplier * spellMultiplier * (damageLBBase + coefLB * stats.SpellPower);
            float dpsLB = hitRollMultiplier * damageLB / secondsToFiveStack;

            //6: Winfury DPS
            float damageWFHit = damageMHSwing + (windfuryWeaponBonus * unhastedMHSpeed / 14);
            float dpsWF = (1 + chanceYellowCrit * (critMultiplierMelee - 1)) * damageWFHit * weaponMastery * hitsPerSWF * (1 - modArmor);

            //7: Lightning Shield DPS
            float staticShockProcsPerS = (hitsPerSMH + hitsPerSOH) * staticShockChance;
            float damageLSBase = 380;
            float damageLSCoef = 1f; // co-efficient from www.wowwiki.com/Spell_power_coefficient
            float damageLS = stormstrikeMultiplier * shieldBonus * (damageLSBase + damageLSCoef * stats.SpellPower);
            float dpsLS = (1 - chanceSpellMiss) * staticShockProcsPerS * damageLS;

            //8: Searing Totem DPS
            float damageSTBase = 105;
            float damageSTCoef = .1667f;
            float damageST = damageSTBase + damageSTCoef * totemBonus;
            float dpsST = hitRollMultiplier * damageST / 2;

            //9: Flametongue Weapon DPS
            float damageFTBase = 35 * hastedOHSpeed; // TODO - fix FTW dps base numbers for lvl 80s range is 88.96-274 dmg according to tooltip
                                                    // however that figure "varies according to weapon speed"
            float damageFTCoef = .1f;
            float damageFT = damageFTBase + damageFTCoef * stats.SpellPower;
            float dpsFT = hitRollMultiplier * damageFT * hitsPerSOH;

            //10: Doggies!  Assuming 240 dps while the dogs are up
            float dpsDogs = (240f * 45) / (3 * 60); // TODO - need to get better base figures and calcs for exactly how they scale

            calculatedStats.DPSPoints = dpsMelee + dpsSS + dpsLL + dpsES + dpsLB + dpsWF + dpsLS + dpsST + dpsFT + dpsDogs;
			calculatedStats.SurvivabilityPoints = stats.Health * 0.002f;
			calculatedStats.OverallPoints = calculatedStats.DPSPoints;
			calculatedStats.AvoidedAttacks = chanceWhiteMiss * 100f;
			calculatedStats.DodgedAttacks = chanceDodge * 100f;
			calculatedStats.MissedAttacks = calculatedStats.AvoidedAttacks - calculatedStats.DodgedAttacks;
            calculatedStats.YellowHit = (1 - chanceYellowMiss) * 100f;
            calculatedStats.SpellHit = (1 - chanceSpellMiss) * 100f;
            calculatedStats.WhiteHit = (1 - chanceWhiteMiss) * 100f;
            calculatedStats.MeleeCrit = chanceWhiteCrit * 100f;
            calculatedStats.YellowCrit = chanceYellowCrit * 100f;
            calculatedStats.SpellCrit = chanceSpellCrit * 100f;
            calculatedStats.AttackSpeed = unhastedOHSpeed / (1f + .3f) / (1f + .2f) / (1f + hasteBonus);
			calculatedStats.ArmorMitigation = modArmor * 100f;

			return calculatedStats;
		}

		public override Stats GetCharacterStats(Character character, Item additionalItem)
		{
			Stats statsRace = new Stats() { 
					Health = 6305f,
                    Mana = 4116f,
					Strength = 121f,
					Agility = 71f,
					Stamina = 185f,
                    Intellect = 129f,
                    AttackPower = 1250f,
                    SpellCritRating = 48.576f,  // TODO - need to identify what the base spell & melee crit ratings should be
                    CritMeleeRating = 64.4736f}; 
			Stats statsBaseGear = GetItemStats(character, additionalItem);
			Stats statsEnchants = GetEnchantsStats(character);
			Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

			Stats statsGearEnchantsBuffs = statsBaseGear + statsEnchants + statsBuffs;
            statsGearEnchantsBuffs.Agility += statsGearEnchantsBuffs.AverageAgility;

			CalculationOptionsEnhance calcOpts = character.CalculationOptions as CalculationOptionsEnhance;
			statsGearEnchantsBuffs.AttackPower += statsGearEnchantsBuffs.DrumsOfWar * calcOpts.DrumsOfWarUptime;
			statsGearEnchantsBuffs.HasteRating += statsGearEnchantsBuffs.DrumsOfBattle * calcOpts.DrumsOfBattleUptime;
			if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Ferocious Inspiration")))
				statsGearEnchantsBuffs.BonusDamageMultiplier = ((1f + statsGearEnchantsBuffs.BonusDamageMultiplier) * 
					(float)Math.Pow(1.03f, calcOpts.NumberOfFerociousInspirations - 1f)) - 1f;

            int AK = character.ShamanTalents.AncestralKnowledge;
            float agiBase = (float)Math.Floor(statsRace.Agility * (1 + statsRace.BonusAgilityMultiplier));
			float agiBonus = (float)Math.Floor(statsGearEnchantsBuffs.Agility * (1 + statsRace.BonusAgilityMultiplier));
			float strBase = (float)Math.Floor(statsRace.Strength * (1 + statsRace.BonusStrengthMultiplier));
			float strBonus = (float)Math.Floor(statsGearEnchantsBuffs.Strength * (1 + statsRace.BonusStrengthMultiplier));
            float intBase = (float)Math.Floor(statsRace.Intellect * (1 + statsRace.BonusIntellectMultiplier) * (1 + .02f * AK));
            float intBonus = (float)Math.Floor(statsGearEnchantsBuffs.Intellect * (1 + statsRace.BonusIntellectMultiplier) * (1 + .02f * AK));
            float staBase = (float)Math.Floor(statsRace.Stamina * (1 + statsRace.BonusStaminaMultiplier));
			float staBonus = (float)Math.Floor(statsGearEnchantsBuffs.Stamina * (1 + statsRace.BonusStaminaMultiplier));
						
			Stats statsTotal = new Stats();
			statsTotal.BonusAttackPowerMultiplier = ((1 + statsRace.BonusAttackPowerMultiplier) * (1 + statsGearEnchantsBuffs.BonusAttackPowerMultiplier)) - 1;
            statsTotal.BonusIntellectMultiplier = ((1 + statsRace.BonusIntellectMultiplier) * (1 + statsGearEnchantsBuffs.BonusIntellectMultiplier)) - 1;
            statsTotal.BonusAgilityMultiplier = ((1 + statsRace.BonusAgilityMultiplier) * (1 + statsGearEnchantsBuffs.BonusAgilityMultiplier)) - 1;
			statsTotal.BonusStrengthMultiplier = ((1 + statsRace.BonusStrengthMultiplier) * (1 + statsGearEnchantsBuffs.BonusStrengthMultiplier)) - 1;
			statsTotal.BonusStaminaMultiplier = ((1 + statsRace.BonusStaminaMultiplier) * (1 + statsGearEnchantsBuffs.BonusStaminaMultiplier)) - 1;
            statsTotal.BonusSpellPowerMultiplier = ((1 + statsRace.BonusSpellPowerMultiplier) * (1 + statsGearEnchantsBuffs.BonusSpellPowerMultiplier)) - 1;
            statsTotal.Agility = agiBase + (float)Math.Floor((agiBase * statsBuffs.BonusAgilityMultiplier) + agiBonus * (1 + statsBuffs.BonusAgilityMultiplier));
			statsTotal.Strength = strBase + (float)Math.Floor((strBase * statsBuffs.BonusStrengthMultiplier) + strBonus * (1 + statsBuffs.BonusStrengthMultiplier));
			statsTotal.Stamina = staBase + (float)Math.Round((staBase * statsBuffs.BonusStaminaMultiplier) + staBonus * (1 + statsBuffs.BonusStaminaMultiplier));
			statsTotal.Resilience = statsRace.Resilience + statsGearEnchantsBuffs.Resilience;
			statsTotal.Health = (float)Math.Round(((statsRace.Health + statsGearEnchantsBuffs.Health + (statsTotal.Stamina * 10f)) * (character.Race == Character.CharacterRace.Tauren ? 1.05f : 1f)));
			statsTotal.ArmorPenetrationRating = statsRace.ArmorPenetrationRating + statsGearEnchantsBuffs.ArmorPenetrationRating;
            statsTotal.Intellect = intBase + (float)Math.Floor((intBase * statsBuffs.BonusIntellectMultiplier) + intBonus * (1 + statsBuffs.BonusIntellectMultiplier));
            statsTotal.Mana = statsRace.Mana + statsBuffs.Mana + statsGearEnchantsBuffs.Mana + 15f * statsTotal.Intellect;

            int MD = character.ShamanTalents.MentalDexterity;
            float intBonusToAP = 0.0f;
            switch (MD)
            {
                case 1:
                    intBonusToAP = .33f * statsTotal.Intellect;
                    break;
                case 2:
                    intBonusToAP = .66f * statsTotal.Intellect;
                    break;
                case 3:
                    intBonusToAP = 1f * statsTotal.Intellect;
                    break;
            }

            statsTotal.AttackPower = (float)Math.Floor((statsRace.AttackPower + statsGearEnchantsBuffs.AttackPower + statsTotal.Agility + statsTotal.Strength + statsTotal.Intellect) *  (1f + statsTotal.BonusAttackPowerMultiplier));
			statsTotal.BloodlustProc = statsRace.BloodlustProc + statsGearEnchantsBuffs.BloodlustProc;
			statsTotal.BonusCritMultiplier = ((1 + statsRace.BonusCritMultiplier) * (1 + statsGearEnchantsBuffs.BonusCritMultiplier)) - 1;
			
			statsTotal.CritRating = statsRace.CritRating + statsGearEnchantsBuffs.CritRating;
            statsTotal.SpellCritRating = statsRace.SpellCritRating + statsGearEnchantsBuffs.SpellCritRating;
            statsTotal.CritMeleeRating = statsRace.CritMeleeRating + statsGearEnchantsBuffs.CritMeleeRating;

            int MQ = character.ShamanTalents.MentalQuickness;
            float FT = 211f * (1 + character.ShamanTalents.ElementalWeapons * .1f);

            statsTotal.SpellPower = (statsTotal.AttackPower * .1f * MQ) + FT + statsRace.SpellPower + statsGearEnchantsBuffs.SpellPower;
			statsTotal.ExpertiseRating = statsRace.ExpertiseRating + statsGearEnchantsBuffs.ExpertiseRating;
			statsTotal.HasteRating = statsRace.HasteRating + statsGearEnchantsBuffs.HasteRating;
            // Haste trinket (Meteorite Whetstone)
            statsTotal.HasteRating += statsGearEnchantsBuffs.HasteRatingOnPhysicalAttack * 10 / 45;
            statsTotal.HitRating = statsRace.HitRating + statsGearEnchantsBuffs.HitRating;
			statsTotal.TerrorProc = statsRace.TerrorProc + statsGearEnchantsBuffs.TerrorProc;
			statsTotal.WeaponDamage = statsRace.WeaponDamage + statsGearEnchantsBuffs.WeaponDamage;
			statsTotal.ExposeWeakness = statsRace.ExposeWeakness + statsGearEnchantsBuffs.ExposeWeakness;
			statsTotal.Bloodlust = statsRace.Bloodlust + statsGearEnchantsBuffs.Bloodlust;
			statsTotal.ShatteredSunMightProc = statsRace.ShatteredSunMightProc + statsGearEnchantsBuffs.ShatteredSunMightProc;

			return statsTotal;
		}

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
					float dpsStr =		(GetCharacterCalculations(character, new Item() { Stats = new Stats() { Strength = 10 } }).OverallPoints - dpsBase);
					float dpsAgi =		(GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = 10 } }).OverallPoints - dpsBase);
				    float dpsAP  =		(GetCharacterCalculations(character, new Item() { Stats = new Stats() { AttackPower = 20 } }).OverallPoints - dpsBase);
                    float dpsInt =      (GetCharacterCalculations(character, new Item() { Stats = new Stats() { Intellect = 10 } }).OverallPoints - dpsBase);
                    float dpsCrit =		(GetCharacterCalculations(character, new Item() { Stats = new Stats() { CritRating = 10} }).OverallPoints - dpsBase);
					float dpsExp =		(GetCharacterCalculations(character, new Item() { Stats = new Stats() { ExpertiseRating = 10 } }).OverallPoints - dpsBase);
					float dpsHaste =	(GetCharacterCalculations(character, new Item() { Stats = new Stats() { HasteRating = 10 } }).OverallPoints - dpsBase);
					float dpsHit =      (GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 10 } }).OverallPoints - dpsBase);
					float dpsDmg =      (GetCharacterCalculations(character, new Item() { Stats = new Stats() { WeaponDamage = 1 } }).OverallPoints - dpsBase);
					float dpsPen =      (GetCharacterCalculations(character, new Item() { Stats = new Stats() { ArmorPenetrationRating = 10 } }).OverallPoints - dpsBase);
                    float dpsSpd =      (GetCharacterCalculations(character, new Item() { Stats = new Stats() { SpellPower = 12 } }).OverallPoints - dpsBase);

					return new ComparisonCalculationBase[] { 
						new ComparisonCalculationEnhance() { Name = "10 Agility", OverallPoints = dpsAgi, DPSPoints = dpsAgi },
						new ComparisonCalculationEnhance() { Name = "10 Strength", OverallPoints = dpsStr, DPSPoints = dpsStr },
						new ComparisonCalculationEnhance() { Name = "20 Attack Power", OverallPoints = dpsAP, DPSPoints = dpsAP },
						new ComparisonCalculationEnhance() { Name = "10 Intellect", OverallPoints = dpsInt, DPSPoints = dpsInt },
						new ComparisonCalculationEnhance() { Name = "10 Crit Rating", OverallPoints = dpsCrit, DPSPoints = dpsCrit },
						new ComparisonCalculationEnhance() { Name = "10 Expertise Rating", OverallPoints = dpsExp, DPSPoints = dpsExp },
						new ComparisonCalculationEnhance() { Name = "10 Haste Rating", OverallPoints = dpsHaste, DPSPoints = dpsHaste },
						new ComparisonCalculationEnhance() { Name = "10 Hit Rating", OverallPoints = dpsHit, DPSPoints = dpsHit },
//						new ComparisonCalculationEnhance() { Name = "Weapon Damage", OverallPoints = dpsDmg, DPSPoints = dpsDmg },
						new ComparisonCalculationEnhance() { Name = "10 Armor Penetration", OverallPoints = dpsPen, DPSPoints = dpsPen },
                        new ComparisonCalculationEnhance() { Name = "12 Spellpower", OverallPoints = dpsSpd, DPSPoints = dpsSpd }
					};

				default:
					return new ComparisonCalculationBase[0];
			}
		}

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
					AttackPower = stats.AttackPower,
					CritRating = stats.CritRating,
					HitRating = stats.HitRating,
					Stamina = stats.Stamina,
					HasteRating = stats.HasteRating,
					ExpertiseRating = stats.ExpertiseRating,
                    ArmorPenetrationRating = stats.ArmorPenetrationRating,
					BloodlustProc = stats.BloodlustProc,
					TerrorProc = stats.TerrorProc,
                    WeaponDamage = stats.WeaponDamage,
					BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
					BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,
					BonusCritMultiplier = stats.BonusCritMultiplier,
					BonusDamageMultiplier = stats.BonusDamageMultiplier,
					BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
					BonusStrengthMultiplier = stats.BonusStrengthMultiplier,
                    BonusSpellPowerMultiplier = stats.BonusSpellPowerMultiplier,
                    Health = stats.Health,
					MangleCatCostReduction = stats.MangleCatCostReduction,
					ExposeWeakness = stats.ExposeWeakness,
					Bloodlust = stats.Bloodlust,
					DrumsOfBattle = stats.DrumsOfBattle,
					DrumsOfWar = stats.DrumsOfWar,
					ShatteredSunMightProc = stats.ShatteredSunMightProc,
					ThreatReductionMultiplier = stats.ThreatReductionMultiplier,
                    SpellPower = stats.SpellPower,
                    SpellCritRating = stats.SpellCritRating,
                    CritMeleeRating = stats.CritMeleeRating,
					AllResist = stats.AllResist,
					ArcaneResistance = stats.ArcaneResistance,
					NatureResistance = stats.NatureResistance,
					FireResistance = stats.FireResistance,
					FrostResistance = stats.FrostResistance,
					ShadowResistance = stats.ShadowResistance,
					ArcaneResistanceBuff = stats.ArcaneResistanceBuff,
					NatureResistanceBuff = stats.NatureResistanceBuff,
					FireResistanceBuff = stats.FireResistanceBuff,
					FrostResistanceBuff = stats.FrostResistanceBuff,
					ShadowResistanceBuff = stats.ShadowResistanceBuff
				};
		}

		public override bool HasRelevantStats(Stats stats)
		{
			return (stats.Agility + stats.ArmorPenetration + stats.AttackPower + stats.BloodlustProc + stats.Intellect +
				stats.BonusAgilityMultiplier + stats.BonusAttackPowerMultiplier + stats.BonusCritMultiplier +
				stats.BonusStaminaMultiplier + stats.BonusStrengthMultiplier + stats.CritRating + stats.ExpertiseRating +
				stats.HasteRating + stats.HitRating + stats.Stamina +
				stats.Strength + stats.TerrorProc + stats.WeaponDamage + stats.ExposeWeakness + stats.Bloodlust +
				stats.DrumsOfBattle + stats.DrumsOfWar + stats.ShatteredSunMightProc + stats.SpellPower +
				stats.BonusSpellPowerMultiplier + stats.ThreatReductionMultiplier + stats.AllResist +
				stats.ArcaneResistance + stats.NatureResistance + stats.FireResistance +
				stats.FrostResistance + stats.ShadowResistance + stats.ArcaneResistanceBuff +
				stats.NatureResistanceBuff + stats.FireResistanceBuff +
				stats.FrostResistanceBuff + stats.ShadowResistanceBuff) > 0;
		}
	}

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

		private int _targetLevel;
		public int TargetLevel
		{
			get { return _targetLevel; }
			set { _targetLevel = value; }
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

		private float _meleeDamage;
		public float MeleeDamage
		{
			get { return _meleeDamage; }
			set { _meleeDamage = value; }
		}
		public List<Buff> ActiveBuffs { get; set; }

		public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
		{
			float critRating = BasicStats.CritRating;
			if (ActiveBuffs.Contains(Buff.GetBuffByName("Improved Judgement of the Crusade")))
				critRating -= 66.24f;
			critRating -= 264.0768f; //Base 5% + 6% from talents
			
			float hitRating = BasicStats.HitRating;
			if (ActiveBuffs.Contains(Buff.GetBuffByName("Improved Faerie Fire")))
				hitRating -= 47.3077f;
			if (ActiveBuffs.Contains(Buff.GetBuffByName("Heroic Presence")))
				hitRating -= 15.769f;

			float armorPenetration = BasicStats.ArmorPenetration;
			if (ActiveBuffs.Contains(Buff.GetBuffByName("Faerie Fire")))
				armorPenetration -= 610f;
			if (ActiveBuffs.Contains(Buff.GetBuffByName("Sunder Armor (x5)")))
				armorPenetration -= 2600f;
			if (ActiveBuffs.Contains(Buff.GetBuffByName("Curse of Recklessness")))
				armorPenetration -= 800f;
			if (ActiveBuffs.Contains(Buff.GetBuffByName("Expose Armor (5cp)")))
				armorPenetration -= 2000f;
			if (ActiveBuffs.Contains(Buff.GetBuffByName("Improved Expose Armor (5cp)")))
				armorPenetration -= 1000f;

			float attackPower = BasicStats.AttackPower;
			if (ActiveBuffs.Contains(Buff.GetBuffByName("Improved Hunter's Mark")))
				attackPower -= 121f;

            //"Complex Stats:Avg MH Speed",
            //"Complex Stats:Avg OH Speed",

			Dictionary<string, string> dictValues = new Dictionary<string, string>();
			dictValues.Add("Health", BasicStats.Health.ToString());
            dictValues.Add("Mana", BasicStats.Mana.ToString());
			dictValues.Add("Attack Power", attackPower.ToString());
			dictValues.Add("Agility", BasicStats.Agility.ToString());
			dictValues.Add("Strength", BasicStats.Strength.ToString());
            dictValues.Add("Intellect", BasicStats.Intellect.ToString());

            dictValues.Add("White Hit", WhiteHit.ToString() + "%");
            dictValues.Add("Yellow Hit", YellowHit.ToString() + "%");
            dictValues.Add("Spell Hit", SpellHit.ToString() + "%");
            dictValues.Add("Melee Crit", MeleeCrit.ToString() + "%");
            dictValues.Add("Spell Crit", SpellCrit.ToString() + "%");

            dictValues.Add("Spellpower", BasicStats.SpellPower.ToString());

			dictValues.Add("Expertise Rating", BasicStats.ExpertiseRating.ToString());
			dictValues.Add("Haste Rating", BasicStats.HasteRating.ToString());
			dictValues.Add("Penetration Rating", BasicStats.ArmorPenetrationRating.ToString());
            dictValues.Add("Avoided Attacks", string.Format("{0}%*{1}% Dodged, {2}% Missed", AvoidedAttacks, DodgedAttacks, MissedAttacks));
			dictValues.Add("Armor Mitigation", ArmorMitigation.ToString() + "%");

			dictValues.Add("DPS Points", DPSPoints.ToString());
			dictValues.Add("Overall Points", OverallPoints.ToString());
			
			return dictValues;
		}

		public override float GetOptimizableCalculationValue(string calculation)
		{
			switch (calculation)
			{
				case "Health": return BasicStats.Health;
				case "Avoided Attacks %": return AvoidedAttacks;
				case "Nature Resist": return BasicStats.NatureResistance;
				case "Fire Resist": return BasicStats.FireResistance;
				case "Frost Resist": return BasicStats.FrostResistance;
				case "Shadow Resist": return BasicStats.ShadowResistance;
				case "Arcane Resist": return BasicStats.ArcaneResistance;
			}
			return 0f;
		}
    }

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

		private bool _equipped = false;
		public override bool Equipped
		{
			get { return _equipped; }
			set { _equipped = value; }
		}

		public override string ToString()
		{
			return string.Format("{0}: ({1}O {2}DPS)", Name, Math.Round(OverallPoints), Math.Round(DPSPoints));
		}
	}
}
