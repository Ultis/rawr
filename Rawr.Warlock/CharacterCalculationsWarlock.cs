using System;
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

    public enum IsbMethod
    {
        Custom,
        Raid
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
        public IsbMethod IsbMethod { get; set; }
        public float CustomIsbUptime { get; set; }
        public int NumRaidWarlocks { get; set; }
        private SUWarlock[] raidWarlocks = new SUWarlock[5];
        public SUWarlock[] RaidWarlocks
        {
            get { return raidWarlocks; }
        }
        public int NumRaidShadowPriests { get; set; }
        private SUShadowPriest[] raidShadowPriests = new SUShadowPriest[5];
        public SUShadowPriest[] RaidShadowPriests
        {
            get { return raidShadowPriests; }
        }

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
            for (int i = 0; i < 5; i++)
            {
                RaidWarlocks[i] = new SUWarlock();
                RaidShadowPriests[i] = new SUShadowPriest();
            }
        }

        public CalculationOptionsWarlock(Character character)
        {
            ImportTalents(character);

            Latency = 0.05f;
            TargetLevel = 73;
            FightDuration = 360;
            DotGap = 1;
            AfflictionDebuffs = 12;
            ShadowPriestDps = 1200;
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
            IsbMethod = IsbMethod.Raid;
            CustomIsbUptime = .6f;
            NumRaidWarlocks = 2;
            NumRaidShadowPriests = 2;

            for (int i = 0; i < 5; i++)
            {
                RaidWarlocks[i] = new SUWarlock();
                RaidShadowPriests[i] = new SUShadowPriest();
            }
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
        public float RaidDpsFromIsb { get; set; }

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
            vals.Add("Total Crit %", CritPercent.ToString("0.00"));
            vals.Add("Hit %", HitPercent.ToString("0.00"));
            vals.Add("Haste %", HastePercent.ToString("0.00"));
            vals.Add("Shadow Damage", ShadowDamage.ToString("0"));
            vals.Add("Fire Damage", FireDamage.ToString("0"));
            vals.Add("ISB Uptime", IsbUptime.ToString("0.00"));
            vals.Add("RDPS from ISB", Math.Round(RaidDpsFromIsb).ToString());
            vals.Add("Total Damage", TotalDamage.ToString());
            vals.Add("DPS", Math.Round(DpsRating).ToString());
            //vals.Add("Casting Speed", (1f / (TotalStats.SpellHasteRating / 1570f + 1f)).ToString());
            //vals.Add("Shadow Damage", (TotalStats.SpellShadowDamageRating + TotalStats.SpellDamageRating).ToString());
            //vals.Add("Fire Damage", (TotalStats.SpellFireDamageRating + TotalStats.SpellDamageRating).ToString());
            //vals.Add("DPS", DPS.ToString());
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
            //vals.Add("#Lifetaps", NumLifetaps.ToString());
            //vals.Add("Mana Per LT", LifetapManaReturn.ToString());
            
            return vals;
        }
    }
}
