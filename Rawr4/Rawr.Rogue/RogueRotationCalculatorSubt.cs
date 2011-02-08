namespace Rawr.Rogue
{
    public class RogueRotationCalculatorSubt : RogueRotationCalculator
    {
        public RogueRotationCalculatorSubt(Character character, int spec, Stats stats, CalculationOptionsRogue calcOpts, float hasteBonus,
            float mainHandSpeed, float offHandSpeed, float mainHandSpeedNorm, float offHandSpeedNorm, float avoidedWhiteMHAttacks, float avoidedWhiteOHAttacks, float avoidedMHAttacks, float avoidedOHAttacks, float avoidedFinisherAttacks, float avoidedPoisonAttacks,
			float chanceExtraCPPerHit, float chanceExtraCPPerMutiHit,
            RogueAbilityStats mainHandStats, RogueAbilityStats offHandStats, RogueAbilityStats mainGaucheStats, RogueAbilityStats backstabStats, RogueAbilityStats hemoStats, RogueAbilityStats sStrikeStats,
            RogueAbilityStats mutiStats, RogueAbilityStats rStrikeStats, RogueAbilityStats ruptStats, RogueAbilityStats evisStats, RogueAbilityStats envenomStats, RogueAbilityStats snDStats, RogueAbilityStats recupStats, RogueAbilityStats exposeStats,
            RogueAbilityStats iPStats, RogueAbilityStats dPStats, RogueAbilityStats wPStats) : base(character, spec, stats, calcOpts, hasteBonus,
                mainHandSpeed, offHandSpeed, mainHandSpeedNorm, offHandSpeedNorm, avoidedWhiteMHAttacks, avoidedWhiteOHAttacks, avoidedMHAttacks, avoidedOHAttacks, avoidedFinisherAttacks, avoidedPoisonAttacks,
                chanceExtraCPPerHit, chanceExtraCPPerMutiHit, mainHandStats, offHandStats, mainGaucheStats, backstabStats, hemoStats, sStrikeStats, mutiStats, rStrikeStats, ruptStats, evisStats, envenomStats, snDStats, recupStats, exposeStats,
                iPStats, dPStats, wPStats)
        {
        }

        public override float getNumberStealths()
        {
            return 1f + Duration / (RV.Vanish.CD - VanishCDReduction) + (StepVanishResetCD > 0 ? Duration / StepVanishResetCD : 0f);
        }

        public override float getEnergyAvailable()
        {
            return RV.BaseEnergy + BonusMaxEnergy + EnergyRegen * Duration +
                (UseTotT ? (-RV.TotT.Cost + ToTTCostReduction) * (Duration - RV.TotT.Duration) / RV.TotT.CD : 0f) +
                (RecupCP > 0 ? Duration / 3 * EnergyOnRecupTick : 0f);
        }

        public override float getCPAvailable()
        {   
            return (Talents.Premeditation > 0 ? RV.Talents.PremeditationBonusCP * Duration / RV.Talents.PremeditationCD : 0) + (Talents.HonorAmongThieves > 0 ? RV.Talents.HonorAmongThievesCPChance[Talents.HonorAmongThieves] * Duration / RV.Talents.HonorAmongThievesCD[Talents.HonorAmongThieves] : 0);
        }

        public override float getCPGEnergy()
        {
            return BackstabStats.EnergyCost;
        }

        public override float getCPPerCPG()
        {
            return BackstabStats.CPPerSwing;
        }
        public override void processFinisher(float cpRequired, float finisherEnergy)
        {
            if (TotalCPAvailable >= cpRequired) TotalCPAvailable -= cpRequired;
            else
            {
                float cpgToUse = (cpRequired - TotalCPAvailable) / CPPerCPG;
                CPGCount += cpgToUse;
                TotalEnergyAvailable -= cpgToUse * CPGEnergy + finisherEnergy;
                TotalCPAvailable = 0;
            }
        }
    }
}