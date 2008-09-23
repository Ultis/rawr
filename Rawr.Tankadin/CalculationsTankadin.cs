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

					"Complex Stats:Chance to be Crit",
					@"Complex Stats:Overall Points*Overall Points are a sum of Mitigation and Survival Points. 
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
					//"Agi Test"
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
						Item.ItemType.TwoHandAxe,
						Item.ItemType.TwoHandMace,
						Item.ItemType.TwoHandSword
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

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
        {
            CalculationOptionsTankadin calcOpts = character.CalculationOptions as CalculationOptionsTankadin;
            Stats stats = GetCharacterStats(character, additionalItem);

            int playerLevel = calcOpts.PlayerLevel;
            int targetLevel = calcOpts.TargetLevel;
            int levelDif = targetLevel - playerLevel;

            CharacterCalculationsTankadin calculatedStats = new CharacterCalculationsTankadin();
            //Threat Modifiers
            float normalThreatMod = 1 / .7f;
            float holyThreatMod = normalThreatMod * 1.9f;

            float damageMulti = 1f + character.PaladinTalents.OneHandedWeaponSpecialization * .02f;

            //Melee Hit Mechanics
            float plusHit = stats.HitRating / 1576f;
            float expertise = .0025f * (character.PaladinTalents.CombatExpertise * 2 + stats.ExpertiseRating / 3.9423f);

            float toMiss = Math.Max(0,(levelDif < 3 ? .05f + levelDif*.005f : .07f + levelDif*.02f) - plusHit);
            float toParry = Math.Max(0,((levelDif < 3 ? 1f : 2f) * (.05f + levelDif * .005f)) - expertise);
            float toDodge = Math.Max(0, (.05f + levelDif * .005f) - expertise);
            float toResist = Math.Max(0, .17f - plusHit);
            float toLand = 1f - toMiss - toParry - toDodge;

            float targetArmor = Math.Max(0, calcOpts.TargetArmor - stats.ArmorPenetration);
            float targetReduction = 1f - targetArmor / (targetArmor + 400f + 85f * (5.5f * targetLevel - 265.5f));

            float bwd = character.MainHand == null ? 0 : ((character.MainHand.MinDamage + character.MainHand.MaxDamage) / 2f);
            float ws = character.MainHand == null ? 2 : character.MainHand.Speed;
            float wd = bwd + stats.AttackPower / 14f * ws;
            float nwd = wd * 2.4f / ws;

            //Hammer of the Righteous
            float hotrDamage = bwd / ws * 3f * damageMulti;
            float hotrThreat = hotrDamage * holyThreatMod;

            //Shield of Righteousness
            //TODO: Add in the correct non-scaling component, I totally made up that number until they release one.
            float shorDamage = (stats.BlockValue + 200f) * damageMulti;
            float shorThreat = shorDamage * holyThreatMod;

            //Avenger's Shield
            float asDamage = (940f + .07f * stats.AttackPower + .07f * stats.SpellDamageRating) * damageMulti;
            float asThreat = asDamage * holyThreatMod;

            //Consecration
            float consDamage = 8f * (113f + .04f * stats.SpellDamageRating + .04f * stats.AttackPower) * damageMulti;
            float consThreat = consDamage * holyThreatMod;

            //Seal of Righteousness
            float sorDamage = ws * (.028f * stats.AttackPower + .055f * stats.SpellDamageRating) * damageMulti;
            float sorDPS = sorDamage * ws * toLand;
            float sorTPS = sorDPS * holyThreatMod;

            //Judgement of Righteousness
            float jorDamage = (1 + .25f * stats.AttackPower + .4f * stats.SpellDamageRating) * damageMulti;
            float jorThreat = jorDamage * holyThreatMod;

            //Holy Shield
            //TODO: Implement correct number of blocks, currently just uses 4
            float hsDamage = (211f + .056f * stats.AttackPower + .09f * stats.SpellDamageRating) * damageMulti;
            float hsProcs = 4;
            float hsTotalDamage =  hsDamage * hsProcs;
            float hsTreat = hsTotalDamage * holyThreatMod;

            //White Attacks
            float whiteDamage = wd * targetReduction * damageMulti;
            float whiteDPS = whiteDamage * ws * toLand;
            float whiteTPS = whiteDPS * normalThreatMod;

            //Threat Rotations: (just 1 for now)
            // #1: 96969 (Level 70 version)
            float rot1Time = 18f;
            float rot1TPS = 3 * hotrThreat + 2 * (hsTreat + jorThreat + consThreat) + (sorTPS + whiteTPS) * rot1Time;
            float rot1DPS = 3 * hotrDamage + 2 * (hsDamage + jorDamage + consDamage) + (sorDPS + whiteDPS) * rot1Time;

            //Incoming Damage Mechanics

            return calculatedStats;
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            Stats statsRace = new Stats() { Health = 3197, Mana = 2673, Stamina = 118, Intellect = 86, Spirit = 88, Agility = 79, DodgeRating = 12.3f };
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
            
            //Calculates bonus from talents
            PaladinTalents talents = character.PaladinTalents;
            float armorBonus = 1f + talents.Toughness * .02f;
            float stamBonus = 1f + talents.SacredDuty * .03f + talents.CombatExpertise * .02f;
            float bvBonus = 1f + talents.Redoubt * .1f;

            Stats statsTotal = statsBaseGear + statsEnchants + statsBuffs + statsRace;
            statsTotal.Strength = (float)Math.Round(statsTotal.Strength * (1 + statsTotal.BonusStrengthMultiplier));
            statsTotal.AttackPower = (float)Math.Round(statsTotal.AttackPower * (1 + statsTotal.BonusAttackPowerMultiplier) + statsTotal.Strength * 2);
            statsTotal.Agility = (float)Math.Round(statsTotal.Agility * (1 + statsTotal.BonusAgilityMultiplier));
            statsTotal.Stamina = (float)Math.Round(statsTotal.Stamina * (1 + statsTotal.BonusStaminaMultiplier) * stamBonus);
            statsTotal.Health = (float)Math.Round(statsTotal.Health + statsTotal.Stamina * 10);
            statsTotal.Armor = (float)Math.Round((statsTotal.Armor + statsTotal.Agility * 2f) * (1 + statsBuffs.BonusArmorMultiplier) * armorBonus);
            statsTotal.BlockValue = (float)Math.Round((statsTotal.BlockValue + statsTotal.Strength / 2f) * (1 + statsTotal.BonusBlockValueMultiplier) * bvBonus);
            statsTotal.SpellDamageRating += statsTotal.Stamina * .1f * talents.TouchedByTheLight;

            return statsTotal;
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
                Health = stats.Health,
                Miss = stats.Miss,
                SpellDamageRating = stats.SpellDamageRating,
                HitRating = stats.HitRating,
                HasteRating = stats.HasteRating,
                ArmorPenetration = stats.ArmorPenetration,
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            return (stats.Agility + stats.Armor + stats.BonusAgilityMultiplier + stats.BonusArmorMultiplier +
                stats.BonusStaminaMultiplier + stats.DefenseRating + stats.DodgeRating + stats.Health +
                stats.Miss + stats.Resilience + stats.Stamina + stats.ParryRating + stats.BlockRating + stats.BlockValue +
                stats.SpellHitRating + stats.SpellDamageRating + stats.HitRating + stats.ArmorPenetration) > 0;
        }
    }

}