using System;
using System.Collections.Generic;
using System.Text;


namespace Rawr.Warlock
{
    public class WarlockSpellRotation
    {
        private CharacterCalculationsWarlock calculations;
        private List<Spell> spellList;
        private Spell fillerSpell;

        public float ShadowSpellsPerSecond { get; set; }
        public float FireSpellsPerSecond { get; set; }
        public float SpellsPerSecond { get; set; }
        public float AfflictionSpellsPerSecond { get; set; }
        public float NonAfflictionSpellsPerSecond { get; set; }
        public float ShadowBoltsPerSecond { get; set; }
        public float IncineratesPerSecond { get; set; }

        public float ShadowBoltCastTime
        {
            get
            {
                return fillerSpell.CastTime;
            }
        }

        public float ShadowBoltCastRatio
        {
            get
            {
                if (fillerSpell is ShadowBolt)
                    return fillerSpell.CastRatio;
                else
                    return 0;
            }
        }

        public WarlockSpellRotation(CharacterCalculationsWarlock ccw)
        {
            calculations = ccw;
            spellList = new List<Spell>();

            if (calculations.CalculationOptions.CastImmolate)
                spellList.Add(new Immolate(calculations));
            switch (calculations.CalculationOptions.CastedCurse)
            {
                case CastedCurse.CurseOfAgony:
                    spellList.Add(new CurseOfAgony(calculations));
                    break;
                case CastedCurse.CurseOfDoom:
                    spellList.Add(new CurseOfDoom(calculations));
                    break;
                case CastedCurse.CurseOfRecklessness:
                    spellList.Add(new CurseOfRecklessness(calculations));
                    break;
                case CastedCurse.CurseOfShadow:
                    spellList.Add(new CurseOfShadow(calculations));
                    break;
                case CastedCurse.CurseOfTheElements:
                    spellList.Add(new CurseOfTheElements(calculations));
                    break;
                case CastedCurse.CurseOfTongues:
                    spellList.Add(new CurseOfTongues(calculations));
                    break;
                case CastedCurse.CurseOfWeakness:
                    spellList.Add(new CurseOfWeakness(calculations));
                    break;
            }
            if (calculations.CalculationOptions.CastCorruption)
                spellList.Add(new Corruption(calculations));
            if (calculations.CalculationOptions.CastSiphonLife)
                spellList.Add(new SiphonLife(calculations));
            if (calculations.CalculationOptions.CastUnstableAffliction)
                spellList.Add(new UnstableAffliction(calculations));

            switch (calculations.CalculationOptions.FillerSpell)
            {
                case FillerSpell.Shadowbolt:
                    fillerSpell = new ShadowBolt(calculations);
                    break;
                case FillerSpell.Incinerate:
                    fillerSpell = new Incinerate(calculations);
                    break;
            }

            ShadowSpellsPerSecond = 0;
            FireSpellsPerSecond = 0;
            SpellsPerSecond = 0;
            AfflictionSpellsPerSecond = 0;
            NonAfflictionSpellsPerSecond = 0;
            ShadowBoltsPerSecond = 0;
            IncineratesPerSecond = 0;
        }

        public float Calculate(bool calcDps)
        {
            float dps = 0;
            float mpsWithoutFiller = calculations.BasicStats.Mana / calculations.CalculationOptions.FightDuration + calculations.BasicStats.Mp5 / 5;
            float remainingCastRatio = 1;

            foreach (Spell s in spellList)
            {
                s.CalculateDerivedStats(calculations);
                mpsWithoutFiller -= s.ManaPerSecond;
                remainingCastRatio -= s.CastRatio;

                if (calcDps)
                {
                    s.CalculateDamage(calculations);
                    dps += s.Damage / s.Frequency;
                }
                else
                {
                    float castsPerSecond = 1 / s.Frequency;

                    //TODO: Find out if damaging curses should count
                    if (s.BaseMinDamage > 0 || s.BasePeriodicDamage > 0)
                    {
                        if (s.MagicSchool == MagicSchool.Shadow)
                            ShadowSpellsPerSecond += castsPerSecond;
                        else if (s.MagicSchool == MagicSchool.Fire)
                            FireSpellsPerSecond += castsPerSecond;
                    }

                    SpellsPerSecond += castsPerSecond;
                    if (s.SpellTree == SpellTree.Affliction)
                        AfflictionSpellsPerSecond += castsPerSecond;
                    else
                        NonAfflictionSpellsPerSecond += castsPerSecond;
                }
            }

            fillerSpell.CalculateDerivedStats(calculations);
            float lifeTapCastTime = calculations.GlobalCooldown + calculations.CalculationOptions.Latency;
            float lifeTapMana = -(float)Math.Round((580 + calculations.ShadowDamage * 0.8f) * (1 + 0.1f * calculations.CalculationOptions.ImprovedLifeTap));

            fillerSpell.CastRatio = (lifeTapMana * fillerSpell.CastTime * remainingCastRatio - mpsWithoutFiller * fillerSpell.CastTime * lifeTapCastTime) / (lifeTapMana * fillerSpell.CastTime - fillerSpell.ManaCost * lifeTapCastTime);
            fillerSpell.ManaPerSecond *= fillerSpell.CastRatio;
            float lifeTapFrequency = lifeTapMana / (mpsWithoutFiller - fillerSpell.ManaPerSecond);
            if (lifeTapFrequency < 0)
                Console.WriteLine("shit");                    
            
            if (calcDps)
            {
                fillerSpell.CalculateDamage(calculations);
                dps += fillerSpell.Damage / fillerSpell.Frequency * fillerSpell.CastRatio;
            }
            else
            {
                float fillerHitsPerSecond = fillerSpell.CastRatio / fillerSpell.CastTime * fillerSpell.ChanceToHit;
                if (fillerSpell is ShadowBolt)
                {
                    ShadowSpellsPerSecond += fillerHitsPerSecond;
                    SpellsPerSecond += fillerHitsPerSecond;
                    NonAfflictionSpellsPerSecond += fillerHitsPerSecond;
                    ShadowBoltsPerSecond = fillerHitsPerSecond;
                    IncineratesPerSecond = 0;
                }
                else if (fillerSpell is Incinerate)
                {
                    FireSpellsPerSecond += fillerHitsPerSecond;
                    SpellsPerSecond += fillerHitsPerSecond;
                    NonAfflictionSpellsPerSecond += fillerHitsPerSecond;
                    IncineratesPerSecond = fillerHitsPerSecond;
                    ShadowBoltsPerSecond = 0;
                }
            }

            return dps;
        }

        public void CalculateAdvancedInfo()
        {
            float durationLeft = calculations.CalculationOptions.FightDuration;
            float manaLeft = calculations.BasicStats.Mana + calculations.BasicStats.Mp5 / 5 * durationLeft;

            foreach (Spell s in spellList)
            {
                s.CalculateDerivedStats(calculations);
                float numCasts = (float)Math.Ceiling(calculations.CalculationOptions.FightDuration / s.Frequency);

                durationLeft -= numCasts * s.CastTime;
                manaLeft -= numCasts * s.ManaCost;

                float castsPerSecond = 1 / s.Frequency;

                //TODO: Find out if damaging curses should count
                if (s.BaseMinDamage > 0 || s.BasePeriodicDamage > 0)
                {
                    if (s.MagicSchool == MagicSchool.Shadow)
                        ShadowSpellsPerSecond += castsPerSecond;
                    else if (s.MagicSchool == MagicSchool.Fire)
                        FireSpellsPerSecond += castsPerSecond;
                }

                SpellsPerSecond += castsPerSecond;
                if (s.SpellTree == SpellTree.Affliction)
                    AfflictionSpellsPerSecond += castsPerSecond;
                else
                    NonAfflictionSpellsPerSecond += castsPerSecond;
            }

            fillerSpell.CalculateDerivedStats(calculations);
            float lifeTapCastTime = calculations.GlobalCooldown + calculations.CalculationOptions.Latency;
            float lifeTapMana = (float)Math.Round((580 + calculations.ShadowDamage * 0.8f) * (1 + 0.1f * calculations.CalculationOptions.ImprovedLifeTap));
            float numLifeTaps = (float)Math.Ceiling((durationLeft * fillerSpell.ManaCost - manaLeft * fillerSpell.CastTime) / (lifeTapCastTime * fillerSpell.ManaCost + lifeTapMana * fillerSpell.CastTime));
            float numFillers = (float)Math.Floor((durationLeft - lifeTapCastTime * numLifeTaps) / fillerSpell.CastTime);

            float fillerHitsPerSecond = fillerSpell.ChanceToHit * numFillers / calculations.CalculationOptions.FightDuration;
            if (fillerSpell is ShadowBolt)
            {
                ShadowSpellsPerSecond += fillerHitsPerSecond;
                SpellsPerSecond += fillerHitsPerSecond;
                NonAfflictionSpellsPerSecond += fillerHitsPerSecond;
                ShadowBoltsPerSecond = fillerHitsPerSecond;
                IncineratesPerSecond = 0;
            }
            else if (fillerSpell is Incinerate)
            {
                FireSpellsPerSecond += fillerHitsPerSecond;
                SpellsPerSecond += fillerHitsPerSecond;
                NonAfflictionSpellsPerSecond += fillerHitsPerSecond;
                IncineratesPerSecond = fillerHitsPerSecond;
                ShadowBoltsPerSecond = 0;
            }
        }

        public void CalculateDps()
        {
            float durationLeft = calculations.CalculationOptions.FightDuration;
            float manaLeft = calculations.BasicStats.Mana + calculations.BasicStats.Mp5 / 5 * durationLeft;
            float totalDamage = 0;

            foreach (Spell s in spellList)
            {
                s.CalculateDerivedStats(calculations);
                s.CalculateDamage(calculations);
                float numCasts = (float)Math.Ceiling(calculations.CalculationOptions.FightDuration / s.Frequency);

                durationLeft -= numCasts * s.CastTime;
                manaLeft -= numCasts * s.ManaCost;
                totalDamage += numCasts * s.Damage;
            }

            fillerSpell.CalculateDerivedStats(calculations);
            fillerSpell.CalculateDamage(calculations);
            float lifeTapCastTime = calculations.GlobalCooldown + calculations.CalculationOptions.Latency;
            float lifeTapMana = (580 + calculations.ShadowDamage * 0.8f) * (1 + 0.1f * calculations.CalculationOptions.ImprovedLifeTap);

            float numLifeTaps = (float)Math.Ceiling((durationLeft * fillerSpell.ManaCost - manaLeft * fillerSpell.CastTime) / (lifeTapCastTime * fillerSpell.ManaCost + lifeTapMana * fillerSpell.CastTime));
            //float numFillers = (float)Math.Floor((durationLeft - lifeTapCastTime * numLifeTaps) / fillerSpell.CastTime);
            //float numLifeTaps2 = (durationLeft * fillerSpell.ManaCost - fillerSpell.CastTime * fillerSpell.ManaCost - manaLeft * fillerSpell.CastTime) / (lifeTapCastTime * fillerSpell.ManaCost + lifeTapMana * fillerSpell.CastTime);
            float numFillers = (float)Math.Floor((durationLeft - lifeTapCastTime * numLifeTaps) / fillerSpell.CastTime);

            manaLeft -= numFillers * fillerSpell.ManaCost;
            manaLeft += numLifeTaps * lifeTapMana;
            while (manaLeft < 0)
            {
                numLifeTaps++;
                manaLeft += lifeTapMana;
            }
            while (manaLeft > lifeTapMana)
            {
                numLifeTaps--;
                manaLeft -= lifeTapMana;
            }
            numFillers = (float)Math.Floor((durationLeft - lifeTapCastTime * numLifeTaps) / fillerSpell.CastTime);
            durationLeft -= numFillers * fillerSpell.CastTime + numLifeTaps * lifeTapCastTime;

            if (manaLeft < 0 || manaLeft > lifeTapMana || durationLeft > fillerSpell.CastTime)
                Console.WriteLine("sigh");

            manaLeft -= numFillers * fillerSpell.ManaCost;
            manaLeft += numLifeTaps * lifeTapMana;
            totalDamage += numFillers * fillerSpell.Damage;

            calculations.TotalDamage = (float)Math.Round(totalDamage);
            float dps = calculations.TotalDamage / calculations.CalculationOptions.FightDuration;
            calculations.SubPoints = new float[] { dps };
            calculations.OverallPoints = dps;
        }
    }
}

