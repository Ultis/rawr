using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Rawr.Hunter
{
    public class HunterBase
    {
        public HunterBase (Character charac, StatsHunter stats, HunterTalents talents, Specialization spec, int targetLvL)
        {
            character = charac;
            Stats = stats;
            Talents = talents;
            Tree = spec;
            TargetLevel = targetLvL;
        }

        private Character character { get; set; }
        public StatsHunter Stats { get; private set; }
        public HunterTalents Talents { get; private set; }
        public Specialization Tree { get; private set; }
        private int TargetLevel { get; set; }

        protected float BeastMasteryMastery { get { return (Tree == Specialization.BeastMastery) ? 0.13f + (0.017f * (8f + StatConversion.GetMasteryFromRating(Stats.MasteryRating))) : 0f; } }
        protected float MarksmanshipMastery { get { return (Tree == Specialization.Marksmanship) ? 0.17f + (0.018f * (8f + StatConversion.GetMasteryFromRating(Stats.MasteryRating))) : 0f; } }
        protected float SurvivalMastery { get { return (Tree == Specialization.Survival) ? 0.08f + (0.01f * (8f + StatConversion.GetMasteryFromRating(Stats.MasteryRating))) : 0f; } }
        protected float MailSpecialization { get { return Character.ValidateArmorSpecialization ( character, ItemType.Mail ) ? 0.05f : 0f; } }

        protected float Stamina { get { return (float)Math.Floor(Stats.Stamina * (1f + Stats.BonusStaminaMultiplier)); } }

        protected float Health { get { return (float)Math.Floor((this.Stamina - 20f) * 14f + 20f) * (1f + Stats.BonusHealthMultiplier); } }
        protected float Focus { get { return 100f + (Talents.KindredSpirits * 5f); } }


    }
}
