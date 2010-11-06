using System;

namespace Rawr.ShadowPriest.Spells
{
    public class VampiricTouch : Spell
    {
        /// <summary>
        /// Shadow Word Pain is a dot that lasts for 15 seconds.
        /// It Benifits from:
        /// Talents:
        /// Twin Disciplines, Dark Evangelism, Darkness (to be handeled in Char stats), Shadowform, Sin and punishment.
        //TODO: Get base Values from Beta, fear from Sin and Punishment (Might not be needed)
        /// </summary>
        public VampiricTouch() : base()
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
            manaCost = 0.16f * Constants.BaseMana;
            shortName = "VT"; 
        }

        public override void Initialize(Rawr.ShadowPriest.Spells.ISpellArgs args)
        {
            //for reference
            //dotTick = totalCoef * (periodicTick * dotBaseCoef + spellPower * dotSpCoef) * (1 + critModifier * CritChance)

            totalCoef += .01f + args.Talents.TwinDisciplines;
            totalCoef += .01f + args.Talents.Evangelism;
            totalCoef += .01f + args.Talents.Shadowform;
            
            ApplyDotHaste(args);
            base.Initialize(args);
        }

        #region hide
        public VampiricTouch(ISpellArgs args)
            : this()
        {
            Initialize(args);
        }

        public static VampiricTouch operator +(VampiricTouch A, VampiricTouch B)
        {
            VampiricTouch C = (VampiricTouch)A.MemberwiseClone();
            add(A, B, C);
            return C;
        }

        public static VampiricTouch operator *(VampiricTouch A, float b)
        {
            VampiricTouch C = (VampiricTouch)A.MemberwiseClone();
            multiply(A, b, C);
            return C;
        }
        #endregion

    }
}
