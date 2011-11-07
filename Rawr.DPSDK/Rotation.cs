using System;
using System.Collections.Generic;
using System.Text;
using Rawr.Base.Algorithms;

namespace Rawr.DK
{
    public class Rotation
    {
        public const uint MIN_GCD_MS_UH = 1000;
        public const uint MIN_GCD_MS = 1500;
        public enum Type
        {
            Custom, Blood, Frost, Unholy, Unknown,
        }
        public enum TalentTrees
        {
            Blood, Frost, Unholy, 
        }

        #region Variables
        public DKCombatTable m_CT;
        /// <summary>
        /// The rotation list of abilities.
        /// </summary>
        public List<AbilityDK_Base> ml_Rot;
//        private List<AbilityDK_Base> _RotCache;
//        private Type _RotCacheType = Type.Unknown;
//        private float _cachedHaste;

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

        string m_szRotationName = "Rotation by Solver";
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ct">Combat Table</param>
        /// <param name="bThreat">True if you want the results/sorting by Threat (rather Dam)</param>
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
            if (t.HighestTree == (int)TalentTrees.Blood)
            {
                // Blood
                curRotationType = Rotation.Type.Blood;
            }
            else if (t.HighestTree == (int)TalentTrees.Frost)
            {
                // Frost
                curRotationType = Rotation.Type.Frost;
            }
            if (t.HighestTree == (int)TalentTrees.Unholy)
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
                // Then if there are any long duration cast time spells
                // Then Rune Cool down.
                return Math.Max(Math.Max(GCDTime, m_CastDuration), m_TotalRuneCD);
            }
        }
        // Total Length of CDs.
        public float m_CooldownDuration { get; set; }
        // Total length of Cast times.
        public float m_CastDuration { get; set; }
        public int m_BloodRunes { get; set; }
        private float m_BonusRunicCorruptionHaste = 0;
        public int m_SingleRuneCD 
        { 
            get
            {
                return (int)(10000 / Math.Max(1f, (1f + m_CT.m_CState.m_Stats.PhysicalHaste + m_CT.m_CState.m_Stats.BonusRuneRegeneration + m_BonusRunicCorruptionHaste)));
            }
        }
        public int m_FrostRunes { get; set; }
        public int m_UnholyRunes { get; set; }
        public int m_DeathRunes { get; set; }
        public int m_TotalRuneCD { get; set; }
        /// <summary>
        /// Total RP generated/Spent for rotation.
        /// </summary>
        public int m_RunicPower { get; set; }
        public float m_CountREAbilities
        {
            get
            {
                float count = 0;
                if (null != ml_Rot)
                {
                    foreach (AbilityDK_Base ab in ml_Rot)
                    {
                        if (ab.AbilityIndex == (int)DKability.RuneStrike
                            || ab.AbilityIndex == (int)DKability.DeathCoil
                            || ab.AbilityIndex == (int)DKability.FrostStrike)
                            count += ab.fPartialValue;
                    }
                }
                return count;
            }
        }
        public uint m_CountDeathStrikes
        {
            get
            {
                uint count = 0;
                if (null != ml_Rot)
                {
                    foreach (AbilityDK_Base ab in ml_Rot)
                    {
                        if (ab.AbilityIndex == (int)DKability.DeathStrike)
                            ++count;
                    }
                }
                return count;
            }
        }
        public float m_FreeRunesFromRE
        {
            get
            {
                if (m_CT.m_CState.m_Talents.RunicCorruption > 0)
                    return 0;
                else
                    return ((float)m_CountREAbilities / .45f);
            }
        }
        
        private float _GCDs = 0;
        public float m_GCDs
        {
            get { return _GCDs; }
            set { _GCDs = value; }
        }
        public float GetGCDs
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
                    return (int)(m_GCDs * MIN_GCD_MS_UH);
                else
                    return (int)(m_GCDs * MIN_GCD_MS);
            }
        }

        private int _meleeSpecials;
        public int m_MeleeSpecials
        {
            get { return _meleeSpecials; }
            set { _meleeSpecials = value; }
        }
        private int _spellSpecials;
        public int m_SpellSpecials
        {
            get { return _spellSpecials; }
            set { _spellSpecials = value; }
        }
        public float getMeleeSpecialsPerSecond()
        {
            return m_MeleeSpecials / (m_RotationDuration / 1000);
        }
        public float getSpellSpecialsPerSecond()
        {
            return m_SpellSpecials / (m_RotationDuration / 1000);
        }
        public float getTotalSpecialsPerSecond()
        {
            return (m_MeleeSpecials + m_SpellSpecials) / (m_RotationDuration / 1000);
        }

        public uint m_FrostSpecials
        {
            get
            {
                return getAbilityCountofType(ItemDamageType.Frost);
            }
        }

        public uint m_ShadowSpecials
        {
            get
            {
                return getAbilityCountofType(ItemDamageType.Shadow);
            }
        }

        public float m_DSperSec
        {
            get
            {
                if (CurRotationDuration > 0)
                    return m_CountDeathStrikes / CurRotationDuration;
                else
                    return 0;
            }
        }

        private uint getAbilityCountofType(ItemDamageType i)
        {
            uint S = 0;
            if (null != ml_Rot)
            {
                foreach (AbilityDK_Base ability in ml_Rot)
                {
                    if (ability.tDamageType == i)
                        S++;
                }
            }
            return S;
        }

        /// <summary>
        /// Get the first Instance of ability used based on DKAbility Index.
        /// </summary>
        /// <param name="DKAi">Index value for ability from enum DKability</param>
        /// <returns></returns>
        public AbilityDK_Base GetAbilityOfType(DKability DKAi)
        {
            return GetAbilityOfType(ml_Rot, DKAi);
        }

        /// <summary>
        /// Get the first Instance of ability used based on DKAbility Index.
        /// </summary>
        /// <param name="DKAi">Index value for ability from enum DKability</param>
        /// <returns></returns>
        public static AbilityDK_Base GetAbilityOfType(List<AbilityDK_Base> l_Rot,  DKability DKAi)
        {
            if (null != l_Rot)
            {
                foreach (AbilityDK_Base a in l_Rot)
                {
                    if (a.AbilityIndex == (int)DKAi)
                        return a;
                }
            }
            return null;
        }

        public void ResetRotation()
        {
            ml_Rot = new List<AbilityDK_Base>();
            m_CT.m_CState.ResetCombatState();
            m_BloodRunes = 0;
            m_UnholyRunes = 0;
            m_FrostRunes = 0;
            m_DeathRunes = 0;
            m_RunicPower = 0;
            m_CastDuration = 0;
            m_GCDs = 0;
            m_MeleeSpecials = 0;
            m_SpellSpecials = 0;
            m_CooldownDuration = 0;
        }

        public void Solver()
        {
            //ReportRotation("Rotation by Solver.");
        }

        #region Preset Rotation
        /// <summary>
        /// This is my fire 1 of everything rotation.
        /// </summary>
        public void PRE_OneEachRot()
        {
            m_szRotationName = "One Each Rotation";
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

            BuildCosts();
        }

        /// <summary>
        /// This a basic IT, PS, HSx4 (or BBx4), DSx3, RSx? rotation.
        /// </summary>
        public void PRE_BloodDiseased()
        {
            m_szRotationName = "Blood Diseased Rotation";

            ResetRotation();
            // This will only happen while tanking...
            //m_CT.m_Opts.presence = Presence.Blood;
            // Setup an instance of each ability.
            // No runes:
            AbilityDK_Outbreak OutB = new AbilityDK_Outbreak(m_CT.m_CState);
            // Single Runes:
            AbilityDK_IcyTouch IT = new AbilityDK_IcyTouch(m_CT.m_CState);
            AbilityDK_FrostFever FF = new AbilityDK_FrostFever(m_CT.m_CState);
            AbilityDK_PlagueStrike PS = new AbilityDK_PlagueStrike(m_CT.m_CState);
            AbilityDK_BloodPlague BP = new AbilityDK_BloodPlague(m_CT.m_CState);
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
            AbilityDK_DancingRuneWeapon DRW = new AbilityDK_DancingRuneWeapon(m_CT.m_CState);

            // Fire DRW every chance we get.
            ml_Rot.Add(DRW);

            // Fill the 1.5 min CD w/ the sub rotation.
            
            uint subrotDuration = (9 * MIN_GCD_MS);
            for (int count = (int)(DRW.Cooldown / subrotDuration); count > 0; count--)
            {
                // Simple ITx1, PSx1, DSx1, HSx2 or BBx2, RS w/ RP (x3ish).
                if (Rawr.Properties.GeneralSettings.Default.PTRMode)
                {
                    ml_Rot.Add(OutB);
                    ml_Rot.Add(DS);
                }
                else
                {
                    ml_Rot.Add(IT);
                    ml_Rot.Add(PS);
                }
                ml_Rot.Add(FF);
                ml_Rot.Add(BP); // Scarlet Fever is tied to this for 4.0.6

                if (HS.TotalThreat > BB.TotalThreat)
                {
                    // Just HSx4
                    ml_Rot.Add(HS);
                    ml_Rot.Add(HS);
                    ml_Rot.Add(HS);
                    ml_Rot.Add(HS);
                }
                else
                {
                    // if BB does more, then BB.
                    ml_Rot.Add(BB);
                    ml_Rot.Add(BB);
                    ml_Rot.Add(BB);
                    ml_Rot.Add(BB);
                }

                // These will create DeathRunes that can allow flexibility in the rotation later on.
                ml_Rot.Add(DS);
                ml_Rot.Add(DS);
                ml_Rot.Add(DS);
            }


            #region Runic Power Pass 1.
            // How much RP do we have at this point?
            foreach (AbilityDK_Base ab in ml_Rot)
                m_RunicPower += ab.RunicPower;
            m_RunicPower = (int)((float)m_RunicPower);
            if (m_CT.m_CState.m_Stats.RPp5 > 0)
                m_RunicPower -= (int)((CurRotationDuration / 5) * m_CT.m_CState.m_Stats.RPp5);

            // Burn what we can.
            float RSCount = 0;
            for (RSCount = Math.Abs(m_RunicPower / RS.RunicPower); RSCount > 0; RSCount--)
            {
                ml_Rot.Add(RS);
                m_RunicPower += RS.RunicPower;
            }
            #endregion
            BuildCosts();
            #region RunicPower Pass 2.
            // How much RP do we have at this point?
            if (m_CT.m_CState.m_Stats.RPp5 > 0)
                m_RunicPower -= (int)((CurRotationDuration / 5) * m_CT.m_CState.m_Stats.RPp5);

            // Burn what we can.
            for (RSCount = Math.Abs((float)m_RunicPower / (float)RS.RunicPower); RSCount > 0; RSCount--)
            {
                if (RSCount >= 1)
                {
                    ml_Rot.Add(RS);
                    m_RunicPower += RS.RunicPower;
                }
                else
                {
                    AbilityDK_RuneStrike RSPartial = new AbilityDK_RuneStrike(m_CT.m_CState);
                    RSPartial.fPartialValue = RSCount;
                    RSPartial.bPartial = true;
                    RSPartial.szName = "RS (partial)";
                    RSPartial.RunicPower = (int)((float)RS.AbilityCost[(int)DKCostTypes.RunicPower] * RSCount);
                    m_RunicPower += RSPartial.RunicPower;
                    ml_Rot.Add(RSPartial);
                }
            }
            #endregion
            BuildCosts();
        }

        /// <summary>
        /// This a basic IT, PS, HSx4 (or BBx4), DSx3, RSx? rotation.
        /// </summary>
        public void PRE_BloodDiseaseLess()
        {
            m_szRotationName = "Blood Diseaseless Rotation";

            ResetRotation();
            // This will only happen while tanking...
            // Setup an instance of each ability.
            // No runes:
            //            AbilityDK_Outbreak OutB = new AbilityDK_Outbreak(m_CT.m_CState);
            // Single Runes:
            //AbilityDK_IcyTouch IT = new AbilityDK_IcyTouch(m_CT.m_CState);
            //AbilityDK_FrostFever FF = new AbilityDK_FrostFever(m_CT.m_CState);
            //AbilityDK_PlagueStrike PS = new AbilityDK_PlagueStrike(m_CT.m_CState);
            //AbilityDK_BloodPlague BP = new AbilityDK_BloodPlague(m_CT.m_CState);
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
            AbilityDK_DancingRuneWeapon DRW = new AbilityDK_DancingRuneWeapon(m_CT.m_CState);

            // Fire DRW every chance we get.
            ml_Rot.Add(DRW);

            // Fill the 1.5 min CD w/ the sub rotation.

            uint subrotDuration = (9 * MIN_GCD_MS);
            for (int count = (int)(DRW.Cooldown / subrotDuration); count > 0; count--)
            {
                // Simple ITx1, PSx1, DSx1, HSx2 or BBx2, RS w/ RP (x3ish).
                ml_Rot.Add(DS);
                ml_Rot.Add(DS);
//                ml_Rot.Add(BP); // Scarlet Fever is tied to this for 4.0.6

                if (HS.TotalThreat > BB.TotalThreat)
                {
                    // Just HSx4
                    ml_Rot.Add(HS);
                    ml_Rot.Add(HS);
                    ml_Rot.Add(HS);
                    ml_Rot.Add(HS);
                }
                else
                {
                    // if BB does more, then BB.
                    ml_Rot.Add(BB);
                    ml_Rot.Add(BB);
                    ml_Rot.Add(BB);
                    ml_Rot.Add(BB);
                }

                // These will create DeathRunes that can allow flexibility in the rotation later on.
                ml_Rot.Add(DS);
                ml_Rot.Add(DS);
                ml_Rot.Add(DS);
            }


            #region Runic Power Pass 1.
            // How much RP do we have at this point?
            foreach (AbilityDK_Base ab in ml_Rot)
                m_RunicPower += ab.RunicPower;
            m_RunicPower = (int)((float)m_RunicPower);
            if (m_CT.m_CState.m_Stats.RPp5 > 0)
                m_RunicPower -= (int)((CurRotationDuration / 5) * m_CT.m_CState.m_Stats.RPp5);

            // Burn what we can.
            float RSCount = 0;
            for (RSCount = Math.Abs(m_RunicPower / RS.RunicPower); RSCount > 0; RSCount--)
            {
                ml_Rot.Add(RS);
                m_RunicPower += RS.RunicPower;
            }
            #endregion
            BuildCosts();
            #region RunicPower Pass 2.
            // How much RP do we have at this point?
            if (m_CT.m_CState.m_Stats.RPp5 > 0)
                m_RunicPower -= (int)((CurRotationDuration / 5) * m_CT.m_CState.m_Stats.RPp5);

            // Burn what we can.
            for (RSCount = Math.Abs((float)m_RunicPower / (float)RS.RunicPower); RSCount > 0; RSCount--)
            {
                if (RSCount >= 1)
                {
                    ml_Rot.Add(RS);
                    m_RunicPower += RS.RunicPower;
                }
                else
                {
                    AbilityDK_RuneStrike RSPartial = new AbilityDK_RuneStrike(m_CT.m_CState);
                    RSPartial.fPartialValue = RSCount;
                    RSPartial.bPartial = true;
                    RSPartial.szName = "RS (partial)";
                    RSPartial.RunicPower = (int)((float)RS.AbilityCost[(int)DKCostTypes.RunicPower] * RSCount);
                    m_RunicPower += RSPartial.RunicPower;
                    ml_Rot.Add(RSPartial);
                }
            }
            #endregion
            BuildCosts();
        }

        /// <summary>
        /// This a basic IT, PS, HSx4 (or BBx4), DSx3, RSx? rotation.
        /// </summary>
        public void PRE_Frost()
        {
            m_szRotationName = "Frost Rotation";

            ResetRotation();
            // Setup an instance of each ability.
            // No runes:
            //            AbilityDK_Outbreak OutB = new AbilityDK_Outbreak(m_CT.m_CState);
            // Single Runes:
            AbilityDK_IcyTouch IT = new AbilityDK_IcyTouch(m_CT.m_CState);
            AbilityDK_FrostFever FF = new AbilityDK_FrostFever(m_CT.m_CState);
            AbilityDK_PlagueStrike PS = new AbilityDK_PlagueStrike(m_CT.m_CState);
            AbilityDK_BloodPlague BP = new AbilityDK_BloodPlague(m_CT.m_CState);
            AbilityDK_BloodStrike BS = new AbilityDK_BloodStrike(m_CT.m_CState);
            // AbilityDK_HeartStrike HS = new AbilityDK_HeartStrike(m_CT.m_CState);
            //            AbilityDK_NecroticStrike NS = new AbilityDK_NecroticStrike(m_CT.m_CState);
            //            AbilityDK_Pestilence Pest = new AbilityDK_Pestilence(m_CT.m_CState);
            // AbilityDK_BloodBoil BB = new AbilityDK_BloodBoil(m_CT.m_CState);
            AbilityDK_HowlingBlast HB = new AbilityDK_HowlingBlast(m_CT.m_CState);
            // AbilityDK_ScourgeStrike SS = new AbilityDK_ScourgeStrike(m_CT.m_CState);
            // AbilityDK_DeathNDecay DnD = new AbilityDK_DeathNDecay(m_CT.m_CState);
            // Multi Runes:
            // AbilityDK_DeathStrike DS = new AbilityDK_DeathStrike(m_CT.m_CState);
            // AbilityDK_FesteringStrike Fest = new AbilityDK_FesteringStrike(m_CT.m_CState);
            AbilityDK_Obliterate OB = new AbilityDK_Obliterate(m_CT.m_CState);
            // RP:  Unlikely to start w/ RP abilities to open.
            // AbilityDK_RuneStrike RS = new AbilityDK_RuneStrike(m_CT.m_CState);
            // each RS gives a 45% chance to renew a fully depleted runes.
            // thus 20 RSs gets us 9 extra runes.
            // Same is true for DC & FS
            // AbilityDK_DeathCoil DC = new AbilityDK_DeathCoil(m_CT.m_CState);
            AbilityDK_FrostStrike FS = new AbilityDK_FrostStrike(m_CT.m_CState);

            // Simple ITx1, PSx1, BSx2, OBx1  RS w/ RP (x3ish).
            // if Glyph of HB, then HB can be used to apply FF.
            if (m_CT.m_CState.m_Talents.GlyphofHowlingBlast
                && HB.DPS > IT.DPS)
            {
                ml_Rot.Add(HB);
            }
            else 
            {
                ml_Rot.Add(IT);
            }
            ml_Rot.Add(FF);
            ml_Rot.Add(PS);
            ml_Rot.Add(BP);

            // TODO: 4.1 Blood Runes are ALWAYS Death Runes.
            // These will create DeathRunes that can allow flexibility in the rotation later on.
            // ml_Rot.Add(BS);
            // ml_Rot.Add(BS);

            ml_Rot.Add(OB);
            ml_Rot.Add(OB);

            ml_Rot.Add(OB);
            ml_Rot.Add(OB);
            ml_Rot.Add(OB);

            #region RunicPower Pass 1.
            // How much RP do we have at this point?
            foreach (AbilityDK_Base ab in ml_Rot)
                m_RunicPower += ab.RunicPower;
            if (m_CT.m_CState.m_Stats.RPp5 > 0)
                m_RunicPower -= (int)((CurRotationDuration / 5) * m_CT.m_CState.m_Stats.RPp5);

            float RSCount = 0;
            // Burn what we can.
            for (RSCount = Math.Abs((float)m_RunicPower / (float)FS.RunicPower); RSCount > 0; RSCount--)
            {
                if (RSCount >= 1)
                {
                    ml_Rot.Add(FS);
                    m_RunicPower += FS.RunicPower;
                }
                // Partials at the end.

            }
            #endregion

            #region Talent: Rime
            if (m_CT.m_CState.m_Talents.Rime > 0)
            {
                AbilityDK_Base ability;
                // we want 1 full use, and then any sub values.
                if (HB.DPS > IT.DPS)
                {
                    ability = new AbilityDK_HowlingBlast(m_CT.m_CState);
                    ability.szName = "HB";
                }
                else
                {
                    ability = new AbilityDK_IcyTouch(m_CT.m_CState);
                    ability.szName = "IT";
                }
                ability.szName += " (Rime)";
                // These are free ITs/HBs.
                ability.AbilityCost[(int)DKCostTypes.Blood] = 0;
                ability.AbilityCost[(int)DKCostTypes.Frost] = 0;
                ability.AbilityCost[(int)DKCostTypes.UnHoly] = 0;
                ability.AbilityCost[(int)DKCostTypes.Death] = 0;
                ability.AbilityCost[(int)DKCostTypes.RunicPower] = 0;

                float fRimeMod = this.Count(DKability.Obliterate) * .15f * m_CT.m_CState.m_Talents.Rime;
                // 60% chance to proc 2 Rimes rather than 1.
                if (m_CT.m_CState.m_Stats.b2T13_DPS)
                    fRimeMod *= 1.6f;
                if (fRimeMod >= 1f)
                {
                    for (; fRimeMod >= 1; fRimeMod--)
                    {
                        ml_Rot.Add(ability);
                    }
                }
                if (fRimeMod > 0f && fRimeMod < 1f)
                {
                    if (HB.DPS > IT.DPS)
                    {
                        ability = new AbilityDK_HowlingBlast(m_CT.m_CState);
                        ability.szName = "HB";
                    }
                    else
                    {
                        ability = new AbilityDK_IcyTouch(m_CT.m_CState);
                        ability.szName = "IT";
                    }
                    ability.szName += " (Rime_partial)";
                    // These are free ITs/HBs.
                    ability.AbilityCost[(int)DKCostTypes.Blood] = 0;
                    ability.AbilityCost[(int)DKCostTypes.Frost] = 0;
                    ability.AbilityCost[(int)DKCostTypes.UnHoly] = 0;
                    ability.AbilityCost[(int)DKCostTypes.Death] = 0;
                    ability.AbilityCost[(int)DKCostTypes.RunicPower] = 0;
                    ability.bPartial = true;
                    ability.fPartialValue = fRimeMod;
                    ml_Rot.Add(ability);
                }
            }
            #endregion
            BuildCosts();
            #region Talent: Killing Machine
            float fRotInMins = CurRotationDuration / 60;
            float fKMPPM = 5 * (m_CT.m_CState.m_Talents.KillingMachine / 3);
            float KMProcCount = fKMPPM * fRotInMins;
            float fOBFSCount = Count(DKability.FrostStrike) + Count(DKability.Obliterate);
            float fPctOBFSCrit = KMProcCount / fOBFSCount;
            OB.SetKMCritChance(fPctOBFSCrit);
            FS.SetKMCritChance(fPctOBFSCrit);
            #endregion
            BuildCosts();
            #region RunicPower Pass 2.
            // How much RP do we have at this point?
            if (m_CT.m_CState.m_Stats.RPp5 > 0)
                m_RunicPower -= (int)((CurRotationDuration / 5) * m_CT.m_CState.m_Stats.RPp5);

            // Burn what we can.
            for (RSCount = Math.Abs((float)m_RunicPower / (float)FS.RunicPower); RSCount > 0; RSCount--)
            {
                if (RSCount >= 1)
                {
                    ml_Rot.Add(FS);
                    m_RunicPower += FS.RunicPower;
                }
                else
                {
                    AbilityDK_FrostStrike FSPartial = new AbilityDK_FrostStrike(m_CT.m_CState);
                    FSPartial.fPartialValue = RSCount;
                    FSPartial.bPartial = true;
                    FSPartial.szName = "FS (partial)";
                    FSPartial.RunicPower = (int)((float)FS.AbilityCost[(int)DKCostTypes.RunicPower] * RSCount);
                    m_RunicPower += FSPartial.RunicPower;
                    ml_Rot.Add(FSPartial);
                }
            }
            #endregion
            BuildCosts();

        }
        
        /// <summary>
        /// This a basic rotation
        /// </summary>
        public void PRE_Unholy_dynamic() // Expensive
        {
            /*
            if (_RotCacheType == Type.Unholy
                && _RotCache.Count > 100
                && Math.Abs(_cachedHaste - m_CT.m_CState.m_Stats.PhysicalHaste) <= 0.05f ) // Haste is within 5% of the cached Haste.
            {
                ml_Rot = _RotCache;
                foreach (AbilityDK_Base ab in ml_Rot)
                {
                    ab.UpdateCombatState(m_CT.m_CState);
                }
                BuildCosts();
                return;
            }
             * */
            // Because, really, Unholy should be in Unholy Presence. 
            // m_CT.m_Opts.presence = Presence.Unholy;
            m_szRotationName = "Unholy Rotation";

            ResetRotation();
            // Setup an instance of each ability.
            // No runes:
            AbilityDK_Outbreak Outbreak = new AbilityDK_Outbreak(m_CT.m_CState);
            // Single Runes:
//            AbilityDK_IcyTouch IT = new AbilityDK_IcyTouch(m_CT.m_CState);
            AbilityDK_FrostFever FF = new AbilityDK_FrostFever(m_CT.m_CState);
//            AbilityDK_PlagueStrike PS = new AbilityDK_PlagueStrike(m_CT.m_CState);
            AbilityDK_BloodPlague BP = new AbilityDK_BloodPlague(m_CT.m_CState);
            // AbilityDK_BloodStrike BS = new AbilityDK_BloodStrike(m_CT.m_CState);
            // AbilityDK_HeartStrike HS = new AbilityDK_HeartStrike(m_CT.m_CState);
            // AbilityDK_NecroticStrike NS = new AbilityDK_NecroticStrike(m_CT.m_CState);
            // AbilityDK_Pestilence Pest = new AbilityDK_Pestilence(m_CT.m_CState);
            // AbilityDK_BloodBoil BB = new AbilityDK_BloodBoil(m_CT.m_CState);
            // AbilityDK_HowlingBlast HB = new AbilityDK_HowlingBlast(m_CT.m_CState);
            // AbilityDK_ScourgeStrike SS = new AbilityDK_ScourgeStrike(m_CT.m_CState);
            // AbilityDK_DeathNDecay DnD = new AbilityDK_DeathNDecay(m_CT.m_CState);
            // Multi Runes:
            // AbilityDK_DeathStrike DS = new AbilityDK_DeathStrike(m_CT.m_CState);
            AbilityDK_FesteringStrike Fest = new AbilityDK_FesteringStrike(m_CT.m_CState);
            // AbilityDK_Obliterate OB = new AbilityDK_Obliterate(m_CT.m_CState);
            // RP:  Unlikely to start w/ RP abilities to open.
            // AbilityDK_RuneStrike RS = new AbilityDK_RuneStrike(m_CT.m_CState);
            // each RS gives a 45% chance to renew a fully depleted runes.
            // thus 20 RSs gets us 9 extra runes.
            // Same is true for DC & FS
            AbilityDK_DeathCoil DC = new AbilityDK_DeathCoil(m_CT.m_CState);
            AbilityDK_UnholyBlight UB = null;
            if (DC.ml_TriggeredAbility.Length > 0) UB = DC.ml_TriggeredAbility[0] as AbilityDK_UnholyBlight;
            // AbilityDK_FrostStrike FS = new AbilityDK_FrostStrike(m_CT.m_CState);
            DarkTranformation Dark = new DarkTranformation(m_CT.m_CState);

            // Simple ITx1, PSx1, BSx1, SSx4 & Festx4.
            // Initial rotation build.
            int[] AvailableResources = new int[EnumHelper.GetCount(typeof(DKCostTypes))];
            AvailableResources[(int)DKCostTypes.Blood] = 2;
            AvailableResources[(int)DKCostTypes.Frost] = 2;
            AvailableResources[(int)DKCostTypes.UnHoly] = 2;
            if (m_CT.m_CState.m_Talents.DarkTransformation > 0)
            {
                // this won't be there if they're not spec'd for it.
                ml_Rot.Add(Dark); // Dark Transformation.
                ProcessRunningRunes(AvailableResources, Dark.AbilityCost);
            }
            uint subrotDuration = 0;
            uint GCDdur = MIN_GCD_MS;
            // Fill the 3 mins duration 
            if (m_CT.m_Opts.presence == Presence.Unholy)
                GCDdur = MIN_GCD_MS_UH;

            subrotDuration = Outbreak.Cooldown;
            uint diseaseGCDs = 0;
            for (int count = (int)(Dark.Cooldown / subrotDuration); count > 0; count--)
            {
                if (m_CT.m_CState.m_Stats.RPp5 > 0)
                    AvailableResources[(int)DKCostTypes.RunicPower] += (int)((subrotDuration / 5000) * m_CT.m_CState.m_Stats.RPp5);
                // TODO: This still assumes that we're filling every GCD w/ an ability. 
                // We know that's not the case in most situations.
                ml_Rot.Add(Outbreak); // 60 sec CD.
                ProcessRunningRunes(AvailableResources, Outbreak.AbilityCost);
                ml_Rot.Add(FF);
                ml_Rot.Add(BP);
                // 1 GCDs

                // Fill the disease.
                diseaseGCDs = Outbreak.Cooldown / GCDdur; 
                
                float MaxRP = 100 + m_CT.m_CState.m_Stats.BonusMaxRunicPower;

                List<AbilityDK_Base> l_Abilities;

                for (; diseaseGCDs > 0; )
                {
                    bool bEnoughRP = (-1 * AvailableResources[(int)DKCostTypes.RunicPower]) > DC.RunicPower;
                    bool bOverCapRP = (AvailableResources[(int)DKCostTypes.RunicPower] + Fest.RunicPower) > MaxRP;

                    if ( (!bEnoughRP 
                        && !bOverCapRP) ) 
                    {
                        l_Abilities = GetFilteredListOfAbilities(AvailableResources, m_CT.m_CState);
                        if (l_Abilities.Count > 0)
                        {
                            foreach (AbilityDK_Base ab in l_Abilities)
                            {
                                ab.UpdateCombatState(m_CT.m_CState);
                            }
                            l_Abilities.Sort(AbilityDK_Base.CompareDPSByRunes);
                            if (l_Abilities[0].AbilityIndex == (int)DKability.BloodStrike ||
                                l_Abilities[0].AbilityIndex == (int)DKability.IcyTouch)
                            {
                                l_Abilities[0] = GetAbilityOfType(l_Abilities, DKability.FesteringStrike);
                            }
                            ProcessRunningRunes(AvailableResources, l_Abilities[0].AbilityCost);
                            ml_Rot.Add(l_Abilities[0]);

                        }
                        else
                        {
                            // If the list is 0, means all the runes have been used. 
                            // Reset runes
                            AvailableResources[(int)DKCostTypes.Blood] = 2;
                            AvailableResources[(int)DKCostTypes.Frost] = 2;
                            AvailableResources[(int)DKCostTypes.UnHoly] = 2;
                        }
                    }
                    else
                    {
                        ml_Rot.Add(DC);
                        ProcessRunningRunes(AvailableResources, DC.AbilityCost);
                    }
                    diseaseGCDs--;
                }
            }

            // How much RP do we have left at this point?
            foreach (AbilityDK_Base ab in ml_Rot)
                m_RunicPower += ab.RunicPower;

            BuildCosts();

            // subsequent:
            #region Unholy Blight
            if (UB != null)
            {
                uint mSecperDC = (uint)m_RotationDuration / Count(DKability.DeathCoil);
                uint UBCount = (uint)m_RotationDuration / Math.Max(UB.Cooldown, mSecperDC);
                for (; UBCount > 0; UBCount--)
                {
                    ml_Rot.Add(UB);
                }
            }
            #endregion
            /*
            _cachedHaste = m_CT.m_CState.m_Stats.PhysicalHaste;
            _RotCacheType = Type.Unholy;
            _RotCache = ml_Rot;
             * */
        }

        /// <summary>
        /// This a basic preset rotation consuming the full CD of Dark Transformation.
        /// </summary>
        public void PRE_Unholy()
        {
            ResetRotation();

            m_szRotationName = "Unholy Rotation";
            // Because, really, Unholy should be in Unholy Presence. 
            if (m_CT.m_Opts.presence != Presence.Unholy)
            {
                m_szRotationName += "\nYou should be in Unholy Presence.";
            }
            // Setup an instance of each ability.
            // No runes:
            AbilityDK_Outbreak Outbreak = new AbilityDK_Outbreak(m_CT.m_CState);
            // Single Runes:
            //            AbilityDK_IcyTouch IT = new AbilityDK_IcyTouch(m_CT.m_CState);
            AbilityDK_FrostFever FF = new AbilityDK_FrostFever(m_CT.m_CState);
            //            AbilityDK_PlagueStrike PS = new AbilityDK_PlagueStrike(m_CT.m_CState);
            AbilityDK_BloodPlague BP = new AbilityDK_BloodPlague(m_CT.m_CState);
            // AbilityDK_BloodStrike BS = new AbilityDK_BloodStrike(m_CT.m_CState);
            AbilityDK_ScourgeStrike SS = new AbilityDK_ScourgeStrike(m_CT.m_CState);
            AbilityDK_FesteringStrike Fest = new AbilityDK_FesteringStrike(m_CT.m_CState);
            AbilityDK_DeathCoil DC = new AbilityDK_DeathCoil(m_CT.m_CState);
            AbilityDK_UnholyBlight UB = null;
            if (DC.ml_TriggeredAbility != null && DC.ml_TriggeredAbility.Length > 0) UB = DC.ml_TriggeredAbility[0] as AbilityDK_UnholyBlight;
            DarkTranformation Dark = new DarkTranformation(m_CT.m_CState);
            AbilityDK_DeathNDecay DnD = new AbilityDK_DeathNDecay(m_CT.m_CState);

            // Simple outbreak, Festx2 SSx6 & Fill w/ DCs
            int[] AvailableResources = new int[EnumHelper.GetCount(typeof(DKCostTypes))];
            if (m_CT.m_CState.m_Talents.DarkTransformation > 0)
            {
                // this won't be there if they're not spec'd for it.
                ml_Rot.Add(Dark); // Dark Transformation.
                ProcessRunningRunes(AvailableResources, Dark.AbilityCost);
            }
            uint subrotDuration = 0;
            uint GCDdur = MIN_GCD_MS;
            // Fill the 3 mins duration 
            if (m_CT.m_Opts.presence == Presence.Unholy)
                GCDdur = MIN_GCD_MS_UH;

            subrotDuration = Outbreak.Cooldown;
            uint diseaseGCDs = Outbreak.Cooldown / GCDdur;
            for (int count = (int)(Dark.Cooldown / subrotDuration); count > 0; count--)
            {
                if (m_CT.m_CState.m_Stats.RPp5 > 0)
                    AvailableResources[(int)DKCostTypes.RunicPower] += (int)((subrotDuration / 5000) * m_CT.m_CState.m_Stats.RPp5);
                // TODO: This still assumes that we're filling every GCD w/ an ability. 
                // We know that's not the case in most situations.
                ml_Rot.Add(Outbreak); // 60 sec CD.
                ProcessRunningRunes(AvailableResources, Outbreak.AbilityCost);
                ml_Rot.Add(FF);
                ml_Rot.Add(BP);
                // 1 GCDs

                // Fill the disease.
                for (uint i = 3; i > 0; i--)
                {
                    if (i == 3)
                    {
                        ml_Rot.Add(DnD); diseaseGCDs--;
                        AvailableResources[(int)DKCostTypes.RunicPower] += DnD.RunicPower;
                    }
                    else
                    {
                        ml_Rot.Add(SS); diseaseGCDs--;
                        AvailableResources[(int)DKCostTypes.RunicPower] += SS.RunicPower;
                    }
                    ml_Rot.Add(SS); diseaseGCDs--;
                    ml_Rot.Add(Fest); diseaseGCDs--;
                    ml_Rot.Add(Fest); diseaseGCDs--;
                    ml_Rot.Add(SS); diseaseGCDs--;
                    ml_Rot.Add(SS); diseaseGCDs--;
                    ml_Rot.Add(SS); diseaseGCDs--;
                    ml_Rot.Add(SS); diseaseGCDs--;
                    AvailableResources[(int)DKCostTypes.RunicPower] += SS.RunicPower * 5;
                    AvailableResources[(int)DKCostTypes.RunicPower] += Fest.RunicPower * 2;
                }
            }
            m_RunicPower = -1 * AvailableResources[(int)DKCostTypes.RunicPower];
            // Burn All of the RP we can.
            for (float RSCount = Math.Abs((float)m_RunicPower / (float)DC.RunicPower); RSCount > 0; RSCount--)
            {
                if (RSCount >= 1)
                {
                    ml_Rot.Add(DC);
                    m_RunicPower += DC.RunicPower;
                }
                else
                {
                    AbilityDK_DeathCoil DCPartial = new AbilityDK_DeathCoil(m_CT.m_CState);
                    DCPartial.fPartialValue = RSCount;
                    DCPartial.bPartial = true;
                    DCPartial.szName = "DC (partial)";
                    DCPartial.RunicPower = (int)((float)DC.RunicPower * DCPartial.fPartialValue);
                    m_RunicPower += DCPartial.RunicPower;
                    ml_Rot.Add(DCPartial);
                }
            }
            BuildCosts();

            #region Sudden Doom
            if (m_CT.m_CState.m_Talents.SuddenDoom > 0)
            {
                AbilityDK_Base ability;
                float fRimeMod = this.Count(DKability.White) * (.05f * (float)m_CT.m_CState.m_Talents.SuddenDoom);
                // 30% chance to proc 2 SDs rather than 1.
                if (m_CT.m_CState.m_Stats.b2T13_DPS)
                    fRimeMod *= 1.3f;
                if (fRimeMod > 1)
                {
                    for (; fRimeMod > 1; fRimeMod--)
                    {
                        ability = new AbilityDK_DeathCoil(m_CT.m_CState);
                        ability.szName = "DC (SuddenDoom)";
                        // These are free DCs.
                        ability.AbilityCost[(int)DKCostTypes.Blood] = 0;
                        ability.AbilityCost[(int)DKCostTypes.Frost] = 0;
                        ability.AbilityCost[(int)DKCostTypes.UnHoly] = 0;
                        ability.AbilityCost[(int)DKCostTypes.Death] = 0;
                        ability.AbilityCost[(int)DKCostTypes.RunicPower] = 0;
                        ml_Rot.Add(ability);
                    }
                }
                if (fRimeMod > 0 && fRimeMod < 1)
                {
                    // we want 1 full use, and then any sub values.
                    ability = new AbilityDK_DeathCoil(m_CT.m_CState);
                    ability.szName = "DC (SD_Partial)";
                    ability.bPartial = true;
                    ability.fPartialValue = fRimeMod;
                    // These are free DCs.
                    ability.AbilityCost[(int)DKCostTypes.Blood] = 0;
                    ability.AbilityCost[(int)DKCostTypes.Frost] = 0;
                    ability.AbilityCost[(int)DKCostTypes.UnHoly] = 0;
                    ability.AbilityCost[(int)DKCostTypes.Death] = 0;
                    ability.AbilityCost[(int)DKCostTypes.RunicPower] = 0;
                    ml_Rot.Add(ability);
                }
            }
            #endregion

            #region Unholy Blight
            if (UB != null)
            {
                uint mSecperDC = (uint)m_RotationDuration / Count(DKability.DeathCoil);
                uint UBCount = (uint)m_RotationDuration / Math.Max(UB.uDuration, mSecperDC);
                for (; UBCount > 0; UBCount--)
                {
                    ml_Rot.Add(UB);
                }
            }
            #endregion
            BuildCosts();

        }

        /// <summary>
        /// This a more basic rotation that assumes Dark Tranformations will be rolled in
        /// </summary>
        public void PRE_Unholy_short()
        {
            ResetRotation();

            m_szRotationName = "Unholy Rotation";
            // Because, really, Unholy should be in Unholy Presence. 
            if (m_CT.m_Opts.presence != Presence.Unholy)
            {
                m_szRotationName += "\nYou should be in Unholy Presence.";
            }
            // Setup an instance of each ability.
            // No runes:
            AbilityDK_Outbreak Outbreak = new AbilityDK_Outbreak(m_CT.m_CState);
            // Single Runes:
            //            AbilityDK_IcyTouch IT = new AbilityDK_IcyTouch(m_CT.m_CState);
            AbilityDK_FrostFever FF = new AbilityDK_FrostFever(m_CT.m_CState);
            //            AbilityDK_PlagueStrike PS = new AbilityDK_PlagueStrike(m_CT.m_CState);
            AbilityDK_BloodPlague BP = new AbilityDK_BloodPlague(m_CT.m_CState);
            // AbilityDK_BloodStrike BS = new AbilityDK_BloodStrike(m_CT.m_CState);
            AbilityDK_ScourgeStrike SS = new AbilityDK_ScourgeStrike(m_CT.m_CState);
            AbilityDK_FesteringStrike Fest = new AbilityDK_FesteringStrike(m_CT.m_CState);
            AbilityDK_DeathCoil DC = new AbilityDK_DeathCoil(m_CT.m_CState);
            AbilityDK_UnholyBlight UB = null;
            if (DC.ml_TriggeredAbility != null && DC.ml_TriggeredAbility.Length > 0) UB = DC.ml_TriggeredAbility[0] as AbilityDK_UnholyBlight;
            AbilityDK_DeathNDecay DnD = new AbilityDK_DeathNDecay(m_CT.m_CState);

            // Simple outbreak, Festx2 DnD, SSx5 & Fill w/ DCs
            int[] AvailableResources = new int[EnumHelper.GetCount(typeof(DKCostTypes))];
            uint subrotDuration = 0;
            uint GCDdur = MIN_GCD_MS;
            // Fill the 3 mins duration 
            if (m_CT.m_Opts.presence == Presence.Unholy)
                GCDdur = MIN_GCD_MS_UH;

            subrotDuration = Outbreak.Cooldown;
            uint diseaseGCDs = Outbreak.Cooldown / GCDdur;
            if (m_CT.m_CState.m_Stats.RPp5 > 0)
                AvailableResources[(int)DKCostTypes.RunicPower] += (int)((subrotDuration / 5000) * m_CT.m_CState.m_Stats.RPp5);
            // TODO: This still assumes that we're filling every GCD w/ an ability. 
            // We know that's not the case in most situations.
            ml_Rot.Add(Outbreak); // 60 sec CD.
            ProcessRunningRunes(AvailableResources, Outbreak.AbilityCost);
            ml_Rot.Add(FF);
            ml_Rot.Add(BP);
            // 1 GCDs

            // Fill the disease.
            for (uint i = 3; i > 0; i--)
            {
                ml_Rot.Add(DnD); diseaseGCDs--;
                ProcessRunningRunes(AvailableResources, DnD.AbilityCost);
                ml_Rot.Add(SS); diseaseGCDs--;
                ProcessRunningRunes(AvailableResources, SS.AbilityCost);
                ml_Rot.Add(Fest); diseaseGCDs--;
                ProcessRunningRunes(AvailableResources, Fest.AbilityCost);
                ml_Rot.Add(Fest); diseaseGCDs--;
                ProcessRunningRunes(AvailableResources, Fest.AbilityCost);
                ml_Rot.Add(SS); diseaseGCDs--;
                ProcessRunningRunes(AvailableResources, SS.AbilityCost);
                ml_Rot.Add(SS); diseaseGCDs--;
                ProcessRunningRunes(AvailableResources, SS.AbilityCost);
                ml_Rot.Add(SS); diseaseGCDs--;
                ProcessRunningRunes(AvailableResources, SS.AbilityCost);
                ml_Rot.Add(SS); diseaseGCDs--;
                ProcessRunningRunes(AvailableResources, SS.AbilityCost);
                while (AvailableResources[(int)DKCostTypes.RunicPower] > DC.RunicPower
                    && diseaseGCDs > 0)
                {
                    ml_Rot.Add(DC); diseaseGCDs--;
                    ProcessRunningRunes(AvailableResources, DC.AbilityCost);
                }
                BuildCosts();
            }
            m_RunicPower = -1 * AvailableResources[(int)DKCostTypes.RunicPower];
            if (m_RunicPower < 0)
            {
                AbilityDK_DeathCoil DCP = new AbilityDK_DeathCoil(m_CT.m_CState);
                DCP.szName = "DC (RP_Partial)";
                DCP.bPartial = true;
                DCP.fPartialValue = (float)Math.Abs(m_RunicPower) / (float)DCP.RunicPower;
                DCP.RunicPower = (int)((float)DCP.RunicPower * DCP.fPartialValue);
                ml_Rot.Add(DCP);
                m_RunicPower += DCP.RunicPower;
            }
            BuildCosts();

            #region Sudden Doom
            if (m_CT.m_CState.m_Talents.SuddenDoom > 0)
            {
                AbilityDK_Base ability;
                float fRimeMod = this.Count(DKability.White) * (.05f * (float)m_CT.m_CState.m_Talents.SuddenDoom);
                if (fRimeMod > 1)
                {
                    for (; fRimeMod > 1; fRimeMod--)
                    {
                        ability = new AbilityDK_DeathCoil(m_CT.m_CState);
                        ability.szName = "DC (SuddenDoom)";
                        // These are free DCs.
                        ability.AbilityCost[(int)DKCostTypes.Blood] = 0;
                        ability.AbilityCost[(int)DKCostTypes.Frost] = 0;
                        ability.AbilityCost[(int)DKCostTypes.UnHoly] = 0;
                        ability.AbilityCost[(int)DKCostTypes.Death] = 0;
                        ability.AbilityCost[(int)DKCostTypes.RunicPower] = 0;
                        ml_Rot.Add(ability);
                    }
                }
                if (fRimeMod > 0 && fRimeMod < 1)
                {
                    // we want 1 full use, and then any sub values.
                    ability = new AbilityDK_DeathCoil(m_CT.m_CState);
                    ability.szName = "DC (SD_Partial)";
                    ability.bPartial = true;
                    ability.fPartialValue = fRimeMod;
                    // These are free DCs.
                    ability.AbilityCost[(int)DKCostTypes.Blood] = 0;
                    ability.AbilityCost[(int)DKCostTypes.Frost] = 0;
                    ability.AbilityCost[(int)DKCostTypes.UnHoly] = 0;
                    ability.AbilityCost[(int)DKCostTypes.Death] = 0;
                    ability.AbilityCost[(int)DKCostTypes.RunicPower] = 0;
                    ml_Rot.Add(ability);
                }
            }
            #endregion

            #region Unholy Blight
            if (UB != null)
            {
                uint mSecperDC = (uint)m_RotationDuration / Count(DKability.DeathCoil);
                uint UBCount = (uint)m_RotationDuration / Math.Max(UB.uDuration, mSecperDC);
                for (; UBCount > 0; UBCount--)
                {
                    ml_Rot.Add(UB);
                }
            }
            #endregion
            BuildCosts();

        }


        #endregion

        public void ResetCosts()
        {
            m_GCDs = 0;
            m_MeleeSpecials = 0;
            m_SpellSpecials = 0;
            m_BloodRunes = 0;
            m_FrostRunes = 0;
            m_UnholyRunes = 0;
            m_DeathRunes = 0;
            TotalDamage = 0;
            TotalThreat = 0;
            m_TotalRuneCD = 0;
        }

        public static bool ProcessRunningRunes(int[] CurrentAbilityStatus, int[] U)
        {
            bool r = true;
            bool bDeathRunes = false;
            int[] _cachedCurrentAbilityStatus = CurrentAbilityStatus.Clone() as int[];
            int[] Update = U.Clone() as int[];
            int[] NewUpdate = new int[EnumHelper.GetCount(typeof(DKCostTypes))];
            foreach (int i in EnumHelper.GetValues(typeof(DKCostTypes)))
            {
                if (i > (int)DKCostTypes.None
                    && i <= (int)DKCostTypes.RunicPower
                    && Update[i] != 0)
                {
                    // If there is not enough resources to process request.
                    if ((Update[i] > 0)
                        && (CurrentAbilityStatus[i] < Update[i]))
                    {
                        // Process Death Runes
                        if (i != (int)DKCostTypes.RunicPower
                            && i != (int)DKCostTypes.Death)
                        {
                            NewUpdate[(int)DKCostTypes.Death]++;
                            bDeathRunes = true;
                            Update[i]--;
                        }
                        else
                        {
                            r = false;
                            goto Exit;
                        }
                    }
                    else
                    {
                        CurrentAbilityStatus[i] -= Update[i];
                    }
                }
            }
            if (bDeathRunes)
            {
                r = ProcessRunningRunes(CurrentAbilityStatus, NewUpdate);
            }

            Exit:
                if (!r)
                {
                    foreach (int i in EnumHelper.GetValues(typeof(DKCostTypes)))
                    {
                        CurrentAbilityStatus[i] = _cachedCurrentAbilityStatus[i];
                    }
                }
                return r;
        }

        public void BuildCosts()
        {
            ResetCosts();
            if (null != ml_Rot)
            {
                foreach (AbilityDK_Base ability in ml_Rot)
                {
                    // Populate the costs here.
                    // GCD count.
                    if (ability.bTriggersGCD)
                    {
                        if (ability.bPartial)
                            m_GCDs += ability.fPartialValue;
                        else
                            m_GCDs++;
                    }

                    // Melee v. SpellSpecial count.
                    if (ability.uRange == AbilityDK_Base.MELEE_RANGE)
                    {
                        m_MeleeSpecials++;
                    }
                    else if (ability.uRange > AbilityDK_Base.MELEE_RANGE)
                    {
                        m_SpellSpecials++;
                    }
                    else
                    {
                        // Diseases should not count.  
                        // And they have a range of 0.
                    }
                    // Rune Counts
                    m_BloodRunes += ability.AbilityCost[(int)DKCostTypes.Blood];
                    m_FrostRunes += ability.AbilityCost[(int)DKCostTypes.Frost];
                    m_UnholyRunes += ability.AbilityCost[(int)DKCostTypes.UnHoly];
                    m_DeathRunes += ability.AbilityCost[(int)DKCostTypes.Death];

                    // m_CooldownDuration += ability.Cooldown; // CDs will overlap.
                    m_CastDuration += ability.CastTime;
                    // m_DurationDuration // Durations will overlap.

                    TotalDamage += ability.GetTotalDamage();
                    TotalThreat += ability.GetTotalThreat();
                }
            }
            #region Talent: Runic Corruption
            // For each death coil, improve the Rune Regen by 50% per point for 3 sec.
            uint RCRegenDur = 0;
            float RCHaste = 0;
            if (m_CT.m_CState.m_Talents.RunicCorruption > 0)
            {
                RCRegenDur = Count(DKability.DeathCoil) * 3 * 1000;
                if (RCRegenDur > 0)
                {
                    RCHaste = (m_CT.m_CState.m_Talents.RunicCorruption * .5f);
                    float RCPpct = RCRegenDur / (GCDTime > 0 ? (Math.Min(GCDTime, RCRegenDur)) : RCRegenDur);
                    RCHaste *= RCPpct * m_CT.m_Opts.EffectiveRE;
                    m_BonusRunicCorruptionHaste = RCHaste;
                }
            }
            #endregion
            // spend the death runes available:
            int[] abCost = new int[EnumHelper.GetCount(typeof(DKCostTypes))];
            abCost[(int)DKCostTypes.Blood] = m_BloodRunes;
            abCost[(int)DKCostTypes.Frost] = m_FrostRunes;
            abCost[(int)DKCostTypes.UnHoly] = m_UnholyRunes;
            if (curRotationType == Type.Frost)
            {
                m_DeathRunes =  m_FrostRunes / -3;
                m_DeathRunes += m_UnholyRunes / -3;
                abCost[(int)DKCostTypes.Death] = m_DeathRunes;
            }
            else
                abCost[(int)DKCostTypes.Death] = m_DeathRunes;
#if false // rune tap
            // Blood Tap
            // Free Death Rune every 60 Sec.
            float BT_DeathRunes = 0;
            BT_DeathRunes = CurRotationDuration / (60f - ((float)m_CT.m_CState.m_Talents.ImprovedBloodTap * 15f));
            // TODO: Start handling partial runes
            m_DeathRunes -= (int)BT_DeathRunes;
            abCost[(int)DKCostTypes.Death] = m_DeathRunes;
            
#endif

            int hiRuneIndex = DKCombatTable.GetHighestRuneCountIndex(abCost);
            int DeathRunesSpent = 0;
            DeathRunesSpent = DKCombatTable.SpendDeathRunes(abCost, DeathRunesSpent);
            m_DeathRunes = DeathRunesSpent;
            m_BloodRunes = abCost[(int)DKCostTypes.Blood];
            m_FrostRunes = abCost[(int)DKCostTypes.Frost];
            m_UnholyRunes = abCost[(int)DKCostTypes.UnHoly];
            int BRCD = m_BloodRunes * m_SingleRuneCD;
            int FRCD = m_FrostRunes * m_SingleRuneCD;
            int URCD = m_UnholyRunes * m_SingleRuneCD;
//            int DRCD = (m_DeathRunes - (int)BT_DeathRunes) * m_SingleRuneCD; // Blood Tap runes are free.
            int DRCD = m_DeathRunes * m_SingleRuneCD; // Blood Tap runes are free.
            if (curRotationType == Type.Unholy) DRCD /= 2;
            //What about multi-rune abilities?
            m_TotalRuneCD = (BRCD + FRCD + URCD + DRCD)/3; // Max CD of the runes.
            // Assume that we can't get a full CD renewed from Runic Empowerment.
            // So we'll distribute the RE procs over all Rune types and bleed some time off as well.
            m_TotalRuneCD -= (int)(m_FreeRunesFromRE * m_CT.m_Opts.EffectiveRE * m_SingleRuneCD / 6); 
#if DEBUG
            // Ensure that m_TotalRuneCD != 0
            if (m_TotalRuneCD == 0)
                throw new Exception("TotalRuneCD == 0");
#endif

            #region White damage
            int iWhiteCount = 0;
            if (null != m_CT.MH)
            {
                iWhiteCount = (int)(CurRotationDuration / m_CT.MH.hastedSpeed);
                uint iCurrentWS = Count(DKability.White);
                AbilityDK_WhiteSwing MHWS = new AbilityDK_WhiteSwing(m_CT.m_CState);
                MHWS.UpdateCombatState(m_CT.m_CState);

                if (iWhiteCount > iCurrentWS)
                {
                    for (int i = 0; i < (iWhiteCount - iCurrentWS); i++)
                    {
                        ml_Rot.Add(MHWS);
                        TotalDamage += MHWS.TotalDamage;
                        TotalThreat += MHWS.TotalThreat;
                    }
                }
            }
            if (null != m_CT.OH)
            {
                iWhiteCount = (int)(CurRotationDuration / m_CT.OH.hastedSpeed);
                uint iCurrentWSOH = Count(DKability.WhiteOH);
                AbilityDK_WhiteSwing OHWS = new AbilityDK_WhiteSwing(m_CT.m_CState);
                OHWS.UpdateCombatState(m_CT.m_CState, true);

                if (iWhiteCount > iCurrentWSOH)
                {
                    for (int i = 0; i < (iWhiteCount - iCurrentWSOH); i++)
                    {
                        ml_Rot.Add(OHWS);
                        TotalDamage += OHWS.TotalDamage;
                        TotalThreat += OHWS.TotalThreat;
                    }
                }
            }
            #endregion
        }

        public static string ReportRotation(List<AbilityDK_Base> l_Openning, string szReportName = "")
        {
            string szReport = szReportName + "\n";
            float DurationDuration = 0;
            string szFormat = "{0,-15}|{1,7}|{2,7:0.0}|{3,7:0}|{4,7:0.0}\n";
            int GCDs = 0;

            szReport += string.Format(szFormat, "Name", "Dam", "DPS", "Threat", "TPS");
            foreach (AbilityDK_Base ability in l_Openning)
            {
                DurationDuration += (float)ability.uDuration;
                szReport += string.Format(szFormat, ability.szName, ability.GetTotalDamage(), ability.GetDPS(), ability.GetTotalThreat(), ability.GetTPS());
                if (ability.bTriggersGCD) GCDs++;
            }

            szReport += string.Format("Duration(sec): {0,6:0.0}\n", DurationDuration);
            szReport += string.Format("GCDs:          {0,6}\n", GCDs);

            return szReport;
        }

        public string ReportRotation()
        {
            string szReport = m_szRotationName + "\n";
            string szFormat = "{0,-15}|{1,7:0}|{2,7:0.0}|{3,7:0}|{4,7:0.0}\n";

            szReport += string.Format(szFormat, "Name", "Dam", "DPS", "Threat", "TPS");
            if (null != ml_Rot)
            {
                foreach (AbilityDK_Base ability in ml_Rot)
                {
                    szReport += string.Format(szFormat, ability.szName, ability.GetTotalDamage(), ability.GetDPS(), ability.GetTotalThreat(), ability.GetTPS());
                }
            }
            szReport += string.Format("Duration(sec): {0,6:0.0}\n", CurRotationDuration);
            szReport += string.Format("GCDs:          {0,6:0.0} ({1} ms) \n", m_GCDs, GCDTime);
            if (m_FreeRunesFromRE > 0)
                szReport += string.Format("Total RuneCD:  {0,6:0.0} *Including procs from RE\n", (float)m_TotalRuneCD / 1000f);
            else
                szReport += string.Format("Total RuneCD:  {0,6:0.0}\n", (float)m_TotalRuneCD / 1000f);
            szReport += string.Format("Blood:         {0,6} ({1} ms) \n", m_BloodRunes, m_SingleRuneCD * m_BloodRunes);
            szReport += string.Format("Frost:         {0,6} ({1} ms) \n", m_FrostRunes, m_SingleRuneCD * m_FrostRunes);
            szReport += string.Format("Unholy:        {0,6} ({1} ms) \n", m_UnholyRunes, m_SingleRuneCD * m_UnholyRunes);
            szReport += string.Format("Death:         {0,6} ({1} ms)\n", m_DeathRunes, m_SingleRuneCD * m_DeathRunes);
            szReport += string.Format("RP:            {0,6} *Neg value means RP left over\n", m_RunicPower);
            return szReport;
        }

        public List<AbilityDK_Base> GetFilteredListOfAbilities(int[] AvailableResources, CombatState CS)
        {
            return GetFilteredListOfAbilities(AvailableResources[(int)DKCostTypes.Blood],
                AvailableResources[(int)DKCostTypes.Frost],
                AvailableResources[(int)DKCostTypes.UnHoly],
                AvailableResources[(int)DKCostTypes.Death],
                CS);
        }

        /// <summary>
        /// Get a list of abilities that can be used w/ the available runes.
        /// </summary>
        /// <param name="iBlood"></param>
        /// <param name="iFrost"></param>
        /// <param name="iUnholy"></param>
        /// <param name="iDeath"></param>
        /// <returns>Filtered list of abilites that can be used w/ the given runes.  CombatState used to create each object needs to be replaced by current state at time of use.</returns>
        public List<AbilityDK_Base> GetFilteredListOfAbilities(int iBlood, int iFrost, int iUnholy, int iDeath, CombatState CS)
        {
            List<AbilityDK_Base> l_Abilities = new List<AbilityDK_Base>();
            #region Multi Runes
            if (iBlood > 0 && iFrost > 0
                || ((iFrost > 0 || iBlood > 0) && iDeath > 0)
                || (iDeath >= 2))
            {
                l_Abilities.Add(new AbilityDK_FesteringStrike(CS));
            }
            if (iFrost > 0 && iUnholy > 0
                || ((iFrost > 0 || iUnholy > 0) && iDeath > 0)
                || (iDeath >= 2))
            {
                l_Abilities.Add(new AbilityDK_DeathStrike(CS));
                l_Abilities.Add(new AbilityDK_Obliterate(CS));
            }
            #endregion
            #region Single Runes
            if (iBlood > 0 || iDeath > 0)
            {
                l_Abilities.Add(new AbilityDK_BloodStrike(CS));
                l_Abilities.Add(new AbilityDK_HeartStrike(CS));
                l_Abilities.Add(new AbilityDK_Pestilence(CS));
                l_Abilities.Add(new AbilityDK_BloodBoil(CS));
            }
            if (iFrost > 0 || iDeath > 0)
            {
                l_Abilities.Add(new AbilityDK_IcyTouch(CS));
                l_Abilities.Add(new AbilityDK_HowlingBlast(CS));
            }
            if (iUnholy > 0 || iDeath > 0)
            {
                l_Abilities.Add(new AbilityDK_PlagueStrike(CS));
                l_Abilities.Add(new AbilityDK_NecroticStrike(CS));
                l_Abilities.Add(new AbilityDK_DeathNDecay(CS));
                l_Abilities.Add(new AbilityDK_ScourgeStrike(CS));
            }
            #endregion

            return l_Abilities;
        }


        public int CountTrigger(Trigger t)
        {
            int iCount = 0;
            if (null != ml_Rot)
            {
                foreach (AbilityDK_Base ab in ml_Rot)
                {
                    switch (t)
                    {
                        case Trigger.BloodStrikeHit:
                            if (ab.AbilityIndex == (int)DKability.BloodStrike)
                                iCount++;
                            break;
                        case Trigger.DeathStrikeHit:
                            if (ab.AbilityIndex == (int)DKability.DeathStrike)
                                iCount++;
                            break;
                        case Trigger.FrostFeverHit:
                            if (ab.AbilityIndex == (int)DKability.FrostFever)
                                iCount++;
                            break;
                        case Trigger.HeartStrikeHit:
                            if (ab.AbilityIndex == (int)DKability.HeartStrike)
                                iCount++;
                            break;
                        case Trigger.IcyTouchHit:
                            if (ab.AbilityIndex == (int)DKability.IcyTouch)
                                iCount++;
                            break;
                        case Trigger.ObliterateHit:
                            if (ab.AbilityIndex == (int)DKability.Obliterate)
                                iCount++;
                            break;
                        case Trigger.PlagueStrikeHit:
                            if (ab.AbilityIndex == (int)DKability.PlagueStrike)
                                iCount++;
                            break;
                        case Trigger.RuneStrikeHit:
                            if (ab.AbilityIndex == (int)DKability.RuneStrike)
                                iCount++;
                            break;
                        case Trigger.ScourgeStrikeHit:
                            if (ab.AbilityIndex == (int)DKability.ScourgeStrike)
                                iCount++;
                            break;
                    }
                }
            }
            return iCount;
        }

        public bool HasTrigger(Trigger t)
        {
            bool bHasTrigger = false;
            if (null != ml_Rot)
            {
                foreach (AbilityDK_Base ab in ml_Rot)
                {
                    switch (t)
                    {
                        case Trigger.BloodStrikeHit:
                            bHasTrigger = (ab.AbilityIndex == (int)DKability.BloodStrike);
                            break;
                        case Trigger.DeathStrikeHit:
                            bHasTrigger = (ab.AbilityIndex == (int)DKability.DeathStrike);
                            break;
                        case Trigger.FrostFeverHit:
                            bHasTrigger = (ab.AbilityIndex == (int)DKability.FrostFever);
                            break;
                        case Trigger.HeartStrikeHit:
                            bHasTrigger = (ab.AbilityIndex == (int)DKability.HeartStrike);
                            break;
                        case Trigger.IcyTouchHit:
                            bHasTrigger = (ab.AbilityIndex == (int)DKability.IcyTouch);
                            break;
                        case Trigger.ObliterateHit:
                            bHasTrigger = (ab.AbilityIndex == (int)DKability.Obliterate);
                            break;
                        case Trigger.PlagueStrikeHit:
                            bHasTrigger = (ab.AbilityIndex == (int)DKability.PlagueStrike);
                            break;
                        case Trigger.RuneStrikeHit:
                            bHasTrigger = (ab.AbilityIndex == (int)DKability.RuneStrike);
                            break;
                        case Trigger.ScourgeStrikeHit:
                            bHasTrigger = (ab.AbilityIndex == (int)DKability.ScourgeStrike);
                            break;
                    }
                }
            }
            return bHasTrigger;
        }

        public bool Contains(DKability t)
        {
            bool bContains = false;
            if (null != ml_Rot)
            {
                foreach (AbilityDK_Base ab in ml_Rot)
                {
                    if (ab.AbilityIndex == (int)t)
                        bContains = true;
                }
            }
            return bContains;
        }

        public uint Count(DKability t)
        {
            uint uContains = 0;
            if (null != ml_Rot)
            {
                foreach (AbilityDK_Base ab in ml_Rot)
                {
                    if (ab.AbilityIndex == (int)t)
                        uContains++;
                }
            }
            return uContains;
        }
    }
}
