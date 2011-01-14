
namespace Rawr.Retribution
{
    public class PaladinConstants
    {
        #region Damage Base, Coefficients and bonuses

        /* DMG consts are additive */
        /// <summary>
        /// Average of base damage from exorcism
        /// </summary>
        internal const float CONS_DMG = 750;
        /* BONUS consts are multiplicative */
        /// <summary>
        /// Base damage bonus from consecration
        /// </summary>
        internal const float CS_DMG_BONUS = 1.15f;
        /// <summary>
        /// damage bonus coefficient added directly to exorcism strike
        /// </summary>
        internal const float EXO_DMG_BONUS = .344f;
        /// <summary>
        /// damage bonus coefficient for Templars Verdict
        /// </summary>
        internal const float TV_DMG_BONUS = 2.35f;
        #endregion

        #region Base cooldown duration Values
        /// <summary>
        /// Crusader Strike cooldown (sec)
        /// </summary>
        internal const float CS_COOLDOWN = 4.5f;
        /// <summary>
        /// Consecration cooldown (sec)
        /// </summary>
        internal const float CONS_COOLDOWN = 0;
        /// <summary>
        /// Exorcism cooldown (sec)
        /// </summary>
        internal const float EXO_COOLDOWN = 0;
        /// <summary>
        /// Hammer of Wrath cooldown (sec)
        /// </summary>
        internal const float HOW_COOLDOWN = 6;
        /// <summary>
        /// Auto-Attack cooldown (sec)
        /// </summary>
        internal const float AUTO_COOLDOWN = 0;
        /// <summary>
        /// Holy Wrath cooldown (sec)
        /// </summary>
        internal const float HW_COOLDOWN = 16;
        #endregion

        /// <summary>
        /// Spell ID's assigned by Blizzard
        /// </summary>
        public enum SpellIds
        {
            /// <summary>
            /// Auto attack
            /// </summary>
            AUTO_ID,
            /// <summary>
            /// Crusader strike
            /// </summary>
            CS_ID = 35395,
            /// <summary>
            /// Exorcism
            /// </summary>
            EXO_ID = 879,
            /// <summary>
            /// Seal Of Truth
            /// </summary>
            SOT_ID,
            /// <summary>
            /// Templars Verdict
            /// </summary>
            TV_ID = 63867,
            /// <summary>
            /// Seal of Righteousness
            /// </summary>
            SOR_ID = 20154,
            /// <summary>
            /// Hammer Of Wrath
            /// </summary>
            HOW_ID = 24275,
            /// <summary>
            /// Holy Wrath
            /// </summary>
            HW_ID = 2812,
            CONS_ID = 38385
        }
    }
}
