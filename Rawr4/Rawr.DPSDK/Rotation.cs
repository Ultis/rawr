using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DK
{
    public class Rotation
    {
        public enum Type
        {
            Custom, Blood, Frost, Unholy, Unknown,
        }

        #region Variables
        public DKCombatTable m_CT;
        /// <summary>
        /// The rotation list of abilities.
        /// </summary>
        public List<AbilityDK_Base> ml_Rot;

        /// <summary>
        /// Set to prioritize threat over DPS.
        /// </summary>
        private bool m_bThreat; 

        public Type curRotationType = Type.Custom;

        /// <summary>
        /// Rotation Duration in Seconds.
        /// </summary>
        public float CurRotationDuration
        {
            get { return (float)m_RotationDuration / 1000f; }
        }

        //disease info
        private double _avgDiseaseMult = 0f;
        public double AvgDiseaseMult
        {
            get { return _avgDiseaseMult; }
            set { _avgDiseaseMult = value; }
        }
        
        private double _numDisease = 0f;
        public double NumDisease
        {
            get { return _numDisease; }
            set { _numDisease = value; }
        }

        private double _diseaseUptime = 0f;
        public double DiseaseUptime
        {
            get { return _diseaseUptime; }
            set { _diseaseUptime = value; }
        }

        private double _gargoyleDuration = 0f;
        public double GargoyleDuration
        {
            get { return _gargoyleDuration; }
            set { _gargoyleDuration = value; }
        }

        private double _KMFS;
        public double KMFS
        {
            get { return _KMFS; }
            set { _KMFS = value; }
        }

        private double _KMRime;
        public double KMRime
        {
            get { return _KMRime; }
            set { _KMRime = value; }
        }

        private double _dancingRuneWeapon = 0f;
        public double DancingRuneWeapon
        {
            get { return _dancingRuneWeapon; }
            set { _dancingRuneWeapon = value; }
        }

        private double _ghoulFrenzy = 0f;
        public double GhoulFrenzy
        {
            get { return _ghoulFrenzy; }
            set { _ghoulFrenzy = value; }
        }

        private Boolean _managedRP = false;
        public Boolean ManagedRP
        {
            get { return _managedRP; }
            set { _managedRP = value; }
        }

        private Boolean _pTRCalcs = false;
        public Boolean PTRCalcs
        {
            get { return _pTRCalcs; }
            set { _pTRCalcs = value; }
        }
        #endregion

        // TODO: Need to figure out a way to build in the TYPEs of rotation.
        // Types currently in theorycrafting looks like:
        // Blood: Diseased or Disease-less (Tanking) So this would be sorted by threat.
        // Frost:
        // Unholy: 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ct">Combat Table</param>
        /// <param name="bThreat">True if you want the results/sorting by Threat (rather Damage)</param>
        public Rotation(DKCombatTable ct, bool bThreat = false)
        {
            m_CT = ct;
            m_bThreat = bThreat;
        }

        /// <summary>
        /// Use the Talent spec to determine which type of rotation we should be looking towards.
        /// </summary>
        /// <param name="t">DeathKnightTalents</param>
        /// <returns>Enum "Type" [Custom, Blood, Frost, Unholy, Unknown]</returns>
        public Type GetRotationType(DeathKnightTalents t)
        {
            curRotationType = Type.Custom;
            const int indexBlood = 0; // start index of Blood Talents.
            const int indexFrost = 28; // start index of Frost Talents.
            const int indexUnholy = indexFrost + 29; // start index of Unholy Talents.
            int[] TalentCounter = new int[4];
            int index = indexBlood;
            foreach (int i in t.Data)
            {
                if (i > 0)
                {
                    // Blood
                    if (index < indexFrost)
                        TalentCounter[(int)Rotation.Type.Blood]++;
                    // Frost
                    else if ((indexFrost <= index) && (index < indexUnholy))
                    {
                        TalentCounter[(int)Rotation.Type.Frost]++;
                    }
                    // Unholy
                    else if (index >= indexUnholy)
                    {
                        TalentCounter[(int)Rotation.Type.Unholy]++;
                    }
                }
                index++;
            }
            if ((TalentCounter[(int)Rotation.Type.Blood] > TalentCounter[(int)Rotation.Type.Frost]) && (TalentCounter[(int)Rotation.Type.Blood] > TalentCounter[(int)Rotation.Type.Unholy]))
            {
                // Blood
                curRotationType = Rotation.Type.Blood;
            }
            else if ((TalentCounter[(int)Rotation.Type.Frost] > TalentCounter[(int)Rotation.Type.Blood]) && (TalentCounter[(int)Rotation.Type.Frost] > TalentCounter[(int)Rotation.Type.Unholy]))
            {
                // Frost
                curRotationType = Rotation.Type.Frost;
            }
            else if ((TalentCounter[(int)Rotation.Type.Unholy] > TalentCounter[(int)Rotation.Type.Frost]) && (TalentCounter[(int)Rotation.Type.Unholy] > TalentCounter[(int)Rotation.Type.Blood]))
            {
                // Unholy
                curRotationType = Rotation.Type.Unholy;
            }
            return curRotationType;
        }

        private float _TotalDamage;
        public float TotalDamage
        {
            get
            {
                return _TotalDamage;
            }
            set
            {
                _TotalDamage = value;
            }
        }
        private float _TotalThreat;
        public float TotalThreat
        {
            get
            {
                return _TotalThreat;
            }
            set
            {
                _TotalThreat = value;
            }
        }

        public float m_TPS // readonly
        {
            get
            {
                float tps = 0;
                if (m_RotationDuration > 0)
                    tps = _TotalThreat / (m_RotationDuration / 1000);
                return tps;
            }
        }
        public float m_DPS // readonly
        {
            get
            {
                float dps = 0;
                if (m_RotationDuration > 0)
                    dps = _TotalDamage / (m_RotationDuration / 1000);
                return dps;
            }
        }

        /// <summary>
        /// Rotation Duration in msec.
        /// </summary>
        public float m_RotationDuration 
        { 
            get
            {
                // Min Duration is total GCDs used so let's start there.
                float dur = GCDTime;
                return Math.Max(Math.Max(dur, m_CastDuration), m_TotalRuneCD);
            }
        }
        public float m_CooldownDuration { get; set; }
        public float m_CastDuration { get; set; }
        public float m_DurationDuration { get; set; }
        public int m_BloodRunes { get; set; }
        public int m_SingleRuneCD { get; set; }
        public int m_FrostRunes { get; set; }
        public int m_UnholyRunes { get; set; }
        public int m_DeathRunes { get; set; }
        public int m_TotalRuneCD { get; set; }
        /// <summary>
        /// Total RP generated/Spent for rotation.
        /// </summary>
        public int m_RunicPower { get; set; }
        public uint m_CountREAbilities
        {
            get
            {
                uint count = 0;
                foreach (AbilityDK_Base ab in ml_Rot)
                {
                    if (ab.AbilityIndex == (int)DKability.RuneStrike
                        || ab.AbilityIndex == (int)DKability.DeathCoil
                        || ab.AbilityIndex == (int)DKability.FrostStrike)
                        ++count;
                }
                return count;
            }
        }
        public uint m_CountDeathStrikes
        {
            get
            {
                uint count = 0;
                foreach (AbilityDK_Base ab in ml_Rot)
                {
                    if (ab.AbilityIndex == (int)DKability.DeathStrike)
                        ++count;
                }
                return count;
            }
        }
        public float m_FreeRunesFromRE
        {
            get
            {
                return ((float)m_CountREAbilities / .45f);
            }
        }
        
        private int _GCDs = 0;
        public int m_GCDs
        {
            get { return _GCDs; }
            set { _GCDs = value; }
        }
        public int GetGCDs
        {
            get { return _GCDs; }
        }
        /// <summary>
        /// Return time spent in GCDs in MS.
        /// </summary>
        public int GCDTime
        {
            get
            {
                if (m_CT.m_Opts.presence == Presence.Unholy)
                    return (int)((float)_GCDs * 1000);
                else
                    return (int)((float)_GCDs * 1500);
            }
        }

        private int _meleeSpecials;
        public int m_MeleeSpecials
        {
            get { return _meleeSpecials; }
            set { _meleeSpecials = value; }
        }
        public float GetMeleeSpecialsPerSecond()
        {
            return m_MeleeSpecials / (m_RotationDuration / 1000);
        }
        private int _spellSpecials;
        public int m_SpellSpecials
        {
            get { return _spellSpecials; }
            set { _spellSpecials = value; }
        }
        public float getMeleeSpecialsPerSecond()
        {
            return m_SpellSpecials / (m_RotationDuration / 1000);
        }
        public float getSpellSpecialsPerSecond()
        {
            return m_MeleeSpecials + m_SpellSpecials / (m_RotationDuration / 1000);
        }

        public void ResetRotation()
        {
            ml_Rot = new List<AbilityDK_Base>();
            m_CT.m_CState.ResetCombatState();
            m_BloodRunes = 0;
            m_UnholyRunes = 0;
            m_FrostRunes = 0;
            m_RunicPower = 0;
            m_CastDuration = 0;
        }

        public void Solver()
        {
            ResetRotation();
            // Setup an instance of each ability.
            // No runes:
            AbilityDK_Outbreak OutB = new AbilityDK_Outbreak(m_CT.m_CState);
            // Single Runes:
            AbilityDK_IcyTouch IT = new AbilityDK_IcyTouch(m_CT.m_CState);
            AbilityDK_FrostFever FF = new AbilityDK_FrostFever(m_CT.m_CState);
            AbilityDK_PlagueStrike PS = new AbilityDK_PlagueStrike(m_CT.m_CState);
            AbilityDK_BloodPlague BP = new AbilityDK_BloodPlague(m_CT.m_CState);
            AbilityDK_BloodStrike BS = new AbilityDK_BloodStrike(m_CT.m_CState);
            AbilityDK_HeartStrike HS = new AbilityDK_HeartStrike(m_CT.m_CState);
            AbilityDK_NecroticStrike NS = new AbilityDK_NecroticStrike(m_CT.m_CState);
            AbilityDK_Pestilence Pest = new AbilityDK_Pestilence(m_CT.m_CState);
            AbilityDK_BloodBoil BB = new AbilityDK_BloodBoil(m_CT.m_CState);
            AbilityDK_HowlingBlast HB = new AbilityDK_HowlingBlast(m_CT.m_CState);
            AbilityDK_ScourgeStrike SS = new AbilityDK_ScourgeStrike(m_CT.m_CState);
            AbilityDK_DeathNDecay DnD = new AbilityDK_DeathNDecay(m_CT.m_CState);
            // Multi Runes:
            AbilityDK_DeathStrike DS = new AbilityDK_DeathStrike(m_CT.m_CState);
            AbilityDK_FesteringStrike Fest = new AbilityDK_FesteringStrike(m_CT.m_CState);
            AbilityDK_Obliterate OB = new AbilityDK_Obliterate(m_CT.m_CState);
            // RP:  Unlikely to start w/ RP abilities to open.
            AbilityDK_RuneStrike RS = new AbilityDK_RuneStrike(m_CT.m_CState);
            AbilityDK_DeathCoil DC = new AbilityDK_DeathCoil(m_CT.m_CState);
            AbilityDK_FrostStrike FS = new AbilityDK_FrostStrike(m_CT.m_CState);

            // Setup our working lists:
            List<AbilityDK_Base> l_Openning = new List<AbilityDK_Base>();

            // For now, let's just blow everything in here one each.
            l_Openning.Add(IT);
            l_Openning.Add(PS);
            l_Openning.Add(BS);
            l_Openning.Add(HS);
            l_Openning.Add(BB);
            l_Openning.Add(NS);
            l_Openning.Add(HB);
            l_Openning.Add(SS);
            l_Openning.Add(DnD);

            l_Openning.Add(DS);
            l_Openning.Add(OB);
            l_Openning.Add(Fest);

            if (m_bThreat)
            {
                l_Openning.Sort(AbilityDK_Base.CompareTPSByRunes);
            }
            else
            {
                l_Openning.Sort(AbilityDK_Base.CompareDPSByRunes);
            }
            // Now we have a list of possible openning events.
            // Sorted by DPS or TPS per rune.  So if any single rune ability will do
            // More for that single rune than a multi-rune ability, we'll use that.
            BuildCosts();
            ReportRotation(l_Openning);
        }

        /// <summary>
        /// This is my fire 1 of everything rotation.
        /// </summary>
        private void OneEachRot()
        {
            ResetRotation();
            // Setup an instance of each ability.
            // No runes:
            AbilityDK_Outbreak OutB = new AbilityDK_Outbreak(m_CT.m_CState);
            // Single Runes:
            AbilityDK_IcyTouch IT = new AbilityDK_IcyTouch(m_CT.m_CState);
            AbilityDK_FrostFever FF = new AbilityDK_FrostFever(m_CT.m_CState);
            AbilityDK_PlagueStrike PS = new AbilityDK_PlagueStrike(m_CT.m_CState);
            AbilityDK_BloodPlague BP = new AbilityDK_BloodPlague(m_CT.m_CState);
            AbilityDK_BloodStrike BS = new AbilityDK_BloodStrike(m_CT.m_CState);
            AbilityDK_HeartStrike HS = new AbilityDK_HeartStrike(m_CT.m_CState);
            AbilityDK_NecroticStrike NS = new AbilityDK_NecroticStrike(m_CT.m_CState);
            AbilityDK_Pestilence Pest = new AbilityDK_Pestilence(m_CT.m_CState);
            AbilityDK_BloodBoil BB = new AbilityDK_BloodBoil(m_CT.m_CState);
            AbilityDK_HowlingBlast HB = new AbilityDK_HowlingBlast(m_CT.m_CState);
            AbilityDK_ScourgeStrike SS = new AbilityDK_ScourgeStrike(m_CT.m_CState);
            AbilityDK_DeathNDecay DnD = new AbilityDK_DeathNDecay(m_CT.m_CState);
            // Multi Runes:
            AbilityDK_DeathStrike DS = new AbilityDK_DeathStrike(m_CT.m_CState);
            AbilityDK_FesteringStrike Fest = new AbilityDK_FesteringStrike(m_CT.m_CState);
            AbilityDK_Obliterate OB = new AbilityDK_Obliterate(m_CT.m_CState);
            // RP:  Unlikely to start w/ RP abilities to open.
            AbilityDK_RuneStrike RS = new AbilityDK_RuneStrike(m_CT.m_CState);
            AbilityDK_DeathCoil DC = new AbilityDK_DeathCoil(m_CT.m_CState);
            AbilityDK_FrostStrike FS = new AbilityDK_FrostStrike(m_CT.m_CState);

            // For now, let's just blow everything in here one each.
            ml_Rot.Add(IT);
            ml_Rot.Add(FF);
            ml_Rot.Add(PS);
            ml_Rot.Add(BP);
            ml_Rot.Add(BS);
            ml_Rot.Add(HS);
            ml_Rot.Add(BB);
            ml_Rot.Add(NS);
            ml_Rot.Add(HB);
            ml_Rot.Add(SS);
            ml_Rot.Add(DnD);

            ml_Rot.Add(DS);
            ml_Rot.Add(OB);
            ml_Rot.Add(Fest);

            ml_Rot.Add(RS);
            ml_Rot.Add(DC);
            ml_Rot.Add(FS);

            if (m_bThreat)
            {
                ml_Rot.Sort(AbilityDK_Base.CompareByTotalThreat);
            }
            else
            {
                ml_Rot.Sort(AbilityDK_Base.CompareByTotalDamage);
            }

            BuildCosts();
        }

        /// <summary>
        /// This a basic HSx2 (or BBx2), DSx2, RSx? rotation.
        /// </summary>
        public void DiseaselessBlood()
        {
            ResetRotation();
            // This will only happen while tanking...
            m_CT.m_Opts.presence = Presence.Blood;
            // Setup an instance of each ability.
            // No runes:
//            AbilityDK_Outbreak OutB = new AbilityDK_Outbreak(m_CT.m_CState);
            // Single Runes:
//            AbilityDK_IcyTouch IT = new AbilityDK_IcyTouch(m_CT.m_CState);
//            AbilityDK_FrostFever FF = new AbilityDK_FrostFever(m_CT.m_CState);
//            AbilityDK_PlagueStrike PS = new AbilityDK_PlagueStrike(m_CT.m_CState);
//            AbilityDK_BloodPlague BP = new AbilityDK_BloodPlague(m_CT.m_CState);
//            AbilityDK_BloodStrike BS = new AbilityDK_BloodStrike(m_CT.m_CState);
            AbilityDK_HeartStrike HS = new AbilityDK_HeartStrike(m_CT.m_CState);
//            AbilityDK_NecroticStrike NS = new AbilityDK_NecroticStrike(m_CT.m_CState);
//            AbilityDK_Pestilence Pest = new AbilityDK_Pestilence(m_CT.m_CState);
            AbilityDK_BloodBoil BB = new AbilityDK_BloodBoil(m_CT.m_CState);
//            AbilityDK_HowlingBlast HB = new AbilityDK_HowlingBlast(m_CT.m_CState);
//            AbilityDK_ScourgeStrike SS = new AbilityDK_ScourgeStrike(m_CT.m_CState);
//            AbilityDK_DeathNDecay DnD = new AbilityDK_DeathNDecay(m_CT.m_CState);
            // Multi Runes:
            AbilityDK_DeathStrike DS = new AbilityDK_DeathStrike(m_CT.m_CState);
//            AbilityDK_FesteringStrike Fest = new AbilityDK_FesteringStrike(m_CT.m_CState);
//            AbilityDK_Obliterate OB = new AbilityDK_Obliterate(m_CT.m_CState);
            // RP:  Unlikely to start w/ RP abilities to open.
            AbilityDK_RuneStrike RS = new AbilityDK_RuneStrike(m_CT.m_CState);
            // each RS gives a 45% chance to renew a fully depleted runes.
            // thus 20 RSs gets us 9 extra runes.
            // Same is true for DC & FS
//            AbilityDK_DeathCoil DC = new AbilityDK_DeathCoil(m_CT.m_CState);
//            AbilityDK_FrostStrike FS = new AbilityDK_FrostStrike(m_CT.m_CState);

            // Simple HSx2, DSx2, RS w/ RP.
            if (HS.TotalThreat > BB.TotalThreat)
            {
                ml_Rot.Add(HS);
                ml_Rot.Add(HS);
            }
            else
            {
                ml_Rot.Add(BB);
                ml_Rot.Add(BB);
            }

            // These will create DeathRunes that can allow flexibility in the rotation later on.
            ml_Rot.Add(DS);
            ml_Rot.Add(DS);

            // How much RP do we have at this point?
            foreach (AbilityDK_Base ab in ml_Rot)
                m_RunicPower += ab.RunicPower;
            m_RunicPower *= (int)((float)m_RunicPower * (1 + m_CT.m_CState.m_Stats.BonusRPMultiplier));

            // Burn what we can.
            for (int RSCount = Math.Abs(m_RunicPower / RS.RunicPower); RSCount > 0; RSCount--)
            {
                ml_Rot.Add(RS);
                m_RunicPower += RS.RunicPower;
            }

            BuildCosts();
        }

        public void BuildCosts()
        {
            // Now we have the list of abilities sorted appropriately.
            foreach (AbilityDK_Base ability in ml_Rot)
            {
                // Populate the costs here.
                //m_CooldownDuration += ability.Cooldown; // CDs will overlap.
                m_CastDuration += ability.CastTime;
                //                m_DurationDuration // Durations will overlap.
                m_BloodRunes += ability.AbilityCost[(int)DKCostTypes.Blood];
                m_FrostRunes += ability.AbilityCost[(int)DKCostTypes.Frost];
                m_UnholyRunes += ability.AbilityCost[(int)DKCostTypes.UnHoly];
                m_DeathRunes += ability.AbilityCost[(int)DKCostTypes.Death];
                // spend the death runes available:
                int[] abCost = new int[EnumHelper.GetCount(typeof(DKCostTypes))];
                abCost[(int)DKCostTypes.Blood] = m_BloodRunes;
                abCost[(int)DKCostTypes.Frost] = m_FrostRunes;
                abCost[(int)DKCostTypes.UnHoly] = m_UnholyRunes;
                abCost[(int)DKCostTypes.Death] = m_DeathRunes;
                int hiRuneIndex = DKCombatTable.GetHighestRuneCountIndex(abCost);
                DKCombatTable.SpendDeathRunes(abCost, 0);

                m_SingleRuneCD = (int)(5 * (1000 / (1f + m_CT.m_CState.m_Stats.PhysicalHaste + m_CT.m_CState.m_Stats.BonusRuneRegeneration))); // CD in MSec, modified by haste
                int totalRuneCount = m_BloodRunes;
                totalRuneCount += m_FrostRunes;
                totalRuneCount += m_UnholyRunes;

                //TODO: What about multi-rune abilities?
                m_TotalRuneCD = abCost[hiRuneIndex] * m_SingleRuneCD + (totalRuneCount / 3) * GCDTime;

                if (ability.bTriggersGCD)
                    m_GCDs++;
                TotalDamage += ability.GetTotalDamage();
                TotalThreat += ability.GetTotalThreat();
                if (ability.uRange == AbilityDK_Base.MELEE_RANGE)
                {
                    m_MeleeSpecials++;
                }
                else
                {
                    m_SpellSpecials++;
                }
                
            }
        }

        public string ReportRotation(List<AbilityDK_Base> l_Openning)
        {
            string szReport = "";
            float DurationDuration = 0;

            szReport += string.Format("{0,-20}{1,10}{2,10}{3,10}{4,10}\n", "Name", "Damage", "DPS", "Threat", "TPS");
            foreach (AbilityDK_Base ability in l_Openning)
            {
                DurationDuration += (float)ability.uDuration;
                szReport += string.Format("{0,-20} {1,10} {2,10} {3,10} {4,10}\n", ability.szName, ability.GetTotalDamage(), ability.GetDPS(), ability.GetTotalThreat(), ability.GetTPS());
            }

            szReport += string.Format("Duration(sec): {0:N}\n", (DurationDuration / 1000));
            szReport += string.Format("GCDs: {0:N}\n", m_GCDs);

            return szReport;
        }

        public string ReportRotation()
        {
            string szReport = "";

            szReport += string.Format("{0,-20}{1,10}{2,10}{3,10}{4,10}\n", "Name", "Damage", "DPS", "Threat", "TPS");
            foreach (AbilityDK_Base ability in ml_Rot)
            {
                szReport += string.Format("{0,-20} {1,10} {2,10} {3,10} {4,10} \n", ability.szName, ability.GetTotalDamage(), ability.GetDPS(), ability.GetTotalThreat(), ability.GetTPS());
            }
            szReport += string.Format("Duration(sec): {0,6}\n", m_DurationDuration / 1000);
            szReport += string.Format("GCDs: {0,6}\n", m_GCDs);
            return szReport;
        }

    }
}
