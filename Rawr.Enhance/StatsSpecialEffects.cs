using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Enhance
{
    class StatsSpecialEffects
    {
        private Character _character;
        private Stats _stats;

        public StatsSpecialEffects(Character character, Stats stats)
        {
            _character = character;
            _stats = stats;
        }

        public Stats getSpecialEffects()
        {
            Stats statsAverage = new Stats();
            CombatStats cs = new CombatStats(_character, _stats);
            foreach (SpecialEffect effect in _stats.SpecialEffects())
            {
                if (effect.Trigger == Trigger.Use)
                {
                    statsAverage += effect.GetAverageStats();
                }
                else
                {
                    float trigger = 0f;
                    float chance = 0f;
                    float unhastedAttackSpeed = 3f;
                    switch (effect.Trigger)
                    {
                        case Trigger.MeleeCrit :
                        case Trigger.PhysicalCrit :
                            trigger = 1f / cs.GetMeleeCritsPerSec();
                            chance = cs.ChanceWhiteCrit;
                            unhastedAttackSpeed = cs.UnhastedMHSpeed;
                            break;
                        case Trigger.MeleeHit :
                        case Trigger.PhysicalHit :
                            trigger = 1f / cs.GetMeleeAttacksPerSec();
                            chance = cs.ChanceMeleeHit;
                            unhastedAttackSpeed = cs.UnhastedMHSpeed;
                            break;
                        case Trigger.DamageSpellCast :
                        case Trigger.SpellCast :
                            trigger = 1f / cs.GetSpellCastsPerSec();
                            chance = 1f;
                            break;
                        case Trigger.DamageSpellHit :
                        case Trigger.SpellHit :
                            trigger = 1f / cs.GetSpellAttacksPerSec();
                            chance = cs.ChanceSpellHit;
                            break;
                        case Trigger.DamageSpellCrit :
                        case Trigger.SpellCrit :
                            trigger = 1f / cs.GetSpellCritsPerSec();
                            chance = cs.ChanceSpellCrit;
                            break;
                        case Trigger.SpellMiss :
                            trigger = 1f / cs.GetSpellMissesPerSec();
                            chance = 1 - cs.ChanceSpellHit;
                            break;
                    }
                    if (effect.MaxStack > 1)
                    {
                        float timeToMax = (float)Math.Min(cs.FightLength, effect.GetChance(unhastedAttackSpeed) * trigger * effect.MaxStack);
                        statsAverage += effect.Stats * (effect.MaxStack * ((cs.FightLength - .5f * timeToMax) / cs.FightLength));
                    }
                    else
                    {
                        statsAverage += effect.GetAverageStats(trigger, chance, unhastedAttackSpeed);
                    }
                }
            }
            return statsAverage;
        }

        public void GreatnessProc()
        {
            //trinket procs
            if (_stats.GreatnessProc > 0)
            {
                float expectedAgi = (float)Math.Floor(_stats.Agility * (1 + _stats.BonusAgilityMultiplier));
                float expectedStr = (float)Math.Floor(_stats.Strength * (1 + _stats.BonusStrengthMultiplier));
                float expectedInt = (float)Math.Floor(_stats.Intellect * (1 + _stats.BonusIntellectMultiplier));
                float intfromAP = _character.ShamanTalents.MentalDexterity / 3;
                // Highest stat
                if (expectedAgi > expectedStr)
                {
                    if (expectedAgi > expectedInt)
                    {
                        _stats.Agility += _stats.GreatnessProc * 15f / 45f;  
                        _stats.AttackPower += _stats.GreatnessProc * 15f / 45f;
                    }
                    else
                    {
                        _stats.Intellect += _stats.GreatnessProc * 15f / 45f;
                        _stats.AttackPower += intfromAP * _stats.GreatnessProc * 15f / 45f ;
                    }
                }
                else
                {
                    if (expectedStr > expectedInt)
                    {
                        _stats.Strength += _stats.GreatnessProc * 15f / 45f;
                        _stats.AttackPower += _stats.GreatnessProc * 15f / 45f;
                    }
                    else
                    {
                        _stats.Intellect += _stats.GreatnessProc * 15f / 45f;
                        _stats.AttackPower += intfromAP * _stats.GreatnessProc * 15f / 45f;
                    }
                }
            }
        }
    }
}
