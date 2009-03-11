using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Threading;

namespace Rawr.Optimizer
{
    /// <summary>
    /// Abstract class that is used by the optimizer to validate a certain range of slots in an
    /// individual. When optimizer is constructing an individual it will call the validator after
    /// it constructs the individual up to EndSlot, if validation fails optimizer restarts construction
    /// at StartSlot.
    /// </summary>
    /// <typeparam name="TItem">Type of items.</typeparam>
    public abstract class OptimizerRangeValidatorBase<TItem>
    {
        /// <summary>
        /// Gets or sets the first slot of validation range.
        /// </summary>
        public int StartSlot { get; set; }

        /// <summary>
        /// Gets or sets the last slot of validation range.
        /// </summary>
        public int EndSlot { get; set; }

        /// <summary>
        /// Checks whether the range of items is valid.
        /// </summary>
        /// <param name="items">Array of items that define an individual.</param>
        /// <returns>True if the range of items is valid, False otherwise.</returns>
        /// <remarks>Validation should only depend on items between StartSlot and EndSlot. Items after EndSlot
        /// may not be initialized.</remarks>
        public abstract bool IsValid(TItem[] items);
    }

    /// <summary>
    /// Abstract optimizer class that supports optimization of individuals that are defined by a list of items
    /// that fill slots where each slot has a list of valid items that can fill it and there can be restrictions
    /// on which item combinations are valid.
    /// </summary>
    /// <typeparam name="TItem">Type of items.</typeparam>
    /// <typeparam name="TIndividual">Type of the individuals.</typeparam>
    /// <typeparam name="TValuation">Type associated with valuation result of an individual.</typeparam>
    public abstract class OptimizerBase<TItem, TIndividual, TValuation>
        where TIndividual : class
    {
        protected int _thoroughness;

        public OptimizationMethod OptimizationMethod { get; set; }

        /// <summary>
        /// Number of item slots that define an individual.
        /// </summary>
        protected int slotCount;
        /// <summary>
        /// Array of lists for each slot of potential items to fill the slot. This should be populated
        /// unless a custom implementation of GetRandomItem and LookForDirectItemUpgrades is provided.
        /// </summary>
        protected List<TItem>[] slotItems;
        /// <summary>
        /// List of validators sorted in the order they should be executed.
        /// </summary>
        protected List<OptimizerRangeValidatorBase<TItem>> validators;

        public bool ThreadPoolValuation { get; set; }

        public virtual void CancelAsync()
        {
            cancellationPending = true;
        }

        protected bool cancellationPending;

        protected abstract void ReportProgress(int progressPercentage, float bestValue);

		protected Random rand;

        protected TIndividual Optimize(out float bestValue)
        {
            TValuation bestValuation;
            bool injected;
            return Optimize(null, 0, out bestValue, out bestValuation, out injected);
        }

        protected TIndividual Optimize(TIndividual injectIndividual, out float bestValue, out bool injected)
        {
            TValuation bestValuation;
            return Optimize(injectIndividual, injectIndividual != null ? GetOptimizationValue(injectIndividual) : 0.0f, out bestValue, out bestValuation, out injected);
        }

        protected TIndividual Optimize(TIndividual injectIndividual, float injectValue, out float bestValue, out TValuation bestValuation, out bool injected)
        {
            bool oldVolatility = Item.OptimizerManagedVolatiliy;
            try
            {
                Item.OptimizerManagedVolatiliy = true;
                switch (OptimizationMethod)
                {
                    case OptimizationMethod.GeneticAlgorithm:
                        return OptimizeGA(injectIndividual, injectValue, out bestValue, out bestValuation, out injected);
                    case OptimizationMethod.SimulatedAnnealing:
                        return OptimizeSA(injectIndividual, injectValue, out bestValue, out bestValuation, out injected);
                }
                bestValue = 0.0f;
                bestValuation = default(TValuation);
                injected = false;
                return null;
            }
            finally
            {
                Item.OptimizerManagedVolatiliy = oldVolatility;
            }
        }

        /// <summary>
        /// Optimization function based on simulated annealing
        /// http://en.wikipedia.org/wiki/Simulated_annealing
        /// Author: ebo
        /// </summary>
        private TIndividual OptimizeSA(TIndividual injectIndividual, float injectValue, out float best, out TValuation bestValuation, out bool injected)
        {
            best = -10000000;
            bestValuation = default(TValuation);
            injected = false;

            rand = new Random();

            TIndividual currentIndividual = BuildRandomIndividual();
            double currentValue = GetOptimizationValue(currentIndividual);

            TIndividual bestIndividual = currentIndividual;
            double bestValue = currentValue;

            int maxCycles = _thoroughness * _thoroughness;

            //http://research.microsoft.com/constraint-reasoning/workshops/autonomous-cp07/papers/2.pdf

            double temp = 10;
            double acceptRate = 0.5;
            double lamRate = 0.5;

            for (int cycle = 0; cycle < maxCycles; cycle++)
            {
                if (cancellationPending) return null;
                ReportProgress((int)Math.Round((float)cycle / ((float)(maxCycles / 100f))), (float)bestValue);

                // Generate new character
                TIndividual nextIndividual = GeneratorBuildSACharacter(currentIndividual);

                double nextValue = GetOptimizationValue(nextIndividual);


                // Save best character
                if (nextValue > bestValue)
                {
                    bestIndividual = nextIndividual;
                    bestValue = nextValue;
                }

                if (nextValue > currentValue)
                {
                    //Better solution. Accept move
                    currentIndividual = nextIndividual;
                    currentValue = nextValue;
                    acceptRate = 1.0 / 500.0 * (499.0 * acceptRate + 1);
                }
                else
                {
                    if (SAAcceptance(currentValue, nextValue, temp, rand))
                    {
                        //accept move
                        currentIndividual = nextIndividual;
                        currentValue = nextValue;
                        acceptRate = 1.0 / 500.0 * (499.0 * acceptRate + 1);
                    }
                    else
                    {
                        //reject move
                        acceptRate = 1.0 / 500.0 * (499.0 * acceptRate);
                    }

                }


                // tune acceptRate
                double part = (double)cycle / maxCycles;
                if (part < 0.15)
                {
                    lamRate = 0.44 + 0.56 * Math.Pow(560, -cycle / (maxCycles * 0.15));
                }
                else if (part < 0.65)
                {
                    lamRate = 0.44;
                }
                else
                {
                    lamRate = 0.44 * Math.Pow(440, -((double)cycle / (double)maxCycles - 0.65) / 0.35);
                }

                if (acceptRate > lamRate)
                {
                    temp *= 0.999;
                }
                else
                {
                    temp /= 0.999;
                }


            }

            best = (float)bestValue;
            bestValuation = GetValuation(bestIndividual);

            return bestIndividual;
        }

        private TIndividual[] population;
        private TValuation[] valuation;
        private AutoResetEvent valuationsComplete = new AutoResetEvent(false);
        private int threadPoolStarted;
        private int threadPoolComplete;
        private int threadPoolSize;

        private void GetThreadPoolValuation(object state)
        {
            int i = Interlocked.Increment(ref threadPoolStarted) - 1;
            valuation[i] = GetValuation(population[i]);
            if (Interlocked.Increment(ref threadPoolComplete) == threadPoolSize)
            {
                valuationsComplete.Set();
            }
        }

		private TIndividual OptimizeGA(TIndividual injectIndividual, float injectValue, out float bestValue, out TValuation bestValuation, out bool injected)
		{
			//Begin Genetic
			int noImprove, i1, i2;
			bestValue = -10000000;
            bestValuation = default(TValuation);
            injected = false;
            // verify inject individual
            if (injectIndividual != null && !IsIndividualValid(injectIndividual))
            {
                injectIndividual = null;
            }

			int popSize = _thoroughness;
            int islandSize = 20;
            int islandStagnationLimit = 50;
            int islandCount = (popSize - 1) / islandSize + 1;
			int cycleLimit = _thoroughness;
            population = new TIndividual[popSize];
            valuation = new TValuation[popSize];
            TIndividual[] popCopy = new TIndividual[popSize];
			float[] values = new float[popSize];
            float[] minIsland = new float[islandCount];
            float[] maxIsland = new float[islandCount];
            float[] bestIsland = new float[islandCount];
            TIndividual[] individualIsland = new TIndividual[islandCount];
            int[] islandNoImprove = new int[islandCount];
            for (int i = 0; i < islandCount; i++)
            {
                bestIsland[i] = -10000000;
            }
            float[] share = new float[popSize];
			float s, sum;
            TIndividual bestIndividual = null;
			rand = new Random();

			if (_thoroughness > 1)
			{
			    for (int i = 0; i < popSize; i++)
			    {
				    population[i] = BuildRandomIndividual();
			    }
			}
			else
			{
				bestIndividual = injectIndividual;
                if (bestIndividual == null)
                {
                    bestIndividual = BuildRandomIndividual();
                }
                bestValue = GetOptimizationValue(injectIndividual);
			}

			noImprove = 0;
			while (noImprove < cycleLimit)
			{
				if (_thoroughness > 1)
				{
				    if (cancellationPending) return null;
					ReportProgress((int)Math.Round((float)noImprove / ((float)cycleLimit / 100f)), bestValue);

                    for (int i = 0; i < islandCount; i++)
                    {
                        minIsland[i] = 10000000;
                        maxIsland[i] = -10000000;
                        islandNoImprove[i]++;
                    }
				    //minv = 10000000;
				    //maxv = -10000000;
                    if (ThreadPoolValuation)
                    {
                        threadPoolStarted = 0;
                        threadPoolComplete = 0;
                        threadPoolSize = popSize;
                        for (int i = 0; i < popSize; i++)
                        {
                            ThreadPool.QueueUserWorkItem(GetThreadPoolValuation);
                        }
                        valuationsComplete.WaitOne();
                    }
                    else
                    {
                        for (int i = 0; i < popSize; i++)
                        {
                            valuation[i] = GetValuation(population[i]);
                        }
                    }
				    for (int i = 0; i < popSize; i++)
				    {
                        int island = i / islandSize;
                        values[i] = GetOptimizationValue(population[i], valuation[i]);
                        if (values[i] < minIsland[island]) minIsland[island] = values[i];
                        if (values[i] > maxIsland[island]) maxIsland[island] = values[i];
                        if (values[i] > bestIsland[island])
                        {
                            bestIsland[island] = values[i];
                            individualIsland[island] = population[i];
                            islandNoImprove[island] = 0;
                        }
                        if (values[i] > bestValue)
					    {
						    bestValue = values[i];
                            bestValuation = valuation[i];
						    bestIndividual = population[i];
						    noImprove = -1;
                            //if (population[i].Geneology != null) System.Diagnostics.Trace.WriteLine(best + " " + population[i].Geneology);
					    }
				    }
                    for (int island = 0; island < islandCount; island++)
                    {
                        sum = 0;
                        for (int i = island * islandSize; i < Math.Min(popSize, (island + 1) * islandSize); i++)
                            sum += values[i] - minIsland[island] + (maxIsland[island] - minIsland[island]) / 2;
                        for (int i = island * islandSize; i < Math.Min(popSize, (island + 1) * islandSize); i++)
                            share[i] = sum == 0 ? 1f / (Math.Min(popSize, (island + 1) * islandSize) - island * islandSize) : (values[i] - minIsland[island] + (maxIsland[island] - minIsland[island]) / 2) / sum;
                    }
				}

				noImprove++;

                if (_thoroughness > 1 && noImprove < cycleLimit)
				{
					population.CopyTo(popCopy, 0);
					for (int i = 0; i < popSize; i++)
					{
                        int island = i / islandSize;
                        if (i % islandSize == 0)
                        {
                            if (individualIsland[island] == null)
                            {
                                population[i] = BuildRandomIndividual();
                                //population[i].Geneology = "Random";
                            }
                            else
                            {
                                population[i] = individualIsland[island];
                            }
                        }
                        else if (rand.NextDouble() < 0.05d)
                        {
                            //completely random
                            population[i] = BuildRandomIndividual();
                            //population[i].Geneology = "Random";
                        }
                        else if (rand.NextDouble() < 0.4d)
                        {
                            int transplant = island;
                            if (islandNoImprove[island] > islandStagnationLimit) transplant = rand.Next(islandCount);
                            //crossover
                            s = (float)rand.NextDouble();
                            sum = 0;
                            for (i1 = transplant * islandSize; i1 < Math.Min(popSize, (transplant + 1) * islandSize) - 1; i1++)
                            {
                                sum += share[i1];
                                if (sum >= s) break;
                            }
                            s = (float)rand.NextDouble();
                            sum = 0;
                            for (i2 = island * islandSize; i2 < Math.Min(popSize, (island + 1) * islandSize) - 1; i2++)
                            {
                                sum += share[i2];
                                if (sum >= s) break;
                            }
                            population[i] = BuildChildIndividual(popCopy[i1], popCopy[i2]);
                            //population[i].Geneology = "Crossover(" + values[i1] + ", " + values[i2] + ")";
                        }
                        else
                        {
                            int transplant = island;
                            if (islandNoImprove[island] > islandStagnationLimit && rand.NextDouble() < 1.0 / islandSize) transplant = rand.Next(islandCount);
                            //mutate
                            s = (float)rand.NextDouble();
                            sum = 0;
                            for (i1 = transplant * islandSize; i1 < Math.Min(popSize, (transplant + 1) * islandSize) - 1; i1++)
                            {
                                sum += share[i1];
                                if (sum >= s) break;
                            }
                            population[i] = BuildMutantIndividual(popCopy[i1]);
                            //population[i].Geneology = "Mutation(" + values[i1] + ")";
                        }
					}
				}
                else if (_thoroughness > 1 && injectIndividual != null && !injected && injectValue > bestValue)
                {
                    population[popSize - 1] = injectIndividual;
                    noImprove = 0;
                    injected = true;
                }
                else
                {
                    //last try, look for single direct upgrades
                    KeyValuePair<float, TIndividual> results;
                    TValuation directValuation;
                    for (int slot = 0; slot < slotCount; slot++)
                    {
                        results = LookForDirectItemUpgrades(slotItems[slot], slot, bestValue, bestIndividual, out directValuation);
                        if (results.Key > bestValue)
                        {
                            bestValue = results.Key;
                            bestValuation = directValuation; 
                            bestIndividual = results.Value; 
                            noImprove = 0;
                            population[0] = bestIndividual;
                            //population[0].Geneology = "DirectUpgrade";
                        }
                    }
                }
			}

            if (bestValue == 0)
            {
                bestIndividual = null;
                bestValuation = default(TValuation);
            }
            else
                ToString();
			return bestIndividual;
		}

        private object directValuationLock = new object();
        private TIndividual bestDirectIndividual;
        private TValuation bestDirectValuation;
        private float bestDirectValue;
        private int directValuationsComplete;
        private int directValuationsQueued;
        private bool directValuationFoundUpgrade;

        private void ThreadPoolDirectUpgradeValuation(object state)
        {
            TIndividual swappedIndividual = (TIndividual)state;
            TValuation valuation;
            float value = GetOptimizationValue(swappedIndividual, valuation = GetValuation(swappedIndividual));
            lock (directValuationLock)
            {
                directValuationsComplete++;
                if (value > bestDirectValue)
                {
                    bestDirectValue = value;
                    bestDirectValuation = valuation;
                    bestDirectIndividual = swappedIndividual;
                    directValuationFoundUpgrade = true;
                }
                if (directValuationsComplete >= directValuationsQueued) Monitor.Pulse(directValuationLock);
            }
        }

		protected virtual KeyValuePair<float, TIndividual> LookForDirectItemUpgrades(List<TItem> items, int slot, float best, TIndividual bestIndividual, out TValuation bestValuation)
		{
			TIndividual swappedIndividual;
            bestValuation = default(TValuation);
			float value;
			bool foundUpgrade = false;
            TItem[] itemList = (TItem[])GetItems(bestIndividual).Clone();
            if (ThreadPoolValuation)
            {
                bestDirectValue = best;
                directValuationFoundUpgrade = false;
                directValuationsQueued = 0;
                directValuationsComplete = 0;
            }
            foreach (TItem item in items)
			{
                itemList[slot] = item;
                if (IsIndividualValid(itemList))
                {
                    swappedIndividual = GenerateIndividual(itemList);
                    if (ThreadPoolValuation)
                    {
                        directValuationsQueued++;
                        ThreadPool.QueueUserWorkItem(ThreadPoolDirectUpgradeValuation, swappedIndividual);
                    }
                    else
                    {
                        TValuation valuation;
                        value = GetOptimizationValue(swappedIndividual, valuation = GetValuation(swappedIndividual));
                        if (value > best)
                        {
                            best = value;
                            bestValuation = valuation;
                            bestIndividual = swappedIndividual;
                            foundUpgrade = true;
                        }
                    }
                }
			}
            if (ThreadPoolValuation)
            {
                lock (directValuationLock)
                {
                    while (directValuationsComplete < directValuationsQueued) Monitor.Wait(directValuationLock);
                    if (directValuationFoundUpgrade)
                    {
                        best = bestDirectValue;
                        bestValuation = bestDirectValuation;
                        bestIndividual = bestDirectIndividual;
                        foundUpgrade = true;
                    }
                }
            }
			if (foundUpgrade)
				return new KeyValuePair<float, TIndividual>(best, bestIndividual);
			return new KeyValuePair<float, TIndividual>(float.NegativeInfinity, null);
		}

        protected virtual float GetOptimizationValue(TIndividual individual)
        {
            return GetOptimizationValue(individual, GetValuation(individual));
        }

        // items can be used as a context to limit the choices if not all in the list are valid
        protected virtual TItem GetRandomItem(int slot, TItem[] items)
        {
            List<TItem> list = slotItems[slot];
            int count = list.Count;
            if (count == 0) return default(TItem);
            return list[rand.Next(count)];
        }

        protected virtual bool IsIndividualValid(TItem[] items)
        {
            foreach (OptimizerRangeValidatorBase<TItem> validator in validators)
            {
                if (!validator.IsValid(items)) return false;
            }
            return true;
        }

        protected virtual bool IsIndividualValid(TIndividual individual)
        {
            return IsIndividualValid(GetItems(individual));
        }

        protected abstract TValuation GetValuation(TIndividual individual);
        protected abstract float GetOptimizationValue(TIndividual individual, TValuation valuation);
        protected abstract TItem GetItem(TIndividual individual, int slot);
        /// <remarks>The returned array will be treated as readonly.</remarks>
        protected abstract TItem[] GetItems(TIndividual individual);
        /// <remarks>If you need to store the array make a copy, because it might be modified.</remarks>
        protected abstract TIndividual GenerateIndividual(TItem[] items);

        protected delegate TItem GeneratorItemSelector(int slot, TItem[] items);

        protected virtual TIndividual GeneratorBuildIndividual(GeneratorItemSelector itemSelector)
		{
            TItem[] item = new TItem[slotCount];
            int validatorIndex = 0;
            for (int slot = 0; slot < slotCount; slot++)
            {
                item[slot] = itemSelector(slot, item);
                // check if we have to validate
                while (validatorIndex < validators.Count && validators[validatorIndex].EndSlot == slot)
                {
                    // validate
                    if (!validators[validatorIndex].IsValid(item))
                    {
                        // rewind
                        slot = validators[validatorIndex].StartSlot - 1; // for will increment it
                        validatorIndex = validators.FindIndex(validator => validator.EndSlot > slot);
                        if (validatorIndex == -1) validatorIndex = validators.Count;
                        break;
                    }
                    else
                    {
                        validatorIndex++; // move to next validator
                    }
                }
            }

            return GenerateIndividual(item);
		}

        protected virtual TIndividual BuildRandomIndividual()
        {
            return GeneratorBuildIndividual(GetRandomItem);
        }

		protected virtual TIndividual BuildChildIndividual(TIndividual father, TIndividual mother)
		{
            return GeneratorBuildIndividual(
                delegate(int slot, TItem[] items)
                {
                    return rand.NextDouble() < 0.5d ? GetItem(father, slot) : GetItem(mother, slot);
                });
		}

        /// <summary>
        /// This is funtions decides wether we take a new character or drop it
        /// Author: ebo
        /// </summary>
        private bool SAAcceptance(double e, double enew, double T, Random R)
        {
            // Always accept character if its better
            if (enew > e)
            {
                return true;
            }
            else
            {
                // Accept based on difference and temperature
                // higher temperature means bigger differences possible (or likely)
                // see http://en.wikipedia.org/wiki/Simulated_annealing
                double chance = Math.Exp((enew - e) / T);
                return chance > R.NextDouble();
            }

        }

        /// <summary>
        /// This is funtions clones a character and changes one item and based on a probability one enchant
        /// Author: ebo
        /// </summary>
        protected virtual TIndividual GeneratorBuildSACharacter(TIndividual parent)
        {
            TItem[] item = new TItem[slotCount];

            for (int slot = 0; slot < slotCount; slot++)
            {
                item[slot] = GetItem(parent, slot);
            }

            double r = rand.NextDouble();
            bool successfull = false;

            r = rand.NextDouble();

            while (!successfull)
            {
                // Make sure to change one item
                // There are better methods to make sure to change one item (shuffled list of all slots) but this works
                int slot = rand.Next(slotCount);
                TItem newitem = GetRandomItem(slot, item);
                TItem oldItem = item[slot];
                item[slot] = newitem;
                if (IsIndividualValid(item))
                {
                    successfull = true;
                }
                else
                {
                    item[slot] = oldItem;
                }
            }

            return GenerateIndividual(item);
        }

		protected virtual TIndividual BuildMutantIndividual(TIndividual parent)
		{
			int targetMutations = 2;
			while (targetMutations < 32 && rand.NextDouble() < 0.75d) targetMutations++;
			double mutationChance = (double)targetMutations / 32d;

            return GeneratorBuildIndividual(
                delegate(int slot, TItem[] items)
                {
                    if (rand.NextDouble() < mutationChance)
                    {
                        return GetRandomItem(slot, items);
                    }
                    else
                    {
                        return GetItem(parent, slot);
                    }
                });
		}

		protected virtual TIndividual BuildSingleItemSwapIndividual(TIndividual baseIndividual, int replaceSlot, TItem replaceItem)
		{
            TItem[] item = new TItem[slotCount];
            Array.Copy(GetItems(baseIndividual), item, slotCount);
            item[replaceSlot] = replaceItem;
            return GenerateIndividual(item);
		}
    }
}
