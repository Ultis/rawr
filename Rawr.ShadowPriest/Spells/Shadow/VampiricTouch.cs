using System;

namespace Rawr.ShadowPriest.Spells
{
    public class VampiricTouch : DoTSpell
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
        protected override void SetDotValues()
        {
            base.SetDotValues();

            tickHasteCoEf = 0.2f;
            debuffDurationBase = 15f;

        }
        protected override void SetBaseValues()
        {
            base.SetBaseValues();

            baseScaling = 0.115000002086163f;
            manaCost = 0.16f * Constants.BaseMana;
            shortName = "VT";
            name = "Vampiric Touch";
        }
        public override float SpellPowerCoef
        {
            get
            {
                return 1.400000005960464f;
            }
        }
        public override void Initialize(Rawr.ShadowPriest.Spells.ISpellArgs args)
        {
            //for reference
            //dotTick = totalCoef * (periodicTick * dotBaseCoef + spellPower * dotSpCoef) * (1 + critModifier * CritChance)

            
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
