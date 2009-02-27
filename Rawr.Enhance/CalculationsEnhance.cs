using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Globalization;

namespace Rawr
{
    [Rawr.Calculations.RawrModelInfo("Enhance", "inv_jewelry_talisman_04", Character.CharacterClass.Shaman)]
	public class CalculationsEnhance : CalculationsBase
    {
        #region Gemming Template
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                ////Relevant Gem IDs for Enhancement Shamans
                //Red
                int[] delicate = { 39905, 39997, 40112, 42143 };
                int[] bright = { 39906, 39999, 40114, 36766 };
                int[] precise = { 39910, 40003, 40118, 42154 };
                
                //Yellow
                int[] rigid = { 39915, 40014, 40125, 42156 };
                int[] smooth = { 39914, 40013, 40124, 42149 };

                //Orange
                int[] accurate = { 39966, 40058, 40162 };
                int[] deadly = { 39952, 40043, 40147 };
                int[] glinting = { 39953, 40044, 40148 };
                int[] pristine = { 39961, 40053, 40157 };
                int[] wicked = { 39960, 40052, 40156 };

                //Meta
                // int austere = 41380;
                int relentless = 41398;

                return new List<GemmingTemplate>() { 
                	new GemmingTemplate() { Model = "Enhance", Group = "Uncommon", //Max Agility
						RedId = delicate[0], YellowId = delicate[0], BlueId = delicate[0], PrismaticId = delicate[0], MetaId = relentless },
                	new GemmingTemplate() { Model = "Enhance", Group = "Uncommon", //Max Attack Power
						RedId = bright[0], YellowId = bright[0], BlueId = bright[0], PrismaticId = bright[0], MetaId = relentless },
                	new GemmingTemplate() { Model = "Enhance", Group = "Uncommon", //Max Expertise
						RedId = precise[0], YellowId = precise[0], BlueId = precise[0], PrismaticId = precise[0], MetaId = relentless },
                	new GemmingTemplate() { Model = "Enhance", Group = "Uncommon", //Max Hit
						RedId = rigid[0], YellowId = rigid[0], BlueId = rigid[0], PrismaticId = rigid[0], MetaId = relentless },
                    new GemmingTemplate() { Model = "Enhance", Group = "Uncommon", //Max Crit
						RedId = smooth[0], YellowId = smooth[0], BlueId = smooth[0], PrismaticId = smooth[0], MetaId = relentless },
                    new GemmingTemplate() { Model = "Enhance", Group = "Uncommon", //Expertise/Hit
						RedId = accurate[0], YellowId = accurate[0], BlueId = accurate[0], PrismaticId = accurate[0], MetaId = relentless },
                    new GemmingTemplate() { Model = "Enhance", Group = "Uncommon", //Agility/Crit
						RedId = deadly[0], YellowId = deadly[0], BlueId = deadly[0], PrismaticId = deadly[0], MetaId = relentless },
                    new GemmingTemplate() { Model = "Enhance", Group = "Uncommon", //Agility/Hit
						RedId = glinting[0], YellowId = glinting[0], BlueId = glinting[0], PrismaticId = glinting[0], MetaId = relentless },
                    new GemmingTemplate() { Model = "Enhance", Group = "Uncommon", //Attack Power/Hit
						RedId = pristine[0], YellowId = pristine[0], BlueId = pristine[0], PrismaticId = pristine[0], MetaId = relentless },
                    new GemmingTemplate() { Model = "Enhance", Group = "Uncommon", //Attack Power/Crit
						RedId = wicked[0], YellowId = wicked[0], BlueId = wicked[0], PrismaticId = wicked[0], MetaId = relentless },

                   	new GemmingTemplate() { Model = "Enhance", Group = "Rare", Enabled = true, //Max Agility
						RedId = delicate[1], YellowId = delicate[1], BlueId = delicate[1], PrismaticId = delicate[1], MetaId = relentless },
                	new GemmingTemplate() { Model = "Enhance", Group = "Rare", Enabled = true, //Max Attack Power
						RedId = bright[1], YellowId = bright[1], BlueId = bright[1], PrismaticId = bright[1], MetaId = relentless },
                	new GemmingTemplate() { Model = "Enhance", Group = "Rare", Enabled = true, //Max Expertise
						RedId = precise[1], YellowId = precise[1], BlueId = precise[1], PrismaticId = precise[1], MetaId = relentless },
                	new GemmingTemplate() { Model = "Enhance", Group = "Rare", Enabled = true, //Max Hit
						RedId = rigid[1], YellowId = rigid[1], BlueId = rigid[1], PrismaticId = rigid[1], MetaId = relentless },
                    new GemmingTemplate() { Model = "Enhance", Group = "Rare", Enabled = true, //Max Crit
						RedId = smooth[1], YellowId = smooth[1], BlueId = smooth[1], PrismaticId = smooth[1], MetaId = relentless },
                    new GemmingTemplate() { Model = "Enhance", Group = "Rare", Enabled = true, //Expertise/Hit
						RedId = accurate[1], YellowId = accurate[1], BlueId = accurate[1], PrismaticId = accurate[1], MetaId = relentless },
                    new GemmingTemplate() { Model = "Enhance", Group = "Rare", Enabled = true, //Agility/Crit
						RedId = deadly[1], YellowId = deadly[1], BlueId = deadly[1], PrismaticId = deadly[1], MetaId = relentless },
                    new GemmingTemplate() { Model = "Enhance", Group = "Rare", Enabled = true, //Agility/Hit
						RedId = glinting[1], YellowId = glinting[1], BlueId = glinting[1], PrismaticId = glinting[1], MetaId = relentless },
                    new GemmingTemplate() { Model = "Enhance", Group = "Rare", Enabled = true, //Attack Power/Hit
						RedId = pristine[1], YellowId = pristine[1], BlueId = pristine[1], PrismaticId = pristine[1], MetaId = relentless },
                    new GemmingTemplate() { Model = "Enhance", Group = "Rare", Enabled = true, //Attack Power/Crit
						RedId = wicked[1], YellowId = wicked[1], BlueId = wicked[1], PrismaticId = wicked[1], MetaId = relentless },

                    new GemmingTemplate() { Model = "Enhance", Group = "Epic", //Max Agility
						RedId = delicate[2], YellowId = delicate[2], BlueId = delicate[2], PrismaticId = delicate[2], MetaId = relentless },
                	new GemmingTemplate() { Model = "Enhance", Group = "Epic", //Max Attack Power
						RedId = bright[2], YellowId = bright[2], BlueId = bright[2], PrismaticId = bright[2], MetaId = relentless },
                	new GemmingTemplate() { Model = "Enhance", Group = "Epic", //Max Expertise
						RedId = precise[2], YellowId = precise[2], BlueId = precise[2], PrismaticId = precise[2], MetaId = relentless },
                	new GemmingTemplate() { Model = "Enhance", Group = "Epic", //Max Hit
						RedId = rigid[2], YellowId = rigid[2], BlueId = rigid[2], PrismaticId = rigid[2], MetaId = relentless },
                    new GemmingTemplate() { Model = "Enhance", Group = "Epic", //Max Crit
						RedId = smooth[2], YellowId = smooth[2], BlueId = smooth[2], PrismaticId = smooth[2], MetaId = relentless },
                    new GemmingTemplate() { Model = "Enhance", Group = "Epic", //Expertise/Hit
						RedId = accurate[2], YellowId = accurate[2], BlueId = accurate[2], PrismaticId = accurate[2], MetaId = relentless },
                    new GemmingTemplate() { Model = "Enhance", Group = "Epic", //Agility/Crit
						RedId = deadly[2], YellowId = deadly[2], BlueId = deadly[2], PrismaticId = deadly[2], MetaId = relentless },
                    new GemmingTemplate() { Model = "Enhance", Group = "Epic", //Agility/Hit
						RedId = glinting[2], YellowId = glinting[2], BlueId = glinting[2], PrismaticId = glinting[2], MetaId = relentless },
                    new GemmingTemplate() { Model = "Enhance", Group = "Epic", //Attack Power/Hit
						RedId = pristine[2], YellowId = pristine[2], BlueId = pristine[2], PrismaticId = pristine[2], MetaId = relentless },
                    new GemmingTemplate() { Model = "Enhance", Group = "Epic", //Attack Power/Crit
						RedId = wicked[2], YellowId = wicked[2], BlueId = wicked[2], PrismaticId = wicked[2], MetaId = relentless },

                    new GemmingTemplate() { Model = "Enhance", Group = "Jeweler", //Max Agility
						RedId = delicate[3], YellowId = delicate[3], BlueId = delicate[3], PrismaticId = delicate[3], MetaId = relentless },
                	new GemmingTemplate() { Model = "Enhance", Group = "Jeweler", //Max Attack Power
						RedId = bright[3], YellowId = bright[3], BlueId = bright[3], PrismaticId = bright[3], MetaId = relentless },
                	new GemmingTemplate() { Model = "Enhance", Group = "Jeweler", //Max Expertise
						RedId = precise[3], YellowId = precise[3], BlueId = precise[3], PrismaticId = precise[3], MetaId = relentless },
                	new GemmingTemplate() { Model = "Enhance", Group = "Jeweler", //Max Hit
						RedId = rigid[3], YellowId = rigid[3], BlueId = rigid[3], PrismaticId = rigid[3], MetaId = relentless },
                    new GemmingTemplate() { Model = "Enhance", Group = "Jeweler", //Max Crit
						RedId = smooth[3], YellowId = smooth[3], BlueId = smooth[3], PrismaticId = smooth[3], MetaId = relentless },
               };
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
					"Basic Stats:Expertise Rating",
					"Basic Stats:Haste Rating",
					"Basic Stats:Armour Pen Rating",
					"Complex Stats:Avoided Attacks",
					"Complex Stats:Avg MH Speed",
                    "Complex Stats:Avg OH Speed",
					"Complex Stats:Armor Mitigation",
                    "Complex Stats:UR Uptime",
                    "Complex Stats:ED Uptime",
                    "Complex Stats:Flurry Uptime",
                    "Complex Stats:Avg Time to 5 Stack",
                    "Complex Stats:Avg Time to Windfury",
					"Complex Stats:DPS Points*DPS Points is your theoretical DPS.",
					"Complex Stats:Overall Points*Rawr is designed to support an Overall point value, comprised of one or more sub point values. Enhancement shamans only care about DPS, so Overall Points will always be identical to DPS Points.",
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
            float windfuryWeaponBonus = 1250f + stats.TotemWFAttackPower;
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

            // glyph stuff
            float spellCritModifier = calcOpts.GlyphFT && (calcOpts.OffhandImbue == "Flametongue" | calcOpts.MainhandImbue == "Flametongue") ? .02f : 0f;

            //totem procs
            stats.HasteRating += stats.LightningBoltHasteProc_15_45 * 10f / 55f; // exact copy of Elemental usage for totem (relic)
            stats.HasteRating += stats.TotemSSHaste * 6f / (10f - character.ShamanTalents.ImprovedStormstrike);
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
                    if (expectedAgi > expectedInt)
                        stats.Agility += stats.GreatnessProc * 15f / 47f;  // proc calc lifted from Rawr.Cat odd that its 47sec CD??
                    else
                        stats.Intellect += stats.GreatnessProc * 15f / 47f;
                else
                    if (expectedAgi > expectedInt)
                        stats.Strength += stats.GreatnessProc * 15f / 47f;
                    else
                        stats.Intellect += stats.GreatnessProc * 15f / 47f;
            }
            if (stats.HasteRatingFor20SecOnUse2Min > 0)
            {
                stats.HasteRating += stats.HasteRatingFor20SecOnUse2Min * 20f / 120f;
            }
            if (stats.SpellHasteFor10SecOnCast_10_45 > 0)
            {
                stats.HasteRating += stats.SpellHasteFor10SecOnCast_10_45 * 10f / 45f;
            }
            if (stats.SpellPowerFor10SecOnCast_15_45 > 0)
            {
                stats.SpellPower += stats.SpellPowerFor10SecOnCast_15_45 * 10f / 45f;
            }
            if (stats.SpellPowerFor10SecOnHit_10_45 > 0)
            {
                stats.SpellPower += stats.SpellPowerFor10SecOnHit_10_45 * 10f / 45f;
            }

            ////////////////////////////
            // Main calculation Block //
            ////////////////////////////

            float baseArmor = Math.Max(0f, targetArmor - stats.ArmorPenetration);
            float modPercentDecrease = stats.ArmorPenetrationRating / 1539.529991f;
            baseArmor = baseArmor * (1 - modPercentDecrease);
            float damageReduction = baseArmor / (baseArmor + (467.5f * 80f - 22167.5f));

            float attackPower = stats.AttackPower + (stats.ExposeWeakness * exposeWeaknessAPValue * (1 + stats.BonusAttackPowerMultiplier));

            float hitBonus = stats.HitRating / 3278.998947f;
            float expertiseBonus = (float)Math.Floor((stats.ExpertiseRating / 32.78998947f) * 4f) * .0025f; 

            float glancingRate = 0.25f;

            float chanceCrit = Math.Min(0.75f, (stats.CritMeleeRating + stats.CritRating) / 4590.598679f + stats.Agility / 8333.333333f + .01f * TS + .000001f); //fudge factor for rounding
            float chanceDodge = Math.Max(0f, 0.065f - expertiseBonus);
            float chanceWhiteMiss = Math.Max(0f, 0.28f - hitBonus - .02f * DWS) + chanceDodge;
            float chanceYellowMiss = Math.Max(0f, 0.08f - hitBonus - .02f * DWS) + chanceDodge; // base miss 8% now

            float hitBonusSpell = stats.HitRating / 2623.199272f;
            float chanceSpellMiss = Math.Max(0f, .17f - hitBonusSpell);
            float chanceSpellCrit = Math.Min(0.75f, (stats.SpellCritRating + stats.CritRating) / 4590.598679f + stats.Intellect / 16666.66709f + .01f * TS + spellCritModifier + .000001f); //fudge factor for rounding
            float spellDamage = stats.SpellPower;

            float chanceWhiteCrit = Math.Min(chanceCrit, 1f - glancingRate - chanceWhiteMiss);
            float chanceYellowCrit = Math.Min(chanceCrit, 1f - chanceYellowMiss);

            float hasteBonus = stats.HasteRating / 3278.998947f;
            float unhastedMHSpeed = 0.0f;
            float wdpsMH = 0.0f;
            float unhastedOHSpeed = 0.0f;
            float wdpsOH = 0.0f;
            if (character != null) 
            {
                if (character.MainHand != null)
                {
                    unhastedMHSpeed = character.MainHand.Item.Speed;
                    wdpsMH = character.MainHand.Item.DPS;
                }
                else // assume a base vendor weapon
                {
                    unhastedMHSpeed = 3.0f;
                    wdpsMH = 46.3f;
                }
                if (character.OffHand != null)
                {
                    unhastedOHSpeed = character.OffHand.Item.Speed;
                    wdpsOH = character.OffHand.Item.DPS;
                }
                else // assume a base vendor weapon
                {
                    unhastedOHSpeed = 3.0f;
                    wdpsOH = 46.3f;
                }
            }

            float baseHastedMHSpeed = unhastedMHSpeed / (1f + hasteBonus) / (1f + windfuryTotemHaste);
            float baseHastedOHSpeed = unhastedOHSpeed / (1f + hasteBonus) / (1f + windfuryTotemHaste);

            if (stats.MongooseProc > 0 | stats.BerserkingProc > 0)
            {
                if (character.MainHandEnchant != null)
                {
                    float whiteAttacksPerSecond = (1f - chanceWhiteMiss - chanceDodge) / baseHastedMHSpeed;
                    float yellowChanceHit = (1f - chanceYellowMiss - chanceDodge);
                    float yellowAttacksPerSecond = getYellowAttacksPerSecond(character, yellowChanceHit, baseHastedMHSpeed, true);
                    if (character.MainHandEnchant.Id == 2673)
                    {
                        float timeBetweenMongooseProcs = 60f / (whiteAttacksPerSecond + yellowAttacksPerSecond);
                        float mongooseUptime = 15f / timeBetweenMongooseProcs;
                        chanceCrit = Math.Min(0.75f, chanceCrit + (120f * mongooseUptime * (1 + stats.BonusAgilityMultiplier)) / 8333.333333f);
                        attackPower += 120f * mongooseUptime * (1 + stats.BonusAgilityMultiplier) * (1 + stats.BonusAttackPowerMultiplier);
                        baseHastedMHSpeed /= 1f + (0.02f * mongooseUptime);
                    }
                    if (character.MainHandEnchant != null && character.MainHandEnchant.Id == 3789)
                    {
                        float timeBetweenBerserkingProcs = 45f / (whiteAttacksPerSecond + yellowAttacksPerSecond);
                        float berserkingUptime = 15f / timeBetweenBerserkingProcs;
                        attackPower += 400f * berserkingUptime * (1 + stats.BonusAttackPowerMultiplier);
                    }
                }
                if (character.OffHandEnchant != null)
                {
                    float whiteAttacksPerSecond = (1f - chanceWhiteMiss - chanceDodge) / baseHastedOHSpeed;
                    float yellowChanceHit = (1f - chanceYellowMiss - chanceDodge);
                    float yellowAttacksPerSecond = getYellowAttacksPerSecond(character, yellowChanceHit, baseHastedOHSpeed, false);
                    if (character.OffHandEnchant.Id == 2673) 
                    {
                        float timeBetweenMongooseProcs = 60f / (whiteAttacksPerSecond + yellowAttacksPerSecond);
                        float mongooseUptime = 15f / timeBetweenMongooseProcs;
                        chanceCrit = Math.Min(0.75f, chanceCrit + (120f * mongooseUptime * (1 + stats.BonusAgilityMultiplier)) / 8333.333333f);
                        attackPower += 120f * mongooseUptime * (1 + stats.BonusAgilityMultiplier) * (1 + stats.BonusAttackPowerMultiplier);
                        baseHastedOHSpeed /= 1f + (0.02f * mongooseUptime);
                    }
                    if (character.OffHandEnchant != null && character.OffHandEnchant.Id == 3789)
                    {
                        float timeBetweenBerserkingProcs = 45f / (whiteAttacksPerSecond + yellowAttacksPerSecond);
                        float berserkingUptime = 15f / timeBetweenBerserkingProcs;
                        attackPower += 400f * berserkingUptime * (1 + stats.BonusAttackPowerMultiplier);
                    }
                }
            }

            //XXX: Only MH WF for now
            float chanceToProcWFPerHit = .2f + (calcOpts.GlyphWF ? .02f : 0f);
            float avgHitsToProcWF = 1 / chanceToProcWFPerHit;

            //The Swing Loop
            //This is where we figure out feedback systems -- WF, MW, ED, Flurry, etc.
            //It's also where we'll figure out GCD interference when we model that.
            //--------------
            float flurryUptime = 1f;
            float edUptime = 0f;
            float urUptime = 0f;
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
            urUptime = 1f - (float)Math.Pow(1 - averageMeleeCritChance, 10 * couldCritSwingsPerSecond);

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

            float dpsMelee = (dpsMHMeleeHits + dpsMHMeleeCrits + dpsMHMeleeGlances + dpsOHMeleeHits + dpsOHMeleeCrits + dpsOHMeleeGlances) * weaponMastery * (1-damageReduction);

            //2: Stormstrike DPS
            float dpsMHSS = (1 + chanceYellowCrit * (critMultiplierMelee - 1)) * damageMHSwing * hitsPerSMHSS;
            float dpsOHSS = (1 + chanceYellowCrit * (critMultiplierMelee - 1)) * damageOHSwing * hitsPerSOHSS;

            float dpsSS = (dpsMHSS + dpsOHSS) * weaponMastery * (1 - damageReduction);

            //3: Lavalash DPS
            float dpsLL = (1 + chanceYellowCrit * (critMultiplierMelee - 1)) * damageOHSwing * hitsPerSLL * 1.25f; //and no armor reduction yeya!
            if (calcOpts.GlyphLL && calcOpts.OffhandImbue == "flametongue")
                dpsLL *= 1.1f; // 10% bonus dmg if Lava Lash Glyph & Flametongue imbue in OH

            //4: Earth Shock DPS
            float ssGlyphBonus = calcOpts.GlyphSS ? .08f : 0f;
            float stormstrikeMultiplier = 1.2f + ssGlyphBonus;
            float damageESBase = 872f;
            float coefES = .3858f;
            float damageES = stormstrikeMultiplier * spellMultiplier * (damageESBase + coefES * stats.SpellPower);
            float hitRollMultiplier = (1 - chanceSpellMiss) + chanceSpellCrit * (critMultiplierSpell - 1);
            float dpsES =  hitRollMultiplier * damageES / shockSpeed;

            //5: Lightning Bolt DPS
            float damageLBBase = 765f;
            float coefLB = .7143f;
            // stats.LightningSpellPower is for totem of hex/the void/ancestral guidance
            float damageLB = stormstrikeMultiplier * spellMultiplier * (damageLBBase + coefLB * (stats.SpellPower + stats.LightningSpellPower));
            float dpsLB = hitRollMultiplier * damageLB / secondsToFiveStack;
            if (calcOpts.GlyphLB)
                dpsLB *= 1.04f; // 4% bonus dmg if Lightning Bolt Glyph
            if (stats.PendulumOfTelluricCurrentsProc > 0)
                dpsLB += (1168f + 1752f) / 2f / 45f; // need to put the bonus dmg somewhere this seems as good a place as any, not great place though :(
            
            //6: Windfury DPS
            float damageWFHit = damageMHSwing + (windfuryWeaponBonus * unhastedMHSpeed / 14);
            float dpsWF = (1 + chanceYellowCrit * (critMultiplierMelee - 1)) * damageWFHit * weaponMastery * hitsPerSWF * (1 - damageReduction);

            //7: Lightning Shield DPS
            float staticShockProcsPerS = (hitsPerSMH + hitsPerSOH) * staticShockChance;
            float damageLSBase = 380;
            float damageLSCoef = 1f; // co-efficient from www.wowwiki.com/Spell_power_coefficient
            float damageLS = stormstrikeMultiplier * shieldBonus * (damageLSBase + damageLSCoef * stats.SpellPower);
            float dpsLS = (1 - chanceSpellMiss) * staticShockProcsPerS * damageLS;
            if (calcOpts.GlyphLS)
                dpsLS *= 1.2f; // 20% bonus dmg if Lightning Shield Glyph

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

            //10: Doggies!  TTT article suggests 300-450 dps while the dogs are up plus 30% of AP
            float dpsDogs = (375f + .3f * APDPS) * (45f / 180f); 

            calculatedStats.DPSPoints = dpsMelee + dpsSS + dpsLL + dpsES + dpsLB + dpsWF + dpsLS + dpsST + dpsFT + dpsDogs;
			calculatedStats.SurvivabilityPoints = stats.Health * 0.002f;
			calculatedStats.OverallPoints = calculatedStats.DPSPoints;
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
            calculatedStats.EDUptime = edUptime * 100f;
            calculatedStats.URUptime = urUptime  * 100f;
            calculatedStats.FlurryUptime = flurryUptime * 100f;

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
       
        private float getYellowAttacksPerSecond(Character character, float yellowHitChance, float weaponSpeed, bool mainhand)
        {
            float yellowAttacksPerSecond = 0f;
            CalculationOptionsEnhance calcOpts = character.CalculationOptions as CalculationOptionsEnhance;
            switch (character.ShamanTalents.ImprovedStormstrike)
            {
                case 0:
                    // 3+5 etc is number of SS and number of LL per time interval
                    yellowAttacksPerSecond = (3 + 5) * yellowHitChance / 30;
                    break;
                case 1:
                    yellowAttacksPerSecond = (2 + 3) * yellowHitChance / 18;
                    break;
                case 2:
                    yellowAttacksPerSecond = (3 + 4) * yellowHitChance / 24;
                    break;

            }
            float WFProcChance = 1f / 6f;
            if (calcOpts.GlyphWF)
            {
                // need to modify WFProcChance if WF Glyph
            }
            // now add WF yellow attacks
            if (calcOpts.MainhandImbue == "windfury" && mainhand)
            {
                if (calcOpts.OffhandImbue == "windfury")
                {
                    // wf on both need to modify chances
                }
                else
                    yellowAttacksPerSecond += (2 * yellowHitChance) / (weaponSpeed / WFProcChance);
            }
            else if (calcOpts.OffhandImbue =="windfury" & !mainhand)
                yellowAttacksPerSecond += (2 * yellowHitChance) / (weaponSpeed / WFProcChance);
            return yellowAttacksPerSecond;
        }
        #endregion 

        #region Get Character Stats
        public override Stats GetCharacterStats(Character character, Item additionalItem)
		{
            Stats statsRace = new Stats() { 
                    Mana = 4116f,
                    AttackPower = 140f, 
                    SpellCritRating = 101f,
                    CritMeleeRating = 134f};

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

            Stats statsBaseGear = GetItemStats(character, additionalItem);
			// Stats statsEnchants = GetEnchantsStats(character);
			Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            Stats statsGearEnchantsBuffs = statsBaseGear + statsBuffs; // +statsEnchants;
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
            float intBase = (float)Math.Floor(statsRace.Intellect * (1 + statsRace.BonusIntellectMultiplier) * (1 + .02f * AK)+ .0001f); // added fudge factor because apparently Visual Studio can't multiply 125 * 1.04 to get 130.
            float intBonus = (float)Math.Floor(statsGearEnchantsBuffs.Intellect * (1 + statsRace.BonusIntellectMultiplier) * (1 + .02f * AK));
            float staBase = (float)Math.Floor(statsRace.Stamina);  // need to add mining bonus if have profession
			float staBonus = (float)Math.Floor(statsGearEnchantsBuffs.Stamina);
						
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
            statsTotal.Health = (float)Math.Round(statsRace.Health * (1 + statsRace.BonusStaminaMultiplier) + statsGearEnchantsBuffs.Health + (statsTotal.Stamina * 10f));
			statsTotal.ArmorPenetrationRating = statsRace.ArmorPenetrationRating + statsGearEnchantsBuffs.ArmorPenetrationRating;
            statsTotal.Intellect = intBase + (float)Math.Floor((intBase * statsBuffs.BonusIntellectMultiplier) + intBonus * (1 + statsBuffs.BonusIntellectMultiplier));
            statsTotal.Mana = statsRace.Mana + statsBuffs.Mana + statsGearEnchantsBuffs.Mana + 15f * statsTotal.Intellect;
            statsTotal.Spirit = statsRace.Spirit + statsBuffs.Spirit + statsGearEnchantsBuffs.Spirit;

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

            statsTotal.AttackPower = (float)Math.Floor((statsRace.AttackPower + statsGearEnchantsBuffs.AttackPower + statsTotal.Agility + statsTotal.Strength + intBonusToAP) * (1f + statsTotal.BonusAttackPowerMultiplier));
			statsTotal.BloodlustProc = statsRace.BloodlustProc + statsGearEnchantsBuffs.BloodlustProc;
			statsTotal.BonusCritMultiplier = ((1 + statsRace.BonusCritMultiplier) * (1 + statsGearEnchantsBuffs.BonusCritMultiplier)) - 1;
			
			statsTotal.CritRating = statsRace.CritRating + statsGearEnchantsBuffs.CritRating;
            statsTotal.SpellCritRating = statsRace.SpellCritRating + statsGearEnchantsBuffs.SpellCritRating;
            statsTotal.CritMeleeRating = statsRace.CritMeleeRating + statsGearEnchantsBuffs.CritMeleeRating;

            int MQ = character.ShamanTalents.MentalQuickness;
            statsTotal.BonusFlametongueDamage = (float) Math.Floor(211f * (1 + character.ShamanTalents.ElementalWeapons * .1f));

            statsTotal.SpellPower = (float) Math.Floor((statsTotal.AttackPower * .1f * MQ) + statsRace.SpellPower + statsGearEnchantsBuffs.SpellPower);
			statsTotal.ExpertiseRating = statsRace.ExpertiseRating + statsGearEnchantsBuffs.ExpertiseRating;
			statsTotal.HasteRating = statsRace.HasteRating + statsGearEnchantsBuffs.HasteRating;
            statsTotal.HasteRatingOnPhysicalAttack = statsGearEnchantsBuffs.HasteRatingOnPhysicalAttack;
            statsTotal.HasteRating += statsTotal.HasteRatingOnPhysicalAttack * 10 / 45; // Haste trinket (Meteorite Whetstone/Dragonspine Trophy)
            statsTotal.HitRating = statsRace.HitRating + statsGearEnchantsBuffs.HitRating;
			statsTotal.WeaponDamage = statsRace.WeaponDamage + statsGearEnchantsBuffs.WeaponDamage;
			statsTotal.ExposeWeakness = statsRace.ExposeWeakness + statsGearEnchantsBuffs.ExposeWeakness;
			statsTotal.Bloodlust = statsRace.Bloodlust + statsGearEnchantsBuffs.Bloodlust;
			statsTotal.ShatteredSunMightProc = statsGearEnchantsBuffs.ShatteredSunMightProc;
            statsTotal.MongooseProc = statsGearEnchantsBuffs.MongooseProc;
            statsTotal.BerserkingProc = statsGearEnchantsBuffs.BerserkingProc;

            // totem special proc stats
            statsTotal.LightningSpellPower = statsGearEnchantsBuffs.LightningSpellPower;
            statsTotal.LightningBoltHasteProc_15_45 = statsGearEnchantsBuffs.LightningBoltHasteProc_15_45;
            statsTotal.TotemSSHaste = statsGearEnchantsBuffs.TotemSSHaste;
            statsTotal.TotemLLAttackPower = statsGearEnchantsBuffs.TotemLLAttackPower;
            statsTotal.TotemShockAttackPower = statsGearEnchantsBuffs.TotemShockAttackPower;
            statsTotal.TotemShockSpellPower = statsGearEnchantsBuffs.TotemShockSpellPower;
            statsTotal.TotemWFAttackPower = statsGearEnchantsBuffs.TotemWFAttackPower;
            
            // Trinket special procs
            statsTotal.GreatnessProc = statsGearEnchantsBuffs.GreatnessProc;
            statsTotal.HasteRatingFor20SecOnUse2Min = statsGearEnchantsBuffs.HasteRatingFor20SecOnUse2Min;
            statsTotal.SpellHasteFor10SecOnCast_10_45 = statsGearEnchantsBuffs.SpellHasteFor10SecOnCast_10_45;
            statsTotal.SpellPowerFor10SecOnCast_15_45 = statsGearEnchantsBuffs.SpellPowerFor10SecOnCast_15_45;
            statsTotal.SpellPowerFor10SecOnHit_10_45 = statsGearEnchantsBuffs.SpellPowerFor10SecOnHit_10_45;
            statsTotal.PendulumOfTelluricCurrentsProc = statsGearEnchantsBuffs.PendulumOfTelluricCurrentsProc;
			return statsTotal;
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
					AttackPower = stats.AttackPower,
					CritRating = stats.CritRating,
					HitRating = stats.HitRating,
					Stamina = stats.Stamina,
					HasteRating = stats.HasteRating,
					ExpertiseRating = stats.ExpertiseRating,
                    ArmorPenetrationRating = stats.ArmorPenetrationRating,
					BloodlustProc = stats.BloodlustProc,
					WeaponDamage = stats.WeaponDamage,
					BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
					BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,
					BonusCritMultiplier = stats.BonusCritMultiplier,
					BonusDamageMultiplier = stats.BonusDamageMultiplier,
					BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
					BonusStrengthMultiplier = stats.BonusStrengthMultiplier,
                    BonusSpellPowerMultiplier = stats.BonusSpellPowerMultiplier,
                    Health = stats.Health,
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
					ShadowResistanceBuff = stats.ShadowResistanceBuff,
                    LightningBoltHasteProc_15_45 = stats.LightningBoltHasteProc_15_45,
                    LightningSpellPower = stats.LightningSpellPower,
                    TotemLLAttackPower = stats.TotemLLAttackPower,
                    TotemShockAttackPower = stats.TotemShockAttackPower,
                    TotemShockSpellPower = stats.TotemShockSpellPower,
                    TotemSSHaste = stats.TotemSSHaste,
                    TotemWFAttackPower = stats.TotemWFAttackPower,
                    GreatnessProc = stats.GreatnessProc,
                    HasteRatingFor20SecOnUse2Min = stats.HasteRatingFor20SecOnUse2Min,
                    SpellHasteFor10SecOnCast_10_45 = stats.SpellHasteFor10SecOnCast_10_45,
                    SpellPowerFor10SecOnCast_15_45 = stats.SpellPowerFor10SecOnCast_15_45,
                    SpellPowerFor10SecOnHit_10_45 = stats.SpellPowerFor10SecOnHit_10_45,
                    PendulumOfTelluricCurrentsProc = stats.PendulumOfTelluricCurrentsProc
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
				stats.NatureResistanceBuff + stats.FireResistanceBuff + stats.FrostResistanceBuff + 
                stats.ShadowResistanceBuff + stats.LightningSpellPower + stats.LightningBoltHasteProc_15_45 +
				stats.TotemWFAttackPower + stats.TotemSSHaste + stats.TotemShockSpellPower + stats.TotemShockAttackPower +
                stats.TotemLLAttackPower + stats.GreatnessProc + stats.HasteRatingFor20SecOnUse2Min +
                stats.SpellHasteFor10SecOnCast_10_45 + stats.SpellPowerFor10SecOnCast_15_45 +
                stats.SpellPowerFor10SecOnHit_10_45 + stats.PendulumOfTelluricCurrentsProc) > 0;
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
            //"Complex Stats:Avg MH Speed",
            //"Complex Stats:Avg OH Speed",

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
            dictValues.Add("Melee Crit", MeleeCrit.ToString("F2", CultureInfo.InvariantCulture) + "%");
            dictValues.Add("Spell Crit", SpellCrit.ToString("F2", CultureInfo.InvariantCulture) + "%");

            dictValues.Add("Spellpower", BasicStats.SpellPower.ToString("F0", CultureInfo.InvariantCulture));

            dictValues.Add("Expertise Rating", BasicStats.ExpertiseRating.ToString("F0", CultureInfo.InvariantCulture));
            dictValues.Add("Haste Rating", BasicStats.HasteRating.ToString("F0", CultureInfo.InvariantCulture));
            dictValues.Add("Armour Pen Rating", BasicStats.ArmorPenetrationRating.ToString("F0", CultureInfo.InvariantCulture));
            dictValues.Add("Avoided Attacks", string.Format("{0}%*{1}% Dodged, {2}% Missed", AvoidedAttacks, DodgedAttacks, MissedAttacks));
            dictValues.Add("Armor Mitigation", ArmorMitigation.ToString("F2", CultureInfo.InvariantCulture) + "%");
            					
            dictValues.Add("UR Uptime", URUptime.ToString("F2", CultureInfo.InvariantCulture) + "%");
            dictValues.Add("ED Uptime", EDUptime.ToString("F2", CultureInfo.InvariantCulture) + "%");
            dictValues.Add("Flurry Uptime", FlurryUptime.ToString("F2", CultureInfo.InvariantCulture) + "%");
//            dictValues.Add("Avg Time to 5 Stack", "");
//            dictValues.Add("Avg Time to Windfury", "");

            dictValues.Add("DPS Points", DPSPoints.ToString("F2", CultureInfo.InvariantCulture));
            dictValues.Add("Overall Points", OverallPoints.ToString("F2", CultureInfo.InvariantCulture));

            dictValues.Add("White Damage", SwingDamage.ToString("F2", CultureInfo.InvariantCulture));
            dictValues.Add("Windfury Attack", WindfuryAttack.ToString("F2", CultureInfo.InvariantCulture));
            dictValues.Add("Flametongue Attack", FlameTongueAttack.ToString("F2", CultureInfo.InvariantCulture));
            dictValues.Add("Lightning Bolt", LightningBolt.ToString("F2", CultureInfo.InvariantCulture));
            dictValues.Add("Earth Shock", EarthShock.ToString("F2", CultureInfo.InvariantCulture));
            dictValues.Add("Searing/Magma Totem", SearingMagma.ToString("F2", CultureInfo.InvariantCulture));
            dictValues.Add("Stormstrike", Stormstrike.ToString("F2", CultureInfo.InvariantCulture));
            dictValues.Add("Spirit Wolf", SpiritWolf.ToString("F2", CultureInfo.InvariantCulture));
            dictValues.Add("Lightning Shield", LightningShield.ToString("F2", CultureInfo.InvariantCulture));
            dictValues.Add("Lava Lash", LavaLash.ToString("F2", CultureInfo.InvariantCulture));
            dictValues.Add("Total DPS", DPSPoints.ToString("F2", CultureInfo.InvariantCulture));

            dictValues.Add("Enhance Version", typeof(CalculationsEnhance).Assembly.GetName().Version.ToString());
			
			return dictValues;
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
        

		public override string ToString()
		{
			return string.Format("{0}: ({1}O {2}DPS)", Name, Math.Round(OverallPoints), Math.Round(DPSPoints));
		}
    }
    #endregion
}
