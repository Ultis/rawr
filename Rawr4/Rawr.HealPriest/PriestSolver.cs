using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace Rawr.HealPriest
{

    public class PriestModels
    {
        // Models
        public static string modelDiscTank = "Disc. Tank";
        public static string modelDiscTank2 = "Disc. on 2 Tanks";
        public static string modelDiscTank3 = "Disc. on 3 Tanks";
        public static string modelDiscRaid = "Disc. Raid";
        public static string modelHolyTank = "Holy Tank";
        public static string modelHolyRaid = "Holy Raid";

        public static string[] Models = {
            modelDiscTank,
            modelDiscTank2,
            modelDiscTank3,
            modelDiscRaid,
            modelHolyTank,
            modelHolyRaid,
            };

        public static PriestSolver GetModel(CharacterCalculationsHealPriest calc, CalculationOptionsHealPriest calcOpts, bool verbose)
        {
            string modelName = calcOpts.Model;
            PriestSolver model = null;

            if (modelName == modelDiscTank)
                model = new PriestSolverDisciplineTank(calc, calcOpts, verbose);
            else if (modelName == modelDiscTank2)
                model = new PriestSolverDisciplineTank2(calc, calcOpts, verbose);
            else if (modelName == modelDiscTank3)
                model = new PriestSolverDisciplineTank3(calc, calcOpts, verbose);
            else if (modelName == modelDiscRaid)
                model = new PriestSolverDisciplineRaid(calc, calcOpts, verbose);
            else if (modelName == modelHolyTank)
                model = new PriestSolverHolyTank(calc, calcOpts, verbose);
            else if (modelName == modelHolyRaid)
                model = new PriestSolverHolyRaid(calc, calcOpts, verbose);
            /*if (verbose)
            {
                List<string> msg = model.MeetsRequirements();
                if (msg.Count > 0)
                {
                    MessageBox.Show(String.Format("Currently selected model might not work optimally due to:{0}", String.Join("\n", msg)));
                }
            }*/

            if (model == null)
                throw new Exception("No model selection for Healing Priest. Not a good situation.");
            return model;
        }

        public static string GetDefault()
        {   // coulda & shoulda be more advanced.
            return modelDiscTank;
        }
    }

    public class ManaSource
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public float Value { get; protected set; }

        public ManaSource(string name, string description, float value)
        {
            Name = name;
            Description = description;
            Value = value;
        }
    }


    public abstract class PriestSolver
    {     
        public string Name { get; protected set; }

        protected Character character;
        protected StatsPriest stats;
        protected CharacterCalculationsHealPriest calc;
        protected CalculationOptionsHealPriest calcOpts;
        protected BossOptions bossOptions;
        protected bool verbose;

        public List<ManaSource> ManaSources = new List<ManaSource>();
        protected List<DirectHealSpell> castSequence = new List<DirectHealSpell>();
      
        public PriestSolver(CharacterCalculationsHealPriest calc, CalculationOptionsHealPriest calcOpts, bool verbose)
        {
            Name = "Priest Solver base";

            this.character = calc.Character;
            this.stats = calc.BasicStats;
            this.calc = calc;
            this.calcOpts = calcOpts;
            this.bossOptions = character.BossOptions;
            this.verbose = verbose;
        }

        public virtual List<string> MeetsRequirements()
        {
            return new List<string>();
        }
        
        public virtual void Solve()
        {
            calc.BurstPoints = 0;
            calc.SustainPoints = 0;
            calc.ManaPoints = 0;
            calc.OverallPoints = calc.BurstPoints + calc.SustainPoints + calc.ManaPoints;
        }

        private float CalcSpiritRegen(float Spirit, float Intellect, float combatRegen)
        {
            return StatConversion.GetSpiritRegenSec(Spirit, Intellect) * combatRegen;
        }

        private float CalcShadowfiend(float mana)
        {   // 3% mana every hit, 15s dura
            // FIXME: 2s swing timer = 7.5 ~ 8 swings = 8*3 = 24% mana. Assume hasted by haste.
            float casts = (float)Math.Floor((bossOptions.BerserkTimer - 30f) / (60f * 5f - PriestInformation.GetVeiledShadows(character.PriestTalents.VeiledShadows))) + 1f;
            return mana * (0.03f * 8f) * casts / bossOptions.BerserkTimer * (1f + calcOpts.Shadowfiend / 100f);
        }

        private float CalcRapture(float mana)
        {
            return mana * PriestInformation.GetRapture(character.PriestTalents.Rapture) / calcOpts.Rapture;
        }

        protected float CalcManaReg(float castsPerSecond, float critsPerSecond)
        {
            // Other things
            bool bBloodElf = character.Race == CharacterRace.BloodElf;

            // Spirit based
            float tmpReg, manaReg = 0;
            float spiritBaseRegen;
            if (verbose)
                ManaSources.Clear();

            tmpReg = CalcSpiritRegen(stats.Spirit, stats.Intellect, stats.SpellCombatManaRegeneration);
            spiritBaseRegen = tmpReg;
            if (verbose)
                ManaSources.Add(new ManaSource("Spirit Regen", "This is your mana regeneration while in combat", tmpReg));
            manaReg += tmpReg;
            
            tmpReg = stats.Mp5 / 5;
            if (verbose)
                ManaSources.Add(new ManaSource("MP5", "This mana regeneration is constant, in combat or not.\n5% is from your base mana, rest from enchants and buffs", tmpReg));
            manaReg += tmpReg;

            tmpReg = CalcShadowfiend(stats.Mana);
            if (verbose)
                ManaSources.Add(new ManaSource("Shadowfiend", "Assuming you cast Shadowfiend after 30 seconds and then on cooldown rest of fight", tmpReg));
            manaReg += tmpReg;

            if (character.PriestTalents.Rapture > 0)
            {
                tmpReg = CalcRapture(stats.Mana);
                if (verbose)
                    ManaSources.Add(new ManaSource("Rapture", String.Format("Assuming you gain the Rapture proc every {0} seconds", calcOpts.Rapture.ToString("0")), tmpReg));
                manaReg += tmpReg;
            }
            if (stats.ManaRestoreFromMaxManaPerSecond > 0)
            {
                tmpReg = stats.Mana * stats.ManaRestoreFromMaxManaPerSecond * (calcOpts.Replenishment / 100f);
                if (verbose)
                    ManaSources.Add(new ManaSource("Replenishment", String.Format("Assuming an uptime of {0}%.", calcOpts.Replenishment.ToString("0.00")), tmpReg));
                manaReg += tmpReg;
            }
            if (bBloodElf)
            {
                tmpReg = stats.Mana * 0.06f / 120;
                if (verbose)
                    ManaSources.Add(new ManaSource("Blood Elf", "Assuming you use it on cooldown", tmpReg));
                manaReg += tmpReg;
            }

            if (calcOpts.ModelProcs)
            {
                foreach (SpecialEffect se in stats.SpecialEffects())
                {
                    float uptime = 0;
                    if (se.Trigger == Trigger.Use)
                    {
                        uptime = se.GetAverageUptime(se.Cooldown, 1f);
                    }
                    else if (se.Trigger == Trigger.HealingSpellCast
                        || se.Trigger == Trigger.HealingSpellCrit
                        || se.Trigger == Trigger.SpellCast
                        || se.Trigger == Trigger.SpellCrit
                        || se.Trigger == Trigger.DamageOrHealingDone)
                    {
                        uptime = se.GetAverageUptime(castsPerSecond, (se.Trigger == Trigger.HealingSpellCrit || se.Trigger == Trigger.SpellCrit) ? critsPerSecond : 1f);
                    }
                    if (se.Stats.Spirit > 0)
                    {   // Bonus Spirit
                        float bonusSpirit = se.Stats.Spirit * (1f + stats.BonusSpiritMultiplier);
                        tmpReg = CalcSpiritRegen(bonusSpirit, stats.Intellect, stats.SpellCombatManaRegeneration) * uptime;
                        if (verbose)
                            ManaSources.Add(new ManaSource(se.ToString(), String.Format("{0}% expected uptime", (uptime * 100f).ToString("0.00")), tmpReg));
                        manaReg += tmpReg;
                    }
                    if (se.Stats.Intellect > 0)
                    {   // Bonus Intellect
                        float bonusInt = se.Stats.Intellect * (1f + stats.BonusIntellectMultiplier);
                        float bonusMana = StatConversion.GetManaFromIntellect(bonusInt) * (1f + stats.BonusManaMultiplier);
                        float spiritReg = CalcSpiritRegen(stats.Spirit, stats.Intellect + bonusInt, stats.SpellCombatManaRegeneration) - spiritBaseRegen;
                        float raptureReg = CalcRapture(bonusMana);
                        float replenishmentReg = bonusMana * stats.ManaRestoreFromMaxManaPerSecond * (calcOpts.Replenishment / 100f);
                        float shadowfiendReg = CalcShadowfiend(bonusMana);
                        float bloodelfReg = bBloodElf ? bonusMana * 0.06f / 120 : 0;
                        tmpReg = (spiritReg + replenishmentReg + raptureReg + shadowfiendReg + bloodelfReg) * uptime;
                        if (verbose)
                            ManaSources.Add(new ManaSource(se.ToString(), String.Format("{0}% expected uptime", (uptime * 100f).ToString("0.00")), tmpReg));
                        manaReg += tmpReg;
                    }
                    if (se.Stats.BonusSpiritMultiplier > 0)
                    {   // Mana Tide
                        // Do some assumptions about the shamans gearing. Lets assume that he stacked spirit in the same way we stacked our best secondary stat.
                        // This may cause some issues which further buff our primary secondary stat, but oh well
                        float maxSecondary = Math.Max(Math.Max(stats.CritRating, stats.HasteRating), Math.Max(stats.Spirit, stats.MasteryRating));
                        float bonusSpirit = maxSecondary * (1f + se.Stats.BonusSpiritMultiplier);
                        tmpReg = CalcSpiritRegen(bonusSpirit, stats.Intellect, stats.SpellCombatManaRegeneration) * uptime;
                        if (verbose)
                            ManaSources.Add(new ManaSource(se.ToString(), String.Format("{0}% expected uptime\n{1} Spirit bonus for duration", (uptime * 100f).ToString("0.00"), bonusSpirit.ToString("0")), tmpReg));
                        manaReg += tmpReg;
                    }
                }
            }
            return manaReg;
        }

        protected void DoCalcs()
        {
            float healed = 0;
            float castTime = 0, baseCastTime = 0;
            float manaCost = 0;
            float critChance = 0;

            for (int x = 0; x < castSequence.Count; x++)
            {
                DirectHealSpell dhs = castSequence[x];
                healed += dhs.HPC();
                baseCastTime += dhs.IsInstant ? dhs.BaseGlobalCooldown : dhs.BaseCastTime;
                castTime += dhs.IsInstant ? dhs.GlobalCooldown : dhs.CastTime;
                manaCost += dhs.ManaCost;
                critChance += dhs.CritChance;
            }

            float effectiveFightLength = bossOptions.BerserkTimer * calcOpts.ActivityRatio / 100f;
            float repeats = effectiveFightLength / baseCastTime;
            float castsPerSecond = castSequence.Count / baseCastTime;
            float critsPerSecond = critChance / baseCastTime;

            healed *= repeats;
            baseCastTime *= repeats;
            castTime *= repeats;
            manaCost *= repeats;
            critChance *= repeats;

            float manaRegen = CalcManaReg(castsPerSecond, critsPerSecond);


            float totalMana = (stats.Mana + stats.ManaRestore * (1f + stats.BonusManaPotionEffectMultiplier))
                + manaRegen * bossOptions.BerserkTimer;

            calc.BurstGoal = 20000f;
            calc.SustainGoal = calc.BurstGoal * 0.5f;
            calc.ManaGoal = manaCost;
            calc.BurstPoints = (1f - (float)Math.Exp(-1f * (((healed / castTime)) / calc.BurstGoal))) * 100000f;
            calc.SustainPoints = (1f - (float)Math.Exp(-1f * ((healed / baseCastTime / calc.SustainGoal)))) * 100000f;
            calc.ManaPoints = (1f - (float)Math.Exp(-1f * (totalMana / calc.ManaGoal))) * 100000f;
            calc.OverallPoints = calc.BurstPoints + calc.SustainPoints + calc.ManaPoints;

            if (verbose)
            {
                List<string> modelInfo = new List<string>();
                modelInfo.Add("The model uses the following spell rotation:");
                for (int x = 0; x < castSequence.Count; x++)
                    modelInfo.Add(castSequence[x].Name);

                Name = String.Format("{0}*{1}", Name, String.Join("\n", modelInfo));
            }
        }


        protected void DoCalcs3()
        {
            float healed = 0;
            float castTime = 0, baseCastTime = 0;
            float manaCost = 0;
            float critChance = 0;
            
            for (int x = 0; x < castSequence.Count; x++)
            {
                DirectHealSpell dhs = castSequence[x];
                healed += dhs.HPC();
                baseCastTime += dhs.IsInstant ? dhs.BaseGlobalCooldown : dhs.BaseCastTime;
                castTime += dhs.IsInstant ? dhs.GlobalCooldown : dhs.CastTime;
                manaCost += dhs.ManaCost;
                critChance += dhs.CritChance;
            }

            float effectiveFightLength = bossOptions.BerserkTimer * calcOpts.ActivityRatio / 100f;
            float repeats = effectiveFightLength / baseCastTime;
            float castsPerSecond = castSequence.Count / baseCastTime;
            float critsPerSecond = critChance / baseCastTime;

            healed *= repeats;
            baseCastTime *= repeats;
            castTime *= repeats;
            manaCost *= repeats;
            critChance *= repeats;

            float manaRegen = CalcManaReg(castsPerSecond, critsPerSecond);


            float totalMana = (stats.Mana + stats.ManaRestore * (1f + stats.BonusManaPotionEffectMultiplier))
                + manaRegen * bossOptions.BerserkTimer;

            calc.BurstPoints = (healed / castTime);
            calc.SustainPoints = (healed / baseCastTime) * (totalMana / manaCost);
            calc.ManaPoints = 0;
            calc.OverallPoints = calc.BurstPoints + calc.SustainPoints + calc.ManaPoints;

            if (verbose)
            {
                List<string> modelInfo = new List<string>();
                modelInfo.Add("The model uses the following spell rotation:");
                for (int x = 0; x < castSequence.Count; x++)
                    modelInfo.Add(castSequence[x].Name);

                Name = String.Format("{0}*{1}", Name, String.Join("\n", modelInfo));
            }
        }

        protected void DoCalcs2()
        {
            float burst = 0, sustain = 0;
            float castTime = 0, baseCastTime = 0;
            float manaSustainUse = 0;
            float critChance = 0;
            for (int x = 0; x < castSequence.Count; x++)
            {
                DirectHealSpell dhs = castSequence[x];
                burst += dhs.HPC();
                baseCastTime += dhs.IsInstant ? dhs.BaseGlobalCooldown : dhs.BaseCastTime;
                castTime += dhs.IsInstant ? dhs.GlobalCooldown : dhs.CastTime;
                manaSustainUse += dhs.ManaCost;
                critChance += dhs.CritChance;
            }
            baseCastTime /= (calcOpts.ActivityRatio / 100f);
            sustain = burst / baseCastTime;
            burst /= castTime;
            manaSustainUse = manaSustainUse / baseCastTime;
            float castsPerSecond = castSequence.Count / baseCastTime;
            float critsPerSecond = critChance / baseCastTime;

            float manaRegen = CalcManaReg(castsPerSecond, critsPerSecond);

            float manaMissing = manaSustainUse - manaRegen;
            float fullBurst = ((stats.Mana + stats.ManaRestore) / manaMissing) / bossOptions.BerserkTimer;
            float waitCast = 1f - fullBurst;
            if (waitCast < 0)
                waitCast = 0;
//            calc.SustainPoints = sustain * fullBurst
//                + sustain * ((manaRegen / manaSustainUse) * waitCast);

//            if (calc.SustainPoints > calc.BurstPoints)
//                calc.SustainPoints = calc.BurstPoints;
            //calc.BurstPoints = burst * fullBurst;
            //calc.SustainPoints = sustain * ((manaRegen / manaSustainUse) * waitCast);

            calc.BurstPoints = burst;
            if (fullBurst > 1)
                calc.SustainPoints = calc.BurstPoints;
            else
                calc.SustainPoints = burst * fullBurst;

            calc.OverallPoints = calc.BurstPoints + calc.SustainPoints + calc.ManaPoints;

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
            
            StatsPriest statsBT = new StatsPriest() { SpellHaste = borrowedTime };
            statsBT.Accumulate(stats);

            castSequence.Clear();

            castSequence.Add(new SpellPowerWordShield(character, stats));
            castSequence.Add(new SpellPrayerOfHealing(character, statsBT));

            DoCalcs();

            if (verbose)
                Name += "\nCasting Power Word: Shield and then Prayer of Healing, repeat.";
        }

        public override List<string> MeetsRequirements()
        {
            List<string> faults = new List<string>();

            ePriestSpec spec = PriestSpec.GetPriestSpec(character.PriestTalents);
            if (spec != ePriestSpec.Spec_Disc)
            {
                faults.Add("This model is meant for Discipline Raid Healing.");
            }
            if (character.PriestTalents.BorrowedTime == 0)
            {
                faults.Add("This model works best with the Borrowed Time Talents.");
            }
            if (!character.PriestTalents.GlyphofPowerWordShield)
            {
                faults.Add("This model works best with Glyph of Power Word: Shield.");
            }
            if (!character.PriestTalents.GlyphofPrayerofHealing)
            {
                faults.Add("This model works best with Glyph of Prayer of Healing.");
            }
            return faults;
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
        // PWS, Penance_BT, GH_BT, GHx4
        public override void Solve()
        {
            float graceBonus = PriestInformation.GetGrace(character.PriestTalents.Grace) * 3;
            float borrowedTime = PriestInformation.GetBorrowedTime(character.PriestTalents.BorrowedTime);
            float renewedHope = PriestInformation.GetRenewedHope(character.PriestTalents.RenewedHope);

            StatsPriest statsGR = new StatsPriest() { BonusHealingDoneMultiplier = graceBonus };
            statsGR.Accumulate(stats);
            
            StatsPriest statsRHGR = new StatsPriest() { SpellCrit = renewedHope, BonusHealingDoneMultiplier = graceBonus };
            statsRHGR.Accumulate(stats);

            StatsPriest statsBTRHGR = new StatsPriest() { SpellHaste = borrowedTime, SpellCrit = renewedHope, BonusHealingDoneMultiplier = graceBonus };
            statsBTRHGR.Accumulate(stats);

            // BT = Borrowed Time, RH = Renewed Hope, GR = Grace
            SpellPowerWordShield pwsGR = new SpellPowerWordShield(character, statsGR);
            SpellPenance penanceBTGRRH = new SpellPenance(character, statsBTRHGR);
            SpellGreaterHeal ghBTGRRH = new SpellGreaterHeal(character, statsBTRHGR);
            SpellGreaterHeal ghGTRH = new SpellGreaterHeal(character, statsRHGR);

            castSequence.Add(pwsGR);
            castSequence.Add(penanceBTGRRH);
            castSequence.Add(ghBTGRRH);
            castSequence.Add(ghGTRH);
            castSequence.Add(ghGTRH);
            castSequence.Add(ghGTRH);
            castSequence.Add(ghGTRH);

            DoCalcs();

            if (verbose)
                Name += "\n\nOne tank healing.";
        }

        public override List<string> MeetsRequirements()
        {
            List<string> faults = new List<string>();

            if (stats.PriestSpec != ePriestSpec.Spec_Disc)
            {
                faults.Add("This model is meant for Discipline Tank Healing.");
            }
            if (character.PriestTalents.BorrowedTime == 0)
            {
                faults.Add("This model works best with the Borrowed Time Talents.");
            }
            if (character.PriestTalents.Grace == 0)
            {
                faults.Add("This model works best wtih the Grace Talents.");
            }
            if (character.PriestTalents.RenewedHope == 0)
            {
                faults.Add("This model works best with the Renewed Hope Talents.");
            }
            if (character.PriestTalents.StrengthOfSoul == 0)
            {
                faults.Add("This model works best with the Strength of Soul Talents.");
            }
            if (character.PriestTalents.InnerFocus == 0)
            {
                faults.Add("This model works best with the Inner Focus Talent.");
            }
            if (!character.PriestTalents.GlyphofPowerWordShield)
            {
                faults.Add("This model works best with Glyph of Power Word: Shield.");
            }
            return faults;
        }
    }

    public class PriestSolverDisciplineTank2 : PriestSolver
    {
        public PriestSolverDisciplineTank2(CharacterCalculationsHealPriest calc, CalculationOptionsHealPriest calcOpts, bool verbose)
            : base(calc, calcOpts, verbose)
        {
            Name = PriestModels.modelDiscTank2;
        }

        //
        // PWS, Penance_BT, GH_BT, PWS, GH_BT, GHx2
        public override void Solve()
        {
            float graceBonus = PriestInformation.GetGrace(character.PriestTalents.Grace) * 3;
            float borrowedTime = PriestInformation.GetBorrowedTime(character.PriestTalents.BorrowedTime);
            float renewedHope = PriestInformation.GetRenewedHope(character.PriestTalents.RenewedHope);

            StatsPriest statsGR = new StatsPriest() { BonusHealingDoneMultiplier = graceBonus };
            statsGR.Accumulate(stats);

            StatsPriest statsRHGR = new StatsPriest() { SpellCrit = renewedHope, BonusHealingDoneMultiplier = graceBonus };
            statsRHGR.Accumulate(stats);

            StatsPriest statsBTRHGR = new StatsPriest() { SpellHaste = borrowedTime, SpellCrit = renewedHope, BonusHealingDoneMultiplier = graceBonus };
            statsBTRHGR.Accumulate(stats);

            // BT = Borrowed Time, RH = Renewed Hope, GR = Grace
            SpellPowerWordShield pwsGR = new SpellPowerWordShield(character, statsGR);
            SpellPenance penanceBTGRRH = new SpellPenance(character, statsBTRHGR);
            SpellGreaterHeal ghBTGRRH = new SpellGreaterHeal(character, statsBTRHGR);
            SpellGreaterHeal ghGTRH = new SpellGreaterHeal(character, statsRHGR);

            castSequence.Add(pwsGR);
            castSequence.Add(penanceBTGRRH);
            castSequence.Add(ghBTGRRH);
            castSequence.Add(pwsGR);
            castSequence.Add(ghBTGRRH);
            castSequence.Add(ghGTRH);
            castSequence.Add(ghGTRH);

            DoCalcs();
            if (verbose)
                Name += "\n\nTwo tank healing, keeping Grace up on 2 targets.";
        }
        public override List<string> MeetsRequirements()
        {
            List<string> faults = new List<string>();

            if (stats.PriestSpec != ePriestSpec.Spec_Disc)
            {
                faults.Add("This model is meant for Discipline Tank Healing.");
            }
            if (character.PriestTalents.BorrowedTime == 0)
            {
                faults.Add("This model works best with the Borrowed Time Talents.");
            }
            if (character.PriestTalents.Grace == 0)
            {
                faults.Add("This model works best wtih the Grace Talents.");
            }
            if (character.PriestTalents.RenewedHope == 0)
            {
                faults.Add("This model works best with the Renewed Hope Talents.");
            }
            if (character.PriestTalents.StrengthOfSoul == 0)
            {
                faults.Add("This model works best with the Strength of Soul Talents.");
            }
            if (character.PriestTalents.InnerFocus == 0)
            {
                faults.Add("This model works best with the Inner Focus Talent.");
            }
            if (!character.PriestTalents.GlyphofPowerWordShield)
            {
                faults.Add("This model works best with Glyph of Power Word: Shield.");
            }
            return faults;
        }
    }

    public class PriestSolverDisciplineTank3 : PriestSolver
    {
        public PriestSolverDisciplineTank3(CharacterCalculationsHealPriest calc, CalculationOptionsHealPriest calcOpts, bool verbose)
            : base(calc, calcOpts, verbose)
        {
            Name = PriestModels.modelDiscTank3;
        }

        //
        // PWS, Penance_BT, PROH_BT, PWS, PROH_BT, PWS, PROH_BT, GH, GH
        public override void Solve()
        {
            float graceBonus = PriestInformation.GetGrace(character.PriestTalents.Grace) * 3;
            float borrowedTime = PriestInformation.GetBorrowedTime(character.PriestTalents.BorrowedTime);
            float renewedHope = PriestInformation.GetRenewedHope(character.PriestTalents.RenewedHope);

            StatsPriest statsGR = new StatsPriest() { BonusHealingDoneMultiplier = graceBonus };
            statsGR.Accumulate(stats);

            StatsPriest statsBTGR = new StatsPriest() { SpellHaste = borrowedTime, BonusHealingDoneMultiplier = graceBonus };
            statsBTGR.Accumulate(stats);
            
            StatsPriest statsRHGR = new StatsPriest() { SpellCrit = renewedHope, BonusHealingDoneMultiplier = graceBonus };
            statsRHGR.Accumulate(stats);

            StatsPriest statsBTRHGR = new StatsPriest() { SpellHaste = borrowedTime, SpellCrit = renewedHope, BonusHealingDoneMultiplier = graceBonus };
            statsBTRHGR.Accumulate(stats);

            // BT = Borrowed Time, RH = Renewed Hope, GR = Grace
            SpellPowerWordShield pwsGR = new SpellPowerWordShield(character, statsGR);
            SpellPenance penanceBTGRRH = new SpellPenance(character, statsBTRHGR);
            SpellGreaterHeal ghBTGRRH = new SpellGreaterHeal(character, statsBTRHGR);
            SpellGreaterHeal ghGTRH = new SpellGreaterHeal(character, statsRHGR);
            SpellPrayerOfHealing prohBTGR = new SpellPrayerOfHealing(character, statsBTGR, 3);

            castSequence.Add(pwsGR);
            castSequence.Add(penanceBTGRRH);
            castSequence.Add(prohBTGR);
            castSequence.Add(ghGTRH);
            castSequence.Add(pwsGR);
            castSequence.Add(prohBTGR);
            castSequence.Add(ghGTRH);
            castSequence.Add(pwsGR);
            castSequence.Add(prohBTGR);

            DoCalcs();
            if (verbose)
                Name += "\n\nThree tank healing, keeping Grace up on 3 targets, using Prayer of Healing as filler.";
        }
        public override List<string> MeetsRequirements()
        {
            List<string> faults = new List<string>();

            if (stats.PriestSpec != ePriestSpec.Spec_Disc)
            {
                faults.Add("This model is meant for Discipline Tank Healing.");
            }
            if (character.PriestTalents.BorrowedTime == 0)
            {
                faults.Add("This model works best with the Borrowed Time Talents.");
            }
            if (character.PriestTalents.Grace == 0)
            {
                faults.Add("This model works best wtih the Grace Talents.");
            }
            if (character.PriestTalents.RenewedHope == 0)
            {
                faults.Add("This model works best with the Renewed Hope Talents.");
            }
            if (character.PriestTalents.InnerFocus == 0)
            {
                faults.Add("This model works best with the Inner Focus Talent.");
            }
            if (!character.PriestTalents.GlyphofPowerWordShield)
            {
                faults.Add("This model works best with Glyph of Power Word: Shield.");
            }
            if (!character.PriestTalents.GlyphofPrayerofHealing)
            {
                faults.Add("This model works best with Glyph of Prayer of Healing.");
            }
            return faults;
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

        public override List<string> MeetsRequirements()
        {
            List<string> faults = new List<string>();
            return faults;
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

        public override List<string> MeetsRequirements()
        {
            List<string> faults = new List<string>();
            return faults;
        }
    }
}
