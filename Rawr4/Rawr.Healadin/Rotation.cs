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
                if (_calcOpts == null) _calcOpts = new CalculationOptionsHealadin();

                _bossOpts = _character.BossOptions;
                if (_bossOpts == null) _bossOpts = new BossOptions();
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

        private BossOptions _bossOpts;
        public BossOptions BossOpts
        {
            get { return _bossOpts; }
        }

        private Stats _stats;
        public Stats Stats
        {
            get { return _stats; }
            set { _stats = value; }
        }

        public float DivinePleas { get; set; }
        public float FightLength { get; set; }
        public float FoLCasts { get; set; }

        private FlashOfLight fol;
        private HolyLight hl;
        private HolyShock hs;
        private DivineLight dl;  //smm-change*************************************************
        private WordofGlory wog;  //smm-change*************************************************
        private LightofDawn lod;  //smm-change*************************************************
        private JudgementsOfThePure jotp;
        private BeaconOfLight bol;

        public Rotation(Character character, Stats stats)
        {
            Character = character;
            FightLength = BossOpts.BerserkTimer;
            Stats = stats;
            fol = new FlashOfLight(this);
            hl = new HolyLight(this);
            dl = new DivineLight(this);  
            wog = new WordofGlory(this); 
            lod = new LightofDawn(this); 
            hs = new HolyShock(this);
            jotp = new JudgementsOfThePure(this, CalcOpts.Judgement);
            bol = new BeaconOfLight(this);
        }

        public float ManaPool(CharacterCalculationsHealadin calc)
        {
            DivinePleas = (float)Math.Ceiling((FightLength - 120f) / (120f * CalcOpts.DivinePlea));

            calc.ManaBase = Stats.Mana;
            calc.ManaLayOnHands = 2175 * ((Talents.GlyphOfDivinity ? 1 : 0) + (CalcOpts.LoHSelf ? 1 : 0)) * (Talents.GlyphOfDivinity ? 2f : 1f); // TODO: Turn into a spell that's cast based on options
            calc.ManaArcaneTorrent = (Character.Race == CharacterRace.BloodElf ? Stats.Mana * .06f * (float)Math.Ceiling(FightLength / 120f - .25f) : 0);
            calc.ManaDivinePlea = Stats.Mana * 0.1f * DivinePleas;
            calc.ManaMp5 = FightLength * Stats.Mp5 / 5f;
            calc.ManaReplenishment = Stats.ManaRestoreFromMaxManaPerSecond * Stats.Mana * FightLength * CalcOpts.Replenishment;
            calc.ManaOther += Stats.ManaRestore;
            // add calc.ManaJudgements
            calc.ManaJudgements = HealadinConstants.basemana * 0.15f * jotp.Casts();
            if (Stats.HighestStat > 0)
            {
                float greatnessMana = Stats.HighestStat * StatConversion.RATING_PER_MANA;
                calc.ManaReplenishment += Stats.ManaRestoreFromMaxManaPerSecond * FightLength * greatnessMana * CalcOpts.Replenishment; // Replenishment
                calc.ManaDivinePlea += DivinePleas * greatnessMana * .1f; // Divine Plea
            }

            // check if this is correct regen per 5 seconds..  
            // combat regen = 50% of spirit regen (from Meditation), plus MP5 from gear, plus 5% base mana per 5 secs.  Base mana = 23422 at 85
            float spirit_regen = StatConversion.GetSpiritRegenSec(Stats.Spirit, Stats.Intellect) * 5f;
            calc.CombatRegenRate = spirit_regen * 0.5f + Stats.Mp5 + HealadinConstants.basemana * 0.05f;
            calc.ManaRegenRate = spirit_regen + Stats.Mp5 + HealadinConstants.basemana * 0.05f;
            calc.CombatRegenTotal = calc.CombatRegenRate * FightLength / 5f;

            return calc.ManaBase + calc.ManaDivinePlea + calc.CombatRegenTotal + calc.ManaOther +
                calc.ManaReplenishment + calc.ManaLayOnHands + calc.ManaJudgements;
        }

        public static float GetHolyLightCastsPerSec(CharacterCalculationsHealadin calc)
        {
            return (calc.RotationHL / calc.HL.CastTime()) / calc.FightLength;
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
                + calc.RotationJotP / calc.JotP.CastTime()) / calc.FightLength;
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
            calc.DL = dl;  
            calc.WoG = wog;  
            calc.LoD = lod;  
            calc.JotP = jotp;
            calc.BoL = bol;

            calc.RotationJotP = jotp.Time();
            calc.UsageJotP = jotp.Usage();

            calc.RotationBoL = bol.Time();
            calc.UsageBoL = bol.Usage();

            calc.RotationHS = hs.Time();
            calc.HealedHS = hs.Healed();
            calc.UsageHS = hs.Usage();
            #endregion

            calc.SpellPowerTotal = Stats.Intellect + Stats.SpellPower;

            #region Divine Favor
            if (Talents.DivineFavor > 0)
            {
                DivineFavor df = new DivineFavor(this);
                calc.RotationHL += df.Time();
                calc.UsageHL += df.Usage();
                calc.HealedHL += df.Healed();
            }
            #endregion

            #region Infusion of Light

            float iol_hlcasts = 0;
            if (CalcOpts.InfusionOfLight)
            {
                iol_hlcasts = hs.Casts() * hs.ChanceToCrit();

                HolyLight hl_iol = new HolyLight(this) { CastTimeReduction = 0.75f * Talents.InfusionOfLight };

                calc.UsageHL += iol_hlcasts * hl_iol.AverageCost();
                calc.RotationHL += iol_hlcasts * hl_iol.CastTime();
                calc.HealedHL += iol_hlcasts * hl_iol.AverageHealed();
            }

            #endregion

            float remainingMana = calc.TotalMana = ManaPool(calc);
            remainingMana -= calc.UsageJotP + calc.UsageBoL + calc.UsageHS + calc.UsageHL + calc.UsageFoL;

            // start with amount of time you are active in a fight
            float remainingTime = FightLength * CalcOpts.Activity;
            // now subtract time for keeping up Judgements of the Pure and Beacon.  Also subtract your Holy Shock casts.  
            // TODO: why is it subtracting Flash of Light and Holy Light casts?
            remainingTime -= calc.RotationJotP + calc.RotationBoL + calc.RotationHS + calc.RotationFoL + calc.RotationHL;

            FoLCasts = 0f;
            if (remainingMana > 0)
            {
                // Calculate how much time we have available to cast regular Holy Lights
                float hl_time_available = Math.Min(remainingTime, Math.Max(0, (remainingMana - (remainingTime * fol.MPS())) / (hl.MPS() - fol.MPS())));

                // The rest of the time will be for Flash of Light
                float fol_time = remainingTime - hl_time_available;

                if (hl_time_available == 0) // If we didn't have any time available for Holy Lights, check to see when we run out of mana while casting Flash of Light
                {
                    fol_time = Math.Min(remainingTime, remainingMana / fol.MPS());
                }
                else // If we do have time available for Holy Lights, update the Holy Light stats
                {
                    calc.HealedHL += hl.HPS() * hl_time_available;
                    calc.UsageHL += hl.MPS() * hl_time_available;
                    calc.RotationHL += hl_time_available;
                }

                // Calculate Flash of Light data
                // Determine how many total casts
                FoLCasts = fol_time / fol.CastTime();

                // Adding Flash of Light stats
                calc.RotationFoL += fol_time;
                calc.UsageFoL += fol.MPS() * fol_time;
                calc.HealedFoL += fol.HPS() * fol_time;
            }

            calc.TotalHealed = calc.HealedFoL + calc.HealedHL + calc.HealedHS;

            if (Talents.BeaconOfLight > 0)
            {
                calc.TotalHealed += calc.HealedBoL = bol.HealingDone(calc.TotalHealed);
            }

            calc.HealedOther = Stats.Healed;
            calc.HealedOther += calc.TotalHealed * Stats.ShieldFromHealedProc;

            calc.TotalHealed += calc.HealedOther;

            calc.AvgHPS = calc.TotalHealed / FightLength;
            calc.AvgHPM = calc.TotalHealed / calc.TotalMana;

            return calc.AvgHPS * (1f - CalcOpts.BurstScale);
        }

        public float CalculateBurstHealing(CharacterCalculationsHealadin calc)
        {
            return hl.HPS() * CalcOpts.BurstScale;
        }

    }
}
