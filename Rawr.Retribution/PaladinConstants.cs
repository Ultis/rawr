
namespace Rawr.Retribution
{
    public class PaladinConstants
    {
        #region Consecration
        internal const float CONS_COOLDOWN = 30;
        internal const float CONS_BASE_DMG = 810;
        internal const float CONS_COEFF_AP = .26f;
        internal const float CONS_COEFF_SP = .26f;
        internal const float CONS_DURATION = 10;
        #endregion

        #region Crusader Strike
        internal const float CS_COOLDOWN = 4.5f;
        internal const float CS_DMG_BONUS = 1.35f;
        #endregion

        #region Devine Strike
        internal const float DS_COOLDOWN = 4.5f;
        internal const float DS_DMG_BONUS = 1f;
        #endregion

        #region Exorcism
        internal const float EXO_COEFF = .344f;
        internal const float EXO_AVG_DMG = 2741;
        internal const float EXO_PROC_CHANCE = .066666666666f;
        #endregion

        #region Hammer Of Wrath
        internal const float HOW_COOLDOWN = 6;
        internal const int HOW_AVG_DMG = 4015;
        internal const float HOW_COEFF_AP = .50f;
        internal const float HOW_COEFF_SP = .15f;
        #endregion

        #region Holy Wrath
        internal const float HOLY_WRATH_COOLDOWN = 15;
        internal const int HOLY_WRATH_BASE_DMG = 2402;
        internal const float HOLY_WRATH_COEFF = .61f;
        #endregion

        #region Judgement
        internal const float JUDGE_DMG = 1;
        internal const float JUDGE_COOLDOWN = 8;
        #endregion

        #region Seal Of Truth and Judgement Of Truth
        internal const float SOT_DMG = 0;
        internal const float SOT_SEAL_COEFF = .15F;
        internal const float SOT_CENSURE_COEFF_AP = .0192f;
        internal const float SOT_CENSURE_COEFF_SP = .01f;
        internal const float SOT_CENSURE_TICK = 3f;
        internal const float JOT_JUDGE_COEFF_AP = .1421f;
        internal const float JOT_JUDGE_COEFF_SP = .2229f;
        internal const float JOT_JUDGE_COEFF_STACK = .1f;
        #endregion

        #region Seal of Righteousness and Judgement of Righteousness
        internal const float SOR_COEFF_AP = .011f;
        internal const float SOR_COEFF_SP = .022f;
        internal const int SOR_ADDTARGET = 2;
        internal const float JOR_COEFF_AP = .02f;
        internal const float JOR_COEFF_SP = .032f;
        #endregion

        #region Seals of Command
        internal const float SOC_COEFF = .07f;
        #endregion

        #region Templars Verdict
        internal const float TV_ONE_STK = .3f;
        internal const float TV_TWO_STK = .9f;
        internal const float TV_THREE_STK = 2.35f;
        #endregion

        #region Talents
        internal const float HOL_COEFF = .021f;

        internal const float SHEATH_AP_COEFF = .3f;
        internal const float SHEATH_SPHIT_COEFF = .08f;

        internal const float TWO_H_SPEC = .2f;
        internal const float COMMUNION = .02f;

        internal const float DP_CHANCE = .075f;

        internal const float SEALS_OF_THE_PURE = .06f;
        internal const float ARBITER_OF_THE_LIGHT = .06f;
        internal const float INQUIRY_OF_FAITH_SEAL = .1f;
        internal const float INQUIRY_OF_FAITH_INQ = .5f;

        internal const float CRUSADE = .1f;
        internal const float RULE_OF_LAW = .05f;

        internal const float SANCTIFIED_WRATH = .2f;

        internal const float BLAZING_LIGHT = .1f;
        internal const float THE_ART_OF_WAR = 1f;
        #endregion

        #region Glyphs
        internal const float GLYPH_OF_EXORCISM = .2f;
        internal const float GLYPH_OF_SEAL_OF_TRUTH = 10f;
        internal const float GLYPH_OF_JUDGEMENT = .1f;
        internal const float GLYPH_OF_TEMPLARS_VERDICT = .15f;
        internal const float GLYPH_OF_CRUSADER_STRIKE = .05f;
        #endregion

        #region Mixed Abilities
        internal const float AW_COOLDOWN = 180;
        internal const float AW_DURATION = 20f;
        internal const float AW_DMG_BONUS = .2f;

        internal const float ZEAL_COOLDOWN = 120;
        internal const float ZEAL_DURATION = 20f;

        internal const float GOAK_COOLDOWN = 300;
        internal const float GOAK_DURATION = 30f;

        internal const float INQ_COEFF = .3f;
        #endregion
    }
}
