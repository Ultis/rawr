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
                abilities.Add(new Ability(EnhanceAbility.ShamanisticRage, 60f, gcd, 0f, ++priority, true));
            if (_talents.FeralSpirit == 1)
                abilities.Add(new Ability(EnhanceAbility.FeralSpirits, 180f, gcd, 0.12f, ++priority, false));
            if (_talents.MaelstromWeapon > 0)
                abilities.Add(new Ability(EnhanceAbility.LightningBolt, _cs.SecondsToFiveStack, gcd, 0.1f * convection, ++priority, false));
            if (_talents.Stormstrike == 1)
                abilities.Add(new Ability(EnhanceAbility.StormStrike, 8f, gcd, 0.08f, ++priority, false));
            if (_character.ShamanTalents.GlyphofShocking)
                abilities.Add(new Ability(EnhanceAbility.EarthShock, _cs.BaseShockSpeed, 1.0f, shockMana * convection, ++priority, false));
            else
                abilities.Add(new Ability(EnhanceAbility.EarthShock, _cs.BaseShockSpeed, gcd, shockMana * convection, ++priority, false));
            if (_talents.LavaLash == 1)
                abilities.Add(new Ability(EnhanceAbility.LavaLash, 6f, gcd, 0.04f, ++priority, false));
            if (_talents.StaticShock > 0)
                abilities.Add(new Ability(EnhanceAbility.LightningShield, _cs.StaticShockAvDuration, gcd, 0f, ++priority, true));
            if (_calcOpts.Magma)
                abilities.Add(new Ability(EnhanceAbility.MagmaTotem, 20f, 1.0f, 0.27f, ++priority, false));
            else
                abilities.Add(new Ability(EnhanceAbility.SearingTotem, 60f, 1.0f, 0.07f, ++priority, false));
            abilities.Add(new Ability(EnhanceAbility.RefreshTotems, 300f, 1.0f, 0.24f, ++priority, true)); // patch 3.2 takes just 1 second GCD to refresh totems.
            abilities.Sort();
            return abilities;
        }

        public void CalculateAbilities()
        {
            float gcd = 1.5f;
            float timeElapsed = 0f;
            float averageLag = _calcOpts.AverageLag / 1000f;
            PriorityQueue<Ability> queue = new PriorityQueue<Ability>();
            foreach (Ability ability in _abilities)
                queue.Enqueue(ability);
            while (queue.Count > 0)
            {
                Ability ability = queue.Dequeue();
                if (ability.MissedCooldown(timeElapsed)) // we missed a cooldown so set new cooldown to current time
                    ability.UpdateCooldown(timeElapsed);
                else
                {
                    // if we have chosen to wait a fraction of a second for next ability 
                    // then we need to ensure that the current time starts when ability is 
                    // actually off cooldown
                    if (ability.CooldownOver > timeElapsed)
                        timeElapsed = ability.CooldownOver; 
                    ability.Use(timeElapsed); // consider adding human delay factor to time elapsed as to when next comes off CD
                    gcd = ability.GCD;
                    timeElapsed += gcd + averageLag;
                }
                if (ability.CooldownOver < fightLength)
                {  // adds ability back into queue if its available again before end of fight
                    queue.Enqueue(ability);
                }
                // DebugPrint(_abilities, timeElapsed - gcd - averageLag, name);
            }
            // at this stage abilities now contains the number of procs per fight for each ability as a whole number
            // to avoid big stepping problems work out the fraction of the ability use based on how long until next 
            // use beyond fight duration.
            foreach (Ability ability in _abilities)
            {
                float overrun = ability.Duration - (ability.CooldownOver - fightLength);
                ability.AddUses(overrun / ability.Duration);
            }
            // DebugPrint(_abilities, timeElapsed - gcd - averageLag, "Final uses");
        }
        
        private void DebugPrint(List<Ability> abilities, float timeElapsed, string name)
        {
            if (abilities.Count > 3)
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
        private int baseMana = 4396;
        private float timedrift = 0.1f;
        
        public Ability(EnhanceAbility abilityType, float duration, float gcd, float manacost, int priority, bool useBeforeCombat)
        {
            _abilityType = abilityType;
            _name = abilityType.ToString();
            _duration = duration;
            _priority = priority;
            _manacost = baseMana * manacost; 
            _gcd = gcd;
            //TODO initial cooldown on SR is when you are almost out of mana for now use its duration ie: 60 seconds
            if (useBeforeCombat)  // if ability is to be used before start of combat refresh is after first duration over (eg: totems)
                _cooldownOver = duration;
            else
                _cooldownOver = 0f;
            _uses = 0;
        }

        public EnhanceAbility AbilityType { get { return _abilityType; } }
        public string Name { get { return _name; } }
        public float Duration { get { return _duration; } }
        public float GCD { get { return _gcd; } }
        public float CooldownOver { get { return _cooldownOver; } }
        public float Uses { get { return _uses; } }
        public float ManaCost { get { return _manacost; } }

        public void Use(float useTime)
        {
            _uses++;
            _cooldownOver = useTime + _duration;
        }

        public void AddUses(float uses)
        {
            _uses += uses;
        }

        public void AverageUses(float iterations)
        {
            _uses /= iterations;
        }

        public void UpdateCooldown(float time)
        {
            _cooldownOver = time;
        }

        public bool MissedCooldown(float starttime)
        {
            return _cooldownOver < starttime;
        }

        public bool OffCooldown(float starttime)
        {
            return starttime >= _cooldownOver;
        }

        public int CompareTo(Ability other)
        {
            float diff = _cooldownOver - other._cooldownOver;
            if (diff < -timedrift)  // current ability is off cooldown earlier than other ability
                return -1;
            else if (diff > timedrift) // current ability is off cooldown later than other ability
                return 1;
            else  // off cooldown at same time so use highest priority ability
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
