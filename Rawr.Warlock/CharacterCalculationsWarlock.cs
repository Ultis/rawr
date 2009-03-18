using System;
using System.Collections.Generic;

namespace Rawr.Warlock
{

    public class CharacterCalculationsWarlock : CharacterCalculationsBase
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

        public float PetDPSPoints
        {
            get { return _subPoints[1]; }
            set { _subPoints[1] = value; }
        }

        /*public float SurvivalPoints
        {
            get { return _subPoints[2]; }
            set { _subPoints[2] = value; }
        }*/

        public Solver GetSolver(Character character, Stats stats)
        {
                return new Solver(stats, character);
        }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();
            CalculationOptionsWarlock calcOptions = character.CalculationOptions as CalculationOptionsWarlock;

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
            dictValues.Add("Spell Power", String.Format("{0}*{1} Bonus Shadow\r\n{2} Bonus Fire",
                Math.Floor(BasicStats.SpellPower),
                Math.Floor(BasicStats.SpellPower + BasicStats.SpellShadowDamageRating),
                Math.Floor(BasicStats.SpellPower + BasicStats.SpellFireDamageRating)));
            dictValues.Add("Regen", String.Format("{0}*MP5: {1}\r\nOutFSR: {2}" , RegenInFSR.ToString("0"), BasicStats.Mp5.ToString(), RegenOutFSR.ToString("0")));
            dictValues.Add("Crit", string.Format("{0}%*{1}% from {2} Spell Crit rating\r\n{3}% from Intellect\r\n{4}% from Base Crit\r\n{5}% from Buffs",
                (BasicStats.SpellCrit * 100f).ToString("0.00"),
                character.StatConversion.GetSpellCritFromRating(BasicStats.CritRating).ToString("0.00"),
                BasicStats.CritRating.ToString("0"),
                (character.StatConversion.GetSpellCritFromIntellect(BasicStats.Intellect)).ToString("0.00"),
                "1,701",
                (BasicStats.SpellCrit * 100f - character.StatConversion.GetSpellCritFromRating(BasicStats.CritRating) - character.StatConversion.GetSpellCritFromIntellect(BasicStats.Intellect) - 1.24f).ToString("0.00")));

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
            float RHitRating = 1f / character.StatConversion.GetSpellHitFromRating(1);
            float TalentHit = character.WarlockTalents.Suppression * 1f;
            float TotalHit = Hit + BonusHit;
            dictValues.Add("Hit", string.Format("{0}%*{1}% from {2} Hit Rating\r\n{3}% from Buffs\r\n{4}{5}% from {6} points in Suppression\r\n{7}",
                BonusHit.ToString("0.00"),
                character.StatConversion.GetSpellHitFromRating(BasicStats.HitRating).ToString("0.00"),
                BasicStats.HitRating,
                (BonusHit - character.StatConversion.GetSpellHitFromRating(BasicStats.HitRating) - RacialHit - TalentHit).ToString("0.00"),
                RacialText,
                TalentHit,
                character.WarlockTalents.Suppression,
                (TotalHit > 100f) ? string.Format("{0} hit rating above cap", Math.Floor((TotalHit - 100f) * RHitRating)) : string.Format("{0} hit rating below cap", Math.Ceiling((100f - TotalHit) * RHitRating))));

            dictValues.Add("Haste", string.Format("{0}%*{1}% from {2} Haste rating\r\n{3}% from Buffs\r\n{4}s Global Cooldown",
                (BasicStats.SpellHaste * 100f).ToString("0.00"),
                character.StatConversion.GetSpellHasteFromRating(BasicStats.HasteRating).ToString("0.00"),
                BasicStats.HasteRating.ToString(),
                (BasicStats.SpellHaste * 100f - character.StatConversion.GetSpellHasteFromRating(BasicStats.HasteRating) - character.PriestTalents.Enlightenment).ToString("0.00"),
                Math.Max(1.0f, 1.5f / (1 + BasicStats.SpellHaste)).ToString("0.00")));

            Solver solver = GetSolver(character, BasicStats);
            solver.Calculate(this);

            dictValues.Add("Rotation", string.Format("{0}*{1}", solver.Name, solver.Rotation));
            dictValues.Add("DPS", string.Format("{0}", solver.DPS.ToString("0")));
            dictValues.Add("Pet DPS", string.Format("{0}", solver.PetDPS.ToString("0")));
            dictValues.Add("Total DPS", string.Format("{0}", solver.TotalDPS.ToString("0")));

            dictValues.Add("Shadow Bolt", new ShadowBolt(BasicStats, character).ToString());
            dictValues.Add("Incinerate", new Incinerate(BasicStats, character).ToString());
            dictValues.Add("Immolate", new Immolate(BasicStats, character).ToString());
            dictValues.Add("Curse of Agony", new CurseOfAgony(BasicStats, character).ToString());
            dictValues.Add("Curse of Doom", new CurseOfDoom(BasicStats, character).ToString());
            dictValues.Add("Corruption", new Corruption(BasicStats, character).ToString());
            /*if (character.WarlockTalents.SiphonLife > 0)
                dictValues.Add("Siphon Life", new SiphonLife(BasicStats, character).ToString());
            else
                dictValues.Add("Siphon Life", "- *Required talent not available");*/
            if (character.WarlockTalents.UnstableAffliction > 0)
                dictValues.Add("Unstable Affliction", new UnstableAffliction(BasicStats, character).ToString());
            else
                dictValues.Add("Unstable Affliction", "- *Required talent not available");
//            dictValues.Add("Life Tap", new LifeTap(BasicStats, character).ToString());
/*            if (character.WarlockTalents.DarkPact > 0)
                dictValues.Add("Dark Pact", new DarkPact(BasicStats, character).ToString());
            else
                dictValues.Add("Dark Pact", "- *Required talent not available");*/
            dictValues.Add("Death Coil", new DeathCoil(BasicStats, character).ToString());
            dictValues.Add("Drain Life", new DrainLife(BasicStats, character).ToString());
            dictValues.Add("Drain Soul", new DrainSoul(BasicStats, character).ToString());
            if (character.WarlockTalents.Haunt > 0)
                dictValues.Add("Haunt", new Haunt(BasicStats, character).ToString());
            else
                dictValues.Add("Haunt", "- *Required talent not available");
            dictValues.Add("Seed of Corruption", new SeedOfCorruption(BasicStats, character).ToString());
            dictValues.Add("Rain of Fire", new RainOfFire(BasicStats, character).ToString());
            dictValues.Add("Hellfire", new Hellfire(BasicStats, character).ToString());
            dictValues.Add("Searing Pain", new SearingPain(BasicStats, character).ToString());
            dictValues.Add("Shadowflame", new Shadowflame(BasicStats, character).ToString());
            dictValues.Add("Soul Fire", new SoulFire(BasicStats, character).ToString());
            if (character.WarlockTalents.Shadowburn > 0)
                dictValues.Add("Shadowburn", new Shadowburn(BasicStats, character).ToString());
            else
                dictValues.Add("Shadowburn", "- *Required talent not available");
            if (character.WarlockTalents.Conflagrate > 0)
                dictValues.Add("Conflagrate", new Conflagrate(BasicStats, character).ToString());
            else
                dictValues.Add("Conflagrate", "- *Required talent not available");
            if (character.WarlockTalents.Shadowfury > 0)
                dictValues.Add("Shadowfury", new Shadowfury(BasicStats, character).ToString());
            else
                dictValues.Add("Shadowfury", "- *Required talent not available");
            if (character.WarlockTalents.ChaosBolt > 0)
                dictValues.Add("Chaos Bolt", new ChaosBolt(BasicStats, character).ToString());
            else
                dictValues.Add("Chaos Bolt", "- *Required talent not available");

            return dictValues;
        }
    }
}