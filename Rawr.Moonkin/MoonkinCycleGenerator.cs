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

        // 4T12 has a big effect on Wrath energy generation
        public bool Has4T12 { get; set; }
        // Euphoria proc chance
        public double EuphoriaChance { get; set; }
        // Rate of dot ticks
        public double DotTickRate { get; set; }
        // Chance of dot ticks to reset the Starsurge CD
        public double ShootingStarsChance { get; set; }

        protected override State<MoonkinCycleAbility> GetInitialState()
        {
            // Bar at middle, we want to initiate Solar first
            return GetState(0, 1, 0, 0, false);
        }

        protected override List<StateTransition<MoonkinCycleAbility>> GetStateTransitions(State<MoonkinCycleAbility> state)
        {
            MoonkinState theState = state as MoonkinState;
            List<StateTransition<MoonkinCycleAbility>> retval = new List<StateTransition<MoonkinCycleAbility>>();
            bool inEclipse = (theState.EclipseDirection > 0 && theState.EclipseEnergy < 0) ||
                             (theState.EclipseDirection < 0 && theState.EclipseEnergy > 0);

            // Probability combinations for Wrath, Euphoria, Shooting Stars
            double shootingStarsWrathTransitionProbability = 1 - Math.Pow((1 - ShootingStarsChance), (2 / DotTickRate));
            double wrathNoEuphoriaWithSSTransitionProbability = (1 - EuphoriaChance) * shootingStarsWrathTransitionProbability;
            double wrathNoEuphoriaNoSSTransitionProbability = (1 - EuphoriaChance) - wrathNoEuphoriaWithSSTransitionProbability;
            double wrathEuphoriaWithSSTransitionProbability = EuphoriaChance * shootingStarsWrathTransitionProbability;
            double wrathEuphoriaNoSSTransitionProbability = EuphoriaChance - wrathEuphoriaWithSSTransitionProbability;

            // Probability combinations for Starfire, Euphoria, Shooting Stars
            double shootingStarsStarfireTransitionProbability = 1 - Math.Pow((1 - ShootingStarsChance), (2.7 / DotTickRate));
            double starfireNoEuphoriaWithSSTransitionProbability = (1 - EuphoriaChance) * shootingStarsStarfireTransitionProbability;
            double starfireNoEuphoriaNoSSTransitionProbability = (1 - EuphoriaChance) - starfireNoEuphoriaWithSSTransitionProbability;
            double starfireEuphoriaWithSSTransitionProbability = EuphoriaChance * shootingStarsStarfireTransitionProbability;
            double starfireEuphoriaNoSSTransitionProbability = EuphoriaChance - starfireEuphoriaWithSSTransitionProbability;

            // Probability of proccing Shooting Stars when casting Starsurge
            double shootingStarsStarsurgeTransitionProbability = 1 - Math.Pow((1 - ShootingStarsChance), ((theState.IsShootingStarsActive ? 1.5 : 2) / DotTickRate));

            if (theState.StarsurgeCooldown <= 0)
            {
                // Solar Eclipse
                if (theState.EclipseEnergy >= 100)
                {
                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = (theState.IsShootingStarsActive ? ShootingStarsEclipse : StarsurgeEclipse), TargetState = GetState(85, -1, theState.WrathCount, 13, false), TransitionDuration = (theState.IsShootingStarsActive ? 1.5 : 2), TransitionProbability = 1 - shootingStarsStarsurgeTransitionProbability });
                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = (theState.IsShootingStarsActive ? ShootingStarsEclipse : StarsurgeEclipse), TargetState = GetState(85, -1, theState.WrathCount, 0, true), TransitionDuration = (theState.IsShootingStarsActive ? 1.5 : 2), TransitionProbability = shootingStarsStarsurgeTransitionProbability });
                }
                // Lunar Eclipse
                else if (theState.EclipseEnergy <= -100)
                {
                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = (theState.IsShootingStarsActive ? ShootingStarsEclipse : StarsurgeEclipse), TargetState = GetState(-85, 1, theState.WrathCount, 13, false), TransitionDuration = (theState.IsShootingStarsActive ? 1.5 : 2), TransitionProbability = 1 - shootingStarsStarsurgeTransitionProbability });
                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = (theState.IsShootingStarsActive ? ShootingStarsEclipse : StarsurgeEclipse), TargetState = GetState(-85, 1, theState.WrathCount, 0, true), TransitionDuration = (theState.IsShootingStarsActive ? 1.5 : 2), TransitionProbability = shootingStarsStarsurgeTransitionProbability });
                }
                // Baseline
                // TODO: If we're 1 cast away from an Eclipse, skip the Starsurge and go into Eclipse instead
                else
                {
                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = (inEclipse ? (theState.IsShootingStarsActive ? ShootingStarsEclipse : StarsurgeEclipse) : (theState.IsShootingStarsActive ? ShootingStars : Starsurge)), TargetState = GetState(theState.EclipseEnergy + theState.EclipseDirection * StarsurgeEnergy, theState.EclipseDirection, theState.WrathCount, 13, false), TransitionDuration = (theState.IsShootingStarsActive ? 1.5 : 2), TransitionProbability = 1 - shootingStarsStarsurgeTransitionProbability });
                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = (inEclipse ? (theState.IsShootingStarsActive ? ShootingStarsEclipse : StarsurgeEclipse) : (theState.IsShootingStarsActive ? ShootingStars : Starsurge)), TargetState = GetState(theState.EclipseEnergy + theState.EclipseDirection * StarsurgeEnergy, theState.EclipseDirection, theState.WrathCount, 0, true), TransitionDuration = (theState.IsShootingStarsActive ? 1.5 : 2), TransitionProbability = shootingStarsStarsurgeTransitionProbability });
                }
            }
            // Solar Eclipse - start casting Wrath
            else if (theState.EclipseEnergy >= 100)
            {
                // Wrath energy generation follows a 13/13/14 pattern.
                // Euphoria does not apply since we are not between 0 and -35 energy.
                // If we have 4T12, we are losing the bonus and need to play monkey games with the Wrath counter
                if (Has4T12)
                {
                    switch (theState.WrathCount)
                    {
                        // 17/16 -> 14 (counter at 0)
                        case 0:
                            retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = WrathEclipse, TargetState = GetState(100 - WrathEnergyHigh, -1, 0, theState.StarsurgeCooldown - 2, false), TransitionDuration = 2, TransitionProbability = 1 - shootingStarsWrathTransitionProbability });
                            retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = WrathEclipse, TargetState = GetState(100 - WrathEnergyHigh, -1, 0, 0, true), TransitionDuration = 2, TransitionProbability = shootingStarsWrathTransitionProbability });
                            break;
                        // 16/17 -> 13 (counter at 2)
                        case 1:
                            retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = WrathEclipse, TargetState = GetState(100 - WrathEnergyLow, -1, 2, theState.StarsurgeCooldown - 2, false), TransitionDuration = 2, TransitionProbability = 1 - shootingStarsWrathTransitionProbability });
                            retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = WrathEclipse, TargetState = GetState(100 - WrathEnergyLow, -1, 2, 0, true), TransitionDuration = 2, TransitionProbability = shootingStarsWrathTransitionProbability });
                            break;
                        // 17/17 -> 13 (counter at 1)
                        case 2:
                            retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = WrathEclipse, TargetState = GetState(100 - WrathEnergyLow, -1, 1, theState.StarsurgeCooldown - 2, false), TransitionDuration = 2, TransitionProbability = 1 - shootingStarsWrathTransitionProbability });
                            retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = WrathEclipse, TargetState = GetState(100 - WrathEnergyLow, -1, 1, 0, true), TransitionDuration = 2, TransitionProbability = shootingStarsWrathTransitionProbability });
                            break;
                    }
                }
                else
                {
                    switch (theState.WrathCount)
                    {
                        case 0:
                        case 1:
                            retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = WrathEclipse, TargetState = GetState(100 - WrathEnergyLow, -1, theState.WrathCount + 1, theState.StarsurgeCooldown - 2, false), TransitionDuration = 2, TransitionProbability = 1 - shootingStarsWrathTransitionProbability });
                            retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = WrathEclipse, TargetState = GetState(100 - WrathEnergyLow, -1, theState.WrathCount + 1, 0, true), TransitionDuration = 2, TransitionProbability = shootingStarsWrathTransitionProbability });
                            break;
                        case 2:
                            retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = WrathEclipse, TargetState = GetState(100 - WrathEnergyHigh, -1, 0, theState.StarsurgeCooldown - 2, false), TransitionDuration = 2, TransitionProbability = 1 - shootingStarsWrathTransitionProbability });
                            retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = WrathEclipse, TargetState = GetState(100 - WrathEnergyHigh, -1, 0, 0, true), TransitionDuration = 2, TransitionProbability = shootingStarsWrathTransitionProbability });
                            break;
                    }
                }
            }
            // Lunar Eclipse - cast Starfire
            else if (theState.EclipseEnergy <= -100)
            {
                // No need for Euphoria since we are inside an Eclipse state
                // No 4T12 since we gained Eclipse
                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = StarfireEclipse, TargetState = GetState(-100 + StarfireEnergy, 1, theState.WrathCount, theState.StarsurgeCooldown - 2.7, false), TransitionDuration = 2.7, TransitionProbability = 1 - shootingStarsStarfireTransitionProbability });
                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = StarfireEclipse, TargetState = GetState(-100 + StarfireEnergy, 1, theState.WrathCount, 0, true), TransitionDuration = 2.7, TransitionProbability = shootingStarsStarfireTransitionProbability });
            }
            // Moving toward Lunar Eclipse, cast Wrath
            else if (theState.EclipseDirection == -1)
            {
                // Euphoria only procs when we're out of eclipse (past 0 on the bar) and there's less than 35 energy in the direction of the eclipse
                if (theState.EclipseEnergy <= 0 && theState.EclipseEnergy >= -35)
                {
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
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyLow4T12, -1, 0, theState.StarsurgeCooldown - 2, false), TransitionDuration = 2, TransitionProbability = wrathNoEuphoriaNoSSTransitionProbability });
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - 30, -1, 0, theState.StarsurgeCooldown - 2, false), TransitionDuration = 2, TransitionProbability = wrathEuphoriaNoSSTransitionProbability });
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyLow4T12, -1, 0, 0, true), TransitionDuration = 2, TransitionProbability = wrathNoEuphoriaWithSSTransitionProbability });
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - 30, -1, 0, 0, true), TransitionDuration = 2, TransitionProbability = wrathEuphoriaWithSSTransitionProbability });
                                    break;
                                // 14/13 -> 17 (counter at 2)
                                case 1:
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyHigh4T12, -1, 2, theState.StarsurgeCooldown - 2, false), TransitionDuration = 2, TransitionProbability = wrathNoEuphoriaNoSSTransitionProbability });
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - 30, -1, 2, theState.StarsurgeCooldown - 2, false), TransitionDuration = 2, TransitionProbability = wrathEuphoriaNoSSTransitionProbability });
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyHigh4T12, -1, 2, 0, true), TransitionDuration = 2, TransitionProbability = wrathNoEuphoriaWithSSTransitionProbability });
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - 30, -1, 2, 0, true), TransitionDuration = 2, TransitionProbability = wrathEuphoriaWithSSTransitionProbability });
                                    break;
                                // 13/13 -> 17 (counter at 1)
                                case 2:
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyLow4T12, -1, 1, theState.StarsurgeCooldown - 2, false), TransitionDuration = 2, TransitionProbability = wrathNoEuphoriaNoSSTransitionProbability });
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - 30, -1, 1, theState.StarsurgeCooldown - 2, false), TransitionDuration = 2, TransitionProbability = wrathEuphoriaNoSSTransitionProbability });
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyLow4T12, -1, 1, 0, true), TransitionDuration = 2, TransitionProbability = wrathNoEuphoriaWithSSTransitionProbability });
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - 30, -1, 1, 0, true), TransitionDuration = 2, TransitionProbability = wrathEuphoriaWithSSTransitionProbability });
                                    break;
                            }
                        }
                        else
                        {
                            switch (theState.WrathCount)
                            {
                                case 0:
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyHigh4T12, -1, 1, theState.StarsurgeCooldown - 2, false), TransitionDuration = 2, TransitionProbability = wrathNoEuphoriaNoSSTransitionProbability });
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - 30, -1, 1, theState.StarsurgeCooldown - 2, false), TransitionDuration = 2, TransitionProbability = wrathEuphoriaNoSSTransitionProbability });
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyHigh4T12, -1, 1, 0, true), TransitionDuration = 2, TransitionProbability = wrathNoEuphoriaWithSSTransitionProbability });
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - 30, -1, 1, 0, true), TransitionDuration = 2, TransitionProbability = wrathEuphoriaWithSSTransitionProbability });
                                    break;
                                case 1:
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyHigh4T12, -1, 2, theState.StarsurgeCooldown - 2, false), TransitionDuration = 2, TransitionProbability = wrathNoEuphoriaNoSSTransitionProbability });
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - 30, -1, 2, theState.StarsurgeCooldown - 2, false), TransitionDuration = 2, TransitionProbability = wrathEuphoriaNoSSTransitionProbability });
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyHigh4T12, -1, 2, 0, true), TransitionDuration = 2, TransitionProbability = wrathNoEuphoriaWithSSTransitionProbability });
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - 30, -1, 2, 0, true), TransitionDuration = 2, TransitionProbability = wrathEuphoriaWithSSTransitionProbability });
                                    break;
                                case 2:
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyLow4T12, -1, 0, theState.StarsurgeCooldown - 2, false), TransitionDuration = 2, TransitionProbability = wrathNoEuphoriaNoSSTransitionProbability });
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - 30, -1, 0, theState.StarsurgeCooldown - 2, false), TransitionDuration = 2, TransitionProbability = wrathEuphoriaNoSSTransitionProbability });
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyLow4T12, -1, 0, 0, true), TransitionDuration = 2, TransitionProbability = wrathNoEuphoriaWithSSTransitionProbability });
                                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - 30, -1, 0, 0, true), TransitionDuration = 2, TransitionProbability = wrathEuphoriaWithSSTransitionProbability });
                                    break;
                            }
                        }
                    }
                    else
                    {
                        switch (theState.WrathCount)
                        {
                            case 0:
                                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyLow, -1, 1, theState.StarsurgeCooldown - 2, false), TransitionDuration = 2, TransitionProbability = wrathNoEuphoriaNoSSTransitionProbability });
                                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - 2 * WrathEnergyLow, -1, 2, theState.StarsurgeCooldown - 2, false), TransitionDuration = 2, TransitionProbability = wrathEuphoriaNoSSTransitionProbability });
                                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyLow, -1, 1, 0, true), TransitionDuration = 2, TransitionProbability = wrathNoEuphoriaWithSSTransitionProbability });
                                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - 2 * WrathEnergyLow, -1, 2, 0, true), TransitionDuration = 2, TransitionProbability = wrathEuphoriaWithSSTransitionProbability });
                                break;
                            case 1:
                                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyLow, -1, 2, theState.StarsurgeCooldown - 2, false), TransitionDuration = 2, TransitionProbability = wrathNoEuphoriaNoSSTransitionProbability });
                                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyLow - WrathEnergyHigh, -1, 0, theState.StarsurgeCooldown - 2, false), TransitionDuration = 2, TransitionProbability = wrathEuphoriaNoSSTransitionProbability });
                                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyLow, -1, 2, 0, true), TransitionDuration = 2, TransitionProbability = wrathNoEuphoriaWithSSTransitionProbability });
                                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyLow - WrathEnergyHigh, -1, 0, 0, true), TransitionDuration = 2, TransitionProbability = wrathEuphoriaWithSSTransitionProbability });
                                break;
                            case 2:
                                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyHigh, -1, 0, theState.StarsurgeCooldown - 2, false), TransitionDuration = 2, TransitionProbability = wrathNoEuphoriaNoSSTransitionProbability });
                                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyHigh - WrathEnergyLow, -1, 1, theState.StarsurgeCooldown - 2, false), TransitionDuration = 2, TransitionProbability = wrathEuphoriaNoSSTransitionProbability });
                                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyHigh, -1, 0, 0, true), TransitionDuration = 2, TransitionProbability = wrathNoEuphoriaWithSSTransitionProbability });
                                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyHigh - WrathEnergyLow, -1, 1, 0, true), TransitionDuration = 2, TransitionProbability = wrathEuphoriaWithSSTransitionProbability });
                                break;
                        }
                    }
                }
                else
                {
                    // Between -35 and -100 energy, 4T12 is active but not Euphoria
                    if (Has4T12 && theState.EclipseEnergy <= 0)
                    {
                        switch (theState.WrathCount)
                        {
                            case 0:
                            case 1:
                                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyHigh4T12, -1, theState.WrathCount + 1, theState.StarsurgeCooldown - 2, false), TransitionDuration = 2, TransitionProbability = 1 - shootingStarsWrathTransitionProbability });
                                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyHigh4T12, -1, theState.WrathCount + 1, 0, true), TransitionDuration = 2, TransitionProbability = shootingStarsWrathTransitionProbability });
                                break;
                            case 2:
                                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyLow4T12, -1, 0, theState.StarsurgeCooldown - 2, false), TransitionDuration = 2, TransitionProbability = 1 - shootingStarsWrathTransitionProbability });
                                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Wrath, TargetState = GetState(theState.EclipseEnergy - WrathEnergyLow4T12, -1, 0, 0, true), TransitionDuration = 2, TransitionProbability = shootingStarsWrathTransitionProbability });
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
                                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = (inEclipse ? WrathEclipse : Wrath), TargetState = GetState(theState.EclipseEnergy - WrathEnergyLow, -1, theState.WrathCount + 1, theState.StarsurgeCooldown - 2, false), TransitionDuration = 2, TransitionProbability = 1 - shootingStarsWrathTransitionProbability });
                                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = (inEclipse ? WrathEclipse : Wrath), TargetState = GetState(theState.EclipseEnergy - WrathEnergyLow, -1, theState.WrathCount + 1, 0, true), TransitionDuration = 2, TransitionProbability = shootingStarsWrathTransitionProbability });
                                break;
                            case 2:
                                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = (inEclipse ? WrathEclipse : Wrath), TargetState = GetState(theState.EclipseEnergy - WrathEnergyHigh, -1, 0, theState.StarsurgeCooldown - 2, false), TransitionDuration = 2, TransitionProbability = 1 - shootingStarsWrathTransitionProbability });
                                retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = (inEclipse ? WrathEclipse : Wrath), TargetState = GetState(theState.EclipseEnergy - WrathEnergyHigh, -1, 0, 0, true), TransitionDuration = 2, TransitionProbability = shootingStarsWrathTransitionProbability });
                                break;
                        }
                    }
                }
            }
            // Moving toward Solar Eclipse, cast Starfire
            else if (theState.EclipseDirection == 1)
            {
                // Euphoria only procs when we're out of eclipse (past 0 on the bar) and there's less than 35 energy in the direction of the eclipse
                if (theState.EclipseEnergy >= 0 && theState.EclipseEnergy <= 35)
                {
                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Starfire, TargetState = GetState(theState.EclipseEnergy + (Has4T12 ? StarfireEnergy4T12 : StarfireEnergy), 1, theState.WrathCount, theState.StarsurgeCooldown - 2.7, false), TransitionDuration = 2.7, TransitionProbability = starfireNoEuphoriaNoSSTransitionProbability });
                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Starfire, TargetState = GetState(theState.EclipseEnergy + (Has4T12 ? StarfireEnergy4T12 : StarfireEnergy) + 20, 1, theState.WrathCount, theState.StarsurgeCooldown - 2.7, false), TransitionDuration = 2.7, TransitionProbability = starfireEuphoriaNoSSTransitionProbability });
                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Starfire, TargetState = GetState(theState.EclipseEnergy + (Has4T12 ? StarfireEnergy4T12 : StarfireEnergy), 1, theState.WrathCount, 0, true), TransitionDuration = 2.7, TransitionProbability = starfireNoEuphoriaWithSSTransitionProbability });
                    retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Starfire, TargetState = GetState(theState.EclipseEnergy + (Has4T12 ? StarfireEnergy4T12 : StarfireEnergy) + 20, 1, theState.WrathCount, 0, true), TransitionDuration = 2.7, TransitionProbability = starfireEuphoriaWithSSTransitionProbability });
                }
                else
                {
                    // If not in Eclipse and 4T12 is equipped, use 4T12 Starfire
                    if (Has4T12 && theState.EclipseEnergy >= 0)
                    {
                        retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Starfire, TargetState = GetState(theState.EclipseEnergy + StarfireEnergy4T12, 1, theState.WrathCount, theState.StarsurgeCooldown - 2.7, false), TransitionDuration = 2.7, TransitionProbability = 1 - shootingStarsStarfireTransitionProbability });
                        retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = Starfire, TargetState = GetState(theState.EclipseEnergy + StarfireEnergy4T12, 1, theState.WrathCount, 0, true), TransitionDuration = 2.7, TransitionProbability = shootingStarsStarfireTransitionProbability });
                    }
                    else
                    {
                        retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = (inEclipse ? StarfireEclipse : Starfire), TargetState = GetState(theState.EclipseEnergy + StarfireEnergy, 1, theState.WrathCount, theState.StarsurgeCooldown - 2.7, false), TransitionDuration = 2.7, TransitionProbability = 1 - shootingStarsStarfireTransitionProbability });
                        retval.Add(new StateTransition<MoonkinCycleAbility> { Ability = (inEclipse ? StarfireEclipse : Starfire), TargetState = GetState(theState.EclipseEnergy + StarfireEnergy, 1, theState.WrathCount, 0, true), TransitionDuration = 2.7, TransitionProbability = shootingStarsStarfireTransitionProbability });
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

        public double DotTickRate
        {
            get
            {
                return _generator.DotTickRate;
            }
            set
            {
                _generator.DotTickRate = value;
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

        public object GenerateCycle()
        {
            _process = new MarkovProcess<MoonkinCycleAbility>(_generator.GenerateStateSpace());

            return null;
        }
    }
}
