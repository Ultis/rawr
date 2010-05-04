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

        #region properties

        public CharacterCalculationsWarlock Mommy { get; protected set; }
        public Stats Stats { get; protected set; }

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

        public float BaseSpellPower { get; protected set; }
        public float SpellPowerCoef { get; protected set; }

        public float SpecialCooldown { get; protected set; }
        public float SpecialCastTime { get; protected set; }
        public float SpecialBaseDamage { get; protected set; }
        public SpellModifiers SpecialModifiers { get; protected set; }

        #endregion

        #region init

        public Pet(
            CharacterCalculationsWarlock mommy,
            float baseHealth,
            float healthPerStamina) {

            Mommy = mommy;
            SpecialModifiers = new SpellModifiers();

            BaseStamina = 328f;
            StaminaCoef = .75f;
            HealthPerStamina = healthPerStamina;
            BaseHealth = baseHealth;

            BaseIntellect = 150f;
            IntellectCoef = .3f;
            ManaPerIntellect = 11.55f;
            BaseMana = 1343f;
            BaseSpellCrit = .03333f;

            BaseSpellPower = 0f;
            SpellPowerCoef = .15f;
        }

        public void CalcStats1() {

            WarlockTalents talents = Mommy.Talents;
            float vitality = talents.FelVitality * .05f;
            Stats = new Stats() {
                Stamina = BaseStamina + StaminaCoef * Mommy.CalcStamina(),
                Intellect
                    = BaseIntellect + IntellectCoef * Mommy.CalcIntellect(),
                BonusStaminaMultiplier = vitality,
                BonusIntellectMultiplier = vitality,
                SpellCrit
                    = BaseSpellCrit
                        + .02f * talents.DemonicTactics
                        + .1f
                            * talents.ImprovedDemonicTactics
                            * (Mommy.CalcSpellCrit()
                                - Mommy.Stats.SpellCritOnTarget),
            };
            Stats.Accumulate(Mommy.PetBuffs);
        }

        public void CalcStats2() {

            Stats.SpellPower
                = BaseSpellPower + SpellPowerCoef * Mommy.CalcSpellPower();
            FinalizeModifiers();
        }

        protected virtual void FinalizeModifiers() {

        }

        #endregion

        #region stats

        public float CalcStamina() {

            return StatUtils.CalcStamina(Stats);
        }

        public float CalcIntellect() {

            return StatUtils.CalcIntellect(Stats);
        }

        public float CalcHealth() {

            // sadly, pets do not have the same stamina->health conversion as
            // players, so have to write our own CalcHealth method.
            return (Stats.Health + HealthPerStamina * CalcStamina())
                * (1 + Stats.BonusHealthMultiplier);
        }

        public float CalcSpellPower() {

            return StatUtils.CalcSpellPower(Stats);
        }

        public float CalcSpellCrit() {

            return StatUtils.CalcSpellCrit(Stats);
        }

        #endregion

        #region dps

        public float CalcMeleeDps() {

            return 0f;
        }

        public float GetSpecialSpeed() {

            return SpecialCooldown
                + SpecialCastTime / StatUtils.CalcSpellHaste(Stats);
        }

        public float GetSpecialDamage() {

            float resist
                = StatConversion.GetAverageResistance(
                    80, Mommy.Options.TargetLevel, 0f, 0f);
            float nonCrit
                = (SpecialBaseDamage + CalcSpellPower())
                    * SpecialModifiers.GetFinalDirectMultiplier()
                    * (1 - resist);
            float crit = nonCrit * SpecialModifiers.GetFinalCritMultiplier();
            float critChance = CalcSpellCrit();
            return Mommy.HitChance
                * Utilities.GetWeightedSum(
                    crit, critChance, nonCrit, 1 - critChance);
        }

        public float CalcSpecialDps() {

            return GetSpecialDamage() / GetSpecialSpeed();
        }

        #endregion
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
            BaseSpellCrit = .01f;

            SpecialBaseDamage = (199f + 223f) / 2f;
            SpecialCastTime = 2.5f - Mommy.Talents.DemonicPower * .25f;
            // real firebolt avg hit, naked untalented = 236, NOT whatever the
            // tooltip indicates
        }

        protected override void FinalizeModifiers() {

            WarlockTalents talents = Mommy.Talents;
            float demonologist = talents.MasterDemonologist * .01f;

            BaseSpellCrit += demonologist;

            SpecialModifiers.AddMultiplicativeMultiplier(
                Stats.BonusFireDamageMultiplier);
            SpecialModifiers.AddMultiplicativeMultiplier(
                Stats.BonusDamageMultiplier);
            SpecialModifiers.AddAdditiveMultiplier(.1f * talents.ImprovedImp);
            SpecialModifiers.AddAdditiveMultiplier(.04f * talents.UnholyPower);
            SpecialModifiers.AddAdditiveMultiplier(demonologist);
            SpecialModifiers.AddAdditiveMultiplier(.1f * talents.EmpoweredImp);
            SpecialModifiers.AddCritBonusMultiplier(.2f * talents.Ruin);
            if (talents.GlyphImp) {
                SpecialModifiers.AddAdditiveMultiplier(.2f);
            }
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
