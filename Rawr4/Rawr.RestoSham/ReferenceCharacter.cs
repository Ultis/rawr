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
        private Stats _TotalStats = null;
        private List<Spell> _AvailableSpells = new List<Spell>();

        internal ReferenceCharacter(CalculationsRestoSham calcs, Character character)
        {
            _Calculations = calcs;
            _RaceStats = Rawr.BaseStats.GetBaseStats(character);
        }

        private void GenerateSpellList(Character character)
        {
            _AvailableSpells.Clear();

            HealingRain healingRain = new HealingRain()
            {
                BaseManaCost = (int)(0.46 * _BaseMana),
                CostScale = 1f - character.ShamanTalents.TidalFocus * .02f,
                EffectModifier = 1.1f + (character.ShamanTalents.SparkOfLife * .02f),
                BonusSpellPower = 1 * ((1 + character.ShamanTalents.ElementalWeapons * .2f) * 150f)
            };
            _AvailableSpells.Add(healingRain);

            if (character.ShamanTalents.Riptide > 0)
            {
                Riptide riptide = new Riptide()
                {
                    BaseManaCost = (int)(0.10 * _BaseMana),
                    HotDuration = 15f + (character.ShamanTalents.GlyphofRiptide ? 6f : 0f),
                    CostScale = 1f - character.ShamanTalents.TidalFocus * .02f,
                    EffectModifier = 1.1f + (character.ShamanTalents.SparkOfLife * .02f),
                    BonusSpellPower = 1 * ((1 + character.ShamanTalents.ElementalWeapons * .2f) * 150f)
                };
                _AvailableSpells.Add(riptide);
            }

            ChainHeal chainHeal = new ChainHeal()
            {
                BaseManaCost = (int)(0.17 * _BaseMana),
                ChainedHeal = (_TotalStats.RestoSham4T10 > 0f),
                CostScale = 1f - character.ShamanTalents.TidalFocus * .02f,
                EffectModifier = 1.1f + (character.ShamanTalents.SparkOfLife * .02f),
                BonusSpellPower = 1 * ((1 + character.ShamanTalents.ElementalWeapons * .2f) * 150f)
            };
            _AvailableSpells.Add(chainHeal);

            HealingSpell healingSurge = new HealingSpell()
            {
                SpellName = "Healing Surge",
                BaseManaCost = (int)(0.27 * _BaseMana),
                Cooldown = 0f,
                BaseCastTime = 1.5f,
                BaseCoefficient = 1.5f / 3.5f,
                BaseEffect = 6004,
                CostScale = 1f - character.ShamanTalents.TidalFocus * .02f,
                EffectModifier = 1.1f + (character.ShamanTalents.SparkOfLife * .02f),
                BonusSpellPower = 1 * ((1 + character.ShamanTalents.ElementalWeapons * .2f) * 150f)
            };
            _AvailableSpells.Add(healingSurge);

            HealingSpell healingWave = new HealingSpell()
            {
                SpellName = "Healing Wave",
                BaseManaCost = (int)(0.09 * _BaseMana),
                Cooldown = 0f,
                BaseCastTime = 3f,
                BaseCoefficient = 3f / 3.5f,
                BaseEffect = 3002,
                CostScale = 1f - character.ShamanTalents.TidalFocus * .02f,
                EffectModifier = 1.1f + (character.ShamanTalents.SparkOfLife * .02f),
                BonusSpellPower = 1 * ((1 + character.ShamanTalents.ElementalWeapons * .2f) * 150f),
                CastTimeReduction = 0.5f // Purification
            };
            _AvailableSpells.Add(healingWave);

            HealingSpell greaterHw = new HealingSpell()
            {
                SpellName = "Greater Healing Wave",
                BaseManaCost = (int)(0.3 * _BaseMana),
                Cooldown = 0f,
                BaseCastTime = 3f,
                BaseCoefficient = 3f / 3.5f,
                BaseEffect = 8005,
                CostScale = 1f - character.ShamanTalents.TidalFocus * .02f,
                EffectModifier = 1.1f + (character.ShamanTalents.SparkOfLife * .02f),
                BonusSpellPower = 1 * ((1 + character.ShamanTalents.ElementalWeapons * .2f) * 150f),
                CastTimeReduction = 0.5f // Purification
            };
            _AvailableSpells.Add(greaterHw);
        }

        internal void FullCalculate(Character character, Item additionalItem)
        {
            GetStats(character, additionalItem);
            GenerateSpellList(character);
            CalculateSpells();
        }

        internal void IncrementalCalculate(Character character, Item additionalItem)
        {
            GetStats(character, additionalItem);
            CalculateSpells();
        }

        private void GetStats(Character character, Item additionalItem)
        {
            _BuffStats = _Calculations.GetBuffsStats(character.ActiveBuffs);
            _ItemStats = _Calculations.GetItemStats(character, additionalItem);
            _TotalStats = _RaceStats + _ItemStats + _BuffStats;
            _TotalStats.SpellPower += _TotalStats.Intellect - 10f;
            _TotalStats.SpellHaste = (1 + StatConversion.GetSpellHasteFromRating(_TotalStats.HasteRating)) * (1 + _TotalStats.SpellHaste) - 1;
            _TotalStats.Mp5 += (StatConversion.GetSpiritRegenSec(_TotalStats.Spirit, _TotalStats.Intellect)) * 2.5f;
            _TotalStats.SpellCrit = .022f + StatConversion.GetSpellCritFromIntellect(_TotalStats.Intellect) + StatConversion.GetSpellCritFromRating(_TotalStats.CritRating) + _TotalStats.SpellCrit + (.01f * (character.ShamanTalents.Acuity));
        }

        private void CalculateSpells()
        {
            float criticalScale = 1.5f * (1 + _TotalStats.BonusCritHealMultiplier);
        }
    }
}
