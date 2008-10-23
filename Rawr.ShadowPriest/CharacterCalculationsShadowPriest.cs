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

        private float overallPoints;
        public override float OverallPoints
        {
            get { return overallPoints; }
            set { overallPoints = value; }
        }

        private float[] subPoints = new float[] { 0f, 0f, 0f };
        public override float[] SubPoints
        {
            get { return subPoints; }
            set { subPoints = value; }
        }

        public float DpsPoints
        {
            get { return subPoints[0]; }
            set { subPoints[0] = value; }
        }

        public float SustainPoints
        {
            get { return subPoints[1]; }
            set { subPoints[1] = value; }
        }

        public float SurvivalPoints
        {
            get { return subPoints[2]; }
            set { subPoints[2] = value; }
        }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();
            CalculationOptionsShadowPriest calcOptions = character.CalculationOptions as CalculationOptionsShadowPriest;

            dictValues.Add("Health", BasicStats.Health.ToString());
            dictValues.Add("Stamina", BasicStats.Stamina.ToString());
            dictValues.Add("Mana", BasicStats.Mana.ToString());
            dictValues.Add("Intellect", BasicStats.Intellect.ToString());
            dictValues.Add("Spirit", Math.Floor(BasicStats.Spirit).ToString("0"));
            dictValues.Add("Spell Power", String.Format("{0}*Shadow: {1}\r\nHoly: {2}", 
                Math.Floor(BasicStats.SpellPower), 
                Math.Floor(BasicStats.SpellPower + BasicStats.SpellShadowDamageRating), 
                Math.Floor(BasicStats.SpellPower /*+ BasicStats.SpellHolyDamageRating*/)));
            dictValues.Add("Regen", String.Format("{0}*MP5: {1}\r\nOutFSR: {2}" , RegenInFSR.ToString("0"), BasicStats.Mp5.ToString(), RegenOutFSR.ToString("0")));

            dictValues.Add("Crit", string.Format("{0}%*{1}% from {2} Spell Crit rating\r\n{3}% from Intellect\r\n{4}% from Base Crit\r\n{5}% from Buffs\r\n{6}% on Mind Blast, Mind Flay and Mind Sear.\r\n{7}% on Smite, Holy Fire and Penance.",
                (BasicStats.SpellCrit * 100f).ToString("0.00"),
                (BasicStats.CritRating / 22.07f).ToString("0.00"),
                BasicStats.CritRating.ToString("0"),
                (BasicStats.Intellect / 80f).ToString("0.00"),
                "1,24",
                (BasicStats.SpellCrit * 100f - BasicStats.CritRating / 22.07f - BasicStats.Intellect / 80f - 1.24f).ToString("0.00"),
                (BasicStats.SpellCrit * 100f + character.PriestTalents.MindMelt * 2f).ToString("0.00"),
                (BasicStats.SpellCrit * 100f + character.PriestTalents.HolySpecialization * 1f).ToString("0.00")));

            float HitCoef = 12.61538506f;
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
            float MiseryHit = 0;
            if (character.PriestTalents.Misery > 0)
            {
                MiseryHit = character.PriestTalents.Misery * 1f;
                if (!character.ActiveBuffsConflictingBuffContains("Spell Hit Chance Taken"))
                    BonusHit += MiseryHit;
            }
            float ShadowFocusHit = character.PriestTalents.ShadowFocus * 1f;
            float HitShadow = Hit + BonusHit + ShadowFocusHit;
            float HitHoly = Hit + BonusHit;
            if (!character.ActiveBuffsConflictingBuffContains("Spell Hit Chance Taken"))
                HitShadow += character.PriestTalents.Misery * 1f;
            dictValues.Add("Hit", string.Format("{0}%*{1}% from {2} Hit Rating\r\n{3}% from Buffs\r\n{4}% from {5} points in Misery\r\n{6}% from {7} points in Shadow Focus\r\n{8}{9}% Hit with Shadow spells, {10}\r\n{11}% Hit with Holy spells, {12}",
                BonusHit.ToString("0.00"),
                (BasicStats.HitRating / HitCoef).ToString("0.00"), BasicStats.HitRating,
                (BonusHit - BasicStats.HitRating / HitCoef - RacialHit - MiseryHit).ToString("0.00"),
                MiseryHit, character.PriestTalents.Misery,
                ShadowFocusHit, character.PriestTalents.ShadowFocus,
                RacialText,
                HitShadow.ToString("0.00"), (HitShadow > 100f) ? string.Format("{0} hit rating above cap", Math.Floor((HitShadow - 100f) * HitCoef)) : string.Format("{0} hit rating below cap", Math.Ceiling((100f - HitShadow) * HitCoef)),
                HitHoly.ToString("0.00"),  (HitHoly > 100f) ? string.Format("{0} hit rating above cap", Math.Floor((HitHoly - 100f) * HitCoef)) : string.Format("{0} hit rating below cap", Math.Ceiling((100f - HitHoly) * HitCoef))));

            dictValues.Add("Haste", string.Format("{0}%*{1}% from {2} Haste rating\r\n{3}% ({3}) points in Enlightenment\r\n{4}% from Buffs\r\n{5}s Global Cooldown",
                (BasicStats.SpellHaste * 100f).ToString("0.00"), (BasicStats.HasteRating / 15.77).ToString("0.00"), BasicStats.HasteRating.ToString(), character.PriestTalents.Enlightenment, (BasicStats.SpellHaste * 100f - (BasicStats.HasteRating / 15.77f)).ToString("0.00"), Math.Max(1.0f, 1.5f / (1 + BasicStats.SpellHaste)).ToString("0.00")));

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

            SolverShadow solver = new SolverShadow(BasicStats, character);
            solver.Calculate(this);

            dictValues.Add("Damage done", solver.OverallDamage.ToString("0"));
            dictValues.Add("DPS", string.Format("{0}*Damage Pr Second", solver.DPS.ToString("0")));
            dictValues.Add("SustainDPS", string.Format("{0}*Mana restrained DPS", solver.SustainDPS.ToString("0")));

            return dictValues;
        }
    }
}
