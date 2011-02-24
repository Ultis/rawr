using System;
using System.Collections.Generic;
using System.ComponentModel;
#if !RAWR3 && !RAWR4 && !SILVERLIGHT
using System.Data;
using System.Drawing;
#endif
using System.Text;
//using System.Windows.Forms;
using System.Xml.Serialization;

namespace Rawr.Hunter
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class PetTalentDataAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">Unique identifier</param>
        /// <param name="name">Talent Name</param>
        /// <param name="maxPoints">Maximum number of Ranks player can assign</param>
        /// <param name="trees">The flags of the Trees (0,1,2), that the talent shows up in</param>
        /// <param name="columns">1 based array, 4 columns (1,2,3,4)</param>
        /// <param name="rows">1 based array, 6 Rows (1,2,3,4,5,6)</param>
        /// <param name="prerequisites">The ID of another Pet Talent that must be maxed before this one can be selected</param>
        /// <param name="description">String array of descriptive stuff</param>
        /// <param name="icon">The icon to use for this talent, must specify the back end name for it</param>
        public PetTalentDataAttribute(int index, string name, int maxPoints, bool[] trees, int[] columns, int[] rows, int[] prerequisites, string[] description, string icon)
        {
            _index = index;
            _name = name;
            _maxPoints = maxPoints;
            _trees = trees;
            _columns = columns;
            _rows = rows;
            _prerequisites = prerequisites;
            _description = description;
            _icon = icon;
        }

        private readonly int _index;
        private readonly string _name;
        private readonly int _maxPoints;
        private readonly bool[] _trees;
        private readonly int[] _columns;
        private readonly int[] _rows;
        private readonly int[] _prerequisites;
        private readonly string _icon;
        private readonly string[] _description;

        public int Index { get { return _index; } }
        public string Name { get { return _name; } }
        public int MaxPoints { get { return _maxPoints; } }
        public bool[] Trees { get { return _trees; } }
        public int[] Columns { get { return _columns; } }
        public int[] Rows { get { return _rows; } }
        public int[] Prerequisites { get { return _prerequisites; } }
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

        public abstract PetTalentsBase Clone();
    }

    public partial class PetTalents : PetTalentsBase
    {
        public override PetTalentsBase Clone()
        {
            PetTalents clone = (PetTalents)MemberwiseClone();
            clone._data = (int[])_data.Clone();
            return clone;
        }

        private int[] _data = new int[38];
        public override int[] Data { get { return _data; } }
        public PetTalents() { }
        public PetTalents(string talents) { LoadString(talents); }
        public static string[] TreeNames = new string[] { "Cunning", "Ferocity", "Tenacity" };
        /*public static string[] TreeBackgrounds = new string[] {
            "http://static.wowhead.com/images/wow/hunterpettalents/live/bg_3.jpg",
            "http://static.wowhead.com/images/wow/hunterpettalents/live/bg_1.jpg",
            "http://static.wowhead.com/images/wow/hunterpettalents/live/bg_2.jpg",
        };*/

        public static PetTalents FromArmoryPet(ArmoryPet pet)
        {
            if (pet.Spec == "") return new PetTalents(); // send back a blank one
            PetTalents retVal = new PetTalents();
            string armoryspec = pet.Spec;
            try
            {
                //retVal.Reset();
                switch (pet.SpecKey)
                {
                    case "Ferocity":
                        {
                            if (armoryspec.Length < 22) break;
                            // Tier 1
                            retVal.SerpentSwiftness = int.Parse(armoryspec[00].ToString());
                            retVal.DiveDash = int.Parse(armoryspec[01].ToString()) + int.Parse(armoryspec[02].ToString());
                            retVal.GreatStamina = int.Parse(armoryspec[03].ToString());
                            retVal.NaturalArmor = int.Parse(armoryspec[04].ToString());
                            // Tier 2
                            retVal.ImprovedCower = int.Parse(armoryspec[05].ToString());
                            retVal.Bloodthirsty = int.Parse(armoryspec[06].ToString());
                            retVal.SpikedCollar = int.Parse(armoryspec[07].ToString());
                            retVal.BoarsSpeed = int.Parse(armoryspec[08].ToString());
                            // Tier 3
                            retVal.CullingTheHerd = int.Parse(armoryspec[09].ToString());
                            retVal.Lionhearted = int.Parse(armoryspec[10].ToString());
                            retVal.ChargeSwoop = int.Parse(armoryspec[11].ToString()) + int.Parse(armoryspec[12].ToString());
                            // Tier 4
                            retVal.HeartOfThePhoenix = int.Parse(armoryspec[13].ToString());
                            retVal.SpidersBite = int.Parse(armoryspec[14].ToString());
                            retVal.GreatResistance = int.Parse(armoryspec[15].ToString());
                            // Tier 5
                            retVal.Rabid = int.Parse(armoryspec[16].ToString());
                            retVal.LickYourWounds = int.Parse(armoryspec[17].ToString());
                            retVal.CallOfTheWild = int.Parse(armoryspec[18].ToString());
                            // Tier 6
                            retVal.SharkAttack = int.Parse(armoryspec[19].ToString());
                            retVal.WildHunt = int.Parse(armoryspec[20].ToString());
                            break;
                        }
                    case "Cunning":
                        {
                            if (armoryspec.Length < 22) break;
                            // Tier 1
                            retVal.SerpentSwiftness = int.Parse(armoryspec[00].ToString());
                            //retVal.Unknown = int.Parse(armoryspec[01].ToString());
                            retVal.DiveDash = int.Parse(armoryspec[02].ToString());
                            retVal.GreatStamina = int.Parse(armoryspec[03].ToString());
                            retVal.NaturalArmor = int.Parse(armoryspec[04].ToString());
                            // Tier 2
                            retVal.BoarsSpeed = int.Parse(armoryspec[05].ToString());
                            //retVal.Unknown = int.Parse(armoryspec[06].ToString());
                            retVal.Mobility = int.Parse(armoryspec[07].ToString());
                            retVal.OwlsFocus = int.Parse(armoryspec[08].ToString());
                            retVal.SpikedCollar = int.Parse(armoryspec[09].ToString());
                            // Tier 3
                            retVal.CullingTheHerd = int.Parse(armoryspec[10].ToString());
                            retVal.Lionhearted = int.Parse(armoryspec[11].ToString());
                            retVal.CarrionFeeder = int.Parse(armoryspec[12].ToString());
                            // Tier 4
                            retVal.GreatResistance = int.Parse(armoryspec[13].ToString());
                            retVal.Cornered = int.Parse(armoryspec[14].ToString());
                            retVal.FeedingFrenzy = int.Parse(armoryspec[15].ToString());
                            // Tier 5
                            retVal.WolverineBite = int.Parse(armoryspec[16].ToString());
                            retVal.RoarOfRecovery = int.Parse(armoryspec[17].ToString());
                            retVal.Bullheaded = int.Parse(armoryspec[18].ToString());
                            retVal.GraceOfTheMantis = int.Parse(armoryspec[19].ToString());
                            // Tier 6
                            retVal.WildHunt = int.Parse(armoryspec[20].ToString());
                            retVal.RoarOfSacrifice = int.Parse(armoryspec[21].ToString());
                            break;
                        }
                    case "Tenacity":
                        {
                            if (armoryspec.Length < 22) break;
                            // Tier 1
                            retVal.SerpentSwiftness = int.Parse(armoryspec[00].ToString());
                            retVal.ChargeSwoop = int.Parse(armoryspec[01].ToString());
                            retVal.GreatStamina = int.Parse(armoryspec[02].ToString());
                            retVal.NaturalArmor = int.Parse(armoryspec[03].ToString());
                            // Tier 2
                            retVal.SpikedCollar = int.Parse(armoryspec[04].ToString());
                            retVal.BoarsSpeed = int.Parse(armoryspec[05].ToString());
                            retVal.BloodOfTheRhino = int.Parse(armoryspec[06].ToString());
                            retVal.PetBarding = int.Parse(armoryspec[07].ToString());
                            // Tier 3
                            retVal.CullingTheHerd = int.Parse(armoryspec[08].ToString());
                            retVal.GuardDog = int.Parse(armoryspec[09].ToString());
                            retVal.Lionhearted = int.Parse(armoryspec[10].ToString());
                            retVal.Thunderstomp = int.Parse(armoryspec[11].ToString());
                            // Tier 4
                            retVal.GraceOfTheMantis = int.Parse(armoryspec[12].ToString());
                            retVal.GreatResistance = int.Parse(armoryspec[13].ToString());
                            // Tier 5
                            retVal.LastStand = int.Parse(armoryspec[14].ToString());
                            retVal.Taunt = int.Parse(armoryspec[15].ToString());
                            retVal.RoarOfSacrifice = int.Parse(armoryspec[16].ToString());
                            retVal.Intervene = int.Parse(armoryspec[17].ToString());
                            // Tier 6
                            retVal.Silverback = int.Parse(armoryspec[18].ToString());
                            retVal.WildHunt = int.Parse(armoryspec[19].ToString());
                            break;
                        }
                    default: { throw new Exception("Failed to determine armory pet spec key"); }
                }

                return retVal;
            } catch (Exception ex) {
                new Base.ErrorBox()
                {
                    Title = "Error converting Armory Pet Talents to PetTalents",
                    Function = "FromArmoryPet(ArmoryPet pet)",
                    TheException = ex,
                }.Show();
            }
            return null;
        }

        #region PetTalentData filled in
        #region Tier 1
        /// <summary>Increases your pet's attack speed by [5*Pts]%.</summary>
        [PetTalentData(0, "Serpent Swiftness", 2, new bool[] {true, true, true }, new int[] { 1, 1, 1 }, new int[] { 1, 1, 1 }, new int[] { -1, -1, -1 }, new string[] {
        @"Increases your pet's attack speed by 5%.",
        @"Increases your pet's attack speed by 10%." }, "ability_hunter_serpentswiftness")]
        public int SerpentSwiftness { get { return _data[0]; } set { _data[0] = value; } }

        /// <summary>Increases your pet's movement speed by 80% for 16 sec.</summary>
        [PetTalentData(1, "Dive/Dash", 1, new bool[] { true, true, false }, new int[] { 2, 2, 2 }, new int[] { 1, 1, 1 }, new int[] { -1, -1, -1 }, new string[] {
        @"Increases your pet's movement speed by 80% for 16 sec." }, "spell_shadow_burningspirit")]
        public int DiveDash { get { return _data[1]; } set { _data[1] = value; } }

        /// <summary>Increases your pet's total Stamina by [4*Pts]%.</summary>
        [PetTalentData(3, "Great Stamina", 3, new bool[] { true, true, true }, new int[] { 3, 3, 3 }, new int[] { 1, 1, 1 }, new int[] { -1, -1, -1 }, new string[] {
        @"Increases your pet's total Stamina by 4%.",
        @"Increases your pet's total Stamina by 8%.",
        @"Increases your pet's total Stamina by 12%." }, "spell_nature_unyeildingstamina")]
        public int GreatStamina { get { return _data[3]; } set { _data[3] = value; } }

        /// <summary>Increases your pet's armor by [5*Pts]%.</summary>
        [PetTalentData(4, "Natural Armor", 2, new bool[] { true, true, true }, new int[] { 4, 4, 4 }, new int[] { 1, 1, 1 }, new int[] { -1, -1, -1 }, new string[] {
        @"Increases your pet's armor by 5%.",
        @"Increases your pet's armor by 10%." }, "spell_nature_spiritarmor")]
        public int NaturalArmor { get { return _data[4]; } set { _data[4] = value; } }

        /// <summary>Your pet charges an enemy, immobilizing the target for 1 sec, and increasing the pet's melee attack power by 25% for its next attack.</summary>
        [PetTalentData(5, "Charge/Swoop", 1, new bool[] { false, true, true }, new int[] { 1, 4, 2 }, new int[] { 1, 3, 1 }, new int[] { -1, -1, -1 }, new string[] {
        @"Your pet charges an enemy, immobilizing the target for 1 sec, and increasing the pet's melee attack power by 25% for its next attack." }, "ability_hunter_pet_bear")]
        public int ChargeSwoop { get { return _data[5]; } set { _data[5] = value; } }
        #endregion
        #region Tier 2
        /// <summary>Increases your pet's movement speed by 30%.</summary>
        [PetTalentData(6, "Boar's Speed", 1, new bool[] { true, true, true }, new int[] { 1, 4, 2 }, new int[] { 2, 2, 2 }, new int[] { -1, -1, -1 }, new string[] {
        @"Increases your pet's movement speed by 30%." }, "ability_hunter_pet_boar")]
        public int BoarsSpeed { get { return _data[6]; } set { _data[6] = value; } }

        /// <summary>Reduces the cooldown on your pet's Dive/Dash ability by [8*Pts] sec.</summary>
        [PetTalentData(7, "Mobility", 2, new bool[] { true, false, false }, new int[] { 2, 1, 1 }, new int[] { 2, 1, 1 }, new int[] { 1, -1, -1 }, new string[] {
        @"Reduces the cooldown on your pet's Dive/Dash ability by 8 sec.",
        @"Reduces the cooldown on your pet's Dive/Dash ability by 16 sec." }, "ability_hunter_animalhandler")]
        public int Mobility { get { return _data[7]; } set { _data[7] = value; } }

        /// <summary>Your pet has a [15*Pts]% chance after using a Basic Attack to cause the next Basic Attack to cost no Focus if used within 8 sec</summary>
        [PetTalentData(8, "Owl's Focus", 2, new bool[] { true, false, false }, new int[] { 3, 1, 1 }, new int[] { 2, 1, 1 }, new int[] { -1, -1, -1 }, new string[] {
        @"Your pet has a 15% chance after using a Basic Attack to cause the next Basic Attack to cost no Focus if used within 8 sec.",
        @"Your pet has a 30% chance after using a Basic Attack to cause the next Basic Attack to cost no Focus if used within 8 sec." }, "ability_hunter_pet_owl")]
        public int OwlsFocus { get { return _data[8]; } set { _data[8] = value; } }

        /// <summary>Increases the damage done by your pet's Basic Attacks by [3*Pts]%.</summary>
        [PetTalentData(9, "Spiked Collar", 3, new bool[] { true, true, true }, new int[] { 4, 3, 1 }, new int[] { 2, 2, 2 }, new int[] { -1, -1, -1 }, new string[] {
        @"Increases the damage done by your pet's Basic Attacks by 3%.",
        @"Increases the damage done by your pet's Basic Attacks by 6%.",
        @"Increases the damage done by your pet's Basic Attacks by 9%." }, "inv_jewelry_necklace_22")]
        public int SpikedCollar { get { return _data[9]; } set { _data[9] = value; } }

        /// <summary>The movement speed penalty of your pet's Cower is reduced by [50*Pts]%.</summary>
        [PetTalentData(10, "Improved Cower", 2, new bool[] { false, true, false }, new int[] { 1, 1, 1 }, new int[] { 1, 2, 1 }, new int[] { -1, -1, -1 }, new string[] {
        @"The movement speed penalty of your pet's Cower is reduced by 50%.",
        @"The movement speed penalty of your pet's Cower is reduced by 100%." }, "ability_druid_cower")]
        public int ImprovedCower { get { return _data[10]; } set { _data[10] = value; } }

        /// <summary>Your pet's attacks have a [10*Pts]% chance to increase its happiness by 5% and heal 5% of its total health.</summary>
        [PetTalentData(11, "Bloodthirsty", 2, new bool[] { false, true, false }, new int[] { 1, 2, 1 }, new int[] { 1, 2, 1 }, new int[] { -1, -1, -1 }, new string[] {
        //@"Your pet's attacks have a [10*Pts]% chance to increase its happiness by 5% and heal 5% of its total health.",
        @"Your pet's attacks have a 10% chance to increase its happiness by 5% and heal 5% of its total health.",
        @"Your pet's attacks have a 20% chance to increase its happiness by 5% and heal 5% of its total health." }, "ability_druid_primaltenacity")]
        public int Bloodthirsty { get { return _data[11]; } set { _data[11] = value; } }

        /// <summary>Increases all healing effects on your pet by [20*Pts]%.</summary>
        [PetTalentData(12, "Blood Of The Rhino", 2, new bool[] { false, false, true }, new int[] { 1, 1, 3 }, new int[] { 1, 1, 2 }, new int[] { -1, -1, 3 }, new string[] {
        @"Increases all healing effects on your pet by 20%.",
        @"Increases all healing effects on your pet by 40%." }, "spell_shadow_lifedrain")]
        public int BloodOfTheRhino { get { return _data[12]; } set { _data[12] = value; } }

        /// <summary>Increases your pet's armor by [5*Pts]% and chance to Dodge by [1*Pts]%.</summary>
        [PetTalentData(13, "Pet Barding", 2, new bool[] { false, false, true }, new int[] { 1, 1, 4 }, new int[] { 1, 1, 2 }, new int[] { -1, -1, 4 }, new string[] {
        @"Increases your pet's armor by 5% and chance to Dodge by 1%.",
        @"Increases your pet's armor by 10% and chance to Dodge by 2%." }, "inv_helmet_94")]
        public int PetBarding { get { return _data[13]; } set { _data[13] = value; } }
        #endregion
        #region Tier 3
        /// <summary>When your pet's Basic Attacks deals a critical strike, you and your pet deal [1*Pts]% increased damage for 10 sec.</summary>
        [PetTalentData(14, "Culling The Herd", 3, new bool[] { true, true, true }, new int[] { 1, 1, 1 }, new int[] { 3, 3, 3 }, new int[] { -1, -1, -1 }, new string[] {
        @"When your pet's Basic Attacks deals a critical strike, you and your pet deal 1% increased damage for 10 sec.",
        @"When your pet's Basic Attacks deals a critical strike, you and your pet deal 2% increased damage for 10 sec.",
        @"When your pet's Basic Attacks deals a critical strike, you and your pet deal 3% increased damage for 10 sec." }, "inv_misc_monsterhorn_06")]
        public int CullingTheHerd { get { return _data[14]; } set { _data[14] = value; } }

        /// <summary>Reduces the duration of all Stun and Fear effects used against your pet by [15*Pts]%.</summary>
        [PetTalentData(15, "Lionhearted", 2, new bool[] { true, true, true }, new int[] { 2, 3, 3 }, new int[] { 3, 3, 3 }, new int[] { -1, -1, -1 }, new string[] {
        @"Reduces the duration of all Stun and Fear effects used against your pet by 15%.",
        @"Reduces the duration of all Stun and Fear effects used against your pet by 30%." }, "inv_bannerpvp_02")]
        public int Lionhearted { get { return _data[15]; } set { _data[15] = value; } }

        /// <summary>Your pet can generate health and happiness by eating a corpse. Will not work on the remains of elemental or mechanical creatures.</summary>
        [PetTalentData(16, "Carrion Feeder", 1, new bool[] { true, false, false }, new int[] { 3, 1, 1 }, new int[] { 3, 1, 1 }, new int[] { -1, -1, -1 }, new string[] {
        @"Your pet can generate health and happiness by eating a corpse. Will not work on the remains of elemental or mechanical creatures." }, "ability_racial_cannibalize")]
        public int CarrionFeeder { get { return _data[16]; } set { _data[16] = value; } }

        /// <summary>Your pet's Growl generates [10*Pts]% additional threat and 10% of it's total happiness.</summary>
        [PetTalentData(17, "Guard Dog", 2, new bool[] { false, false, true }, new int[] { 1, 1, 2 }, new int[] { 1, 1, 3 }, new int[] { -1, -1, -1 }, new string[] {
        @"Your pet's Growl generates 10% additional threat and 10% of it's total happiness.",
        @"Your pet's Growl generates 20% additional threat and 10% of it's total happiness." }, "ability_physical_taunt")]
        public int GuardDog { get { return _data[17]; } set { _data[17] = value; } }

        /// <summary>Shakes the ground with thundering force, doing 3 to 5 Nature damage to all enemeies within 8 yards. This ability causes a moderate amount of additional threat.</summary>
        [PetTalentData(18, "Thunderstomp", 1, new bool[] { false, false, true }, new int[] { 1, 1, 4 }, new int[] { 1, 1, 3 }, new int[] { -1, -1, -1 }, new string[] {
        @"Shakes the ground with thundering force, doing 3 to 5 Nature damage to all enemeies within 8 yards. This ability causes a moderate amount of additional threat." }, "ability_golemthunderclap")]
        public int Thunderstomp { get { return _data[18]; } set { _data[18] = value; } }
        #endregion
        #region Tier 4
        /// <summary>Your pet takes [5*Pts]% less damage from Arcane, Fire, Frost, Nature and Shadow magic.</summary>
        [PetTalentData(19, "Great Resistance", 3, new bool[] { true, true, true }, new int[] { 2, 4, 4 }, new int[] { 4, 4, 4 }, new int[] { -1, -1, -1 }, new string[] {
        @"Your pet takes 5% less damage from Arcane, Fire, Frost, Nature and Shadow magic.",
        @"Your pet takes 10% less damage from Arcane, Fire, Frost, Nature and Shadow magic.",
        @"Your pet takes 15% less damage from Arcane, Fire, Frost, Nature and Shadow magic." }, "spell_nature_resistnature")]
        public int GreatResistance { get { return _data[19]; } set { _data[19] = value; } }

        /// <summary>When at less than 35% health, your pet does [25*Pts]% more damage and has a [30*Pts]% reduced chance to be critically hit.</summary>
        [PetTalentData(20, "Cornered", 2, new bool[] { true, false, false }, new int[] { 3, 1, 1 }, new int[] { 4, 1, 1 }, new int[] { -1, -1, -1 }, new string[] {
        @"When at less than 35% health, your pet does 25% more damage and has a 30% reduced chance to be critically hit.",
        @"When at less than 35% health, your pet does 50% more damage and has a 60% reduced chance to be critically hit." }, "ability_hunter_survivalinstincts")]
        public int Cornered { get { return _data[20]; } set { _data[20] = value; } }

        /// <summary>Your pet does [8*Pts]% additional damage to targets with less than 35% health.</summary>
        [PetTalentData(21, "Feeding Frenzy", 2, new bool[] { true, false, false }, new int[] { 4, 1, 1 }, new int[] { 4, 1, 1 }, new int[] { 9, -1, -1 }, new string[] {
        @"Your pet does 8% additional damage to targets with less than 35% health.",
        @"Your pet does 16% additional damage to targets with less than 35% health." }, "inv_misc_fish_48")]
        public int FeedingFrenzy { get { return _data[21]; } set { _data[21] = value; } }

        /// <summary>When used, your pet will miraculously return to life with full health.</summary>
        [PetTalentData(22, "Heart Of The Phoenix", 1, new bool[] { false, true, false }, new int[] { 1, 2, 1 }, new int[] { 1, 4, 1 }, new int[] { -1, 11, -1 }, new string[] {
        @"When used, your pet will miraculously return to life with full health." }, "inv_misc_pheonixpet_01")]
        public int HeartOfThePhoenix { get { return _data[22]; } set { _data[22] = value; } }

        /// <summary>Increases the critical strike chance of your pet by [3*Pts]%.</summary>
        [PetTalentData(23, "Spider's Bite", 3, new bool[] { false, true, false }, new int[] { 1, 3, 1 }, new int[] { 1, 4, 1 }, new int[] { -1, -1, -1 }, new string[] {
        @"Increases the critical strike chance of your pet by 3%.",
        @"Increases the critical strike chance of your pet by 6%.",
        @"Increases the critical strike chance of your pet by 9%." }, "ability_hunter_pet_spider")]
        public int SpidersBite { get { return _data[23]; } set { _data[23] = value; } }

        /// <summary>Reduces the chance your pet will be critically hit by melee attacks by [3*Pts]%.</summary>
        [PetTalentData(24, "Grace Of The Mantis", 2, new bool[] { true, false, true }, new int[] { 4, 1, 3 }, new int[] { 5, 1, 4 }, new int[] { -1, -1, -1 }, new string[] {
        @"Reduces the chance your pet will be critically hit by melee attacks by 3%.",
        @"Reduces the chance your pet will be critically hit by melee attacks by 6%." }, "inv_misc_ahnqirajtrinket_02")]
        public int GraceOfTheMantis { get { return _data[24]; } set { _data[24] = value; } }
        #endregion
        #region Tier 5
        /// <summary>A fierce attack causing (1 + ((RAP * 0.40) * 0.10)) damage, that your pet can use after it makes a critical attack.  Cannot be dodged, blocked or parried.</summary>
        [PetTalentData(25, "Wolverine Bite", 1, new bool[] { true, false, false }, new int[] { 1, 1, 1 }, new int[] { 5, 1, 1 }, new int[] { -1, -1, -1 }, new string[] {
        @"A fierce attack causing (1 + ((RAP * 0.40) * 0.10)) damage, that your pet can use after it makes a critical attack.  Cannot be dodged, blocked or parried." }, "ability_druid_lacerate")]
        public int WolverineBite { get { return _data[25]; } set { _data[25] = value; } }

        /// <summary>Your pet's inspiring roar restores 30 focus over 9 sec.</summary>
        [PetTalentData(26, "Roar Of Recovery", 1, new bool[] { true, false, false }, new int[] { 2, 1, 1 }, new int[] { 5, 1, 1 }, new int[] { -1, -1, -1 }, new string[] {
        @"Your pet's inspiring roar restores 30 focus over 9 sec." }, "ability_druid_mastershapeshifter")]
        public int RoarOfRecovery { get { return _data[26]; } set { _data[26] = value; } }

        /// <summary>Removes all movement impairing effects and all effects which cause loss of control of your pet, and reduces damage done to your pet by 20% for 12 sec.</summary>
        [PetTalentData(27, "Bullheaded", 1, new bool[] { true, false, false }, new int[] { 3, 1, 1 }, new int[] { 5, 1, 1 }, new int[] { 20, -1, -1 }, new string[] {
        @"Removes all movement impairing effects and all effects which cause loss of control of your pet, and reduces damage done to your pet by 20% for 12 sec." }, "ability_warrior_bullrush")]
        public int Bullheaded { get { return _data[27]; } set { _data[27] = value; } }

        /// <summary>Your pet goes into a killing frenzy. Successful attacks have a chance to increase attack power by 5%. This effect will stack up to 5 times. Lasts 20 sec.</summary>
        [PetTalentData(28, "Rabid", 1, new bool[] { false, true, false }, new int[] { 1, 1, 1 }, new int[] { 1, 5, 1 }, new int[] { -1, -1, -1 }, new string[] {
        @"Your pet goes into a killing frenzy. Successful attacks have a chance to increase attack power by 5%. This effect will stack up to 5 times. Lasts 20 sec." }, "ability_druid_berserk")]
        public int Rabid { get { return _data[28]; } set { _data[28] = value; } }

        /// <summary>Your pet heals itself for 100% of its total health over 5 sec while channeling.</summary>
        [PetTalentData(29, "Lick Your Wounds", 1, new bool[] { false, true, false }, new int[] { 1, 2, 1 }, new int[] { 1, 5, 1 }, new int[] { -1, 22, -1 }, new string[] {
        @"Your pet heals itself for 100% of its total health over 5 sec while channeling." }, "ability_hunter_mendpet")]
        public int LickYourWounds { get { return _data[29]; } set { _data[29] = value; } }

        /// <summary>Your pet roars, increasing your pet's and your melee and ranged attack power by 10%. Lasts 20 sec. 5 min cooldown.</summary>
        [PetTalentData(30, "Call Of The Wild", 1, new bool[] { false, true, false }, new int[] { 1, 3, 1 }, new int[] { 1, 5, 1 }, new int[] { -1, 23, -1 }, new string[] {
        @"Your pet roars, increasing your pet's and your melee and ranged attack power by 10%. Lasts 20 sec. 5 min cooldown." }, "ability_druid_kingofthejungle")]
        public int CallOfTheWild { get { return _data[30]; } set { _data[30] = value; } }

        /// <summary>Your pet temporarily gains 30% of it's maximum health for 20 sec. After the effect expires, the health is lost.</summary>
        [PetTalentData(31, "Last Stand", 1, new bool[] { false, false, true }, new int[] { 1, 1, 1 }, new int[] { 1, 1, 5 }, new int[] { -1, -1, -1 }, new string[] {
        @"Your pet temporarily gains 30% of it's maximum health for 20 sec. After the effect expires, the health is lost." }, "spell_nature_shamanrage")]
        public int LastStand { get { return _data[31]; } set { _data[31] = value; } }

        /// <summary>Your pet taunts the target to attack it for 3 sec.</summary>
        [PetTalentData(32, "Taunt", 1, new bool[] { false, false, true }, new int[] { 1, 1, 2 }, new int[] { 1, 1, 5 }, new int[] { -1, -1, 17 }, new string[] {
        @"Your pet taunts the target to attack it for 3 sec." }, "spell_nature_reincarnation")]
        public int Taunt { get { return _data[32]; } set { _data[32] = value; } }

        /// <summary>Protects a friendly target from critical strikes, making attacks against that target unable to be critical strikes, but 20% of all damage taken by that target is also taken by the pet. Lasts 12 sec.</summary>
        [PetTalentData(33, "Roar Of Sacrifice", 1, new bool[] { true, false, true }, new int[] { 4, 1, 3 }, new int[] { 6, 1, 5 }, new int[] { 24, -1, 24 }, new string[] {
        @"Protects a friendly target from critical strikes, making attacks against that target unable to be critical strikes, but 20% of all damage taken by that target is also taken by the pet. Lasts 12 sec." }, "ability_druid_demoralizingroar")]
        public int RoarOfSacrifice { get { return _data[33]; } set { _data[33] = value; } }

        /// <summary>Your pet runs at high speed towards a group member, intercepting the next melee or ranged attack made against them.</summary>
        [PetTalentData(34, "Intervene", 1, new bool[] { false, false, true }, new int[] { 1, 1, 4 }, new int[] { 1, 1, 5 }, new int[] { -1, -1, -1 }, new string[] {
        @"Your pet runs at high speed towards a group member, intercepting the next melee or ranged attack made against them." }, "ability_hunter_pet_turtle")]
        public int Intervene { get { return _data[34]; } set { _data[34] = value; } }
        #endregion
        #region Tier 6
        /// <summary>When your Pet is at or above 50 Focus, your Pet's Basic Attacks will deal [60*Pts]% more damage, but cost [50*Pts]% additional focus.</summary>
        [PetTalentData(35, "Wild Hunt", 2, new bool[] { true, true, true }, new int[] { 1, 3, 3 }, new int[] { 6, 6, 6 }, new int[] { 25, 30, 33 }, new string[] {
        @"When your Pet is at or above 50 Focus, your Pet's Basic Attacks will deal 60% more damage, but cost 50% additional focus.",
        @"When your Pet is at or above 50 Focus, your Pet's Basic Attacks will deal 120% more damage, but cost 100% additional focus." }, "inv_misc_horn_04")]
        public int WildHunt { get { return _data[35]; } set { _data[35] = value; } }

        /// <summary>Your pet does an additional [3*Pts]% damage with all attacks.</summary>
        [PetTalentData(36, "Shark Attack", 2, new bool[] { false, true, false }, new int[] { 1, 1, 1 }, new int[] { 1, 6, 1 }, new int[] { -1, -1, -1 }, new string[] {
        @"Your pet does an additional 3% damage with all attacks.",
        @"Your pet does an additional 6% damage with all attacks." }, "inv_misc_fish_35")]
        public int SharkAttack { get { return _data[36]; } set { _data[36] = value; } }

        /// <summary>Your pet's Growl also heals it for [1*Pts]% of its total health.</summary>
        [PetTalentData(37, "Silverback", 2, new bool[] { false, false, true }, new int[] { 1, 1, 2 }, new int[] { 1, 1, 6 }, new int[] { -1, -1, -1 }, new string[] {
        @"Your pet's Growl also heals it for 1% of its total health.",
        @"Your pet's Growl also heals it for 2% of its total health." }, "ability_hunter_pet_gorilla")]
        public int Silverback { get { return _data[37]; } set { _data[37] = value; } }
        #endregion
        #endregion
    }
}
