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

        public Rotation(Character character, Stats stats)
        {
            Character = character;
            Stats = stats;
            fol = new FlashOfLight(this);
            hl = new HolyLight(this);
            hs = new HolyShock(this);
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

        public float CalculateHealing(CharacterCalculationsHealadin calc)
        {
            FightLength = CalcOpts.Length * 60f;
            CopyStats(calc);

            float gcd = 1.5f / (1f + Stats.SpellHaste);
            float benediction = 1f - .02f * Talents.Benediction;

            #region Judgements of the Pure
            if (Talents.JudgementsOfThePure > 0 && CalcOpts.JotP)
            {
                float miss_chance = (float)Math.Max(0f, .09f - Talents.EnlightenedJudgements * .02f - Stats.PhysicalHit);
                float average_casts = 1 / (1f - miss_chance);
                float judgements_cast = (float)Math.Ceiling(FightLength / 60f);

                calc.JotPCasts = judgements_cast * average_casts;
                calc.JotPUsage = 219f * judgements_cast * average_casts;
            }
            #endregion

            #region Beacon of Light
            if (Talents.BeaconOfLight > 0)
            {
                calc.BoLCasts = (float)Math.Ceiling(FightLength * CalcOpts.BoLUp / (Talents.GlyphOfBeaconOfLight ? 90f : 60f));
                calc.BoLUsage = calc.BoLCasts * ((float)Math.Round(1536f * benediction) - Stats.SpellsManaReduction);
            }
            #endregion

            #region Holy Shock
            float hs_casts = FightLength * CalcOpts.HolyShock / hs.Cooldown();
            calc.HSTime = hs_casts * hs.CastTime();
            calc.HSUsage = hs_casts * hs.AverageCost();
            calc.HSHealed = hs_casts * hs.AverageHealed();
            #endregion

            #region Sacred Shield
            calc.SSAvgAbsorb = (500f + .75f * Stats.SpellPower) * (1f + Talents.DivineGuardian * .1f);
            calc.SSCasts = (float)Math.Ceiling(FightLength / (30f * (1f + Talents.DivineGuardian * .5f)) * CalcOpts.SSUptime);
            calc.SSUsage = calc.SSCasts * 527f * benediction - Stats.SpellsManaReduction;
            calc.SSAbsorbed = (float)Math.Floor(FightLength * CalcOpts.SSUptime / (6f - Stats.SacredShieldICDReduction)) * calc.SSAvgAbsorb;
            calc.SSHPM = calc.SSAbsorbed / calc.SSUsage;
            calc.SSHPS = calc.SSAbsorbed / (calc.SSCasts * gcd);
            #endregion

            #region Infusion of Light
            if (CalcOpts.InfusionOfLight)
            {
                Spell hl_iol = new HolyLight(this) { ExtraCritChance = .1f * Talents.InfusionOfLight };
                float iol_hlcasts = hs_casts * CalcOpts.IoLHolyLight;
                calc.HLUsage += iol_hlcasts * hl_iol.AverageCost();
                calc.HLTime += iol_hlcasts * hl_iol.CastTime();
                calc.HLHealed += iol_hlcasts * hl_iol.AverageHealed();

                float iol_folcasts = hs_casts * (1f - CalcOpts.IoLHolyLight);
                calc.FoLUsage += iol_folcasts * fol.AverageCost();
                calc.FoLTime += iol_folcasts * fol.CastTime();
                calc.FoLHealed += iol_folcasts * fol.AverageHealed();
            }
            #endregion

            #region Divine Illumination
            if (Talents.DivineIllumination > 0)
            {
                float di_time = 0;
                Spell hl_di = new HolyLight(this) { DivineIllumination = true };
                calc.HLTime += di_time = (float)Math.Ceiling((FightLength - 1f) / 180f) * 15f;
                calc.HLUsage += di_time * hl_di.MPS() * CalcOpts.Activity;
                calc.HLHealed += di_time * hl_di.HPS() * CalcOpts.Activity;
            }
            #endregion

            #region Divine Favor
            if (Talents.DivineFavor > 0)
            {
                float df_casts = 0, df_manaCost = 0, df_manaSaved = 0;
                Spell hl_df = new HolyLight(this) { ExtraCritChance = 1f };
                df_casts = (float)Math.Ceiling((FightLength - .5f) / 120f);
                df_manaCost = 130f * df_casts * benediction - Stats.SpellsManaReduction;
                df_manaSaved = df_casts * (hl_df.AverageCost() - hl.AverageCost());
                calc.HLHealed += hl_df.AverageHealed() * df_casts;
                calc.HLUsage += df_manaCost - df_manaSaved;
                calc.HLTime += df_casts * hl_df.CastTime();
            }
            #endregion

            float remainingMana = calc.TotalMana = ManaPool(calc);
            remainingMana -= calc.JotPUsage + calc.BoLUsage + calc.HSUsage + calc.HLUsage + calc.FoLUsage + calc.SSUsage;

            float remainingTime = FightLength * CalcOpts.Activity;
            remainingTime -= (calc.JotPCasts + calc.BoLCasts + calc.SSCasts) * gcd + calc.HSTime + calc.HLTime + calc.FoLTime;


            calc.HLTime = Math.Min(remainingTime, Math.Max(0, (remainingMana - (remainingTime * fol.MPS())) / (hl.MPS() - fol.MPS())));
            calc.FoLTime = remainingTime - calc.HLTime;
            if (calc.HLTime == 0)
            {
                calc.FoLTime = Math.Min(remainingTime, remainingMana / fol.MPS());
            }

            calc.HLHealed += hl.HPS() * calc.HLTime;
            calc.HLUsage += hl.MPS() * calc.HLTime;

            calc.FoLHealed += fol.HPS() * calc.FoLTime;
            calc.FoLUsage += fol.MPS() * calc.FoLTime;

            calc.TotalHealed = calc.FoLHealed + calc.HLHealed + calc.HSHealed;

            if (Talents.BeaconOfLight > 0)
            {
                calc.TotalHealed += calc.BoLHealed = calc.TotalHealed * CalcOpts.BoLUp * CalcOpts.BoLEff;
            }

            calc.TotalHealed += calc.HLGlyph = calc.HLHealed * (Talents.GlyphOfHolyLight ? .1f * CalcOpts.GHL_Targets : 0f);
            calc.TotalHealed += calc.SSAbsorbed;
            calc.TotalHealed += calc.HealedOther = Stats.Healed;

            calc.AvgHPS = calc.TotalHealed / FightLength;
            calc.AvgHPM = calc.TotalHealed / calc.TotalMana;

            calc.FightPoints = calc.AvgHPS * (1f - CalcOpts.BurstScale);
            calc.BurstPoints = hl.HPS() * CalcOpts.BurstScale;

            return calc.FightPoints + calc.BurstPoints;
        }

        public void CopyStats(CharacterCalculationsHealadin calc)
        {
            calc.HSAvgHeal = hs.AverageHealed();
            calc.HSCastTime = hs.CastTime();
            calc.HSCost = hs.AverageCost();
            calc.HSCrit = hs.ChanceToCrit();
            calc.HSHPM = hs.HPM();
            calc.HSHPS = hs.HPS();

            calc.HLAvgHeal = hl.AverageHealed();
            calc.HLCastTime = hl.CastTime();
            calc.HLCost = hl.AverageCost();
            calc.HLCrit = hl.ChanceToCrit();
            calc.HLHPM = hl.HPM();
            calc.HLHPS = hl.HPS();

            calc.FoLAvgHeal = fol.AverageHealed();
            calc.FoLCastTime = fol.CastTime();
            calc.FoLCost = fol.AverageCost();
            calc.FoLCrit = fol.ChanceToCrit();
            calc.FoLHPM = fol.HPM();
            calc.FoLHPS = fol.HPS();
        }

    }
}
