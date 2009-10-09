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
        private List<Ability> abilities = new List<Ability>();
        private float fightLength;

        public Priorities(CombatStats cs, CalculationOptionsEnhance calcOpts, Character character, Stats stats, ShamanTalents talents)
        {
            _cs = cs;
            _calcOpts = calcOpts;
            _character = character;
            _stats = stats;
            _talents = talents;
            fightLength = _calcOpts.FightLength * 60f; 
            SetupAbilities();
        }

        private void SetupAbilities()
        {
            int priority = 0;
            float gcd = Math.Max(1.0f, 1.5f * (1f - StatConversion.GetSpellHasteFromRating(_stats.HasteRating)));
            if (_talents.FeralSpirit == 1)
                abilities.Add(new Ability("Feral Spirits", 180f, gcd, ++priority));
            if (_talents.MaelstromWeapon > 0)
                abilities.Add(new Ability("Lightning Bolt", _cs.SecondsToFiveStack, gcd, ++priority));
            if (_talents.Stormstrike == 1)
                abilities.Add(new Ability("Stormstrike", 8f, gcd, ++priority));
            if (_character.ShamanTalents.GlyphofShocking)
                abilities.Add(new Ability("Earth Shock", _cs.BaseShockSpeed, 1.0f, ++priority));
            else
                abilities.Add(new Ability("Earth Shock", _cs.BaseShockSpeed, gcd, ++priority));
            if (_talents.LavaLash == 1)
                abilities.Add(new Ability("Lava Lash", 6f, gcd, ++priority));
            if (_talents.StaticShock > 0)
                abilities.Add(new Ability("Lightning Shield", _cs.StaticShockAvDuration, gcd, ++priority));
            if (_calcOpts.Magma)
                abilities.Add(new Ability("Magma Totem", 20f, 1.0f, ++priority));
            else
                abilities.Add(new Ability("Searing Totem", 60f, 1.0f, ++priority));
            abilities.Add(new Ability("Refresh Totems", 300f, 1.0f, ++priority)); // patch 3.2 takes just 1 second GCD to refresh totems.
            abilities.Sort();
        }

        public void CalculateAbilities()
        {
            float gcd = 1.5f;
            string name = "";
            for (float timeElapsed = 0f; timeElapsed < fightLength; timeElapsed += gcd)
            {
                gcd = 0.1f; // set GCD to small value step for dead time as dead time doesn't use a GCD its just waiting time
                name = "deadtime";
                float averageLag = _calcOpts.AverageLag;
                foreach (Ability ability in abilities)
                {
                    if (ability.OffCooldown(timeElapsed))
                    {
                        ability.AddUse(timeElapsed, averageLag / 1000f);
                        gcd = ability.GCD;
                        name = ability.Name;
                        break;
                    }
                }
                // DebugPrint(float timeElapsed, string name);
            }
            // at this stage abilities now contains the number of procs per fight for each ability as a whole number
            // to avoid big stepping problems work out the fraction of the ability use based on how long until next 
            // use beyond fight duration.
            foreach (Ability ability in abilities)
            {
                ability.AddRemainder((ability.CooldownOver - fightLength) / ability.Duration);
            }
        }

        private void DebugPrint(float timeElapsed, string name)
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

        public float AbilityCooldown(string name)
        {
            foreach (Ability ability in abilities)
            {
                if (ability.Name.Equals(name))
                    return ability.Uses == 0 ? ability.Duration : fightLength / ability.Uses;
            }
            return fightLength;
        }


    }

    #region Ability class
    public class Ability : IComparable<Ability>
    {
        private string _name;
        private float _duration;
        private int _priority;
        private float _cooldownOver;
        private float _uses;
        private float _gcd = 1.5f;

        public Ability(string name, float duration, float gcd, int priority)
        {
            _name = name;
            _duration = duration;
            _priority = priority;
            _gcd = gcd;
            _cooldownOver = 0f;
            _uses = 0;
        }

        public string Name { get { return _name; } }
        public float Duration { get { return _duration; } }
        public float GCD { get { return _gcd; } }
        public float CooldownOver { get { return _cooldownOver; } }
        public float Uses { get { return _uses; } }

        public void AddUse(float useTime, float lag)
        {
            _uses++;
            _cooldownOver = useTime + _duration + lag;
        }

        public void AddRemainder(float remainder)
        {
            _uses += remainder;
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
}
