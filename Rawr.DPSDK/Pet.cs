using System;
using Rawr.DK;

namespace Rawr.DK
{
    abstract public class Pet
    {
        /// <summary>
        /// The Stats for the Ghoul
        /// </summary>
        public StatsDK m_Stats;
        // From DK:
        public BossOptions m_BO;
        public StatsDK m_DKStats;
        public DeathKnightTalents m_Talents;
        public Presence m_Presence;

        public Pet()
        {

        }

        public Pet(StatsDK dkstats, DeathKnightTalents t, BossOptions bo, Presence p)
        {
            m_BO = bo;
            m_DKStats = dkstats;
            m_Talents = t;
            m_Presence = p;

            AccumulateStats();
        }

        virtual public void AccumulateStats()
        {
            if (m_Stats == null)
                m_Stats = new StatsDK();

            m_Stats.Strength = m_DKStats.Strength;
            m_Stats.Stamina = m_DKStats.Stamina;
            m_Stats.PhysicalHaste = m_DKStats.PhysicalHaste;
            m_Stats.CritRating = m_DKStats.CritRating;
            m_Stats.PhysicalHit = m_DKStats.PhysicalHit;
            m_Stats.BonusDamageMultiplier = m_DKStats.BonusDamageMultiplier;
            m_Stats.Expertise = StatConversion.GetExpertiseFromDodgeParryReduc(m_DKStats.PhysicalHit);

            if (m_Talents.GlyphofRaiseDead)
            {
                m_Stats.Strength += (m_DKStats.Strength * .4f);
                m_Stats.Stamina += (m_DKStats.Stamina * .4f);
            }
            m_Stats.DamageTakenMultiplier -= .9f; // 90% passive AOE damage reduction.

            // Apply ratings and such.
            m_Stats.AttackPower = (float)Math.Floor(m_Stats.Strength * 2);
            m_Stats.PhysicalCrit = StatConversion.GetCritFromRating(m_Stats.CritRating);
            m_Stats.SpellCrit = StatConversion.GetCritFromRating(m_Stats.CritRating);

        }

        // TODO: Test this in-game.
        private int _damage = 1000;
        /// <summary>
        /// Per Attack Damage
        /// </summary>
        virtual public int Damage
        {
            get
            {
                return (int)(_damage + (m_Stats.AttackPower / 14));
            }
            set
            {
                _damage = value;
            }
        }

        private float _attackspeed = 2f; 
        virtual public float AttackSpeed 
        {
            get
            {
                return _attackspeed / (1 + m_Stats.PhysicalHaste);
            }
            set
            {
                // TODO: we don't want the attack speed to be 0, but definitely not negative.
                _attackspeed = Math.Max(value, 0);
            }
        }

        virtual public ItemDamageType DamageType { get; set; }

        [Percentage]
        virtual public float CritChance
        {
            get
            {
                float crit = .0065f;
                crit += m_Stats.PhysicalCrit;
                crit += StatConversion.NPC_LEVEL_CRIT_MOD[3];
                return Math.Min(1, crit);
            }
        }

        /// <summary>
        /// Chance for the ability to hit the target.  
        /// Includes Expertise
        /// </summary>
        [Percentage]
        virtual public float HitChance
        {
            get
            {
                float ChanceToHit = 1;
                // Determine Dodge chance
                float fDodgeChanceForTarget = 0;
                fDodgeChanceForTarget = StatConversion.YELLOW_DODGE_CHANCE_CAP[3] - StatConversion.GetDodgeParryReducFromExpertise(m_Stats.Expertise);
                // Determine Parry Chance  (Only for Tank... Since only they should be in front of the target.)
                float fParryChanceForTarget = StatConversion.YELLOW_PARRY_CHANCE_CAP[3] - StatConversion.GetDodgeParryReducFromExpertise(m_Stats.Expertise);
                // Determine Miss Chance
                float fMissChance = (StatConversion.YELLOW_MISS_CHANCE_CAP[3] - m_Stats.PhysicalHit);
                ChanceToHit -= Math.Max(0, fMissChance);
                ChanceToHit -= Math.Max(0, fDodgeChanceForTarget);
                return ChanceToHit;
            }
        }

        /// <summary>
        /// Damage per hit including Crit & Hit chance
        /// </summary>
        virtual public float TotalDamage
        {
            get 
            {
                // Start w/ getting the base damage values.
                int iDamage = Damage;

                // Factor in max value for Crit, Hit, Glancing
                float glancechance = .24f;
                float misschance = 1 - HitChance;
                iDamage = (int)((float)iDamage * (1 + Math.Min(CritChance, 1 - (glancechance + misschance))) * Math.Min(1, HitChance) * (.94)/* Glancing */ );
                float dmgTypeModifier = 0f;
                switch (DamageType)
                {
                    case ItemDamageType.Arcane:
                    {
                        dmgTypeModifier += m_Stats.BonusArcaneDamageMultiplier;
                        break;
                    }
                    case ItemDamageType.Fire:
                    {
                        dmgTypeModifier += m_Stats.BonusFireDamageMultiplier;
                        break;
                    }
                    case ItemDamageType.Frost:
                    {
                        dmgTypeModifier += m_Stats.BonusFrostDamageMultiplier;
                        break;
                    }
                    case ItemDamageType.Holy:
                    {
                        dmgTypeModifier += m_Stats.BonusHolyDamageMultiplier;
                        break;
                    }
                    case ItemDamageType.Nature:
                    {
                        dmgTypeModifier += m_Stats.BonusNatureDamageMultiplier;
                        break;
                    }
                    case ItemDamageType.Physical:
                    {
                        dmgTypeModifier += m_Stats.BonusPhysicalDamageMultiplier;
                        break;
                    }
                    case ItemDamageType.Shadow:
                    {
                        dmgTypeModifier += m_Stats.BonusShadowDamageMultiplier;
                        break;
                    }
                }
                return iDamage * (1 + m_Stats.BonusDamageMultiplier) * (1 + dmgTypeModifier); 
            }
        }

        virtual public float DPS 
        {
            get
            {
                return TotalDamage / AttackSpeed;
            } 
        }

    }

    // Putting this here since it has to do specifically with Pets.
    public class DarkTranformation : AbilityDK_Base
    {
        public DarkTranformation()
        {
            this.szName = "Dark Transformation";
            this.bWeaponRequired = false;
            this.fWeaponDamageModifier = 1;
            this.AbilityCost[(int)DKCostTypes.UnHoly] = 1;
            this.AbilityCost[(int)DKCostTypes.RunicPower] = -10;
            this.bTriggersGCD = true;
            this.AbilityCost[(int)DKCostTypes.CooldownTime] = 3 * 60 * 1000; // 3 min.
        }
    }

    public class Ghoul : Pet
    {
        public Ghoul(StatsDK dkstats, DeathKnightTalents t, BossOptions bo, Presence p)
        {
            m_BO = bo;
            m_DKStats = dkstats;
            m_Talents = t;
            m_Presence = p;

            AccumulateStats();
            DamageType = ItemDamageType.Physical;
        }
    }
/*
    public class Gargoyle : Pet
    {

    }
    public class Army : Pet
    {

    }
 */ 
}
