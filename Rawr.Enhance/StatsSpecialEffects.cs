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
                    float chance = 1f;
                    float unhastedAttackSpeed = 3f;
                    switch (effect.Trigger)
                    {
                        case Trigger.DamageDone:
                            trigger = 1f / (cs.GetMeleeAttacksPerSec() + cs.GetSpellAttacksPerSec());
                            chance = (float) Math.Min(1.0f, cs.ChanceMeleeHit + cs.ChanceSpellHit); // limit to 100% chance
                            unhastedAttackSpeed = cs.UnhastedMHSpeed;
                            break;
                        case Trigger.MeleeCrit:
                        case Trigger.PhysicalCrit :
                            trigger = 1f / cs.GetMeleeCritsPerSec();
                            chance = cs.ChanceMeleeCrit;
                            unhastedAttackSpeed = cs.UnhastedMHSpeed;
                            break;
                        case Trigger.MeleeHit:
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
                        case Trigger.ShamanLightningBolt :
                            trigger = 1f / cs.AbilityCooldown("Lightning Bolt");
                            chance = cs.ChanceSpellHit;
                            break;
                        case Trigger.ShamanStormStrike :
                            trigger = 1f / cs.AbilityCooldown("Stormstrike");
                            chance = cs.ChanceYellowHitMH;
                            break;
                        case Trigger.ShamanShock :
                            trigger = 1f / cs.AbilityCooldown("Earth Shock");
                            chance = cs.ChanceSpellHit;
                            break;
                        case Trigger.ShamanLavaLash :
                            trigger = 1f / cs.AbilityCooldown("Lava Lash");
                            chance = cs.ChanceYellowHitOH;
                            break;
                    }
                    if (effect.MaxStack > 1)
                    {
                        float timeToMax = (float)Math.Min(cs.FightLength, effect.GetChance(unhastedAttackSpeed) * trigger * effect.MaxStack);
                        statsAverage += effect.Stats * (effect.MaxStack * ((cs.FightLength - .5f * timeToMax) / cs.FightLength));
                    }
                    else
                    {
                        statsAverage += effect.GetAverageStats(trigger, chance, unhastedAttackSpeed, cs.FightLength);
                    }
                }
            }
            AddParagon(statsAverage);
            AddHighestStat(statsAverage);
            return statsAverage;
        }

        private void AddParagon(Stats statsAverage)
        {
            if (statsAverage.Paragon > 0)
            {
                float paragon = 0f;
                if (_stats.Strength > _stats.Agility)
                {
                    paragon = (float)Math.Floor((float)(statsAverage.Paragon * (1 + _stats.BonusStrengthMultiplier)));
                    statsAverage.Strength += paragon;
                    statsAverage.AttackPower += paragon;
                }
                else
                {
                    paragon = (float)Math.Floor((float)(statsAverage.Paragon * (1 + _stats.BonusAgilityMultiplier)));
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
                        highestStat = (float)Math.Floor((float)(statsAverage.HighestStat * (1 + _stats.BonusAgilityMultiplier)));
                        statsAverage.Agility += highestStat;
                        statsAverage.AttackPower += highestStat;
                    }
                    else
                    {
                        highestStat = (float)Math.Floor((float)(statsAverage.HighestStat * (1 + _stats.BonusIntellectMultiplier)));
                        statsAverage.Intellect += highestStat;
                        statsAverage.AttackPower += intfromAP * highestStat;
                    }
                }
                else
                {
                    if (_stats.Strength > _stats.Intellect)
                    {
                        highestStat = (float)Math.Floor((float)(statsAverage.HighestStat * (1 + _stats.BonusStrengthMultiplier)));
                        statsAverage.Strength += highestStat;
                        statsAverage.AttackPower += highestStat;
                    }
                    else
                    {
                        highestStat = (float)Math.Floor((float)(statsAverage.HighestStat * (1 + _stats.BonusIntellectMultiplier)));
                        statsAverage.Intellect += highestStat;
                        statsAverage.AttackPower += intfromAP * highestStat;
                    }
                }
            }
        }
    }
}
