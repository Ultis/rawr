using System;
using System.Collections.Generic;
using System.Text;
using Rawr.DPSWarr;

namespace Rawr.DPSWarr
{
    public class Rotation
    {
        // Constructors
        public Rotation(Character character, WarriorTalents talents, Stats stats, CombatFactors combatFactors, Skills.WhiteAttacks whiteStats)
        {
            Char = character;
            Talents = talents;
            StatS = stats;
            CombatFactors = combatFactors;
            WhiteAtks = whiteStats;
            CalcOpts = character.CalculationOptions as CalculationOptionsDPSWarr;
        }
        // Variables
        private Character CHARACTER;
        private WarriorTalents TALENTS;
        private Stats STATS;
        private CombatFactors COMBATFACTORS;
        private Skills.WhiteAttacks WHITEATTACKS;
        private CalculationOptionsDPSWarr CALCOPTS;

        public Skills.Slam SL;
        public Skills.Rend RD;
        public Skills.MortalStrike MS;
        public Skills.OverPower OP;
        public Skills.Swordspec SS;
        public Skills.SweepingStrikes SW;
        public Skills.Bladestorm BLS;
        public Skills.Suddendeath SD;
        public Skills.BloodThirst BT;
        public Skills.WhirlWind WW;
        public Skills.HeroicStrike HS;
        public Skills.BloodSurge BS;
        public Skills.DeepWounds DW;
//        public Skills.Rend RND;
        
        public const float ROTATION_LENGTH_FURY = 8.0f;
        public const float ROTATION_LENGTH_ARMS_GLYPH = 42.0f;
        public const float ROTATION_LENGTH_ARMS_NOGLYPH = 30.0f;
        // Get/Set
        public Character Char { get { return CHARACTER; } set { CHARACTER = value; } }
        public WarriorTalents Talents { get { return TALENTS; } set { TALENTS = value; } }
        public Stats StatS { get { return STATS; } set { STATS = value; } }
        public CombatFactors CombatFactors { get { return COMBATFACTORS; } set { COMBATFACTORS = value; } }
        public Skills.WhiteAttacks WhiteAtks { get { return WHITEATTACKS; } set { WHITEATTACKS = value; } }
        public CalculationOptionsDPSWarr CalcOpts { get { return CALCOPTS; } set { CALCOPTS = value; } }
        // Functions
        public void Initialize(CharacterCalculationsDPSWarr calcs)
        {
            initAbilities();
            doIterations();

            WHITEATTACKS.HS_Freq = HS.GetActivates() * COMBATFACTORS.MainHandSpeed / HS.GetRotation();
            calcs.WhiteDPSMH = (CHARACTER.MainHand == null ? 0f : WHITEATTACKS.CalcMhWhiteDPS());
            calcs.WhiteDPSOH = (CHARACTER.OffHand == null ? 0f : WHITEATTACKS.CalcOhWhiteDPS());
            calcs.WhiteDPS = calcs.WhiteDPSMH + calcs.WhiteDPSOH;
            calcs.WhiteDmg = (CHARACTER.MainHand == null ? 0f : WHITEATTACKS.MhAvgSwingDmg());
            WHITEATTACKS.HS_Freq = 0;

            calcDeepWounds();

            calcs.SL = SL;
            calcs.RD = RD;
            calcs.MS = MS;
            calcs.OP = OP;
            calcs.SS = SS;
            calcs.SW = SW;
            calcs.BLS = BLS;
            calcs.SD = SD;
            calcs.BT = BT;
            calcs.WW = WW;
            calcs.HS = HS;
            calcs.BS = BS;
            calcs.DW = DW;
            //calcs.RND = RND;
        }
        public void Initialize()
        {
            initAbilities();
            doIterations();
            calcDeepWounds();
        }

        private void initAbilities()
        {
            SL = new Skills.Slam(CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            RD = new Skills.Rend(CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);//calculatedSTATS.RendDPS = skillAttacks.Rend();
            MS = new Skills.MortalStrike(CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            OP = new Skills.OverPower(CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS); //calculatedSTATS.OverpowerDPS = skillAttacks.Overpower();
            SS = new Skills.Swordspec(CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS); //calculatedSTATS.SwordSpecDPS = skillAttacks.SwordSpec();
            SW = new Skills.SweepingStrikes(CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            BLS = new Skills.Bladestorm(CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS); //calculatedSTATS.BladestormDPS = skillAttacks.BladeStorm();
            SD = new Skills.Suddendeath(CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS); //calculatedSTATS.SuddenDeathDPS = skillAttacks.SuddenDeath();
            BT = new Skills.BloodThirst(CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            WW = new Skills.WhirlWind(CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            HS = new Skills.HeroicStrike(CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            BS = new Skills.BloodSurge(CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            DW = new Skills.DeepWounds(CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            //RND = new Skills.Rend(CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);

            HS.RageCost = heroicStrikeRageCost();
            
            SD.FreeRage = freeRage();
            
            BS.Slam = SL;
            BS.Whirlwind = WW;
            BS.Bloodthirst = BT;
            
            BLS.WhirlWind = WW;

            SS.Slam = SL;
            SS.Overpower = OP;
            SS.MortalStrike = MS;
            SS.SuddenDeath = SD;
        }
        private void doIterations()
        {
            // Fury Iteration
            HS.ActsPerSec = GetHSActivates();
            float oldHSActivates = 0.0f, newHSActivates = HS.GetActivates();
            

            while (CALCOPTS.FuryStance && Math.Abs(newHSActivates - oldHSActivates) > 0.01f)
            {
                oldHSActivates = HS.GetActivates();
                BS.hsActivates = oldHSActivates;
                _bloodsurgeRPS = HS.bloodsurgeRPS = BS.GetRageUsePerSecond();
                HS.ActsPerSec = GetHSActivates();
                newHSActivates = HS.GetActivates();
            }
            
            // Arms Iteration
            float oldSDActivates = 0.0f, newSDActivates = SD.GetActivates();
            while (!CALCOPTS.FuryStance && Math.Abs(newSDActivates - oldSDActivates) > 0.01f)
            {
                oldSDActivates = SD.GetActivates();
                SD.LandedAtksPerSec = GetLandedAtksPerSec();
                newSDActivates = SD.GetActivates();
            }
        }

        private void calcDeepWounds()
        {
            DW = new Skills.DeepWounds(CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            float MHAbilityActivates = SL.GetActivates() + MS.GetActivates() +
                OP.GetActivates() + /*SW.GetActivates() + SS.GetActivates()*/ +
                BLS.GetActivates() * 6f + BS.GetActivates() + SD.GetActivates() +
                BT.GetActivates() + WW.GetActivates();
            float OHAbilityActivates = 0f;
            if (CHARACTER.OffHand != null) { OHAbilityActivates = WW.GetActivates() + BLS.GetActivates() * 6f; }
            DW.SetAllAbilityActivates(MHAbilityActivates, OHAbilityActivates);
            _DW_PerHit = DW.GetTickSize();
            _DW_DPS = DW.GetDPS();

        }

        private float GetHSActivates()
        {
            return freeRage() / heroicStrikeRageCost();
        }
        private float GetLandedAtksPerSecNoSS()
        {
            float MS_Acts = MS.GetActivates();
            float OP_Acts = OP.GetActivates();
            float SD_Acts = SD.GetActivates();
            float SL_Acts = MS.GetRotation() / 1.5f - MS.GetActivates() - OP.GetActivates() - SD.GetActivates();

            float Dable = MS_Acts + SD_Acts + SL_Acts;
            float nonDable = OP_Acts;

            float white = (COMBATFACTORS.ProbMhWhiteHit + COMBATFACTORS.MhCrit + COMBATFACTORS.GlanceChance)
                * (COMBATFACTORS.MainHand.Speed / COMBATFACTORS.TotalHaste);

            float ProbYellowHit = (1f - COMBATFACTORS.WhiteMissChance - COMBATFACTORS.MhDodgeChance);
            float ProbYellowHitOP = (1f - COMBATFACTORS.WhiteMissChance);

            float result = white + (Dable * ProbYellowHit) + (nonDable * ProbYellowHitOP);

            return result;
        }
        public float GetLandedAtksPerSec()
        {
            return GetLandedAtksPerSecNoSS(); // TODO: add swordspec to this
        }
        public float GetCritHsSlamPerSec()
        {
            float MS_Acts = MS.GetActivates();
            float OP_Acts = OP.GetActivates();
            float SD_Acts = SD.GetActivates();
            float HS_Acts = HS.GetActivates();
            float SL_Acts = MS.GetRotation() / 1.5f - MS_Acts - OP_Acts - SD_Acts;

            float result = COMBATFACTORS.MhYellowCrit * (SL_Acts / HS.GetRotation() + HS_Acts / HS.GetRotation());

            return result;
        }


        #region Rage Calcs
        public virtual float BloodRageGainRagePerSec()
        {
            return (20f * (1f + 0.25f * Talents.ImprovedBloodrage)) /
                (60f * (1f - 1.0f / 9.0f * Talents.IntensifyRage));
        }
        public virtual float AngerManagementRagePerSec() { return Talents.AngerManagement / 3.0f; }
        public virtual float ImprovedBerserkerRagePerSec() { return Talents.ImprovedBerserkerRage * 10 / (30 * (1 - 1.0f / 9.0f * Talents.IntensifyRage)); }
        public virtual float OtherRageGenPerSec()
        {
            // Ebs: Removed a lot of this due to crit chances already being factored in the WhiteAttacks class
            float rage;// = (14.0f / 8.0f * (1 + combatFactors.MhCrit - (1.0f - combatFactors.ProbMhWhiteHit)));
            //if (combatFactors.OffHand.DPS > 0 && (combatFactors.MainHand.Slot != Item.ItemSlot.TwoHand || Talents.TitansGrip == 1))
            //    rage += 7.0f / 8.0f * (1 + combatFactors.OhCrit - (1.0f - combatFactors.ProbOhWhiteHit));
            //rage *= combatFactors.TotalHaste;
            rage /*+*/= AngerManagementRagePerSec() + ImprovedBerserkerRagePerSec() + BloodRageGainRagePerSec();

            // 4pcT7
            if (StatS.DreadnaughtBonusRageProc != 0f)
                rage += 5.0f * 0.1f * ((Talents.DeepWounds > 0f ? 1f : 0f) + (CalcOpts.FuryStance == false ? 1f / 3f : 0f));

            rage *= 1 + Talents.EndlessRage * 0.25f;

            return rage;
        }
        public virtual float neededRagePerSecond()
        {
            float BTRage = BT.GetRageUsePerSecond();
            float WWRage = WW.GetRageUsePerSecond();
            float MSRage = MS.GetRageUsePerSecond();
            float OPRage = OP.GetRageUsePerSecond();
            float SDRage = SD.GetRageUsePerSecond();
            float SlamRage = SL.GetRageUsePerSecond();
            float BloodSurgeRage = _bloodsurgeRPS;// BS.GetRageUsePerSecond();
            float BladestormRage = BLS.GetRageUsePerSecond();
            float SweepingRage = SW.GetRageUsePerSecond();
            float RendRage = RD.GetRageUsePerSecond();
            // Total
            float rage = BTRage + WWRage + MSRage + OPRage + SDRage + SlamRage +
                BloodSurgeRage + SweepingRage + BladestormRage + RendRage;
            return rage;
        }
        public virtual float neededRage()
        {
            float BTRage = BT.GetRageUsePerSecond();
            //float BTRage = BloodThirstHits() * 30; // ORIGINAL LINE

            float WWRage = WW.GetRageUsePerSecond();
            //float WWRage = WhirlWindHits() * 30; // ORIGINAL LINE

            float MSRage = MS.GetRageUsePerSecond();
            //float MSRage = MortalStrikeHits() * 30; // ORIGINAL LINE

            float OPRage = OP.GetRageUsePerSecond();
            //float OPRage = OverpowerHits() * 5; // ORIGINAL LINE

            float SDRage = SD.GetRageUsePerSecond();
            //float SDRage = SuddenDeathHits() * 10; // ORIGINAL LINE

            float SlamRage = SL.GetRageUsePerSecond();
            //float SlamRage = SlamHits() * 15; // ORIGINAL LINE

            float BloodSurgeRage = BS.GetRageUsePerSecond();
            // NO ORIGINAL LINE

            float BladestormRage = BLS.GetRageUsePerSecond();
            //float BladestormRage = BladestormHits() * 30; // ORIGINAL LINE

            float SweepingRage = SW.GetRageUsePerSecond();
            //float SweepingRage = SweepingHits() * (_talents.GlyphOfSweepingStrikes ? 0 : 30); // ORIGINAL LINE

            float RendRage = RD.GetRageUsePerSecond();
            //float RendRage = RendHits() * 10; // ORIGINAL LINE

            // Total
            float rage = BTRage + WWRage + MSRage + OPRage + SDRage + SlamRage
                + BloodSurgeRage + SweepingRage + BladestormRage + RendRage;
            return rage;
        }
        public virtual float freeRage()
        {
            if (Char.MainHand == null) { return 0f; }
            float white = WHITEATTACKS.whiteRageGenPerSec();
            float other = OtherRageGenPerSec();
            float death = SD.GetRageUsePerSecond();
            float needy = neededRagePerSecond();
            return white + other + death - needy;
        }
        public float heroicStrikeRageCost()
        {
            float rageCost = HS.RageCost;
            if (Talents.GlyphOfHeroicStrike) { rageCost -= 10.0f * CombatFactors.MhCrit; } // Glyph bonus rage on crit
            rageCost += WhiteAtks.GetSwingRage(CombatFactors.MainHand, true);
            return rageCost;
        }
        public virtual float BloodRageGain() { return (20f + 5f * Talents.ImprovedBloodrage) / (60f * (1f - 0.11f * Talents.IntensifyRage)); }
        public virtual float AngerManagementGain() { return Talents.AngerManagement / 3.0f; }
        public virtual float ImprovedBerserkerRage() { return Talents.ImprovedBerserkerRage * 10f / (30f * (1f - 0.11f * Talents.IntensifyRage)); }
        public virtual float UnbridledWrathGain() { return Talents.UnbridledWrath * 3.0f / 60.0f; }
        public virtual float OtherRage()
        {
            if (Char.MainHand == null) { return 0f; }
            float rage = (14.0f / 8.0f * (1 + CombatFactors.MhCrit - (1.0f - CombatFactors.ProbMhWhiteHit)));
            if (CombatFactors.OffHand != null && CombatFactors.OffHand.DPS > 0 &&
                (CombatFactors.MainHand.Slot != Item.ItemSlot.TwoHand || Talents.TitansGrip == 1))
                rage += 7.0f / 8.0f * (1 + CombatFactors.OhCrit - (1.0f - CombatFactors.ProbOhWhiteHit));
            rage *= CombatFactors.TotalHaste;
            rage += AngerManagementGain() + ImprovedBerserkerRage() + BloodRageGain() + UnbridledWrathGain();
            rage *= 1 + Talents.EndlessRage * 0.25f;

            return rage;
        }
        #endregion

        #region FuryRotVariables
        float _bloodsurgeRPS;

        #endregion

        #region ArmsRotVariables
        public float _MS_PerHit = 0f; public float _MS_DPS = 0f; public float _MS_GCDs = 0f; public float _MS_GCDsD = 0f;
        public float _RD_PerHit = 0f; public float _RD_DPS = 0f; public float _RD_GCDs = 0f; public float _RD_GCDsD = 0f;
        public float _OP_PerHit = 0f; public float _OP_DPS = 0f; public float _OP_GCDs = 0f; public float _OP_GCDsD = 0f;
        public float _SD_PerHit = 0f; public float _SD_DPS = 0f; public float _SD_GCDs = 0f; public float _SD_GCDsD = 0f;
        public float _SL_PerHit = 0f; public float _SL_DPS = 0f; public float _SL_GCDs = 0f; public float _SL_GCDsD = 0f;
        public float _BLS_PerHit = 0f; public float _BLS_DPS = 0f; public float _BLS_GCDs = 0f; public float _BLS_GCDsD = 0f;
        public float _Shout_GCDs = 0f; public float _Shout_GCDsD = 0f;
        public float _Shatt_GCDs = 0f; public float _Shatt_GCDsD = 0f;
        public float _Death_GCDs = 0f; public float _Death_GCDsD = 0f;
        public float _Reck_GCDs = 0f; public float _Reck_GCDsD = 0f;
        public float _DW_PerHit = 0f; public float _DW_DPS = 0f;
        public float _WhiteDPSMH = 0f; public float _WhiteDPSOH = 0f; public float _WhiteDPS = 0f; public float _WhitePerHit = 0f;
        public float _HS_PerHit = 0f; public float _HS_DPS = 0f;
        #endregion
        public float MakeRotationandDoDPS_Arms()
        {
            // Starting Numbers
            float DPS_TTL = 0f;
            float rotation = (Talents.GlyphOfRending ? ROTATION_LENGTH_ARMS_GLYPH : ROTATION_LENGTH_ARMS_NOGLYPH);
            float duration = CalcOpts.Duration;
            float NumGCDs = rotation / 1.5f;
            float GCDsused = 0f;
            float availGCDs = (float)System.Math.Max(0f, NumGCDs - GCDsused);

            if (Char.MainHand == null) { return 0f; }

            // White DPS
            /*DPS_TTL += _whiteStats.CalcMhWhiteDPS();
            // White DPS is being handled in CharacterCalculationsBase() now*/

            // Passive DPS (occurs regardless)
            /*Ability DW = new DeepWounds(_character, _stats, _combatFactors, _whiteStats);
            _DW_PerHit = DW.GetDamageOnUse();
            _DW_DPS = DW.GetDPS();
            DPS_TTL += _DW_DPS;
            // DW is being handled in GetCharacterCalcs right now*/

            // Periodic GCD users (DPS for these handled elsewhere)
            // TODO: Enforce a "Maintaining" argument so we know if we are the ones putting this up or not
            Skills.BattleShout Shout = new Skills.BattleShout(Char, StatS, CombatFactors, WhiteAtks);
            float Shout_GCDs = Shout.GetActivates();
            if (Shout_GCDs > availGCDs) { Shout_GCDs = availGCDs; }
            _Shout_GCDs = Shout_GCDs; _Shout_GCDs = _Shout_GCDs * duration / rotation;
            GCDsused += (float)System.Math.Min(NumGCDs, Shout_GCDs);
            availGCDs = (float)System.Math.Max(0f, NumGCDs - GCDsused);
            if (availGCDs <= 0f) { return DPS_TTL; }
            if (!CalcOpts.FuryStance)
            {
                // TODO: Enforce a "Maintaining" argument so we know if we are the ones putting this up or not
                Skills.ShatteringThrow Shatt = new Skills.ShatteringThrow(Char, StatS, CombatFactors, WhiteAtks);
                float Shatt_GCDs = Shatt.GetActivates();
                if (Shatt_GCDs > availGCDs) { Shatt_GCDs = availGCDs; }
                _Shatt_GCDs = Shatt_GCDs; _Shatt_GCDs = _Shatt_GCDs * duration / rotation;
                GCDsused += (float)System.Math.Min(NumGCDs, Shatt_GCDs);
                availGCDs = (float)System.Math.Max(0f, NumGCDs - GCDsused);
                if (availGCDs <= 0f) { return DPS_TTL; }
            }
            else
            {
                // TODO: Enforce a "Maintaining" argument so we know if we are the ones putting this up or not
                Skills.DeathWish Death = new Skills.DeathWish(Char, StatS, CombatFactors, WhiteAtks);
                float Death_GCDs = Death.GetActivates();
                if (Death_GCDs > availGCDs) { Death_GCDs = availGCDs; }
                _Death_GCDs = Death_GCDs; _Death_GCDs = _Death_GCDs * duration / rotation;
                GCDsused += (float)System.Math.Min(NumGCDs, Death_GCDs);
                availGCDs = (float)System.Math.Max(0f, NumGCDs - GCDsused);
                if (availGCDs <= 0f) { return DPS_TTL; }
                // TODO: Enforce a "Maintaining" argument so we know if we are the ones putting this up or not
                Skills.Recklessness Reck = new Skills.Recklessness(Char, StatS, CombatFactors, WhiteAtks);
                float Reck_GCDs = Reck.GetActivates();
                if (Reck_GCDs > availGCDs) { Reck_GCDs = availGCDs; }
                _Reck_GCDs = Reck_GCDs; _Reck_GCDs = _Reck_GCDs * duration / rotation;
                GCDsused += (float)System.Math.Min(NumGCDs, Reck_GCDs);
                availGCDs = (float)System.Math.Max(0f, NumGCDs - GCDsused);
                if (availGCDs <= 0f) { return DPS_TTL; }
            }

            // Periodic DPS (run only once every few rotations)
            //Skills.Bladestorm BLS = new Skills.Bladestorm(Char, StatS, CombatFactors, WhiteAtks);
            float BLS_GCDs = BLS.GetActivates();
            if (BLS_GCDs > availGCDs) { BLS_GCDs = availGCDs; }
            _BLS_GCDs = BLS_GCDs; _BLS_GCDsD = _BLS_GCDs * duration / rotation;
            GCDsused += (float)System.Math.Min(NumGCDs, BLS_GCDs * 4f); // the *4 is because it is channeled over 6 secs (4 GCD's consumed from 1 activate)
            availGCDs = (float)System.Math.Max(0f, NumGCDs - GCDsused);
            _BLS_DPS = BLS.GetDPS(BLS_GCDs);
            DPS_TTL += _BLS_DPS;
            if (availGCDs <= 0f) { return DPS_TTL; }

            // Priority 1 : Mortal Strike on every CD
            //Skills.MortalStrike MS = new Skills.MortalStrike(Char, StatS, CombatFactors, WhiteAtks);
            //float MS_GCDs = (float)System.Math.Floor(MS.GetActivates());
            float MS_GCDs = MS.GetActivates();
            if (MS_GCDs > availGCDs) { MS_GCDs = availGCDs; }
            _MS_GCDs = MS_GCDs; _MS_GCDsD = _MS_GCDs * duration / rotation;
            GCDsused += (float)System.Math.Min(NumGCDs, MS_GCDs);
            availGCDs = (float)System.Math.Max(0f, NumGCDs - GCDsused);
            _MS_DPS = MS.GetDPS(MS_GCDs);
            DPS_TTL += _MS_DPS;
            if (availGCDs <= 0f) { return DPS_TTL; }

            // Priority 2 : Rend on every tick off
            Skills.Rend RND = RD;// new Skills.Rend(Char, StatS, CombatFactors, WhiteAtks);
            //float RND_GCDs = (float)System.Math.Floor(RND.GetActivates());
            float RND_GCDs = RND.GetActivates();
            if (RND_GCDs > availGCDs) { RND_GCDs = availGCDs; }
            _RD_GCDs = RND_GCDs; _RD_GCDsD = _RD_GCDs * duration / rotation;
            GCDsused += (float)System.Math.Min(NumGCDs, RND_GCDs);
            availGCDs = (float)System.Math.Max(0f, NumGCDs - GCDsused);
            _RD_DPS = RND.GetDPS(RND_GCDs);
            DPS_TTL += _RD_DPS;
            if (availGCDs <= 0f) { return DPS_TTL; }

            // Priority 3 : Taste for Blood Proc (Do Overpower) if available
            //Skills.OverPower OP = new Skills.OverPower(Char, StatS, CombatFactors, WhiteAtks);
            //float OP_GCDs = (float)System.Math.Floor(OP.GetActivates());
            float OP_GCDs = OP.GetActivates();
            if (OP_GCDs > availGCDs) { OP_GCDs = availGCDs; }
            _OP_GCDs = OP_GCDs; _OP_GCDsD = _OP_GCDs * duration / rotation;
            GCDsused += (float)System.Math.Min(NumGCDs, OP_GCDs);
            availGCDs = (float)System.Math.Max(0f, NumGCDs - GCDsused);
            _OP_DPS = OP.GetDPS(OP_GCDs);
            DPS_TTL += _OP_DPS;
            if (availGCDs <= 0f) { return DPS_TTL; }

            // Priority 4 : Sudden Death Proc (Do Execute) if available
            //Skills.Suddendeath SD = new Skills.Suddendeath(Char, StatS, CombatFactors, WhiteAtks);
            //float SD_GCDs = (float)System.Math.Floor(SD.GetActivates(false));
            float SD_GCDs = SD.GetActivates(false);
            if (SD_GCDs > availGCDs) { SD_GCDs = availGCDs; }
            _SD_GCDs = SD_GCDs; _SD_GCDsD = _SD_GCDs * duration / rotation;
            GCDsused += (float)System.Math.Min(NumGCDs, SD_GCDs);
            availGCDs = (float)System.Math.Max(0f, NumGCDs - GCDsused);
            _SD_DPS = SD.GetDPS(SD_GCDs);
            DPS_TTL += _SD_DPS;
            if (availGCDs <= 0f) { return DPS_TTL; }

            // Priority 5 : Slam for remainder of GCDs
            //Skills.Slam SL = new Skills.Slam(Char, StatS, CombatFactors, WhiteAtks);
            float SL_GCDs = (SL.GetValided() ? availGCDs : 0f);
            if (SL_GCDs > availGCDs) { SL_GCDs = availGCDs; }
            _SL_GCDs = SL_GCDs; _SL_GCDsD = _SL_GCDs * duration / rotation;
            GCDsused += (float)System.Math.Min(NumGCDs, SL_GCDs);
            availGCDs = (float)System.Math.Max(0f, NumGCDs - GCDsused);
            _SL_DPS = SL.GetDPS(SL_GCDs);
            DPS_TTL += _SL_DPS;
            //if (availGCDs <= 0f) { return DPS_TTL; } // don't leave, we wanna do HS

            // Priority 6 : Heroic Strike, when there is rage to do so, handled by the Heroic Strike class
            //Skills.HeroicStrike HS = new Skills.HeroicStrike(Char, StatS, CombatFactors, WhiteAtks);
            WhiteAtks.HS_Freq = HS.GetActivates() * CombatFactors.MainHandSpeed / HS.GetRotation();
            _WhiteDPSMH = WhiteAtks.CalcMhWhiteDPS();
            _WhiteDPS = _WhiteDPSMH;
            _WhitePerHit = WhiteAtks.MhAvgSwingDmg();
            WhiteAtks.HS_Freq = 0;
            _HS_DPS = HS.GetDPS();
            _HS_PerHit = HS.GetDamageOnUse();
            //calculatedStats.HS = HS;

            // Return result
            return DPS_TTL;
        }
    }
}
