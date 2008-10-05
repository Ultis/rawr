using System;
using System.Collections.Generic;

namespace Rawr.RestoSham
  {
    public class EarthShield : HealSpell
      {
        private static SpellRank[] _spellRanks = new SpellRank[] {
                null,
                new SpellRank(50, 150, 150, 1.5f, 300),
                new SpellRank(60, 205, 205, 1.5f, 372),
                new SpellRank(70, 270, 270, 1.5f, 450)};
        
        public EarthShield()
          : this(_spellRanks.Length - 1)
          { }
          
        public EarthShield(int rank)
          {
            this.Rank = rank;
            this.Name = "Earth Shield";
            this.Level = _spellRanks[rank].Level;
            this.ManaCost = _spellRanks[rank].Mana;
            this.CastTime = _spellRanks[rank].CastTime;
            this.AverageHealed = _spellRanks[rank].MaxHeal;
            this.HealType = HealSpells.EarthShield;
          }
        
        protected override SpellRank[] SpellRanks
          {
            get { return _spellRanks; }
          }

        protected override float SpellCoefficient
          {
            get { return .286f; }
          }

        public override void Calcluate(Stats stats, Character character)
          {
            // Earth Shield only gets the Purification talent bonus if cast on the Shaman. Also, it
            //  can crit but uses the crit chance of the person it is cast on.  Since in the majority
            //  of cases Earth Shield will probably be cast on a person other than the Shaman these factors
            //  will be left out of the computations.
            
            CalculationOptionsRestoSham options = character.CalculationOptions as CalculationOptionsRestoSham;

            // Base heal amount:

            float baseHeal = (SpellRanks[Rank].MaxHeal + SpellRanks[Rank].MinHeal) / 2;

            // Bonus amount (Earth Shield not subject to downrank penalties?):

            float bonus = stats.SpellPower * 1.88f;
            bonus *= SpellCoefficient;
            
            AverageHealed = (baseHeal + bonus) * 6;
            HealPerCharge = baseHeal + bonus;

            // Cast time is considered to be the global cooldown (which won't reduce below 1 sec):

            CastTime = Math.Max(SpellRanks[Rank].CastTime / (1 + (stats.SpellHasteRating / 1570)), 1.0f);
            
            // Mana not reduced by Tidal Focus:
            
            ManaCost = SpellRanks[Rank].Mana;
          }
          
        
        public float HealPerCharge
          {
            get; protected set;
          }
      }
  
  
    public class GiftOfTheNaaru : HealSpell
      {
        private static SpellRank[] _spellRanks = new SpellRank[] {
                null,
                new SpellRank(70, 1085, 1085, 1.5f, 0)};
        
        public GiftOfTheNaaru()
          : this(1)
          { }
          
        public GiftOfTheNaaru(int rank)
          {
            this.Rank = 1;
            this.Name = "Gift of the Naaru";
            this.Level = _spellRanks[rank].Level;
            this.ManaCost = _spellRanks[rank].Mana;
            this.CastTime = _spellRanks[rank].CastTime;
            this.AverageHealed = _spellRanks[rank].MaxHeal;
            this.HealType = HealSpells.GiftOfTheNaaru;
          }
        
        protected override SpellRank[] SpellRanks
          {
            get { return _spellRanks; }
          }

        protected override float SpellCoefficient
          {
            get { return 1.0f; }
          }

        public override void Calcluate(Stats stats, Character character)
          {
            // Gift of the Naaru gets no benefit from talents, but does get the full +heal bonus:
            
            float bonus = stats.SpellPower * 1.88f * SpellCoefficient;
            this.AverageHealed = SpellRanks[Rank].MaxHeal + bonus;
          }

        public override string FullName
          {
            get
              {
                return this.Name;
              }
          }
      }
    
    
    public class LesserHealingWave : HealSpell
      {
        private static SpellRank[] _spellRanks = new SpellRank[] {
                null,
                new SpellRank(20,  162,  186, 1.5f, 105),
                new SpellRank(28,  247,  281, 1.5f, 145),
                new SpellRank(36,  337,  381, 1.5f, 185),
                new SpellRank(44,  458,  514, 1.5f, 235),
                new SpellRank(52,  631,  705, 1.5f, 305),
                new SpellRank(60,  832,  928, 1.5f, 380),
                new SpellRank(66, 1039, 1185, 1.5f, 440)};
        
        public LesserHealingWave()
          : this(_spellRanks.Length - 1)
          { }
          
        public LesserHealingWave(int rank)
          {
            this.Rank = rank;
            this.Name = "Lesser Healing Wave";
            this.Level = _spellRanks[rank].Level;
            this.ManaCost = _spellRanks[rank].Mana;
            this.CastTime = _spellRanks[rank].CastTime;
            this.AverageHealed = (float)((_spellRanks[rank].MaxHeal + _spellRanks[rank].MinHeal) / 2.0f);
            this.HealType = HealSpells.LesserHealingWave;
          }
        
        protected override SpellRank[] SpellRanks
          {
            get { return _spellRanks; }
          }
      }
    
    public class HealingWave : HealSpell
      {
        private static SpellRank[] _spellRanks = new SpellRank[] {
                null,
                new SpellRank( 1,   34,   44, 1.5f,  25),
                new SpellRank( 6,   64,   78, 2.0f,  45),
                new SpellRank(12,  129,  155, 2.5f,  80),
                new SpellRank(18,  268,  316, 3.0f, 155),
                new SpellRank(24,  376,  440, 3.0f, 200),
                new SpellRank(32,  536,  622, 3.0f, 265),
                new SpellRank(40,  740,  854, 3.0f, 340),
                new SpellRank(48, 1017, 1167, 3.0f, 440),
                new SpellRank(56, 1367, 1561, 3.0f, 560),
                new SpellRank(60, 1620, 1850, 3.0f, 620),
                new SpellRank(63, 1725, 1969, 3.0f, 655),
                new SpellRank(70, 2134, 2436, 3.0f, 720)};
        
        
        public HealingWave()
          : this(_spellRanks.Length - 1)
          { }
          
        public HealingWave(int rank)
          {
            this.Rank = rank;
            this.Name = "Healing Wave";
            this.Level = _spellRanks[rank].Level;
            this.ManaCost = _spellRanks[rank].Mana;
            this.CastTime = _spellRanks[rank].CastTime;
            this.AverageHealed = (float)((_spellRanks[rank].MaxHeal + _spellRanks[rank].MinHeal) / 2.0f);
            this.HealType = HealSpells.HealingWave;
          }

        public override void Calcluate(Stats stats, Character character)
          {
            base.Calcluate(stats, character);
            
            // Healing Way bonus (assumes full stack) if spec'd for Healing Way:
            
            //int points = CalculationsRestoSham.GetTalentPoints("Healing Way", "Restoration", character.AllTalents);
            HealingWay = (character.ShamanTalents.HealingWay > 0 ? AverageHealed * .18f : 0f);
            
            // Adjust cast time based on Improved Healing Wave talent:
            
            //points = CalculationsRestoSham.GetTalentPoints("Improved Healing Wave", "Restoration", character.AllTalents);
            float baseTime = SpellRanks[Rank].CastTime - (.1f * character.ShamanTalents.ImprovedHealingWave);
            CastTime = baseTime / (1 + (stats.SpellHasteRating / 1570));
          }

        protected override SpellRank[] SpellRanks
          {
            get { return _spellRanks; }
          }
          
        public float HealingWay
          {
            get; protected set;
          }
      }
    
  
    public class ChainHeal : HealSpell
      {
        private static SpellRank[] _spellRanks = new SpellRank[] {
                null,
                new SpellRank(40, 320, 368, 2.5f, 260),
                new SpellRank(46, 405, 465, 2.5f, 315),
                new SpellRank(54, 551, 629, 2.5f, 405),
                new SpellRank(61, 605, 691, 2.5f, 435),
                new SpellRank(68, 826, 942, 2.5f, 540)}; 
        
        public ChainHeal()
          : this(_spellRanks.Length - 1)
          { }
          
        public ChainHeal(int rank)
          {
            this.Rank = rank;
            this.Name = "Chain Heal";
            this.Level = _spellRanks[rank].Level;
            this.ManaCost = _spellRanks[rank].Mana;
            this.CastTime = _spellRanks[rank].CastTime;
            this.AverageHealed = (float)((_spellRanks[rank].MaxHeal + _spellRanks[rank].MinHeal) / 2.0f);
            this.AverageHealed += this.AverageHealed / 2 + this.AverageHealed / 4;
            this.HealType = HealSpells.ChainHeal;
          }
        
        public override void Calcluate(Stats stats, Character character)
          {
            base.Calcluate(stats, character);

            // Improved Chain Heal talent:
            
            float impCH = 1.0f;
            //int points = CalculationsRestoSham.GetTalentPoints("Improved Chain Heal", "Restoration", character.AllTalents);
            impCH = 1f + .1f * character.ShamanTalents.ImprovedChainHeal;
            
            // Skyshatter 4-piece bonus:
            
            impCH += stats.CHHealIncrease;

            // Now compute the chain heal numbers:

            _targetHeals[0] = this.AverageHealed * impCH;
            _targetHeals[1] = _targetHeals[0] / 2;
            _targetHeals[2] = _targetHeals[1] / 2;

            CalculationOptionsRestoSham options = character.CalculationOptions as CalculationOptionsRestoSham;
            this.AverageHealed = _targetHeals[0];
            if (options.NumCHTargets > 1)
              this.AverageHealed += _targetHeals[1];
            if (options.NumCHTargets == 3)
              this.AverageHealed += _targetHeals[2];
          }
          
          
        private float[] _targetHeals = new float[3];
        public float[] HealsOnTargets
          {
            get { return _targetHeals; }
          }
          
          
        public float TotalHealed
          {
            get { return _targetHeals[0] + _targetHeals[1] + _targetHeals[2]; }
          }

        protected override SpellRank[] SpellRanks
          {
            get { return _spellRanks; }
          }
      }
    
  
    public abstract class HealSpell
      {   
        /// <summary>
        /// Get / set the rank of this spell.
        /// </summary>
        public int Rank
          { get; protected set; }
          
        
        /// <summary>
        /// Get / set the name of this spell.
        /// </summary>
        public string Name
          { get; protected set; }
          
          
        /// <summary>
        /// Get the full name of the spell (name + rank).
        /// </summary>
        public virtual string FullName
          {
            get { return Name + " (Rank " + Rank.ToString() + ")"; }
          }
          
          
        /// <summary>
        /// The level at which this spell is learned.
        /// </summary>
        public int Level
          { get; protected set; }
          
          
        /// <summary>
        /// Get the which healing spell this is.
        /// </summary>
        public HealSpells HealType
          {
            get; protected set;
          }
          
          
        /// <summary>
        /// Get information about the ranks available to this spell.
        /// </summary>
        protected abstract SpellRank[] SpellRanks
          {
            get;
          }
          
          
        /// <summary>
        /// Get the spell coefficient for this spell (there can be exceptions to the general rule
        ///  so we allow inheritors of this class to override this).
        /// </summary>
        protected virtual float SpellCoefficient
          {
            get { return this.SpellRanks[this.Rank].CastTime / 3.5f; }
          }
          
          
        /// <summary>
        /// Given some character statistics, and some fight criteria, compute various performance aspects
        ///  of this spell.
        /// </summary>
        public virtual void Calcluate(Stats stats, Character character)
          {
            CalculationOptionsRestoSham options = character.CalculationOptions as CalculationOptionsRestoSham;

            // Base heal amount:

            float baseHeal = (SpellRanks[Rank].MaxHeal + SpellRanks[Rank].MinHeal) / 2;
            if (options.Totems[HealType].ID != 0 && options.Totems[HealType].Effect == TotemEffect.BaseHeal)
              baseHeal += options.Totems[HealType].Amount;
            
            // Bonus amount:

            float bonus = stats.SpellPower * 1.88f;
            if (options.Totems[HealType].ID != 0 && options.Totems[HealType].Effect == TotemEffect.BonusHeal)
              bonus += options.Totems[HealType].Amount;
            bonus *= SpellCoefficient;
            if (Rank < 70)
              bonus *= DownrankCoefficient;

            // Crit rate:

            float crit = .022f + ((stats.Intellect / 80f) / 100) + ((stats.CritRating / 22.08f) / 100) +
                                  stats.SpellCrit;
            float critRate = 1 + 0.5f * crit;

            // Purification talent:

            float purificationBonus = 1.0f;
            //int points = CalculationsRestoSham.GetTalentPoints("Purification", "Restoration", character.AllTalents);
            purificationBonus = 1f + .02f * character.ShamanTalents.Purification;

            // Now get total average heal:

            AverageHealed = (baseHeal + bonus) * critRate * purificationBonus;
            
            // Compute spell cast time:
            
            CastTime = SpellRanks[Rank].CastTime / (1 + (stats.SpellHasteRating / 1570));
            
            // Compute mana cost:
            
            //points = CalculationsRestoSham.GetTalentPoints("Tidal Focus", "Restoration", character.AllTalents);
            float f = 0.0f;
            if (options.Totems[HealType].ID != 0 && options.Totems[HealType].Effect == TotemEffect.ReduceMana)
              f = options.Totems[HealType].Amount;
              
            if (HealType == HealSpells.ChainHeal)
              f += stats.CHManaReduction * SpellRanks[Rank].Mana;
            if (HealType == HealSpells.LesserHealingWave)
              f += stats.LHWManaReduction * SpellRanks[Rank].Mana;
              
            ManaCost = (SpellRanks[Rank].Mana - f) * (1 - .01f * character.ShamanTalents.TidalFocus);
          }
        
        
        /// <summary>
        /// The average amount healed by this spell.
        /// </summary>
        public float AverageHealed
          { get; protected set; }
          
          
        /// <summary>
        /// The mana cost of this spell.
        /// </summary>
        public float ManaCost
          { get; protected set; }
          
          
        /// <summary>
        /// The cast time of this spell, in seconds.
        /// </summary>
        public float CastTime
          { get; protected set; }
          
          
        /// <summary>
        /// Get the downrank penalty coefficient for this spell (assumes being cast by a level 70 player).
        /// </summary>
        protected float DownrankCoefficient
          {
            get
              {
                float coef = Math.Min(1f, (this.Level + 11f) / 70f);
                if (this.Level <= 20)
                  coef *= 1f - ((20f - this.Level) * 0.0375f);
                return coef;
              }
          }
          
          
        /// <summary>
        /// Used by calculation routines when determining relative spell weights.
        /// </summary>
        public float Weight
          { get; set; }
      }
      
      
    public class SpellRank
      {
        public SpellRank(int lvl, int min, int  max, float time, int mana)
          {
            Level = lvl;
            MinHeal = min;
            MaxHeal = max;
            CastTime = time;
            Mana = mana;
          }
        
        public int Level;
        public int MinHeal;
        public int MaxHeal;
        public float CastTime;
        public int Mana;
      }
  }
