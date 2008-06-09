using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Rawr.RestoSham
  {
    public enum HealSpells
      {
        HealingWave,
        LesserHealingWave,
        ChainHeal,
        GiftOfTheNaaru,
        EarthShield
      }
    
    public enum TotemEffect
      {
        BaseHeal,
        BonusHeal,
        ReduceMana
      }
    
    
    [Serializable]
    public class TotemRelic
      {
        private HealSpells  _appliesTo;
        private string      _name;
        private TotemEffect _effect;
        private float       _amount;
        private int         _id;
        
        
        public TotemRelic()
          {
            _id = 0;
            _name = "None";
            _amount = 0;
          }
          
        public TotemRelic(int ID, string szName, HealSpells modifies, float amount, TotemEffect effect)
          {
            _id = ID;
            _appliesTo = modifies;
            _name = szName;
            _effect = effect;
            _amount = amount;
          }


        public override string ToString()
          {
            return _name;
          }

        public override bool Equals(object obj)
          {
            if (!(obj is TotemRelic))
              return false;
              
            TotemRelic totem = obj as TotemRelic;
            return (totem.ID == this.ID);
          }

        public override int GetHashCode()
          {
            return ID;
          }
        
          
        public string Description
          {
            get
              {
                string sz = "";
                
                if (this.ID == 0)
                  return string.Empty;
                
                if (this.Effect == TotemEffect.BaseHeal)
                  sz += "Increases base amount healed by ";
                else if (this.Effect == TotemEffect.BonusHeal)
                  sz += "Increases amount healed by up to ";
                else
                  sz += "Reduces mana cost by ";
                  
                sz += this.Amount.ToString();
                
                return sz;
              }
          }
          
        public int ID
          {
            get { return _id; }
            set { _id = value; }
          }
          
        public string Name
          {
            get { return _name; }
            set { _name = value; }
          }
          
        public HealSpells AppliesTo
          {
            get { return _appliesTo; }
            set {_appliesTo = value; }
          }
          
        public TotemEffect Effect
          {
            get { return _effect; }
            set { _effect = value; }
          }
          
        public float Amount
          {
            get { return _amount; }
            set { _amount = value; }
          }
          
          
        private static List<TotemRelic> _list = null;
        public static List<TotemRelic> TotemList
          {
            get
              {
                if (_list == null)
                  {
                    _list = new List<TotemRelic>(new TotemRelic[] {
                        new TotemRelic(25645, "Totem of the Plains", HealSpells.LesserHealingWave, 79, TotemEffect.BonusHeal),
                        new TotemRelic(23200, "Totem of Sustaining", HealSpells.LesserHealingWave, 53, TotemEffect.BonusHeal),
                        new TotemRelic(27544, "Totem of Spontaneous Regrowth", HealSpells.HealingWave, 88, TotemEffect.BonusHeal),
                        new TotemRelic(30023, "Totem of the Maelstrom", HealSpells.HealingWave, 24, TotemEffect.ReduceMana),
                        new TotemRelic(33505, "Totem of Living Water", HealSpells.ChainHeal, 20, TotemEffect.ReduceMana),
                        new TotemRelic(22396, "Totem of Life", HealSpells.LesserHealingWave, 80, TotemEffect.BonusHeal),
                        new TotemRelic(28523, "Totem of Healing Rains", HealSpells.ChainHeal, 88, TotemEffect.BaseHeal),
                        new TotemRelic(23005, "Totem of Flowing Water", HealSpells.LesserHealingWave, 10, TotemEffect.ReduceMana) });  
                  }
                return _list;
              }
          }
      }
  }
