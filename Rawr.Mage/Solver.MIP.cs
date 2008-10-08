//#define DEBUG_BRANCHING
using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Mage
{
    public enum MIPMethod
    {
        BestBound,
        DepthFirst
    }

    public partial class Solver
    {
#if DEBUG_BRANCHING
        private List<SolverLP> childList;
#endif

        private class BranchNode : IComparable<BranchNode>
        {
            public SolverLP Lp;
            public BranchNode Parent;
            public List<BranchNode> Children = new List<BranchNode>();
            public int Index;
            public int Depth;
            public double Value;
            public double ProbeValue;

            int IComparable<BranchNode>.CompareTo(BranchNode other)
            {                
                return -(this.Lp != null ? this.Lp.Value : this.Value).CompareTo(other.Lp != null ? other.Lp.Value : other.Value);
            }
        }

        private Heap<SolverLP> heap;
        private BranchNode currentNode;

        private List<int>[] hexList;
        private double[] segmentFilled;
        private int[] hexMask;

        private void RestrictSolution()
        {
#if DEBUG_BRANCHING
            childList = new List<SolverLP>();
#endif

            switch (calculationOptions.MIPMethod)
            {
                case MIPMethod.BestBound:
                    BestBoundSearch();
                    break;
                case MIPMethod.DepthFirst:
                    DepthFirstSearch();
                    break;
            }
        }

        private void DepthFirstSearch()
        {
            int sizeLimit = calculationOptions.MaxHeapLimit;
            lp.SolvePrimalDual(); // solve primal and recalculate to get a stable starting point

            SolverLP rootCopy = lp.Clone();
            BranchNode root = new BranchNode() { Lp = lp };

            currentNode = root;

            double lowerBound = 0.0;
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
                    do
                    {
                        currentNode = currentNode.Parent;
                        currentNode.Children[currentNode.Index] = null;
                        currentNode.Index++;
                    } while (currentNode.Index >= currentNode.Children.Count && currentNode.Parent != null);
                    if (currentNode.Index >= currentNode.Children.Count) break; // we explored the whole search space
                    if (value > 0 && currentNode.Depth < highestBacktrack)
                    {
                        highestBacktrack = currentNode.Depth;
                        System.Diagnostics.Trace.WriteLine("Backtrack at " + highestBacktrack + ", value = " + lowerBound + ", root = " + currentNode.Value + ", round = " + round);
                    }
                    currentNode = currentNode.Children[currentNode.Index];
                }
                else
                {
                    lp = currentNode.Lp;
                    if (lp != null)
                    {
                        solution = lp.Solve();
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
                                    currentNode.Children[currentNode.Index] = null;
                                    currentNode.Index++;
                                } while (currentNode.Index >= currentNode.Children.Count && currentNode.Parent != null);
                            }
                            else
                            {
                                break;
                            }
                            if (currentNode.Index >= currentNode.Children.Count) break; // we explored the whole search space
                            if (currentNode.Depth < highestBacktrack)
                            {
                                highestBacktrack = currentNode.Depth;
                                System.Diagnostics.Trace.WriteLine("Backtrack at " + highestBacktrack + ", value = " + lowerBound + ", root = " + currentNode.Value + ", round = " + round);
                            }
                            currentNode = currentNode.Children[currentNode.Index];
                        }
                        else
                        {
                            BranchNode first = null;
                            foreach (BranchNode node in currentNode.Children)
                            {
                                ProbeDive(node, ref round, ref incumbent, ref lowerBound, sizeLimit);
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
                                ProbeDive(node, ref round, ref incumbent, ref lowerBound, sizeLimit);
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
                                // can happen, so just handle it
                                if (currentNode.Parent != null)
                                {
                                    do
                                    {
                                        currentNode = currentNode.Parent;
                                        currentNode.Children[currentNode.Index] = null;
                                        currentNode.Index++;
                                    } while (currentNode.Index >= currentNode.Children.Count && currentNode.Parent != null);
                                }
                                else
                                {
                                    break;
                                }
                                if (currentNode.Index >= currentNode.Children.Count) break; // we explored the whole search space
                                if (currentNode.Depth < highestBacktrack)
                                {
                                    highestBacktrack = currentNode.Depth;
                                    System.Diagnostics.Trace.WriteLine("Backtrack at " + highestBacktrack + ", value = " + lowerBound + ", root = " + currentNode.Value + ", round = " + round);
                                }
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
                                    currentNode.Children[currentNode.Index] = null;
                                    currentNode.Index++;
                                } while (currentNode.Index >= currentNode.Children.Count && currentNode.Parent != null);
                            }
                            else
                            {
                                break;
                            }
                            if (currentNode.Index >= currentNode.Children.Count) break; // we explored the whole search space
                            if (currentNode.Depth < highestBacktrack)
                            {
                                highestBacktrack = currentNode.Depth;
                                System.Diagnostics.Trace.WriteLine("Backtrack at " + highestBacktrack + ", value = " + lowerBound + ", root = " + currentNode.Value + ", round = " + round);
                            }
                            currentNode = currentNode.Children[currentNode.Index];
                        }
                    }
                }
                round++;
            } while (round < sizeLimit);

            lp = incumbent;
            if (lp == null) lp = rootCopy;
            solution = lp.Solve();
        }

        private void ProbeDive(BranchNode node, ref int round, ref SolverLP incumbent, ref double lowerBound, int sizeLimit)
        {
            if (node.ProbeValue != 0) return;
            BranchNode store = currentNode;
            currentNode = node;
            do
            {
                double value = currentNode.Lp != null ? currentNode.Lp.Value : currentNode.Value; // force evaluation and get value
                currentNode.Value = value;
                if (value < lowerBound + 0.00001)
                {
                    if (value == 0.0)
                    {
                        value = -1.0; // if we have a value of 0 then all siblings have a value of 0, so this whole subtree is unfeasible, reprobe
                        currentNode.ProbeValue = value;
                        if (currentNode == node)
                        {
                            currentNode = store;
                            return;
                        }
                        currentNode = currentNode.Parent; // this node has all children with value of 0
                        currentNode.Value = 0.0;
                        currentNode.ProbeValue = value;
                        if (currentNode == node)
                        {
                            currentNode = store;
                            return;
                        }
                        currentNode = currentNode.Parent; // this node has to be reevaluated
                        // reprobe
                        currentNode.Children.Sort();
                        // evaluate child nodes
                        currentNode = currentNode.Children[0];
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
                    solution = lp.Solve();
                    if (currentNode.Depth > 100)
                    {
                        lp = lp; // investigate
                    }
                    bool valid = IsLpValid();
                    if (valid)
                    {
                        // we found a new lower bound
                        lowerBound = value;
                        incumbent = lp;
                        System.Diagnostics.Trace.WriteLine("Probe value = " + lowerBound + ", root = " + currentNode.Value + ", round = " + round);
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
                        currentNode.Children.Sort();
                        currentNode.Lp = null; // current lp may be reused by one of its children
                        // evaluate child nodes
                        currentNode = currentNode.Children[0];
                    }
                }
                round++;
            } while (round < sizeLimit);
        }

        private void BestBoundSearch()
        {
            int maxHeap = calculationOptions.MaxHeapLimit;
            lp.SolvePrimalDual(); // solve primal and recalculate to get a stable starting point
            heap = new Heap<SolverLP>(HeapType.MaximumHeap);
            HeapPush(lp);

            double max = lp.Value;

            bool valid = true;
            do
            {
                if (heap.Head.Value > max + 0.001) // lowered instability threshold, in case it is still an issue just recompute the solution which "should" give a stable result hopefully
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
                    max = lp.Value;
                    HeapPush(lp);
                    continue;
                    //}
                    //System.Windows.Forms.MessageBox.Show("Instability detected, aborting SMP algorithm (max = " + max + ", value = " + lp.Value + ")");
                    // find something reasonably stable
                    //while (heap.Count > 0 && (lp = heap.Pop()).Value > max + 0.000001) { }
                    //break;
                }
                lp = heap.Pop();
                max = lp.Value;
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
            } while (heap.Count > 0 && !valid);
            //System.Diagnostics.Trace.WriteLine("Heap at solution " + heap.Count);
        }

        private bool IsLpValid()
        {
            AnalyzeSolution();
            bool valid = true;

#if DEBUG_BRANCHING
                childList.Clear();
#endif

            if (integralMana)
            {
                if (valid && calculationOptions.ManaPotionEnabled)
                {
                    valid = ValidateIntegralConsumableOverall(VariableType.ManaPotion, 1.0);
                }
                if (valid && calculationOptions.ManaGemEnabled)
                {
                    valid = ValidateIntegralConsumableOverall(VariableType.ManaGem, 1.0);
                }
                if (valid && calculationOptions.EvocationEnabled)
                {
                    valid = ValidateIntegralConsumableOverall(VariableType.Evocation, 2.0 / calculationResult.BaseState.CastingSpeed);
                }
            }

            if (segmentCooldowns)
            {
                // drums
                if (valid && calculationOptions.DrumsOfBattle)
                {
                    valid = ValidateIntegralConsumableOverall(VariableType.DrumsOfBattle, calculationResult.BaseState.GlobalCooldown);
                }
                // drums
                if (valid && calculationOptions.DrumsOfBattle)
                {
                    valid = ValidateCooldown(Cooldown.DrumsOfBattle, 30.0, 120.0, true, 30.0);
                }
                // make sure all cooldowns are tightly packed and not fragmented
                // mf is trivially satisfied
                // heroism
                if (valid && heroismAvailable)
                {
                    valid = ValidateCooldown(Cooldown.Heroism, 40, -1);
                }
                // ap
                if (valid && arcanePowerAvailable)
                {
                    valid = ValidateCooldown(Cooldown.ArcanePower, calculationResult.ArcanePowerDuration, calculationResult.ArcanePowerCooldown, true, 15.0);
                }
                // iv
                if (valid && icyVeinsAvailable)
                {
                    valid = ValidateCooldown(Cooldown.IcyVeins, 20.0 + (coldsnapAvailable ? 20.0 : 0.0), calculationResult.IcyVeinsCooldown + (coldsnapAvailable ? 20.0 : 0.0), true, 20.0);
                }
                // water elemental
                if (valid && waterElementalAvailable)
                {
                    valid = ValidateIntegralConsumableOverall(VariableType.SummonWaterElemental, calculationResult.BaseState.GlobalCooldown);
                }
                if (valid && waterElementalAvailable)
                {
                    valid = ValidateCooldown(Cooldown.WaterElemental, calculationResult.WaterElementalDuration + (coldsnapAvailable ? calculationResult.WaterElementalDuration : 0.0), calculationResult.WaterElementalCooldown + (coldsnapAvailable ? calculationResult.WaterElementalDuration : 0.0), true, calculationResult.WaterElementalDuration);
                }
                // coldsnap
                if (valid && icyVeinsAvailable && coldsnapAvailable)
                {
                    valid = ValidateColdsnap();
                }
                // combustion
                if (valid && combustionAvailable)
                {
                    valid = ValidateCooldown(Cooldown.Combustion, 15.0, 180.0 + 15.0); // the durations are only used to compute segment distances, for 30 sec segments this should work pretty well
                }
                // flamecap
                if (valid && flameCapAvailable)
                {
                    valid = ValidateCooldown(Cooldown.FlameCap, 60.0, 180.0, integralMana, 60.0);
                }
                // destruction
                if (valid && destructionPotionAvailable)
                {
                    valid = ValidateCooldown(Cooldown.DestructionPotion, 15, 120);
                }
                // t1
                if (valid && trinket1Available)
                {
                    valid = ValidateCooldown(Cooldown.Trinket1, trinket1Duration, trinket1Cooldown);
                }
                // t2
                if (valid && trinket2Available)
                {
                    valid = ValidateCooldown(Cooldown.Trinket2, trinket2Duration, trinket2Cooldown);
                }
                if (valid && trinket1OnManaGem)
                {
                    valid = ValidateSCB(Cooldown.Trinket1);
                }
                if (valid && trinket2OnManaGem)
                {
                    valid = ValidateSCB(Cooldown.Trinket2);
                }
            }

            if (integralMana)
            {
                if (valid && calculationOptions.ManaPotionEnabled)
                {
                    valid = ValidateIntegralConsumable(VariableType.ManaPotion);
                }
                if (valid && calculationOptions.ManaGemEnabled)
                {
                    valid = ValidateIntegralConsumable(VariableType.ManaGem);
                }
            }

            if (segmentCooldowns)
            {
                if (valid && flameCapAvailable)
                {
                    valid = ValidateFlamecap();
                }
                if (valid)
                {
                    valid = ValidateCycling();
                }
                if (valid)
                {
                    valid = ValidateSupergroupCycles();
                }
                if (valid)
                {
                    valid = ValidateSupergroupFragmentation();
                }
                if (valid && calculationOptions.DrumsOfBattle)
                {
                    valid = ValidateActivation(Cooldown.DrumsOfBattle, 30.0, 120.0, VariableType.DrumsOfBattle);
                }
                if (valid && waterElementalAvailable)
                {
                    if (coldsnapAvailable)
                    {
                        valid = ValidateWaterElementalSummon();
                    }
                    else
                    {
                        valid = ValidateActivation(Cooldown.WaterElemental, calculationResult.WaterElementalDuration, calculationResult.WaterElementalCooldown, VariableType.SummonWaterElemental);
                    }
                }
            }

#if DEBUG_BRANCHING
                if (!valid)
                {
                    bool allLower = true;
                    foreach (SolverLP childLP in childList)
                    {
                        if (childLP.Value >= max - 1.0)
                        {
                            allLower = false;
                            break;
                        }
                    }
                    if (allLower && childList.Count > 0 && childList[0].Log != null)
                    {
                        System.Diagnostics.Trace.WriteLine("\n\nProves branch has nontrivially lower value:");
                        System.Diagnostics.Trace.Write(childList[0].Log.ToString());
                    }
                    childList.Clear();
                }
#endif
            return valid;
        }

        private void HeapPush(SolverLP childLP)
        {
            switch (calculationOptions.MIPMethod)
            {
                case MIPMethod.BestBound:
                    heap.Push(childLP);
                    break;
                case MIPMethod.DepthFirst:
                    currentNode.Children.Add(new BranchNode() { Lp = childLP, Parent = currentNode, Depth = currentNode.Depth + 1 });
                    break;
            }
#if DEBUG_BRANCHING
            childList.Add(childLP);
#endif
        }

        private void AnalyzeSolution()
        {
            hexList = new List<int>[segments];
            segmentFilled = new double[segments];
            hexMask = new int[segments];
            for (int seg = 0; seg < segments; seg++)
            {
                hexList[seg] = new List<int>();
                for (int index = 0; index < calculationResult.SolutionVariable.Count; index++)
                {
                    CastingState state = calculationResult.SolutionVariable[index].State;
                    int iseg = calculationResult.SolutionVariable[index].Segment;
                    if (state != null && solution[index] > 0 && iseg == seg)
                    {
                        int h = state.GetHex();
                        if (h != 0)
                        {
                            hexMask[seg] |= h;
                            if (!hexList[seg].Contains(h)) hexList[seg].Add(h);
                            segmentFilled[seg] += solution[index];
                        }
                    }
                }
            }
        }

        private bool ValidateSupergroupCycles()
        {
            for (int seg = 1; seg < segments - 1; seg++)
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
                                    if (hexRemovedLP.Log != null) hexRemovedLP.Log.AppendLine("Breaking supergroup cycle at " + seg + ", removing left " + ind1);
                                    for (int index = 0; index < calculationResult.SolutionVariable.Count; index++)
                                    {
                                        CastingState state = calculationResult.SolutionVariable[index].State;
                                        int iseg = calculationResult.SolutionVariable[index].Segment;
                                        if (state != null && iseg == seg - 1 && (state.GetHex() & ind1) != 0) hexRemovedLP.EraseColumn(index);
                                    }
                                    HeapPush(hexRemovedLP);
                                    // eliminate right
                                    hexRemovedLP = lp.Clone();
                                    if (hexRemovedLP.Log != null) hexRemovedLP.Log.AppendLine("Breaking supergroup cycle at " + seg + ", removing right " + ind2);
                                    for (int index = 0; index < calculationResult.SolutionVariable.Count; index++)
                                    {
                                        CastingState state = calculationResult.SolutionVariable[index].State;
                                        int iseg = calculationResult.SolutionVariable[index].Segment;
                                        if (state != null && iseg == seg + 1 && (state.GetHex() & ind2) != 0) hexRemovedLP.EraseColumn(index);
                                    }
                                    HeapPush(hexRemovedLP);
                                    // eliminate middle
                                    hexRemovedLP = lp.Clone();
                                    int ind = ind1 | ind2;
                                    if (hexRemovedLP.Log != null) hexRemovedLP.Log.AppendLine("Breaking supergroup cycle at " + seg + ", removing center " + ind);
                                    for (int index = 0; index < calculationResult.SolutionVariable.Count; index++)
                                    {
                                        CastingState state = calculationResult.SolutionVariable[index].State;
                                        int iseg = calculationResult.SolutionVariable[index].Segment;
                                        if (state != null && iseg == seg && (state.GetHex() & ind) == ind) hexRemovedLP.EraseColumn(index);
                                    }
                                    HeapPush(hexRemovedLP);
                                    // force to full
                                    // this is equivalent to disabling all that are not good (at least I think it is)
                                    if (lp.Log != null) lp.Log.AppendLine("Breaking supergroup cycle at " + seg + ", force to max");
                                    //int row = lp.AddConstraint(false);
                                    for (int index = 0; index < calculationResult.SolutionVariable.Count; index++)
                                    {
                                        CastingState state = calculationResult.SolutionVariable[index].State;
                                        int iseg = calculationResult.SolutionVariable[index].Segment;
                                        if (state != null && iseg == seg)
                                        {
                                            int hex = state.GetHex();
                                            if ((state.GetHex() & (ind1 | ind2)) == 0) lp.EraseColumn(index);
                                            //if ((state.GetHex() & (ind1 | ind2)) != 0) lp.SetConstraintElement(row, index, -1.0);
                                        }
                                    }
                                    //lp.SetConstraintRHS(row, -segmentDuration);
                                    //lp.ForceRecalculation(true);
                                    HeapPush(lp);
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            for (int seg = 0; seg < segments; seg++)
            {
                for (int seg2 = seg - 1; seg2 <= seg + 1; seg2 += 2)
                {
                    if (seg2 >= 0 && seg2 < segments)
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
                                                for (int index = 0; index < calculationResult.SolutionVariable.Count; index++)
                                                {
                                                    CastingState state = calculationResult.SolutionVariable[index].State;
                                                    int iseg = calculationResult.SolutionVariable[index].Segment;
                                                    if (state != null && iseg == seg2 && (state.GetHex() & c) != 0) hexRemovedLP.EraseColumn(index);
                                                }
                                                HeapPush(hexRemovedLP);

                                                // eliminate d from mask
                                                if (d != c)
                                                {
                                                    hexRemovedLP = lp.Clone();
                                                    if (hexRemovedLP.Log != null) hexRemovedLP.Log.AppendLine("Breaking advanced supergroup cycle at " + seg + ", removing from neighbor mask " + d);
                                                    for (int index = 0; index < calculationResult.SolutionVariable.Count; index++)
                                                    {
                                                        CastingState state = calculationResult.SolutionVariable[index].State;
                                                        int iseg = calculationResult.SolutionVariable[index].Segment;
                                                        if (state != null && iseg == seg2 && (state.GetHex() & d) != 0) hexRemovedLP.EraseColumn(index);
                                                    }
                                                    HeapPush(hexRemovedLP);
                                                }

                                                // eliminate bridge
                                                int bridge = c | d | v | z;
                                                hexRemovedLP = lp.Clone();
                                                if (hexRemovedLP.Log != null) hexRemovedLP.Log.AppendLine("Breaking advanced supergroup cycle at " + seg + ", removing bridge " + bridge);
                                                for (int index = 0; index < calculationResult.SolutionVariable.Count; index++)
                                                {
                                                    CastingState state = calculationResult.SolutionVariable[index].State;
                                                    int iseg = calculationResult.SolutionVariable[index].Segment;
                                                    if (state != null && iseg == seg && (state.GetHex() & bridge) == bridge) hexRemovedLP.EraseColumn(index);
                                                }
                                                HeapPush(hexRemovedLP);

                                                // eliminate hcore
                                                hexRemovedLP = lp.Clone();
                                                if (hexRemovedLP.Log != null) hexRemovedLP.Log.AppendLine("Breaking advanced supergroup cycle at " + seg + ", removing doublecross " + v + "-" + z);
                                                for (int index = 0; index < calculationResult.SolutionVariable.Count; index++)
                                                {
                                                    CastingState state = calculationResult.SolutionVariable[index].State;
                                                    int iseg = calculationResult.SolutionVariable[index].Segment;
                                                    if (state != null && iseg == seg && (state.GetHex() & (v | z)) == v) hexRemovedLP.EraseColumn(index);
                                                }
                                                HeapPush(hexRemovedLP);

                                                // eliminate gcore
                                                hexRemovedLP = lp.Clone();
                                                if (hexRemovedLP.Log != null) hexRemovedLP.Log.AppendLine("Breaking advanced supergroup cycle at " + seg + ", removing doublecross " + z + "-" + v);
                                                for (int index = 0; index < calculationResult.SolutionVariable.Count; index++)
                                                {
                                                    CastingState state = calculationResult.SolutionVariable[index].State;
                                                    int iseg = calculationResult.SolutionVariable[index].Segment;
                                                    if (state != null && iseg == seg && (state.GetHex() & (v | z)) == z) hexRemovedLP.EraseColumn(index);
                                                }
                                                HeapPush(hexRemovedLP);
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
            for (int seg = 1; seg < segments - 1; seg++)
            {
                int N = hexList[seg].Count;
                if (N > 0 && segmentFilled[seg] < segmentDuration - 0.000001)
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
                            for (int index = 0; index < calculationResult.SolutionVariable.Count; index++)
                            {
                                CastingState state = calculationResult.SolutionVariable[index].State;
                                int iseg = calculationResult.SolutionVariable[index].Segment;
                                if (state != null && iseg >= seg - 1 && iseg <= seg + 1 && state.GetHex() == minHexChain[i]) hexRemovedLP.EraseColumn(index);
                            }
                            HeapPush(hexRemovedLP);
                        }
                        if (lp.Log != null) lp.Log.AppendLine("Breaking supergroup fragmentation at " + seg + ", force to max");
                        //int row = lp.AddConstraint(false);
                        for (int index = 0; index < calculationResult.SolutionVariable.Count; index++)
                        {
                            CastingState state = calculationResult.SolutionVariable[index].State;
                            int iseg = calculationResult.SolutionVariable[index].Segment;
                            if (state != null && iseg == seg && state.GetHex() == 0) lp.EraseColumn(index);
                            //if (state != null && iseg == seg && state.GetHex() != 0) lp.SetConstraintElement(row, index, -1.0);
                        }
                        lp.SetLHS(rowSegment + seg, segmentDuration);
                        //lp.SetConstraintRHS(row, -segmentDuration);
                        lp.ForceRecalculation(true);
                        HeapPush(lp);
                        return false;
                    }
                }
            }
            return true;
        }

        private List<int> FindShortestTailPath(int core, int t, int node, int[] used, int N, List<int> segmentHexList)
        {
            int[] dist = new int[N]; // distance from CTn indicator
            for (int k = 0; k < N; k++)
            {
                if (used[k] == t + 1)
                {
                    if ((segmentHexList[k] & core & ~node) != 0) dist[k] = 1;
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
                                if ((segmentHexList[k] & segmentHexList[l]) != 0 && (dist[l] + 1 < dist[k] || dist[k] == 0))
                                {
                                    dist[k] = dist[l] + 1;
                                    updated = true;
                                }
                            }
                        }
                    }
                }
            } while (updated);
            int min = int.MaxValue;
            for (int k = 0; k < N; k++)
            {
                if (used[k] == t + 1)
                {
                    if ((segmentHexList[k] & ~core & node) != 0 && dist[k] + 1 < min) min = dist[k] + 1;
                }
            }
            List<int> ret = new List<int>();
            int last = node;
            int d = min - 1;
            for (int k = 0; k < N; k++)
            {
                if (dist[k] == d && (node & segmentHexList[k] & ~core) != 0)
                {
                    last = segmentHexList[k];
                    ret.Add(last);
                    break;
                }
            }
            d--;
            while (d > 0)
            {
                for (int k = 0; k < N; k++)
                {
                    if (dist[k] == d && (last & segmentHexList[k]) != 0)
                    {
                        last = segmentHexList[k];
                        ret.Add(last);
                        break;
                    }
                }
                d--;
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
            for (int seg = 0; seg < segments - 1; seg++)
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
                List<int> threeTail = null;
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

                                        List<int> path = FindShortestTailPath(core, t, node, used, N, hexList12);
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
                                        used[j] = t + 1;
                                        tail[t] |= hexList12[j];
                                        break;
                                    }
                                    else if (!different)
                                    {
                                        alldifferent = false;
                                    }
                                }
                                if (t < 3)
                                {
                                    if (tail[t] == 0)
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
                                    else if (used[j] > 0)
                                    {
                                        // node connected to one of the tails
                                        // it is still possible that it connects to some other tail also

                                        //      XX    tail2
                                        //    XXX     core
                                        //   XX       tail1                                        
                                        //   X   X

                                        // in this case we also have a cycle, find the shortest path through
                                        // each tail and loop it into cycle

                                        List<int> path1 = FindShortestTailPath(core, used[j] - 1, node, used, N, hexList12);
                                        for (t = 0; t < 3 && tail[t] != 0; t++)
                                        {
                                            if (t != used[j] - 1 && (~core & tail[t] & node) != 0)
                                            {
                                                List<int> path2 = FindShortestTailPath(core, t, node, used, N, hexList12);
                                                if (shortestCycle == null || path1.Count + path2.Count + 2 < shortestCycle.Count)
                                                {
                                                    shortestCycle = new List<int>();
                                                    shortestCycle.Add(node);
                                                    shortestCycle.AddRange(path1);
                                                    shortestCycle.Add(core);
                                                    path2.Reverse();
                                                    shortestCycle.AddRange(path2);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            BREAKCYCLE:
                if (shortestCycle != null && (threeTail == null || shortestCycle.Count < threeTail.Count))
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
                    // break cycle by eliminating one pair of indicators
                    for (int i = 0; i < M; i++)
                    {
                        int j = (i + 1) % M;
                        SolverLP hexRemovedLP = lp.Clone();
                        if (hexRemovedLP.Log != null) hexRemovedLP.Log.AppendLine("Breaking cycle at boundary " + seg + ", removing " + indicator[i] + "+" + indicator[j]);
                        for (int index = 0; index < calculationResult.SolutionVariable.Count; index++)
                        {
                            CastingState state = calculationResult.SolutionVariable[index].State;
                            int iseg = calculationResult.SolutionVariable[index].Segment;
                            if (state != null && (iseg == seg || iseg == seg + 1))
                            {
                                int h = state.GetHex();
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
                    for (int index = 0; index < calculationResult.SolutionVariable.Count; index++)
                    {
                        CastingState state = calculationResult.SolutionVariable[index].State;
                        int iseg = calculationResult.SolutionVariable[index].Segment;
                        if (state != null && (iseg == seg || iseg == seg + 1))
                        {
                            int h = state.GetHex();
                            bool isindicated = (h & mask) == (core & mask);
                            if (isindicated) hexRemovedLP.EraseColumn(index);
                        }
                    }
                    HeapPush(hexRemovedLP);
                    for (int i = 0; i < 3; i++)
                    {
                        hexRemovedLP = lp.Clone();
                        if (hexRemovedLP.Log != null) hexRemovedLP.Log.AppendLine("Breaking threetail at boundary " + seg + ", removing " + (threeTail[i + 1] & mask));
                        for (int index = 0; index < calculationResult.SolutionVariable.Count; index++)
                        {
                            CastingState state = calculationResult.SolutionVariable[index].State;
                            int iseg = calculationResult.SolutionVariable[index].Segment;
                            if (state != null && (iseg == seg || iseg == seg + 1))
                            {
                                int h = state.GetHex();
                                bool isindicated = (h & mask) == (threeTail[i + 1] & mask);
                                if (isindicated) hexRemovedLP.EraseColumn(index);
                            }
                        }
                        HeapPush(hexRemovedLP);
                    }
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
                    for (int index = 0; index < calculationResult.SolutionVariable.Count; index++)
                    {
                        CastingState state = calculationResult.SolutionVariable[index].State;
                        int iseg = calculationResult.SolutionVariable[index].Segment;
                        if (state != null && iseg == seg1)
                        {
                            int h = state.GetHex();
                            if ((h & c1) != 0 && (h & c2) == 0) elimLP.EraseColumn(index);
                        }
                    }
                    if (elimLP.Log != null) elimLP.Log.AppendLine("Doublecrossing at " + seg1 + ", " + cool1 + " - " + cool2);
                    HeapPush(elimLP);
                    // eliminate cool2 - cool1 in seg1
                    elimLP = lp.Clone();
                    for (int index = 0; index < calculationResult.SolutionVariable.Count; index++)
                    {
                        CastingState state = calculationResult.SolutionVariable[index].State;
                        int iseg = calculationResult.SolutionVariable[index].Segment;
                        if (state != null && iseg == seg1)
                        {
                            int h = state.GetHex();
                            if ((h & c2) != 0 && (h & c1) == 0) elimLP.EraseColumn(index);
                        }
                    }
                    if (elimLP.Log != null) elimLP.Log.AppendLine("Doublecrossing at " + seg1 + ", " + cool2 + " - " + cool1);
                    HeapPush(elimLP);
                    // eliminate cool1 in seg2
                    elimLP = lp.Clone();
                    for (int index = 0; index < calculationResult.SolutionVariable.Count; index++)
                    {
                        CastingState state = calculationResult.SolutionVariable[index].State;
                        int iseg = calculationResult.SolutionVariable[index].Segment;
                        if (state != null && iseg == seg2)
                        {
                            int h = state.GetHex();
                            if ((h & c1) != 0) elimLP.EraseColumn(index);
                        }
                    }
                    if (elimLP.Log != null) elimLP.Log.AppendLine("Doublecrossing at " + seg1 + "+1, delete " + cool1);
                    HeapPush(elimLP);
                    // eliminate cool2 in seg2
                    elimLP = lp;
                    for (int index = 0; index < calculationResult.SolutionVariable.Count; index++)
                    {
                        CastingState state = calculationResult.SolutionVariable[index].State;
                        int iseg = calculationResult.SolutionVariable[index].Segment;
                        if (state != null && iseg == seg2)
                        {
                            int h = state.GetHex();
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
            for (int index = 0; index < calculationResult.SolutionVariable.Count; index++)
            {
                if (calculationResult.SolutionVariable[index].Type == integralConsumable)
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
                        if (lp.Log != null) lp.Log.AppendLine("Integral consumable " + integralConsumable + " at " + calculationResult.SolutionVariable[index].Segment + ", min " + Math.Ceiling(value));
                        HeapPush(lp);
                        // count <= floor(value)
                        maxCount.SetColumnUpperBound(index, Math.Floor(value));
                        maxCount.ForceRecalculation(true);
                        if (maxCount.Log != null) maxCount.Log.AppendLine("Integral consumable " + integralConsumable + " at " + calculationResult.SolutionVariable[index].Segment + ", max " + Math.Floor(value));
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
            for (int index = 0; index < calculationResult.SolutionVariable.Count; index++)
            {
                if (calculationResult.SolutionVariable[index].Type == integralConsumable)
                {
                    value += solution[index];
                }
            }
            double count = Math.Round(value / unit) * unit;
            bool valid = (Math.Abs(value - count) < 0.000001);
            int row = -1;
            switch (integralConsumable)
            {
                case VariableType.ManaGem:
                    row = rowManaGemOnly;
                    break;
                case VariableType.ManaPotion:
                    row = rowManaPotion;
                    break;
                case VariableType.Evocation:
                    row = rowEvocation;
                    break;
                case VariableType.DrumsOfBattle:
                    row = rowDrumsOfBattle;
                    break;
                case VariableType.SummonWaterElemental:
                    row = rowSummonWaterElementalCount;
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
            double[] ivCount = new double[segments];
            double[] weCount = new double[segments];
            for (int outseg = 0; outseg < segments; outseg++)
            {
                for (int index = segmentColumn[outseg]; index < segmentColumn[outseg + 1]; index++)
                {
                    CastingState state = calculationResult.SolutionVariable[index].State;
                    if (state != null && state.GetCooldown(Cooldown.IcyVeins))
                    {
                        ivCount[outseg] += solution[index];
                    }
                    if (state != null && state.GetCooldown(Cooldown.WaterElemental))
                    {
                        weCount[outseg] += solution[index];
                    }
                }
            }
            for (int index = 0; index < segmentColumn[0]; index++)
            {
                CastingState state = calculationResult.SolutionVariable[index].State;
                //if (state != null && state.GetCooldown(Cooldown.IcyVeins))
                //{
                //    ivCount[calculationResult.SolutionVariable[index].Segment] += solution[index];
                //}
                if (state != null && state.GetCooldown(Cooldown.WaterElemental))
                {
                    weCount[calculationResult.SolutionVariable[index].Segment] += solution[index];
                }
            }
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
            for (int seg = 0; seg < segments; seg++)
            {
                if (!ivActive && ivCount[seg] > eps)
                {
                    if (ivCooldown + ivCount[seg] > segmentDuration + eps)
                    {
                        bool reuse = false;
                        // we need to coldsnap somewhere from [lastIVstart] and [(seg + 1) * segmentDuration - ivCount[seg]]
                        double restrictedColdsnapMin = Math.Max(coldsnapTimeMin, lastIVstart);
                        double restrictedColdsnapMax = Math.Min(coldsnapTimeMax, (seg + 1) * segmentDuration - ivCount[seg]);
                        if (restrictedColdsnapMax > restrictedColdsnapMin + eps)
                        {
                            // we can reuse last coldsnap
                            coldsnapTimeMin = restrictedColdsnapMin;
                            coldsnapTimeMax = restrictedColdsnapMax;
                            ivCooldown = 0.0;
                            reuse = true;
                        }
                        else if (calculationResult.ColdsnapCooldown - ((seg + 1) * segmentDuration - ivCount[seg] - coldsnapTimeMin) <= eps)
                        {
                            // coldsnap is ready
                            coldsnapTimeMin = Math.Max(coldsnapTimeMin + calculationResult.ColdsnapCooldown, lastIVstart);
                            coldsnapTimeMax = (seg + 1) * segmentDuration - ivCount[seg];
                            ivCooldown = 0.0;
                        }
                        else
                        {
                            // coldsnap is not ready
                            ivStart.Add((seg + 1) * segmentDuration - ivCount[seg]);
                            e1 = e2;
                            e1t1 = e2t1;
                            e1t2 = e2t2;
                            e2 = Cooldown.IcyVeins;
                            e2t1 = ivStart[ivStart.Count - 2];
                            e2t2 = ivStart[ivStart.Count - 1];
                            valid = false;
                            break;
                        }
                        double ivActivation = Math.Max(coldsnapTimeMin, seg * segmentDuration);
                        if (seg + 1 < segments && ivCount[seg + 1] > 0) ivActivation = Math.Max(coldsnapTimeMin, (seg + 1) * segmentDuration - ivCount[seg]);
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
                        ivCooldown = calculationResult.IcyVeinsCooldown + ivActivation - seg * segmentDuration;
                        ivActive = true;
                        lastIVstart = ivActivation;
                    }
                    else
                    {
                        // start as soon as possible
                        double ivActivation = Math.Max(seg * segmentDuration + ivCooldown, seg * segmentDuration);
                        if (seg + 1 < segments && ivCount[seg + 1] > 0) ivActivation = (seg + 1) * segmentDuration - ivCount[seg];
                        ivStart.Add(ivActivation);
                        ivCooldown = calculationResult.IcyVeinsCooldown + ivActivation - seg * segmentDuration;
                        ivActive = true;
                        lastIVstart = ivActivation;
                    }
                }
                if (ivActive)
                {
                    if (ivCount[seg] > 0.0)
                    {
                        if (seg * segmentDuration + ivCount[seg] > lastIVstart + 20.0 + eps)
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
                            if (seg * segmentDuration + ivCount[seg] > lastIVstart + 20.0 + eps)
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
                    if (weCooldown + weCount[seg] > segmentDuration + eps)
                    {
                        bool reuse = false;
                        // we need to coldsnap somewhere from [lastWEstart] and [(seg + 1) * segmentDuration - weCount[seg]]
                        double restrictedColdsnapMin = Math.Max(coldsnapTimeMin, lastWEstart);
                        double restrictedColdsnapMax = Math.Min(coldsnapTimeMax, (seg + 1) * segmentDuration - weCount[seg]);
                        if (restrictedColdsnapMax > restrictedColdsnapMin + eps)
                        {
                            // we can reuse last coldsnap
                            coldsnapTimeMin = restrictedColdsnapMin;
                            coldsnapTimeMax = restrictedColdsnapMax;
                            weCooldown = 0.0;
                            reuse = true;
                        }
                        else if (calculationResult.ColdsnapCooldown - ((seg + 1) * segmentDuration - weCount[seg] - coldsnapTimeMin) <= eps)
                        {
                            // coldsnap is ready
                            coldsnapTimeMin = Math.Max(coldsnapTimeMin + calculationResult.ColdsnapCooldown, lastWEstart);
                            coldsnapTimeMax = (seg + 1) * segmentDuration - weCount[seg];
                            weCooldown = 0.0;
                        }
                        else
                        {
                            // coldsnap is not ready
                            weStart.Add((seg + 1) * segmentDuration - weCount[seg]);
                            e1 = e2;
                            e1t1 = e2t1;
                            e1t2 = e2t2;
                            e2 = Cooldown.WaterElemental;
                            e2t1 = weStart[weStart.Count - 2];
                            e2t2 = weStart[weStart.Count - 1];
                            valid = false;
                            break;
                        }
                        double weActivation = Math.Max(coldsnapTimeMin, seg * segmentDuration);
                        if (seg + 1 < segments && weCount[seg + 1] > 0) weActivation = Math.Max(coldsnapTimeMin, (seg + 1) * segmentDuration - weCount[seg]);
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
                        weCooldown = calculationResult.WaterElementalCooldown + weActivation - seg * segmentDuration;
                        weActive = true;
                        lastWEstart = weActivation;
                    }
                    else
                    {
                        // start as soon as possible
                        double weActivation = Math.Max(seg * segmentDuration + weCooldown, seg * segmentDuration);
                        if (seg + 1 < segments && weCount[seg + 1] > 0) weActivation = (seg + 1) * segmentDuration - weCount[seg];
                        weStart.Add(weActivation);
                        weCooldown = calculationResult.WaterElementalCooldown + weActivation - seg * segmentDuration;
                        weActive = true;
                        lastWEstart = weActivation;
                    }
                }
                if (weActive)
                {
                    if (weCount[seg] > 0.0)
                    {
                        if (seg * segmentDuration + weCount[seg] > lastWEstart + calculationResult.WaterElementalDuration + eps)
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
                            if (seg * segmentDuration + weCount[seg] > lastWEstart + calculationResult.WaterElementalDuration + eps)
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
                ivCooldown -= segmentDuration;
                weCooldown -= segmentDuration;
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
                    EnforceEffectCooldown(t1, e, c, d);
                    // t4 - t3 < c2
                    EnforceEffectCooldown(t3, e, c, d);
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
                    EnforceEffectCooldown(t1, e1, c1, d1);
                    // t4 - t3 < c2
                    EnforceEffectCooldown(t3, e2, c2, d2);
                    // t3 >= t2
                    // [----]             [-----]
                    //                      [------------][-------------]
                    int seg2 = (int)((t2 + eps) / segmentDuration);
                    int seg3 = (int)((t3 + eps) / segmentDuration);
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
                    int row = branchlp.AddConstraint(false);
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
                return false;
            }
            return valid;
        }

        private void EnforceEffectCooldown(double firstEffectActivation, Cooldown effect, double effectCooldown, double effectDuration)
        {
            const double eps = 0.000001;
            int seg1 = (int)((firstEffectActivation + eps) / segmentDuration);
            int seg2 = seg1 + (int)((effectCooldown + eps) / segmentDuration);
            SolverLP branchlp;
            // if first activation stays in seg1 then restrict the next activation
            RestrictConsecutiveActivation(effect, effectCooldown, effectDuration, seg1);
            // if first activation moves to seg1 - 1
            if (seg1 >= 1) RestrictConsecutiveActivation(effect, effectCooldown, effectDuration, seg1 - 1);
            // otherwise effect is either started at or before seg1 - 2
            // in this case the last segment with effect is (seg1 - 1) + d / segmentDuration
            if (seg1 >= 2)
            {
                int segmin = (int)Math.Ceiling((seg1 - 1) + effectDuration / segmentDuration - eps);
                if (segmin > seg1)
                {
                    branchlp = lp.Clone();
                    if (branchlp.Log != null) branchlp.Log.AppendLine("Enforce cooldown after " + effect + " at " + seg1 + ", move first activation left");
                    DisableCooldown(branchlp, effect, segmin, (seg1 - 1) + (int)((effectCooldown + eps) / segmentDuration));
                    HeapPush(branchlp);
                }
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
            for (int index = 0; index < calculationResult.SolutionVariable.Count; index++)
            {
                if (calculationResult.SolutionVariable[index].Segment == segment && calculationResult.SolutionVariable[index].Type == variable)
                {
                    if (solution[index] > eps) return true;
                }
            }
            return false;
        }

        private void RestrictConsecutiveActivation(Cooldown effect, double effectCooldown, double effectDuration, int firstActivationSegment)
        {
            const double eps = 0.000001;
            SolverLP branchlp = lp.Clone();
            int seg2 = firstActivationSegment + (int)((effectCooldown + eps) / segmentDuration);
            if (branchlp.Log != null) branchlp.Log.AppendLine("Restrict consecutive activation of " + effect + " at " + firstActivationSegment);
            // first make sure that second activation is not too close
            int row = branchlp.AddConstraint(false);
            SetCooldownElements(branchlp, row, effect, firstActivationSegment, seg2 - 1, 1.0);
            branchlp.SetConstraintRHS(row, effectDuration);
            // make sure there is enough distance between the activations
            // t1 = (seg1 + 1) * segmentDuration - count[seg1]
            // t2 = (seg2 + 1) * segmentDuration - count[seg2]
            // this will potentially overrestrict for IV because effect duration is less than segment duration
            // in that case make another case for t1 = seg1 * segmentDuration, but must make sure that seg1 + 1 does not have IV
            // count[seg2] - count[seg1] <= (seg2 - seg1) * segmentDuration - c
            row = branchlp.AddConstraint(false);
            SetCooldownElements(branchlp, row, effect, firstActivationSegment, -1.0);
            SetCooldownElements(branchlp, row, effect, seg2, 1.0);
            branchlp.SetConstraintRHS(row, (seg2 - firstActivationSegment) * segmentDuration - effectCooldown);
            // make sure it actually starts in this segment
            // count[seg1] + count[seg1 + 1] >= min(segmentDuration, effectDuration)
            row = branchlp.AddConstraint(false);
            SetCooldownElements(branchlp, row, effect, firstActivationSegment, firstActivationSegment + 1, 1.0);
            branchlp.SetConstraintRHS(row, 2 * segmentDuration);
            branchlp.SetConstraintLHS(row, Math.Min(effectDuration, segmentDuration));
            // TODO what if the first activation comes after coldsnap and we have some leftover effect in first segment
            branchlp.ForceRecalculation(true);
            HeapPush(branchlp);
            if (effectDuration < segmentDuration - eps)
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
                            row = branchlp.AddConstraint(false);
                            SetCooldownElements(branchlp, row, effect, seg2, 1.0);
                            branchlp.SetConstraintRHS(row, (seg2 - firstActivationSegment + 1) * segmentDuration - effectCooldown);
                            // make sure it actually starts in this segment
                            // count[seg1] + count[seg1 + 1] >= min(segmentDuration, effectDuration)
                            row = branchlp.AddConstraint(false);
                            SetCooldownElements(branchlp, row, effect, firstActivationSegment, firstActivationSegment + 1, 1.0);
                            branchlp.SetConstraintRHS(row, 2 * segmentDuration);
                            branchlp.SetConstraintLHS(row, Math.Min(effectDuration, segmentDuration));
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
                row = branchlp.AddConstraint(false);
                SetCooldownElements(branchlp, row, effect, seg2, 1.0);
                branchlp.SetConstraintRHS(row, (seg2 - firstActivationSegment + 1) * segmentDuration - effectCooldown - buffer);
                // make sure it actually starts in this segment
                // count[seg1] + count[seg1 + 1] >= min(segmentDuration, effectDuration)
                row = branchlp.AddConstraint(false);
                SetCooldownElements(branchlp, row, effect, firstActivationSegment, firstActivationSegment + 1, 1.0);
                branchlp.SetConstraintRHS(row, 2 * segmentDuration);
                branchlp.SetConstraintLHS(row, Math.Min(effectDuration, segmentDuration));
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
            for (int index = 0; index < calculationResult.SolutionVariable.Count; index++)
            {
                CastingState state = calculationResult.SolutionVariable[index].State;
                if (state != null && state.GetCooldown(cooldown))
                {
                    int seg = calculationResult.SolutionVariable[index].Segment;
                    if (seg >= minSegment && seg <= maxSegment) branchlp.SetConstraintElement(row, index, value);
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
            for (int index = 0; index < calculationResult.SolutionVariable.Count; index++)
            {
                if (calculationResult.SolutionVariable[index].Type == variable)
                {
                    int seg = calculationResult.SolutionVariable[index].Segment;
                    if (seg >= minSegment && seg <= maxSegment) branchlp.EraseColumn(index);
                }
            }
        }

        private void DisableCooldown(SolverLP branchlp, Cooldown cooldown, int minSegment, int maxSegment)
        {
            for (int index = 0; index < calculationResult.SolutionVariable.Count; index++)
            {
                CastingState state = calculationResult.SolutionVariable[index].State;
                if (state != null && state.GetCooldown(cooldown))
                {
                    int seg = calculationResult.SolutionVariable[index].Segment;
                    if (seg >= minSegment && seg <= maxSegment) branchlp.EraseColumn(index);
                }
            }
        }

        private bool ValidateCooldown(Cooldown cooldown, double effectDuration, double cooldownDuration)
        {
            return ValidateCooldown(cooldown, effectDuration, cooldownDuration, false, effectDuration);
        }

        private bool ValidateWaterElementalSummon()
        {
            Cooldown cooldown = Cooldown.WaterElemental;
            double effectDuration = calculationResult.WaterElementalDuration;
            double cooldownDuration = calculationResult.WaterElementalCooldown;
            VariableType activation = VariableType.SummonWaterElemental;

            const double eps = 0.00001;
            double[] segCount = new double[segments];
            double[] segActivation = new double[segments];
            for (int outseg = 0; outseg < segments; outseg++)
            {
                double s = 0.0;
                for (int index = segmentColumn[outseg]; index < segmentColumn[outseg + 1]; index++)
                {
                    CastingState state = calculationResult.SolutionVariable[index].State;
                    if (state != null && state.GetCooldown(cooldown))
                    {
                        s += solution[index];
                    }
                }
                segCount[outseg] = s;
            }
            for (int index = 0; index < segmentColumn[0]; index++) // fix if variable ordering changes
            {
                CastingState state = calculationResult.SolutionVariable[index].State;
                if (state != null && state.GetCooldown(cooldown)) segCount[calculationResult.SolutionVariable[index].Segment] += solution[index];
            }
            for (int index = 0; index < segmentColumn[0]; index++) // fix if variable ordering changes
            {
                if (calculationResult.SolutionVariable[index].Type == activation) segActivation[calculationResult.SolutionVariable[index].Segment] += solution[index];
            }
            int mindist = (int)Math.Ceiling(effectDuration / segmentDuration);
            int mindist2 = (int)Math.Floor(effectDuration / segmentDuration);
            int maxdist = (cooldownDuration < 0) ? 3 * segments : ((int)Math.Floor((cooldownDuration - effectDuration) / segmentDuration));
            int maxdist2 = (cooldownDuration < 0) ? 3 * segments : ((int)Math.Floor(cooldownDuration / segmentDuration));

            bool valid = true;

            // special summon validation with coldsnap, without coldsnap use ValidateActivation

            return valid;
        }

        private bool ValidateActivation(Cooldown cooldown, double effectDuration, double cooldownDuration, VariableType activation)
        {
            const double eps = 0.00001;
            double[] segCount = new double[segments];
            double[] segActivation = new double[segments];
            for (int outseg = 0; outseg < segments; outseg++)
            {
                double s = 0.0;
                for (int index = segmentColumn[outseg]; index < segmentColumn[outseg + 1]; index++)
                {
                    CastingState state = calculationResult.SolutionVariable[index].State;
                    if (state != null && state.GetCooldown(cooldown))
                    {
                        s += solution[index];
                    }
                }
                segCount[outseg] = s;
            }
            for (int index = 0; index < segmentColumn[0]; index++) // fix if variable ordering changes
            {
                CastingState state = calculationResult.SolutionVariable[index].State;
                if (state != null && state.GetCooldown(cooldown)) segCount[calculationResult.SolutionVariable[index].Segment] += solution[index];
            }
            for (int index = 0; index < segmentColumn[0]; index++) // fix if variable ordering changes
            {
                if (calculationResult.SolutionVariable[index].Type == activation) segActivation[calculationResult.SolutionVariable[index].Segment] += solution[index];
            }
            int mindist = (int)Math.Ceiling(effectDuration / segmentDuration);
            int mindist2 = (int)Math.Floor(effectDuration / segmentDuration);
            int maxdist = (cooldownDuration < 0) ? 3 * segments : ((int)Math.Floor((cooldownDuration - effectDuration) / segmentDuration));
            int maxdist2 = (cooldownDuration < 0) ? 3 * segments : ((int)Math.Floor(cooldownDuration / segmentDuration));

            bool valid = true;

            for (int seg = 0; seg < segments; seg++)
            {
                if (segActivation[seg] > eps)
                {
                    for (int s = 0; s < segments; s++)
                    {
                        if (Math.Abs(seg - s) <= mindist && s < seg && segCount[s] > eps) valid = false; 
                    }
                    if (!valid)
                    {
                        // can't have effect before activation
                        // either activation is not here or effect is not before it
                        SolverLP cooldownUsed = lp.Clone();
                        // activation not used
                        if (lp.Log != null) lp.Log.AppendLine("Disable activation of " + activation.ToString() + " at " + seg);
                        for (int index = 0; index < segmentColumn[0]; index++) // fix if variable ordering changes
                        {
                            if (calculationResult.SolutionVariable[index].Type == activation && calculationResult.SolutionVariable[index].Segment == seg) lp.EraseColumn(index);
                        }
                        HeapPush(lp);
                        if (cooldownUsed.Log != null) cooldownUsed.Log.AppendLine("No " + cooldown.ToString() + " before activation at " + seg);
                        for (int s = 0; s < segments; s++)
                        {
                            if (Math.Abs(seg - s) <= mindist && s < seg)
                            {
                                for (int index = segmentColumn[s]; index < segmentColumn[s + 1]; index++)
                                {
                                    CastingState state = calculationResult.SolutionVariable[index].State;
                                    if (state != null && state.GetCooldown(cooldown)) cooldownUsed.EraseColumn(index);
                                }
                            }
                        }
                        for (int index = 0; index < segmentColumn[0]; index++) // fix if variable ordering changes
                        {
                            CastingState state = calculationResult.SolutionVariable[index].State;
                            if (state != null && state.GetCooldown(cooldown))
                            {
                                int outseg = calculationResult.SolutionVariable[index].Segment;
                                if (Math.Abs(seg - outseg) <= mindist && outseg < seg) cooldownUsed.EraseColumn(index);
                            }
                        }
                        HeapPush(cooldownUsed);
                        return false;
                    }
                }
            }

            for (int i = 0; i < segmentColumn[0]; i++) // fix if variable ordering changes
            {
                if (calculationResult.SolutionVariable[i].Type == activation && solution[i] > eps)
                {
                    int seg = calculationResult.SolutionVariable[i].Segment;
                    int seghex = calculationResult.SolutionVariable[i].State.GetHex();
                    // check all cooldowns that link to this drums activation
                    // if any of them is also before this segment we have a cycle if it's not present at activation
                    int linkedHex = 0;
                    for (int s = 0; s < segments; s++)
                    {
                        if (Math.Abs(seg - s) <= mindist)
                        {
                            for (int index = segmentColumn[s]; index < segmentColumn[s + 1]; index++)
                            {
                                CastingState state = calculationResult.SolutionVariable[index].State;
                                if (state != null && state.GetCooldown(cooldown) && solution[index] > eps)
                                {
                                    linkedHex |= state.GetHex();
                                }
                            }
                        }
                    }
                    for (int index = 0; index < segmentColumn[0]; index++) // fix if variable ordering changes
                    {
                        CastingState state = calculationResult.SolutionVariable[index].State;
                        if (Math.Abs(seg - calculationResult.SolutionVariable[index].Segment) <= mindist && state != null && state.GetCooldown(cooldown) && solution[index] > eps)
                        {
                            linkedHex |= state.GetHex();
                        }
                    }

                    int brokenHex = 0;
                    for (int s = 0; s < segments; s++)
                    {
                        if (s == seg - 1)
                        {
                            for (int index = segmentColumn[s]; index < segmentColumn[s + 1]; index++)
                            {
                                CastingState state = calculationResult.SolutionVariable[index].State;
                                if (state != null && solution[index] > eps)
                                {
                                    int h = state.GetHex() & linkedHex & ~seghex;
                                    if (h != 0) brokenHex = h;
                                }
                            }
                        }
                    }
                    for (int index = 0; index < segmentColumn[0]; index++) // fix if variable ordering changes
                    {
                        CastingState state = calculationResult.SolutionVariable[index].State;
                        if (state != null && solution[index] > eps)
                        {
                            int outseg = calculationResult.SolutionVariable[index].Segment;
                            if (outseg == seg - 1)
                            {
                                int h = state.GetHex() & linkedHex & ~seghex;
                                if (h != 0) brokenHex = h;
                            }
                        }
                    }
                    if (brokenHex != 0)
                    {
                        // either we don't have activation that is without broken hex or drums casting with hex or hex before activation
                        SolverLP drumsnohex = lp.Clone();
                        if (drumsnohex.Log != null) drumsnohex.Log.AppendLine(cooldown.ToString() + " without hex");
                        for (int s = 0; s < segments; s++)
                        {
                            if (Math.Abs(seg - s) <= mindist)
                            {
                                for (int index = segmentColumn[s]; index < segmentColumn[s + 1]; index++)
                                {
                                    CastingState state = calculationResult.SolutionVariable[index].State;
                                    if (state != null && state.GetCooldown(cooldown) && (state.GetHex() & brokenHex) != 0) drumsnohex.EraseColumn(index);
                                }
                            }
                        }
                        HeapPush(drumsnohex);

                        SolverLP nohex = lp.Clone();
                        if (nohex.Log != null) nohex.Log.AppendLine("No hex");
                        for (int s = 0; s < segments; s++)
                        {
                            if (s == seg - 1)
                            {
                                for (int index = segmentColumn[s]; index < segmentColumn[s + 1]; index++)
                                {
                                    CastingState state = calculationResult.SolutionVariable[index].State;
                                    if (state != null && (state.GetHex() & brokenHex) != 0) nohex.EraseColumn(index);
                                }
                            }
                        }
                        HeapPush(nohex);

                        if (lp.Log != null) lp.Log.AppendLine("Disable activation of " + activation.ToString() + " at " + seg + " without hex");
                        for (int index = 0; index < segmentColumn[0]; index++) // fix if variable ordering changes
                        {
                            if (calculationResult.SolutionVariable[index].Type == activation && calculationResult.SolutionVariable[index].Segment == seg && (calculationResult.SolutionVariable[index].State.GetHex() & brokenHex) != brokenHex) lp.EraseColumn(index);
                        }
                        HeapPush(lp);
                        return false;
                    }

                    // another class of activation cycles is if drums link into next segment, this
                    // causes drums to be full from activation to the end of segment so anything in this or previous
                    // segment that links to some of the drums has to cross the activation and so has to be present
                    // in the activation

                    // first check if drums are present in next segment                    
                    if (seg < segments - 1)
                    {
                        bool drumsinnext = false;
                        int drumshex = 1 << 6;
                        for (int j = 0; j < hexList[seg + 1].Count; j++)
                        {
                            if ((hexList[seg + 1][j] & drumshex) != 0)
                            {
                                drumsinnext = true;
                                break;
                            }
                        }
                        if (drumsinnext)
                        {
                            // check what links to drums, we already have all that is active during drums in linkedHex
                            for (int ss = Math.Max(0, seg - 1); ss <= seg; ss++)
                            {
                                for (int j = 0; j < hexList[ss].Count; j++)
                                {
                                    int h = hexList[ss][j];
                                    if ((h & linkedHex) != 0 && (h & drumshex) == 0 && (h & linkedHex & ~seghex) != 0)
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
                                        for (int s = 0; s < segments; s++)
                                        {
                                            if (s >= seg - 1 && s <= seg)
                                            {
                                                for (int index = segmentColumn[s]; index < segmentColumn[s + 1]; index++)
                                                {
                                                    CastingState state = calculationResult.SolutionVariable[index].State;
                                                    if (state != null && !state.GetCooldown(cooldown) && (state.GetHex() & ind) != 0) drumsnohex.EraseColumn(index);
                                                }
                                            }
                                        }
                                        HeapPush(drumsnohex);

                                        // no cooldown with drums
                                        drumsnohex = lp.Clone();
                                        if (drumsnohex.Log != null) drumsnohex.Log.AppendLine("Disable " + ind + " with drums in " + seg);
                                        for (int s = 0; s < segments; s++)
                                        {
                                            if (Math.Abs(seg - s) <= mindist)
                                            {
                                                for (int index = segmentColumn[s]; index < segmentColumn[s + 1]; index++)
                                                {
                                                    CastingState state = calculationResult.SolutionVariable[index].State;
                                                    if (state != null && state.GetCooldown(cooldown) && (state.GetHex() & ind) != 0) drumsnohex.EraseColumn(index);
                                                }
                                            }
                                        }
                                        HeapPush(drumsnohex);

                                        // drums don't extend to next segment
                                        drumsnohex = lp.Clone();
                                        if (drumsnohex.Log != null) drumsnohex.Log.AppendLine("Disable drums in next segment " + (seg + 1));
                                        for (int s = 0; s < segments; s++)
                                        {
                                            if (s == seg + 1)
                                            {
                                                for (int index = segmentColumn[s]; index < segmentColumn[s + 1]; index++)
                                                {
                                                    CastingState state = calculationResult.SolutionVariable[index].State;
                                                    if (state != null && state.GetCooldown(cooldown)) drumsnohex.EraseColumn(index);
                                                }
                                            }
                                        }
                                        HeapPush(drumsnohex);

                                        // activation has this cooldown
                                        if (lp.Log != null) lp.Log.AppendLine("Disable activation of " + activation.ToString() + " at " + seg + " without hex");
                                        for (int index = 0; index < segmentColumn[0]; index++) // fix if variable ordering changes
                                        {
                                            if (calculationResult.SolutionVariable[index].Type == activation && calculationResult.SolutionVariable[index].Segment == seg && (calculationResult.SolutionVariable[index].State.GetHex() & ind) == 0) lp.EraseColumn(index);
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

        private int GetSegmentedCooldownRow(Cooldown cooldown, int minSegment, int maxSegment)
        {
            if (minSegment < 0) minSegment = 0;
            if (maxSegment > segments - 1) maxSegment = segments - 1;
            switch (cooldown)
            {
                case Cooldown.ArcanePower:
                    for (int ss = 0; ss < segments; ss++)
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
        }

        private bool ValidateCooldown(Cooldown cooldown, double effectDuration, double cooldownDuration, bool needsFullEffect, double fullEffectDuration)
        {
            const double eps = 0.00001;
            double[] segCount = new double[segments];
            for (int outseg = 0; outseg < segments; outseg++)
            {
                double s = 0.0;
                for (int index = segmentColumn[outseg]; index < segmentColumn[outseg + 1]; index++)
                {
                    CastingState state = calculationResult.SolutionVariable[index].State;
                    if (state != null && state.GetCooldown(cooldown))
                    {
                        s += solution[index];
                    }
                }
                segCount[outseg] = s;
            }
            for (int index = 0; index < segmentColumn[0]; index++) // fix if variable ordering changes
            {
                CastingState state = calculationResult.SolutionVariable[index].State;
                if (state != null && state.GetCooldown(cooldown)) segCount[calculationResult.SolutionVariable[index].Segment] += solution[index];
            }
            int mindist = (int)Math.Ceiling(effectDuration / segmentDuration);
            int mindist2 = (int)Math.Floor(effectDuration / segmentDuration);
            int maxdist = (cooldownDuration < 0) ? 3 * segments : ((int)Math.Floor((cooldownDuration - effectDuration) / segmentDuration));
            int maxdist2 = (cooldownDuration < 0) ? 3 * segments : ((int)Math.Floor(cooldownDuration / segmentDuration));

            bool valid = true;

            if (needsFullEffect)
            {
                for (int seg = 0; seg < segments; seg++)
                {
                    if (segCount[seg] > 0.0 && (seg + 1) * segmentDuration + effectDuration < calculationOptions.FightDuration)
                    {
                        double total = 0.0;
                        double localtotal = 0.0;
                        for (int s = 0; s < segments; s++)
                        {
                            if (Math.Abs(seg - s) <= mindist) total += segCount[s];
                            if (Math.Abs(seg - s) <= 1) localtotal += segCount[s];
                        }
                        if (localtotal > fullEffectDuration + eps && total < effectDuration - eps)
                        {
                            // there might be other similar cases, but I'm afraid to overrestrict
                            // needs some heavy evaluation to determine when it is safe to restrict
                            // two branches, either force to full full total or restrict to just one
                            SolverLP cooldownUsed = lp.Clone();
                            if (cooldownUsed.Log != null) cooldownUsed.Log.AppendLine("Force total full " + cooldown.ToString() + " at " + seg);
                            int row = cooldownUsed.AddConstraint(false);
                            for (int s = 0; s < segments; s++)
                            {
                                if (Math.Abs(seg - s) <= mindist)
                                {
                                    for (int index = segmentColumn[s]; index < segmentColumn[s + 1]; index++)
                                    {
                                        CastingState state = calculationResult.SolutionVariable[index].State;
                                        if (state != null)
                                        {
                                            if (state.GetCooldown(cooldown)) cooldownUsed.SetConstraintElement(row, index, -1.0);
                                        }
                                    }
                                }
                            }
                            for (int index = 0; index < segmentColumn[0]; index++) // fix if variable ordering changes
                            {
                                CastingState state = calculationResult.SolutionVariable[index].State;
                                if (state != null && state.GetCooldown(cooldown))
                                {
                                    int outseg = calculationResult.SolutionVariable[index].Segment;
                                    if (Math.Abs(seg - outseg) <= mindist) cooldownUsed.SetConstraintElement(row, index, -1.0);
                                }
                            }
                            cooldownUsed.SetConstraintRHS(row, -effectDuration);
                            cooldownUsed.ForceRecalculation(true);
                            HeapPush(cooldownUsed);
                            cooldownUsed = lp.Clone();
                            if (cooldownUsed.Log != null) cooldownUsed.Log.AppendLine("Limit total full " + cooldown.ToString() + " at " + seg);
                            row = cooldownUsed.AddConstraint(false);
                            for (int s = 0; s < segments; s++)
                            {
                                if (Math.Abs(seg - s) <= mindist)
                                {
                                    for (int index = segmentColumn[s]; index < segmentColumn[s + 1]; index++)
                                    {
                                        CastingState state = calculationResult.SolutionVariable[index].State;
                                        if (state != null)
                                        {
                                            if (state.GetCooldown(cooldown)) cooldownUsed.SetConstraintElement(row, index, 1.0);
                                        }
                                    }
                                }
                            }
                            for (int index = 0; index < segmentColumn[0]; index++) // fix if variable ordering changes
                            {
                                CastingState state = calculationResult.SolutionVariable[index].State;
                                if (state != null && state.GetCooldown(cooldown))
                                {
                                    int outseg = calculationResult.SolutionVariable[index].Segment;
                                    if (Math.Abs(seg - outseg) <= mindist) cooldownUsed.SetConstraintElement(row, index, 1.0);
                                }
                            }
                            cooldownUsed.SetConstraintRHS(row, fullEffectDuration);
                            cooldownUsed.ForceRecalculation(true);
                            HeapPush(cooldownUsed);
                            return false;
                        }
                        if (total < fullEffectDuration - eps)
                        {
                            SolverLP cooldownUsed = lp.Clone();
                            if (cooldownUsed.Log != null) cooldownUsed.Log.AppendLine("Force full " + cooldown.ToString() + " at " + seg);
                            int row = cooldownUsed.AddConstraint(false);
                            for (int s = 0; s < segments; s++)
                            {
                                if (Math.Abs(seg - s) <= mindist)
                                {
                                    for (int index = segmentColumn[s]; index < segmentColumn[s + 1]; index++)
                                    {
                                        CastingState state = calculationResult.SolutionVariable[index].State;
                                        if (state != null)
                                        {
                                            if (state.GetCooldown(cooldown)) cooldownUsed.SetConstraintElement(row, index, -1.0);
                                        }
                                    }
                                }
                            }
                            for (int index = 0; index < segmentColumn[0]; index++) // fix if variable ordering changes
                            {
                                CastingState state = calculationResult.SolutionVariable[index].State;
                                if (state != null && state.GetCooldown(cooldown))
                                {
                                    int outseg = calculationResult.SolutionVariable[index].Segment;
                                    if (Math.Abs(seg - outseg) <= mindist) cooldownUsed.SetConstraintElement(row, index, -1.0);
                                }
                                if (cooldown == Cooldown.FlameCap && integralMana)
                                {
                                    // if we push flame cap to full in this segment then we can further restrict mana gems to eliminate unnecessary MIP branches
                                    if (calculationResult.SolutionVariable[index].Type == VariableType.ManaGem)
                                    {
                                        int gemdist = (int)Math.Floor(120.0 / segmentDuration);
                                        int outseg = calculationResult.SolutionVariable[index].Segment;
                                        if (Math.Abs(outseg - seg) < gemdist) cooldownUsed.EraseColumn(index);
                                    }
                                }
                            }
                            cooldownUsed.SetConstraintRHS(row, -fullEffectDuration);
                            cooldownUsed.ForceRecalculation(true);
                            HeapPush(cooldownUsed);
                            // cooldown not used
                            if (lp.Log != null) lp.Log.AppendLine("Disable " + cooldown.ToString() + " at " + seg);
                            for (int index = segmentColumn[seg]; index < segmentColumn[seg + 1]; index++)
                            {
                                CastingState state = calculationResult.SolutionVariable[index].State;
                                if (state != null && state.GetCooldown(cooldown)) lp.EraseColumn(index);
                            }
                            for (int index = 0; index < segmentColumn[0]; index++) // fix if variable ordering changes
                            {
                                CastingState state = calculationResult.SolutionVariable[index].State;
                                if (state != null && state.GetCooldown(cooldown) && calculationResult.SolutionVariable[index].Segment == seg) lp.EraseColumn(index);
                            }
                            HeapPush(lp);
                            return false;
                        }
                    }
                }
            }

            for (int seg = 0; seg < segments; seg++)
            {
                double inseg = segCount[seg];
                double mxdist = (seg == 0 ? maxdist2 : maxdist); // effect started in first segment couldn't be started earlier
                if (inseg > 0)
                {
                    // verify that outside duration segments are 0
                    for (int outseg = 0; outseg < segments; outseg++)
                    {
                        if (Math.Abs(outseg - seg) > mindist && Math.Abs(outseg - seg) < mxdist)
                        {
                            if (segCount[outseg] > 0)
                            {
                                valid = false;
                                break;
                            }
                        }
                    }
                    if (!valid)
                    {
                        //if (lp.disabledHex == null) lp.disabledHex = new int[CooldownMax];
                        // branch on whether cooldown is used in this segment
                        SolverLP cooldownUsed = lp.Clone();
                        // cooldown used
                        if (cooldownUsed.Log != null) cooldownUsed.Log.AppendLine("Use " + cooldown.ToString() + " at " + seg + ", disable around");
                        for (int outseg = 0; outseg < segments; outseg++)
                        {
                            if (Math.Abs(outseg - seg) > mindist && Math.Abs(outseg - seg) < mxdist)
                            {
                                //cooldownUsed.IVHash += 1 << outseg;
                                for (int index = segmentColumn[outseg]; index < segmentColumn[outseg + 1]; index++)
                                {
                                    CastingState state = calculationResult.SolutionVariable[index].State;
                                    if (state != null && state.GetCooldown(cooldown)) cooldownUsed.EraseColumn(index);
                                }
                            }
                        }
                        for (int index = 0; index < segmentColumn[0]; index++) // fix if variable ordering changes
                        {
                            CastingState state = calculationResult.SolutionVariable[index].State;
                            if (state != null && state.GetCooldown(cooldown))
                            {
                                int outseg = calculationResult.SolutionVariable[index].Segment;
                                if (Math.Abs(outseg - seg) > mindist && Math.Abs(outseg - seg) < mxdist) cooldownUsed.EraseColumn(index);
                            }
                        }
                        HeapPush(cooldownUsed);
                        // cooldown not used
                        //lp.IVHash += 1 << seg;
                        if (lp.Log != null) lp.Log.AppendLine("Disable " + cooldown.ToString() + " at " + seg);
                        for (int index = segmentColumn[seg]; index < segmentColumn[seg + 1]; index++)
                        {
                            CastingState state = calculationResult.SolutionVariable[index].State;
                            if (state != null && state.GetCooldown(cooldown)) lp.EraseColumn(index);
                        }
                        for (int index = 0; index < segmentColumn[0]; index++) // fix if variable ordering changes
                        {
                            CastingState state = calculationResult.SolutionVariable[index].State;
                            if (state != null && state.GetCooldown(cooldown) && calculationResult.SolutionVariable[index].Segment == seg) lp.EraseColumn(index);
                        }
                        HeapPush(lp);
                        return false;
                    }
                }
            }

            // detect holes
            for (int seg = 0; seg < segments; seg++)
            {
                double inseg = segCount[seg];
                if (inseg > 0)
                {
                    int lastseg = -1;
                    for (int outseg = seg + 1; outseg < Math.Min(segments, seg + mindist + 1); outseg++)
                    {
                        if (segCount[outseg] > 0) lastseg = outseg;
                    }
                    if (lastseg != -1)
                    {
                        for (int outseg = seg + 1; outseg < lastseg; outseg++)
                        {
                            if (segCount[outseg] < segmentDuration - eps) valid = false;
                        }
                        if (!valid) // coldsnapped icy veins doesn't have to be contiguous, but getting better results assuming it is
                        {
                            // either seg must be disabled, lastseg disabled, or middle to max
                            SolverLP leftDisabled = lp.Clone();
                            if (leftDisabled.Log != null) leftDisabled.Log.AppendLine("Disable left " + cooldown.ToString() + " at " + seg);
                            for (int outseg = 0; outseg < segments; outseg++)
                            {
                                if (outseg == seg)
                                {
                                    for (int index = segmentColumn[outseg]; index < segmentColumn[outseg + 1]; index++)
                                    {
                                        CastingState state = calculationResult.SolutionVariable[index].State;
                                        if (state != null && state.GetCooldown(cooldown)) leftDisabled.EraseColumn(index);
                                    }
                                }
                            }
                            for (int index = 0; index < segmentColumn[0]; index++) // fix if variable ordering changes
                            {
                                CastingState state = calculationResult.SolutionVariable[index].State;
                                if (state != null && state.GetCooldown(cooldown))
                                {
                                    int outseg = calculationResult.SolutionVariable[index].Segment;
                                    if (outseg == seg) leftDisabled.EraseColumn(index);
                                }
                            }
                            HeapPush(leftDisabled);
                            SolverLP rightDisabled = lp.Clone();
                            if (rightDisabled.Log != null) rightDisabled.Log.AppendLine("Disable right " + cooldown.ToString() + " at " + lastseg);
                            for (int outseg = 0; outseg < segments; outseg++)
                            {
                                if (outseg == lastseg)
                                {
                                    for (int index = segmentColumn[outseg]; index < segmentColumn[outseg + 1]; index++)
                                    {
                                        CastingState state = calculationResult.SolutionVariable[index].State;
                                        if (state != null && state.GetCooldown(cooldown)) rightDisabled.EraseColumn(index);
                                    }
                                }
                            }
                            for (int index = 0; index < segmentColumn[0]; index++) // fix if variable ordering changes
                            {
                                CastingState state = calculationResult.SolutionVariable[index].State;
                                if (state != null && state.GetCooldown(cooldown))
                                {
                                    int outseg = calculationResult.SolutionVariable[index].Segment;
                                    if (outseg == lastseg) rightDisabled.EraseColumn(index);
                                }
                            }
                            HeapPush(rightDisabled);
                            if (lp.Log != null) lp.Log.AppendLine("Force " + cooldown.ToString() + " to max from " + seg + " to " + lastseg);
                            int row = lp.AddConstraint(false); // need the extra constraint because just removing others won't force this one to full 30 sec
                            for (int outseg = 0; outseg < segments; outseg++)
                            {
                                if (outseg > seg && outseg < lastseg)
                                {
                                    for (int index = segmentColumn[outseg]; index < segmentColumn[outseg + 1]; index++)
                                    {
                                        CastingState state = calculationResult.SolutionVariable[index].State;
                                        if (state != null && state.GetCooldown(cooldown)) lp.SetConstraintElement(row, index, -1.0);
                                        if (state != null && !state.GetCooldown(cooldown)) lp.EraseColumn(index);
                                    }
                                }
                            }
                            for (int index = 0; index < segmentColumn[0]; index++) // fix if variable ordering changes
                            {
                                CastingState state = calculationResult.SolutionVariable[index].State;
                                int outseg = calculationResult.SolutionVariable[index].Segment;
                                if (outseg > seg && outseg < lastseg)
                                {
                                    if (state != null && state.GetCooldown(cooldown)) lp.SetConstraintElement(row, index, -1.0);
                                    if (state != null && !state.GetCooldown(cooldown)) lp.EraseColumn(index);
                                }
                            }
                            lp.SetConstraintRHS(row, -segmentDuration * (lastseg - seg - 1));
                            lp.ForceRecalculation(true);
                            HeapPush(lp);
                            return false;
                        }
                    }
                }
            }

            // for irregular cooldown durations have to make a special pass verifying everything is in order
            // if cooldowns are broken need to add special constraints that ensure cooldowns are respected
            // do this only for effects that can't be coldsnapped as those don't have to respect cooldown always and are handled separately

            if (!coldsnapAvailable || (cooldown != Cooldown.WaterElemental && cooldown != Cooldown.IcyVeins))
            {
                double lastStart = double.NegativeInfinity;
                double effectCooldown = 0.0;
                bool effectActive = false;
                for (int seg = 0; seg < segments; seg++)
                {
                    if (!effectActive && segCount[seg] > eps)
                    {
                        if (effectCooldown + segCount[seg] > segmentDuration + eps)
                        {
                            valid = false;
                            break;
                        }
                        else
                        {
                            // start as soon as possible
                            double activation = Math.Max(seg * segmentDuration + effectCooldown, seg * segmentDuration);
                            if (seg + 1 < segments && segCount[seg + 1] > 0) activation = (seg + 1) * segmentDuration - segCount[seg];
                            effectCooldown = cooldownDuration + activation - seg * segmentDuration;
                            effectActive = true;
                            lastStart = activation;
                        }
                    }
                    if (effectActive)
                    {
                        if (segCount[seg] > 0.0)
                        {
                            if (seg * segmentDuration + segCount[seg] > lastStart + effectDuration + eps)
                            {
                                valid = false;
                                break;
                            }
                        }
                        else
                        {
                            effectActive = false;
                        }
                    }
                    effectCooldown -= segmentDuration;
                }
                if (!valid)
                {
                    EnforceEffectCooldown(lastStart, cooldown, cooldownDuration, effectDuration);
                    return false;
                }
            }

            return valid;
        }

        private bool ValidateFlamecap()
        {
            Cooldown cooldown = Cooldown.FlameCap;
            double effectDuration = 60.0;
            double cooldownDuration = 180.0;
            const double eps = 0.00001;
            double[] segCount = new double[segments];
            for (int outseg = 0; outseg < segments; outseg++)
            {
                double s = 0.0;
                for (int index = segmentColumn[outseg]; index < segmentColumn[outseg + 1]; index++)
                {
                    CastingState state = calculationResult.SolutionVariable[index].State;
                    if (state != null && state.GetCooldown(cooldown))
                    {
                        s += solution[index];
                    }
                }
                segCount[outseg] = s;
            }
            for (int index = 0; index < segmentColumn[0]; index++) // fix if variable ordering changes
            {
                CastingState state = calculationResult.SolutionVariable[index].State;
                if (state != null && state.GetCooldown(cooldown)) segCount[calculationResult.SolutionVariable[index].Segment] += solution[index];
            }
            int mindist = (int)Math.Ceiling(effectDuration / segmentDuration);
            int mindist2 = (int)Math.Floor(effectDuration / segmentDuration);
            int maxdist = (cooldownDuration < 0) ? 3 * segments : ((int)Math.Floor((cooldownDuration - effectDuration) / segmentDuration));
            int maxdist2 = (cooldownDuration < 0) ? 3 * segments : ((int)Math.Floor(cooldownDuration / segmentDuration));

            bool valid = true;

            double[] manaGem = new double[segments];
            for (int index = 0; index < calculationResult.SolutionVariable.Count; index++)
            {
                if (calculationResult.SolutionVariable[index].Type == VariableType.ManaGem)
                {
                    manaGem[calculationResult.SolutionVariable[index].Segment] += solution[index];
                }
            }

            float manaBurn = 80;
            if (calculationOptions.AoeDuration > 0)
            {
                Spell s = calculationResult.BaseState.GetSpell(SpellId.ArcaneExplosion);
                manaBurn = s.CostPerSecond - s.ManaRegenPerSecond;
            }
            else if (character.MageTalents.EmpoweredFireball > 0)
            {
                Spell s = calculationResult.BaseState.GetSpell(SpellId.Fireball);
                manaBurn = s.CostPerSecond - s.ManaRegenPerSecond;
            }
            else if (character.MageTalents.EmpoweredFrostbolt > 0)
            {
                Spell s = calculationResult.BaseState.GetSpell(SpellId.Frostbolt);
                manaBurn = s.CostPerSecond - s.ManaRegenPerSecond;
            }
            else if (character.MageTalents.SpellPower > 0)
            {
                Spell s = calculationResult.BaseState.GetSpell(SpellId.ArcaneBlast33);
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
                for (firstSeg = 0; firstSeg < segments; firstSeg++)
                {
                    if (segCount[firstSeg] > 0) break;
                }
                if (firstSeg < segments)
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

                    if (120.0 * totalGem - overflow / manaBurn + segCount[firstSeg] > (firstSeg * segmentDuration + segmentDuration) - calculationResult.ManaGemValue * (1 + calculationResult.BaseStats.BonusManaGem) / manaBurn + eps)
                    {
                        // no gem
                        SolverLP nogem = lp.Clone();
                        if (nogem.Log != null) nogem.Log.AppendLine("No gem at 0");
                        for (int index = 0; index < calculationResult.SolutionVariable.Count; index++)
                        {
                            if (calculationResult.SolutionVariable[index].Type == VariableType.ManaGem && calculationResult.SolutionVariable[index].Segment == 0)
                            {
                                nogem.EraseColumn(index);
                            }
                        }
                        HeapPush(nogem);
                        // restrict flame cap/overflow
                        if (lp.Log != null) lp.Log.AppendLine("Restrict flame cap with gem at 0");
                        int row = lp.AddConstraint(false);
                        lp.SetConstraintRHS(row, (firstSeg * segmentDuration + segmentDuration) - calculationResult.ManaGemValue * (1 + calculationResult.BaseStats.BonusManaGem) / manaBurn);
                        for (int index = 0; index < calculationResult.SolutionVariable.Count; index++)
                        {
                            CastingState state = calculationResult.SolutionVariable[index].State;
                            if (calculationResult.SolutionVariable[index].Type == VariableType.ManaGem && calculationResult.SolutionVariable[index].Segment < firstSeg) lp.SetConstraintElement(row, index, 120.0);
                            else if (calculationResult.SolutionVariable[index].Type == VariableType.ManaOverflow && calculationResult.SolutionVariable[index].Segment == 0) lp.SetConstraintElement(row, index, -1.0 / manaBurn);
                            else if (state != null && state.FlameCap && calculationResult.SolutionVariable[index].Segment == firstSeg) lp.SetConstraintElement(row, index, 1.0);
                        }
                        lp.ForceRecalculation(true);
                        HeapPush(lp);
                        return false;
                    }
                }
            }

            if (manaGem[segments - 1] > 0)
            {
                // either no gem or make sure it starts early enough
                int lastSeg;
                for (lastSeg = segments - 1; lastSeg >= 0; lastSeg--)
                {
                    if (segCount[lastSeg] > 0) break;
                }
                if (lastSeg >= 0)
                {
                    while (lastSeg > 0 && segCount[lastSeg - 1] > 0) lastSeg--;
                    double totalGem = 0.0;
                    for (int seg = lastSeg + 1; seg < segments; seg++)
                    {
                        totalGem += manaGem[seg];
                    }
                    // tfc = lastSeg * 30 + 30 - segCount[lastSeg]
                    // tgem >= tfc + 60.0 + 120 * totalGem
                    // overflow >= 2400 - fight * manaBurn + tgem * manaBurn

                    // tfc + 60.0 + 120 * totalGem <= tgem <= overflow / manaBurn - 2400 / manaBurn + fight

                    // tfc + 120 * totalGem - overflow / manaBurn <= - 2400 / manaBurn + fight - 60.0
                    // 120 * totalGem - overflow / manaBurn - segCount[lastSeg] <= fight - 90.0 - lastSeg * 30 - 2400 / manaBurn

                    double overflow = solution[calculationResult.ColumnManaOverflow + segments - 1];

                    if (120.0 * totalGem - overflow / manaBurn - segCount[lastSeg] > (calculationOptions.FightDuration - 60.0 - lastSeg * segmentDuration - segmentDuration) - calculationResult.ManaGemValue * (1 + calculationResult.BaseStats.BonusManaGem) / manaBurn + eps)
                    {
                        // no gem
                        SolverLP nogem = lp.Clone();
                        if (nogem.Log != null) nogem.Log.AppendLine("No gem at end");
                        for (int index = 0; index < calculationResult.SolutionVariable.Count; index++)
                        {
                            if (calculationResult.SolutionVariable[index].Type == VariableType.ManaGem && calculationResult.SolutionVariable[index].Segment == segments - 1)
                            {
                                nogem.EraseColumn(index);
                            }
                        }
                        HeapPush(nogem);
                        // restrict flame cap/overflow
                        if (lp.Log != null) lp.Log.AppendLine("Restrict flame cap with gem at end");
                        int row = lp.AddConstraint(false);
                        lp.SetConstraintRHS(row, (calculationOptions.FightDuration - 60.0 - lastSeg * segmentDuration - segmentDuration) - calculationResult.ManaGemValue * (1 + calculationResult.BaseStats.BonusManaGem) / manaBurn);
                        for (int index = 0; index < calculationResult.SolutionVariable.Count; index++)
                        {
                            CastingState state = calculationResult.SolutionVariable[index].State;
                            if (calculationResult.SolutionVariable[index].Type == VariableType.ManaGem && calculationResult.SolutionVariable[index].Segment > lastSeg) lp.SetConstraintElement(row, index, 120.0);
                            else if (calculationResult.SolutionVariable[index].Type == VariableType.ManaOverflow && calculationResult.SolutionVariable[index].Segment == segments - 1) lp.SetConstraintElement(row, index, -1.0 / manaBurn);
                            else if (state != null && state.FlameCap && calculationResult.SolutionVariable[index].Segment == lastSeg) lp.SetConstraintElement(row, index, -1.0);
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
            int segdist = (int)Math.Ceiling(120.0 / segmentDuration);
            for (int seg = 0; seg < segments; seg++)
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
                        for (int index = 0; index < calculationResult.SolutionVariable.Count; index++)
                        {
                            if (calculationResult.SolutionVariable[index].Type == VariableType.ManaGem && calculationResult.SolutionVariable[index].Segment == seg)
                            {
                                nogem.EraseColumn(index);
                            }
                        }
                        HeapPush(nogem);
                        // restrict flame cap
                        if (lp.Log != null) lp.Log.AppendLine("Restrict flame cap around " + seg);
                        int row = lp.AddConstraint(false);
                        lp.SetConstraintRHS(row, maxfc);
                        for (int index = 0; index < calculationResult.SolutionVariable.Count; index++)
                        {
                            CastingState state = calculationResult.SolutionVariable[index].State;
                            int iseg = calculationResult.SolutionVariable[index].Segment;
                            if (state != null && state.GetCooldown(cooldown) && Math.Abs(seg - iseg) <= segdist) lp.SetConstraintElement(row, index, 1.0);
                        }
                        lp.ForceRecalculation(true);
                        HeapPush(lp);
                        return false;
                    }
                }
            }
            return valid;
        }

        private bool ValidateSCB(Cooldown trinket)
        {
            const double eps = 0.00001;
            double[] trinketCount = new double[segments];
            for (int outseg = 0; outseg < segments; outseg++)
            {
                double s = 0.0;
                for (int index = segmentColumn[outseg]; index < segmentColumn[outseg + 1]; index++)
                {
                    CastingState state = calculationResult.SolutionVariable[index].State;
                    if (state != null && state.GetCooldown(trinket))
                    {
                        s += solution[index];
                    }
                }
                trinketCount[outseg] = s;
            }
            double[] manaGem = new double[segments];
            for (int index = 0; index < calculationResult.SolutionVariable.Count; index++)
            {
                if (calculationResult.SolutionVariable[index].Type == VariableType.ManaGem)
                {
                    manaGem[calculationResult.SolutionVariable[index].Segment] += solution[index];
                }
            }

            // make sure gem is activated together with SCB
            for (int i = 0; i < segments; i++)
            {
                if (trinketCount[i] > 0.0 && (i == 0 || trinketCount[i - 1] == 0.0))
                {
                    if (manaGem[i] < 1.0 - eps)
                    {
                        // either pop gem
                        SolverLP gem = lp.Clone();
                        if (gem.Log != null) gem.Log.AppendLine("Pop gem with SCB at " + i);
                        for (int index = 0; index < calculationResult.SolutionVariable.Count; index++)
                        {
                            if (calculationResult.SolutionVariable[index].Type == VariableType.ManaGem && calculationResult.SolutionVariable[index].Segment == i)
                            {
                                gem.SetColumnLowerBound(index, 1.0);
                            }
                        }
                        gem.ForceRecalculation(true);
                        HeapPush(gem);
                        // or delay activation
                        if (lp.Log != null) lp.Log.AppendLine("Delay activation of SCB at " + i);
                        for (int index = segmentColumn[i]; index < segmentColumn[i + 1]; index++)
                        {
                            CastingState state = calculationResult.SolutionVariable[index].State;
                            if (state != null && state.GetCooldown(trinket) && calculationResult.SolutionVariable[index].Segment == i)
                            {
                                lp.EraseColumn(index);
                            }
                        }
                        HeapPush(lp);
                        return false;
                    }
                }
            }

            float manaBurn = 80;
            if (calculationOptions.AoeDuration > 0)
            {
                Spell s = calculationResult.BaseState.GetSpell(SpellId.ArcaneExplosion);
                manaBurn = s.CostPerSecond - s.ManaRegenPerSecond;
            }
            else if (character.MageTalents.EmpoweredFireball > 0)
            {
                Spell s = calculationResult.BaseState.GetSpell(SpellId.Fireball);
                manaBurn = s.CostPerSecond - s.ManaRegenPerSecond;
            }
            else if (character.MageTalents.EmpoweredFrostbolt > 0)
            {
                Spell s = calculationResult.BaseState.GetSpell(SpellId.Frostbolt);
                manaBurn = s.CostPerSecond - s.ManaRegenPerSecond;
            }
            else if (character.MageTalents.SpellPower > 0)
            {
                Spell s = calculationResult.BaseState.GetSpell(SpellId.ArcaneBlast33);
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
            if (manaGem[0] > 0 && trinketCount[0] > 0)
            {
                // either no activation at 0 or make sure it starts late enough
                // tgem <= 30 - trinketCount[0]
                // overflow >= 2400 - tgem * manaBurn

                // 30 - trinketCount[0] >= tgem >= (2400 - overflow) / manaBurn

                // (2400 - overflow) / manaBurn <= 30 - trinketCount[0]
                // trinketCount[0] - overflow / manaBurn <= 30 - 2400 / manaBurn

                double overflow = solution[calculationResult.ColumnManaOverflow];

                if (trinketCount[0] - overflow / manaBurn > segmentDuration - calculationResult.ManaGemValue * (1 + calculationResult.BaseStats.BonusManaGem) / manaBurn + eps)
                {
                    // no gem/trinket at 0
                    SolverLP nogem = lp.Clone();
                    if (nogem.Log != null) nogem.Log.AppendLine("No SCB at 0");
                    for (int index = 0; index < calculationResult.SolutionVariable.Count; index++)
                    {
                        if (calculationResult.SolutionVariable[index].Type == VariableType.ManaGem && calculationResult.SolutionVariable[index].Segment == 0)
                        {
                            nogem.EraseColumn(index);
                        }
                        CastingState state = calculationResult.SolutionVariable[index].State;
                        if (state != null && state.GetCooldown(trinket) && calculationResult.SolutionVariable[index].Segment == 0)
                        {
                            nogem.EraseColumn(index);
                        }
                    }
                    HeapPush(nogem);
                    // restrict SCB/overflow
                    if (lp.Log != null) lp.Log.AppendLine("Restrict SCB at 0");
                    int row = lp.AddConstraint(false);
                    lp.SetConstraintRHS(row, segmentDuration - calculationResult.ManaGemValue * (1 + calculationResult.BaseStats.BonusManaGem) / manaBurn);
                    for (int index = 0; index < calculationResult.SolutionVariable.Count; index++)
                    {
                        CastingState state = calculationResult.SolutionVariable[index].State;
                        if (calculationResult.SolutionVariable[index].Type == VariableType.ManaOverflow && calculationResult.SolutionVariable[index].Segment == 0) lp.SetConstraintElement(row, index, -1.0 / manaBurn);
                        else if (state != null && state.GetCooldown(trinket) && calculationResult.SolutionVariable[index].Segment == 0) lp.SetConstraintElement(row, index, 1.0);
                    }
                    lp.ForceRecalculation(true);
                    HeapPush(lp);
                    return false;
                }
            }

            return true;
        }
    }
}
