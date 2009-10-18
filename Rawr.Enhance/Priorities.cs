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

        public Priorities(CombatStats cs, CalculationOptionsEnhance calcOpts, Character character, Stats stats, ShamanTalents talents)
        {
            _cs = cs;
            _calcOpts = calcOpts;
            _character = character;
            _stats = stats;
            _talents = talents;
            fightLength = _calcOpts.FightLength * 60f;
            _abilities = SetupAbilities();
        }

        private List<Ability> SetupAbilities()
        {
            List<Ability> abilities = new List<Ability>();
            float convection = 1f - _talents.Convection * 0.02f;
            float ESMana = _talents.ShamanisticFocus == 1 ? 0.55f * 0.18f : 0.18f; // 45% reduction if Shamanistic Focus
            float FSMana = _talents.ShamanisticFocus == 1 ? 0.55f * 0.17f : 0.17f; // 45% reduction if Shamanistic Focus
            float gcd = Math.Max(1.0f, 1.5f * (1f - StatConversion.GetSpellHasteFromRating(_stats.HasteRating)));
            int priority = _calcOpts.GetAbilityPriorityValue(EnhanceAbility.ShamanisticRage);
            if (priority > 0)
                if (_talents.ShamanisticRage == 1)
                    abilities.Add(new Ability(EnhanceAbility.ShamanisticRage, 60f, gcd, 0f, priority, true));
            priority = _calcOpts.GetAbilityPriorityValue(EnhanceAbility.FeralSpirits);
            if (priority > 0)
                if (_talents.FeralSpirit == 1)
                    abilities.Add(new Ability(EnhanceAbility.FeralSpirits, 180f, gcd, 0.12f, priority, false));
            priority = _calcOpts.GetAbilityPriorityValue(EnhanceAbility.LightningBolt);
            if (priority > 0)           
                if (_talents.MaelstromWeapon > 0)
                    abilities.Add(new Ability(EnhanceAbility.LightningBolt, _cs.SecondsToFiveStack, gcd, 0.1f * convection, priority, false));
            priority = _calcOpts.GetAbilityPriorityValue(EnhanceAbility.FlameShock);
            if (priority > 0)           
                if (_character.ShamanTalents.GlyphofShocking)
                    abilities.Add(new Ability(EnhanceAbility.FlameShock, 18f, 1.0f, FSMana * convection, priority, false));
                else
                    abilities.Add(new Ability(EnhanceAbility.FlameShock, 18f, gcd, FSMana * convection, priority, false));
            priority = _calcOpts.GetAbilityPriorityValue(EnhanceAbility.StormStrike);
            if (priority > 0)           
                if (_talents.Stormstrike == 1)
                    abilities.Add(new Ability(EnhanceAbility.StormStrike, 8f, gcd, 0.08f, priority, false));
            priority = _calcOpts.GetAbilityPriorityValue(EnhanceAbility.EarthShock);
            if (priority > 0)           
                if (_character.ShamanTalents.GlyphofShocking)
                    abilities.Add(new Ability(EnhanceAbility.EarthShock, _cs.BaseShockSpeed, 1.0f, ESMana * convection, priority, false));
                else
                    abilities.Add(new Ability(EnhanceAbility.EarthShock, _cs.BaseShockSpeed, gcd, ESMana * convection, priority, false));
            priority = _calcOpts.GetAbilityPriorityValue(EnhanceAbility.LavaLash);
            if (priority > 0)           
                if (_talents.LavaLash == 1)
                    abilities.Add(new Ability(EnhanceAbility.LavaLash, 6f, gcd, 0.04f, priority, false));
            priority = _calcOpts.GetAbilityPriorityValue(EnhanceAbility.LightningShield);
            if (priority > 0)           
                if (_talents.StaticShock > 0)
                    abilities.Add(new Ability(EnhanceAbility.LightningShield, _cs.StaticShockAvDuration, gcd, 0f, priority, true));
            priority = _calcOpts.GetAbilityPriorityValue(EnhanceAbility.MagmaTotem);
            if (priority > 0 && _calcOpts.Magma)
                abilities.Add(new Ability(EnhanceAbility.MagmaTotem, 20f, 1.0f, 0.27f, priority, false));
            priority = _calcOpts.GetAbilityPriorityValue(EnhanceAbility.SearingTotem);
            if (priority > 0 && !_calcOpts.Magma)
                abilities.Add(new Ability(EnhanceAbility.SearingTotem, 60f, 1.0f, 0.07f, priority, false));
            priority = _calcOpts.GetAbilityPriorityValue(EnhanceAbility.RefreshTotems);
            if (priority > 0)
                abilities.Add(new Ability(EnhanceAbility.RefreshTotems, 300f, 1.0f, 0.24f, _calcOpts.GetAbilityPriorityValue(EnhanceAbility.ShamanisticRage), true)); // patch 3.2 takes just 1 second GCD to refresh totems.
            abilities.Sort();
            return abilities;
        }

        public void CalculateAbilities()
        {
            float gcd = 1.5f;
            float currentTime = 0f;
            float averageLag = _calcOpts.AverageLag / 1000f;
            float shockOffCooldown = 0f;
            float shockCooldown = _cs.BaseShockSpeed;
            PriorityQueue<Ability> queue = new PriorityQueue<Ability>();
            foreach (Ability ability in _abilities)
                queue.Enqueue(ability);
            while (queue.Count > 0)
            {
                Ability ability = queue.Dequeue();
                if (ability.MissedCooldown(currentTime)) // we missed a cooldown so set new cooldown to current time
                    ability.UpdateCooldown(currentTime);
                else
                {
                    // if we have chosen to wait a fraction of a second for next ability then we need
                    // to ensure that the current time starts when ability is actually off cooldown
                    if (ability.CooldownOver > currentTime)
                        currentTime = ability.CooldownOver;
                    if ((ability.AbilityType == EnhanceAbility.EarthShock || ability.AbilityType == EnhanceAbility.FlameShock) &&
                        currentTime < shockOffCooldown)
                    {
                        // this is a shock and previous shock is still on cooldown
                        // so we update the attempted shock's cooldown to when the shock is next available
                        ability.UpdateCooldown(shockOffCooldown);
                    }
                    else
                    {
                        // all is ok so use the ability
                        if (ability.AbilityType == EnhanceAbility.EarthShock || ability.AbilityType == EnhanceAbility.FlameShock)
                            shockOffCooldown = currentTime + shockCooldown;
                        ability.Use(currentTime); // consider adding human delay factor to time elapsed as to when next comes off CD
                        gcd = ability.GCD;
                        currentTime += gcd + averageLag;
                    }
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
                    return ability.Uses == 0 ? ability.Duration : fightLength / ability.Uses;
            }
            return fightLength;
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
        None = 0,
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

    [Serializable]
    public class Priority
    {
        private EnhanceAbility _abilityType;
        private string _priorityName;
        private string _description;
        private bool _inUse;
        private int _priority;

        public Priority()
        {
            _abilityType = EnhanceAbility.None;
            _priorityName = "None";
            _description = "Empty Priority";
            _inUse = false;
            _priority = 0;
        }

        public Priority(string priorityName, EnhanceAbility abilityType, string description, bool onByDefault, int priority)
        {
            _priorityName = priorityName;
            _abilityType = abilityType;
            _description = description;
            _inUse = onByDefault;
            _priority = priority;
        }

        public string PriorityName { get { return _priorityName; } set { _priorityName = value; } }
        public EnhanceAbility AbilityType { get { return _abilityType; } set { _abilityType = value; } }
        public string Description { get { return _description; } set { _description = value; } }
        public int PriorityValue { get { return _priority; } set { _priority = value; } }
        public bool Checked { get { return _inUse; } set { _inUse = value; } }

        public override string ToString()
        {
            return _priorityName;
        }
    }
}
