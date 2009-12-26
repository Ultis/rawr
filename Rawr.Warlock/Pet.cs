using System;

namespace Rawr.Warlock
{
    /// <summary>
    /// The base class from which all warlock pets are derived.
    /// </summary>
    /// <remarks>Pet scaling stats taken from SimCraft - should be using http://www.wowwiki.com/Warlock_minions instead.</remarks>
    public abstract class Pet
    {
        public String Name { get; set; }

        protected double BaseStrength;
        protected double BaseAgility;
        protected double BaseStamina;
        protected double BaseIntellect;
        protected double BaseSpirit;
        protected double BaseHealth;
        protected double BaseMana;
        protected double BaseMp5;
        protected double BaseAttackPower;
        protected double BaseAttackCrit;

        protected double HealthPerStamina;
        protected double ManaPerIntellect;
        protected double Mp5PerIntellect;
        protected double InitialAttackPowerPerStrength;
        protected double InitialAttackCritPerAgility;

        protected Pet( String name )
        {
            //initial attribute values for the melee pets [Felguard, Doomguard, Voidwalker & Succubus]
            //these are overridden in the derived caster pet classes [Imp & Felhunter] and also the Infernal (because it has much higher melee base stats)
            BaseStrength = 314;
            BaseAgility = 90;
            BaseStamina = 328;
            BaseIntellect = 150;
            BaseSpirit = 209;

            Name = name;
        }
 
    }

    /// <summary>
    /// Imp class definition
    /// </summary>
    public class Imp : Pet
    {
        public Imp() : base("Imp")
        {
            BaseStrength = 297;
            BaseAgility = 79;
            BaseStamina = 118;
            BaseIntellect = 424;
            BaseSpirit = 367;
            BaseHealth = 4011;
            BaseMana = 1175;
            
            HealthPerStamina = 8.4;
            ManaPerIntellect = 4.9;
            Mp5PerIntellect = 5.0 / 6.0;

            BaseMp5 = -257;
        }
    }

    /// <summary>
    /// Felhunter class definition
    /// </summary>
    public class Felhunter : Pet
    {
        public Felhunter() : base("Felhunter")
        {
            BaseStrength = 314;
            BaseAgility = 90;
            BaseStamina = 328;
            BaseIntellect = 150;
            BaseSpirit = 209;
            BaseHealth = 4788;
            BaseMana = 1559;

            BaseAttackPower = -20;
            InitialAttackPowerPerStrength = 2.0;
            InitialAttackCritPerAgility = 0.01 / 52.0;

            HealthPerStamina = 9.5;
            ManaPerIntellect = 11.5;
            Mp5PerIntellect = 8.0 / 324.0;

            BaseMp5 = 11.22;

            BaseAttackCrit = 0.0327;

            //melee = new warlock_pet_melee_t(this, "felhunter_melee");

        }
    }

    /// <summary>
    /// Felguard class definition
    /// </summary>
    public class Felguard : Pet
    {
        public Felguard() : base("Felguard")
        {
            BaseHealth = 1627;
            BaseMana = 3331;

            BaseAttackPower = -20;

            //melee = new melee_t(this);
        }
    }

    /// <summary>
    /// Succubus class definition
    /// </summary>
    public class Succubus : Pet
    {
        public Succubus() : base("Succubus")
        {
            BaseHealth = 1468;
            BaseMana = 1559;
        }
    }

    /// <summary>
    /// Voidwalker class definition
    /// </summary>
    public class Voidwalker : Pet
    {
        public Voidwalker() : base("Voidwalker")
        {
            throw new System.NotImplementedException();
        }
    }

    /// <summary>
    /// Doomguard class definition
    /// </summary>
    public class Doomguard : Pet
    {
        public Doomguard(): base("Doomguard")
        {
            BaseHealth = 18000;
            BaseMana = 3000;
        }
    }

    /// <summary>
    /// Infernal class definition
    /// </summary>
    public class Infernal : Pet
    {
        public Infernal() : base("Infernal")
        {
            BaseStrength = 331;
            BaseAgility = 113;
            BaseStamina = 361;
            BaseIntellect = 65;
            BaseSpirit = 109;
            BaseHealth = 20300;
            BaseMana = 0;
        }
    }
}
