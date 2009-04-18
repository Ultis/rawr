using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Rawr.Enhance
{
    class CombatStats
    {

        private CalculationOptionsEnhance _calcOpts = new CalculationOptionsEnhance();
        private Character _character = new Character();
        private Stats _stats = new Stats();
        private ShamanTalents _talents = new ShamanTalents();

        private float glancingRate = 0.24f;
        private float bloodlustHaste = 0f;
        private float chanceDodge = 0f;
        private float expertiseBonus = 0f;

        private float chanceSpellMiss = 0f;
        private float chanceWhiteMiss = 0f;
        private float chanceYellowMiss = 0f;
        private float chanceYellowCrit = 0f;
        private float chanceSpellCrit = 0f;
        private float chanceWhiteCrit = 0f;
        private float chanceMeleeHit = 0f;

        private float unhastedMHSpeed = 0f;
        private float hastedMHSpeed = 0f;
        private float unhastedOHSpeed = 0f;
        private float hastedOHSpeed = 0f;

        private float secondsToFiveStack = 0f;
        private float hitsPerSOH = 0f;
        private float hitsPerSMH = 0f;
        private float hitsPerSOHSS = 0f;
        private float hitsPerSMHSS = 0f;
        private float hitsPerSWF = 0f;
        private float hitsPerSLL = 0f;

        private float flurryUptime = 1f;
        private float edUptime = 0f;
        private float urUptime = 0f;

        private float meleeAttacksPerSec = 0f;
        private float meleeCritsPerSec = 0f;
        private float spellAttacksPerSec = 0f;
        private float spellCritsPerSec = 0f;
        private float spellCastsPerSec = 0f;
        private float spellMissesPerSec = 0f;

        public CombatStats(Character character, Stats stats)
        {
            _stats = stats;
            _character = character;
            _calcOpts = _character.CalculationOptions as CalculationOptionsEnhance;
            _talents = _character.ShamanTalents;
            UpdateCalcs();
        }

        public float GlancingRate { get { return glancingRate; } }
        public float FightLength { get { return _calcOpts.FightLength * 60f; } }
        public float BloodlustHaste { get { return bloodlustHaste; } }
        public float ChanceDodge { get { return chanceDodge; } }
        public float ExpertiseBonus { get { return expertiseBonus; } }

        public float NormalHitModifier { get { return 1 - chanceWhiteCrit - glancingRate; } }
        public float CritHitModifier { get { return chanceWhiteCrit * (2f + _stats.BonusCritMultiplier); } }
        public float GlancingHitModifier { get { return glancingRate * .35f; } }

        public float ChanceSpellHit { get { return 1 - chanceSpellMiss; } }
        public float ChanceWhiteHit { get { return 1 - chanceWhiteMiss; } }
        public float ChanceYellowHit { get { return 1 - chanceYellowMiss; } }
        public float ChanceSpellCrit { get { return chanceSpellCrit; } }
        public float ChanceWhiteCrit { get { return chanceWhiteCrit; } }
        public float ChanceYellowCrit { get { return chanceYellowCrit; } }
        public float ChanceMeleeHit { get { return chanceMeleeHit; } }

        public float UnhastedMHSpeed { get { return unhastedMHSpeed; } }
        public float HastedMHSpeed { get { return hastedMHSpeed; } }
        public float UnhastedOHSpeed { get { return unhastedOHSpeed; } }
        public float HastedOHSpeed { get { return hastedOHSpeed; } }

        public float SecondsToFiveStack { get { return secondsToFiveStack; } }
        public float HitsPerSOH { get { return hitsPerSOH; } }
        public float HitsPerSMH { get { return hitsPerSMH; } }
        public float HitsPerSOHSS { get { return hitsPerSOHSS; } }
        public float HitsPerSMHSS { get { return hitsPerSMHSS; } }
        public float HitsPerSWF { get { return hitsPerSWF; } }
        public float HitsPerSLL { get { return hitsPerSLL; } }

        public float URUptime { get { return urUptime; } }
        public float EDUptime { get { return edUptime; } }
        public float FlurryUptime { get { return flurryUptime; } }
      
        public float DamageReduction { 
            get { return 1 - ArmorCalculations.GetDamageReduction(_character.Level, _calcOpts.TargetArmor, _stats.ArmorPenetration, _stats.ArmorPenetrationRating); } 
        }

        public void UpdateCalcs()
        {
            // Melee
            float hitBonus = _stats.PhysicalHit + StatConversion.GetHitFromRating(_stats.HitRating);
            expertiseBonus = 0.0025f * (_stats.Expertise + StatConversion.GetExpertiseFromRating(_stats.ExpertiseRating));

            float meleeCritModifier = _stats.PhysicalCrit;
            float baseMeleeCrit = StatConversion.GetCritFromRating(_stats.CritMeleeRating + _stats.CritRating) + 
                                  StatConversion.GetCritFromAgility(_stats.Agility, _character.Class) + .01f * _talents.ThunderingStrikes;
            float chanceCrit = Math.Min(0.75f, (1 + _stats.BonusCritChance) * (baseMeleeCrit + meleeCritModifier) + .00005f); //fudge factor for rounding
            chanceDodge = Math.Max(0f, 0.065f - expertiseBonus);
            chanceWhiteMiss = Math.Max(0f, 0.27f - hitBonus - .02f * _talents.DualWieldSpecialization) + chanceDodge;
            chanceYellowMiss = Math.Max(0f, 0.08f - hitBonus - .02f * _talents.DualWieldSpecialization) + chanceDodge; // base miss 8% now
            chanceWhiteCrit = Math.Min(chanceCrit, 1f - glancingRate - chanceWhiteMiss);
            chanceYellowCrit = Math.Min(chanceCrit, 1f - chanceYellowMiss);

            // Spells
            float spellCritModifier = _stats.SpellCrit;
            if (_calcOpts.MainhandImbue == "Flametongue")
                spellCritModifier += _talents.GlyphofFlametongueWeapon ? .02f : 0f;
            if (_calcOpts.OffhandImbue == "Flametongue" && _talents.DualWield == 1)
                spellCritModifier += _talents.GlyphofFlametongueWeapon ? .02f : 0f;

            float hitBonusSpell = _stats.SpellHit + StatConversion.GetSpellHitFromRating(_stats.HitRating);
            chanceSpellMiss = Math.Max(0f, .17f - hitBonusSpell);
            float baseSpellCrit = StatConversion.GetSpellCritFromRating(_stats.SpellCritRating + _stats.CritRating) + 
                                  StatConversion.GetSpellCritFromIntellect(_stats.Intellect) + .01f * _talents.ThunderingStrikes;
            chanceSpellCrit = Math.Min(0.75f, (1 + _stats.BonusCritChance) * (baseSpellCrit + spellCritModifier) + .00005f); //fudge factor for rounding

            float hasteBonus = StatConversion.GetHasteFromRating(_stats.HasteRating, _character.Class);
            unhastedMHSpeed = _character.MainHand == null ? 3.0f : _character.MainHand.Item.Speed;
            unhastedOHSpeed = _character.OffHand == null ? 3.0f : _character.OffHand.Item.Speed;
           
            float baseHastedMHSpeed = unhastedMHSpeed / (1f + hasteBonus) / (1f + _stats.PhysicalHaste);
            float baseHastedOHSpeed = unhastedOHSpeed / (1f + hasteBonus) / (1f + _stats.PhysicalHaste);

            // Only MH WF for now
            float chanceToProcWFPerHit = .2f + (_character.ShamanTalents.GlyphofWindfuryWeapon ? .02f : 0f);
            float avgHitsToProcWF = 1 / chanceToProcWFPerHit;

            //The Swing Loop
            //This is where we figure out feedback systems -- WF, MW, ED, Flurry, etc.
            //It's also where we'll figure out GCD interference when we model that.
            //--------------
            flurryUptime = 1f;
            edUptime = 0f;
            urUptime = 0f;
            int stormstrikeSpeed = 8;
            float shockSpeed = 6f - (.2f * _talents.Reverberation);
            float mwPPM = 2 * _talents.MaelstromWeapon * (1 + _stats.BonusMWFreq);
            float flurryHasteBonus = .05f * _talents.Flurry + _stats.BonusFlurryHaste;
            float edCritBonus = .03f * _talents.ElementalDevastation;
            bloodlustHaste = 1 + (_calcOpts.BloodlustUptime * _stats.Bloodlust);
            hastedMHSpeed = baseHastedMHSpeed / bloodlustHaste;
            hastedOHSpeed = baseHastedOHSpeed / bloodlustHaste;
            hitsPerSMHSS = (1f - chanceYellowMiss) / stormstrikeSpeed;
            hitsPerSOHSS = _character.ShamanTalents.DualWield == 1 ? ((1f - 2 * chanceYellowMiss) / stormstrikeSpeed) : 0f; //OH only swings if MH connects
            hitsPerSLL = (1f - chanceYellowMiss) / 6f;
            float swingsPerSMHMelee = 0f;
            float swingsPerSOHMelee = 0f;
            float wfProcsPerSecond = 0f;
            float mwProcsPerSecond = 0f;
            secondsToFiveStack = 10f;
            float averageMeleeCritChance = chanceYellowCrit;
            float earthShocksPerS = (1f - chanceSpellMiss) / shockSpeed;
            float couldCritSwingsPerSecond = 0f;
            hitsPerSOH = 0f;
            hitsPerSMH = 0f;
            hitsPerSWF = 0f;
            for (int i = 0; i < 5; i++)
            {
                float bonusHaste = (1f + (flurryUptime * flurryHasteBonus)) * bloodlustHaste;
                hastedMHSpeed = baseHastedMHSpeed / bonusHaste;
                hastedOHSpeed = baseHastedOHSpeed / bonusHaste;
                swingsPerSMHMelee = 1f / hastedMHSpeed;
                swingsPerSOHMelee = 1f / hastedOHSpeed;
                
                float hitsThatProcWFPerS = (1f - chanceWhiteMiss) * swingsPerSMHMelee + hitsPerSMHSS;

                // old WF model - aka Flat Windfury Society
                /* 
                float windfuryTimeToFirstHit = hastedMHSpeed - (3 % hastedMHSpeed);
                //later -- //windfuryTimeToFirstHit = hasted
                wfProcsPerSecond = 1f / (3f + windfuryTimeToFirstHit + ((avgHitsToProcWF - 1) * hitsThatProcWFPerS));
                */
                // new WF model - slighly curved Windfury Society
                // /*
                float maxExpectedWFPerFight = hitsThatProcWFPerS * chanceToProcWFPerHit * FightLength;
                float ineligibleSeconds = maxExpectedWFPerFight * (3f - hastedMHSpeed);
                float expectedWFPerFight = hitsThatProcWFPerS * chanceToProcWFPerHit * (FightLength - ineligibleSeconds);
                wfProcsPerSecond = expectedWFPerFight / FightLength;
                // */
                hitsPerSWF = 2f * wfProcsPerSecond * (1f - chanceYellowMiss);
                
                //Due to attack table, a white swing has the same chance to crit as a yellow hit
                couldCritSwingsPerSecond = swingsPerSMHMelee + swingsPerSOHMelee + hitsPerSMHSS + hitsPerSOHSS + hitsPerSLL + hitsPerSWF;
                float swingsThatConsumeFlurryPerSecond = swingsPerSMHMelee + swingsPerSOHMelee;
                flurryUptime = 1f - (float)Math.Pow(1 - averageMeleeCritChance, (3 / swingsThatConsumeFlurryPerSecond) * couldCritSwingsPerSecond);

                hitsPerSMH = swingsPerSMHMelee * (1f - chanceWhiteMiss - chanceDodge) + hitsPerSWF + hitsPerSMHSS;
                if (_character.ShamanTalents.DualWield == 1)
                    hitsPerSOH = swingsPerSOHMelee * (1f - chanceWhiteMiss - chanceDodge) + hitsPerSOHSS + hitsPerSLL;
                mwProcsPerSecond = (mwPPM / (60f / unhastedMHSpeed)) * hitsPerSMH + (mwPPM / (60f / unhastedOHSpeed)) * hitsPerSOH;
                secondsToFiveStack /* oh but i want it now! */ = 5 / mwProcsPerSecond;

                float couldCritSpellsPerS = (earthShocksPerS + 1 / secondsToFiveStack) * (1f - chanceSpellMiss);
                edUptime = 1f - (float)Math.Pow(1 - chanceSpellCrit, 10 * couldCritSpellsPerS);

                averageMeleeCritChance = chanceYellowCrit + edUptime * edCritBonus;
            }
            urUptime = 1f - (float)Math.Pow(1 - averageMeleeCritChance, 10 * couldCritSwingsPerSecond);
            float yellowAttacksPerSecond = hitsPerSWF + hitsPerSMHSS + (_character.ShamanTalents.DualWield == 1 ? hitsPerSOHSS : 0f);

            // set output variables
            meleeAttacksPerSec = hitsPerSMH + hitsPerSOH;
            meleeCritsPerSec = meleeAttacksPerSec * chanceWhiteCrit;
            spellAttacksPerSec = 1 / secondsToFiveStack + 1 / shockSpeed;
            spellCritsPerSec = spellAttacksPerSec * ChanceSpellCrit;
            spellCastsPerSec = spellAttacksPerSec;
            spellMissesPerSec = spellAttacksPerSec * chanceSpellMiss;
            chanceMeleeHit = meleeAttacksPerSec / (swingsPerSMHMelee + swingsPerSOHMelee + 2f * wfProcsPerSecond + .25f + 1f/6f);
        }

        public float GetMeleeCritsPerSec()
        {
            return  meleeCritsPerSec;
        }

        public float GetMeleeAttacksPerSec()
        {
            return meleeAttacksPerSec;
        }

        public float GetSpellCritsPerSec()
        {
            return spellCritsPerSec;
        }

        public float GetSpellAttacksPerSec()
        {
            return spellAttacksPerSec;
        }

        public float GetSpellCastsPerSec()
        {
            return spellCastsPerSec;
        }
 
        public float GetSpellMissesPerSec()
        {
            return spellMissesPerSec;
        }
    }

    #region DPSAnalysis

    public class DPSAnalysis
    {
        float _dps = 0f;
        float _miss = -1f;
        float _dodge = -1f;
        float _glancing = -1f;
        float _hit = -1f;
        float _crit = -1f;

        public DPSAnalysis(float dps, float miss, float dodge, float glancing, float crit)
        {
            _dps = dps;
            _miss = miss;
            _dodge = dodge;
            _glancing = glancing;
            _crit = crit;
            _hit = 1f;
            if (miss > 0)
                _hit -= miss;  // only need to subtract miss from hit as at this point miss includes dodge
            if (dodge > 0)
                _miss -= dodge;  // so if we have dodge then the true miss is less dodge
            if (glancing > 0)
                _hit -= glancing;
            if (crit > 0)
                _hit -= crit;
        }

        public float dps { get { return _dps; } }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(_dps.ToString("F2", CultureInfo.InvariantCulture) + "*");
            if (_miss >= 0)
                sb.AppendLine("Miss            " + (100f * _miss).ToString("F2", CultureInfo.InvariantCulture) + "%");
            if (_dodge >= 0)
                sb.AppendLine("Dodge        " + (100f * _dodge).ToString("F2", CultureInfo.InvariantCulture) + "%");
            if (_glancing >= 0)
                sb.AppendLine("Glancing     " + (100f * _glancing).ToString("F2", CultureInfo.InvariantCulture) + "%");
            if (_hit >= 0)
                sb.AppendLine("Normal Hit  " + (100f * _hit).ToString("F2", CultureInfo.InvariantCulture) + "%");
            if (_crit >= 0)
                sb.AppendLine("Crit Hit       " + (100f * _crit).ToString("F2", CultureInfo.InvariantCulture) + "%");
            return sb.ToString();
        }
    }
    #endregion
}
