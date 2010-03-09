using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.WarlockTmp {

    public abstract class Pet {

        public float BaseStrength { get; private set; }
        public float BaseAgility { get; private set; }
        public float BaseStamina { get; private set; }
        public float BaseIntellect { get; private set; }
        public float BaseHealth { get; private set; }
        public float BaseMana { get; private set; }
        public float BaseMp5 { get; private set; }
        public float BaseAttackPower { get; private set; }

        public float HealthPerStamina { get; private set; }
        public float ManaPerIntellect { get; private set; }
        public float Mp5PerIntellect { get; private set; }
        public float AttackPowerPerStrength { get; private set; }

        #region constructor for normal pets
        public Pet(
            float baseHealth,
            float baseMana,
            float baseMp5,
            float baseAttackPower,
            float healthPerStamina,
            float manaPerIntellect,
            float mp5PerIntellect,
            float attackPowerPerStrength)
            : this(
                314f, // strength
                90f, // agility
                328f, // stamina
                150f, // intellect
                baseHealth,
                baseMana,
                baseMp5,
                baseAttackPower,
                healthPerStamina,
                manaPerIntellect,
                mp5PerIntellect,
                attackPowerPerStrength) { }
        #endregion

        #region kitchen sink constructor
        public Pet(
            float baseStrength,
            float baseAgility,
            float baseStamina,
            float baseIntellect,
            float baseHealth,
            float baseMana,
            float baseMp5,
            float baseAttackPower,
            float healthPerStamina,
            float manaPerIntellect,
            float mp5PerIntellect,
            float attackPowerPerStrength) {

            BaseStrength = baseStrength;
            BaseAgility = baseAgility;
            BaseStamina = baseStamina;
            BaseIntellect = baseIntellect;
            BaseHealth = baseHealth;
            BaseMana = baseMana;
            BaseMp5 = baseMp5;
            BaseAttackPower = baseAttackPower;
            HealthPerStamina = healthPerStamina;
            ManaPerIntellect = manaPerIntellect;
            Mp5PerIntellect = mp5PerIntellect;
            attackPowerPerStrength = AttackPowerPerStrength;
        }
        #endregion
    }

    public class Imp : Pet {

        public Imp()
            : base(
                297f, // float baseStrength,
                79f, // float baseAgility,
                118f, // float baseStamina,
                369f, // float baseIntellect,
                2877f, // float baseHealth,
                1082f, // float baseMana,
                0f, // float baseMp5,
                574f, // float baseAttackPower,
                8.4f, // float healthPerStamina,
                5.0f, // float manaPerIntellect,
                .02f, // float mp5PerIntellect,
                0f) { // float attackPowerPerStrength,

            // real firebolt avg hit, naked untalented = 236, NOT whatever the
            // tooltip would indicate
        }
    }

    public class Felhunter : Pet {

        public Felhunter()
            : base(
                314f, // float baseStrength,
                90f, // float baseAgility,
                328f, // float baseStamina,
                150f, // float baseIntellect,
                1672f, // float baseHealth,
                1344f, // float baseMana,
                0f, // float baseMp5,
                574f, // float baseAttackPower,
                9.5f, // float healthPerStamina,
                11.6f, // float manaPerIntellect,
                0f, // float mp5PerIntellect,
                0f) { } // float attackPowerPerStrength,
    }

    //public class Felguard : Pet {
    //    public Felguard()
    //        : base("Felguard") {
    //        BaseHealth = 1627;
    //        BaseMana = 3331;

    //        BaseAttackPower = -20;

    //        //melee = new melee_t(this);
    //    }
    //}

    public class Succubus : Pet {
        public Succubus()
            : base(
                314f, // float baseStrength,
                90f, // float baseAgility,
                328f, // float baseStamina,
                150f, // float baseIntellect,
                1622f, // float baseHealth,
                1344f, // float baseMana,
                4.8f, // float baseMp5,
                574f, // float baseAttackPower,
                9f, // float healthPerStamina,
                11.6f, // float manaPerIntellect,
                .02f, // float mp5PerIntellect,
                0f) { } // float attackPowerPerStrength,
    }

    public class Voidwalker : Pet {
        public Voidwalker()
            : base(
                314f, // float baseStrength,
                90f, // float baseAgility,
                328f, // float baseStamina,
                150f, // float baseIntellect,
                1820f, // float baseHealth,
                1344f, // float baseMana,
                4.8f, // float baseMp5,
                574f, // float baseAttackPower,
                11f, // float healthPerStamina,
                11.6f, // float manaPerIntellect,
                .02f, // float mp5PerIntellect,
                0f) { } // float attackPowerPerStrength,
    }

    //public class Doomguard : Pet {
    //    public Doomguard()
    //        : base("Doomguard") {
    //        BaseHealth = 18000;
    //        BaseMana = 3000;
    //    }
    //}

    //public class Infernal : Pet {
    //    public Infernal()
    //        : base("Infernal") {
    //        BaseStrength = 331;
    //        BaseAgility = 113;
    //        BaseStamina = 361;
    //        BaseIntellect = 65;
    //        BaseSpirit = 109;
    //        BaseHealth = 20300;
    //        BaseMana = 0;
    //    }
    //}
}
