using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
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
					"Relative Gem Values",
                    "MH Weapon Speeds",
                    "OH Weapon Speeds",
					};
				return _customChartNames;
			}
        }
        #endregion 

        #region Overrides
        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
        public override CalculationOptionsPanelBase CalculationOptionsPanel
        {
            get
            {
                if (_calculationOptionsPanel == null)
                    _calculationOptionsPanel = new CalculationOptionsPanelEnhance();
                return _calculationOptionsPanel;
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
        #endregion

        #region Main Calculations
        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
        {
            #region Applied Stats
            //_cachedCharacter = character;
            ItemInstance offhand = character.OffHand;
			CalculationOptionsEnhance calcOpts = character.CalculationOptions as CalculationOptionsEnhance;
            CharacterCalculationsEnhance calculatedStats = new CharacterCalculationsEnhance();
            Stats stats = GetCharacterStats(character, additionalItem);
            calculatedStats.BasicStats = stats;
            Stats statsRace = GetRaceStats(character);
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            calculatedStats.BaseStats = ApplyTalents(character, statsRace, statsBaseGear);
            calculatedStats.BuffStats = GetBuffsStats(character.ActiveBuffs);
            calculatedStats.TargetLevel = calcOpts.TargetLevel;
            calculatedStats.ActiveBuffs = new List<Buff>(character.ActiveBuffs);
            float initialAP = stats.AttackPower;
            
            // deal with Special Effects - for now add into stats regardless of effect later need to be more precise
            StatsSpecialEffects se = new StatsSpecialEffects(character, stats);
            Stats specialEffects = se.getSpecialEffects();
            stats += specialEffects;
            if (stats.GreatnessProc > 0)
                se.GreatnessProc();
            //Set up some talent variables
            float concussionMultiplier = 1f + .01f * character.ShamanTalents.Concussion;
            float staticShockChance = .02f * character.ShamanTalents.StaticShock;
            float shieldBonus = 1f + .05f * character.ShamanTalents.ImprovedShields;
            float callofFlameBonus = 1f + .05f * character.ShamanTalents.CallOfFlame;
            float windfuryWeaponBonus = 1250f + stats.TotemWFAttackPower;
            float callOfThunder = .05f * character.ShamanTalents.CallOfThunder;
            float mentalQuickness = .1f * character.ShamanTalents.MentalQuickness;
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
            float critMultiplierMelee = 2f + stats.BonusCritMultiplier;
            float critMultiplierSpell = 1.5f + .1f * character.ShamanTalents.ElementalFury + stats.BonusSpellCritMultiplier;
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
            switch (character.ShamanTalents.UnleashedRage)
            {
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
            if (stats.ShatteredSunMightProc > 0 && calcOpts.ShattrathFaction == "Aldor")
                stats.AttackPower += 39.13f;

            if (calcOpts.MainhandImbue == "Flametongue")
                stats.SpellPower += (float)Math.Floor((float)(211f * (1 + character.ShamanTalents.ElementalWeapons * .1f)));
            if (calcOpts.OffhandImbue == "Flametongue" && character.ShamanTalents.DualWield == 1)
                stats.SpellPower += (float)Math.Floor((float)(211f * (1 + character.ShamanTalents.ElementalWeapons * .1f)));
            
            //totem procs
            stats.HasteRating += stats.TotemSSHaste * 6f / 8f; // 8 = SS speed
            stats.SpellPower += stats.TotemShockSpellPower;
            stats.AttackPower += stats.TotemLLAttackPower + stats.TotemShockAttackPower;

            // Finally make sure to add in the spellpower from MQ gained from all the bonus AP added in this section
            stats.SpellPower += mentalQuickness * (stats.AttackPower - initialAP);
            #endregion


            ////////////////////////////
            // Main calculation Block //
            ////////////////////////////

			#region Damage Model
            CombatStats cs = new CombatStats(character, stats); // calculate the combat stats using modified stats
            // only apply unleashed rage talent if not already applied Unleashed Rage buff.
            float URattackPower = (calculatedStats.BuffStats.BonusAttackPowerMultiplier == .1f) ? 0f : 
                                                    (stats.AttackPower * unleashedRage * cs.URUptime);
            float attackPower = stats.AttackPower + URattackPower;
            float wdpsMH = character.MainHand == null ? 46.3f : character.MainHand.Item.DPS;
            float wdpsOH = character.OffHand == null ? 46.3f : character.OffHand.Item.DPS;
            float spellPower = (stats.SpellPower + URattackPower * mentalQuickness) * (1 + stats.BonusSpellPowerMultiplier);
            float AP_SP_Ratio = (spellPower-274) / attackPower;
            float bonusSpellDamage = stats.BonusDamageMultiplier;
            float bonusPhysicalDamage = (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusPhysicalDamageMultiplier) - 1f;
            float bonusFireDamage = (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusFireDamageMultiplier) - 1f;
            float bonusNatureDamage = (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusNatureDamageMultiplier) - 1f;
            float bonusLSDamage = stats.BonusLSDamage; // 2 piece T7 set bonus
            int baseResistance =  Math.Max((calcOpts.TargetLevel - character.Level) * 5, 0);
            float bossFireResistance = 1f - ((baseResistance + calcOpts.TargetFireResistance) / (character.Level * 5f)) * .75f;
            float bossNatureResistance = 1f - ((baseResistance + calcOpts.TargetNatureResistance) / (character.Level * 5f)) * .75f;

            #endregion

            #region Individual DPS
            //1: Melee DPS
            float APDPS = attackPower / 14f;
            float adjustedMHDPS = (wdpsMH + APDPS);
            float adjustedOHDPS = 0f;
            float dpsOHMeleeTotal = 0f;

            float dpsMHMeleeNormal = adjustedMHDPS * cs.NormalHitModifier;
            float dpsMHMeleeCrits = adjustedMHDPS * cs.CritHitModifier;
            float dpsMHMeleeGlances = adjustedMHDPS * cs.GlancingHitModifier;

            float meleeMultipliers = weaponMastery * cs.DamageReduction * cs.ChanceWhiteHit * (1 + bonusPhysicalDamage);
            float dpsMHMeleeTotal = ((dpsMHMeleeNormal + dpsMHMeleeCrits + dpsMHMeleeGlances) * cs.UnhastedMHSpeed / cs.HastedMHSpeed) * meleeMultipliers;

            if (character.ShamanTalents.DualWield == 1 && cs.HastedOHSpeed != 0)
            {
                adjustedOHDPS = (wdpsOH + APDPS) * .5f;
                float dpsOHMeleeNormal = adjustedOHDPS * cs.NormalHitModifier;
                float dpsOHMeleeCrits = adjustedOHDPS * cs.CritHitModifier;
                float dpsOHMeleeGlances = adjustedOHDPS * cs.GlancingHitModifier;
                dpsOHMeleeTotal = ((dpsOHMeleeNormal + dpsOHMeleeCrits + dpsOHMeleeGlances) * cs.UnhastedOHSpeed / cs.HastedOHSpeed) * meleeMultipliers;
            }

            float dpsMelee = dpsMHMeleeTotal + dpsOHMeleeTotal;
                              
            //2: Stormstrike DPS
            float damageMHSwing = adjustedMHDPS * cs.UnhastedMHSpeed + stats.TotemSSDamage;
            float damageOHSwing = 0f;
            if (character.ShamanTalents.DualWield == 1)
                damageOHSwing = adjustedOHDPS * cs.UnhastedOHSpeed + stats.TotemSSDamage;
            float dpsSS = 0f;
            if (character.ShamanTalents.Stormstrike == 1)
            {
                float swingDPS = damageMHSwing * cs.HitsPerSMHSS + damageOHSwing * cs.HitsPerSOHSS;
                float SSnormal = (cs.ChanceYellowHit - cs.ChanceYellowCrit) * swingDPS;
                float SScrit = swingDPS * cs.ChanceYellowCrit * critMultiplierMelee;
                dpsSS = (SSnormal + SScrit) * cs.DamageReduction * (1 + bonusNatureDamage) * (1 + stats.BonusLLSSDamage) * bossNatureResistance;
            }

            //3: Lavalash DPS
            float dpsLL = 0f;
            if (character.ShamanTalents.LavaLash == 1 && character.ShamanTalents.DualWield == 1)
            {
                float lavalashDPS = damageOHSwing * cs.HitsPerSLL;
                float LLnormal = (cs.ChanceYellowHit - cs.ChanceYellowCrit) * lavalashDPS;
                float LLcrit = lavalashDPS * cs.ChanceYellowCrit * critMultiplierMelee;
                dpsLL = (LLnormal + LLcrit) * (1 + bonusFireDamage) * (1 + stats.BonusLLSSDamage) * bossFireResistance; //and no armor reduction yeya!
                if (calcOpts.OffhandImbue == "Flametongue")
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
            float damageES = stormstrikeMultiplier * concussionMultiplier * (damageESBase + coefES * spellPower);
            float spellHitRollMultiplier = cs.ChanceSpellHit + cs.ChanceSpellCrit * (critMultiplierSpell - 1);
            float shockSpeed = 6f - (.2f * character.ShamanTalents.Reverberation);
            float dpsES = (spellHitRollMultiplier * damageES / shockSpeed) * (1 + bonusNatureDamage) * bossNatureResistance;

            //5: Lightning Bolt DPS
            float damageLBBase = 765f;
            float coefLB = .7143f;
            // LightningSpellPower is for totem of hex/the void/ancestral guidance
            float damageLB = stormstrikeMultiplier * concussionMultiplier * (damageLBBase + coefLB * (spellPower + stats.LightningSpellPower));
            float lbHitRollMultiplier = cs.ChanceSpellHit + (cs.ChanceSpellCrit + callOfThunder) * (critMultiplierSpell - 1);
            float dpsLB = (lbHitRollMultiplier * damageLB / cs.SecondsToFiveStack) * (1 + bonusNatureDamage) * bossNatureResistance;
            if (character.ShamanTalents.GlyphofLightningBolt)
                dpsLB *= 1.04f; // 4% bonus dmg if Lightning Bolt Glyph
            
            //6: Windfury DPS
            float dpsWF = 0f;
            if (calcOpts.MainhandImbue == "Windfury")
            {
                float damageWFHit = damageMHSwing + (windfuryWeaponBonus / 14 * cs.UnhastedMHSpeed);
                float WFnormal = (cs.ChanceYellowHit - cs.ChanceYellowCrit) * damageWFHit;
                float WFcrit = cs.ChanceYellowCrit * critMultiplierMelee * damageWFHit;
                dpsWF = (WFnormal + WFcrit) * weaponMastery * cs.HitsPerSWF * cs.DamageReduction * (1 + bonusPhysicalDamage);
//                dpsWF = (1 + cs.ChanceYellowCrit * (critMultiplierMelee - 1)) * damageWFHit * weaponMastery * cs.HitsPerSWF
//                        * cs.DamageReduction * cs.ChanceYellowHit * (1 + bonusPhysicalDamage);
            }

            //7: Lightning Shield DPS
            float staticShockProcsPerS = (cs.HitsPerSMH + cs.HitsPerSOH) * staticShockChance;
            float damageLSBase = 380;
            float damageLSCoef = 0.33f; // co-efficient from www.wowwiki.com/Spell_power_coefficient
            float damageLS = stormstrikeMultiplier * shieldBonus * (damageLSBase + damageLSCoef * spellPower);
            float dpsLS = cs.ChanceSpellHit * staticShockProcsPerS * damageLS * (1 + bonusNatureDamage) * (1 + bonusLSDamage) * bossNatureResistance;
            if (character.ShamanTalents.GlyphofLightningShield)
                dpsLS *= 1.2f; // 20% bonus dmg if Lightning Shield Glyph

            //8: Searing/Magma Totem DPS
            float damageSTMTBase = calcOpts.Magma ? 371f : 105f;
            float damageSTMTCoef = calcOpts.Magma ? .1f : .1667f;
            float damageSTMT = (damageSTMTBase + damageSTMTCoef * spellPower) * callofFlameBonus;
            float dpsSTMT = (spellHitRollMultiplier * damageSTMT / 2) * (1 + bonusFireDamage) * bossFireResistance;

            //9: Flametongue Weapon DPS
            float dpsFT = 0f;
            if (calcOpts.MainhandImbue == "Flametongue")
            {
                float damageFTBase = 274f * cs.UnhastedMHSpeed / 4.0f;
                float damageFTCoef = 0.03811f * cs.UnhastedMHSpeed;
                float damageFT = damageFTBase + damageFTCoef * spellPower;
                dpsFT += spellHitRollMultiplier * damageFT * cs.HitsPerSMH * (1 + bonusFireDamage) * bossFireResistance;
            }
            if (calcOpts.OffhandImbue == "Flametongue" && character.ShamanTalents.DualWield == 1)
            {
                float damageFTBase = 274f * cs.UnhastedOHSpeed / 4.0f;
                float damageFTCoef = 0.03811f * cs.UnhastedOHSpeed;
                float damageFT = damageFTBase + damageFTCoef * spellPower;
                dpsFT += spellHitRollMultiplier * damageFT * cs.HitsPerSOH * (1 + bonusFireDamage) * bossFireResistance;
            } 

            //10: Doggies!  TTT article suggests 300-450 dps while the dogs are up plus 30% of AP
            // my analysis reveals they get 31% of shaman AP + 2 * their STR and base 206.17 dps.
            float dpsDogs = 0f;
            if (character.ShamanTalents.FeralSpirit == 1)
            {
                float hitBonus = stats.PhysicalHit + StatConversion.GetHitFromRating(stats.HitRating);
                float FSglyphAP = character.ShamanTalents.GlyphofFeralSpirit ? attackPower * .3f : 0f;
                float soeBuff = IsBuffChecked("Strength of Earth Totem") ? 155f : 0f;
                float enhTotems = IsBuffChecked("Enhancing Totems (Agility/Strength)") ? 23f : 0f;
                float dogsStr = 331f + soeBuff + enhTotems; // base str = 331 and assume SoE totem giving 178 str buff
                float dogsAP = ((dogsStr * 2f - 20f) + .31f * attackPower + FSglyphAP) * cs.URUptime * (1f + unleashedRage);
                float dogsMissrate = Math.Max(0f, 0.08f - hitBonus - .02f * character.ShamanTalents.DualWieldSpecialization) + 0.065f;
                float dogsCrit = 0.05f * (1 + stats.BonusCritChance);
                float dogsBaseSpeed = 1.5f;
                float dogsHitsPerS = 1f / (dogsBaseSpeed / (1f + stats.PhysicalHaste) / cs.BloodlustHaste);
                float dogsBaseDamage = (206.17f + dogsAP / 14f) * dogsBaseSpeed;

                float dogsMeleeNormal = dogsBaseDamage * (1 - dogsMissrate - dogsCrit - cs.GlancingRate);
                float dogsMeleeCrits = dogsBaseDamage * dogsCrit * critMultiplierMelee;
                float dogsMeleeGlances = dogsBaseDamage * cs.GlancingHitModifier;

                float dogsTotalDPS = dogsMeleeNormal + dogsMeleeCrits + dogsMeleeGlances;
                float dogsMultipliers = cs.DamageReduction * (1 + bonusPhysicalDamage);

                dpsDogs = 2 * (45f / 180f) * dogsTotalDPS * dogsHitsPerS * dogsMultipliers;
                calculatedStats.SpiritWolf = new DPSAnalysis(dpsDogs, dogsMissrate, 0.065f, cs.GlancingRate, dogsCrit);
            }
            else
            {
                calculatedStats.SpiritWolf = new DPSAnalysis(0, 0, 0, 0, 0);
            }
            #endregion

            calculatedStats.DPSPoints = dpsMelee + dpsSS + dpsLL + dpsES + dpsLB + dpsWF + dpsLS + dpsSTMT + dpsFT + dpsDogs;
			calculatedStats.SurvivabilityPoints = stats.Health * 0.02f;
            calculatedStats.OverallPoints = calculatedStats.DPSPoints + calculatedStats.SurvivabilityPoints;
			calculatedStats.AvoidedAttacks = (1 - cs.ChanceWhiteHit) * 100f;
			calculatedStats.DodgedAttacks = cs.ChanceDodge * 100f;
			calculatedStats.MissedAttacks = calculatedStats.AvoidedAttacks - calculatedStats.DodgedAttacks;
            calculatedStats.YellowHit = (float)Math.Floor((float)(cs.ChanceYellowHit * 10000f)) / 100f;
            calculatedStats.SpellHit = (float)Math.Floor((float)(cs.ChanceSpellHit * 10000f)) / 100f;
            calculatedStats.OverSpellHitCap = (float)Math.Floor((float)(cs.OverSpellHitCap * 10000f)) / 100f;
            calculatedStats.WhiteHit = (float)Math.Floor((float)(cs.ChanceWhiteHit * 10000f)) / 100f; 
            calculatedStats.MeleeCrit = (float)Math.Floor((float)((cs.ChanceWhiteCrit - cs.EDBonusCrit)) * 10000f) / 100f;
            calculatedStats.YellowCrit = (float)Math.Floor((float)((cs.ChanceYellowCrit - cs.EDBonusCrit)) * 10000f) / 100f;
            calculatedStats.SpellCrit = (float)Math.Floor((float)(cs.ChanceSpellCrit * 10000f)) / 100f;
		    calculatedStats.GlancingBlows = cs.GlancingRate * 100f;
            calculatedStats.ArmorMitigation = cs.DamageReduction * 100f;
            calculatedStats.AvMHSpeed = cs.HastedMHSpeed;
            calculatedStats.AvOHSpeed = cs.HastedOHSpeed;
            calculatedStats.EDBonusCrit = cs.EDBonusCrit * 100f;
            calculatedStats.EDUptime = cs.EDUptime * 100f;
            calculatedStats.URUptime = cs.URUptime  * 100f;
            calculatedStats.FlurryUptime = cs.FlurryUptime * 100f;
            calculatedStats.SecondsTo5Stack = cs.SecondsToFiveStack;
            calculatedStats.TotalExpertise = (float) Math.Floor((float)(cs.ExpertiseBonus * 400f));

            calculatedStats.SwingDamage = new DPSAnalysis(dpsMelee, 1 - cs.ChanceWhiteHit, cs.ChanceDodge, cs.GlancingRate, cs.ChanceMeleeCrit);
            calculatedStats.Stormstrike = new DPSAnalysis(dpsSS, 1 - cs.ChanceYellowHit, cs.ChanceDodge, -1, cs.ChanceYellowCrit);
            calculatedStats.LavaLash = new DPSAnalysis(dpsLL, 1 - cs.ChanceYellowHit, cs.ChanceDodge, -1, cs.ChanceYellowCrit);
            calculatedStats.EarthShock = new DPSAnalysis(dpsES, 1 - cs.ChanceSpellHit, -1, -1, cs.ChanceSpellCrit);
            calculatedStats.LightningBolt = new DPSAnalysis(dpsLB, 1 - cs.ChanceSpellHit, -1, -1, cs.ChanceSpellCrit);
            calculatedStats.WindfuryAttack = new DPSAnalysis(dpsWF, 1 - cs.ChanceYellowHit, cs.ChanceDodge, -1, cs.ChanceYellowCrit);
            calculatedStats.LightningShield = new DPSAnalysis(dpsLS, 1 - cs.ChanceSpellHit, -1, -1, -1);
            calculatedStats.SearingMagma = new DPSAnalysis(dpsSTMT, 1 - cs.ChanceYellowHit, -1, -1, cs.ChanceYellowCrit);
            calculatedStats.FlameTongueAttack = new DPSAnalysis(dpsFT, 1 - cs.ChanceSpellHit, -1, -1, cs.ChanceSpellCrit);
            
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
            Stats statsRace = BaseStats.GetBaseStats(character); // GetRaceStats(character);
            Stats statsBaseGear = GetItemStats(character, additionalItem);
			// Stats statsEnchants = GetEnchantsStats(character);
			Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
            Stats statsGearEnchantsBuffs = statsBaseGear + statsBuffs; // +statsEnchants;

			CalculationOptionsEnhance calcOpts = character.CalculationOptions as CalculationOptionsEnhance;
            int AK = character.ShamanTalents.AncestralKnowledge;
            float agiBase = (float)Math.Floor((float)(statsRace.Agility * (1 + statsRace.BonusAgilityMultiplier)));
            float agiBonus = (float)Math.Floor((float)(statsGearEnchantsBuffs.Agility * (1 + statsBaseGear.BonusAgilityMultiplier)));
			float strBase = (float)Math.Floor((float)(statsRace.Strength * (1 + statsRace.BonusStrengthMultiplier)));
            float strBonus = (float)Math.Floor((float)(statsGearEnchantsBuffs.Strength * (1 + statsBaseGear.BonusStrengthMultiplier)));
            float intBase = (float)Math.Floor((float)(statsRace.Intellect * (1 + statsRace.BonusIntellectMultiplier) * (1 + .02f * AK)));
            float intBonus = (float)Math.Floor((float)(statsGearEnchantsBuffs.Intellect * (1 + statsBaseGear.BonusIntellectMultiplier) * (1 + .02f * AK)));
            float staBase = (float)Math.Floor((float)(statsRace.Stamina));  
			float staBonus = (float)Math.Floor((float)(statsGearEnchantsBuffs.Stamina));
            float spiBase = (float)Math.Floor((float)(statsRace.Spirit));  
			float spiBonus = (float)Math.Floor((float)(statsGearEnchantsBuffs.Spirit));
						
			Stats statsTotal = GetRelevantStats(statsRace + statsGearEnchantsBuffs);
            statsTotal.BonusAttackPowerMultiplier = ((1 + statsRace.BonusAttackPowerMultiplier) * (1 + statsGearEnchantsBuffs.BonusAttackPowerMultiplier)) - 1;
            statsTotal.BonusIntellectMultiplier = ((1 + statsRace.BonusIntellectMultiplier) * (1 + statsGearEnchantsBuffs.BonusIntellectMultiplier)) - 1;
            statsTotal.BonusSpiritMultiplier = ((1 + statsRace.BonusSpiritMultiplier) * (1 + statsGearEnchantsBuffs.BonusSpiritMultiplier)) - 1;
            statsTotal.BonusAgilityMultiplier = ((1 + statsRace.BonusAgilityMultiplier) * (1 + statsGearEnchantsBuffs.BonusAgilityMultiplier)) - 1;
			statsTotal.BonusStrengthMultiplier = ((1 + statsRace.BonusStrengthMultiplier) * (1 + statsGearEnchantsBuffs.BonusStrengthMultiplier)) - 1;
			statsTotal.BonusStaminaMultiplier = ((1 + statsRace.BonusStaminaMultiplier) * (1 + statsGearEnchantsBuffs.BonusStaminaMultiplier)) - 1;
            statsTotal.BonusSpellPowerMultiplier = ((1 + statsRace.BonusSpellPowerMultiplier) * (1 + statsGearEnchantsBuffs.BonusSpellPowerMultiplier)) - 1;
            statsTotal.BonusCritMultiplier = ((1 + statsRace.BonusCritMultiplier) * (1 + statsGearEnchantsBuffs.BonusCritMultiplier)) - 1;
            statsTotal.Agility = (float)Math.Floor((float)((agiBase + agiBonus) * (1 + statsBuffs.BonusAgilityMultiplier)));
			statsTotal.Strength = (float)Math.Floor((float)((strBase + strBonus) * (1 + statsBuffs.BonusStrengthMultiplier)));
			statsTotal.Stamina = (float)Math.Round((staBase + staBonus) * (1 + statsBuffs.BonusStaminaMultiplier));
			statsTotal.Health = (float)Math.Round((statsRace.Health + statsGearEnchantsBuffs.Health) * (1 + statsRace.BonusStaminaMultiplier) );
            statsTotal.Intellect = (float)Math.Floor((float)((intBase + intBonus) * (1 + statsBuffs.BonusIntellectMultiplier)));
            statsTotal.Spirit = (float)Math.Floor((float)((spiBase + spiBonus) * (1 + statsBuffs.BonusSpiritMultiplier))); 
            statsTotal.Mana = statsRace.Mana + statsGearEnchantsBuffs.Mana;
            statsTotal.AttackPower = (float)Math.Floor((float)((statsRace.AttackPower + statsGearEnchantsBuffs.AttackPower) * (1f + statsTotal.BonusAttackPowerMultiplier)));
			statsTotal.SpellPower = (statsRace.SpellPower + statsGearEnchantsBuffs.SpellPower) * (1f + statsTotal.BonusSpellPowerMultiplier);
            statsTotal = ApplyTalents(character, statsTotal, null);
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
            return (float)Math.Floor((float)(strength + agility + intBonusToAP));
        }

        private Stats ApplyTalents(Character character, Stats stats, Stats gear) // also includes basic class benefits
        {
            if (gear != null)
            {
                int AK = character.ShamanTalents.AncestralKnowledge;
                float intBase = (float)Math.Floor((float)(stats.Intellect * (1 + stats.BonusIntellectMultiplier) * (1 + .02f * AK))); // added fudge factor because apparently Visual Studio can't multiply 125 * 1.04 to get 130.
                float intBonus = (float)Math.Floor((float)(gear.Intellect * (1 + gear.BonusIntellectMultiplier) * (1 + .02f * AK)));
                stats += gear;
                stats.Intellect = (float)Math.Floor((float)(intBase + intBonus));
            }
            
            stats.Mana += 15f * stats.Intellect;
            stats.Health += 10f * stats.Stamina;
            stats.Expertise += 3 * character.ShamanTalents.UnleashedRage;
            
            int MQ = character.ShamanTalents.MentalQuickness;
            stats.AttackPower += AddAPFromStrAgiInt(character, stats.Strength, stats.Agility, stats.Intellect); 
            stats.AttackPower = (float)Math.Floor((float)(stats.AttackPower * (1f + stats.BonusAttackPowerMultiplier)));
            stats.SpellPower = (float)Math.Floor((float)(stats.SpellPower + (stats.AttackPower * .1f * MQ * (1f + stats.BonusSpellPowerMultiplier))));
            return stats;
        }
        #endregion

        #region Buff Functions
        private bool IsBuffChecked(string buffName)
        {
            TabControl tabs = Calculations.CalculationOptionsPanel.Parent.Parent as TabControl;
            Control buffControl = tabs.TabPages[2].Controls[0];
            Type buffControlType = buffControl.GetType();

            if (buffControlType.FullName == "Rawr.BuffSelector")
            {
                PropertyInfo checkBoxesInfo = buffControlType.GetProperty("BuffCheckBoxes");
                Dictionary<Buff, CheckBox> checkBoxes = checkBoxesInfo.GetValue(buffControl, null) as Dictionary<Buff, CheckBox>;
                foreach (CheckBox checkbox in checkBoxes.Values)
                {
                    Buff buff = checkbox.Tag as Buff;
                    if (buff.Name.Equals(buffName))
                        return checkbox.Checked;
                }
            }
            return false;
        }

        public override void SetDefaults(Character character)
        {
            character.ActiveBuffs.Add(Buff.GetBuffByName("Strength of Earth Totem"));
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
        }
        #endregion

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

        #region Relevant Stats
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

        public override bool IsItemRelevant(Item item)
		{
			if ((item.Slot == Item.ItemSlot.Ranged && item.Type != Item.ItemType.Totem)) 
				return false;
            if (item.Slot == Item.ItemSlot.OffHand && item.Type == Item.ItemType.None)
                return false;
			return base.IsItemRelevant(item);
		}

		public override Stats GetRelevantStats(Stats stats)
		{
			Stats s = new Stats()
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
                    LightningSpellPower = stats.LightningSpellPower,
                    TotemLLAttackPower = stats.TotemLLAttackPower,
                    TotemShockAttackPower = stats.TotemShockAttackPower,
                    TotemShockSpellPower = stats.TotemShockSpellPower,
                    TotemSSHaste = stats.TotemSSHaste,
                    TotemSSDamage = stats.TotemSSDamage,
                    TotemWFAttackPower = stats.TotemWFAttackPower,
                    GreatnessProc = stats.GreatnessProc,
                    BonusLSDamage = stats.BonusLSDamage,
                    BonusFlurryHaste = stats.BonusFlurryHaste,
                    BonusMWFreq = stats.BonusMWFreq,
                    BonusLLSSDamage = stats.BonusLLSSDamage,
                    PhysicalHit = stats.PhysicalHit,
                    PhysicalHaste = stats.PhysicalHaste,
                    PhysicalCrit = stats.PhysicalCrit,
                    SpellHit = stats.SpellHit,
                    SpellHaste = stats.SpellHaste,
                    SpellCrit = stats.SpellCrit
				};
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (effect.Trigger != Trigger.ManaGem || effect.Trigger != Trigger.HealingSpellCast || effect.Trigger != Trigger.HealingSpellCrit ||
                    effect.Trigger != Trigger.HealingSpellHit)
                {
                    if (relevantStats(effect.Stats))
                        s.AddSpecialEffect(effect);
                }
            }
            return s;
		}

		public override bool HasRelevantStats(Stats stats)
		{
            if (relevantStats(stats))
                return true;
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (effect.Trigger != Trigger.ManaGem || effect.Trigger != Trigger.HealingSpellCast || effect.Trigger != Trigger.HealingSpellCrit ||
                    effect.Trigger != Trigger.HealingSpellHit)
                {
                    if (relevantStats(effect.Stats))
                        return true;
                }
            }
            return false;
        }

        private bool relevantStats(Stats stats)
        {
            return (stats.Agility + stats.Intellect + stats.Stamina + stats.Strength + stats.Spirit +
                stats.AttackPower + stats.SpellPower + stats.Mana + stats.WeaponDamage +
                stats.ArmorPenetration + stats.ArmorPenetrationRating + stats.CritMeleeRating + 
                stats.Expertise + stats.ExpertiseRating + stats.HasteRating + stats.CritRating + stats.HitRating + 
                stats.BonusAgilityMultiplier + stats.BonusAttackPowerMultiplier + stats.BonusCritMultiplier + 
                stats.BonusStrengthMultiplier + stats.BonusSpellPowerMultiplier + stats.BonusIntellectMultiplier + 
                stats.BonusSpiritMultiplier + stats.BonusDamageMultiplier + stats.BonusPhysicalDamageMultiplier + 
                stats.BonusNatureDamageMultiplier + stats.BonusFireDamageMultiplier +
                stats.ExposeWeakness + stats.Bloodlust +
                stats.PhysicalCrit + stats.PhysicalHaste + stats.PhysicalHit +
                stats.SpellCrit + stats.SpellHaste + stats.SpellHit + 
                stats.ShatteredSunMightProc + stats.MongooseProc + stats.BerserkingProc + stats.GreatnessProc +
                stats.SpellCritRating + stats.LightningSpellPower + stats.BonusMWFreq + stats.BonusFlurryHaste +
                stats.TotemWFAttackPower + stats.TotemSSHaste +
                stats.TotemShockSpellPower + stats.TotemShockAttackPower + stats.TotemLLAttackPower +
                stats.BonusLSDamage + stats.BonusLLSSDamage + stats.TotemSSDamage) > 0

                &&

                stats.TigersFuryCooldownReduction == 0;
        }
        #endregion

        #region Custom Chart Data
        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            switch (chartName)
            {
                case "Combat Table (White)":
                    CharacterCalculationsEnhance currentCalculationsEnhanceWhite = GetCharacterCalculations(character) as CharacterCalculationsEnhance;
                    ComparisonCalculationEnhance calcMissWhite = new ComparisonCalculationEnhance() { Name = "    Miss    " };
                    ComparisonCalculationEnhance calcDodgeWhite = new ComparisonCalculationEnhance() { Name = "   Dodge   " };
                    ComparisonCalculationEnhance calcCritWhite = new ComparisonCalculationEnhance() { Name = "  Crit  " };
                    ComparisonCalculationEnhance calcGlanceWhite = new ComparisonCalculationEnhance() { Name = " Glance " };
                    ComparisonCalculationEnhance calcHitWhite = new ComparisonCalculationEnhance() { Name = "Hit" };
                    if (currentCalculationsEnhanceWhite != null)
                    {
                        calcMissWhite.OverallPoints = calcMissWhite.DPSPoints = currentCalculationsEnhanceWhite.MissedAttacks;
                        calcDodgeWhite.OverallPoints = calcDodgeWhite.DPSPoints = currentCalculationsEnhanceWhite.DodgedAttacks;
                        calcCritWhite.OverallPoints = calcCritWhite.DPSPoints = currentCalculationsEnhanceWhite.MeleeCrit;
                        calcGlanceWhite.OverallPoints = calcGlanceWhite.DPSPoints = currentCalculationsEnhanceWhite.GlancingBlows;
                        calcHitWhite.OverallPoints = calcHitWhite.DPSPoints = (100f - calcMissWhite.OverallPoints -
                                                     calcDodgeWhite.OverallPoints - calcCritWhite.OverallPoints - calcGlanceWhite.OverallPoints);
                    }
                    return new ComparisonCalculationBase[] { calcMissWhite, calcDodgeWhite, calcCritWhite, calcGlanceWhite, calcHitWhite };

                case "Combat Table (Yellow)":
                    CharacterCalculationsEnhance currentCalculationsEnhanceYellow = GetCharacterCalculations(character) as CharacterCalculationsEnhance;
                    ComparisonCalculationEnhance calcMissYellow = new ComparisonCalculationEnhance() { Name = "    Miss    " };
                    ComparisonCalculationEnhance calcDodgeYellow = new ComparisonCalculationEnhance() { Name = "   Dodge   " };
                    ComparisonCalculationEnhance calcCritYellow = new ComparisonCalculationEnhance() { Name = "  Crit  " };
                    ComparisonCalculationEnhance calcGlanceYellow = new ComparisonCalculationEnhance() { Name = " Glance " };
                    ComparisonCalculationEnhance calcHitYellow = new ComparisonCalculationEnhance() { Name = "Hit" };
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

                case "Relative Gem Values":
                    float dpsBase = GetCharacterCalculations(character).OverallPoints;
                    float dpsStr = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { Strength = 16 } }).OverallPoints - dpsBase);
                    float dpsAgi = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = 16 } }).OverallPoints - dpsBase);
                    float dpsAP = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { AttackPower = 32 } }).OverallPoints - dpsBase);
                    float dpsInt = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { Intellect = 16 } }).OverallPoints - dpsBase);
                    float dpsCrit = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { CritRating = 16 } }).OverallPoints - dpsBase);
                    float dpsExp = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { ExpertiseRating = 16 } }).OverallPoints - dpsBase);
                    float dpsHaste = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { HasteRating = 16 } }).OverallPoints - dpsBase);
                    float dpsHit = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 16 } }).OverallPoints - dpsBase);
                    float dpsDmg = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { WeaponDamage = 1 } }).OverallPoints - dpsBase);
                    float dpsPen = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { ArmorPenetrationRating = 16 } }).OverallPoints - dpsBase);
                    float dpsSpd = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { SpellPower = 19 } }).OverallPoints - dpsBase);
                    float dpsSta = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { Stamina = 24 } }).OverallPoints - dpsBase);

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
						new ComparisonCalculationEnhance() { Name = "16 Armor Penetration", OverallPoints = dpsPen, DPSPoints = dpsPen },
                        new ComparisonCalculationEnhance() { Name = "19 Spellpower", OverallPoints = dpsSpd, DPSPoints = dpsSpd }
					};

                case "MH Weapon Speeds":
                    if (character.MainHand == null)
                        return new ComparisonCalculationBase[0];
                    ComparisonCalculationBase MHonePointFour = CheckWeaponSpeedEffect(character, 1.4f, true);
                    ComparisonCalculationBase MHonePointFive = CheckWeaponSpeedEffect(character, 1.5f, true);
                    ComparisonCalculationBase MHonePointSix = CheckWeaponSpeedEffect(character, 1.6f, true);
                    ComparisonCalculationBase MHonePointSeven = CheckWeaponSpeedEffect(character, 1.7f, true);
                    ComparisonCalculationBase MHtwoPointFour = CheckWeaponSpeedEffect(character, 2.4f, true);
                    ComparisonCalculationBase MHtwoPointFive = CheckWeaponSpeedEffect(character, 2.5f, true);
                    ComparisonCalculationBase MHtwoPointSix = CheckWeaponSpeedEffect(character, 2.6f, true);
                    ComparisonCalculationBase MHtwoPointSeven = CheckWeaponSpeedEffect(character, 2.7f, true);
                    return new ComparisonCalculationBase[] { MHonePointFour, MHonePointFive, MHonePointSix, MHonePointSeven, 
                                                             MHtwoPointFour, MHtwoPointFive, MHtwoPointSix, MHtwoPointSeven };

                case "OH Weapon Speeds":
                    if (character.OffHand == null || character.ShamanTalents.DualWield != 1)
                        return new ComparisonCalculationBase[0];
                    ComparisonCalculationBase OHonePointFour = CheckWeaponSpeedEffect(character, 1.4f, false);
                    ComparisonCalculationBase OHonePointFive = CheckWeaponSpeedEffect(character, 1.5f, false);
                    ComparisonCalculationBase OHonePointSix = CheckWeaponSpeedEffect(character, 1.6f, false);
                    ComparisonCalculationBase OHonePointSeven = CheckWeaponSpeedEffect(character, 1.7f, false);
                    ComparisonCalculationBase OHtwoPointFour = CheckWeaponSpeedEffect(character, 2.4f, false);
                    ComparisonCalculationBase OHtwoPointFive = CheckWeaponSpeedEffect(character, 2.5f, false);
                    ComparisonCalculationBase OHtwoPointSix = CheckWeaponSpeedEffect(character, 2.6f, false);
                    ComparisonCalculationBase OHtwoPointSeven = CheckWeaponSpeedEffect(character, 2.7f, false);
                    return new ComparisonCalculationBase[] { OHonePointFour, OHonePointFive, OHonePointSix, OHonePointSeven, 
                                                             OHtwoPointFour, OHtwoPointFive, OHtwoPointSix, OHtwoPointSeven };

                default:
                    return new ComparisonCalculationBase[0];
            }
        }

        private ComparisonCalculationBase CheckWeaponSpeedEffect(Character character, float newSpeed, bool mainHand)
        {
            float baseSpeed = 0f;
            int minDamage = 0;
            int maxDamage = 0;
            Item newWeapon;
            CharacterCalculationsBase baseCalc = Calculations.GetCharacterCalculations(character);
            Character deltaChar = character.Clone();

            if (mainHand)
            {
                newWeapon = character.MainHand.Item.Clone();
                baseSpeed = character.MainHand.Speed;
                minDamage = character.MainHand.MinDamage;
                maxDamage = character.MainHand.MaxDamage;
            }
            else
            {
                newWeapon = character.OffHand.Item.Clone();
                baseSpeed = character.OffHand.Speed;
                minDamage = character.OffHand.MinDamage;
                maxDamage = character.OffHand.MaxDamage;
            }
            newWeapon.MinDamage = (int)Math.Round(minDamage / baseSpeed * newSpeed);
            newWeapon.MaxDamage = (int)Math.Round(maxDamage / baseSpeed * newSpeed);
            newWeapon.Speed = newSpeed;
            String speed = newSpeed.ToString() + " Speed";
            if (mainHand)
                deltaChar.MainHand = new ItemInstance(newWeapon, character.MainHand.Gem1, character.MainHand.Gem2, character.MainHand.Gem3, character.MainHand.Enchant);
            else
                deltaChar.OffHand = new ItemInstance(newWeapon, character.OffHand.Gem1, character.OffHand.Gem2, character.OffHand.Gem3, character.OffHand.Enchant);
            ComparisonCalculationBase result = Calculations.GetCharacterComparisonCalculations(baseCalc, deltaChar, speed, baseSpeed == newWeapon.Speed);
            result.Item = null;
            return result;
        }

        #endregion
    }
}
