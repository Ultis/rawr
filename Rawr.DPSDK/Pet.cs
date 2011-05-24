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
            m_Stats.DamageTakenReductionMultiplier = 1f - (1f - m_Stats.DamageTakenReductionMultiplier) * (1f - .90f); // 90% passive AOE damage reduction.

            // Apply ratings and such.
            m_Stats.AttackPower = (float)Math.Floor(m_Stats.Strength * 2);
            m_Stats.PhysicalCrit = StatConversion.GetCritFromRating(m_Stats.CritRating);
            m_Stats.SpellCrit = StatConversion.GetCritFromRating(m_Stats.CritRating);

        }

        private int _damage = (546 + 742)/2; // (644)
        /// <summary>
        /// Per Attack Damage
        /// </summary>
        virtual public int WhiteDamage
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

        /// <summary>
        /// Per Attack "Claw" Damage
        /// </summary>
        virtual public int YellowDamage
        {
            get
            {
                return (int)(WhiteDamage * 1.5);
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
                fDodgeChanceForTarget = StatConversion.WHITE_DODGE_CHANCE_CAP[3] - StatConversion.GetDodgeParryReducFromExpertise(m_Stats.Expertise);
                // Determine Parry Chance  (Only for Tank... Since only they should be in front of the target.)
                float fParryChanceForTarget = StatConversion.WHITE_PARRY_CHANCE_CAP[3] - StatConversion.GetDodgeParryReducFromExpertise(m_Stats.Expertise);
                // Determine Miss Chance
                float fMissChance = (StatConversion.WHITE_MISS_CHANCE_CAP[3] - m_Stats.PhysicalHit);
                ChanceToHit -= Math.Max(0, fMissChance);
                ChanceToHit -= Math.Max(0, fDodgeChanceForTarget);
                return ChanceToHit;
            }
        }

        /// <summary>
        /// Damage per hit including Crit, Hit and Glancing chance
        /// </summary>
        virtual public float TotalDamage
        {
            get 
            {
                // Start w/ getting the base damage values.
                int iDamage = WhiteDamage;

                // Factor in max value for Crit, Hit, Glancing
                float misschance = 1 - HitChance;
                iDamage = (int)((float)iDamage * (1 + Math.Min(CritChance, 1 - (misschance))) * Math.Min(1, HitChance) * (.94)/* Glancing */ );
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
        public DarkTranformation(CombatState CS)
        {
            this.szName = "Dark Transformation";
            this.bWeaponRequired = false;
            this.fWeaponDamageModifier = 1;
            this.AbilityCost[(int)DKCostTypes.UnHoly] = 1;
            this.AbilityCost[(int)DKCostTypes.RunicPower] = -10;
            this.bTriggersGCD = true;
            this.AbilityCost[(int)DKCostTypes.CooldownTime] = 3 * 60 * 1000; // 3 min.
            UpdateCombatState(CS);
            this.AbilityIndex = (int)DKability.DarkTransformation;
        }
    }

    public class Ghoul : Pet
    {
        // Naked Forsaken DK Ghoul on Target Dummy stats:
        // Melee hit 546-742 (melee events: 27 over total duration)
        // Melee Crit x1 1152

        // Claw 695 avg (17 events over total duration)

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

    /// <summary>
    /// Summons an Army of 8 Ghouls.  Each hit for a much reduced amount (50%?) compared to an individual ghoul.
    /// melee 141 @ 24674 total 
    /// (69 hits 161-226) (193)
    /// (40 glances)
    /// (14 crits)
    /// 
    /// claw 88 @ 20009 
    /// (69 hits)
    /// (7 crits)
    /// (6 dodged)
    /// (6 missed)
    /// </summary>
    public class Army : Pet
    {
        public Army(StatsDK dkstats, DeathKnightTalents t, BossOptions bo, Presence p)
        {
            m_BO = bo;
            m_DKStats = dkstats;
            m_Talents = t;
            m_Presence = p;

            AccumulateStats();
            DamageType = ItemDamageType.Physical;
        }

        public override int WhiteDamage
        {
            get
            {
                return (int)(base.WhiteDamage * .3f);
            }
            set
            {
                base.WhiteDamage = value;
            }
        }

        public override float AttackSpeed
        {
            get
            {
                // assuming 8 ghouls
                return (base.AttackSpeed / 8);
            }
            set
            {
                base.AttackSpeed = value;
            }
        }
    }

    public class Bloodworm : Pet
    {
        public Bloodworm(StatsDK dkstats, DeathKnightTalents t, BossOptions bo, Presence p)
        {
            m_BO = bo;
            m_DKStats = dkstats;
            m_Talents = t;
            m_Presence = p;

            AccumulateStats();
            DamageType = ItemDamageType.Physical;
        }
    }

    /// <summary>
    /// 40 Sec duration
    /// 3 min cooldown.
    /// </summary>
    public class Gargoyle : Pet
    {
        public Gargoyle(StatsDK dkstats, DeathKnightTalents t, BossOptions bo, Presence p)
        {
            m_BO = bo;
            m_DKStats = dkstats;
            m_Talents = t;
            m_Presence = p;

            AccumulateStats();
            DamageType = ItemDamageType.Physical;
        }
    }
}
