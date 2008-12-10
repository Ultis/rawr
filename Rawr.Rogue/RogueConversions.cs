namespace Rawr.Rogue
{
    public class RogueConversions
    {
        public static readonly float AgilityToAP = 1.0f;
        public static readonly float AgilityToArmor = 2.0f;
        public static readonly float AgilityToCrit = 1.0f/40.0f;
        public static readonly float AgilityToDodge = 1.0f/20.0f;
        public static readonly float CritRatingToCrit = 1.0f/22.0769f; //14*82/52
        public static readonly float CritToCritRating = 22.0769f; //14*82/52
        public static readonly float ExpertiseRatingToExpertise = 1.0f/3.9423f;
        public static readonly float ExpertiseToDodgeParryReduction = 0.25f;
        public static readonly float HasteRatingToHaste = 1.0f/15.77f;
        public static readonly float HitRatingToHit = 1.0f/15.7692f;
        public static readonly float ParryRatingToParry = 1.0f/23.6538461538462f;
        public static readonly float StaminaToHP = 10.0f;
        public static readonly float StrengthToAP = 1.0f;
    }
}