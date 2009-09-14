using System;
using System.Collections.Generic;
using System.Globalization;

namespace Rawr.Warlock 
{
    public class CharacterCalculationsWarlock : CharacterCalculationsBase 
    {
        #region Variables
        private Stats basicStats;
        private Character character;

        public float SpiritRegen { get; set; }
        public float RegenInFSR { get; set; }
        public float RegenOutFSR { get; set; }
        public CharacterRace Race { get; set; }

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
                foreach (float f2 in _subPoints) { f += f2; }
                return f;
            }
            set { }
        }

        private float[] _subPoints = new float[] { 0f, 0f };
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
        #endregion

        public Solver GetSolver(Character character, Stats stats) 
        { 
            return new Solver(stats, character); 
        }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues() 
        {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();
            CalculationOptionsWarlock calcOptions = character.CalculationOptions as CalculationOptionsWarlock;

            float Hit = calcOptions.TargetHit;
            float BonusHit = BasicStats.SpellHit * 100f;
            float RacialHit = 0;

            if (character.Race == CharacterRace.Draenei)
            {
                RacialHit = 1;
                if (!character.ActiveBuffsContains("Heroic Presence")) 
                { 
                    BonusHit += 1; 
                }
            }
            float RHitRating = 0.01f / StatConversion.GetSpellHitFromRating(1);
            float TalentHit = character.WarlockTalents.Suppression * 1f;
            float TotalHit = Hit + BonusHit;

            Solver solver = GetSolver(character, BasicStats);
            solver.Calculate(this);

            #region Simulation stats
            dictValues.Add("Rotation", String.Format(CultureInfo.InvariantCulture, "{0}*{1}", solver.Name, solver.Rotation));
            dictValues.Add("DPS", String.Format(CultureInfo.InvariantCulture, "{0}", solver.DPS.ToString("0", CultureInfo.InvariantCulture)));
            dictValues.Add("Pet DPS", String.Format(CultureInfo.InvariantCulture, "{0}", solver.PetDPS.ToString("0", CultureInfo.InvariantCulture)));
            dictValues.Add("Total DPS", String.Format(CultureInfo.InvariantCulture, "{0}", solver.TotalDPS.ToString("0", CultureInfo.InvariantCulture)));
            #endregion
            #region HP/Mana stats
            dictValues.Add("Health", BasicStats.Health.ToString("0", CultureInfo.InvariantCulture));
            dictValues.Add("Mana", BasicStats.Mana.ToString("0", CultureInfo.InvariantCulture));
            #endregion
            #region Base stats
            dictValues.Add("Strength", BasicStats.Strength.ToString(CultureInfo.InvariantCulture));
            dictValues.Add("Agility", BasicStats.Agility.ToString(CultureInfo.InvariantCulture));
            dictValues.Add("Stamina", BasicStats.Stamina.ToString(CultureInfo.InvariantCulture));
            dictValues.Add("Intellect", BasicStats.Intellect.ToString(CultureInfo.InvariantCulture));
            dictValues.Add("Spirit", BasicStats.Spirit.ToString(CultureInfo.InvariantCulture));
            dictValues.Add("Armor", BasicStats.Armor.ToString(CultureInfo.InvariantCulture));
            #endregion
            #region Spell stats
            dictValues.Add("Bonus Damage", String.Format(CultureInfo.InvariantCulture, "{0}*Shadow Damage\t{1}\r\nFire Damage\t{2}\r\n\r\nYour Fire Damage increases your pet's Attack Power by {3} and Spell Damage by {4}.",
                BasicStats.SpellPower,
                BasicStats.SpellPower + BasicStats.SpellShadowDamageRating,
                BasicStats.SpellPower + BasicStats.SpellFireDamageRating,
                Math.Round((BasicStats.SpellPower + BasicStats.SpellFireDamageRating) * 0.57f, 0),
                Math.Round((BasicStats.SpellPower + BasicStats.SpellFireDamageRating) * 0.15f, 0)
                ));

            dictValues.Add("Hit Rating", String.Format("{0}", BasicStats.HitRating));

            dictValues.Add("Miss Chance", String.Format(CultureInfo.InvariantCulture, "{0}%*{1}% Total Hit Chance\r\n\r\n{2}%\tfrom {3} Hit Rating\r\n{4}%\tfrom {5} points in Suppression\r\n{6}%\tfrom Buffs\r\n\r\n{7}",
                Math.Max(0, 100 - TotalHit).ToString("0.00", CultureInfo.InvariantCulture),
                BonusHit.ToString("0.00", CultureInfo.InvariantCulture),
                (StatConversion.GetSpellHitFromRating(BasicStats.HitRating) * 100f).ToString("0.00", CultureInfo.InvariantCulture),
                BasicStats.HitRating,
                TalentHit,
                character.WarlockTalents.Suppression,
                (BonusHit - StatConversion.GetSpellHitFromRating(BasicStats.HitRating) * 100f - RacialHit - TalentHit).ToString("0.00", CultureInfo.InvariantCulture),
                (TotalHit > 100f) ? String.Format(CultureInfo.InvariantCulture, "{0} hit rating above cap", Math.Floor((TotalHit - 100f) * RHitRating)) : String.Format(CultureInfo.InvariantCulture, "{0} hit rating below cap", Math.Ceiling((100f - TotalHit) * RHitRating))));
            
            dictValues.Add("Crit", String.Format(CultureInfo.InvariantCulture, "{0}%*{1}%\tfrom {2} Spell Crit rating\r\n{3}%\tfrom Intellect\r\n{4}%\tfrom Base Crit\r\n{5}%\tfrom Demonic Tactics\r\n{6}%\tfrom Backlash",
                    (BasicStats.SpellCrit * 100f).ToString("0.00", CultureInfo.InvariantCulture),
                    (StatConversion.GetSpellCritFromRating(BasicStats.CritRating) * 100f).ToString("0.00", CultureInfo.InvariantCulture),
                    BasicStats.CritRating.ToString("0", CultureInfo.InvariantCulture),
                    (StatConversion.GetSpellCritFromIntellect(BasicStats.Intellect) * 100f).ToString("0.00", CultureInfo.InvariantCulture),
                    "1.701",
                    ((character.WarlockTalents.DemonicTactics * 0.02f) * 100f).ToString("0", CultureInfo.InvariantCulture),
                    ((character.WarlockTalents.Backlash * 0.01f) * 100f).ToString("0", CultureInfo.InvariantCulture)
                ));

            dictValues.Add("Haste", String.Format(CultureInfo.InvariantCulture, "{0}%*{1}%\tfrom {2} Haste rating\r\n{3}%\tfrom Buffs\r\n{4}s\tGlobal Cooldown",
                (BasicStats.SpellHaste * 100f).ToString("0.00", CultureInfo.InvariantCulture),
                (StatConversion.GetSpellHasteFromRating(BasicStats.HasteRating) * 100f).ToString("0.00", CultureInfo.InvariantCulture),
                BasicStats.HasteRating.ToString(CultureInfo.InvariantCulture),
                (BasicStats.SpellHaste * 100f - StatConversion.GetSpellHasteFromRating(BasicStats.HasteRating) * 100f).ToString("0.00", CultureInfo.InvariantCulture),
                Math.Max(1.0f, 1.5f / (1 + BasicStats.SpellHaste)).ToString("0.00", CultureInfo.InvariantCulture)));

            dictValues.Add("Mana Regen", String.Format(CultureInfo.InvariantCulture, "{0}*{1} mana regenerated every 5 seconds while not casting\r\n{2} mana regenerated every 5 seconds while casting", 
                RegenOutFSR.ToString(CultureInfo.InvariantCulture),
                RegenOutFSR.ToString(CultureInfo.InvariantCulture), 
                RegenInFSR.ToString(CultureInfo.InvariantCulture)));
            #endregion
            #region Shadow school
            dictValues.Add("Shadow Bolt", new ShadowBolt(BasicStats, character).ToString());
            if (character.WarlockTalents.Haunt > 0)
                dictValues.Add("Haunt", new Haunt(BasicStats, character).ToString());
            else
                dictValues.Add("Haunt", "- *Required talent not available");
            dictValues.Add("Corruption", new Corruption(BasicStats, character).ToString());
            dictValues.Add("Curse of Agony", new CurseOfAgony(BasicStats, character).ToString());
            dictValues.Add("Curse of Doom", new CurseOfDoom(BasicStats, character).ToString());
            if (character.WarlockTalents.UnstableAffliction > 0)
                dictValues.Add("Unstable Affliction", new UnstableAffliction(BasicStats, character).ToString());
            else
                dictValues.Add("Unstable Affliction", "- *Required talent not available");
            dictValues.Add("Death Coil", new DeathCoil(BasicStats, character).ToString());
            dictValues.Add("Drain Life", new DrainLife(BasicStats, character).ToString());
            dictValues.Add("Drain Soul", new DrainSoul(BasicStats, character).ToString());
            dictValues.Add("Seed of Corruption", new SeedOfCorruption(BasicStats, character).ToString());
            dictValues.Add("Shadowflame", new Shadowflame(BasicStats, character).ToString());
            if (character.WarlockTalents.Shadowburn > 0)
                dictValues.Add("Shadowburn", new Shadowburn(BasicStats, character).ToString());
            else
                dictValues.Add("Shadowburn", "- *Required talent not available");
            if (character.WarlockTalents.Shadowfury > 0)
                dictValues.Add("Shadowfury", new Shadowfury(BasicStats, character).ToString());
            else
                dictValues.Add("Shadowfury", "- *Required talent not available");
            /*
            dictValues.Add("Life Tap", new LifeTap(BasicStats, character).ToString());
            if (character.WarlockTalents.DarkPact > 0)
                dictValues.Add("Dark Pact", new DarkPact(BasicStats, character).ToString());
            else
            dictValues.Add("Dark Pact", "- *Required talent not available");
            */
            #endregion
            #region Fire school
            dictValues.Add("Incinerate", new Incinerate(BasicStats, character).ToString());
            dictValues.Add("Immolate", new Immolate(BasicStats, character).ToString());
            if (character.WarlockTalents.Conflagrate > 0)
                dictValues.Add("Conflagrate", new Conflagrate(BasicStats, character).ToString());
            else
                dictValues.Add("Conflagrate", "- *Required talent not available");
            if (character.WarlockTalents.ChaosBolt > 0)
                dictValues.Add("Chaos Bolt", new ChaosBolt(BasicStats, character).ToString());
            else
                dictValues.Add("Chaos Bolt", "- *Required talent not available");
            dictValues.Add("Rain of Fire", new RainOfFire(BasicStats, character).ToString());
            dictValues.Add("Hellfire", new Hellfire(BasicStats, character).ToString());
            dictValues.Add("Searing Pain", new SearingPain(BasicStats, character).ToString());
            dictValues.Add("Soul Fire", new SoulFire(BasicStats, character).ToString());
            #endregion
            return dictValues;
        }
    }
}