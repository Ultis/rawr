using System;


namespace Rawr.HealPriest
{
    public abstract class PriestSolver
    {
        public string Name { get; protected set; }

        protected Character character;
        protected Stats stats;
        protected CharacterCalculationsHealPriest calc;
        protected CalculationOptionsHealPriest calcOpts;

        protected float fightLength;
      
        public PriestSolver(CharacterCalculationsHealPriest calc, CalculationOptionsHealPriest calcOpts)
        {
            Name = "Priest Solver base";

            this.character = calc.Character;
            this.stats = calc.BasicStats;
            this.calc = calc;
            this.calcOpts = calcOpts;

            fightLength = calcOpts.FightLengthSeconds * 60f;
        }

        public virtual void Solve()
        {
            calc.BurstPoints = 0;
            calc.SustainPoints = 0;
            calc.OverallPoints = calc.BurstPoints + calc.SustainPoints;
        }

        protected float CalcManaReg()
        {
            // Spirit based
            float manaReg = StatConversion.GetSpiritRegenSec(stats.Spirit, stats.Intellect) * stats.SpellCombatManaRegeneration;
            manaReg += stats.Mp5 / 5;
            manaReg += stats.Mana * 0.3f / 300;
            manaReg += stats.Mana * PriestInformation.GetRapture(character.PriestTalents.Rapture) / 30;
            if (character.Race == CharacterRace.BloodElf)
            {
                manaReg += stats.Mana * 0.06f / 120;
            }
            return manaReg;
        }
    }

    public class PriestSolverDisciplineRaid : PriestSolver
    {
        public PriestSolverDisciplineRaid(CharacterCalculationsHealPriest calc, CalculationOptionsHealPriest calcOpts)
            : base(calc, calcOpts)
        {
            Name = "Discipline Raid";
        }

        public override void Solve()
        {
            SpellPowerWordShield pws = new SpellPowerWordShield(character, stats);
            float burst = pws.HPC();
            Stats tStats = new Stats() { SpellHaste = PriestInformation.GetBorrowedTime(character.PriestTalents.BorrowedTime) };
            tStats.Accumulate(stats);
            SpellPrayerOfHealing proh = new SpellPrayerOfHealing(character, tStats, 5);
            burst += proh.HPC();

            float castTime = pws.GlobalCooldown + proh.CastTime + 0.2f * 2;
            burst /= castTime;
            calc.BurstPoints = burst;
            float manaRegen = CalcManaReg();
            float useMana = (proh.ManaCost + pws.ManaCost) / castTime;
            float missingMana = useMana - manaRegen;
            float manaDrain = stats.Mana / missingMana;
            if (manaDrain > fightLength)
                calc.SustainPoints = burst;
            else
            {
                float fullBurst = manaDrain / fightLength;
                float waitCast = (fightLength - manaDrain) / fightLength;
                calc.SustainPoints = burst * fullBurst
                    + burst * (waitCast * (manaRegen / useMana));
            }
            calc.OverallPoints = calc.BurstPoints + calc.SustainPoints + calc.SurvPoints;
        }
    }
}
