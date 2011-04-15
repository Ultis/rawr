using System;
using System.Collections.Generic;


namespace Rawr.HealPriest
{
    public abstract class PriestModels
    {
        // Models
        public static string modelDiscTank = "Disc. Tank";
        public static string modelDiscRaid = "Disc. Raid";
        public static string modelHolyTank = "Holy Tank";
        public static string modelHolyRaid = "Holy Raid";

        public static string[] Models = {
            modelDiscTank,
            modelDiscRaid,
            modelHolyTank,
            modelHolyRaid,
            };

        public static PriestSolver GetModel(string model, CharacterCalculationsHealPriest calc, CalculationOptionsHealPriest calcOpts, bool verbose)
        {
            if (model == modelDiscTank)
                return new PriestSolverDisciplineTank(calc, calcOpts, verbose);
            if (model == modelDiscRaid)
                return new PriestSolverDisciplineRaid(calc, calcOpts, verbose);
            if (model == modelHolyTank)
                return new PriestSolverHolyTank(calc, calcOpts, verbose);
            if (model == modelHolyRaid)
                return new PriestSolverHolyRaid(calc, calcOpts, verbose);
            return null;
        }
    }

    public class ManaSource
    {
        public string Name { get; protected set; }
        public float Value { get; protected set; }

        public ManaSource(string name, float value)
        {
            Name = name;
            Value = value;
        }
    }


    public abstract class PriestSolver
    {     
        public string Name { get; protected set; }

        protected Character character;
        protected Stats stats;
        protected CharacterCalculationsHealPriest calc;
        protected CalculationOptionsHealPriest calcOpts;
        protected bool verbose;

        protected float fightLength;

        public List<ManaSource> ManaSources = new List<ManaSource>();
      
        public PriestSolver(CharacterCalculationsHealPriest calc, CalculationOptionsHealPriest calcOpts, bool verbose)
        {
            Name = "Priest Solver base";

            this.character = calc.Character;
            this.stats = calc.BasicStats;
            this.calc = calc;
            this.calcOpts = calcOpts;
            this.verbose = verbose;

            fightLength = calcOpts.FightLengthSeconds;
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
            float tmpReg, manaReg = 0;
            
            tmpReg = StatConversion.GetSpiritRegenSec(stats.Spirit, stats.Intellect) * stats.SpellCombatManaRegeneration;
            if (verbose)
                ManaSources.Add(new ManaSource("Spirit Regen", tmpReg));
            manaReg += tmpReg;
            
            tmpReg = stats.Mp5 / 5;
            if (verbose)
                ManaSources.Add(new ManaSource("MP5 w/more", tmpReg));
            manaReg += tmpReg;

            tmpReg = stats.Mana * 0.3f / (300 - PriestInformation.GetVeiledShadows(character.PriestTalents.VeiledShadows));     // Shadowfiend
            if (verbose)
                ManaSources.Add(new ManaSource("Shadowfiend", tmpReg));
            manaReg += tmpReg;

            if (character.PriestTalents.Rapture > 0)
            {
                tmpReg = stats.Mana * PriestInformation.GetRapture(character.PriestTalents.Rapture) / 30;
                if (verbose)
                    ManaSources.Add(new ManaSource("Rapture", tmpReg));
                manaReg += tmpReg;
            }
            if (stats.ManaRestoreFromMaxManaPerSecond > 0)
            {
                tmpReg = stats.Mana * stats.ManaRestoreFromMaxManaPerSecond;
                if (verbose)
                    ManaSources.Add(new ManaSource("Replenishment", tmpReg));
                manaReg += tmpReg;
            }
            if (character.Race == CharacterRace.BloodElf)
            {
                tmpReg = stats.Mana * 0.06f / 120;
                if (verbose)
                    ManaSources.Add(new ManaSource("Blood Elf", tmpReg));
                manaReg += tmpReg;
            }
            return manaReg;
        }
    }

    public class PriestSolverDisciplineRaid : PriestSolver
    {
        public PriestSolverDisciplineRaid(CharacterCalculationsHealPriest calc, CalculationOptionsHealPriest calcOpts, bool verbose)
            : base(calc, calcOpts, verbose)
        {
            Name = PriestModels.modelDiscRaid;
        }

        // PWS
        // ProH_BT
        public override void Solve()
        {
            float borrowedTime = PriestInformation.GetBorrowedTime(character.PriestTalents.BorrowedTime);
            Stats statsBT = new Stats() { SpellHaste = borrowedTime };
            statsBT.Accumulate(stats);

            List<DirectHealSpell> castSequence = new List<DirectHealSpell>();

            castSequence.Add(new SpellPowerWordShield(character, stats));
            castSequence.Add(new SpellPrayerOfHealing(character, statsBT));

            float burst = 0, sustain = 0;
            float castTime = 0, baseCastTime = 0;
            float manaSustainUse = 0;
            for (int x = 0; x < castSequence.Count; x++)
            {
                DirectHealSpell dhs = castSequence[x];
                burst += dhs.HPC();
                baseCastTime += dhs.IsInstant ? dhs.BaseGlobalCooldown : dhs.BaseCastTime + 0.2f;
                castTime += dhs.IsInstant ? dhs.GlobalCooldown : dhs.CastTime + 0.2f;
                manaSustainUse += dhs.ManaCost;
            }
            sustain = burst / baseCastTime;
            burst /= castTime;
            manaSustainUse = manaSustainUse / baseCastTime;

            float manaRegen = CalcManaReg();

            calc.BurstPoints = burst;

            float missingMana = manaSustainUse - manaRegen;
            float manaDrain = stats.Mana / missingMana;
            float fullBurst = manaDrain / fightLength;
            float waitCast = 1f - fullBurst;
            calc.SustainPoints = sustain * fullBurst
                + sustain * ((manaRegen / missingMana) * waitCast);

            if (calc.SustainPoints > calc.BurstPoints)
                calc.SustainPoints = calc.BurstPoints;  // NOT FRIGGEN LIKELY EVER LOLZ

            calc.OverallPoints = calc.BurstPoints + calc.SustainPoints + calc.SurvPoints;

            if (verbose)
            {
                List<string> modelInfo = new List<string>();
                modelInfo.Add("The model uses the following spell rotation:");
                for (int x = 0; x < castSequence.Count; x++)
                    modelInfo.Add(castSequence[x].Name);

                Name = String.Format("{0}*{1}", Name, String.Join("\n", modelInfo));
            }
        }
    }

    public class PriestSolverDisciplineTank : PriestSolver
    {
        public PriestSolverDisciplineTank(CharacterCalculationsHealPriest calc, CalculationOptionsHealPriest calcOpts, bool verbose)
            : base(calc, calcOpts, verbose)
        {
            Name = PriestModels.modelDiscTank;
        }

        
        //
        // PWS
        // Penance - BT
        // GH - BT
        // GH
        // GH
        // GH
        // GH
        public override void Solve()
        {
            float graceBonus = PriestInformation.GetGrace(character.PriestTalents.Grace) * 3;
            Stats statsGR = new Stats() { BonusHealingDoneMultiplier = graceBonus };
            float borrowedTime = PriestInformation.GetBorrowedTime(character.PriestTalents.BorrowedTime);
            Stats statsBT = new Stats() { SpellHaste = borrowedTime, BonusHealingDoneMultiplier = graceBonus };
            statsBT.Accumulate(stats);
            float renewedHope = PriestInformation.GetRenewedHope(character.PriestTalents.RenewedHope);
            Stats statsRH = new Stats() { SpellCrit = renewedHope, BonusHealingDoneMultiplier = graceBonus };
            statsRH.Accumulate(stats);

            Stats statsBTRH = new Stats() { SpellHaste = borrowedTime, SpellCrit = renewedHope, BonusHealingDoneMultiplier = graceBonus };
            statsBTRH.Accumulate(stats);

            List<DirectHealSpell> castSequence = new List<DirectHealSpell>();

            castSequence.Add(new SpellPowerWordShield(character, stats));
            castSequence.Add(new SpellPenance(character, statsBT));
            castSequence.Add(new SpellGreaterHeal(character, statsBTRH));
            SpellGreaterHeal sgh = new SpellGreaterHeal(character, statsRH);
            castSequence.Add(sgh);
            castSequence.Add(sgh);
            castSequence.Add(sgh);
            castSequence.Add(sgh);

            float burst = 0, sustain = 0;
            float castTime = 0, baseCastTime = 0;
            float manaSustainUse = 0;
            for (int x = 0; x < castSequence.Count; x++)
            {
                DirectHealSpell dhs = castSequence[x];
                burst += dhs.HPC();
                baseCastTime += dhs.IsInstant ? dhs.BaseGlobalCooldown : dhs.BaseCastTime + 0.2f;
                castTime += dhs.IsInstant ? dhs.GlobalCooldown : dhs.CastTime + 0.2f;
                manaSustainUse += dhs.ManaCost;
            }
            sustain = burst / baseCastTime;
            burst /= castTime;
            manaSustainUse /= baseCastTime;

            float manaRegen = CalcManaReg();

            calc.BurstPoints = burst;

            float missingMana = manaSustainUse - manaRegen;
            float manaDrain = (stats.Mana + stats.ManaRestore)/ missingMana;
            float fullBurst = manaDrain / fightLength;
            float waitCast = 1f - fullBurst;
            calc.SustainPoints = sustain * fullBurst
                + sustain * ((manaRegen / missingMana) * waitCast);

            if (calc.SustainPoints > calc.BurstPoints)
                calc.SustainPoints = calc.BurstPoints;

            calc.OverallPoints = calc.BurstPoints + calc.SustainPoints + calc.SurvPoints;

            if (verbose)
            {
                List<string> modelInfo = new List<string>();
                modelInfo.Add("The model uses the following spell rotation:");
                for (int x = 0; x < castSequence.Count; x++)
                    modelInfo.Add(castSequence[x].Name);

                Name = String.Format("{0}*{1}", Name, String.Join("\n", modelInfo));
            }
        }
    }

    public class PriestSolverHolyTank : PriestSolver
    {
        public PriestSolverHolyTank(CharacterCalculationsHealPriest calc, CalculationOptionsHealPriest calcOpts, bool verbose)
            : base(calc, calcOpts, verbose)
        {
            Name = PriestModels.modelHolyTank;
        }

        public override void Solve()
        {
        }
    }

    public class PriestSolverHolyRaid : PriestSolver
    {
        public PriestSolverHolyRaid(CharacterCalculationsHealPriest calc, CalculationOptionsHealPriest calcOpts, bool verbose)
            : base(calc, calcOpts, verbose)
        {
            Name = PriestModels.modelHolyRaid;
        }

        public override void Solve()
        {
        }
    }
}
