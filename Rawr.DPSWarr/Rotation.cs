using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DPSWarr {
    public class Rotation {
        // Constructors
        public Rotation(Character character, WarriorTalents talents, Stats stats, CombatFactors combatFactors, Skills.WhiteAttacks whiteStats){
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
        #region ArmsRotVariables
        public float _MS_PerHit  = 0f;public float _MS_DPS  = 0f;public float _MS_GCDs  = 0f;public float _MS_GCDsD  = 0f;
        public float _RD_PerHit  = 0f;public float _RD_DPS  = 0f;public float _RD_GCDs  = 0f;public float _RD_GCDsD  = 0f;
        public float _OP_PerHit  = 0f;public float _OP_DPS  = 0f;public float _OP_GCDs  = 0f;public float _OP_GCDsD  = 0f;
        public float _SD_PerHit  = 0f;public float _SD_DPS  = 0f;public float _SD_GCDs  = 0f;public float _SD_GCDsD  = 0f;
        public float _SL_PerHit  = 0f;public float _SL_DPS  = 0f;public float _SL_GCDs  = 0f;public float _SL_GCDsD  = 0f;
        public float _BLS_PerHit = 0f;public float _BLS_DPS = 0f;public float _BLS_GCDs = 0f;public float _BLS_GCDsD = 0f;
        public float _Shout_GCDs = 0f;public float _Shout_GCDsD = 0f;
        public float _Shatt_GCDs = 0f;public float _Shatt_GCDsD = 0f;
        public float _Death_GCDs = 0f;public float _Death_GCDsD = 0f;
        public float _Reck_GCDs  = 0f;public float _Reck_GCDsD  = 0f;
        public float _DW_PerHit  = 0f;public float _DW_DPS      = 0f;
        public float _WhiteDPSMH = 0f;public float _WhiteDPSOH = 0f;public float _WhiteDPS = 0f;public float _WhitePerHit = 0f;
        public float _HS_PerHit  = 0f;public float _HS_DPS  = 0f;
        #endregion
        public float MakeRotationandDoDPS_Arms() {
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
            if (!CalcOpts.FuryStance) {
                // TODO: Enforce a "Maintaining" argument so we know if we are the ones putting this up or not
                Skills.ShatteringThrow Shatt = new Skills.ShatteringThrow(Char, StatS, CombatFactors, WhiteAtks);
                float Shatt_GCDs = Shatt.GetActivates();
                if (Shatt_GCDs > availGCDs) { Shatt_GCDs = availGCDs; }
                _Shatt_GCDs = Shatt_GCDs; _Shatt_GCDs = _Shatt_GCDs * duration / rotation;
                GCDsused += (float)System.Math.Min(NumGCDs, Shatt_GCDs);
                availGCDs = (float)System.Math.Max(0f, NumGCDs - GCDsused);
                if (availGCDs <= 0f) { return DPS_TTL; }
            }else{
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
            Skills.Bladestorm BLS = new Skills.Bladestorm(Char, StatS, CombatFactors, WhiteAtks);
            float BLS_GCDs = BLS.GetActivates();
            if (BLS_GCDs > availGCDs) { BLS_GCDs = availGCDs; }
            _BLS_GCDs = BLS_GCDs; _BLS_GCDsD = _BLS_GCDs * duration / rotation;
            GCDsused += (float)System.Math.Min(NumGCDs, BLS_GCDs*4f); // the *4 is because it is channeled over 6 secs (4 GCD's consumed from 1 activate)
            availGCDs = (float)System.Math.Max(0f, NumGCDs - GCDsused);
            _BLS_DPS = BLS.GetDPS(BLS_GCDs);
            DPS_TTL += _BLS_DPS;
            if (availGCDs <= 0f) { return DPS_TTL; }

            // Priority 1 : Mortal Strike on every CD
            Skills.MortalStrike MS = new Skills.MortalStrike(Char, StatS, CombatFactors, WhiteAtks);
            //float MS_GCDs = (float)System.Math.Floor(MS.GetActivates());
            float MS_GCDs = MS.GetActivates();
            if (MS_GCDs > availGCDs) { MS_GCDs = availGCDs; }
            _MS_GCDs = MS_GCDs; _MS_GCDsD = _MS_GCDs * duration / rotation;
            GCDsused += (float)System.Math.Min(NumGCDs, MS_GCDs);
            availGCDs = (float)System.Math.Max(0f,NumGCDs - GCDsused);
            _MS_DPS = MS.GetDPS(MS_GCDs);
            DPS_TTL += _MS_DPS;
            if (availGCDs <= 0f) { return DPS_TTL; }

            // Priority 2 : Rend on every tick off
            Skills.Rend RND = new Skills.Rend(Char, StatS, CombatFactors, WhiteAtks);
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
            Skills.OverPower OP = new Skills.OverPower(Char, StatS, CombatFactors, WhiteAtks);
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
            Skills.Suddendeath SD = new Skills.Suddendeath(Char, StatS, CombatFactors, WhiteAtks);
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
            Skills.Slam SL = new Skills.Slam(Char, StatS, CombatFactors, WhiteAtks);
            float SL_GCDs = (SL.GetValided()?availGCDs:0f);
            if (SL_GCDs > availGCDs) { SL_GCDs = availGCDs; }
            _SL_GCDs = SL_GCDs; _SL_GCDsD = _SL_GCDs * duration / rotation;
            GCDsused += (float)System.Math.Min(NumGCDs, SL_GCDs);
            availGCDs = (float)System.Math.Max(0f, NumGCDs - GCDsused);
            _SL_DPS = SL.GetDPS(SL_GCDs);
            DPS_TTL += _SL_DPS;
            //if (availGCDs <= 0f) { return DPS_TTL; } // don't leave, we wanna do HS

            // Priority 6 : Heroic Strike, when there is rage to do so, handled by the Heroic Strike class
            Skills.HeroicStrike HS = new Skills.HeroicStrike(Char, StatS, CombatFactors, WhiteAtks);
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
