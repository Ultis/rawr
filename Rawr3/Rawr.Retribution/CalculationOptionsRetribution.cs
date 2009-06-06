using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace Rawr.Retribution
{
    public class CalculationOptionsRetribution : ICalculationOptionBase
    {
        public string GetXml()
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsRetribution));
            StringBuilder xml = new StringBuilder();
            System.IO.StringWriter writer = new System.IO.StringWriter(xml);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }

        public int TargetLevel = 83;
        public MobType Mob = MobType.Humanoid;
        public SealOf Seal = SealOf.Blood;
        public float FightLength = 5f;
        public float TimeUnder20 = .18f;
        public float Delay = .05f;
        public float Wait = .05f;
        public float Targets = 1f;
        public float InFront = 0f;
        public float ConsEff = 1f;
        public bool Bloodlust = true;
        public int StackTrinketReset = 0;

        public bool SimulateRotation = true;

        private Ability[] _order = { Ability.CrusaderStrike, Ability.HammerOfWrath, Ability.Judgement,
                                                   Ability.DivineStorm, Ability.Consecration, Ability.Exorcism };
        public Ability[] Order
        {
            get { _cache = null; return _order; }
            set { _cache = null; _order = value; }
        }

        private bool[] _selected = { true, true, true, true, true, true };
        public bool[] Selected
        {
            get { _cache = null; return _selected; }
            set { _cache = null; _selected = value; }
        }

        private Ability[] _cache = null;

        [XmlIgnore]
        public Ability[] Priorities
        {
            get
            {
                if (_cache == null)
                {
                    int count = 0;
                    foreach (bool b in _selected) { if (b) count++; }
                    _cache = new Ability[count];

                    int sel = 0;
                    for (int i = 0; i < _order.Length; i++)
                    {
                        if (_selected[i])
                        {
                            _cache[sel] = _order[i];
                            sel++;
                        }
                    }
                }
                return _cache;
            }
        }

        public float JudgeCD = 7.1f;
        public float CSCD = 7.1f;
        public float DSCD = 10.5f;
        public float ConsCD = 10.5f;
        public float ExoCD = 18f;

        public float JudgeCD20 = 7.1f;
        public float CSCD20 = 7.1f;
        public float DSCD20 = 12.5f;
        public float ConsCD20 = 12.5f;
        public float ExoCD20 = 25f;
        public float HoWCD20 = 6.4f;

        public CalculationOptionsRetribution Clone()
        {
            CalculationOptionsRetribution clone = new CalculationOptionsRetribution();

            clone.TargetLevel = TargetLevel;
            clone.Mob = Mob;
            clone.FightLength = FightLength;
            clone.TimeUnder20 = TimeUnder20;
            clone.Delay = Delay;
            clone.SimulateRotation = SimulateRotation;
            clone.Targets = Targets;
            clone.InFront = InFront;

            clone.JudgeCD = JudgeCD;
            clone.CSCD = CSCD;
            clone.DSCD = DSCD;
            clone.ConsCD = ConsCD;
            clone.ExoCD = ExoCD;

            clone.JudgeCD20 = JudgeCD20;
            clone.CSCD20 = CSCD20;
            clone.DSCD20 = DSCD20;
            clone.ConsCD20 = ConsCD20;
            clone.ExoCD20 = ExoCD20;
            clone.HoWCD20 = HoWCD20;

            clone._order = (Ability[])_order.Clone();
            clone._selected = (bool[])_selected.Clone();

            return clone;
        }


    }
}