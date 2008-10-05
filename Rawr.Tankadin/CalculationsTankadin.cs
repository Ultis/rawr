using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Tankadin
{
    [Rawr.Calculations.RawrModelInfo("Tankadin", "Spell_Holy_AvengersShield", Character.CharacterClass.Paladin)]
    public class CalculationsTankadin : CalculationsBase
    {

        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
        public override CalculationOptionsPanelBase CalculationOptionsPanel
        {
            get
            {
                if (_calculationOptionsPanel == null)
                {
                    _calculationOptionsPanel = new CalculationOptionsPanelTankadin();
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
						"Basic Stats:Armor",
						"Basic Stats:Stamina",
						"Basic Stats:Agility",
						"Basic Stats:Defense",
						"Basic Stats:Block Value",
						"Basic Stats:Attack Power",
						"Basic Stats:Spell Damage",
						"Combat Table:Miss",
						"Combat Table:Dodge",
						"Combat Table:Parry",
						"Combat Table:Block",
						"Combat Table:Crit",
						"Combat Table:Hit",
						"Defensive Stats:Avoidance*Chance to not get hit by an attack.",
						"Defensive Stats:Mitigation*How much you reduced damage taken when hit.",
						"Defensive Stats:Damage Taken*The weighted average damage per hit.",
						"Defensive Stats:Damage When Hit*How much damage you take when hit.",
						"Defensive Stats:Chance to be Crit",
                        "Threat Stats:Total Threat",
                        "Threat Stats:ShoR",
                        "Threat Stats:HotR",
                        "Threat Stats:SoC",
                        "Threat Stats:JoC",
                        "Threat Stats:Consecrate",
                        "Threat Stats:Holy Shield",
                        "Threat Stats:White Damage",
                        "Threat Stats:Avenger's Shield",
                        "Threat Stats:SoR",
                        "Threat Stats:JoR",
                        "Threat Stats:Chance to Hit",
                        "Threat Stats:Chance to Crit"
/*						@"Complex Stats:Overall Points*Overall Points are a sum of Mitigation and Survival Points. 
Overall is typically, but not always, the best way to rate gear. 
For specific encounters, closer attention to Mitigation and 
Survival Points individually may be important.",
						@"Complex Stats:Mitigation Points*Mitigation Points represent the amount of damage you mitigate, 
on average, through armor mitigation and avoidance. It is directly 
relational to your Damage Taken. Ideally, you want to maximize 
Mitigation Points, while maintaining 'enough' Survival Points 
(see Survival Points). If you find yourself dying due to healers 
running OOM, or being too busy healing you and letting other 
raid members die, then focus on Mitigation Points.",
						@"Complex Stats:Survival Points*Survival Points represents the total raw physical damage 
(pre-mitigation) you can take before dying. Unlike 
Mitigation Points, you should not attempt to maximize this, 
but rather get 'enough' of it, and then focus on Mitigation. 
'Enough' can vary greatly by fight and by your healers, but 
keeping it roughly even with Mitigation Points is a good 
way to maintain 'enough' as you progress. If you find that 
you are being killed by burst damage, focus on Survival Points.",
						@"Complex Stats:Threat Points*How much threat per secound you do."*/
				};
                return _characterDisplayCalculationLabels;
            }
        }

        private string[] _customChartNames = null;
        public override string[] CustomChartNames
        {
            get
            {
                if (_customChartNames == null)
                    _customChartNames = new string[] {
					//"Combat Table",

					//"Relative Stat Values",

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
                    _subPointNameColors.Add("Mitigation", System.Drawing.Color.Red);
                    _subPointNameColors.Add("Survival", System.Drawing.Color.Blue);
                    _subPointNameColors.Add("Threat", System.Drawing.Color.DarkOliveGreen);
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
                        Item.ItemType.Plate,
                        Item.ItemType.None,
						Item.ItemType.Shield,
						Item.ItemType.Libram,
						Item.ItemType.OneHandAxe,
						Item.ItemType.OneHandMace,
						Item.ItemType.OneHandSword,
					});
                }
                return _relevantItemTypes;
            }
        }

        public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.Paladin; } }
        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationTankadin(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsTankadin(); }

        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsTankadin));
            System.IO.StringReader reader = new System.IO.StringReader(xml);
            CalculationOptionsTankadin calcOpts = serializer.Deserialize(reader) as CalculationOptionsTankadin;
            return calcOpts;
        }

        private static float[] AgiTable = { .000403f, .000372f, .000345f, .000322f, .000298f, .000277f, .000257f, .000239f, .000223f, .0002207f, .000192f };
        private float ConvertAgi(float agi, int level)
        {
            if (level >= 70 && level <= 80) return agi * AgiTable[level - 70];
            else return 0;
        }

        private static float[] IntTable = { .000125f, .000116f, .000108f, .000101f, .000093f, .000087f, .000081f, .000075f, .00007f, .000065f, .00006f };
        private float ConvertInt(float intel, int level)
        {
            if (level >= 70 && level <= 80) return intel * IntTable[level - 70];
            else return 0;
        
        }
        private float ConvertCrit(float crit, int level) { return ConvertBaseRating(crit, level, 14); }
        private float ConvertDodge(float dodge, int level) { return ConvertBaseRating(dodge, level, 12); }
        private float ConvertDefense(float defense, int level) { return ConvertBaseRating(defense, level, 1.5f) * 100f; }
        private float ConvertParry(float parry, int level) { return ConvertBaseRating(parry, level, 15); }
        private float ConvertBlock(float block, int level) { return ConvertBaseRating(block, level, 5); }
        private float ConvertHit(float hit, int level) { return ConvertBaseRating(hit, level, 10); }
        private float ConvertSpellHit(float hit, int level) { return ConvertBaseRating(hit, level, 8); }
        private float ConvertExpertise(float expertise, int level) { return ConvertBaseRating(expertise, level, 10) * 100f; }

        private float ConvertBaseRating(float rating, int level, float baseRating) { 
            return rating / baseRating / (8200f / 52f * (float)Math.Pow(131d / 63d, (level - 70d) / 10d));
        }

        private float DRDodge(float dodge)
        {
            if (dodge <= 0) return 0;
            return .01f / (1 / 88.1290208866f + .00956f / dodge);
        }

        private float DRParry(float parry)
        {
            if (parry <= 0) return 0;
            return .01f / (1 / 47.003525644f + .00956f / parry);
        }


        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
        {
            CalculationOptionsTankadin calcOpts = character.CalculationOptions as CalculationOptionsTankadin;
            Stats stats = GetCharacterStats(character, additionalItem);
			PaladinTalents talents = character.PaladinTalents;

            int playerLevel = calcOpts.PlayerLevel;
            int targetLevel = calcOpts.TargetLevel;
            int levelDif = targetLevel - playerLevel;

            CharacterCalculationsTankadin cs = new CharacterCalculationsTankadin();
			cs.BasicStats = stats;

            float defDif = (targetLevel - playerLevel) * .002f;
            cs.Defense = stats.Defense;
            cs.Miss = Math.Min(1f, .05f + cs.Defense * .0004f - defDif);
            cs.Dodge = Math.Min(1f - cs.Miss, stats.Dodge - defDif);
            cs.Parry = Math.Min(1f - cs.Miss - cs.Dodge, stats.Parry - defDif);
            cs.Avoidance = cs.Miss + cs.Dodge + cs.Parry;
            cs.Block = Math.Min(1f - cs.Avoidance, stats.Block + .3f * talents.HolyShield - defDif);
            cs.CritAvoidance = cs.Defense * .0004f - defDif;
            cs.Crit = Math.Max(0, Math.Min(1f - cs.Avoidance - cs.Block, .05f - cs.CritAvoidance));
            cs.Hit = 1f - cs.Avoidance - cs.Block - cs.Crit;
            cs.BlockValue = stats.BlockValue;

            cs.ArmorReduction = Math.Min(.75f, stats.Armor / (stats.Armor + 400f + 85f * (5.5f * targetLevel - 265.5f)));
            cs.Mitigation = (1f - cs.ArmorReduction) * (1f - .02f * talents.ImprovedRighteousFury) * (1f - .01f * talents.ShieldOfTheTemplar);

            cs.DamagePerHit = calcOpts.AverageHit * cs.Mitigation;
            cs.DamagePerBlock = cs.DamagePerHit - stats.BlockValue;
            cs.DamageTaken = ((cs.Crit * 2 + cs.Hit) * cs.DamagePerHit + cs.Block * cs.DamagePerBlock) / calcOpts.AverageHit;

            cs.MitigationPoints = calcOpts.MitigationScale / cs.DamageTaken;

            float beHit = cs.Block + cs.Crit + cs.Hit;
            cs.DamageWhenHit = cs.Block / beHit * cs.DamagePerBlock + cs.Hit / beHit * cs.DamagePerHit + cs.Crit / beHit * cs.DamagePerHit * 2;
            cs.SurvivalPoints = stats.Health / cs.DamageWhenHit * calcOpts.AverageHit * (1f + .02571f * talents.ArdentDefender);
			
            //Threat Calculations
            float normalThreatMod = 1 / .7f * (1f + stats.ThreatIncreaseMultiplier);
            float holyThreatMod = normalThreatMod * 1.9f;
            float SotT = 1f + talents.ShieldOfTheTemplar * .1f;
            float holyMulti = 1f;// stats.BonusHolySpellPowerMultiplier;
            float damageMulti = 1f + talents.OneHandedWeaponSpecialization * .02f;
            float expertise = .0025f * stats.Expertise;

            cs.ToMiss = Math.Max(0,(levelDif < 3 ? .05f + levelDif*.005f : .07f + (levelDif-2)*.02f) - stats.Hit);
            cs.ToParry = Math.Max(0, ((levelDif < 3 ? 1f : 2f) * (.05f + levelDif * .005f)) - expertise);
            cs.ToDodge = Math.Max(0, (.05f + levelDif * .005f) - expertise);
            cs.ToResist = Math.Min(1, .83f + stats.SpellHit);
            cs.ToLand = 1f - cs.ToMiss - cs.ToParry - cs.ToDodge;
            cs.ToCrit = stats.Crit;
            cs.ToSpellCrit = stats.SpellCrit;

            float targetArmor = Math.Max(0, calcOpts.TargetArmor - stats.ArmorPenetration);
            float targetReduction = 1f - targetArmor / (targetArmor + 400f + 85f * (5.5f * targetLevel - 265.5f));

            float bwd = character.MainHand == null ? 73 : ((character.MainHand.MinDamage + character.MainHand.MaxDamage) / 2f);
            float ws = character.MainHand == null ? 1.8f : character.MainHand.Speed;
            float wd = bwd + stats.AttackPower / 14f * ws;
            float nwd = wd * 2.4f / ws;

            //TODO Implement better reckoning uptime estimator
            float reckoning = .25f;

            //Hammer of the Righteous
            if (talents.HammerOfTheRighteous > 0)
            {
                cs.HotRDamage = bwd / ws * 4f * damageMulti * holyMulti;
                cs.HotRThreat = cs.HotRDamage * holyThreatMod * (cs.ToLand + cs.ToCrit);
            }

            //Shield of Righteousness
            if (playerLevel >= 75)
            {
                cs.ShoRDamage = (stats.BlockValue + 300f) * damageMulti * SotT * holyMulti;
                cs.ShoRThreat = cs.ShoRDamage * holyThreatMod * (cs.ToLand + cs.ToCrit);
            }

            //Avenger's Shield
            if (talents.AvengersShield > 0)
            {
                cs.ASDamage = ((playerLevel >= 80 ? 940f : 680f) + .07f * stats.AttackPower + .07f * stats.SpellPower) * damageMulti * SotT * holyMulti;
                cs.ASThreat = cs.ASDamage * holyThreatMod * (1f - cs.ToMiss + cs.ToCrit);
            }

            //Consecration
            cs.ConsDuration = 8f;
            cs.ConsDamage = cs.ConsDuration * ((playerLevel >= 75 ? (playerLevel >= 80 ? 113f : 87f) : 72f)
                + .04f * stats.SpellPower + .04f * stats.AttackPower) * damageMulti * holyMulti;
            cs.ConsThreat = cs.ConsDamage * holyThreatMod;

            //Seal of Righteousness
            cs.SoRDamage = ws * (.028f * stats.AttackPower + .055f * stats.SpellPower) * damageMulti * holyMulti * (1 + .03f * talents.SealsOfThePure);
            cs.SoRThreat = cs.SoRDamage / ws * cs.ToLand * holyThreatMod * (1f + reckoning);

            //Judgement of Righteousness
            cs.JoRDamage = (1 + .25f * stats.AttackPower + .4f * stats.SpellPower) * damageMulti * holyMulti * (1 + .03f * talents.SealsOfThePure);
            cs.JoRThreat = cs.JoRDamage * holyThreatMod * (cs.ToResist + cs.ToSpellCrit);

            //Seal of Corruption
            cs.SoCDamage = (0.016f * stats.SpellPower + 0.032f * stats.AttackPower) * damageMulti * 5f * holyMulti * (1 + .03f * talents.SealsOfThePure);
            cs.SoCThreat = cs.SoCDamage * holyThreatMod / 3f;

            //Judgement of Corruption
            cs.JoCDamage = (1f + 0.28f * stats.SpellPower + 0.175f * stats.AttackPower) * 1.5f * damageMulti * holyMulti * (1 + .03f * talents.SealsOfThePure);
            cs.JoCThreat = cs.JoCDamage * holyThreatMod * (cs.ToResist + cs.ToSpellCrit);

            //Holy Shield
            //TODO Implement correct number of blocks, currently just uses 4
            if (talents.HolyShield > 0)
            {
                cs.HSDamage = ((playerLevel >= 75 ? (playerLevel >= 80 ? 211f : 181f) : 160f)
                    + .056f * stats.AttackPower + .09f * stats.SpellPower) * damageMulti * SotT * holyMulti;
                cs.HSProcs = 4;
                cs.HSThreat = cs.HSDamage * cs.HSProcs * (cs.ToResist + cs.ToSpellCrit) * holyThreatMod;
            }

            //White Attacks
            cs.WhiteDamage = wd * targetReduction * damageMulti;
            cs.WhiteThreat = cs.WhiteDamage / ws * cs.ToLand * normalThreatMod * (1f + reckoning);

            //Threat Rotations: (just 1 for now)

            // #1: 96969 (Level 70 version)

            float rot1Time = 18f;
            cs.Rot1TPS = (3 * cs.HotRThreat + 2 * (cs.HSThreat + cs.JoCThreat + cs.ConsThreat) + (cs.SoCThreat + cs.WhiteThreat) * rot1Time) / rot1Time;
            float rot1DPS = (3 * cs.HotRDamage + 2 * (cs.HSDamage + cs.JoCDamage + cs.ConsDamage) + (cs.SoCDamage + cs.WhiteDamage) * rot1Time) / rot1Time;

            cs.ThreatPoints = cs.Rot1TPS * calcOpts.ThreatScale;

            //Incoming Damage Mechanics



            cs.OverallPoints = cs.ThreatPoints + cs.MitigationPoints + cs.SurvivalPoints;
            return cs;
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            PaladinTalents talents = character.PaladinTalents;
            CalculationOptionsTankadin calcOpts = character.CalculationOptions as CalculationOptionsTankadin;
            int lvl = calcOpts.PlayerLevel;

            Stats statsRace;
            switch (character.Race)
            {
                case Character.CharacterRace.BloodElf:
                    statsRace = new Stats() { Strength = 123f, Agility = 79f, Stamina = 118f, Intellect = 87f, Spirit = 88f };
                    break;
                case Character.CharacterRace.Draenei:
                    statsRace = new Stats() { Strength = 127f, Agility = 74f, Stamina = 119f, Intellect = 84f, Spirit = 89f, Hit = .01f, SpellHit = .01f };
                    break;
                case Character.CharacterRace.Human:
                    statsRace = new Stats() { Strength = 126f, Agility = 77f, Stamina = 120f, Intellect = 83f, Spirit = 89f, BonusSpiritMultiplier = 0.1f, };
                    //Expertise for Humans
                    if (character.MainHand != null && (character.MainHand.Type == Item.ItemType.OneHandMace || character.MainHand.Type == Item.ItemType.OneHandSword))
                        statsRace.Expertise = 5f;
                    break;
                default: //defaults to Dwarf stats
                    statsRace = new Stats() { Strength = 128f, Agility = 73f, Stamina = 120f, Intellect = 83f, Spirit = 89f, };
                    break;
            }
            statsRace.Dodge = .032685f;
            statsRace.Parry = .05f;
            statsRace.AttackPower = 190f;
            statsRace.Health = 3197f;
            statsRace.Mana = 2673f;

            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            Stats stats = statsBaseGear + statsEnchants + statsBuffs + statsRace;
            stats.Strength = (float)Math.Round(stats.Strength * (1 + stats.BonusStrengthMultiplier)) * (1f + talents.DivineStrength * .03f);
            stats.AttackPower = (float)Math.Round((stats.AttackPower + stats.Strength * 2) * (1 + stats.BonusAttackPowerMultiplier));
            stats.Agility = (float)Math.Round(stats.Agility * (1 + stats.BonusAgilityMultiplier));
            stats.Stamina = (float)Math.Round(stats.Stamina * (1 + stats.BonusStaminaMultiplier) * (1f + talents.SacredDuty * .03f) * (1f + talents.CombatExpertise * .02f));
            stats.Health = (float)Math.Round(stats.Health + stats.Stamina * 10);
            stats.Armor = (float)Math.Round((stats.Armor + stats.Agility * 2f) * (1 + statsBuffs.BonusArmorMultiplier) * (1f + talents.Toughness * .02f));

            stats.Hit = ConvertHit(stats.HitRating, lvl);
            stats.SpellHit = ConvertSpellHit(stats.HitRating, lvl) + stats.SpellHit; 
            stats.Expertise += (float)Math.Round(talents.CombatExpertise * 2 + ConvertExpertise(stats.ExpertiseRating, lvl));

            float talentCrit = talents.CombatExpertise * .02f + talents.Conviction * .01f + talents.SanctifiedSeals * .01f;
            stats.Crit = stats.Crit + ConvertCrit(stats.CritRating, lvl) + ConvertAgi(stats.Agility, lvl) + talentCrit;
            stats.SpellCrit = stats.SpellCrit + ConvertCrit(stats.CritRating, lvl) + ConvertInt(stats.Intellect, lvl) + talentCrit;

            stats.Defense += ConvertDefense(stats.DefenseRating, lvl);
            stats.BlockValue = (float)Math.Round((stats.BlockValue + stats.Strength / 2f) * (1 + stats.BonusBlockValueMultiplier) * (1f + talents.Redoubt * .1f));
            stats.Block += .05f + stats.Defense * .0004f + ConvertBlock(stats.BlockRating, lvl);

            float fullDodge = stats.Defense * .0004f + ConvertAgi(stats.Agility - statsRace.Agility, lvl) + ConvertDodge(stats.DodgeRating, lvl);
            stats.Dodge = stats.Dodge + character.PaladinTalents.Anticipation * .01f + ConvertAgi(statsRace.Agility, lvl) + DRDodge(fullDodge);

            float fullParry = stats.Defense * .0004f + ConvertParry(stats.ParryRating, lvl);
            stats.Parry = stats.Parry + character.PaladinTalents.Deflection * .01f + DRParry(fullParry);

            stats.SpellPower += stats.Stamina * .1f * talents.TouchedByTheLight;
            return stats;
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            return new ComparisonCalculationBase[0];
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            return new Stats()
            {
                Armor = stats.Armor,
                Stamina = stats.Stamina,
                Agility = stats.Agility,
                Strength = stats.Strength,
                AttackPower = stats.AttackPower,
                DodgeRating = stats.DodgeRating,
                DefenseRating = stats.DefenseRating,
                Resilience = stats.Resilience,
                ExpertiseRating = stats.ExpertiseRating,
                ParryRating = stats.ParryRating,
                BlockRating = stats.BlockRating,
                BlockValue = stats.BlockValue,
                BonusStrengthMultiplier = stats.BonusStrengthMultiplier,
                BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
                BonusArmorMultiplier = stats.BonusArmorMultiplier,
                BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,
                BonusBlockValueMultiplier = stats.BonusBlockValueMultiplier,
                ThreatIncreaseMultiplier = stats.ThreatIncreaseMultiplier,
                Health = stats.Health,
                Miss = stats.Miss,
                SpellPower = stats.SpellPower,
                HitRating = stats.HitRating,
                HasteRating = stats.HasteRating,
                CritRating = stats.CritRating,
                ArmorPenetration = stats.ArmorPenetration,
               // BonusHolySpellPowerMultiplier = stats.BonusHolySpellPowerMultiplier,
                WeaponDamage = stats.WeaponDamage
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            return (stats.Agility + stats.Armor + stats.BonusAgilityMultiplier + stats.BonusArmorMultiplier + stats.BonusAttackPowerMultiplier +
                stats.BonusStaminaMultiplier + stats.DefenseRating + stats.DodgeRating + stats.Health + //stats.BonusHolySpellPowerMultiplier +
                stats.Miss + stats.Resilience + stats.Stamina + stats.ParryRating + stats.BlockRating + stats.BlockValue +
                stats.SpellHitRating + stats.SpellPower + stats.HitRating + stats.ArmorPenetration + stats.WeaponDamage + stats.Strength +
                stats.AttackPower + stats.ThreatIncreaseMultiplier + stats.CritRating) > 0;
        }
    }

}