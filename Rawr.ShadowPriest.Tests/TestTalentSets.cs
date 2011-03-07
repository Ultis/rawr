using System;
using Rawr;

namespace Tests.Rawr.ShadowPriest
{
    /// <summary>
    /// Test talent settings.
    /// </summary>
    public class TestTalentSets
    {
        /// <summary>
        /// Suggested talent set from http://elitistjerks.com/f77/t112651-shadow_priest_--_cataclysm/
        /// circa 27/02/2011
        /// </summary>
        /// <param name="talents"></param>
        public static void LoadSuggested(PriestTalents talents)
        {
            // Discipline Talents
            talents.ImprovedPowerWordShield = 0;
            talents.TwinDisciplines = 3;
            talents.MentalAgility = 2;

            talents.Evangelism = 2;
            talents.Archangel = 1;
            talents.InnerSanctum = 0;
            talents.SoulWarding = 0;

            // Holy Talents
            talents.ImprovedRenew = 0;
            talents.EmpoweredHealing = 0;
            talents.DivineFury = 0;

            talents.DesperatePrayer = 0;
            talents.SurgeOfLight = 0;
            talents.Inspiration = 0;

            // Shadow Talents
            talents.Darkness = 3;
            talents.ImprovedShadowWordPain = 2;
            talents.VeiledShadows = 2;

            talents.ImprovedPsychicScream = 0;
            talents.ImprovedMindBlast = 3;
            talents.ImprovedDevouringPlague = 2;
            talents.TwistedFaith = 2;

            talents.Shadowform = 1;
            talents.Phantasm = 0;
            talents.HarnessedShadows = 2;

            talents.Silence = 0;
            talents.VampiricEmbrace = 1;
            talents.Masochism = 2;
            talents.MindMelt = 2;

            talents.PainAndSuffering = 2;
            talents.VampiricTouch = 1;
            talents.Paralysis = 0;

            talents.PsychicHorror = 0;
            talents.SinAndPunishment = 2;
            talents.ShadowyApparition = 3;

            talents.Dispersion = 1;
        }
    }
}