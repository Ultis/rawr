using System;
using System.Collections.Generic;

namespace Rawr.RestoSham
{
    internal sealed class ReferenceCharacter
    {
        private const float _BaseMana = 23430f;

        private Stats _TotalStats = null;
        private List<Spell> _AvailableSpells = new List<Spell>();
        private CalculationOptionsRestoSham _CalculationOptions = null;
        private CalculationsRestoSham _Calculations = null;
        private float _GcdLatency = 0f;
        private float _Latency = 0f;

        internal ReferenceCharacter(CalculationsRestoSham calculationsObject)
        {
            _Calculations = calculationsObject;
        }

        private void GenerateSpellList(Character character)
        {
            _AvailableSpells.Clear();

            float tankHealingModifier = (_CalculationOptions.EarthShield && _CalculationOptions.Targets == "Tank") ? 0.15f : 0f;

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
                    DurationModifer = (character.ShamanTalents.GlyphofRiptide ? 6f : 0f),
                    CostScale = 1f - character.ShamanTalents.TidalFocus * .02f,
                    EffectModifier = (1.1f + (character.ShamanTalents.SparkOfLife * .02f) + tankHealingModifier),
                    BonusSpellPower = 1 * ((1 + character.ShamanTalents.ElementalWeapons * .2f) * 150f)
                };
                riptide.EffectModifier *= (1 + _TotalStats.RestoSham2T9 * 0.2f);
                _AvailableSpells.Add(riptide);
            }

            ChainHeal chainHeal = new ChainHeal()
            {
                BaseManaCost = (int)(0.17 * _BaseMana),
                ChainedHeal = (_TotalStats.RestoSham4T10 > 0f),
                CostScale = 1f - character.ShamanTalents.TidalFocus * .02f,
                EffectModifier = 1.1f + (character.ShamanTalents.SparkOfLife * .02f) + tankHealingModifier,
                BonusSpellPower = 1 * ((1 + character.ShamanTalents.ElementalWeapons * .2f) * 150f)
            };
            _AvailableSpells.Add(chainHeal);

            HealingSpell healingSurge = new HealingSpell()
            {
                SpellName = "Healing Surge",
                BaseManaCost = (int)(0.27 * _BaseMana),
                BaseCastTime = 1.5f,
                BaseCoefficient = 1.5f / 3.5f,
                BaseEffect = 6004f,
                CostScale = 1f - character.ShamanTalents.TidalFocus * .02f,
                EffectModifier = 1.1f + (character.ShamanTalents.SparkOfLife * .02f) + tankHealingModifier,
                BonusSpellPower = 1 * ((1 + character.ShamanTalents.ElementalWeapons * .2f) * 150f)
            };
            _AvailableSpells.Add(healingSurge);

            HealingSpell healingWave = new HealingSpell()
            {
                SpellName = "Healing Wave",
                BaseManaCost = (int)(0.09 * _BaseMana),
                BaseCastTime = 3f,
                BaseCoefficient = 3f / 3.5f,
                BaseEffect = 3002f,
                CostScale = 1f - character.ShamanTalents.TidalFocus * .02f,
                EffectModifier = 1.1f + (character.ShamanTalents.SparkOfLife * .02f) + tankHealingModifier,
                BonusSpellPower = 1 * ((1 + character.ShamanTalents.ElementalWeapons * .2f) * 150f),
                CastTimeReduction = 0.5f // Purification
            };
            _AvailableSpells.Add(healingWave);

            HealingSpell greaterHw = new HealingSpell()
            {
                SpellName = "Greater Healing Wave",
                BaseManaCost = (int)(0.3 * _BaseMana),
                BaseCastTime = 3f,
                BaseCoefficient = 3f / 3.5f,
                BaseEffect = 8005f,
                CostScale = 1f - character.ShamanTalents.TidalFocus * .02f,
                EffectModifier = 1.1f + (character.ShamanTalents.SparkOfLife * .02f) + tankHealingModifier,
                BonusSpellPower = 1 * ((1 + character.ShamanTalents.ElementalWeapons * .2f) * 150f),
                CastTimeReduction = 0.5f // Purification
            };
            _AvailableSpells.Add(greaterHw);

            if (_CalculationOptions.EarthShield)
            {
                EarthShield es = new EarthShield()
                {
                    BaseManaCost = (int)(0.19 * _BaseMana),
                    CostScale = 1f - character.ShamanTalents.TidalFocus * .02f,
                    EffectModifier = 1.1f + (character.ShamanTalents.SparkOfLife * .02f),
                    BonusSpellPower = 1 * ((1 + character.ShamanTalents.ElementalWeapons * .2f) * 150f)
                };

                if (character.ShamanTalents.GlyphofEarthShield)
                    es.EffectModifier += 0.2f;
                if (character.ShamanTalents.ImprovedShields > 0)
                    es.EffectModifier += character.ShamanTalents.ImprovedShields * 0.05f;

                _AvailableSpells.Add(es);
            }
        }

        internal void FullCalculate(Character character, Item additionalItem)
        {
            // Stats
            _TotalStats = _Calculations.GetCharacterStats(character, additionalItem);

            // Options
            _CalculationOptions = character.CalculationOptions as CalculationOptionsRestoSham;
            
            // Network calcs
            _GcdLatency = _CalculationOptions.Latency / 1000f;
            _Latency = Math.Max(Math.Min(_GcdLatency, 0.275f) - 0.14f, 0f) + Math.Max(_GcdLatency - 0.275f, 0f);

            GenerateSpellList(character);
        }

        internal void IncrementalCalculate(Character character, Item additionalItem)
        {
            _TotalStats = _Calculations.GetCharacterStats(character, additionalItem);
        }

        private bool GetArmorSpecializationStatus(Character character)
        {
            List<CharacterSlot> relevantSlots = new List<CharacterSlot>(8)
            {
                CharacterSlot.Head,
                CharacterSlot.Shoulders,
                CharacterSlot.Chest,
                CharacterSlot.Wrist,
                CharacterSlot.Hands,
                CharacterSlot.Waist,
                CharacterSlot.Legs,
                CharacterSlot.Feet
            };

            foreach (CharacterSlot s in relevantSlots)
            {
                if (character[s] == null)
                    return false;
                if (character[s].Type != ItemType.Mail)
                    return false;
                // if we get to here, the character has a mail item in this slot
            }

            return true;
        }

        internal CharacterCalculationsBase GetCharacterCalculations()
        {
            CharacterCalculationsRestoSham calcs = new CharacterCalculationsRestoSham()
            {
                BasicStats = _TotalStats.Clone(),
                BurstSequence = _CalculationOptions.BurstStyle,
                SustainedSequence = _CalculationOptions.SustStyle
            };

            float criticalScale = 1.5f * (1 + _TotalStats.BonusCritHealMultiplier);
            float hasteScale = 1f / (1f + _TotalStats.SpellHaste);

            calcs.SustainedHPS = 0f;
            calcs.BurstHPS = 0f;
            foreach (Spell spell in _AvailableSpells)
            {
                spell.EffectModifier *= (1 + _TotalStats.BonusHealingDoneMultiplier);
                spell.CriticalScale = criticalScale;
                spell.HasteScale = hasteScale;
                spell.Latency = _Latency;
                spell.GcdLatency = _GcdLatency;
                spell.SpellPower = _TotalStats.SpellPower;
                spell.CritRate = _TotalStats.SpellCrit;
                
                spell.Calculate();
                if (spell is HealingSpell)
                {
                    calcs.SustainedHPS += spell.EPS;
                    calcs.BurstHPS += spell.EPS;
                }

                if (spell is EarthShield)
                    calcs.ESHPS = spell.EPS;
            }

            calcs.Survival = (calcs.BasicStats.Health + calcs.BasicStats.Hp5) * (_CalculationOptions.SurvivalPerc * .01f);
            
            // Set the sub categories
            calcs.SubPoints[0] = calcs.BurstHPS;
            calcs.SubPoints[1] = calcs.SustainedHPS;
            calcs.SubPoints[2] = calcs.Survival;
            
            // Sum up
            calcs.OverallPoints = 0f;
            for (short i = 0; i < calcs.SubPoints.Length; i++)
                calcs.OverallPoints += calcs.SubPoints[i];

            return calcs;
        }
    }
}
