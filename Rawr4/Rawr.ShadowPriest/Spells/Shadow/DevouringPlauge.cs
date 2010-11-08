using System;

namespace Rawr.ShadowPriest.Spells
{
    public class DevouringPlauge : Spell
    {
        /// <summary>
        /// Devouring Plauge is a dot that lasts for 24 seconds.
        /// </summary>
        /// It Benifits from:
        /// Talents:
        /// Twin Disciplines, Mental Agility, Dark Evangelism, Darkness (to be handeled in Char stats), Improved Devouring Plauge, Shadowform.
        //TODO: Get base Values from Beta, Imp Devouring Plauge (Instant 15% of total periodic damage), Healing Effect
        
        public DevouringPlauge() : base()
        { 
        }

        protected override void SetBaseValues()
        {
            base.SetBaseValues();

            baseMinDamage = 221; //check
            baseMaxDamage = 221; //check
            spCoef = 1.5f / 3.5f / 2f; //Check
            dotSpCoef = .1f; //Check
            periodicTick = 556f / 4f; //Check
            periodicTicks = 6f; //Check
            periodicTickTime = 3f; //Check          
            manaCost = 0.25f * Constants.BaseMana;
            shortName = "DP";
            name = "Devouring Plauge";
        }

        public override void Initialize(Rawr.ShadowPriest.Spells.ISpellArgs args)
        {
            //for reference
            //dotTick = totalCoef * (periodicTick * dotBaseCoef + spellPower * dotSpCoef) * (1 + critModifier * CritChance)

            manaCost *= 1 - .2f * args.Talents.MentalAgility;
            totalCoef += .01f + args.Talents.TwinDisciplines;
            totalCoef += .01f + args.Talents.Evangelism;
            totalCoef += .01f + args.Talents.Shadowform;
            
            ApplyDotHaste(args);
            base.Initialize(args);
        }

        #region hide
        public DevouringPlauge(ISpellArgs args)
            : this()
        {
            Initialize(args);
        }

        public static DevouringPlauge operator +(DevouringPlauge A, DevouringPlauge B)
        {
            DevouringPlauge C = (DevouringPlauge)A.MemberwiseClone();
            add(A, B, C);
            return C;
        }

        public static DevouringPlauge operator *(DevouringPlauge A, float b)
        {
            DevouringPlauge C = (DevouringPlauge)A.MemberwiseClone();
            multiply(A, b, C);
            return C;
        }
        #endregion

    }
}
