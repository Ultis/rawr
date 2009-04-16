using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.HolyPriest
{

    public class BaseSolver
    {
        public Character character;
        public Stats stats;
        public float baseMana;
        public CalculationOptionsPriest calculationOptions;

        public string Role { get; protected set; }
        public string ActionList { get; protected set; }

        public List<ManaSource> ManaSources = new List<ManaSource>();
        public class ManaSource
        {
            public string Name { get; set; }
            public float Value { get; set; }

            public ManaSource(string name, float value)
            {
                Name = name; Value = value;
            }
        }

        public float ProcInterval(float ProcChance, float ProcDelay, float ProcCooldown)
        {
            float ProcActual = 1f - (float)Math.Pow(1f - ProcChance, 1f / ProcChance);
            float EffCooldown = ProcCooldown + ProcDelay * 0.5f + (float)Math.Log(ProcChance) / (float)Math.Log(ProcActual) / ProcDelay / ProcActual;
            return EffCooldown;
        }

        public BaseSolver(Stats _stats, Character _char, float _baseMana)
        {
            stats = _stats;
            character = _char;
            baseMana = _baseMana;
            calculationOptions = character.CalculationOptions as CalculationOptionsPriest;

            Role = string.Empty;
            ActionList = "Cast List:";
        }

        public virtual void Calculate(CharacterCalculationsHolyPriest calculatedStats)
        {
            ActionList += "- Virtual.";
            calculatedStats.HPSBurstPoints = calculatedStats.HPSSustainPoints = calculatedStats.SurvivabilityPoints = 0;
        }
    }

    public class Solver : BaseSolver
    {
        private int Rotation;
      
        public Solver(Stats _stats, Character _char, float _baseMana)
            : base(_stats, _char, _baseMana)
        {
            Rotation = calculationOptions.Rotation;

            if (Rotation == 0) // OOOH MAGIC TANK ROTATION!!!
            {
                Role = "Auto ";
                if (character.PriestTalents.Penance > 0)
                {
                    if (character.PriestTalents.DivineFury < 5)
                        Rotation = 8; // Disc-MT, Using Flash Heal instead of GH
                    else
                        Rotation = 7; // Disc-MT
                }
                else
                    Rotation = 5; // Holy-MT
            }
            else if (Rotation == 1) // Raid rotation
            {
                Role = "Auto ";
                if (character.PriestTalents.Penance > 0)
                    Rotation = 9; // Disc-Raid (PW:S/Penance/Flash)
                else if (character.PriestTalents.CircleOfHealing > 0)
                    Rotation = 6; // Holy-Raid (CoH/FH)
                else
                    Rotation = 3; // Fallback to Flash Heal raid.

            }
        }

        public override void Calculate(CharacterCalculationsHolyPriest calculatedStats)
        {
            Stats simstats = calculatedStats.BasicStats.Clone();

            Stats UseProcs = new Stats();

            // Pre calc Procs (Power boosting Procs)
            if (calculationOptions.ModelProcs)
            {
                if (simstats.SpiritFor20SecOnUse2Min > 0)
                    // Trinkets with Use: Increases Spirit with. (Like Earring of Soulful Meditation / Bangle of Endless blessings)
                    UseProcs.Spirit += simstats.SpiritFor20SecOnUse2Min * 20f / 120f;
                //                if (simstats.BangleProc > 0)
                // Bangle of Endless Blessings. Use: 130 spirit over 20 seconds. 120 sec cd.
                //UseProcs.Spirit += 130f * 20f / 120f;              
                if (simstats.SpellPowerFor15SecOnUse2Min > 0)
                    UseProcs.SpellPower += simstats.SpellPowerFor15SecOnUse2Min * 15f / 120f;
                if (simstats.SpellPowerFor15SecOnUse90Sec > 0)
                    UseProcs.SpellPower += simstats.SpellPowerFor15SecOnUse90Sec * 15f / 90f;
                if (simstats.SpellPowerFor20SecOnUse2Min > 0)
                    UseProcs.SpellPower += simstats.SpellPowerFor20SecOnUse2Min * 20f / 120f;
                if (simstats.HasteRatingFor20SecOnUse5Min > 0)
                    UseProcs.SpellHaste += StatConversion.GetSpellHasteFromRating(simstats.HasteRatingFor20SecOnUse5Min) * 20f / 300f;
                if (simstats.HasteRatingFor20SecOnUse2Min > 0)
                    UseProcs.SpellHaste += StatConversion.GetSpellHasteFromRating(simstats.HasteRatingFor20SecOnUse2Min) * 20f / 120f;
            }

            UseProcs.Spirit = (float)Math.Round(UseProcs.Spirit * (1 + simstats.BonusSpiritMultiplier));
            UseProcs.SpellPower += (float)Math.Round(UseProcs.Spirit * simstats.SpellDamageFromSpiritPercentage);

            simstats += UseProcs;

            // Insightful Earthstorm Diamond.
            float metaSpellCostReduction = simstats.ManaRestoreOnCast_5_15 * 0.05f;
//            float hcchance = (character.PriestTalents.HolyConcentration * 0.1f + character.PriestTalents.ImprovedHolyConcentration * .05f)
//                * (simstats.SpellCrit + character.PriestTalents.HolySpecialization * 0.01f);
            // improved holy concentration removed in patch 3.1
            float hcchance = (character.PriestTalents.HolyConcentration * 0.1f)
                * (simstats.SpellCrit + character.PriestTalents.HolySpecialization * 0.01f);
            float ihcastshasted = 2f * hcchance - (float)Math.Pow(hcchance, 2f);
            float ihchaste = 0f; // character.PriestTalents.ImprovedHolyConcentration * 0.1f; // improved holy concentration removed in patch 3.1
            float solchance = (character.PriestTalents.HolySpecialization * 0.01f + simstats.SpellCrit) * character.PriestTalents.SurgeOfLight * 0.25f;
            float sol5chance = 1f - (float)Math.Pow(1f - solchance, 5);
            float serendipityconst = calculationOptions.Serendipity / 100f * character.PriestTalents.Serendipity * 0.25f / 3f;
            float raptureconst = CalculationsHolyPriest.GetRaptureConst(character) * simstats.Mana / baseMana * character.PriestTalents.Rapture / 5f * calculationOptions.Rapture / 100f;
            float healmultiplier = (1 + character.PriestTalents.TestOfFaith * 0.02f * calculationOptions.TestOfFaith / 100f) * (1 + character.PriestTalents.Grace * 0.03f) * (1 + simstats.HealingReceivedMultiplier);

            // Test of Faith gives 2-6% extra crit on targets below 50%.
            // this is no longer true in patch 3.1
            //simstats.SpellCrit += character.PriestTalents.TestOfFaith * 0.02f * calculationOptions.TestOfFaith / 100f;

            // Add on Renewed Hope crit for Disc Maintank Rotation.
            if (Rotation == 7)
                simstats.SpellCrit += character.PriestTalents.RenewedHope * 0.02f;

            //Spell spell;
            Heal gh = new Heal(simstats, character);
            FlashHeal fh = new FlashHeal(simstats, character);
            CircleOfHealing coh = new CircleOfHealing(simstats, character);
            Penance penance = new Penance(simstats, character);
            PowerWordShield pws = new PowerWordShield(simstats, character);
            PrayerOfMending prom_1 = new PrayerOfMending(simstats, character, 1);
            PrayerOfMending prom_max = new PrayerOfMending(simstats, character);
            Renew renew = new Renew(simstats, character);

            // Surge of Light Flash Heal (cannot crit, is free)
            FlashHeal fh_sol = new FlashHeal(simstats, character);
            fh_sol.SurgeOfLight();

            // Improved Holy Concentration Haste
            simstats.SpellHaste += ihchaste;
            Heal gh_hc = new Heal(simstats, character);
            FlashHeal fh_hc = new FlashHeal(simstats, character);
            FlashHeal fh_hc_sol = new FlashHeal(simstats, character);
            fh_hc_sol.SurgeOfLight();
            simstats.SpellHaste -= ihchaste;

            // Borrowed Time Haste
            simstats.SpellHaste += character.PriestTalents.BorrowedTime * 0.05f;
            Heal gh_bt = new Heal(simstats, character);
            FlashHeal fh_bt = new FlashHeal(simstats, character);
            Penance penance_bt = new Penance(simstats, character);
            simstats.SpellHaste -= character.PriestTalents.BorrowedTime * 0.05f;

            List<Spell> sr = new List<Spell>();
            switch (Rotation)
            {
                case 2:     // Greater Heal Spam
                    Role += "Greater Heal";
                    sr.Add(gh);
                    break;
                case 3:     // Flash Heal Spam
                    Role += "Flash Heal";
                    sr.Add(fh);
                    break;
                case 4:     // Circle of Healing Spam
                    Role += "Circle of Healing";
                    sr.Add(coh);
                    sr.Add(fh);
                    sr.Add(fh);
                    sr.Add(fh);
                    break;
                case 5:     // Holy MT Healing, renew + prom + ghx5 repeat
                    Role += "Holy Tank";
                    sr.Add(renew);      // 1.5s 1.5  -13.5 -??.?
                    sr.Add(prom_1);     // 1.5s 3.0  -12.0 -8.5
                    sr.Add(gh);         // 2.5s 5.5  -9.5  -6
                    sr.Add(gh);         // 2.5s 8.0  -7.0  -3.5
                    sr.Add(gh);         // 2.5s 10.5 -4.5  -1.0
                    sr.Add(gh);         // 2.5s 13.0 -2    -??
                    sr.Add(gh);         // 2.5s 15.5 -??   -??   Although, adjusted for haste and improved holy conc, this gets better and better.
                    break;
                case 6:     // Holy Raid Healing, prom, cohx2, fh, cohx2, fh
                    Role += "Holy Raid";
                    sr.Add(prom_max);   // 1.5s 1.5 -8.5
                    sr.Add(coh);        // 1.5s 3.0 -7.0
                    sr.Add(fh);        // 1.5s 4.5 -5.5
                    sr.Add(fh);         // 1.5s 6.0 -4.0
                    // Repeat
                    break;
                case 7:     // Disc MT Healing, pws, penance, prom, gh, penance
                    Role += "Disc Tank w/Gheal";
                    sr.Add(pws);        // 1.5s 1.5  -15.0 -2.5 -??   -??
                    sr.Add(penance_bt); // 1.5s 3.0  -13.5 -1.0 -??   -6.5
                    sr.Add(prom_1);     // 1.5s 4.5  -12.0 -??  -8.5  -5.0
                    sr.Add(gh);         // 2.5s 7.0  -9.5  -??  -6.0  -2.5
                    sr.Add(gh);         // 2.5s 9.5  -7.0  -??  -3.5  -??
                    sr.Add(penance);    // 2.0s 11.5 -5.0  -??  -1.5  -6.0
                    sr.Add(gh);         // 2.5s 14.0 -2.5  -??  -??   -3.5
                    sr.Add(prom_1);     // 1.5s 17.0 -1.0  -??  -8.5  -2.0
                    sr.Add(gh);         // 2.5s 19.5 -1.5  -??  -6.0  -??
                    sr.Add(penance);    // 2.0s 20.5 -0.5  -??  -5.0  -6.0
                    sr.Add(gh);         // 2.5s 23.0 -??   -??  -3.5  -3.5
                    // repeat
                    break;
                case 8:     // Disc MT Healing, pws, penance, prom, fh - Does not have Divine Fury.
                    Role += "Disc Tank w/Fheal";
                    sr.Add(pws);        // 1.5s 1.5  -15.0 -2.5 -??   -??
                    sr.Add(penance_bt); // 1.5s 3.0  -13.5 -1.0 -??   -6.5
                    sr.Add(prom_1);     // 1.5s 4.5  -12.0 -??  -8.5  -5.0
                    sr.Add(fh);         // 1.5s 6.0  -10.5 -??  -7.0  -3.5
                    sr.Add(fh);         // 1.5s 7.5  -9.0  -??  -5.5  -2.0
                    sr.Add(fh);         // 1.5s 9.0  -7.5  -??  -4.0  -0.5
                    sr.Add(fh);         // 1.5s 10.5 -6.0  -??  -2.5  -??
                    sr.Add(penance);    // 2.0s 12.5 -4.0  -??  -0.5  -6.0
                    sr.Add(fh);         // 1.5s 14.0 -2.5  -??  -0.5  -4.5
                    sr.Add(fh);         // 1.5s 15.5 -1.0  -??  -0.5  -3.0
                    sr.Add(prom_1);     // 1.5s 12.5 -4.0  -??  -8.5  -1.5
                    sr.Add(fh);         // 1.5s 14.0 -2.5  -??  -7.0  -??
                    sr.Add(penance);    // 2.0s 16.0 -0.5  -??  -5.0  -6.0
                    sr.Add(fh);         // 1.5s 19.0 -??   -??  -3.5  -4.5
                    sr.Add(fh);         // 1.5s 19.0 -??   -??  -3.5  -3.0
                    // repeat
                    break;
                case 9:     // Disc Raid Healing, pws, penance, prom, pw:s, fh, fh
                    Role += "Disc Raid";
                    sr.Add(pws);        // 1.5  1.5  -2.5  -??   -??
                    sr.Add(penance_bt); // 1.5  3.0  -1.0  -8.0  -??
                    sr.Add(prom_max);   // 1.5  4.5  -??   -6.5  -8.5
                    sr.Add(pws);        // 1.5  6.0  -2.5  -5.0  -7.0
                    sr.Add(fh_bt);      // 1.5  7.5  -1.0  -3.5  -5.5
                    sr.Add(fh);         // 1.5  9.0  -??   -2.0  -4.0
                    // repeat
                    break;
                default:
                    break;
            }

            foreach (Spell s in sr)
                ActionList += "\r\n- " + s.Name;

            float manacost = 0, cyclelen = 0, healamount = 0, solctr = 0, castctr = 0, crittable = 0, rapturetot = 0, serendipitytot = 0, metareductiontot = 0;
            for (int x = 0; x < sr.Count; x++)
            {
                float mcost = 0, absorb = 0, heal = 0, rheal = 0, clen = 0;
                if (sr[x] == gh || sr[x] == gh_bt)
                {   // Greater Heal (A Borrowed Time GHeal cannot also be improved Holy conc hasted, so this works)
                    clen = sr[x].CastTime * (1f - ihcastshasted) + gh_hc.CastTime * ihcastshasted;
                    rheal = sr[x].AvgTotHeal * healmultiplier;
                    absorb = sr[x].AvgCrit * healmultiplier * sr[x].CritChance * character.PriestTalents.DivineAegis * 0.1f;
                    solctr = 1f - (1f - solctr) * (1f - solchance * (1f - hcchance));
                    mcost = sr[x].ManaCost;
                    mcost -= mcost * hcchance;
                    serendipitytot += mcost * serendipityconst;
                    mcost -= simstats.ManaGainOnGreaterHealOverheal * calculationOptions.Serendipity / 100f;
                    castctr++;
                    crittable += sr[x].CritChance;
                }
                else if (sr[x] == fh || sr[x] == fh_bt)
                {   // Flash Heal (Same applies to FH as GHeal with regards to borrowed time)
                    clen = sr[x].CastTime * (1f - hcchance) + fh_hc.CastTime * hcchance;
                    rheal = sr[x].AvgTotHeal * healmultiplier;
                    absorb = sr[x].AvgCrit * healmultiplier * sr[x].CritChance * character.PriestTalents.DivineAegis * 0.1f;
                    solctr = 1f - (1f - solctr) * (1f - solchance * (1f - hcchance));
                    mcost = sr[x].ManaCost;
                    mcost -= mcost * hcchance;
                    mcost -= mcost * solctr;
                    solctr = 0;
                    serendipitytot += mcost * serendipityconst;
                    castctr++;
                    crittable += sr[x].CritChance;
                }
                else if (sr[x] == penance || sr[x] == penance_bt)
                {
                    clen = sr[x].CastTime;
                    rheal = sr[x].AvgTotHeal * healmultiplier;
                    absorb = sr[x].AvgCrit * healmultiplier * sr[x].CritChance * character.PriestTalents.DivineAegis * 0.1f;
                    mcost = sr[x].ManaCost;
                    castctr += 3f; // Penance counts as 3 casts for some purposes.
                    crittable += 1f - (float)Math.Pow(1f - sr[x].CritChance, 3f);
                }
                else if (sr[x] == coh)
                {   // Circle of Healing
                    clen = coh.GlobalCooldown;
                    heal = coh.AvgTotHeal * healmultiplier;
                    solctr = 1f - (1f - solctr) * (1f - sol5chance);
                    mcost = coh.ManaCost;
                    castctr += sr[x].Targets;
                    crittable += 1f - (float)Math.Pow(1f - sr[x].CritChance, sr[x].Targets);
                }
                else if (sr[x] == renew)
                {   // Renew
                    clen = renew.GlobalCooldown;
                    heal = renew.AvgTotHeal * healmultiplier;
                    mcost = renew.ManaCost;
                    castctr++;
                }
                else if (sr[x] == pws)
                {
                    clen = pws.GlobalCooldown;
                    absorb = pws.AvgTotHeal;
                    mcost = pws.ManaCost;
                    castctr++;
                }
                else if (sr[x] == prom_1 || sr[x] == prom_max)
                {
                    clen = sr[x].GlobalCooldown;
                    heal = sr[x].AvgTotHeal * healmultiplier;
                    absorb = sr[x].AvgCrit * healmultiplier * sr[x].CritChance * character.PriestTalents.DivineAegis * 0.1f;
                    mcost = sr[x].ManaCost;
                    castctr += sr[x].Targets;
                    crittable += 1f - (float)Math.Pow(1f - sr[x].CritChance, sr[x].Targets);
                }
                cyclelen += clen;
                healamount += heal + rheal + absorb;
                manacost += mcost;
                rapturetot += (rheal + absorb) * raptureconst;
                metareductiontot += metaSpellCostReduction;
            }

            float avgcastlen = cyclelen / castctr;
            float avgcritcast = crittable / sr.Count;

            float periodicRegenOutFSR = StatConversion.GetSpiritRegenSec(simstats.Spirit, simstats.Intellect);

            // Add up all mana gains.
            float regen = 0, tmpregen = 0;

            // Spirit/Intellect based Regeneration and MP5
            tmpregen = periodicRegenOutFSR * (1f - calculationOptions.FSRRatio / 100f);
            if (tmpregen > 0f)
            {
                ManaSources.Add(new ManaSource("OutFSR", tmpregen));
                regen += tmpregen;
            }
            tmpregen = periodicRegenOutFSR * simstats.SpellCombatManaRegeneration * calculationOptions.FSRRatio / 100f;
            if (tmpregen > 0f)
            {
                ManaSources.Add(new ManaSource("Meditation", tmpregen));
                regen += tmpregen;
            }
            tmpregen = simstats.Mp5 / 5;
            ManaSources.Add(new ManaSource("MP5", tmpregen));
            regen += tmpregen;
            tmpregen = simstats.Mana / (calculationOptions.FightLengthSeconds);
            ManaSources.Add(new ManaSource("Intellect", tmpregen));
            regen += tmpregen;
            if (calculationOptions.ModelProcs)
            {
                if (simstats.BangleProc > 0)
                {
                    float BangleLevelMod = 0.15f - (character.Level - 70f) / 200f;
                    tmpregen = periodicRegenOutFSR * BangleLevelMod * 15f / 60f * (1f - (float)Math.Pow(1f - BangleLevelMod, 15f / avgcastlen));
                    if (BangleLevelMod > 0f && tmpregen > 0f)
                    {
                        ManaSources.Add(new ManaSource("Bangle of Endless Blessings", tmpregen));
                        regen += tmpregen;
                    }
                }
                if (simstats.FullManaRegenFor15SecOnSpellcast > 0)
                {
                    // Blue Dragon. 2% chance to proc on cast, no known internal cooldown. calculate as the chance to have procced during its duration. 2% proc/cast.
                    tmpregen = periodicRegenOutFSR * (1f - simstats.SpellCombatManaRegeneration) * (1f - (float)Math.Pow(1f - 0.02f, 15f / avgcastlen));
                    if (tmpregen > 0f)
                    {
                        ManaSources.Add(new ManaSource("Darkmoon Card: Blue Dragon", tmpregen));
                        regen += tmpregen;
                    }
                }
                if (simstats.ManaRestoreOnCrit_25_45 > 0)
                {   // X mana back every 25%*critchance spell every 45seconds.
                    tmpregen = simstats.ManaRestoreOnCrit_25_45 / ProcInterval(0.25f * avgcritcast, avgcritcast, 45f);
                    if (tmpregen > 0f)
                    {
                        ManaSources.Add(new ManaSource("Soul of the Dead" , tmpregen));
                        regen += tmpregen;
                    }
                }
                if (simstats.ManaRestoreOnCast_10_45 > 0)
                {
                    tmpregen = simstats.ManaRestoreOnCast_10_45 / ProcInterval(0.1f, avgcastlen, 45f);
                    if (tmpregen > 0f)
                    {
                        ManaSources.Add(new ManaSource("Spark of Life", tmpregen));
                        regen += tmpregen;
                    }
                }

                float trinketmp5 = 0;
                if (simstats.Mp5OnCastFor20SecOnUse2Min > 0)
                    trinketmp5 += (20f / avgcastlen) * 21f / 2f * 20f / 120f;
                /* Memento wrapped into ManaRestoreOnCast_10_45, Trolando
                if (simstats.MementoProc > 0)
                    trinketmp5 += simstats.MementoProc * 3f * 5f / (45f + 15f * (1f - (float)Math.Pow(1f - 0.1f, 15f / avgcastlen)));
                 */
                if (simstats.ManacostReduceWithin15OnHealingCast > 0)
                    trinketmp5 += simstats.ManacostReduceWithin15OnHealingCast * (1f - (float)Math.Pow(1f - 0.02f, castctr)) * 5f / cyclelen;
                if (simstats.ManaregenFor8SecOnUse5Min > 0)
                    trinketmp5 += simstats.ManaregenFor8SecOnUse5Min * 8f / 300f * 5f;
                if (trinketmp5 > 0f)
                {
                    tmpregen = trinketmp5 / 5f;
                    ManaSources.Add(new ManaSource("Trinkets", tmpregen));
                    regen += tmpregen;
                }
                if (simstats.ManacostReduceWithin15OnUse1Min > 0)
                    manacost -= simstats.ManacostReduceWithin15OnUse1Min * (float)Math.Floor(15f / cyclelen * sr.Count) / 60f;
            }

            // External and Other mana sources.
            tmpregen = simstats.Mana * 0.0025f * calculationOptions.Replenishment / 100f;
            if (tmpregen > 0f)
            {
                ManaSources.Add(new ManaSource("Replenishment", tmpregen));
                regen += tmpregen;
            }

            if (rapturetot > 0f)
            {
                tmpregen = rapturetot / cyclelen;
                ManaSources.Add(new ManaSource("Rapture", tmpregen));
                regen += tmpregen;

            }
            if (serendipitytot > 0f)
            {
                tmpregen = serendipitytot / cyclelen;
                ManaSources.Add(new ManaSource("Serendipity", tmpregen));
                regen += tmpregen;
            }
            if (metareductiontot > 0f)
            {
                tmpregen = metareductiontot / cyclelen;
                ManaSources.Add(new ManaSource("Metagem", tmpregen));
                regen += tmpregen;
            }

            ActionList += "\r\n\r\nMana Options:";

            // Real Cyclelen also has time for FSR. To get 80% FSR, a cycle of 20 seconds needs to include:
            // (20 + 5) / 0.8 = 31.25 seconds. (31.25 - 5 - 20 = 6.25 / 31.25 = 0.2 seconds of FSR regen).
            //float realcyclelen = (cyclelen + 5f) / (calculationOptions.FSRRatio / 100f);
            // Extra fudge model: (As you approach 100% FSR, realcyclelen approaches cyclelen)
            //float realcyclelen = (cyclelen + 5f * (1f - (float)Math.Pow(calculationOptions.FSRRatio / 100f, 2f))) / (calculationOptions.FSRRatio / 100f);
            // Xtreme fudge model: Cast 25 seconds, 5 seconds no casting, then slap on FSR.
            // ((25 + 5) / FSR) / 25 * cyclelen = realcyclelen.
            float realcyclelen = cyclelen * ((25f + 5f) / (calculationOptions.FSRRatio / 100f)) / 25f;
            float mp1use = manacost / realcyclelen;

            if (mp1use > regen && character.Race == Character.CharacterRace.BloodElf)
            {   // Arcane Torrent is 6% max mana every 2 minutes.
                tmpregen = simstats.Mana * 0.06f / 120f;
                ManaSources.Add(new ManaSource("Arcane Torrent", tmpregen));
                regen += tmpregen;
                ActionList += string.Format("\r\n- Used Arcane Torrent");
            }

            if (mp1use > regen && calculationOptions.ManaAmt > 0f)
            {
                float ManaPot = calculationOptions.ManaAmt * (1f + simstats.BonusManaPotion);
                tmpregen = ManaPot / (calculationOptions.FightLengthSeconds);
                ManaSources.Add(new ManaSource("Mana Potion", tmpregen));
                ActionList += string.Format("\r\n- Used Mana Potion ({0})", ManaPot.ToString("0"));
                regen += tmpregen;
            }
            if (mp1use > regen)
            {
                tmpregen = (simstats.Mana * 0.4f * calculationOptions.Shadowfiend / 100f)
                    / ((5f - character.PriestTalents.VeiledShadows * 1f) * 60f);
                ManaSources.Add(new ManaSource("Shadowfiend", tmpregen));
                ActionList += string.Format("\r\n- Used Shadowfiend");
                regen += tmpregen;
            }
            if (mp1use > regen)
            {
                tmpregen = (simstats.Mana * 0.08f)
                    / (5f * 60f);
                ManaSources.Add(new ManaSource("Hymn of Hope", tmpregen));
                ActionList += string.Format("\r\n- Used Hymn of Hope");
                regen += tmpregen;
            }
                         
            calculatedStats.HPSBurstPoints = healamount / cyclelen;
            // Sustained is limited by how much mana you regenerate over the time it would take to cast the spells, divided by the cost.
            if (regen > mp1use) // Regenerating more mana than we can use. Dont make user believe this is an upgrade.
                calculatedStats.HPSSustainPoints = calculatedStats.HPSBurstPoints;
            else
                calculatedStats.HPSSustainPoints = calculatedStats.HPSBurstPoints * regen / mp1use;

            // Lets just say that 15% of resilience scales all health by 150%.
            float Resilience = (float)Math.Min(15f, StatConversion.GetResilienceFromRating(simstats.Resilience) * 100f) / 15f;
            calculatedStats.SurvivabilityPoints = calculatedStats.BasicStats.Health * (Resilience * 1.5f + 1f) * calculationOptions.Survivability / 100f;
        }   

    }

    public class AdvancedSolver : BaseSolver
    {
        public AdvancedSolver(Stats _stats, Character _char, float _baseMana)
            : base(_stats, _char, _baseMana)
        {
            Role = "Custom";
        }

        public override void Calculate(CharacterCalculationsHolyPriest calculatedStats)
        {
            Stats simstats = calculatedStats.BasicStats.Clone();

            Stats UseProcs = new Stats();

            // Pre calc Procs (Power boosting Procs)
            if (calculationOptions.ModelProcs)
            {
                if (simstats.SpiritFor20SecOnUse2Min > 0)
                    // Trinkets with Use: Increases Spirit with. (Like Earring of Soulful Meditation / Bangle of Endless blessings)
                    UseProcs.Spirit += simstats.SpiritFor20SecOnUse2Min * 20f / 120f;
                //                if (simstats.BangleProc > 0)
                // Bangle of Endless Blessings. Use: 130 spirit over 20 seconds. 120 sec cd.
                //UseProcs.Spirit += 130f * 20f / 120f;              
                if (simstats.SpellPowerFor15SecOnUse2Min > 0)
                    UseProcs.SpellPower += simstats.SpellPowerFor15SecOnUse2Min * 15f / 120f;
                if (simstats.SpellPowerFor15SecOnUse90Sec > 0)
                    UseProcs.SpellPower += simstats.SpellPowerFor15SecOnUse90Sec * 15f / 90f;
                if (simstats.SpellPowerFor20SecOnUse2Min > 0)
                    UseProcs.SpellPower += simstats.SpellPowerFor20SecOnUse2Min * 20f / 120f;
                if (simstats.HasteRatingFor20SecOnUse5Min > 0)
                    UseProcs.SpellHaste += StatConversion.GetSpellHasteFromRating(simstats.HasteRatingFor20SecOnUse5Min) * 20f / 300f;
                if (simstats.HasteRatingFor20SecOnUse2Min > 0)
                    UseProcs.SpellHaste += StatConversion.GetSpellHasteFromRating(simstats.HasteRatingFor20SecOnUse2Min) * 20f / 120f;
            }

            UseProcs.Spirit = (float)Math.Round(UseProcs.Spirit * (1 + simstats.BonusSpiritMultiplier));
            UseProcs.SpellPower += (float)Math.Round(UseProcs.Spirit * simstats.SpellDamageFromSpiritPercentage);

            simstats += UseProcs;

            // Insightful Earthstorm Diamond.
            float metaSpellCostReduction = simstats.ManaRestoreOnCast_5_15 * 0.05f;
//            float hcchance = (character.PriestTalents.HolyConcentration * 0.1f + character.PriestTalents.ImprovedHolyConcentration * .05f)
//                * (simstats.SpellCrit + character.PriestTalents.HolySpecialization * 0.01f);
            // improved holy concentration removed in patch 3.1
            float hcchance = (character.PriestTalents.HolyConcentration * 0.1f)
                * (simstats.SpellCrit + character.PriestTalents.HolySpecialization * 0.01f);
            float ihcastshasted = 2f * hcchance - (float)Math.Pow(hcchance, 2f);
//            float ihchaste = character.PriestTalents.ImprovedHolyConcentration * 0.1f; // improved holy concentration removed in patch 3.1
            float serendipityconst = calculationOptions.Serendipity / 100f * character.PriestTalents.Serendipity * 0.25f / 3f;
            float raptureconst = CalculationsHolyPriest.GetRaptureConst(character) * simstats.Mana / baseMana * character.PriestTalents.Rapture / 5f * calculationOptions.Rapture / 100f;
            float healmultiplier = (1 + character.PriestTalents.TestOfFaith * 0.02f * calculationOptions.TestOfFaith / 100f) * (1 + character.PriestTalents.Grace * 0.03f) * (1 + simstats.HealingReceivedMultiplier);
            float divineaegis = character.PriestTalents.DivineAegis * 0.1f;

            // Test of Faith gives 2-6% extra crit on targets below 50%.
            // no longer true in patch 3.1
            //simstats.SpellCrit += character.PriestTalents.TestOfFaith * 0.02f * calculationOptions.TestOfFaith / 100f;

            float solchance = (character.PriestTalents.HolySpecialization * 0.01f + simstats.SpellCrit) * character.PriestTalents.SurgeOfLight * 0.25f;
            float solbhchance = 1f - (float)Math.Pow(1f - solchance, 2);
            float solcohchance = 1f - (float)Math.Pow(1f - solchance, character.PriestTalents.GlyphofCircleOfHealing ? 6 : 5);
            float solpohhnchance = 1f - (float)Math.Pow(1f - solchance, 5);
            float solpromchance = 1f - (float)Math.Pow(1f - solchance, (calculationOptions.ProMCast == 0)?0:(calculationOptions.ProMTicks / calculationOptions.ProMCast));

            // Add on Renewed Hope crit for Disc Maintank Rotation, adjusted by uptime of Weakened Soul
            float WeakenedSoulUptime = (float)Math.Min(1f, calculationOptions.PWSCast * 15f / calculationOptions.FightLengthSeconds);
            simstats.SpellCrit += character.PriestTalents.RenewedHope * 0.02f * WeakenedSoulUptime;

            int TotalCasts = 0;
            float ManaUsed = 0f;
            float TimeUsed = 0f;
            float BaseTimeUsed = 0f;
            float DirectHeal = 0f;
            float OtherHeal = 0f;
            float AbsorbHeal = 0f;
            float SerendipityBase = 0f;
            float CritCounter = 0f;

            FlashHeal fh = new FlashHeal(simstats, character);
            BindingHeal bh = new BindingHeal(simstats, character);
            Heal gh = new Heal(simstats, character);
            Penance pen = new Penance(simstats, character);
            Renew renew = new Renew(simstats, character);
            PrayerOfMending prom = new PrayerOfMending(simstats, character, 1);
            PrayerOfMending prom_max = new PrayerOfMending(simstats, character);
            PrayerOfHealing proh = new PrayerOfHealing(simstats, character);
            PowerWordShield pws = new PowerWordShield(simstats, character);
            CircleOfHealing coh = new CircleOfHealing(simstats, character, 1);
            CircleOfHealing coh_max = new CircleOfHealing(simstats, character);
            HolyNova hn = new HolyNova(simstats, character, 1);
            Dispel dispel = new Dispel(simstats, character);
            MassDispel md = new MassDispel(simstats, character);

            // Calculate how many of the flash heals are actually paid for.
            float FreeFlashes = calculationOptions.FlashHealCast * solchance
                + calculationOptions.BindingHealCast * solbhchance
                + calculationOptions.GreaterHealCast * solchance
                + calculationOptions.CoHCast * solcohchance
                + calculationOptions.ProMCast * solpromchance
                + calculationOptions.PoHCast * solpohhnchance
                + calculationOptions.HolyNovaCast * solpohhnchance;
            FreeFlashes = (float)Math.Min(calculationOptions.FlashHealCast, FreeFlashes);

            // Flash Heal
            if (calculationOptions.FlashHealCast > 0)
            {
                TotalCasts += calculationOptions.FlashHealCast;
                ActionList += String.Format("\r\n- {0} Flash Heal{1}",
                    calculationOptions.FlashHealCast,
                    (FreeFlashes > 0f) ? String.Format(", {0} Surge of Lights", FreeFlashes.ToString("0")) : String.Empty);
                float Cost = fh.ManaCost * (calculationOptions.FlashHealCast - FreeFlashes);
                SerendipityBase += Cost;
                ManaUsed += Cost;
                TimeUsed += fh.CastTime * calculationOptions.FlashHealCast;
                BaseTimeUsed += fh.BaseCastTime * calculationOptions.FlashHealCast;
                DirectHeal += fh.AvgTotHeal * healmultiplier * calculationOptions.FlashHealCast;
                AbsorbHeal += fh.AvgCrit * fh.CritChance * healmultiplier * calculationOptions.FlashHealCast * divineaegis;
                CritCounter += fh.CritChance * calculationOptions.FlashHealCast;
            }

            // Binding Heal
            if (calculationOptions.BindingHealCast > 0)
            {
                TotalCasts += calculationOptions.BindingHealCast;
                ActionList += String.Format("\r\n- {0} Binding Heal", calculationOptions.BindingHealCast);
                ManaUsed += bh.ManaCost * calculationOptions.BindingHealCast;
                TimeUsed += bh.CastTime * calculationOptions.BindingHealCast;
                BaseTimeUsed += bh.BaseCastTime * calculationOptions.BindingHealCast;
                OtherHeal += bh.AvgTotHeal * healmultiplier * calculationOptions.BindingHealCast;
                AbsorbHeal += bh.AvgCrit * 2 * bh.CritChance * healmultiplier * calculationOptions.BindingHealCast * divineaegis;
                CritCounter += bh.CritChance * 2 * calculationOptions.BindingHealCast;
            }

            // Greater Heal
            if (calculationOptions.GreaterHealCast > 0)
            {
                TotalCasts += calculationOptions.GreaterHealCast;
                ActionList += String.Format("\r\n- {0} Greater Heal", calculationOptions.GreaterHealCast);
                float Cost = gh.ManaCost * calculationOptions.GreaterHealCast;
                SerendipityBase += Cost;
                ManaUsed += Cost;
                TimeUsed += gh.CastTime * calculationOptions.GreaterHealCast;
                BaseTimeUsed += gh.BaseCastTime * calculationOptions.GreaterHealCast;
                DirectHeal += gh.AvgTotHeal * healmultiplier * calculationOptions.GreaterHealCast;
                AbsorbHeal += gh.AvgCrit * gh.CritChance * healmultiplier * calculationOptions.GreaterHealCast * divineaegis;
                CritCounter += gh.CritChance * calculationOptions.GreaterHealCast;
            }

            // Penance
            if (calculationOptions.PenanceCast > 0 && character.PriestTalents.Penance > 0)
            {
                TotalCasts += calculationOptions.PenanceCast;
                ActionList += String.Format("\r\n- {0} Penance", calculationOptions.PenanceCast);
                ManaUsed += pen.ManaCost * calculationOptions.PenanceCast;
                TimeUsed += pen.CastTime * calculationOptions.PenanceCast;
                BaseTimeUsed += pen.BaseCastTime * calculationOptions.PenanceCast;
                DirectHeal += pen.AvgTotHeal * healmultiplier * calculationOptions.PenanceCast;
                AbsorbHeal += pen.AvgCrit * pen.CritChance * healmultiplier * calculationOptions.PenanceCast * divineaegis;
                CritCounter += pen.CritChance * 3f * calculationOptions.PenanceCast;
            }

            // Renew
            if (calculationOptions.RenewCast > 0)
            {
                TotalCasts += calculationOptions.RenewCast;
                ActionList += String.Format("\r\n- {0} Renew, {1} Ticks", calculationOptions.RenewCast, calculationOptions.RenewTicks);
                ManaUsed += renew.ManaCost * calculationOptions.RenewCast;
                TimeUsed += renew.GlobalCooldown * calculationOptions.RenewCast;
                BaseTimeUsed += 1.5f * calculationOptions.RenewCast;
                OtherHeal += renew.AvgHeal / (renew.HotDuration * 3) * healmultiplier * calculationOptions.RenewTicks;
            }

            // Prayer of Mending
            if (calculationOptions.ProMCast > 0)
            {
                TotalCasts += calculationOptions.ProMCast;
                ActionList += String.Format("\r\n- {0} Prayer of Mending, {1} Procs", calculationOptions.ProMCast, calculationOptions.ProMTicks);
                ManaUsed += prom.ManaCost * calculationOptions.ProMCast;
                TimeUsed += prom.GlobalCooldown * calculationOptions.ProMCast;
                BaseTimeUsed += 1.5f * calculationOptions.ProMCast;
                DirectHeal += prom.AvgTotHeal * healmultiplier * calculationOptions.ProMTicks;
                AbsorbHeal += prom.AvgCrit * prom.CritChance * healmultiplier * calculationOptions.ProMTicks * divineaegis;
                CritCounter += prom.CritChance * calculationOptions.ProMTicks;
            }

            // Prayer of Healing
            if (calculationOptions.PoHCast > 0)
            {
                TotalCasts += calculationOptions.PoHCast;
                ActionList += String.Format("\r\n- {0} Prayer of Healing", calculationOptions.PoHCast);
                ManaUsed += proh.ManaCost * calculationOptions.PoHCast;
                TimeUsed += proh.CastTime * calculationOptions.PoHCast;
                BaseTimeUsed += proh.BaseCastTime * calculationOptions.PoHCast;
                DirectHeal += proh.AvgTotHeal * healmultiplier * calculationOptions.PoHCast;
                AbsorbHeal += proh.AvgCrit * proh.CritChance * healmultiplier * calculationOptions.PoHCast * divineaegis;
                CritCounter += proh.CritChance * 5 * calculationOptions.PoHCast;
            }

            // PW:Shield
            if (calculationOptions.PWSCast > 0)
            {
                TotalCasts += calculationOptions.PWSCast;
                ActionList += String.Format("\r\n- {0} Power Word: Shield", calculationOptions.PWSCast);
                ManaUsed += pws.ManaCost * calculationOptions.PWSCast;
                TimeUsed += pws.GlobalCooldown * calculationOptions.PWSCast;
                BaseTimeUsed += 1.5f * calculationOptions.PWSCast;
                AbsorbHeal += pws.AvgTotHeal * calculationOptions.PWSCast;
                float pwsglyphheal = pws.AvgTotHeal * 0.2f * healmultiplier * ((character.PriestTalents.GlyphofPowerWordShield) ? 0.2f : 0.0f);
                OtherHeal += pwsglyphheal * (1f - simstats.SpellCrit) + pwsglyphheal * 1.5f * simstats.SpellCrit;
            }

            // Circle of Healing
            if (calculationOptions.CoHCast > 0)
            {
                TotalCasts += calculationOptions.CoHCast;
                ActionList += String.Format("\r\n- {0} Circle of Healing", calculationOptions.CoHCast);
                ManaUsed += coh_max.ManaCost * calculationOptions.CoHCast;
                TimeUsed += coh_max.GlobalCooldown * calculationOptions.CoHCast;
                BaseTimeUsed += 1.5f * calculationOptions.CoHCast;
                OtherHeal += coh_max.AvgTotHeal * calculationOptions.CoHCast;
                CritCounter += coh_max.CritChance * coh_max.Targets * calculationOptions.CoHCast;
            }

            // Holy Nova
            if (calculationOptions.HolyNovaCast > 0)
            {
                TotalCasts += calculationOptions.HolyNovaCast;
                ActionList += String.Format("\r\n- {0} Holy Nova", calculationOptions.HolyNovaCast);
                ManaUsed += hn.ManaCost * calculationOptions.HolyNovaCast;
                TimeUsed += hn.GlobalCooldown * calculationOptions.HolyNovaCast;
                BaseTimeUsed += 1.5f * calculationOptions.HolyNovaCast;
                DirectHeal += hn.AvgTotHeal * healmultiplier * calculationOptions.HolyNovaCast;
                AbsorbHeal += hn.AvgCrit * hn.CritChance * healmultiplier * calculationOptions.HolyNovaCast * divineaegis;
                CritCounter += hn.CritChance * 5 * calculationOptions.HolyNovaCast;
            }

            // Dispel
            if (calculationOptions.DispelCast > 0)
            {
                TotalCasts += calculationOptions.DispelCast;
                ActionList += String.Format("\r\n- {0} Dispel", calculationOptions.DispelCast);
                ManaUsed += dispel.ManaCost * calculationOptions.DispelCast;
                TimeUsed += dispel.GlobalCooldown * calculationOptions.DispelCast;
                BaseTimeUsed += 1.5f * calculationOptions.DispelCast;
            }

            // Mass Dispel
            if (calculationOptions.MDCast > 0)
            {
                TotalCasts += calculationOptions.MDCast;
                ActionList += String.Format("\r\n- {0} Mass Dispel", calculationOptions.MDCast);
                ManaUsed += md.ManaCost * calculationOptions.MDCast;
                TimeUsed += md.CastTime * calculationOptions.MDCast;
                BaseTimeUsed += md.BaseCastTime * calculationOptions.MDCast;
            }

            if (TimeUsed > calculationOptions.FightLengthSeconds)
            {
                ActionList += "\r\n\r\nWARNING:\r\nFight Length is less than time needed to perform the actions listed!";
                calculatedStats.HPSBurstPoints = -1;
                calculatedStats.HPSSustainPoints = -1;
            }
            else
            {
                float mp1use = ManaUsed / calculationOptions.FightLengthSeconds;

                float periodicRegenOutFSR = StatConversion.GetSpiritRegenSec(simstats.Spirit, simstats.Intellect);

                // Add up all mana gains.
                float regen = 0, tmpregen = 0;

                // Spirit/Intellect based Regeneration and MP5
                tmpregen = periodicRegenOutFSR * (1f - calculationOptions.FSRRatio / 100f);
                if (tmpregen > 0f)
                {
                    ManaSources.Add(new ManaSource("OutFSR", tmpregen));
                    regen += tmpregen;
                }
                tmpregen = periodicRegenOutFSR * simstats.SpellCombatManaRegeneration * calculationOptions.FSRRatio / 100f;
                if (tmpregen > 0f)
                {
                    ManaSources.Add(new ManaSource("Meditation", tmpregen));
                    regen += tmpregen;
                }
                tmpregen = simstats.Mp5 / 5;
                ManaSources.Add(new ManaSource("MP5", tmpregen));
                regen += tmpregen;
                tmpregen = simstats.Mana / (calculationOptions.FightLengthSeconds);
                ManaSources.Add(new ManaSource("Intellect", tmpregen));
                regen += tmpregen;

                // TODO: Trinkets here.
                if (simstats.ManaRestoreOnCrit_25_45 > 0)
                {   // X mana back every 25%*critchance spell every 45seconds.
                    float avgcritcast = CritCounter / TotalCasts;
                    tmpregen = simstats.ManaRestoreOnCrit_25_45 / ProcInterval(0.25f * avgcritcast, avgcritcast, 45f);
                    if (tmpregen > 0f)
                    {
                        ManaSources.Add(new ManaSource("Soul of the Dead", tmpregen));
                        regen += tmpregen;
                    }
                }
                if (simstats.ManaRestoreOnCast_10_45 > 0)
                {
                    tmpregen = simstats.ManaRestoreOnCast_10_45 / ProcInterval(0.1f, TimeUsed / TotalCasts, 45f);
                    if (tmpregen > 0f)
                    {
                        if (simstats.ManaRestoreOnCast_10_45 == 300)
                            ManaSources.Add(new ManaSource("Je'Tze's Bell", tmpregen));
                        else if (simstats.ManaRestoreOnCast_10_45 == 528)
                            ManaSources.Add(new ManaSource("Spark of Life", tmpregen));
                        else
                            ManaSources.Add(new ManaSource("MP5 Proc Trinket", tmpregen));
                        regen += tmpregen;
                    }
                }

                // External and Other mana sources.
                tmpregen = simstats.Mana * 0.0025f * calculationOptions.Replenishment / 100f;
                if (tmpregen > 0f)
                {
                    ManaSources.Add(new ManaSource("Replenishment", tmpregen));
                    regen += tmpregen;
                }

                if (raptureconst > 0f)
                {
                    tmpregen = (AbsorbHeal + DirectHeal) * raptureconst / calculationOptions.FightLengthSeconds;
                    ManaSources.Add(new ManaSource("Rapture", tmpregen));
                    regen += tmpregen;
                }
                if (serendipityconst > 0f)
                {
                    tmpregen = SerendipityBase * serendipityconst / calculationOptions.FightLengthSeconds;
                    ManaSources.Add(new ManaSource("Serendipity", tmpregen));
                    regen += tmpregen;
                }
                if (metaSpellCostReduction > 0f)
                {
                    tmpregen = (metaSpellCostReduction * TotalCasts) / calculationOptions.FightLengthSeconds;
                    ManaSources.Add(new ManaSource("Metagem", tmpregen));
                    regen += tmpregen;
                }

                ActionList += "\r\n\r\nMana Options:";

                if (mp1use > regen && character.Race == Character.CharacterRace.BloodElf)
                {   // Arcane Torrent is 6% max mana every 2 minutes.
                    tmpregen = simstats.Mana * 0.06f / 120f;
                    ManaSources.Add(new ManaSource("Arcane Torrent", tmpregen));
                    regen += tmpregen;
                    ActionList += string.Format("\r\n- Used Arcane Torrent");
                }

                if (mp1use > regen && calculationOptions.ManaAmt > 0f)
                {
                    float ManaPot = calculationOptions.ManaAmt * (1f + simstats.BonusManaPotion);
                    tmpregen = ManaPot / (calculationOptions.FightLengthSeconds);
                    ManaSources.Add(new ManaSource("Mana Potion", tmpregen));
                    ActionList += string.Format("\r\n- Used Mana Potion ({0})", ManaPot.ToString("0"));
                    regen += tmpregen;
                }
                if (mp1use > regen)
                {
                    tmpregen = (simstats.Mana * 0.4f * calculationOptions.Shadowfiend / 100f)
                        / ((5f - character.PriestTalents.VeiledShadows * 1f) * 60f);
                    ManaSources.Add(new ManaSource("Shadowfiend", tmpregen));
                    ActionList += string.Format("\r\n- Used Shadowfiend");
                    regen += tmpregen;
                }
                if (mp1use > regen)
                {
                    tmpregen = (simstats.Mana * 0.08f)
                        / (5f * 60f);
                    ManaSources.Add(new ManaSource("Hymn of Hope", tmpregen));
                    ActionList += string.Format("\r\n- Used Hymn of Hope");
                    regen += tmpregen;
                }

                calculatedStats.HPSBurstPoints = (DirectHeal + AbsorbHeal + OtherHeal) / TimeUsed;
                calculatedStats.HPSSustainPoints = (DirectHeal + AbsorbHeal + OtherHeal) / calculationOptions.FightLengthSeconds;
                if (regen < mp1use)
                    calculatedStats.HPSSustainPoints *= regen / mp1use;
                /*if (regen > mp1use)
                    calculatedStats.HPSSustainPoints = calculatedStats.HPSBurstPoints;
                else
                    calculatedStats.HPSSustainPoints = calculatedStats.HPSBurstPoints * regen / mp1use;*/
            }

            // Lets just say that 15% of resilience scales all health by 150%.
            float Resilience = (float)Math.Min(15f, StatConversion.GetResilienceFromRating(simstats.Resilience) * 100f) / 15f;
            calculatedStats.SurvivabilityPoints = calculatedStats.BasicStats.Health * (Resilience * 1.5f + 1f) * calculationOptions.Survivability / 100f;
        }
    }
}