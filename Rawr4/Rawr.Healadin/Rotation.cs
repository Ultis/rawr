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
        public float ActiveTime { get; set; }

        private FlashOfLight fol;
        private HolyLight hl;
        private HolyShock hs;
        private DivineLight dl;  
        private WordofGlory wog;  
        private LightofDawn lod;
        private HolyRadiance hr;    
        private JudgementsOfThePure jotp;
        private BeaconOfLight bol;
        private LayonHands loh;
        private EnlightenedJudgements ej;
        private ProtectoroftheInnocent poti;

        public Rotation(Character character, Stats stats)
        {
            Character = character;
            FightLength = BossOpts.BerserkTimer;
            ActiveTime = FightLength * CalcOpts.Activity;
            Stats = stats;
            fol = new FlashOfLight(this);
            hl = new HolyLight(this);
            dl = new DivineLight(this);  
            wog = new WordofGlory(this); 
            lod = new LightofDawn(this);
            hr = new HolyRadiance(this);
            hs = new HolyShock(this);
            jotp = new JudgementsOfThePure(this, CalcOpts.JudgementCasts);
            bol = new BeaconOfLight(this);
            loh = new LayonHands(this);
            ej = new EnlightenedJudgements(this);
            poti = new ProtectoroftheInnocent(this);
        }

        public float ManaPool(CharacterCalculationsHealadin calc)
        {
            DivinePleas = (float)Math.Ceiling((FightLength - 120f) / (120f * CalcOpts.DivinePlea));

            calc.ManaBase = Stats.Mana;
            calc.ManaLayOnHands = Stats.Mana * ((Talents.GlyphOfDivinity ? 0.1f : 0) * loh.Casts());
            calc.ManaArcaneTorrent = (Character.Race == CharacterRace.BloodElf ? Stats.Mana * .06f * (float)Math.Ceiling(FightLength / 120f - .25f) : 0);
            calc.ManaDivinePlea = Stats.Mana * (Talents.GlyphOfDivinePlea ? 0.18f : 0.12f) * DivinePleas;
            calc.ManaMp5 = FightLength * Stats.Mp5 / 5f;
            // this Stats.ManaRestoreFromMaxManaPerSecond is 0 is is messing up the replenishment calculation!
            //calc.ManaReplenishment = Stats.ManaRestoreFromMaxManaPerSecond * Stats.Mana * FightLength * CalcOpts.Replenishment;
            calc.ManaReplenishment = 0.001f * Stats.Mana * FightLength * CalcOpts.Replenishment;
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
                + calc.RotationJudge / calc.JotP.CastTime()) / calc.FightLength;
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
            calc.HR = hr; 
            calc.JotP = jotp;
            calc.BoL = bol;
            calc.LoH = loh;
            calc.PotI = poti;
            calc.EJ = ej;

            calc.RotationJudge = jotp.Time();
            calc.UsageJudge = jotp.Usage();
            calc.JudgeCasts = jotp.Casts();

            calc.RotationBoL = bol.Time();
            calc.UsageBoL = bol.Usage();

            calc.RotationHS = hs.Time();
            calc.HealedHS = hs.Healed();
            calc.UsageHS = hs.Usage();

            calc.RotationLoH = loh.Casts() * loh.CastTime();
            calc.HealedLoH = loh.Casts() * loh.AverageHealed();
            calc.UsageLoH = 0f; // always 0, costs no mana, code was: loh.Casts() * loh.BaseMana;
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

            // This infusion of Light should be dead code now
            #region Infusion of Light
            /*
            float iol_hlcasts = 0;
            if (CalcOpts.InfusionOfLight)
            {
                iol_hlcasts = hs.Casts() * hs.ChanceToCrit();

                HolyLight hl_iol = new HolyLight(this) { CastTimeReduction = 0.75f * Talents.InfusionOfLight };

                calc.UsageHL += iol_hlcasts * hl_iol.AverageCost();
                calc.RotationHL += iol_hlcasts * hl_iol.CastTime();
                calc.HealedHL += iol_hlcasts * hl_iol.AverageHealed();
            }*/

            #endregion

            float remainingMana = calc.TotalMana = ManaPool(calc);
            remainingMana -= calc.UsageJudge + calc.UsageBoL + calc.UsageHS;

            // start with amount of time you are active in a fight
            calc.ActiveTime = FightLength * CalcOpts.Activity;
            float remainingTime = calc.ActiveTime;
            // now subtract time for keeping up Judgements of the Pure and Beacon.  Also subtract your Holy Shock casts.  
            remainingTime -= calc.RotationJudge + calc.RotationBoL + calc.RotationHS + calc.RotationLoH;

            calc.HolyPowerCasts = hs.Casts() / 3f;
            if (Talents.LightOfDawn != 0)
            {
              calc.LoDCasts = (float)Math.Floor(calc.HolyPowerCasts * CalcOpts.HolyPoints);
              calc.WoGCasts = (float)Math.Floor(calc.HolyPowerCasts * (1f - CalcOpts.HolyPoints));
            }
            else {
                calc.WoGCasts = (float)Math.Floor(calc.HolyPowerCasts);
                calc.LoDCasts = 0;
            }
            calc.RotationWoG = calc.WoGCasts * wog.CastTime();
            calc.UsageWoG = calc.WoGCasts * wog.BaseMana;
            calc.HealedWoG = calc.WoGCasts * wog.AverageHealed();
            calc.RotationLoD = calc.LoDCasts * lod.CastTime();
            calc.UsageLoD = calc.LoDCasts * lod.BaseMana;
            calc.HealedLoD = calc.LoDCasts * lod.AverageHealed() * CalcOpts.LoDTargets;

            calc.HRCasts = (float)Math.Floor(FightLength / 30f * CalcOpts.HRCasts);
            calc.RotationHR = calc.HRCasts * hr.CastTime();
            calc.UsageHR = calc.HRCasts * hr.BaseMana;
            calc.HealedHR = calc.HRCasts * hr.AverageHealed() * CalcOpts.HREff;
            
            remainingTime -= calc.RotationWoG + calc.RotationLoD + calc.RotationHR;
            remainingMana -= calc.UsageHR;

            // now that we did everything from the options panel, fill in the rest of the available cast time
            // we will use Holy Light to fill in the time.  If we still have mana left, we will replace Holy Light casts with Divine Light, until we run out of mana.
            // Note, when I kept the filler casts to whole numbers, Haste was not valued.  Trying the other way to see how haste is valued then.
            #region Filler casts
            // DL and HL have the same casttime, so lets just figure max number of casts with time remaining
            float fill_casts = /*(float)Math.Floor*/(remainingTime / hl.CastTime());
            calc.HLCasts = 0; // Holy Light
            calc.DLCasts = 0; // Divine Light
            calc.FoLCasts = 0;
            float mana_fill_hl = fill_casts * hl.BaseMana;
            float mana_fill_dl = fill_casts * dl.BaseMana;
            if (remainingMana < mana_fill_hl)       // if you would run out of mana just casting Holy Light
            {                                                   // then figure out how many Holy Lights you can cast
                calc.HLCasts = /*(float)Math.Floor*/(remainingMana / hl.BaseMana);
            }
            else if (remainingMana < mana_fill_dl)  // else if you would run out of mana just casting Divine Light
            {
                remainingMana -= fill_casts * hl.BaseMana;       // how much mana do we have to spare if we casts all Holy Lights
                calc.DLCasts = /*(float)Math.Floor*/(remainingMana / (dl.BaseMana - hl.BaseMana));
                calc.HLCasts = fill_casts - calc.DLCasts;
            }
            else                                                // my God, you have a crapton of mana, start casting Flash of Light
            {
                remainingMana -= fill_casts * dl.BaseMana;       // how much mana do we have to spare if we casts all Divine Lights
                calc.FoLCasts = /*(float)Math.Floor*/((remainingMana / (fol.MPS() - dl.MPS()) / fol.CastTime()));
                remainingTime -= calc.FoLCasts * fol.CastTime();
                calc.DLCasts = /*(float)Math.Floor*/(remainingTime / dl.CastTime());
            }

            calc.RotationHL = calc.HLCasts * hl.CastTime();
            calc.UsageHL = calc.HLCasts * hl.BaseMana;
            calc.HealedHL = calc.HLCasts * hl.AverageHealed();
            calc.RotationDL = calc.DLCasts * dl.CastTime();
            calc.UsageDL = calc.DLCasts * dl.BaseMana;
            calc.HealedDL = calc.DLCasts * dl.AverageHealed();
            calc.RotationFoL = calc.FoLCasts * fol.CastTime();
            calc.UsageFoL = calc.FoLCasts * fol.BaseMana;
            calc.HealedFoL = calc.FoLCasts * fol.AverageHealed();
            #endregion

            // Enlightened Judgement talent heals
            calc.HealedJudge = calc.JudgeCasts * ej.AverageHealed();
            calc.PotICasts = fill_casts + calc.WoGCasts + calc.LoDCasts + calc.LoHCasts + calc.HSCasts;
            calc.HealedPotI = calc.PotICasts * poti.AverageHealed();

            calc.UsageTotal = calc.UsageFoL + calc.UsageDL + calc.UsageHL + calc.UsageLoD +
                              calc.UsageWoG + calc.UsageHS + calc.UsageHR + calc.UsageJudge + calc.UsageBoL;
            calc.RotationTotal = calc.RotationFoL + calc.RotationDL + calc.RotationHL + calc.RotationLoD + calc.RotationWoG +
                                 calc.RotationHS + calc.RotationHR + calc.RotationJudge + calc.RotationBoL + calc.RotationLoH;

            calc.HealedIH = calc.HealedFoL + calc.HealedHL + calc.HealedHS + calc.HealedWoG + calc.HealedLoD + calc.HealedDL;
            calc.TotalHealed = calc.HealedIH + calc.HealedPotI + calc.HealedJudge;
            calc.HealedIH += calc.HealedLoH;
            calc.HealedIH *= CalcOpts.IHEff * (0.12f + ((8 + Stats.MasteryRating / 179.28f) * 0.015f));                   

            if (Talents.BeaconOfLight > 0)
            {
                calc.TotalHealed += calc.HealedBoL = bol.HealingDone(calc.TotalHealed);
            }

            // adding Holy Radiance and LoH after BoL calculation, as they do not count towards BoL heals
            calc.TotalHealed += calc.HealedHR + calc.HealedLoH + calc.HealedIH; 

            calc.HealedOther = Stats.Healed;
            calc.HealedOther += calc.TotalHealed * Stats.ShieldFromHealedProc;

            calc.TotalHealed += calc.HealedOther;

            calc.AvgHPS = calc.TotalHealed / FightLength;
            calc.AvgHPM = calc.TotalHealed / calc.TotalMana;

            return calc.AvgHPS * (1f - CalcOpts.BurstScale);
        }

        public float CalculateBurstHealing(CharacterCalculationsHealadin calc)
        {
            return fol.HPS() * CalcOpts.BurstScale;
        }

    }
}
