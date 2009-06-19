using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DPSDK
{
    class StatsSpecialEffects
    {
        public Character character;
        public Stats stats;
        public CombatTable combatTable;

        public StatsSpecialEffects(Character c, Stats s, CombatTable t)
        {
            character = c;
            stats = s;
            combatTable = t;
        }


        public Stats getSpecialEffects(CalculationOptionsDPSDK calcOpts, SpecialEffect effect)
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
                        trigger = (1f / rotation.getMeleeSpecialsPerSecond()) + (combatTable.combinedSwingTime != 0 ? 1f / combatTable.combinedSwingTime : 0.5f);
                        chance = combatTable.physCrits;
                        unhastedAttackSpeed = (combatTable.MH != null ? combatTable.MH.baseSpeed : 2.0f);
                        break;
                    case Trigger.MeleeHit:
                    case Trigger.PhysicalHit:
                        trigger = (1f / rotation.getMeleeSpecialsPerSecond()) + (combatTable.combinedSwingTime != 0 ? 1f / combatTable.combinedSwingTime : 0.5f);
                        chance = 1f - (combatTable.missedSpecial + combatTable.dodgedSpecial);
                        unhastedAttackSpeed = (combatTable.MH != null ? combatTable.MH.baseSpeed : 2.0f);
                        break;
                    case Trigger.DamageSpellCast:
                    case Trigger.SpellCast:
                    case Trigger.DamageSpellHit:
                    case Trigger.SpellHit:
                        trigger = 1f /rotation.getSpellSpecialsPerSecond();
                        chance = 1f - combatTable.spellResist;
                        break;
                    case Trigger.DamageSpellCrit:
                    case Trigger.SpellCrit:
                        trigger = 1f /rotation.getSpellSpecialsPerSecond();
                        chance = combatTable.spellCrits;
                        break;
                    case Trigger.BloodStrikeOrHeartStrikeHit:
                        trigger = (rotation.BloodStrike + rotation.HeartStrike) / rotation.curRotationDuration;
                        chance = 1f;
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
    }
}
