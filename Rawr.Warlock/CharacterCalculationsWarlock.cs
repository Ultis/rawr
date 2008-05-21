using System;
using System.Collections.Generic;
using System.Text;

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
        Felguard, 
        Imp, 
        Voidwalker
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

        public bool EnforceMetagemRequirements { get; set; }
        public float Latency { get; set; }
        public int TargetLevel { get; set; }
        public float FightDuration { get; set; }
        public FillerSpell FillerSpell { get; set; }
        public CastedCurse CastedCurse { get; set; }
        public Pet Pet { get; set; }
        public bool PetSacrificed { get; set; }
        public float DotGap { get; set; }
        public int AfflictionDebuffs { get; set; }
        public bool CastImmolate { get; set; }
        public bool CastCorruption { get; set; }
        public bool CastUnstableAffliction { get; set; }
        public bool CastSiphonLife { get; set; }

        //talents
        public bool TalentsSaved { get; set; }
        public int Suppression { get; set; }
        public int ImprovedCorruption { get; set; }
        public int ImprovedDrainSoul { get; set; }
        public int ImprovedLifeTap { get; set; }
        public int SoulSiphon { get; set; }
        public int ImprovedCurseOfAgony { get; set; }
        public int AmplifyCurse { get; set; }
        public int Nightfall { get; set; }
        public int EmpoweredCorruption { get; set; }
        public int ShadowMastery { get; set; }
        public int Contagion { get; set; }
        public int ImprovedImp { get; set; }
        public int DemonicEmbrace { get; set; }
        public int FelIntellect { get; set; }
        public int FelStamina { get; set; }
        public int DemonicAegis { get; set; }
        public int UnholyPower { get; set; }
        public int DemonicSacrifice { get; set; }
        public int ManaFeed { get; set; }
        public int MasterDemonologist { get; set; }
        public int SoulLink { get; set; }
        public int DemonicKnowledge { get; set; }
        public int DemonicTactics { get; set; }
        public int ImprovedShadowbolt { get; set; }
        public int Cataclysm { get; set; }
        public int Bane { get; set; }
        public int ImprovedFirebolt { get; set; }
        public int ImprovedLashOfPain { get; set; }
        public int Devastation { get; set; }
        public int ImprovedSearingPain { get; set; }
        public int DestructiveReach { get; set; }
        public int ImprovedImmolate { get; set; }
        public int Ruin { get; set; }
        public int Emberstorm { get; set; }
        public int Backlash { get; set; }
        public int SoulLeech { get; set; }
        public int ShadowAndFlame { get; set; }

        public CalculationOptionsWarlock()
        {
            EnforceMetagemRequirements = false;
            Latency = 0.05f;
            TargetLevel = 73;
            FightDuration = 360;
            FillerSpell = FillerSpell.Shadowbolt;
            CastedCurse = CastedCurse.CurseOfShadow;
            CastImmolate = false;
            CastCorruption = false;
            CastUnstableAffliction = false;
            CastSiphonLife = false;
            Pet = Pet.Succubus;
            PetSacrificed = true;
            DotGap = 1;
            AfflictionDebuffs = 12;

            TalentsSaved = false;
            Suppression = 0;
            ImprovedCorruption = 0;
            ImprovedDrainSoul = 0;
            ImprovedLifeTap = 0;
            SoulSiphon = 0;
            ImprovedCurseOfAgony = 0;
            AmplifyCurse = 0;
            Nightfall = 0;
            EmpoweredCorruption = 0;
            ShadowMastery = 0;
            Contagion = 0;
            ImprovedImp = 0;
            DemonicEmbrace = 0;
            FelIntellect = 0;
            FelStamina = 0;
            DemonicAegis = 0;
            UnholyPower = 0;
            DemonicSacrifice = 0;
            ManaFeed = 0;
            MasterDemonologist = 0;
            SoulLink = 0;
            DemonicKnowledge = 0;
            DemonicTactics = 0;
            ImprovedShadowbolt = 0;
            Cataclysm = 0;
            Bane = 0;
            ImprovedFirebolt = 0;
            ImprovedLashOfPain = 0;
            Devastation = 0;
            ImprovedSearingPain = 0;
            DestructiveReach = 0;
            ImprovedImmolate = 0;
            Ruin = 0;
            Emberstorm = 0;
            Backlash = 0;
            SoulLeech = 0;
            ShadowAndFlame = 0;
        }
    }

    class CharacterCalculationsWarlock : CharacterCalculationsBase
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

        public float GlobalCooldown { get; set; }
        public float HitPercent { get; set; }
        public float CritPercent { get; set; }
        public float HastePercent { get; set; }
        public float ShadowDamage { get; set; }
        public float FireDamage { get; set; }

        public float TotalDamage { get; set; }

        private Stats _totalStats;
        public Stats TotalStats
        {
            get { return _totalStats; }
            set { _totalStats = value; }
        }

        public Dictionary<Spell, int> NumCasts
        {
            get;
            set;
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

        public List<Spell> Spells { get; set; }
        public Character character { get; set; }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            CalculationsWarlock cw = new CalculationsWarlock();
            
            Dictionary<string, string> vals = new Dictionary<string, string>();
            vals.Add("Health", BasicStats.Health.ToString());
            vals.Add("Mana", BasicStats.Mana.ToString());
            vals.Add("Stamina", BasicStats.Stamina.ToString());
            vals.Add("Intellect", BasicStats.Intellect.ToString());
            vals.Add("Total Crit %", CritPercent.ToString());
            vals.Add("Hit %", HitPercent.ToString());
            vals.Add("Haste %", HastePercent.ToString());
            vals.Add("Shadow Damage", ShadowDamage.ToString());
            vals.Add("Fire Damage", FireDamage.ToString());
            vals.Add("Total Damage", TotalDamage.ToString());
            vals.Add("DPS", DpsRating.ToString());
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
