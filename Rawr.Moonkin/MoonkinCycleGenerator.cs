using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rawr.Base.Algorithms;

namespace Rawr.Moonkin
{
    public class MoonkinCycleAbility
    {
        public string Name { get; set; }
    }

    // Current Eclipse energy and direction, and the current Wrath counter, dictate the next spell to cast
    public class MoonkinState : State<MoonkinCycleAbility>
    {
        // Current energy level
        public int EclipseEnergy { get; set; }
        // -1 for lunar, 1 for solar
        public int EclipseDirection { get; set; }
        // Wrath counter, for fractional energy
        public int WrathCount { get; set; }
        // Starsurge cooldown
        public double StarsurgeCooldown { get; set; }
        // Is Shooting Stars proc active? (reduces SS cast time to GCD)
        public bool IsShootingStarsActive { get; set; }
    }

    public class MoonkinStateSpaceGenerator : StateSpaceGenerator<MoonkinCycleAbility>
    {
        private Dictionary<string, MoonkinState> _stateSpace = new Dictionary<string, MoonkinState>();

        private int StarfireEnergy = 20;
        private int StarfireEnergy4T12 = 25;
        private int WrathEnergyLow = 13;
        private int WrathEnergyHigh = 14;
        private int WrathEnergyLow4T12 = 16;
        private int WrathEnergyHigh4T12 = 17;
        private int StarsurgeEnergy = 15;

        public MoonkinCycleAbility Starfire = new MoonkinCycleAbility { Name = "Starfire" };
        public MoonkinCycleAbility StarfireEclipse = new MoonkinCycleAbility { Name = "Starfire (Eclipse)" };
        public MoonkinCycleAbility Wrath = new MoonkinCycleAbility { Name = "Wrath" };
        public MoonkinCycleAbility WrathEclipse = new MoonkinCycleAbility { Name = "Wrath (Eclipse)" };
        public MoonkinCycleAbility Starsurge = new MoonkinCycleAbility { Name = "Starsurge" };
        public MoonkinCycleAbility StarsurgeEclipse = new MoonkinCycleAbility { Name = "Starsurge (Eclipse)" };
        public MoonkinCycleAbility ShootingStars = new MoonkinCycleAbility { Name = "Shooting Stars" };
        public MoonkinCycleAbility ShootingStarsEclipse = new MoonkinCycleAbility { Name = "Shooting Stars (Eclipse)" };

        // Inputs
        // 4T12 has a big effect on Wrath energy generation
        public bool Has4T12 { get; set; }
        // Euphoria proc chance
        public double EuphoriaChance { get; set; }
        // Rate of dot ticks
        private double DotTickRate = 1;
        // Starlight Wrath, 0-3
        public int StarlightWrathLevel { get; set; }
        // Haste value (for calculating transition time and DoT tick rate)
        public double HasteLevel { get; set; }
        // Chance of dot ticks to reset the Starsurge CD
        public double ShootingStarsChance { get; set; }

        // Calculation results
        // Wrath final cast time
        double wrathCastTime;
        // Wrath combinatorial solutions
        double shootingStarsWrathTransitionProbability;
        double wrathNoEuphoriaWithSSTransitionProbability;
        double wrathNoEuphoriaNoSSTransitionProbability;
        double wrathEuphoriaWithSSTransitionProbability;
        double wrathEuphoriaNoSSTransitionProbability;
        // Starfire final cast time
        double starfireCastTime;
        // Starfire combinatorial solutions
        double shootingStarsStarfireTransitionProbability;
        double starfireNoEuphoriaWithSSTransitionProbability;
        double starfireNoEuphoriaNoSSTransitionProbability;
        double starfireEuphoriaWithSSTransitionProbability;
        double starfireEuphoriaNoSSTransitionProbability;
        // Starsurge final cast time
        double starsurgeCastTime;
        // Starsurge combinatorial solutions
        double shootingStarsStarsurgeTransitionProbability;
        // Global cooldown final duration (used for Shooting Stars transitions)
        double globalCooldown;

        protected override State<MoonkinCycleAbility> GetInitialState()
        {
            // Cast time calculations
            wrathCastTime = 2.5;
            starsurgeCastTime = 2.0;
            starfireCastTime = 3.2;
            globalCooldown = 1.5;
            switch (StarlightWrathLevel)
            {
                case 1:
                    wrathCastTime -= 0.15;
                    starfireCastTime -= 0.15;
                    break;
                case 2:
                    wrathCastTime -= 0.25;
                    starfireCastTime -= 0.25;
                    break;
                case 3:
                    wrathCastTime -= 0.5;
                    starfireCastTime -= 0.5;
                    break;
            }
            wrathCastTime = Math.Max(1, wrathCastTime / (1 + HasteLevel));
            starfireCastTime = Math.Max(1, starfireCastTime / (1 + HasteLevel));
            globalCooldown = Math.Max(1, globalCooldown / (1 + HasteLevel));
            starsurgeCastTime = Math.Max(1, starsurgeCastTime / (1 + HasteLevel));

            DotTickRate = 1 / (1 + HasteLevel);

            // Probability combinations for Wrath, Euphoria, Shooting Stars
            shootingStarsWrathTransitionProbability = 1 - Math.Pow((1 - ShootingStarsChance), (wrathCastTime / DotTickRate));
            wrathNoEuphoriaWithSSTransitionProbability = (1 - EuphoriaChance) * shootingStarsWrathTransitionProbability;
            wrathNoEuphoriaNoSSTransitionProbability = (1 - EuphoriaChance) - wrathNoEuphoriaWithSSTransitionProbability;
            wrathEuphoriaWithSSTransitionProbability = EuphoriaChance * shootingStarsWrathTransitionProbability;
            wrathEuphoriaNoSSTransitionProbability = EuphoriaChance - wrathEuphoriaWithSSTransitionProbability;

            // Probability combinations for Starfire, Euphoria, Shooting Stars
            shootingStarsStarfireTransitionProbability = 1 - Math.Pow((1 - ShootingStarsChance), (starfireCastTime / DotTickRate));
            starfireNoEuphoriaWithSSTransitionProbability = (1 - EuphoriaChance) * shootingStarsStarfireTransitionProbability;
            starfireNoEuphoriaNoSSTransitionProbability = (1 - EuphoriaChance) - starfireNoEuphoriaWithSSTransitionProbability;
            starfireEuphoriaWithSSTransitionProbability = EuphoriaChance * shootingStarsStarfireTransitionProbability;
            starfireEuphoriaNoSSTransitionProbability = EuphoriaChance - starfireEuphoriaWithSSTransitionProbability;

            // Probability of proccing Shooting Stars when hard casting Starsurge
            // Note: Shooting Stars procs that occur when consuming an existing proc will not reset the cooldown until another spell is cast
            shootingStarsStarsurgeTransitionProbability = 1 - Math.Pow((1 - ShootingStarsChance), (starsurgeCastTime / DotTickRate));

            // Bar at middle, we want to initiate Solar first
            return GetState(0, 1, 0, 0, false);
        }

        protected override List<StateTransition<MoonkinCycleAbility>> GetStateTransitions(State<MoonkinCycleAbility> state)
        {
            MoonkinState theState = state as MoonkinState;
            List<StateTransition<MoonkinCycleAbility>> retval = new List<StateTransition<MoonkinCycleAbility>>();
            bool inEclipse = (theState.EclipseDirection > 0 && theState.EclipseEnergy < 0) ||
                             (theState.EclipseDirection < 0 && theState.EclipseEnergy > 0);

            if (theState.StarsurgeCooldown == 0)
            {
                MoonkinCycleAbility usedAbility = (inEclipse ? (theState.IsShootingStarsActive ? ShootingStarsEclipse : StarsurgeEclipse) : (theState.IsShootingStarsActive ? ShootingStars : Starsurge));
                double transitionDuration = (theState.IsShootingStarsActive ? globalCooldown : starsurgeCastTime);
                double shootingStarsProbability = (theState.IsShootingStarsActive ? 0 : shootingStarsStarsurgeTransitionProbability);
                double normalProbability = 1 - shootingStarsProbability;

                // Solar Eclipse
                if (theState.EclipseEnergy == 100)
                {
                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = StarsurgeEclipse, TargetState = GetState(100 - StarsurgeEnergy, -1, theState.WrathCount, 15 - starsurgeCastTime, false), TransitionDuration = transitionDuration, TransitionProbability = normalProbability });
                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = ShootingStarsEclipse, TargetState = GetState(100 - StarsurgeEnergy, -1, theState.WrathCount, 0, true), TransitionDuration = transitionDuration, TransitionProbability = shootingStarsProbability });
                }
                // Lunar Eclipse
                else if (theState.EclipseEnergy == -100)
                {
                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = StarsurgeEclipse, TargetState = GetState(-100 + StarsurgeEnergy, 1, theState.WrathCount, 15 - starsurgeCastTime, false), TransitionDuration = transitionDuration, TransitionProbability = normalProbability });
                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = ShootingStarsEclipse, TargetState = GetState(-100 + StarsurgeEnergy, 1, theState.WrathCount, 0, true), TransitionDuration = transitionDuration, TransitionProbability = shootingStarsProbability });
                }
                // Baseline
                // TODO: If we're 1 cast away from an Eclipse, skip the Starsurge and go into Eclipse instead
                else
                {
                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = usedAbility, TargetState = GetState(theState.EclipseEnergy + theState.EclipseDirection * StarsurgeEnergy, theState.EclipseDirection, theState.WrathCount, 15 - starsurgeCastTime, false), TransitionDuration = transitionDuration, TransitionProbability = normalProbability });
                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = usedAbility, TargetState = GetState(theState.EclipseEnergy + theState.EclipseDirection * StarsurgeEnergy, theState.EclipseDirection, theState.WrathCount, 0, true), TransitionDuration = transitionDuration, TransitionProbability = shootingStarsProbability });
                }
            }
            // Solar Eclipse - start casting Wrath
            else if (theState.EclipseEnergy == 100)
            {
                double transitionTime = wrathCastTime;
                double shootingStarsProbability = shootingStarsWrathTransitionProbability;
                // Wrath energy generation follows a 13/13/14 pattern.
                // Euphoria does not apply since we are not between 0 and -35 energy.
                // If we have 4T12, we are losing the bonus and need to play monkey games with the Wrath counter
                if (Has4T12)
                {
                    switch (theState.WrathCount)
                    {
                        // 17/16 -> 14 (counter at 0)
                        case 0:
                            retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = WrathEclipse, TargetState = GetState(100 - WrathEnergyHigh, -1, 0, theState.StarsurgeCooldown - transitionTime, false), TransitionDuration = transitionTime, TransitionProbability = 1 - shootingStarsProbability });
                            retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = WrathEclipse, TargetState = GetState(100 - WrathEnergyHigh, -1, 0, 0, true), TransitionDuration = transitionTime, TransitionProbability = shootingStarsProbability });
                            break;
                        // 16/17 -> 13 (counter at 2)
                        case 1:
                            retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = WrathEclipse, TargetState = GetState(100 - WrathEnergyLow, -1, 2, theState.StarsurgeCooldown - transitionTime, false), TransitionDuration = transitionTime, TransitionProbability = 1 - shootingStarsProbability });
                            retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = WrathEclipse, TargetState = GetState(100 - WrathEnergyLow, -1, 2, 0, true), TransitionDuration = transitionTime, TransitionProbability = shootingStarsProbability });
                            break;
                        // 17/17 -> 13 (counter at 1)
                        case 2:
                            retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = WrathEclipse, TargetState = GetState(100 - WrathEnergyLow, -1, 1, theState.StarsurgeCooldown - transitionTime, false), TransitionDuration = transitionTime, TransitionProbability = 1 - shootingStarsProbability });
                            retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = WrathEclipse, TargetState = GetState(100 - WrathEnergyLow, -1, 1, 0, true), TransitionDuration = transitionTime, TransitionProbability = shootingStarsProbability });
                            break;
                    }
                }
                else
                {
                    switch (theState.WrathCount)
                    {
                        case 0:
                        case 1:
                            retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = WrathEclipse, TargetState = GetState(100 - WrathEnergyLow, -1, theState.WrathCount + 1, theState.StarsurgeCooldown - transitionTime, false), TransitionDuration = transitionTime, TransitionProbability = 1 - shootingStarsProbability });
                            retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = WrathEclipse, TargetState = GetState(100 - WrathEnergyLow, -1, theState.WrathCount + 1, 0, true), TransitionDuration = transitionTime, TransitionProbability = shootingStarsProbability });
                            break;
                        case 2:
                            retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = WrathEclipse, TargetState = GetState(100 - WrathEnergyHigh, -1, 0, theState.StarsurgeCooldown - transitionTime, false), TransitionDuration = transitionTime, TransitionProbability = 1 - shootingStarsProbability });
                            retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = WrathEclipse, TargetState = GetState(100 - WrathEnergyHigh, -1, 0, 0, true), TransitionDuration = transitionTime, TransitionProbability = shootingStarsProbability });
                            break;
                    }
                }
            }
            // Lunar Eclipse - cast Starfire
            else if (theState.EclipseEnergy == -100)
            {
                double transitionTime = starfireCastTime;
                double shootingStarsProbability = shootingStarsStarfireTransitionProbability;
                // No need for Euphoria since we are inside an Eclipse state
                // No 4T12 since we gained Eclipse
                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = StarfireEclipse, TargetState = GetState(-100 + StarfireEnergy, 1, theState.WrathCount, theState.StarsurgeCooldown - transitionTime, false), TransitionDuration = transitionTime, TransitionProbability = 1 - shootingStarsProbability });
                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = StarfireEclipse, TargetState = GetState(-100 + StarfireEnergy, 1, theState.WrathCount, 0, true), TransitionDuration = transitionTime, TransitionProbability = shootingStarsProbability });
            }
            // Moving toward Lunar Eclipse, cast Wrath
            else if (theState.EclipseDirection == -1)
            {
                double transitionTime = wrathCastTime;
                // Euphoria only procs when we're out of eclipse (past 0 on the bar) and there's less than 35 energy in the direction of the eclipse
                if (theState.EclipseEnergy <= 0 && theState.EclipseEnergy >= -35)
                {
                    double baseProbability = wrathNoEuphoriaNoSSTransitionProbability;
                    double baseEuphoriaProbability = wrathEuphoriaNoSSTransitionProbability;
                    double shootingStarsProbability = wrathNoEuphoriaWithSSTransitionProbability;
                    double shootingStarsEuphoriaProbability = wrathEuphoriaWithSSTransitionProbability;
                    if (Has4T12)
                    {
                        // 4T12 has just come active if the 14 Wrath results in 0 to -13 energy, or a 13 Wrath results in 0 to -12 energy
                        bool bActivated4T12 = (theState.WrathCount == 0 ? theState.EclipseEnergy >= -13 : theState.EclipseEnergy >= -12);
                        // Change the Wrath counter if we drop out of Eclipse (activating the set bonus)
                        if (bActivated4T12)
                        {
                            switch (theState.WrathCount)
                            {
                                // 13/14 -> 16 (counter at 0)
                                case 0:
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyLow4T12, -1, 0, theState.StarsurgeCooldown - transitionTime, false), TransitionDuration = transitionTime, TransitionProbability = baseProbability });
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyLow4T12 - WrathEnergyHigh4T12, -1, 1, theState.StarsurgeCooldown - transitionTime, false), TransitionDuration = transitionTime, TransitionProbability = baseEuphoriaProbability });
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyLow4T12, -1, 0, 0, true), TransitionDuration = transitionTime, TransitionProbability = shootingStarsProbability });
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyLow4T12 - WrathEnergyHigh4T12, -1, 1, 0, true), TransitionDuration = transitionTime, TransitionProbability = shootingStarsEuphoriaProbability });
                                    break;
                                // 14/13 -> 17 (counter at 2)
                                case 1:
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyHigh4T12, -1, 2, theState.StarsurgeCooldown - transitionTime, false), TransitionDuration = transitionTime, TransitionProbability = baseProbability });
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyHigh4T12 - WrathEnergyLow4T12, -1, 0, theState.StarsurgeCooldown - transitionTime, false), TransitionDuration = transitionTime, TransitionProbability = baseEuphoriaProbability });
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyHigh4T12, -1, 2, 0, true), TransitionDuration = transitionTime, TransitionProbability = shootingStarsProbability });
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyHigh4T12 - WrathEnergyLow4T12, -1, 0, 0, true), TransitionDuration = transitionTime, TransitionProbability = shootingStarsEuphoriaProbability });
                                    break;
                                // 13/13 -> 17 (counter at 1)
                                case 2:
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyHigh4T12, -1, 1, theState.StarsurgeCooldown - transitionTime, false), TransitionDuration = transitionTime, TransitionProbability = baseProbability });
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyHigh4T12 - WrathEnergyLow4T12, -1, 2, theState.StarsurgeCooldown - transitionTime, false), TransitionDuration = transitionTime, TransitionProbability = baseEuphoriaProbability });
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyHigh4T12, -1, 1, 0, true), TransitionDuration = transitionTime, TransitionProbability = shootingStarsProbability });
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyHigh4T12 - WrathEnergyLow4T12, -1, 2, 0, true), TransitionDuration = transitionTime, TransitionProbability = shootingStarsEuphoriaProbability });
                                    break;
                            }
                        }
                        else
                        {
                            switch (theState.WrathCount)
                            {
                                case 0:
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyHigh4T12, -1, 1, theState.StarsurgeCooldown - transitionTime, false), TransitionDuration = transitionTime, TransitionProbability = baseProbability });
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyHigh4T12 - WrathEnergyHigh4T12, -1, 2, theState.StarsurgeCooldown - transitionTime, false), TransitionDuration = transitionTime, TransitionProbability = baseEuphoriaProbability });
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyHigh4T12, -1, 1, 0, true), TransitionDuration = transitionTime, TransitionProbability = shootingStarsProbability });
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyHigh4T12 - WrathEnergyHigh4T12, -1, 2, 0, true), TransitionDuration = transitionTime, TransitionProbability = shootingStarsEuphoriaProbability });
                                    break;
                                case 1:
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyHigh4T12, -1, 2, theState.StarsurgeCooldown - transitionTime, false), TransitionDuration = transitionTime, TransitionProbability = baseProbability });
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyHigh4T12 - WrathEnergyLow4T12, -1, 0, theState.StarsurgeCooldown - transitionTime, false), TransitionDuration = transitionTime, TransitionProbability = baseEuphoriaProbability });
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyHigh4T12, -1, 2, 0, true), TransitionDuration = transitionTime, TransitionProbability = shootingStarsProbability });
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyHigh4T12 - WrathEnergyLow4T12, -1, 0, 0, true), TransitionDuration = transitionTime, TransitionProbability = shootingStarsEuphoriaProbability });
                                    break;
                                case 2:
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyLow4T12, -1, 0, theState.StarsurgeCooldown - transitionTime, false), TransitionDuration = transitionTime, TransitionProbability = baseProbability });
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyLow4T12 - WrathEnergyHigh4T12, -1, 1, theState.StarsurgeCooldown - transitionTime, false), TransitionDuration = transitionTime, TransitionProbability = baseEuphoriaProbability });
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyLow4T12, -1, 0, 0, true), TransitionDuration = transitionTime, TransitionProbability = shootingStarsProbability });
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyLow4T12 - WrathEnergyHigh4T12, -1, 1, 0, true), TransitionDuration = transitionTime, TransitionProbability = shootingStarsEuphoriaProbability });
                                    break;
                            }
                        }
                    }
                    else
                    {
                        switch (theState.WrathCount)
                        {
                            case 0:
                                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyLow, -1, 1, theState.StarsurgeCooldown - transitionTime, false), TransitionDuration = transitionTime, TransitionProbability = baseProbability });
                                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - 2 * WrathEnergyLow, -1, 2, theState.StarsurgeCooldown - transitionTime, false), TransitionDuration = transitionTime, TransitionProbability = baseEuphoriaProbability });
                                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyLow, -1, 1, 0, true), TransitionDuration = transitionTime, TransitionProbability = shootingStarsProbability });
                                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - 2 * WrathEnergyLow, -1, 2, 0, true), TransitionDuration = transitionTime, TransitionProbability = shootingStarsEuphoriaProbability });
                                break;
                            case 1:
                                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyLow, -1, 2, theState.StarsurgeCooldown - transitionTime, false), TransitionDuration = transitionTime, TransitionProbability = baseProbability });
                                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyLow - WrathEnergyHigh, -1, 0, theState.StarsurgeCooldown - transitionTime, false), TransitionDuration = transitionTime, TransitionProbability = baseEuphoriaProbability });
                                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyLow, -1, 2, 0, true), TransitionDuration = transitionTime, TransitionProbability = shootingStarsProbability });
                                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyLow - WrathEnergyHigh, -1, 0, 0, true), TransitionDuration = transitionTime, TransitionProbability = shootingStarsEuphoriaProbability });
                                break;
                            case 2:
                                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyHigh, -1, 0, theState.StarsurgeCooldown - transitionTime, false), TransitionDuration = transitionTime, TransitionProbability = baseProbability });
                                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyHigh - WrathEnergyLow, -1, 1, theState.StarsurgeCooldown - transitionTime, false), TransitionDuration = transitionTime, TransitionProbability = baseEuphoriaProbability });
                                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyHigh, -1, 0, 0, true), TransitionDuration = transitionTime, TransitionProbability = shootingStarsProbability });
                                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyHigh - WrathEnergyLow, -1, 1, 0, true), TransitionDuration = transitionTime, TransitionProbability = shootingStarsEuphoriaProbability });
                                break;
                        }
                    }
                }
                else
                {
                    double shootingStarsProbability = shootingStarsWrathTransitionProbability;
                    // Between -35 and -100 energy, 4T12 is active but not Euphoria
                    if (Has4T12 && theState.EclipseEnergy <= 0)
                    {
                        switch (theState.WrathCount)
                        {
                            case 0:
                            case 1:
                                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyHigh4T12, -1, theState.WrathCount + 1, theState.StarsurgeCooldown - transitionTime, false), TransitionDuration = transitionTime, TransitionProbability = 1 - shootingStarsProbability });
                                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyHigh4T12, -1, theState.WrathCount + 1, 0, true), TransitionDuration = transitionTime, TransitionProbability = shootingStarsProbability });
                                break;
                            case 2:
                                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyLow4T12, -1, 0, theState.StarsurgeCooldown - transitionTime, false), TransitionDuration = transitionTime, TransitionProbability = 1 - shootingStarsProbability });
                                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyLow4T12, -1, 0, 0, true), TransitionDuration = transitionTime, TransitionProbability = shootingStarsProbability });
                                break;
                        }
                    }
                    // No special Eclipse modifiers active, revert to plain 13/13/14
                    else
                    {
                        switch (theState.WrathCount)
                        {
                            case 0:
                            case 1:
                                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = (inEclipse ? WrathEclipse : Wrath), TargetState = GetState(theState.EclipseEnergy - WrathEnergyLow, -1, theState.WrathCount + 1, theState.StarsurgeCooldown - transitionTime, false), TransitionDuration = transitionTime, TransitionProbability = 1 - shootingStarsProbability });
                                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = (inEclipse ? WrathEclipse : Wrath), TargetState = GetState(theState.EclipseEnergy - WrathEnergyLow, -1, theState.WrathCount + 1, 0, true), TransitionDuration = transitionTime, TransitionProbability = shootingStarsProbability });
                                break;
                            case 2:
                                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = (inEclipse ? WrathEclipse : Wrath), TargetState = GetState(theState.EclipseEnergy - WrathEnergyHigh, -1, 0, theState.StarsurgeCooldown - transitionTime, false), TransitionDuration = transitionTime, TransitionProbability = 1 - shootingStarsProbability });
                                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = (inEclipse ? WrathEclipse : Wrath), TargetState = GetState(theState.EclipseEnergy - WrathEnergyHigh, -1, 0, 0, true), TransitionDuration = transitionTime, TransitionProbability = shootingStarsProbability });
                                break;
                        }
                    }
                }
            }
            // Moving toward Solar Eclipse, cast Starfire
            else if (theState.EclipseDirection == 1)
            {
                double transitionTime = starfireCastTime;
                // Euphoria only procs when we're out of eclipse (past 0 on the bar) and there's less than 35 energy in the direction of the eclipse
                if (theState.EclipseEnergy >= 0 && theState.EclipseEnergy <= 35)
                {
                    double baseProbability = starfireNoEuphoriaNoSSTransitionProbability;
                    double baseEuphoriaProbability = starfireEuphoriaNoSSTransitionProbability;
                    double shootingStarsProbability = starfireNoEuphoriaWithSSTransitionProbability;
                    double shootingStarsEuphoriaProbability = starfireEuphoriaWithSSTransitionProbability;

                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Starfire, TargetState = GetState(theState.EclipseEnergy + (Has4T12 ? StarfireEnergy4T12 : StarfireEnergy), 1, theState.WrathCount, theState.StarsurgeCooldown - transitionTime, false), TransitionDuration = starfireCastTime, TransitionProbability = baseProbability });
                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Starfire, TargetState = GetState(theState.EclipseEnergy + (Has4T12 ? StarfireEnergy4T12 : StarfireEnergy) + 20, 1, theState.WrathCount, theState.StarsurgeCooldown - transitionTime, false), TransitionDuration = starfireCastTime, TransitionProbability = baseEuphoriaProbability });
                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Starfire, TargetState = GetState(theState.EclipseEnergy + (Has4T12 ? StarfireEnergy4T12 : StarfireEnergy), 1, theState.WrathCount, 0, true), TransitionDuration = starfireCastTime, TransitionProbability = shootingStarsProbability });
                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Starfire, TargetState = GetState(theState.EclipseEnergy + (Has4T12 ? StarfireEnergy4T12 : StarfireEnergy) + 20, 1, theState.WrathCount, 0, true), TransitionDuration = starfireCastTime, TransitionProbability = shootingStarsEuphoriaProbability });
                }
                else
                {
                    double shootingStarsProbability = shootingStarsStarfireTransitionProbability;
                    // If not in Eclipse and 4T12 is equipped, use 4T12 Starfire
                    if (Has4T12 && theState.EclipseEnergy >= 0)
                    {
                        retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Starfire, TargetState = GetState(theState.EclipseEnergy + StarfireEnergy4T12, 1, theState.WrathCount, theState.StarsurgeCooldown - transitionTime, false), TransitionDuration = starfireCastTime, TransitionProbability = 1 - shootingStarsProbability });
                        retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Starfire, TargetState = GetState(theState.EclipseEnergy + StarfireEnergy4T12, 1, theState.WrathCount, 0, true), TransitionDuration = starfireCastTime, TransitionProbability = shootingStarsProbability });
                    }
                    else
                    {
                        retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = (inEclipse ? StarfireEclipse : Starfire), TargetState = GetState(theState.EclipseEnergy + StarfireEnergy, 1, theState.WrathCount, theState.StarsurgeCooldown - transitionTime, false), TransitionDuration = starfireCastTime, TransitionProbability = 1 - shootingStarsProbability });
                        retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = (inEclipse ? StarfireEclipse : Starfire), TargetState = GetState(theState.EclipseEnergy + StarfireEnergy, 1, theState.WrathCount, 0, true), TransitionDuration = starfireCastTime, TransitionProbability = shootingStarsProbability });
                    }
                }
            }

            return retval;
        }

        private MoonkinState GetState(int eclipseEnergy, int eclipseDirection, int wrathCount, double starsurgeCooldown, bool shootingStarsActive)
        {
            // Trim state space by clipping values at their maximums/minimums: Eclipse energy -100 <= x <= 100, SS CD >= 0
            if (eclipseEnergy < -100)
                eclipseEnergy = -100;
            if (eclipseEnergy > 100)
                eclipseEnergy = 100;
            if (starsurgeCooldown < 0)
                starsurgeCooldown = 0;

            string name = String.Format("E{0}D{1}W{2}C{3}S{4}", eclipseEnergy, eclipseDirection, wrathCount, starsurgeCooldown, shootingStarsActive ? 1 : 0);
            MoonkinState state;
            if (!_stateSpace.TryGetValue(name, out state))
            {
                state = new MoonkinState { Name = name, EclipseDirection = eclipseDirection, EclipseEnergy = eclipseEnergy, WrathCount = wrathCount, StarsurgeCooldown = starsurgeCooldown, IsShootingStarsActive = shootingStarsActive };
                _stateSpace.Add(name, state);
            }
            return state;
        }
    }

    public class MoonkinCycleGenerator
    {
        private MarkovProcess<MoonkinCycleAbility> _process;
        private MoonkinStateSpaceGenerator _generator = new MoonkinStateSpaceGenerator();

        public bool Has4T12
        {
            get
            {
                return _generator.Has4T12;
            }
            set
            {
                _generator.Has4T12 = value;
            }
        }

        public double EuphoriaChance
        {
            get
            {
                return _generator.EuphoriaChance;
            }
            set
            {
                _generator.EuphoriaChance = value;
            }
        }

        public double ShootingStarsChance
        {
            get
            {
                return _generator.ShootingStarsChance;
            }
            set
            {
                _generator.ShootingStarsChance = value;
            }
        }

        public int StarlightWrathLevel
        {
            get
            {
                return _generator.StarlightWrathLevel;
            }
            set
            {
                _generator.StarlightWrathLevel = value;
            }
        }

        public double HasteLevel
        {
            get
            {
                return _generator.HasteLevel;
            }
            set
            {
                _generator.HasteLevel = value;
            }
        }

        public double[] GenerateCycle()
        {
            _process = new MarkovProcess<MoonkinCycleAbility>(_generator.GenerateStateSpace());

            double[] retval = new double[8];
            foreach (KeyValuePair<MoonkinCycleAbility, double> kvp in _process.AbilityWeight)
            {
                int idx = Array.IndexOf(MoonkinSolver.CastDistributionSpells, kvp.Key.Name);
                retval[idx] = kvp.Value;
            }

            return retval;
        }

        public double GetRotationLength()
        {
            List<int> solarIndices = new List<int>();
            List<int> lunarIndices = new List<int>();

            double toSolarTotalWeight = 0;
            double toLunarTotalWeight = 0;

            for (int i = 0; i < _process.StateSpace.Count; ++i)
            {
                MoonkinState st = (MoonkinState)_process.StateSpace[i];
                if (st.EclipseEnergy == -100)
                {
                    lunarIndices.Add(i);
                    toSolarTotalWeight += _process.StateWeight[i];
                }
                else if (st.EclipseEnergy == 100)
                {
                    solarIndices.Add(i);
                    toLunarTotalWeight += _process.StateWeight[i];
                }
            }

            double[] toSolarDurations = _process.GetAverageTimeToEnd(st => ((MoonkinState)st).EclipseEnergy == 100);
            double[] toLunarDurations = _process.GetAverageTimeToEnd(st => ((MoonkinState)st).EclipseEnergy == -100);

            double lunarToSolarDuration = 0;
            double solarToLunarDuration = 0;

            foreach (int idx in lunarIndices)
            {
                lunarToSolarDuration += toSolarDurations[idx] * (_process.StateWeight[idx] / toSolarTotalWeight);
            }
            foreach (int idx in solarIndices)
            {
                solarToLunarDuration += toLunarDurations[idx] * (_process.StateWeight[idx] / toLunarTotalWeight);
            }

            return lunarToSolarDuration + solarToLunarDuration;
        }
    }
}
