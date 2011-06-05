using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rawr.Warlock
{
    public abstract class Pet
    {
        protected static float[] WARLOCKSPELLBASEVALUES = { 
            861.855224609375000f, 881.517456054687500f, 901.397033691406250f, 
            921.494018554687500f, 941.806640625000000f, 962.335632324218750f
        };

        public static List<string> ALL_PETS = new List<String>();

        static Pet()
        {
            Type petType = Type.GetType("Rawr.Warlock.Pet");
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (type.IsSubclassOf(petType))
                {
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
        public float ManaPerIntellect { get; protected set; }
        public float BaseMana { get; protected set; }
        public float SpellCritPerIntellect { get; protected set; }
        public float BaseSpellCrit { get; protected set; }

        public float BaseSpellPower { get; protected set; }
        public float SpellPowerCoef { get; protected set; }

        public float BaseAttackPower { get; protected set; }
        public float AttackPowerCoef { get; protected set; }

        public float BaseMeleeDamage { get; protected set; }
        public float DamagePerAttackPower { get; protected set; }

        public float SpecialCooldown { get; protected set; }
        public float SpecialCastTime { get; protected set; }
        public float SpecialBaseDamage { get; protected set; }
        public float SpecialDamagePerSpellPower { get; protected set; }

        public SpellModifiers TotalModifiers { get; protected set; }
        public SpellModifiers MeleeModifiers { get; protected set; }
        public SpellModifiers SpecialModifiers { get; protected set; }

        #endregion
        
        // init
        public Pet(CharacterCalculationsWarlock mommy, float baseHealth, float healthPerStamina)
        {
            Mommy = mommy;
            TotalModifiers = new SpellModifiers();
            MeleeModifiers = new SpellModifiers();
            SpecialModifiers = new SpellModifiers();

            BaseMana = 1343f;
            BaseAttackPower = 886f;

            Stats = new Stats();
        }
        public void CalcStats1()
        {
            WarlockTalents talents = Mommy.Talents;

            //Stats.Strength = 453f;
            //Stats.Agility = 883f;
            Stats.SpellPower = CalcSpellPower();
            Stats.AttackPower = CalcAttackPower();
            Stats.PhysicalCrit = .0328f;
            Stats.Accumulate(Mommy.PetBuffs);

            FinalizeModifiers();
            SpecialModifiers.Accumulate(TotalModifiers);
            MeleeModifiers.Accumulate(TotalModifiers);

            Stats.SpellHaste = GetHasteWithProcs(s => s.SpellHaste);
            Stats.PhysicalHaste = GetHasteWithProcs(s => s.PhysicalHaste);
        }
        private float GetHasteWithProcs(StatExtractor statExtractor)
        {
            Dictionary<int, float> periods = new Dictionary<int, float>();
            Dictionary<int, float> chances = new Dictionary<int, float>();
            periods.Add((int)Trigger.Use, 0f);
            chances.Add((int)Trigger.Use, 1f);
            WeightedStat[] hasteProcs = Mommy.GetUptimes(Stats, periods, chances, statExtractor,
                    (a, b, c, d, e, f, g, h) => SpecialEffect.GetAverageCombinedUptimeCombinationsMultiplicative(a, b, c, d, e, f, g, h));
            float avgProcHaste = 0f;
            foreach (WeightedStat proc in hasteProcs)
            {
                avgProcHaste += proc.Chance * proc.Value;
            }
            return (1f + statExtractor(Stats)) * (1f + avgProcHaste) - 1f;
        }
        public void CalcStats2()
        {
            Stats.SpellPower = CalcSpellPower();
            Stats.AttackPower = CalcAttackPower();
        }
        protected virtual void FinalizeModifiers()
        {
            TotalModifiers.AddMultiplicativeMultiplier(Stats.BonusDamageMultiplier);
            MeleeModifiers.AddMultiplicativeMultiplier(Stats.BonusPhysicalDamageMultiplier);
            if (Mommy.Character.Race == CharacterRace.Orc)
            {
                TotalModifiers.AddMultiplicativeMultiplier(.05f);
            }
        }

        //stats
        public float CalcMana()
        {
            // not clear how accurate this is; factor of 13 was reportedly 7.5 in wrath
            return BaseMana + (Mommy.CalcIntellect() - Mommy.BaseIntellect) * (Mommy.CalcOpts.PlayerLevel / 80) * 13f;
        }
        public float CalcSpellPower()
        {
            return Mommy.CalcSpellPower() * (Mommy.CalcOpts.PlayerLevel / 80) * 0.5f;
        }
        public float CalcSpellCrit()
        {
            return Mommy.CalcSpellCrit();
        }
        public float CalcAttackPower()
        {
            return BaseAttackPower + Stats.Strength * 2f + Mommy.CalcSpellPower() * (Mommy.CalcOpts.PlayerLevel / 80);
        }
        public float CalcMeleeCrit()
        {
            return Mommy.CalcSpellCrit();
        }
                
        //dps
        public float CalcMeleeSpeed()
        {
            return 2f / Mommy.AvgHaste;
        }
        public float CalcMeleeHitChance()
        {
            // the warlock's miss rate, not including buff/debuff
            float miss = .17f - Mommy.CalcSpellHit() + Stats.SpellHit;

            // adjust to melee miss rate
            miss *= 8f / 13f;

            // add in physical hit buff/debuffs
            miss -= Stats.PhysicalHit;

            return Math.Min(1f, 1f - miss);
        }
        public float CalcMeleeDamage()
        {
            return CalcMeleeDamage(true, 0f);
        }
        protected float CalcMeleeDamage(bool canGlance, float bonusDamage)
        {
            int level = Mommy.CalcOpts.TargetLevel;
            int levelDelta = level - Mommy.CalcOpts.PlayerLevel;
            if (levelDelta > 3)
            {
                levelDelta = 3;
            }

            float characterSheetDamage = BaseMeleeDamage + DamagePerAttackPower * CalcAttackPower() + bonusDamage;
            float combatTableModifier = CalcMeleeHitChance() + CalcMeleeCrit() - StatConversion.WHITE_DODGE_CHANCE_CAP[levelDelta];
            if (canGlance)
            {
                combatTableModifier -= .3f * StatConversion.WHITE_GLANCE_CHANCE_CAP[levelDelta];
            }
            float armorModifier = 1f - StatConversion.GetArmorDamageReduction(level, StatConversion.NPC_ARMOR[levelDelta], Stats.TargetArmorReduction, 0f);
            return characterSheetDamage * combatTableModifier * armorModifier * MeleeModifiers.GetFinalDirectMultiplier();
        }
        public float CalcMeleeDps()
        {
            return CalcMeleeDamage() / CalcMeleeSpeed();
        }
        public float GetSpecialSpeed()
        {
            return SpecialCooldown + SpecialCastTime / StatUtils.CalcSpellHaste(Stats, Mommy.CalcOpts.PlayerLevel);
        }
        public virtual float GetSpecialDamage()
        {
            float resist = StatConversion.GetAverageResistance(Mommy.CalcOpts.PlayerLevel, Mommy.CalcOpts.TargetLevel, 0f, 0f);
            float nonCrit = (SpecialBaseDamage + SpecialDamagePerSpellPower * CalcSpellPower()) * SpecialModifiers.GetFinalDirectMultiplier() * (1 - resist);
            float crit = nonCrit * SpecialModifiers.GetFinalCritMultiplier();
            float critChance = CalcSpellCrit();
            return Mommy.HitChance * Utilities.GetWeightedSum(crit, critChance, nonCrit, 1 - critChance);
        }
        public float CalcSpecialDps()
        {
            float speed = GetSpecialSpeed();
            if (speed == 0)
            {
                return 0f;
            }
            else
            {
                return GetSpecialDamage() / GetSpecialSpeed();
            }
        }
        protected float GetEmpowermentCooldown()
        {
            return 60f;
        }
    }

    public class Felguard : Pet
    {
        public Felguard(CharacterCalculationsWarlock mommy)
            : base(
                mommy,
                5395f, // baseHealth,
                11f) // healthPerStamina
        { 
            BaseMeleeDamage = 926.3f;
            DamagePerAttackPower = .187f;
            
            SpecialDamagePerSpellPower = 0.5f * 2f * 0.264f;
            SpecialCooldown = 6f;
        }

        public override float GetSpecialDamage()
        {
            //spellid 30213, effectid 19795, 19796
            return CalcMeleeDamage(false, WARLOCKSPELLBASEVALUES[Mommy.CalcOpts.PlayerLevel - 80] * 0.1439999938f);
        }
        protected override void FinalizeModifiers()
        {
            base.FinalizeModifiers();
            SpecialModifiers.AddMultiplicativeMultiplier(.05f * Mommy.Talents.DarkArts);
            if (Mommy.Talents.GlyphOfFelguard)
            {
                SpecialModifiers.AddMultiplicativeMultiplier(.05f);
            }
        }
    }

    public class Felhunter : Pet
    {
        public Felhunter(CharacterCalculationsWarlock mommy)
            : base(
                mommy,
                5395f, // baseHealth
                9.5f) // healthPerStamina
        { 
            BaseMeleeDamage = 926.3f;
            DamagePerAttackPower = .086786f; //not sure why
            // 19072 base mana

            //spellid 54049, effectid 46748-9
            SpecialBaseDamage = WARLOCKSPELLBASEVALUES[mommy.CalcOpts.PlayerLevel - 80] * 0.126f;
            SpecialDamagePerSpellPower = 1.228f;
            SpecialCooldown = 6f;
        }

        protected override void FinalizeModifiers()
        {
            base.FinalizeModifiers();

            // multipliers go into SpecialModifiers
            SpecialModifiers.AddMultiplicativeMultiplier(Stats.BonusShadowDamageMultiplier);
            if (Mommy.CastSpells.ContainsKey("Corruption"))
            {
                SpecialModifiers.AddAdditiveMultiplier(.3f);
            }
            if (Mommy.CastSpells.ContainsKey("Bane Of Agony"))
            {
                SpecialModifiers.AddAdditiveMultiplier(.3f);
            }
            if (Mommy.CastSpells.ContainsKey("Immolate"))
            {
                SpecialModifiers.AddAdditiveMultiplier(.3f);
            }
            if (Mommy.CastSpells.ContainsKey("Unstable Affliction"))
            {
                SpecialModifiers.AddAdditiveMultiplier(.3f);
            }
            if (Mommy.CastSpells.ContainsKey("Bane Of Doom"))
            {
                SpecialModifiers.AddAdditiveMultiplier(.3f);
            }
            if (Mommy.CastSpells.ContainsKey("Drain Soul"))
            {
                SpecialModifiers.AddAdditiveMultiplier(.3f);
            }
            if (Mommy.CastSpells.ContainsKey("Drain Life"))
            {
                SpecialModifiers.AddAdditiveMultiplier(.3f);
            }
            SpecialModifiers.AddMultiplicativeMultiplier(.05f * Mommy.Talents.DarkArts);
            SpecialModifiers.AddCritBonusMultiplier(1.0f); // 199.5% crit effect
        }
    }

    // TODO: Burning Embers
    // TODO: Empowered Imp
    public class Imp : Pet
    {
        public Imp(CharacterCalculationsWarlock mommy)
            : base(
                mommy,
                5026f, // baseHealth,
                8.4f) // healthPerStamina
        { 
            BaseStamina = 118f;
            BaseIntellect = 368f;
            ManaPerIntellect = 4.95f;
            BaseMana = 1052f;
            BaseSpellCrit = .01f;

            //spellID 3110, effectID 929, 92747
            SpecialBaseDamage = WARLOCKSPELLBASEVALUES[Mommy.CalcOpts.PlayerLevel - 80] * 0.1230000034f;
            SpecialDamagePerSpellPower = .657f * .5f;
            SpecialCastTime = 2.5f - Mommy.Talents.DarkArts * .25f;
        }

        public float GetCritsPerSec()
        {
            return Mommy.HitChance * CalcSpellCrit() / GetSpecialSpeed();
        }

        protected override void FinalizeModifiers()
        {
            base.FinalizeModifiers();

            // multipliers go into SpecialModifiers
            SpecialModifiers.AddMultiplicativeMultiplier(Stats.BonusFireDamageMultiplier);
            if (Mommy.Talents.GlyphOfImp)
            {
                SpecialModifiers.AddMultiplicativeMultiplier(.2f);
            }
            SpecialModifiers.AddMultiplicativeMultiplier(.05f * Mommy.Talents.DarkArts);
        }
    }

    public class Succubus : Pet
    {
        public Succubus(CharacterCalculationsWarlock mommy)
            : base(
                mommy,
                1671f, // baseHealth,
                9f) // healthPerStamina
        { 
            BaseMeleeDamage = (363f + 546f) / 2f;
            DamagePerAttackPower = .157f;

            //spellID 7814, effectID 3106
            SpecialBaseDamage = WARLOCKSPELLBASEVALUES[Mommy.CalcOpts.PlayerLevel - 80] * 0.2010000050f;
            SpecialDamagePerSpellPower = .5f * .612f;
        }

        protected override void FinalizeModifiers()
        {
            base.FinalizeModifiers();

            // multipliers go into SpecialModifiers
            SpecialModifiers.AddMultiplicativeMultiplier(Stats.BonusShadowDamageMultiplier);
            if (Mommy.Talents.GlyphOfLashPain)
            {
                SpecialModifiers.AddMultiplicativeMultiplier(0.25f);
            }
        }
    }

    public class Voidwalker : Pet
    {
        public Voidwalker(CharacterCalculationsWarlock mommy)
            : base(
                mommy,
                1671f, // baseHealth,
                11f) // healthPerStamina
        { 
            BaseMeleeDamage = (297f + 448f) / 2f;
            DamagePerAttackPower = .13f;
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

    // Ebon Imp?
}
