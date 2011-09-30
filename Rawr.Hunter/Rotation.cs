using System;
using System.Collections.Generic;
using System.Text;
using Rawr.Hunter.Skills;

namespace Rawr.Hunter {
    /// <summary>
    /// Which stage of the fight does the current progression of abilities suggest.
    /// </summary>
    public enum FightStage : int
    {
        Opening = 0,
        Body,
        Below35,
        Below20,
    }

    /// <summary>
    /// Way to track the progression through a fight.
    /// </summary>
    public class FightTime
    {

        private BossOptions BossOpts;

        /// <summary>
        /// Way to move forward or back in the progression of time.
        /// </summary>
        private float _TimePointer;
        /// <summary>
        /// Time remaining in fight in Seconds
        /// </summary>
        public float TimeRemaining { get { return BossOpts.BerserkTimer - _TimePointer; } }
        /// <summary>
        /// Time from Start of the fight in Seconds
        /// </summary>
        public float TimeFromStart { get { return _TimePointer; } }
        public float TimeRemainingInStage 
        {
            get 
            {
                switch (CurrentStage)
                {
                    case FightStage.Opening:
                        return DurationOfStage(FightStage.Opening) - TimeFromStart;
                        // break;
                    case FightStage.Below35:
                    case FightStage.Below20:
                        return TimeRemaining;
                        //break;
                    default:
                        return TimeRemaining - DurationOfStage(FightStage.Below35);
                }
            }
        }
        [Percentage]
        public float PercTimeForOpening 
        {
            get { return _PercOpening; }
            set { _PercOpening = value; } 
        }
        private float _PercOpening = 0;

        public float DurationOfStage(FightStage fs)
        {
            float dur = 0;
            switch (fs)
            {
                case FightStage.Opening:
                    // Switch this up to a custom set openning duration.
                    dur = BossOpts.BerserkTimer * PercTimeForOpening;
                    break;
                case FightStage.Below35:
                    dur += BossOpts.BerserkTimer * (float)(BossOpts.Under35Perc + BossOpts.Under20Perc);
                    break;
                case FightStage.Below20:
                    dur += BossOpts.BerserkTimer * (float)BossOpts.Under20Perc;
                    break;
                default:
                    dur = BossOpts.BerserkTimer - (BossOpts.BerserkTimer * (float)(BossOpts.Under35Perc + BossOpts.Under20Perc));
                    break;
            }
            return dur;
        }

        public void AddSegment(float fTimeInSec)
        {
            _TimePointer += fTimeInSec;
        }

        public FightStage CurrentStage 
        { 
            get
            {
                if (TimeFromStart < DurationOfStage(FightStage.Opening))
                    return FightStage.Opening;
                else if (TimeRemaining < DurationOfStage(FightStage.Below35))
                    return FightStage.Below35;
                else if (TimeRemaining < DurationOfStage(FightStage.Below20))
                    return FightStage.Below20;
                else
                    return FightStage.Body;
            }
        }

        public FightTime(BossOptions bo)
        {
            BossOpts = bo;
        }
    }
    public class Rotation
    {

        #region Enums
        public enum RotationType
        {
            Custom, BeastMastery, Marksmanship, Survival, Unknown,
        }
        public enum TalentTrees
        {
            BeastMastery, Marksmanship, Survival,
        }
        #endregion

        // Constructors
        public Rotation()
        {
            Char = null;
            StatS = null;
            Talents = null;
            CombatFactors = null;
            CalcOpts = null;
            BossOpts = null;

            AbilityList = new Dictionary<Type,AbilWrapper>();
            InvalidateCache();
        }

        public Rotation(CombatFactors CF, HunterTalents t)
        {
            Char = null;
            StatS = null;
            Talents = t;
            CombatFactors = CF;
            CalcOpts = null;
            BossOpts = null;

            AbilityList = new Dictionary<Type, AbilWrapper>();
            InvalidateCache();
        }

        #region Variables

        private Dictionary<Type,AbilWrapper> AbilityList;

        // TODO: Update this to filter the list by spec.
        public List<AbilWrapper> GetDamagingAbilities()
        {
            return new List<AbilWrapper>(AbilityList.Values).FindAll(e => e.isDamaging);
        }

        public List<AbilWrapper> GetMaintenanceAbilities()
        {
            return new List<AbilWrapper>(AbilityList.Values).FindAll(e => e.ability.isMaint);
        }

        public List<AbilWrapper> GetAbilityList()
        {
            return new List<AbilWrapper>(AbilityList.Values);
        }

        public AbilWrapper GetWrapper<T>()
        {
            return AbilityList[typeof(T)];
        }

        public float _DPS_TTL;
        public string GCDUsage = "";
        protected CharacterCalculationsHunter calcs = null;
        
        public bool _needDisplayCalcs = true;
        
        protected float FightDuration
        {
            get { return BossOpts.BerserkTimer; }
        }
        protected float TimeLostGCDs;
        protected float RageGainedWhileMoving;
        public float TimesStunned = 0f;
        public float TimesFeared = 0f;
        public float TimesRooted = 0f;
        public float TimesDisarmed = 0f;
        
        #region Abilities
       // Shots
        public Skills.ExplosiveShot Explosive;
        public Skills.MultiShot Multi;
        public Skills.SteadyShot Steady;
        public Skills.CobraShot Cobra;
        public Skills.AimedShot Aimed;
        public Skills.ArcaneShot Arcane;        
        public Skills.ChimeraShot ChimeraShot;
        // Generic
        public Skills.KillShot Kill;

        // Buffs.
        public Skills.BestialWrath Bestial;
        public Skills.RapidFire Rapid;
//        public Skills.BlackArrowBuff BlackArrowB;
        public Skills.Readiness Ready;

        // DoTs.
        public Skills.PiercingShots Piercing;
        public Skills.BlackArrow BlackArrowD;

        public Skills.SerpentSting Serpent;
        public Skills.ChimeraShot_Serpent Chimera;

        // Traps.
        public Skills.ImmolationTrap Immolation;
        public Skills.ExplosiveTrap ExplosiveT;
        public Skills.FreezingTrap Freezing;
        public Skills.FrostTrap Frost;

        public float _Steady_DPS = 0f, _Steady_HPS = 0f, _Steady_GCDs = 0f;
        public float _Multi_DPS = 0f, _Multi_HPS = 0f, _Multi_GCDs = 0f;
        
        #endregion

        #endregion
        #region Get/Set
        protected Character Char { get; set; }
        protected HunterTalents Talents { get; set; }
        protected StatsHunter StatS { get; set; }
        public CombatFactors CombatFactors { get; set; }
        public WhiteAttacks WhiteAtks { get; protected set; }
        protected CalculationOptionsHunter CalcOpts { get; set; }
        protected BossOptions BossOpts { get; set; }
        private FightTime Fight { get; set; }
        protected float LatentGCD { get { return 1f + CalcOpts.Latency + CalcOpts.AllowedReact; } }
        
        /// <summary>
        /// How many GCDs are in the rotation, based on fight duration and latency
        /// </summary>
        protected float NumGCDs { get { return FightDuration / LatentGCD; } }
        
        /// <summary>
        /// How many GCDs have been used by the rotation
        /// </summary>
        protected float GCDsUsed {
            get {
                float gcds = 0f;
                foreach (AbilWrapper aw in GetAbilityList()) {
                    if (aw.ability.UsesGCD) {
                        gcds += aw.numActivates * aw.ability.UseTime / LatentGCD;
                    }
                }
                return gcds;
            }
        }

        /// <summary>
        /// How many GCDs are still available in the rotation
        /// </summary>
        protected float GCDsAvailable { get { return Math.Max(0f, NumGCDs - GCDsUsed - TimeLostGCDs); } }
        
        #endregion
        #region Functions
        public virtual void Initialize(CharacterCalculationsHunter calcs) {
            this.calcs = calcs;
            StatS = calcs.Hunter.Stats;
            Char = calcs.character;
            BossOpts = calcs.BossOpts;
            Fight = new FightTime(BossOpts);
            CalcOpts = calcs.CalcOpts;

            // Get our Ability list with valid options.
            initAbilities();

            // Generate the rotation
            doIterations();

            float iActions = WhiteAtks.RwActivates;
            iActions += AbilityList[typeof(SteadyShot)].numActivates;
            iActions += AbilityList[typeof(CobraShot)].numActivates;
            iActions += AbilityList[typeof(MultiShot)].numActivates;
            iActions += AbilityList[typeof(ArcaneShot)].numActivates;
            iActions += AbilityList[typeof(ExplosiveShot)].numActivates;
            iActions += AbilityList[typeof(AimedShot)].numActivates;
            iActions += AbilityList[typeof(ChimeraShot)].numActivates;
            iActions += AbilityList[typeof(KillShot)].numActivates;
            iActions += AbilityList[typeof(SerpentSting)].numActivates;

            if (FightDuration > 0)
            {
                float numWQProcs = (iActions * calcs.Hunter.MasteryRatePercent);
                calcs.WildQuiverDPS = (numWQProcs * WhiteAtks.RwDamageOnUse) / FightDuration;
            }
            float fDPS = WhiteAtks.RwDPS;
            fDPS += calcs.WildQuiverDPS;
            fDPS += AbilityList[typeof(SteadyShot)].DPS;
            fDPS += AbilityList[typeof(CobraShot)].DPS;
            fDPS += AbilityList[typeof(MultiShot)].DPS;
            fDPS += AbilityList[typeof(ArcaneShot)].DPS;
            fDPS += AbilityList[typeof(ExplosiveShot)].DPS;
            fDPS += AbilityList[typeof(AimedShot)].DPS;
            fDPS += AbilityList[typeof(ChimeraShot)].DPS;
            fDPS += AbilityList[typeof(KillShot)].DPS;
            fDPS += AbilityList[typeof(SerpentSting)].DPS;
            calcs.CustomDPS = fDPS;

            #region Populate the display values.
            // Whites
            calcs.Whites = WhiteAtks;

            // Filler/ Focus regen.
            calcs.Steady = AbilityList[typeof(SteadyShot)];
            calcs.Cobra = AbilityList[typeof(CobraShot)]; // SV or BM (Replaces Steady Shot)
            
            // Shared Instants
            calcs.Multi = AbilityList[typeof(MultiShot)];
            calcs.Arcane = AbilityList[typeof(ArcaneShot)];

            // Spec specific.
            calcs.Explosive = AbilityList[typeof(ExplosiveShot)]; // SV
            calcs.BlackArrowD = BlackArrowD; // SV
            calcs.Aimed = this.AbilityList[typeof(AimedShot)]; // MM
            calcs.Chimera = this.AbilityList[typeof(ChimeraShot)]; // MM
            // calcs.Intimidate = Intimidate; // BM
            calcs.Bestial = Bestial; // BM

            // Generic
            calcs.Piercing = Piercing;
            calcs.Kill = AbilityList[typeof(KillShot)];

            // Buffs
            calcs.Rapid = Rapid;
            calcs.Ready = Ready;
            
            // DOT
            calcs.Serpent = AbilityList[typeof(SerpentSting)];

            // Traps
            calcs.Immolation = Immolation;
            calcs.ExplosiveT = ExplosiveT;
            calcs.Freezing = Freezing;
            calcs.Frost = Frost;
            #endregion

        }
        public virtual void Initialize() { initAbilities(); /*doIterations();*/ }

        public virtual void MakeRotationandDoDPS(bool setCalcs, bool needsDisplayCalculations) {
            _needDisplayCalcs = needsDisplayCalculations;
        }

        public RotationType GetRotationType(HunterTalents t)
        {
            RotationType curRotationType = RotationType.Custom;
            if (t != null)
            {
                if (t.HighestTree == (int)Specialization.BeastMastery)
                {
                    // Beast Mastery
                    curRotationType = Rotation.RotationType.BeastMastery;
                }
                else if (t.HighestTree == (int)Specialization.Marksmanship)
                {
                    // Marksmanship
                    curRotationType = Rotation.RotationType.Marksmanship;
                }
                if (t.HighestTree == (int)Specialization.Survival)
                {
                    // Survival
                    curRotationType = Rotation.RotationType.Survival;
                }
            }
            return curRotationType;
        }

        /// <summary>
        /// Initilize the abilties for use in generating the rotation.
        /// </summary>
        protected virtual void initAbilities() {
            AbilityList.Clear();
            if (WhiteAtks != null)
                WhiteAtks.InvalidateCache();

            // Whites
            WhiteAtks = new Skills.WhiteAttacks(Char, StatS, CombatFactors, CalcOpts, BossOpts);

            // Filler/ Focus regen.
            Steady = new Skills.SteadyShot(Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            AddAbility(new AbilWrapper(Steady));
            Cobra = new Skills.CobraShot(Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            AddAbility(new AbilWrapper(Cobra));

            // Shared Instants
            Multi = new Skills.MultiShot(Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            AddAbility(new AbilWrapper(Multi));
            Arcane = new Skills.ArcaneShot(Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            AddAbility(new AbilWrapper(Arcane));

            // Spec specific.
            Explosive = new Skills.ExplosiveShot(Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            AddAbility(new AbilWrapper(Explosive));
            this.BlackArrowD = new Skills.BlackArrow(Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            AddAbility(new AbilWrapper(BlackArrowD));
            Aimed = new Skills.AimedShot(Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            AddAbility(new AbilWrapper(Aimed));
            ChimeraShot = new Skills.ChimeraShot(Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            AddAbility(new AbilWrapper(ChimeraShot));
            //Skills.Ability Intimidate = new Skills.Intimidate(Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            Bestial = new Skills.BestialWrath(Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            AddAbility(new AbilWrapper(Bestial));

            // Generic
            Piercing = new Skills.PiercingShots(Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            AddAbility(new AbilWrapper(Piercing));
            Kill = new Skills.KillShot(Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            AddAbility(new AbilWrapper(Kill));

            // Buffs
            Rapid = new Skills.RapidFire(Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            AddAbility(new AbilWrapper(Rapid));
            Ready = new Skills.Readiness(Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            AddAbility(new AbilWrapper(Ready));

            // DOT
            Serpent = new Skills.SerpentSting(Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            AddAbility(new AbilWrapper(Serpent));

            // Traps
            //AddAbility(new AbilWrapper(new Skills.ImmolationTrap(Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            //AddAbility(new AbilWrapper(new Skills.ExplosiveTrap(Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            //AddAbility(new AbilWrapper(new Skills.FreezingTrap(Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            //AddAbility(new AbilWrapper(new Skills.FrostTrap(Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
        }

        private void AddAbility(AbilWrapper abilWrapper)
        {
            AbilityList.Add(abilWrapper.ability.GetType(), abilWrapper);
        }

        public virtual void doIterations() 
        {
            if (this.Char.Ranged == null) return;

            // Opening: first 10 % health.  Terrible hack at this point.
            float fBodyDur = this.BossOpts.BerserkTimer;
            float fOpeningDur = this.BossOpts.BerserkTimer * .1f;
            fBodyDur -= fOpeningDur;
            float fKSDur = this.BossOpts.BerserkTimer * (float)(this.BossOpts.Under35Perc + this.BossOpts.Under20Perc);
            fBodyDur -= fKSDur;

            List<AbilWrapper> AL = GetDamagingAbilities();


            // For starters lets build up what the rotation would look like for a given Spec:
            #region  BM
            if (GetRotationType(Talents) == RotationType.BeastMastery)
            { }
            #endregion
            #region MM
            else /*if (GetRotationType(Talents) == RotationType.Marksmanship)*/
            {
                // Taken from:
                // http://www.warcrafthuntersunion.com/2010/11/cataclysm-mm-hunter-shot-rotation-guide/

                // Setup:  
                // TODO: Assume Hunter's Mark
                // TODO: Assume AOTHawk

                // TODO: Check movement rate
                #region Opening
                // Open with RapidFire/Readiness/RapidFire
                //      Aimed Shot procs (instant cast from Master Marksman procs)
                //      Aimed Shot hardcast (manually casting the 2.9-second cast version)
                //      Steady Shot
                // May replace some of this if movement is high, for SerpentSting & Chimera to refresh.
                // For now, static fight.
                Fight.PercTimeForOpening = .1f;
                // start w/ SS x2 to get ISS
                Ability current = AbilityList[typeof(SteadyShot)].ability;
                float SSCast2 = current.CastTime * 2;
                float SSCast2ISS = SSCast2 / 1.15f;
                float fISSDur = 8; // duration of ISS == 8 Sec.
                int numActions = 0;
                int ssCount = 0;
                float CAAimedShots = 0; // Aimed shots with health > 90%
                AbilityList[typeof(SteadyShot)].numActivates += 2; // ISS
                Fight.AddSegment(SSCast2);
                ssCount += 2;
                while (Fight.CurrentStage == FightStage.Opening)
                {
                    // Master Marksman procs are after average of 8.33 SS.
                    // Aimed during ISS Dur
                    // We get bonus to Crit for these values only during Opening.
                    current = AbilityList[typeof(AimedShot)].ability;
                    // Aimed Shot Procs.
                    if (GetAimedShotProc(ssCount))
                    {
                        AbilityList[typeof(AimedShot)].numActivates++;
                        Fight.AddSegment(LatentGCD); 
                        ssCount = 0;
                    }
                    // Bulk adds of the Aimed Shot within either:
                    // ** How much time left in the phase to cast Aimed while ISS is still active.
                    // ** How many Aimed Shots can we squeeze in an ISS duration.
                    // Then refresh ISS.
                    // TODO: Limited by Focus.
                    numActions = (int)(Math.Min((Fight.TimeRemainingInStage / (current.CastTime / 1.15f)), (fISSDur - (SSCast2ISS + (ssCount == 0 ? 1 : 0)) )) / (current.CastTime / 1.15f));
                    AbilityList[typeof(AimedShot)].numActivates += numActions;
                    Fight.AddSegment(numActions * (current.CastTime / 1.15f));
                    // Refresh ISS
                    current = AbilityList[typeof(SteadyShot)].ability;
                    numActions = 2;
                    ssCount += numActions;
                    AbilityList[typeof(SteadyShot)].numActivates += numActions; // ISS
                    Fight.AddSegment(SSCast2ISS);
                }
                #endregion
                CAAimedShots = AbilityList[typeof(AimedShot)].numActivates;

                #region Body: Above 35% health
                //    Chimera Shot - Limited by CD.
                //    Only need to do this once for this phase.
                current = AbilityList[typeof(ChimeraShot)].ability;
                float fCSCD = AbilityList[typeof(ChimeraShot)].ability.Cd;
                numActions = (int)(Fight.TimeRemainingInStage / (fCSCD));
                AbilityList[typeof(ChimeraShot)].numActivates += numActions;
                Fight.AddSegment(numActions * LatentGCD);
                while (Fight.CurrentStage == FightStage.Body)
                {
                    // Squeeze the rest of this in between CS's above
                    // Reset Cooldown length.
                    fCSCD = AbilityList[typeof(ChimeraShot)].ability.Cd;
                    // Aimed Shot Procs.
                    if (GetAimedShotProc(ssCount))
                    {
                        AbilityList[typeof(AimedShot)].numActivates++;
                        Fight.AddSegment(LatentGCD); // just a GCD.
                        ssCount = 0;
                        // consume 1 GCD of the Cobra Shot CD.
                        fCSCD--;
                    }
                    //    Arcane Shot - Limited by Focus
                    // Consume the rest of the CD with Arcanes for now.  We'll worry about the focus later.
                    fCSCD -= SSCast2ISS;
                    current = AbilityList[typeof(ArcaneShot)].ability;
                    numActions = (int)(Math.Min(fCSCD, (fISSDur - (SSCast2ISS + (ssCount == 0 ? 1 : 0)))));
                    AbilityList[typeof(ArcaneShot)].numActivates += numActions; // ISS
                    Fight.AddSegment(numActions * LatentGCD);
                    //    Steady Shot (in pairs)
                    current = AbilityList[typeof(SteadyShot)].ability;
                    numActions = 2;
                    ssCount += numActions;
                    AbilityList[typeof(SteadyShot)].numActivates += numActions; // ISS
                    Fight.AddSegment(SSCast2ISS);
                    // TODO: Setup some eject if the CSCD > TimeLeftInStage
                }
                #endregion
                // This transition may happen mid CobraShot CD.
                #region Kill Shot time.
                //    Chimera Shot - Limited by CD.
                //    Only need to do this once for this phase.
                current = AbilityList[typeof(ChimeraShot)].ability;
                fCSCD = AbilityList[typeof(ChimeraShot)].ability.Cd;
                numActions = (int)(Fight.TimeRemainingInStage / (fCSCD));
                AbilityList[typeof(ChimeraShot)].numActivates += numActions;
                float totalCSCDTime = fCSCD * numActions;
                int numCS = numActions;
                Fight.AddSegment(numActions * LatentGCD);
                //    Kill Shot - limited by CD.
                float fKSCD = AbilityList[typeof(KillShot)].ability.Cd;
                current = AbilityList[typeof(KillShot)].ability;
                numActions = (int)(Fight.TimeRemainingInStage / (fKSCD));
                AbilityList[typeof(KillShot)].numActivates += numActions;
                Fight.AddSegment(numActions * LatentGCD);
                // Reduce the overall average CSCD by those consumed by KS.
                float remainingTotalCSCDTime = totalCSCDTime - (fKSCD * numActions);
                float BaseCSCDTime = remainingTotalCSCDTime / numCS; 
                while ((Fight.CurrentStage == FightStage.Below35
                    || Fight.CurrentStage == FightStage.Below20)
                    && Fight.TimeRemaining > 0)
                {
                    // Squeeze the rest of this in between CS's above
                    // Reset Cooldown length.
                    fCSCD = BaseCSCDTime;
                    // Aimed Shot Procs.
                    if (GetAimedShotProc(ssCount))
                    {
                        AbilityList[typeof(AimedShot)].numActivates++;
                        Fight.AddSegment(LatentGCD); // just a GCD.
                        ssCount = 0;
                        // consume 1 GCD of the Cobra Shot CD.
                        fCSCD--;
                    }
                    //    Arcane Shot - Limited by Focus
                    // Consume the rest of the CD with Arcanes for now.  We'll worry about the focus later.
                    fCSCD -= SSCast2ISS;
                    current = AbilityList[typeof(ArcaneShot)].ability;
                    numActions = (int)(Math.Min(fCSCD, (fISSDur - (SSCast2ISS + (ssCount == 0 ? 1 : 0)))));
                    AbilityList[typeof(ArcaneShot)].numActivates += numActions; // ISS
                    Fight.AddSegment(numActions * LatentGCD);
                    //    Steady Shot (in pairs)
                    current = AbilityList[typeof(SteadyShot)].ability;
                    numActions = 2;
                    ssCount += numActions;
                    AbilityList[typeof(SteadyShot)].numActivates += numActions; // ISS
                    Fight.AddSegment(SSCast2ISS);
                }
                #endregion
                float TotalAimedShots = AbilityList[typeof(AimedShot)].numActivates;
                AbilityList[typeof(AimedShot)].ability.BonusCritChance += (CAAimedShots / TotalAimedShots) * (.3f * Talents.CarefulAim);
                
            }
            #endregion

            // SV
        }

        private bool GetAimedShotProc(int ssCount)
        {
            float fMM = Talents.MasterMarksman * .2f;
            if (fMM > 0)
            {
                return ssCount > (5f / fMM);
            }
            else
                return false;
        }

        public void InvalidateCache()
        {
            for (int i = 0; i < EnumHelper.GetCount(typeof(ShotResult)); i++) for (int k = 0; k < EnumHelper.GetCount(typeof(AttackType)); k++)
                _atkOverDurs[i,k] = -1f;
        }

        #region Attacks over Duration
        public enum ShotResult : int { Attempt=0, Land, Crit };
        [Flags]
        public enum AttackType : int { Yellow, White}; // this should be [Flags] were Yellow & White are the flags so that Both would be Yellow + White
        private float[,] _atkOverDurs = new float[EnumHelper.GetCount(typeof(ShotResult)), EnumHelper.GetCount(typeof(AttackType))];
        public float GetAttackOverDuration(ShotResult shotResult, AttackType attackType)
        {
            if (_atkOverDurs[(int)shotResult, (int)attackType] == -1f)
            {
                SetTable(shotResult, attackType);
            }
            return _atkOverDurs[(int)shotResult, (int)attackType];
        }
        private void SetTable(ShotResult sr, AttackType at)
        {
            float count = 0f;
            float mod;
            CombatTable table;

            if ((at & AttackType.Yellow) == AttackType.Yellow) 
            {
                foreach (AbilWrapper abil in GetDamagingAbilities())
                {
                    if (!abil.ability.Validated) { continue; }
                    table = abil.ability.RWAtkTable;
                    mod = GetTableFromSwingResult(sr, table);
                    count += abil.numActivates * abil.ability.AvgTargets * abil.ability.SwingsPerActivate * mod;
                }
            }
            if ((at & AttackType.White) == AttackType.White) 
            {
                table = WhiteAtks.RWAtkTable;
                mod = GetTableFromSwingResult(sr, table);
                count += WhiteAtks.RwActivates * mod;
            }
            
            _atkOverDurs[(int)sr, (int)at] = count;
        }
        private float GetTableFromSwingResult(ShotResult sr, CombatTable table)
        {
            if (table == null) return 0f;
            switch (sr)
            {
                case ShotResult.Attempt: return 1f;
                case ShotResult.Crit: return table.Crit;
                case ShotResult.Land: return table.AnyLand;
                default: return 0f;
            }
        }

        public float AttemptedAtksOverDur { get { return GetAttackOverDuration(ShotResult.Attempt, AttackType.Yellow | AttackType.White); } }
        public float AttemptedYellowsOverDur { get { return GetAttackOverDuration(ShotResult.Attempt, AttackType.Yellow); } }
        public float LandedAtksOverDur { get { return GetAttackOverDuration(ShotResult.Land, AttackType.Yellow | AttackType.White); } }
        public float CriticalAtksOverDur { get { return GetAttackOverDuration(ShotResult.Crit, AttackType.Yellow | AttackType.White); } }

        public float CriticalYellowsOverDur { get { return GetAttackOverDuration(ShotResult.Crit, AttackType.Yellow | AttackType.White); } }
        public float LandedYellowsOverDur { get { return GetAttackOverDuration(ShotResult.Land, AttackType.Yellow | AttackType.White); } }
        #endregion

        public float MaintainCDs { get {
            float cds = 0f;
            foreach (AbilWrapper aw in GetMaintenanceAbilities()) cds += aw.numActivates;
            return cds;
        } }
        #endregion
        #region Focus Calcs
        protected virtual float FocusGenOverDur_Other {
            get {
                float focus = (100f * StatS.ManaorEquivRestore);  // 0.02f becomes 2f

                    foreach (AbilWrapper aw in GetAbilityList())
                    {
                        if (aw.Focus < 0) 
                            focus += (-1f) * aw.Focus;
                    }
                    
                return focus;
            }
        }
        protected float FocusNeededOverDur {
            get {
                float focus = 0f;
                foreach (AbilWrapper aw in GetAbilityList())
                {
                    if (aw.Focus > 0f) 
                        focus += aw.Focus;
                }
                return focus;
            }
        }
        #endregion

        #region AddAnItem(s)
        /// <summary>
        /// Adds every maintenance ability to the rotation
        /// </summary>
        /// <param name="totalPercTimeLost">Time lost due to Boss Handler options</param>
        /// <returns>Change in focus from these abilities</returns>
        protected float DoMaintenanceActivates(float totalPercTimeLost)
        {
            float netFocus = 0f;

            foreach (AbilWrapper aw in GetMaintenanceAbilities())
            {
                netFocus += AddMaintenanceAbility(totalPercTimeLost, aw);
            }
            if (netFocus != 0f && _needDisplayCalcs) GCDUsage += Environment.NewLine;
            return netFocus;
        }

        /// <summary>
        /// Adds a maintenance ability to the rotation if it's been validated
        /// </summary>
        /// <param name="totalPercTimeLost">Time lost due to stun/fear/movement</param>
        /// <param name="abil">The ability to add</param>
        /// <returns>The final result from Abil.GetManaUseOverDur</returns>
        private float AddMaintenanceAbility(float totalPercTimeLost, AbilWrapper aw)
        {
            if (!aw.ability.Validated) return 0f;

            float Abil_GCDs = Math.Min(GCDsAvailable, aw.ability.Activates * (1f - totalPercTimeLost));
            aw.numActivates = Abil_GCDs;
            //availGCDs -= Abil_GCDs;
            if (_needDisplayCalcs && Abil_GCDs > 0)
                GCDUsage += Abil_GCDs.ToString("000.00") + " : " + aw.ability.Name + (aw.ability.UsesGCD ? "\n" : "(Doesn't use GCDs)\n");

            _DPS_TTL += aw.DPS;
            return aw.ability.GetFocusUseOverDur(Abil_GCDs);
        }
        #endregion

        #region Lost Time due to Combat Factors
        //private float /*_emActs/*, _emRecovery, _emRecoveryTotal*/;

        /// <summary>
        /// Calculates percentage of time lost due to moving, being rooted, etc
        /// </summary>
        /// <param name="MS">Placeholder right now for juggernaut handling.  Fury should pass null</param>
        /// <returns>Percentage of time lost as a float</returns>
        protected float CalculateTimeLost(Ability MS)
        {
            //_emActs = 0f; //_emRecovery = 0f; _emRecoveryTotal = 0f;
            TimeLostGCDs = 0;

            float percTimeInMovement = CalculateMovement(MS);
            float percTimeInFear = CalculateFear();
            float percTimeInStun = CalculateStun();
            float percTimeInRoot = CalculateRoot(); // Root will not have much effect on hunters since they are physical ranged.
            return Math.Min(1f, percTimeInStun + percTimeInMovement + percTimeInFear + percTimeInRoot);
        }

        private float CalculateRoot()
        {
            return 0f; // Not yet implemetned in this model
            /*float percTimeInRoot = 0f;
            if (CalcOpts.RootingTargets && CalcOpts.Roots.Count > 0)
            {
                float timelostwhilerooted = 0f;
                float BaseRootDur = 0f, rootActs = 0f, reducedDur = 0f,
                      MaxTimeRegain = 0f,
                      ChanceYouAreRooted = 1f;
                /*AbilWrapper HF = GetWrapper<HeroicFury>();
                float HFMaxActs = HF.ability.Activates;
                float HFActualActs = 0f;*//*
                AbilWrapper EM = GetWrapper<EveryManForHimself>();
                float EMMaxActs = EM.ability.Activates - EM.numActivates;
                float EMOldActs = EM.numActivates;
                TimesRooted = 0f;
                foreach (Impedence r in CalcOpts.Roots)
                {
                    BaseRootDur = Math.Max(0f, (r.Duration / 1000f * (1f - StatS.SnareRootDurReduc)));
                    rootActs = FightDuration / r.Frequency;
                    if (rootActs > 0f)
                    {
                        TimesRooted += rootActs;
                        /*if (HFMaxActs - HFActualActs > 0f)
                        {
                            MaxTimeRegain = Math.Max(0f, (BaseRootDur - LatentGCD - CalcOpts.React / 1000f));
                            float BZNewActs = Math.Min(HFMaxActs - HFActualActs, rootActs);
                            HFActualActs += BZNewActs;
                            // Use up to the maximum, leaving a 0 boundary so we don't mess up later numbers
                            reducedDur = Math.Max(0f, BaseRootDur - MaxTimeRegain);
                            float percBZdVsUnBZd = BZNewActs / rootActs;
                            timelostwhilerooted += (reducedDur * rootActs * percBZdVsUnBZd * ChanceYouAreRooted)
                                                 + (BaseRootDur * rootActs * (1f - percBZdVsUnBZd) * ChanceYouAreRooted);
                        }
                        else*//* if (Char.Race == CharacterRace.Human && EMMaxActs - EM.numActivates > 0f)
                        {
                            MaxTimeRegain = Math.Max(0f, (BaseRootDur - LatentGCD - CalcOpts.React / 1000f));

                            float availEMacts = EM.ability.Activates - EM.numActivates;
                            float EMNewActs = Math.Min(rootActs, availEMacts);
                            EM.numActivates += EMNewActs;
                            _emActs = EM.numActivates;

                            //EMActualActs += EMNewActs;
                            // Use up to the maximum, leaving a 0 boundary so we don't mess up later numbers
                            reducedDur = Math.Max(0f, BaseRootDur - MaxTimeRegain);
                            float percEMdVsUnEMd = EMNewActs / rootActs;
                            timelostwhilerooted += (reducedDur * rootActs * percEMdVsUnEMd * ChanceYouAreRooted)
                                                 + (BaseRootDur * rootActs * (1f - percEMdVsUnEMd) * ChanceYouAreRooted);
                        }
                        else
                        {
                            timelostwhilerooted += BaseRootDur * rootActs * ChanceYouAreRooted;
                        }
                    }
                }
                //HF.numActivates = HFActualActs;
                if (_needDisplayCalcs)
                {
                    GCDUsage += (TimesFeared > 0 ? TimesFeared.ToString("000.00") + " x " + BaseRootDur.ToString("0.00") + "secs : Lost to Fears\n" : "");
                    //GCDUsage += (HF.numActivates > 0 ? HF.numActivates.ToString("000.00") + " x " + (BaseRootDur - reducedDur).ToString("0.00") + "secs : - " + HF.ability.Name + "\n" : "");
                    GCDUsage += (EM.numActivates - EMOldActs > 0 ? (EM.numActivates - EMOldActs).ToString("000.00") + " x " + (BaseRootDur - reducedDur).ToString("0.00") + "secs : - " + EM.ability.Name + "\n" : "");
                }
                TimeLostGDCs += Math.Min(NumGCDs, (BaseRootDur * TimesFeared) / LatentGCD);
                TimeLostGDCs -= Math.Min(TimeLostGDCs, (reducedDur * HF.numActivates) / LatentGCD);
                TimeLostGDCs -= Math.Min(TimeLostGDCs, (reducedDur * _emActs) / LatentGCD);

                timelostwhilerooted = timelostwhilerooted;
                percTimeInRoot = timelostwhilerooted / FightDuration;
            }
            SecondWind SndW = GetWrapper<SecondWind>().ability as SecondWind;
            SndW.NumStunsOverDur += TimesRooted;
            return percTimeInRoot;*/
        }

        private float CalculateStun()
        {
            return 0f; // Not yet implemented in this model
            /*
            float percTimeInStun = 0f;
            if (CalcOpts.StunningTargets && CalcOpts.Stuns.Count > 0)
            {
                float timelostwhilestunned = 0f;
                float BaseStunDur = 0f, stunActs = 0f, reducedDur = 0f,
                      MaxTimeRegain = 0f,
                      ChanceYouAreStunned = 1f;
                AbilWrapper EM = GetWrapper<EveryManForHimself>();
                float EMMaxActs = EM.ability.Activates - EM.numActivates;
                float EMOldActs = EM.numActivates;
                TimesFeared = 0f;
                foreach (Impedence s in CalcOpts.Stuns)
                {
                    BaseStunDur = Math.Max(0f, (s.Duration / 1000f * (1f - StatS.StunDurReduc)));
                    stunActs = FightDuration / s.Frequency;
                    if (stunActs > 0f)
                    {
                        TimesFeared += stunActs;
                        if (Char.Race == CharacterRace.Human && EMMaxActs - EM.numActivates > 0f) {
                            MaxTimeRegain = Math.Max(0f, (BaseStunDur - LatentGCD - CalcOpts.React / 1000f));

                            float availEMacts = EM.ability.Activates - EM.numActivates;
                            float EMNewActs = Math.Min(stunActs, availEMacts);
                            EM.numActivates += EMNewActs;
                            _emActs = EM.numActivates;

                            //EMActualActs += EMNewActs;
                            // Use up to the maximum, leaving a 0 boundary so we don't mess up later numbers
                            reducedDur = Math.Max(0f, BaseStunDur - MaxTimeRegain);
                            float percEMdVsUnEMd = EMNewActs / stunActs;
                            timelostwhilestunned += (reducedDur * stunActs * percEMdVsUnEMd * ChanceYouAreStunned)
                                                 + (BaseStunDur * stunActs * (1f - percEMdVsUnEMd) * ChanceYouAreStunned);
                        } else {
                            timelostwhilestunned += BaseStunDur * stunActs * ChanceYouAreStunned;
                        }
                    }
                }
                if (_needDisplayCalcs)
                {
                    GCDUsage += (TimesFeared > 0 ? TimesFeared.ToString("000.00") + " x " + BaseStunDur.ToString("0.00") + "secs : Lost to Fears\n" : "");
                    GCDUsage += (EM.numActivates - EMOldActs > 0 ? (EM.numActivates - EMOldActs).ToString("000.00") + " x " + (BaseStunDur - reducedDur).ToString("0.00") + "secs : - " + EM.ability.Name + "\n" : "");
                }
                TimeLostGDCs += Math.Min(NumGCDs, (BaseStunDur * TimesFeared) / LatentGCD);
                TimeLostGDCs -= Math.Min(TimeLostGDCs, (reducedDur * _emActs) / LatentGCD);

                timelostwhilestunned = timelostwhilestunned;
                percTimeInStun = timelostwhilestunned / FightDuration;

                SecondWind SndW = GetWrapper<SecondWind>().ability as SecondWind;
                SndW.NumStunsOverDur += stunActs;
            }
            
            return percTimeInStun;*/
        }

        private float CalculateFear()
        {
            return 0f; // Not yet implemented in this model
            /*
            float percTimeInFear = 0f;
            if (CalcOpts.FearingTargets && CalcOpts.Fears.Count > 0)
            {
                float timelostwhilefeared = 0f;
                float BaseFearDur = 0f, fearActs = 0f, reducedDur = 0f,
                      MaxTimeRegain = 0f,
                      ChanceYouAreFeared = 1f;
                AbilWrapper BZ = GetWrapper<BerserkerRage>();
                float BZMaxActs = BZ.ability.Activates;
                float BZActualActs = 0f;
                AbilWrapper EM = GetWrapper<EveryManForHimself>();
                float EMMaxActs = EM.ability.Activates - EM.numActivates;
                float EMOldActs = EM.numActivates;
                TimesFeared = 0f;
                foreach (Impedence f in CalcOpts.Fears)
                {
                    BaseFearDur = Math.Max(0f, (f.Duration / 1000f * (1f - StatS.FearDurReduc)));
                    fearActs = FightDuration / f.Frequency;
                    if (fearActs > 0f)
                    {
                        TimesFeared += fearActs;
                        if (BZMaxActs - BZActualActs > 0f)
                        {
                            MaxTimeRegain = Math.Max(0f, (BaseFearDur - LatentGCD - CalcOpts.React / 1000f));
                            float BZNewActs = Math.Min(BZMaxActs - BZActualActs, fearActs);
                            BZActualActs += BZNewActs;
                            // Use up to the maximum, leaving a 0 boundary so we don't mess up later numbers
                            reducedDur = Math.Max(0f, BaseFearDur - MaxTimeRegain);
                            float percBZdVsUnBZd = BZNewActs / fearActs;
                            timelostwhilefeared += (reducedDur * fearActs * percBZdVsUnBZd * ChanceYouAreFeared)
                                                 + (BaseFearDur * fearActs * (1f - percBZdVsUnBZd) * ChanceYouAreFeared);
                        }
                        else if (Char.Race == CharacterRace.Human && EMMaxActs - EM.numActivates > 0f)
                        {
                            MaxTimeRegain = Math.Max(0f, (BaseFearDur - LatentGCD - CalcOpts.React / 1000f));

                            float availEMacts = EM.ability.Activates - EM.numActivates;
                            float EMNewActs = Math.Min(fearActs, availEMacts);
                            EM.numActivates += EMNewActs;
                            _emActs = EM.numActivates;

                            //EMActualActs += EMNewActs;
                            // Use up to the maximum, leaving a 0 boundary so we don't mess up later numbers
                            reducedDur = Math.Max(0f, BaseFearDur - MaxTimeRegain);
                            float percEMdVsUnEMd = EMNewActs / fearActs;
                            timelostwhilefeared += (reducedDur * fearActs * percEMdVsUnEMd * ChanceYouAreFeared)
                                                 + (BaseFearDur * fearActs * (1f - percEMdVsUnEMd) * ChanceYouAreFeared);
                        }
                        else
                        {
                            timelostwhilefeared += BaseFearDur * fearActs * ChanceYouAreFeared;
                        }
                    }
                }
                BZ.numActivates = BZActualActs;
                if (_needDisplayCalcs)
                {
                    GCDUsage += (TimesFeared > 0 ? TimesFeared.ToString("000.00") + " x " + BaseFearDur.ToString("0.00") + "secs : Lost to Fears\n" : "");
                    GCDUsage += (BZ.numActivates > 0 ? BZ.numActivates.ToString("000.00") + " x " + (BaseFearDur - reducedDur).ToString("0.00") + "secs : - " + BZ.ability.Name + "\n" : "");
                    GCDUsage += (EM.numActivates - EMOldActs > 0 ? (EM.numActivates - EMOldActs).ToString("000.00") + " x " + (BaseFearDur - reducedDur).ToString("0.00") + "secs : - " + EM.ability.Name + "\n" : "");
                }
                TimeLostGDCs += Math.Min(NumGCDs, (BaseFearDur * TimesFeared) / LatentGCD);
                TimeLostGDCs -= Math.Min(TimeLostGDCs, (reducedDur * BZ.numActivates) / LatentGCD);
                TimeLostGDCs -= Math.Min(TimeLostGDCs, (reducedDur * _emActs) / LatentGCD);

                timelostwhilefeared = timelostwhilefeared;
                percTimeInFear = timelostwhilefeared / FightDuration;
            }
            
            return percTimeInFear;*/
        }

        private float CalculateMovement(Ability MS)
        {
            return 0f; // Not yet handled by this model
            /*
            float percTimeInMovement = 0f;
            if (CalcOpts.MovingTargets && CalcOpts.Moves.Count > 0)
            {
                /* = Movement Speed =
                 * According to a post I found on WoWWiki, Standard (Run) Movement
                 * Speed is 7 yards per 1 sec.
                 * Cat's Swiftness (and similar) bring this to 7.56 (7x1.08)
                 * If you are moving for 5 seconds, this is 35 yards (37.8 w/ bonus)
                 * All the movement effects have a min 8 yards, so you have to be
                 * moving for 1.142857 seconds (1.08 seconds w/ bonus) before Charge
                 * would be viable. If you had to be moving more than Charge's Max
                 * Range (25 yards, editable by certain bonuses) then we'd benefit
                 * again from move speed bonuses, etc.
                 * 
                 * Charge Max = 25
                 * that's 25/7.00 = 3.571428571428571 seconds at 7.00 yards per sec
                 * that's 25/7.56 = 3.306878306873070 seconds at 7.56 yards per sec
                 * Charge (Glyph of Charge) Max = 25+5=30
                 * that's 30/7.00 = 4.285714285714286 seconds at 7.00 yards per sec
                 * that's 30/7.56 = 3.968253968253968 seconds at 7.56 yards per sec
                 * 
                 * = Now let's try and get some of those GCDs back =
                 * Let's assume that if the movement duration is longer
                 * than the before mentioned (1.142857|1.08) seconds,
                 * you are far enough away that you can use a Movement
                 * Ability (Charge, Intercept or  Intervene)
                 * Since some of these abilities are usable in combat
                 * only by talents, we have to make those checks first
                 * Since some stuff is kind of weird, we're going to
                 * enforce an ~3 sec est minimum move time before activating
                 * Charge
                 *//*
                AbilWrapper HF = GetWrapper<HeroicFury>();
                AbilWrapper CH;
                if (CalcOpts.FuryStance) CH = GetWrapper<Intercept>();
                else CH = GetWrapper<Charge>();
                
                float MovementSpeed = 7f * (1f + StatS.MovementSpeed); // 7 yards per sec * 1.08 (if have bonus) = 7.56
                float BaseMoveDur = 0f, movedActs = 0f, reducedDur = 0f,
                      MinMovementTimeRegain = 0f, MaxMovementTimeRegain = 0f,
                      ChanceYouHaveToMove = 1f;
                float ChargeMaxActs = CH.ability.Activates;
                if (CalcOpts.FuryStance && HF.ability.Validated)
                {
                    ChargeMaxActs += HF.ability.Activates - HF.numActivates;
                }
                float ChargeActualActs = 0f;
                float timelostwhilemoving = 0f;
                float moveGCDs = 0f;
                foreach (Impedence m in CalcOpts.Moves)
                {
                    BaseMoveDur = (m.Duration / 1000f * (1f - StatS.MovementSpeed));
                    moveGCDs += movedActs = FightDuration / m.Frequency;

                    if ((ChargeMaxActs - ChargeActualActs > 0f) && (movedActs > 0f))
                    {
                        MaxMovementTimeRegain = Math.Max(0f,
                            Math.Min((BaseMoveDur - CalcOpts.React / 1000f),
                                     (CH.ability.MaxRange / MovementSpeed - CalcOpts.React / 1000f)));
                        MinMovementTimeRegain = Math.Max(0f,
                            Math.Min((BaseMoveDur - CalcOpts.React / 1000f),
                                     (CH.ability.MinRange / MovementSpeed - CalcOpts.React / 1000f)));
                        if (BaseMoveDur >= MinMovementTimeRegain)
                        {
                            float ChargeNewActs = Math.Min(ChargeMaxActs - ChargeActualActs, movedActs);
                            ChargeActualActs += ChargeNewActs;
                            // Use up to the maximum, leaving a 0 boundary so we don't mess up later numbers
                            reducedDur = Math.Max(0f, BaseMoveDur - MaxMovementTimeRegain);
                            float percChargedVsUncharged = ChargeNewActs / movedActs;
                            timelostwhilemoving += (reducedDur * movedActs * percChargedVsUncharged * ChanceYouHaveToMove)
                                                 + (BaseMoveDur * movedActs * (1f - percChargedVsUncharged) * ChanceYouHaveToMove);
                        }
                    }
                    else if (movedActs > 0f)
                    {
                        timelostwhilemoving += BaseMoveDur * movedActs * ChanceYouHaveToMove;
                    }
                }
                float actsCharge = ChargeActualActs;
                
                TimeLostGDCs += Math.Min(NumGCDs, (BaseMoveDur * moveGCDs) / LatentGCD);
                TimeLostGDCs -= Math.Min(TimeLostGDCs, (reducedDur * actsCharge) / LatentGCD);
                CH.numActivates = actsCharge;
                float actsHF = 0f;
                if (CH.numActivates > CH.ability.Activates) actsHF += (CH.numActivates - CH.ability.Activates);
                HF.numActivates += actsHF;
                if (_needDisplayCalcs)
                {
                    GCDUsage += (moveGCDs > 0 ? moveGCDs.ToString("000.00") + " x " + BaseMoveDur.ToString("0.00") + "secs : Lost to Movement\n" : "");
                    GCDUsage += (actsCharge > 0 ? actsCharge.ToString("000.00") + " x " + (BaseMoveDur - reducedDur).ToString("0.00") + "secs : - " + CH.ability.Name + "\n" : "");
                    GCDUsage += (actsHF > 0 ? actsHF.ToString("000.00") + " activates of " + HF.ability.Name + " to refresh " + CH.ability.Name : "");
                }
                //RageGainedWhileMoving += CH.Rage;
                // Need to add the special effect from Juggernaut to Mortal Strike, not caring about Slam right now
                if (Talents.Juggernaut > 0 && MS != null && CH.ability is Charge)
                {
                    Stats stats = new Stats
                    {
                        BonusWarrior_T8_4P_MSBTCritIncrease = 0.25f *
                            (new SpecialEffect(Trigger.Use, null, 10, CH.ability.Cd)
                             ).GetAverageUptime(FightDuration / actsCharge, 1f, CombatFactors._c_rwItemSpeed, FightDuration)
                    };
                    stats.Accumulate(StatS);
                    // I'm not sure if this is gonna work, but hell, who knows
                    MS.BonusCritChance = stats.BonusWarrior_T8_4P_MSBTCritIncrease;
                    //MS = new Skills.MortalStrike(Char, stats, CombatFactors, WhiteAtks, CalcOpts);
                }
                timelostwhilemoving = timelostwhilemoving;
                percTimeInMovement = timelostwhilemoving / FightDuration;
            }
            return percTimeInMovement;*/
        }
        #endregion

        public void AddValidatedSpecialEffects(Stats statsTotal, HunterTalents talents)
        {
            /*Ability ST = GetWrapper<ShatteringThrow>().ability,
                    BTS = GetWrapper<BattleShout>().ability,
                    CS = GetWrapper<CommandingShout>().ability,
                    DS = GetWrapper<DemoralizingShout>().ability,
                    TH = GetWrapper<ThunderClap>().ability,
                    SN = GetWrapper<SunderArmor>().ability,
                    SW = GetWrapper<SweepingStrikes>().ability,
                    RK = GetWrapper<Recklessness>().ability;
            if (ST.Validated)
            {
                SpecialEffect shatt = new SpecialEffect(Trigger.Use,
                    new Stats() { ArmorPenetration = 0.20f, },
                    ST.Duration, ST.Cd,
                    ST.RWAtkTable.AnyLand);
                statsTotal.AddSpecialEffect(shatt);
            }
            if (BTS.Validated)
            {
                SpecialEffect bs = new SpecialEffect(Trigger.Use,
                    new Stats() { AttackPower = (548f * (1f + talents.CommandingPresence * 0.05f)), },
                    BTS.Duration, BTS.Cd);
                statsTotal.AddSpecialEffect(bs);
            }
            if (CS.Validated)
            {
                //float value = (2255f * (1f + talents.CommandingPresence * 0.05f));
                SpecialEffect cs = new SpecialEffect(Trigger.Use,
                    new Stats() { Health = 2255f * (1f + talents.CommandingPresence * 0.05f), },
                    CS.Duration, CS.Cd);
                statsTotal.AddSpecialEffect(cs);
            }
            if (DS.Validated)
            {
                //float value = (410f * (1f + talents.ImprovedDemoralizingShout * 0.08f));
                SpecialEffect ds = new SpecialEffect(Trigger.Use,
                    new Stats() { BossAttackPower = 410f * (1f + talents.ImprovedDemoralizingShout * 0.08f) * -1f, },
                    DS.Duration, DS.Cd);
                statsTotal.AddSpecialEffect(ds);
            }
            if (TH.Validated)
            {
                //float value = (0.10f * (1f + (float)Math.Ceiling(talents.ImprovedThunderClap * 10f / 3f) / 100f));
                SpecialEffect tc = new SpecialEffect(Trigger.Use,
                    new Stats() { BossAttackSpeedMultiplier = (-0.10f * (1f + talents.ImprovedThunderClap / 30f)), },
                    TH.Duration, TH.Cd, TH.RWAtkTable.AnyLand);
                statsTotal.AddSpecialEffect(tc);
            }
            if (SN.Validated)
            {
                //float value = 0.04f;
                SpecialEffect sn = new SpecialEffect(Trigger.Use,
                    new Stats() { ArmorPenetration = 0.04f, },
                    SN.Duration, SN.Cd, SN.RWAtkTable.AnyLand, 5);
                statsTotal.AddSpecialEffect(sn);
            }
            float landedAtksInterval = LandedAtksOverDur / CalcOpts.Duration;
            float critRate = CriticalAtksOverDur / AttemptedAtksOverDur;
            if (SW.Validated)
            {
                SpecialEffect sweep = new SpecialEffect(Trigger.Use,
                    new Stats() { BonusTargets = 1f, },
                    landedAtksInterval * 5f, SW.Cd);
                statsTotal.AddSpecialEffect(sweep);
            }
            /*if (RK.Validated && CalcOpts.FuryStance)
            {
                SpecialEffect reck = new SpecialEffect(Trigger.Use,
                    new Stats() { PhysicalCrit = 1f - critRate },
                    landedAtksInterval * 3f, RK.Cd);
                statsTotal.AddSpecialEffect(reck);
            }*/
            /*if (talents.Flurry > 0 && CalcOpts.FuryStance)
            {
                //float value = talents.Flurry * 0.05f;
                SpecialEffect flurry = new SpecialEffect(Trigger.MeleeCrit,
                    new Stats() { PhysicalHaste = talents.Flurry * 0.05f, }, landedAtksInterval * 3f, 0f);
                statsTotal.AddSpecialEffect(flurry);
            }*/
        }

        internal void ResetHitTables() { foreach (AbilWrapper aw in GetAbilityList()) { if (aw.ability.CanCrit) { aw.ability.RWAtkTable.Reset(); } } }
    }
}

