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
            float DebuffHit = 0;
            if (character.ActiveBuffsContains("Spell Hit Chance Taken"))
            {
                DebuffHit = 3f;
                BonusHit += DebuffHit;
            }
            float RHitRating = 1f / character.StatConversion.GetSpellHitFromRating(1);
            float AfflictionHit = character.WarlockTalents.Suppression * 1f;
            float DestructionHit = character.WarlockTalents.Cataclysm * 1f;
            float HitAffliction = Hit + BonusHit + AfflictionHit;
            float HitDestruction = Hit + BonusHit + DestructionHit;
            dictValues.Add("Hit", string.Format("{0}%*{1}% from {2} Hit Rating\r\n{3}% from Buffs\r\n{4}% from Misery or Improved Faerie Fire\r\n{5}% from {6} points in Suppression\r\n{7}% from {8} points in Cataclysm\r\n{9}{10}% Hit with Shadow spells, {11}\r\n{12}% Hit with Fire spells, {13}",
                BonusHit.ToString("0.00"),
                character.StatConversion.GetSpellHitFromRating(BasicStats.HitRating).ToString("0.00"), 
                BasicStats.HitRating,
                (BonusHit - character.StatConversion.GetSpellHitFromRating(BasicStats.HitRating)- RacialHit - DebuffHit).ToString("0.00"),
                DebuffHit, 
                AfflictionHit, 
                character.WarlockTalents.Suppression,
                DestructionHit, 
                character.WarlockTalents.Cataclysm,
                RacialText,
                HitAffliction.ToString("0.00"), 
                (HitAffliction > 100f) ? string.Format("{0} hit rating above cap", Math.Floor((HitAffliction - 100f) * RHitRating)) : string.Format("{0} hit rating below cap", Math.Ceiling((100f - HitAffliction) * RHitRating)),
                HitDestruction.ToString("0.00"),  
                (HitDestruction > 100f) ? string.Format("{0} hit rating above cap", Math.Floor((HitDestruction - 100f) * RHitRating)) : string.Format("{0} hit rating below cap", Math.Ceiling((100f - HitDestruction) * RHitRating))));

            dictValues.Add("Haste", string.Format("{0}%*{1}% from {2} Haste rating\r\n{3}% from Buffs\r\n{4}s Global Cooldown",
                (BasicStats.SpellHaste * 100f).ToString("0.00"), character.StatConversion.GetSpellHasteFromRating(BasicStats.HasteRating).ToString("0.00"), BasicStats.HasteRating.ToString(), (BasicStats.SpellHaste * 100f - character.StatConversion.GetSpellHasteFromRating(BasicStats.HasteRating) - character.PriestTalents.Enlightenment).ToString("0.00"), Math.Max(1.0f, 1.5f / (1 + BasicStats.SpellHaste)).ToString("0.00")));

            Solver solver = GetSolver(character, BasicStats);
            solver.Calculate(this);

            dictValues.Add("Rotation", string.Format("{0}*{1}", solver.Name, solver.Rotation));
            dictValues.Add("Burst DPS", string.Format("{0}*DPS", solver.DPS.ToString("0")));

            dictValues.Add("Shadow Bolt", new ShadowBolt(BasicStats, character).ToString());
            dictValues.Add("Incinerate", new Incinerate(BasicStats, character).ToString());
            dictValues.Add("Immolate", new Immolate(BasicStats, character).ToString());
            dictValues.Add("Curse of Agony", new CurseOfAgony(BasicStats, character).ToString());
            dictValues.Add("Curse of Doom", new CurseOfDoom(BasicStats, character).ToString());
            dictValues.Add("Corruption", new Corruption(BasicStats, character).ToString());
            if (character.WarlockTalents.SiphonLife > 0)
                dictValues.Add("Siphon Life", new SiphonLife(BasicStats, character).ToString());
            else
                dictValues.Add("Siphon Life", "- *Required talent not available");
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

/*using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Reflection;

namespace Rawr.Warlock
{
    public enum FillerSpell
    {
        Shadowbolt,
        Incinerate
    }

    public enum CastedCurse
    {
        CurseOfAgony, 
        CurseOfDoom, 
        CurseOfTheElements,
        CurseOfShadow, 
        CurseOfRecklessness,
        CurseOfWeakness, 
        CurseOfTongues
    }

    public enum Pet
    {
        Succubus,
        Felhunter,
        Imp, 
        Voidwalker,
        Felguard
    }

    [Serializable]
    public class CalculationOptionsWarlock : ICalculationOptionBase
    {

        public string GetXml()
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsWarlock));
            StringBuilder xml = new StringBuilder();
            System.IO.StringWriter writer = new System.IO.StringWriter(xml);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }

        public float Latency { get; set; }
        public int TargetLevel { get; set; }
        public float FightDuration { get; set; }
        public FillerSpell FillerSpell { get; set; }
        public CastedCurse CastedCurse { get; set; }
        public Pet Pet { get; set; }
        public bool PetSacrificed { get; set; }
        public float DotGap { get; set; }
        public int AfflictionDebuffs { get; set; }
        public float ShadowPriestDps { get; set; }
        public bool CastImmolate { get; set; }
        public bool CastCorruption { get; set; }
        public bool CastUnstableAffliction { get; set; }
        public bool CastSiphonLife { get; set; }
        public bool CastShadowburn { get; set; }
        public bool CastConflagrate { get; set; }

        //affliction talents
        public int Suppression { get; set; }
        public int ImprovedCorruption { get; set; }
        public int ImprovedDrainSoul { get; set; }
        public int ImprovedLifeTap { get; set; }
        public int SoulSiphon { get; set; }
        public int ImprovedCurseOfAgony { get; set; }
        public int AmplifyCurse { get; set; }
        public int Nightfall { get; set; }
        public int EmpoweredCorruption { get; set; }
        public int SiphonLife { get; set; }
        public int ShadowMastery { get; set; }
        public int Contagion { get; set; }
        public int DarkPact { get; set; }
        public int UnstableAffliction { get; set; }
        public int Frailty { get; set; }
        public int ImprovedFear { get; set; }
        public int ImprovedFelhunter { get; set; }
        public int Eradication { get; set; }
        public int DeathsEmbrace { get; set; }
        public int Pandemic { get; set; }
        public int EverlastingAffliction { get; set; }
        public int Haunt { get; set; }

        //demonology talents
        public int ImprovedImp { get; set; }
        public int DemonicEmbrace { get; set; }
        public int FelIntellect { get; set; }
        public int ImprovedSuccubus { get; set; }
        public int FelStamina { get; set; }
        public int DemonicAegis { get; set; }
        public int UnholyPower { get; set; }
        public int DemonicSacrifice { get; set; }
        public int ManaFeed { get; set; }
        public int MasterDemonologist { get; set; }
        public int SoulLink { get; set; }
        public int DemonicKnowledge { get; set; }
        public int DemonicTactics { get; set; }
        public int SummonFelguard { get; set; }
        public int DemonicBrutality { get; set; }
        public int FelVitality { get; set; }
        public int DemonicEmpowerment { get; set; }
        public int FelSynergy { get; set; }
        public int ImprovedDemonicTactics { get; set; }
        public int DemonicEmpathy { get; set; }
        public int DemonicPact { get; set; }
        public int Metamorphosis { get; set; }

        //destruction talents
        public int ImprovedShadowBolt { get; set; }
        public int Cataclysm { get; set; }
        public int Bane { get; set; }
        public int ImprovedFirebolt { get; set; }
        public int ImprovedLashOfPain { get; set; }
        public int Devastation { get; set; }
        public int Shadowburn { get; set; }
        public int ImprovedSearingPain { get; set; }
        public int ImprovedImmolate { get; set; }
        public int Ruin { get; set; }
        public int Emberstorm { get; set; }
        public int Backlash { get; set; }
        public int Conflagrate { get; set; }
        public int SoulLeech { get; set; }
        public int ShadowAndFlame { get; set; }
        public int MoltenCore { get; set; }
        public int DemonicPower { get; set; }
        public int ImprovedSoulLeech { get; set; }
        public int Backdraft { get; set; }
        public int EmpoweredImp { get; set; }
        public int FireAndBrimstone { get; set; }
        public int ChaosBolt { get; set; }

        //not implemented
        public int ImprovedCurseOfWeakness { get; set; }
        public int FelConcentration { get; set; }
        public int GrimReach { get; set; }
        public int ShadowEmbrace { get; set; }
        public int CurseOfExhaustion { get; set; }
        public int ImprovedHowlOfTerror { get; set; }
        public int Malediction { get; set; }
        public int ImprovedHealthstone { get; set; }
        public int ImprovedHealthFunnel { get; set; }
        public int ImprovedVoidwalker { get; set; }
        public int FelDomination { get; set; }
        public int MasterSummoner { get; set; }
        public int ImprovedEnslaveDemon { get; set; }
        public int MasterConjuror { get; set; }
        public int DemonicResilience { get; set; }
        public int Aftermath { get; set; }
        public int Intensity { get; set; }
        public int DestructiveReach { get; set; }
        public int Pyroclasm { get; set; }
        public int NetherProtection { get; set; }
        public int Shadowfury { get; set; }

        private CalculationOptionsWarlock()
        {
        }
        
        public CalculationOptionsWarlock(Character character)
        {
            ImportTalents(character);

            Latency = 0.05f;
            TargetLevel = 83;
            FightDuration = 300;
            DotGap = 1;
            AfflictionDebuffs = 12;
            FillerSpell = FillerSpell.Shadowbolt;
            CastedCurse = CastedCurse.CurseOfShadow;
            CastImmolate = false;
            CastCorruption = false;
            CastUnstableAffliction = false;
            CastSiphonLife = false;
            CastShadowburn = false;
            CastConflagrate = false;
            Pet = Pet.Succubus;
            PetSacrificed = true;
        }

        private void ImportTalents(Character character)
        {
            try
            {
                WebRequestWrapper wrw = new WebRequestWrapper();

                if (character.Class == Character.CharacterClass.Warlock && character.Name != null && character.Realm != null)
                {
                    XmlDocument docTalents = wrw.DownloadCharacterTalentTree(character.Name, character.Region, character.Realm);

                    if (docTalents != null)
                    {
                        string talentCode = docTalents.SelectSingleNode("page/characterInfo/talentTab/talentTree").Attributes["value"].Value;
                        Suppression = int.Parse(talentCode.Substring(0, 1));
                        ImprovedCorruption = int.Parse(talentCode.Substring(1, 1));
                        ImprovedCurseOfWeakness = int.Parse(talentCode.Substring(2, 1));
                        ImprovedDrainSoul = int.Parse(talentCode.Substring(3, 1));
                        ImprovedLifeTap = int.Parse(talentCode.Substring(4, 1));
                        SoulSiphon = int.Parse(talentCode.Substring(5, 1));
                        ImprovedCurseOfAgony = int.Parse(talentCode.Substring(6, 1));
                        FelConcentration = int.Parse(talentCode.Substring(7, 1));
                        AmplifyCurse = int.Parse(talentCode.Substring(8, 1));
                        GrimReach = int.Parse(talentCode.Substring(9, 1));
                        Nightfall = int.Parse(talentCode.Substring(10, 1));
                        EmpoweredCorruption = int.Parse(talentCode.Substring(11, 1));
                        ShadowEmbrace = int.Parse(talentCode.Substring(12, 1));
                        SiphonLife = int.Parse(talentCode.Substring(13, 1));
                        CurseOfExhaustion = int.Parse(talentCode.Substring(14, 1));
                        ShadowMastery = int.Parse(talentCode.Substring(15, 1));
                        Contagion = int.Parse(talentCode.Substring(16, 1));
                        DarkPact = int.Parse(talentCode.Substring(17, 1));
                        ImprovedHowlOfTerror = int.Parse(talentCode.Substring(18, 1));
                        Malediction = int.Parse(talentCode.Substring(19, 1));
                        UnstableAffliction = int.Parse(talentCode.Substring(20, 1));
                        ImprovedHealthstone = int.Parse(talentCode.Substring(21, 1));
                        ImprovedImp = int.Parse(talentCode.Substring(22, 1));
                        DemonicEmbrace = int.Parse(talentCode.Substring(23, 1));
                        ImprovedHealthFunnel = int.Parse(talentCode.Substring(24, 1));
                        ImprovedVoidwalker = int.Parse(talentCode.Substring(25, 1));
                        FelIntellect = int.Parse(talentCode.Substring(26, 1));
                        ImprovedSuccubus = int.Parse(talentCode.Substring(27, 1));
                        FelDomination = int.Parse(talentCode.Substring(28, 1));
                        FelStamina = int.Parse(talentCode.Substring(29, 1));
                        DemonicAegis = int.Parse(talentCode.Substring(30, 1));
                        MasterSummoner = int.Parse(talentCode.Substring(31, 1));
                        UnholyPower = int.Parse(talentCode.Substring(32, 1));
                        ImprovedEnslaveDemon = int.Parse(talentCode.Substring(33, 1));
                        DemonicSacrifice = int.Parse(talentCode.Substring(34, 1));
                        MasterConjuror = int.Parse(talentCode.Substring(35, 1));
                        ManaFeed = int.Parse(talentCode.Substring(36, 1));
                        MasterDemonologist = int.Parse(talentCode.Substring(37, 1));
                        DemonicResilience = int.Parse(talentCode.Substring(38, 1));
                        SoulLink = int.Parse(talentCode.Substring(39, 1));
                        DemonicKnowledge = int.Parse(talentCode.Substring(40, 1));
                        DemonicTactics = int.Parse(talentCode.Substring(41, 1));
                        SummonFelguard = int.Parse(talentCode.Substring(42, 1));
                        ImprovedShadowBolt = int.Parse(talentCode.Substring(43, 1));
                        Cataclysm = int.Parse(talentCode.Substring(44, 1));
                        Bane = int.Parse(talentCode.Substring(45, 1));
                        Aftermath = int.Parse(talentCode.Substring(46, 1));
                        ImprovedFirebolt = int.Parse(talentCode.Substring(47, 1));
                        ImprovedLashOfPain = int.Parse(talentCode.Substring(48, 1));
                        Devastation = int.Parse(talentCode.Substring(49, 1));
                        Shadowburn = int.Parse(talentCode.Substring(50, 1));
                        Intensity = int.Parse(talentCode.Substring(51, 1));
                        DestructiveReach = int.Parse(talentCode.Substring(52, 1));
                        ImprovedSearingPain = int.Parse(talentCode.Substring(53, 1));
                        Pyroclasm = int.Parse(talentCode.Substring(54, 1));
                        ImprovedImmolate = int.Parse(talentCode.Substring(55, 1));
                        Ruin = int.Parse(talentCode.Substring(56, 1));
                        NetherProtection = int.Parse(talentCode.Substring(57, 1));
                        Emberstorm = int.Parse(talentCode.Substring(58, 1));
                        Backlash = int.Parse(talentCode.Substring(59, 1));
                        Conflagrate = int.Parse(talentCode.Substring(60, 1));
                        SoulLeech = int.Parse(talentCode.Substring(61, 1));
                        ShadowAndFlame = int.Parse(talentCode.Substring(62, 1));
                        Shadowfury = int.Parse(talentCode.Substring(63, 1));
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public CalculationOptionsWarlock Clone()
        {
            return MemberwiseClone() as CalculationOptionsWarlock;
        }

        public int GetTalentByName(string name)
        {
            Type t = typeof(CalculationOptionsWarlock);
            return (int)t.GetProperty(name).GetValue(this, null);
        }

        public void SetTalentByName(string name, int value)
        {
            Type t = typeof(CalculationOptionsWarlock);
            t.GetProperty(name).SetValue(this, value, null);
        } 
    }

    public class CharacterCalculationsWarlock : CharacterCalculationsBase
    {

        private float _overallPoints = 0f;
        public override float OverallPoints
        {
            get { return _overallPoints; }
            set { _overallPoints = value; }
        }

        /// <summary>
        /// The Sub rating points for the whole character, in the order defined in SubPointNameColors.
        /// Should sum up to OverallPoints. You could have this field build/parse an array of floats based
        /// on floats stored in other fields, or you could have this get/set a private float[], and
        /// have the fields of your individual Sub points refer to specific indexes of this field.
        /// </summary>
        private float[] _subPoints = new float[] { 0f };
        public override float[] SubPoints
        {
            get { return _subPoints; }
            set { _subPoints = value; }
        }

        public float DpsRating
        {
            get { return _subPoints[0]; }
        }

        public Stats BasicStats { get; set; }
        public CalculationOptionsWarlock CalculationOptions { get; set; }
        public WarlockSpellRotation SpellRotation { get; set; }

        public float GlobalCooldown { get; set; }
        public float HitPercent { get; set; }
        public float CritPercent { get; set; }
        public float HastePercent { get; set; }
        public float ShadowDamage { get; set; }
        public float FireDamage { get; set; }

        public float TotalDamage { get; set; }
        public float IsbUptime { get; set; }

        private Stats _totalStats;
        public Stats TotalStats
        {
            get { return _totalStats; }
            set { _totalStats = value; }
        }

        public int NumLifetaps
        {
            get;
            set;
        }

        public int LifetapManaReturn
        {
            get;
            set;
        }

        public float DPS { get; set; }

        public Character character { get; set; }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            CalculationsWarlock cw = new CalculationsWarlock();
            
            Dictionary<string, string> vals = new Dictionary<string, string>();
            vals.Add("Health", BasicStats.Health.ToString());
            vals.Add("Mana", BasicStats.Mana.ToString());
            vals.Add("Stamina", BasicStats.Stamina.ToString());
            vals.Add("Intellect", BasicStats.Intellect.ToString());
            vals.Add("Spirit", BasicStats.Spirit.ToString());
            vals.Add("Total Crit %", CritPercent.ToString("0.00"));
            vals.Add("Hit %", HitPercent.ToString("0.00"));
            vals.Add("Haste %", HastePercent.ToString("0.00"));
            vals.Add("Shadow Damage", ShadowDamage.ToString("0"));
            vals.Add("Fire Damage", FireDamage.ToString("0"));
            vals.Add("ISB Uptime", IsbUptime.ToString("0.00"));
            vals.Add("Total Damage", TotalDamage.ToString());
            vals.Add("DPS", Math.Round(DpsRating).ToString());
            
            //vals.Add("Casting Speed", (1f / (TotalStats.SpellHasteRating / 1570f + 1f)).ToString());
            vals.Add("Shadow Damage", (TotalStats.SpellShadowDamageRating + TotalStats.SpellDamageRating).ToString());
            vals.Add("Fire Damage", (TotalStats.SpellFireDamageRating + TotalStats.SpellDamageRating).ToString());
            vals.Add("DPS", DPS.ToString());
            //if (Spells.Exists(delegate(Spell s) { return s.Name.ToUpper() == "SHADOWBOLT"; }))
            //{
            //    ShadowBolt sb = new ShadowBolt(character, TotalStats);
            //    vals.Add("SB Min Hit", sb.MinDamage.ToString());
            //    vals.Add("SB Max Hit", sb.MaxDamage.ToString());
            //    vals.Add("SB Min Crit", (sb.MinDamage * sb.CritModifier).ToString());
            //    vals.Add("SB Max Crit", (sb.MaxDamage * sb.CritModifier).ToString());
            //    vals.Add("SB Average Hit", sb.AverageDamage.ToString());
            //    vals.Add("SB Crit Rate", sb.CritPercent.ToString());
            //    vals.Add("ISB Uptime", (sb.ISBuptime * 100f).ToString());
            //    vals.Add("#SB Casts", NumCasts[sb].ToString());
            //}
            //else
            //{
            //    vals.Add("SB Min Hit", "");
            //    vals.Add("SB Max Hit", "");
            //    vals.Add("SB Min Crit", "");
            //    vals.Add("SB Max Crit", "");
            //    vals.Add("SB Average Hit", "");
            //    vals.Add("SB Crit Rate", "");
            //    vals.Add("ISB Uptime", "");
            //    vals.Add("#SB Casts", "0");
            //}
            //if (Spells.Exists(delegate(Spell s) { return s.Name.ToUpper() == "INCINERATE"; }))
            //{
            //    Incinerate sb = new Incinerate(character, TotalStats);
            //    vals.Add("Incinerate Min Hit", sb.MinDamage.ToString());
            //    vals.Add("Incinerate Max Hit", sb.MaxDamage.ToString());
            //    vals.Add("Incinerate Min Crit", (sb.MinDamage * sb.CritModifier).ToString());
            //    vals.Add("Incinerate Max Crit", (sb.MaxDamage * sb.CritModifier).ToString());
            //    vals.Add("Incinerate Average Hit", sb.AverageDamage.ToString());
            //    vals.Add("Incinerate Crit Rate", sb.CritPercent.ToString());
            //    vals.Add("#Incinerate Casts", NumCasts[sb].ToString());
            //}
            //else
            //{
            //    vals.Add("Incinerate Min Hit","");
            //    vals.Add("Incinerate Max Hit","");
            //    vals.Add("Incinerate Min Crit","");
            //    vals.Add("Incinerate Max Crit","");
            //    vals.Add("Incinerate Average Hit", "");
            //    vals.Add("Incinerate Crit Rate","");
            //    vals.Add("#Incinerate Casts","0");
            //}
            //if (Spells.Exists(delegate(Spell s) { return s.Name.ToUpper() == "IMMOLATE"; }))
            //{
            //    Immolate sb = new Immolate(character, TotalStats);
            //    vals.Add("ImmolateMin Hit", sb.MinDamage.ToString());
            //    vals.Add("ImmolateMax Hit", sb.MaxDamage.ToString());
            //    vals.Add("ImmolateMin Crit", (sb.MinDamage * sb.CritModifier).ToString());
            //    vals.Add("ImmolateMax Crit", (sb.MaxDamage * sb.CritModifier).ToString());
            //    vals.Add("ImmolateAverage Hit", sb.AverageDamage.ToString());
            //    vals.Add("ImmolateCrit Rate", sb.CritPercent.ToString());
            //    vals.Add("#Immolate Casts", NumCasts[sb].ToString());
            //}
            //else
            //{
            //    vals.Add("ImmolateMin Hit","");
            //    vals.Add("ImmolateMax Hit","");
            //    vals.Add("ImmolateMin Crit","");
            //    vals.Add("ImmolateMax Crit","");
            //    vals.Add("ImmolateAverage Hit","");
            //    vals.Add("ImmolateCrit Rate","");
            //    vals.Add("#Immolate Casts","0");
            //}
            //if (Spells.Exists(delegate(Spell s) { return s.Name.ToUpper() == "CURSEOFAGONY"; }))
            //{
            //    CurseOfAgony sb = new CurseOfAgony(character, TotalStats);
            //    vals.Add("CoA Tick", (sb.AverageDamage / (sb.PeriodicDuration / sb.PeriodicTickInterval)).ToString());
            //    vals.Add("CoA Total Damage", sb.AverageDamage.ToString());
            //    vals.Add("#CoA Casts", NumCasts[sb].ToString());
            //}
            //else
            //{
            //    vals.Add("CoA Tick","");
            //    vals.Add("CoA Total Damage","");
            //    vals.Add("#CoA Casts","0");
            //}
            //if (Spells.Exists(delegate(Spell s) { return s.Name.ToUpper() == "CURSEOFDOOM"; }))
            //{
            //    CurseOfDoom sb = new CurseOfDoom(character, TotalStats);
            //    vals.Add("CoD Total Damage", sb.AverageDamage.ToString());
            //    vals.Add("#CoD Casts", NumCasts[sb].ToString());
            //}
            //else
            //{
            //    vals.Add("CoD Total Damage","");
            //    vals.Add("#CoD Casts","");
            //}
            //if (Spells.Exists(delegate(Spell s) { return s.Name.ToUpper() == "CORRUPTION"; }))
            //{
            //    Corruption sb = new Corruption(character, TotalStats);
            //    vals.Add("Corr Tick", (sb.AverageDamage / (sb.PeriodicDuration / sb.PeriodicTickInterval)).ToString());
            //    vals.Add("Corr Total Damage", sb.AverageDamage.ToString());
            //    vals.Add("#Corr Casts", NumCasts[sb].ToString());
            //}
            //else
            //{
            //    vals.Add("Corr Tick", "");
            //    vals.Add("Corr Total Damage","");
            //    vals.Add("#Corr Casts","");
            //}
            //if (Spells.Exists(delegate(Spell s) { return s.Name.ToUpper() == "UNSTABLEAFFLICTION"; }))
            //{
            //    UnstableAffliction sb = new UnstableAffliction(character, TotalStats);
            //    vals.Add("UA Tick", (sb.AverageDamage / (sb.PeriodicDuration / sb.PeriodicTickInterval)).ToString());
            //    vals.Add("UA Total Damage", sb.AverageDamage.ToString());
            //    vals.Add("#UA Casts", NumCasts[sb].ToString());
            //}
            //else
            //{
            //    vals.Add("UA Tick", "");
            //    vals.Add("UA Total Damage","");
            //    vals.Add("#UA Casts", "0");
            //}
            //if (Spells.Exists(delegate(Spell s) { return s.Name.ToUpper() == "SIPHONLIFE"; }))
            //{
            //    SiphonLife sb = new SiphonLife(character, TotalStats);
            //    vals.Add("SL Tick", (sb.AverageDamage / (sb.PeriodicDuration / sb.PeriodicTickInterval)).ToString());
            //    vals.Add("SL Total Damage", sb.AverageDamage.ToString());
            //    vals.Add("#SL Casts", NumCasts[sb].ToString());
            //}
            //else
            //{
            //    vals.Add("SL Tick","");
            //    vals.Add("SL Total Damage","");
            //    vals.Add("#SL Casts","0");
            //}
            vals.Add("#Lifetaps", NumLifetaps.ToString());
            vals.Add("Mana Per LT", LifetapManaReturn.ToString());
            
            return vals;
        }
    }
}
*/