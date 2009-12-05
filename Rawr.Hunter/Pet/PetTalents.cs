using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Rawr.Hunter
{
    public class PetTalentData {
        /// <summary>A Pet Talent</summary>
        public PetTalentData() {
            ID = -1;
            Name = "Invalid";
            Max = 1;
            Desc = new string[] { "" };
        }
        /// <summary>A Pet Talent</summary>
        public PetTalentData(int id, string name, int max, string[] desc) {
            ID = id;
            Name = name;
            Max = max;
            Desc = desc;
        }
        /* sadface, This doesn't work
        public static PetTalent operator= (PetTalent p, int rank) {
            return new PetTalent(p.Id, p.Name, rank, p.Desc);
        }*/
        /// <summary>The Unique ID of this Talent</summary>
        public int ID;
        /// <summary>The Name of the Talent</summary>
        public string Name;
        /// <summary>The Maximum Number of Ranks of the Talent</summary>
        public int Max;
        /// <summary>
        /// List of Descriptions, each point in the array is
        /// the description of the matching Rank number.
        /// </summary>
        public string[] Desc;

        public override string ToString() {
            try{
 	            return Name + " [" + Max.ToString() + "]\r\n" + Desc[0];
            }catch (Exception){
                return "Failed to convert PetTalentData to string.";
            }
        }
    }

    public class PetTalent : PetTalentData {
        /// <summary>A Pet Talent</summary>
        public PetTalent() {
            Value = 0;
        }
        /// <summary>A Pet Talent</summary>
        public PetTalent(PetTalentData Base, int value) {
            ID = Base.ID;
            Name = Base.Name;
            Max = Base.Max;
            Desc = Base.Desc;

            Value = value;
        }
        /// <summary>A Pet Talent</summary>
        public PetTalent(int id, string name, int value, int max, string[] desc) {
            ID = id;
            Name = name;
            Value = value;
            Max = max;
            Desc = desc;
        }
        /// <summary>The Current Rank of the Talent</summary>
        public int Value;

        public override string ToString() {
            try{
 	            return Name + " [" + Value.ToString() + "/" + Max.ToString() + "]\r\n" + Desc[0];
            }catch (Exception){
                return "Failed to convert PetTalent to string.";
            }
        }
    }

    public static class PetTalentsBase {
        /// <summary>Reduces the damage your pet takes from area of effect attacks by [25*Pts]%.</summary>
        public readonly static PetTalentData Avoidance = new PetTalentData(0, "Avoidance", 3, new string[] {
            @"Reduces the damage your pet takes from area of effect attacks by [25*Pts]%.",
            @"Reduces the damage your pet takes from area of effect attacks by 25%.",
            @"Reduces the damage your pet takes from area of effect attacks by 50%.",
            @"Reduces the damage your pet takes from area of effect attacks by 75%." });
        /// <summary>Increases your pet's total Stamina by [2*Pts]% and increases all healing effects on your pet by [20*Pts]%.</summary>
        public readonly static PetTalentData BloodOfTheRhino = new PetTalentData(1, "Blood Of The Rhino", 2, new string[] {
            @"Increases your pet's total Stamina by [2*Pts]% and increases all healing effects on your pet by [20*Pts]%.",
            @"Increases your pet's total Stamina by 2% and increases all healing effects on your pet by 20%.",
            @"Increases your pet's total Stamina by 4% and increases all healing effects on your pet by 40%." });
        /// <summary>Your pet's attacks have a [10*Pts]% chance to increase its happiness by 5% and heal 5% of its total health.</summary>
        public readonly static PetTalentData Bloodthirsty = new PetTalentData(2, "Bloodthirsty", 2, new string[] {
            @"Your pet's attacks have a [10*Pts]% chance to increase its happiness by 5% and heal 5% of its total health.",
            @"Your pet's attacks have a 10% chance to increase its happiness by 5% and heal 5% of its total health.",
            @"Your pet's attacks have a 20% chance to increase its happiness by 5% and heal 5% of its total health." });
        /// <summary>Increases your pet's movement speed by 30%.</summary>
        public readonly static PetTalentData BoarsSpeed = new PetTalentData(3, "Boar's Speed", 1, new string[] {
            @"Increases your pet's movement speed by 30%.",
            @"Increases your pet's movement speed by 30%." });
        /// <summary>Removes all movement impairing effects and all effects which cause loss of control of your pet, and reduces damage done to your pet by 20% for 12 sec.</summary>
        public readonly static PetTalentData Bullheaded = new PetTalentData(4, "Bullheaded", 1, new string[] {
            @"Removes all movement impairing effects and all effects which cause loss of control of your pet, and reduces damage done to your pet by 20% for 12 sec.",
            @"Removes all movement impairing effects and all effects which cause loss of control of your pet, and reduces damage done to your pet by 20% for 12 sec." });
        /// <summary>Your pet roars, increasing your pet's and your melee and ranged attack power by 10%. Lasts 20 sec. 5 min cooldown.</summary>
        public readonly static PetTalentData CallOfTheWild = new PetTalentData(5, "Call Of The Wild", 1, new string[] {
            @"Your pet roars, increasing your pet's and your melee and ranged attack power by 10%. Lasts 20 sec. 5 min cooldown.",
            @"Your pet roars, increasing your pet's and your melee and ranged attack power by 10%. Lasts 20 sec. 5 min cooldown." });
        /// <summary>Your pet can generate health and happiness by eating a corpse. Will not work on the remains of elemental or mechanical creatures.</summary>
        public readonly static PetTalentData CarrionFeeder = new PetTalentData(6, "Carrion Feeder", 1, new string[] {
            @"Your pet can generate health and happiness by eating a corpse. Will not work on the remains of elemental or mechanical creatures.",
            @"Your pet can generate health and happiness by eating a corpse. Will not work on the remains of elemental or mechanical creatures." });
        /// <summary>Your pet charges an enemy, immobilizing the target for 1 sec, and increasing the pet's melee attack power by 25% for its next attack.</summary>
        public readonly static PetTalentData ChargeSwoop = new PetTalentData(7, "Charge/Swoop", 1, new string[] {
            @"Your pet charges an enemy, immobilizing the target for 1 sec, and increasing the pet's melee attack power by 25% for its next attack.",
            @"Your pet charges an enemy, immobilizing the target for 1 sec, and increasing the pet's melee attack power by 25% for its next attack." });
        /// <summary>Increases your pet's attack speed by [15*Pts]%. Your pet will hit faster but each hit will do less damage.</summary>
        public readonly static PetTalentData CobraReflexes = new PetTalentData(8, "Cobra Reflexes", 2, new string[] {
            @"Increases your pet's attack speed by [15*Pts]%. Your pet will hit faster but each hit will do less damage.",
            @"Increases your pet's attack speed by 15%. Your pet will hit faster but each hit will do less damage.",
            @"Increases your pet's attack speed by 30%. Your pet will hit faster but each hit will do less damage." });
        /// <summary>When at less than 35% health, your pet does [25*Pts]% more damage and has a [30*Pts]% reduced chance to eb critically hit.</summary>
        public readonly static PetTalentData Cornered = new PetTalentData(9, "Cornered", 2, new string[] {
            @"When at less than 35% health, your pet does [25*Pts]% more damage and has a [30*Pts]% reduced chance to be critically hit.",
            @"When at less than 35% health, your pet does 25% more damage and has a 30% reduced chance to be critically hit.",
            @"When at less than 35% health, your pet does 50% more damage and has a 60% reduced chance to be critically hit." });
        /// <summary>Increases your pet's movement speed by 80% for 16 sec.</summary>
        public readonly static PetTalentData DiveDash = new PetTalentData(10, "Dive/Dash", 1, new string[] {
            @"Increases your pet's movement speed by 80% for 16 sec.",
            @"Increases your pet's movement speed by 80% for 16 sec." });
        /// <summary>Your pet does [8*Pts]% additional damage to targets with les than 35% health.</summary>
        public readonly static PetTalentData FeedingFrenzy = new PetTalentData(11, "Feeding Frenzy", 2, new string[] {
            @"Your pet does [8*Pts]% additional damage to targets with les than 35% health.",
            @"Your pet does 8% additional damage to targets with les than 35% health.",
            @"Your pet does 16% additional damage to targets with les than 35% health." });
        /// <summary>Reduces the chance your pet will be critically hit by melee attacks by [2*Pts]%.</summary>
        public readonly static PetTalentData GraceOfTheMantis = new PetTalentData(12, "Grace Of The Mantis", 2, new string[] {
            @"Reduces the chance your pet will be critically hit by melee attacks by [2*Pts]%.",
            @"Reduces the chance your pet will be critically hit by melee attacks by 2%.",
            @"Reduces the chance your pet will be critically hit by melee attacks by 4%." });
        /// <summary>Your pet takes [5*Pts]% less damage from Arcane, Fire, Frost, Nature and Shadow magic.</summary>
        public readonly static PetTalentData GreatResistance = new PetTalentData(13, "Great Resistance", 3, new string[] {
            @"Your pet takes [5*Pts]% less damage from Arcane, Fire, Frost, Nature and Shadow magic.",
            @"Your pet takes 5% less damage from Arcane, Fire, Frost, Nature and Shadow magic.",
            @"Your pet takes 10% less damage from Arcane, Fire, Frost, Nature and Shadow magic.",
            @"Your pet takes 15% less damage from Arcane, Fire, Frost, Nature and Shadow magic." });
        /// <summary>Increases your pet's total Stamina by [4*Pts]%.</summary>
        public readonly static PetTalentData GreatStamina = new PetTalentData(14, "Great Stamina", 3, new string[] {
            @"Increases your pet's total Stamina by [4*Pts]%.",
            @"Increases your pet's total Stamina by 4%.",
            @"Increases your pet's total Stamina by 8%.",
            @"Increases your pet's total Stamina by 12%." });
        /// <summary>Your pet's Growl generates [10*Pts]% additional threat and 10% of it's total happiness.</summary>
        public readonly static PetTalentData GuardDog = new PetTalentData(15, "Guard Dog", 2, new string[] {
            @"Your pet's Growl generates [10*Pts]% additional threat and 10% of it's total happiness.",
            @"Your pet's Growl generates 10% additional threat and 10% of it's total happiness.",
            @"Your pet's Growl generates 20% additional threat and 10% of it's total happiness." });
        /// <summary>When your pet dies, it will miraculously return to life with full health.</summary>
        public readonly static PetTalentData HeartOfThePhoenix = new PetTalentData(16, "Heart Of The Phoenix", 1, new string[] {
            @"When your pet dies, it will miraculously return to life with full health.",
            @"When your pet dies, it will miraculously return to life with full health." });
        /// <summary>Your pet's Cower also decreases damage taken by [10*Pts]% for the next 10 sec.</summary>
        public readonly static PetTalentData ImprovedCower = new PetTalentData(17, "Improved Cower", 2, new string[] {
            @"Your pet's Cower also decreases damage taken by [10*Pts]% for the next 10 sec.",
            @"Your pet's Cower also decreases damage taken by 10% for the next 10 sec.",
            @"Your pet's Cower also decreases damage taken by 20% for the next 10 sec." });
        /// <summary>Your pet runs at high speed towards a group member, intercepting the next melee or ranged attack made against them.</summary>
        public readonly static PetTalentData Intervene = new PetTalentData(18, "Intervene", 1, new string[] {
            @"Your pet runs at high speed towards a group member, intercepting the next melee or ranged attack made against them.",
            @"Your pet runs at high speed towards a group member, intercepting the next melee or ranged attack made against them." });
        /// <summary>Your pet temporarily gains 30% of it's maximum health for 20 sec. After the effect expires, the health is lost.</summary>
        public readonly static PetTalentData LastStand = new PetTalentData(19, "Last Stand", 1, new string[] {
            @"Your pet temporarily gains 30% of it's maximum health for 20 sec. After the effect expires, the health is lost.",
            @"Your pet temporarily gains 30% of it's maximum health for 20 sec. After the effect expires, the health is lost." });
        /// <summary>Your pet heals itself for 100% of its total health over 5 sec while channeling.</summary>
        public readonly static PetTalentData LickYourWounds = new PetTalentData(20, "Lick Your Wounds", 1, new string[] {
            @"Your pet heals itself for 100% of its total health over 5 sec while channeling.",
            @"Your pet heals itself for 100% of its total health over 5 sec while channeling." });
        /// <summary>Reduces the duration of all Stun and Fear effects used against your pet by [15*Pts]%.</summary>
        public readonly static PetTalentData Lionhearted = new PetTalentData(21, "Lionhearted", 2, new string[] {
            @"Reduces the duration of all Stun and Fear effects used against your pet by [15*Pts]%.",
            @"Reduces the duration of all Stun and Fear effects used against your pet by 15%.",
            @"Reduces the duration of all Stun and Fear effects used against your pet by 30%." });
        /// <summary>Reduces the cooldown on your pet's Dive/Dash ability by [8*Pts] sec.</summary>
        public readonly static PetTalentData Mobility = new PetTalentData(22, "Mobility", 2, new string[] {
            @"Reduces the cooldown on your pet's Dive/Dash ability by [8*Pts] sec.",
            @"Reduces the cooldown on your pet's Dive/Dash ability by 8 sec.",
            @"Reduces the cooldown on your pet's Dive/Dash ability by 16 sec." });
        /// <summary>Increases your pet's armor by [5*Pts]%.</summary>
        public readonly static PetTalentData NaturalArmor = new PetTalentData(23, "Natural Armor", 2, new string[] {
            @"Increases your pet's armor by [5*Pts]%.",
            @"Increases your pet's armor by 5%.",
            @"Increases your pet's armor by 10%." });
        /// <summary>Your pet has a [15*Pts]% chance after using an ability that the next ability will cost no Focus if used within 8 sec.</summary>
        public readonly static PetTalentData OwlsFocus = new PetTalentData(24, "Owl's Focus", 2, new string[] {
            @"Your pet has a [15*Pts]% chance after using an ability that the next ability will cost no Focus if used within 8 sec.",
            @"Your pet has a 15% chance after using an ability that the next ability will cost no Focus if used within 8 sec.",
            @"Your pet has a 30% chance after using an ability that the next ability will cost no Focus if used within 8 sec." });
        /// <summary>Increases your pet's armor by [5*Pts]% and chance to Dodge by [1*Pts]%.</summary>
        public readonly static PetTalentData PetBarding = new PetTalentData(25, "Pet Barding", 2, new string[] {
            @"Increases your pet's armor by [5*Pts]% and chance to Dodge by [1*Pts]%.",
            @"Increases your pet's armor by 5% and chance to Dodge by 1%.",
            @"Increases your pet's armor by 10% and chance to Dodge by 2%." });
        /// <summary>Your pet goes into a killing frenzy. Successful attacks have a chance to increase attack power by 5%. This effect will stack up to 5 times. Lasts 20 sc.</summary>
        public readonly static PetTalentData Rabid = new PetTalentData(26, "Rabid", 1, new string[] {
            @"Your pet goes into a killing frenzy. Successful attacks have a chance to increase attack power by 5%. This effect will stack up to 5 times. Lasts 20 sc.",
            @"Your pet goes into a killing frenzy. Successful attacks have a chance to increase attack power by 5%. This effect will stack up to 5 times. Lasts 20 sc." });
        /// <summary>Your pet's inspiring roar restores 30% of your total mana over 9 sec.</summary>
        public readonly static PetTalentData RoarOfRecovery = new PetTalentData(27, "Roar Of Recovery", 2, new string[] {
            @"Your pet's inspiring roar restores 30% of your total mana over 9 sec.",
            @"Your pet's inspiring roar restores 30% of your total mana over 9 sec." });
        /// <summary>Protects a friendly target from critical strikes, making attacks against that target unable to be critical strikes, but 20% of all damage taken by that target is also taken by the pet. Lasts 12 sec.</summary>
        public readonly static PetTalentData RoarOfSacrifice = new PetTalentData(28, "Roar Of Sacrifice", 1, new string[] {
            @"Protects a friendly target from critical strikes, making attacks against that target unable to be critical strikes, but 20% of all damage taken by that target is also taken by the pet. Lasts 12 sec.",
            @"Protects a friendly target from critical strikes, making attacks against that target unable to be critical strikes, but 20% of all damage taken by that target is also taken by the pet. Lasts 12 sec." });
        /// <summary>Your pet does an additional [3*Pts]% damage with all attacks.</summary>
        public readonly static PetTalentData SharkAttack = new PetTalentData(29, "Shark Attack", 2, new string[] {
            @"Your pet does an additional [3*Pts]% damage with all attacks.",
            @"Your pet does an additional 3% damage with all attacks.",
            @"Your pet does an additional 6% damage with all attacks." });
        /// <summary>Your pet's Growl also heals it for [1*Pts]% of its total health.</summary>
        public readonly static PetTalentData Silverback = new PetTalentData(30, "Silverback", 2, new string[] {
            @"Your pet's Growl also heals it for [1*Pts]% of its total health.",
            @"Your pet's Growl also heals it for 1% of its total health.",
            @"Your pet's Growl also heals it for 2% of its total health." });
        /// <summary>Increases the critical strike chance of your pet by [3*Pts]%.</summary>
        public readonly static PetTalentData SpidersBite = new PetTalentData(31, "Spider's Bite", 3, new string[] {
            @"Increases the critical strike chance of your pet by [3*Pts]%.",
            @"Increases the critical strike chance of your pet by 3%.",
            @"Increases the critical strike chance of your pet by 6%.",
            @"Increases the critical strike chance of your pet by 9%." });
        /// <summary>Your pet does an additional [3*Pts]% damage with all attacks.</summary>
        public readonly static PetTalentData SpikedCollar = new PetTalentData(32, "Spiked Collar", 3, new string[] {
            @"Your pet does an additional [3*Pts]% damage with all attacks.",
            @"Your pet does an additional 3% damage with all attacks.",
            @"Your pet does an additional 6% damage with all attacks.",
            @"Your pet does an additional 9% damage with all attacks." });
        /// <summary>Your pet taunts the target to attack it for 3 sec.</summary>
        public readonly static PetTalentData Taunt = new PetTalentData(33, "Taunt", 1, new string[] {
            @"Your pet taunts the target to attack it for 3 sec.",
            @"Your pet taunts the target to attack it for 3 sec." });
        /// <summary>Shakes the ground with thundering force, doing 3 to 5 Nature damage to all enemeies within 8 yards. This ability causes a moderate amount of additional threat.</summary>
        public readonly static PetTalentData Thunderstomp = new PetTalentData(34, "Thunderstomp", 1, new string[] {
            @"Shakes the ground with thundering force, doing 3 to 5 Nature damage to all enemeies within 8 yards. This ability causes a moderate amount of additional threat.",
            @"Shakes the ground with thundering force, doing 3 to 5 Nature damage to all enemeies within 8 yards. This ability causes a moderate amount of additional threat." });
        /// <summary>Increases the contribution your pet gets from your Stamina by [20*Pts]% and attack power by [15*Pts]%.</summary>
        public readonly static PetTalentData WildHunt = new PetTalentData(35, "Wild Hunt", 2, new string[] {
            @"Increases the contribution your pet gets from your Stamina by [20*Pts]% and attack power by [15*Pts]%.",
            @"Increases the contribution your pet gets from your Stamina by 20% and attack power by 15%.",
            @"Increases the contribution your pet gets from your Stamina by 40% and attack power by 30%." });
        /// <summary>A fierce attack causing 5 damage, modified by pet level, that your pet can use after its target dodges. Cannot be dodged, blocked or parried.</summary>
        public readonly static PetTalentData WolverineBite = new PetTalentData(36, "Wolverine Bite", 2, new string[] {
            @"A fierce attack causing 5 damage, modified by pet level, that your pet can use after its target dodges. Cannot be dodged, blocked or parried.",
            @"A fierce attack causing 5 damage, modified by pet level, that your pet can use after its target dodges. Cannot be dodged, blocked or parried." });
        /// <summary>Increases pet and hunter damage by [1*Pts]% for 10 seconds each time the pet deals a critical strike with Claw, Bite, or Smack.</summary>
        public readonly static PetTalentData CullingTheHerd = new PetTalentData(37, "Culling The Herd", 3, new string[] {
            @"Increases pet and hunter damage by [1*Pts]% for 10 seconds each time the pet deals a critical strike with Claw, Bite, or Smack.",
            @"Increases pet and hunter damage by 1% for 10 seconds each time the pet deals a critical strike with Claw, Bite, or Smack.",
            @"Increases pet and hunter damage by 2% for 10 seconds each time the pet deals a critical strike with Claw, Bite, or Smack.",
            @"Increases pet and hunter damage by 3% for 10 seconds each time the pet deals a critical strike with Claw, Bite, or Smack." });
    }

    // Pet Talents
    public class PetTalentTree
    {
        public PetTalentTree() {
            Initialize();
            MakeTree();
        }
        public PetTalentTree(string source) {
            if (TalentTree != null) { TalentTree.Clear(); }
            Initialize();
            MakeTree();
            if (String.IsNullOrEmpty(source)) { return; }
            int index = 0;
            try {
                foreach (char pt in source.ToCharArray())
                {
                    TalentTree[index].Value = int.Parse(pt.ToString());
                    index++;
                }
            }
            catch (Exception ex) {
                Rawr.Base.ErrorBox eb = new Rawr.Base.ErrorBox("Error rebuilding the pet talents from file",
                    ex.Message, "PetTalentTree(source)", "Source: " + source, ex.StackTrace);
            }
        }
        [XmlIgnore]
        private List<PetTalent> _TalentTree;
        [XmlIgnore]
        public List<PetTalent> TalentTree {
            get { return _TalentTree; }
            set { _TalentTree = value; }
        }
        public void MakeTree() {
            if (TalentTree == null || TalentTree.Count == 0)
            {
                TalentTree = new List<PetTalent>() {
                    Avoidance,
                    BloodOfTheRhino,
                    Bloodthirsty,
                    BoarsSpeed,
                    Bullheaded,
                    CallOfTheWild,
                    CarrionFeeder,
                    ChargeSwoop,
                    CobraReflexes,
                    Cornered,
                    DiveDash,
                    FeedingFrenzy,
                    GraceOfTheMantis,
                    GreatResistance,
                    GreatStamina,
                    GuardDog,
                    HeartOfThePhoenix,
                    ImprovedCower,
                    Intervene,
                    LastStand,
                    LickYourWounds,
                    Lionhearted,
                    Mobility,
                    NaturalArmor,
                    OwlsFocus,
                    PetBarding,
                    Rabid,
                    RoarOfRecovery,
                    RoarOfSacrifice,
                    SharkAttack,
                    Silverback,
                    SpidersBite,
                    SpikedCollar,
                    Taunt,
                    Thunderstomp,
                    WildHunt,
                    WolverineBite,
                    CullingTheHerd,
                };
            }
        }
        #region Talents
        [XmlIgnore]
        /// <summary>Reduces the damage your pet takes from area of effect attacks by [25*Pts]%.</summary>
        public PetTalent Avoidance;
        [XmlIgnore]
        /// <summary>Increases your pet's total Stamina by [2*Pts]% and increases all healing effects on your pet by [20*Pts]%.</summary>
        public PetTalent BloodOfTheRhino;
        [XmlIgnore]
        /// <summary>Your pet's attacks have a [10*Pts]% chance to increase its happiness by 5% and heal 5% of its total health.</summary>
        public PetTalent Bloodthirsty;
        [XmlIgnore]
        /// <summary>Increases your pet's movement speed by 30%.</summary>
        public PetTalent BoarsSpeed;
        [XmlIgnore]
        /// <summary>Removes all movement impairing effects and all effects which cause loss of control of your pet, and reduces damage done to your pet by 20% for 12 sec.</summary>
        public PetTalent Bullheaded;
        [XmlIgnore]
        /// <summary>Your pet roars, increasing your pet's and your melee and ranged attack power by 10%. Lasts 20 sec. 5 min cooldown.</summary>
        public PetTalent CallOfTheWild;
        [XmlIgnore]
        /// <summary>Your pet can generate health and happiness by eating a corpse. Will not work on the remains of elemental or mechanical creatures.</summary>
        public PetTalent CarrionFeeder;
        [XmlIgnore]
        /// <summary>Your pet charges an enemy, immobilizing the target for 1 sec, and increasing the pet's melee attack power by 25% for its next attack.</summary>
        public PetTalent ChargeSwoop;
        [XmlIgnore]
        /// <summary>Increases your pet's attack speed by [15*Pts]%. Your pet will hit faster but each hit will do less damage.</summary>
        public PetTalent CobraReflexes;
        [XmlIgnore]
        /// <summary>When at less than 35% health, your pet does [25*Pts]% more damage and has a [30*Pts]% reduced chance to eb critically hit.</summary>
        public PetTalent Cornered;
        [XmlIgnore]
        /// <summary>Increases your pet's movement speed by 80% for 16 sec.</summary>
        public PetTalent DiveDash;
        [XmlIgnore]
        /// <summary>Your pet does [8*Pts]% additional damage to targets with les than 35% health.</summary>
        public PetTalent FeedingFrenzy;
        [XmlIgnore]
        /// <summary>Reduces the chance your pet will be critically hit by melee attacks by [2*Pts]%.</summary>
        public PetTalent GraceOfTheMantis;
        [XmlIgnore]
        /// <summary>Your pet takes [5*Pts]% less damage from Arcane, Fire, Frost, Nature and Shadow magic.</summary>
        public PetTalent GreatResistance;
        [XmlIgnore]
        /// <summary>Increases your pet's total Stamina by [4*Pts]%.</summary>
        public PetTalent GreatStamina;
        [XmlIgnore]
        /// <summary>Your pet's Growl generates [10*Pts]% additional threat and 10% of it's total happiness.</summary>
        public PetTalent GuardDog;
        [XmlIgnore]
        /// <summary>When your pet dies, it will miraculously return to life with full health.</summary>
        public PetTalent HeartOfThePhoenix;
        [XmlIgnore]
        /// <summary>Your pet's Cower also decreases damage taken by [10*Pts]% for the next 10 sec.</summary>
        public PetTalent ImprovedCower;
        [XmlIgnore]
        /// <summary>Your pet runs at high speed towards a group member, intercepting the next melee or ranged attack made against them.</summary>
        public PetTalent Intervene;
        [XmlIgnore]
        /// <summary>Your pet temporarily gains 30% of it's maximum health for 20 sec. After the effect expires, the health is lost.</summary>
        public PetTalent LastStand;
        [XmlIgnore]
        /// <summary>Your pet heals itself for 100% of its total health over 5 sec while channeling.</summary>
        public PetTalent LickYourWounds;
        [XmlIgnore]
        /// <summary>Reduces the duration of all Stun and Fear effects used against your pet by [15*Pts]%.</summary>
        public PetTalent Lionhearted;
        [XmlIgnore]
        /// <summary>Reduces the cooldown on your pet's Dive/Dash ability by [8*Pts] sec.</summary>
        public PetTalent Mobility;
        [XmlIgnore]
        /// <summary>Increases your pet's armor by [5*Pts]%.</summary>
        public PetTalent NaturalArmor;
        [XmlIgnore]
        /// <summary>Your pet has a [15*Pts]% chance after using an ability that the next ability will cost no Focus if used within 8 sec.</summary>
        public PetTalent OwlsFocus;
        [XmlIgnore]
        /// <summary>Increases your pet's armor by [5*Pts]% and chance to Dodge by [1*Pts]%.</summary>
        public PetTalent PetBarding;
        [XmlIgnore]
        /// <summary>Your pet goes into a killing frenzy. Successful attacks have a chance to increase attack power by 5%. This effect will stack up to 5 times. Lasts 20 sc.</summary>
        public PetTalent Rabid;
        [XmlIgnore]
        /// <summary>Your pet's inspiring roar restores 30% of your total mana over 9 sec.</summary>
        public PetTalent RoarOfRecovery;
        [XmlIgnore]
        /// <summary>Protects a friendly target from critical strikes, making attacks against that target unable to be critical strikes, but 20% of all damage taken by that target is also taken by the pet. Lasts 12 sec.</summary>
        public PetTalent RoarOfSacrifice;
        [XmlIgnore]
        /// <summary>Your pet does an additional [3*Pts]% damage with all attacks.</summary>
        public PetTalent SharkAttack;
        [XmlIgnore]
        /// <summary>Your pet's Growl also heals it for [1*Pts]% of its total health.</summary>
        public PetTalent Silverback;
        [XmlIgnore]
        /// <summary>Increases the critical strike chance of your pet by [3*Pts]%.</summary>
        public PetTalent SpidersBite;
        [XmlIgnore]
        /// <summary>Your pet does an additional [3*Pts]% damage with all attacks.</summary>
        public PetTalent SpikedCollar;
        [XmlIgnore]
        /// <summary>Your pet taunts the target to attack it for 3 sec.</summary>
        public PetTalent Taunt;
        [XmlIgnore]
        /// <summary>Shakes the ground with thundering force, doing 3 to 5 Nature damage to all enemeies within 8 yards. This ability causes a moderate amount of additional threat.</summary>
        public PetTalent Thunderstomp;
        [XmlIgnore]
        /// <summary>Increases the contribution your pet gets from your Stamina by [20*Pts]% and attack power by [15*Pts]%.</summary>
        public PetTalent WildHunt;
        [XmlIgnore]
        /// <summary>A fierce attack causing 5 damage, modified by pet level, that your pet can use after its target dodges. Cannot be dodged, blocked or parried.</summary>
        public PetTalent WolverineBite;
        [XmlIgnore]
        /// <summary>Increases pet and hunter damage by [1*Pts]% for 10 seconds each time the pet deals a critical strike with Claw, Bite, or Smack.</summary>
        public PetTalent CullingTheHerd;
        #endregion
        /// <summary>Initializes the Abilities</summary>
        public void Initialize() {
            Avoidance = new PetTalent(PetTalentsBase.Avoidance, 0);
            BloodOfTheRhino = new PetTalent(PetTalentsBase.BloodOfTheRhino, 0);
            Bloodthirsty = new PetTalent(PetTalentsBase.Bloodthirsty, 0);
            BoarsSpeed = new PetTalent(PetTalentsBase.BoarsSpeed, 0);
            Bullheaded = new PetTalent(PetTalentsBase.Bullheaded, 0);
            CallOfTheWild = new PetTalent(PetTalentsBase.CallOfTheWild, 0);
            CarrionFeeder = new PetTalent(PetTalentsBase.CarrionFeeder, 0);
            ChargeSwoop = new PetTalent(PetTalentsBase.ChargeSwoop, 0);
            CobraReflexes = new PetTalent(PetTalentsBase.CobraReflexes, 0);
            Cornered = new PetTalent(PetTalentsBase.Cornered, 0);
            DiveDash = new PetTalent(PetTalentsBase.DiveDash, 0);
            FeedingFrenzy = new PetTalent(PetTalentsBase.FeedingFrenzy, 0);
            GraceOfTheMantis = new PetTalent(PetTalentsBase.GraceOfTheMantis, 0);
            GreatResistance = new PetTalent(PetTalentsBase.GreatResistance, 0);
            GreatStamina = new PetTalent(PetTalentsBase.GreatStamina, 0);
            GuardDog = new PetTalent(PetTalentsBase.GuardDog, 0);
            HeartOfThePhoenix = new PetTalent(PetTalentsBase.HeartOfThePhoenix, 0);
            ImprovedCower = new PetTalent(PetTalentsBase.ImprovedCower, 0);
            Intervene = new PetTalent(PetTalentsBase.Intervene, 0);
            LastStand = new PetTalent(PetTalentsBase.LastStand, 0);
            LickYourWounds = new PetTalent(PetTalentsBase.LickYourWounds, 0);
            Lionhearted = new PetTalent(PetTalentsBase.Lionhearted, 0);
            Mobility = new PetTalent(PetTalentsBase.Mobility, 0);
            NaturalArmor = new PetTalent(PetTalentsBase.NaturalArmor, 0);
            OwlsFocus = new PetTalent(PetTalentsBase.OwlsFocus, 0);
            PetBarding = new PetTalent(PetTalentsBase.PetBarding, 0);
            Rabid = new PetTalent(PetTalentsBase.Rabid, 0);
            RoarOfRecovery = new PetTalent(PetTalentsBase.RoarOfRecovery, 0);
            RoarOfSacrifice = new PetTalent(PetTalentsBase.RoarOfSacrifice, 0);
            SharkAttack = new PetTalent(PetTalentsBase.SharkAttack, 0);
            Silverback = new PetTalent(PetTalentsBase.Silverback, 0);
            SpidersBite = new PetTalent(PetTalentsBase.SpidersBite, 0);
            SpikedCollar = new PetTalent(PetTalentsBase.SpikedCollar, 0);
            Taunt = new PetTalent(PetTalentsBase.Taunt, 0);
            Thunderstomp = new PetTalent(PetTalentsBase.Thunderstomp, 0);
            WildHunt = new PetTalent(PetTalentsBase.WildHunt, 0);
            WolverineBite = new PetTalent(PetTalentsBase.WolverineBite, 0);
            CullingTheHerd = new PetTalent(PetTalentsBase.CullingTheHerd, 0);
        }
        /// <summary>Resets all Talents to 0</summary>
        public void Reset()
        {
            if (CobraReflexes.ID == -1) { Initialize(); return; }
            CobraReflexes.Value = 0;
            DiveDash.Value = 0;
            ChargeSwoop.Value = 0;
            GreatStamina.Value = 0;
            NaturalArmor.Value = 0;
            BoarsSpeed.Value = 0;
            Mobility.Value = 0;
            SpikedCollar.Value = 0;
            ImprovedCower.Value = 0;
            Bloodthirsty.Value = 0;
            BloodOfTheRhino.Value = 0;
            PetBarding.Value = 0;
            Avoidance.Value = 0; CullingTheHerd.Value = 0;
            Lionhearted.Value = 0;
            CarrionFeeder.Value = 0;
            GuardDog.Value = 0;
            Thunderstomp.Value = 0;
            GreatResistance.Value = 0;
            OwlsFocus.Value = 0;
            Cornered.Value = 0;
            FeedingFrenzy.Value = 0;
            HeartOfThePhoenix.Value = 0;
            SpidersBite.Value = 0;
            WolverineBite.Value = 0;
            RoarOfRecovery.Value = 0;
            Bullheaded.Value = 0;
            GraceOfTheMantis.Value = 0;
            Rabid.Value = 0;
            LickYourWounds.Value = 0;
            CallOfTheWild.Value = 0;
            LastStand.Value = 0;
            Taunt.Value = 0;
            Intervene.Value = 0;
            WildHunt.Value = 0;
            RoarOfSacrifice.Value = 0;
            SharkAttack.Value = 0;
            Silverback.Value = 0;
        }

        public override string ToString()
        {
            string ret = "";
            if (TalentTree == null
                || TalentTree.Count <= 0
                || TalentTree.Count > 38)
            {
                TalentTree.Clear();
                Initialize();
            }
            foreach (PetTalent pt in TalentTree) {
                ret += pt.Value.ToString();
            }
            return ret;
        }
    }
}
