namespace Rawr.Rogue
{
    public class RogueRotationCalculatorCombat : RogueRotationCalculator
    {
        public float EnergyRegenMultiplier { get; set; }

        public RogueRotationCalculatorCombat(Character character, int spec, Stats stats, CalculationOptionsRogue calcOpts, float hasteBonus,
            float mainHandSpeed, float offHandSpeed, float mainHandSpeedNorm, float offHandSpeedNorm, float avoidedWhiteMHAttacks, float avoidedWhiteOHAttacks, float avoidedMHAttacks, float avoidedOHAttacks, float avoidedFinisherAttacks, float avoidedPoisonAttacks,
            float chanceExtraCPPerHit, float chanceExtraCPPerMutiHit,
            RogueAbilityStats mainHandStats, RogueAbilityStats offHandStats, RogueAbilityStats mainGaucheStats, RogueAbilityStats backstabStats, RogueAbilityStats hemoStats, RogueAbilityStats sStrikeStats,
            RogueAbilityStats mutiStats, RogueAbilityStats rStrikeStats, RogueAbilityStats ruptStats, RogueAbilityStats evisStats, RogueAbilityStats envenomStats, RogueAbilityStats snDStats, RogueAbilityStats recupStats, RogueAbilityStats exposeStats,
            RogueAbilityStats iPStats, RogueAbilityStats dPStats, RogueAbilityStats wPStats) : base(character, spec, stats, calcOpts, hasteBonus,
                mainHandSpeed, offHandSpeed, mainHandSpeedNorm, offHandSpeedNorm, avoidedWhiteMHAttacks, avoidedWhiteOHAttacks, avoidedMHAttacks, avoidedOHAttacks, avoidedFinisherAttacks, avoidedPoisonAttacks,
                chanceExtraCPPerHit, chanceExtraCPPerMutiHit, mainHandStats, offHandStats, mainGaucheStats, backstabStats, hemoStats, sStrikeStats, mutiStats, rStrikeStats, ruptStats, evisStats, envenomStats, snDStats, recupStats, exposeStats,
                iPStats, dPStats, wPStats)
        {
            EnergyRegenMultiplier = (1f + (spec == 1 ? RV.Mastery.VitalityRegenMult : 0f)) * (1f + (RV.AR.Duration + (Talents.GlyphOfAdrenalineRush ? RV.Glyph.ARDurationBonus : 0f)) / RV.AR.CD * Talents.AdrenalineRush) * (1f + HasteBonus) - 1f;
        }

        public override float getEnergyRegen()
        {
            return RV.BaseEnergyRegen * (1f + EnergyRegenMultiplier);
        }

        public override float getEnergyAvailable()
        {
            return RV.BaseEnergy + BonusMaxEnergy + EnergyRegen * Duration +
                (UseTotT ? (-RV.TotT.Cost + ToTTCostReduction) * (Duration - RV.TotT.Duration) / RV.TotT.CD : 0f) +
                Talents.AdrenalineRush * RV.AR.Duration * EnergyRegen * (1f + RV.AR.EnergyRegenMult) * Duration / RV.AR.CD;
        }

        public override float getCPGEnergy()
        {
            return SStrikeStats.EnergyCost;
        }

        public override float getCPPerCPG()
        {
            return SStrikeStats.CPPerSwing;
        }
    }
}
