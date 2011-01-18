
namespace Rawr.Retribution
{
    public class PaladinConstants
    {
        #region Auto Attack
        internal const float AUTO_COOLDOWN = 0;
        #endregion

        #region Consecration
        internal const int CONS_ID = 38385;
        /// <summary>
        /// Consecration cooldown (sec)
        /// </summary>
        internal const float CONS_COOLDOWN = 30;
        /// <summary>
        /// Consecration base damage/second
        /// </summary>
        internal const float CONS_BASE_DMG = 810;
        internal const float CONS_COEFF = .26f;
        internal const float CONS_DEFAULT_DURATION = 10;
        #endregion

        #region Crusader Strike
        internal const int CS_ID = 35395;
        internal const float CS_COOLDOWN = 4.5f;
        internal const float CS_DMG_BONUS = 1.15f;
        internal const float CS_COEFF = 3.3f;
        #endregion

        #region Exorcism
        internal const int EXO_ID = 879;
        internal const float EXO_COOLDOWN = 0;
        internal const float EXO_DMG_BONUS = .344f;
        internal const float EXO_AVG_DMG = 2741;

        #endregion

        #region Hammer Of Wrath
        internal const int HOW_ID = 24275;
        internal const float HOW_COOLDOWN = 6;
        internal const int HOW_AVG_DMG = 4015;
        #endregion

        #region Holy Wrath
        /// <summary>
        /// Holy Wrath
        /// </summary>
        internal const int HOLY_WRATH_ID = 2812;
        /// <summary>
        /// Holy Wrath cooldown (sec)
        /// </summary>
        internal const float HOLY_WRATH_COOLDOWN = 16;
        /// <summary>
        /// Holy Wrath base damage
        /// </summary>
        internal const int HOLY_WRATH_BASE_DMG = 2435;
        /// <summary>
        /// used for calculating % of holy power applied
        /// </summary>
        internal const float HOLY_WRATH_COEFF = .61f;
        #endregion

        #region Seal Of Truth and Judgement Of Truth
        /// <summary>
        /// Seal Of Truth ID
        /// </summary>
        internal const int SOT_ID = 31801;
        internal const float SOT_DMG = 0;
        internal const int JOT_ID = 31804;
        #endregion

        #region Seal of Justice and Judgement of Justice
        internal const int SOJ_ID = 20164;
        internal const float SOJ_AP_COEFF = .005f;
        internal const float SOJ_HOLY_COEFF = .01f;
        #endregion

        #region Judgement Of Insight
        internal const int JOI_ID = 20165;
        internal const float JOI_AP_COEFF = .16f;
        internal const float JOI_HOLY_COEFF = .25f;
        #endregion

        #region Templars Verdict
        /// <summary>
        /// Templars Verdict
        /// </summary>
        internal const int TV_ID = 85256;
        internal const float TV_ONE_STK = .3f;
        internal const float TV_TWO_STK = .9f;
        internal const float TV_THREE_STK = 2.35f;

        #endregion

        #region Sheath of Light
        internal const float SHEATH_AP_COEFF = .3f;
        internal const float SHEATH_SPHIT_COEFF = .08f;
        #endregion

        #region Avenging Wrath
        internal const int AW_ID = 31884;
        internal const float AW_COOLDOWN = 180;
        internal const float AW_DURATION = 20f;
        internal const float AW_DMG_BONUS = .2f;
        #endregion

        #region Two Handed Weapon Specialization
        internal const float TWO_H_SPEC = .2f;
        #endregion

        internal const float JUDGE_COOLDOWN = 8;
    }
}
