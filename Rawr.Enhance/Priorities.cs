using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Enhance
{
    class Priorities
    {
        private CombatStats _cs;
        private CalculationOptionsEnhance _calcOpts;
        private Character _character;
        private Stats _stats;
        private ShamanTalents _talents;
        private List<Ability> _abilities;
        private float fightLength;
        private float averageFightLength;

        public Priorities(CombatStats cs, CalculationOptionsEnhance calcOpts, Character character, Stats stats, ShamanTalents talents)
        {
            _cs = cs;
            _calcOpts = calcOpts;
            _character = character;
            _stats = stats;
            _talents = talents;
            fightLength = _calcOpts.FightLength * 60f;
            averageFightLength = fightLength;
            _abilities = SetupAbilities();
        }

        private List<Ability> SetupAbilities()
        {
            int priority = 0;
            List<Ability> abilities = new List<Ability>();
            float convection = 1f - _talents.Convection * 0.02f;
            float shockMana = _talents.ShamanisticFocus == 1 ? 0.55f * 0.18f : 0.18f; // 45% reduction if Shamanistic Focus
            float gcd = Math.Max(1.0f, 1.5f * (1f - StatConversion.GetSpellHasteFromRating(_stats.HasteRating)));
            if (_talents.ShamanisticRage == 1)
                abilities.Add(new Ability(EnhanceAbility.ShamanisticRage, 60f, gcd, 0f, ++priority));
            if (_talents.FeralSpirit == 1)
                abilities.Add(new Ability(EnhanceAbility.FeralSpirits, 180f, gcd, 0.12f, ++priority));
            if (_talents.MaelstromWeapon > 0)
                abilities.Add(new Ability(EnhanceAbility.LightningBolt, _cs.SecondsToFiveStack, gcd, 0.1f * convection, ++priority));
            if (_talents.Stormstrike == 1)
                abilities.Add(new Ability(EnhanceAbility.StormStrike, 8f, gcd, 0.08f, ++priority));
            if (_character.ShamanTalents.GlyphofShocking)
                abilities.Add(new Ability(EnhanceAbility.EarthShock, _cs.BaseShockSpeed, 1.0f, shockMana * convection, ++priority));
            else
                abilities.Add(new Ability(EnhanceAbility.EarthShock, _cs.BaseShockSpeed, gcd, shockMana * convection, ++priority));
            if (_talents.LavaLash == 1)
                abilities.Add(new Ability(EnhanceAbility.LavaLash, 6f, gcd, 0.04f, ++priority));
            if (_talents.StaticShock > 0)
                abilities.Add(new Ability(EnhanceAbility.LightningShield, _cs.StaticShockAvDuration, gcd, 0f, ++priority));
            if (_calcOpts.Magma)
                abilities.Add(new Ability(EnhanceAbility.MagmaTotem, 20f, 1.0f, 0.27f, ++priority));
            else
                abilities.Add(new Ability(EnhanceAbility.SearingTotem, 60f, 1.0f, 0.07f, ++priority));
            abilities.Add(new Ability(EnhanceAbility.RefreshTotems, 300f, 1.0f, 0.24f, ++priority)); // patch 3.2 takes just 1 second GCD to refresh totems.
            abilities.Sort();
            return abilities;
        }

        public void CalculateAbilities()
        {
            float gcd = 1.5f;
            string name = "";
            Random random = new Random();
            float totalFightDuration = 0f;
            int totalIterations = 10;
            List<Ability> tempAbilities = new List<Ability>();
        
 //           for (int iteration = 1; iteration <= totalIterations; iteration++)
            {
                float deltaDuration = 1f; // 0.995f + .001f * iteration;  // varies fight duration +/- 0.5%
                float currentFightDuration = fightLength * deltaDuration;
                totalFightDuration += currentFightDuration;
 //               tempAbilities = SetupAbilities();
                for (float timeElapsed = 0f; timeElapsed < currentFightDuration; timeElapsed += gcd)
                {
                    gcd = 0.1f; // set GCD to small value step for dead time as dead time doesn't use a GCD its just waiting time
                    name = "deadtime";
                    float averageLag = _calcOpts.AverageLag;
                    foreach (Ability ability in _abilities)
                    {
                        if (ability.OffCooldown(timeElapsed))
                        {
                            ability.AddUse(timeElapsed, averageLag / 1000f);
                            gcd = ability.GCD;
                            name = ability.Name;
                            break;
                        }
                    }
                    // DebugPrint(abilities, timeElapsed, name);
                }
                // at this stage abilities now contains the number of procs per fight for each ability as a whole number
                // to avoid big stepping problems work out the fraction of the ability use based on how long until next 
                // use beyond fight duration.
                foreach (Ability ability in _abilities)
                    ability.AddRemainder((ability.CooldownOver - currentFightDuration) / ability.Duration);
            }
            // at this stage we have done X iterations with random adjustments to fight duration
            // now the uses are divided by X to average out over the fights
 //           foreach (Ability ability in abilities)
 //               ability.AverageUses(totalIterations);
 //           averageFightLength = totalFightDuration / totalIterrations;
        }

        private void DebugPrint(List<Ability> abilities, float timeElapsed, string name)
        {
            System.Diagnostics.Debug.Print(
                "Time: {0} - FS {1}, {2} - LB {3}, {4} - SS {5}, {6} - ES {7}, {8} - LL {9}, {10} - LS {11}, {12} - MT {13}, {14} - used {15}",
                   timeElapsed,
                   abilities[0].Uses, abilities[0].CooldownOver,
                   abilities[1].Uses, abilities[1].CooldownOver,
                   abilities[2].Uses, abilities[2].CooldownOver,
                   abilities[3].Uses, abilities[3].CooldownOver,
                   abilities[4].Uses, abilities[4].CooldownOver,
                   abilities[5].Uses, abilities[5].CooldownOver,
                   abilities[6].Uses, abilities[6].CooldownOver, name);
        }

        public float AbilityCooldown(EnhanceAbility abilityType)
        {
            foreach (Ability ability in _abilities)
            {
                if (ability.AbilityType == abilityType)
                    return ability.Uses == 0 ? ability.Duration : averageFightLength / ability.Uses;
            }
            return averageFightLength;
        }
    }

    #region Ability class
    public class Ability : IComparable<Ability>
    {
        private EnhanceAbility _abilityType;
        private string _name;
        private float _duration;
        private int _priority;
        private float _cooldownOver;
        private float _uses;
        private float _manacost;
        private float _gcd = 1.5f;
        private int baseMana = 4386;

        public Ability(EnhanceAbility abilityType, float duration, float gcd, float manacost, int priority)
        {
            _abilityType = abilityType;
            _name = abilityType.ToString();
            _duration = duration;
            _priority = priority;
            _manacost = baseMana * manacost; 
            _gcd = gcd;
            //TODO initial cooldown on SR is when you are almost out of mana for now use its duration ie: 60 seconds
            _cooldownOver = abilityType == EnhanceAbility.ShamanisticRage ? duration : 0f; 
            _uses = 0;
        }

        public EnhanceAbility AbilityType { get { return _abilityType; } }
        public string Name { get { return _name; } }
        public float Duration { get { return _duration; } }
        public float GCD { get { return _gcd; } }
        public float CooldownOver { get { return _cooldownOver; } }
        public float Uses { get { return _uses; } }
        public float ManaCost { get { return _manacost; } }

        public void AddUse(float useTime, float lag)
        {
            _uses++;
            _cooldownOver = useTime + _duration + lag;
        }

        public void AddRemainder(float remainder)
        {
            _uses += remainder;
        }

        public void AverageUses(int iterrations)
        {
            _uses /= iterrations;
        }

        public bool OffCooldown(float starttime)
        {
            return starttime >= _cooldownOver;
        }

        public int CompareTo(Ability other)
        {
            return _priority.CompareTo(other._priority);
        }

    }
    #endregion

    public enum EnhanceAbility
    {
        FeralSpirits = 1,
        LightningBolt = 2,
        StormStrike = 3,
        EarthShock = 4,
        FlameShock = 5,
        LavaLash = 6,
        LightningShield = 7,
        MagmaTotem = 8,
        SearingTotem = 9,
        RefreshTotems = 10,
        ShamanisticRage = 11,
    }
}
