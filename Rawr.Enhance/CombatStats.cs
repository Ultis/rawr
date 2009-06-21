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
        private float whiteCritDepression = 0.048f;
        private float yellowCritDepression = 0.018f;
        private float bloodlustHaste = 0f;
        private float chanceDodge = 0f;
        private float chanceCrit = 0f;
        private float expertiseBonus = 0f;

        float critMultiplierMelee = 0f;
        float critMultiplierSpell = 0f;

        private float chanceSpellMiss = 0f;
        private float chanceWhiteMiss = 0f;
        private float chanceYellowMiss = 0f;
        private float chanceYellowCrit = 0f;
        private float chanceSpellCrit = 0f;
        private float chanceWhiteCrit = 0f;
        private float chanceMeleeHit = 0f;
        private float overSpellHitCap = 0f;
        
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
        private float edBonusCrit = 0f;
        private float ftBonusCrit = 0f;
        private float urUptime = 0f;

        private float meleeAttacksPerSec = 0f;
        private float meleeCritsPerSec = 0f;
        private float spellAttacksPerSec = 0f;
        private float spellCritsPerSec = 0f;
        private float spellCastsPerSec = 0f;
        private float spellMissesPerSec = 0f;

        private float callOfThunder = 0f;

        private List<Ability> abilities = new List<Ability>();
        private float _gcd = 0f;

        public CombatStats(Character character, Stats stats)
        {
            _stats = stats;
            _character = character;
            _calcOpts = _character.CalculationOptions as CalculationOptionsEnhance;
            _talents = _character.ShamanTalents;
            UpdateCalcs();
            SetupAbilities();
            CalculateAbilities();
            UpdateCalcs(); // second pass to revise calcs based on new ability cooldowns
        }

        public float GCD { get { return _gcd; } }
        
        public float GlancingRate { get { return glancingRate; } }
        public float FightLength { get { return _calcOpts.FightLength * 60f; } }
        public float BloodlustHaste { get { return bloodlustHaste; } }
        public float ChanceDodge { get { return chanceDodge; } }
        public float ExpertiseBonus { get { return expertiseBonus; } }

        public float NormalHitModifier { get { return 1 - chanceWhiteCrit - glancingRate; } }
        public float CritHitModifier { get { return chanceWhiteCrit * (2f * (1f + _stats.BonusCritMultiplier)); } }
        public float GlancingHitModifier { get { return glancingRate * .7f; } }
        public float YellowHitModifier { get { return ChanceYellowHit * (1 - chanceYellowCrit); } }
        public float YellowCritModifier { get { return ChanceYellowHit * chanceYellowCrit; } }
        public float SpellHitModifier { get { return ChanceSpellHit * (1 - chanceSpellCrit); } }
        public float SpellCritModifier { get { return ChanceSpellHit * chanceSpellCrit; } }
        public float LBHitModifier { get { return ChanceSpellHit * (1 - chanceSpellCrit - callOfThunder); } }
        public float LBCritModifier { get { return ChanceSpellHit * (chanceSpellCrit + callOfThunder); } }

        public float CritMultiplierMelee { get { return critMultiplierMelee; } }
        public float CritMultiplierSpell { get { return critMultiplierSpell; } }

        public float ChanceSpellHit { get { return 1 - chanceSpellMiss; } }
        public float ChanceWhiteHit { get { return 1 - chanceWhiteMiss; } }
        public float ChanceYellowHit { get { return 1 - chanceYellowMiss; } }
        public float ChanceSpellCrit { get { return chanceSpellCrit; } }
        public float ChanceWhiteCrit { get { return chanceWhiteCrit; } }
        public float ChanceYellowCrit { get { return chanceYellowCrit; } }
        public float ChanceMeleeHit { get { return chanceMeleeHit; } }
        public float ChanceMeleeCrit { get { return chanceCrit; } }
        public float OverSpellHitCap { get { return overSpellHitCap; } }

        public float UnhastedMHSpeed { get { return unhastedMHSpeed; } }
        public float HastedMHSpeed { get { return hastedMHSpeed; } }
        public float UnhastedOHSpeed { get { return unhastedOHSpeed; } }
        public float HastedOHSpeed { get { return hastedOHSpeed; } }

        public float SecondsToFiveStack { get { return secondsToFiveStack; } }
        public float BaseShockSpeed { get { return 6f - .2f * _character.ShamanTalents.Reverberation; } }
        public float StaticShockProcsPerS { get { return (HitsPerSMH + HitsPerSOH) * .02f * _character.ShamanTalents.StaticShock; } }
        public float StaticShockAvDuration 
        { get { 
            return StaticShockProcsPerS == 0 ? 600f : ((3f + 2f * _character.ShamanTalents.StaticShock) / StaticShockProcsPerS); 
        } }
            
        public float HitsPerSOH { get { return hitsPerSOH; } }
        public float HitsPerSMH { get { return hitsPerSMH; } }
        public float HitsPerSOHSS { get { return hitsPerSOHSS; } }
        public float HitsPerSMHSS { get { return hitsPerSMHSS; } }
        public float HitsPerSWF { get { return hitsPerSWF; } }
        public float HitsPerSLL { get { return hitsPerSLL; } }

        public float URUptime { get { return urUptime; } }
        public float EDUptime { get { return edUptime; } }
        public float EDBonusCrit { get { return edBonusCrit; } }
        public float FlurryUptime { get { return flurryUptime; } }

        public float DisplayMeleeCrit { get { return chanceCrit; } }
        public float DisplayYellowCrit { get { return chanceYellowCrit + yellowCritDepression; } }
        public float DisplaySpellCrit { get { return chanceSpellCrit - ftBonusCrit; } }
      
        public float DamageReduction {
            get { return 1f - StatConversion.GetArmorDamageReduction(_character.Level, _calcOpts.TargetArmor, _stats.ArmorPenetration, 0f, _stats.ArmorPenetrationRating); }
        }

        private const float DODGE = 0.065f;
        private const float WHITE_MISS = 0.27f;
        private const float YELLOW_MISS = 0.08f;
        private const float SPELL_MISS = 0.17f;

        private void SetupAbilities()
        {
            int priority = 0;
            if (_talents.FeralSpirit == 1)
                abilities.Add(new Ability("Feral Spirits", 300f, ++priority));
            if (_talents.MaelstromWeapon > 0)
                abilities.Add(new Ability("Lightning Bolt", SecondsToFiveStack, ++priority));
            if (_talents.Stormstrike == 1)
                abilities.Add(new Ability("Stormstrike", 8f, ++priority));
            abilities.Add(new Ability("Earth Shock", BaseShockSpeed, ++priority));
            if (_talents.LavaLash == 1)
                abilities.Add(new Ability("Lava Lash", 6f, ++priority));
            if (_talents.StaticShock > 0)
                abilities.Add(new Ability("Lightning Shield", StaticShockAvDuration, ++priority));
            if (_calcOpts.Magma)
                abilities.Add(new Ability("Magma Totem", 20f, ++priority));
            else
                abilities.Add(new Ability("Searing Totem", 60f, ++priority));
            abilities.Sort();
        }

        private void CalculateAbilities()
        {
            _gcd = Math.Max(1.0f, 1.5f * (1f - StatConversion.GetSpellHasteFromRating(_stats.HasteRating)));
            float combatDuration = FightLength;
            for (float timeElapsed = 0f; timeElapsed < combatDuration; timeElapsed += _gcd)
            {
                foreach (Ability ability in abilities)
                {
                    if (ability.OffCooldown(timeElapsed))
                    {
                        ability.AddUse(timeElapsed, _calcOpts.AverageLag / 1000f);
                        break;
                    }
                }
            }
            // at this stage abilities now contains the number of procs per fight for each ability.
        }

        public float AbilityCooldown(string name)
        {
            foreach (Ability ability in abilities)
            {
                if (ability.Name.Equals(name))
                    return ability.Uses == 0 ? ability.Duration : FightLength / ability.Uses;
            }
            return FightLength;
        }

        public void UpdateCalcs()
        {
            // talents
            callOfThunder = .05f * _character.ShamanTalents.CallOfThunder;
            critMultiplierMelee = 2f * (1 + _stats.BonusCritMultiplier);
            critMultiplierSpell = (1.5f + .1f * _character.ShamanTalents.ElementalFury) * (1 + _stats.BonusSpellCritMultiplier);

            // Melee
            float hitBonus = _stats.PhysicalHit + StatConversion.GetHitFromRating(_stats.HitRating) + .02f * _talents.DualWieldSpecialization;
            expertiseBonus = 0.0025f * (_stats.Expertise + StatConversion.GetExpertiseFromRating(_stats.ExpertiseRating));

            float meleeCritModifier = _stats.PhysicalCrit;
            float baseMeleeCrit = StatConversion.GetCritFromRating(_stats.CritMeleeRating + _stats.CritRating) + 
                                  StatConversion.GetCritFromAgility(_stats.Agility, _character.Class) + .01f * _talents.ThunderingStrikes;
            chanceCrit = Math.Min(1 - glancingRate, (1 + _stats.BonusCritChance) * (baseMeleeCrit + meleeCritModifier) + .00005f); //fudge factor for rounding
            chanceDodge = Math.Max(0f, DODGE - expertiseBonus);
            chanceWhiteMiss = Math.Max(0f, WHITE_MISS - hitBonus) + chanceDodge;
            chanceYellowMiss = Math.Max(0f, YELLOW_MISS - hitBonus) + chanceDodge; // base miss 8% now
            chanceWhiteCrit = Math.Min(chanceCrit - whiteCritDepression, 1f - glancingRate - chanceWhiteMiss);
            chanceYellowCrit = Math.Min(chanceCrit - yellowCritDepression, 1f - chanceYellowMiss);

            // Spells
            ftBonusCrit = 0f;
            if (_calcOpts.MainhandImbue == "Flametongue")
                ftBonusCrit += _talents.GlyphofFlametongueWeapon ? .02f : 0f;
            if (_calcOpts.OffhandImbue == "Flametongue" && _talents.DualWield == 1)
                ftBonusCrit += _talents.GlyphofFlametongueWeapon ? .02f : 0f;
        
            float spellCritModifier = _stats.SpellCrit + ftBonusCrit;
            float hitBonusSpell = _stats.SpellHit + StatConversion.GetSpellHitFromRating(_stats.HitRating);
            chanceSpellMiss = Math.Max(0f, SPELL_MISS - hitBonusSpell);
            overSpellHitCap = Math.Max(0f, hitBonusSpell - SPELL_MISS);
            float baseSpellCrit = StatConversion.GetSpellCritFromRating(_stats.CritRating) + 
                                  StatConversion.GetSpellCritFromIntellect(_stats.Intellect) + .01f * _talents.ThunderingStrikes;
            chanceSpellCrit = Math.Min(0.75f, (1 + _stats.BonusCritChance) * (baseSpellCrit + spellCritModifier) + .00005f); //fudge factor for rounding

            float hasteBonus = StatConversion.GetHasteFromRating(_stats.HasteRating, _character.Class);
            unhastedMHSpeed = _character.MainHand == null ? 3.0f : _character.MainHand.Item.Speed;
            unhastedOHSpeed = _character.OffHand == null ? 3.0f : _character.OffHand.Item.Speed;
           
            float baseHastedMHSpeed = unhastedMHSpeed / (1f + hasteBonus) / (1f + _stats.PhysicalHaste);
            float baseHastedOHSpeed = unhastedOHSpeed / (1f + hasteBonus) / (1f + _stats.PhysicalHaste);

            // Only MH WF for now
            float chanceToProcWFPerHit = .2f + (_character.ShamanTalents.GlyphofWindfuryWeapon ? .02f : 0f);

            //The Swing Loop
            //This is where we figure out feedback systems -- WF, MW, ED, Flurry, etc.
            //It's also where we'll figure out GCD interference when we model that.
            //--------------
            flurryUptime = 1f;
            edUptime = 0f;
            urUptime = 0f;
            float stormstrikeSpeed = AbilityCooldown("Stormstrike");
            float shockSpeed = AbilityCooldown("Earth Shock");
            float lavaLashSpeed = AbilityCooldown("Lava Lash");
            float mwPPM = 2 * _talents.MaelstromWeapon * (1 + _stats.BonusMWFreq);
            float flurryHasteBonus = .05f * _talents.Flurry + _stats.BonusFlurryHaste;
            float edCritBonus = .03f * _talents.ElementalDevastation;
            float bloodlustUptime = 40f / FightLength;
            bloodlustHaste = 1 + (bloodlustUptime * _stats.Bloodlust);
            hastedMHSpeed = baseHastedMHSpeed / bloodlustHaste;
            hastedOHSpeed = baseHastedOHSpeed / bloodlustHaste;
            hitsPerSMHSS = (1f - chanceYellowMiss) / stormstrikeSpeed;
            hitsPerSOHSS = _character.ShamanTalents.DualWield == 1 ? ((1f - 2 * chanceYellowMiss) / stormstrikeSpeed) : 0f; //OH only swings if MH connects
            hitsPerSLL = (1f - chanceYellowMiss) / lavaLashSpeed;
            float swingsPerSMHMelee = 0f;
            float swingsPerSOHMelee = 0f;
            float wfProcsPerSecond = 0f;
            float mwProcsPerSecond = 0f;
            secondsToFiveStack = 10f;
            float averageMeleeCritChance = chanceYellowCrit;
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
                swingsPerSOHMelee = hastedOHSpeed == 0f ? 0f : 1f / hastedOHSpeed;
                
                float hitsThatProcWFPerS = (1f - chanceWhiteMiss) * swingsPerSMHMelee + hitsPerSMHSS;

                // new WF model - slighly curved Windfury Society
                float maxExpectedWFPerFight = hitsThatProcWFPerS * chanceToProcWFPerHit * FightLength;
                float ineligibleSeconds = maxExpectedWFPerFight * (3f - hastedMHSpeed);
                float expectedWFPerFight = hitsThatProcWFPerS * chanceToProcWFPerHit * (FightLength - ineligibleSeconds);
                wfProcsPerSecond = expectedWFPerFight / FightLength;
                hitsPerSWF = 2f * wfProcsPerSecond * (1f - chanceYellowMiss);
                
                //Due to attack table, a white swing has the same chance to crit as a yellow hit
                couldCritSwingsPerSecond = swingsPerSMHMelee + swingsPerSOHMelee + hitsPerSMHSS + hitsPerSOHSS + hitsPerSLL + hitsPerSWF;
                float swingsThatConsumeFlurryPerSecond = swingsPerSMHMelee + swingsPerSOHMelee;
                flurryUptime = 1f - (float)Math.Pow(1 - averageMeleeCritChance, (3 / swingsThatConsumeFlurryPerSecond) * couldCritSwingsPerSecond);

                hitsPerSMH = swingsPerSMHMelee * (1f - chanceWhiteMiss - chanceDodge) + hitsPerSWF + hitsPerSMHSS;
                mwProcsPerSecond = (mwPPM / (60f / unhastedMHSpeed)) * hitsPerSMH;
                if (_character.ShamanTalents.DualWield == 1 && unhastedOHSpeed != 0f)
                {
                    hitsPerSOH = swingsPerSOHMelee * (1f - chanceWhiteMiss - chanceDodge) + hitsPerSOHSS + hitsPerSLL;
                    mwProcsPerSecond += (mwPPM / (60f / unhastedOHSpeed)) * hitsPerSOH;
                }
                secondsToFiveStack = 5 / mwProcsPerSecond;

                spellAttacksPerSec = 1 / secondsToFiveStack + 1 / shockSpeed;
                float couldCritSpellsPerS = spellAttacksPerSec * (1f - chanceSpellMiss);
                edUptime = 1f - (float)Math.Pow(1 - chanceSpellCrit, 10 * couldCritSpellsPerS);

                averageMeleeCritChance = chanceYellowCrit + edUptime * edCritBonus;
            }
            urUptime = 1f - (float)Math.Pow(1 - averageMeleeCritChance, 10 * couldCritSwingsPerSecond);
            float yellowAttacksPerSecond = hitsPerSWF + hitsPerSMHSS;
            if (_character.ShamanTalents.DualWield == 1 && unhastedMHSpeed != 0)
                yellowAttacksPerSecond += hitsPerSOHSS;

            // set output variables
            edBonusCrit = edUptime * edCritBonus;
            chanceWhiteCrit += edBonusCrit;
            chanceYellowCrit += edBonusCrit;
            meleeAttacksPerSec = hitsPerSMH + hitsPerSOH;
            meleeCritsPerSec = meleeAttacksPerSec * chanceWhiteCrit;
            spellAttacksPerSec = 1 / secondsToFiveStack + 1 / shockSpeed;
            spellCritsPerSec = spellAttacksPerSec * ChanceSpellCrit;
            spellCastsPerSec = spellAttacksPerSec;
            spellMissesPerSec = spellAttacksPerSec * chanceSpellMiss;
            chanceMeleeHit = meleeAttacksPerSec / (swingsPerSMHMelee + swingsPerSOHMelee + 2f * wfProcsPerSecond + .25f + 1f/6f);
        }

        #region getters
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
        #endregion
    }

    #region DPSAnalysis

    public class DPSAnalysis
    {
        private float _dps = 0f;
        private float _miss = -1f;
        private float _dodge = -1f;
        private float _glancing = -1f;
        private float _hit = -1f;
        private float _crit = -1f;

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

    #region Ability class
    public class Ability : IComparable<Ability>
    {
        private string _name;
        private float _duration;
        private int _priority;
        private float _cooldownOver;
        private int _uses;

        public Ability(string name, float duration, int priority)
        {
            _name = name;
            _duration = duration;
            _priority = priority;
            _cooldownOver = 0f;
            _uses = 0;
        }

        public string Name { get { return _name; } }
        public float Duration { get { return _duration; } }
        public float CooldownOver { get { return _cooldownOver; } }
        public int Uses { get { return _uses; } }

        public void AddUse(float useTime, float lag)
        {
            _uses++;
            _cooldownOver = useTime + _duration + lag;
        }

        public bool OffCooldown(float starttime)
        {
            return starttime >= _cooldownOver;
        }

        public int CompareTo(Ability other)
        {
            return _priority.CompareTo(other._priority);
        }

    }
    #endregion
}
