namespace Rawr.DPSWarr
{
    public class WarriorConversions
    {
        public static readonly float StrengthToAP = 2.0f;
        public static readonly float AgilityToArmor = 2.0f;
        public static readonly float AgilityToCrit = 1.0f/62.5f;
        public static readonly float AgilityToDodge = 1.0f/20.0f;
        public static readonly float CritRatingToCrit = 1.0f/45.90598f;
        public static readonly float CritToCritRating = 45.90598f; //14*82/52
        public static readonly float ExpertiseRatingToExpertise = 1.0f/8.1974969f;
        public static readonly float ExpertiseToDodgeParryReduction = 0.25f;
        public static readonly float HasteRatingToHaste = 1.0f/32.7899f;
        public static readonly float HitRatingToHit = 1.0f / 32.7899f;
        public static readonly float ParryRatingToParry = 1.0f/23.6538461538462f;
        public static readonly float StaminaToHP = 10.0f;
        public static readonly float ArPToArmorPenetration = 1.0f / 15.395298f;
    }
}