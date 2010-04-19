using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Rawr.Warlock {

    public abstract class Pet {

        public static List<string> ALL_PETS = new List<String>();

        static Pet() {

            Type petType = Type.GetType("Rawr.Warlock.Pet");
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes()) {
                if (type.IsSubclassOf(petType)) {
                    ALL_PETS.Add(type.Name);
                }
            }
        }

        public CharacterCalculationsWarlock Mommy { get; protected set; }

        public float BaseStamina { get; protected set; }
        public float StaminaCoef { get; protected set; }
        public float HealthPerStamina { get; protected set; }
        public float BaseHealth { get; protected set; }

        public float BaseIntellect { get; protected set; }
        public float IntellectCoef { get; protected set; }
        public float ManaPerIntellect { get; protected set; }
        public float BaseMana { get; protected set; }
        public float SpellCritPerIntellect { get; protected set; }
        public float BaseSpellCrit { get; protected set; }

        public Pet(
            CharacterCalculationsWarlock mommy,
            float baseHealth,
            float healthPerStamina) {

            Mommy = mommy;

            BaseStamina = 328f;
            StaminaCoef = .75f;
            HealthPerStamina = healthPerStamina;
            BaseHealth = baseHealth;

            BaseIntellect = 150f;
            IntellectCoef = .3f;
            ManaPerIntellect = 11.55f;
            BaseMana = 1343f;
            SpellCritPerIntellect = .006f;
            BaseSpellCrit = 3.333f;
        }

        public float CalcStamina() {

            return (BaseStamina + StaminaCoef * Mommy.CalcStamina())
                * (1f + Mommy.Talents.FelVitality * .05f);
        }

        public float CalcIntellect() {

            return (BaseIntellect + IntellectCoef * Mommy.CalcIntellect())
                * (1f + Mommy.Talents.FelVitality * .05f);
        }

        public float CalcHealth() {

            return BaseHealth + HealthPerStamina * CalcStamina();
        }
    }

    public class Felguard : Pet {
        public Felguard(CharacterCalculationsWarlock mommy)
            : base(
                mommy,
                1820f, // baseHealth,
                11f) { // healthPerStamina

        }
    }

    public class Felhunter : Pet {

        public Felhunter(CharacterCalculationsWarlock mommy)
            : base(
                mommy,
                1671f, // baseHealth,
                9.5f) { // healthPerStamina,


        }
    }

    public class Imp : Pet {

        public Imp(CharacterCalculationsWarlock mommy)
            : base(
                mommy,
                2877f, // baseHealth,
                8.4f) { // healthPerStamina,

            BaseStamina = 118f;
            BaseIntellect = 368f;
            ManaPerIntellect = 4.95f;
            BaseMana = 1052f;
            BaseSpellCrit = 1f;

            // real firebolt avg hit, naked untalented = 236, NOT whatever the
            // tooltip indicates
        }
    }

    public class Succubus : Pet {
        public Succubus(CharacterCalculationsWarlock mommy)
            : base(
                mommy,
                1671f, // baseHealth,
                9f) { // healthPerStamina,


        }
    }

    public class Voidwalker : Pet {
        public Voidwalker(CharacterCalculationsWarlock mommy)
            : base(
                mommy,
                1671f, // baseHealth,
                11f) { // healthPerStamina,


        }
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
