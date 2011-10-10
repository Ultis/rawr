using System;
using System.Collections.Generic;
using Rawr.ModelFramework;

namespace Rawr.Tree
{
    sealed class TreeSolver
    {
        class RandomIntellectEffect
        {
            public double ManaIncrease;
            public SpecialEffect Effect;
            public double Interval;
            public double Chance;

            public RandomIntellectEffect(double manaIncrease, SpecialEffect effect, double interval, double chance)
            {
                this.ManaIncrease = manaIncrease;
                this.Effect = effect;
                this.Interval = interval;
                this.Chance = chance;
            }
        }

        Character character;
        CharacterCalculationsTree calc;
        CalculationOptionsTree opts;
        double PerseveranceHPS;
        double NaturesWardUptime;
        List<RandomIntellectEffect> RandomIntellectEffects;
        double OnUseIntellectProcsMana;
        Stats MeanStats;
        int T11Count;
        int T12Count;
        int T13Count;
        DruidTalents Talents;
        bool Restoration;

        class TreeComputedData
        {
            public double LifebloomRefreshInterval;

            public double LifebloomMPSGain;
        }

        TreeComputedData[] DivisionData;

        public TreeSolver(Character character, CharacterCalculationsTree calc)
        {
            this.character = character;
            this.calc = calc;
            this.opts = (CalculationOptionsTree)character.CalculationOptions;
            if (opts == null)
                opts = new CalculationOptionsTree();
            
            compute();
        }

        void computeBasics()
        {
            BossOptions bossOpts = character.BossOptions;
            calc.FightLength = bossOpts.BerserkTimer;

            character.SetBonusCount.TryGetValue("Stormrider's Vestments", out T11Count);
            character.SetBonusCount.TryGetValue("Obsidian Arborweave Vestments", out T12Count);
            character.SetBonusCount.TryGetValue("Deep Earth Vestments", out T13Count);
            Talents = character.DruidTalents;
            Restoration = (opts != null) ? opts.Restoration : true;
        }

        void computeStats()
        {
            RandomIntellectEffects = new List<RandomIntellectEffect>();
            RandomIntellectEffects.Add(new RandomIntellectEffect(0, null, 0, 0));
            OnUseIntellectProcsMana = 0.0f;

            calc.MeanMana = calc.BasicStats.Mana;

            List<KeyValuePair<double, SpecialEffect>> hasteProcsList = new List<KeyValuePair<double, SpecialEffect>>();

            // ToL, NG, Heroism, Shard of Woe, etc.
            List<SpecialEffect> dividingEffectsList = new List<SpecialEffect>();
            List<int> dividingEffectBucketsList = new List<int>();

            KeyValuePair<double, SpecialEffect>[] hasteProcs;
            Stats statsDividing = new Stats();
            Stats statsProcs = new Stats();

            Dictionary<Trigger, float> triggerIntervals = new Dictionary<Trigger,float>();
            Dictionary<Trigger, float> triggerChances = new Dictionary<Trigger, float>();

            triggerIntervals[Trigger.Use] = 0;

            triggerIntervals[Trigger.HealingSpellCast] = triggerIntervals[Trigger.HealingSpellCrit] = triggerIntervals[Trigger.HealingSpellHit] =
                triggerIntervals[Trigger.SpellCast] = triggerIntervals[Trigger.SpellCrit] = triggerIntervals[Trigger.SpellHit] =
                triggerIntervals[Trigger.DamageOrHealingDone] = (float)calc.ProcTriggerInterval;

            triggerIntervals[Trigger.HoTTick] = (float)calc.ProcPeriodicTriggerInterval;

            foreach(Trigger trigger in triggerIntervals.Keys)
                triggerChances[trigger] = 1;

            // NOTE: this ignores crit from procs, but hopefully this shouldn't matter much
            triggerChances[Trigger.HealingSpellCrit] = triggerChances[Trigger.SpellCrit] =
                StatConversion.GetSpellCritFromIntellect(calc.BasicStats.Intellect) + StatConversion.GetSpellCritFromRating(calc.BasicStats.CritRating) + calc.BasicStats.SpellCrit;

            foreach (SpecialEffect effect in calc.BasicStats.SpecialEffects())
            {
                if (CalculationsTree.RelevantTriggers.Contains(effect.Trigger))
                {
                    if (effect.Stats.Intellect > 0 || effect.Stats.HighestStat > 0)
                    {
                        double mana = StatConversion.GetManaFromIntellect((effect.Stats.Intellect + effect.Stats.HighestStat) * (1 + calc.BasicStats.BonusIntellectMultiplier)) * (1 + calc.BasicStats.BonusManaMultiplier);
                        double avgMana = mana * effect.GetAverageFactor(triggerIntervals[effect.Trigger], triggerChances[effect.Trigger], 3.0f, character.BossOptions.BerserkTimer);
                        if (effect.Trigger != Trigger.Use)
                            RandomIntellectEffects.Add(new RandomIntellectEffect(mana - avgMana, effect, triggerIntervals[effect.Trigger], triggerChances[effect.Trigger]));

                        if (effect.Trigger == Trigger.Use && effect.Cooldown <= 180.0f)
                            OnUseIntellectProcsMana += mana - avgMana;
                        calc.MeanMana += avgMana;
                    }

                    Stats stats = effect.GetAverageStats(triggerIntervals, triggerChances, 3.0f, character.BossOptions.BerserkTimer);
                    
                    if (effect.Trigger == Trigger.Use
                        && effect.MaxStack <= 1
                        && (effect.Stats is CalculationsTree.TreeOfLifeStats || effect.Stats.HasteRating > 0 || effect.Stats.SpellHaste > 0 || effect.Stats.Intellect > 0 || effect.Stats.SpellPower > 0 || effect.Stats.CritRating > 0 || effect.Stats.SpellCrit > 0 || effect.Stats.MasteryRating > 0 || effect.Stats.NatureSpellsManaCostReduction != 0 || effect.Stats.SpellsManaCostReduction != 0 || effect.Stats.ManaCostReductionMultiplier != 0)
                        )
                    {
                        dividingEffectsList.Add(effect);
                        int bucket = -1;
                        if(opts.SeparateHasteEffects && (effect.Stats.HasteRating > 0 || effect.Stats.SpellHaste > 0))
                            bucket = 0;
                        dividingEffectBucketsList.Add(bucket);
                        statsDividing.Accumulate(stats);
                    }
                    else
                    {
                        if (effect.Stats.HasteRating > 0 || effect.Stats.SpellHaste > 0)
                        {
                            double uptime = effect.GetAverageUptime(triggerIntervals[effect.Trigger], triggerChances[effect.Trigger], 3.0f, character.BossOptions.BerserkTimer);
                            hasteProcsList.Add(new KeyValuePair<double, SpecialEffect>(uptime, effect));
                            stats.HasteRating = 0;
                            stats.SpellHaste = 0;
                        }
                        statsProcs.Accumulate(stats);
                    }
                }
            }

            MeanStats = new Stats();
            MeanStats.Accumulate(statsProcs);
            MeanStats.Accumulate(statsDividing);

            // XXX: this fixes Tyrande's Favorite Doll, but seems to be a bug in Rawr
            MeanStats.Mp5 = MeanStats.ManaRestore * 5;
            MeanStats.ManaRestore = 0;

            if (T11Count >= 4)
                MeanStats.Spirit += 540 * opts.HarmonyPeriodicRate;

            CalculationsTree.FinalizeStats(MeanStats, calc.BasicStats);
            MeanStats.Accumulate(calc.BasicStats);

            hasteProcs = hasteProcsList.ToArray();

            calc.Division = FightDivision.ComputeNaive(dividingEffectsList.ToArray(), calc.FightLength, dividingEffectBucketsList.ToArray());

            if (opts.ActivityRate < 1.0f)
            {                                
                SpecialEffect[] newEffects = new SpecialEffect[calc.Division.Effects.Length + 1];
                Array.Copy(calc.Division.Effects, newEffects, calc.Division.Effects.Length);
                newEffects[calc.Division.Effects.Length] = CalculationsTree.InactiveEffect;

                int[] newEffectMasks = new int[calc.Division.Count + 1];
                Array.Copy(calc.Division.EffectMasks, newEffectMasks, calc.Division.Count);
                newEffectMasks[calc.Division.Count] = 1 << calc.Division.Effects.Length;

                double[] newFractions = new double[calc.Division.Count + 1];
                Array.Copy(calc.Division.Fractions, newFractions, calc.Division.Count);

                double curActivityRate = 1;
                for (int i = 0; i < calc.Division.Count; ++i)
                {
                    if (calc.Division.EffectMasks[i] == 0)
                    {
                        double t = Math.Min(1 - opts.ActivityRate, newFractions[i]);
                        curActivityRate -= t;
                        newFractions[i] -= t;
                    }
                }

                if (curActivityRate > opts.ActivityRate)
                {
                    for (int i = 0; i < calc.Division.Count; ++i)
                    {
                        newFractions[i] *= opts.ActivityRate;
                    }
                }

                newFractions[calc.Division.Count] = 1 - opts.ActivityRate;

                calc.Division.Fractions = newFractions;
                calc.Division.Effects = newEffects;
                calc.Division.EffectMasks = newEffectMasks;
                ++calc.Division.Count;
            }

            double baseSpellPower = (float)(MeanStats.SpellPower + Math.Max(0f, calc.BasicStats.Intellect - 10));
            // TODO: does nurturing instinct actually work like this?
            baseSpellPower += Talents.NurturingInstinct * 0.5 * calc.BasicStats.Agility;
            baseSpellPower *= 1 + calc.BasicStats.BonusSpellPowerMultiplier;
            calc.BaseSpellPower = baseSpellPower;

            calc.Stats = new TreeStats[calc.Division.Count];

            for (int div = 0; div < calc.Division.Fractions.Length; ++div)
            {
                Stats statsWithOnUse = new Stats();
                bool treeOfLifeActive = false;
                for (int j = 0; j < calc.Division.Effects.Length; ++j)
                {
                    if ((calc.Division.EffectMasks[div] & (1 << j)) != 0)
                    {
                        if (calc.Division.Effects[j].Stats is CalculationsTree.TreeOfLifeStats)
                            treeOfLifeActive = true;
                        else
                            statsWithOnUse.Accumulate(calc.Division.Effects[j].Stats);
                    }
                }

                statsWithOnUse.Accumulate(statsProcs);

                // XXX: this fixes Tyrande's Favorite Doll, but seems to be a bug in Rawr
                statsWithOnUse.Mp5 = statsWithOnUse.ManaRestore * 5;
                statsWithOnUse.ManaRestore = 0;

                CalculationsTree.FinalizeStats(statsWithOnUse, calc.BasicStats);
                statsWithOnUse.Accumulate(calc.BasicStats);

                calc.Stats[div] = new TreeStats(character, statsWithOnUse, hasteProcs, treeOfLifeActive ? 1.0 : 0.0);
            }

        }

        void computeActions()
        {
            calc.Spells = new ComputedSpell[calc.Division.Count][];
            DivisionData = new TreeComputedData[calc.Division.Count];
            calc.Actions = new ContinuousAction[calc.Division.Count][];
            for (int div = 0; div < calc.Division.Fractions.Length; ++div)
            {
                DivisionData[div] = new TreeComputedData();
                calc.Spells[div] = computeDivisionSpells(calc.Stats[div], DivisionData[div]);
                computeDivisionSpellActions(calc.Stats[div], DivisionData[div], calc.Spells[div]);
                if (opts.ActivityRate == 1.0f || (div + 1) != calc.Division.Fractions.Length)
                    calc.Actions[div] = computeDivisionActions(calc.Stats[div], DivisionData[div], calc.Spells[div]);
                else
                    calc.Actions[div] = new ContinuousAction[(int)TreeAction.Count];
            }
        }

        void computeManaRegen()
        {            
            // TODO: maybe share this innervate computation with Rawr.Moonkin?
            double bestInnervateGains = double.NegativeInfinity;
            double innervateRatio;
            double innervateCooldown = 180;
            double innervateDuration = 10; // haste doesn't affect Innervate
            calc.InnervateMana = 0;
            calc.InnervateSize = 0;
            calc.Innervates = 0;
            calc.InnervateEffect = null;
            calc.InnervateEffectDelay = 0;

            if (opts.InnervateOther)
            {
                if (character.DruidTalents.GlyphOfInnervate)
                    innervateRatio = 0.1;
                else
                    innervateRatio = 0;
            }
            else
                innervateRatio = 0.2 + character.DruidTalents.Dreamstate * 0.15;
            
            innervateRatio += opts.ExternalInnervates * 0.05;

            if (innervateRatio > 0)
            {
                /* TODO: support using this burstMPS value in the before and after the first and last innervates
                double burstMPS = 0;
                for (int model = 1; model < calc.Solutions.Length; model += 2)
                    burstMPS = Math.Max(burstMPS, calc.Solutions[model].Distribution.TotalMPS);
                 */

                List<RandomIntellectEffect> randomIntellectEffects;
                if(opts.TimedInnervates)
                    randomIntellectEffects = RandomIntellectEffects;
                else
                {
                    randomIntellectEffects = new List<RandomIntellectEffect>();
                    randomIntellectEffects.Add(new RandomIntellectEffect(0, null, 0, 0));
                }
                foreach (RandomIntellectEffect effect in randomIntellectEffects)
                {
                    /* First of all, after the first innervate it always makes sense to innervate as soon as possible in our model,
                     * since we are spending mana at a higher rate than the innervate regen due to the need to spend initial mana.
                     * 
                     * Hence, the number of innervates can be either the maximum possible, or one less, if starting late.
                     * The first and last innervates might be wasted due to capping for the first, and not being able to spend the mana for the last.
                     * To model this, we compute the average Innervate mana regen and use it to cap the mana gain from the first and last innervates.
                     * 
                     * Note that it can be assumed that the first Innervate is done exactly as soon as it wouldn't cap mana (which
                     * thus determines the number of innervates), but code-wise it's simpler to just try both values for the number of innervates.
                     * 
                     * This model has the significant advantage that mana regeneration is continuous in the fight length,
                     * which is good especially because the effective innervate cooldown is random, if synced to random procs.
                     * 
                     * TODO: it would be better to use a whole-fight probability distribution of proc trigger intervals, rather than taking the average
                     * TODO: we perhaps don't necessarily spend mana evenly enough to not cap the second innervate (due to saving for Tree of Life), although we currently ignore this
                     */

                    double delay = 0;
                    if (effect.Effect != null)
                        delay = effect.Effect.Cooldown * 0.5 + effect.Interval * (1.0 / effect.Chance - 0.5);

                    for (int i = 0; i < 2; ++i)
                    {
                        double innervateMana = calc.MeanMana + effect.ManaIncrease;
                        if (opts.BoostIntellectBeforeInnervate)
                            innervateMana += OnUseIntellectProcsMana;

                        double innervateDelay = innervateMana;

                        double innervateGain = innervateMana * innervateRatio;
                        double innervateInterval = innervateCooldown + delay;
                        double numInnervates = Math.Floor((calc.FightLength + innervateDuration) / innervateInterval) + i;
                        if (numInnervates == 0)
                            continue;

                        if (numInnervates > 1)
                        {
                            /* Cap first/last innervate MPS using mana usage between innervates
                             * 
                             * Innervate is not instantaneous, but we model it by considering when it *ends*, with the
                             * exception of the latest, which we assume instantaneous, so we gain 10 seconds of edge time */
                            double innervateEdgeTime = calc.FightLength - (numInnervates - 1) * innervateInterval + innervateDuration;

                            double innerManaUsage = (calc.BasicStats.Mana / (calc.FightLength - innervateEdgeTime)) + innervateGain / innervateInterval;

                            double lostInnervates = 2 - innervateEdgeTime * innerManaUsage / innervateGain;
                            if (lostInnervates >= 0)
                                numInnervates -= lostInnervates;
                        }
                        else if (calc.FightLength < innervateDuration)
                        {
                            // this is only for completeness, <10 sec fights won't be modelled that well anyway :)
                            numInnervates = innervateDuration / calc.FightLength;
                        }

                        double innervateGains = numInnervates * innervateGain;

                        if (innervateGains > bestInnervateGains)
                        {
                            bestInnervateGains = innervateGains;
                            calc.InnervateMana = innervateMana;
                            calc.Innervates = numInnervates;
                            calc.InnervateEffectDelay = delay;
                            calc.InnervateEffect = effect.Effect;
                            calc.InnervateSize = innervateGain;
                        }
                    }
                }
            }

            // TODO: should estimate this properly
            double revitalizeDelay = 1.0f;

            calc.ManaPoolRegen = calc.BasicStats.Mana / calc.FightLength;
            calc.BaseRegen = MeanStats.Mp5 / 5f;
            calc.SpiritRegen = opts.Restoration ? (StatConversion.GetSpiritRegenSec(MeanStats.Spirit, MeanStats.Intellect) / 2) : 0;
            calc.ReplenishmentRegen = MeanStats.ManaRestoreFromMaxManaPerSecond * calc.MeanMana;
            calc.InnervateRegen = calc.Innervates * calc.InnervateSize / calc.FightLength;
            calc.RevitalizeRegen = character.DruidTalents.Revitalize * (1.0f / (12.0f + revitalizeDelay)) * 0.01f * calc.MeanMana;
            calc.PotionRegen = MeanStats.ManaRestore / calc.FightLength;
            calc.ManaRegen = calc.ManaPoolRegen + calc.BaseRegen + calc.SpiritRegen + calc.ReplenishmentRegen + calc.InnervateRegen + calc.RevitalizeRegen + calc.PotionRegen;
        }

        // TODO: the rejuvenation from Nature's Ward currently isn't accounted for Nature's Bounty or swiftmendability (might be a good thing...)
        void computeSelfHealing()
        {
            PerseveranceHPS = 0;
            NaturesWardUptime = 0;

/*
            // TODO: re-enable once the BossHandler changes get merged

            if (character.DruidTalents.Perseverance > 0)
            {
                double dps = character.BossOptions.GetNonTankDPS(character, MeanStats, PLAYER_ROLES.RaidHealer);
                PerseveranceHPS = dps * (0.02 * character.DruidTalents.Perseverance);
            }

            if (character.DruidTalents.NaturesWard > 0)
            {
                // assume that pure DoTs don't cause a proc
                // TODO: this list is probably generally too inclusive, need to look at all bosses one by one

                // TODO: re-enable once the BossHandler changes get merged
                List<Attack> attacks = character.BossOptions.GetFilteredAttackList(1 << (int)ATTACK_TYPES.AT_RANGED).FindAll(attack => attack.DamagePerHit > 0 && attack.AffectsRole[PLAYER_ROLES.RaidHealer]);
                if(attacks.Count > 0)
                {
                    float cooldown = 6.0f; // according to comments on Wowhead
                    float duration = 12.0f; // haste affects this slightly, but let's ignore that for now
                    double naturesWardChance = character.DruidTalents.NaturesWard * 0.5;

                    // approximate this by reducing to minimum attack speed
                    // TODO: can it be done better?

                    double minAttackSpeed = float.PositiveInfinity;
                    foreach (Attack attack in attacks)
                        minAttackSpeed = Math.Min(minAttackSpeed, attack.AttackSpeed);

                    double nonTriggerChance = 1.0;
                    foreach (Attack attack in attacks)
                    {
                        double attackChance = Math.Min(1.0, (double)attack.MaxNumTargets / Math.Max(1, character.BossOptions.GetEligiblePlayersForAttack(attack)));
                        nonTriggerChance *= 1 - (attackChance * minAttackSpeed / attack.AttackSpeed * naturesWardChance);
                    }

                    SpecialEffect specialEffect = new SpecialEffect(Trigger.DamageTaken, new Stats(), duration, cooldown, 1.0f, 1);
                    NaturesWardUptime = specialEffect.GetAverageUptime((float)minAttackSpeed, (float)(1 - nonTriggerChance));
                }
            }
 */
        }

        void computeModels(bool infMana)
        {
            for (int model = infMana ? 1 : 0; model < 4; model += 2)
            {
                int[] candidates;
                bool isTank = (model >> 1) == 1;
                ActionDistribution[] dists = isTank ? computeTankHealing(infMana, out candidates) : computeRaidHealing(infMana, out candidates);

                ActionOptimization.AddBestActions(dists, calc.Division.Fractions, candidates, isTank ? opts.TankUnevenlyAllocatedFillerMana : opts.RaidUnevenlyAllocatedFillerMana);
                calc.Solutions[model] = new ActionDistributionsByDivision(calc.Division, dists);
            }
        }

        void computeIteration()
        {
            computeBasics();
            computeStats();
            computeActions();
            computeSelfHealing();

            computeModels(true);
            computeManaRegen();
            computeModels(false);

            // this is just for display purposes
            for (int model = 0; model < calc.Solutions.Length; ++model)
            {
                calc.Solutions[model].Distribution.MaxMPS = calc.ManaRegen;
                for(int i = 0; i < calc.Solutions[model].Distributions.Length; ++i)
                    calc.Solutions[model].Distributions[i].MaxMPS = calc.ManaRegen;
            }

            double[] weights = new double[(int)PointsTree.Count];
            weights[(int)PointsTree.RaidSustained] = (1 - opts.TankRatio) * (1 - opts.RaidBurstRatio);
            weights[(int)PointsTree.RaidBurst] = (1 - opts.TankRatio) * opts.RaidBurstRatio;
            weights[(int)PointsTree.TankSustained] = opts.TankRatio * (1 - opts.TankBurstRatio);
            weights[(int)PointsTree.TankBurst] = opts.TankRatio * opts.TankBurstRatio;

            double CPS = 0;
            double TPS = 0;
            calc.OverallPoints = 0;
            for(int i = 0; i < (int)PointsTree.Count; ++i)
            {
                calc.SubPoints[i] = (float)(calc.Solutions[i].Distribution.TotalEPS() * weights[i]);
                calc.OverallPoints += calc.SubPoints[i];
                CPS += calc.Solutions[i].Distribution.TotalCPS() * weights[i];
                TPS += calc.Solutions[i].Distribution.TotalTPS() * weights[i];
            }

            calc.ProcTriggerInterval = 1 / (CPS + TPS);
            calc.ProcPeriodicTriggerInterval = 1 / TPS;
        }

        void compute()
        {
            calc.ProcTriggerInterval = opts.ProcTriggerInterval;
            calc.ProcPeriodicTriggerInterval = opts.ProcPeriodicTriggerInterval;
            for (int i = 0; i <= opts.ProcTriggerIterations; ++i)
                computeIteration();
        }

        double getCritMultiplier(TreeStats stats, double extraCritChance, double livingSeed)
        {
            double crit = Math.Min(1, stats.SpellCrit + extraCritChance);
            return crit * 2.0 * (1.0 + stats.BonusCritHealMultiplier) * (1 + livingSeed) + (1 - crit);
        }

        // cast Rejuvenation rjn times to get Nature's Bounty, then cast Nourish until it drops off
        // TODO: do we have some extra time before it drops off?
        DiscreteAction buildNourishNB(TreeStats stats, DiscreteAction rj, double rjduration, int rjn, double rjWeight, DiscreteAction nourish)
        {
            DiscreteAction action = new DiscreteAction();
            double nourishCasts = (rjduration - rj.Time * rjn) / ((1.0f - 0.1f * Talents.NaturesBounty) * nourish.Time);
            action.Time = rjduration;
            action.Periodic = rj.Periodic * rjn * rjWeight;
            action.Direct = rj.Direct * rjn * rjWeight + nourishCasts * nourish.Direct;
            action.Mana = rj.Mana * rjn + nourishCasts * nourish.Mana;
            action.Casts = rj.Casts * rjn + nourishCasts;
            action.Ticks = rj.Ticks * rjn;
            return action;
        }

        // cast Lifebloom on raid during ToL, then an amortized Healing Touch using the average number of clearcasts from the Lifebloom
        DiscreteAction buildTolLbCcHt(TreeStats stats, double lifebloomTicks, DiscreteAction tollb, double tollbWeight, DiscreteAction ccht)
        {
            double cchtsPerLifebloom = lifebloomTicks * 0.02f * Talents.MalfurionsGift;

            DiscreteAction action = new DiscreteAction();
            action.Time = tollb.Time + cchtsPerLifebloom * ccht.Time;
            action.Direct = tollb.Direct * tollbWeight + cchtsPerLifebloom * ccht.Direct;
            action.Periodic = tollb.Periodic * tollbWeight;
            action.Mana = tollb.Mana;
            action.Casts = cchtsPerLifebloom;
            action.Ticks = lifebloomTicks + cchtsPerLifebloom;
            return action;
        }

        DiscreteAction applyCC(DiscreteAction action)
        {
            action.Mana = 0;
            return action;
        }

        /* Spell data is computed in the following stages:
         * 1. SpellData with constants
         * 2. ComputedSpell containing computed data for a generic target (thus excluding crit, mastery, targets, effective healing)
         * 3. ComputedSpell with RaidAction and TankAction filled with information except effective healing and multiplication by number of targets
         * 4. SpellAction adding effective healing, targets and strange options, plus special SpellActions containing combination of spells
         * 5. Actions containing the SpellActions converted to EPS/MPS/time format
         * 
         * Note that Tank mostly means "target always with hots, hit frequently" while Raid means "target which might have hots (from wild growth), hit rarely"
         */

        ComputedSpell[] computeDivisionSpells(TreeStats stats, TreeComputedData data)
        {
            ComputedSpell[] spells = new ComputedSpell[(int)TreeSpell.Count];
            for (int i = 0; i < (int)TreeSpell.Count; ++i)
                spells[i] = new ComputedSpell(CalculationsTree.SpellData[i], stats);

            spells[(int)TreeSpell.Tranquility].DirectMultiplier *= 4 * 5;
            spells[(int)TreeSpell.Tranquility].TickMultiplier *= 4 * 5;

            #region Talents
            double rejuvenationInstantTicks = 0;

            spells[(int)TreeSpell.Swiftmend].ExtraDirectBonus += 0.02f * Talents.Genesis;

            spells[(int)TreeSpell.Rejuvenation].ExtraTickBonus += Talents.BlessingOfTheGrove * 0.02f;

            spells[(int)TreeSpell.Nourish].TimeReductionMS += 250 * Talents.Naturalist;
            spells[(int)TreeSpell.HealingTouch].TimeReductionMS += 250 * Talents.Naturalist;

            spells[(int)TreeSpell.Rejuvenation].ExtraTickBonus += 0.05f * Talents.ImprovedRejuvenation;
            spells[(int)TreeSpell.Swiftmend].ExtraTickBonus += 0.05f * Talents.ImprovedRejuvenation;
            spells[(int)TreeSpell.Swiftmend].ExtraDirectBonus += 0.05f * Talents.ImprovedRejuvenation;

            // according to Paragon's Anaram posting on ElitistJerks, Efflorescence double-dips Master Shapeshifter and Harmony
            spells[(int)TreeSpell.Swiftmend].TickMultiplier *= 0.04f * Talents.Efflorescence * (1.0f + Talents.MasterShapeshifter * 0.04f) * (1.0f + stats.Harmony);

            spells[(int)TreeSpell.Nourish].ExtraDirectBonus += 0.05f * Talents.EmpoweredTouch;
            spells[(int)TreeSpell.HealingTouch].ExtraDirectBonus += 0.05f * Talents.EmpoweredTouch;
            spells[(int)TreeSpell.Regrowth].ExtraDirectBonus += 0.05f * Talents.EmpoweredTouch;

            // formula from TreeCalcs
            // TODO: test this in-game
            rejuvenationInstantTicks = Talents.GiftOfTheEarthmother * 0.05f * Math.Floor(4.0f * (1 + StatConversion.GetSpellHasteFromRating((float)stats.Haste.HasteRating)) + 0.5f) * 1.0135f;
            spells[(int)TreeSpell.Lifebloom].DirectMultiplier *= 1 + Talents.GiftOfTheEarthmother * 0.05f;

            if (Talents.SwiftRejuvenation > 0)
                spells[(int)TreeSpell.Rejuvenation].TimeReductionMS = 500;
            #endregion

            #region Glyphs
            if (Talents.GlyphOfRejuvination)
                spells[(int)TreeSpell.Rejuvenation].ExtraTickBonus += 0.1f;

            if (Talents.GlyphOfRegrowth)
                spells[(int)TreeSpell.Regrowth].ExtraDurationMS += (Talents.GlyphOfRegrowth ? (int)(Math.Min(opts.GlyphOfRegrowthExtraDuration, calc.FightLength) * 1000.0) : 0);
            #endregion

            for (int i = 0; i < (int)TreeSpell.Count; ++i)
            {
                if (i != (int)TreeSpell.HealingTouch && i != (int)TreeSpell.WildGrowth)
                    spells[i].ComputeTiming();
            }

            if (T13Count >= 4)
            {
                // here we assume that the tick structure is computed as if the spell just had double duration
                // this is NOT the same as doubling the amount of ticks, due to rounding

                // also we assume that overhealing ratio doesn't depend on duration in our model
                // if the model is changed to do that, then this averaging will need to be done much later

                for (int i = 0; i < CalculationsTree.T13Spells.Length; ++i)
                {
                    ComputedSpell extspell = new ComputedSpell(CalculationsTree.T13SpellData[i], stats);
                    extspell.ComputeTiming();

                    ComputedSpell spell = spells[(int)CalculationsTree.T13Spells[i]];
                    spell.Ticks = 0.9 * spell.Ticks + 0.1 * extspell.Ticks;
                    spell.TPS = 0.9 * spell.TPS + 0.1 * extspell.TPS;
                    spell.Duration = 0.9 * spell.Duration + 0.1 * extspell.Duration;
                }
            }

            // optimization to avoid duplicating computations
            spells[(int)TreeSpell.HealingTouch].Action.Time = spells[(int)TreeSpell.Nourish].Action.Time;
            spells[(int)TreeSpell.WildGrowth].Action.Time = spells[(int)TreeSpell.Swiftmend].Action.Time;
            spells[(int)TreeSpell.WildGrowth].Ticks = spells[(int)TreeSpell.Swiftmend].Ticks;
            spells[(int)TreeSpell.WildGrowth].Duration = spells[(int)TreeSpell.Swiftmend].Duration;
            spells[(int)TreeSpell.WildGrowth].TPS = spells[(int)TreeSpell.Swiftmend].TPS;

            for (int i = 0; i < (int)TreeSpell.Count; ++i)
                spells[i].ComputeRest();

            if (rejuvenationInstantTicks > 0)
            {
                spells[(int)TreeSpell.Rejuvenation].Action.Direct += rejuvenationInstantTicks * spells[(int)TreeSpell.Rejuvenation].Tick;
                ++spells[(int)TreeSpell.Rejuvenation].Action.Ticks;
            }

            #region Cooldowns
            spells[(int)TreeSpell.WildGrowth].Action.Cooldown = 8 + opts.WildGrowthCastDelay;
            spells[(int)TreeSpell.Tranquility].Action.Cooldown = 8 * 60 - (150 * Talents.MalfurionsGift) + opts.TranquilityCastDelay;
            spells[(int)TreeSpell.Swiftmend].Action.Cooldown = 15 + opts.SwiftmendCastDelay;
            #endregion

            return spells;
        }

        void computeDivisionSpellActions(TreeStats stats, TreeComputedData data, ComputedSpell[] spells)
        {
            for (int i = 0; i < (int)TreeSpell.Count; ++i)
            {
                spells[i].RaidAction = spells[i].Action;
                spells[i].TankAction = spells[i].Action;
            }

            double lifebloomExtraDirectCrit = 0;
            double lifebloomExtraTickCrit = 0;
            double regrowthExtraCrit = 0;
            double tankLivingSeed, raidLivingSeed;

            #region Talents
            if (T11Count >= 2)
                lifebloomExtraTickCrit += 0.05f;

            tankLivingSeed = raidLivingSeed = 0.1f * Talents.LivingSeed;
            raidLivingSeed *= opts.LivingSeedEH;

            regrowthExtraCrit += 0.2f * Talents.NaturesBounty;

            if (Talents.GlyphOfLifebloom)
            {
                lifebloomExtraDirectCrit += 0.1f;
                lifebloomExtraTickCrit += 0.1f;
            }
            #endregion

            #region Crit
            double defaultCritMultiplier = getCritMultiplier(stats, 0, 0);
            double raidLivingSeedCritMultiplier = getCritMultiplier(stats, 0, raidLivingSeed);
            double tankLivingSeedCritMultiplier = getCritMultiplier(stats, 0, tankLivingSeed);

            spells[(int)TreeSpell.Lifebloom].MultiplyDirect(getCritMultiplier(stats, lifebloomExtraDirectCrit, 0));
            spells[(int)TreeSpell.Lifebloom].MultiplyPeriodic(getCritMultiplier(stats, lifebloomExtraTickCrit, 0));

            spells[(int)TreeSpell.WildGrowth].MultiplyPeriodic(defaultCritMultiplier);

            spells[(int)TreeSpell.Tranquility].Multiply(defaultCritMultiplier);

            // gotem doesn't crit, but is affected by Symbiosis, according to TreeCalcs
            spells[(int)TreeSpell.Rejuvenation].MultiplyPeriodic(defaultCritMultiplier);

            spells[(int)TreeSpell.Swiftmend].RaidAction.Direct *= raidLivingSeedCritMultiplier;
            spells[(int)TreeSpell.Swiftmend].TankAction.Direct *= tankLivingSeedCritMultiplier;
            spells[(int)TreeSpell.Swiftmend].MultiplyPeriodic(defaultCritMultiplier);

            spells[(int)TreeSpell.HealingTouch].RaidAction.Direct *= raidLivingSeedCritMultiplier;
            spells[(int)TreeSpell.HealingTouch].TankAction.Direct *= tankLivingSeedCritMultiplier;

            spells[(int)TreeSpell.Nourish].RaidAction.Direct *= raidLivingSeedCritMultiplier;
            spells[(int)TreeSpell.Nourish].TankAction.Direct *= tankLivingSeedCritMultiplier;

            spells[(int)TreeSpell.Regrowth].RaidAction.Direct *= getCritMultiplier(stats, regrowthExtraCrit, raidLivingSeed);
            spells[(int)TreeSpell.Regrowth].TankAction.Direct *= getCritMultiplier(stats, regrowthExtraCrit, tankLivingSeed);
            spells[(int)TreeSpell.Regrowth].MultiplyPeriodic(getCritMultiplier(stats, regrowthExtraCrit, 0));
            #endregion

            #region Nourish HoTs
            spells[(int)TreeSpell.Nourish].RaidAction.Direct *= 1.0 + opts.NourishHoTRate * 0.2;
            spells[(int)TreeSpell.Nourish].TankAction.Direct *= 1.2;
            #endregion
        }

        ContinuousAction[] computeDivisionActions(TreeStats stats, TreeComputedData data, ComputedSpell[] spells)
        {
            double healingTouchNSReduction = 0;
            bool swiftmendEatsRejuvenation = true;
            double swiftmendExtraTargets = 0;
            double lifebloomManaPerTick = 0;

            if (T12Count >= 2)
                lifebloomManaPerTick += CalculationsTree.BaseMana * 0.01 * 0.4;

            if (T12Count >= 4)
                swiftmendExtraTargets += opts.SwiftmendEH;

            if (Talents.GlyphOfSwiftmend)
                swiftmendEatsRejuvenation = false;

            if (Talents.GlyphOfHealingTouch)
                healingTouchNSReduction = 10;

            data.LifebloomMPSGain = lifebloomManaPerTick * spells[(int)TreeSpell.Lifebloom].TPS;

            DiscreteAction[] actions = new DiscreteAction[(int)TreeAction.Count];

            for (int i = 0; i < spells.Length; ++i)
            {
                actions[i] = spells[i].RaidAction;
                actions[spells.Length + i] = spells[i].TankAction;
            }

            #region Effective healing
            actions[(int)TreeAction.RaidTolLb].Direct *= opts.ToLLifebloomEH;
            actions[(int)TreeAction.RaidTolLb].Periodic *= opts.ToLLifebloomEH;
            actions[(int)TreeAction.RaidRejuvenation].Periodic *= opts.RejuvenationEH;
            actions[(int)TreeAction.RaidHealingTouch].Direct *= opts.HealingTouchEH;
            actions[(int)TreeAction.RaidNourish].Direct *= opts.NourishEH;
            actions[(int)TreeAction.RaidRegrowth].Direct *= opts.NourishEH;
            actions[(int)TreeAction.RaidRegrowth].Periodic *= opts.NourishEH;
            actions[(int)TreeAction.RaidWildGrowth].Periodic *= opts.WildGrowthEH;
            actions[(int)TreeAction.RaidSwiftmend].Periodic *= opts.SwiftmendEH;
            actions[(int)TreeAction.RaidTranquility].Direct *= opts.TranquilityEH;
            actions[(int)TreeAction.RaidTranquility].Periodic *= opts.TranquilityEH;
            #endregion

            #region Targets
            double wgTargets = (Talents.GlyphOfWildGrowth ? 6 : 5) + stats.TreeOfLifeUptime * 2;

            actions[(int)TreeAction.TankWildGrowth].Periodic += (wgTargets - 1) * actions[(int)TreeAction.RaidWildGrowth].Periodic * opts.TankRaidHealingWeight;
            actions[(int)TreeAction.RaidWildGrowth].Periodic *= wgTargets;

            actions[(int)TreeAction.TankWildGrowth].Ticks *= wgTargets;
            actions[(int)TreeAction.RaidWildGrowth].Ticks *= wgTargets;
            
            actions[(int)TreeAction.TankSwiftmend].Direct *= (1 + swiftmendExtraTargets * stats.DirectHealMultiplier * opts.TankRaidHealingWeight);
            actions[(int)TreeAction.RaidSwiftmend].Direct *= (1 + swiftmendExtraTargets * stats.DirectHealMultiplier);

            actions[(int)TreeAction.TankSwiftmend].Casts *= (1 + swiftmendExtraTargets);
            actions[(int)TreeAction.RaidSwiftmend].Casts *= (1 + swiftmendExtraTargets);

            actions[(int)TreeAction.TankSwiftmend].Periodic *= 1 + opts.TankRaidHealingWeight * Math.Max(3 * opts.EfflorescenceEH - 1.0, 0);
            actions[(int)TreeAction.RaidSwiftmend].Periodic *= 3 * opts.EfflorescenceEH;

            actions[(int)TreeAction.TankSwiftmend].Ticks *= Math.Max(3 * opts.EfflorescenceEH, 1);
            actions[(int)TreeAction.RaidSwiftmend].Ticks *= 3 * opts.EfflorescenceEH;
            #endregion

            #region Swiftmend
            if (swiftmendEatsRejuvenation)
                actions[(int)TreeAction.RaidSwiftmend].Direct -= actions[(int)TreeAction.RaidRejuvenation].Periodic * 0.5f;
            #endregion

            #region Lifebloom
            if (stats.TreeOfLifeUptime > 0)
            {
                // TODO: figure out how 2T12 works in ToL and adjust the code as needed
                //actions[(int)TreeAction.RaidTolLb].Mana -= lifebloomManaPerTick * spells[(int)TreeSpell.Lifebloom].Ticks;
                //actions[(int)TreeAction.TankTolLb].Mana -= lifebloomManaPerTick * spells[(int)TreeSpell.Lifebloom].Ticks;
            }
            else
            {
                actions[(int)TreeAction.RaidTolLb].Direct = 0;
                actions[(int)TreeAction.RaidTolLb].Periodic = 0;
                actions[(int)TreeAction.TankTolLb].Direct = 0;
                actions[(int)TreeAction.TankTolLb].Periodic = 0;
            }
            #endregion

            #region Rejuvenation
            actions[(int)TreeAction.TankRejuvenation].Cooldown = spells[(int)TreeSpell.Rejuvenation].Duration;
            #endregion

            #region Nature's Swiftness
            if (Talents.NaturesSwiftness > 0)
            {
                // Nature's Swiftness is actually additive
                double nshtMultiplier = 1 + 0.5 / (1 + stats.PassiveDirectHealBonus + spells[(int)TreeSpell.HealingTouch].ExtraDirectBonus);

                actions[(int)TreeAction.RaidSwiftHT] = actions[(int)TreeAction.RaidHealingTouch];
                actions[(int)TreeAction.TankSwiftHT] = actions[(int)TreeAction.TankHealingTouch];

                actions[(int)TreeAction.RaidSwiftHT].Time = stats.Haste.HastedGCD;
                actions[(int)TreeAction.TankSwiftHT].Time = stats.Haste.HastedGCD;

                actions[(int)TreeAction.RaidSwiftHT].Cooldown = 180 + opts.NaturesSwiftnessCastDelay;
                actions[(int)TreeAction.TankSwiftHT].Cooldown = 180 + opts.NaturesSwiftnessCastDelay;

                actions[(int)TreeAction.RaidSwiftHT].Direct *= nshtMultiplier;
                actions[(int)TreeAction.TankSwiftHT].Direct *= nshtMultiplier;

                if (healingTouchNSReduction > 0)
                {
                    // add the NS effect as an amortized extra heal to each HT
                    double swiftHtFraction = (healingTouchNSReduction / (180.0 + opts.NaturesSwiftnessCastDelay));
                    double htAddTime = swiftHtFraction * stats.Haste.HastedGCD;
                    double htMulDirect = 1 + swiftHtFraction * nshtMultiplier;
                    double htMulMana = 1 + swiftHtFraction;

                    actions[(int)TreeAction.TankHealingTouch].Time += htAddTime;
                    actions[(int)TreeAction.TankHealingTouch].Mana *= htMulMana;
                    actions[(int)TreeAction.TankHealingTouch].Direct *= htMulDirect;

                    actions[(int)TreeAction.RaidHealingTouch].Time += htAddTime;
                    actions[(int)TreeAction.RaidHealingTouch].Mana *= htMulMana;
                    actions[(int)TreeAction.RaidHealingTouch].Direct *= htMulDirect;
                }
            }
            #endregion

            #region Clearcasting
            actions[(int)TreeAction.RaidClearHT] = applyCC(actions[(int)TreeAction.RaidHealingTouch]);
            actions[(int)TreeAction.TankClearHT] = applyCC(actions[(int)TreeAction.TankHealingTouch]);
            actions[(int)TreeAction.RaidClearRegrowth] = applyCC(actions[(int)TreeAction.RaidRegrowth]);
            actions[(int)TreeAction.TankClearRegrowth] = applyCC(actions[(int)TreeAction.TankRegrowth]);

            if (stats.TreeOfLifeUptime > 0)
            {
                actions[(int)TreeAction.RaidTolLbCcHt] = buildTolLbCcHt(stats, spells[(int)TreeSpell.Lifebloom].Ticks, actions[(int)TreeAction.RaidTolLb], 1, actions[(int)TreeAction.RaidClearHT]);
                actions[(int)TreeAction.TankTolLbCcHt] = buildTolLbCcHt(stats, spells[(int)TreeSpell.Lifebloom].Ticks, actions[(int)TreeAction.RaidTolLb], opts.TankRaidHealingWeight, actions[(int)TreeAction.TankClearHT]);
            }
            #endregion

            #region Nature's Bounty
            actions[(int)TreeAction.RaidRj2NourishNB] = buildNourishNB(stats, actions[(int)TreeAction.RaidRejuvenation], spells[(int)TreeSpell.Rejuvenation].Duration, 2, 1, actions[(int)TreeAction.RaidNourish]);
            actions[(int)TreeAction.RaidRj3NourishNB] = buildNourishNB(stats, actions[(int)TreeAction.RaidRejuvenation], spells[(int)TreeSpell.Rejuvenation].Duration, 3, 1, actions[(int)TreeAction.RaidNourish]);
            actions[(int)TreeAction.TankRj2NourishNB] = buildNourishNB(stats, actions[(int)TreeAction.RaidRejuvenation], spells[(int)TreeSpell.Rejuvenation].Duration, 2, opts.TankRaidHealingWeight, actions[(int)TreeAction.TankNourish]);
            #endregion

            #region Additional actions
            data.LifebloomRefreshInterval = spells[(int)TreeSpell.Lifebloom].Duration - opts.LifebloomWastedDuration * stats.Haste.HastedSecond;
            if (data.LifebloomRefreshInterval < stats.Haste.HastedSecond * 3)
                data.LifebloomRefreshInterval = stats.Haste.HastedSecond * 3;

            actions[(int)TreeAction.ReLifebloom].Time = spells[(int)TreeSpell.Lifebloom].Action.Time;
            actions[(int)TreeAction.ReLifebloom].Mana = spells[(int)TreeSpell.Lifebloom].Action.Mana;
            actions[(int)TreeAction.ReLifebloom].Direct = 0;
            actions[(int)TreeAction.ReLifebloom].Periodic = 0;
            actions[(int)TreeAction.ReLifebloom].Cooldown = data.LifebloomRefreshInterval;
            #endregion

            ContinuousAction[] factions = new ContinuousAction[(int)TreeAction.Count];
            for (int i = 0; i < (int)TreeAction.Count; ++i)
                factions[i] = new ContinuousAction(actions[i]);
            return factions;
        }

        void addLifebloomRefresh(ActionDistribution dist, ContinuousAction[] actions, ComputedSpell spell, TreeComputedData data, bool automatic, bool rejuvenationUp)
        {
            if (!automatic)
                dist.AddActionOnCooldown((int)TreeAction.ReLifebloom);
            
            dist.AddPassive((int)TreePassive.RollingLifebloom, (rejuvenationUp ? spell.TankAction.Periodic : spell.RaidAction.Periodic) * 3 * opts.TankLifebloomEH / spell.Duration, -data.LifebloomMPSGain);
            dist.AddPassiveTPS((int)TreePassive.RollingLifebloom, spell.TPS);
        }

        void addSpecialDirectHeals(ActionDistribution dist, ComputedSpell[] spells, TreeStats stats, bool onTank, bool useCCs, double minRate, bool htFiller)
        {
            // TODO: possibly make configurable, it's disabled because in practice it's unlikely to have a clearcast exactly when Nature's Grace is to be triggered
            bool procNaturesGraceWithCCs = false;

            bool naturesGraceHandled = false;

            double dhrate = 0;

            double ccrate = spells[(int)TreeSpell.Lifebloom].TPS * 0.02f * Talents.MalfurionsGift;
            if (ccrate != 0.0f)
            {
                if (procNaturesGraceWithCCs && Talents.NaturesGrace > 0)
                {
                    naturesGraceHandled = true;
                    dist.AddAction(onTank ? (int)TreeAction.TankClearRegrowth : (int)TreeAction.RaidClearRegrowth, 1.0f / 60.0f * spells[(int)TreeSpell.Regrowth].Action.Time);
                    ccrate -= 1.0f / 60.0f;
                    dhrate += 1.0f / 60.0f;
                }

                if (useCCs)
                {
                    dist.AddAction(onTank ? (int)TreeAction.TankClearHT : (int)TreeAction.RaidClearHT, ccrate * spells[(int)TreeSpell.HealingTouch].Action.Time);
                    dhrate += ccrate;
                }
            }

            if (!naturesGraceHandled && Talents.NaturesGrace > 0)
            {
                dist.AddAction(onTank ? (int)TreeAction.TankRegrowth : (int)TreeAction.RaidRegrowth, 1.0f / 60.0f * spells[(int)TreeSpell.Regrowth].Action.Time);
                dhrate += 1.0f / 60.0f;
            }

            if (dhrate < minRate)
            {
                if (!htFiller)
                    dist.AddAction(onTank ? (int)TreeAction.TankNourish : (int)TreeAction.RaidNourish, (minRate - dhrate) * spells[(int)TreeSpell.Nourish].Action.Time);
                else
                    dist.AddAction(onTank ? (int)TreeAction.TankHealingTouch : (int)TreeAction.RaidHealingTouch, (minRate - dhrate) * spells[(int)TreeSpell.HealingTouch].Action.Time);
            }
        }

        void addPassiveHealing(ActionDistribution dist, TreeStats stats)
        {
            dist.AddPassive((int)TreePassive.HealingTrinkets, stats.Healed * stats.DirectHealMultiplier * getCritMultiplier(stats, 0, 0));
        }

        void addSelfHealing(ActionDistribution dist, ContinuousAction[] actions, ComputedSpell[] spells, double weight)
        {
            dist.AddPassive((int)TreePassive.Perserverance, PerseveranceHPS * weight);
            dist.AddPassive((int)TreePassive.NaturesWard, actions[(int)TreeAction.RaidRejuvenation].EPS * actions[(int)TreeAction.RaidRejuvenation].Time / spells[(int)TreeSpell.Rejuvenation].Duration * NaturesWardUptime * weight);
        }

        ActionDistribution[] computeRaidHealing(bool burst, out int[] candidates)
        {
            ActionDistribution[] dists = new ActionDistribution[calc.Division.Count];

            for (int div = 0; div < calc.Division.Count; ++div)
            {
                TreeStats stats = calc.Stats[div];
                ComputedSpell[] spells = calc.Spells[div];
                TreeComputedData data = DivisionData[div];

                bool refreshLBWithDHs = Talents.EmpoweredTouch == 2 && opts.RefreshLifebloomWithDirectHeals;
                ContinuousAction[] actions = calc.Actions[div];
                ActionDistribution dist = new ActionDistribution(actions, (int)TreePassive.Count);
                if (!burst)
                    dist.MaxMPS = calc.ManaRegen;

                addPassiveHealing(dist, stats);
                addSelfHealing(dist, actions, spells, 1);

                addLifebloomRefresh(dist, actions, spells[(int)TreeSpell.Lifebloom], data, refreshLBWithDHs, opts.RejuvenationTankDuringRaid);

                double minDHRate = 0;
                if (refreshLBWithDHs)
                    minDHRate = Math.Max(minDHRate, 1.0f / data.LifebloomRefreshInterval);
                // note that 1/15 + 1/25 > 1/10, so in practice swiftmends and cchts alone should be enough to keep up Harmony, unless the Swiftmend cast delay has been set high (> 4 seconds)
                if (opts.Restoration)
                    minDHRate = Math.Max(minDHRate, 1.0f / 10.0f - 1.0 / (15 + opts.SwiftmendCastDelay));
                addSpecialDirectHeals(dist, spells, stats, refreshLBWithDHs, !burst, minDHRate, burst);

                // TODO: add option to choose when to use tranquility (maybe even model it as a division?)
                dist.AddActionOnCooldown((int)TreeAction.RaidTranquility);

                if (Restoration)
                    dist.AddActionOnCooldown((int)TreeAction.RaidSwiftmend);

                if (opts.RejuvenationTankDuringRaid)
                    dist.AddActionOnCooldown((int)TreeAction.TankRejuvenation);

                if (Talents.WildGrowth > 0)
                    dist.AddActionOnCooldown((int)TreeAction.RaidWildGrowth);

                dists[div] = dist;
            }

            List<int> candidatesList = new List<int>();
            if(Talents.NaturesSwiftness > 0)
                candidatesList.Add((int)TreeAction.RaidSwiftHT);
            candidatesList.Add((int)TreeAction.RaidRejuvenation);
            candidatesList.Add((int)TreeAction.RaidHealingTouch);
            candidatesList.Add((int)TreeAction.RaidNourish);
            candidatesList.Add((int)TreeAction.RaidRegrowth);
            if(character.DruidTalents.NaturesBounty > 0)
                candidatesList.Add(opts.RejuvenationTankDuringRaid ? (int)TreeAction.RaidRj2NourishNB : (int)TreeAction.RaidRj3NourishNB);
            candidatesList.Add((int)TreeAction.RaidTolLb);
            candidatesList.Add((int)TreeAction.RaidTolLbCcHt);
            candidates = candidatesList.ToArray();

            return dists;
        }

        ActionDistribution[] computeTankHealing(bool burst, out int[] candidates)
        {
            ActionDistribution[] dists = new ActionDistribution[calc.Division.Count];
            for (int div = 0; div < calc.Division.Count; ++div)
            {
                TreeStats stats = calc.Stats[div];
                ComputedSpell[] spells = calc.Spells[div];
                TreeComputedData data = DivisionData[div];
                ContinuousAction[] actions = calc.Actions[div];
                ActionDistribution dist = new ActionDistribution(actions, (int)TreePassive.Count);

                if (!burst)
                    dist.MaxMPS = calc.ManaRegen;

                addPassiveHealing(dist, stats);
                addSelfHealing(dist, actions, spells, opts.TankRaidHealingWeight);

                addLifebloomRefresh(dist, actions, spells[(int)TreeSpell.Lifebloom], data, Talents.EmpoweredTouch != 0, true);

                // assume we will automatically heal the tank enough to refresh lifebloom if we have Empowered Touch
                addSpecialDirectHeals(dist, spells, stats, true, !burst, 0, false);

                if (Restoration && opts.TankSwiftmend)
                    dist.AddActionOnCooldown((int)TreeAction.TankSwiftmend);

                if (!burst && opts.TankWildGrowth && Talents.WildGrowth > 0)
                    dist.AddActionOnCooldown((int)TreeAction.TankWildGrowth);

                dists[div] = dist;
            }

            List<int> candidatesList = new List<int>();
            if (Talents.NaturesSwiftness > 0)
                candidatesList.Add((int)TreeAction.TankSwiftHT);
            candidatesList.Add((int)TreeAction.TankRejuvenation);
            candidatesList.Add((int)TreeAction.TankHealingTouch);
            candidatesList.Add((int)TreeAction.TankNourish);
            candidatesList.Add((int)TreeAction.TankRegrowth);
            if(character.DruidTalents.NaturesBounty > 0)
                candidatesList.Add((int)TreeAction.TankRj2NourishNB);
            candidatesList.Add((int)TreeAction.TankTolLbCcHt);
            candidates = candidatesList.ToArray();

            return dists;
        }
    }
}
