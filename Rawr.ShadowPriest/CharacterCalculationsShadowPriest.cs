using System;
using System.Collections.Generic;

namespace Rawr.ShadowPriest
{

    public class CharacterCalculationsShadowPriest : CharacterCalculationsBase
    {
        private Stats basicStats;
        private Character character;

        public float SpiritRegen { get; set; }
        public float RegenInFSR { get; set; }
        public float RegenOutFSR { get; set; }
        public Character.CharacterRace Race { get; set; }

        public bool bHoly { get; set; }

        public Character Character
        {
            get { return character; }
            set { character = value; }
        }
        
        public Stats BasicStats
        {
            get { return basicStats; }
            set { basicStats = value; }
        }

        public override float OverallPoints
        {
            get
            {
                float f = 0f;
                foreach (float f2 in _subPoints)
                    f += f2;
                return f;
            }
            set { }
        }

        private float[] _subPoints = new float[] { 0f, 0f, 0f };
        public override float[] SubPoints
        {
            get { return _subPoints; }
            set { _subPoints = value; }
        }

        public float DpsPoints
        {
            get { return _subPoints[0]; }
            set { _subPoints[0] = value; }
        }

        public float SustainPoints
        {
            get { return _subPoints[1]; }
            set { _subPoints[1] = value; }
        }

        public float SurvivalPoints
        {
            get { return _subPoints[2]; }
            set { _subPoints[2] = value; }
        }

        public SolverBase GetSolver(Character character, Stats stats)
        {
            if (character.PriestTalents.MindFlay > 0)
                return new SolverShadow(stats, character);
            else
                return new SolverHoly(stats, character);
        }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();
            CalculationOptionsShadowPriest calcOptions = character.CalculationOptions as CalculationOptionsShadowPriest;

            dictValues.Add("Health", BasicStats.Health.ToString());
            float ResilienceCap = 15f, ResilienceFromRating = character.StatConversion.GetResilienceFromRating(1);
            float Resilience = character.StatConversion.GetResilienceFromRating(BasicStats.Resilience);
            dictValues.Add("Resilience", string.Format("{0}*-{1}% Damage from DoT and Mana Drains\n\r-{1}% Chance to be crit\r\n-{2}% Damage from Crits.\r\n{3}",
                BasicStats.Resilience.ToString(),
                Resilience.ToString("0.00"),
                (Resilience * 2.2f).ToString("0.00"),
                (Resilience > ResilienceCap) ? (string.Format("{0} rating above cap", ((float)Math.Floor((Resilience - ResilienceCap) / ResilienceFromRating)).ToString("0"))) : (string.Format("{0} rating below cap", ((float)Math.Ceiling((ResilienceCap - Resilience) / ResilienceFromRating)).ToString("0")))));
            dictValues.Add("Stamina", BasicStats.Stamina.ToString());
            dictValues.Add("Mana", BasicStats.Mana.ToString());
            dictValues.Add("Intellect", BasicStats.Intellect.ToString());
            dictValues.Add("Spirit", Math.Floor(BasicStats.Spirit).ToString("0"));
            dictValues.Add("Spell Power", String.Format("{0}*{1} Bonus Shadow\r\n{2} Bonus Holy\r\n{3} from Inner Fire", 
                Math.Floor(BasicStats.SpellPower), 
                Math.Floor(BasicStats.SpellPower + BasicStats.SpellShadowDamageRating), 
                Math.Floor(BasicStats.SpellPower /*+ BasicStats.SpellHolyDamageRating*/),
                CalculationsShadowPriest.GetInnerFireSpellPowerBonus(character)));
            dictValues.Add("Regen", String.Format("{0}*MP5: {1}\r\nOutFSR: {2}" , RegenInFSR.ToString("0"), BasicStats.Mp5.ToString(), RegenOutFSR.ToString("0")));

            dictValues.Add("Crit", string.Format("{0}%*{1}% from {2} Spell Crit rating\r\n{3}% from Intellect\r\n{4}% from Base Crit\r\n{5}% from Buffs\r\n{6}% on Mind Blast, Mind Flay and Mind Sear.\r\n{7}% on Smite, Holy Fire and Penance.",
                (BasicStats.SpellCrit * 100f).ToString("0.00"),
                character.StatConversion.GetSpellCritFromRating(BasicStats.CritRating).ToString("0.00"),
                BasicStats.CritRating.ToString("0"),
                (character.StatConversion.GetSpellCritFromIntellect(BasicStats.Intellect)).ToString("0.00"),
                "1,24",
                (BasicStats.SpellCrit * 100f - character.StatConversion.GetSpellCritFromRating(BasicStats.CritRating) - character.StatConversion.GetSpellCritFromIntellect(BasicStats.Intellect) - 1.24f).ToString("0.00"),
                (BasicStats.SpellCrit * 100f + character.PriestTalents.MindMelt * 2f).ToString("0.00"),
                (BasicStats.SpellCrit * 100f + character.PriestTalents.HolySpecialization * 1f).ToString("0.00")));

            float Hit = calcOptions.TargetHit;
            float BonusHit = BasicStats.SpellHit * 100f;
            float RacialHit = 0;
            string RacialText = "";
            if (character.Race == Character.CharacterRace.Draenei)
            {
                RacialHit = 1;
                RacialText = "1% from Draenei Racial\r\n";
                if (!character.ActiveBuffsContains("Heroic Presence"))
                    BonusHit += 1;
            }
            float DebuffHit = character.PriestTalents.Misery * 1f;
            if (character.ActiveBuffsConflictingBuffContains("Spell Hit Chance Taken"))
                DebuffHit = 3f;
            else
                BonusHit += DebuffHit;

            float RHitRating = 1f / character.StatConversion.GetSpellHitFromRating(1);
            float ShadowFocusHit = character.PriestTalents.ShadowFocus * 1f;
            float HitShadow = Hit + BonusHit + ShadowFocusHit;
            float HitHoly = Hit + BonusHit;
            dictValues.Add("Hit", string.Format("{0}%*{1}% from {2} Hit Rating\r\n{3}% from Buffs\r\n{4}% from {5} points in Misery\r\n{6}% from {7} points in Shadow Focus\r\n{8}{9}% Hit with Shadow spells, {10}\r\n{11}% Hit with Holy spells, {12}",
                BonusHit.ToString("0.00"),
                character.StatConversion.GetSpellHitFromRating(BasicStats.HitRating).ToString("0.00"), BasicStats.HitRating,
                (BonusHit - character.StatConversion.GetSpellHitFromRating(BasicStats.HitRating)- RacialHit - DebuffHit).ToString("0.00"),
                DebuffHit, character.PriestTalents.Misery,
                ShadowFocusHit, character.PriestTalents.ShadowFocus,
                RacialText,
                HitShadow.ToString("0.00"), (HitShadow > 100f) ? string.Format("{0} hit rating above cap", Math.Floor((HitShadow - 100f) * RHitRating)) : string.Format("{0} hit rating below cap", Math.Ceiling((100f - HitShadow) * RHitRating)),
                HitHoly.ToString("0.00"),  (HitHoly > 100f) ? string.Format("{0} hit rating above cap", Math.Floor((HitHoly - 100f) * RHitRating)) : string.Format("{0} hit rating below cap", Math.Ceiling((100f - HitHoly) * RHitRating))));

            dictValues.Add("Haste", string.Format("{0}%*{1}% from {2} Haste rating\r\n{3}% ({3}) points in Enlightenment\r\n{4}% from Buffs\r\n{5}s Global Cooldown",
                (BasicStats.SpellHaste * 100f).ToString("0.00"), character.StatConversion.GetSpellHasteFromRating(BasicStats.HasteRating).ToString("0.00"), BasicStats.HasteRating.ToString(), character.PriestTalents.Enlightenment, (BasicStats.SpellHaste * 100f - character.StatConversion.GetSpellHasteFromRating(BasicStats.HasteRating) - character.PriestTalents.Enlightenment).ToString("0.00"), Math.Max(1.0f, 1.5f / (1 + BasicStats.SpellHaste)).ToString("0.00")));

            SolverBase solver = GetSolver(character, BasicStats);
            solver.Calculate(this);

            dictValues.Add("Rotation", string.Format("{0}*{1}", solver.Name, solver.Rotation));
            dictValues.Add("DPS", string.Format("{0}*Damage Pr Second", solver.DPS.ToString("0")));
            dictValues.Add("SustainDPS", string.Format("{0}*Mana restrained DPS", solver.SustainDPS.ToString("0")));
            
            dictValues.Add("SW Pain", new ShadowWordPain(BasicStats, character).ToString());
            dictValues.Add("Devouring Plague", new DevouringPlague(BasicStats, character).ToString());
            dictValues.Add("SW Death", new ShadowWordDeath(BasicStats, character).ToString());
            dictValues.Add("Mind Blast", new MindBlast(BasicStats, character).ToString());
            dictValues.Add("PW Shield", new PowerWordShield(BasicStats, character).ToString());

            if (character.PriestTalents.VampiricEmbrace > 0)
                dictValues.Add("Vampiric Embrace", new VampiricEmbrace(BasicStats, character).ToString());
            else
                dictValues.Add("Vampiric Embrace", "- *No required talents");

            if (character.PriestTalents.VampiricTouch > 0)
                dictValues.Add("Vampiric Touch", new VampiricTouch(BasicStats, character).ToString());
            else
                dictValues.Add("Vampiric Touch", "- *No required talents");

            if (character.PriestTalents.MindFlay > 0)
                dictValues.Add("Mind Flay", new MindFlay(BasicStats, character).ToString());
            else
                dictValues.Add("Mind Flay", "- *No required talents");

            dictValues.Add("Smite", new Smite(BasicStats, character).ToString());
            dictValues.Add("Holy Fire", new HolyFire(BasicStats, character).ToString());
            if (character.PriestTalents.Penance > 0)
                dictValues.Add("Penance", new Penance(BasicStats, character).ToString());
            else
                dictValues.Add("Penance", "- *No required talents");

            return dictValues;
        }
    }
}
