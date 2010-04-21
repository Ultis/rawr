using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.TankDK
{
    enum RSState : int
    {
        Good = 0,
        RPStarved = 0x1,
        TimeStarved = 0x2,
    }

    class CombatTable2
    {
        #region Member Vars
        #region Basic setup variables
        private Character m_Char;
        private Stats m_Stats;
        private CharacterCalculationsTankDK m_Calcs;
        private CalculationOptionsTankDK m_Opts;
        #endregion

        // TODO: Find a way to balance 2h vs. DW.
        Weapon MH, OH;
        List<AbilityDK_Base> ml_Rot;

        #region Output Values
        private float _TotalDamage;
        public float TotalDamage
        {
            get
            {
                return _TotalDamage;
            }
        }
        private float _TotalThreat;
        public float TotalThreat
        {
            get
            {
                return _TotalThreat;
            }
        }

        public float m_TPS // readonly
        {
            get
            {
                return _TotalThreat / (m_RotationDuration / 1000);
            }
        }
        public float m_DPS // readonly
        {
            get
            {
                return _TotalDamage / (m_RotationDuration / 1000);
            }
        }

        private int[] _AbilityCost;
        public float m_RotationDuration // readonly
        {
            get 
            {
                // At the very least, the rotation duration is how long it takes to cast the 
                // Abilities selected in the rotations.
                int iRotDur = _AbilityCost[(int)DKCostTypes.CastTime] + (m_GCDs * 1500);
                // However, if the Cooldowns of the rotations take LONGER, 
                // then we need to extend the rotation Duration to be that length.
                iRotDur = Math.Max(iRotDur, _AbilityCost[(int)DKCostTypes.CooldownTime]);
                return iRotDur; 
            }
        }
        public float m_CooldownDuration // readonly
        {
            get { return _AbilityCost[(int)DKCostTypes.CooldownTime]; }
        }
        public float m_CastDuration // readonly
        {
            get { return _AbilityCost[(int)DKCostTypes.CastTime]; }
        }
        public float m_DurationDuration // readonly
        {
            get { return _AbilityCost[(int)DKCostTypes.DurationTime]; }
        }

        public int m_BloodRunes
        {
            get { return _AbilityCost[(int)DKCostTypes.Blood]; }
        }
        public int m_FrostRunes
        {
            get { return _AbilityCost[(int)DKCostTypes.Frost]; }
        }
        public int m_UnholyRunes
        {
            get { return _AbilityCost[(int)DKCostTypes.UnHoly]; }
        }
        public int m_DeathRunes
        {
            get { return _AbilityCost[(int)DKCostTypes.Death]; }
        }
        public int m_RunicPower
        {
            get { return _AbilityCost[(int)DKCostTypes.RunicPower]; }
        }
        private int _GCDs;
        public int m_GCDs
        {
            get { return _GCDs; }
        }

        public int m_iRSState = (int)RSState.Good;



        #endregion
        #endregion

        public CombatTable2(Character c, Stats stats, CharacterCalculationsTankDK calcs, CalculationOptionsTankDK calcOpts)
        {
            m_Char = c.Clone();
            m_Stats = stats.Clone();
            m_Calcs = calcs;
            m_Opts = calcOpts;

            //TODO: Handle Expertise
            if (c.MainHand != null && c.MainHand.Item.Type != ItemType.None)
            {
                MH = new Weapon(c.MainHand.Item, stats, calcOpts, 0);
                OH = null;
                if (c.MainHand.Slot != ItemSlot.TwoHand)
                {
                    if (c.OffHand != null && c.OffHand.Item.Type != ItemType.None)
                        OH = new Weapon(c.OffHand.Item, stats, calcOpts, 0);
                }
            }
            else
            {
                MH = null;
                OH = null;
            }

            // BuildRotation();

            // TODO: move this out of the constructor
            CompileRotation(calcOpts.m_Rotation);
        }

        /// <summary>
        /// Generate a rotation based on available resources.
        /// </summary>
        public void BuildRotation()
        {
            // TODO: need to setup a CombatState object. 
            // Setup an instance of each ability.
            // Single Runes:
            AbilityDK_IcyTouch IT = new AbilityDK_IcyTouch(m_Stats);
            AbilityDK_FrostFever FF = new AbilityDK_FrostFever(m_Stats, (uint)m_Char.DeathKnightTalents.Epidemic);
            AbilityDK_PlagueStrike PS = new AbilityDK_PlagueStrike(m_Stats, MH, OH, m_Char.DeathKnightTalents.ThreatOfThassarian);
            AbilityDK_BloodPlague BP = new AbilityDK_BloodPlague(m_Stats, (uint)m_Char.DeathKnightTalents.Epidemic);
            AbilityDK_BloodStrike BS = new AbilityDK_BloodStrike(m_Stats, MH, OH, m_Char.DeathKnightTalents.ThreatOfThassarian);
            AbilityDK_HeartStrike HS = new AbilityDK_HeartStrike(m_Stats, MH, OH);
            AbilityDK_Pestilence Pest = new AbilityDK_Pestilence(m_Stats);
            AbilityDK_BloodBoil BB = new AbilityDK_BloodBoil(m_Stats);
            // Multi Runes:
            AbilityDK_DeathStrike DS = new AbilityDK_DeathStrike(m_Stats, MH, OH, m_Char.DeathKnightTalents.ThreatOfThassarian);
            AbilityDK_HowlingBlast HB = new AbilityDK_HowlingBlast(m_Stats);
            AbilityDK_Obliterate OB = new AbilityDK_Obliterate(m_Stats, MH, OH, m_Char.DeathKnightTalents.ThreatOfThassarian);
            AbilityDK_ScourgeStrike SS = new AbilityDK_ScourgeStrike(m_Stats, MH, OH);
            AbilityDK_DeathNDecay DnD = new AbilityDK_DeathNDecay(m_Stats);

            // RP:
            AbilityDK_RuneStrike RS = new AbilityDK_RuneStrike(m_Stats, MH, OH, m_Char.DeathKnightTalents.ThreatOfThassarian);
            AbilityDK_DeathCoil DC = new AbilityDK_DeathCoil(m_Stats);
            AbilityDK_FrostStrike FS = new AbilityDK_FrostStrike(m_Stats, MH, OH, m_Char.DeathKnightTalents.ThreatOfThassarian);

        }

        /// <summary>
        /// Do the work for populating data based on the Rotation settings.
        /// </summary>
        public void CompileRotation(Rotation Rot)
        {
            // Limitation w/ current setup - can't have partial strikes
            int i = 0;
            _AbilityCost = new int[(int)DKCostTypes.NumCostTypes];
            // Build the rotation into a list of abilities to play with.
            ml_Rot = new List<AbilityDK_Base>();
            #region Runes
            #region Frost
            if (Rot.IcyTouch > 0)
            {
                for (i = (int)Rot.IcyTouch; i > 0; i--)
                {
                    ml_Rot.Add(new AbilityDK_IcyTouch(m_Stats));
                    ml_Rot.Add(new AbilityDK_FrostFever(m_Stats, (uint)m_Char.DeathKnightTalents.Epidemic));
                }
            }
            #endregion
            #region Unholy
            if (Rot.PlagueStrike > 0)
            {
                for (i = (int)Rot.PlagueStrike; i > 0; i--)
                {
                    ml_Rot.Add(new AbilityDK_PlagueStrike(m_Stats, MH, OH, m_Char.DeathKnightTalents.ThreatOfThassarian));
                    ml_Rot.Add(new AbilityDK_BloodPlague(m_Stats, (uint)m_Char.DeathKnightTalents.Epidemic));
                }
            }
            #endregion
            #region Blood
            if (Rot.BloodStrike > 0)
            {
                for (i = (int)Rot.BloodStrike; i > 0; i--)
                {
                    ml_Rot.Add(new AbilityDK_BloodStrike(m_Stats, MH, OH, m_Char.DeathKnightTalents.ThreatOfThassarian));
                    if (m_Char.DeathKnightTalents.BloodOfTheNorth > 0)
                    {
                        _AbilityCost[(int)DKCostTypes.Death] -= (m_Char.DeathKnightTalents.BloodOfTheNorth / 3);
                    }
                }
            }
            if (Rot.HeartStrike > 0)
            {
                if (m_Char.DeathKnightTalents.HeartStrike > 0)
                {
                    for (i = (int)Rot.HeartStrike; i > 0; i--)
                        ml_Rot.Add(new AbilityDK_HeartStrike(m_Stats, MH, OH));
                }
                else
                {
                    // Error
                    // Shot in rotation but not talented for.
                }
            }
            if (Rot.BloodBoil > 0)
            {
                for (i = (int)Rot.BloodBoil; i > 0; i--)
                    ml_Rot.Add(new AbilityDK_BloodBoil(m_Stats));
            }
            if (Rot.Pestilence > 0)
            {
                for (i = (int)Rot.Pestilence; i > 0; i--)
                {
                    ml_Rot.Add(new AbilityDK_Pestilence(m_Stats));
                    if (m_Char.DeathKnightTalents.BloodOfTheNorth > 0)
                    {
                        _AbilityCost[(int)DKCostTypes.Death] -= (m_Char.DeathKnightTalents.BloodOfTheNorth / 3);
                    }
                }
            }
            #endregion
            #region Frost+Unholy
            if (Rot.DeathStrike > 0)
            {
                for (i = (int)Rot.DeathStrike; i > 0; i--)
                {
                    ml_Rot.Add(new AbilityDK_DeathStrike(m_Stats, MH, OH, m_Char.DeathKnightTalents.ThreatOfThassarian));
                    if (m_Char.DeathKnightTalents.DeathRuneMastery > 0)
                    {
                        _AbilityCost[(int)DKCostTypes.Death] -= 2 * (m_Char.DeathKnightTalents.DeathRuneMastery / 3);
                    }
                }
            }
            if (Rot.HowlingBlast > 0)
            {
                for (i = (int)Rot.HowlingBlast; i > 0; i--)
                {
                    ml_Rot.Add(new AbilityDK_HowlingBlast(m_Stats));
                    if (m_Char.DeathKnightTalents.GlyphofHowlingBlast)
                    {
                        ml_Rot.Add(new AbilityDK_FrostFever(m_Stats, (uint)m_Char.DeathKnightTalents.Epidemic));
                    }
                }
            }
            if (Rot.Obliterate > 0)
            {
                for (i = (int)Rot.Obliterate; i > 0; i--)
                {
                    ml_Rot.Add(new AbilityDK_Obliterate(m_Stats, MH, OH, m_Char.DeathKnightTalents.ThreatOfThassarian));
                    if (m_Char.DeathKnightTalents.DeathRuneMastery > 0)
                    {
                        _AbilityCost[(int)DKCostTypes.Death] -= 2 * (m_Char.DeathKnightTalents.DeathRuneMastery / 3);
                    }
                    // Oblit will consume the diseases... so we need to cut their duration short.
                    if (m_Char.DeathKnightTalents.Annihilation < 3)
                    {
                        float percDuration = m_Char.DeathKnightTalents.Annihilation / 3;
                        foreach (AbilityDK_Base a in ml_Rot)
                        {
                            if (a.szName == "Blood Plague"
                                || a.szName == "Frost Fever"
                                || a.szName == "Crypt Fever"
                                || a.szName == "Ebon Plague")
                            {
                                a.AbilityCost[(int)DKCostTypes.DurationTime] = (int)(a.AbilityCost[(int)DKCostTypes.DurationTime] * percDuration);
                            }
                        }
                    }
                }
            }
            if (Rot.ScourgeStrike > 0)
            {
                if (m_Char.DeathKnightTalents.ScourgeStrike > 0)
                {
                    for (i = (int)Rot.ScourgeStrike; i > 0; i--)
                    {
                        ml_Rot.Add(new AbilityDK_ScourgeStrike(m_Stats, MH, OH));
                    }
                }
                else
                {
                    // Error
                    // Shot in rotation but not talented for.
                }

            }
            #endregion
            #region BloodFrostUnholy
            if (Rot.DeathNDecay > 0)
            {
                for (i = (int)Rot.DeathNDecay; i > 0; i--)
                    ml_Rot.Add(new AbilityDK_DeathNDecay(m_Stats));
            }
            #endregion
            #endregion

            #region RunicPower
            if (Rot.RuneStrike > 0)
            {
                for (i = (int)Rot.RuneStrike; i > 0; i--)
                    ml_Rot.Add(new AbilityDK_RuneStrike(m_Stats, MH, OH, m_Char.DeathKnightTalents.ThreatOfThassarian));
                // TODO: need to have these replace melee strikes.
            }
            if (Rot.DeathCoil > 0)
            {
                for (i = (int)Rot.DeathCoil; i > 0; i--)
                    ml_Rot.Add(new AbilityDK_DeathCoil(m_Stats));
            }
            if (Rot.FrostStrike > 0)
            {
                if (m_Char.DeathKnightTalents.FrostStrike > 0)
                {
                    for (i = (int)Rot.FrostStrike; i > 0; i--)
                        ml_Rot.Add(new AbilityDK_FrostStrike(m_Stats, MH, OH, m_Char.DeathKnightTalents.ThreatOfThassarian));
                }
                else
                {
                    // Error
                    // Shot in rotation, but not available via talents.
                }
            }
            #endregion

            // Now that the list is built, setup the costs so that we know what the rotation looks like in runes.
            #region Sum of Costs
            _GCDs = 0;
            foreach (AbilityDK_Base ability in ml_Rot)
            {
                for (int cost = 0; cost < (int)DKCostTypes.NumCostTypes; cost++ )
                {
                    _AbilityCost[cost] += ability.AbilityCost[cost];
                }
                // Setup the total GCD count
                if (ability.bTriggersGCD) 
                    ++_GCDs;
            }
            #endregion
            // TODO: Handle Disease/DoT Clipping
            
            // Now we have the total costs
            // Need to figure out how to factor in Death Runes
            SpendDeathRunes(_AbilityCost, 0);

            // Let's evaluate the rune cooldown cost to the GCD cost:
            // 2 runes every 10 secs 
            // in ms
            int BloodCD = _AbilityCost[(int)DKCostTypes.Blood] * 10 * 1000 / 2;
            int FrostCD = _AbilityCost[(int)DKCostTypes.Frost] * 10 * 1000 / 2;
            int UnHolyCD = _AbilityCost[(int)DKCostTypes.UnHoly] * 10 * 1000 / 2;
            int DeathCD = Math.Abs(_AbilityCost[(int)DKCostTypes.Death]) * 10 * 1000 / 2; // Assuming all DeathRune possible generating strikes actually generate death runes.

            int maxCD = Math.Max(BloodCD, FrostCD);
            maxCD = Math.Max(maxCD, UnHolyCD);
            maxCD = Math.Max(maxCD, DeathCD);

            // This sums up all Cooldowntime
            // Not really the best way of handling this since cooldowns can overlap.
            // TODO: Fix cooldown math.
            _AbilityCost[(int)DKCostTypes.CooldownTime] = Math.Max(maxCD, _AbilityCost[(int)DKCostTypes.CooldownTime]);
            

            #region White Swings
            float fWhiteSwings = 0;
            if (MH != null && MH.hastedSpeed != 0)
            {
                fWhiteSwings = (float)m_RotationDuration / (int)(MH.hastedSpeed * 1000);
            }
            // This will provide the opportunity to increase available RP by swings due to Talents.
            // Also get a swing count to balance out the RuneStrikes.
            bool bAvailableWhiteSwings = false;
            float fWhiteDamage = 0;
            if (fWhiteSwings < Rot.RuneStrike)
            {
                // There's not enough time to get in all the RS's we have slated for
                bAvailableWhiteSwings = false;
                m_iRSState |= (int)RSState.TimeStarved;
            }
            else
            {
                // Since there are spare white swings, let's figure out how much white damage is done
                bAvailableWhiteSwings = true;
                fWhiteSwings = (fWhiteSwings - Rot.RuneStrike);
                fWhiteDamage = (MH.damage * fWhiteSwings);
                if (null != OH)
                    fWhiteDamage += (OH.damage * fWhiteSwings);
            }

            #endregion

            bool bSpareGCDs = false;
            bool bAvailableRP = false;
            if (maxCD > _AbilityCost[(int)DKCostTypes.CooldownTime])
            {
                // We have spare GCDs waiting for Runes to come back.
                bSpareGCDs = true;
            }

            // Are we starving the RSs?
            // We may be wanting to save RP for Mind Freeze or something.
            if (_AbilityCost[(int)DKCostTypes.RunicPower] > 0)
            {
                // starving our RP based abilities in the rotation.
                bAvailableRP = false;
                // We need to adjust the number of RSs due to starvation.
                m_iRSState |= (int)RSState.RPStarved;
            }
            else
            {
                bAvailableRP = true;
            }

            #region Sum of DPS & Threat
            this._TotalDamage = 0;
            this._TotalThreat = 0;
            _TotalDamage += fWhiteDamage;
            _TotalThreat += fWhiteDamage * AbilityDK_Base.THREAT_FROST_PRESENCE;

            foreach (AbilityDK_Base ability in ml_Rot)
            {
                this._TotalDamage += ability.GetTotalDamage();
                this._TotalThreat += ability.GetTotalThreat();
            }
            #endregion


            // I need a way of setting up the rotation in such a way that I can get a state of the target at the time
            // the ability is used.  
        }

        /// <summary>
        /// Pass in the AblityCost[] to process the DeathRunes
        /// </summary>
        /// <param name="AbilityCost"></param>
        public void SpendDeathRunes(int[] AbilityCost, int DRSpent)
        {
            // Need to figure out how to factor in Death Runes
            // Since each death rune replaces any other rune on the rotation,
            // for each death rune, cut the cost of the highest other rune by 1.
            // Do not run this if there are no DeathRunes to spend.
            if (Math.Abs(AbilityCost[(int)DKCostTypes.Death]) > DRSpent)
            {
                int iHighestCostAbilityIndex = 0;
                int iPreviousCostValue = 0;
                for (int t = 0; t < (int)DKCostTypes.Death; t++)
                {
                    // Is the cost higher than our previous checked value.
                    if (AbilityCost[t] > iPreviousCostValue)
                    {
                        // If so, save off the index of that ability.
                        iHighestCostAbilityIndex = t;
                    }
                    iPreviousCostValue = AbilityCost[t];
                }
                // After going through the full list, spend a death rune and 
                // then iterate through that list again. 
                AbilityCost[iHighestCostAbilityIndex] -= 1;
                // increment the death runes.
                DRSpent++;
                SpendDeathRunes(AbilityCost, DRSpent);
            }
        }
    }
}
