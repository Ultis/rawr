using System;
using System.Collections.Generic;
using System.ComponentModel;
#if !RAWR3 && !SILVERLIGHT
using System.Data;
using System.Drawing;
#endif
using System.Text;
//using System.Windows.Forms;
using System.Xml.Serialization;

namespace Rawr.Hunter
{
    /*[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class PetTalentDataAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">Unique identifier</param>
        /// <param name="name">Talent Name</param>
        /// <param name="maxPoints">Maximum number of Ranks player can assign</param>
        /// <param name="tree">The id of the Tree (0,1,2), should aways be 0 on Pet</param>
        /// <param name="column">1 based array, 4 columns (1,2,3,4)</param>
        /// <param name="row">1 based array, 6 Rows (1,2,3,4,5,6)</param>
        /// <param name="prerequisite">The ID of another Pet Talent that must be maxed before this one can be selected</param>
        /// <param name="description">String array of descriptive stuff</param>
        /// <param name="icon">The icon to use for this talent, must specify the back end name for it</param>
        public PetTalentDataAttribute(int index, string name, int maxPoints, int tree, int column, int row, int prerequisite, string[] description, string icon)
        {
            _index = index;
            _name = name;
            _maxPoints = maxPoints;
            _tree = tree;
            _column = column;
            _row = row;
            _prerequisite = prerequisite;
            _description = description;
            _icon = icon;
        }

        private readonly int _index;
        private readonly string _name;
        private readonly int _maxPoints;
        private readonly int _tree;
        private readonly int _column;
        private readonly int _row;
        private readonly int _prerequisite;
        private readonly string _icon;
        private readonly string[] _description;

        public int Index { get { return _index; } }
        public string Name { get { return _name; } }
        public int MaxPoints { get { return _maxPoints; } }
        public int Tree { get { return _tree; } }
        public int Column { get { return _column; } }
        public int Row { get { return _row; } }
        public int Prerequisite { get { return _prerequisite; } }
        public string[] Description { get { return _description; } }
        public string Icon { get { return _icon; } }

        public override string ToString() {
            try {
                return Name + " [" + MaxPoints.ToString() + "]\r\n" + Description[0];
            } catch (Exception) { return "Failed to convert PetTalentDataAttribute to string."; }
        }
    }

    public abstract class PetTalentsBase
    {
        public abstract int[] Data { get; }

        protected void LoadString(string code)
        {
            if (string.IsNullOrEmpty(code)) return;
            int[] _data = Data;
            string[] tmp = code.Split('.');
            string talents = tmp[0];
            if (talents.Length >= _data.Length)
            {
                List<int> data = new List<int>();
                foreach (Char digit in talents)
                    data.Add(int.Parse(digit.ToString()));
                data.CopyTo(0, _data, 0, _data.Length);
            }
        }

        public override string ToString()
        {
            StringBuilder ret = new StringBuilder();
            foreach (int digit in Data)
            {
                ret.Append(digit.ToString());
            }
            return ret.ToString();
        }

        public CharacterClass GetClass() { return CharacterClass.Hunter; }

#if RAWR3
		public abstract PetTalentsBase Clone();
#endif
    }

    public partial class PetTalents : PetTalentsBase
#if RAWR3
	{
		public override PetTalentsBase Clone()
#else
, ICloneable
    {
        public PetTalents Clone() { return (PetTalents)((ICloneable)this).Clone(); }
        object ICloneable.Clone()
#endif
        {
            PetTalents clone = (PetTalents)MemberwiseClone();
            clone._data = (int[])_data.Clone();
            return clone;
        }

        private int[] _data = new int[81];
        public override int[] Data { get { return _data; } }
        public PetTalents() { }
        public PetTalents(string talents) { LoadString(talents); }
        public static string[] TreeNames = new string[] { @"Tenacity", @"Cunning", @"Ferocity", };

/// <summary>Increases your pet's attack speed by [15*Pts]%. Your pet will hit faster but each hit will do less damage.</summary>
[PetTalentData(0, "Cobra Reflexes", 2, 0, 1, 1, -1, new string[] {
@"Increases your pet's attack speed by [15*Pts]%. Your pet will hit faster but each hit will do less damage.",
@"Increases your pet's attack speed by 15%. Your pet will hit faster but each hit will do less damage.",
@"Increases your pet's attack speed by 30%. Your pet will hit faster but each hit will do less damage." }, "ability_rogue_ambush")]
public int CobraReflexes_Ten { get { return _data[0]; } set { _data[0] = value; } }
/// <summary>Increases your pet's attack speed by [15*Pts]%. Your pet will hit faster but each hit will do less damage.</summary>
public int CobraReflexes_Cun { get { return _data[0]; } set { _data[0] = value; } }
/// <summary>Increases your pet's attack speed by [15*Pts]%. Your pet will hit faster but each hit will do less damage.</summary>
public int CobraReflexes_Fer { get { return _data[0]; } set { _data[0] = value; } }

/// <summary>Your pet charges an enemy, immobilizing the target for 1 sec, and increasing the pet's melee attack power by 25% for its next attack.</summary>
[PetTalentData(1, "Charge/Swoop", 1, 0, 1, 1, -1, new string[] {
@"Your pet charges an enemy, immobilizing the target for 1 sec, and increasing the pet's melee attack power by 25% for its next attack.",
@"Your pet charges an enemy, immobilizing the target for 1 sec, and increasing the pet's melee attack power by 25% for its next attack." }, "ability_rogue_ambush")]
public int ChargeSwoop { get { return _data[1]; } set { _data[1] = value; } }

        
/// <summary>Reduces the damage your pet takes from area of effect attacks by [25*Pts]%.</summary>
[PetTalentData(0, "Avoidance", 3, 0, 1, 1, -1, new string[] {
@"Reduces the damage your pet takes from area of effect attacks by [25*Pts]%.",
@"Reduces the damage your pet takes from area of effect attacks by 25%.",
@"Reduces the damage your pet takes from area of effect attacks by 50%.",
@"Reduces the damage your pet takes from area of effect attacks by 75%." }, "ability_rogue_ambush")]
public int Avoidance { get { return _data[0]; } set { _data[0] = value; } }
/// <summary>Increases your pet's total Stamina by [2*Pts]% and increases all healing effects on your pet by [20*Pts]%.</summary>
[PetTalentData(1, "Blood Of The Rhino", 2, 0, 1, 1, -1, new string[] {
@"Increases your pet's total Stamina by [2*Pts]% and increases all healing effects on your pet by [20*Pts]%.",
@"Increases your pet's total Stamina by 2% and increases all healing effects on your pet by 20%.",
@"Increases your pet's total Stamina by 4% and increases all healing effects on your pet by 40%." }, "ability_rogue_ambush")]
public int BloodOfTheRhino { get { return _data[1]; } set { _data[1] = value; } }
/// <summary>Your pet's attacks have a [10*Pts]% chance to increase its happiness by 5% and heal 5% of its total health.</summary>
[PetTalentData(2, "Bloodthirsty", 2, 0, 1, 1, -1, new string[] {
@"Your pet's attacks have a [10*Pts]% chance to increase its happiness by 5% and heal 5% of its total health.",
@"Your pet's attacks have a 10% chance to increase its happiness by 5% and heal 5% of its total health.",
@"Your pet's attacks have a 20% chance to increase its happiness by 5% and heal 5% of its total health." }, "ability_rogue_ambush")]
public int Bloodthirsty { get { return _data[2]; } set { _data[2] = value; } }
/// <summary>Increases your pet's movement speed by 30%.</summary>
[PetTalentData(3, "Boar's Speed", 1, 0, 1, 1, -1, new string[] {
@"Increases your pet's movement speed by 30%.",
@"Increases your pet's movement speed by 30%." }, "ability_rogue_ambush")]
public int BoarsSpeed { get { return _data[3]; } set { _data[3] = value; } }
/// <summary>Removes all movement impairing effects and all effects which cause loss of control of your pet, and reduces damage done to your pet by 20% for 12 sec.</summary>
[PetTalentData(4, "Bullheaded", 1, 0, 1, 1, -1, new string[] {
@"Removes all movement impairing effects and all effects which cause loss of control of your pet, and reduces damage done to your pet by 20% for 12 sec.",
@"Removes all movement impairing effects and all effects which cause loss of control of your pet, and reduces damage done to your pet by 20% for 12 sec." }, "ability_rogue_ambush")]
public int Bullheaded { get { return _data[4]; } set { _data[4] = value; } }
/// <summary>Your pet roars, increasing your pet's and your melee and ranged attack power by 10%. Lasts 20 sec. 5 min cooldown.</summary>
[PetTalentData(5, "Call Of The Wild", 1, 0, 1, 1, -1, new string[] {
@"Your pet roars, increasing your pet's and your melee and ranged attack power by 10%. Lasts 20 sec. 5 min cooldown.",
@"Your pet roars, increasing your pet's and your melee and ranged attack power by 10%. Lasts 20 sec. 5 min cooldown." }, "ability_rogue_ambush")]
public int CallOfTheWild { get { return _data[5]; } set { _data[5] = value; } }
/// <summary>Your pet can generate health and happiness by eating a corpse. Will not work on the remains of elemental or mechanical creatures.</summary>
[PetTalentData(6, "Carrion Feeder", 1, 0, 1, 1, -1, new string[] {
@"Your pet can generate health and happiness by eating a corpse. Will not work on the remains of elemental or mechanical creatures.",
@"Your pet can generate health and happiness by eating a corpse. Will not work on the remains of elemental or mechanical creatures." }, "ability_rogue_ambush")]
public int CarrionFeeder { get { return _data[6]; } set { _data[6] = value; } }
/// <summary>When at less than 35% health, your pet does [25*Pts]% more damage and has a [30*Pts]% reduced chance to eb critically hit.</summary>
[PetTalentData(9, "Cornered", 2, 0, 1, 1, -1, new string[] {
@"When at less than 35% health, your pet does [25*Pts]% more damage and has a [30*Pts]% reduced chance to be critically hit.",
@"When at less than 35% health, your pet does 25% more damage and has a 30% reduced chance to be critically hit.",
@"When at less than 35% health, your pet does 50% more damage and has a 60% reduced chance to be critically hit." }, "ability_rogue_ambush")]
public int Cornered { get { return _data[9]; } set { _data[9] = value; } }
/// <summary>Increases your pet's movement speed by 80% for 16 sec.</summary>
[PetTalentData(10, "Dive/Dash", 1, 0, 1, 1, -1, new string[] {
@"Increases your pet's movement speed by 80% for 16 sec.",
@"Increases your pet's movement speed by 80% for 16 sec." }, "ability_rogue_ambush")]
public int DiveDash { get { return _data[10]; } set { _data[10] = value; } }
/// <summary>Your pet does [8*Pts]% additional damage to targets with less than 35% health.</summary>
[PetTalentData(11, "Feeding Frenzy", 2, 0, 1, 1, -1, new string[] {
@"Your pet does [8*Pts]% additional damage to targets with less than 35% health.",
@"Your pet does 8% additional damage to targets with less than 35% health.",
@"Your pet does 16% additional damage to targets with less than 35% health." }, "ability_rogue_ambush")]
public int FeedingFrenzy { get { return _data[11]; } set { _data[11] = value; } }
/// <summary>Reduces the chance your pet will be critically hit by melee attacks by [2*Pts]%.</summary>
[PetTalentData(12, "Grace Of The Mantis", 2, 0, 1, 1, -1, new string[] {
@"Reduces the chance your pet will be critically hit by melee attacks by [2*Pts]%.",
@"Reduces the chance your pet will be critically hit by melee attacks by 2%.",
@"Reduces the chance your pet will be critically hit by melee attacks by 4%." }, "ability_rogue_ambush")]
public int GraceOfTheMantis { get { return _data[12]; } set { _data[12] = value; } }
/// <summary>Your pet takes [5*Pts]% less damage from Arcane, Fire, Frost, Nature and Shadow magic.</summary>
[PetTalentData(13, "Great Resistance", 3, 0, 1, 1, -1, new string[] {
@"Your pet takes [5*Pts]% less damage from Arcane, Fire, Frost, Nature and Shadow magic.",
@"Your pet takes 5% less damage from Arcane, Fire, Frost, Nature and Shadow magic.",
@"Your pet takes 10% less damage from Arcane, Fire, Frost, Nature and Shadow magic.",
@"Your pet takes 15% less damage from Arcane, Fire, Frost, Nature and Shadow magic." }, "ability_rogue_ambush")]
public int GreatResistance { get { return _data[0]; } set { _data[0] = value; } }
/// <summary>Increases your pet's total Stamina by [4*Pts]%.</summary>
[PetTalentData(14, "Great Stamina", 3, 0, 1, 1, -1, new string[] {
@"Increases your pet's total Stamina by [4*Pts]%.",
@"Increases your pet's total Stamina by 4%.",
@"Increases your pet's total Stamina by 8%.",
@"Increases your pet's total Stamina by 12%." }, "ability_rogue_ambush")]
public int GreatStamina { get { return _data[0]; } set { _data[0] = value; } }
/// <summary>Your pet's Growl generates [10*Pts]% additional threat and 10% of it's total happiness.</summary>
[PetTalentData(15, "Guard Dog", 2, 0, 1, 1, -1, new string[] {
@"Your pet's Growl generates [10*Pts]% additional threat and 10% of it's total happiness.",
@"Your pet's Growl generates 10% additional threat and 10% of it's total happiness.",
@"Your pet's Growl generates 20% additional threat and 10% of it's total happiness." }, "ability_rogue_ambush")]
public int GuardDog { get { return _data[0]; } set { _data[0] = value; } }
/// <summary>When your pet dies, it will miraculously return to life with full health.</summary>
[PetTalentData(16, "Heart Of The Phoenix", 1, 0, 1, 1, -1, new string[] {
@"When your pet dies, it will miraculously return to life with full health.",
@"When your pet dies, it will miraculously return to life with full health." }, "ability_rogue_ambush")]
public int HeartOfThePhoenix { get { return _data[0]; } set { _data[0] = value; } }
/// <summary>Your pet's Cower also decreases damage taken by [10*Pts]% for the next 10 sec.</summary>
[PetTalentData(17, "Improved Cower", 2, 0, 1, 1, -1, new string[] {
@"Your pet's Cower also decreases damage taken by [10*Pts]% for the next 10 sec.",
@"Your pet's Cower also decreases damage taken by 10% for the next 10 sec.",
@"Your pet's Cower also decreases damage taken by 20% for the next 10 sec." }, "ability_rogue_ambush")]
public int ImprovedCower { get { return _data[0]; } set { _data[0] = value; } }
/// <summary>Your pet runs at high speed towards a group member, intercepting the next melee or ranged attack made against them.</summary>
[PetTalentData(18, "Intervene", 1, 0, 1, 1, -1, new string[] {
@"Your pet runs at high speed towards a group member, intercepting the next melee or ranged attack made against them.",
@"Your pet runs at high speed towards a group member, intercepting the next melee or ranged attack made against them." }, "ability_rogue_ambush")]
public int Intervene { get { return _data[0]; } set { _data[0] = value; } }
/// <summary>Your pet temporarily gains 30% of it's maximum health for 20 sec. After the effect expires, the health is lost.</summary>
[PetTalentData(19, "Last Stand", 1, 0, 1, 1, -1, new string[] {
@"Your pet temporarily gains 30% of it's maximum health for 20 sec. After the effect expires, the health is lost.",
@"Your pet temporarily gains 30% of it's maximum health for 20 sec. After the effect expires, the health is lost." }, "ability_rogue_ambush")]
public int LastStand { get { return _data[0]; } set { _data[0] = value; } }
/// <summary>Your pet heals itself for 100% of its total health over 5 sec while channeling.</summary>
[PetTalentData(20, "Lick Your Wounds", 1, 0, 1, 1, -1, new string[] {
@"Your pet heals itself for 100% of its total health over 5 sec while channeling.",
@"Your pet heals itself for 100% of its total health over 5 sec while channeling." }, "ability_rogue_ambush")]
public int LickYourWounds { get { return _data[0]; } set { _data[0] = value; } }
/// <summary>Reduces the duration of all Stun and Fear effects used against your pet by [15*Pts]%.</summary>
[PetTalentData(21, "Lionhearted", 2, 0, 1, 1, -1, new string[] {
@"Reduces the duration of all Stun and Fear effects used against your pet by [15*Pts]%.",
@"Reduces the duration of all Stun and Fear effects used against your pet by 15%.",
@"Reduces the duration of all Stun and Fear effects used against your pet by 30%." }, "ability_rogue_ambush")]
public int Lionhearted { get { return _data[0]; } set { _data[0] = value; } }
/// <summary>Reduces the cooldown on your pet's Dive/Dash ability by [8*Pts] sec.</summary>
[PetTalentData(22, "Mobility", 2, 0, 1, 1, -1, new string[] {
@"Reduces the cooldown on your pet's Dive/Dash ability by [8*Pts] sec.",
@"Reduces the cooldown on your pet's Dive/Dash ability by 8 sec.",
@"Reduces the cooldown on your pet's Dive/Dash ability by 16 sec." }, "ability_rogue_ambush")]
public int Mobility { get { return _data[0]; } set { _data[0] = value; } }
/// <summary>Increases your pet's armor by [5*Pts]%.</summary>
[PetTalentData(23, "Natural Armor", 2, 0, 1, 1, -1, new string[] {
@"Increases your pet's armor by [5*Pts]%.",
@"Increases your pet's armor by 5%.",
@"Increases your pet's armor by 10%." }, "ability_rogue_ambush")]
public int NaturalArmor { get { return _data[0]; } set { _data[0] = value; } }
/// <summary>Your pet has a [15*Pts]% chance after using an ability that the next ability will cost no Focus if used within 8 sec.</summary>
[PetTalentData(24, "Owl's Focus", 2, 0, 1, 1, -1, new string[] {
@"Your pet has a [15*Pts]% chance after using an ability that the next ability will cost no Focus if used within 8 sec.",
@"Your pet has a 15% chance after using an ability that the next ability will cost no Focus if used within 8 sec.",
@"Your pet has a 30% chance after using an ability that the next ability will cost no Focus if used within 8 sec." }, "ability_rogue_ambush")]
public int OwlsFocus { get { return _data[0]; } set { _data[0] = value; } }
/// <summary>Increases your pet's armor by [5*Pts]% and chance to Dodge by [1*Pts]%.</summary>
[PetTalentData(25, "Pet Barding", 2, 0, 1, 1, -1, new string[] {
@"Increases your pet's armor by [5*Pts]% and chance to Dodge by [1*Pts]%.",
@"Increases your pet's armor by 5% and chance to Dodge by 1%.",
@"Increases your pet's armor by 10% and chance to Dodge by 2%." }, "ability_rogue_ambush")]
public int PetBarding { get { return _data[0]; } set { _data[0] = value; } }
/// <summary>Your pet goes into a killing frenzy. Successful attacks have a chance to increase attack power by 5%. This effect will stack up to 5 times. Lasts 20 sc.</summary>
[PetTalentData(26, "Rabid", 1, 0, 1, 1, -1, new string[] {
@"Your pet goes into a killing frenzy. Successful attacks have a chance to increase attack power by 5%. This effect will stack up to 5 times. Lasts 20 sc.",
@"Your pet goes into a killing frenzy. Successful attacks have a chance to increase attack power by 5%. This effect will stack up to 5 times. Lasts 20 sc." }, "ability_rogue_ambush")]
public int Rabid { get { return _data[0]; } set { _data[0] = value; } }
/// <summary>Your pet's inspiring roar restores 30% of your total mana over 9 sec.</summary>
[PetTalentData(27, "Roar Of Recovery", 2, 0, 1, 1, -1, new string[] {
@"Your pet's inspiring roar restores 30% of your total mana over 9 sec.",
@"Your pet's inspiring roar restores 30% of your total mana over 9 sec." }, "ability_rogue_ambush")]
public int RoarOfRecovery { get { return _data[0]; } set { _data[0] = value; } }
/// <summary>Protects a friendly target from critical strikes, making attacks against that target unable to be critical strikes, but 20% of all damage taken by that target is also taken by the pet. Lasts 12 sec.</summary>
[PetTalentData(28, "Roar Of Sacrifice", 1, 0, 1, 1, -1, new string[] {
@"Protects a friendly target from critical strikes, making attacks against that target unable to be critical strikes, but 20% of all damage taken by that target is also taken by the pet. Lasts 12 sec.",
@"Protects a friendly target from critical strikes, making attacks against that target unable to be critical strikes, but 20% of all damage taken by that target is also taken by the pet. Lasts 12 sec." }, "ability_rogue_ambush")]
public int RoarOfSacrifice { get { return _data[0]; } set { _data[0] = value; } }
/// <summary>Your pet does an additional [3*Pts]% damage with all attacks.</summary>
[PetTalentData(29, "Shark Attack", 2, 0, 1, 1, -1, new string[] {
@"Your pet does an additional [3*Pts]% damage with all attacks.",
@"Your pet does an additional 3% damage with all attacks.",
@"Your pet does an additional 6% damage with all attacks." }, "ability_rogue_ambush")]
public int SharkAttack { get { return _data[0]; } set { _data[0] = value; } }
/// <summary>Your pet's Growl also heals it for [1*Pts]% of its total health.</summary>
[PetTalentData(30, "Silverback", 2, 0, 1, 1, -1, new string[] {
@"Your pet's Growl also heals it for [1*Pts]% of its total health.",
@"Your pet's Growl also heals it for 1% of its total health.",
@"Your pet's Growl also heals it for 2% of its total health." }, "ability_rogue_ambush")]
public int Silverback { get { return _data[0]; } set { _data[0] = value; } }
/// <summary>Increases the critical strike chance of your pet by [3*Pts]%.</summary>
[PetTalentData(31, "Spider's Bite", 3, 0, 1, 1, -1, new string[] {
@"Increases the critical strike chance of your pet by [3*Pts]%.",
@"Increases the critical strike chance of your pet by 3%.",
@"Increases the critical strike chance of your pet by 6%.",
@"Increases the critical strike chance of your pet by 9%." }, "ability_rogue_ambush")]
public int SpidersBite { get { return _data[0]; } set { _data[0] = value; } }
/// <summary>Your pet does an additional [3*Pts]% damage with all attacks.</summary>
[PetTalentData(32, "Spiked Collar", 3, 0, 1, 1, -1, new string[] {
@"Your pet does an additional [3*Pts]% damage with all attacks.",
@"Your pet does an additional 3% damage with all attacks.",
@"Your pet does an additional 6% damage with all attacks.",
@"Your pet does an additional 9% damage with all attacks." }, "ability_rogue_ambush")]
public int SpikedCollar { get { return _data[0]; } set { _data[0] = value; } }
/// <summary>Your pet taunts the target to attack it for 3 sec.</summary>
[PetTalentData(33, "Taunt", 1, 0, 1, 1, -1, new string[] {
@"Your pet taunts the target to attack it for 3 sec.",
@"Your pet taunts the target to attack it for 3 sec." }, "ability_rogue_ambush")]
public int Taunt { get { return _data[0]; } set { _data[0] = value; } }
/// <summary>Shakes the ground with thundering force, doing 3 to 5 Nature damage to all enemeies within 8 yards. This ability causes a moderate amount of additional threat.</summary>
[PetTalentData(34, "Thunderstomp", 1, 0, 1, 1, -1, new string[] {
@"Shakes the ground with thundering force, doing 3 to 5 Nature damage to all enemeies within 8 yards. This ability causes a moderate amount of additional threat.",
@"Shakes the ground with thundering force, doing 3 to 5 Nature damage to all enemeies within 8 yards. This ability causes a moderate amount of additional threat." }, "ability_rogue_ambush")]
public int Thunderstomp { get { return _data[0]; } set { _data[0] = value; } }
/// <summary>Increases the contribution your pet gets from your Stamina by [20*Pts]% and attack power by [15*Pts]%.</summary>
[PetTalentData(35, "Wild Hunt", 2, 0, 1, 1, -1, new string[] {
@"Increases the contribution your pet gets from your Stamina by [20*Pts]% and attack power by [15*Pts]%.",
@"Increases the contribution your pet gets from your Stamina by 20% and attack power by 15%.",
@"Increases the contribution your pet gets from your Stamina by 40% and attack power by 30%." }, "ability_rogue_ambush")]
public int WildHunt { get { return _data[0]; } set { _data[0] = value; } }
/// <summary>A fierce attack causing 5 damage, modified by pet level, that your pet can use after its target dodges. Cannot be dodged, blocked or parried.</summary>
[PetTalentData(36, "Wolverine Bite", 2, 0, 1, 1, -1, new string[] {
@"A fierce attack causing 5 damage, modified by pet level, that your pet can use after its target dodges. Cannot be dodged, blocked or parried.",
@"A fierce attack causing 5 damage, modified by pet level, that your pet can use after its target dodges. Cannot be dodged, blocked or parried." }, "ability_rogue_ambush")]
public int WolverineBite { get { return _data[0]; } set { _data[0] = value; } }
/// <summary>Increases pet and hunter damage by [1*Pts]% for 10 seconds each time the pet deals a critical strike with Claw, Bite, or Smack.</summary>
[PetTalentData(37, "Culling The Herd", 3, 0, 1, 1, -1, new string[] {
@"Increases pet and hunter damage by [1*Pts]% for 10 seconds each time the pet deals a critical strike with Claw, Bite, or Smack.",
@"Increases pet and hunter damage by 1% for 10 seconds each time the pet deals a critical strike with Claw, Bite, or Smack.",
@"Increases pet and hunter damage by 2% for 10 seconds each time the pet deals a critical strike with Claw, Bite, or Smack.",
@"Increases pet and hunter damage by 3% for 10 seconds each time the pet deals a critical strike with Claw, Bite, or Smack." }, "ability_rogue_ambush")]
public int CullingTheHerd { get { return _data[0]; } set { _data[0] = value; } }
    }
    */
    
    public class PetTalentData {
        /// <summary>A Pet Talent</summary>
        public PetTalentData() {
            ID = -1;
            Name = "Invalid";
            Max = 1;
            Desc = new string[] { "" };
            Icon = "";
        }
        /// <summary>A Pet Talent</summary>
        public PetTalentData(int id, string name, int max, string[] desc, string icon) {
            ID = id;
            Name = name;
            Max = max;
            Desc = desc;
            Icon = icon;
        }
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
        /// <summary>The Icon</summary>
        public string Icon;

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
        public PetTalent() { Value = 0; }
        /// <summary>A Pet Talent</summary>
        public PetTalent(PetTalentData Base, int value) {
            ID = Base.ID;
            Name = Base.Name;
            Max = Base.Max;
            Desc = Base.Desc;
            Icon = Base.Icon;

            Value = value;
        }
        /// <summary>A Pet Talent</summary>
        public PetTalent(int id, string name, int value, int max, string[] desc, string icon) {
            ID = id;
            Name = name;
            Max = max;
            Desc = desc;
            Icon = icon;
            
            Value = value;
        }
        /// <summary>The Current Rank of the Talent</summary>
        private int _value;
        public int Value {
            get { return _value; }
            set {
                _value = value;
                #if !RAWR3 && !SILVERLIGHT
                UpdateIcon();
                #endif
            }
        }

#if !RAWR3 && !SILVERLIGHT
        private Image _icon;
        /// <summary>The actual Icon Image</summary>
        public Image TheIcon {
            get {
                if(_icon == null && !String.IsNullOrEmpty(Icon)){ UpdateIcon(); }
                return _icon;
            }
        }
        private void UpdateIcon() {
            try{
                _icon = ItemIconsPet.GetTalentIcon(CharacterClass.Hunter, "Pet",
                    Name + (Value == 0 ? "-off" : ""),
                    (Value == 0 ? "grey/" : "") + Icon + ".jpg");
            }catch(Exception ex){
                Rawr.Base.ErrorBox eb = new Rawr.Base.ErrorBox("Error setting the Icon",
                    ex.Message, "UpdateIcon()", "No Additional Info", ex.StackTrace);
            }
        }
#endif

        public override string ToString() {
            try{
 	            return Name + " [" + Value.ToString() + "/" + Max.ToString() + "]\r\n" + Desc[0];
            }catch (Exception){
                return "Failed to convert PetTalent to string.";
            }
        }
    }

    public static class PetTalentsBase {
        /// <summary>Increases your pet's total Stamina by [2*Pts]% and increases all healing effects on your pet by [20*Pts]%.</summary>
        public readonly static PetTalentData BloodOfTheRhino = new PetTalentData(0, "Blood Of The Rhino", 2, new string[] {
            @"Increases your pet's total Stamina by [2*Pts]% and increases all healing effects on your pet by [20*Pts]%.",
            @"Increases your pet's total Stamina by 2% and increases all healing effects on your pet by 20%.",
            @"Increases your pet's total Stamina by 4% and increases all healing effects on your pet by 40%." }, "spell_shadow_lifedrain");
        /// <summary>Your pet's attacks have a [10*Pts]% chance to increase its happiness by 5% and heal 5% of its total health.</summary>
        public readonly static PetTalentData Bloodthirsty = new PetTalentData(1, "Bloodthirsty", 2, new string[] {
            @"Your pet's attacks have a [10*Pts]% chance to increase its happiness by 5% and heal 5% of its total health.",
            @"Your pet's attacks have a 10% chance to increase its happiness by 5% and heal 5% of its total health.",
            @"Your pet's attacks have a 20% chance to increase its happiness by 5% and heal 5% of its total health." }, "ability_druid_primaltenacity");
        /// <summary>Increases your pet's movement speed by 30%.</summary>
        public readonly static PetTalentData BoarsSpeed = new PetTalentData(2, "Boar's Speed", 1, new string[] {
            @"Increases your pet's movement speed by 30%.",
            @"Increases your pet's movement speed by 30%." }, "ability_hunter_pet_boar");
        /// <summary>Removes all movement impairing effects and all effects which cause loss of control of your pet, and reduces damage done to your pet by 20% for 12 sec.</summary>
        public readonly static PetTalentData Bullheaded = new PetTalentData(3, "Bullheaded", 1, new string[] {
            @"Removes all movement impairing effects and all effects which cause loss of control of your pet, and reduces damage done to your pet by 20% for 12 sec.",
            @"Removes all movement impairing effects and all effects which cause loss of control of your pet, and reduces damage done to your pet by 20% for 12 sec." }, "ability_warrior_bullrush");
        /// <summary>Your pet roars, increasing your pet's and your melee and ranged attack power by 10%. Lasts 20 sec. 5 min cooldown.</summary>
        public readonly static PetTalentData CallOfTheWild = new PetTalentData(4, "Call Of The Wild", 1, new string[] {
            @"Your pet roars, increasing your pet's and your melee and ranged attack power by 10%. Lasts 20 sec. 5 min cooldown.",
            @"Your pet roars, increasing your pet's and your melee and ranged attack power by 10%. Lasts 20 sec. 5 min cooldown." }, "ability_druid_kingofthejungle");
        /// <summary>Your pet can generate health and happiness by eating a corpse. Will not work on the remains of elemental or mechanical creatures.</summary>
        public readonly static PetTalentData CarrionFeeder = new PetTalentData(5, "Carrion Feeder", 1, new string[] {
            @"Your pet can generate health and happiness by eating a corpse. Will not work on the remains of elemental or mechanical creatures.",
            @"Your pet can generate health and happiness by eating a corpse. Will not work on the remains of elemental or mechanical creatures." }, "ability_racial_cannibalize");
        /// <summary>Your pet charges an enemy, immobilizing the target for 1 sec, and increasing the pet's melee attack power by 25% for its next attack.</summary>
        public readonly static PetTalentData ChargeSwoop = new PetTalentData(6, "Charge/Swoop", 1, new string[] {
            @"Your pet charges an enemy, immobilizing the target for 1 sec, and increasing the pet's melee attack power by 25% for its next attack.",
            @"Your pet charges an enemy, immobilizing the target for 1 sec, and increasing the pet's melee attack power by 25% for its next attack." }, "ability_hunter_pet_bear");
        /// <summary>Increases your pet's attack speed by [15*Pts]%. Your pet will hit faster but each hit will do less damage.</summary>
        public readonly static PetTalentData CobraReflexes = new PetTalentData(7, "Cobra Reflexes", 2, new string[] {
            @"Increases your pet's attack speed by [15*Pts]%. Your pet will hit faster but each hit will do less damage.",
            @"Increases your pet's attack speed by 15%. Your pet will hit faster but each hit will do less damage.",
            @"Increases your pet's attack speed by 30%. Your pet will hit faster but each hit will do less damage." }, "spell_nature_guardianward");
        /// <summary>When at less than 35% health, your pet does [25*Pts]% more damage and has a [30*Pts]% reduced chance to eb critically hit.</summary>
        public readonly static PetTalentData Cornered = new PetTalentData(8, "Cornered", 2, new string[] {
            @"When at less than 35% health, your pet does [25*Pts]% more damage and has a [30*Pts]% reduced chance to be critically hit.",
            @"When at less than 35% health, your pet does 25% more damage and has a 30% reduced chance to be critically hit.",
            @"When at less than 35% health, your pet does 50% more damage and has a 60% reduced chance to be critically hit." }, "ability_hunter_survivalinstincts");
        /// <summary>Increases pet and hunter damage by [1*Pts]% for 10 seconds each time the pet deals a critical strike with Claw, Bite, or Smack.</summary>
        public readonly static PetTalentData CullingTheHerd = new PetTalentData(9, "Culling The Herd", 3, new string[] {
            @"Increases pet and hunter damage by [1*Pts]% for 10 seconds each time the pet deals a critical strike with Claw, Bite, or Smack.",
            @"Increases pet and hunter damage by 1% for 10 seconds each time the pet deals a critical strike with Claw, Bite, or Smack.",
            @"Increases pet and hunter damage by 2% for 10 seconds each time the pet deals a critical strike with Claw, Bite, or Smack.",
            @"Increases pet and hunter damage by 3% for 10 seconds each time the pet deals a critical strike with Claw, Bite, or Smack." }, "inv_misc_monsterhorn_06");
        /// <summary>Increases your pet's movement speed by 80% for 16 sec.</summary>
        public readonly static PetTalentData DiveDash = new PetTalentData(10, "Dive/Dash", 1, new string[] {
            @"Increases your pet's movement speed by 80% for 16 sec.",
            @"Increases your pet's movement speed by 80% for 16 sec." }, "spell_shadow_burningspirit");
        /// <summary>Your pet does [8*Pts]% additional damage to targets with less than 35% health.</summary>
        public readonly static PetTalentData FeedingFrenzy = new PetTalentData(11, "Feeding Frenzy", 2, new string[] {
            @"Your pet does [8*Pts]% additional damage to targets with less than 35% health.",
            @"Your pet does 8% additional damage to targets with less than 35% health.",
            @"Your pet does 16% additional damage to targets with less than 35% health." }, "inv_misc_fish_48");
        /// <summary>Reduces the chance your pet will be critically hit by melee attacks by [2*Pts]%.</summary>
        public readonly static PetTalentData GraceOfTheMantis = new PetTalentData(12, "Grace Of The Mantis", 2, new string[] {
            @"Reduces the chance your pet will be critically hit by melee attacks by [2*Pts]%.",
            @"Reduces the chance your pet will be critically hit by melee attacks by 2%.",
            @"Reduces the chance your pet will be critically hit by melee attacks by 4%." }, "inv_misc_ahnqirajtrinket_02");
        /// <summary>Your pet takes [5*Pts]% less damage from Arcane, Fire, Frost, Nature and Shadow magic.</summary>
        public readonly static PetTalentData GreatResistance = new PetTalentData(13, "Great Resistance", 3, new string[] {
            @"Your pet takes [5*Pts]% less damage from Arcane, Fire, Frost, Nature and Shadow magic.",
            @"Your pet takes 5% less damage from Arcane, Fire, Frost, Nature and Shadow magic.",
            @"Your pet takes 10% less damage from Arcane, Fire, Frost, Nature and Shadow magic.",
            @"Your pet takes 15% less damage from Arcane, Fire, Frost, Nature and Shadow magic." }, "spell_nature_resistnature");
        /// <summary>Increases your pet's total Stamina by [4*Pts]%.</summary>
        public readonly static PetTalentData GreatStamina = new PetTalentData(14, "Great Stamina", 3, new string[] {
            @"Increases your pet's total Stamina by [4*Pts]%.",
            @"Increases your pet's total Stamina by 4%.",
            @"Increases your pet's total Stamina by 8%.",
            @"Increases your pet's total Stamina by 12%." }, "spell_nature_unyeildingstamina");
        /// <summary>Your pet's Growl generates [10*Pts]% additional threat and 10% of it's total happiness.</summary>
        public readonly static PetTalentData GuardDog = new PetTalentData(15, "Guard Dog", 2, new string[] {
            @"Your pet's Growl generates [10*Pts]% additional threat and 10% of it's total happiness.",
            @"Your pet's Growl generates 10% additional threat and 10% of it's total happiness.",
            @"Your pet's Growl generates 20% additional threat and 10% of it's total happiness." }, "ability_physical_taunt");
        /// <summary>When your pet dies, it will miraculously return to life with full health.</summary>
        public readonly static PetTalentData HeartOfThePhoenix = new PetTalentData(16, "Heart Of The Phoenix", 1, new string[] {
            @"When your pet dies, it will miraculously return to life with full health.",
            @"When your pet dies, it will miraculously return to life with full health." }, "inv_misc_pheonixpet_01");
        /// <summary>Your pet's Cower also decreases damage taken by [10*Pts]% for the next 10 sec.</summary>
        public readonly static PetTalentData ImprovedCower = new PetTalentData(17, "Improved Cower", 2, new string[] {
            @"Your pet's Cower also decreases damage taken by [10*Pts]% for the next 10 sec.",
            @"Your pet's Cower also decreases damage taken by 10% for the next 10 sec.",
            @"Your pet's Cower also decreases damage taken by 20% for the next 10 sec." }, "ability_druid_cower");
        /// <summary>Your pet runs at high speed towards a group member, intercepting the next melee or ranged attack made against them.</summary>
        public readonly static PetTalentData Intervene = new PetTalentData(18, "Intervene", 1, new string[] {
            @"Your pet runs at high speed towards a group member, intercepting the next melee or ranged attack made against them.",
            @"Your pet runs at high speed towards a group member, intercepting the next melee or ranged attack made against them." }, "ability_hunter_pet_turtle");
        /// <summary>Your pet temporarily gains 30% of it's maximum health for 20 sec. After the effect expires, the health is lost.</summary>
        public readonly static PetTalentData LastStand = new PetTalentData(19, "Last Stand", 1, new string[] {
            @"Your pet temporarily gains 30% of it's maximum health for 20 sec. After the effect expires, the health is lost.",
            @"Your pet temporarily gains 30% of it's maximum health for 20 sec. After the effect expires, the health is lost." }, "spell_nature_shamanrage");
        /// <summary>Your pet heals itself for 100% of its total health over 5 sec while channeling.</summary>
        public readonly static PetTalentData LickYourWounds = new PetTalentData(20, "Lick Your Wounds", 1, new string[] {
            @"Your pet heals itself for 100% of its total health over 5 sec while channeling.",
            @"Your pet heals itself for 100% of its total health over 5 sec while channeling." }, "ability_hunter_mendpet");
        /// <summary>Reduces the duration of all Stun and Fear effects used against your pet by [15*Pts]%.</summary>
        public readonly static PetTalentData Lionhearted = new PetTalentData(21, "Lionhearted", 2, new string[] {
            @"Reduces the duration of all Stun and Fear effects used against your pet by [15*Pts]%.",
            @"Reduces the duration of all Stun and Fear effects used against your pet by 15%.",
            @"Reduces the duration of all Stun and Fear effects used against your pet by 30%." }, "inv_bannerpvp_02");
        /// <summary>Reduces the cooldown on your pet's Dive/Dash ability by [8*Pts] sec.</summary>
        public readonly static PetTalentData Mobility = new PetTalentData(22, "Mobility", 2, new string[] {
            @"Reduces the cooldown on your pet's Dive/Dash ability by [8*Pts] sec.",
            @"Reduces the cooldown on your pet's Dive/Dash ability by 8 sec.",
            @"Reduces the cooldown on your pet's Dive/Dash ability by 16 sec." }, "ability_hunter_animalhandler");
        /// <summary>Increases your pet's armor by [5*Pts]%.</summary>
        public readonly static PetTalentData NaturalArmor = new PetTalentData(23, "Natural Armor", 2, new string[] {
            @"Increases your pet's armor by [5*Pts]%.",
            @"Increases your pet's armor by 5%.",
            @"Increases your pet's armor by 10%." }, "spell_nature_spiritarmor");
        /// <summary>Your pet has a [15*Pts]% chance after using an ability that the next ability will cost no Focus if used within 8 sec.</summary>
        public readonly static PetTalentData OwlsFocus = new PetTalentData(24, "Owl's Focus", 2, new string[] {
            @"Your pet has a [15*Pts]% chance after using an ability that the next ability will cost no Focus if used within 8 sec.",
            @"Your pet has a 15% chance after using an ability that the next ability will cost no Focus if used within 8 sec.",
            @"Your pet has a 30% chance after using an ability that the next ability will cost no Focus if used within 8 sec."}, "ability_hunter_pet_owl");
        /// <summary>Increases your pet's armor by [5*Pts]% and chance to Dodge by [1*Pts]%.</summary>
        public readonly static PetTalentData PetBarding = new PetTalentData(25, "Pet Barding", 2, new string[] {
            @"Increases your pet's armor by [5*Pts]% and chance to Dodge by [1*Pts]%.",
            @"Increases your pet's armor by 5% and chance to Dodge by 1%.",
            @"Increases your pet's armor by 10% and chance to Dodge by 2%." }, "inv_helmet_94");
        /// <summary>Your pet goes into a killing frenzy. Successful attacks have a chance to increase attack power by 5%. This effect will stack up to 5 times. Lasts 20 sec.</summary>
        public readonly static PetTalentData Rabid = new PetTalentData(26, "Rabid", 1, new string[] {
            @"Your pet goes into a killing frenzy. Successful attacks have a chance to increase attack power by 5%. This effect will stack up to 5 times. Lasts 20 sec.",
            @"Your pet goes into a killing frenzy. Successful attacks have a chance to increase attack power by 5%. This effect will stack up to 5 times. Lasts 20 sec." }, "ability_druid_berserk");
        /// <summary>Your pet's inspiring roar restores 30% of your total mana over 9 sec.</summary>
        public readonly static PetTalentData RoarOfRecovery = new PetTalentData(27, "Roar Of Recovery", 1, new string[] {
            @"Your pet's inspiring roar restores 30% of your total mana over 9 sec.",
            @"Your pet's inspiring roar restores 30% of your total mana over 9 sec." }, "ability_druid_mastershapeshifter");
        /// <summary>Protects a friendly target from critical strikes, making attacks against that target unable to be critical strikes, but 20% of all damage taken by that target is also taken by the pet. Lasts 12 sec.</summary>
        public readonly static PetTalentData RoarOfSacrifice = new PetTalentData(28, "Roar Of Sacrifice", 1, new string[] {
            @"Protects a friendly target from critical strikes, making attacks against that target unable to be critical strikes, but 20% of all damage taken by that target is also taken by the pet. Lasts 12 sec.",
            @"Protects a friendly target from critical strikes, making attacks against that target unable to be critical strikes, but 20% of all damage taken by that target is also taken by the pet. Lasts 12 sec." }, "ability_druid_demoralizingroar");
        /// <summary>Your pet does an additional [3*Pts]% damage with all attacks.</summary>
        public readonly static PetTalentData SharkAttack = new PetTalentData(29, "Shark Attack", 2, new string[] {
            @"Your pet does an additional [3*Pts]% damage with all attacks.",
            @"Your pet does an additional 3% damage with all attacks.",
            @"Your pet does an additional 6% damage with all attacks." }, "inv_misc_fish_35");
        /// <summary>Your pet's Growl also heals it for [1*Pts]% of its total health.</summary>
        public readonly static PetTalentData Silverback = new PetTalentData(30, "Silverback", 2, new string[] {
            @"Your pet's Growl also heals it for [1*Pts]% of its total health.",
            @"Your pet's Growl also heals it for 1% of its total health.",
            @"Your pet's Growl also heals it for 2% of its total health." }, "ability_hunter_pet_gorilla");
        /// <summary>Increases the critical strike chance of your pet by [3*Pts]%.</summary>
        public readonly static PetTalentData SpidersBite = new PetTalentData(31, "Spider's Bite", 3, new string[] {
            @"Increases the critical strike chance of your pet by [3*Pts]%.",
            @"Increases the critical strike chance of your pet by 3%.",
            @"Increases the critical strike chance of your pet by 6%.",
            @"Increases the critical strike chance of your pet by 9%." }, "ability_hunter_pet_spider");
        /// <summary>Your pet does an additional [3*Pts]% damage with all attacks.</summary>
        public readonly static PetTalentData SpikedCollar = new PetTalentData(32, "Spiked Collar", 3, new string[] {
            @"Your pet does an additional [3*Pts]% damage with all attacks.",
            @"Your pet does an additional 3% damage with all attacks.",
            @"Your pet does an additional 6% damage with all attacks.",
            @"Your pet does an additional 9% damage with all attacks." }, "inv_jewelry_necklace_22");
        /// <summary>Your pet taunts the target to attack it for 3 sec.</summary>
        public readonly static PetTalentData Taunt = new PetTalentData(33, "Taunt", 1, new string[] {
            @"Your pet taunts the target to attack it for 3 sec.",
            @"Your pet taunts the target to attack it for 3 sec." }, "spell_nature_reincarnation");
        /// <summary>Shakes the ground with thundering force, doing 3 to 5 Nature damage to all enemeies within 8 yards. This ability causes a moderate amount of additional threat.</summary>
        public readonly static PetTalentData Thunderstomp = new PetTalentData(34, "Thunderstomp", 1, new string[] {
            @"Shakes the ground with thundering force, doing 3 to 5 Nature damage to all enemeies within 8 yards. This ability causes a moderate amount of additional threat.",
            @"Shakes the ground with thundering force, doing 3 to 5 Nature damage to all enemeies within 8 yards. This ability causes a moderate amount of additional threat." }, "ability_golemthunderclap");
        /// <summary>Increases the contribution your pet gets from your Stamina by [20*Pts]% and attack power by [15*Pts]%.</summary>
        public readonly static PetTalentData WildHunt = new PetTalentData(35, "Wild Hunt", 2, new string[] {
            @"Increases the contribution your pet gets from your Stamina by [20*Pts]% and attack power by [15*Pts]%.",
            @"Increases the contribution your pet gets from your Stamina by 20% and attack power by 15%.",
            @"Increases the contribution your pet gets from your Stamina by 40% and attack power by 30%." }, "inv_misc_horn_04");
        /// <summary>A fierce attack causing 5 damage, modified by pet level, that your pet can use after its target dodges. Cannot be dodged, blocked or parried.</summary>
        public readonly static PetTalentData WolverineBite = new PetTalentData(36, "Wolverine Bite", 1, new string[] {
            @"A fierce attack causing 5 damage, modified by pet level, that your pet can use after its target dodges. Cannot be dodged, blocked or parried.",
            @"A fierce attack causing 5 damage, modified by pet level, that your pet can use after its target dodges. Cannot be dodged, blocked or parried." }, "ability_druid_lacerate");
    }

    public class PetTalentTree {
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
                foreach (char pt in source.ToCharArray()) {
                    TalentTree[index].Value = Math.Min(TalentTree[index].Max, int.Parse(pt.ToString()));
                    index++;
                }
            } catch (Exception /*ex*/) {
                /* We don't care that it's wrong
                 Rawr.Base.ErrorBox eb = new Rawr.Base.ErrorBox("Error rebuilding the pet talents from file",
                    ex.Message, "PetTalentTree(source)", "Source: " + source, ex.StackTrace);*/
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
                    BloodOfTheRhino,
                    Bloodthirsty,
                    BoarsSpeed,
                    Bullheaded,
                    CallOfTheWild,
                    CarrionFeeder,
                    ChargeSwoop,
                    CobraReflexes,
                    Cornered,
                    CullingTheHerd,
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
                };
            }
        }
        #region Talents
        /// <summary>Increases your pet's total Stamina by [2*Pts]% and increases all healing effects on your pet by [20*Pts]%.</summary>
        [XmlIgnore]
        public PetTalent BloodOfTheRhino;
        /// <summary>Your pet's attacks have a [10*Pts]% chance to increase its happiness by 5% and heal 5% of its total health.</summary>
        [XmlIgnore]
        public PetTalent Bloodthirsty;
        /// <summary>Increases your pet's movement speed by 30%.</summary>
        [XmlIgnore]
        public PetTalent BoarsSpeed;
        /// <summary>Removes all movement impairing effects and all effects which cause loss of control of your pet, and reduces damage done to your pet by 20% for 12 sec.</summary>
        [XmlIgnore]
        public PetTalent Bullheaded;
        /// <summary>Your pet roars, increasing your pet's and your melee and ranged attack power by 10%. Lasts 20 sec. 5 min cooldown.</summary>
        [XmlIgnore]
        public PetTalent CallOfTheWild;
        /// <summary>Your pet can generate health and happiness by eating a corpse. Will not work on the remains of elemental or mechanical creatures.</summary>
        [XmlIgnore]
        public PetTalent CarrionFeeder;
        /// <summary>Your pet charges an enemy, immobilizing the target for 1 sec, and increasing the pet's melee attack power by 25% for its next attack.</summary>
        [XmlIgnore]
        public PetTalent ChargeSwoop;
        /// <summary>Increases your pet's attack speed by [15*Pts]%. Your pet will hit faster but each hit will do less damage.</summary>
        [XmlIgnore]
        public PetTalent CobraReflexes;
        /// <summary>When at less than 35% health, your pet does [25*Pts]% more damage and has a [30*Pts]% reduced chance to be critically hit.</summary>
        [XmlIgnore]
        public PetTalent Cornered;
        /// <summary>Increases your pet's movement speed by 80% for 16 sec.</summary>
        [XmlIgnore]
        public PetTalent DiveDash;
        /// <summary>Your pet does [8*Pts]% additional damage to targets with less than 35% health.</summary>
        [XmlIgnore]
        public PetTalent FeedingFrenzy;
        /// <summary>Reduces the chance your pet will be critically hit by melee attacks by [2*Pts]%.</summary>
        [XmlIgnore]
        public PetTalent GraceOfTheMantis;
        /// <summary>Your pet takes [5*Pts]% less damage from Arcane, Fire, Frost, Nature and Shadow magic.</summary>
        [XmlIgnore]
        public PetTalent GreatResistance;
        /// <summary>Increases your pet's total Stamina by [4*Pts]%.</summary>
        [XmlIgnore]
        public PetTalent GreatStamina;
        /// <summary>Your pet's Growl generates [10*Pts]% additional threat and 10% of it's total happiness.</summary>
        [XmlIgnore]
        public PetTalent GuardDog;
        /// <summary>When your pet dies, it will miraculously return to life with full health.</summary>
        [XmlIgnore]
        public PetTalent HeartOfThePhoenix;
        /// <summary>Your pet's Cower also decreases damage taken by [10*Pts]% for the next 10 sec.</summary>
        [XmlIgnore]
        public PetTalent ImprovedCower;
        /// <summary>Your pet runs at high speed towards a group member, intercepting the next melee or ranged attack made against them.</summary>
        [XmlIgnore]
        public PetTalent Intervene;
        /// <summary>Your pet temporarily gains 30% of it's maximum health for 20 sec. After the effect expires, the health is lost.</summary>
        [XmlIgnore]
        public PetTalent LastStand;
        /// <summary>Your pet heals itself for 100% of its total health over 5 sec while channeling.</summary>
        [XmlIgnore]
        public PetTalent LickYourWounds;
        /// <summary>Reduces the duration of all Stun and Fear effects used against your pet by [15*Pts]%.</summary>
        [XmlIgnore]
        public PetTalent Lionhearted;
        /// <summary>Reduces the cooldown on your pet's Dive/Dash ability by [8*Pts] sec.</summary>
        [XmlIgnore]
        public PetTalent Mobility;
        /// <summary>Increases your pet's armor by [5*Pts]%.</summary>
        [XmlIgnore]
        public PetTalent NaturalArmor;
        /// <summary>Your pet has a [15*Pts]% chance after using an ability that the next ability will cost no Focus if used within 8 sec.</summary>
        [XmlIgnore]
        public PetTalent OwlsFocus;
        /// <summary>Increases your pet's armor by [5*Pts]% and chance to Dodge by [1*Pts]%.</summary>
        [XmlIgnore]
        public PetTalent PetBarding;
        /// <summary>Your pet goes into a killing frenzy. Successful attacks have a chance to increase attack power by 5%. This effect will stack up to 5 times. Lasts 20 sec.</summary>
        [XmlIgnore]
        public PetTalent Rabid;
        /// <summary>Your pet's inspiring roar restores 30% of your total mana over 9 sec.</summary>
        [XmlIgnore]
        public PetTalent RoarOfRecovery;
        /// <summary>Protects a friendly target from critical strikes, making attacks against that target unable to be critical strikes, but 20% of all damage taken by that target is also taken by the pet. Lasts 12 sec.</summary>
        [XmlIgnore]
        public PetTalent RoarOfSacrifice;
        /// <summary>Your pet does an additional [3*Pts]% damage with all attacks.</summary>
        [XmlIgnore]
        public PetTalent SharkAttack;
        /// <summary>Your pet's Growl also heals it for [1*Pts]% of its total health.</summary>
        [XmlIgnore]
        public PetTalent Silverback;
        /// <summary>Increases the critical strike chance of your pet by [3*Pts]%.</summary>
        [XmlIgnore]
        public PetTalent SpidersBite;
        /// <summary>Your pet does an additional [3*Pts]% damage with all attacks.</summary>
        [XmlIgnore]
        public PetTalent SpikedCollar;
        /// <summary>Your pet taunts the target to attack it for 3 sec.</summary>
        [XmlIgnore]
        public PetTalent Taunt;
        /// <summary>Shakes the ground with thundering force, doing 3 to 5 Nature damage to all enemeies within 8 yards. This ability causes a moderate amount of additional threat.</summary>
        [XmlIgnore]
        public PetTalent Thunderstomp;
        /// <summary>Increases the contribution your pet gets from your Stamina by [20*Pts]% and attack power by [15*Pts]%.</summary>
        [XmlIgnore]
        public PetTalent WildHunt;
        /// <summary>A fierce attack causing 5 damage, modified by pet level, that your pet can use after its target dodges. Cannot be dodged, blocked or parried.</summary>
        [XmlIgnore]
        public PetTalent WolverineBite;
        /// <summary>Increases pet and hunter damage by [1*Pts]% for 10 seconds each time the pet deals a critical strike with Claw, Bite, or Smack.</summary>
        [XmlIgnore]
        public PetTalent CullingTheHerd;
        #endregion
        /// <summary>Initializes the Abilities</summary>
        public void Initialize() {
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
            CullingTheHerd.Value = 0;
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

        public int TotalPoints() {
            int total = 0;
            foreach (PetTalent pt in TalentTree) { total += pt.Value; }
            return total;
        }
        public PetFamilyTree GetClass() {
            if (RoarOfRecovery.Value + Cornered.Value + Bullheaded.Value + FeedingFrenzy.Value > 0)
                return PetFamilyTree.Cunning;
            else if (SharkAttack.Value + Rabid.Value + LickYourWounds.Value > 0)
                return PetFamilyTree.Ferocity;
            else if (Silverback.Value + LastStand.Value + Intervene.Value + Thunderstomp.Value + BloodOfTheRhino.Value > 0)
                return PetFamilyTree.Tenacity;
            else
                return PetFamilyTree.None;
        }
        public override string ToString()
        {
            string ret = "";
            if (TalentTree == null
                || TalentTree.Count <= 0
                || TalentTree.Count > 38)
            {
                if (TalentTree != null) { TalentTree.Clear(); }
                Initialize();
            }
            foreach (PetTalent pt in TalentTree) {
                ret += pt.Value.ToString();
            }
            return ret;
        }

        public static PetTalentTree FromArmoryPet(ArmoryPet pet) {
            PetTalentTree retVal = new PetTalentTree();
            string armoryspec = pet.Spec;
            try {
                retVal.Reset();
                switch (pet.SpecKey) {
                    case "Ferocity": {
                        if (armoryspec.Length < 21) break;
                        // Tier 1
                        retVal.CobraReflexes.Value = int.Parse(armoryspec[00].ToString());
                        //retVal.Unknown.Value = int.Parse(armoryspec[01].ToString());
                        retVal.DiveDash.Value = int.Parse(armoryspec[02].ToString());
                        retVal.GreatStamina.Value = int.Parse(armoryspec[03].ToString());
                        retVal.NaturalArmor.Value = int.Parse(armoryspec[04].ToString());
                        // Tier 2
                        retVal.ImprovedCower.Value = int.Parse(armoryspec[05].ToString());
                        retVal.Bloodthirsty.Value = int.Parse(armoryspec[06].ToString());
                        retVal.SpikedCollar.Value = int.Parse(armoryspec[07].ToString());
                        retVal.BoarsSpeed.Value = int.Parse(armoryspec[08].ToString());
                        // Tier 3
                        retVal.CullingTheHerd.Value = int.Parse(armoryspec[09].ToString());
                        //retVal.Unknown.Value = int.Parse(armoryspec[10].ToString());
                        retVal.Lionhearted.Value = int.Parse(armoryspec[11].ToString());
                        retVal.ChargeSwoop.Value = int.Parse(armoryspec[12].ToString());
                        // Tier 4
                        retVal.HeartOfThePhoenix.Value = int.Parse(armoryspec[13].ToString());
                        retVal.SpidersBite.Value = int.Parse(armoryspec[14].ToString());
                        retVal.GreatResistance.Value = int.Parse(armoryspec[15].ToString());
                        // Tier 5
                        retVal.Rabid.Value = int.Parse(armoryspec[16].ToString());
                        retVal.LickYourWounds.Value = int.Parse(armoryspec[17].ToString());
                        retVal.CallOfTheWild.Value = int.Parse(armoryspec[18].ToString());
                        // Tier 6
                        retVal.SharkAttack.Value = int.Parse(armoryspec[19].ToString());
                        retVal.WildHunt.Value = int.Parse(armoryspec[20].ToString());
                        break;
                    }
                    case "Cunning": {
                        if (armoryspec.Length < 22) break;
                        // Tier 1
                        retVal.CobraReflexes.Value = int.Parse(armoryspec[00].ToString());
                        //retVal.Unknown.Value = int.Parse(armoryspec[01].ToString());
                        retVal.DiveDash.Value = int.Parse(armoryspec[02].ToString());
                        retVal.GreatStamina.Value = int.Parse(armoryspec[03].ToString());
                        retVal.NaturalArmor.Value = int.Parse(armoryspec[04].ToString());
                        // Tier 2
                        retVal.BoarsSpeed.Value = int.Parse(armoryspec[05].ToString());
                        //retVal.Unknown.Value = int.Parse(armoryspec[06].ToString());
                        retVal.Mobility.Value = int.Parse(armoryspec[07].ToString());
                        retVal.OwlsFocus.Value = int.Parse(armoryspec[08].ToString());
                        retVal.SpikedCollar.Value = int.Parse(armoryspec[09].ToString());
                        // Tier 3
                        retVal.CullingTheHerd.Value = int.Parse(armoryspec[10].ToString());
                        retVal.Lionhearted.Value = int.Parse(armoryspec[11].ToString());
                        retVal.CarrionFeeder.Value = int.Parse(armoryspec[12].ToString());
                        // Tier 4
                        retVal.GreatResistance.Value = int.Parse(armoryspec[13].ToString());
                        retVal.Cornered.Value = int.Parse(armoryspec[14].ToString());
                        retVal.FeedingFrenzy.Value = int.Parse(armoryspec[15].ToString());
                        // Tier 5
                        retVal.WolverineBite.Value = int.Parse(armoryspec[16].ToString());
                        retVal.RoarOfRecovery.Value = int.Parse(armoryspec[17].ToString());
                        retVal.Bullheaded.Value = int.Parse(armoryspec[18].ToString());
                        retVal.GraceOfTheMantis.Value = int.Parse(armoryspec[19].ToString());
                        // Tier 6
                        retVal.WildHunt.Value = int.Parse(armoryspec[20].ToString());
                        retVal.RoarOfSacrifice.Value = int.Parse(armoryspec[21].ToString());
                        break;
                    }
                    case "Tenacity": {
                        if (armoryspec.Length < 20) break;
                        // Tier 1
                        retVal.CobraReflexes.Value = int.Parse(armoryspec[00].ToString());
                        retVal.ChargeSwoop.Value = int.Parse(armoryspec[01].ToString());
                        retVal.GreatStamina.Value = int.Parse(armoryspec[02].ToString());
                        retVal.NaturalArmor.Value = int.Parse(armoryspec[03].ToString());
                        // Tier 2
                        retVal.SpikedCollar.Value = int.Parse(armoryspec[04].ToString());
                        retVal.BoarsSpeed.Value = int.Parse(armoryspec[05].ToString());
                        retVal.BloodOfTheRhino.Value = int.Parse(armoryspec[06].ToString());
                        retVal.PetBarding.Value = int.Parse(armoryspec[07].ToString());
                        // Tier 3
                        retVal.CullingTheHerd.Value = int.Parse(armoryspec[08].ToString());
                        retVal.GuardDog.Value = int.Parse(armoryspec[09].ToString());
                        retVal.Lionhearted.Value = int.Parse(armoryspec[10].ToString());
                        retVal.Thunderstomp.Value = int.Parse(armoryspec[11].ToString());
                        // Tier 4
                        retVal.GraceOfTheMantis.Value = int.Parse(armoryspec[12].ToString());
                        retVal.GreatResistance.Value = int.Parse(armoryspec[13].ToString());
                        // Tier 5
                        retVal.LastStand.Value = int.Parse(armoryspec[14].ToString());
                        retVal.Taunt.Value = int.Parse(armoryspec[15].ToString());
                        retVal.RoarOfSacrifice.Value = int.Parse(armoryspec[16].ToString());
                        retVal.Intervene.Value = int.Parse(armoryspec[17].ToString());
                        // Tier 6
                        retVal.Silverback.Value = int.Parse(armoryspec[18].ToString());
                        retVal.WildHunt.Value = int.Parse(armoryspec[19].ToString());
                        break;
                    }
                    default: { throw new Exception("Failed to determine armory pet spec key"); }
                }

                return retVal;
            }catch(Exception ex){
                Rawr.Base.ErrorBox eb = new Rawr.Base.ErrorBox(
                    "Error converting Armory Pet Talents to PetTalentTree",
                    ex.Message,
                    "FromArmoryPet(ArmoryPet pet)",
                    "No Additional Info",
                    ex.StackTrace
                );
            }
            return null;
        }
    }
}
