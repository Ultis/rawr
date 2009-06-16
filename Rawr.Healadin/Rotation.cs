using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Healadin
{
    public class Rotation
    {


        private Character _character;
        public Character Character
        {
            get { return _character; }
            set
            {
                _character = value;
                _talents = _character.PaladinTalents;
                _calcOpts = _character.CalculationOptions as CalculationOptionsHealadin;
            }
        }

        private PaladinTalents _talents;
        public PaladinTalents Talents
        {
            get { return _talents; }
        }
        private CalculationOptionsHealadin _calcOpts;
        public CalculationOptionsHealadin CalcOpts
        {
            get { return _calcOpts; }
        }
        private Stats _stats;
        public Stats Stats
        {
            get { return _stats; }
            set { _stats = value; }
        }

        public float DivinePleas { get; set; }
        public float FightLength { get; set; }

        private FlashOfLight fol;
        private HolyLight hl;
        private HolyShock hs;
        private SacredShield ss;
        private JudgementsOfThePure jotp;
        private BeaconOfLight bol;

        public Rotation(Character character, Stats stats)
        {
            Character = character;
            FightLength = CalcOpts.Length * 60f;
            Stats = stats;
            fol = new FlashOfLight(this);
            hl = new HolyLight(this);
            hs = new HolyShock(this);
            ss = new SacredShield(this);
            jotp = new JudgementsOfThePure(this);
            bol = new BeaconOfLight(this);
        }

        public float ManaPool(CharacterCalculationsHealadin calc)
        {
            DivinePleas = (float)Math.Ceiling((FightLength - 60f) / (60f * CalcOpts.DivinePlea));

            calc.ManaBase = Stats.Mana;
            calc.ManaLayOnHands = 1950 * ((Talents.GlyphOfDivinity ? 1 : 0) + (CalcOpts.LoHSelf ? 1 : 0)) * (Talents.GlyphOfDivinity ? 2f : 1f);
            calc.ManaArcaneTorrent = (Character.Race == Character.CharacterRace.BloodElf ? Stats.Mana * .06f * (float)Math.Ceiling(FightLength / 120f - .25f) : 0);
            calc.ManaDivinePlea = Stats.Mana * .25f * DivinePleas;
            calc.ManaMp5 = FightLength * Stats.Mp5 / 5;
            calc.ManaPotion = (1 + Stats.BonusManaPotion) * CalcOpts.ManaAmt;
            calc.ManaReplenishment = Stats.ManaRestoreFromMaxManaPerSecond * Stats.Mana * FightLength * CalcOpts.Replenishment;
            calc.ManaOther += Stats.ManaRestore;
            if (Stats.HighestStat > 0)
            {
                float greatnessMana = Stats.HighestStat * 15;
                calc.ManaReplenishment += Stats.ManaRestoreFromMaxManaPerSecond * FightLength * greatnessMana * CalcOpts.Replenishment; // Replenishment
                calc.ManaDivinePlea += DivinePleas * greatnessMana * .25f; // Divine Plea
            }
            return calc.ManaBase + calc.ManaDivinePlea + calc.ManaMp5 + calc.ManaOther + calc.ManaPotion +
                calc.ManaReplenishment + calc.ManaLayOnHands;
        }

        public static float GetHealingCastsPerSec(CharacterCalculationsHealadin calc)
        {
            return (calc.RotationHL / calc.HL.CastTime()
                + calc.RotationFoL / calc.FoL.CastTime()
                + calc.RotationHS / calc.HS.CastTime()) / calc.FightLength;
        }

        public static float GetSpellCastsPerSec(CharacterCalculationsHealadin calc)
        {
            return GetHealingCastsPerSec(calc)
                + (calc.RotationBoL / calc.BoL.CastTime()
                + calc.RotationJotP / calc.JotP.CastTime()
                + calc.RotationSS / calc.SS.CastTime()) / calc.FightLength;
        }

        public static float GetSpellCritsPerSec(CharacterCalculationsHealadin calc) { return GetHealingCritsPerSec(calc); }
        public static float GetHealingCritsPerSec(CharacterCalculationsHealadin calc)
        {
            return (calc.RotationHL / calc.HL.CastTime() * calc.HL.ChanceToCrit()
                + calc.RotationFoL / calc.FoL.CastTime() * calc.FoL.ChanceToCrit()
                + calc.RotationHS / calc.HS.CastTime() * calc.HS.ChanceToCrit()) / calc.FightLength;
        }

        public float CalculateFightHealing(CharacterCalculationsHealadin calc)
        {
            #region Copying Stuff to Calc
            calc.FightLength = FightLength;

            calc.HL = hl;
            calc.FoL = fol;
            calc.HS = hs;
            calc.SS = ss;
            calc.JotP = jotp;
            calc.BoL = bol;

            calc.RotationJotP = jotp.Time();
            calc.UsageJotP = jotp.Usage();

            calc.RotationSS = ss.Time();
            calc.UsageSS = ss.Usage();
            calc.HealedSS = ss.TotalAborb();

            calc.RotationBoL = bol.Time();
            calc.UsageBoL = bol.Usage();

            calc.RotationHS = hs.Time();
            calc.HealedHS = hs.Healed();
            calc.UsageHS = hs.Usage();
            #endregion

            #region Infusion of Light
            if (CalcOpts.InfusionOfLight)
            {
                Heal hl_iol = new HolyLight(this) { ExtraCritChance = .1f * Talents.InfusionOfLight };
                float iol_hlcasts = hs.Casts() * CalcOpts.IoLHolyLight * hs.ChanceToCrit();
                calc.UsageHL += iol_hlcasts * hl_iol.AverageCost();
                calc.RotationHL += iol_hlcasts * hl_iol.CastTime();
                calc.HealedHL += iol_hlcasts * hl_iol.AverageHealed();

                float iol_folcasts = hs.Casts() * (1f - CalcOpts.IoLHolyLight) * hs.ChanceToCrit();
                calc.UsageFoL += iol_folcasts * fol.AverageCost();
                calc.RotationFoL += iol_folcasts * fol.CastTime();
                calc.HealedFoL += iol_folcasts * fol.AverageHealed();
            }
            #endregion

            #region Divine Illumination
            if (Talents.DivineIllumination > 0)
            {
                DivineIllumination di = new DivineIllumination(this);
                calc.RotationHL += di.Time();
                calc.UsageHL += di.Usage();
                calc.HealedHL += di.Healed();
            }
            #endregion

            #region Divine Favor
            if (Talents.DivineFavor > 0)
            {
                DivineFavor df = new DivineFavor(this);
                calc.RotationHL += df.Time();
                calc.UsageHL += df.Usage();
                calc.HealedHL += df.Healed();
            }
            #endregion

            float remainingMana = calc.TotalMana = ManaPool(calc);
            remainingMana -= calc.UsageJotP + calc.UsageBoL + calc.UsageHS + calc.UsageHL + calc.UsageFoL + calc.UsageSS;

            float remainingTime = FightLength * CalcOpts.Activity;
            remainingTime -= calc.RotationJotP + calc.RotationBoL + calc.RotationSS + calc.RotationHS + calc.RotationFoL + calc.RotationHL;

            if (remainingMana > 0)
            {
                float hl_time = Math.Min(remainingTime, Math.Max(0, (remainingMana - (remainingTime * fol.MPS())) / (hl.MPS() - fol.MPS())));
                float fol_time = remainingTime - hl_time;
                if (hl_time == 0)
                {
                    fol_time = Math.Min(remainingTime, remainingMana / fol.MPS());
                }

                calc.HealedHL += hl.HPS() * hl_time;
                calc.UsageHL += hl.MPS() * hl_time;
                calc.RotationHL += hl_time;

                calc.RotationFoL += fol_time;
                calc.UsageFoL += fol.MPS() * fol_time;
                calc.HealedFoL += fol.HPS() * fol_time;
            }

            calc.TotalHealed = calc.HealedFoL + calc.HealedHL + calc.HealedHS;

            if (Talents.BeaconOfLight > 0)
            {
                calc.TotalHealed += calc.HealedBoL = bol.HealingDone(calc.TotalHealed);
            }

            calc.TotalHealed += calc.HealedGHL = hl.GlyphOfHolyLight(calc.HealedHL);
            
            calc.HealedOther = Stats.Healed;
            calc.HealedOther += calc.TotalHealed * Stats.ShieldFromHealed;

            calc.TotalHealed += calc.HealedOther;
            calc.TotalHealed += calc.HealedSS;

            calc.AvgHPS = calc.TotalHealed / FightLength;
            calc.AvgHPM = calc.TotalHealed / calc.TotalMana;

            calc.FightPoints = calc.AvgHPS * (1f - CalcOpts.BurstScale);

            return calc.FightPoints + calc.BurstPoints;
        }

        public float CalculateBurstHealing(CharacterCalculationsHealadin calc)
        {
            return hl.HPS() * CalcOpts.BurstScale;
        }

    }
}
