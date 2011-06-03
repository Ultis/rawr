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
        private Cleanse cleanse;

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
            cleanse = new Cleanse(this);
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
            float effective_spirit = Stats.Spirit + Stats.BonusCritChanceFrostStrike * 540 * 6 / CalcOpts.HolyShock; // add in bonus spirit from 4T11 procs
            float spirit_regen = StatConversion.GetSpiritRegenSec(effective_spirit, Stats.Intellect) * 5f;
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

            calc.JudgeCasts = jotp.Casts();
            calc.RotationJudge = jotp.Time() + CalcOpts.Userdelay * calc.JudgeCasts;
            calc.UsageJudge = jotp.Usage();
            

            if (Talents.BeaconOfLight > 0)
            {
                calc.RotationBoL = bol.Time();
                calc.UsageBoL = bol.Usage();
            }

            calc.RotationHS = hs.Time() + hs.Casts() + CalcOpts.Userdelay;
            calc.HealedHS = hs.Healed();
            calc.UsageHS = hs.Usage();

            calc.RotationLoH = loh.Casts() * (loh.CastTime() + CalcOpts.Userdelay);
            calc.HealedLoH = loh.Casts() * loh.AverageHealed();
            calc.UsageLoH = 0f; // always 0, costs no mana, code was: loh.Casts() * loh.BaseMana;

            calc.UsageCleanse = CalcOpts.Cleanse * cleanse.BaseMana;
            calc.RotationCleanse = CalcOpts.Cleanse * ( cleanse.CastTime() + CalcOpts.Userdelay);
            calc.CleanseCasts = CalcOpts.Cleanse;
            #endregion
 
            calc.HasteJotP = Talents.JudgementsOfThePure * 3f;
            calc.HasteSoL = Talents.SpeedOfLight * 1f;
            calc.SpellPowerTotal = Stats.Intellect + Stats.SpellPower;

            #region Divine Favor - old code, commented out for now
        /*    if (Talents.DivineFavor > 0)
            {
                DivineFavor df = new DivineFavor(this);
                calc.RotationHL += df.Time();
                calc.UsageHL += df.Usage();
                calc.HealedHL += df.Healed();
            } */
            #endregion

            #region active time, remaining mana / time
            float remainingMana = calc.TotalMana = ManaPool(calc);
            remainingMana -= calc.UsageJudge + calc.UsageBoL + calc.UsageHS + calc.UsageHR + calc.UsageCleanse;

            // start with amount of time you are active in a fight
            calc.ActiveTime = FightLength * CalcOpts.Activity;
            float remainingTime = calc.ActiveTime;
            // now subtract time for lots of stuff we need to cast  
            remainingTime -= calc.RotationJudge + calc.RotationBoL + calc.RotationHS + calc.RotationLoH + calc.RotationHR + calc.RotationCleanse;
            float RotationDP = DivinePleas * (calc.HS.CastTime() + CalcOpts.Userdelay);
            remainingTime -= RotationDP; // subtract divine plea cast times.  HS is also a GCD, so I just used that to calculate it
            #endregion

            #region HS and holy power
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
            calc.RotationWoG = calc.WoGCasts * (wog.CastTime() + CalcOpts.Userdelay);
            calc.UsageWoG = calc.WoGCasts * wog.BaseMana;
            calc.HealedWoG = calc.WoGCasts * wog.AverageHealed();
            calc.RotationLoD = calc.LoDCasts * (lod.CastTime() + CalcOpts.Userdelay);
            calc.UsageLoD = calc.LoDCasts * lod.BaseMana;
            calc.HealedLoD = calc.LoDCasts * lod.AverageHealed() * CalcOpts.LoDTargets;
            remainingTime -= calc.RotationWoG + calc.RotationLoD;
            #endregion

            #region Holy Radiance
            calc.HRCasts = (float)Math.Floor(FightLength / CalcOpts.HRCasts);
            calc.RotationHR = calc.HRCasts * (hr.CastTime() + CalcOpts.Userdelay);
            calc.UsageHR = calc.HRCasts * hr.BaseMana;
            calc.HealedHR = calc.HRCasts * hr.AverageHealed() * CalcOpts.HREff;
            remainingTime -= calc.RotationHR;
            #endregion

            #region Melee mana regen
            // Melee hits generate 4% base mana.  From what I read, this proc has a 4 sec CD.  15ppm max.
            float InstantCastRotationTotal = calc.RotationLoD + calc.RotationWoG + calc.RotationHS + calc.RotationHR + calc.RotationJudge + calc.RotationBoL + calc.RotationLoH + calc.RotationCleanse;
            float MeleeTime = InstantCastRotationTotal * CalcOpts.Melee;
            // Ideally the SwingTime would be the weapon speed, then logic would be added to estimate the # of procs.  As the # of swings increases, 
            // the number of procs would have dimishing returns.  (because more frequent swings means more swings happen during the 4 sec CD, and don't proc)
            // For now I am going to fudge it and estimate a proc every 5 secs.  Every 4 is unrealistic since 4 is not divisable by weapon speed,
            // so some time is always wasted.  Also consider when one alternates between swings and casts, then each swing would proc.  Whatever, 5 is good enough for now.
            float SwingTime = 5f; 
            float MeleeManaPerProc = HealadinConstants.basemana * 0.04f;
            // calc.MeleeSwings = MeleeTime / SwingTime;
            calc.MeleeProcs = MeleeTime / SwingTime; // find a way to model this better, as swings approaches MaxMeleeProcs, it should have diminishing returns somehow
            calc.ManaMelee = calc.MeleeProcs * MeleeManaPerProc;
            remainingMana += calc.ManaMelee;
            calc.TotalMana += calc.ManaMelee;
            #endregion Melee mana regen

            #region Filler casts
            // now that we did everything from the options panel, fill in the rest of the available cast time
            // we will use Holy Light to fill in the time.  If we still have mana left, we will replace Holy Light casts with Divine Light, until we run out of mana.
            // Note, when I kept the filler casts to whole numbers, Haste was not valued.  Trying the other way to see how haste is valued then.
            // DL and HL have the same casttime, so lets just figure max number of casts with time remaining
            float IoLprocs = hs.Casts() * Talents.InfusionOfLight * hs.ChanceToCrit();  // Infusion of Light procs - -0.75 sec to next HL or DL casttime per point
            remainingTime += IoLprocs * 0.75f;  // assuming we will be casting at least a few HL or DL, and can cast them when IoL procs, before next HS cast
            float fill_casts = /*(float)Math.Floor*/(remainingTime / (hl.CastTime() + CalcOpts.Userdelay));
            calc.HLCasts = 0; // Holy Light
            calc.DLCasts = 0; // Divine Light
            calc.FoLCasts = 0;
            float mana_fill_hl = fill_casts * hl.BaseMana;
            float mana_fill_dl = fill_casts * dl.BaseMana;
            if (remainingMana < mana_fill_hl)       // if you would run out of mana just casting Holy Light
            {     // then figure out how many Holy Lights you can cast
                // calc.HLCasts = /*(float)Math.Floor*/(remainingMana / hl.BaseMana);
                float timeleft = calc.HLCasts * (hl.CastTime() + + CalcOpts.Userdelay);
                float MeleeMPS = calc.MeleeProcs / MeleeManaPerProc;
                calc.HLCasts = ((remainingMana / (hl.MPS() - MeleeMPS) / hl.CastTime()));  // use time to spare to melee for more mana, for more HL casts
                remainingTime -= calc.HLCasts * (hl.CastTime() + + CalcOpts.Userdelay);
                float moremeleemana = remainingTime / SwingTime * MeleeManaPerProc;
                calc.RotationMelee = remainingTime;
                calc.MeleeProcs += remainingTime / SwingTime;
                calc.ManaMelee += moremeleemana;
                calc.TotalMana += moremeleemana;
            }
            else if (remainingMana < mana_fill_dl)  // else if you would run out of mana just casting Divine Light
            {
                remainingMana -= fill_casts * hl.BaseMana;       // how much mana do we have to spare if we casts all Holy Lights
                calc.DLCasts = /*(float)Math.Floor*/(remainingMana / (dl.BaseMana - hl.BaseMana));
                calc.HLCasts = fill_casts - calc.DLCasts;
            }
            else                                                // my God, you have a crapton of mana, start casting Flash of Light
            {
                calc.DLCasts = (IoLprocs * 0.75f) / dl.CastTime();
                remainingTime -= IoLprocs * 0.75f;
                remainingMana -= calc.DLCasts * dl.BaseMana;
                remainingMana -= fill_casts * dl.BaseMana;       // how much mana do we have to spare if we casts all Divine Lights
                calc.FoLCasts = /*(float)Math.Floor*/((remainingMana / (fol.MPS() - dl.MPS()) / fol.CastTime()));
                if (calc.FoLCasts > (remainingTime / (fol.CastTime() + CalcOpts.Userdelay)))
                    calc.FoLCasts = remainingTime / (fol.CastTime() + CalcOpts.Userdelay);
                remainingTime -= calc.FoLCasts * (fol.CastTime() + CalcOpts.Userdelay);
                calc.DLCasts += /*(float)Math.Floor*/(remainingTime / (dl.CastTime() + CalcOpts.Userdelay));
            }

            calc.RotationHL = calc.HLCasts * (hl.CastTime() + CalcOpts.Userdelay);
            calc.UsageHL = calc.HLCasts * hl.BaseMana;
            calc.HealedHL = calc.HLCasts * hl.AverageHealed();
            calc.RotationDL = calc.DLCasts * (dl.CastTime() + CalcOpts.Userdelay);
            calc.UsageDL = calc.DLCasts * dl.BaseMana;
            calc.HealedDL = calc.DLCasts * dl.AverageHealed();
            calc.RotationFoL = calc.FoLCasts * (fol.CastTime() + CalcOpts.Userdelay);
            calc.UsageFoL = calc.FoLCasts * fol.BaseMana;
            calc.HealedFoL = calc.FoLCasts * fol.AverageHealed();
            if (calc.RotationHL > (IoLprocs * 0.75f))
                calc.RotationHL -= IoLprocs * 0.75f;
            else if (calc.RotationDL > (IoLprocs * 0.75f))
                calc.RotationDL -= IoLprocs * 0.75f;
            #endregion Filler Casts

            #region Conviction
            // does enlightened judgements crit seperately from judgement?  does it count towards conviction proc?
            // does Protector of the Innocent crit seperately?  does it count towards conviction proc?
            // I'm assuming yes for all these questions for now.
            // Frankly this was my first pass at modeling Conviction.  It's far from perfect, but the results are
            // in the same ballpark as combatlogs, and should be somewhat accurate in this talent's effect on
            // the value of crit.

            float ConvictionUptime;  // time having 3 stacks up
            float ConvictionCasts = 2f * (calc.HLCasts + calc.DLCasts + calc.FoLCasts +
                                 calc.WoGCasts + calc.JudgeCasts + calc.HSCasts) + calc.LoHCasts +
                                  calc.LoDCasts * CalcOpts.LoDTargets * (5f + (Talents.GlyphOfLightOfDawn ? 1f : 0f)) + 
                                  calc.MeleeSwings;
            float CastsPerSec = ConvictionCasts / ActiveTime;
            // Average crit chance is for all spells.  Start with spellcrit, then add extra for each spell.
            float AverageCritChance = Stats.SpellCrit +
                  (hs.Casts() / ConvictionCasts * (hs.ChanceToCrit() - Stats.SpellCrit)) +
                  (calc.HLCasts / ConvictionCasts * (hl.ChanceToCrit() - Stats.SpellCrit));
            float ChancePerCastToDrop = (float)Math.Pow((1 - AverageCritChance), (CastsPerSec * 15));

            float FullStackTime =   // average time we keep 3 stacks once we have it
                // might not be the best way, but for now using the time to get to 60% chance the stack has dropped
                // considering the tail 40% can take very long to drop potentially, pulling the median out past 50%
                // combat logs show this is the correct ballpark
                  1f / CastsPerSec * (float)Math.Log(0.4f, (1f - ChancePerCastToDrop));
            float AverageStackBuildTime = // average time to build from 0 to 3 stacks
                  1f / CastsPerSec *
                  (3 / AverageCritChance);  // time to get 3 stacks, assuming you dont drop the stack along the way
            // now should add time for if stacks drop along the way
            float StackBuilds = // number of times we will build the stack in the fight
                    1 + (ActiveTime - AverageStackBuildTime) / (AverageStackBuildTime + FullStackTime);
            ConvictionUptime = StackBuilds * AverageStackBuildTime / 3f +  // lets count stack builds as having 1 stack on average for now 
                               (StackBuilds - 1f) * FullStackTime;
            float ConvictionBuff = 1f + ConvictionUptime / ActiveTime * 0.03f * Talents.Conviction;

            calc.HealedHL *= ConvictionBuff;
            calc.HealedDL *= ConvictionBuff;
            calc.HealedFoL *= ConvictionBuff;
            calc.HealedHS  *= ConvictionBuff;
            calc.HealedWoG  *= ConvictionBuff;
            calc.HealedLoD  *= ConvictionBuff;
            calc.HealedLoH *= ConvictionBuff; 

            #endregion Conviction

            #region Talent heals: Enlightened Judgement, Protector of the Innocent, Illuminated healing, Beacon
            // Enlightened Judgement talent heals
            calc.HealedJudge = calc.JudgeCasts * ej.AverageHealed() * ConvictionBuff;

            // Protector of the Innocent
            calc.PotICasts = fill_casts + calc.WoGCasts + calc.LoDCasts + calc.LoHCasts + calc.HSCasts;
            calc.HealedPotI = calc.PotICasts * poti.AverageHealed() * ConvictionBuff;

            calc.UsageTotal = calc.UsageFoL + calc.UsageDL + calc.UsageHL + calc.UsageLoD + calc.UsageWoG +
                                 calc.UsageHS + calc.UsageHR + calc.UsageJudge + calc.UsageBoL + calc.UsageCleanse;
            calc.RotationTotal = calc.RotationFoL + calc.RotationDL + calc.RotationHL + calc.RotationLoD + calc.RotationWoG + calc.RotationHS +
                                 calc.RotationHR + calc.RotationJudge + calc.RotationBoL + calc.RotationLoH + calc.RotationCleanse + calc.RotationMelee
                                 + RotationDP;  

            // Illumnated Healing
            calc.HealedIH = calc.HealedFoL + calc.HealedHL + calc.HealedHS + calc.HealedWoG + calc.HealedLoD + calc.HealedDL;
            calc.TotalHealed = calc.HealedIH + calc.HealedPotI + calc.HealedJudge;
            // BoL doesn't use LoH, but Illuminated healing does - so add LoH to IH and do final IH calcs
            calc.HealedIH += calc.HealedLoH;
            calc.HealedIH *= CalcOpts.IHEff * (0.12f + ((8 + Stats.MasteryRating / 179.28f) * 0.015f));                   
            
            // Beacon of Light
            calc.HealedBoL =  bol.HealingDone(calc.TotalHealed);
            #endregion


            // adding Holy Radiance and LoH after BoL calculation, as they do not count towards BoL heals
            calc.TotalHealed += calc.HealedHR + calc.HealedLoH + calc.HealedIH + calc.HealedBoL; 

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
