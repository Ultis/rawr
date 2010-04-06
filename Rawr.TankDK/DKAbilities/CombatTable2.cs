using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.TankDK
{
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
        private float _TPS;
        public float m_TPS // readonly
        {
            get
            {
                return _TPS;
            }
        }
        private float _DPS;
        public float m_DPS // readonly
        {
            get
            {
                return _DPS;
            }
        }
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
                if (c.MainHand.Item.Type == ItemType.OneHandAxe ||
                    c.MainHand.Item.Type == ItemType.OneHandMace ||
                    c.MainHand.Item.Type == ItemType.OneHandSword)
                {
                    if (c.OffHand != null && c.OffHand.Item.Type != ItemType.None)
                        OH = new Weapon(c.OffHand.Item, stats, calcOpts, 0);
                }
            }
            else
                MH = null;

            CompileRotation(calcOpts.m_Rotation);
        }

        /// <summary>
        /// Do the work for populating data based on the Rotation settings.
        /// </summary>
        public void CompileRotation(Rotation Rot)
        {
            int i = 0;
            // Build the rotation into a list of abilities to play with.
            ml_Rot = new List<AbilityDK_Base>();
            #region Runes
            #region Frost
            if (Rot.IcyTouch > 0)
            {
                for (i = (int)Rot.IcyTouch; i > 0; i--)
                    ml_Rot.Add(new AbilityDK_IcyTouch(m_Stats));
            }
            #endregion
            #region Unholy
            if (Rot.PlagueStrike > 0)
            {
                for (i = (int)Rot.PlagueStrike; i > 0; i--)
                    ml_Rot.Add(new AbilityDK_PlagueStrike(m_Stats, MH, OH));
            }
            #endregion
            #region Blood
            if (Rot.BloodStrike > 0)
            {
                for (i = (int)Rot.BloodStrike; i > 0; i--)
                    ml_Rot.Add(new AbilityDK_BloodStrike(m_Stats, MH, OH));
            }
            if (Rot.HeartStrike > 0)
            {
                for (i = (int)Rot.HeartStrike; i > 0; i--)
                    ml_Rot.Add(new AbilityDK_HeartStrike(m_Stats, MH, OH));
            }
            if (Rot.BloodBoil > 0)
            {
                // TODO: Add BB ability.
            }
            if (Rot.Pestilence > 0)
            {
                // TODO: Add Pest ability.
            }
            #endregion
            #region Frost+Unholy
            if (Rot.DeathStrike > 0)
            {
                for (i = (int)Rot.DeathStrike; i > 0; i--)
                    ml_Rot.Add(new AbilityDK_DeathStrike(m_Stats, MH, OH));
            }
            if (Rot.Obliterate > 0)
            {
                for (i = (int)Rot.Obliterate; i > 0; i--)
                    ml_Rot.Add(new AbilityDK_Obliterate(m_Stats, MH, OH));
            }
            if (Rot.HowlingBlast > 0)
            {
                for (i = (int)Rot.HowlingBlast; i > 0; i--)
                    ml_Rot.Add(new AbilityDK_HowlingBlast(m_Stats));
            }
            #endregion
            #region BloodFrostUnholy
            if (Rot.DeathNDecay > 0)
            {
                // TODO: Add DND
            }
            #endregion
            #endregion

            #region RunicPower
            if (Rot.RuneStrike > 0)
            {
                // TODO: Add Rune Strike
            }
            if (Rot.DeathCoil > 0)
            {
                for(i = (int)Rot.DeathCoil; i == 0; i--)
                    ml_Rot.Add(new AbilityDK_DeathCoil(m_Stats));
            }
            if (Rot.FrostStrike > 0)
            {
                // TODO: add Frost Strike.
            }
            #endregion

            // Now that the list is built, setup the costs so that we know what the rotation looks like in runes.
            #region Sum of Costs
            int[] AbilityCost = new int[(int)DKCostTypes.NumCostTypes];
            foreach (AbilityDK_Base ability in ml_Rot)
            {
                for (int cost = 0; cost < (int)DKCostTypes.NumCostTypes; cost++ )
                {
                    AbilityCost[cost] += ability.AbilityCost[cost];
                }
            }
            #endregion

            // Now we have the total costs
            // Let's evaluate the rune cost to the GCD cost:
            // 2 runes every 10 secs (11.5 secs with a 2 sec forgiveness)
            // in ms
            int BloodCD = AbilityCost[(int)DKCostTypes.Blood] * 10 * 1000 / 2;
            int FrostCD = AbilityCost[(int)DKCostTypes.Frost] * 10 * 1000 / 2;
            int UnHolyCD = AbilityCost[(int)DKCostTypes.UnHoly] * 10 * 1000 / 2;

            int maxCD = Math.Max(BloodCD, FrostCD);
            maxCD = Math.Max(maxCD, UnHolyCD);
            if (maxCD > AbilityCost[(int)DKCostTypes.DurationTime])
            {
                // We have spare GCDs waiting for Runes to come back.
            }

            // Are we starving the RSs?
            if (AbilityCost[(int)DKCostTypes.RunicPower] > 0)
            {
                // starving our RP based abilities in the rotation.
            }

            // I need a way of setting up the rotation in such a way that I can get a state of the target at the time
            // the ability is used.  
        }


    }
}
