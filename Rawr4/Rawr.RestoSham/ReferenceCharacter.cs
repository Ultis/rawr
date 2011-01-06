using System;
using System.Linq;
using System.Collections.Generic;
using Rawr.Base.Algorithms;

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
        private bool _PerformSequencing = false;

        private double _BurstSequenceDuration = 0d;
        private Dictionary<Spell, double> _BurstSpellSequence = null;
        private double _SustSequenceDuration = 0d;
        private Dictionary<Spell, double> _SustSpellSequence = null;
        private Stats _SequencedStats = null;

        internal ReferenceCharacter(CalculationsRestoSham calculationsObject)
        {
            _Calculations = calculationsObject;
        }

        private void GenerateSpellList(Character character)
        {
            _AvailableSpells.Clear();

            float tankHealingModifier = (_CalculationOptions.EarthShield && _CalculationOptions.Targets == "Tank") ? 0.15f : 0f;
            short orbValue = (short)((1516 * (1 + (character.ShamanTalents.ImprovedShields * .05f))) * (1 + _TotalStats.WaterShieldIncrease));

            if (_CalculationOptions.EarthShield)
            {
                EarthShield es = new EarthShield()
                {
                    BaseManaCost = (int)(0.19f * _BaseMana),
                    CostScale = 1f - character.ShamanTalents.TidalFocus * .02f,
                    EffectModifier = 1.1f + (character.ShamanTalents.SparkOfLife * .02f),
                    BonusSpellPower = 1 * ((1 + character.ShamanTalents.ElementalWeapons * .2f) * 150f),
                    AbsorbOrbChance = 0f
                };

                if (character.ShamanTalents.GlyphofEarthShield)
                    es.EffectModifier += 0.2f;
                if (character.ShamanTalents.ImprovedShields > 0)
                    es.EffectModifier += character.ShamanTalents.ImprovedShields * 0.05f;

                _AvailableSpells.Add(es);
            }

            if (character.ShamanTalents.Riptide > 0)
            {
                Riptide riptide = new Riptide()
                {
                    BaseManaCost = (int)(0.10f * _BaseMana),
                    DurationModifer = (character.ShamanTalents.GlyphofRiptide ? 6f : 0f),
                    CostScale = 1f - character.ShamanTalents.TidalFocus * .02f,
                    EffectModifier = (1.1f + (character.ShamanTalents.SparkOfLife * .02f) + tankHealingModifier),
                    BonusSpellPower = 1 * ((1 + character.ShamanTalents.ElementalWeapons * .2f) * 150f),
                    AbsorbOrbChance = 0.5f * character.ShamanTalents.ImprovedWaterShield,
                    OrbValue = orbValue,
                    ProvidesTidalWaves = (character.ShamanTalents.TidalWaves > 0)
                };
                riptide.EffectModifier *= (1 + _TotalStats.RestoSham2T9 * 0.2f);
                if (_TotalStats.RestoSham2T10 > 0f)
                    riptide.TemporaryBuffs.Add(new TemporaryBuff() { CastCount = 1, Stats = new Stats() { SpellHaste = 0.2f } });
                _AvailableSpells.Add(riptide);
            }

            HealingRain healingRain = new HealingRain()
            {
                BaseManaCost = (int)(0.46f * _BaseMana),
                CostScale = 1f - character.ShamanTalents.TidalFocus * .02f,
                EffectModifier = 1.1f + (character.ShamanTalents.SparkOfLife * .02f),
                AbsorbOrbChance = 0f,
                BonusSpellPower = 1 * ((1 + character.ShamanTalents.ElementalWeapons * .2f) * 150f)
            };
            //_AvailableSpells.Add(healingRain);

            ChainHeal chainHeal = new ChainHeal()
            {
                BaseManaCost = (int)(0.17f * _BaseMana),
                ChainedHeal = (_TotalStats.RestoSham4T10 > 0f),
                CostScale = 1f - character.ShamanTalents.TidalFocus * .02f,
                EffectModifier = 1.1f + (character.ShamanTalents.SparkOfLife * .02f) + tankHealingModifier,
                BonusSpellPower = 1 * ((1 + character.ShamanTalents.ElementalWeapons * .2f) * 150f),
                AbsorbOrbChance = 0.15f * character.ShamanTalents.ImprovedWaterShield,
                OrbValue = orbValue,
                ProvidesTidalWaves = (character.ShamanTalents.TidalWaves > 0)
            };
            _AvailableSpells.Add(chainHeal);

            HealingSpell healingSurge = new HealingSpell()
            {
                SpellId = 8004,
                SpellName = "Healing Surge",
                SpellAbrv = "HS",
                BaseManaCost = (int)(0.27f * _BaseMana),
                BaseCastTime = 1.5f,
                BaseCoefficient = 1.5f / 3.5f,
                BaseEffect = 6004f,
                CostScale = 1f - character.ShamanTalents.TidalFocus * .02f,
                EffectModifier = 1.1f + (character.ShamanTalents.SparkOfLife * .02f) + tankHealingModifier,
                BonusSpellPower = 1 * ((1 + character.ShamanTalents.ElementalWeapons * .2f) * 150f),
                AbsorbOrbChance = 0.3f * character.ShamanTalents.ImprovedWaterShield,
                OrbValue = orbValue,
                ConsumesTidalWaves = true
            };
            if (character.ShamanTalents.TidalWaves > 0)
                healingSurge.TidalWavesBuff = new TemporaryBuff() { Name = "Tidal Waves", Stats = new Stats() { SpellCrit = 0.1f * character.ShamanTalents.TidalWaves } };
            _AvailableSpells.Add(healingSurge);

            HealingSpell healingWave = new HealingSpell()
            {
                SpellId = 331,
                SpellName = "Healing Wave",
                SpellAbrv = "HW",
                BaseManaCost = (int)(0.09f * _BaseMana),
                BaseCastTime = 3f,
                BaseCoefficient = 3f / 3.5f,
                BaseEffect = 3002f,
                CostScale = 1f - character.ShamanTalents.TidalFocus * .02f,
                EffectModifier = 1.1f + (character.ShamanTalents.SparkOfLife * .02f) + tankHealingModifier,
                BonusSpellPower = 1 * ((1 + character.ShamanTalents.ElementalWeapons * .2f) * 150f),
                ConsumesTidalWaves = true,
                AbsorbOrbChance = 0.5f * character.ShamanTalents.ImprovedWaterShield,
                OrbValue = orbValue,
                CastTimeReduction = 0.5f // Purification
            };
            if (character.ShamanTalents.TidalWaves > 0)
                healingWave.TidalWavesBuff = new TemporaryBuff() { Name = "Tidal Waves", Stats = new Stats() { SpellHaste = 0.1f * character.ShamanTalents.TidalWaves } };
            _AvailableSpells.Add(healingWave);

            HealingSpell greaterHw = new HealingSpell()
            {
                SpellId = 77472,
                SpellName = "Greater Healing Wave",
                SpellAbrv = "GHW",
                BaseManaCost = (int)(0.3f * _BaseMana),
                BaseCastTime = 3f,
                BaseCoefficient = 3f / 3.5f,
                BaseEffect = 8005f,
                CostScale = 1f - character.ShamanTalents.TidalFocus * .02f,
                EffectModifier = 1.1f + (character.ShamanTalents.SparkOfLife * .02f) + tankHealingModifier,
                BonusSpellPower = 1 * ((1 + character.ShamanTalents.ElementalWeapons * .2f) * 150f),
                ConsumesTidalWaves = true,
                AbsorbOrbChance = 0.5f * character.ShamanTalents.ImprovedWaterShield,
                OrbValue = orbValue,
                CastTimeReduction = 0.5f // Purification
            };
            if (character.ShamanTalents.TidalWaves > 0)
                greaterHw.TidalWavesBuff = new TemporaryBuff() { Name = "Tidal Waves", Stats = new Stats() { SpellHaste = 0.1f * character.ShamanTalents.TidalWaves } };
            _AvailableSpells.Add(greaterHw);

            if (character.ShamanTalents.TelluricCurrents > 0)
            {
                LightningBolt lb = new LightningBolt()
                {
                    BaseManaCost = (int)(0.06f * _BaseMana),
                    CostScale = 1f - character.ShamanTalents.Convection * .05f,
                    EffectModifier = 1f + (character.ShamanTalents.Concussion * 0.02f) + (character.ShamanTalents.GlyphofLightningBolt ? 0.04f : 0f),
                    TCPercent = character.ShamanTalents.TelluricCurrents * 0.2f
                };
                _AvailableSpells.Add(lb);
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
            _PerformSequencing = true;
        }

        internal void IncrementalCalculate(Character character, Item additionalItem)
        {
            _TotalStats = _Calculations.GetCharacterStats(character, additionalItem);
            _PerformSequencing = false;
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

        internal CharacterCalculationsBase GetCharacterCalculations(Character character)
        {
            CharacterCalculationsRestoSham calcs = new CharacterCalculationsRestoSham()
            {
                BasicStats = _TotalStats.Clone(),
                BurstSequence = _CalculationOptions.BurstStyle,
                SustainedSequence = _CalculationOptions.SustStyle,
                MailSpecialization = (GetArmorSpecializationStatus(character) ? 0.05f : 0f)
            };

            float healingCriticalScale = 1.5f * (1 + _TotalStats.BonusCritHealMultiplier);
            float dmgCriticalScale = 2f * (1 + _TotalStats.BonusCritMultiplier);
            float hasteScale = 1f / (1f + _TotalStats.SpellHaste);

            calcs.SustainedHPS = 0f;
            calcs.BurstHPS = 0f;
            foreach (HealingSpell spell in _AvailableSpells.OfType<HealingSpell>())
            {
                spell.EffectModifier *= (1 + _TotalStats.BonusHealingDoneMultiplier);
                spell.CriticalScale = healingCriticalScale;
                spell.HasteScale = hasteScale;
                spell.Latency = _Latency;
                spell.GcdLatency = _GcdLatency;
                spell.SpellPower = _TotalStats.SpellPower;
                spell.CritRate = _TotalStats.SpellCrit;
                
                spell.Calculate();
                calcs.SustainedHPS += spell.EPS;

                if (spell is EarthShield)
                    calcs.ESHPS = spell.EPS;
            }
            foreach (LightningBolt spell in _AvailableSpells.OfType<LightningBolt>())
            {
                spell.EffectModifier *= (1 + _TotalStats.BonusDamageMultiplier);
                spell.CriticalScale = dmgCriticalScale;
                spell.HasteScale = hasteScale;
                spell.Latency = _Latency;
                spell.GcdLatency = _GcdLatency;
                spell.SpellPower = _TotalStats.SpellPower;
                spell.CritRate = _TotalStats.SpellCrit;
                spell.Calculate();
                calcs.LBRestore = spell.ManaBack - spell.ManaCost;
                calcs.LBCast = spell.CastTime;
            }

            float mp5 = _TotalStats.Mp5 + (_TotalStats.ManaRestoreFromMaxManaPerSecond * _TotalStats.Mana);
            if (_PerformSequencing)
            {
                {
                    StateMachine.StateGenerator gen = new StateMachine.StateGenerator(_AvailableSpells, StateMachine.SequenceType.Burst, _TotalStats.Mana, mp5);
                    List<State<Spell>> stateSpace = gen.GenerateStateSpace();
                    MarkovProcess<Spell> mp = new MarkovProcess<Spell>(stateSpace);
                    _BurstSpellSequence = new Dictionary<Spell, double>(mp.AbilityWeight);
                    _BurstSequenceDuration = mp.AverageTransitionDuration;
                }
                {
                    StateMachine.StateGenerator gen = new StateMachine.StateGenerator(_AvailableSpells, StateMachine.SequenceType.Sustained, _TotalStats.Mana, mp5);
                    List<State<Spell>> stateSpace = gen.GenerateStateSpace();
                    MarkovProcess<Spell> mp = new MarkovProcess<Spell>(stateSpace);
                    _SustSpellSequence = new Dictionary<Spell, double>(mp.AbilityWeight);
                    _SustSequenceDuration = mp.AverageTransitionDuration;
                }

                if (_PerformSequencing)
                    _SequencedStats = _TotalStats.Clone();
            }

            if (_BurstSpellSequence != null)
            {
                double avgHealing = 0d;
                string seq = "";
                string seqShort = "";
                foreach (var item in _BurstSpellSequence)
                {
                    if (item.Key is HealingSpell)
                    {
                        if (seq.Length > 0)
                        {
                            seqShort += ",";
                            seq += ", ";
                        }
                        seqShort += item.Key.SpellAbrv;
                        seq += item.Key.SpellName;
                        avgHealing += item.Key.Effect * item.Value;
                        if (item.Key is Hot)
                            avgHealing += ((Hot)item.Key).TotalHotEffect * item.Value;
                    }
                }
                double hps = avgHealing / _BurstSequenceDuration;
                calcs.BurstHPS = (float)hps;
                calcs.BurstSequence = seq;
                calcs.BurstSequenceShort = seqShort;
            }

            if (_SustSpellSequence != null)
            {
                double avgHealing = 0d;
                string seq = "";
                string seqShort = "";
                foreach (var item in _SustSpellSequence)
                {
                    if (item.Key is HealingSpell)
                    {
                        if (seq.Length > 0)
                        {
                            seqShort += ",";
                            seq += ", ";
                        }
                        seqShort += item.Key.SpellAbrv;
                        seq += item.Key.SpellName;
                        avgHealing += item.Key.Effect * item.Value;
                        if (item.Key is Hot)
                            avgHealing += ((Hot)item.Key).TotalHotEffect * item.Value;
                    }
                }
                double hps = avgHealing / _SustSequenceDuration;
                calcs.SustainedHPS = (float)hps;
                calcs.SustainedSequence = seq;
                calcs.SustainedSequenceShort = seqShort;
            }

            if (calcs.BurstHPS < 0f)
                calcs.BurstHPS = 0f;
            if (calcs.SustainedHPS < 0f)
                calcs.SustainedHPS = 0f;
            if (calcs.Survival < 0f)
                calcs.Survival = 0f;

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
