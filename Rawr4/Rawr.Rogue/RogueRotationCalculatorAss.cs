using System;

namespace Rawr.Rogue
{
    public class RogueRotationCalculatorAss : RogueRotationCalculator
    {
        public float BonusStealthEnergyRegen { get; set; }

        public RogueRotationCalculatorAss(Character character, int spec, Stats stats, CalculationOptionsRogue calcOpts, float hasteBonus,
            float mainHandSpeed, float offHandSpeed, float mainHandSpeedNorm, float offHandSpeedNorm, float avoidedWhiteMHAttacks, float avoidedWhiteOHAttacks, float avoidedMHAttacks, float avoidedOHAttacks, float avoidedFinisherAttacks, float avoidedPoisonAttacks,
			float chanceExtraCPPerHit, float chanceExtraCPPerMutiHit,
            RogueAbilityStats mainHandStats, RogueAbilityStats offHandStats, RogueAbilityStats mainGaucheStats, RogueAbilityStats backstabStats, RogueAbilityStats hemoStats, RogueAbilityStats sStrikeStats,
            RogueAbilityStats mutiStats, RogueAbilityStats rStrikeStats, RogueAbilityStats ruptStats, RogueAbilityStats evisStats, RogueAbilityStats envenomStats, RogueAbilityStats snDStats, RogueAbilityStats recupStats, RogueAbilityStats exposeStats,
            RogueAbilityStats iPStats, RogueAbilityStats dPStats, RogueAbilityStats wPStats) : base (character, spec, stats, calcOpts, hasteBonus,
            mainHandSpeed, offHandSpeed, mainHandSpeedNorm, offHandSpeedNorm, avoidedWhiteMHAttacks, avoidedWhiteOHAttacks, avoidedMHAttacks, avoidedOHAttacks, avoidedFinisherAttacks, avoidedPoisonAttacks,
			chanceExtraCPPerHit, chanceExtraCPPerMutiHit, mainHandStats, offHandStats, mainGaucheStats, backstabStats, hemoStats, sStrikeStats, mutiStats, rStrikeStats, ruptStats, evisStats, envenomStats, snDStats, recupStats, exposeStats,
            iPStats, dPStats, wPStats)
		{
            BonusStealthEnergyRegen = RV.Talents.OverkillRegenMult * Talents.Overkill;
        }

        public override float getEnergyAvailable()
        {
            return RV.BaseEnergy + BonusMaxEnergy + EnergyRegen * Duration +
                (UseTotT ? (-RV.TotT.Cost + ToTTCostReduction) * (Duration - RV.TotT.Duration) / RV.TotT.CD : 0f) + 
                NumberOfStealths * RV.Talents.OverkillRegenDuration * EnergyRegen * (1f + BonusStealthEnergyRegen) + 
                RV.ColdBlood.EnergyBonus * Talents.ColdBlood * Duration / RV.ColdBlood.CD;
        }

        public override float getCPGEnergy()
        {
            return (CPG == 1 ? BackstabStats.EnergyCost - EnergyOnBelow35BS : MutiStats.EnergyCost);
        }

        public override float getCPPerCPG()
        {
            return (CPG == 1 ? BackstabStats.CPPerSwing : MutiStats.CPPerSwing);
        }

        public override float getSnDCount(float snDDuration)
        {
            float snDCount = Duration / snDDuration;
            return Math.Max(1f, snDCount * (1f - ChanceOnSnDResetOnEvisEnv));
        }
    }
}
