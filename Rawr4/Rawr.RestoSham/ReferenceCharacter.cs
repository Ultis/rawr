using System.Collections.Generic;

namespace Rawr.RestoSham
{
    internal sealed class ReferenceCharacter
    {
        private const float _BaseMana = 23430f;

        private CalculationsRestoSham _Calculations = null;
        private Stats _RaceStats = null;
        private Stats _ItemStats = null;
        private Stats _BuffStats = null;
        private Stats _TotalStats { get { return _RaceStats + _ItemStats + _BuffStats;}}
        private List<Spell> _AvailableSpells = new List<Spell>();

        internal ReferenceCharacter(CalculationsRestoSham calcs, Character character)
        {
            _Calculations = calcs;
            _RaceStats = Rawr.BaseStats.GetBaseStats(character);
        }

        private void GenerateSpellList(Character character)
        {
            _AvailableSpells.Clear();

            if (character.Level >= 83)
            {
                HealingRain healingRain = new HealingRain()
                {
                    BaseManaCost = (int)(0.46 * _BaseMana)
                };
                _AvailableSpells.Add(healingRain);
            }

            if (character.ShamanTalents.Riptide > 0)
            {
                Riptide riptide = new Riptide()
                {
                    BaseManaCost = (int)(0.10 * _BaseMana),
                    HotDuration = 15f + (character.ShamanTalents.GlyphofRiptide ? 6f : 0f)
                };
                _AvailableSpells.Add(riptide);
            }

            ChainHeal chainHeal = new ChainHeal()
            {
                BaseManaCost = (int)(0.17 * _BaseMana),
                ChainedHeal = (_TotalStats.RestoSham4T10 > 0f)
            };
            _AvailableSpells.Add(chainHeal);

            HealingSpell healingSurge = new HealingSpell()
            {
                BaseManaCost = (int)(0.27 * _BaseMana),
                Cooldown = 0f,
                BaseCastTime = 1.5f,
                BaseCoefficient = 1.5f / 3.5f,
            };
            _AvailableSpells.Add(healingSurge);

            HealingSpell healingWave = new HealingSpell()
            {
                BaseManaCost = (int)(0.09 * _BaseMana),
                Cooldown = 0f,
                BaseCastTime = 3f,
                BaseCoefficient = 3f / 3.5f,
            };
            _AvailableSpells.Add(healingWave);

            HealingSpell greaterHw = new HealingSpell()
            {
                BaseManaCost = (int)(0.3 * _BaseMana),
                Cooldown = 0f,
                BaseCastTime = 3f,
                BaseCoefficient = 3f / 3.5f,
            };
            _AvailableSpells.Add(greaterHw);
        }

        internal void FullCalculate(Character character, Item additionalItem)
        {
            _BuffStats = _Calculations.GetBuffsStats(character.ActiveBuffs);
            _ItemStats = _Calculations.GetItemStats(character, additionalItem);
            GenerateSpellList(character);
        }

        internal void IncrementalCalculate(Character character, Item additionalItem)
        {
        }
    }
}
