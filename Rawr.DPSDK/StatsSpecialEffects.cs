using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DPSDK
{
    class StatsSpecialEffects
    {
        private Character _character;
        private static Stats _stats;

        public StatsSpecialEffects(Character character, Stats stats)
        {
            _character = character;
            _stats = stats;
        }

        public static Stats getSpecialEffects(CalculationOptionsDPSDK calcOpts, SpecialEffect effect)
        {
            Stats statsAverage = new Stats();
            Rotation rotation = calcOpts.rotation;
                if (effect.Trigger == Trigger.Use)
                {
                    statsAverage += effect.GetAverageStats();
                }
                else
                {
                    float trigger = 0f;
                    float chance = 0f;
                    float unhastedAttackSpeed = 2f;
                    switch (effect.Trigger)
                    {
                        case Trigger.MeleeCrit:
                        case Trigger.PhysicalCrit:
                            trigger = (1f / rotation.getMeleeSpecialsPerSecond()) + 1/3.5f;
                            chance = 1f;
                            unhastedAttackSpeed = 3.5f;
                            break;
                        case Trigger.MeleeHit:
                        case Trigger.PhysicalHit:
                            trigger = (1f /rotation.getMeleeSpecialsPerSecond()) + 1/3.5f;
                            chance = 1f;
                            unhastedAttackSpeed = 3.5f;
                            break;
                        case Trigger.DamageSpellCast:
                        case Trigger.SpellCast:
                        case Trigger.DamageSpellHit:
                        case Trigger.SpellHit:
                            trigger = 1f /rotation.getSpellSpecialsPerSecond();
                            chance = 1f;
                            break;
                        case Trigger.DamageSpellCrit:
                        case Trigger.SpellCrit:
                            trigger = 1f /rotation.getSpellSpecialsPerSecond();
                            chance = 1f;
                            break;
                        case Trigger.BloodStrikeOrHeartStrikeHit:
                            trigger = (rotation.BloodStrike + rotation.HeartStrike) / rotation.curRotationDuration;
                            chance = 0.15f;
                            break;
                        case Trigger.PlagueStrikeHit:
                            trigger = rotation.PlagueStrike / rotation.curRotationDuration;
                            chance = 1f;
                            break;
                        case Trigger.DoTTick:
                            trigger = (rotation.BloodPlague + rotation.FrostFever) / 3;
                            chance = 1f;
                            break;

                    }
                    if (effect.MaxStack > 1)
                    {
                        float timeToMax = (float)Math.Min(calcOpts.FightLength * 60, effect.GetChance(unhastedAttackSpeed) * trigger * effect.MaxStack);
                        statsAverage += effect.Stats * (effect.MaxStack * (((calcOpts.FightLength * 60) - .5f * timeToMax) / (calcOpts.FightLength * 60)));
                    }
                    else
                    {
                        statsAverage += effect.GetAverageStats(trigger, chance, unhastedAttackSpeed, calcOpts.FightLength * 60);
                    }
                }
            
            return statsAverage;
        }

        public void GreatnessProc()
        {
            //trinket procs
            if (_stats.GreatnessProc > 0)
            {
                float expectedStr = (float)Math.Floor(_stats.Strength * (1 + _stats.BonusStrengthMultiplier));
                _stats.Strength += _stats.GreatnessProc * 15f / 45f;
            }
        }
    }
}
