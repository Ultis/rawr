//#define DEBUG_BRANCHING
using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Mage
{
    public enum MIPMethod
    {
        BestBound,
        DepthFirst,
        HybridGuided,
        HybridUnguided,
    }

    public partial class Solver
    {
        private class BranchNode : IComparable<BranchNode>
        {
            public SolverLP Lp;
            public BranchNode Parent;
            public List<BranchNode> Children = new List<BranchNode>();
            public int Index;
            public int Depth;
            public double Value;
            public double ProbeValue;

            public double LpValue
            {
                get
                {
                    return Lp != null ? Lp.Value : Value;
                }
            }

            int IComparable<BranchNode>.CompareTo(BranchNode other)
            {                
                return -LpValue.CompareTo(other.LpValue);
            }

            public void ReleaseConstraints()
            {
                foreach (BranchNode node in Children)
                {
                    if (node != null) node.ReleaseConstraints();
                }
                if (Lp != null) Lp.ReleaseConstraints();
            }
        }

        private Heap<SolverLP> heap;
        private BranchNode currentNode;

        private List<int>[] hexList;
        private double[][] segmentCooldownCount;
        private double[] manaList;
        private double[] segmentFilled;
        private int[] hexMask;

        private struct ResolutionMapping
        {
            public int MinSegment;
            public int MaxSegment;
        }

        private ResolutionMapping[] lowResolution;
        private ResolutionMapping[] highResolution;
        private ResolutionMapping[] resolution;

        private void ComputeResolutionMaps()
        {
            double res = 30.0;
            lowResolution = new ResolutionMapping[segmentList.Count];
            for (int i = 0; i < segmentList.Count; i++)
            {
                int max = segmentList.FindLastIndex(segment => segment.TimeEnd - segmentList[i].TimeStart <= res + 0.00001);
                if (max < i) max = i;
                for (int j = i; j <= max; j++)
                {
                    lowResolution[j].MinSegment = i;
                    lowResolution[j].MaxSegment = max;
                }
                i = max;
            }

            highResolution = new ResolutionMapping[segmentList.Count];
            for (int i = 0; i < segmentList.Count; i++)
            {
                highResolution[i].MinSegment = i;
                highResolution[i].MaxSegment = i;
            }

            resolution = highResolution;
        }

        private void RestrictSolution()
        {
            ComputeResolutionMaps();

#if DEBUG_BRANCHING
            DebugBranching();
            return;
#endif

            switch (calculationOptions.MIPMethod)
            {
                case MIPMethod.BestBound:
                    BestBoundSearch();
                    break;
                case MIPMethod.DepthFirst:
                    DepthFirstSearch();
                    break;
                case MIPMethod.HybridUnguided:
                    HybridSearch(false);
                    break;
                case MIPMethod.HybridGuided:
                    HybridSearch(true);
                    break;
            }
        }

#if DEBUG_BRANCHING
        private void DebugBranching()
        {
            int[] depthCount = new int[10];
            lp.SolvePrimalDual();

            double rootValue = lp.Value;
            BranchNode root = new BranchNode() { Lp = lp };
            currentNode = root;

            lowerBound = 0.998 * rootValue;
            // explore the top 1% value error of branch tree
            do
            {
                double value = (currentNode.Lp != null) ? currentNode.Lp.Value : currentNode.Value; // force evaluation and get value
                currentNode.Value = value;
                if (value < lowerBound + 0.00001)
                {
                    // prune this, free space by removing from parent, backtrack
                    do
                    {
                        currentNode = currentNode.Parent;
                        currentNode.Children[currentNode.Index].ReleaseConstraints();
                        currentNode.Children[currentNode.Index] = null;
                        currentNode.Index++;
                    } while (currentNode.Index >= currentNode.Children.Count && currentNode.Parent != null);
                    if (currentNode.Index >= currentNode.Children.Count)
                    {
                        break; // we explored the whole search space
                    }
                    currentNode = currentNode.Children[currentNode.Index];
                }
                else
                {
                    int errorIndex = (int)((1 - value / rootValue) / 0.001);
                    if (errorIndex >= 0 && errorIndex < 10)
                    {
                        depthCount[errorIndex]++;
                    }
                    lp = currentNode.Lp;
                    if (lp != null)
                    {
                        solution = lp.Solve();
                        bool valid = IsLpValid();
                        if (valid)
                        {
                            currentNode.ReleaseConstraints();
                            // now backtrack
                            if (currentNode.Parent != null)
                            {
                                do
                                {
                                    currentNode = currentNode.Parent;
                                    currentNode.Children[currentNode.Index].ReleaseConstraints();
                                    currentNode.Children[currentNode.Index] = null;
                                    currentNode.Index++;
                                } while (currentNode.Index >= currentNode.Children.Count && currentNode.Parent != null);
                            }
                            else
                            {
                                break;
                            }
                            if (currentNode.Index >= currentNode.Children.Count) break; // we explored the whole search space
                            currentNode = currentNode.Children[currentNode.Index];
                        }
                        else
                        {
                            currentNode.Lp = null; // current lp may be reused by one of its children
                            // evaluate child nodes
                            currentNode = currentNode.Children[0];
                        }
                    }
                }
            } while (true);

            for (int i = 0; i < 10; i++)
            {
                System.Diagnostics.Trace.WriteLine("Error range " + i + ": " + depthCount[i]);
            }
            currentNode = null;
        }
#endif

        private void HybridSearch(bool guided)
        {
            int sizeLimit = calculationOptions.MaxHeapLimit;
            lp.SolvePrimalDual(); // solve primal and recalculate to get a stable starting point

            int round = 0;

            upperBound = lp.Value;
            lowerBound = calculationOptions.LowerBoundHint;

            SolverLP incumbent = null;

            LinkedList<BranchNode> leafNodes = new LinkedList<BranchNode>();

            SolverLP rootCopy = lp.Clone();
            BranchNode root = new BranchNode() { Lp = lp };
            ProbeDive(root, ref round, ref incumbent, sizeLimit, guided);

            leafNodes.AddFirst(root);
            root = null;

            do
            {
                double weight = (double)round / sizeLimit;
                double maxValue = double.NegativeInfinity;
                double maxWeightedValue = double.NegativeInfinity;
                LinkedListNode<BranchNode> bestNode = null;

                for (LinkedListNode<BranchNode> node = leafNodes.First; node != null; )
                {
                    if (node.Value.LpValue < lowerBound + 0.00001)
                    {
                        LinkedListNode<BranchNode> drop = node;
                        node = node.Next;
                        drop.Value.ReleaseConstraints();
                        leafNodes.Remove(drop);
                    }
                    else
                    {
                        if (node.Value.ProbeValue != 0.0 && node.Value.ProbeValue < lowerBound + 0.00001)
                        {
                            BranchNode drop = node.Value;
                            while (drop != null)
                            {
                                while (drop.Children.Count > 0 && drop.Children[0].LpValue < lowerBound + 0.00001)
                                {
                                    drop.Children[0].ReleaseConstraints();
                                    drop.Children.RemoveAt(0);
                                }
                                if (drop.Children.Count > 0)
                                {
                                    drop = drop.Children[0];
                                }
                                else
                                {
                                    drop = null;
                                }
                            }
                        }
                        if (node.Value.LpValue > maxValue) maxValue = node.Value.LpValue;
                        double weightedValue = weight * node.Value.LpValue + (1 - weight) * node.Value.ProbeValue;
                        if (weightedValue > maxWeightedValue)
                        {
                            maxWeightedValue = weightedValue;
                            bestNode = node;
                        }
                        node = node.Next;
                    }
                }

                if (bestNode != null)
                {
                    if (maxValue < upperBound - 0.00001)
                    {
                        upperBound = maxValue;
                        System.Diagnostics.Trace.WriteLine("Upper bound lowered to " + upperBound + " at round " + round);
                    }
                    // the node was already probed, so at the very least we have the child nodes
                    // probe each child and add them to leaf nodes
                    leafNodes.Remove(bestNode);
                    foreach (BranchNode child in bestNode.Value.Children)
                    {
                        child.Parent = null;
                        if (child.LpValue > lowerBound + 0.00001)
                        {
                            ProbeDive(child, ref round, ref incumbent, sizeLimit, guided);
                            leafNodes.AddLast(child);
                        }
                        else
                        {
                            child.ReleaseConstraints();
                        }
                    }
                }
            } while (round < sizeLimit && leafNodes.Count > 0 && !cancellationPending);

            if (round < sizeLimit && !cancellationPending) System.Diagnostics.Trace.WriteLine("Full search complete at round " + round);

            if (leafNodes.Count == 0 && incumbent != null)
            {
                upperBound = lowerBound;
            }

            lp = incumbent;
            if (lp == null)
            {
                lp = rootCopy;
                lowerBound = 0.0;
            }
            solution = lp.Solve();
            currentNode = null;
        }

        private void DepthFirstSearch()
        {
            int sizeLimit = calculationOptions.MaxHeapLimit;
            lp.SolvePrimalDual(); // solve primal and recalculate to get a stable starting point

            upperBound = lp.Value;
            lowerBound = calculationOptions.LowerBoundHint;

            SolverLP rootCopy = lp.Clone();
            BranchNode root = new BranchNode() { Lp = lp };

            currentNode = root;

            SolverLP incumbent = null;

            int highestBacktrack = int.MaxValue;

            int round = 0;
            do
            {
                double value = (currentNode.Lp != null) ? currentNode.Lp.Value : currentNode.Value; // force evaluation and get value
                currentNode.Value = value;
                if (value < lowerBound + 0.00001)
                {
                    // prune this, free space by removing from parent, backtrack
                    if (currentNode.Parent == null) break;
                    do
                    {
                        currentNode = currentNode.Parent;
                        currentNode.Children[currentNode.Index].ReleaseConstraints();
                        currentNode.Children[currentNode.Index] = null;
                        currentNode.Index++;
                        if (currentNode.Depth < highestBacktrack)
                        {
                            highestBacktrack = currentNode.Depth;
                            System.Diagnostics.Trace.WriteLine("Backtrack at " + highestBacktrack + ", value = " + lowerBound + ", root = " + currentNode.Value + ", round = " + round);
                        }
                    } while (currentNode.Index >= currentNode.Children.Count && currentNode.Parent != null);
                    if (currentNode.Index >= currentNode.Children.Count)
                    {
                        System.Diagnostics.Trace.WriteLine("Full search complete at round = " + round);
                        upperBound = lowerBound;
                        break; // we explored the whole search space
                    }
                    currentNode = currentNode.Children[currentNode.Index];
                }
                else
                {
                    lp = currentNode.Lp;
                    if (lp != null)
                    {
                        solution = lp.Solve();
                        round++;
                        bool valid = IsLpValid();
                        if (valid)
                        {
                            // we found a new lower bound
                            lowerBound = value;
                            incumbent = lp;
                            // now backtrack
                            if (currentNode.Parent != null)
                            {
                                do
                                {
                                    currentNode = currentNode.Parent;
                                    currentNode.Children[currentNode.Index].ReleaseConstraints();
                                    currentNode.Children[currentNode.Index] = null;
                                    currentNode.Index++;
                                    if (currentNode.Depth < highestBacktrack)
                                    {
                                        highestBacktrack = currentNode.Depth;
                                        System.Diagnostics.Trace.WriteLine("Backtrack at " + highestBacktrack + ", value = " + lowerBound + ", root = " + currentNode.Value + ", round = " + round);
                                    }
                                } while (currentNode.Index >= currentNode.Children.Count && currentNode.Parent != null);
                            }
                            else
                            {
                                break;
                            }
                            if (currentNode.Index >= currentNode.Children.Count) break; // we explored the whole search space
                            currentNode = currentNode.Children[currentNode.Index];
                        }
                        else
                        {
                            BranchNode first = null;
                            foreach (BranchNode node in currentNode.Children)
                            {
                                ProbeDive(node, ref round, ref incumbent, sizeLimit, true);
                                if (node.ProbeValue > lowerBound - 0.00001) first = node;
                            }
                            if (first != null)
                            {
                                currentNode.Children.Remove(first);
                                currentNode.Children.Insert(0, first);
                            }
                            currentNode.Lp = null; // current lp may be reused by one of its children
                            // evaluate child nodes
                            currentNode = currentNode.Children[0];
                        }
                    }
                    else
                    {
                        if (currentNode.Children.Count > 0)
                        {
                            BranchNode first = null;
                            foreach (BranchNode node in currentNode.Children)
                            {
                                ProbeDive(node, ref round, ref incumbent, sizeLimit, true);
                                if (node.ProbeValue > lowerBound - 0.00001) first = node;
                            }
                            if (first != null)
                            {
                                currentNode.Children.Remove(first);
                                currentNode.Children.Insert(0, first);
                            }
                            currentNode.Lp = null; // current lp may be reused by one of its children
                            // evaluate child nodes
                            if (currentNode.Children.Count == 0)
                            {
                                // can happen, so just handle it (no it can't?)
                                if (currentNode.Parent != null)
                                {
                                    do
                                    {
                                        currentNode = currentNode.Parent;
                                        currentNode.Children[currentNode.Index].ReleaseConstraints();
                                        currentNode.Children[currentNode.Index] = null;
                                        currentNode.Index++;
                                        if (currentNode.Depth < highestBacktrack)
                                        {
                                            highestBacktrack = currentNode.Depth;
                                            System.Diagnostics.Trace.WriteLine("Backtrack at " + highestBacktrack + ", value = " + lowerBound + ", root = " + currentNode.Value + ", round = " + round);
                                        }
                                    } while (currentNode.Index >= currentNode.Children.Count && currentNode.Parent != null);
                                }
                                else
                                {
                                    break;
                                }
                                if (currentNode.Index >= currentNode.Children.Count) break; // we explored the whole search space
                                currentNode = currentNode.Children[currentNode.Index];
                            }
                            else
                            {
                                currentNode = currentNode.Children[0];
                            }
                        }
                        else
                        {
                            if (currentNode.Parent != null)
                            {
                                do
                                {
                                    currentNode = currentNode.Parent;
                                    currentNode.Children[currentNode.Index].ReleaseConstraints();
                                    currentNode.Children[currentNode.Index] = null;
                                    currentNode.Index++;
                                    if (currentNode.Depth < highestBacktrack)
                                    {
                                        highestBacktrack = currentNode.Depth;
                                        System.Diagnostics.Trace.WriteLine("Backtrack at " + highestBacktrack + ", value = " + lowerBound + ", root = " + currentNode.Value + ", round = " + round);
                                    }
                                } while (currentNode.Index >= currentNode.Children.Count && currentNode.Parent != null);
                            }
                            else
                            {
                                break;
                            }
                            if (currentNode.Index >= currentNode.Children.Count) break; // we explored the whole search space
                            currentNode = currentNode.Children[currentNode.Index];
                        }
                    }
                }
            } while (round < sizeLimit && !cancellationPending);

            lp = incumbent;
            if (lp == null)
            {
                lp = rootCopy;
                lowerBound = 0.0;
            }
            solution = lp.Solve();
            currentNode = null;
        }

        private void ProbeDive(BranchNode node, ref int round, ref SolverLP incumbent, int sizeLimit, bool sortedDive)
        {
            if (node.ProbeValue != 0) return;
            BranchNode store = currentNode;
            currentNode = node;
            int probeRound = 0;
            do
            {
                double value = currentNode.Lp != null ? currentNode.Lp.Value : currentNode.Value; // force evaluation and get value
                currentNode.Value = value;
                if (value < lowerBound + 0.00001)
                {
                    if (value == 0.0)
                    {
                        value = -1.0; // if we have a value of 0 then all siblings have a value of 0, so this whole subtree is unfeasible, reprobe (but only in sorted dive)
                        currentNode.ProbeValue = value;
                        if (currentNode == node)
                        {
                            currentNode = store;
                            return;
                        }
                        // go to the next unprobed child
                        currentNode = currentNode.Parent;
                        bool allProbed = true;
                        if (!sortedDive)
                        {
                            foreach (BranchNode child in currentNode.Children)
                            {
                                if (child.ProbeValue == 0)
                                {
                                    allProbed = false;
                                    currentNode = child;
                                    break;
                                }
                            }
                        }
                        if (allProbed)
                        {
                            // this node has all children with value of 0
                            currentNode.Value = 0.0;
                            currentNode.ProbeValue = value;
                            currentNode.ReleaseConstraints();
                            currentNode.Children.Clear(); // clean out the trash, make it available for garbage collection
                            if (currentNode == node)
                            {
                                currentNode = store;
                                return;
                            }
                            currentNode = currentNode.Parent; // this node has to be reevaluated
                            // reprobe
                            if (sortedDive) currentNode.Children.Sort();
                            // evaluate child nodes
                            currentNode = currentNode.Children[0];
                        }
                    }
                    else
                    {
                        currentNode.ProbeValue = value;
                        while (currentNode != node)
                        {
                            currentNode = currentNode.Parent;
                            currentNode.ProbeValue = value;
                        }
                        currentNode = store;
                        return;
                    }
                }
                else
                {
                    lp = currentNode.Lp;
                    bool valid = false;
                    if (lp != null)
                    {
                        solution = lp.Solve();
                        if (currentNode.Depth > 100)
                        {
                            lp = lp; // investigate
                        }
                        round++;
                        probeRound++;
                        valid = IsLpValid();
                    }
                    if (valid)
                    {
                        // we found a new lower bound
                        lowerBound = value;
                        incumbent = lp;
                        System.Diagnostics.Trace.WriteLine("Probe value = " + lowerBound + ", root = " + node.Value + ", round = " + round);
                        currentNode.ProbeValue = value;
                        while (currentNode != node)
                        {
                            currentNode = currentNode.Parent;
                            currentNode.ProbeValue = value;
                        }
                        currentNode = store;
                        return;
                    }
                    else
                    {
                        if (sortedDive) currentNode.Children.Sort();
                        currentNode.Lp = null; // current lp may be reused by one of its children
                        // evaluate child nodes
                        currentNode = currentNode.Children[0];
                    }
                }
            } while (round < sizeLimit && probeRound < 100 && !cancellationPending);
            // don't spend too much time on a single probe, hope another branch will give something
            currentNode = store;
        }

        private void BestBoundSearch()
        {
            int maxHeap = calculationOptions.MaxHeapLimit;
            lp.SolvePrimalDual(); // solve primal and recalculate to get a stable starting point
            heap = new Heap<SolverLP>(HeapType.MaximumHeap);
            HeapPush(lp);

            upperBound = lp.Value;
            lowerBound = 0.0;

            bool valid = true;
            do
            {
                if (heap.Head.Value > upperBound + 0.001) // lowered instability threshold, in case it is still an issue just recompute the solution which "should" give a stable result hopefully
                {
                    // recovery measures first
                    double current = heap.Head.Value;
                    lp = heap.Pop();
                    lp.ForceRecalculation();
                    lp.SolvePrimalDual();
                    // some testing indicates that the recalculated solution gives the correct result, so the previous solution is most likely to be the problematic one, since we just discarded it not a big deal
                    //if (lp.Value <= max + 1.0)
                    //{
                    // give more fudge room in case the previous max was the one that was unstable
                    upperBound = lp.Value;
                    HeapPush(lp);
                    continue;
                    //}
                    //System.Windows.Forms.MessageBox.Show("Instability detected, aborting SMP algorithm (max = " + max + ", value = " + lp.Value + ")");
                    // find something reasonably stable
                    //while (heap.Count > 0 && (lp = heap.Pop()).Value > max + 0.000001) { }
                    //break;
                }
                lp = heap.Pop();
                if (lp.Value < upperBound - 0.00001)
                {
                    upperBound = lp.Value;
                    System.Diagnostics.Trace.WriteLine("Upper bound lowered to " + upperBound + " at round " + heap.Count);
                }
                // this is the best non-evaluated option (highest partially-constrained LP, the optimum has to be lower)
                // if this one is valid than all others are sub-optimal
                // validate all segments for each cooldown
                solution = lp.Solve();
                /*System.Diagnostics.Trace.WriteLine("Solution basis (value = " + lp.Value + "):");
                for (int index = 0; index < lpCols; index++)
                {
                    if (solution[index] > 0.000001) System.Diagnostics.Trace.WriteLine(index);
                }*/
                if (heap.Count > maxHeap)
                {
                    System.Windows.Forms.MessageBox.Show("SMP algorithm exceeded maximum allowed computation limit. Displaying the last working solution. Increase the limit in options if you would like to compute the correct solution.");
                    break;
                }
                valid = IsLpValid();
            } while (heap.Count > 0 && !valid && !cancellationPending);
            if (valid)
            {
                System.Diagnostics.Trace.WriteLine("Full search complete at round = " + heap.Count);
                lowerBound = upperBound;
            }
            heap = null;
        }

        private bool IsLpValid()
        {
            AnalyzeSolution();

            if (integralMana)
            {
                if (calculationOptions.ManaPotionEnabled && !ValidateIntegralConsumableOverall(VariableType.ManaPotion, 1.0)) return false;
                if (calculationOptions.ManaGemEnabled && !ValidateIntegralConsumableOverall(VariableType.ManaGem, 1.0)) return false;
                if (calculationOptions.EvocationEnabled && !ValidateIntegralConsumableOverall(VariableType.Evocation, 2.0 / calculationResult.BaseState.CastingSpeed)) return false;
                if (calculationOptions.EnableHastedEvocation)
                {
                    if (calculationOptions.EvocationEnabled && icyVeinsAvailable && !ValidateIntegralConsumableOverall(VariableType.EvocationIV, 2.0 / calculationResult.BaseState.CastingSpeed / 1.2)) return false;
                    if (calculationOptions.EvocationEnabled && heroismAvailable && !ValidateIntegralConsumableOverall(VariableType.EvocationHero, 2.0 / calculationResult.BaseState.CastingSpeed / 1.3)) return false;
                    if (calculationOptions.EvocationEnabled && icyVeinsAvailable && heroismAvailable && !ValidateIntegralConsumableOverall(VariableType.EvocationIVHero, 2.0 / calculationResult.BaseState.CastingSpeed / 1.2 / 1.3)) return false;
                }
                if (conjureManaGem && !ValidateIntegralConsumableOverall(VariableType.ConjureManaGem, calculationResult.ConjureManaGem.CastTime)) return false;
            }

            // low resolution
            if (segmentCooldowns && advancedConstraintsLevel >= 1)
            {
                resolution = lowResolution;
                // drums
                if (calculationOptions.DrumsOfBattle && !ValidateIntegralConsumableOverall(VariableType.DrumsOfBattle, calculationResult.BaseState.GlobalCooldown)) return false;
                // drums
                if (calculationOptions.DrumsOfBattle && !ValidateCooldown(Cooldown.DrumsOfBattle, 30.0, 120.0, true, 30.0, rowSegmentDrumsOfBattle, VariableType.None)) return false;
                // make sure all cooldowns are tightly packed and not fragmented
                // mf is trivially satisfied
                // ap
                if (arcanePowerAvailable && !ValidateCooldown(Cooldown.ArcanePower, calculationResult.ArcanePowerDuration, calculationResult.ArcanePowerCooldown, true, calculationResult.ArcanePowerDuration, rowSegmentArcanePower, VariableType.None)) return false;
                // iv
                if (icyVeinsAvailable && !ValidateCooldown(Cooldown.IcyVeins, 20.0 + (coldsnapAvailable ? 20.0 : 0.0), calculationResult.IcyVeinsCooldown + (coldsnapAvailable ? 20.0 : 0.0), true, 20.0, rowSegmentIcyVeins, VariableType.None)) return false;
                // pi
                if (powerInfusionAvailable && !ValidateCooldown(Cooldown.PowerInfusion, calculationResult.PowerInfusionDuration, calculationResult.PowerInfusionCooldown, true, calculationResult.PowerInfusionDuration, rowSegmentPowerInfusion, VariableType.None)) return false;
                // water elemental
                if (waterElementalAvailable && !ValidateIntegralConsumableOverall(VariableType.SummonWaterElemental, calculationResult.BaseState.GlobalCooldown)) return false;
                if (waterElementalAvailable && !ValidateCooldown(Cooldown.WaterElemental, calculationResult.WaterElementalDuration + (coldsnapAvailable ? calculationResult.WaterElementalDuration : 0.0), calculationResult.WaterElementalCooldown + (coldsnapAvailable ? calculationResult.WaterElementalDuration : 0.0), true, calculationResult.WaterElementalDuration, rowSegmentWaterElemental, VariableType.None)) return false;
                // combustion
                if (combustionAvailable && !ValidateCooldown(Cooldown.Combustion, 15.0, 180.0 + 15.0)) return false; // the durations are only used to compute segment distances, for 30 sec segments this should work pretty well
                // flamecap
                if (flameCapAvailable && !ValidateCooldown(Cooldown.FlameCap, 60.0, 180.0, integralMana, 60.0, rowSegmentFlameCap, VariableType.None)) return false;
                // t1
                if (trinket1Available && !ValidateCooldown(Cooldown.Trinket1, trinket1Duration, trinket1Cooldown, true, trinket1Duration, rowSegmentTrinket1, VariableType.None)) return false;
                // t2
                if (trinket2Available && !ValidateCooldown(Cooldown.Trinket2, trinket2Duration, trinket2Cooldown, true, trinket2Duration, rowSegmentTrinket2, VariableType.None)) return false;
                // mana gem effect
                if (manaGemEffectAvailable && !ValidateCooldown(Cooldown.ManaGemEffect, manaGemEffectDuration, 120f, true, manaGemEffectDuration, rowSegmentManaGemEffect, VariableType.None)) return false;
                // evocation
                if (calculationOptions.EvocationEnabled && !ValidateCooldown(Cooldown.Evocation, calculationResult.EvocationDuration, calculationResult.EvocationCooldown, false, 0.0, rowSegmentEvocation, VariableType.None)) return false;
                if (calculationOptions.EvocationEnabled && calculationOptions.EnableHastedEvocation)
                {
                    if (!ValidateHastedEvocation()) return false;
                    if (icyVeinsAvailable && !ValidateActivation(Cooldown.Evocation, VariableType.EvocationIV, calculationResult.EvocationDurationIV, calculationResult.EvocationCooldown, VariableType.EvocationIV, Cooldown.Evocation | Cooldown.IcyVeins)) return false;
                }
                // heroism
                if (heroismAvailable && !ValidateCooldown(Cooldown.Heroism, 40, -1)) return false;
                if (calculationOptions.EvocationEnabled && calculationOptions.EnableHastedEvocation)
                {
                    if (heroismAvailable && !ValidateActivation(Cooldown.Evocation, VariableType.EvocationHero, calculationResult.EvocationDurationHero, calculationResult.EvocationCooldown, VariableType.EvocationHero, Cooldown.Evocation | Cooldown.Heroism)) return false;
                    if (icyVeinsAvailable && heroismAvailable && !ValidateActivation(Cooldown.Evocation, VariableType.EvocationIVHero, calculationResult.EvocationDurationIVHero, calculationResult.EvocationCooldown, VariableType.EvocationIVHero, Cooldown.Evocation | Cooldown.IcyVeins | Cooldown.Heroism)) return false;
                }
                if (effectPotionAvailable && !ValidateEffectPotion()) return false;
                // potion of wild magic
                if (potionOfWildMagicAvailable && !ValidateCooldown(Cooldown.PotionOfWildMagic, 15, -1)) return false;
                // potion of speed
                if (potionOfSpeedAvailable && !ValidateCooldown(Cooldown.PotionOfSpeed, 15, -1)) return false;
            }

            // high resolution
            if (segmentCooldowns && advancedConstraintsLevel >= 2)
            {
                resolution = highResolution;
                // drums
                if (calculationOptions.DrumsOfBattle && !ValidateCooldown(Cooldown.DrumsOfBattle, 30.0, 120.0, true, 30.0, rowSegmentDrumsOfBattle, VariableType.None)) return false;
                // make sure all cooldowns are tightly packed and not fragmented
                // mf is trivially satisfied
                // ap
                if (arcanePowerAvailable && !ValidateCooldown(Cooldown.ArcanePower, calculationResult.ArcanePowerDuration, calculationResult.ArcanePowerCooldown, true, calculationResult.ArcanePowerDuration, rowSegmentArcanePower, VariableType.None)) return false;
                // iv
                if (icyVeinsAvailable && !ValidateCooldown(Cooldown.IcyVeins, 20.0 + (coldsnapAvailable ? 20.0 : 0.0), calculationResult.IcyVeinsCooldown + (coldsnapAvailable ? 20.0 : 0.0), true, 20.0, rowSegmentIcyVeins, VariableType.None)) return false;
                // ap
                if (powerInfusionAvailable && !ValidateCooldown(Cooldown.PowerInfusion, calculationResult.PowerInfusionDuration, calculationResult.PowerInfusionCooldown, true, calculationResult.PowerInfusionDuration, rowSegmentPowerInfusion, VariableType.None)) return false;
                // water elemental
                if (waterElementalAvailable && !ValidateCooldown(Cooldown.WaterElemental, calculationResult.WaterElementalDuration + (coldsnapAvailable ? calculationResult.WaterElementalDuration : 0.0), calculationResult.WaterElementalCooldown + (coldsnapAvailable ? calculationResult.WaterElementalDuration : 0.0), true, calculationResult.WaterElementalDuration, rowSegmentWaterElemental, VariableType.None)) return false;
                // coldsnap
                if (icyVeinsAvailable && coldsnapAvailable && !ValidateColdsnap()) return false;
                // combustion
                if (combustionAvailable && !ValidateCooldown(Cooldown.Combustion, 15.0, 180.0 + 15.0)) return false; // the durations are only used to compute segment distances, for 30 sec segments this should work pretty well
                // flamecap
                if (flameCapAvailable && !ValidateCooldown(Cooldown.FlameCap, 60.0, 180.0, integralMana, 60.0, rowSegmentFlameCap, VariableType.None)) return false;
                // t1
                if (trinket1Available && !ValidateCooldown(Cooldown.Trinket1, trinket1Duration, trinket1Cooldown, true, trinket1Duration, rowSegmentTrinket1, VariableType.None)) return false;
                // t2
                if (trinket2Available && !ValidateCooldown(Cooldown.Trinket2, trinket2Duration, trinket2Cooldown, true, trinket2Duration, rowSegmentTrinket2, VariableType.None)) return false;
                // mana gem effect
                if (manaGemEffectAvailable && !ValidateCooldown(Cooldown.ManaGemEffect, manaGemEffectDuration, 120f, true, manaGemEffectDuration, rowSegmentManaGemEffect, VariableType.None)) return false;
                // evocation
                if (calculationOptions.EvocationEnabled && !ValidateCooldown(Cooldown.Evocation, calculationResult.EvocationDuration, calculationResult.EvocationCooldown, false, 0.0, rowSegmentEvocation, VariableType.None)) return false;
                if (calculationOptions.EvocationEnabled && calculationOptions.EnableHastedEvocation)
                {
                    if (!ValidateHastedEvocation()) return false;
                    if (icyVeinsAvailable && !ValidateActivation(Cooldown.Evocation, VariableType.EvocationIV, calculationResult.EvocationDurationIV, calculationResult.EvocationCooldown, VariableType.EvocationIV, Cooldown.Evocation | Cooldown.IcyVeins)) return false;
                }
                // mana
                if (calculationOptions.EvocationEnabled && !ValidateEvocation()) return false;
                if (manaGemEffectAvailable && !ValidateManaGemEffect()) return false;
                // heroism
                if (heroismAvailable && !ValidateCooldown(Cooldown.Heroism, 40, -1)) return false;
                if (calculationOptions.EvocationEnabled && calculationOptions.EnableHastedEvocation)
                {
                    if (heroismAvailable && !ValidateActivation(Cooldown.Evocation, VariableType.EvocationHero, calculationResult.EvocationDurationHero, calculationResult.EvocationCooldown, VariableType.EvocationHero, Cooldown.Evocation | Cooldown.Heroism)) return false;
                    if (icyVeinsAvailable && heroismAvailable && !ValidateActivation(Cooldown.Evocation, VariableType.EvocationIVHero, calculationResult.EvocationDurationIVHero, calculationResult.EvocationCooldown, VariableType.EvocationIVHero, Cooldown.Evocation | Cooldown.IcyVeins | Cooldown.Heroism)) return false;
                }
                // potion of wild magic
                if (potionOfWildMagicAvailable && !ValidateCooldown(Cooldown.PotionOfWildMagic, 15, -1)) return false;
                // potion of speed
                if (potionOfSpeedAvailable && !ValidateCooldown(Cooldown.PotionOfSpeed, 15, -1)) return false;
            }

            if (integralMana && advancedConstraintsLevel >= 2)
            {
                if (calculationOptions.ManaPotionEnabled && !ValidateIntegralConsumable(VariableType.ManaPotion)) return false;
                if (calculationOptions.ManaGemEnabled && !ValidateIntegralConsumable(VariableType.ManaGem)) return false;
            }

            if (segmentCooldowns && advancedConstraintsLevel >= 3)
            {
                if (flameCapAvailable && !ValidateFlamecap()) return false;
                if (!ValidateCycling()) return false;
                if (!ValidateSupergroupCycles()) return false;
                if (!ValidateSupergroupFragmentation()) return false;

                if (calculationOptions.EvocationEnabled && calculationOptions.EnableHastedEvocation)
                {
                    if (icyVeinsAvailable && !ValidateActivationAdvanced(Cooldown.Evocation, VariableType.EvocationIV, calculationResult.EvocationDurationIV, calculationResult.EvocationCooldown, VariableType.EvocationIV, Cooldown.Evocation | Cooldown.IcyVeins)) return false;
                    if (heroismAvailable && !ValidateActivationAdvanced(Cooldown.Evocation, VariableType.EvocationHero, calculationResult.EvocationDurationHero, calculationResult.EvocationCooldown, VariableType.EvocationHero, Cooldown.Evocation | Cooldown.Heroism)) return false;
                    if (icyVeinsAvailable && heroismAvailable && !ValidateActivationAdvanced(Cooldown.Evocation, VariableType.EvocationIVHero, calculationResult.EvocationDurationIVHero, calculationResult.EvocationCooldown, VariableType.EvocationIVHero, Cooldown.Evocation | Cooldown.IcyVeins | Cooldown.Heroism)) return false;
                }

            }

            if (segmentCooldowns && advancedConstraintsLevel >= 4)
            {
                // advanced cooldown validation
                if (arcanePowerAvailable && !ValidateCooldownAdvanced(Cooldown.ArcanePower, calculationResult.ArcanePowerDuration, calculationResult.ArcanePowerCooldown, VariableType.None)) return false;
                if (icyVeinsAvailable && !coldsnapAvailable && !ValidateCooldownAdvanced(Cooldown.IcyVeins, 20.0, calculationResult.IcyVeinsCooldown, VariableType.None)) return false;
                if (powerInfusionAvailable && !ValidateCooldownAdvanced(Cooldown.PowerInfusion, calculationResult.PowerInfusionDuration, calculationResult.PowerInfusionCooldown, VariableType.None)) return false;
                if (trinket1Available && !ValidateCooldownAdvanced(Cooldown.Trinket1, trinket1Duration, trinket1Cooldown, VariableType.None)) return false;
                if (trinket2Available && !ValidateCooldownAdvanced(Cooldown.Trinket2, trinket2Duration, trinket2Cooldown, VariableType.None)) return false;
                if (manaGemEffectAvailable && !ValidateCooldownAdvanced(Cooldown.ManaGemEffect, manaGemEffectDuration, 120.0, VariableType.None)) return false;
            }

            if (segmentCooldowns && advancedConstraintsLevel >= 5)
            {
                if (calculationOptions.DrumsOfBattle && !ValidateActivation(Cooldown.DrumsOfBattle, VariableType.None, 30.0, 120.0, VariableType.DrumsOfBattle, Cooldown.None)) return false;
                if (waterElementalAvailable)
                {
                    if (coldsnapAvailable)
                    {
                        if (!ValidateWaterElementalSummon()) return false;
                    }
                    else
                    {
                        if (!ValidateActivation(Cooldown.WaterElemental, VariableType.None, calculationResult.WaterElementalDuration, calculationResult.WaterElementalCooldown, VariableType.SummonWaterElemental, Cooldown.None)) return false;
                    }
                }

                // advanced cooldown validation 2
                if (arcanePowerAvailable && !ValidateCooldownAdvanced2(Cooldown.ArcanePower, calculationResult.ArcanePowerDuration, calculationResult.ArcanePowerCooldown, VariableType.None)) return false;
                if (icyVeinsAvailable && !coldsnapAvailable && !ValidateCooldownAdvanced2(Cooldown.IcyVeins, 20.0, calculationResult.IcyVeinsCooldown, VariableType.None)) return false;
                if (powerInfusionAvailable && !ValidateCooldownAdvanced2(Cooldown.PowerInfusion, calculationResult.PowerInfusionDuration, calculationResult.PowerInfusionCooldown, VariableType.None)) return false;
                if (trinket1Available && !ValidateCooldownAdvanced2(Cooldown.Trinket1, trinket1Duration, trinket1Cooldown, VariableType.None)) return false;
                if (trinket2Available && !ValidateCooldownAdvanced2(Cooldown.Trinket2, trinket2Duration, trinket2Cooldown, VariableType.None)) return false;
                if (manaGemEffectAvailable && !ValidateCooldownAdvanced2(Cooldown.ManaGemEffect, manaGemEffectDuration, 120.0, VariableType.None)) return false;
            }

            return true;
        }

        private void HeapPush(SolverLP childLP)
        {
#if DEBUG_BRANCHING
            currentNode.Children.Add(new BranchNode() { Lp = childLP, Parent = currentNode, Depth = currentNode.Depth + 1 });
            return;
#endif
            switch (calculationOptions.MIPMethod)
            {
                case MIPMethod.BestBound:
                    if (childLP.Value > 0) heap.Push(childLP);
                    break;
                case MIPMethod.DepthFirst:
                case MIPMethod.HybridUnguided:
                case MIPMethod.HybridGuided:
                    currentNode.Children.Add(new BranchNode() { Lp = childLP, Parent = currentNode, Depth = currentNode.Depth + 1 });
                    break;
            }
#if DEBUG
            if (childLP.Log != null)
            {
                string[] lines = childLP.Log.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                if (lines.Length >= 2 && lines[lines.Length - 1] == lines[lines.Length - 2])
                {
                    childLP = childLP; // something looks to be buggy
                }
            }
#endif
        }

        private double[] GetSegmentCooldownCount(Cooldown cooldown, VariableType cooldownType)
        {
            int ind = -1;
            switch (cooldown)
            {
                case Cooldown.ArcanePower:
                    ind = 0;
                    break;
                case Cooldown.Combustion:
                    ind = 1;
                    break;
                case Cooldown.DrumsOfBattle:
                    ind = 2;
                    break;
                case Cooldown.FlameCap:
                    ind = 3;
                    break;
                case Cooldown.Heroism:
                    ind = 4;
                    break;
                case Cooldown.IcyVeins:
                    ind = 5;
                    break;
                case Cooldown.ManaGemEffect:
                    ind = 6;
                    break;
                case Cooldown.PotionOfSpeed:
                    ind = 7;
                    break;
                case Cooldown.PotionOfWildMagic:
                    ind = 8;
                    break;
                case Cooldown.Trinket1:
                    ind = 9;
                    break;
                case Cooldown.Trinket2:
                    ind = 10;
                    break;
                case Cooldown.WaterElemental:
                    ind = 11;
                    break;
                case Cooldown.Evocation:
                    switch (cooldownType)
                    {
                        case VariableType.None:
                            ind = 12;
                            break;
                        case VariableType.Evocation:
                            ind = 21;
                            break;
                        case VariableType.EvocationIV:
                            ind = 18;
                            break;
                        case VariableType.EvocationHero:
                            ind = 19;
                            break;
                        case VariableType.EvocationIVHero:
                            ind = 20;
                            break;
                    }
                    break;
                case Cooldown.Evocation | Cooldown.IcyVeins:
                    ind = 15;
                    break;
                case Cooldown.Evocation | Cooldown.Heroism:
                    ind = 16;
                    break;
                case Cooldown.Evocation | Cooldown.IcyVeins | Cooldown.Heroism:
                    ind = 17;
                    break;
                case Cooldown.PowerInfusion:
                    ind = 22;
                    break;
                default:
                    switch (cooldownType)
                    {
                        //case VariableType.Evocation:
                        //    ind = 12;
                        //    break;
                        case VariableType.ManaGem:
                            ind = 13;
                            break;
                        case VariableType.SummonWaterElemental:
                            ind = 14;
                            break;
                    }
                    break;
            }
            double[] segCount = segmentCooldownCount[ind];
            if (segCount == null)
            {
                segCount = new double[segmentList.Count];
                segmentCooldownCount[ind] = segCount;
                if (cooldown != Cooldown.None)
                {
                    for (int outseg = 0; outseg < segmentList.Count; outseg++)
                    {
                        double s = 0.0;
                        for (int index = segmentColumn[outseg]; index < segmentColumn[outseg + 1]; index++)
                        {
                            if (solutionVariable[index].IsMatch(cooldown, cooldownType))
                            {
                                s += solution[index];
                            }
                        }
                        segCount[outseg] = s;
                    }
                }
                for (int index = 0; index < segmentColumn[0]; index++) // fix if variable ordering changes
                {
                    if (solutionVariable[index].IsMatch(cooldown, cooldownType)) segCount[solutionVariable[index].Segment] += solution[index];
                }
            }
            return segCount;
        }

        private void AnalyzeSolution()
        {
            manaList = new double[segmentList.Count];
            segmentCooldownCount = new double[23][];
            hexList = new List<int>[segmentList.Count];
            segmentFilled = new double[segmentList.Count];
            hexMask = new int[segmentList.Count];
            for (int seg = 0; seg < segmentList.Count; seg++)
            {
                manaList[seg] = calculationResult.StartingMana;
                hexList[seg] = new List<int>();
            }
            for (int index = 0; index < solutionVariable.Count; index++)
            {
                CastingState state = solutionVariable[index].State;
                int iseg = solutionVariable[index].Segment;
                if (!calculationOptions.UnlimitedMana)
                {
                    for (int seg = iseg + 1; seg < segmentList.Count; seg++)
                    {
                        manaList[seg] -= solution[index] * lp[rowManaRegen, index];
                    }
                }
                if (solution[index] > 0.00001 && state != null)
                {
                    int h = (int)state.Cooldown;
                    if (h != 0)
                    {
                        hexMask[iseg] |= h;
                        if (!hexList[iseg].Contains(h)) hexList[iseg].Add(h);
                        segmentFilled[iseg] += solution[index];
                    }
                }
            }
        }

        private int AddConstraint(SolverLP branchlp, Cooldown cooldown, int segment)
        {
            return AddConstraint(branchlp, cooldown, VariableType.None, segment, segment);
        }

        private int AddConstraint(SolverLP branchlp, Cooldown cooldown, int minSegment, int maxSegment)
        {
            return AddConstraint(branchlp, cooldown, VariableType.None, minSegment, maxSegment);
        }

        private int AddConstraint(SolverLP branchlp, Cooldown cooldown, VariableType cooldownType, int minSegment, int maxSegment)
        {
            bool newConstraint;
            int index = branchlp.AddConstraint(string.Format("{0}-{1}-{2}-{3}", cooldown, cooldownType, minSegment, maxSegment), out newConstraint);
            if (newConstraint)
            {
                SetCooldownElements(branchlp, index, cooldown, cooldownType, minSegment, maxSegment, 1.0);
            }
            return index;
        }

        private bool ValidateSupergroupCycles()
        {
            for (int seg = 1; seg < segmentList.Count - 1; seg++)
            {
                int N = hexList[seg].Count;
                if (N > 0)
                {
                    for (int i = 0; i < N; i++)
                    {
                        int current = hexList[seg][i];
                        int leftmask = 0;
                        for (int j = 0; j < hexList[seg - 1].Count; j++)
                        {
                            leftmask |= (current & hexList[seg - 1][j]);
                        }
                        int rightmask = 0;
                        for (int j = 0; j < hexList[seg + 1].Count; j++)
                        {
                            rightmask |= (current & hexList[seg + 1][j]);
                        }
                        if (leftmask != 0 && rightmask != 0)
                        {
                            // single node connects to both left and right
                            // this means that every element in this segment has to contain either full left mask
                            // or full right mask
                            // to make the branch as powerful as possible pick one indicator from left mask
                            // and one indicator from right mask
                            // the situation is we have ind1 in left segment, ind2 in right segment and ind1 | ind2 here
                            // if this is the case then all items in this segment must have either ind1 or ind2
                            // otherwise we have to break the preconditions, so either no ind1 in left, no ind2 in right
                            // or not ind1 | ind2 here

                            // first detect if we have a problem
                            for (int j = 0; j < N; j++)
                            {
                                int h = hexList[seg][j];
                                if ((h & leftmask) != leftmask && (h & rightmask) != rightmask)
                                {
                                    // this indicates a problem
                                    // we have an item that doesn't have one of the masks
                                    // determine which indicators identify this issue

                                    //  XXX XX       leftmask
                                    //  XXXXXXXXX    current
                                    //   XXXX XX XX  issue
                                    //     XX  XX    rightmask

                                    //       *
                                    //  XXXXXXXXX
                                    //   XXXX XX XX
                                    //          *

                                    // basically find an entry from leftmask that is not in this one and an entry
                                    // from right mask that is not in this one

                                    int ind1 = 1;
                                    while ((ind1 & leftmask & ~h) == 0) ind1 <<= 1;
                                    int ind2 = 1;
                                    while ((ind2 & rightmask & ~h) == 0) ind2 <<= 1;

                                    // do elimination
                                    // eliminate left
                                    SolverLP hexRemovedLP = lp.Clone();
                                    if (hexRemovedLP.Log != null) hexRemovedLP.Log.AppendLine("Breaking supergroup cycle " + (Cooldown)ind1 + "-" + (Cooldown)ind2 + " at " + seg + ", removing left " + ind1);
                                    for (int index = 0; index < solutionVariable.Count; index++)
                                    {
                                        CastingState state = solutionVariable[index].State;
                                        int iseg = solutionVariable[index].Segment;
                                        if (state != null && iseg == seg - 1 && ((int)state.Cooldown & ind1) != 0) hexRemovedLP.EraseColumn(index);
                                    }
                                    HeapPush(hexRemovedLP);
                                    // eliminate right
                                    hexRemovedLP = lp.Clone();
                                    if (hexRemovedLP.Log != null) hexRemovedLP.Log.AppendLine("Breaking supergroup cycle " + (Cooldown)ind1 + "-" + (Cooldown)ind2 + " at " + seg + ", removing right " + ind2);
                                    for (int index = 0; index < solutionVariable.Count; index++)
                                    {
                                        CastingState state = solutionVariable[index].State;
                                        int iseg = solutionVariable[index].Segment;
                                        if (state != null && iseg == seg + 1 && ((int)state.Cooldown & ind2) != 0) hexRemovedLP.EraseColumn(index);
                                    }
                                    HeapPush(hexRemovedLP);
                                    // eliminate middle
                                    hexRemovedLP = lp.Clone();
                                    int ind = ind1 | ind2;
                                    if (hexRemovedLP.Log != null) hexRemovedLP.Log.AppendLine("Breaking supergroup cycle " + (Cooldown)ind1 + "-" + (Cooldown)ind2 + " at " + seg + ", removing center " + ind);
                                    for (int index = 0; index < solutionVariable.Count; index++)
                                    {
                                        CastingState state = solutionVariable[index].State;
                                        int iseg = solutionVariable[index].Segment;
                                        if (state != null && iseg == seg && ((int)state.Cooldown & ind) == ind) hexRemovedLP.EraseColumn(index);
                                    }
                                    HeapPush(hexRemovedLP);
                                    // force to full
                                    // this is equivalent to disabling all that are not good (at least I think it is)
                                    if (lp.Log != null) lp.Log.AppendLine("Breaking supergroup cycle " + (Cooldown)ind1 + "-" + (Cooldown)ind2 + " at " + seg + ", force to max");
                                    //int row = lp.AddConstraint(false);
                                    for (int index = 0; index < solutionVariable.Count; index++)
                                    {
                                        CastingState state = solutionVariable[index].State;
                                        int iseg = solutionVariable[index].Segment;
                                        if (state != null && iseg == seg && !solutionVariable[index].IsZeroTime)
                                        {
                                            int hex = (int)state.Cooldown;
                                            if (((int)state.Cooldown & (ind1 | ind2)) == 0) lp.EraseColumn(index);
                                            //if ((state.GetHex() & (ind1 | ind2)) != 0) lp.SetConstraintElement(row, index, -1.0);
                                        }
                                    }
                                    //lp.SetConstraintRHS(row, -segmentDuration);
                                    //lp.ForceRecalculation(true);
                                    lp.SetLHS(rowSegment + seg, segmentList[seg].Duration);
                                    HeapPush(lp);
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            for (int seg = 0; seg < segmentList.Count; seg++)
            {
                for (int seg2 = seg - 1; seg2 <= seg + 1; seg2 += 2)
                {
                    if (seg2 >= 0 && seg2 < segmentList.Count)
                    {
                        int mask = hexMask[seg2];
                        for (int i = 0; i < hexList[seg].Count; i++)
                        {
                            int core = hexList[seg][i];

                            // example problem cycle

                            // CDH   mask

                            // ICDH  core
                            // DH    <= connects to core but missing something from coremask
                            // IDG   <= connects to core but missing something from coremask

                            // key is that there is a pair of cooldowns that one has and the other does not that connect them to core
                            // this causes a variant of the double crossing except that here
                            // the whole thing happens in single segment, but by extension from other segment

                            int coremask = core & mask;
                            for (int j = 0; j < hexList[seg].Count; j++)
                            {
                                int h = hexList[seg][j];
                                if ((h & core) != 0 && (~h & coremask) != 0)
                                {
                                    for (int k = j + 1; k < hexList[seg].Count; k++)
                                    {
                                        int g = hexList[seg][k];
                                        if ((g & core) != 0 && (~g & coremask) != 0)
                                        {
                                            int hcore = core & h & ~g;
                                            int gcore = core & g & ~h;
                                            if (hcore != 0 && gcore != 0)
                                            {
                                                // key elements are:
                                                // - element from coremask that is missing in h
                                                // - element from coremask that is missing in g
                                                // - connecting element between core and h not in g
                                                // - connecting element between core and g not in h

                                                //  CD

                                                // VCDZ
                                                //    Z
                                                // V

                                                int c = 1;
                                                while ((c & ~h & coremask) == 0) c <<= 1;
                                                int d = 1;
                                                while ((d & ~g & coremask) == 0) d <<= 1;
                                                int v = 1;
                                                while ((v & hcore) == 0) v <<= 1;
                                                int z = 1;
                                                while ((z & gcore) == 0) z <<= 1;

                                                // either break one of the links forcing h and g behind core
                                                // or eliminate the double crossing

                                                // eliminate c from mask
                                                SolverLP hexRemovedLP = lp.Clone();
                                                if (hexRemovedLP.Log != null) hexRemovedLP.Log.AppendLine("Breaking advanced supergroup cycle at " + seg + ", removing from neighbor mask " + c);
                                                for (int index = 0; index < solutionVariable.Count; index++)
                                                {
                                                    CastingState state = solutionVariable[index].State;
                                                    int iseg = solutionVariable[index].Segment;
                                                    if (state != null && iseg == seg2 && ((int)state.Cooldown & c) != 0) hexRemovedLP.EraseColumn(index);
                                                }
                                                HeapPush(hexRemovedLP);

                                                // eliminate d from mask
                                                if (d != c)
                                                {
                                                    hexRemovedLP = lp.Clone();
                                                    if (hexRemovedLP.Log != null) hexRemovedLP.Log.AppendLine("Breaking advanced supergroup cycle at " + seg + ", removing from neighbor mask " + d);
                                                    for (int index = 0; index < solutionVariable.Count; index++)
                                                    {
                                                        CastingState state = solutionVariable[index].State;
                                                        int iseg = solutionVariable[index].Segment;
                                                        if (state != null && iseg == seg2 && ((int)state.Cooldown & d) != 0) hexRemovedLP.EraseColumn(index);
                                                    }
                                                    HeapPush(hexRemovedLP);
                                                }

                                                // eliminate bridge
                                                int bridge = c | d | v | z;
                                                hexRemovedLP = lp.Clone();
                                                if (hexRemovedLP.Log != null) hexRemovedLP.Log.AppendLine("Breaking advanced supergroup cycle at " + seg + ", removing bridge " + bridge);
                                                for (int index = 0; index < solutionVariable.Count; index++)
                                                {
                                                    CastingState state = solutionVariable[index].State;
                                                    int iseg = solutionVariable[index].Segment;
                                                    if (state != null && iseg == seg && ((int)state.Cooldown & bridge) == bridge) hexRemovedLP.EraseColumn(index);
                                                }
                                                HeapPush(hexRemovedLP);

                                                // eliminate hcore
                                                hexRemovedLP = lp.Clone();
                                                if (hexRemovedLP.Log != null) hexRemovedLP.Log.AppendLine("Breaking advanced supergroup cycle at " + seg + ", removing doublecross " + v + "-" + z);
                                                for (int index = 0; index < solutionVariable.Count; index++)
                                                {
                                                    CastingState state = solutionVariable[index].State;
                                                    int iseg = solutionVariable[index].Segment;
                                                    if (state != null && iseg == seg && ((int)state.Cooldown & (v | z)) == v) hexRemovedLP.EraseColumn(index);
                                                }
                                                HeapPush(hexRemovedLP);

                                                // eliminate gcore
                                                hexRemovedLP = lp.Clone();
                                                if (hexRemovedLP.Log != null) hexRemovedLP.Log.AppendLine("Breaking advanced supergroup cycle at " + seg + ", removing doublecross " + z + "-" + v);
                                                for (int index = 0; index < solutionVariable.Count; index++)
                                                {
                                                    CastingState state = solutionVariable[index].State;
                                                    int iseg = solutionVariable[index].Segment;
                                                    if (state != null && iseg == seg && ((int)state.Cooldown & (v | z)) == z) hexRemovedLP.EraseColumn(index);
                                                }
                                                HeapPush(hexRemovedLP);

                                                // lp unused
                                                lp.ReleaseConstraints();
                                                return false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        private bool ValidateSupergroupFragmentation()
        {
            for (int seg = 1; seg < segmentList.Count - 1; seg++)
            {
                int N = hexList[seg].Count;
                if (N > 0 && segmentFilled[seg] < segmentList[seg].Duration - 0.00001)
                {
                    // if any hex links to left and right segment we have a problem
                    List<int> minHexChain = null;
                    for (int i = 0; i < N; i++)
                    {
                        // compute distance
                        int[] hexDistance = new int[N];
                        for (int j = 0; j < N; j++)
                        {
                            hexDistance[j] = -1;
                        }
                        hexDistance[i] = 0;
                        int current = hexList[seg][i];
                        int distance = 0;
                        int next;
                        bool unlinked = true;
                        while (unlinked)
                        {
                            unlinked = false;
                            distance++;
                            next = current;
                            for (int j = 0; j < N; j++)
                            {
                                if (hexDistance[j] == -1 && (hexList[seg][j] & current) != 0)
                                {
                                    hexDistance[j] = distance;
                                    next |= hexList[seg][j];
                                    unlinked = true;
                                }
                            }
                            current = next;
                        }
                        // find closest link on left and right
                        int leftMin = int.MaxValue;
                        int leftMinHex = 0;
                        for (int j = 0; j < hexList[seg - 1].Count; j++)
                        {
                            int h = hexList[seg - 1][j];
                            if ((h & current) != 0)
                            {
                                for (int k = 0; k < N; k++)
                                {
                                    if ((h & hexList[seg][k]) != 0)
                                    {
                                        if (hexDistance[k] != -1 && hexDistance[k] < leftMin)
                                        {
                                            leftMin = hexDistance[k];
                                            leftMinHex = h;
                                        }
                                    }
                                }
                            }
                        }
                        int rightMin = int.MaxValue;
                        int rightMinHex = 0;
                        for (int j = 0; j < hexList[seg + 1].Count; j++)
                        {
                            int h = hexList[seg + 1][j];
                            if ((h & current) != 0)
                            {
                                for (int k = 0; k < N; k++)
                                {
                                    if ((h & hexList[seg][k]) != 0)
                                    {
                                        if (hexDistance[k] != -1 && hexDistance[k] < rightMin)
                                        {
                                            rightMin = hexDistance[k];
                                            rightMinHex = h;
                                        }
                                    }
                                }
                            }
                        }
                        if (leftMinHex != 0 && rightMinHex != 0)
                        {
                            // we found an offensive hex chain
                            if (minHexChain == null || (leftMin + rightMin + 2) < minHexChain.Count)
                            {
                                // reconstruct chain
                                minHexChain = new List<int>();
                                int currentHex = leftMinHex;
                                int currentDist = leftMin;
                                if (!minHexChain.Contains(currentHex)) minHexChain.Add(currentHex);
                                while (currentDist > 0)
                                {
                                    for (int j = 0; j < N; j++)
                                    {
                                        if (hexDistance[j] == currentDist && (hexList[seg][j] & currentHex) != 0)
                                        {
                                            currentHex = hexList[seg][j];
                                            currentDist--;
                                            if (!minHexChain.Contains(currentHex)) minHexChain.Add(currentHex);
                                            break;
                                        }
                                    }
                                }
                                currentHex = rightMinHex;
                                currentDist = rightMin;
                                if (!minHexChain.Contains(currentHex)) minHexChain.Add(currentHex);
                                while (currentDist > 0)
                                {
                                    for (int j = 0; j < N; j++)
                                    {
                                        if (hexDistance[j] == currentDist && (hexList[seg][j] & currentHex) != 0)
                                        {
                                            currentHex = hexList[seg][j];
                                            currentDist--;
                                            if (!minHexChain.Contains(currentHex)) minHexChain.Add(currentHex);
                                            break;
                                        }
                                    }
                                }
                                currentHex = hexList[seg][i];
                                if (!minHexChain.Contains(currentHex)) minHexChain.Add(currentHex);
                            }
                        }
                    }
                    if (minHexChain != null)
                    {
                        // we have a problem and the shortest hex chain that identifies it
                        // solve by branching on eliminating one of the elements in the chain
                        // or forcing the segment to max
                        for (int i = 0; i < minHexChain.Count; i++)
                        {
                            SolverLP hexRemovedLP = lp.Clone();
                            if (hexRemovedLP.Log != null) hexRemovedLP.Log.AppendLine("Breaking supergroup fragmentation at " + seg + ", removing " + minHexChain[i]);
                            for (int index = 0; index < solutionVariable.Count; index++)
                            {
                                CastingState state = solutionVariable[index].State;
                                int iseg = solutionVariable[index].Segment;
                                if (state != null && iseg >= seg - 1 && iseg <= seg + 1 && (int)state.Cooldown == minHexChain[i]) hexRemovedLP.EraseColumn(index);
                            }
                            HeapPush(hexRemovedLP);
                        }
                        if (lp.Log != null) lp.Log.AppendLine("Breaking supergroup fragmentation at " + seg + ", force to max");
                        //int row = lp.AddConstraint(false);
                        for (int index = 0; index < solutionVariable.Count; index++)
                        {
                            CastingState state = solutionVariable[index].State;
                            int iseg = solutionVariable[index].Segment;
                            if (state != null && iseg == seg && (int)state.Cooldown == 0 && !solutionVariable[index].IsZeroTime) lp.EraseColumn(index);
                            //if (state != null && iseg == seg && state.GetHex() != 0) lp.SetConstraintElement(row, index, -1.0);
                        }
                        lp.SetLHS(rowSegment + seg, segmentList[seg].Duration);
                        //lp.SetConstraintRHS(row, -segmentDuration);
                        lp.ForceRecalculation(true);
                        HeapPush(lp);
                        return false;
                    }
                }
            }
            return true;
        }

        private List<int> FindShortestTailPath(int core, int t, int node, int[] used, int N, List<int> segmentHexList, int distinctFrom)
        {
            int[] dist = new int[N]; // distance from CTn indicator
            int[] parent = new int[N];
            for (int k = 0; k < N; k++)
            {
                if (used[k] == t + 1)
                {
                    if ((segmentHexList[k] & core & ~distinctFrom) != 0)
                    {
                        dist[k] = 1;
                        parent[k] = -1;
                    }
                }
            }
            bool updated;
            do
            {
                updated = false;
                for (int k = 0; k < N; k++)
                {
                    if (used[k] == t + 1)
                    {
                        for (int l = 0; l < N; l++)
                        {
                            if (used[l] == t + 1 && dist[l] > 0)
                            {
                                if ((segmentHexList[k] & segmentHexList[l] & ~core & ~distinctFrom) != 0 && (dist[l] + 1 < dist[k] || dist[k] == 0))
                                {
                                    dist[k] = dist[l] + 1;
                                    parent[k] = l;
                                    updated = true;
                                }
                            }
                        }
                    }
                }
            } while (updated);
            int min = int.MaxValue;
            int closest = -1;
            for (int k = 0; k < N; k++)
            {
                if (used[k] == t + 1)
                {
                    if (dist[k] > 0 && (segmentHexList[k] & node & ~core) != 0 && dist[k] + 1 < min)
                    {
                        min = dist[k] + 1;
                        closest = k;
                    }
                }
            }
            List<int> ret = new List<int>();
            while (closest != -1)
            {
                ret.Add(segmentHexList[closest]);
                closest = parent[closest];
            }
            return ret;
        }

        private bool ValidateCycling()
        {
            // eliminate packing cycles
            // example:
            // H+IV:10
            // IV+Icon:10
            // H+Icon:10
            bool valid = true;
            for (int seg = 0; seg < segmentList.Count - 1; seg++)
            {
                // collect all cooldowns on the boundary seg...(seg+1)
                // assume one instance of cooldown max (coldsnap theoretically doesn't satisfy this, but I think it should still work)
                // calculate hex values for boolean arithmetic
                // verify if there are cycles

                // ###   ###
                // ######
                //    ######

                // ##
                //  ##
                //   ##
                //    ##
                //     ##
                // #    #

                // cycle = no element can be placed at the start, all have two tails that intersect to 0
                // inside the boundary we can have more than one cycle and several nice packings
                // find elements that can be placed at the start, those are the ones with nice packing
                // for each one you find remove the corresponding group
                // if we remove everything then there are no cycles

                // we identify problematic node by enumerating over all
                // for each we start building tails that connect to the current core
                // we ignore nodes until they connect to one of the tails
                // if a node connects to the core or tailes we examine existing tails
                // and if it is consistent/connects with any of the tails we attach it to the tail
                // if at any point we get 3 tails or any 2 tails connect we have a packing cycle/impossibility

                // if node connects to tail at noncore position (tail & node & ~core != 0) and also connects to
                // core, but at a point not in the tail (core & node & ~tail != 0) then the node has to come before the tail
                // if reverse is also true, that is the tail needs to be before node because it connects at distinct point
                // (tail & core & ~node != 0) then the node is incompatible with the tail, but since it also connects to it
                // it forms a 2 tail cycle with it

                // if node connects to tail at noncore position (tail & node & ~core != 0) then it has to be in the same tail

                // if node connects to core where tail does not (core & node & ~tail != 0) and tail where node does not
                // (tail & core & ~node != 0) then they have to be in different tails

                // XX 
                //  XX
                // X X
                
                //  X
                // XXX
                // X
                //   X

                List<int> hexList1 = hexList[seg];
                List<int> hexList2 = hexList[seg + 1];
                List<int> hexList12 = new List<int>();
                hexList12.AddRange(hexList1);
                hexList12.AddRange(hexList2);

                int N = hexList12.Count;
                List<int> shortestCycle = null;
                List<int> shortestBridge = null;
                List<int> threeTail = null;
                List<int> connectingTails = new List<int>();
                for (int i = 0; i < N; i++)
                {
                    int core = hexList12[i];
                    int[] tail = new int[3];
                    int[] used = new int[N]; // index+1 of the tail to which it is attached, -1 for core
                    int[] firsttail = new int[3];
                    used[i] = -1;
                    bool cont = true;
                    while (cont)
                    {
                        cont = false;
                        for (int j = 0; j < N; j++)
                        {
                            if (used[j] == 0)
                            {
                                int node = hexList12[j];
                                int t = 0;
                                bool alldifferent = true;
                                if (t == 0 && tail[t] == 0)
                                {
                                    alldifferent = (node & ~core) != 0 && (core & ~node) != 0 && (node & core) != 0;
                                }
                                connectingTails.Clear();
                                for (; t < 3 && tail[t] != 0; t++)
                                {
                                    int cTN = tail[t] & node & ~core;
                                    int CtN = ~tail[t] & node & core;
                                    int CTn = tail[t] & ~node & core;
                                    bool connects = cTN != 0;
                                    bool different = CtN != 0 && CTn != 0;
                                    if (connects && different)
                                    {
                                        // this is a cycle
                                        // XXXXX        core  oo........    XX XX
                                        // XXX  XXX     tail  ......oo.. => XX    XX
                                        //   XXX XXXX   node  ...oo.....       XX XX

                                        // to break the cycle we have to find the connecting link in tail
                                        // if we have a 4 node cycle then the corresponding indicators
                                        // are ABcd, aBCd, abCD, AbcD

                                        // XXXXX        core   oo........    XX XX
                                        // XX   X       tail_1 .....o....    XX   X
                                        //   X  XXX     tail_2 ......oo.. =>      XXX
                                        //   XXX XXXX   node   ...oo.....       XX XX

                                        // XXXXX        core   oo........    XX XX
                                        // XXX  XX      tail_1 .....o....    XX   X
                                        //   X  XXX     tail_2 .......o.. =>      X X
                                        //   XXX XXXX   node   ...oo.....       XX  X

                                        // XXXXX        core   oo........    XX XX
                                        // XXX  XX      tail_1 ......o...    XX    X
                                        //   XXX XXXX   node   ...oo.....       XX X
                                        
                                        // when looking for a connection it is enough that it touches both
                                        // sides, it does not to completely cover it
                                        // we want to use single indicators anyway as they're more powerful
                                        // in eliminating options and potential future cycles

                                        List<int> path = FindShortestTailPath(core, t, node, used, N, hexList12, node);
                                        if (shortestCycle == null || path.Count + 2 < shortestCycle.Count)
                                        {
                                            shortestCycle = new List<int>();
                                            shortestCycle.Add(node);
                                            shortestCycle.AddRange(path);
                                            shortestCycle.Add(core);
                                            if (shortestCycle.Count == 3) goto BREAKCYCLE;
                                        }
                                        cont = true;
                                        used[j] = -1;
                                        break;
                                    }
                                    else if (connects && !different)
                                    {
                                        cont = true;
                                        connectingTails.Add(t);
                                    }
                                    else if (!different)
                                    {
                                        alldifferent = false;
                                    }
                                }
                                if (used[j] != -1)
                                {
                                    if (connectingTails.Count == 0 && t < 3 && tail[t] == 0)
                                    {
                                        if (alldifferent)
                                        {
                                            // it doesn't connect to any existing tails and we can prove it's not in any of them
                                            // this means it has to be in a different tail
                                            cont = true;
                                            tail[t] = hexList12[j];
                                            firsttail[t] = hexList12[j];
                                            used[j] = t + 1;
                                            if (t == 2)
                                            {
                                                // we have a three-tail
                                                threeTail = new List<int>();
                                                threeTail.Add(core);
                                                for (t = 0; t < 3; t++)
                                                {
                                                    threeTail.Add(firsttail[t]);
                                                }
                                            }
                                        }
                                        // otherwise it could be in one of the previous tails so we can't do anything
                                    }
                                    else if (connectingTails.Count >= 2)
                                    {
                                        // node connected to one of the tails
                                        // it is still possible that it connects to some other tail also

                                        //      XX    tail2
                                        //    XXX     core
                                        //   XX       tail1                                        
                                        //   X   X

                                        // in this case we also have a cycle, find the shortest path through
                                        // each tail and loop it into cycle
                                        int t0 = connectingTails[0];
                                        int t1 = connectingTails[1];
                                        used[j] = -1;
                                        List<int> path1 = FindShortestTailPath(core, t0, node, used, N, hexList12, firsttail[t1]);
                                        List<int> path2 = FindShortestTailPath(core, t1, node, used, N, hexList12, firsttail[t0]);
                                        if (shortestBridge == null || path1.Count + path2.Count + 2 < shortestBridge.Count)
                                        {
                                            shortestBridge = new List<int>();
                                            shortestBridge.Add(node);
                                            shortestBridge.AddRange(path1);
                                            shortestBridge.Add(core);
                                            path2.Reverse();
                                            shortestBridge.AddRange(path2);
                                        }
                                    }
                                    else if (connectingTails.Count == 1)
                                    {
                                        used[j] = connectingTails[0] + 1;
                                        tail[connectingTails[0]] |= hexList12[j];
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            BREAKCYCLE:
                if (shortestCycle != null && (threeTail == null || shortestCycle.Count < threeTail.Count) && (shortestBridge == null || shortestCycle.Count < shortestBridge.Count))
                {
                    // break the cycle
                    // first compute indicators and for each indicator select a single representative
                    int M = shortestCycle.Count;
                    List<int> indicator = new List<int>();
                    for (int i = 0; i < M; i++)
                    {
                        int j = (i + 1) % M;
                        int ind = shortestCycle[i] & shortestCycle[j];
                        for (int k = 0; k < M; k++)
                        {
                            if (k != i && k != j) ind &= ~shortestCycle[k];
                        }
                        int s = 1;
                        while ((s & ind) == 0) s <<= 1;
                        indicator.Add(s);
                    }
                    int cycle = 0;
                    for (int i = 0; i < M; i++)
                    {
                        cycle |= indicator[i];
                    }
                    // break cycle by eliminating one pair of indicators
                    for (int i = 0; i < M; i++)
                    {
                        int j = (i + 1) % M;
                        SolverLP hexRemovedLP = lp.Clone();
                        if (hexRemovedLP.Log != null) hexRemovedLP.Log.AppendLine("Breaking cycle " + (Cooldown)cycle + " at boundary " + seg + ", removing " + indicator[i] + "+" + indicator[j]);
                        for (int index = 0; index < solutionVariable.Count; index++)
                        {
                            CastingState state = solutionVariable[index].State;
                            int iseg = solutionVariable[index].Segment;
                            if (state != null && (iseg == seg || iseg == seg + 1))
                            {
                                int h = (int)state.Cooldown;
                                bool isindicated = (h & indicator[i]) != 0 && (h & indicator[j]) != 0;
                                if (isindicated)
                                {
                                    for (int k = 0; k < M; k++)
                                    {
                                        if (k != i && k != j && (h & indicator[k]) != 0)
                                        {
                                            isindicated = false;
                                            break;
                                        }
                                    }
                                }
                                if (isindicated) hexRemovedLP.EraseColumn(index);
                            }
                        }
                        HeapPush(hexRemovedLP);
                    }
                    // lp unused
                    lp.ReleaseConstraints();
                    return false;
                }
                else if (shortestBridge != null && (threeTail == null || shortestBridge.Count < threeTail.Count))
                {
                    // break the cycle
                    // first compute indicators and for each indicator select a single representative
                    int M = shortestBridge.Count;
                    List<int> indicator = new List<int>();
                    for (int i = 0; i < M; i++)
                    {
                        int j = (i + 1) % M;
                        int ind = shortestBridge[i] & shortestBridge[j];
                        for (int k = 0; k < M; k++)
                        {
                            if (i == 0 || i == M - 1)
                            {
                                // bridge connection
                                if (k != i && k != j) ind &= ~shortestBridge[k];
                            }
                            else
                            {
                                // path
                                if (k != i && k != j && k != 0) ind &= ~shortestBridge[k];
                            }
                        }
                        int s = 1;
                        while ((s & ind) == 0) s <<= 1;
                        indicator.Add(s);
                    }
                    // break cycle by eliminating one pair of indicators
                    for (int i = 0; i < M; i++)
                    {
                        int j = (i + 1) % M;
                        SolverLP hexRemovedLP = lp.Clone();
                        if (hexRemovedLP.Log != null) hexRemovedLP.Log.AppendLine("Breaking bridge at boundary " + seg + ", removing " + indicator[i] + "+" + indicator[j]);
                        for (int index = 0; index < solutionVariable.Count; index++)
                        {
                            CastingState state = solutionVariable[index].State;
                            int iseg = solutionVariable[index].Segment;
                            if (state != null && (iseg == seg || iseg == seg + 1))
                            {
                                int h = (int)state.Cooldown;
                                bool isindicated = (h & indicator[i]) != 0 && (h & indicator[j]) != 0;
                                if (isindicated && i < M - 1) // for bridge it is indicated as long as it contains bridge connections
                                {
                                    for (int k = 0; k < M; k++)
                                    {
                                        if (k != i && k != j && (h & indicator[k]) != 0)
                                        {
                                            isindicated = false;
                                            break;
                                        }
                                    }
                                }
                                if (isindicated) hexRemovedLP.EraseColumn(index);
                            }
                        }
                        HeapPush(hexRemovedLP);
                    }
                    // lp unused
                    lp.ReleaseConstraints();
                    return false;
                }
                else if (threeTail != null)
                {
                    // find the indicators
                    List<int> indicator = new List<int>();
                    int core = threeTail[0];
                    int mask = (threeTail[1] | threeTail[2] | threeTail[3]) & ~(threeTail[1] & threeTail[2] & threeTail[3]);
                    SolverLP hexRemovedLP = lp.Clone();
                    if (hexRemovedLP.Log != null) hexRemovedLP.Log.AppendLine("Breaking threetail at boundary " + seg + ", removing core");
                    for (int index = 0; index < solutionVariable.Count; index++)
                    {
                        CastingState state = solutionVariable[index].State;
                        int iseg = solutionVariable[index].Segment;
                        if (state != null && (iseg == seg || iseg == seg + 1))
                        {
                            int h = (int)state.Cooldown;
                            bool isindicated = (h & mask) == (core & mask);
                            if (isindicated) hexRemovedLP.EraseColumn(index);
                        }
                    }
                    HeapPush(hexRemovedLP);
                    for (int i = 0; i < 3; i++)
                    {
                        hexRemovedLP = lp.Clone();
                        if (hexRemovedLP.Log != null) hexRemovedLP.Log.AppendLine("Breaking threetail at boundary " + seg + ", removing " + (threeTail[i + 1] & mask));
                        for (int index = 0; index < solutionVariable.Count; index++)
                        {
                            CastingState state = solutionVariable[index].State;
                            int iseg = solutionVariable[index].Segment;
                            if (state != null && (iseg == seg || iseg == seg + 1))
                            {
                                int h = (int)state.Cooldown;
                                bool isindicated = (h & mask) == (threeTail[i + 1] & mask);
                                if (isindicated) hexRemovedLP.EraseColumn(index);
                            }
                        }
                        HeapPush(hexRemovedLP);
                    }
                    // lp unused
                    lp.ReleaseConstraints();
                    return false;
                }

                // detect and eliminate double crossings
                // #X |X #
                // # Y| Y#
                // problem happens if we have X without Y and Y without X in one segment and X and Y
                // somewhere in the other segment
                // so either X-Y can't exist or Y-X or X and Y in the other segment
                // eliminate by 4-way branch
                int hex1 = 0;
                for (int i = 0; i < hexList1.Count; i++)
                {
                    hex1 |= hexList1[i];
                }
                int hex2 = 0;
                for (int i = 0; i < hexList2.Count; i++)
                {
                    hex2 |= hexList2[i];
                }
                int hex = hex1 & hex2; // crossings
                int cool1 = 0, cool2 = 0, seg1 = 0, seg2 = 0;
                for (int i = 0; i < hexList1.Count; i++)
                {
                    for (int j = i + 1; j < hexList1.Count; j++)
                    {
                        int placed = hex & hexList1[i];
                        int h = hex & hexList1[j];
                        int hp = h & placed;
                        if (placed != hp && h != hp)
                        {
                            // XXX XX   placed
                            //  XXXX    h
                            //  .. .    hp
                            // #  - #   placed - hp
                            // -  # -   h - hp
                            valid = false;
                            for (cool1 = 0; cool1 < cooldownCount; cool1++)
                            {
                                if (((1 << cool1) & placed & ~hp) != 0) break;
                            }
                            for (cool2 = 0; cool2 < cooldownCount; cool2++)
                            {
                                if (((1 << cool2) & h & ~hp) != 0) break;
                            }
                            seg1 = seg;
                            seg2 = seg + 1;
                            goto ELIMINATECROSSING;
                        }
                    }
                }
                for (int i = 0; i < hexList2.Count; i++)
                {
                    for (int j = i + 1; j < hexList2.Count; j++)
                    {
                        int placed = hex & hexList2[i];
                        int h = hex & hexList2[j];
                        int hp = h & placed;
                        if (placed != hp && h != hp)
                        {
                            // XXX XX   placed
                            //  XXXX    h
                            //  .. .    hp
                            // #  - #   placed - hp
                            // -  # -   h - hp
                            valid = false;
                            for (cool1 = 0; cool1 < cooldownCount; cool1++)
                            {
                                if (((1 << cool1) & placed & ~hp) != 0) break;
                            }
                            for (cool2 = 0; cool2 < cooldownCount; cool2++)
                            {
                                if (((1 << cool2) & h & ~hp) != 0) break;
                            }
                            seg1 = seg + 1;
                            seg2 = seg;
                            goto ELIMINATECROSSING;
                        }
                    }
                }
            ELIMINATECROSSING:
                if (!valid)
                {
                    int c1 = 1 << cool1;
                    int c2 = 1 << cool2;
                    // eliminate cool1 - cool2 in seg1
                    SolverLP elimLP = lp.Clone();
                    for (int index = 0; index < solutionVariable.Count; index++)
                    {
                        CastingState state = solutionVariable[index].State;
                        int iseg = solutionVariable[index].Segment;
                        if (state != null && iseg == seg1)
                        {
                            int h = (int)state.Cooldown;
                            if ((h & c1) != 0 && (h & c2) == 0) elimLP.EraseColumn(index);
                        }
                    }
                    if (elimLP.Log != null) elimLP.Log.AppendLine("Doublecrossing at " + seg1 + ", " + cool1 + " - " + cool2);
                    HeapPush(elimLP);
                    // eliminate cool2 - cool1 in seg1
                    elimLP = lp.Clone();
                    for (int index = 0; index < solutionVariable.Count; index++)
                    {
                        CastingState state = solutionVariable[index].State;
                        int iseg = solutionVariable[index].Segment;
                        if (state != null && iseg == seg1)
                        {
                            int h = (int)state.Cooldown;
                            if ((h & c2) != 0 && (h & c1) == 0) elimLP.EraseColumn(index);
                        }
                    }
                    if (elimLP.Log != null) elimLP.Log.AppendLine("Doublecrossing at " + seg1 + ", " + cool2 + " - " + cool1);
                    HeapPush(elimLP);
                    // eliminate cool1 in seg2
                    elimLP = lp.Clone();
                    for (int index = 0; index < solutionVariable.Count; index++)
                    {
                        CastingState state = solutionVariable[index].State;
                        int iseg = solutionVariable[index].Segment;
                        if (state != null && iseg == seg2)
                        {
                            int h = (int)state.Cooldown;
                            if ((h & c1) != 0) elimLP.EraseColumn(index);
                        }
                    }
                    if (elimLP.Log != null) elimLP.Log.AppendLine("Doublecrossing at " + seg1 + "+1, delete " + cool1);
                    HeapPush(elimLP);
                    // eliminate cool2 in seg2
                    elimLP = lp;
                    for (int index = 0; index < solutionVariable.Count; index++)
                    {
                        CastingState state = solutionVariable[index].State;
                        int iseg = solutionVariable[index].Segment;
                        if (state != null && iseg == seg2)
                        {
                            int h = (int)state.Cooldown;
                            if ((h & c2) != 0) elimLP.EraseColumn(index);
                        }
                    }
                    if (elimLP.Log != null) elimLP.Log.AppendLine("Doublecrossing at " + seg1 + "+1, delete " + cool2);
                    HeapPush(elimLP);
                    break;
                }
            }
            return valid;
        }

        private bool ValidateIntegralConsumable(VariableType integralConsumable)
        {
            for (int index = 0; index < solutionVariable.Count; index++)
            {
                if (solutionVariable[index].Type == integralConsumable)
                {
                    double value = solution[index];
                    int count = (int)Math.Round(value);
                    bool valid = (Math.Abs(value - count) < 0.000001);

                    if (!valid)
                    {
                        SolverLP maxCount = lp.Clone();
                        // count >= ceiling(value)
                        lp.SetColumnLowerBound(index, Math.Ceiling(value));
                        lp.ForceRecalculation(true);
                        if (lp.Log != null) lp.Log.AppendLine("Integral consumable " + integralConsumable + " at " + solutionVariable[index].Segment + ", min " + Math.Ceiling(value));
                        HeapPush(lp);
                        // count <= floor(value)
                        maxCount.SetColumnUpperBound(index, Math.Floor(value));
                        maxCount.ForceRecalculation(true);
                        if (maxCount.Log != null) maxCount.Log.AppendLine("Integral consumable " + integralConsumable + " at " + solutionVariable[index].Segment + ", max " + Math.Floor(value));
                        HeapPush(maxCount);
                        return false;
                    }
                }
            }
            return true;
        }

        private bool ValidateIntegralConsumableOverall(VariableType integralConsumable, double unit)
        {
            double value = 0.0;
            for (int index = 0; index < solutionVariable.Count; index++)
            {
                if (solutionVariable[index].Type == integralConsumable)
                {
                    value += solution[index];
                }
                else if (integralConsumable == VariableType.Evocation && solutionVariable[index].Type == VariableType.EvocationIV)
                {
                    value += solution[index] * 1.2;
                }
                else if (integralConsumable == VariableType.Evocation && solutionVariable[index].Type == VariableType.EvocationHero)
                {
                    value += solution[index] * 1.3;
                }
                else if (integralConsumable == VariableType.Evocation && solutionVariable[index].Type == VariableType.EvocationIVHero)
                {
                    value += solution[index] * 1.2 * 1.3;
                }
                else if (integralConsumable == VariableType.EvocationHero && solutionVariable[index].Type == VariableType.EvocationIVHero)
                {
                    value += solution[index] * 1.2;
                }
            }
            double count = Math.Round(value / unit) * unit;
            bool valid = (Math.Abs(value - count) < 0.0001); // had to loosen the verification, seem to have problems with hasted evocation, maybe because of the activation constraints?
            int row = -1;
            switch (integralConsumable)
            {
                case VariableType.ManaGem:
                    row = rowManaGemMax;
                    break;
                case VariableType.ManaPotion:
                    row = rowManaPotion;
                    break;
                case VariableType.Evocation:
                    row = rowEvocation;
                    break;
                case VariableType.EvocationIV:
                    row = rowEvocationIV;
                    break;
                case VariableType.EvocationHero:
                    row = rowEvocationHero;
                    break;
                case VariableType.EvocationIVHero:
                    row = rowEvocationIVHero;
                    break;
                case VariableType.DrumsOfBattle:
                    row = rowDrumsOfBattle;
                    break;
                case VariableType.SummonWaterElemental:
                    row = rowSummonWaterElementalCount;
                    break;
                case VariableType.ConjureManaGem:
                    row = rowConjureManaGem;
                    break;
            }
            if (!valid)
            {
                SolverLP maxCount = lp.Clone();
                // count <= floor(value)
                maxCount.SetRHS(row, Math.Floor(value / unit) * unit);
                maxCount.ForceRecalculation(true);
                if (maxCount.Log != null) maxCount.Log.AppendLine("Integral consumable " + integralConsumable + " overall, max " + Math.Floor(value / unit));
                HeapPush(maxCount);
                // count >= ceiling(value)
                lp.SetLHS(row, Math.Ceiling(value / unit) * unit);
                lp.ForceRecalculation(true);
                if (lp.Log != null) lp.Log.AppendLine("Integral consumable " + integralConsumable + " overall, min " + Math.Ceiling(value / unit));
                HeapPush(lp);
                return false;
            }
            return true;
        }

        private bool ValidateColdsnap()
        {
            const double eps = 0.000001;
            double[] ivCount = GetSegmentCooldownCount(Cooldown.IcyVeins, VariableType.None);
            double[] weCount = GetSegmentCooldownCount(Cooldown.WaterElemental, VariableType.None);

            // everything is valid, except possibly coldsnap so we can assume the effects are nicely packed
            // check where coldsnaps are needed, similar to evaluation in sequence reconstruction
            bool ivActive = false;
            bool weActive = false;
            double lastIVstart = double.NegativeInfinity;
            double lastWEstart = double.NegativeInfinity;
            double ivCooldown = 0.0;
            double weCooldown = 0.0;
            bool valid = true;
            double coldsnapTimeMin = double.NegativeInfinity;
            double coldsnapTimeMax = double.NegativeInfinity;
            List<double> ivStart = new List<double>();
            List<double> weStart = new List<double>();
            double e1t1 = double.NegativeInfinity;
            double e1t2 = double.NegativeInfinity;
            Cooldown e1 = Cooldown.IcyVeins;
            double e2t1 = double.NegativeInfinity;
            double e2t2 = double.NegativeInfinity;
            Cooldown e2 = Cooldown.IcyVeins;
            for (int seg = 0; seg < segmentList.Count; seg++)
            {
                if (!ivActive && ivCount[seg] > eps)
                {
                    if (ivCooldown + ivCount[seg] > segmentList[seg].Duration + eps)
                    {
                        bool reuse = false;
                        // we need to coldsnap somewhere from [lastIVstart] and [(seg + 1) * segmentDuration - ivCount[seg]]
                        double restrictedColdsnapMin = Math.Max(coldsnapTimeMin, lastIVstart);
                        double restrictedColdsnapMax = Math.Min(coldsnapTimeMax, segmentList[seg].TimeEnd - ivCount[seg]);
                        if (restrictedColdsnapMax > restrictedColdsnapMin + eps)
                        {
                            // we can reuse last coldsnap
                            coldsnapTimeMin = restrictedColdsnapMin;
                            coldsnapTimeMax = restrictedColdsnapMax;
                            ivCooldown = 0.0;
                            reuse = true;
                        }
                        else if (calculationResult.ColdsnapCooldown - (segmentList[seg].TimeEnd - ivCount[seg] - coldsnapTimeMin) <= eps)
                        {
                            // coldsnap is ready
                            coldsnapTimeMin = Math.Max(coldsnapTimeMin + calculationResult.ColdsnapCooldown, lastIVstart);
                            coldsnapTimeMax = segmentList[seg].TimeEnd - ivCount[seg];
                            ivCooldown = 0.0;
                        }
                        else
                        {
                            // coldsnap is not ready
                            ivStart.Add(segmentList[seg].TimeEnd - ivCount[seg]);
                            e1 = e2;
                            e1t1 = e2t1;
                            e1t2 = e2t2;
                            e2 = Cooldown.IcyVeins;
                            e2t1 = ivStart[ivStart.Count - 2];
                            e2t2 = ivStart[ivStart.Count - 1];
                            valid = false;
                            break;
                        }
                        double ivActivation = Math.Max(coldsnapTimeMin, segmentList[seg].TimeStart);
                        if (seg + 1 < segmentList.Count && ivCount[seg + 1] > 0) ivActivation = Math.Max(coldsnapTimeMin, segmentList[seg].TimeEnd - ivCount[seg]);
                        ivStart.Add(ivActivation);
                        if (!reuse)
                        {
                            e1 = e2;
                            e1t1 = e2t1;
                            e1t2 = e2t2;
                            e2 = Cooldown.IcyVeins;
                            e2t1 = ivStart[ivStart.Count - 2];
                            e2t2 = ivStart[ivStart.Count - 1];
                        }
                        ivCooldown = calculationResult.IcyVeinsCooldown + ivActivation - segmentList[seg].TimeStart;
                        ivActive = true;
                        lastIVstart = ivActivation;
                    }
                    else
                    {
                        // start as soon as possible
                        double ivActivation = Math.Max(segmentList[seg].TimeStart + ivCooldown, segmentList[seg].TimeStart);
                        if (seg + 1 < segmentList.Count && ivCount[seg + 1] > 0) ivActivation = segmentList[seg].TimeEnd - ivCount[seg];
                        ivStart.Add(ivActivation);
                        ivCooldown = calculationResult.IcyVeinsCooldown + ivActivation - segmentList[seg].TimeStart;
                        ivActive = true;
                        lastIVstart = ivActivation;
                    }
                }
                if (ivActive)
                {
                    if (ivCount[seg] > 0.0)
                    {
                        if (segmentList[seg].TimeStart + ivCount[seg] > lastIVstart + 20.0 + eps)
                        {
                            // we need to coldsnap somewhere from [ivTime] and [ivTime + 20]
                            double restrictedColdsnapMin = Math.Max(coldsnapTimeMin, lastIVstart);
                            double restrictedColdsnapMax = Math.Min(coldsnapTimeMax, lastIVstart + 20.0);
                            if (restrictedColdsnapMax > restrictedColdsnapMin + eps)
                            {
                                // we can reuse last coldsnap
                                coldsnapTimeMin = restrictedColdsnapMin;
                                coldsnapTimeMax = restrictedColdsnapMax;
                                lastIVstart += 20.0;
                                ivStart.Add(lastIVstart);
                                //e1 = e2;
                                //e1t1 = e2t1;
                                //e1t2 = e2t2;
                                //e2 = Cooldown.IcyVeins;
                                //e2t1 = ivStart[ivStart.Count - 2];
                                //e2t2 = ivStart[ivStart.Count - 1];
                                ivCooldown += 20.0;
                            }
                            else if (calculationResult.ColdsnapCooldown + coldsnapTimeMin <= lastIVstart + 20.0 + eps)
                            {
                                coldsnapTimeMin = Math.Max(coldsnapTimeMin + calculationResult.ColdsnapCooldown, lastIVstart);
                                coldsnapTimeMax = lastIVstart + 20.0;
                                lastIVstart += 20.0;
                                if (ivCount[seg] >= 20.0 - eps && ivCount[seg] <= 20.0 + eps)
                                {
                                    // VERY VERY special case, experimental
                                    // this IV has room to move around a bit and placing it at the very start
                                    // in many cases causes bad coldsnap interaction with WE, so nudge it a bit
                                    coldsnapTimeMax += calculationResult.BaseState.GlobalCooldown;
                                    lastIVstart += calculationResult.BaseState.GlobalCooldown;
                                }
                                ivStart.Add(lastIVstart);
                                e1 = e2;
                                e1t1 = e2t1;
                                e1t2 = e2t2;
                                e2 = Cooldown.IcyVeins;
                                e2t1 = ivStart[ivStart.Count - 2];
                                e2t2 = ivStart[ivStart.Count - 1];
                                ivCooldown += 20.0;
                            }
                            if (segmentList[seg].TimeStart + ivCount[seg] > lastIVstart + 20.0 + eps)
                            {
                                // we need to coldsnap iv, but it is on cooldown
                                ivStart.Add(lastIVstart + 20.0);
                                e1 = e2;
                                e1t1 = e2t1;
                                e1t2 = e2t2;
                                e2 = Cooldown.IcyVeins;
                                e2t1 = ivStart[ivStart.Count - 2];
                                e2t2 = ivStart[ivStart.Count - 1];
                                valid = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        ivActive = false;
                    }
                }
                if (!weActive && weCount[seg] > eps)
                {
                    if (weCooldown + weCount[seg] > segmentList[seg].Duration + eps)
                    {
                        bool reuse = false;
                        // we need to coldsnap somewhere from [lastWEstart] and [(seg + 1) * segmentDuration - weCount[seg]]
                        double restrictedColdsnapMin = Math.Max(coldsnapTimeMin, lastWEstart);
                        double restrictedColdsnapMax = Math.Min(coldsnapTimeMax, segmentList[seg].TimeEnd - weCount[seg]);
                        if (restrictedColdsnapMax > restrictedColdsnapMin + eps)
                        {
                            // we can reuse last coldsnap
                            coldsnapTimeMin = restrictedColdsnapMin;
                            coldsnapTimeMax = restrictedColdsnapMax;
                            weCooldown = 0.0;
                            reuse = true;
                        }
                        else if (calculationResult.ColdsnapCooldown - (segmentList[seg].TimeEnd - weCount[seg] - coldsnapTimeMin) <= eps)
                        {
                            // coldsnap is ready
                            coldsnapTimeMin = Math.Max(coldsnapTimeMin + calculationResult.ColdsnapCooldown, lastWEstart);
                            coldsnapTimeMax = segmentList[seg].TimeEnd - weCount[seg];
                            weCooldown = 0.0;
                        }
                        else
                        {
                            // coldsnap is not ready
                            weStart.Add(segmentList[seg].TimeEnd - weCount[seg]);
                            e1 = e2;
                            e1t1 = e2t1;
                            e1t2 = e2t2;
                            e2 = Cooldown.WaterElemental;
                            e2t1 = weStart[weStart.Count - 2];
                            e2t2 = weStart[weStart.Count - 1];
                            valid = false;
                            break;
                        }
                        double weActivation = Math.Max(coldsnapTimeMin, segmentList[seg].TimeStart);
                        if (seg + 1 < segmentList.Count && weCount[seg + 1] > 0) weActivation = Math.Max(coldsnapTimeMin, segmentList[seg].TimeEnd - weCount[seg]);
                        weStart.Add(weActivation);
                        if (!reuse)
                        {
                            e1 = e2;
                            e1t1 = e2t1;
                            e1t2 = e2t2;
                            e2 = Cooldown.WaterElemental;
                            e2t1 = weStart[weStart.Count - 2];
                            e2t2 = weStart[weStart.Count - 1];
                        }
                        weCooldown = calculationResult.WaterElementalCooldown + weActivation - segmentList[seg].TimeStart;
                        weActive = true;
                        lastWEstart = weActivation;
                    }
                    else
                    {
                        // start as soon as possible
                        double weActivation = Math.Max(segmentList[seg].TimeStart + weCooldown, segmentList[seg].TimeStart);
                        if (seg + 1 < segmentList.Count && weCount[seg + 1] > 0) weActivation = segmentList[seg].TimeEnd - weCount[seg];
                        weStart.Add(weActivation);
                        weCooldown = calculationResult.WaterElementalCooldown + weActivation - segmentList[seg].TimeStart;
                        weActive = true;
                        lastWEstart = weActivation;
                    }
                }
                if (weActive)
                {
                    if (weCount[seg] > 0.0)
                    {
                        if (segmentList[seg].TimeStart + weCount[seg] > lastWEstart + calculationResult.WaterElementalDuration + eps)
                        {
                            // we need to coldsnap somewhere from [weTime] and [weTime + 45]
                            double restrictedColdsnapMin = Math.Max(coldsnapTimeMin, lastWEstart);
                            double restrictedColdsnapMax = Math.Min(coldsnapTimeMax, lastWEstart + calculationResult.WaterElementalDuration);
                            if (restrictedColdsnapMax > restrictedColdsnapMin + eps)
                            {
                                // we can reuse last coldsnap
                                coldsnapTimeMin = restrictedColdsnapMin;
                                coldsnapTimeMax = restrictedColdsnapMax;
                                lastWEstart += calculationResult.WaterElementalDuration;
                                weStart.Add(lastWEstart);
                                //e1 = e2;
                                //e1t1 = e2t1;
                                //e1t2 = e2t2;
                                //e2 = Cooldown.WaterElemental;
                                //e2t1 = weStart[weStart.Count - 2];
                                //e2t2 = weStart[weStart.Count - 1];
                                weCooldown += calculationResult.WaterElementalDuration;
                            }
                            else if (calculationResult.ColdsnapCooldown + coldsnapTimeMin <= lastWEstart + calculationResult.WaterElementalDuration + eps)
                            {
                                coldsnapTimeMin = Math.Max(coldsnapTimeMin + calculationResult.ColdsnapCooldown, lastWEstart);
                                coldsnapTimeMax = lastWEstart + calculationResult.WaterElementalDuration;
                                lastWEstart += calculationResult.WaterElementalDuration;
                                weStart.Add(lastWEstart);
                                e1 = e2;
                                e1t1 = e2t1;
                                e1t2 = e2t2;
                                e2 = Cooldown.WaterElemental;
                                e2t1 = weStart[weStart.Count - 2];
                                e2t2 = weStart[weStart.Count - 1];
                                weCooldown += calculationResult.WaterElementalDuration;
                            }
                            if (segmentList[seg].TimeStart + weCount[seg] > lastWEstart + calculationResult.WaterElementalDuration + eps)
                            {
                                // we need to coldsnap iv, but it is on cooldown
                                weStart.Add(lastWEstart + calculationResult.WaterElementalDuration);
                                e1 = e2;
                                e1t1 = e2t1;
                                e1t2 = e2t2;
                                e2 = Cooldown.WaterElemental;
                                e2t1 = weStart[weStart.Count - 2];
                                e2t2 = weStart[weStart.Count - 1];
                                valid = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        weActive = false;
                    }
                }
                ivCooldown -= segmentList[seg].Duration;
                weCooldown -= segmentList[seg].Duration;
            }
            if (!valid)
            {
                // there are three possible constructions that are not valid
                // 3 consecutive activations of same cooldown
                // 2 pairs of activations of same cooldown
                // 2 pairs of activations of different cooldowns
                // each configuration has a different way to resolve infeasibility

                // TODO right now this should have an exhaustive coverage of all the cases
                // but might not be ideal for branching, examine branching behavior and optimize

                // in some cases reusing of coldsnap for two effects will miss the fact that we have coldsnap problem within the same cooldown
                // those should have priority, because if they are not resolved they cause problem with estimating cross cooldown behavior
                if (e1 != e2)
                {
                    int firstColdsnap = -1;
                    for (int i = 0; i < ivStart.Count - 1; i++)
                    {
                        if (ivStart[i + 1] - ivStart[i] < calculationResult.IcyVeinsCooldown - eps)
                        {
                            if (firstColdsnap == -1)
                            {
                                firstColdsnap = i;
                            }
                            else
                            {
                                if (ivStart[i + 1] - ivStart[firstColdsnap] < calculationResult.ColdsnapCooldown - eps)
                                {
                                    // we have intra cooldown problem
                                    e1t1 = ivStart[firstColdsnap];
                                    e1t2 = ivStart[firstColdsnap + 1];
                                    e2t1 = ivStart[i];
                                    e2t2 = ivStart[i + 1];
                                    e1 = Cooldown.IcyVeins;
                                    e2 = Cooldown.IcyVeins;
                                    break;
                                }
                                else
                                {
                                    firstColdsnap = i;
                                }
                            }
                        }
                    }
                    if (e1 != e2)
                    {
                        firstColdsnap = -1;
                        for (int i = 0; i < weStart.Count - 1; i++)
                        {
                            if (weStart[i + 1] - weStart[i] < calculationResult.WaterElementalCooldown - eps)
                            {
                                if (firstColdsnap == -1)
                                {
                                    firstColdsnap = i;
                                }
                                else
                                {
                                    if (weStart[i + 1] - weStart[firstColdsnap] < calculationResult.ColdsnapCooldown - eps)
                                    {
                                        // we have intra cooldown problem
                                        e1t1 = weStart[firstColdsnap];
                                        e1t2 = weStart[firstColdsnap + 1];
                                        e2t1 = weStart[i];
                                        e2t2 = weStart[i + 1];
                                        e1 = Cooldown.WaterElemental;
                                        e2 = Cooldown.WaterElemental;
                                        break;
                                    }
                                    else
                                    {
                                        firstColdsnap = i;
                                    }
                                }
                            }
                        }
                    }
                }

                if (e1 == e2)
                {
                    // 2 pairs of activations of same cooldown, t3 can be equal to t2
                    double t1 = e1t1;
                    double t2 = e1t2;
                    double t3 = e2t1;
                    double t4 = e2t2;
                    Cooldown e = e1;
                    double c = (e == Cooldown.IcyVeins) ? calculationResult.IcyVeinsCooldown : calculationResult.WaterElementalCooldown;
                    double d = (e == Cooldown.IcyVeins) ? 20.0 : calculationResult.WaterElementalDuration;

                    // branch on one of the conditions responsible for infeasibility
                    // t2 - t1 < c1
                    EnforceEffectCooldown(t1, e, c, d, true);
                    // t4 - t3 < c2
                    EnforceEffectCooldown(t3, e, c, d, true);
                    // t4 - t1 < cs
                    // cs is at least 384 sec
                    // notice that t3 >= t2, then t4 - t1 <= (t4 - t3) + (t2 - t1)
                    // to break this condition at least one of pairs has to be at min 192 sec apart
                    // since IV and WE cooldown is 180 sec at most this means that breaking this
                    // constraint automatically breaks one of the other constraints also
                }
                else
                {
                    // 2 pairs of activations of different cooldowns
                    double t1 = e1t1;
                    double t2 = e1t2;
                    double t3 = e2t1;
                    double t4 = e2t2;
                    double c1 = (e1 == Cooldown.IcyVeins) ? calculationResult.IcyVeinsCooldown : calculationResult.WaterElementalCooldown;
                    double d1 = (e1 == Cooldown.IcyVeins) ? 20.0 : calculationResult.WaterElementalDuration;
                    double c2 = (e2 == Cooldown.IcyVeins) ? calculationResult.IcyVeinsCooldown : calculationResult.WaterElementalCooldown;
                    double d2 = (e2 == Cooldown.IcyVeins) ? 20.0 : calculationResult.WaterElementalDuration;

                    // branch on one of the conditions responsible for infeasibility
                    // t2 - t1 < c1
                    EnforceEffectCooldown(t1, e1, c1, d1, true);
                    // t4 - t3 < c2
                    EnforceEffectCooldown(t3, e2, c2, d2, true);
                    // t3 >= t2
                    // [----]             [-----]
                    //                      [------------][-------------]
                    int seg2 = segmentList.FindIndex(segment => segment.TimeEnd > t2 + eps); //(int)((t2 + eps) / segmentDuration);
                    int seg3 = segmentList.FindIndex(segment => segment.TimeEnd > t3 + eps); //(int)((t3 + eps) / segmentDuration);
                    // assume e2 will start in seg3, if it does not that is already examined in t4 - t3 < c2 case
                    // so in this case we have to force e1 to not start before seg3
                    SolverLP branchlp = lp.Clone();
                    if (branchlp.Log != null) branchlp.Log.AppendLine("Force shared coldsnap activation after " + seg3);
                    if (seg2 < seg3) DisableCooldown(branchlp, e1, seg2, seg3 - 1);
                    // make sure t2 is after t3
                    // but it has to be at least a gcd later, or we won't have enough room to coldsnap
                    // t2 = (seg3 + 1) * segmentDuration - count1[seg3]
                    // t3 = (seg3 + 1) * segmentDuration - count2[seg3]
                    // count1[seg3] - count2[seg3] <= - gcd
                    // if e1 is WE then forcing t2 after t3 will cause WE summon to be during IV active
                    // this is currently not supported, so make sure in this case the shift is big enough that summon comes after IV is over
                    int row = branchlp.AddConstraint();
                    SetCooldownElements(branchlp, row, e1, seg3, 1.0);
                    SetCooldownElements(branchlp, row, e2, seg3, -1.0);
                    if (e1 == Cooldown.WaterElemental)
                    {
                        branchlp.SetConstraintRHS(row, -20.0);
                    }
                    else
                    {
                        branchlp.SetConstraintRHS(row, -calculationResult.BaseState.GlobalCooldown);
                    }
                    branchlp.ForceRecalculation(true);
                    HeapPush(branchlp);
                    if (e2 == Cooldown.IcyVeins)
                    {
                        // in this case we have an additional option to nudge IV not to cross segments
                        branchlp = lp.Clone();
                        if (branchlp.Log != null) branchlp.Log.AppendLine("Force shared coldsnap activation after " + seg3 + ", nudge IV");
                        if (seg2 < seg3) DisableCooldown(branchlp, e1, seg2, seg3 - 1);
                        DisableCooldown(branchlp, e2, seg3 + 1);
                        HeapPush(branchlp);
                    }
                    // t4 - t1 < cs
                    // cs is at least 384 sec
                    // if t3 >= t2, then similar to above at least one pair has to be 192 sec apart
                    // if not then the t3 >= t2 condition is broken, so we can ignore this constraint in this case also
                }
                // lp unused
                lp.ReleaseConstraints();
                return false;
            }
            return valid;
        }

        private void EnforceRemoteEffectCooldown(int seg1, int seg3, int cooldowns, Cooldown effect, double effectCooldown, double effectDuration)
        {
            const double eps = 0.000001;
            // example case:
            // activations at seg1, seg2, seg3
            // we know that activations are ok consecutively
            // (seg2 - seg1 + 1) * segmentDuration - segCount[seg2] >= effectCooldown +? (segmentDuration - segCount[seg1])
            // (seg3 - seg2 + 1) * segmentDuration - segCount[seg3] >= effectCooldown +? (segmentDuration - segCount[seg2])
            // from this it follows that
            // (seg3 - seg1 + 2) * segmentDuration - segCount[seg2] - segCount[seg3] >= 2 * effectCooldown +? (segmentDuration - segCount[seg1]) +? (segmentDuration - segCount[seg2])
            // (seg3 - seg1 + 1) * segmentDuration - segCount[seg3] + (segmentDuration - segCount[seg2]) >= 2 * effectCooldown +? (segmentDuration - segCount[seg1]) +? (segmentDuration - segCount[seg2])
            // (seg3 - seg1 + 1) * segmentDuration - segCount[seg3] >= 2 * effectCooldown +? (segmentDuration - segCount[seg1]) +? (segmentDuration - segCount[seg2]) - (segmentDuration - segCount[seg2])
            // if seg2 is a free segment then we're loose by (segmentDuration - segCount[seg2])
            // on more remote activations each loose activation in between increases the gap and can increase over one segmentDuration
            // however if all less remote activations are satisfied then the looseness is hard-limited to the least free segment (if there is at least one right bound activation in between then this is auto satisfied)
            // 1 >= (1 - segCount[seg2] / segmentDuration) >= 2 * effectCooldown / segmentDuration +? (1 - segCount[seg1] / segmentDuration) - (seg3 - seg1 + 1) + segCount[seg3] / segmentDuration >= 0

            // if seg1 free then seg2 starting late gives
            // (seg2 - seg1) * segmentDuration >= ([effectCooldown / segmentDuration] + 1) * segmentDuration
            // (seg3 - seg2 + 1) * segmentDuration - segCount[seg3] >= effectCooldown +? (segmentDuration - segCount[seg2])
            // (seg3 - seg1 + 1) * segmentDuration - segCount[seg3] >= (effectCooldown + ([effectCooldown / segmentDuration] + 1) * segmentDuration) +? (segmentDuration - segCount[seg2]) >=? 2 * effectCooldown +? (segmentDuration - segCount[seg1])
            // so its auto satisfied
            // if seg1 is right bound then
            // (seg2 - seg1) * segmentDuration >= 2 * segmentDuration + [(effectCooldown - segCount[seg1]) / segmentDuration] * segmentDuration
            // (seg3 - seg2 + 1) * segmentDuration - segCount[seg3] >= effectCooldown +? (segmentDuration - segCount[seg2])
            // (seg3 - seg1 + 1) * segmentDuration - segCount[seg3] >= effectCooldown +? (segmentDuration - segCount[seg2]) + 2 * segmentDuration + [(effectCooldown - segCount[seg1]) / segmentDuration] * segmentDuration >=? 2 * effectCooldown + segmentDuration - segCount[seg1]
            // segmentDuration + [(effectCooldown - segCount[seg1]) / segmentDuration] * segmentDuration >=? effectCooldown - segCount[seg1]
            // in all cases seg2 activation has to happen exactly in that segment for this situation to happen

            // to eliminate this situation therefore we have the following branches
            // seg1 is not free and we restrict the distance
            // seg1 is free and we restrict the distance
            // no activation at seg1
            // no activation at seg2
            // no activation at seg3

            SolverLP branchlp = lp.Clone();
            if (branchlp.Log != null) branchlp.Log.AppendLine("Restrict remote activation of " + effect + " at " + seg1);
            // make sure there is enough distance between the activations
            // t1 = (seg1 + 1) * segmentDuration - count[seg1]
            // t2 = (seg3 + 1) * segmentDuration - count[seg3]
            // count[seg3] - count[seg1] <= (seg3 - seg1) * segmentDuration - c
            int row = branchlp.AddConstraint();
            SetCooldownElements(branchlp, row, effect, seg1, -1.0);
            SetCooldownElements(branchlp, row, effect, seg3, 1.0);
            branchlp.SetConstraintRHS(row, segmentList[seg3].TimeEnd - segmentList[seg1].TimeEnd - cooldowns * effectCooldown);
            branchlp.ForceRecalculation(true);
            HeapPush(branchlp);
            if (effectDuration < segmentList[seg1].Duration - eps)
            {
                double buffer = 0.0;
                branchlp = lp.Clone();
                if (branchlp.Log != null) branchlp.Log.AppendLine("Restrict remote activation of " + effect + " at " + seg1 + ", version for short effects");
                // make sure seg1 is actually a free activation
                DisableCooldown(branchlp, effect, seg1 + 1);
                // make sure there is enough distance between the activations
                // t1 = seg1 * segmentDuration
                // t2 = (seg3 + 1) * segmentDuration - count[seg3]
                // count[seg3] <= (seg3 - seg1 + 1) * segmentDuration - c
                row = AddConstraint(branchlp, effect, seg3);
                branchlp.SetConstraintRHS(row, segmentList[seg3].TimeEnd - segmentList[seg1].TimeStart - cooldowns * effectCooldown - buffer);
                branchlp.ForceRecalculation(true);
                HeapPush(branchlp);
            }
            // if first activation moves to seg1 - 1
            if (seg1 >= 1) RestrictConsecutiveActivation(effect, effectCooldown, effectDuration, seg1 - 1, false);
            // if seg1 is disabled
            branchlp = lp.Clone();
            if (branchlp.Log != null) branchlp.Log.AppendLine("Enforce remote cooldown after " + effect + " at " + seg1 + ", remove effect at " + seg1);
            DisableCooldown(branchlp, effect, seg1);
            HeapPush(branchlp);
            // if seg3 is disabled
            branchlp = lp.Clone();
            if (branchlp.Log != null) branchlp.Log.AppendLine("Enforce remote cooldown after " + effect + " at " + seg1 + ", remove effect at " + seg3);
            DisableCooldown(branchlp, effect, seg3);
            HeapPush(branchlp);
            // restrict intermediate activations
            branchlp = lp.Clone();
            if (branchlp.Log != null) branchlp.Log.AppendLine("Enforce remote cooldown after " + effect + " at " + seg1 + ", remove intermediate effects");
            row = AddConstraint(branchlp, effect, seg1, seg3 - 1);
            branchlp.SetConstraintRHS(row, (cooldowns - 1) * effectDuration);
            branchlp.ForceRecalculation(true);
            HeapPush(branchlp);
        }

        private void BranchOnLeftPadding(ActivationConstraints act, Cooldown effect)
        {
            SolverLP branchlp;
            int seg = act.Segment;
            // dropping right link: remove joint items with effect in seg1 or remove items in seg1+1 for each effect from right link
            for (int cool = 0; cool < cooldownCount; cool++)
            {
                Cooldown cooldown = (Cooldown)(1 << cool);
                if (cooldown != effect && (cooldown & act.RightLink) != 0)
                {
                    branchlp = lp.Clone();
                    if (branchlp.Log != null) branchlp.Log.AppendLine("Remove left padding for " + effect + " at " + seg + ", drop right link here " + cooldown);
                    DisableCooldown(branchlp, effect | cooldown, seg);
                    branchlp.ForceRecalculation(true);
                    HeapPush(branchlp);
                    branchlp = lp.Clone();
                    if (branchlp.Log != null) branchlp.Log.AppendLine("Remove left padding for " + effect + " at " + seg + ", drop right link there " + cooldown);
                    DisableCooldown(branchlp, cooldown, seg + 1);
                    branchlp.ForceRecalculation(true);
                    HeapPush(branchlp);
                }
            }
            // dropping left full link: remove items in seg1 or in seg1-1 for all effects from full left link that are not in left link
            for (int cool = 0; cool < cooldownCount; cool++)
            {
                Cooldown cooldown = (Cooldown)(1 << cool);
                if (cooldown != effect && (cooldown & act.FullLeftLink & ~act.LeftLink) != 0)
                {
                    branchlp = lp.Clone();
                    if (branchlp.Log != null) branchlp.Log.AppendLine("Remove left padding for " + effect + " at " + seg + ", drop left full link here " + cooldown);
                    DisableCooldown(branchlp, cooldown, seg);
                    branchlp.ForceRecalculation(true);
                    HeapPush(branchlp);
                    branchlp = lp.Clone();
                    if (branchlp.Log != null) branchlp.Log.AppendLine("Remove left padding for " + effect + " at " + seg + ", drop left full link there " + cooldown);
                    DisableCooldown(branchlp, cooldown, seg - 1);
                    branchlp.ForceRecalculation(true);
                    HeapPush(branchlp);
                }
            }
            // increasing left link: for each effect not in left link force there to be some left link+effect items on both sides
            if (seg > 0)
            {
                for (int cool = 0; cool < cooldownCount; cool++)
                {
                    Cooldown cooldown = (Cooldown)(1 << cool);
                    if (cooldown != effect && (cooldown & availableCooldownMask & ~act.LeftLink) != 0)
                    {
                        // these are expensive branches, hopefully they die out quick
                        branchlp = lp.Clone();
                        if (branchlp.Log != null) branchlp.Log.AppendLine("Remove left padding for " + effect + " at " + seg + ", increase left link " + cooldown);
                        int row = AddConstraint(branchlp, cooldown, seg - 1);
                        branchlp.SetConstraintRHS(row, segmentList[seg - 1].Duration);
                        branchlp.SetConstraintLHS(row, 0.1);
                        row = AddConstraint(branchlp, effect | cooldown, seg);
                        branchlp.SetConstraintRHS(row, segmentList[seg].Duration);
                        branchlp.SetConstraintLHS(row, 0.1);
                        branchlp.ForceRecalculation(true);
                        HeapPush(branchlp);
                    }
                }
            }
        }

        private void BranchOnRightPadding(ActivationConstraints act, Cooldown effect)
        {
            SolverLP branchlp;
            int seg = act.Segment;
            // dropping right link: remove joint items with effect in seg1 or remove items in seg1+1 for each effect from right link
            for (int cool = 0; cool < cooldownCount; cool++)
            {
                Cooldown cooldown = (Cooldown)(1 << cool);
                if (cooldown != effect && (cooldown & act.LeftLink) != 0)
                {
                    branchlp = lp.Clone();
                    if (branchlp.Log != null) branchlp.Log.AppendLine("Remove right padding for " + effect + " at " + seg + ", drop left link here " + cooldown);
                    DisableCooldown(branchlp, effect | cooldown, seg);
                    branchlp.ForceRecalculation(true);
                    HeapPush(branchlp);
                    branchlp = lp.Clone();
                    if (branchlp.Log != null) branchlp.Log.AppendLine("Remove right padding for " + effect + " at " + seg + ", drop left link there " + cooldown);
                    DisableCooldown(branchlp, cooldown, seg - 1);
                    branchlp.ForceRecalculation(true);
                    HeapPush(branchlp);
                }
            }
            // dropping left full link: remove items in seg1 or in seg1-1 for all effects from full left link that are not in left link
            for (int cool = 0; cool < cooldownCount; cool++)
            {
                Cooldown cooldown = (Cooldown)(1 << cool);
                if (cooldown != effect && (cooldown & act.FullRightLink & ~act.RightLink) != 0)
                {
                    branchlp = lp.Clone();
                    if (branchlp.Log != null) branchlp.Log.AppendLine("Remove right padding for " + effect + " at " + seg + ", drop right full link here " + cooldown);
                    DisableCooldown(branchlp, cooldown, seg);
                    branchlp.ForceRecalculation(true);
                    HeapPush(branchlp);
                    branchlp = lp.Clone();
                    if (branchlp.Log != null) branchlp.Log.AppendLine("Remove right padding for " + effect + " at " + seg + ", drop right full link there " + cooldown);
                    DisableCooldown(branchlp, cooldown, seg + 1);
                    branchlp.ForceRecalculation(true);
                    HeapPush(branchlp);
                }
            }
            // increasing left link: for each effect not in left link force there to be some left link+effect items on both sides
            if (seg + 1 < segmentList.Count)
            {
                for (int cool = 0; cool < cooldownCount; cool++)
                {
                    Cooldown cooldown = (Cooldown)(1 << cool);
                    if (cooldown != effect && (cooldown & availableCooldownMask & ~act.RightLink) != 0)
                    {
                        // these are expensive branches, hopefully they die out quick
                        branchlp = lp.Clone();
                        if (branchlp.Log != null) branchlp.Log.AppendLine("Remove right padding for " + effect + " at " + seg + ", increase right link " + cooldown);
                        int row = AddConstraint(branchlp, cooldown, seg + 1);
                        branchlp.SetConstraintRHS(row, segmentList[seg + 1].Duration);
                        branchlp.SetConstraintLHS(row, 0.1);
                        row = AddConstraint(branchlp, effect | cooldown, seg);
                        branchlp.SetConstraintRHS(row, segmentList[seg].Duration);
                        branchlp.SetConstraintLHS(row, 0.1);
                        branchlp.ForceRecalculation(true);
                        HeapPush(branchlp);
                    }
                }
            }
        }

        private void EnforceEffectCooldown(ActivationConstraints act1, ActivationConstraints act2, Cooldown effect, double effectCooldown, double effectDuration)
        {
            const double eps = 0.000001;

            // if we have no padding or if both are fixed use the simpler version
            if ((act1.LeftPaddding < eps || !act1.Loose) && (act2.RightPadding < eps || !act2.Loose))
            {
                EnforceEffectCooldown(act1.MinTime, effect, effectCooldown, effectDuration, false);
                return;
            }

            // FullLeft  XX  XXX
            //
            // Left      XX
            // Joint     XXXX
            // Right     X  X
            //
            // FullRight X  X   XX

            // we have to enumerate all possibilities factoring around consecutive activations act1 and act2
            // possibilities include all options with there actually being activations at act1 and act2
            // if there is no activation at one of them it can be because there is effect in one segment to left
            // or because there is no effect in that segment

            int seg1 = act1.Segment;
            int seg2 = act2.Segment;
            SolverLP branchlp;
            // we have activations at seg1 and seg2
            // what is included in the padding strongly depends on link
            // therefore we have to branch on possibilities that the link changes
            // item is included for left padding if it doesn't include all right link
            // or if it includes more than left link from full left link
            // we can always add additional restrictions if needed, we just have to make sure
            // that we don't overrestrict
            // constraint will be of form
            // t1 = seg1start + leftPadding
            // t2 = seg2end - count[seg2] - rightPadding
            // count[seg2] + rightPadding + leftPadding <= seg2end - seg1start -c
            // it basically says that things that we think are in the padding there can't be too much of them
            // so if changes to links make new things to appear in the padding there won't be any problems with this constraint
            // what we have to worry is if something is no longer a padding, we need to branch on possibilities
            // that remove something from padding
            // so looking at the case of left padding, it doesn't depend on full right link
            // if right link increases it is still a padding
            // if right link decreases we have to branch on it
            // if left link decreases or if full left link increases it is still a padding
            // if left link increases or if full left link decreases we have to branch on it
            // so the actual branches are:
            // dropping right link: remove joint items with effect in seg1 or remove items in seg1+1 for each effect from right link
            // dropping left full link: remove items in seg1 or in seg1-1 for all effects from full left link that are not in left link
            // increasing left link: for each effect not in left link force there to be some left link+effect items on both sides
            // analogous for right padding
            // THERE IS A WHOLE TON OF BRANCHES TO DO, SO MAKE SURE NOT TO CALL THIS IF YOU DON'T HAVE TO
            // THIS IS SUPPOSED TO HANDLE ONLY THE MOST OBSCURE PROBLEMS
            // taking the lessons from above, we should only include paddings for things things that actually appear as
            // padding in the current solution, this way we only have to care for branches that would make those
            // not in the padding
            // special case is if there is either no left padding or right padding
            // treat it as special case to avoid creating too many branches, we already special case if there is no padding
            if (act1.LeftPaddding < eps || !act1.Loose)
            {
                // only right padding
                BranchOnRightPadding(act2, effect);
                // either act1 is loose with no padding
                // t1 = seg1start
                // t2 = seg2end - count[seg2] - rightPadding
                // count[seg2] + rightPadding <= seg2end - seg1start -c
                branchlp = lp.Clone();
                if (branchlp.Log != null) branchlp.Log.AppendLine("Restrict consecutive activation of " + effect + " at " + seg1 + " and " + seg2 + ", act1 loose with no padding");
                DisableCooldown(branchlp, effect, seg1 + 1, seg2 - 1);
                int row = branchlp.AddConstraint();
                SetCooldownElements(branchlp, row, effect, seg2, 1.0);
                SetRightPaddingElements(branchlp, row, act2, effect, 1.0);
                branchlp.SetConstraintRHS(row, segmentList[seg2].TimeEnd - segmentList[seg1].TimeStart - effectCooldown);
                row = AddConstraint(branchlp, effect, seg1);
                branchlp.SetConstraintRHS(row, segmentList[seg1].Duration);
                branchlp.SetConstraintLHS(row, 0.1);
                row = AddConstraint(branchlp, effect, seg2);
                branchlp.SetConstraintRHS(row, segmentList[seg2].Duration);
                branchlp.SetConstraintLHS(row, 0.1);
                branchlp.ForceRecalculation(true);
                HeapPush(branchlp);
                // or act1 is fixed
                // t1 = seg1end - count[seg1]
                // t2 = seg2end - count[seg2] - rightPadding
                // count[seg2] - count[seg1] + rightPadding <= seg2end - seg1end -c
                branchlp = lp.Clone();
                if (branchlp.Log != null) branchlp.Log.AppendLine("Restrict consecutive activation of " + effect + " at " + seg1 + " and " + seg2 + ", act1 fixed");
                DisableCooldown(branchlp, effect, segmentList.FindIndex(segment => segment.TimeStart >= segmentList[seg1].TimeEnd + effectDuration - eps), seg2 - 1);
                row = branchlp.AddConstraint();
                SetCooldownElements(branchlp, row, effect, seg2, 1.0);
                SetCooldownElements(branchlp, row, effect, seg1, -1.0);
                SetRightPaddingElements(branchlp, row, act2, effect, 1.0);
                branchlp.SetConstraintRHS(row, segmentList[seg2].TimeEnd - segmentList[seg1].TimeEnd - effectCooldown);
                row = AddConstraint(branchlp, effect, seg1);
                branchlp.SetConstraintRHS(row, segmentList[seg1].Duration);
                branchlp.SetConstraintLHS(row, 0.1);
                row = AddConstraint(branchlp, effect, seg2);
                branchlp.SetConstraintRHS(row, segmentList[seg2].Duration);
                branchlp.SetConstraintLHS(row, 0.1);
                branchlp.ForceRecalculation(true);
                HeapPush(branchlp);
            }
            else if (act2.RightPadding < eps || !act2.Loose)
            {
                // only left padding
                BranchOnLeftPadding(act1, effect);
                // t1 = seg1start + leftPadding
                // t2 = seg2end - count[seg2]
                // count[seg2] + leftPadding <= seg2end - seg1start -c
                branchlp = lp.Clone();
                if (branchlp.Log != null) branchlp.Log.AppendLine("Restrict consecutive activation of " + effect + " at " + seg1 + " and " + seg2 + ", only left padding");
                DisableCooldown(branchlp, effect, segmentList.FindIndex(segment => segment.TimeStart >= segmentList[seg1].TimeEnd + effectDuration - eps), seg2 - 1);
                int row = branchlp.AddConstraint();
                SetCooldownElements(branchlp, row, effect, seg2, 1.0);
                SetLeftPaddingElements(branchlp, row, act1, effect, 1.0);
                branchlp.SetConstraintRHS(row, segmentList[seg2].TimeEnd - segmentList[seg1].TimeStart - effectCooldown);
                row = AddConstraint(branchlp, effect, seg1);
                branchlp.SetConstraintRHS(row, segmentList[seg1].Duration);
                branchlp.SetConstraintLHS(row, 0.1);
                row = AddConstraint(branchlp, effect, seg2);
                branchlp.SetConstraintRHS(row, segmentList[seg2].Duration);
                branchlp.SetConstraintLHS(row, 0.1);
                branchlp.ForceRecalculation(true);
                HeapPush(branchlp);
            }
            else
            {
                // both paddings
                BranchOnLeftPadding(act1, effect);
                BranchOnRightPadding(act2, effect);
                // t1 = seg1start + leftPadding
                // t2 = seg2end - count[seg2] - rightPadding
                // count[seg2] + rightPadding + leftPadding <= seg2end - seg1start -c
                branchlp = lp.Clone();
                if (branchlp.Log != null) branchlp.Log.AppendLine("Restrict consecutive activation of " + effect + " at " + seg1 + " and " + seg2 + ", both paddings");
                DisableCooldown(branchlp, effect, segmentList.FindIndex(segment => segment.TimeStart >= segmentList[seg1].TimeEnd + effectDuration - eps), seg2 - 1);
                int row = branchlp.AddConstraint();
                SetCooldownElements(branchlp, row, effect, seg2, 1.0);
                SetRightPaddingElements(branchlp, row, act2, effect, 1.0);
                SetLeftPaddingElements(branchlp, row, act1, effect, 1.0);
                branchlp.SetConstraintRHS(row, segmentList[seg2].TimeEnd - segmentList[seg1].TimeStart - effectCooldown);
                row = AddConstraint(branchlp, effect, seg1);
                branchlp.SetConstraintRHS(row, segmentList[seg1].Duration);
                branchlp.SetConstraintLHS(row, 0.1);
                row = AddConstraint(branchlp, effect, seg2);
                branchlp.SetConstraintRHS(row, segmentList[seg2].Duration);
                branchlp.SetConstraintLHS(row, 0.1);
                branchlp.ForceRecalculation(true);
                HeapPush(branchlp);
            }
            // there is effect before seg1
            if (seg1 >= 1)
            {
                branchlp = lp.Clone();
                if (branchlp.Log != null) branchlp.Log.AppendLine("Enforce cooldown after " + effect + " at " + seg1 + " and " + seg2 + ", activation before");
                int row = AddConstraint(branchlp, effect, seg1 - 1);
                branchlp.SetConstraintRHS(row, segmentList[seg1 - 1].Duration);
                branchlp.SetConstraintLHS(row, 0.1);
                branchlp.ForceRecalculation(true);
                HeapPush(branchlp);
            }
            // or there is no effect in seg1
            branchlp = lp.Clone();
            if (branchlp.Log != null) branchlp.Log.AppendLine("Enforce cooldown after " + effect + " at " + seg1 + " and " + seg2 + ", remove effect");
            DisableCooldown(branchlp, effect, seg1);
            HeapPush(branchlp);
            // there is effect before seg2
            if (seg2 >= 1)
            {
                branchlp = lp.Clone();
                if (branchlp.Log != null) branchlp.Log.AppendLine("Enforce cooldown after " + effect + " at " + seg1 + " and " + seg2 + ", activation2 before");
                int row = AddConstraint(branchlp, effect, seg2 - 1);
                branchlp.SetConstraintRHS(row, segmentList[seg2 - 1].Duration);
                branchlp.SetConstraintLHS(row, 0.1);
                branchlp.ForceRecalculation(true);
                HeapPush(branchlp);
            }
            // or there is no effect in seg2
            branchlp = lp.Clone();
            if (branchlp.Log != null) branchlp.Log.AppendLine("Enforce cooldown after " + effect + " at " + seg1 + " and " + seg2 + ", remove effect2");
            DisableCooldown(branchlp, effect, seg2);
            HeapPush(branchlp);
        }

        private void EnforceEffectCooldown(double firstEffectActivation, Cooldown effect, double effectCooldown, double effectDuration, bool needsToCleanCloseActivations)
        {
            const double eps = 0.000001;
            int seg1 = segmentList.FindIndex(segment => segment.TimeEnd > firstEffectActivation + eps); //(int)((firstEffectActivation + eps) / segmentDuration);
            SolverLP branchlp;
            // if we have effect in seg1 then either
            // there is activation in seg1
            RestrictConsecutiveActivation(effect, effectCooldown, effectDuration, seg1, needsToCleanCloseActivations);
            // there is effect before seg1
            if (seg1 >= 1)
            {
                branchlp = lp.Clone();
                if (branchlp.Log != null) branchlp.Log.AppendLine("Enforce cooldown after " + effect + " at " + seg1 + ", activation before");
                int row = AddConstraint(branchlp, effect, seg1 - 1);
                branchlp.SetConstraintRHS(row, segmentList[seg1 - 1].Duration);
                branchlp.SetConstraintLHS(row, 0.1);
                branchlp.ForceRecalculation(true);
                HeapPush(branchlp);
            }
            // or there is no effect in seg1
            branchlp = lp.Clone();
            if (branchlp.Log != null) branchlp.Log.AppendLine("Enforce cooldown after " + effect + " at " + seg1 + ", remove effect");
            DisableCooldown(branchlp, effect, seg1);
            HeapPush(branchlp);
        }

        private bool SegmentContainsEffect(int segment, Cooldown effect)
        {
            int hex = (int)effect;
            foreach (int h in hexList[segment])
            {
                if ((hex & h) == hex)
                {
                    return true;
                }
            }
            return false;
        }

        private bool SegmentContainsVariable(int segment, VariableType variable)
        {
            const double eps = 0.000001;
            for (int index = 0; index < solutionVariable.Count; index++)
            {
                if (solutionVariable[index].Segment == segment && solutionVariable[index].Type == variable)
                {
                    if (solution[index] > eps) return true;
                }
            }
            return false;
        }

        private bool SegmentContainsCooldown(Cooldown cooldown, VariableType cooldownType, int minSegment, int maxSegment)
        {
            const double eps = 0.000001;
            double[] segCount = GetSegmentCooldownCount(cooldown, cooldownType);
            for (int seg = 0; seg < segmentList.Count; seg++)
            {
                if (segCount[seg] > eps && seg >= minSegment && seg <= maxSegment) return true;
            }
            return false;
        }


        private void RestrictConsecutiveActivation(Cooldown effect, double effectCooldown, double effectDuration, int firstActivationSegment, bool needsToCleanCloseActivations)
        {
            const double eps = 0.000001;
            int seg2 = segmentList.FindIndex(segment => segment.TimeEnd > segmentList[firstActivationSegment].TimeStart + effectCooldown + eps); //firstActivationSegment + (int)((effectCooldown + eps) / segmentDuration);
            SolverLP branchlp;
            int row;
            if (seg2 == -1)
            {
                // in this case the only thing we can enforce is that there are no subsequent activations
                branchlp = lp.Clone();
                if (branchlp.Log != null) branchlp.Log.AppendLine("Restrict all activation of " + effect + " after " + firstActivationSegment);
                DisableCooldown(branchlp, effect, segmentList.FindIndex(segment => segment.TimeStart >= segmentList[firstActivationSegment].TimeEnd + effectDuration - eps), segmentList.Count - 1);
                HeapPush(branchlp);
                return;
            }
            if (-effectDuration <= segmentList[seg2].TimeEnd - segmentList[firstActivationSegment].TimeEnd - effectCooldown)
            {
                branchlp = lp.Clone();
                if (branchlp.Log != null) branchlp.Log.AppendLine("Restrict consecutive activation of " + effect + " at " + firstActivationSegment);
                // first make sure that second activation is not too close
                if (needsToCleanCloseActivations)
                {
                    row = AddConstraint(branchlp, effect, firstActivationSegment, seg2 - 1);
                    branchlp.SetConstraintRHS(row, effectDuration);
                }
                // make sure there is enough distance between the activations
                // t1 = (seg1 + 1) * segmentDuration - count[seg1]
                // t2 = (seg2 + 1) * segmentDuration - count[seg2]
                // this will potentially overrestrict for IV because effect duration is less than segment duration
                // in that case make another case for t1 = seg1 * segmentDuration, but must make sure that seg1 + 1 does not have IV
                // count[seg2] - count[seg1] <= (seg2 - seg1) * segmentDuration - c
                row = branchlp.AddConstraint();
                SetCooldownElements(branchlp, row, effect, firstActivationSegment, -1.0);
                SetCooldownElements(branchlp, row, effect, seg2, 1.0);
                branchlp.SetConstraintRHS(row, segmentList[seg2].TimeEnd - segmentList[firstActivationSegment].TimeEnd - effectCooldown);
                // make sure it actually starts in this segment
                // this is kind of a cheat, but I can't think of any better way to enforce this
                row = AddConstraint(branchlp, effect, firstActivationSegment);
                branchlp.SetConstraintRHS(row, segmentList[firstActivationSegment].Duration);
                branchlp.SetConstraintLHS(row, 0.1);
                // TODO what if the first activation comes after coldsnap and we have some leftover effect in first segment
                branchlp.ForceRecalculation(true);
                HeapPush(branchlp);
            }
            // there is another case if cooldown duration is funny (i.e. IV)
            // this case has a minimum difference of (int)(effectCooldown / segmentDuration) * segmentDuration
            // t1 = (seg1 + 1) * segmentDuration - count[seg1]
            // t2 = (seg2 + 2) * segmentDuration - count[seg2]
            if (seg2 + 1 < segmentList.Count)
            {
                branchlp = lp.Clone();
                if (branchlp.Log != null) branchlp.Log.AppendLine("Restrict consecutive activation of " + effect + " at " + firstActivationSegment + ", long distance");
                if (needsToCleanCloseActivations)
                {
                    row = AddConstraint(branchlp, effect, firstActivationSegment, seg2);
                    branchlp.SetConstraintRHS(row, effectDuration);
                }
                DisableCooldown(branchlp, effect, seg2); // have to disable it in seg2, normal constraints don't enforce this
                row = branchlp.AddConstraint();
                SetCooldownElements(branchlp, row, effect, firstActivationSegment, -1.0);
                SetCooldownElements(branchlp, row, effect, seg2 + 1, 1.0);
                branchlp.SetConstraintRHS(row, segmentList[seg2 + 1].TimeEnd - segmentList[firstActivationSegment].TimeEnd - effectCooldown);
                row = AddConstraint(branchlp, effect, firstActivationSegment);
                branchlp.SetConstraintRHS(row, segmentList[firstActivationSegment].Duration);
                branchlp.SetConstraintLHS(row, 0.1);
                branchlp.ForceRecalculation(true);
                HeapPush(branchlp);
            }
            if (effectDuration < segmentList[firstActivationSegment].Duration - eps)
            {
                // the effect has some room for movement, but we have to be careful because it can't be moved
                // to the very beginning of segment in all cases
                // for example if the effect is IV and it happens together with WE and there is a WE summon in the same 
                // segment and we don't have coldsnap, then we know the summon must happen before IV, so the actual start is a GCD after segment start
                double buffer = 0.0;
                if (effect == Cooldown.IcyVeins)
                {
                    if (SegmentContainsEffect(firstActivationSegment, Cooldown.IcyVeins | Cooldown.WaterElemental))
                    {
                        if (SegmentContainsVariable(firstActivationSegment, VariableType.SummonWaterElemental))
                        {
                            buffer = calculationResult.BaseState.GlobalCooldown;
                            // it is possible to force effect to the start of segment if we eliminate summoning
                            branchlp = lp.Clone();
                            if (branchlp.Log != null) branchlp.Log.AppendLine("Restrict consecutive activation of " + effect + " at " + firstActivationSegment + ", force to start of segment by eliminating summoning");
                            // first make sure that second activation is not too close                
                            DisableCooldown(branchlp, effect, firstActivationSegment + 1, seg2 - 1);
                            DisableVariable(branchlp, VariableType.SummonWaterElemental, firstActivationSegment);
                            // make sure there is enough distance between the activations
                            // t1 = seg1 * segmentDuration
                            // t2 = (seg2 + 1) * segmentDuration - count[seg2]
                            // count[seg2] <= (seg2 - seg1 + 1) * segmentDuration - c
                            row = AddConstraint(branchlp, effect, seg2);
                            branchlp.SetConstraintRHS(row, segmentList[seg2].TimeEnd - segmentList[firstActivationSegment].TimeStart - effectCooldown);
                            // make sure it actually starts in this segment
                            // count[seg1] + count[seg1 + 1] >= min(segmentDuration, effectDuration)
                            row = AddConstraint(branchlp, effect, firstActivationSegment);
                            branchlp.SetConstraintRHS(row, segmentList[firstActivationSegment].Duration);
                            branchlp.SetConstraintLHS(row, 0.1);
                            // TODO what if the first activation comes after coldsnap and we have some leftover effect in first segment
                            branchlp.ForceRecalculation(true);
                            HeapPush(branchlp);
                        }
                    }
                }
                branchlp = lp.Clone();
                if (branchlp.Log != null) branchlp.Log.AppendLine("Restrict consecutive activation of " + effect + " at " + firstActivationSegment + ", version for short effects");
                // first make sure that second activation is not too close
                DisableCooldown(branchlp, effect, firstActivationSegment + 1, seg2 - 1);
                // make sure there is enough distance between the activations
                // t1 = seg1 * segmentDuration
                // t2 = (seg2 + 1) * segmentDuration - count[seg2]
                // count[seg2] <= (seg2 - seg1 + 1) * segmentDuration - c
                row = AddConstraint(branchlp, effect, seg2);
                branchlp.SetConstraintRHS(row, segmentList[seg2].TimeEnd - segmentList[firstActivationSegment].TimeStart - effectCooldown - buffer);
                // make sure it actually starts in this segment
                // count[seg1] + count[seg1 + 1] >= min(segmentDuration, effectDuration)
                row = AddConstraint(branchlp, effect, firstActivationSegment);
                branchlp.SetConstraintRHS(row, segmentList[firstActivationSegment].Duration);
                branchlp.SetConstraintLHS(row, 0.1);
                // TODO what if the first activation comes after coldsnap and we have some leftover effect in first segment
                branchlp.ForceRecalculation(true);
                HeapPush(branchlp);
            }
        }

        private void SetCooldownElements(SolverLP branchlp, int row, Cooldown cooldown, int segment, double value)
        {
            SetCooldownElements(branchlp, row, cooldown, segment, segment, value);
        }

        private void SetCooldownElements(SolverLP branchlp, int row, Cooldown cooldown, int minSegment, int maxSegment, double value)
        {
            for (int index = 0; index < solutionVariable.Count; index++)
            {
                CastingState state = solutionVariable[index].State;
                if (state != null && state.GetCooldown(cooldown))
                {
                    int seg = solutionVariable[index].Segment;
                    if (seg >= minSegment && seg <= maxSegment) branchlp.SetConstraintElement(row, index, value);
                }
            }
        }

        private void SetCooldownElements(SolverLP branchlp, int row, Cooldown cooldown, VariableType cooldownType, int minSegment, int maxSegment, double value)
        {
            for (int index = 0; index < solutionVariable.Count; index++)
            {
                int seg = solutionVariable[index].Segment;
                if (seg >= minSegment && seg <= maxSegment && solutionVariable[index].IsMatch(cooldown, cooldownType)) branchlp.SetConstraintElement(row, index, value);
            }
        }

        private void SetVariableElements(SolverLP branchlp, int row, VariableType variable, int segment, double value)
        {
            SetVariableElements(branchlp, row, variable, segment, segment, value);
        }

        private void SetVariableElements(SolverLP branchlp, int row, VariableType variable, int minSegment, int maxSegment, double value)
        {
            for (int index = 0; index < solutionVariable.Count; index++)
            {
                if (solutionVariable[index].Type == variable)
                {
                    int seg = solutionVariable[index].Segment;
                    if (seg >= minSegment && seg <= maxSegment) branchlp.SetConstraintElement(row, index, value);
                }
            }
        }

        private void SetLeftPaddingElements(SolverLP branchlp, int row, ActivationConstraints activation, Cooldown effect, double value)
        {
            for (int index = 0; index < solutionVariable.Count; index++)
            {
                CastingState state = solutionVariable[index].State;
                if (state != null && solutionVariable[index].Segment == activation.Segment && !solutionVariable[index].IsZeroTime)
                {
                    Cooldown c = state.Cooldown;
                    // item is included for left padding if it doesn't include all right link
                    // or if it includes more than left link from full left link
                    if ((c & effect) == 0 && ((c & activation.RightLink) != activation.RightLink || ((c & activation.LeftLink) == activation.LeftLink && (c & activation.FullLeftLink) != activation.LeftLink)))
                    {
                        branchlp.SetConstraintElement(row, index, value);
                    }
                }
            }
        }

        private void SetRightPaddingElements(SolverLP branchlp, int row, ActivationConstraints activation, Cooldown effect, double value)
        {
            for (int index = 0; index < solutionVariable.Count; index++)
            {
                CastingState state = solutionVariable[index].State;
                if (state != null && solutionVariable[index].Segment == activation.Segment && !solutionVariable[index].IsZeroTime)
                {
                    Cooldown c = state.Cooldown;
                    // item is included for left padding if it doesn't include all right link
                    // or if it includes more than left link from full left link
                    if ((c & effect) == 0 && ((c & activation.LeftLink) != activation.LeftLink || ((c & activation.RightLink) == activation.RightLink && (c & activation.FullRightLink) != activation.RightLink)))
                    {
                        branchlp.SetConstraintElement(row, index, value);
                    }
                }
            }
        }

        private void DisableVariable(SolverLP branchlp, VariableType variable, int segment)
        {
            DisableVariable(branchlp, variable, segment, segment);
        }

        private void DisableCooldown(SolverLP branchlp, Cooldown cooldown, int segment)
        {
            DisableCooldown(branchlp, cooldown, segment, segment);
        }

        private void DisableVariable(SolverLP branchlp, VariableType variable, int minSegment, int maxSegment)
        {
            for (int index = 0; index < solutionVariable.Count; index++)
            {
                if (solutionVariable[index].Type == variable)
                {
                    int seg = solutionVariable[index].Segment;
                    if (seg >= minSegment && seg <= maxSegment) branchlp.EraseColumn(index);
                }
            }
        }

        private void DisableCooldown(SolverLP branchlp, Cooldown cooldown, int minSegment, int maxSegment)
        {
            for (int index = 0; index < solutionVariable.Count; index++)
            {
                CastingState state = solutionVariable[index].State;
                if (state != null && state.GetCooldown(cooldown))
                {
                    int seg = solutionVariable[index].Segment;
                    if (seg >= minSegment && seg <= maxSegment) branchlp.EraseColumn(index);
                }
            }
        }

        private void DisableCooldown(SolverLP branchlp, Cooldown cooldown, VariableType cooldownType, int minSegment, int maxSegment)
        {
            for (int index = 0; index < solutionVariable.Count; index++)
            {
                if (solutionVariable[index].IsMatch(cooldown, cooldownType))
                {
                    int seg = solutionVariable[index].Segment;
                    if (seg >= minSegment && seg <= maxSegment) branchlp.EraseColumn(index);
                }
            }
        }

        private void DisableCooldown(SolverLP branchlp, Cooldown cooldown, VariableType cooldownType, Predicate<int> includeSegment)
        {
            for (int index = 0; index < solutionVariable.Count; index++)
            {
                if (solutionVariable[index].IsMatch(cooldown, cooldownType))
                {
                    int seg = solutionVariable[index].Segment;
                    if (includeSegment(seg)) branchlp.EraseColumn(index);
                }
            }
        }

        private bool ValidateCooldown(Cooldown cooldown, double effectDuration, double cooldownDuration)
        {
            return ValidateCooldown(cooldown, effectDuration, cooldownDuration, false, effectDuration, null, VariableType.None);
        }

        private bool ValidateWaterElementalSummon()
        {
            Cooldown cooldown = Cooldown.WaterElemental;
            double effectDuration = calculationResult.WaterElementalDuration;
            double cooldownDuration = calculationResult.WaterElementalCooldown;
            VariableType activation = VariableType.SummonWaterElemental;

            const double eps = 0.00001;
            double[] segCount = GetSegmentCooldownCount(cooldown, VariableType.None);
            double[] segActivation = GetSegmentCooldownCount(Cooldown.None, activation);

            //int mindist = (int)Math.Ceiling(effectDuration / segmentDuration);
            //int mindist2 = (int)Math.Floor(effectDuration / segmentDuration);
            //int maxdist = (cooldownDuration < 0) ? 3 * segments : ((int)Math.Floor((cooldownDuration - effectDuration) / segmentDuration));
            //int maxdist2 = (cooldownDuration < 0) ? 3 * segments : ((int)Math.Floor(cooldownDuration / segmentDuration));

            bool valid = true;

            // special summon validation with coldsnap, without coldsnap use ValidateActivation

            return valid;
        }

        /// <summary>
        /// Determines if effect in two segments could potentially belong to the same effect activation.
        /// </summary>
        /// <param name="seg1"></param>
        /// <param name="seg2"></param>
        /// <param name="effectDuration"></param>
        /// <returns></returns>
        private bool InActivationDistance(int seg1, int seg2, double effectDuration)
        {
            if (seg1 > seg2)
            {
                int tmp = seg1;
                seg1 = seg2;
                seg2 = tmp;
            }
            return segmentList[seg2].TimeStart - segmentList[seg1].TimeEnd < effectDuration - 0.00001;
        }

        private bool InActivationDistance(int seg1min, int seg1max, int seg2, double effectDuration)
        {
            if (seg2 > seg1max)
            {
                return segmentList[seg2].TimeStart - segmentList[seg1max].TimeEnd < effectDuration - 0.00001;
            }
            else if (seg2 < seg1min)
            {
                return segmentList[seg1min].TimeStart - segmentList[seg2].TimeEnd < effectDuration - 0.00001;
            }
            return true;
        }

        /// <summary>
        /// Determines if effect in two segments must necessarily belong to the same cooldown
        /// </summary>
        /// <param name="seg1"></param>
        /// <param name="seg2"></param>
        /// <param name="cooldownDuration"></param>
        /// <returns></returns>
        private bool InCooldownDistance(int seg1, int seg2, double effectDuration, double cooldownDuration)
        {
            if (seg1 > seg2)
            {
                int tmp = seg1;
                seg1 = seg2;
                seg2 = tmp;
            }
            return segmentList[seg2].TimeEnd - Math.Max(0.0, segmentList[seg1].TimeStart - effectDuration) <= cooldownDuration + 0.00001;
        }

        private bool InCooldownDistance(int seg1min, int seg1max, int seg2, double effectDuration, double cooldownDuration)
        {
            if (seg2 > seg1max)
            {
                return segmentList[seg2].TimeEnd - Math.Max(0.0, segmentList[seg1min].TimeStart - effectDuration) <= cooldownDuration + 0.00001;
            }
            else if (seg2 < seg1min)
            {
                return segmentList[seg1max].TimeEnd - Math.Max(0.0, segmentList[seg2].TimeStart - effectDuration) <= cooldownDuration + 0.00001;
            }
            return true;
        }

        private bool ValidateActivation(Cooldown cooldown, VariableType cooldownType, double effectDuration, double cooldownDuration, VariableType activation, Cooldown activationCooldown)
        {
            const double eps = 0.00001;
            double[] segCount = GetSegmentCooldownCount(cooldown, cooldownType);
            double[] segActivation = GetSegmentCooldownCount(activationCooldown, activation);

            //int mindist = (int)Math.Ceiling(effectDuration / segmentDuration);
            //int mindist2 = (int)Math.Floor(effectDuration / segmentDuration);
            //int maxdist = (cooldownDuration < 0) ? 3 * segments : ((int)Math.Floor((cooldownDuration - effectDuration) / segmentDuration));
            //int maxdist2 = (cooldownDuration < 0) ? 3 * segments : ((int)Math.Floor(cooldownDuration / segmentDuration));

            bool valid = true;

            for (int seg = 0; seg < segmentList.Count; seg++)
            {
                int segmin = resolution[seg].MinSegment;
                int segmax = resolution[seg].MaxSegment;
                if (segActivation[seg] > eps)
                {
                    for (int s = 0; s < segmentList.Count; s++)
                    {
                        if (InCooldownDistance(segmin, segmax, s, effectDuration, cooldownDuration) && s < segmin && segCount[s] > eps) valid = false; 
                    }
                    if (!valid)
                    {
                        // can't have effect before activation
                        // either activation is not here or effect is not before it
                        SolverLP cooldownUsed = lp.Clone();
                        // activation not used
                        if (lp.Log != null) lp.Log.AppendLine("Disable activation of " + activation.ToString() + " at " + segmin + "-" + segmax);
                        DisableCooldown(lp, activationCooldown, activation, segmin, segmax);
                        HeapPush(lp);
                        if (cooldownUsed.Log != null) cooldownUsed.Log.AppendLine("No " + cooldown.ToString() + " before activation at " + segmin);
                        DisableCooldown(cooldownUsed, cooldown, cooldownType, segmentList.FindIndex(segment => InCooldownDistance(segmin, segmax, segment.Index, effectDuration, cooldownDuration)), segmin - 1);
                        HeapPush(cooldownUsed);
                        return false;
                    }
                }
            }

            // each effect needs activation
            for (int seg = 0; seg < segmentList.Count; seg++)
            {
                int segmin = resolution[seg].MinSegment;
                int segmax = resolution[seg].MaxSegment;
                int prevsegmin = 0;
                int prevsegmax = 0;
                if (segmin > 0)
                {
                    prevsegmin = resolution[segmin - 1].MinSegment;
                    prevsegmax = resolution[segmin - 1].MaxSegment;
                }
                if (segCount[seg] > eps && (segmin == 0 || !SegmentContainsCooldown(cooldown, cooldownType, prevsegmin, prevsegmax)))
                {
                    if (!SegmentContainsCooldown(activationCooldown, activation, prevsegmin, segmax))
                    {
                        // either there is no effect or there is activation
                        SolverLP cooldownUsed = lp.Clone();
                        // force activation
                        if (lp.Log != null) lp.Log.AppendLine("Force activation of " + activation + " at " + prevsegmin + "-" + segmax);
                        int row = AddConstraint(lp, activationCooldown, activation, prevsegmin, segmax);
                        lp.SetConstraintRHS(row, segmentList[segmax].TimeEnd - segmentList[prevsegmin].TimeStart);
                        lp.SetConstraintLHS(row, 0.1);
                        lp.ForceRecalculation(true);
                        HeapPush(lp);
                        // no effect
                        if (cooldownUsed.Log != null) cooldownUsed.Log.AppendLine("No " + cooldown + "-" + cooldownType + " without activation at " + segmin + "-" + segmax);
                        DisableCooldown(cooldownUsed, cooldown, cooldownType, segmin, segmax);
                        HeapPush(cooldownUsed);
                        return false;
                    }
                }
            }

            return true;
        }

        private bool ValidateActivationAdvanced(Cooldown cooldown, VariableType cooldownType, double effectDuration, double cooldownDuration, VariableType activation, Cooldown activationCooldown)
        {
            const double eps = 0.00001;
            double[] segCount = GetSegmentCooldownCount(cooldown, cooldownType);
            double[] segActivation = GetSegmentCooldownCount(activationCooldown, activation);

            //int mindist = (int)Math.Ceiling(effectDuration / segmentDuration);
            //int mindist2 = (int)Math.Floor(effectDuration / segmentDuration);
            //int maxdist = (cooldownDuration < 0) ? 3 * segments : ((int)Math.Floor((cooldownDuration - effectDuration) / segmentDuration));
            //int maxdist2 = (cooldownDuration < 0) ? 3 * segments : ((int)Math.Floor(cooldownDuration / segmentDuration));

            for (int i = 0; i < segmentColumn[0]; i++) // fix if variable ordering changes
            {
                if (solutionVariable[i].IsMatch(activationCooldown, activation) && solution[i] > eps)
                {
                    int seg = solutionVariable[i].Segment;
                    int seghex = (int)solutionVariable[i].State.Cooldown;
                    // check all cooldowns that link to this drums activation
                    // if any of them is also before this segment we have a cycle if it's not present at activation
                    int linkedHex = 0;
                    for (int s = 0; s < segmentList.Count; s++)
                    {
                        if (InActivationDistance(s, seg, effectDuration)/*Math.Abs(seg - s) <= mindist*/)
                        {
                            for (int index = segmentColumn[s]; index < segmentColumn[s + 1]; index++)
                            {
                                CastingState state = solutionVariable[index].State;
                                if (solutionVariable[index].IsMatch(cooldown, cooldownType) && solution[index] > eps)
                                {
                                    linkedHex |= (int)state.Cooldown;
                                }
                            }
                        }
                    }
                    for (int index = 0; index < segmentColumn[0]; index++) // fix if variable ordering changes
                    {
                        CastingState state = solutionVariable[index].State;
                        if (InActivationDistance(seg, solutionVariable[index].Segment, effectDuration)/*Math.Abs(seg - solutionVariable[index].Segment) <= mindist*/ && solutionVariable[index].IsMatch(cooldown, cooldownType) && solution[index] > eps)
                        {
                            linkedHex |= (int)state.Cooldown;
                        }
                    }

                    int brokenHex = 0;
                    for (int s = 0; s < segmentList.Count; s++)
                    {
                        if (s == seg - 1)
                        {
                            for (int index = segmentColumn[s]; index < segmentColumn[s + 1]; index++)
                            {
                                CastingState state = solutionVariable[index].State;
                                if (state != null && solution[index] > eps)
                                {
                                    int h = (int)state.Cooldown & linkedHex & ~seghex;
                                    if (h != 0) brokenHex = h;
                                }
                            }
                        }
                    }
                    for (int index = 0; index < segmentColumn[0]; index++) // fix if variable ordering changes
                    {
                        CastingState state = solutionVariable[index].State;
                        if (state != null && solution[index] > eps)
                        {
                            int outseg = solutionVariable[index].Segment;
                            if (outseg == seg - 1)
                            {
                                int h = (int)state.Cooldown & linkedHex & ~seghex;
                                if (h != 0) brokenHex = h;
                            }
                        }
                    }
                    if (brokenHex != 0)
                    {
                        // either we don't have activation that is without broken hex or drums casting with hex or hex before activation
                        SolverLP drumsnohex = lp.Clone();
                        if (drumsnohex.Log != null) drumsnohex.Log.AppendLine(cooldown.ToString() + " without hex");
                        for (int s = 0; s < segmentList.Count; s++)
                        {
                            if (InActivationDistance(seg, s, effectDuration)/*Math.Abs(seg - s) <= mindist*/)
                            {
                                for (int index = segmentColumn[s]; index < segmentColumn[s + 1]; index++)
                                {
                                    CastingState state = solutionVariable[index].State;
                                    if (state != null && solutionVariable[index].IsMatch(cooldown, cooldownType) && ((int)state.Cooldown & brokenHex) != 0) drumsnohex.EraseColumn(index);
                                }
                            }
                        }
                        HeapPush(drumsnohex);

                        SolverLP nohex = lp.Clone();
                        if (nohex.Log != null) nohex.Log.AppendLine("No hex");
                        for (int s = 0; s < segmentList.Count; s++)
                        {
                            if (s == seg - 1)
                            {
                                for (int index = segmentColumn[s]; index < segmentColumn[s + 1]; index++)
                                {
                                    CastingState state = solutionVariable[index].State;
                                    if (state != null && ((int)state.Cooldown & brokenHex) != 0) nohex.EraseColumn(index);
                                }
                            }
                        }
                        HeapPush(nohex);

                        if (lp.Log != null) lp.Log.AppendLine("Disable activation of " + activation.ToString() + " at " + seg + " without hex");
                        for (int index = 0; index < segmentColumn[0]; index++) // fix if variable ordering changes
                        {
                            if (solutionVariable[index].IsMatch(activationCooldown, activation) && solutionVariable[index].Segment == seg && ((int)solutionVariable[index].State.Cooldown & brokenHex) != brokenHex) lp.EraseColumn(index);
                        }
                        HeapPush(lp);
                        return false;
                    }

                    // another class of activation cycles is if drums link into next segment, this
                    // causes drums to be full from activation to the end of segment so anything in this or previous
                    // segment that links to some of the drums has to cross the activation and so has to be present
                    // in the activation

                    // first check if drums are present in next segment                    
                    if (seg < segmentList.Count - 1)
                    {
                        bool cooldownInNext = false;
                        int cooldownHex = (int)activationCooldown;
                        for (int j = 0; j < hexList[seg + 1].Count; j++)
                        {
                            if ((hexList[seg + 1][j] & cooldownHex) != 0)
                            {
                                cooldownInNext = true;
                                break;
                            }
                        }
                        if (cooldownInNext)
                        {
                            // check that effect has activation cooldown if it extends into next segment
                            int extendedCooldown = (cooldownHex & hexMask[seg + 1]);
                            for (int j = 0; j < hexList[seg].Count; j++)
                            {
                                if ((hexList[seg][j] & (int)cooldown) == (int)cooldown && (hexList[seg][j] & extendedCooldown) != extendedCooldown)
                                {
                                    // we have a problem, extended cooldown is not present during effect even though it's present at activation and in next segment
                                    // identify which part of extended cooldown is responsible
                                    int h = hexList[seg][j];
                                    int ind = 1;
                                    while ((ind & ~h & extendedCooldown) == 0) ind <<= 1;
                                    // either all effect has this cooldown or this effect is not present at activation or it is not present in next segment
                                    SolverLP branchlp = lp.Clone();
                                    if (branchlp.Log != null) branchlp.Log.AppendLine("Disable " + cooldown + " without " + (Cooldown)ind + " at " + seg);
                                    for (int index = 0; index < solutionVariable.Count; index++)
                                    {
                                        CastingState state = solutionVariable[index].State;
                                        if (solutionVariable[index].Segment == seg)
                                        {
                                            if (state != null && state.GetCooldown(cooldown) && !state.GetCooldown((Cooldown)ind))
                                            {
                                                branchlp.EraseColumn(index);
                                            }
                                        }
                                    }
                                    HeapPush(branchlp);
                                    branchlp = lp.Clone();
                                    if (branchlp.Log != null) branchlp.Log.AppendLine("Disable " + (Cooldown)ind + " at activation of " + cooldown + " at " + seg);
                                    for (int index = 0; index < solutionVariable.Count; index++)
                                    {
                                        CastingState state = solutionVariable[index].State;
                                        if (solutionVariable[index].Segment == seg)
                                        {
                                            if (state != null && solutionVariable[index].IsMatch(activationCooldown, activation) && state.GetCooldown((Cooldown)ind))
                                            {
                                                branchlp.EraseColumn(index);
                                            }
                                        }
                                    }
                                    HeapPush(branchlp);
                                    branchlp = lp;
                                    DisableCooldown(branchlp, (Cooldown)ind, seg + 1);
                                    HeapPush(branchlp);
                                    return false;
                                }
                            }

                            // check what links to drums, we already have all that is active during drums in linkedHex
                            for (int ss = Math.Max(0, seg - 1); ss <= seg; ss++)
                            {
                                for (int j = 0; j < hexList[ss].Count; j++)
                                {
                                    int h = hexList[ss][j];
                                    if ((h & linkedHex) != 0 && (h & cooldownHex) == 0 && (h & linkedHex & ~seghex) != 0)
                                    {
                                        // we found an item that is before activation that has cooldown not present at activation
                                        // identify one such cooldown
                                        int ind = 1;
                                        while ((ind & h & linkedHex & ~seghex) == 0) ind <<= 1;
                                        // we branch
                                        // either activation has this cooldown, this cooldown without drums is not present in this or previous segment
                                        // or drums don't link to this cooldown, or drums don't extend to next segment

                                        // no cooldown without drums
                                        SolverLP drumsnohex = lp.Clone();
                                        if (drumsnohex.Log != null) drumsnohex.Log.AppendLine("Disable " + ind + " without drums before activation at " + seg);
                                        for (int s = 0; s < segmentList.Count; s++)
                                        {
                                            if (s >= seg - 1 && s <= seg)
                                            {
                                                for (int index = segmentColumn[s]; index < segmentColumn[s + 1]; index++)
                                                {
                                                    CastingState state = solutionVariable[index].State;
                                                    if (state != null && !solutionVariable[index].IsMatch(cooldown, cooldownType) && ((int)state.Cooldown & ind) != 0) drumsnohex.EraseColumn(index);
                                                }
                                            }
                                        }
                                        HeapPush(drumsnohex);

                                        // no cooldown with drums
                                        drumsnohex = lp.Clone();
                                        if (drumsnohex.Log != null) drumsnohex.Log.AppendLine("Disable " + ind + " with drums in " + seg);
                                        for (int s = 0; s < segmentList.Count; s++)
                                        {
                                            if (InActivationDistance(seg, s, effectDuration)/*Math.Abs(seg - s) <= mindist*/)
                                            {
                                                for (int index = segmentColumn[s]; index < segmentColumn[s + 1]; index++)
                                                {
                                                    CastingState state = solutionVariable[index].State;
                                                    if (state != null && solutionVariable[index].IsMatch(cooldown, cooldownType) && ((int)state.Cooldown & ind) != 0) drumsnohex.EraseColumn(index);
                                                }
                                            }
                                        }
                                        HeapPush(drumsnohex);

                                        // drums don't extend to next segment
                                        drumsnohex = lp.Clone();
                                        if (drumsnohex.Log != null) drumsnohex.Log.AppendLine("Disable drums in next segment " + (seg + 1));
                                        for (int s = 0; s < segmentList.Count; s++)
                                        {
                                            if (s == seg + 1)
                                            {
                                                for (int index = segmentColumn[s]; index < segmentColumn[s + 1]; index++)
                                                {
                                                    CastingState state = solutionVariable[index].State;
                                                    if (state != null && solutionVariable[index].IsMatch(cooldown, cooldownType)) drumsnohex.EraseColumn(index);
                                                }
                                            }
                                        }
                                        HeapPush(drumsnohex);

                                        // activation has this cooldown
                                        if (lp.Log != null) lp.Log.AppendLine("Disable activation of " + activation.ToString() + " at " + seg + " without hex");
                                        for (int index = 0; index < segmentColumn[0]; index++) // fix if variable ordering changes
                                        {
                                            if (solutionVariable[index].Type == activation && solutionVariable[index].State.GetCooldown(activationCooldown) && solutionVariable[index].Segment == seg && ((int)solutionVariable[index].State.Cooldown & ind) == 0) lp.EraseColumn(index);
                                        }
                                        HeapPush(lp);
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }

        /*private int GetSegmentedCooldownRow(Cooldown cooldown, int minSegment, int maxSegment)
        {
            if (minSegment < 0) minSegment = 0;
            if (maxSegment > segmentList.Count - 1) maxSegment = segmentList.Count - 1;
            switch (cooldown)
            {
                case Cooldown.ArcanePower:
                    for (int ss = 0; ss < segmentList.Count; ss++)
                    {
                        double cool = calculationResult.ArcanePowerCooldown;
                        int maxs = (int)Math.Floor(ss + cool / segmentDuration) - 1;
                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                        if (minSegment >= ss && maxSegment <= maxs) return rowSegmentArcanePower + ss;
                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                    return -1;
                case Cooldown.Combustion:
                    for (int ss = 0; ss < segments; ss++)
                    {
                        double cool = 180 + 15;
                        int maxs = (int)Math.Floor(ss + cool / segmentDuration) - 1;
                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                        if (minSegment >= ss && maxSegment <= maxs) return rowSegmentArcanePower + ss;
                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                    return -1;
                case Cooldown.DrumsOfBattle:
                    for (int ss = 0; ss < segments; ss++)
                    {
                        double cool = 120;
                        int maxs = (int)Math.Floor(ss + cool / segmentDuration) - 1;
                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                        if (minSegment >= ss && maxSegment <= maxs) return rowSegmentArcanePower + ss;
                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                    return -1;
                case Cooldown.FlameCap:
                    for (int ss = 0; ss < segments; ss++)
                    {
                        double cool = 180;
                        int maxs = (int)Math.Floor(ss + cool / segmentDuration) - 1;
                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                        if (minSegment >= ss && maxSegment <= maxs) return rowSegmentArcanePower + ss;
                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                    return -1;
                case Cooldown.IcyVeins:
                    for (int ss = 0; ss < segments; ss++)
                    {
                        double cool = calculationResult.IcyVeinsCooldown + (coldsnapAvailable ? 20 : 0);
                        int maxs = (int)Math.Floor(ss + cool / segmentDuration) - 1;
                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                        if (minSegment >= ss && maxSegment <= maxs) return rowSegmentArcanePower + ss;
                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                    return -1;
                case Cooldown.Trinket1:
                    for (int ss = 0; ss < segments; ss++)
                    {
                        double cool = trinket1Cooldown;
                        int maxs = (int)Math.Floor(ss + cool / segmentDuration) - 1;
                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                        if (minSegment >= ss && maxSegment <= maxs) return rowSegmentArcanePower + ss;
                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                    return -1;
                case Cooldown.Trinket2:
                    for (int ss = 0; ss < segments; ss++)
                    {
                        double cool = trinket2Cooldown;
                        int maxs = (int)Math.Floor(ss + cool / segmentDuration) - 1;
                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                        if (minSegment >= ss && maxSegment <= maxs) return rowSegmentArcanePower + ss;
                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                    return -1;
            }
            return -1;
        }*/

        private bool ValidateCooldown(Cooldown cooldown, double effectDuration, double cooldownDuration, bool needsFullEffect, double fullEffectDuration, List<SegmentConstraint> segmentConstraints, VariableType cooldownType)
        {
            const double eps = 0.00002;
            double[] segCount = GetSegmentCooldownCount(cooldown, cooldownType);
            
            if (cooldownDuration < 0) cooldownDuration = 3 * calculationOptions.FightDuration;

            //if (resolution > (cooldownDuration - effectDuration) * 0.5) resolution = (cooldownDuration - effectDuration) * 0.5;

            bool valid = true;

            if (needsFullEffect)
            {
                for (int seg = 0; seg < segmentList.Count; seg++)
                {
                    if (segCount[seg] > 0.0 && segmentList[seg].TimeEnd + effectDuration < calculationOptions.FightDuration)
                    {
                        // extend to resolution
                        int segmin = resolution[seg].MinSegment;
                        int segmax = resolution[seg].MaxSegment;
                        // check
                        double total = 0.0;
                        double localtotal = 0.0;
                        for (int s = 0; s < segmentList.Count; s++)
                        {
                            if (InActivationDistance(segmin, segmax, s, effectDuration)) total += segCount[s];
                            if (Math.Abs(seg - s) <= 1) localtotal += segCount[s];
                        }
                        if (localtotal > fullEffectDuration + eps && total < effectDuration - eps)
                        {
                            // there might be other similar cases, but I'm afraid to overrestrict
                            // needs some heavy evaluation to determine when it is safe to restrict
                            // two branches, either force to full full total or restrict to just one
                            SolverLP cooldownUsed = lp.Clone();
                            if (cooldownUsed.Log != null) cooldownUsed.Log.AppendLine("Force total full " + cooldown.ToString() + " at " + seg);
                            int row = cooldownUsed.AddConstraint();
                            for (int s = 0; s < segmentList.Count; s++)
                            {
                                if (InActivationDistance(seg, s, effectDuration)/*Math.Abs(seg - s) <= mindist*/)
                                {
                                    for (int index = segmentColumn[s]; index < segmentColumn[s + 1]; index++)
                                    {
                                        if (solutionVariable[index].IsMatch(cooldown, cooldownType)) cooldownUsed.SetConstraintElement(row, index, -1.0);
                                    }
                                }
                            }
                            for (int index = 0; index < segmentColumn[0]; index++) // fix if variable ordering changes
                            {
                                if (solutionVariable[index].IsMatch(cooldown, cooldownType))
                                {
                                    int outseg = solutionVariable[index].Segment;
                                    if (InActivationDistance(seg, outseg, effectDuration)/*Math.Abs(seg - outseg) <= mindist*/) cooldownUsed.SetConstraintElement(row, index, -1.0);
                                }
                            }
                            cooldownUsed.SetConstraintRHS(row, -effectDuration);
                            cooldownUsed.ForceRecalculation(true);
                            HeapPush(cooldownUsed);
                            cooldownUsed = lp;
                            if (cooldownUsed.Log != null) cooldownUsed.Log.AppendLine("Limit total full " + cooldown.ToString() + " at " + seg);
                            row = cooldownUsed.AddConstraint();
                            for (int s = 0; s < segmentList.Count; s++)
                            {
                                if (InActivationDistance(seg, s, effectDuration)/*Math.Abs(seg - s) <= mindist*/)
                                {
                                    for (int index = segmentColumn[s]; index < segmentColumn[s + 1]; index++)
                                    {
                                        if (solutionVariable[index].IsMatch(cooldown, cooldownType)) cooldownUsed.SetConstraintElement(row, index, 1.0);
                                    }
                                }
                            }
                            for (int index = 0; index < segmentColumn[0]; index++) // fix if variable ordering changes
                            {
                                if (solutionVariable[index].IsMatch(cooldown, cooldownType))
                                {
                                    int outseg = solutionVariable[index].Segment;
                                    if (InActivationDistance(seg, outseg, effectDuration)/*Math.Abs(seg - outseg) <= mindist*/) cooldownUsed.SetConstraintElement(row, index, 1.0);
                                }
                            }
                            cooldownUsed.SetConstraintRHS(row, fullEffectDuration);
                            cooldownUsed.ForceRecalculation(true);
                            HeapPush(cooldownUsed);
                            return false;
                        }
                        if (total < fullEffectDuration - 0.0001) // loosened tolerance, sometimes it can't enforce
                        {
                            // check if we can reuse an existing constraint
                            int minseg = -1;
                            int maxseg = -1;
                            bool foundConstraint = false;
                            SegmentConstraint constraint = default(SegmentConstraint);
                            minseg = -1;
                            maxseg = -1;
                            for (int s = 0; s < segmentList.Count; s++)
                            {
                                if (InActivationDistance(segmin, segmax, s, effectDuration))
                                {
                                    if (minseg == -1) minseg = s;
                                    maxseg = s;
                                }
                            }
                            foundConstraint = false;                                
                            if (segmentConstraints != null)
                            {
                                foreach (SegmentConstraint c in segmentConstraints)
                                {
                                    if (c.MinSegment <= minseg && c.MaxSegment >= maxseg)
                                    {
                                        foundConstraint = true;
                                        constraint = c;
                                        break;
                                    }
                                }
                            }
                            if (foundConstraint && Math.Abs(lp.GetRHS(constraint.Row) - fullEffectDuration) < eps)
                            {
                                SolverLP cooldownUsed = lp.Clone();
                                if (cooldownUsed.Log != null) cooldownUsed.Log.AppendLine("Force full " + cooldown.ToString() + " at " + segmin + "-" + segmax);
                                cooldownUsed.SetLHS(constraint.Row, fullEffectDuration);
                                if (constraint.MinSegment < minseg) DisableCooldown(cooldownUsed, cooldown, cooldownType, constraint.MinSegment, minseg - 1);
                                if (constraint.MaxSegment > maxseg) DisableCooldown(cooldownUsed, cooldown, cooldownType, maxseg + 1, constraint.MaxSegment);
                                // make sure something is in segmin-segmax or we're duplicating work
                                int row = AddConstraint(cooldownUsed, cooldown, cooldownType, segmin, segmax);
                                cooldownUsed.SetConstraintRHS(row, segmentList[segmax].TimeEnd - segmentList[segmin].TimeStart);
                                cooldownUsed.SetConstraintLHS(row, 0.1);
                                cooldownUsed.ForceRecalculation(true);
                                HeapPush(cooldownUsed);
                            }
                            else
                            {
                                SolverLP cooldownUsed = lp.Clone();
                                if (cooldownUsed.Log != null) cooldownUsed.Log.AppendLine("Force full " + cooldown.ToString() + " at " + segmin + "-" + segmax);
                                int row = cooldownUsed.AddConstraint();
                                for (int s = 0; s < segmentList.Count; s++)
                                {
                                    if (s >= minseg && s <= maxseg)
                                    {
                                        for (int index = segmentColumn[s]; index < segmentColumn[s + 1]; index++)
                                        {
                                            if (solutionVariable[index].IsMatch(cooldown, cooldownType)) cooldownUsed.SetConstraintElement(row, index, -1.0);
                                        }
                                    }
                                }
                                for (int index = 0; index < segmentColumn[0]; index++) // fix if variable ordering changes
                                {
                                    if (solutionVariable[index].IsMatch(cooldown, cooldownType))
                                    {
                                        int outseg = solutionVariable[index].Segment;
                                        if (outseg >= minseg && outseg <= maxseg) cooldownUsed.SetConstraintElement(row, index, -1.0);
                                    }
                                    if (cooldown == Cooldown.FlameCap && integralMana)
                                    {
                                        // if we push flame cap to full in this segment then we can further restrict mana gems to eliminate unnecessary MIP branches
                                        if (solutionVariable[index].Type == VariableType.ManaGem)
                                        {
                                            //int gemdist = (int)Math.Floor(120.0 / segmentDuration);
                                            // TODO double check how this has to be with variable resolution stuff
                                            int outseg = solutionVariable[index].Segment;
                                            if (InCooldownDistance(seg, outseg, 0.0, 120.0)/*Math.Abs(outseg - seg) < gemdist*/) cooldownUsed.EraseColumn(index);
                                        }
                                    }
                                }
                                cooldownUsed.SetConstraintRHS(row, -fullEffectDuration);
                                // make sure something is in segmin-segmax or we're duplicating work
                                row = AddConstraint(cooldownUsed, cooldown, cooldownType, segmin, segmax);
                                cooldownUsed.SetConstraintRHS(row, segmentList[segmax].TimeEnd - segmentList[segmin].TimeStart);
                                cooldownUsed.SetConstraintLHS(row, 0.1);
                                cooldownUsed.ForceRecalculation(true);
                                HeapPush(cooldownUsed);
                            }
                            // cooldown not used
                            if (lp.Log != null) lp.Log.AppendLine("Disable " + cooldown.ToString() + " at " + segmin + "-" + segmax);
                            DisableCooldown(lp, cooldown, cooldownType, segmin, segmax);
                            HeapPush(lp);
                            return false;
                        }
                    }
                }
            }

            for (int seg = 0; seg < segmentList.Count; seg++)
            {
                double inseg = segCount[seg];
                if (inseg > eps)
                {
                    // extend to resolution
                    int segmin = resolution[seg].MinSegment;
                    int segmax = resolution[seg].MaxSegment;
                    // verify that outside duration segments are 0
                    for (int outseg = 0; outseg < segmentList.Count; outseg++)
                    {
                        if (!InActivationDistance(segmin, segmax, outseg, effectDuration) && InCooldownDistance(segmin, segmax, outseg, effectDuration, cooldownDuration))
                        {
                            if (segCount[outseg] > eps)
                            {
                                valid = false;
                                break;
                            }
                        }
                    }
                    if (!valid)
                    {
                        // branch on whether cooldown is used in this segment
                        SolverLP cooldownUsed = lp.Clone();
                        // cooldown used
                        if (cooldownUsed.Log != null) cooldownUsed.Log.AppendLine("Use " + cooldown.ToString() + " at " + segmin + "-" + segmax + ", disable around");
                        DisableCooldown(cooldownUsed, cooldown, cooldownType, outseg => !InActivationDistance(segmin, segmax, outseg, effectDuration) && InCooldownDistance(segmin, segmax, outseg, effectDuration, cooldownDuration));
                        // make sure something is in segmin-segmax or we're duplicating work
                        int row = AddConstraint(cooldownUsed, cooldown, cooldownType, segmin, segmax);
                        cooldownUsed.SetConstraintRHS(row, segmentList[segmax].TimeEnd - segmentList[segmin].TimeStart);
                        cooldownUsed.SetConstraintLHS(row, 0.1);
                        cooldownUsed.ForceRecalculation(true);
                        HeapPush(cooldownUsed);
                        // cooldown not used
                        if (lp.Log != null) lp.Log.AppendLine("Disable " + cooldown.ToString() + " at " + segmin + "-" + segmax);
                        DisableCooldown(lp, cooldown, cooldownType, segmin, segmax);
                        HeapPush(lp);
                        return false;
                    }
                }
            }

            // detect holes
            for (int seg = 0; seg < segmentList.Count; seg++)
            {
                double inseg = segCount[seg];
                if (inseg > eps)
                {
                    int lastseg = -1;
                    for (int outseg = seg + 1; outseg < segmentList.Count && InActivationDistance(seg, outseg, effectDuration); outseg++)
                    {
                        if (segCount[outseg] > eps) lastseg = outseg;
                    }
                    if (lastseg != -1)
                    {
                        // extend to resolution
                        int segmin = resolution[seg].MinSegment;
                        int segmax = resolution[seg].MaxSegment;
                        int lastsegmin = resolution[lastseg].MinSegment;
                        int lastsegmax = resolution[lastseg].MaxSegment;

                        for (int outseg = segmax + 1; outseg < lastsegmin; outseg++)
                        {
                            if (segCount[outseg] < segmentList[outseg].Duration - eps) valid = false;
                        }
                        if (!valid) // coldsnapped icy veins doesn't have to be contiguous, but getting better results assuming it is
                        {
                            // either seg must be disabled, lastseg disabled, or middle to max
                            SolverLP leftDisabled = lp.Clone();
                            SolverLP rightDisabled = lp.Clone();
                            // do force to full first if all are equally good
                            if (lp.Log != null) lp.Log.AppendLine("Force " + cooldown.ToString() + " to max from " + segmax + " to " + lastsegmin);
                            // don't need extra constraints, can use the existing constraint that limits segment size
                            //int row = lp.AddConstraint(false); // need the extra constraint because just removing others won't force this one to full 30 sec
                            for (int outseg = 0; outseg < segmentList.Count; outseg++)
                            {
                                if (outseg > segmax && outseg < lastsegmin)
                                {
                                    for (int index = segmentColumn[outseg]; index < segmentColumn[outseg + 1]; index++)
                                    {
                                        CastingState state = solutionVariable[index].State;
                                        if (!solutionVariable[index].IsMatch(cooldown, cooldownType)) lp.EraseColumn(index);
                                    }
                                }
                            }
                            for (int index = 0; index < segmentColumn[0]; index++) // fix if variable ordering changes
                            {
                                CastingState state = solutionVariable[index].State;
                                int outseg = solutionVariable[index].Segment;
                                if (outseg > segmax && outseg < lastsegmin)
                                {
                                    if (!solutionVariable[index].IsMatch(cooldown, cooldownType) && !solutionVariable[index].IsZeroTime) lp.EraseColumn(index); // don't remove zero-length variables
                                }
                            }
                            //double spanDuration = 0.0;
                            for (int outseg = segmax + 1; outseg < lastsegmin; outseg++)
                            {
                                //spanDuration += segmentList[outseg].Duration;
                                lp.SetLHS(rowSegment + outseg, segmentList[outseg].Duration);
                            }
                            //lp.SetConstraintRHS(row, -spanDuration /*segmentDuration * (lastseg - seg - 1)*/);
                            lp.ForceRecalculation(true);
                            HeapPush(lp);
                            if (leftDisabled.Log != null) leftDisabled.Log.AppendLine("Disable left " + cooldown.ToString() + " at " + segmin + "-" + segmax);
                            DisableCooldown(leftDisabled, cooldown, cooldownType, segmin, segmax);
                            HeapPush(leftDisabled);
                            if (rightDisabled.Log != null) rightDisabled.Log.AppendLine("Disable right " + cooldown.ToString() + " at " + lastsegmin + "-" + lastsegmax);
                            DisableCooldown(rightDisabled, cooldown, cooldownType, lastsegmin, lastsegmax);
                            HeapPush(rightDisabled);
                            return false;
                        }
                    }
                }
            }

            return valid;
        }

        private bool ValidateCooldownAdvanced(Cooldown cooldown, double effectDuration, double cooldownDuration, VariableType cooldownType)
        {
            const double eps = 0.00001;
            double[] segCount = GetSegmentCooldownCount(cooldown, cooldownType);

            // for irregular cooldown durations have to make a special pass verifying everything is in order
            // if cooldowns are broken need to add special constraints that ensure cooldowns are respected
            // do this only for effects that can't be coldsnapped as those don't have to respect cooldown always and are handled separately

            if (cooldown != Cooldown.None && (!coldsnapAvailable || (cooldown != Cooldown.WaterElemental && cooldown != Cooldown.IcyVeins))) // TODO: consider extending for Cooldown.None, but for now we don't need it for evocation
            {
                List<int> activations = new List<int>();
                // validate consecutive activations
                double lastStart = double.NegativeInfinity;
                for (int seg = 0; seg < segmentList.Count; seg++)
                {
                    if (segCount[seg] > eps && (seg == 0 || segCount[seg - 1] < eps))
                    {
                        activations.Add(seg);
                        // activation at the latest (seg + 1) * segmentDuration - segCount[seg]
                        // make sure (seg + 1) * segmentDuration - segCount[seg] - lastStart >= cooldownDuration
                        if (segmentList[seg].TimeEnd - segCount[seg] - lastStart < cooldownDuration - eps)
                        {
                            EnforceEffectCooldown(lastStart, cooldown, cooldownDuration, effectDuration, false);
                            //lp unused
                            lp.ReleaseConstraints();
                            return false;
                        }
                        lastStart = segmentList[seg].TimeStart;
                        if (seg + 1 < segmentList.Count && segCount[seg + 1] > eps) lastStart = segmentList[seg].TimeEnd - segCount[seg];
                    }
                }

                for (int cooldowns = 2; cooldowns < activations.Count; cooldowns++)
                {
                    for (int act = 0; act < activations.Count - cooldowns; act++)
                    {
                        int seg1 = activations[act];
                        int seg3 = activations[act + cooldowns];
                        // first activation at seg1 * segmentDuration or (seg1 + 1) * segmentDuration - segCount[seg1] if activation is right bound
                        double firstActivation = segmentList[seg1].TimeStart;
                        if (seg1 + 1 < segmentList.Count && segCount[seg1 + 1] > eps) firstActivation = segmentList[seg1].TimeEnd - segCount[seg1];
                        // remote activation at (seg3 + 1) * segmentDuration - segCount[seg3]
                        // make sure (seg3 + 1) * segmentDuration - segCount[seg3] - firstActivation >= cooldowns * cooldownDuration
                        if (segmentList[seg3].TimeEnd - segCount[seg3] - firstActivation < cooldowns * cooldownDuration - eps)
                        {
                            EnforceRemoteEffectCooldown(seg1, seg3, cooldowns, cooldown, cooldownDuration, effectDuration);
                            // lp unused
                            lp.ReleaseConstraints();
                            return false;
                        }

                    }
                }
            }

            return true;
        }

        private bool ValidateCooldownAdvanced2(Cooldown cooldown, double effectDuration, double cooldownDuration, VariableType cooldownType)
        {
            const double eps = 0.00001;
            double[] segCount = GetSegmentCooldownCount(cooldown, cooldownType);

            // for irregular cooldown durations have to make a special pass verifying everything is in order
            // if cooldowns are broken need to add special constraints that ensure cooldowns are respected
            // do this only for effects that can't be coldsnapped as those don't have to respect cooldown always and are handled separately

            if (cooldown != Cooldown.None && (!coldsnapAvailable || (cooldown != Cooldown.WaterElemental && cooldown != Cooldown.IcyVeins))) // TODO: consider extending for Cooldown.None, but for now we don't need it for evocation
            {
                List<ActivationConstraints> activations = new List<ActivationConstraints>();
                // validate consecutive activations
                double lastStart = double.NegativeInfinity;
                ActivationConstraints lastActivation = null;
                for (int seg = 0; seg < segmentList.Count; seg++)
                {
                    if (segCount[seg] > eps && (seg == 0 || segCount[seg - 1] < eps))
                    {
                        ActivationConstraints activation = GetActivationConstraints(cooldown, seg, segCount[seg]);
                        activations.Add(activation);
                        // activation at the latest (seg + 1) * segmentDuration - segCount[seg]
                        // make sure (seg + 1) * segmentDuration - segCount[seg] - lastStart >= cooldownDuration
                        //if (segmentList[seg].TimeEnd - segCount[seg] - lastStart < cooldownDuration - eps)
                        if (activation.MaxTime - lastStart < cooldownDuration - eps)
                        {
                            //EnforceEffectCooldown(lastStart, cooldown, cooldownDuration, effectDuration, false);
                            EnforceEffectCooldown(lastActivation, activation, cooldown, cooldownDuration, effectDuration);
                            // lp unused
                            lp.ReleaseConstraints();
                            return false;
                        }
                        //lastStart = segmentList[seg].TimeStart;
                        //if (seg + 1 < segmentList.Count && segCount[seg + 1] > eps) lastStart = segmentList[seg].TimeEnd - segCount[seg];
                        lastStart = activation.MinTime;
                        lastActivation = activation;
                    }
                }

                /*for (int cooldowns = 2; cooldowns < activations.Count; cooldowns++)
                {
                    for (int act = 0; act < activations.Count - cooldowns; act++)
                    {
                        int seg1 = activations[act].Segment;
                        int seg3 = activations[act + cooldowns].Segment;
                        // first activation at seg1 * segmentDuration or (seg1 + 1) * segmentDuration - segCount[seg1] if activation is right bound
                        double firstActivation = segmentList[seg1].TimeStart;
                        if (seg1 + 1 < segmentList.Count && segCount[seg1 + 1] > eps) firstActivation = segmentList[seg1].TimeEnd - segCount[seg1];
                        // remote activation at (seg3 + 1) * segmentDuration - segCount[seg3]
                        // make sure (seg3 + 1) * segmentDuration - segCount[seg3] - firstActivation >= cooldowns * cooldownDuration
                        if (segmentList[seg3].TimeEnd - segCount[seg3] - firstActivation < cooldowns * cooldownDuration - eps)
                        {
                            EnforceRemoteEffectCooldown(seg1, seg3, cooldowns, cooldown, cooldownDuration, effectDuration);
                            return false;
                        }

                    }
                }*/
            }

            return true;
        }

        private class ActivationConstraints
        {
            public int Segment;
            public bool Loose;
            public double MinTime;
            public double MaxTime;
            public Cooldown JointEffect;
            public Cooldown LeftLink;
            public Cooldown RightLink;
            public Cooldown FullLeftLink;
            public Cooldown FullRightLink;
            public double LeftPaddding;
            public double RightPadding;
        }

        private ActivationConstraints GetActivationConstraints(Cooldown effect, int segment, double effectInSegment)
        {
            ActivationConstraints result = new ActivationConstraints();
            result.Segment = segment;
            result.Loose = true;
            int hex = (int)effect;
            int N = hexList[segment].Count;
            for (int i = 0; i < N; i++)
            {
                if (((int)effect & hexList[segment][i]) != 0)
                {
                    hex |= hexList[segment][i];
                }
            }
            result.JointEffect = (Cooldown)hex;
            if (segment < segmentList.Count - 1)
            {
                result.RightLink = (Cooldown)(hex & hexMask[segment + 1]);
                result.FullRightLink = (Cooldown)(hexMask[segment] & hexMask[segment + 1]);
            }
            if (segment > 0)
            {
                result.LeftLink = (Cooldown)(hex & hexMask[segment - 1]);
                result.FullLeftLink = (Cooldown)(hexMask[segment] & hexMask[segment - 1]);
            }
            result.MaxTime = segmentList[segment].TimeEnd - effectInSegment;
            result.MinTime = segmentList[segment].TimeStart;
            // things that are linked, but don't include the full link have to be on the other side of the link
            // things that are linked and include the full link and link some more have to be on the link
            // things that are linked and include the full link and don't link anything else can be on either side
            // things that are not linked have to be on the other side if there is a link
            // things that are not linked can be on either side if there is no link
            const double eps = 0.00001;
            for (int index = segmentColumn[segment]; index < segmentColumn[segment + 1]; index++)
            {
                if (solution[index] > eps)
                {
                    CastingState state = solutionVariable[index].State;
                    if (state != null && !state.GetCooldown(effect))
                    {
                        bool hasToBeLeft = (state.Cooldown & result.RightLink) != result.RightLink || ((state.Cooldown & result.LeftLink) == result.LeftLink && (state.Cooldown & result.FullLeftLink) != result.LeftLink);
                        bool hasToBeRight = (state.Cooldown & result.LeftLink) != result.LeftLink || ((state.Cooldown & result.RightLink) == result.RightLink && (state.Cooldown & result.FullRightLink) != result.RightLink);
                        if (hasToBeLeft)
                        {
                            result.MinTime += solution[index];
                            result.LeftPaddding += solution[index];
                        }
                        if (hasToBeRight)
                        {
                            result.MaxTime -= solution[index];
                            result.RightPadding += solution[index];
                        }
                    }
                }
            }
            for (int index = 0; index < segmentColumn[0]; index++) // fix if variable ordering changes
            {
                if (solution[index] > eps && solutionVariable[index].Segment == segment && !solutionVariable[index].IsZeroTime)
                {
                    CastingState state = solutionVariable[index].State;
                    if (state != null && !state.GetCooldown(effect))
                    {
                        bool hasToBeLeft = (state.Cooldown & result.RightLink) != result.RightLink || ((state.Cooldown & result.LeftLink) == result.LeftLink && (state.Cooldown & result.FullLeftLink) != result.LeftLink);
                        bool hasToBeRight = (state.Cooldown & result.LeftLink) != result.LeftLink || ((state.Cooldown & result.RightLink) == result.RightLink && (state.Cooldown & result.FullRightLink) != result.RightLink);
                        if (hasToBeLeft)
                        {
                            result.MinTime += solution[index];
                            result.LeftPaddding += solution[index];
                        }
                        if (hasToBeRight)
                        {
                            result.MaxTime -= solution[index];
                            result.RightPadding += solution[index];
                        }
                    }
                }
            }
            if (segment < segmentList.Count - 1 && (hexMask[segment + 1] & (int)effect) == (int)effect)
            {
                result.Loose = false;
                result.MinTime = result.MaxTime = segmentList[segment].TimeEnd - effectInSegment;
            }
            return result;
        }

        private bool ValidateFlamecap()
        {
            Cooldown cooldown = Cooldown.FlameCap;
            //double effectDuration = 60.0;
            //double cooldownDuration = 180.0;
            const double eps = 0.00001;
            double[] segCount = GetSegmentCooldownCount(cooldown, VariableType.None);

            //int mindist = (int)Math.Ceiling(effectDuration / segmentDuration);
            //int mindist2 = (int)Math.Floor(effectDuration / segmentDuration);
            //int maxdist = (cooldownDuration < 0) ? 3 * segments : ((int)Math.Floor((cooldownDuration - effectDuration) / segmentDuration));
            //int maxdist2 = (cooldownDuration < 0) ? 3 * segments : ((int)Math.Floor(cooldownDuration / segmentDuration));

            bool valid = true;

            double[] manaGem = GetSegmentCooldownCount(Cooldown.None, VariableType.ManaGem);

            float manaBurn = 80;
            if (calculationOptions.AoeDuration > 0)
            {
                Spell s = calculationResult.BaseState.GetSpell(SpellId.ArcaneExplosion);
                manaBurn = s.CostPerSecond - s.ManaRegenPerSecond;
            }
            else if (talents.EmpoweredFire > 0)
            {
                Spell s = calculationResult.BaseState.GetSpell(SpellId.Fireball);
                manaBurn = s.CostPerSecond - s.ManaRegenPerSecond;
            }
            else if (talents.EmpoweredFrostbolt > 0)
            {
                Spell s = calculationResult.BaseState.GetSpell(SpellId.FrostboltFOF);
                manaBurn = s.CostPerSecond - s.ManaRegenPerSecond;
            }
            else if (talents.SpellPower > 0)
            {
                Spell s = calculationResult.BaseState.GetSpell(SpellId.AB3ABar3C);
                manaBurn = s.CostPerSecond - s.ManaRegenPerSecond;
            }
            if (icyVeinsAvailable)
            {
                manaBurn *= 1.1f;
            }
            if (arcanePowerAvailable)
            {
                manaBurn *= 1.1f;
            }

            // check border case if we have mana gem in first or last segment
            if (manaGem[0] > 0)
            {
                // either no gem at 0 or make sure it starts late enough
                int firstSeg;
                for (firstSeg = 0; firstSeg < segmentList.Count; firstSeg++)
                {
                    if (segCount[firstSeg] > 0) break;
                }
                if (firstSeg < segmentList.Count)
                {
                    double totalGem = 0.0;
                    for (int seg = 0; seg < firstSeg; seg++)
                    {
                        totalGem += manaGem[seg];
                    }
                    // tfc = firstSeg * 30 + 30 - segCount[firstSeg]
                    // tgem <= tfc - 120.0 * totalGem
                    // overflow >= 2400 - tgem * manaBurn

                    // tfc - 120.0 * totalGem >= tgem >= (2400 - overflow) / manaBurn

                    // (2400 - overflow) / manaBurn <= tfc - 120.0 * totalGem
                    // 120.0 * manaBurn * totalGem - overflow <= (firstSeg * 30 + 30 - segCount[firstSeg]) * manaBurn - 2400
                    // 120.0 * totalGem - overflow / manaBurn + segCount[firstSeg] <= (firstSeg * 30 + 30) - 2400 / manaBurn

                    double overflow = solution[calculationResult.ColumnManaOverflow];

                    if (120.0 * totalGem - overflow / manaBurn + segCount[firstSeg] > segmentList[firstSeg].TimeEnd - calculationResult.ManaGemValue * (1 + calculationResult.BaseStats.BonusManaGem) / manaBurn + eps)
                    {
                        // no gem
                        SolverLP nogem = lp.Clone();
                        if (nogem.Log != null) nogem.Log.AppendLine("No gem at 0");
                        for (int index = 0; index < solutionVariable.Count; index++)
                        {
                            if (solutionVariable[index].Type == VariableType.ManaGem && solutionVariable[index].Segment == 0)
                            {
                                nogem.EraseColumn(index);
                            }
                        }
                        HeapPush(nogem);
                        // restrict flame cap/overflow
                        if (lp.Log != null) lp.Log.AppendLine("Restrict flame cap with gem at 0");
                        int row = lp.AddConstraint();
                        lp.SetConstraintRHS(row, segmentList[firstSeg].TimeEnd - calculationResult.ManaGemValue * (1 + calculationResult.BaseStats.BonusManaGem) / manaBurn);
                        for (int index = 0; index < solutionVariable.Count; index++)
                        {
                            CastingState state = solutionVariable[index].State;
                            if (solutionVariable[index].Type == VariableType.ManaGem && solutionVariable[index].Segment < firstSeg) lp.SetConstraintElement(row, index, 120.0);
                            else if (solutionVariable[index].Type == VariableType.ManaOverflow && solutionVariable[index].Segment == 0) lp.SetConstraintElement(row, index, -1.0 / manaBurn);
                            else if (state != null && state.FlameCap && solutionVariable[index].Segment == firstSeg) lp.SetConstraintElement(row, index, 1.0);
                        }
                        lp.ForceRecalculation(true);
                        HeapPush(lp);
                        return false;
                    }
                }
            }

            if (manaGem[segmentList.Count - 1] > 0)
            {
                // either no gem or make sure it starts early enough
                int lastSeg;
                for (lastSeg = segmentList.Count - 1; lastSeg >= 0; lastSeg--)
                {
                    if (segCount[lastSeg] > 0) break;
                }
                if (lastSeg >= 0)
                {
                    while (lastSeg > 0 && segCount[lastSeg - 1] > 0) lastSeg--;
                    double totalGem = 0.0;
                    for (int seg = lastSeg + 1; seg < segmentList.Count; seg++)
                    {
                        totalGem += manaGem[seg];
                    }
                    // tfc = lastSeg * 30 + 30 - segCount[lastSeg]
                    // tgem >= tfc + 60.0 + 120 * totalGem
                    // overflow >= 2400 - fight * manaBurn + tgem * manaBurn

                    // tfc + 60.0 + 120 * totalGem <= tgem <= overflow / manaBurn - 2400 / manaBurn + fight

                    // tfc + 120 * totalGem - overflow / manaBurn <= - 2400 / manaBurn + fight - 60.0
                    // 120 * totalGem - overflow / manaBurn - segCount[lastSeg] <= fight - 90.0 - lastSeg * 30 - 2400 / manaBurn

                    double overflow = solution[calculationResult.ColumnManaOverflow + segmentList.Count - 1];

                    if (120.0 * totalGem - overflow / manaBurn - segCount[lastSeg] > (calculationOptions.FightDuration - 60.0 - segmentList[lastSeg].TimeEnd) - calculationResult.ManaGemValue * (1 + calculationResult.BaseStats.BonusManaGem) / manaBurn + eps)
                    {
                        // no gem
                        SolverLP nogem = lp.Clone();
                        if (nogem.Log != null) nogem.Log.AppendLine("No gem at end");
                        for (int index = 0; index < solutionVariable.Count; index++)
                        {
                            if (solutionVariable[index].Type == VariableType.ManaGem && solutionVariable[index].Segment == segmentList.Count - 1)
                            {
                                nogem.EraseColumn(index);
                            }
                        }
                        HeapPush(nogem);
                        // restrict flame cap/overflow
                        if (lp.Log != null) lp.Log.AppendLine("Restrict flame cap with gem at end");
                        int row = lp.AddConstraint();
                        lp.SetConstraintRHS(row, (calculationOptions.FightDuration - 60.0 - segmentList[lastSeg].TimeEnd) - calculationResult.ManaGemValue * (1 + calculationResult.BaseStats.BonusManaGem) / manaBurn);
                        for (int index = 0; index < solutionVariable.Count; index++)
                        {
                            CastingState state = solutionVariable[index].State;
                            if (solutionVariable[index].Type == VariableType.ManaGem && solutionVariable[index].Segment > lastSeg) lp.SetConstraintElement(row, index, 120.0);
                            else if (solutionVariable[index].Type == VariableType.ManaOverflow && solutionVariable[index].Segment == segmentList.Count - 1) lp.SetConstraintElement(row, index, -1.0 / manaBurn);
                            else if (state != null && state.FlameCap && solutionVariable[index].Segment == lastSeg) lp.SetConstraintElement(row, index, -1.0);
                        }
                        lp.ForceRecalculation(true);
                        HeapPush(lp);
                        return false;
                    }
                }
            }

            // . . . . . . X . . . . .
            // . .[. . . . X . . .]. . option 1, gem at start of X
            // . . .[. . . X . . . .]. option 2, gem at end of X
            // or anywhere in between
            // . .[. . . . X . . . .]. flamecap in this area is limited
            // Math.Ceiling(120.0 / segmentDuration) in each way
            // total area (2 * Math.Ceiling(120.0 / segmentDuration) + 1) * segmentDuration
            // max flame cap: (2 * Math.Ceiling(120.0 / segmentDuration) + 1) * segmentDuration - 240.0
            // for partial mana gems convert each 1.0 mana gem into 40 sec of extra flame cap
            //int segdist = (int)Math.Ceiling(120.0 / segmentDuration);
            /*for (int seg = 0; seg < segmentList.Count; seg++)
            {
                if (manaGem[seg] > 0)
                {
                    double maxfc = (2 * segdist + 1) * segmentDuration - 240.0 + (1.0 - manaGem[seg]) * 40.0;
                    double count = 0.0;
                    for (int segfc = Math.Max(seg - segdist, 0); segfc < Math.Min(seg + segdist + 1, segments); segfc++)
                    {
                        count += segCount[segfc];
                    }
                    if (count > maxfc + eps)
                    {
                        // remove mana gem
                        SolverLP nogem = lp.Clone();
                        if (nogem.Log != null) nogem.Log.AppendLine("No gem at " + seg);
                        for (int index = 0; index < solutionVariable.Count; index++)
                        {
                            if (solutionVariable[index].Type == VariableType.ManaGem && solutionVariable[index].Segment == seg)
                            {
                                nogem.EraseColumn(index);
                            }
                        }
                        HeapPush(nogem);
                        // restrict flame cap
                        if (lp.Log != null) lp.Log.AppendLine("Restrict flame cap around " + seg);
                        int row = lp.AddConstraint(false);
                        lp.SetConstraintRHS(row, maxfc);
                        for (int index = 0; index < solutionVariable.Count; index++)
                        {
                            CastingState state = solutionVariable[index].State;
                            int iseg = solutionVariable[index].Segment;
                            if (state != null && state.GetCooldown(cooldown) && Math.Abs(seg - iseg) <= segdist) lp.SetConstraintElement(row, index, 1.0);
                        }
                        lp.ForceRecalculation(true);
                        HeapPush(lp);
                        return false;
                    }
                }
            }*/
            return valid;
        }

        private bool ValidateManaGemEffect()
        {
            const double eps = 0.00001;
            double[] trinketCount = GetSegmentCooldownCount(Cooldown.ManaGemEffect, VariableType.None);
            double[] manaGem = GetSegmentCooldownCount(Cooldown.None, VariableType.ManaGem);

            // make sure gem is activated together with SCB
            for (int i = 0; i < segmentList.Count; i++)
            {
                if (trinketCount[i] > 0.0 && (i == 0 || trinketCount[i - 1] == 0.0))
                {
                    if (manaGem[i] < 1.0 - eps)
                    {
                        // either pop gem
                        SolverLP gem = lp.Clone();
                        if (gem.Log != null) gem.Log.AppendLine("Pop gem with gem effect at " + i);
                        for (int index = 0; index < solutionVariable.Count; index++)
                        {
                            if (solutionVariable[index].Type == VariableType.ManaGem && solutionVariable[index].Segment == i)
                            {
                                gem.SetColumnLowerBound(index, 1.0);
                            }
                        }
                        gem.ForceRecalculation(true);
                        HeapPush(gem);
                        // or force activation before this segment
                        if (i > 0)
                        {
                            gem = lp.Clone();
                            if (gem.Log != null) gem.Log.AppendLine("Force gem effect before " + i);
                            int row = gem.AddConstraint();
                            gem.SetConstraintRHS(row, segmentList[i - 1].Duration);
                            gem.SetConstraintLHS(row, 0.1);
                            SetCooldownElements(gem, row, Cooldown.ManaGemEffect, i - 1, 1.0);
                            gem.ForceRecalculation(true);
                            HeapPush(gem);
                        }
                        // or remove effect
                        if (lp.Log != null) lp.Log.AppendLine("Remove gem effect at " + i);
                        for (int index = segmentColumn[i]; index < segmentColumn[i + 1]; index++)
                        {
                            CastingState state = solutionVariable[index].State;
                            if (state != null && state.GetCooldown(Cooldown.ManaGemEffect) && solutionVariable[index].Segment == i)
                            {
                                lp.EraseColumn(index);
                            }
                        }
                        HeapPush(lp);
                        return false;
                    }
                }
            }

            for (int seg = 0; seg < segmentList.Count; seg++)
            {
                if (manaGem[seg] > 0 && trinketCount[seg] > 0)
                {
                    // make accurate prediction about mana burn if you can, assume all non-effect comes in front (that consumes mana)
                    double manaSpentBeforeGem = 0;
                    double manaChangeBeforeGem = 0;
                    for (int index = 0; index < solutionVariable.Count; index++)
                    {
                        if (solutionVariable[index].Segment == seg)
                        {
                            CastingState state = solutionVariable[index].State;
                            if (solutionVariable[index].Type != VariableType.ManaGem && (state == null || !state.GetCooldown(Cooldown.ManaGemEffect)))
                            {
                                double mps = lp[rowManaRegen, index];
                                manaChangeBeforeGem += mps * solution[index];
                                if (mps > 0)
                                {
                                    manaSpentBeforeGem += mps * solution[index];
                                }
                            }
                        }
                    }

                    // mana overflow guards
                    if (calculationResult.BaseStats.Mana - manaList[seg] + manaSpentBeforeGem < calculationResult.ManaGemValue * (1 + calculationResult.BaseStats.BonusManaGem) - 0.001)
                    {
                        SolverLP nogem = lp.Clone();
                        // restrict SCB/overflow
                        if (lp.Log != null) lp.Log.AppendLine("Restrict overflow gem effect at " + seg);
                        int row = lp.AddConstraint();
                        lp.SetConstraintRHS(row, double.PositiveInfinity);
                        lp.SetConstraintLHS(row, 0.1 * (calculationResult.ManaGemValue * (1 + calculationResult.BaseStats.BonusManaGem) - calculationResult.BaseStats.Mana + calculationResult.StartingMana));
                        for (int index = 0; index < solutionVariable.Count; index++)
                        {
                            CastingState state = solutionVariable[index].State;
                            int s = solutionVariable[index].Segment;
                            if (s < seg)
                            {
                                double mps = lp[rowManaRegen, index];
                                lp.SetConstraintElement(row, index, 0.1 * mps);
                            }
                            else if (s == seg && solutionVariable[index].Type != VariableType.ManaGem && (state == null || !state.GetCooldown(Cooldown.ManaGemEffect)))
                            {
                                double mps = lp[rowManaRegen, index];
                                if (mps > 0) lp.SetConstraintElement(row, index, 0.1 * mps);
                            }
                        }
                        lp.ForceRecalculation(true);
                        HeapPush(lp);
                        // no gem/trinket at 0
                        if (nogem.Log != null) nogem.Log.AppendLine("No gem at " + seg);
                        for (int index = 0; index < solutionVariable.Count; index++)
                        {
                            if (solutionVariable[index].Type == VariableType.ManaGem && solutionVariable[index].Segment == seg)
                            {
                                nogem.EraseColumn(index);
                            }
                            // you can't do this, all you can say is there is no activation here, could happen that we get effect that continues from previous segment
                            //CastingState state = solutionVariable[index].State;
                            //if (state != null && state.GetCooldown(Cooldown.ManaGemEffect) && solutionVariable[index].Segment == 0)
                            //{
                            //    nogem.EraseColumn(index);
                            //}
                        }
                        HeapPush(nogem);
                        return false;
                    }

                    // mana underflow guards, can only use if mana gem effect activation is fixed
                    ActivationConstraints activation = GetActivationConstraints(Cooldown.ManaGemEffect, seg, trinketCount[seg]);
                    if (manaList[seg] - manaChangeBeforeGem < -0.001 && !activation.Loose)
                    {
                        // either make sure gem is popped soon enough
                        SolverLP branchlp = lp.Clone();
                        if (branchlp.Log != null) branchlp.Log.AppendLine("Restrict underflow gem effect at " + seg);
                        int row = branchlp.AddConstraint();
                        // mana spending <= start mana
                        branchlp.SetConstraintRHS(row, 0.1 * calculationResult.StartingMana);
                        for (int index = 0; index < solutionVariable.Count; index++)
                        {
                            CastingState state = solutionVariable[index].State;
                            int s = solutionVariable[index].Segment;
                            if (s < seg)
                            {
                                double mps = branchlp[rowManaRegen, index];
                                branchlp.SetConstraintElement(row, index, 0.1 * mps);
                            }
                            else if (s == seg && solutionVariable[index].Type != VariableType.ManaGem && (state == null || !state.GetCooldown(Cooldown.ManaGemEffect)))
                            {
                                double mps = branchlp[rowManaRegen, index];
                                branchlp.SetConstraintElement(row, index, 0.1 * mps);
                            }
                        }
                        branchlp.ForceRecalculation(true);
                        HeapPush(branchlp);
                        // or make it loose
                        branchlp = lp.Clone();
                        if (branchlp.Log != null) branchlp.Log.AppendLine("Remove gem effect at " + (seg + 1));
                        DisableCooldown(branchlp, Cooldown.ManaGemEffect, seg + 1);
                        branchlp.ForceRecalculation(true);
                        HeapPush(branchlp);
                        // or there is no activation here
                        if (lp.Log != null) lp.Log.AppendLine("No gem at " + seg);
                        for (int index = 0; index < solutionVariable.Count; index++)
                        {
                            if (solutionVariable[index].Type == VariableType.ManaGem && solutionVariable[index].Segment == seg)
                            {
                                lp.EraseColumn(index);
                            }
                        }
                        HeapPush(lp);
                        return false;
                    }
                }
            }        

            return true;
        }

        private bool ValidateEffectPotion()
        {
            const double eps = 0.00001;
            double[] potionOfSpeedCount = GetSegmentCooldownCount(Cooldown.PotionOfSpeed, VariableType.None);
            double[] potionOfWildMagicCount = GetSegmentCooldownCount(Cooldown.PotionOfWildMagic, VariableType.None);
            double[][] segCount = new double[][] { potionOfSpeedCount, potionOfWildMagicCount };
            Cooldown[] effect = new Cooldown[] { Cooldown.PotionOfSpeed, Cooldown.PotionOfWildMagic };
            double[] total = new double[2];

            for (int i = 0; i < total.Length; i++)
            {
                for (int seg = 0; seg < segmentList.Count; seg++)
                {
                    total[i] += segCount[i][seg];
                }
            }

            for (int i = 0; i < total.Length; i++)
            {
                if (total[i] > eps)
                {
                    for (int j = i + 1; j < total.Length; j++)
                    {
                        if (total[j] > eps)
                        {
                            // can't have both
                            SolverLP branchlp = lp.Clone();
                            if (branchlp.Log != null) branchlp.Log.AppendLine("Disable " + effect[i]);
                            DisableCooldown(branchlp, effect[i], 0, segmentList.Count - 1);
                            HeapPush(branchlp);
                            if (lp.Log != null) lp.Log.AppendLine("Disable " + effect[j]);
                            DisableCooldown(lp, effect[j], 0, segmentList.Count - 1);
                            HeapPush(lp);
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private bool ValidateHastedEvocation()
        {
            const double eps = 0.00001;
            double[] evoCount = GetSegmentCooldownCount(Cooldown.Evocation, VariableType.Evocation);
            double[] evoIVCount = GetSegmentCooldownCount(Cooldown.Evocation, VariableType.EvocationIV);
            double[] evoHeroCount = GetSegmentCooldownCount(Cooldown.Evocation, VariableType.EvocationHero);
            double[] evoIVHeroCount = GetSegmentCooldownCount(Cooldown.Evocation, VariableType.EvocationIVHero);
            double[][] segCount = new double[][] { evoCount, evoIVCount, evoHeroCount, evoIVHeroCount };
            VariableType[] speedType = new VariableType[] { VariableType.Evocation, VariableType.EvocationIV, VariableType.EvocationHero, VariableType.EvocationIVHero };

            // can't have different speeds of evocation in same activation
            for (int seg = 0; seg < segmentList.Count; seg++)
            {
                int segmin = resolution[seg].MinSegment;
                int segmax = resolution[seg].MaxSegment;
                for (int speed = 0; speed < 3; speed++)
                {
                    double inseg = segCount[speed][seg];
                    if (inseg > eps)
                    {
                        bool valid = true;
                        // verify that in cooldown distance there is no different speed
                        for (int outseg = 0; outseg < segmentList.Count; outseg++)
                        {
                            if (InCooldownDistance(segmin, segmax, outseg, calculationResult.EvocationDuration, calculationResult.EvocationCooldown))
                            {
                                for (int speed2 = speed + 1; speed2 < 4; speed2++)
                                {
                                    if (segCount[speed2][outseg] > eps)
                                    {
                                        valid = false;
                                        break;
                                    }
                                }
                            }
                        }
                        if (!valid)
                        {
                            // branch on whether cooldown is used in this segment
                            SolverLP cooldownUsed = lp.Clone();
                            // cooldown used
                            if (cooldownUsed.Log != null) cooldownUsed.Log.AppendLine(speedType[speed] + " at " + segmin + "-" + segmax);
                            for (int index = 0; index < segmentColumn[0]; index++) // fix if variable ordering changes
                            {
                                int outseg = solutionVariable[index].Segment;
                                CastingState state = solutionVariable[index].State;
                                if (InCooldownDistance(segmin, segmax, outseg, calculationResult.EvocationDuration, calculationResult.EvocationCooldown))
                                {
                                    VariableType okSpeed = speedType[speed];
                                    if (state != null && state.Evocation && solutionVariable[index].Type != okSpeed)
                                    {
                                        cooldownUsed.EraseColumn(index);
                                    }
                                }
                            }
                            int row = AddConstraint(cooldownUsed, Cooldown.Evocation, speedType[speed], segmin, segmax);
                            cooldownUsed.SetConstraintRHS(row, calculationResult.EvocationDuration);
                            cooldownUsed.SetConstraintLHS(row, 0.1);
                            cooldownUsed.ForceRecalculation(true);
                            HeapPush(cooldownUsed);
                            // cooldown not used
                            if (lp.Log != null) lp.Log.AppendLine("Disable " + speedType[speed] + " at " + segmin + "-" + segmax);
                            DisableCooldown(lp, Cooldown.Evocation, speedType[speed], segmin, segmax);
                            HeapPush(lp);
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private bool ValidateEvocation()
        {
            const double eps = 0.00001;
            double[] evoCount = GetSegmentCooldownCount(Cooldown.Evocation, VariableType.None);
            double[][] evoHastedCount = new double[3][];
            double[][] evoHastedActivation = new double[3][];
            bool[][] cooldownPresent = new bool[3][];
            Cooldown[] hastedCooldown = new Cooldown[] { Cooldown.IcyVeins, Cooldown.Heroism, Cooldown.IcyVeins | Cooldown.Heroism };
            VariableType[] hastedCooldownType = new VariableType[] { VariableType.EvocationIV, VariableType.EvocationHero, VariableType.EvocationIVHero };
            if (calculationOptions.EnableHastedEvocation)
            {
                for (int i = 0; i < hastedCooldown.Length; i++)
                {
                    evoHastedCount[i] = GetSegmentCooldownCount(Cooldown.Evocation, hastedCooldownType[i]);
                    evoHastedActivation[i] = GetSegmentCooldownCount(Cooldown.Evocation | hastedCooldown[i], hastedCooldownType[i]);
                }
                double[] ivCount = GetSegmentCooldownCount(Cooldown.IcyVeins, VariableType.None);
                double[] heroCount = GetSegmentCooldownCount(Cooldown.Heroism, VariableType.None);
                cooldownPresent[0] = new bool[segmentList.Count];
                cooldownPresent[1] = new bool[segmentList.Count];
                cooldownPresent[2] = new bool[segmentList.Count];
                for (int seg = 0; seg < segmentList.Count; seg++)
                {
                    cooldownPresent[0][seg] = ivCount[seg] > eps;
                    cooldownPresent[1][seg] = heroCount[seg] > eps;
                    cooldownPresent[2][seg] = ivCount[seg] + heroCount[seg] > eps;
                }
            }

            for (int seg = 0; seg < segmentList.Count; seg++)
            {
                if (evoCount[seg] > eps && (seg + 1 < segmentList.Count && evoCount[seg + 1] > eps) && (seg == 0 || evoCount[seg - 1] < eps))
                {
                    // this is the activation of evocation and it is fixed, so it has to come at the end
                    // validate against underflow

                    // make accurate prediction about mana burn if you can, assume all non-effect comes in front (that consumes mana)
                    double manaChangeBefore = 0;
                    for (int index = 0; index < solutionVariable.Count; index++)
                    {
                        if (solutionVariable[index].Segment == seg)
                        {
                            CastingState state = solutionVariable[index].State;
                            if (state == null || !state.Evocation)
                            {
                                double mps = lp[rowManaRegen, index];
                                manaChangeBefore += mps * solution[index];
                            }
                        }
                    }

                    if (manaList[seg] - manaChangeBefore < -0.001)
                    {
                        // either make sure evo is popped soon enough
                        SolverLP branchlp = lp.Clone();
                        if (branchlp.Log != null) branchlp.Log.AppendLine("Restrict underflow evocation at " + seg);
                        int row = branchlp.AddConstraint();
                        // mana spending <= start mana
                        branchlp.SetConstraintRHS(row, 0.1 * calculationResult.StartingMana);
                        for (int index = 0; index < solutionVariable.Count; index++)
                        {
                            int s = solutionVariable[index].Segment;
                            CastingState state = solutionVariable[index].State;
                            if (s < seg)
                            {
                                double mps = branchlp[rowManaRegen, index];
                                branchlp.SetConstraintElement(row, index, 0.1 * mps);
                            }
                            else if (s == seg && (state == null || !state.Evocation))
                            {
                                double mps = branchlp[rowManaRegen, index];
                                branchlp.SetConstraintElement(row, index, 0.1 * mps);
                            }
                        }
                        branchlp.ForceRecalculation(true);
                        HeapPush(branchlp);
                        // or make it loose
                        branchlp = lp.Clone();
                        if (branchlp.Log != null) branchlp.Log.AppendLine("Remove evocation at " + (seg + 1));
                        DisableCooldown(branchlp, Cooldown.Evocation, seg + 1);
                        branchlp.ForceRecalculation(true);
                        HeapPush(branchlp);
                        // or there is no activation here
                        // this means that either there is evo before or it isn't here
                        branchlp = lp.Clone();
                        if (branchlp.Log != null) branchlp.Log.AppendLine("Evocation starts before " + seg);
                        row = branchlp.AddConstraint();
                        SetCooldownElements(branchlp, row, Cooldown.Evocation, seg - 1, 1.0);
                        branchlp.SetConstraintRHS(row, segmentList[seg - 1].Duration);
                        branchlp.SetConstraintLHS(row, 0.1);
                        branchlp.ForceRecalculation(true);
                        HeapPush(branchlp);
                        if (lp.Log != null) lp.Log.AppendLine("No evocation at " + seg);
                        DisableCooldown(lp, Cooldown.Evocation, seg);
                        HeapPush(lp);
                        return false;
                    }
                }

                if (evoCount[seg] > eps && (seg + 1 >= segmentList.Count || evoCount[seg + 1] < eps) && (seg > 0 && evoCount[seg - 1] > eps))
                {
                    double overflow = 0;
                    double evoRegen = 0;
                    for (int index = 0; index < solutionVariable.Count; index++)
                    {
                        if (solutionVariable[index].Segment == seg)
                        {
                            CastingState state = solutionVariable[index].State;
                            if (solutionVariable[index].Type == VariableType.ManaOverflow)
                            {
                                overflow += solution[index];
                            }
                            else if (state != null && state.Evocation)
                            {
                                evoRegen -= solution[index] * lp[rowManaRegen, index];
                            }
                        }
                    }

                    // this is the last segment of evocation activation that is fixed, has to be activated in previous segment
                    // guard against overflow
                    if (calculationResult.BaseStats.Mana - manaList[seg] + overflow < evoRegen - 0.001)
                    {
                        SolverLP branchlp = lp.Clone();
                        // restrict overflow
                        if (branchlp.Log != null) branchlp.Log.AppendLine("Restrict overflow evocation at " + seg);
                        int row = branchlp.AddConstraint();
                        branchlp.SetConstraintRHS(row, double.PositiveInfinity);
                        branchlp.SetConstraintLHS(row, 0.1 * (calculationResult.StartingMana - calculationResult.BaseStats.Mana));
                        for (int index = 0; index < solutionVariable.Count; index++)
                        {
                            int s = solutionVariable[index].Segment;
                            CastingState state = solutionVariable[index].State;
                            if (s < seg || (s == seg && (solutionVariable[index].Type == VariableType.ManaOverflow || (state != null && state.Evocation))))
                            {
                                double mps = branchlp[rowManaRegen, index];
                                branchlp.SetConstraintElement(row, index, 0.1 * mps);
                            }
                        }
                        branchlp.ForceRecalculation(true);
                        HeapPush(branchlp);
                        // otherwise it has to be loose
                        branchlp = lp.Clone();
                        if (branchlp.Log != null) branchlp.Log.AppendLine("No evocation before " + seg);
                        DisableCooldown(branchlp, Cooldown.Evocation, seg - 1);
                        HeapPush(branchlp);
                        // or no evocation here
                        if (lp.Log != null) lp.Log.AppendLine("No evocation at " + seg);
                        DisableCooldown(lp, Cooldown.Evocation, seg);
                        HeapPush(lp);
                        return false;
                    }
                }

                if (calculationOptions.EnableHastedEvocation)
                {
                    for (int i = 0; i < hastedCooldown.Length; i++)
                    {
                        if (evoHastedCount[i][seg] > eps && evoHastedCount[i][seg] > evoHastedActivation[i][seg] + eps)
                        {
                            // has to happen after IV, so make sure we don't run oom
                            // this only has to hold if there is some evocation left when IV is out
                            // if evocation happens during IV it can be placed anywhere within IV
                            double manaChangeBefore = 0;
                            for (int index = 0; index < solutionVariable.Count; index++)
                            {
                                if (solutionVariable[index].Segment == seg)
                                {
                                    CastingState state = solutionVariable[index].State;
                                    if (state == null || !state.Evocation)
                                    {
                                        double mps = lp[rowManaRegen, index];
                                        if ((state != null && state.GetCooldown(hastedCooldown[i])) || mps < 0) // count what has to be before and what is benefitial to be before
                                        {
                                            manaChangeBefore += mps * solution[index];
                                        }
                                    }
                                }
                            }

                            if (manaList[seg] - manaChangeBefore < -0.001)
                            {
                                // either make sure evo is popped soon enough
                                SolverLP branchlp = lp.Clone();
                                if (branchlp.Log != null) branchlp.Log.AppendLine("Restrict underflow " + hastedCooldownType[i] + " at " + seg);
                                int row = branchlp.AddConstraint();
                                // mana spending <= start mana
                                branchlp.SetConstraintRHS(row, 0.1 * calculationResult.StartingMana);
                                for (int index = 0; index < solutionVariable.Count; index++)
                                {
                                    int s = solutionVariable[index].Segment;
                                    CastingState state = solutionVariable[index].State;
                                    if (s < seg)
                                    {
                                        double mps = branchlp[rowManaRegen, index];
                                        branchlp.SetConstraintElement(row, index, 0.1 * mps);
                                    }
                                    else if (s == seg && (state == null || !state.Evocation))
                                    {
                                        double mps = branchlp[rowManaRegen, index];
                                        if ((state != null && state.GetCooldown(hastedCooldown[i])) || mps < 0)
                                        {
                                            branchlp.SetConstraintElement(row, index, 0.1 * mps);
                                        }
                                    }
                                }
                                branchlp.ForceRecalculation(true);
                                HeapPush(branchlp);
                                // or there is no evo here without IV
                                if (lp.Log != null) lp.Log.AppendLine("No " + hastedCooldownType[i] + " without cooldown at " + seg);
                                for (int index = 0; index < solutionVariable.Count; index++)
                                {
                                    if (solutionVariable[index].Type == hastedCooldownType[i] && !solutionVariable[index].State.GetCooldown(hastedCooldown[i]))
                                    {
                                        if (solutionVariable[index].Segment == seg) lp.EraseColumn(index);
                                    }
                                }
                                HeapPush(lp);
                                return false;
                            }
                        }

                        if (evoHastedCount[i][seg] > eps && seg + 1 < segmentList.Count && cooldownPresent[i][seg + 1])
                        {
                            // if cooldown happens at next segment then we're fixed in the constraints of
                            // the cooldown, meaning that anything that is not cooldown must happen
                            // in front of evocation
                            double manaChangeBefore = 0;
                            for (int index = 0; index < solutionVariable.Count; index++)
                            {
                                if (solutionVariable[index].Segment == seg)
                                {
                                    CastingState state = solutionVariable[index].State;
                                    if (state == null || !state.Evocation)
                                    {
                                        double mps = lp[rowManaRegen, index];
                                        if ((state != null && (state.Cooldown & hastedCooldown[i]) == 0) || mps < 0) // count what has to be before and what is benefitial to be before
                                        {
                                            manaChangeBefore += mps * solution[index];
                                        }
                                    }
                                }
                            }

                            if (manaList[seg] - manaChangeBefore < -0.001)
                            {
                                // either make sure evo is popped soon enough
                                SolverLP branchlp = lp.Clone();
                                if (branchlp.Log != null) branchlp.Log.AppendLine("Restrict underflow " + hastedCooldownType[i] + " with cooldown at " + seg);
                                int row = branchlp.AddConstraint();
                                // mana spending <= start mana
                                branchlp.SetConstraintRHS(row, 0.1 * calculationResult.StartingMana);
                                for (int index = 0; index < solutionVariable.Count; index++)
                                {
                                    int s = solutionVariable[index].Segment;
                                    CastingState state = solutionVariable[index].State;
                                    if (s < seg)
                                    {
                                        double mps = branchlp[rowManaRegen, index];
                                        branchlp.SetConstraintElement(row, index, 0.1 * mps);
                                    }
                                    else if (s == seg && (state == null || !state.Evocation))
                                    {
                                        double mps = branchlp[rowManaRegen, index];
                                        if ((state != null && (state.Cooldown & hastedCooldown[i]) == 0) || mps < 0)
                                        {
                                            branchlp.SetConstraintElement(row, index, 0.1 * mps);
                                        }
                                    }
                                }
                                branchlp.ForceRecalculation(true);
                                HeapPush(branchlp);
                                // or there is no evo here without IV
                                if (lp.Log != null) lp.Log.AppendLine("Nothing of " + hastedCooldown[i] + " at " + (seg + 1));
                                for (int index = 0; index < solutionVariable.Count; index++)
                                {
                                    CastingState state = solutionVariable[index].State;
                                    if (state != null && (state.Cooldown & hastedCooldown[i]) != 0)
                                    {
                                        if (solutionVariable[index].Segment == seg + 1) lp.EraseColumn(index);
                                    }
                                }
                                HeapPush(lp);
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }
    }
}
