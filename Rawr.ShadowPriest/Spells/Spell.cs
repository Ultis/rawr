using System;
using System.Collections.Generic;


namespace Rawr.ShadowPriest.Spells
{
    public abstract class Spell
    {

        protected float gcd = 1.5f;
        protected float castTimeBase = 0f;
        protected float cooldown = 0f;
        public string shortName = "ShortSpellName";
        public string name = "SpellName";

        protected float manaCost = 0f;

        protected float crit = 0f;
        protected float critModifier = 1f;
        protected float missChance = .17f;

        protected float spellPower = 0f;
        protected float spellPowerModifer = 0f;

        protected float latencyGcd = .15f;
        protected float latencyCast = .075f;


        protected float baseScaling = 0f;
        //this is number for level 85
        protected float spellScaling = 945.188842773438f;
 
        /// <summary>
        /// Average Damage per Application
        /// </summary>
        protected float averageDamage = 0f;



        /// <summary>
        /// This Constructor calls SetBaseValues.
        /// </summary>
        public Spell()
        {
            SetBaseValues();
        }

        protected virtual void SetBaseValues()
        {
            castTimeBase = 0f;
            manaCost = 0f;
            gcd = 1.5f;
            critModifier = 1f;
            cooldown = 0f;
            missChance = .17f;
        }

        public void Update(ISpellArgs args)
        {
            SetBaseValues();
            Initialize(args);
        }

        /// <summary>
        /// Add Spells
        /// </summary>
        /// <param name="sp1">spell 1</param>
        /// <param name="sp2">spell 2</param>
        /// <param name="nS">spell1+spell2</param>
        protected static void add(Spell sp1, Spell sp2, Spell nS)
        {
            nS.castTimeBase = (sp1.castTimeBase + sp2.castTimeBase);
            nS.manaCost = (sp1.manaCost + sp2.manaCost);
            nS.gcd = (sp1.gcd + sp2.gcd);
            nS.critModifier = (sp1.critModifier + sp2.critModifier);
            nS.cooldown = (sp1.cooldown + sp2.cooldown);
            nS.missChance = (sp1.missChance + sp2.missChance);
            nS.spellPower = (sp1.spellPower + sp2.spellPower);
        }

        /// <summary>
        /// Multiply Spell by ammount
        /// </summary>
        /// <param name="sp1">Spell</param>
        /// <param name="c">Multiplication ammount</param>
        /// <param name="nS">Spell * Multiplication Ammount</param>
        protected static void multiply(Spell sp1, float c, Spell nS)
        {
            nS.castTimeBase = sp1.castTimeBase * c;
            nS.manaCost = sp1.manaCost * c;
            nS.gcd = sp1.gcd * c;
            nS.critModifier = sp1.critModifier * c;
            nS.cooldown = sp1.cooldown * c;
            nS.missChance = sp1.missChance * c;
            nS.spellPower = sp1.spellPower * c;
        }

        public float BaseDamage
        { get { return baseScaling * spellScaling; } }

        /// <summary>
        /// This is to ensure that the constraints on the GCD are met on abilities that have Gcd
        /// </summary>
        public float Gcd
        { get { return (gcd >= 1 ? gcd : 1); } }

        /// <summary>
        /// The effective Cast Time. Taking GCD and latency into account.
        /// </summary>
        public float CastTime
        {

            get
            {
                if (gcd == 0 && castTimeBase == 0)
                    return 0;
                if (castTimeBase > Gcd)
                    return castTimeBase + Latency;
                else
                    return Gcd + Latency;
            }
        }

        /// <summary>
        /// The effective Latency of this spell effecting the start cast time of the next one.
        /// </summary>
        public float Latency
        {
            get
            {
                if (gcd == 0 && castTimeBase == 0)
                    return 0;
                if (castTimeBase >= gcd)
                    return latencyCast;
                else
                    return latencyGcd;
            }
        }

        /// <summary>
        /// CoolDown of Spell
        /// </summary>
        public float Cooldown
        { get { return cooldown; } }

        /// <summary>
        /// ManaCost of Spell
        /// </summary>
        public float ManaCost
        { get { return manaCost; } }

        /// <summary>
        /// Average Damage
        /// </summary>
        public virtual float AverageDamage
        { get { return averageDamage; } }

        /// <summary>
        /// SpellPower-Coef
        /// </summary>
        public virtual float SpellPowerCoef
        { get { return 0f; } }

        /// <summary>
        /// Crit chance with check that crit isnt bigger than one
        /// </summary>
        public float CritChance
        { get { return Math.Min(1f, crit); } }

        /// <summary>
        /// Initialize Spell based on ISpellArgs
        /// </summary>
        /// <param name="args"></param>
        public virtual void Initialize(ISpellArgs args)
        {
            float Speed = (1f + args.Stats.SpellHaste) * (1f + StatConversion.GetSpellHasteFromRating(args.Stats.HasteRating));
            gcd = (float)Math.Round(gcd / Speed, 4);
            castTimeBase = (float)Math.Round(castTimeBase / Speed, 4);
            latencyGcd = args.LatencyGCD;
            latencyCast = args.LatencyCast;
            critModifier *= (float)Math.Round(1.5f + args.Stats.BonusSpellCritDamageMultiplier, 6);
            //critModifier += 1f;
            spellPower += args.Stats.SpellPower;
            crit += args.Stats.SpellCrit;
            missChance -= args.Stats.SpellHit;
            //totalCoef *= 1 + args.Stats.BonusDamageMultiplier; //ret + bm + arcane buff
            if (missChance < 0) missChance = 0;
            manaCost = (float)Math.Floor(manaCost);
            //TODO: base resistance by level depending on what bossis being battled
            //totalCoef *= 1f - StatConversion.GetAverageResistance(85, 87, 0, 0);
        }

        public virtual Spell Clone()
        {
            return (Spell)this.MemberwiseClone();
        }

        public override string ToString()
        {
            return shortName;
        }
    }
}
