using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Rawr.HolyPriest
{
    public class BaseSpell
    {
        public float MinHeal { get; protected set; }
        public float MaxHeal { get; protected set; }
        public float RankCoef { get; protected set; }
        public int ManaCostBase { get; protected set; }
        public float CastTime { get; protected set; }
        public int Rank { get; protected set; }
        public float HotDuration { get; protected set; }

        public static Dictionary<int, int> BaseMana = new Dictionary<int, int>();
        static BaseSpell()
        {
            BaseMana[70] = 2620;
            BaseMana[71] = 0;
            BaseMana[72] = 0;
            BaseMana[73] = 0;
            BaseMana[74] = 0;
            BaseMana[75] = 0;
            BaseMana[76] = 0;
            BaseMana[77] = 0;
            BaseMana[78] = 0;
            BaseMana[79] = 0;
            BaseMana[80] = 0;
        }

        public BaseSpell(int rank, float minHeal, float maxHeal, int manaCostBase, float castTime, float rankCoef, float hotDuration)
        {
            MinHeal = minHeal;
            MaxHeal = maxHeal;
            RankCoef = rankCoef;
            ManaCostBase = manaCostBase;
            CastTime = castTime;
            HotDuration = hotDuration;
            Rank = rank;
        }

        public BaseSpell(int rank, float minHeal, float maxHeal, int manaCostBase, float castTime, float rankCoef):
            this(rank, minHeal, maxHeal, manaCostBase, castTime, rankCoef, 0)
        {}

        public BaseSpell(BaseSpell baseSpell):
            this(baseSpell.Rank, baseSpell.MinHeal, baseSpell.MaxHeal, baseSpell.ManaCostBase, baseSpell.CastTime, baseSpell.RankCoef, baseSpell.HotDuration) { }
    }

    public class Spell: BaseSpell
    {
        public int ManaCost { get; protected set; }

        public string Name { get; protected set; }

        public float HealingCoef { get; protected set; }
        public int Range { get; protected set; }
        public int Targets { get; protected set; }

        public float GlobalCooldown { get; protected set; }
        public Color GraphColor { get; protected set; }

        #region Properties

        public bool IsHot
        {
            get { return HotDuration > 0; }
        }

        public float AvgHeal
        {
            get
            {
                return (MinHeal + MaxHeal) / 2;
            }
        }

        public virtual float HpS
        {
            get
            {
                if (IsHot)
                    return AvgHeal/HotDuration;

                return AvgHeal / CastTime;
            }
        }

        public virtual float HpM
        {
            get
            {
                return AvgHeal / ManaCost;
            }
        }

        public float AvgCrit
        {
            get
            {
                return (MinHeal * 1.5f + MaxCrit)/2;
            }
        }

        public float MaxCrit
        {
            get
            {
                return MaxHeal * 1.5f;
            }
        }

        #endregion

        protected Spell(Stats stats, BaseSpell baseSpell, string name, float coef, float hotDuration, Color col)
            : base(baseSpell)
        {
            Name = name;
            CastTime = baseSpell.CastTime;
            HealingCoef = coef;
            HotDuration = hotDuration;
            GlobalCooldown = 1.5f * (1 - stats.SpellHasteRating / 15.7f / 100f);
            GraphColor = col;
        }

        protected Spell(Stats stats, BaseSpell baseSpell, string name, float coef, Color col)
            : this(stats, baseSpell, name, coef, 0, col)
        {}

        public static float GetGlobalCooldown(Stats stats)
        {
            return 1.5f * (1 - stats.SpellHasteRating / 15.7f / 100f);
        }

        public override string ToString()
        {
            return String.Format("{0} *HpS: {1}\r\nHpM: {2}\r\nMin Heal: {3}\r\nMax Heal: {4}\r\nAvg Crit: {5}\r\nMax Crit: {6}\r\nCast: {7}\r\nCost: {8}",
                AvgHeal.ToString("0"),
                HpS.ToString("0.00"),
                HpM.ToString("0.00"),
                MinHeal.ToString("0"),
                MaxHeal.ToString("0"),
                AvgCrit.ToString("0"),
                MaxCrit.ToString("0"),
                CastTime.ToString("0.00"),
                ManaCost.ToString("0"));
        }
    }

    public class Renew : Spell
    {
        private static readonly List<BaseSpell> baseSpellTable = new List<BaseSpell> (){   
        /*                Rank MinHeal MaxHeal ManaCost CastTime RankCoef HotDuration    */
            new BaseSpell(1,      45,     45,     17,     0,      0.851f, 15f),
            new BaseSpell(2,      100,    100,    17,     0,      0.723f, 15f),
            new BaseSpell(3,      175,    175,    17,     0,      0.557f, 15f),
            new BaseSpell(4,      245,    245,    17,     0,      0.471f, 15f),
            new BaseSpell(5,      315,    315,    17,     0,      0.386f, 15f),
            new BaseSpell(6,      400,    400,    17,     0,      0.300f, 15f),
            new BaseSpell(7,      510,    510,    17,     0,      0.214f, 15f),
            new BaseSpell(8,      650,    650,    17,     0,      0.129f, 15f),
            new BaseSpell(9,      810,    810,    17,     0,      0.043f, 15f),
            new BaseSpell(10,     970,    970,    17,     0,      0,      15f),
            new BaseSpell(11,     1010,   1010,   17,     0,      0,      15f),
            new BaseSpell(12,     1110,   1110,   17,     0,      0,      15f)
            };
        
        public Renew(Stats stats, Character character)
            : this(stats, character, baseSpellTable.Count){}

		public Renew(Stats stats, Character character, int rank)
            : base(stats, baseSpellTable[rank - 1], "Renew", 15f / 15f, 15f, Color.Green)
        {
            Calculate(stats, character, rank);
        }

		public static List<Spell> GetAllRanks(Stats stats, Character character)
        {
            List<Spell> list = new List<Spell>(baseSpellTable.Count);
            for (int i = 1; i <= baseSpellTable.Count; i++)
                list.Add(new Renew(stats, character, i));

            return list;
        }

		public static List<Spell> GetAllCommonRanks(Stats stats, Character character)
        {
            return new List<Spell> { new Renew(stats, character)};
        }


		protected void Calculate(Stats stats, Character character, int rank)
        {
            Rank = rank;
            MinHeal = MaxHeal = (baseSpellTable[Rank - 1].MinHeal +
                stats.SpellPower * 1.88f * HealingCoef * (1 - baseSpellTable[Rank - 1].RankCoef))
                * (1 + character.PriestTalents.ImprovedRenew * 0.05f) 
                * (1 + character.PriestTalents.SpiritualHealing * 0.02f);

            ManaCost = (int)Math.Round(baseSpellTable[Rank - 1].ManaCostBase / 100f * BaseMana[character.Level]
                * (1 - character.PriestTalents.MentalAgility * 0.02f));
        }

        public override string ToString()
        {
            return String.Format("{0} *HpS: {1}\r\nHpM: {2}\r\nTick: {3}\r\nCost: {4}",
                          MinHeal.ToString("0"),
                          HpS.ToString("0.00"),
                          HpM.ToString("0.00"),
                          (MinHeal/HotDuration*3).ToString("0"),
                          ManaCost.ToString("0"));
        }
    }

    public class FlashHeal : Spell
    {
        private static readonly List<BaseSpell> baseSpellTable = new List<BaseSpell>(){   
        /*                Rank MinHeal MaxHeal ManaCost CastTime RankCoef    */
            new BaseSpell(1,    139,    237,    18,    1.5f,   0.557f),
            new BaseSpell(2,    253,    314,    18,    1.5f,   0.471f),
            new BaseSpell(3,    327,    393,    18,    1.5f,   0.386f),
            new BaseSpell(4,    400,    478,    18,    1.5f,   0.300f),
            new BaseSpell(5,    518,    616,    18,    1.5f,   0.214f),
            new BaseSpell(6,    644,    764,    18,    1.5f,   0.129f),
            new BaseSpell(7,    812,    958,    18,    1.5f,   0.057f),
            new BaseSpell(8,    913,    1059,   18,    1.5f,   0),
            new BaseSpell(9,    1101,   1279,   18,    1.5f,   0)
            };

		public FlashHeal(Stats stats, Character character, int rank)
            : base(stats, baseSpellTable[rank - 1], "Flash Heal", 0.4286f, Color.YellowGreen)
        {
            Calculate(stats, character, rank);
        }

		public FlashHeal(Stats stats, Character character)
            : this(stats, character, baseSpellTable.Count){}

		public static List<Spell> GetAllRanks(Stats stats, Character character)
        {
            List<Spell> list = new List<Spell>(baseSpellTable.Count);
            for (int i = 1; i <= baseSpellTable.Count; i++)
                list.Add(new FlashHeal(stats, character, i));

            return list;
        }

		public static List<Spell> GetAllCommonRanks(Stats stats, Character character)
        {
            return new List<Spell> { new FlashHeal(stats, character) };
        }

		protected void Calculate(Stats stats, Character character, int rank)
        {
            Rank = rank;
            MinHeal = (baseSpellTable[Rank - 1].MinHeal +
                stats.SpellPower * 1.88f * (1 - baseSpellTable[Rank - 1].RankCoef) 
                * (character.PriestTalents.EmpoweredHealing * 0.02f + HealingCoef)) 
                * (1 + character.PriestTalents.SpiritualHealing * 0.02f);

            MaxHeal = (baseSpellTable[Rank - 1].MaxHeal +
                stats.SpellPower * 1.88f * (1 - baseSpellTable[Rank - 1].RankCoef) 
                * (character.PriestTalents.EmpoweredHealing * 0.02f + HealingCoef)) 
                * (1 + character.PriestTalents.SpiritualHealing * 0.02f);

            CastTime = baseSpellTable[Rank - 1].CastTime * (1 - stats.SpellHasteRating / 15.7f / 100f);

            ManaCost = (int)Math.Round(ManaCostBase / 100f * BaseMana[character.Level]);
        }
    }

    public class Heal : Spell
    {
        private static readonly List<BaseSpell> baseSpellTable = new List<BaseSpell>(){   
        /*                Rank MinHeal MaxHeal ManaCost CastTime RankCoef    */
            new BaseSpell(1,    307,    353,    32,    3f,   0.672f),
            new BaseSpell(2,    445,    507,    32,    3f,   0.529f),
            new BaseSpell(3,    586,    662,    32,    3f,   0.443f),
            new BaseSpell(4,    734,    827,    32,    3f,   0.357f)
            };

		public Heal(Stats stats, Character character, int rank)
            : base(stats, baseSpellTable[rank - 1], "Heal", 0.8571f, Color.DarkGreen)
        {
            Calculate(stats, character, rank);
        }

		public Heal(Stats stats, Character character)
            : this(stats, character, baseSpellTable.Count)
        {}

		public static List<Spell> GetAllRanks(Stats stats, Character character)
        {
            List<Spell> list = new List<Spell>(baseSpellTable.Count);
            for (int i = 1; i <= baseSpellTable.Count; i++)
                list.Add(new Heal(stats, character, i));

            return list;
        }

		public static List<Spell> GetAllCommonRanks(Stats stats, Character character)
        {
            return new List<Spell> { new Heal(stats, character) };
        }

		protected void Calculate(Stats stats, Character character, int rank)
        {
            Rank = rank;
            MinHeal = (baseSpellTable[Rank - 1].MinHeal + 
                stats.SpellPower * 1.88f * HealingCoef * (1 - baseSpellTable[Rank - 1].RankCoef)) 
                * (1 + character.PriestTalents.SpiritualHealing * 0.02f);
            MaxHeal = (baseSpellTable[Rank - 1].MaxHeal +
                stats.SpellPower * 1.88f * HealingCoef * (1 - baseSpellTable[Rank - 1].RankCoef)) 
                * (1 + character.PriestTalents.SpiritualHealing * 0.02f);

            ManaCost = (int)Math.Round(baseSpellTable[Rank - 1].ManaCostBase / 100f * BaseMana[character.Level]
                * (1 - character.PriestTalents.ImprovedHealing * 0.05f));

            CastTime = (baseSpellTable[Rank - 1].CastTime - character.PriestTalents.DivineFury * 0.1f)
                * (1 - stats.SpellHasteRating / 15.7f / 100f);
        }
    }

    public class GreaterHeal : Spell
    {
        private static readonly List<BaseSpell> baseSpellTable = new List<BaseSpell>(){   
        /*                Rank MinHeal MaxHeal ManaCost CastTime RankCoef    */
            new BaseSpell(1,    924,    1039,    32,    3f,   0.271f),
            new BaseSpell(2,    1178,   1318,    32,    3f,   0.186f),
            new BaseSpell(3,    1470,   1642,    32,    3f,   0.100f),
            new BaseSpell(4,    1835,   2044,    32,    3f,   0.071f),
            new BaseSpell(5,    2006,   2235,    32,    3f,   0.029f),
            new BaseSpell(6,    2107,   2444,    32,    3f,   0),
            new BaseSpell(7,    2414,   2803,    32,    3f,   0)
            };

		public GreaterHeal(Stats stats, Character character, int rank)
            : base(stats, baseSpellTable[rank - 1], "Greater Heal", 0.8571f, Color.DarkSeaGreen)
        {
            Calculate(stats, character, rank);
        }

        public GreaterHeal(Stats stats, Character character)
            : this(stats, character, baseSpellTable.Count)
        {}

        public static List<Spell> GetAllRanks(Stats stats, Character character)
        {
            List<Spell> list = new List<Spell>(baseSpellTable.Count);
            for (int i = 1; i <= baseSpellTable.Count; i++)
                list.Add(new GreaterHeal(stats, character, i));

            return list;
        }

        public static List<Spell> GetAllCommonRanks(Stats stats, Character character)
        {
            return new List<Spell> { new GreaterHeal(stats, character, 7), new GreaterHeal(stats, character, 2), new GreaterHeal(stats, character, 1) };
        }

        protected void Calculate(Stats stats, Character character, int rank)
        {
            Rank = rank;
            MinHeal = (baseSpellTable[Rank - 1].MinHeal + 
                stats.SpellPower * 1.88f * (1 - baseSpellTable[Rank - 1].RankCoef) 
                * (character.PriestTalents.EmpoweredHealing * 0.04f + HealingCoef)) 
                * (1 + character.PriestTalents.SpiritualHealing * 0.02f) 
                * (1 + stats.BonusGHHealingMultiplier);

            MaxHeal = (baseSpellTable[Rank - 1].MaxHeal +
                stats.SpellPower * 1.88f * (1 - baseSpellTable[Rank - 1].RankCoef) 
                * (character.PriestTalents.EmpoweredHealing * 0.04f + HealingCoef)) 
                * (1 + character.PriestTalents.SpiritualHealing * 0.02f) 
                * (1 + stats.BonusGHHealingMultiplier);

            ManaCost = (int)Math.Round(baseSpellTable[Rank - 1].ManaCostBase / 100f * BaseMana[character.Level]
                * (1 - character.PriestTalents.ImprovedHealing * 0.05f));

            CastTime = (baseSpellTable[Rank - 1].CastTime - character.PriestTalents.DivineFury * 0.1f) 
                * (1 - stats.SpellHasteRating / 15.7f / 100f);
        }
    }

    public class PrayerOfHealing : Spell
    {
        private static readonly List<BaseSpell> baseSpellTable = new List<BaseSpell>(){   
        /*                Rank MinHeal MaxHeal ManaCost CastTime RankCoef    */
            new BaseSpell(1,    312,    333,    48,    3f,   0.357f),
            new BaseSpell(2,    458,    487,    48,    3f,   0.214f),
            new BaseSpell(3,    675,    713,    48,    3f,   0.071f),
            new BaseSpell(4,    960,    1013,   48,    3f,   0.071f),
            new BaseSpell(5,    1019,   1076,   48,    3f,   0),
            new BaseSpell(6,    1251,   1322,   48,    3f,   0)
            };

        private static readonly Color[] targetColors = new Color[]
                                           {
                                               Color.Orange,
                                               Color.Orange,
                                               Color.Orange,
                                               Color.OrangeRed,
                                               Color.DarkOrange
                                           };

        public override float HpS
        {
            get
            {
                return AvgHeal * Targets / CastTime;
            }
        }

        public override float HpM
        {
            get
            {
                return AvgHeal * Targets / ManaCost;
            }
        }

        public PrayerOfHealing(Stats stats, Character character, int rank, int targets)
            : base(stats, baseSpellTable[rank - 1], "Prayer of Healing (" + targets + " targets)", 0.2857f, targetColors[targets - 1])
        {
            Targets = targets;
            Calculate(stats, character, rank);
        }

        public PrayerOfHealing(Stats stats, Character character)
            : this(stats, character, baseSpellTable.Count, 3)
        {}

        public static List<Spell> GetAllRanks(Stats stats, Character character, int targets)
        {
            List<Spell> list = new List<Spell>(baseSpellTable.Count);
            for (int i = 1; i <= baseSpellTable.Count; i++)
                list.Add(new PrayerOfHealing(stats, character, i, targets));

            return list;
        }

        public static List<Spell> GetAllCommonRanks(Stats stats, Character character, int targets)
        {
            return new List<Spell> { new PrayerOfHealing(stats, character, baseSpellTable.Count, targets) };
        }

        protected void Calculate(Stats stats, Character character, int rank)
        {
            Rank = rank;
            Range = 30;

            MinHeal = (baseSpellTable[rank - 1].MinHeal +
                stats.SpellPower * 1.88f * HealingCoef * (1 - baseSpellTable[rank - 1].RankCoef)) 
                * (1 + character.PriestTalents.SpiritualHealing * 0.02f);
            MaxHeal = (baseSpellTable[rank - 1].MaxHeal +
                stats.SpellPower * 1.88f * HealingCoef * (1 - baseSpellTable[rank - 1].RankCoef)) 
                * (1 + character.PriestTalents.SpiritualHealing * 0.02f);

            ManaCost = (int)Math.Round(baseSpellTable[rank - 1].ManaCostBase / 100f * BaseMana[character.Level]
                * (1 - character.PriestTalents.HealingPrayers * 0.1f) 
                * (1 - stats.BonusPoHManaCostReductionMultiplier));

            Range = (int)Math.Round(Range * (1 + character.PriestTalents.HolyReach * 0.1f));
        }

        public override string ToString()
        {
            return String.Format("{0} *HpS(3): {1}\r\nHpS(4): {10}\r\nHpS(5): {12}\r\nHpM(3): {2}\r\nHpM(4): {11}\r\nHpM(5): {13}\r\nMin Heal: {3}\r\nMax Heal: {4}\r\nAvg Crit: {5}\r\nMax Crit: {6}\r\nCast: {7}\r\nCost: {8}\r\nRange: {9}",
                AvgHeal.ToString("0"),
                (AvgHeal * 3 / CastTime).ToString("0.00"),
                (AvgHeal * 3 / ManaCost).ToString("0.00"),
                MinHeal.ToString("0"),
                MaxHeal.ToString("0"),
                AvgCrit.ToString("0"),
                MaxCrit.ToString("0"),
                CastTime.ToString("0.00"),
                ManaCost.ToString("0"),
                Range,
                (AvgHeal * 4 / CastTime).ToString("0.00"),
                (AvgHeal * 4 / ManaCost).ToString("0.00"),
                (AvgHeal * 5 / CastTime).ToString("0.00"),
                (AvgHeal * 5 / ManaCost).ToString("0.00"));
        }
    }

    public class CircleOfHealing : Spell
    {
        private static readonly List<BaseSpell> baseSpellTable = new List<BaseSpell>(){   
        /*                Rank MinHeal MaxHeal ManaCost CastTime RankCoef    */
            new BaseSpell(1,    250,    274,    21,    0,   0.129f),
            new BaseSpell(2,    292,    323,    21,    0,   0.071f),
            new BaseSpell(3,    332,    367,    21,    0,   0),
            new BaseSpell(4,    376,    415,    21,    0,   0),
            new BaseSpell(5,    409,    451,    21,    0,   0)
            };

        private static readonly Color[] targetColors = new Color[]
                                           {
                                               Color.Yellow,
                                               Color.Yellow,
                                               Color.Yellow,
                                               Color.Gold,
                                               Color.Goldenrod
                                           };

        public override float HpS
        {
            get
            {
                return (AvgHeal / GlobalCooldown) * Targets;
            }
        }

        public override float HpM
        {
            get
            {
                return AvgHeal * Targets / ManaCost;
            }
        }

        public CircleOfHealing(Stats stats, Character character, int rank, int targets)
            : base(stats, baseSpellTable[rank - 1], "Circle of Healing (" + targets + " targets)", 0.2143f, targetColors[targets - 1])
        {
            Targets = targets;
            Calculate(stats, character, rank);
        }

        public CircleOfHealing(Stats stats, Character character)
            : this(stats, character, baseSpellTable.Count, 3)
        {}

        public static List<Spell> GetAllRanks(Stats stats, Character character, int targets)
        {
            List<Spell> list = new List<Spell>(baseSpellTable.Count);
            for (int i = 1; i <= baseSpellTable.Count; i++)
                list.Add(new CircleOfHealing(stats, character, i, targets));

            return list;
        }

        public static List<Spell> GetAllCommonRanks(Stats stats, Character character, int targets)
        {
            return new List<Spell> { new CircleOfHealing(stats, character, baseSpellTable.Count, targets) };
        }

        protected void Calculate(Stats stats, Character character, int rank)
        {
            if (character.PriestTalents.CircleOfHealing == 0)
            {
                MinHeal = MaxHeal = 0;
                return;
            }

            Rank = rank;
            Range = 15;
            MinHeal = (baseSpellTable[Rank - 1].MinHeal +
                stats.SpellPower * 1.88f * HealingCoef * (1 - baseSpellTable[Rank - 1].RankCoef)) 
                * (1 + character.PriestTalents.SpiritualHealing * 0.02f);
            MaxHeal = (baseSpellTable[Rank - 1].MaxHeal +
                stats.SpellPower * 1.88f * HealingCoef * (1 - baseSpellTable[Rank - 1].RankCoef)) 
                * (1 + character.PriestTalents.SpiritualHealing * 0.02f);

            ManaCost = (int)Math.Round(baseSpellTable[Rank - 1].ManaCostBase / 100f * BaseMana[character.Level]
                * (1 - character.PriestTalents.MentalAgility * 0.02f));
            Range = (int)Math.Round(Range * (1 + character.PriestTalents.HolyReach * 0.1f));
        }

        public override string ToString()
        {
            return String.Format("{0} *HpS(3): {1}\r\nHpS(4): {9}\r\nHpS(5): {11}\r\nHpM(3): {2}\r\nHpM(4): {10}\r\nHpM(5): {12}\r\nMin Heal: {3}\r\nMax Heal: {4}\r\nAvg Crit: {5}\r\nMax Crit: {6}\r\nCost: {7}\r\nRange: {8}",
                AvgHeal.ToString("0"),
                (AvgHeal * 3 / GlobalCooldown).ToString("0.00"),
                (AvgHeal * 3 / ManaCost).ToString("0.00"),
                MinHeal.ToString("0"),
                MaxHeal.ToString("0"),
                AvgCrit.ToString("0"),
                MaxCrit.ToString("0"),
                ManaCost.ToString("0"),
                Range,
                (AvgHeal * 4 / GlobalCooldown).ToString("0.00"),
                (AvgHeal * 4 / ManaCost).ToString("0.00"),
                (AvgHeal * 5 / GlobalCooldown).ToString("0.00"),
                (AvgHeal * 5 / ManaCost).ToString("0.00"));
        }
    }

    public class HolyNova : Spell
    {
        private static readonly List<BaseSpell> baseSpellTable = new List<BaseSpell>(){   
        /*                Rank MinHeal MaxHeal ManaCost CastTime RankCoef    */
            new BaseSpell(1,    52,    60,    25,    0,   0.529f),
            new BaseSpell(2,    86,    98,    25,    0,   0.414f),
            new BaseSpell(3,    121,   139,   25,    0,   0.300f),
            new BaseSpell(4,    161,   188,   25,    0,   0.186f),
            new BaseSpell(5,    235,   272,   25,    0,   0.071f),
            new BaseSpell(6,    302,   450,   25,    0,   0),
            new BaseSpell(7,    384,   446,   25,    0,   0)
            };

        private static readonly Color[] targetColors = new Color[]
                                           {
                                               Color.Coral,
                                               Color.Coral,
                                               Color.Coral,
                                               Color.BurlyWood,
                                               Color.Brown
                                           };

        public override float HpS
        {
            get
            {
                return (AvgHeal / GlobalCooldown) * Targets;
            }
        }

        public override float HpM
        {
            get
            {
                return (AvgHeal / ManaCost) * Targets;
            }
        }

        public HolyNova(Stats stats, Character character, int rank, int targets)
            : base(stats, baseSpellTable[rank - 1], "Holy Nova (" + targets + " targets)", 0.16f, targetColors[targets - 1])
        {
            Targets = 3;
            Calculate(stats, character, rank);
        }

        public HolyNova(Stats stats, Character character)
            : this(stats, character, baseSpellTable.Count, 3)
        {}

        public static List<Spell> GetAllRanks(Stats stats, Character character, int targets)
        {
            List<Spell> list = new List<Spell>(baseSpellTable.Count);
            for (int i = 1; i <= baseSpellTable.Count; i++)
                list.Add(new HolyNova(stats, character, i, targets));

            return list;
        }

        public static List<Spell> GetAllCommonRanks(Stats stats, Character character, int targets)
        {
            return new List<Spell> { new HolyNova(stats, character, baseSpellTable.Count, targets) };
        }

        protected void Calculate(Stats stats, Character character, int rank)
        {
            //if (talents.HolyNova == 0)
			//{
			//    MinHeal = MaxHeal = 0;
			//    return;
			//}

            Rank = rank;
            Range = 10;
            
            MinHeal = (baseSpellTable[Rank - 1].MinHeal +
                stats.SpellPower * 1.88f * HealingCoef * (1 - baseSpellTable[Rank - 1].RankCoef)) 
                * (1 + character.PriestTalents.SpiritualHealing * 0.02f);
            MaxHeal = (baseSpellTable[Rank - 1].MaxHeal +
                stats.SpellPower * 1.88f * HealingCoef * (1 - baseSpellTable[Rank - 1].RankCoef)) 
                * (1 + character.PriestTalents.SpiritualHealing * 0.02f);

            ManaCost = (int)Math.Round(baseSpellTable[Rank - 1].ManaCostBase / 100f * BaseMana[character.Level]
                * (1 - character.PriestTalents.MentalAgility * 0.02f));
            Range = (int)Math.Round(Range * (1 + character.PriestTalents.HolyReach * 0.1f));
        }

        public override string ToString()
        {
            return String.Format("{0} *HpS(3): {1}\r\nHpS(4): {9}\r\nHpS(5): {11}\r\nHpM(3): {2}\r\nHpM(4): {10}\r\nHpM(5): {12}\r\nMin Heal: {3}\r\nMax Heal: {4}\r\nAvg Crit: {5}\r\nMax Crit: {6}\r\nCost: {7}\r\nRange: {8}",
                AvgHeal.ToString("0"),
                (AvgHeal * 3 / GlobalCooldown).ToString("0.00"),
                (AvgHeal * 3 / ManaCost).ToString("0.00"),
                MinHeal.ToString("0"),
                MaxHeal.ToString("0"),
                AvgCrit.ToString("0"),
                MaxCrit.ToString("0"),
                ManaCost.ToString("0"),
                Range,
                (AvgHeal * 4 / GlobalCooldown).ToString("0.00"),
                (AvgHeal * 4 / ManaCost).ToString("0.00"),
                (AvgHeal * 5 / GlobalCooldown).ToString("0.00"),
                (AvgHeal * 5 / ManaCost).ToString("0.00"));
        }
    }

    public class BindingHeal : Spell
    {
        private static readonly List<BaseSpell> baseSpellTable = new List<BaseSpell>(){   
        /*                Rank MinHeal MaxHeal ManaCost CastTime RankCoef    */
            new BaseSpell(1,    1042,   1338,   27,    1.5f,   0)
            };

        public override float HpS
        {
            get
            {
                return AvgHeal * 2/CastTime;
            }
        }

        public override float HpM
        {
            get
            {
                return (AvgHeal / ManaCost) * 2;
            }
        }

        public BindingHeal(Stats stats, Character character, int rank)
            : base(stats, baseSpellTable[rank - 1], "Binding Heal", 0.4286f, Color.Purple)
        {
            Calculate(stats, character, rank);
        }

        public BindingHeal(Stats stats, Character character)
            : this(stats, character, baseSpellTable.Count)
        {}

        public static List<Spell> GetAllRanks(Stats stats, Character character)
        {
            List<Spell> list = new List<Spell>(baseSpellTable.Count);
            for (int i = 1; i <= baseSpellTable.Count; i++)
                list.Add(new BindingHeal(stats, character, i));

            return list;
        }

        public static List<Spell> GetAllCommonRanks(Stats stats, Character character)
        {
            return new List<Spell> { new BindingHeal(stats, character) };
        }

        protected void Calculate(Stats stats, Character character, int rank)
        {
            Rank = rank;

            MinHeal = (baseSpellTable[Rank - 1].MinHeal +
                stats.SpellPower * 1.88f * (1 - baseSpellTable[Rank - 1].RankCoef) 
                * (character.PriestTalents.EmpoweredHealing * 0.02f + HealingCoef)) 
                * (1 + character.PriestTalents.SpiritualHealing * 0.02f);

            MaxHeal = (baseSpellTable[Rank - 1].MaxHeal + stats.SpellPower * 1.88f
                * (1 - baseSpellTable[Rank - 1].RankCoef) 
                * (character.PriestTalents.EmpoweredHealing * 0.02f + HealingCoef)) 
                * (1 + character.PriestTalents.SpiritualHealing * 0.02f);

            ManaCost = (int)Math.Round(ManaCostBase / 100f * BaseMana[character.Level]);
            
            CastTime = baseSpellTable[Rank - 1].CastTime * (1 - stats.SpellHasteRating / 15.7f / 100f);
        }

        public override string ToString()
        {
            return String.Format("{0} *HpS(2): {1}\r\nHpM (2): {2}\r\nMin Heal: {3}\r\nMax Heal: {4}\r\nAvg Crit: {5}\r\nMax Crit: {6}\r\nCost: {7}",
                AvgHeal.ToString("0"),
                HpS.ToString("0.00"),
                HpM.ToString("0.00"),
                MinHeal.ToString("0"),
                MaxHeal.ToString("0"),
                AvgCrit.ToString("0"),
                MaxCrit.ToString("0"),
                ManaCost.ToString("0"));
        }
    }

    public class PrayerOfMending : Spell
    {
        private static readonly List<BaseSpell> baseSpellTable = new List<BaseSpell>(){   
        /*                Rank MinHeal MaxHeal ManaCost CastTime RankCoef    */
            new BaseSpell(1,    800,   800,   15,    0,   0)
            };

        private static readonly Color[] targetColors = new Color[]
                                           {
                                               Color.Cyan,
                                               Color.Cyan,
                                               Color.Cyan,
                                               Color.DarkCyan,
                                               Color.DeepSkyBlue
                                           };

        public override float HpS
        {
            get
            {
                return AvgHeal;
            }
        }

        public override float HpM
        {
            get
            {
                return (AvgHeal / ManaCost) * Targets;
            }
        }

        public PrayerOfMending(Stats stats, Character character, int rank, int targets)
            : base(stats, baseSpellTable[rank - 1], "Prayer of Mending (" + targets + " targets)", 0.4286f, targetColors[targets - 1])
        {
            Targets = targets;
            Calculate(stats, character, rank);
        }

        public PrayerOfMending(Stats stats, Character character)
            : this(stats, character, baseSpellTable.Count, 3)
        {}

        public static List<Spell> GetAllRanks(Stats stats, Character character, int targets)
        {
            List<Spell> list = new List<Spell>(baseSpellTable.Count);
            for (int i = 1; i <= baseSpellTable.Count; i++)
                list.Add(new PrayerOfMending(stats, character, i, targets));

            return list;
        }

        public static List<Spell> GetAllCommonRanks(Stats stats, Character character, int targets)
        {
            return new List<Spell> { new PrayerOfMending(stats, character, 1, targets) };
        }

        protected void Calculate(Stats stats, Character character, int rank)
        {
            Range = 15;
            Rank = rank;

            MinHeal = (baseSpellTable[Rank - 1].MinHeal +
                stats.SpellPower * 1.88f * HealingCoef * (1 - baseSpellTable[Rank - 1].RankCoef)) 
                * (1 + character.PriestTalents.SpiritualHealing * 0.02f);
            MaxHeal = (baseSpellTable[Rank - 1].MaxHeal +
                stats.SpellPower * 1.88f * HealingCoef * (1 - baseSpellTable[Rank - 1].RankCoef)) 
                * (1 + character.PriestTalents.SpiritualHealing * 0.02f);

            ManaCost = (int)Math.Round(baseSpellTable[Rank - 1].ManaCostBase / 100f * BaseMana[character.Level]
                * (1 - character.PriestTalents.HealingPrayers * 0.1f) 
                * (1 - character.PriestTalents.MentalAgility * 0.02f));
            Range = (int)Math.Round(Range * (1 + character.PriestTalents.HolyReach * 0.1f));
        }

        public override string ToString()
        {
            return String.Format("{0} *HpM(1): {1}\r\nHpM(2): {2}\r\nHpM 3): {3}\r\nHpM(4): {4}\r\nHpM(5): {5}\r\nCrit: {6}\r\nCost: {7}\r\nRange: {8}",
                AvgHeal.ToString("0"),
                (AvgHeal / ManaCost).ToString("0.00"),
                (AvgHeal * 2 / ManaCost).ToString("0.00"),
                (AvgHeal * 3 / ManaCost).ToString("0.00"),
                (AvgHeal * 4 / ManaCost).ToString("0.00"),
                (AvgHeal * 5 / ManaCost).ToString("0.00"),
                AvgCrit.ToString("0"),
                ManaCost.ToString("0"),
                Range);
        }
    }

    public class PowerWordShield : Spell
    {
        private static readonly List<BaseSpell> baseSpellTable = new List<BaseSpell>(){   
        /*                Rank MinHeal MaxHeal ManaCost CastTime RankCoef    */
            new BaseSpell(1,    44,    44,     23,      0,    0.885f),
            new BaseSpell(2,    88,    88,     23,      0,    0.770f),
            new BaseSpell(3,    158,   158,    23,      0,    0.617f),
            new BaseSpell(4,    234,   234,    23,      0,    0.500f),
            new BaseSpell(5,    301,   301,    23,      0,    0.414f),
            new BaseSpell(6,    381,   381,    23,      0,    0.329f),
            new BaseSpell(7,    484,   484,    23,      0,    0.243f),
            new BaseSpell(8,    605,   605,    23,      0,    0.157f),
            new BaseSpell(9,    763,   763,    23,      0,    0.071f),
            new BaseSpell(10,   942,   942,    23,      0,    0),
            new BaseSpell(11,   1147,  1147,   23,      0,    0),
            new BaseSpell(12,   1315,  1315,   23,      0,    0)
            };

        public override float HpS
        {
            get
            {
                return AvgHeal/GlobalCooldown;
            }
        }

        public PowerWordShield(Stats stats, Character character, int rank)
            : base(stats, baseSpellTable[rank - 1], "Power Word Shield", 0.3f, Color.SlateGray)
        {
            Calculate(stats, character, rank);
        }

        public PowerWordShield(Stats stats, Character character)
            : this(stats, character, baseSpellTable.Count)
        {}

        public static List<Spell> GetAllRanks(Stats stats, Character character)
        {
            List<Spell> list = new List<Spell>(baseSpellTable.Count);
            for (int i = 1; i <= baseSpellTable.Count; i++)
                list.Add(new PowerWordShield(stats, character, i));

            return list;
        }

        public static List<Spell> GetAllCommonRanks(Stats stats, Character character)
        {
            return new List<Spell> { new PowerWordShield(stats, character) };
        }

        protected void Calculate(Stats stats, Character character, int rank)
        {
            Rank = rank;
            MinHeal = (baseSpellTable[Rank - 1].MinHeal +
                stats.SpellPower * 1.88f * HealingCoef * (1 - baseSpellTable[Rank - 1].RankCoef)) 
                * (1 + character.PriestTalents.ImprovedPowerWordShield * 0.05f);
            MaxHeal = MinHeal;

            ManaCost = (int)Math.Round(baseSpellTable[Rank - 1].ManaCostBase / 100f * BaseMana[character.Level]
                * (1 - character.PriestTalents.MentalAgility * 0.02f));
        }

        public override string ToString()
        {
            return String.Format("{0} *HpM: {1}\r\nCost: {2}",
                MinHeal.ToString("0"),
                HpM.ToString("0.00"),
                ManaCost.ToString("0"));
        }
    }

    public class Lightwell : Spell
    {
        private static readonly List<BaseSpell> baseSpellTable = new List<BaseSpell>(){   
        /*                Rank MinHeal MaxHeal ManaCost CastTime RankCoef    */
            new BaseSpell(1,    800,    800,    17,    1.5f,   0.214f),
            new BaseSpell(2,    1165,   1165,   17,    1.5f,   0.071f),
            new BaseSpell(3,    1600,   1600,   17,    1.5f,   0),
            new BaseSpell(4,    2361,   2361,   17,    1.5f,   0)
            };

        public override float HpS
        {
            get
            {
                return AvgHeal / HotDuration;
            }
        }

        public Lightwell(Stats stats, Character character, int rank)
            : base(stats, baseSpellTable[rank - 1], "Lightwell", 1f, 6, Color.Gray)
        {
            Calculate(stats, character, rank);
        }

        public Lightwell(Stats stats, Character character)
            : this(stats, character, baseSpellTable.Count)
        {}

        public static List<Spell> GetAllRanks(Stats stats, Character character)
        {
            List<Spell> list = new List<Spell>(baseSpellTable.Count);
            for (int i = 1; i <= baseSpellTable.Count; i++)
                list.Add(new Lightwell(stats, character, i));

            return list;
        }

        public static List<Spell> GetAllCommonRanks(Stats stats, Character character)
        {
            return new List<Spell> { new Lightwell(stats, character) };
        }

        protected void Calculate(Stats stats, Character character, int rank)
        {
            if (character.PriestTalents.Lightwell == 0)
            {
                MinHeal = MaxHeal = 0;
                return;
            }

            Rank = rank;
            MinHeal = (baseSpellTable[Rank - 1].MinHeal +
                stats.SpellPower * 1.88f * HealingCoef * (1 - baseSpellTable[Rank - 1].RankCoef)) 
                * (1 + character.PriestTalents.SpiritualHealing * 0.02f);
            MaxHeal = MinHeal;

            ManaCost = (int)Math.Round(ManaCostBase / 100f * BaseMana[character.Level]);
        }

        public override string ToString()
        {
            return String.Format("{0} *HpS: {1}\r\nHpM: {2}\r\nTick: {3}\r\nCost: {4}",
                          MinHeal.ToString("0"),
                          HpS.ToString("0.00"),
                          HpM.ToString("0.00"),
                          (MinHeal / HotDuration * 2).ToString("0"),
                          ManaCost.ToString("0"));
        }
    }

    public class GiftOfTheNaaru : Spell
    {
        private static readonly List<BaseSpell> baseSpellTable = new List<BaseSpell>(){   
        /*                Rank MinHeal MaxHeal ManaCost CastTime RankCoef HotDuration    */
            new BaseSpell(1,     1085,   1085,   0,    1.5f,      1f,      15f)
            };

        public GiftOfTheNaaru(Stats stats, Character character)
            : this(stats, character, baseSpellTable.Count) { }

        public GiftOfTheNaaru(Stats stats, Character character, int rank)
            : base(stats, baseSpellTable[rank - 1], "Gift of the Naaru", 1f, 15f, Color.Green)
        {
            Calculate(stats, character, rank);
        }

        protected void Calculate(Stats stats, Character character, int rank)
        {
            Rank = rank;
            MinHeal = MaxHeal = (baseSpellTable[Rank - 1].MinHeal +
                stats.SpellPower * 1.88f * HealingCoef)
                * (1 + character.PriestTalents.SpiritualHealing * 0.02f);
            ManaCost = (int)Math.Round(ManaCostBase / 100f * BaseMana[character.Level]);
        }

        public override string ToString()
        {
            return String.Format("{0} *HpS: {1}\r\nHpM: {2}\r\nTick: {3}",
                          MinHeal.ToString("0"),
                          HpS.ToString("0.00"),
                          HpM.ToString("0.00"),
                          (MinHeal / HotDuration * 3).ToString("0"));
        }
    }

}
