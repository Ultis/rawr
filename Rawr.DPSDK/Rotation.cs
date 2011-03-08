using System;
using System.Collections.Generic;
using System.Text;

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
        #endregion

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
            const int indexFrost = 20; // start index of Frost Talents.
            const int indexUnholy = indexFrost + 20; // start index of Unholy Talents.
            int[] TalentCounter = new int[4];
            int index = indexBlood;
            foreach (int i in t.Data)
            {
                // Blood
                if (index < indexFrost)
                    TalentCounter[(int)Rotation.Type.Blood]+= i;
                // Frost
                else if ((indexFrost <= index) && (index < indexUnholy))
                {
                    TalentCounter[(int)Rotation.Type.Frost]+= i;
                }
                // Unholy
                else if (index >= indexUnholy)
                {
                    TalentCounter[(int)Rotation.Type.Unholy]+= i;
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
        public int m_SingleRuneCD 
        { 
            get
            { 
                return (int)((15 / 2) * (1000 / (1f + m_CT.m_CState.m_Stats.PhysicalHaste + m_CT.m_CState.m_Stats.BonusRuneRegeneration)));
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
                if (m_CT.m_CState.m_Talents.RunicCorruption > 0)
                    return 0;
                else
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
            ResetRotation();
            // Let's do some basic rune tracking internal to the function before doing the heavy cost building.
            int[] abCost = new int[EnumHelper.GetCount(typeof(DKCostTypes))];

            AbilityDK_IcyTouch IT = new AbilityDK_IcyTouch(m_CT.m_CState);
            AbilityDK_PlagueStrike PS = new AbilityDK_PlagueStrike(m_CT.m_CState);
            // RP:  Unlikely to start w/ RP abilities to open.
            AbilityDK_RuneStrike RS = new AbilityDK_RuneStrike(m_CT.m_CState);
            AbilityDK_DeathCoil DC = new AbilityDK_DeathCoil(m_CT.m_CState);
            AbilityDK_FrostStrike FS = new AbilityDK_FrostStrike(m_CT.m_CState);
            AbilityDK_DancingRuneWeapon DRW = new AbilityDK_DancingRuneWeapon(m_CT.m_CState);
            // Setup our working lists:
            List<AbilityDK_Base> l_Openning = new List<AbilityDK_Base>();

            if (m_bThreat)
            {
                l_Openning.Sort(AbilityDK_Base.CompareThreatByRunes);
            }
            else
            {
                l_Openning.Sort(AbilityDK_Base.CompareDamageByRunes);
            }

            // Sorted by DPS or TPS per rune.  So if any single rune ability will do
            // More for that single rune than a multi-rune ability, we'll use that.
            // Let's actually open w/ IT & PS
            // then move on to 2ndary abilities.

            ml_Rot.Add(IT); // F
            ml_Rot.Add(IT.ml_TriggeredAbility[0]);
            ml_Rot.Add(PS); // U
            ml_Rot.Add(PS.ml_TriggeredAbility[0]);

            abCost[(int)DKCostTypes.Blood] = 2;
            abCost[(int)DKCostTypes.Frost] = 1;
            abCost[(int)DKCostTypes.UnHoly] = 1;
            abCost[(int)DKCostTypes.Death] = 0;
            abCost[(int)DKCostTypes.RunicPower] = 0;

            if ((GetRotationType(m_CT.m_CState.m_Talents) == Type.Blood) 
                && (m_CT.m_CState.m_Talents.ScarletFever > 0))
            {
                ml_Rot.Add(new AbilityDK_BloodBoil(m_CT.m_CState));
                abCost[(int)DKCostTypes.Blood] -= 1;
            }

            List<AbilityDK_Base> RPList = new List<AbilityDK_Base>();
            int MaxIterations = 2;
            for (int i = 0; i < MaxIterations; i++)
            {
                // Do this until we're out of runes.
                while (abCost[(int)DKCostTypes.Blood] != 0
                    || abCost[(int)DKCostTypes.Frost] != 0
                    || abCost[(int)DKCostTypes.UnHoly] != 0)
                {
                    // TODO: Integrate Ability CD & durations.
                    RPList = GetFilteredListOfAbilities(abCost[(int)DKCostTypes.Blood], abCost[(int)DKCostTypes.Frost], abCost[(int)DKCostTypes.UnHoly], 0);
                    foreach (AbilityDK_Base ab in RPList)
                    {
                        ab.UpdateCombatState(m_CT.m_CState);
                    }
                    if (m_bThreat)
                        RPList.Sort(AbilityDK_Base.CompareTPSByRunes);
                    else
                        RPList.Sort(AbilityDK_Base.CompareDPSByRunes);
                    ml_Rot.Add(RPList[0]);
                    abCost[(int)DKCostTypes.Blood] -= RPList[0].AbilityCost[(int)DKCostTypes.Blood];
                    abCost[(int)DKCostTypes.Frost] -= RPList[0].AbilityCost[(int)DKCostTypes.Frost];
                    abCost[(int)DKCostTypes.UnHoly] -= RPList[0].AbilityCost[(int)DKCostTypes.UnHoly];
                    abCost[(int)DKCostTypes.Death] -= RPList[0].AbilityCost[(int)DKCostTypes.Death];
                    abCost[(int)DKCostTypes.RunicPower] -= RPList[0].AbilityCost[(int)DKCostTypes.RunicPower];
                }
                // reset runes for multiple passes:
                abCost[(int)DKCostTypes.Blood] = 2;
                abCost[(int)DKCostTypes.Frost] = 2;
                abCost[(int)DKCostTypes.UnHoly] = 2;
            }

            #region Runic Power
            // How much RP do we have at this point?
            foreach (AbilityDK_Base ab in ml_Rot)
                m_RunicPower += ab.RunicPower;
            m_RunicPower = (int)((float)m_RunicPower);
            if (m_CT.m_CState.m_Talents.Butchery > 0)
                m_RunicPower -= (int)((CurRotationDuration / 5) * m_CT.m_CState.m_Stats.RPp5);

            // Which RP ability should we use?
            RPList = new List<AbilityDK_Base>();
            if (GetRotationType(m_CT.m_CState.m_Talents) == Type.Blood && m_CT.m_CState.m_Presence == Presence.Blood)
                // We can only RS at will when in blood presence & we'll say for the blood spec as well.
                RPList.Add(RS);
            RPList.Add(DC);
            RPList.Add(FS);

            if (m_bThreat)
                RPList.Sort(AbilityDK_Base.CompareThreatByRP);
            else
                RPList.Sort(AbilityDK_Base.CompareDamageByRP);
            // we need to add DRW if DRW is being factored in
            ml_Rot.Add(DRW);
            m_RunicPower += DRW.RunicPower;
            // Burn the remainder.
            for (int RPAbCount = Math.Abs(m_RunicPower / RPList[0].RunicPower); RPAbCount > 0; RPAbCount--)
            {
                ml_Rot.Add(RPList[0]);
                m_RunicPower += RPList[0].RunicPower;
            }
            #endregion

            BuildCosts();

            ReportRotation(l_Openning);
        }

        /// <summary>
        /// Get a list of abilities that can be used w/ the available runes.
        /// </summary>
        /// <param name="iBlood"></param>
        /// <param name="iFrost"></param>
        /// <param name="iUnholy"></param>
        /// <param name="iDeath"></param>
        /// <returns>Filtered list of abilites that can be used w/ the given runes.  CombatState used to create each object needs to be replaced by current state at time of use.</returns>
        public List<AbilityDK_Base> GetFilteredListOfAbilities(int iBlood, int iFrost, int iUnholy, int iDeath)
        {
            CombatState CS = new CombatState();
            CS.m_Talents = new DeathKnightTalents();
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

        #region Preset Rotation
        /// <summary>
        /// This is my fire 1 of everything rotation.
        /// </summary>
        public void PRE_OneEachRot()
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

            BuildCosts();
        }

        /// <summary>
        /// This a basic IT, PS, HSx4 (or BBx4), DSx3, RSx? rotation.
        /// </summary>
        public void PRE_BloodDiseased()
        {
            ResetRotation();
            // This will only happen while tanking...
            m_CT.m_Opts.presence = Presence.Blood;
            // Setup an instance of each ability.
            // No runes:
            //            AbilityDK_Outbreak OutB = new AbilityDK_Outbreak(m_CT.m_CState);
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
                ml_Rot.Add(IT);
                ml_Rot.Add(FF);
                ml_Rot.Add(PS);
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


            // How much RP do we have at this point?

            foreach (AbilityDK_Base ab in ml_Rot)
                m_RunicPower += ab.RunicPower;
            m_RunicPower = (int)((float)m_RunicPower);
            if (m_CT.m_CState.m_Talents.Butchery > 0)
                m_RunicPower -= (int)((CurRotationDuration / 5) * m_CT.m_CState.m_Stats.RPp5);

            // Burn what we can.
            for (int RSCount = Math.Abs(m_RunicPower / RS.RunicPower); RSCount > 0; RSCount--)
            {
                ml_Rot.Add(RS);
                m_RunicPower += RS.RunicPower;
            }

            BuildCosts();
        }

        /// <summary>
        /// This a basic IT, PS, HSx4 (or BBx4), DSx3, RSx? rotation.
        /// </summary>
        public void PRE_Frost()
        {
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
            ml_Rot.Add(IT);
            ml_Rot.Add(FF);
            ml_Rot.Add(PS);
            ml_Rot.Add(BP);

            ml_Rot.Add(BS);
            ml_Rot.Add(BS);

            // These will create DeathRunes that can allow flexibility in the rotation later on.
            ml_Rot.Add(OB);

            // ml_Rot.Add(HB); // Assuming the free HB due to proc.
            ml_Rot.Add(OB);
            ml_Rot.Add(OB);
            ml_Rot.Add(OB);


            // How much RP do we have at this point?
            foreach (AbilityDK_Base ab in ml_Rot)
                m_RunicPower += ab.RunicPower;
            m_RunicPower = (int)((float)m_RunicPower);
            if (m_CT.m_CState.m_Talents.Butchery > 0)
                m_RunicPower -= (int)((CurRotationDuration / 5) * m_CT.m_CState.m_Stats.RPp5);

            // Burn what we can.
            for (int RSCount = Math.Abs(m_RunicPower / FS.RunicPower); RSCount > 0; RSCount--)
            {
                ml_Rot.Add(FS);
                m_RunicPower += FS.RunicPower;
            }

            BuildCosts();
        }
        
        // TODO: Expand this to the 3 min CD duration of Dark Transformation.
        /// <summary>
        /// This a basic rotation
        /// </summary>
        public void PRE_Unholy()
        {
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
            // AbilityDK_HowlingBlast HB = new AbilityDK_HowlingBlast(m_CT.m_CState);
            AbilityDK_ScourgeStrike SS = new AbilityDK_ScourgeStrike(m_CT.m_CState);
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
            // AbilityDK_FrostStrike FS = new AbilityDK_FrostStrike(m_CT.m_CState);
            DarkTranformation Dark = new DarkTranformation();

            // Simple ITx1, PSx1, BSx1, SSx4 & Festx4.
            // Initial rotation build.
            ml_Rot.Add(Dark); // Dark Transformation.
            int curRP = Dark.RunicPower;

            uint subrotDuration = 0;
            uint GCDdur = MIN_GCD_MS;
            // Fill the 3 mins duration 
            if (m_CT.m_Opts.presence == Presence.Unholy)
                GCDdur = MIN_GCD_MS_UH;

            subrotDuration = (FF.uDuration);
            if (m_CT.m_CState.m_Talents.Butchery > 0)
                curRP -= (int)((subrotDuration / 5000) * m_CT.m_CState.m_Stats.RPp5);
            uint diseaseGCDs = 0;
            for (int count = (int)(Dark.Cooldown / subrotDuration); count > 0; count--)
            {
                // TODO: This still assumes that we're filling every GCD w/ an ability. 
                // We know that's not the case in most situations.
                ml_Rot.Add(IT);
                curRP += IT.RunicPower;
                ml_Rot.Add(FF);
                ml_Rot.Add(PS);
                curRP += PS.RunicPower;
                ml_Rot.Add(BP);
                ml_Rot.Add(BS);
                curRP += BS.RunicPower;
                // 3 GCDs

                // Fill the disease.
                diseaseGCDs = (FF.uDuration - (2 * GCDdur)) / GCDdur;
                diseaseGCDs += (uint)count % 2; // To deal w/ the floating rune from a previous rotation.
                int runeAbilityCount = 0;
                for (; diseaseGCDs > 0; )
                {
                    if ((-1 * curRP) < DC.RunicPower // If there isn't enough RP,
                        || (SS.GetTotalDamage() > DC.GetTotalDamage() // or if SS will do more damage than DC
                        && curRP + SS.RunicPower < 100 + m_CT.m_CState.m_Stats.BonusMaxRunicPower) // and make sure we don't overcap RP
                        || (Fest.GetTotalDamage() > DC.GetTotalDamage() // or if Fest will do more damage than DC
                        && curRP + Fest.RunicPower < 100 + m_CT.m_CState.m_Stats.BonusMaxRunicPower)) // make sure we don't over cap.
                    {
                        if (runeAbilityCount % 2 == 0)
                        {
                            ml_Rot.Add(SS);
                            curRP += SS.RunicPower;
                            diseaseGCDs--;
                            runeAbilityCount++;
                        }
                        else
                        {
                            ml_Rot.Add(Fest);
                            curRP += Fest.RunicPower;
                            diseaseGCDs--;
                            runeAbilityCount++;
                        }
                    }
                    else
                    {
                        ml_Rot.Add(DC);
                        curRP += DC.RunicPower;
                        diseaseGCDs--;
                    }
                }
            }


            // subsequent:


            // How much RP do we have at this point?
            foreach (AbilityDK_Base ab in ml_Rot)
                m_RunicPower += ab.RunicPower;


            BuildCosts();
        }

        #endregion

        public void BuildCosts()
        {
            int totalRuneCount = 0;
            // Now we have the list of abilities sorted appropriately.
            foreach (AbilityDK_Base ability in ml_Rot)
            {
                // Populate the costs here.
                // GCD count.
                if (ability.bTriggersGCD)
                    m_GCDs++;

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
            totalRuneCount = m_BloodRunes;
            totalRuneCount += m_FrostRunes;
            totalRuneCount += m_UnholyRunes;

            // spend the death runes available:
            int[] abCost = new int[EnumHelper.GetCount(typeof(DKCostTypes))];
            abCost[(int)DKCostTypes.Blood] = m_BloodRunes;
            int BRCD = m_BloodRunes * m_SingleRuneCD;
            abCost[(int)DKCostTypes.Frost] = m_FrostRunes;
            int FRCD = m_FrostRunes * m_SingleRuneCD;
            abCost[(int)DKCostTypes.UnHoly] = m_UnholyRunes;
            int URCD = m_UnholyRunes * m_SingleRuneCD;
            abCost[(int)DKCostTypes.Death] = m_DeathRunes;
            int hiRuneIndex = DKCombatTable.GetHighestRuneCountIndex(abCost);
            int DeathRunesSpent = 0;
            DKCombatTable.SpendDeathRunes(abCost, DeathRunesSpent);
            m_DeathRunes = DeathRunesSpent;
            //What about multi-rune abilities?
            m_TotalRuneCD = Math.Max(Math.Max(BRCD, FRCD), URCD); // Max CD of the runes.  TODO: Death runes aren't factored in just yet.
            // Ensure that m_TotalRuneCD != 0

            // Runic Corruption:
            // For each death coil, improve the Rune Regen by 50% per point for 3 sec.
            uint RCRegenDur = 0;
            float RCHaste = 0;
            if (m_CT.m_CState.m_Talents.RunicCorruption > 0)
            {
                RCRegenDur = Count(DKability.DeathCoil) * 3 * 1000;
                RCHaste = (m_CT.m_CState.m_Talents.RunicCorruption * .5f);
            }
            float RCperc = 0;
            if (m_TotalRuneCD > 0)
                RCperc = (RCRegenDur / m_TotalRuneCD) * RCHaste;
            m_TotalRuneCD = (int)(m_TotalRuneCD / (1 + RCperc));

            // Add White damage
            int iWhiteCount = (int)(CurRotationDuration / m_CT.combinedSwingTime);
            AbilityDK_WhiteSwing WS = new AbilityDK_WhiteSwing(m_CT.m_CState);
            for (int i = 0; i < iWhiteCount; i++)
            {
                ml_Rot.Add(WS);
                TotalDamage += WS.TotalDamage;
                TotalThreat += WS.TotalThreat;
            }

        }

        public string ReportRotation(List<AbilityDK_Base> l_Openning)
        {
            string szReport = "";
            float DurationDuration = 0;
            string szFormat = "{0,-15}|{1,7}|{2,7:0.0}|{3,7:0}|{4,7:0.0}\n";

            szReport += string.Format(szFormat, "Name", "Damage", "DPS", "Threat", "TPS");
            foreach (AbilityDK_Base ability in l_Openning)
            {
                DurationDuration += (float)ability.uDuration;
                szReport += string.Format(szFormat, ability.szName, ability.GetTotalDamage(), ability.GetDPS(), ability.GetTotalThreat(), ability.GetTPS());
            }

            szReport += string.Format("Duration(sec): {0,6:0.0}\n", CurRotationDuration);
            szReport += string.Format("GCDs:          {0,6}\n", m_GCDs);

            return szReport;
        }

        public string ReportRotation()
        {
            string szReport = "";
            string szFormat = "{0,-15}|{1,7}|{2,7:0.0}|{3,7:0}|{4,7:0.0}\n";

            szReport += string.Format(szFormat, "Name", "Damage", "DPS", "Threat", "TPS");
            foreach (AbilityDK_Base ability in ml_Rot)
            {
                szReport += string.Format(szFormat, ability.szName, ability.GetTotalDamage(), ability.GetDPS(), ability.GetTotalThreat(), ability.GetTPS());
            }
            szReport += string.Format("Duration(sec): {0,6}\n", CurRotationDuration);
            szReport += string.Format("GCDs:          {0,6}\n", m_GCDs);
            return szReport;
        }

        public int CountTrigger(Trigger t)
        {
            int iCount = 0;
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

            return iCount;
        }

        public bool HasTrigger(Trigger t)
        {
            bool bHasTrigger = false;
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
            return bHasTrigger;
        }

        public bool Contains(DKability t)
        {
            bool bContains = false;
            foreach (AbilityDK_Base ab in ml_Rot)
            {
                if (ab.AbilityIndex == (int)t) 
                    bContains = true;
            }
            return bContains;
        }

        public uint Count(DKability t)
        {
            uint uContains = 0;
            foreach (AbilityDK_Base ab in ml_Rot)
            {
                if (ab.AbilityIndex == (int)t)
                    uContains++;
            }
            return uContains;
        }
    }
}
