using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DK
{
    /// <summary>
    /// The state of combat at the time an ability is used.
    /// This is a container for all the necessary stats that get passed around
    /// during rotation evaluation.
    /// </summary>
    public class CombatState
    {
        public Character m_Char;
        public StatsDK m_Stats;
        public DeathKnightTalents m_Talents;
        public Weapon MH, OH;
        public float m_NumberOfTargets;
        private uint _uDiseaseCount;
        public uint m_uDiseaseCount
        {
            get
            {
                if (m_Stats.b2T12_Tank) _uDiseaseCount = 2;
                return Math.Min(2, _uDiseaseCount); 
            } 
            set{ _uDiseaseCount = value; }
        }
        public int m_CurrentRP;
        public bool m_bAttackingFromBehind;
        public Rotation.Type m_Spec;
        public Presence m_Presence;
        public float fBossArmor;

        public void ResetCombatState()
        {
            m_uDiseaseCount = 0;
            m_CurrentRP = 0;
        }
    }
}
