using System;
using Rawr.DK;

namespace Rawr.DK
{
    abstract public class Pet
    {
        /// <summary>
        /// The Stats for the Ghoul
        /// </summary>
        StatsDK m_Stats;
        // From DK:
        BossOptions m_BO;
        StatsDK m_DKStats;
        DeathKnightTalents m_Talents;
        Presence m_Presence;

        public Pet(StatsDK dkstats, DeathKnightTalents t, BossOptions bo, Presence p)
        {
            m_BO = bo;
            m_DKStats = dkstats;
            m_Talents = t;
            m_Presence = p;
        }

        virtual public void AccumulateStats()
        {
            m_Stats.Strength = m_DKStats.Strength;
            m_Stats.Stamina = m_DKStats.Stamina;
            m_Stats.HasteRating = m_DKStats.HasteRating;
            m_Stats.CritRating = m_DKStats.CritRating;
            m_Stats.HitRating = m_DKStats.HitRating;

            if (m_Talents.GlyphofRaiseDead)
            {
                m_Stats.Strength += (m_DKStats.Strength * .4f);
                m_Stats.Stamina += (m_DKStats.Stamina * .4f);
            }
            // TODO: Pet Expertise is based on incoming DK hit rating w/o Dranaei aura. 

        }

    }
    /*
    public class Ghoul : Pet
    {

    }

    public class Gargoyle : Pet
    {

    }
    public class Army : Pet
    {

    }
    */
}
