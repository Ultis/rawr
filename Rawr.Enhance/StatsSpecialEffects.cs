using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Enhance
{
    class StatsSpecialEffects
    {
        private Character _character;
        private Stats _stats;
        private CombatStats _cs;
        float trigger = 0f;
        float chance = 1f;
        float unhastedAttackSpeed = 3f;

        public StatsSpecialEffects(Character character, Stats stats, CalculationOptionsEnhance calcOpts)
        {
            _character = character;
            _stats = stats;
            _cs = new CombatStats(_character, _stats, calcOpts);
        }

        public Stats getSpecialEffects()
        {
            Stats statsAverage = new Stats();
            foreach (SpecialEffect effect in _stats.SpecialEffects())
            {
                statsAverage.Accumulate(getSpecialEffects(effect));
            }
            AddParagon(statsAverage);
            AddHighestStat(statsAverage);
            return statsAverage;
        }

        public Stats getSpecialEffects(SpecialEffect effect)
        {
            Stats statsAverage = new Stats();
            if (effect.Trigger == Trigger.Use)
            {
                effect.AccumulateAverageStats(statsAverage);
                foreach (SpecialEffect e in effect.Stats.SpecialEffects())
                    statsAverage.Accumulate(this.getSpecialEffects(e) * (effect.Duration / effect.Cooldown));
            }
            else
            {
                SetTriggerChanceAndSpeed(effect);
                foreach (SpecialEffect e in effect.Stats.SpecialEffects())  // deal with secondary effects
                {
                    statsAverage.Accumulate(this.getSpecialEffects(e));
                }
                if (effect.MaxStack > 1)
                {
                    float timeToMax = (float)Math.Min(_cs.FightLength, effect.GetChance(unhastedAttackSpeed) * trigger * effect.MaxStack);
                    float buffDuration = _cs.FightLength;
                    if (effect.Stats.AttackPower == 250f || effect.Stats.AttackPower == 215f || effect.Stats.HasteRating == 57f || effect.Stats.HasteRating == 64f)
                    {
                        buffDuration = 20f;
                    }
                    if (timeToMax * .5f > buffDuration)
                    {
                        timeToMax = 2 * buffDuration;
                    }
                    statsAverage.Accumulate(effect.Stats * (effect.MaxStack * (((buffDuration) - .5f * timeToMax) / (buffDuration))));
                }
                else
                {
                    effect.AccumulateAverageStats(statsAverage, trigger, chance, unhastedAttackSpeed, _cs.FightLength);
                }
            }
            return statsAverage;
        }

        private void SetTriggerChanceAndSpeed(SpecialEffect effect)
        {
            trigger = 0f;
            chance = 1f;
            unhastedAttackSpeed = 3f;
            switch (effect.Trigger)
            {
                case Trigger.DamageDone:
                    trigger = (_cs.HastedMHSpeed + 1f / _cs.GetSpellAttacksPerSec()) / 2f;
                    chance = (float)Math.Min(1.0f, _cs.ChanceMeleeHit + _cs.ChanceSpellHit); // limit to 100% chance
                    unhastedAttackSpeed = _cs.UnhastedMHSpeed;
                    break;
                case Trigger.MeleeCrit:
                case Trigger.PhysicalCrit:
                    trigger = _cs.HastedMHSpeed;
                    chance = _cs.ChanceMeleeCrit;
                    unhastedAttackSpeed = _cs.UnhastedMHSpeed;
                    break;
                case Trigger.MeleeHit:
                case Trigger.PhysicalHit:
                    trigger = _cs.HastedMHSpeed;
                    chance = _cs.ChanceMeleeHit;
                    unhastedAttackSpeed = _cs.UnhastedMHSpeed;
                    break;
                case Trigger.DamageSpellCast:
                case Trigger.SpellCast:
                    trigger = 1f / _cs.GetSpellCastsPerSec();
                    chance = 1f;
                    break;
                case Trigger.DamageSpellHit:
                case Trigger.SpellHit:
                    trigger = 1f / _cs.GetSpellAttacksPerSec();
                    chance = _cs.ChanceSpellHit;
                    break;
                case Trigger.DamageSpellCrit:
                case Trigger.SpellCrit:
                    trigger = 1f / _cs.GetSpellCritsPerSec();
                    chance = _cs.ChanceSpellCrit;
                    break;
                case Trigger.SpellMiss:
                    trigger = 1f / _cs.GetSpellMissesPerSec();
                    chance = 1 - _cs.ChanceSpellHit;
                    break;
                case Trigger.ShamanLightningBolt:
                    trigger = 1f / _cs.AbilityCooldown(EnhanceAbility.LightningBolt);
                    chance = _cs.ChanceSpellHit;
                    break;
                case Trigger.ShamanStormStrike:
                    trigger = 1f / _cs.AbilityCooldown(EnhanceAbility.StormStrike);
                    chance = _cs.ChanceYellowHitMH;
                    break;
                case Trigger.ShamanShock:
                    trigger = 1f / _cs.AbilityCooldown(EnhanceAbility.EarthShock);
                    chance = _cs.ChanceSpellHit;
                    break;
                case Trigger.ShamanLavaLash:
                    trigger = 1f / _cs.AbilityCooldown(EnhanceAbility.LavaLash);
                    chance = _cs.ChanceYellowHitOH;
                    unhastedAttackSpeed = _cs.UnhastedOHSpeed;
                    break;
                case Trigger.ShamanShamanisticRage:
                    trigger = 1f / _cs.AbilityCooldown(EnhanceAbility.ShamanisticRage);
                    chance = 1f;
                    break;
            }
        }

        public float GetUptime(ItemInstance item)
        {
            float uptime = 0;
            if (item != null)
                foreach (SpecialEffect effect in item.GetTotalStats().SpecialEffects())
                {
                    SetTriggerChanceAndSpeed(effect);
                    uptime = effect.GetAverageUptime(trigger, chance, unhastedAttackSpeed, _cs.FightLength);
                }
            return uptime;
        }

        // Handling for Paragon trinket procs
        private void AddParagon(Stats statsAverage)
        {
            if (statsAverage.Paragon > 0)
            {
                float paragon = 0f;
                if (_stats.Strength > _stats.Agility)
                {
                    paragon = statsAverage.Paragon * (1 + _stats.BonusStrengthMultiplier);
                    statsAverage.Strength += paragon;
                    statsAverage.AttackPower += paragon;
                }
                else
                {
                    paragon = statsAverage.Paragon * (1 + _stats.BonusAgilityMultiplier);
                    statsAverage.Agility += paragon;
                    statsAverage.AttackPower += paragon;
                }
            }
        }

        private void AddHighestStat(Stats statsAverage)
        {
            //trinket procs
            if (statsAverage.HighestStat > 0)
            {
                float intfromAP = _character.ShamanTalents.MentalDexterity / 3;
                // Highest stat
                float highestStat = 0f;
                if (_stats.Agility > _stats.Strength)
                {
                    if (_stats.Agility > _stats.Intellect)
                    {
                        highestStat = statsAverage.HighestStat * (1 + _stats.BonusAgilityMultiplier);
                        statsAverage.Agility += highestStat;
                        statsAverage.AttackPower += highestStat;
                    }
                    else
                    {
                        highestStat = statsAverage.HighestStat * (1 + _stats.BonusIntellectMultiplier);
                        statsAverage.Intellect += highestStat;
                        statsAverage.AttackPower += intfromAP * highestStat;
                    }
                }
                else
                {
                    if (_stats.Strength > _stats.Intellect)
                    {
                        highestStat = statsAverage.HighestStat * (1 + _stats.BonusStrengthMultiplier);
                        statsAverage.Strength += highestStat;
                        statsAverage.AttackPower += highestStat;
                    }
                    else
                    {
                        highestStat = statsAverage.HighestStat * (1 + _stats.BonusIntellectMultiplier);
                        statsAverage.Intellect += highestStat;
                        statsAverage.AttackPower += intfromAP * highestStat;
                    }
                }
            }
        }
    }
}
