using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.HolyPriest
{

	public class Spell
	{
		private string name;
		private int rank;

		private float baseHeal;
		private float baseMana;
		private float baseCastTime;
		private float coef;
		private float downrank;
		private float healMultiple;

		private float hps;
		private float mps;
		private float hpm;
		private float spellCrit;
		private float castTime;
		private float avgHeal;

		private static float[] RENEWMANA = new float[] { 0, 30, 65, 105, 140, 170, 205, 250, 305, 365, 410, 430, 450 };
		private static float[] RENEWHEAL = new float[] { 0, 45, 100, 175, 245, 315, 400, 510, 650, 810, 970, 1010, 1110 };
		private static float[] RENEWLEVEL = new float[] { 0, 8, 14, 20, 26, 32, 38, 44, 50, 56, 60, 65, 70 };

		private static float[] FLASHMANA = new float[] { 0, 125, 155, 185, 215, 268, 315, 380, 400, 470 };
		private static float[] FLASHHEAL = new float[] { 0, 139, 253, 327, 400, 518, 644, 812, 913, 1101 };
		private static float[] FLASHLEVEL = new float[] { 0, 20, 26, 32, 38, 44, 50, 56, 61, 67 };

		public Spell(string name, int rank)
		{
			this.name = name;
			this.rank = rank;
			healMultiple = 1.15f; // Spiritual Healing 5/5

			if (name.Equals("Renew") && rank > 0 && rank < 13)
            {
                hotCoef = 1;
                directCoef = 0;
                hotBaseHeal = RENEWHEAL[rank];
                directBaseHeal = 0;
				baseCastTime = 0;
                baseManaCost = RENEWMANA[rank] - 0.10f * RENEWMANA[rank]; // Mental Agility 3/3
				downrank = Math.Min((RENEWLEVEL[rank] + 12) / 70f, 1f);
				healMultiple = healMultiple * 1.10f; // Improved renew 3/3
            }
			else if (name.Equals("Flash Heal") && rank > 0 && rank < 10)
            {
                hotCoef = 0;
                directCoef = 0.4286f;
                hotBaseHeal = 0;
                directBaseHeal = FLASHHEAL[rank];
				baseCastTime = 1.5f;
                baseManaCost = FLASHMANA[rank]
				downrank = Math.Min((FLASHLEVEL[rank] + 9) / 70f, 1f);
				healMultiple = healMultiple;
            }
        }

		public void Calculate(Stats stats)
		{
			float bonus, multi = 0, cost;
			spellCrit = .08336f + stats.Intellect / 8000 + stats.SpellCritRating / 2208;
			castTime = baseCastTime / (1 + stats.SpellHasteRating / 1570);

			if (name.Equals("Renew"))
			{
				spellCrit = 0;
				bonus = stats.RenweHeal;
				multi = healMultiple + stats.Spirit * 0.25; // Spiritual Guidance 5/5
				cost = stats.RenwewCost;
            }
			else if (name.Equals("Flash Heal"))
			{
				spellCrit = 0;
				bonus = stats.FlashHeal;
				multi = healMultiple + stats.Spirit * 0.25; // Spiritual Guidance 5/5
				cost = stats.FlashCost;			
			}

			float plusheal = stats.Healing + stats.AverageHeal + bonus;
			avgHeal = multi * (baseHeal + (plusheal * coef) * downrank);
			hps = avgHeal * (1 + .5f * spellCrit) / castTime;
			mps = (baseMana - cost - stats.ManaRestorePerCast) / castTime;
			hpm = hps / mps;
        }

		public override string ToString()
		{
			return name + " " + rank;
		}

		public string Name { get { return name; } }
		public int Rank { get { return rank; } }

		public float Hps { get { return hps; } }
		public float Mps { get { return mps; } }
		public float Hpm { get { return hpm; } }
		public float SpellCrit { get { return spellCrit; } }
		public float CastTime { get { return castTime; } }
		public float AverageHeal { get { return avgHeal; } }

	}
}
